using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mustachio;
using Newtonsoft.Json;

namespace lastpage
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        private const string PageExtension = "lpe";
        private const string PartialExtension = "lpl";
        private const string PartialPrefix = "_p_";
        private const string BuildDefinitionPath = "lastpage-config.lpc";
        private const string SkeletonPath = "skeleton.lps";
        private const string PageContentStart = "{{#page_content}}";
        private const string PageContentEnd = "{{/page_content}}";
        private const string PageContent = "page_content";
        private const string SkeletonContent = "content";
        private const string PagePrefix = "page_";

        //private static bool _defer;
        private static BuildDefinition _cfg;

        private static void Log(string msg, LogLevel lvl)
        {
            Console.WriteLine($"[{lvl.ToString()}] {msg}");
        }

        private static async Task<BuildDefinition> GetConfigAsync()
        {
            try
            {
                var cfg = await File.ReadAllTextAsync(BuildDefinitionPath);
                return JsonConvert.DeserializeObject<BuildDefinition>(cfg);
            }
            catch (IOException)
            {
                Log("Build defition file not found, creating one!", LogLevel.Warning);
                // no build definition exists
                var cfg = BuildDefinition.GetDefault;
                await File.WriteAllTextAsync(BuildDefinitionPath, JsonConvert.SerializeObject(cfg, Formatting.Indented));
                return cfg;
            }
            catch (Exception e)
            {
                // something else is the issue, maybe cfg is wrong
                Log("Something went wrong while trying to load the build definition!", LogLevel.Error);
                Log(e.ToString(), LogLevel.Error);
                throw;
            }
        }

        private static async Task Main()
        {
            // read build definition
            _cfg = await GetConfigAsync();
            Log("Loaded build definition!", LogLevel.Information);

            // TODO filesystemwatcher maybe?
            // if ^ uncomment defer lines

            var b = await BuildAsyncWrapper();
            if (b)
            {
                Log("Preliminary build complete!", LogLevel.Information);                
            }
            else
            {
                Log("Failed initial build!", LogLevel.Error);                
                return;
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Press...");
                Console.WriteLine(" [ESC] to quit");
                //Console.WriteLine(" [D] to defer updates");
                Console.WriteLine(" [F] to force regeneration");
                Console.Write(" > ");
                var rk = Console.ReadKey();
                Console.WriteLine();

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (rk.Key)
                {
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    //case ConsoleKey.D:
                    //    _defer = true;
                    //    Console.WriteLine("Defer enabled, press any key to release!");
                    //    Console.ReadKey();
                    //    _defer = false;
                    //    Build();
                    //    break;
                    case ConsoleKey.F:
                        await BuildAsyncWrapper();
                        break;
                    // nothing to do
                }
                Console.WriteLine();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // get the subdirectories for the specified directory
            var dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists) throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            var dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName)) Directory.CreateDirectory(destDirName);

            // get the files in the directory and copy them to the new location
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // when copying subdirectories, copy them and their contents to new location
            if (!copySubDirs) return;
            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                CopyDirectory(subdir.FullName, temppath, true);
            }
        }

        private static async Task<bool> BuildAsyncWrapper()
        {
            try
            {
                await BuildAsync();
                Log("Build completed!", LogLevel.Information);
                return true;
            }
            catch (FileNotFoundException fne)
            {
                Log($"Build failed!", LogLevel.Error);
                Log($"Missing file: {fne.FileName}", LogLevel.Error);
            }
            catch (Exception e)
            {
                Log("Build failed!", LogLevel.Error);
                Log($"Exception: {e}", LogLevel.Error);
            }
            return false;
        }

        private static async Task BuildAsync()
        {
            // clear & recreate target folder
            Directory.Delete(_cfg.targetFolder, true);
            Directory.CreateDirectory(_cfg.targetFolder);
            Log("Cleaned target directory!", LogLevel.Information);

            // load partials
            var partialsPreAsync = Directory.GetFiles(_cfg.sourceFolder, $"*.{PartialExtension}").Select(async x => new PathContentPair
                {
                    Path = x,
                    Content = await File.ReadAllTextAsync(x)
                }).ToList();
            var partialsPre = await Task.WhenAll(partialsPreAsync);
            foreach (var p in partialsPre)
            {
                var jsonPath = p.Path.ChangeExtension(PartialExtension, "json");
                if (!File.Exists(jsonPath)) continue;
                var eo = JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(jsonPath));
                var template = Parser.Parse(p.Content);
                p.Content = template(eo);
            }
            var partials = partialsPre.ToDictionary(k => PartialPrefix + k.Path.Split('\\').Last().RemoveExtension(PartialExtension), v => v.Content);
            Log("Loaded & templated partials!", LogLevel.Information);

            // load skeleton
            var skeleton = await File.ReadAllTextAsync(Path.Combine(_cfg.sourceFolder, SkeletonPath));
            var skeleTemplate = Parser.Parse(skeleton);
            var skeleDict = partials.ToDictionary(k => k.Key, v => (object) v.Value);
            Log("Preloaded skeleton!", LogLevel.Information);

            // render
            var files = Directory.GetFiles(_cfg.sourceFolder, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                // we skip the skeleton
                if (file.EndsWith(SkeletonPath)) continue;

                // we skip partials
                if (file.EndsWith(PartialExtension)) continue;

                // we skip json files if there's a matching page or partial file
                if(file.EndsWith("json") && File.Exists(file.ChangeExtension("json", PageExtension))) continue;
                if(file.EndsWith("json") && File.Exists(file.ChangeExtension("json", PartialExtension))) continue;

                var newPath = file.ReplaceFirst(_cfg.sourceFolder, _cfg.targetFolder);

                // check if it's a page we want to template
                if (file.EndsWith(PageExtension))
                {
                    var pageEmbed = PageContentStart + Environment.NewLine + await File.ReadAllTextAsync(file) + Environment.NewLine + PageContentEnd;
                    var template = Parser.Parse(pageEmbed);
                    var jsonPath = file.ChangeExtension(PageExtension, "json");

                    // create model with partials if there are any
                    var model = File.Exists(jsonPath) ? JsonConvert.DeserializeObject<ExpandoObject>(await File.ReadAllTextAsync(jsonPath)) : new ExpandoObject();
                    var eod = (IDictionary<string, object>) model;
                    if (partials.Any())
                    {
                        foreach (var p in partials)
                        {
                            eod.Add(p.Key, p.Value);
                        }
                    }

                    // ensure that the page_content object exists
                    if (!eod.ContainsKey(PageContent)) eod.Add(PageContent, new object());

                    // template page + template skeleton = final page
                    var pageContent = template(model);
                    var pr = skeleDict.Keys.Where(x => x.StartsWith(PagePrefix)).ToList();
                    foreach (var key in pr) skeleDict.Remove(key);
                    foreach (var mk in model.Where(x => x.Key.StartsWith(PagePrefix)))
                    {
                        skeleDict[mk.Key] = mk.Value;
                    }
                    skeleDict[SkeletonContent] = pageContent;
                    var finalContent = skeleTemplate(skeleDict);
                    await File.WriteAllTextAsync(newPath.ChangeExtension(PageExtension, "html"), finalContent);
                    Log($"Templated {newPath}", LogLevel.Information);
                    continue;
                }

                //otherwise just simply copy the file
                File.Copy(file, newPath);
                Log($"Copied {newPath}", LogLevel.Information);
            }

            // static post copy
            if (!string.IsNullOrWhiteSpace(_cfg.postFolder) && new DirectoryInfo(_cfg.postFolder).Exists)
            {
                Log("Copying post-build files...", LogLevel.Information);
                CopyDirectory(_cfg.postFolder, _cfg.targetFolder, true);
            }
            else
            {
                Log("Post folder not configured or doesn't exist.", LogLevel.Warning);
            }
        }
    }
}
