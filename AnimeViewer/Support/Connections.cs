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
            using (WebClient wc = new WebClient())
            {
                return wc.DownloadString(url);
            }
        }
        public static void downloadFile(string url, string filename)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(url, filename);
            }
        }
    }
}
