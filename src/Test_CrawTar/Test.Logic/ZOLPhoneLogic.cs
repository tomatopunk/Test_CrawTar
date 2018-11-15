using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Test.Util;

namespace Test.Logic
{
    public class ZolPhoneLogic
    {

        public string BaseUrl { get; set; }

        public string AreaUrl { get; set; }

        private string CrawTarGetUrl { get; set; }


        //构造函数
        public ZolPhoneLogic()
        {
//            this.AreaUrl = areaUrl;
//            this.BaseUrl = baseUrl;
//            this.CrawTarGetUrl = GetCrawlTargetUrl();
        }

        ~ZolPhoneLogic() => Console.WriteLine($"已结束执行 Date:{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        public void JobStart()
        {
            this.BaseUrl = "detail.zol.com.cn";

            var res = new List<Zol>();
            //1 . 获取机型列表 http://detail.zol.com.cn/cell_phone_index/subcate57_0_list_1_0_1_2_0_101.html

            string htmlRes = null;
            var htmlDoc = new HtmlDocument();

            //MaxId = 103
            for (int i = 1; i < 103; i++)
            {
                try
                {
                    string listUrl = GetCrawlTargetUrl($"/cell_phone_index/subcate57_0_list_1_0_1_2_0_{i}");
                    Console.WriteLine($"抓取目录目标:{listUrl}");
                    htmlRes = Util_HtmlHelper.GetHtml(listUrl).Result;
                    htmlDoc.LoadHtml(htmlRes);

                    // 获取机型 
                    var factoriesFrm = htmlDoc.GetElementbyId("J_PicMode") ?? throw new Exception("获取表单失败");

                    // cell_phone/index1225826.shtml
                    var factoriesFrmData = factoriesFrm
                        .SelectNodes("li")
                        .Select(p => new
                        {
                            FactoryClient=p.SelectSingleNode("div[@class='comment-row']/a[@class='comment-num']")
                                .Attributes["href"]
                                .Value
                                .Replace("review.shtml", "param"),
                            Title=p.SelectSingleNode("h3/a").Attributes["title"].Value
                        }).ToList();

                    Console.WriteLine($"获取第{i}页,共{factoriesFrmData.Count}条数据");

                    foreach (var d in factoriesFrmData)
                    {
                        Console.WriteLine($"TiTle:{d.Title}");
                        var htmlDoc2 = new HtmlDocument();
                        htmlRes = Util_HtmlHelper.GetHtml(GetCrawlTargetUrl(d.FactoryClient,"shtml")).Result;
                        htmlDoc2.LoadHtml(htmlRes);

                        var factory = htmlDoc2
                            .DocumentNode.SelectNodes("/html[1]/body[1]/div[10]");


                        res.Add(new Zol
                        {
                             
                        });




                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error->>>"+e);
                }            

            }

//            await ;
        }


        private string GetCrawlTargetUrl(string areaUrl,string lastUrl="html")
        {
            return $"http://detail.zol.com.cn{areaUrl}.{lastUrl}";
        }

        class Zol
        {
            public DateTime MarletTime { get; set; }

            public string Brand { get; set; }

            public string Ram { get; set; }

            public string Rom { get; set; }

            public string Category { get; set; }

            public string Color { get; set; }
        }
    }
}
