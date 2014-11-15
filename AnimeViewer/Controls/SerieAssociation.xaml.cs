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
            ObservableCollection<SerieAssociationEntity> temp = new ObservableCollection<SerieAssociationEntity>();
            List<Serie> series = repository.Series;
            if(repository  == null)
            {
                showResults();
                return;
            }
            PBprogress.Maximum = series.Count;


            new Thread(new ThreadStart(() =>
                {
                    Thread[] threads = new Thread[3];

                    int currThread = 0;
                    foreach (Serie serie in series)
                    {
                        while(true)
                        {
                            if (threads[currThread] == null || !threads[currThread].IsAlive)
                            {
                                threads[currThread] = new Thread(new ThreadStart(() =>
                                {
                                    temp.Add(new SerieAssociationEntity(serie.Name));
                                    this.Dispatcher.Invoke(new dvoid(() =>
                                    {
                                        PBprogress.Value++;
                                    }));
                                }));
                                threads[currThread].Start();
                                if (currThread == threads.Length - 1)
                                    currThread = 0;
                                else
                                    currThread++;
                                break;
                            }
                            Thread.Sleep(10);
                        }
                        
                    }
                    foreach (Thread hilo in threads)
                        hilo.Join();
                    this.Dispatcher.Invoke(new dvoid(() =>
                        {
                            DGassociation.DataContext = temp;
                            showResults();
                        }));
                })).Start();
        }
        private void showResults()
        {
            Bscan.IsEnabled = true;
            DGassociation.Visibility = System.Windows.Visibility.Visible;
            Pwait.Visibility = System.Windows.Visibility.Hidden;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pwait.Visibility = System.Windows.Visibility.Visible;
            Bscan.IsEnabled = false;
            DGassociation.Visibility = System.Windows.Visibility.Hidden;
            load();
        }

        #region listview presentation helper
        private void DGassociation_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnsWidth(sender as ListView);
        }
        private void UpdateColumnsWidth(ListView listView)
        {
            int autoFillColumnIndex = 0;
            if (listView.ActualWidth == Double.NaN)
                listView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            double remainingSpace = listView.ActualWidth;
            for (int i = 0; i < (listView.View as GridView).Columns.Count; i++)
                if (i != autoFillColumnIndex)
                    remainingSpace -= (listView.View as GridView).Columns[i].ActualWidth;
            (listView.View as GridView).Columns[autoFillColumnIndex].Width = remainingSpace >= 0 ? remainingSpace-30 : 0;
        }
        #endregion
    }
}
