using AnimeViewer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace AnimeViewer.Support
{
    public static class Parser
    {
        public static Dictionary<int,List<Chapter>> GetEpisodeName(List<Chapter> episodes)
        {
            Dictionary<int,List<Chapter>> chapters = new Dictionary<int,List<Chapter>>();
            foreach(Chapter episode in episodes)
            {
                try
                {
                    int episodeNumber = Convert.ToInt32(Regex.Match(System.IO.Path.GetFileNameWithoutExtension(episode.Name), @"\d+").Value);
                    if (chapters.ContainsKey(episodeNumber))
                        chapters[episodeNumber].Add(episode);
                    else
                        chapters.Add(episodeNumber, new List<Chapter>() { episode });
                }
                catch
                {
                    if (chapters.ContainsKey(-1))
                        chapters[-1].Add(episode);
                    else
                        chapters.Add(-1, new List<Chapter>() { episode });
                }
            }
            return chapters;
        }
    }
}
