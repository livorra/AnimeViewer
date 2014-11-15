using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AnimeViewer.Classes
{
    public class AnimeInfo
    {
        public string Title { get; set; }
        public List<string> Genres { get; set; }
        public string Start { get; set; }
        public string Web { get; set; }
        public string ScreenShot { get; set; }
        public string Description { get; set; }
        public Dictionary<int, string> OfficialEpisodes { get; set; }
        public AnimeInfo()
        {
        }
        public AnimeInfo(string _title, List<string> _genres, string _start, string _web, string _screenShot, string _description, Dictionary<int, string> _officialEpisodes)
        {
            Title = _title;
            Genres = _genres;
            Start = _start;
            Web = _web;
            ScreenShot = _screenShot;
            Description = _description;
            OfficialEpisodes = _officialEpisodes;
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
