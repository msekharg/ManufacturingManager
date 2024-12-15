using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsc.Blazor.Components.Helpers
{
    public static class Functions
    {
        public static string ReplaceNewLinesToHtmlBreak(string value)
        {
            return value.Replace("\n", "<BR>");
        }
    }
}
