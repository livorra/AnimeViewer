using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace AnimeViewer.Support
{
    static class Connections
    {
        public static string GetResponse(string url)
        {
            using (WebClient wc = new WebClient() { Proxy = null, Encoding = System.Text.Encoding.UTF8 })
            {
                return wc.DownloadString(url);
            }
        }
        public static void downloadFile(string url, string filename,bool overwrite = true)
        {
            if (File.Exists(filename) && !overwrite)
                return;
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (WebClient wc = new WebClient() { Proxy = null})
            {
                wc.DownloadFile(url, filename);
            }
        }
    }
}
