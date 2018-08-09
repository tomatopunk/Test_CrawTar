using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Logic
{

    public class BbsGuanJia
    {
        public string BaseUrl { get; set; }

        public string AreaUrl { get; set; }

        private string CrawTarGetUrl { get; set; }


        private string tst(string value) => $"http://{BaseUrl}/{AreaUrl}.html>";

        //构造函数
        public BbsGuanJia(string baseUrl, string areaUrl)
        {
            this.AreaUrl = areaUrl;
            this.BaseUrl = baseUrl;
            this.CrawTarGetUrl = GetCrawlTargetUrl();
        }

        ~BbsGuanJia() => Console.WriteLine($"已结束执行 Date:{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        public async Task JobStart()
        {
            if (string.IsNullOrEmpty(BaseUrl))
            {
                BaseUrl = "bbs.guanjia.qq.com";
            }

            if (string.IsNullOrEmpty(AreaUrl))
            {
                AreaUrl = "forum-84-1";
                //                throw new Exception("请指定爬取板块");
            }

            Console.WriteLine($"抓取目标:{GetCrawlTargetUrl()}");


        }




        public string GetCrawlTargetUrl()
        {
            return $"http://{BaseUrl}/{AreaUrl}.html>";
        }
    }


}
