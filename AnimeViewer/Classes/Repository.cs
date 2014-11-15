using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnimeViewer.Classes
{
    public class Repository
    {
        private string path;
        public string Path
        {
          get { return path; }
        }
        private List<Serie> series;
        public List<Serie> Series
        {
            get { return series; }
        }
        

        public Repository(string path)
        {

            this.path = path;
            if (path != null)
                this.series = Directory.GetDirectories(path).Select(s => new Serie(s)).ToList();
            else
                this.series = new List<Serie>();
        }
        public static Repository CreateVoid()
        {
            return new Repository(null);
        }

    }
}
