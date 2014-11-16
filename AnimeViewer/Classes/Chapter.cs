using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeViewer.Classes
{
    public class Chapter
    {
        string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        string name;
        public string Name
        {
            get { return name; }
        }
        public Chapter(string path)
        {
            this.path = path;
            this.name = System.IO.Path.GetFileName(path);
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
