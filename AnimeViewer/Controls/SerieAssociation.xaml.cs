using AnimeViewer.Classes;
using AnimeViewer.External;
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
        ObservableCollection<SerieAssociationEntity> temp = new ObservableCollection<SerieAssociationEntity>();
        public Repository repository
        {
            get { return (Repository)GetValue(repositoryProperty); }
            set { SetValue(repositoryProperty, (Repository)value); }
        }
        public static readonly DependencyProperty repositoryProperty = DependencyProperty.Register("repository", typeof(Repository), typeof(SerieAssociation));

        public AnimeInfo lastSearchSelected
        {
            get { return (AnimeInfo)GetValue(lastSearchSelectedProperty); }
            set { SetValue(lastSearchSelectedProperty, (AnimeInfo)value); }
        }
        public static readonly DependencyProperty lastSearchSelectedProperty = DependencyProperty.Register("lastSearchSelected", typeof(AnimeInfo), typeof(SerieAssociation));

        
        public SerieAssociation()
        {
            InitializeComponent();
        }
        public void load()
        {
            if(repository  == null)
            {
                showResults();
                return;
            }
           
            List<Serie> series = repository.Series;
            PBprogress.Maximum = series.Count;

            new Thread(new ThreadStart(() =>
                {
                    Thread[] threads = new Thread[5];

                    int max = 0;
                    int currThread = 0;
                    foreach (Serie serie in series)
                    {
                        max++;
                        if (max > 5)
                            break;
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
                                        Lprogress.Text = "Obtained posibilities for " +serie.Name + " ["+ PBprogress.Value + " of " + PBprogress.Maximum+"]";
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
            Pwait.Visibility = System.Windows.Visibility.Hidden;
            PScaned.Visibility = System.Windows.Visibility.Visible;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pwait.Visibility = System.Windows.Visibility.Visible;
            Bscan.IsEnabled = false;
            PScaned.Visibility = System.Windows.Visibility.Hidden;
            load();
        }
        private void Bsave_Click(object sender, RoutedEventArgs e)
        {

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
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            animenewsnetworkSearch selected = ((ComboBox)e.Source).SelectedItem as animenewsnetworkSearch;
            if (selected == null)
                return;
            new Thread(new ThreadStart(() =>
            {

                AnimeInfo info = Animenewsnetwork.getInformation(selected.id);
                this.Dispatcher.Invoke(new dvoid(() =>
                {
                    lastSearchSelected = info;
                }));
            })).Start();

        }
        #endregion

        


    }
}
