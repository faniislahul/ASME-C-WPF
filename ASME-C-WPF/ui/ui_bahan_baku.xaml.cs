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
using ASME_C_WPF.ui.dialog;

namespace ASME_C_WPF.ui
{
    /// <summary>
    /// Interaction logic for ui_bahan_baku.xaml
    /// </summary>
    public partial class ui_bahan_baku : Page
    {
        CoreDataContext db = new CoreDataContext();
        Core core = new Core();
        int selected_bb = 0;
        int pointer = 0;
        public ui_bahan_baku()
        {
            InitializeComponent();
            refresh_bb();
        }

        private void refresh_bb()
        {
            list_bb.Items.Clear();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Bahan_bakus);
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
                lbi.Name = "__" + id + "__" + nama.Replace(' ', '_');
                lbi.Content = st;


                list_bb.Items.Add(lbi);
            }
            if (pointer > -1)
            {
                list_bb.SelectedItem = list_bb.Items.GetItemAt(pointer);
                refresh_stock();
            }
            
        }

        private void refresh_stock()
        {

            stock_list.Children.Clear();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.bb_stocks);
            ListBoxItem nteu = list_bb.SelectedItem as ListBoxItem;
            Bahan_baku bb = db.Bahan_bakus.FirstOrDefault(c => c.Id == selected_bb);
            bb_title.Content = bb.nama;
            bb_satuan.Content = "Satuan : " + bb.Satuan_bb.nama;
            var ez = db.bb_logs.Where(c => c.tipe == selected_bb && c.used > 0);
            DateTime date = new DateTime();
            foreach (bb_log log in ez)
            {
                date = log.date; 
            }
            bb_date.Content = "Terakhir digunakan : " + date.ToString("ddd dd/MM/yyyy HH:mm:ss");

            var list = db.bb_stocks.Where(c => c.tipe == bb.Id);
            foreach (bb_stock stock in list)
            {
                Thickness n = new Thickness(0, 5, 0, 0);
                Label nama = new Label();
                nama.Content = "Stok " + stock.Id;
                nama.Margin = n;
                nama.Padding = n;
                Label harga = new Label();
                harga.Content = "Harga Beli : " + stock.harga_beli;
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
            if (db.bb_stocks.Where(c => c.tipe == selected_bb).Count() == 0)
            {
                penggunaan.IsEnabled = false;
            }
            else
            {
                penggunaan.IsEnabled = true;
            }
            }

        

        private void on_bb_select(object sender, SelectionChangedEventArgs e)
        {
            if (list_bb.SelectedItem != null)
            {
                int id = 0;
                ListBoxItem nteu = list_bb.SelectedItem as ListBoxItem;
                id = Int32.Parse(nteu.Name.Split('_')[2]);
                selected_bb = id;
                pointer = list_bb.SelectedIndex;
                refresh_stock();
                
            }
            
            
        }

        private void penggunaan_Click(object sender, RoutedEventArgs e)
        {
            if (selected_bb > 0)
            {
                if(db.bb_stocks.Where(c => c.tipe == selected_bb).Count() > 0)
                {
                    int count = db.bb_stocks.Where(c => c.tipe == selected_bb).Sum(c => c.quantity);
                    dialog.dialog_penggunaan_bb dd = new ui.dialog.dialog_penggunaan_bb();
                    dd.count = count;
                    dd.ShowDialog();
                    if (dd.done == true)
                    {
                        Dictionary<String, dynamic> data = new Dictionary<string, dynamic>();
                        data.Add("quantity", dd.qty);
                        data.Add("id", selected_bb);
                        core.Pengurangan_stock(data);
                        refresh_bb();
                    }
                }
                
            }

        }

        private void tambah_Click(object sender, RoutedEventArgs e)
        {
            if (selected_bb > 0)
            {
                dialog.dialog_penambahan_bb dd = new ui.dialog.dialog_penambahan_bb();
                dd.ShowDialog();
                if (dd.done == true)
                {
                    Dictionary<String, dynamic> data = new Dictionary<string, dynamic>();
                    data.Add("jumlah", dd.total);
                    data.Add("quantity", dd.qty);
                    data.Add("id", selected_bb);
                    core.Penambahan_stock(data);
                    refresh_bb();
                }
            }
        }

        private void search_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != "")
            {
                foreach (ListBoxItem s in list_bb.Items)
                {
                    if (s.Name.ToLower().Replace('_', ' ').Contains(search.Text.ToLower()))
                    {
                        s.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        s.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                foreach (ListBoxItem s in list_bb.Items)
                {
                    s.Visibility = Visibility.Visible;
                }
            }
        }

        private void baru_Click(object sender, RoutedEventArgs e)
        {
            dialog_bb_baru dd = new dialog_bb_baru();
            dd.ShowDialog();
            if (dd.done != false)
            {
                Bahan_baku bb = new Bahan_baku();
                bb.nama = dd.nama;
                bb.satuan = dd.satuan;
                db.Bahan_bakus.InsertOnSubmit(bb);
                db.SubmitChanges();
                refresh_bb();
            }
        }
    }
}
