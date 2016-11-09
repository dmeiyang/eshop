using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EShop.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(GetHref()));

            var response = request.GetResponse();

            var html = new HtmlAgilityPack.HtmlDocument();

            html.Load(response.GetResponseStream(), true);

            var content = html.DocumentNode.InnerHtml;

            var nodes = html.DocumentNode.SelectNodes("html//div[@class='content']/p");

            foreach (var v in nodes)
            {
                Console.WriteLine(v.InnerHtml);

                Console.WriteLine("--------------------------------------------");
            }
        }

        static string GetHref()
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri("http://www.youdaili.net/Daili/guonei/"));

            var response = request.GetResponse();

            var html = new HtmlAgilityPack.HtmlDocument();

            html.Load(response.GetResponseStream(), true);

            var content = html.DocumentNode.InnerHtml;

            var node = html.DocumentNode.SelectNodes("html//div[@class='chunlist']/ul/li//a").FirstOrDefault();

            return node.GetAttributeValue("href", "");
        }
    }
}
