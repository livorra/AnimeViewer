using AnimeViewer.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    /// Lógica de interacción para SerieInfoControl.xaml
    /// </summary>
    public partial class SerieInfoControl : UserControl
    {
        public AnimeInfo Info
        {
            get { return (AnimeInfo)GetValue(InfoProperty); }
            set { SetValue(InfoProperty, (AnimeInfo)value); }
        }
        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register("Info", typeof(AnimeInfo), typeof(SerieInfoControl));
        public SerieInfoControl()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string link = ((Label)sender).Content.ToString();
            try
            {
                Process.Start(link);
            }
            catch
            {
                MessageBox.Show("Bad link " + link);
            }
        }
    }
}
