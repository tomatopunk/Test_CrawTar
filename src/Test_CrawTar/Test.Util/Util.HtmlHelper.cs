using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.Util
{
   public class Util
    {
        public static string GetHtml(string path)
        {
            using (HttpClient http=new HttpClient())
            {
                return http.GetStringAsync(path).Result;
            }
        }
    }
}
