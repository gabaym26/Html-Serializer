// See https://aka.ms/new-console-template for more information
using PraktyCode;
using System.Text.RegularExpressions;
using System.Xml.Linq;

HtmlHelper htmlHelper = HtmlHelper.Instance;

var html = await Load("https://demo.guru99.com/payment-gateway/index.php");
var cleanHtml = new Regex("\\s").Replace(html, " ");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => (!s.StartsWith(" ")) && (!s.Equals("")));

HtmlElement root = new HtmlElement() { };
HtmlElement temp = root;

foreach (var item in htmlLines)
{
    int endIndex = item.IndexOf(" ");
    if (endIndex < 0)
        endIndex = item.Length;

    string result = item.Substring(0, endIndex);

    if (result.EndsWith('/'))
    {
        endIndex -= 1;
        result = item.Substring(0, endIndex);
    }

    if (item.StartsWith('/'))
    {
        temp = temp.Parent;
    }

    else if (htmlHelper.Tags.Contains(result))
    {
        temp.Children.Add(createElement(item));
        temp.Children[temp.Children.Count - 1].Parent = temp;
        temp = temp.Children[temp.Children.Count - 1];
    }

    else if (htmlHelper.SingelTags.Contains(result))
    {
        temp.Children.Add(createElement(item));
        temp.Children[temp.Children.Count - 1].Parent = temp;
    }
    else
    {
        temp.InnerHtml = item.ToString();
    }
}
Selector s1 = Selectors("html header#header .inner");
HashSet<HtmlElement> h1 = root.StartFind(s1);
Selector s2 = Selectors("#interstitial script");
HashSet<HtmlElement> h2 = root.StartFind(s2);
Selector s3 = Selectors("div div div");
HashSet<HtmlElement> h3 = root.StartFind(s3);
Console.WriteLine("end");

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    return await response.Content.ReadAsStringAsync();
}

static HtmlElement createElement(string element)
{
    int startIndex;
    string result;
    int endIndex = element.IndexOf(" ");
    if (endIndex < 0)
        endIndex = element.Length;
    string name = element.Substring(0, endIndex);
    if (name.EndsWith("/") || name.EndsWith("\""))
    {
        name = element.Substring(0, endIndex - 1);
    }
    HtmlElement htmlElement = new HtmlElement();
    htmlElement.Name = name;
    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(element);

    foreach (var item in attributes)
    {
        if (item.ToString().StartsWith("id"))
        {
            startIndex = item.ToString().IndexOf("=") + 1;
            result = item.ToString().Substring(startIndex + 1);
            if (result.EndsWith("\""))
            {
               endIndex = result.IndexOf("\"");
                result = result.Substring(0, endIndex);
            }
            if (result.EndsWith("\\"))
            {
                result = result.Substring(0, result.Length - 2);
            }
            result = result.Trim();
            htmlElement.Id = result;
        }
        else if (item.ToString().StartsWith("class"))
        {
            startIndex = item.ToString().IndexOf("=") + 1;
            result = item.ToString().Substring(startIndex + 1);
            if (result.EndsWith("\""))
            {
                endIndex = result.IndexOf("\"");
                result = result.Substring(0, endIndex);
            }
            if (result.EndsWith("\\"))
            {
                result = result.Substring(0, result.Length - 2);
            }
            htmlElement.Classes = result.Split(" ").ToList();
        }
        else
        {
            htmlElement.Attributes.Add(item.ToString());
        }

    }

    return htmlElement;
}

static Selector Selectors(string select)
{
    string[] selsctors = new Regex(" ").Split(select);
    Selector root = new Selector();
    root.ParseStringToSElector(selsctors[0]);
    Selector temp = root;
    for (int i = 1; i < selsctors.Length; i++)
    {
        temp.Child = new Selector();
        temp.Child.Parent = temp;
        temp = temp.Child;
        temp.ParseStringToSElector(selsctors[i]);

    }
    return root;
}


static void printElement(HtmlElement h)
{
    if (h.Children == null)
    {
        return;
    }
    Console.WriteLine($"name:{h.Name} ,id: {h.Id},innerhtml: {h.InnerHtml} ");
    foreach (var item in h.Attributes)
    {
        Console.WriteLine("attribute" + item);
    }
    foreach (var item in h.Classes)
    {
        Console.WriteLine("classes" + item);
    }
    foreach (var item in h.Children)
    {
        printElement(item);
    }

    Console.WriteLine($"name:{h.Name}");

}