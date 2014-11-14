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
    /// Lógica de interacción para LeftPanel.xaml
    /// </summary>
    public partial class LeftPanel : UserControl
    {
        public List<Repository> Repositories
        {
            get { return (List<Repository>)GetValue(RepositoriesProperty); }
            set { SetValue(RepositoriesProperty, (List<Repository>)value); }
        }
        public static readonly DependencyProperty RepositoriesProperty = DependencyProperty.Register("Repositories", typeof(List<Repository>), typeof(LeftPanel));

        public Repository SelectedRepository
        {
            get { return (Repository)GetValue(SelectedRepositoryProperty); }
            set { SetValue(SelectedRepositoryProperty, (Repository)value); }
        }
        public static readonly DependencyProperty SelectedRepositoryProperty = DependencyProperty.Register("SelectedRepository", typeof(Repository), typeof(LeftPanel),null);

        public Serie SelectedSerie
        {
            get { return (Serie)GetValue(SelectedSerieProperty); }
            set { SetValue(SelectedSerieProperty, (Serie)value); }
        }
        public static readonly DependencyProperty SelectedSerieProperty = DependencyProperty.Register("SelectedSerie", typeof(Serie), typeof(LeftPanel));
        

        public LeftPanel()
        {
            InitializeComponent();
        }


    }
}
