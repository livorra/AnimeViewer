using AnimeViewer.Classes;
using AnimeViewer.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimeViewer.Controls
{
    /// <summary>
    /// Lógica de interacción para ChapterAssociation.xaml
    /// </summary>
    public partial class ChapterAssociation : UserControl
    {
        static List<ChapterAssociationEntity> currentChapters = new List<ChapterAssociationEntity>();
        public Serie Serie
        {
            get { return (Serie)GetValue(repositoryProperty); }
            set 
            { 
                SetValue(repositoryProperty, (Serie)value);
            }
        }
        public static readonly DependencyProperty repositoryProperty = DependencyProperty.Register("Serie", typeof(Serie), typeof(ChapterAssociation), new PropertyMetadata(OnSerieChange));

        private static void OnSerieChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Serie newSerie = e.NewValue as Serie;
            if(newSerie != null)
                currentChapters = fillChapterAssociationEntity(newSerie.Info.OfficialEpisodes);
        }
        
        //public List<ChapterAssociationEntity> ChapterAssociationEntities
        //{
        //    get { return (List<ChapterAssociationEntity>)GetValue(ChapterAssociationEntitiesProperty); }
        //    set { SetValue(ChapterAssociationEntitiesProperty, (List<ChapterAssociationEntity>)value); }
        //}
        //public static readonly DependencyProperty ChapterAssociationEntitiesProperty = DependencyProperty.Register("ChapterAssociationEntities", typeof(List<ChapterAssociationEntity>), typeof(ChapterAssociation));
        
        public ChapterAssociation()
        {
            InitializeComponent();
        }
        public static List<ChapterAssociationEntity> fillChapterAssociationEntity(Dictionary<int,string> OfficialChapters)
        {
            List<ChapterAssociationEntity> chapters = new List<ChapterAssociationEntity>();
            foreach(int key in OfficialChapters.Keys)
            {
                ChapterAssociationEntity newChapter = new ChapterAssociationEntity();
                string mask = "00";
                if (OfficialChapters.Keys.Count > 100)
                    mask = "000";
                newChapter.OfficialChapter = key.ToString(mask) + " - " + OfficialChapters[key];
                chapters.Add(newChapter);
            }
            return chapters;
        }

        #region listview behavior
        private void DGassociation_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnsWidth(sender as ListView);
        }
        private void UpdateColumnsWidth(ListView listView)
        {
            int autoFillColumnIndex = 1;
            if (listView.ActualWidth == Double.NaN)
                listView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            double remainingSpace = listView.ActualWidth;
            for (int i = 0; i < (listView.View as GridView).Columns.Count; i++)
                if (i != autoFillColumnIndex)
                    remainingSpace -= (listView.View as GridView).Columns[i].ActualWidth;
            (listView.View as GridView).Columns[autoFillColumnIndex].Width = remainingSpace >= 0 ? remainingSpace - 30 : 0;
        }
        #endregion

    }
}
