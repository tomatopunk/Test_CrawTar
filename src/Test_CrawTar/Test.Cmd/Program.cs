using System;
using Test.Logic;

namespace TestCmd
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"开始抓取BBS.GuanJia.qq.com内容 Date:{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            BbsGuanJia bbsGuanJia = new BbsGuanJia(baseUrl:"",areaUrl:"");

            
        }
    }
}
