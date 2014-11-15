using AnimeViewer.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeViewer.SupportClasses
{
    public class SerieAssociationEntity
    {
        public string Name { get; set; }
        List<animenewsnetworkSearch> posibilities;

        public List<animenewsnetworkSearch> Posibilities
        {
            get { return posibilities; }
            set { posibilities = value; }
        }

        public SerieAssociationEntity(string name)
        {
            Name = name;
            posibilities = Animenewsnetwork.GetPosibleNames(name);
        }

    }
}
