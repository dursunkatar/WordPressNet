using System.IO;
using System.Net;
using System.Text;
using WordPressNet.Models;

namespace WordPressNet
{
    public class WordPressClient
    {
        private readonly WordPressRequestXml request;
        private readonly string url;

        public WordPressClient(WordpressConfig wpConfig)
        {
            request = new WordPressRequestXml(wpConfig);
            url = wpConfig.Url;
        }

        public string NewCategory(Category category)
        {
            return GetResponseWpService(request.NewCategory(category));
        }

        public string UploadFile(string filePath, bool overwrite = true)
        {
            return GetResponseWpService(request.UploadFile(filePath, overwrite));
        }

        private string GetResponseWpService(string reqXml)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            var bytes = Encoding.UTF8.GetBytes(reqXml);
            req.ContentLength = bytes.Length;
            using (var stream = req.GetRequestStream())
                stream.Write(bytes, 0, bytes.Length);
            using (var stream = new StreamReader(req.GetResponse().GetResponseStream()))
                return stream.ReadToEnd();
        }
    }
}
