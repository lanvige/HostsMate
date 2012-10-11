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
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HostsMate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            XDocument doc = XDocument.Load(@"d:\hosts.xml");
            
            var text = from t in doc.Descendants("host")
                       select new
                       {
                           IP = t.Element("IP").Value,
                           Name = t.Element("Name").Value,
                           Branch = t.Element("Branch").Value
                       };

            this.dataGrid2.ItemsSource = text.ToList();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var ip = this.txtIP.Text.Trim();

            //judge the ip format.
            Regex r = new Regex(@"^(\d{1,3}\.){3}\d{1,3}$");
            Match m = r.Match(ip);
            if (!m.Success)
            {
                MessageBox.Show("error");
                return;
            }

            HostsDal hostsDal = new HostsDal();
            IList<HostItem> hostItems = hostsDal.GetHosts();

            foreach (var item in hostItems)
            {
                Regex RegItem = new Regex(@"test([\s\S]*?).englishtown.com", RegexOptions.Compiled);

                if (!item.IsComment)
                {
                    //item.Name
                    Match match = RegItem.Match(item.Name);
                    if (match.Success)
                    {
                        item.IP = ip;
                    }
                }
            }

            hostsDal.Save(hostItems.ToList());
        }


        private void dataGrid2_SelectedCellsChanged_1(object sender, SelectedCellsChangedEventArgs e)
        {
            MessageBox.Show(this.dataGrid2.CurrentItem.ToString());
        }

    }
}
