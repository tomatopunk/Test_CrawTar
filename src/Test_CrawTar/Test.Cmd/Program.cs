using System;
using Test.Logic;

namespace Test.Cmd
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"开始抓取BBS.GuanJia.qq.com内容 Date:{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            BbsGuanJia bbsGuanJia = new BbsGuanJia(baseUrl: "", areaUrl: "");

            bbsGuanJia.JobStart();


        }
    }
}
