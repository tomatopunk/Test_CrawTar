using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Test.Logic
{

    public class BbsGuanJia
    {
        public string BaseUrl { get; set; }

        public string AreaUrl { get; set; }

        private string CrawTarGetUrl { get; set; }


        //构造函数
        public BbsGuanJia(string baseUrl, string areaUrl)
        {
            this.AreaUrl = areaUrl;
            this.BaseUrl = baseUrl;
            this.CrawTarGetUrl = GetCrawlTargetUrl();
        }

        ~BbsGuanJia() => Console.WriteLine($"已结束执行 Date:{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        public void JobStart()
        {
            if (string.IsNullOrEmpty(BaseUrl))
            {
                BaseUrl = "bbs.guanjia.qq.com";
            }

            if (string.IsNullOrEmpty(AreaUrl))
            {
                AreaUrl = "forum-84-1";
            }

            Console.WriteLine($"抓取目标:{GetCrawlTargetUrl()}");

            //获取目标页面HTML

            var html = Util.Util.GetHtml(GetCrawlTargetUrl());
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            Console.WriteLine("HTML获取完毕");
            //            var tst = htmlDoc.DocumentNode.SelectNodes("//html/body/div/div/div/div/div/div/div/form");

            //            var tst = htmlDoc.GetElementbyId("normalthread_*");

            //获取ID为 moderate 表单
            var moderateFrm = htmlDoc.GetElementbyId("moderate");

            var frmDatas = moderateFrm?
                .SelectNodes("table/tbody")
                .Where(p => p.Attributes["id"].Value != "separatorline")
                .Select(p => new
                {
                    by = p.SelectNodes("tr/td[@class='by']"),
                    title = p.SelectSingleNode("tr/th"),
                    num = p.SelectSingleNode("tr/td[@class='num']")

                }).ToList();



            //将查询结构转换成强类型
            List<PostList> postList = frmDatas?
                .Select(p => new PostList
                {
                    Title = p.title?.SelectSingleNode("a")?.InnerText?.Replace(" ", ""),
                    PostTextUri = p.title?.SelectSingleNode("a")?.Attributes["href"].Value,
                    PostStatus = p.title?.SelectSingleNode("img")?.Attributes["title"]?.Value,
                    CreatedTime = DateTime.TryParse(p.by[0].SelectSingleNode("em/span")?.InnerText, out DateTime time) ? time : (DateTime?)null,
                    CreatedUser = p.by[0].SelectSingleNode("cite/a")?.InnerText,
                    ReplayNum = int.TryParse(p.num.SelectSingleNode("a")?.InnerText, out var rnum) ? rnum : (int?)null,
                    WatchNum = int.TryParse(p.num.SelectSingleNode("em")?.InnerText, out var wnum) ? wnum : (int?)null,
                    LastTime = DateTime.TryParse(p.by[1].SelectSingleNode("em/a")?.InnerText, out DateTime lastTime) ? lastTime : (DateTime?)null
                }).ToList();
        }


        private string GetCrawlTargetUrl()
        {
            return $"http://{BaseUrl}/{AreaUrl}.html";
        }
    }

    class PostList
    {
        public string Title { get; set; }

        public string PostStatus { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? LastTime { get; set; }

        public int? ReplayNum { get; set; }

        public int? WatchNum { get; set; }

        public string PostTextUri { get; set; }


        public string CreatedUser { get; set; }

        //        public string CreatedUserId { get; set; }
    }


}
