using AnimeViewer.Classes;
using AnimeViewer.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeViewer.SupportClasses
{
    public class SerieAssociationEntity
    {
        public Serie Serie { get; set; }
        List<animenewsnetworkSearch> posibilities;

        public List<animenewsnetworkSearch> Posibilities
        {
            get { return posibilities; }
            set { posibilities = value; }
        }
        public animenewsnetworkSearch SelectedOfferId { get; set; }
        public SerieAssociationEntity(Serie serie)
        {
            this.Serie = serie;
            posibilities = new List<animenewsnetworkSearch>();
            posibilities.Add(new animenewsnetworkSearch(-1, "", ""));
            if (Serie.Info != null)
            {
                animenewsnetworkSearch current = new animenewsnetworkSearch(serie.Info.Id, serie.Info.Title, "CURRENT");
                posibilities.Add(current);
                SelectedOfferId = current;
            }
            posibilities.AddRange(Animenewsnetwork.GetPosibleNames(serie.Name));
            
          
        }

    }
}
