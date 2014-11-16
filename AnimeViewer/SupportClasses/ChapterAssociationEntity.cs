using AnimeViewer.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AnimeViewer.SupportClasses
{
    public class ChapterAssociationEntity : INotifyPropertyChanged
    {
        string officialChapter;

        public string OfficialChapter
        {
            get { return officialChapter; }
            set
            {
                officialChapter = value;
                OnPropertyChanged("OfficialChapter");
            }
        }
        List<Chapter> chapters;

        public List<Chapter> Chapters
        {
            get { return chapters; }
            set
            {
                chapters = value;
                OnPropertyChanged("Filename");
            }
        }
        string newChapterName;

        public string NewChapterName
        {
            get { return newChapterName; }
            set { newChapterName = value; }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null && PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
