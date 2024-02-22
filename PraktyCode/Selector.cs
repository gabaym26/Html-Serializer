using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PraktyCode
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public void ParseStringToSElector(string select)
        {
            HtmlHelper htmlHelper = HtmlHelper.Instance;
            string pattern = @"(?<tag>\w+)(?:#(?<id>\w+))?(?:\.(?<class>\w+))*";
            var result = Regex.Match(select, pattern);
            string tagName = result.Groups["tag"].Value;
            Id = result.Groups["id"].Value;
            string classes = string.Join(" ", result.Groups["class"].Captures.Cast<Capture>().Select(c => c.Value));
            string[] arr_class = classes.Split(" ");
            if (htmlHelper.Tags.Contains(tagName) || htmlHelper.SingelTags.Contains(tagName))
            {
                TagName = tagName;
            }
            else
            {
                if (select.StartsWith("."))
                {
                    Classes.Add(tagName);
                }
                if (select.StartsWith("#"))
                {
                    Id = tagName;
                }
            }
            foreach (var item in arr_class)
            {
                Classes.Add(item);
            }
        }
    }

}
