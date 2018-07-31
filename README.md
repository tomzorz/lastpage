# lastpage

Lastpage is a .net core static website generator based on [Mustachio](https://github.com/wildbit/mustachio/) which is based on [Mustache](https://mustache.github.io/).

![](https://img.shields.io/badge/platform-any-green.svg?longCache=true&style=flat-square) ![](https://img.shields.io/badge/nuget-yes-green.svg?longCache=true&style=flat-square) ![](https://img.shields.io/badge/license-MIT-blue.svg?longCache=true&style=flat-square)

You must have [.NET Core 2.1](https://www.microsoft.com/net/download/windows) or higher installed.

## Try the pre-built `lastpage`

You can quickly install and try [lastpage from nuget.org](https://www.nuget.org/packages/lastpage/) using the following commands:

```console
dotnet tool install -g lastpage
mkdir build
mkdir live
lastpage
```

> Note: You may need to open a new command/terminal window the first time you install a tool.

> Note 2: **Running the code above will yield a crash**, as it can't find a few files it's looking for. See more info below. (And a nicer OOBE later.)

You can uninstall the tool using the following command:

```console
dotnet tool uninstall -g lastpage
```

## How does it work?

The main ingredients are as follows:

The **skeleton** is the... well... the skeleton 😊 of the site, usually containing the html, head and body tags - scripts, styles etc...

**Partials** are common parts that can be inserted into the skeleton or pages - think menus, or a social share widget..

**Pages** are what get embedded in the skeleton after being templated - think "index", "news" or "cat gifs 🐱‍👤".

Lastpage uses 4 custom file extensions: .lpc, .lps, .lpl and .lpe.

* .lpc files are actually json files, containing the configuration for the tool - at the moment the tool looks for "lastpage-config.lpc" where it was started
* the .lps file is the skeleton file, actually html inside - this can use partials in it
* the .lpl files are the partials, these can be templated using .json files with the same name (templates will be applied before embedding)
* the .lpe files are the pages, these are going to be .html files after being optionally templated (by a .json file with the same name) and then embedded in the skeleton

**There is a demo setup in the `demo` folder, check it out!**

Laspage also has the ability to copy additional files into the build folder after post. You might have various reasons to do this:

* some files other than lastpage use the .lp? extensions and it breaks lastpage
* you have a lot of content that definitely doesn't need templating, so it's faster to skip all the matching logic in lastpage and just bulk copy files

## Future features

### Nicer OOBE

Maybe add a default template?

### Implementing automatic updates based on file changes

This is already seen in some comments in the code, sadly `FileSystemWatcher` needs some taming before primetime in this, causes too many exceptions that can't really be caught.

### Ignore files in config

Add a way to ignore certain .json, .lp? or other files from templating even if lastpage finds a match.