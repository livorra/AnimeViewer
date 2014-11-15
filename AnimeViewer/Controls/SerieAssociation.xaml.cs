using AnimeViewer.Classes;
using AnimeViewer.SupportClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para SerieAssociation.xaml
    /// </summary>
    public partial class SerieAssociation : UserControl
    {
        private delegate void dvoid();
        public Repository repository
        {
            get { return (Repository)GetValue(repositoryProperty); }
            set { SetValue(repositoryProperty, (Repository)value); }
        }
        public static readonly DependencyProperty repositoryProperty = DependencyProperty.Register("repository", typeof(Repository), typeof(SerieAssociation));
        public SerieAssociation()
        {
            InitializeComponent();
        }
        public void load()
        {
            if(repository  == null)
            {
                DGassociation.Visibility = System.Windows.Visibility.Visible;
                return;
            }
            List<Serie> series = repository.Series;
            PBprogress.Maximum = series.Count;
            ObservableCollection<SerieAssociationEntity> temp = new ObservableCollection<SerieAssociationEntity>();
            DGassociation.Visibility = System.Windows.Visibility.Hidden;

            new Thread(new ThreadStart(() =>
                {
                    //maybe do a threadpool
                    //NEED REFACTOR
                    int i = 0;
                    foreach (Serie serie in series)
                    {
                        i++;
                        if (serie.Info == null)
                            temp.Add(new SerieAssociationEntity(serie.Name));
                        else
                            temp.Add(null);
                        if (i > 3)
                            break;
                        this.Dispatcher.Invoke(new dvoid(() =>
                        {
                            PBprogress.Value++;
                        }));
                    }
                    this.Dispatcher.Invoke(new dvoid(() =>
                        {
                            DGassociation.DataContext = temp;
                            DGassociation.Visibility = System.Windows.Visibility.Visible;
                            Pwait.Visibility = System.Windows.Visibility.Hidden;
                            
                        }));
                })).Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pwait.Visibility = System.Windows.Visibility.Visible;
            Bscan.IsEnabled = false;
            load();
        }
    }
}
