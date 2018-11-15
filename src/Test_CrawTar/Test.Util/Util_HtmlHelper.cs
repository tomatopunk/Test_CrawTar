using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.Util
{
    public static class Util_HtmlHelper
    {
        public static async Task<string> GetHtml(string path, int retryCount = 0)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var res = await http.GetAsync(path);

                    if (res.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception("访问连接出错 Message: "+res.RequestMessage);
                    }
                    var contentType = res.Content.Headers.ContentType.ToString();

                    var bytes = await res.Content.ReadAsByteArrayAsync();
                    if (contentType.Contains("utf8") || contentType.Contains("utf-8"))
                    {                        
                        return Encoding.UTF8.GetString(bytes);
                    }

                    if (contentType.Contains("gbk") || contentType.Contains("GBK"))
                    {
                        return Encoding.GetEncoding("gbk").GetString(bytes);
                    }
                    return Encoding.Default.GetString(bytes);
                }

            }
            catch (TaskCanceledException e)
            {
                return "Error-> TimeOut~ " + e;
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }
    }
}
