using AnimeViewer.Classes;
using AnimeViewer.External;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AnimeViewer.SupportClasses
{
    public class SerieAssociationEntity : INotifyPropertyChanged
    {
        public Serie Serie { get; set; }
        List<animenewsnetworkSearch> posibilities;

        public List<animenewsnetworkSearch> Posibilities
        {
            get { return posibilities; }
            set 
            {
                posibilities = new List<animenewsnetworkSearch>();
                posibilities.Add(new animenewsnetworkSearch(-1, "", ""));
                if (Serie.Info != null)
                {
                    animenewsnetworkSearch current = new animenewsnetworkSearch(Serie.Info.Id, Serie.Info.Title, "CURRENT");
                    posibilities.Add(current);
                    SelectedOfferId = current;
                }
                posibilities.AddRange(value);
                OnPropertyChanged("Posibilities");
            }
        }
        public animenewsnetworkSearch SelectedOfferId { get; set; }
        public SerieAssociationEntity(Serie serie)
        {
            this.Serie = serie;

            Posibilities =Animenewsnetwork.GetPosibleNames(serie.Name);
            
          
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null && PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
