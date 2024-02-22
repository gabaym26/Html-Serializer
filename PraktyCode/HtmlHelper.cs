using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace PraktyCode
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance=new HtmlHelper();
        public static HtmlHelper Instance=> _instance;
        public string[] Tags{ get; set; }
        public string[] SingelTags { get; set; }
        private HtmlHelper()
        {
            Tags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("tags/HtmlTags.json"));
            SingelTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("tags/HtmlVoidTags.json"));
        }
    }
}
