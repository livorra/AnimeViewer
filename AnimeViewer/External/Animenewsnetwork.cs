using AnimeViewer.Classes;
using AnimeViewer.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnimeViewer.External
{
    class Animenewsnetwork
    {

        public static List<animenewsnetworkSearch> GetPosibleNames(string name)
        {
            bool min1 = false;
            List<animenewsnetworkSearch> posibles = new List<animenewsnetworkSearch>();
            string formattedName = name.Replace(" ", "+");
            string response = Connections.GetResponse(Properties.Settings.Default.SearchApiurl + formattedName);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            XmlNodeList items = doc.GetElementsByTagName("item");

            foreach (XmlNode item in items)
            {
                animenewsnetworkSearch element = new animenewsnetworkSearch(
                    Convert.ToInt32(item.SelectSingleNode("id").InnerText),
                    item.SelectSingleNode("name").InnerText,
                    item.SelectSingleNode("type").InnerText);
                    min1 = true;
                    posibles.Add(element);
            }
            if (!min1)
            {
                string[] words = name.Split(' ');
                if (words.Length > 1)
                {
                    var longestWords = words.Where(w => w.Length == words.Max(m => m.Length));
                    return GetPosibleNames(longestWords.ToArray()[0]);
                }

            }
            return posibles;
        }
        public static Classes.AnimeInfo getInformation(int id)
        {
            try
            {
                string response = Connections.GetResponse(Properties.Settings.Default.InformationApiUrl + id.ToString());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                AnimeInfo info = new AnimeInfo();
                info.Id = id;
                info.Title = doc.SelectSingleNode("/ann/anime[1]/info[@type='Main title']").InnerText;
                info.Start = doc.SelectSingleNode("/ann/anime[1]/info[@type='Vintage'][1]").InnerText;
                try
                {
                    info.Web = doc.SelectSingleNode("/ann/anime[1]/info[@type='Official website'][1]").Attributes["href"].Value;
                }
                catch
                {
                }
                XmlNodeList genreList = doc.SelectNodes("/ann/anime[1]/info[@type='Themes'] | /ann/anime[1]/info[@type='Genres']");
                foreach (XmlNode genre in genreList)
                {
                    info.Genres.Add(genre.InnerText);
                }
                XmlNodeList episodes = doc.SelectNodes("/ann/anime[1]/episode");
                foreach (XmlNode episode in episodes)
                {
                    int number;
                    if (int.TryParse(episode.Attributes["num"].Value, out number))
                    {
                        string title = episode.FirstChild.InnerText;
                        if (number > 0)
                            info.OfficialEpisodes.Add(number, title);
                    }
                }
                string screenshotUrl = doc.SelectSingleNode("/ann/anime[1]/info[@type='Picture'][1]").Attributes["src"].Value;
                info.ScreenShot = Properties.Settings.Default.ImagesPath + "\\" + id + ".jpg";
                Connections.downloadFile(screenshotUrl, info.ScreenShot, false);
                info.Description = doc.SelectSingleNode("/ann/anime[1]/info[@type='Plot Summary']").InnerText;

                return info;
            }
            catch
            {
                return null;
            }
        }
    }
    public class animenewsnetworkSearch
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public animenewsnetworkSearch(int _id, string _name,string _type)
        {
            id = _id;
            name = _name;
            type = _type;
        }
        public override string ToString()
        {
            return type.ToUpper()+" - "+ name;
        }


    }
}
