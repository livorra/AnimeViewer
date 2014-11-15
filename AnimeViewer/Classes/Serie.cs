﻿using System;
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
                return AnimeInfo.createNewFromFile(System.IO.Path.Combine(path, Properties.Settings.Default.SerieInfoFile));
            }
            set
            {
                if(value != null)
                    Files.WriteFile(System.IO.Path.Combine(path,Properties.Settings.Default.SerieInfoFile),JsonConvert.SerializeObject(value));
                else
                {
                    File.Delete(System.IO.Path.Combine(path,Properties.Settings.Default.SerieInfoFile));
                }
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
