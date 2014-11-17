using AnimeViewer.Classes;
using AnimeViewer.Support;
using AnimeViewer.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        Dictionary<string, string> InitialBindings = new Dictionary<string, string>() { { "TBcurrentChapterName", "Chapters" }, { "TBnewChapterName", "NewChapterName" } };
        Regex regex;
        public Serie Serie
        {
            get { return (Serie)GetValue(repositoryProperty); }
            set 
            { 
                SetValue(repositoryProperty, (Serie)value);
            }
        }
        public readonly DependencyProperty repositoryProperty = DependencyProperty.Register("Serie", typeof(Serie), typeof(ChapterAssociation), new PropertyMetadata(OnSerieChange));
        public List<ChapterAssociationEntity> ChapterAssociationEntities
        {
            get { return (List<ChapterAssociationEntity>)GetValue(ChapterAssociationEntitiesProperty); }
            set { SetValue(ChapterAssociationEntitiesProperty, (List<ChapterAssociationEntity>)value); }
        }
        public static readonly DependencyProperty ChapterAssociationEntitiesProperty = DependencyProperty.Register("ChapterAssociationEntities", typeof(List<ChapterAssociationEntity>), typeof(ChapterAssociation));
       
        public ChapterAssociation()
        {
            InitializeComponent();
        }
        private static void OnSerieChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChapterAssociation hinstance = (ChapterAssociation)d;
            Serie newSerie = e.NewValue as Serie;
            if (newSerie != null && newSerie.Info != null)
                hinstance.ChapterAssociationEntities = hinstance.fillChapterAssociationEntity(newSerie.Chapters);
        }
        public List<ChapterAssociationEntity> fillChapterAssociationEntity(List<Chapter> chapters)
        {
            Dictionary<int, List<Chapter>> localChapters = Parser.GetEpisodeName(chapters);
            return associate(localChapters);
        }
        public List<ChapterAssociationEntity> associate(Dictionary<int, List<Chapter>> localChapters)
        {
            List<ChapterAssociationEntity> chapterAssociations = new List<ChapterAssociationEntity>();
            string mask = GetIntegerMask(Serie.Info.OfficialEpisodes.Keys.Count);
            List<int> addedKeys = new List<int>();


            foreach (int key in Serie.Info.OfficialEpisodes.Keys)
            {
                addedKeys.Add(key);
                ChapterAssociationEntity newChapter = new ChapterAssociationEntity();
                newChapter.OfficialChapter = newChapter.NewChapterName = key.ToString(mask) + " - " + Serie.Info.OfficialEpisodes[key];
                if (localChapters.ContainsKey(key))
                    newChapter.Chapters = localChapters[key];
                else
                    newChapter.Chapters = new List<Chapter>();
                chapterAssociations.Add(newChapter);
            }
            List<Chapter> unversioned = new List<Chapter>();
            foreach (int key in localChapters.Keys)
            {
                if (!addedKeys.Contains(key))
                    unversioned.AddRange(localChapters[key]);
            }
            if (unversioned.Count > 0)
            {
                ChapterAssociationEntity newChapter = new ChapterAssociationEntity();
                newChapter.Chapters = unversioned;
                newChapter.OfficialChapter = "Unrecognised files";
                chapterAssociations.Add(newChapter);
            }
            return chapterAssociations;
        }
        public string GetIntegerMask(int max)
        {
            string mask = "00";
            if (max > 100)
                mask = "000";
            return mask;
        }

        #region listview behavior
        private void DGassociation_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnsWidth(sender as ListView);
        }
        private void UpdateColumnsWidth(ListView listView)
        {
            int autoFillColumnIndex = 2;
            if (listView.ActualWidth == Double.NaN)
                listView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            if (Double.IsNaN( (listView.View as GridView).Columns[autoFillColumnIndex].Width))
                return;
            double remainingSpace = listView.ActualWidth;
            for (int i = 0; i < (listView.View as GridView).Columns.Count; i++)
                if (i != autoFillColumnIndex)
                    remainingSpace -= (listView.View as GridView).Columns[i].ActualWidth;
            if (remainingSpace > 5)
                (listView.View as GridView).Columns[autoFillColumnIndex].Width = remainingSpace >= 0 ? remainingSpace - 5 : 0;
            else
                (listView.View as GridView).Columns[autoFillColumnIndex].Width = remainingSpace >= 0 ? remainingSpace : 0;
        }
        #endregion

        #region hightlight search
        private void CBcontext_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hightlightTextUpdate(TBSearch.Utf8Text);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            hightlightTextUpdate(TBSearch.Utf8Text);
        }
        private void hightlightTextUpdate(string pattern)
        {
            try
            {
                regex = new Regex("(" + TBSearch.Utf8Text + ")", RegexOptions.IgnoreCase);
                ListBoxItem selectedItem = ((ListBoxItem)CBcontext.SelectedItem);
                string tbname = selectedItem.Tag.ToString();
                FindListViewItem(LVchaptersSync, pattern, tbname, InitialBindings[tbname]);
            }
            catch
            {

            }
        }
        public void FindListViewItem(DependencyObject obj, string pattern,string tbname, string bindingPropery)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                ListViewItem lv = obj as ListViewItem;
                if (lv != null)
                {
                    HighlightText(lv, pattern,tbname, bindingPropery);
                }
                FindListViewItem(VisualTreeHelper.GetChild(obj as DependencyObject, i), pattern,tbname, bindingPropery);
            }
        }
        private void HighlightText(Object itx,string pattern,string tbname,string bindingPropery)
        {

            if (itx != null)
            {
                if (itx is TextBlock)
                {

                    regex = new Regex("(" + pattern + ")", RegexOptions.IgnoreCase);
                    TextBlock tb = itx as TextBlock;

                    if (pattern.Length == 0 && tb.Name == tbname)
                    {
                        Binding bind = new Binding(bindingPropery);
                        tb.SetBinding(TextBlock.TextProperty, bind);
                        return;
                    }
                    string[] substrings = regex.Split(tb.Text);
                    tb.Inlines.Clear();
                    foreach (var item in substrings)
                    {
                        if (tb.Name == tbname && regex.Match(item).Success)
                        {
                            Run runx = new Run(item);
                            runx.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3399FF")); ;
                            tb.Inlines.Add(runx);
                        }
                        else
                        {
                            tb.Inlines.Add(item);
                        }
                    }
                    return;
                }
                else
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(itx as DependencyObject); i++)
                    {
                        HighlightText(VisualTreeHelper.GetChild(itx as DependencyObject, i), pattern,tbname, bindingPropery);
                    }
                }
            }
        }
        #endregion

        private void Breplace_Click(object sender, RoutedEventArgs e)
        {
            switch (CBcontext.SelectedIndex)
            {
                case 0:
                    updateNewFilenames(TBSearch.Utf8Text, TBreplace.Text);
                    break;
                case 1:
                    updateCurrentFilenames(TBSearch.Utf8Text, TBreplace.Text);
                    break;
            }
        }
        void updateNewFilenames(string search,string replace)
        {
            try
            {
                
                hightlightTextUpdate("");
                regex = new Regex("(" + search + ")");
                foreach (ChapterAssociationEntity chapter in ChapterAssociationEntities)
                { 
                    chapter.NewChapterName = regex.Replace(chapter.NewChapterName, replace);
                }
            }
            catch
            {

            }
        }
        void updateCurrentFilenames(string search, string replace)
        {
            try
            {
                regex = regex = new Regex("(" + search + ")");
                List<Chapter> modified = Serie.Chapters.ConvertAll(chapter => new Chapter(chapter.Path) { Name = regex.Replace(chapter.Name, replace) });
                ChapterAssociationEntities = fillChapterAssociationEntity(modified);
            }
            catch
            {

            }
            
        }

        
    }
}
