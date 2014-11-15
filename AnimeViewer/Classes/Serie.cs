using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using AnimeViewer.Support;

namespace AnimeViewer.Classes
{
    public class Serie
    {
        string name;
        public string Name
        {
            get { return name; }
        }
        string path;
        public string Path
        {
            get { return path; }
        }
        List<Chapter> chapters;
        public List<Chapter> Chapters
        {
            get { return chapters; }
        }
        public AnimeInfo Info
        {
            get
            {
                string content = Files.ReadFile(System.IO.Path.Combine(path, Properties.Settings.Default.SerieInfoFile));
                if (content == null)
                    return null;
                try
                {
                    return JsonConvert.DeserializeObject<AnimeInfo>(content);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                Files.WriteFile(System.IO.Path.Combine(path,Properties.Settings.Default.SerieInfoFile),JsonConvert.SerializeObject(value));
            }
        }


        public Serie(string path)
        {
            this.path = path;
            this.name = System.IO.Path.GetFileName(path);
            this.chapters = Directory.GetFiles(path).Select(s => new Chapter(s)).ToList();
        }
    }
}
