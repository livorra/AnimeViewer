using AnimeViewer.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnimeViewer
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
    public partial class MainWindow : Window
	{
        public List<Repository> Repositories
        {
            get { return (List<Repository>)GetValue(RepositoriesProperty); }
            set { SetValue(RepositoriesProperty, (List<Repository>)value); }
        }
        public static readonly DependencyProperty RepositoriesProperty = DependencyProperty.Register("Repositories", typeof(List<Repository>), typeof(MainWindow),new PropertyMetadata(new List<Repository>()));

        public Serie CurrentSerie
        {
            get { return (Serie)GetValue(CurrentSerieProperty); }
            set { SetValue(CurrentSerieProperty, (Serie)value); }
        }
        public static readonly DependencyProperty CurrentSerieProperty = DependencyProperty.Register("CurrentSerie", typeof(Serie), typeof(MainWindow));

        public Chapter CurrentChapter
        {
            get { return (Chapter)GetValue(CurrentChapterProperty); }
            set { SetValue(CurrentChapterProperty, (Chapter)value); }
        }
        public static readonly DependencyProperty CurrentChapterProperty = DependencyProperty.Register("CurrentChapter", typeof(Chapter), typeof(MainWindow));
		
        public MainWindow()
		{
			this.InitializeComponent();
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Repositories.Add(new Repository("G:\\Anime"));
        }

    }
}