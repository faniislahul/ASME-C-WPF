using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ASME_C_WPF.core;

namespace ASME_C_WPF.ui.dialog
{
    /// <summary>
    /// Interaction logic for dialog_penggunaan_bb.xaml
    /// </summary>
    public partial class dialog_bb_baru : Window
    {
        CoreDataContext db = new CoreDataContext();
        public int satuan = 0;
        public String nama = "";
        public bool done = false;
        public dialog_bb_baru()
        {
            InitializeComponent();
            nama_f.Focus();
            Pesan.IsEnabled = false;

            var st = db.Satuan_bbs.Where(c=>c.active==true);
            foreach(Satuan_bb sat in st)
            {
                Label n = new Label();
                n.Content = sat.nama;
                n.Name = "_" + sat.Id;

                ///ComboBoxItem list = new ComboBoxItem();
                ///list.Content = n;
                ///list.Name = "_"+sat.Id;

                comboBox.Items.Add(n);
            }
            if (db.Satuan_bbs.Count() > 0)
            {
                comboBox.SelectedItem = comboBox.Items.GetItemAt(0);
            }
            
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pesan_Click(object sender, RoutedEventArgs e)
        {
            if (nama_f.Text != "")
            {
                nama = nama_f.Text;
                done = true;
                this.Close();

            }
        }


        private void nama_f_KeyUp(object sender, KeyEventArgs e)
        {
            if (nama_f.Text != "")
            {
                    
                        Pesan.IsEnabled = true;
                      
            }
            else
            {
                Pesan.IsEnabled = false;
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Label combo = (Label) comboBox.SelectedItem;
            satuan = Int32.Parse(combo.Name.Substring(1).ToString());
        }
    }
}
