using System;
using System.Collections.Generic;
using System.Text;

namespace lastpage.PlatformAdapters
{
    public interface IPlatformAdapter
    {
        (string fileName, string fileContent) GenerateRedirects(List<(string from, string to)> redirects);
    }
}
