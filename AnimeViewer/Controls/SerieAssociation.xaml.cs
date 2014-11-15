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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pwait.Visibility = System.Windows.Visibility.Visible;
            Bscan.IsEnabled = false;
            PScaned.Visibility = System.Windows.Visibility.Hidden;
            load();
        }
        private void Bsave_Click(object sender, RoutedEventArgs e)
        {
            hideResults();
            PBprogress.Maximum = DGassociation.Items.Count;
            PBprogress.Value = 0;
            TBMessage.Text = Properties.Settings.Default.MessageUpdating;

            new Thread(new ThreadStart(() =>
            {
                for (int i = 0; i < DGassociation.Items.Count; i++)
                {
                    SerieAssociationEntity currentElement = DGassociation.Items[i] as SerieAssociationEntity;
                    if (currentElement != null && currentElement.SelectedOfferId != null)
                    {
                        int selectedId = currentElement.SelectedOfferId.id;
                        AnimeInfo newInfo;
                        if (selectedId == -1)
                            newInfo = null;
                        else
                            newInfo = Animenewsnetwork.getInformation(selectedId);

                        this.Dispatcher.Invoke(new dvoid(() =>
                        {
                            Serie serie = repository.Series.FirstOrDefault(o => o.Name == currentElement.Serie.Name);
                            serie.Info = newInfo;
                            PBprogress.Value = i + 1;
                            Lprogress.Text = "Updated " + serie.Name + " [" + i + 1 + " of " + PBprogress.Maximum + "]";
                        }));

                    }
                    else
                    {
                        this.Dispatcher.Invoke(new dvoid(() =>
                        {
                            PBprogress.Value = i + 1;
                        }));
                    }

                }
            })).Start();
            this.Dispatcher.Invoke(new dvoid(() =>
            {
                TBMessage.Text = Properties.Settings.Default.MessageUpdated;
            }));

        }
        public void load()
        {
            if(repository  == null)
            {
                showResults();
                return;
            }
           
            List<Serie> series = repository.Series;
            temp.Clear();
            PBprogress.Maximum = series.Count;
            PBprogress.Value = 0;
            TBMessage.Text = Properties.Settings.Default.MessageScanningSeries;

            new Thread(new ThreadStart(() =>
                {
                    Thread[] threads = new Thread[5];

                    int currThread = 0;
                    foreach (Serie serie in series)
                    {
                        while(true)
                        {
                            if (threads[currThread] == null || !threads[currThread].IsAlive)
                            {
                                threads[currThread] = new Thread(new ThreadStart(() =>
                                {
                                    SerieAssociationEntity entity = new SerieAssociationEntity(serie);
                                    this.Dispatcher.Invoke(new dvoid(() =>
                                    {
                                        temp.Add(entity);
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
        private void hideResults()
        {
            Bscan.IsEnabled = true;
            Pwait.Visibility = System.Windows.Visibility.Visible;
            PScaned.Visibility = System.Windows.Visibility.Hidden;
        }
        private void showAnmeNetworkSearch(animenewsnetworkSearch selected)
        {
            if (selected == null)
                return;
            new Thread(new ThreadStart(() =>
            {
                if (selected.id == -1)
                    return;
                AnimeInfo info = Animenewsnetwork.getInformation(selected.id);

                this.Dispatcher.Invoke(new dvoid(() =>
                {
                    lastSearchSelected = info;
                }));
            })).Start();
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
            showAnmeNetworkSearch(selected);

        }
        private void DGassociation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SerieAssociationEntity selected = ((ListView)e.Source).SelectedItem as SerieAssociationEntity;
            if(selected != null)
                showAnmeNetworkSearch(selected.SelectedOfferId);
        }
        #endregion

        private void BchangeName_Click(object sender, RoutedEventArgs e)
        {
            TBnewName.Text = string.Empty;
            Button send = sender as Button;
            Point relativePoint = send.TransformToAncestor(serieassociation).Transform(new Point(0, 0));
            GchangeName.Margin = new Thickness( relativePoint.X,relativePoint.Y-GchangeName.Height,0,0);
            GchangeName.Tag = send.Tag;

            GchangeName.Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_GchangeName_close(object sender, RoutedEventArgs e)
        {
            GchangeName.Visibility = System.Windows.Visibility.Hidden;
        }

        private void TBnewName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GchangeName.Visibility = System.Windows.Visibility.Hidden;
                SerieAssociationEntity entityToModify = GchangeName.Tag as SerieAssociationEntity;
                entityToModify.Posibilities = Animenewsnetwork.GetPosibleNames(TBnewName.Text);
            }
            else if(e.Key == Key.Escape)
            {
                GchangeName.Visibility = System.Windows.Visibility.Hidden;
            }
        }


        

        


    }
}
