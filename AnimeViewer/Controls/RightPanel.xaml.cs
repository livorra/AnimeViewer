using AnimeViewer.Classes;
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
    /// Lógica de interacción para RightPanel.xaml
    /// </summary>
    public partial class RightPanel : UserControl
    {
        public Serie Serie
        {
            get { return (Serie)GetValue(SerieProperty); }
            set { SetValue(SerieProperty, (Serie)value); }
        }
        public static readonly DependencyProperty SerieProperty = DependencyProperty.Register("Serie", typeof(Serie), typeof(RightPanel));

        public Chapter SelectedChapter
        {
            get { return (Chapter)GetValue(SelectedChapterProperty); }
            set { SetValue(SelectedChapterProperty, (Chapter)value); }
        }
        public static readonly DependencyProperty SelectedChapterProperty = DependencyProperty.Register("SelectedChapter", typeof(Chapter), typeof(RightPanel));
        public RightPanel()
        {
            InitializeComponent();
        }
    }
}
