using System.IO;
using System.Net;

namespace UnlocLoader
{
    public class FileDownloader
    {
        public void DownloadFile(string url, string file)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(url, file);
            }
        }

        public string DownloadTemp(string url)
        {
            var tempFile = Path.GetTempFileName();

            using (var client = new WebClient())
            {
                client.DownloadFile(url, tempFile);
            }

            return tempFile;
        }
    }
}