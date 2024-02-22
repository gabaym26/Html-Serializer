using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PraktyCode
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();

        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                HtmlElement element = queue.Dequeue();
                foreach (var item in element.Children)
                {
                    queue.Enqueue(item);
                }
                yield return element;
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while (current.Parent != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        public HashSet<HtmlElement> StartFind(Selector selector)
        {
            HashSet<HtmlElement> hs = new HashSet<HtmlElement>();
            var y = this.Descendants();
            foreach (var item in y)
            {
                hs.Add(item);
            }
            return FindElementBySelect(selector, hs);
        }

        public static HashSet<HtmlElement> FindElementBySelect(Selector selector, HashSet<HtmlElement> e)
        {
            if (selector == null)
            {
                return e;
            }
            HashSet<HtmlElement> hs = new HashSet<HtmlElement>();
            HashSet<HtmlElement> h;
            foreach (var item in e)
            {
                h = item.FindElementBySelector(selector);
                
                
                foreach (var i in h)
                {
                    hs.Add(i);
                }
            }
            return FindElementBySelect(selector.Child, hs);
        }

        public HashSet<HtmlElement> FindElementBySelector(Selector selector)
        {
            var flag = false;
            HashSet<HtmlElement> hs = new HashSet<HtmlElement>();
            var children = Descendants();
            foreach (var item in children)
            {
                if (((selector.Id == null)||(selector.Id == "") || (selector.Id.Equals(item.Id))) &&
                    ((selector.TagName == null)||(selector.TagName == "") || (selector.TagName.Equals(item.Name))))
                {
                    if (selector.Classes[0] != "")
                    {
                        foreach (var classS in selector.Classes)
                        {
                            if (classS=="")
                            {
                              continue;
                            }
                            if (!item.Classes.Contains(classS))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                    if (selector.Child != null)
                    {   
                        foreach (var item2 in item.Descendants())
                        {
                          hs.Add(item2);
                        }
                        hs.Remove(item);
                    }
                    else hs.Add(item);
                }
            }
            return hs;
        }

    }
}
