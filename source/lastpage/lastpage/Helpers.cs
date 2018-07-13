using System;

namespace lastpage
{
    public static class Helpers
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            var pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0) return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string ChangeExtension(this string path, string origExt, string targetExt)
        {
            if (!path.EndsWith(origExt)) throw new Exception($"file {path} has the wrong extension!");
            return path.Substring(0, path.Length - origExt.Length) + targetExt;
        }

        public static string RemoveExtension(this string path, string extension)
        {
            if (!path.EndsWith(extension)) throw new Exception($"file {path} has the wrong extension!");
            return path.Substring(0, path.Length - extension.Length - 1);
        }
    }
}
