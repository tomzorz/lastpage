using System;
using System.Collections.Generic;
using System.Text;

namespace lastpage.PlatformAdapters
{
    public class Netlify : IPlatformAdapter
    {
        public (string fileName, string fileContent) GenerateRedirects(List<(string @from, string to)> redirects)
        {
            throw new NotImplementedException();
        }
    }
}
