using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ASME_C_WPF.core;

namespace ASME_C_WPF.ui
{
    /// <summary>
    /// Interaction logic for ui_bahan_baku.xaml
    /// </summary>
    public partial class ui_bahan_baku : Page
    {
        CoreDataContext db = new CoreDataContext();
        public ui_bahan_baku()
        {
            InitializeComponent();

            
            var list = db.Bahan_bakus;
            foreach (Bahan_baku bb in list)
            {
                Thickness n = new Thickness(0, 0, 0, 0);
                String nama = bb.nama;
                int id = bb.Id;
                long harga_beli = bb.harga_beli;
                int quantity = bb.quantity;
                String satuan = bb.Satuan_bb.nama;
                Label ln = new Label();
                ln.Name = "nama_" + id;
                ln.Content = nama;
                ln.Padding = n;
                ln.Margin = n;

                Label ls = new Label();
                ls.Name = "Stock_" + id;
                ls.Content = "Sisa Stok = " + quantity + " " + satuan;
                ls.FontSize = 10;
                ls.Foreground = SystemColors.GrayTextBrush;
                ls.Margin = n;
                ls.Padding = n;


                StackPanel st = new StackPanel();
                st.Children.Add(ln);
                st.Children.Add(ls);

                ListBoxItem lbi = new ListBoxItem();
                lbi.Name = nama;
                lbi.Content = st;
                

                list_bb.Items.Add(lbi);
            }

            
             
        }

        private void penggunaan_Click(object sender, RoutedEventArgs e)
        {
            dialog.dialog_penggunaan_bb dialog = new ui.dialog.dialog_penggunaan_bb();
            dialog.Show();
        }

        private void on_bb_select(object sender, SelectionChangedEventArgs e)
        {

            stock_list.Children.Clear();
            ListBoxItem nteu = list_bb.SelectedItem as ListBoxItem;
            Bahan_baku bb = db.Bahan_bakus.FirstOrDefault(c => c.nama == nteu.Name);
            bb_title.Content = nteu.Name;
            bb_satuan.Content = bb.Satuan_bb.nama;


            var list = db.bb_stocks.Where(c=>c.tipe == bb.Id);
            foreach(bb_stock stock in list)
            {
                Thickness n = new Thickness(0, 0, 0, 0);
                Label nama = new Label();
                nama.Content = "Stok " + stock.Id;
                nama.Margin = n;
                nama.Padding = n;
                Label harga = new Label();
                harga.Content = "Harga Beli : "+stock.harga_beli;
                harga.FontSize = 10;
                harga.Foreground = SystemColors.GrayTextBrush;
                harga.Margin = n;
                harga.Padding = n;
                Label qty = new Label();
                qty.Content = "Kuatitas : " + stock.quantity;
                qty.FontSize = 10;
                qty.Foreground = SystemColors.GrayTextBrush;
                qty.Margin = n;
                qty.Padding = n;
                stock_list.Children.Add(nama);
                stock_list.Children.Add(harga);
                stock_list.Children.Add(qty);

            }
        }
    }
}
