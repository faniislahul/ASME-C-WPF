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
        int pointer = -1;
        public ui_bahan_baku()
        {
            InitializeComponent();
            refresh_bb();
            activate.IsEnabled = false;
            edit.IsEnabled = false;
        }

        private void refresh_bb()
        {
            list_bb.Items.Clear();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Bahan_bakus);
           
            /*
            Label ls = new Label();
            ls.Content = "AKTIF";
            ls.FontSize = 10;
            ls.Foreground = SystemColors.GrayTextBrush;
            ls.Margin = new Thickness(0, 0, 0, 0); 
            ls.Padding = new Thickness(0, 0, 0, 0);

            ListBoxItem act = new ListBoxItem();
            act.Content = ls;
            act.IsEnabled = false;

            list_bb.Items.Add(act);
            */
            var list = db.Bahan_bakus.Where(c => c.active == true);
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

                Label lo = new Label();
                lo.Name = "Stock_" + id;
                lo.Content = "Sisa Stok = " + quantity + " " + satuan;
                lo.FontSize = 10;
                lo.Foreground = SystemColors.GrayTextBrush;
                lo.Margin = n;
                lo.Padding = n;

                Label lx = new Label();
                if (bb.active == true)
                {
                    lx.Content = "Status = AKTIF";
                }
                else
                {
                    lx.Content = "Status = NONAKTIF";
                }
                
                lx.FontSize = 10;
                lx.Foreground = SystemColors.GrayTextBrush;
                lx.Margin = n;
                lx.Padding = n;


                StackPanel st = new StackPanel();
                st.Children.Add(ln);
                st.Children.Add(lo);
                st.Children.Add(lx);

                ListBoxItem lbi = new ListBoxItem();
                lbi.Name = "__" + id + "__" + nama.Replace(' ', '_');
                lbi.Content = st;


                list_bb.Items.Add(lbi);
            }

            var enlist = db.Bahan_bakus.Where(c => c.active == false);
            foreach (Bahan_baku bb in enlist)
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
                ln.Foreground = SystemColors.GrayTextBrush;

                Label lo = new Label();
                lo.Name = "Stock_" + id;
                lo.Content = "Sisa Stok = " + quantity + " " + satuan;
                lo.FontSize = 10;
                lo.Foreground = SystemColors.GrayTextBrush;
                lo.Margin = n;
                lo.Padding = n;

                Label lx = new Label();
                if (bb.active == true)
                {
                    lx.Content = "Status = AKTIF";
                }
                else
                {
                    lx.Content = "Status = NONAKTIF";
                }

                lx.FontSize = 10;
                lx.Foreground = SystemColors.GrayTextBrush;
                lx.Margin = n;
                lx.Padding = n;

                StackPanel st = new StackPanel();
                st.Children.Add(ln);
                st.Children.Add(lo);
                st.Children.Add(lx);

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
            if (bb.active == true)
            {
                if (db.master_users.FirstOrDefault(c => c.Id == Properties.Settings.Default.Active_user).role < 3)
                {
                    edit.IsEnabled = true;
                    activate.IsEnabled = true;
                    activate.IsChecked = true;
                }
                else
                {
                    activate.IsEnabled = false;
                }
            }
            else
            {
                if (db.master_users.FirstOrDefault(c => c.Id == Properties.Settings.Default.Active_user).role < 3)
                {
                    activate.IsEnabled = true;
                    activate.IsChecked = false;
                }
                else
                {
                    activate.IsEnabled = false;
                }
            }
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
                Thickness n = new Thickness(0, 0, 0, 0);
                Label nama = new Label();
                nama.Content = "Stok " + stock.Id;
                nama.Margin = new Thickness(0, 5, 0, 0); ;
                nama.Padding = n;
                Label harga = new Label();
                harga.Content = "Harga Beli : " + stock.harga_beli;
                harga.FontSize = 11;
                harga.Foreground = SystemColors.GrayTextBrush;
                harga.Margin = n;
                harga.Padding = n;
                Label qty = new Label();
                qty.Content = "Kuatitas : " + stock.quantity;
                qty.FontSize = 11;
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

        

        private void hapus_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void activate_Unchecked(object sender, RoutedEventArgs e)
        {
            if (db.Bahan_bakus.FirstOrDefault(c => c.Id == selected_bb).active == true)
            {
                if (db.master_users.FirstOrDefault(c => c.Id == Properties.Settings.Default.Active_user).role < 3)
                {
                    int countstock = db.bb_stocks.Where(c => c.tipe == selected_bb).Count();
                    int count_product = db.p_bbs.Where(c => c.bb == selected_bb).Count();
                    if (countstock > 0)
                    {

                        dialog_confirm dd = new dialog_confirm();
                        dd.textBlock.Text = "Bahan baku masih memiliki stok aktif, habiskan stok sebelum melanjutkan";
                        dd.Batal.Visibility = Visibility.Collapsed;
                        dd.Title = "Warning";
                        dd.ok.HorizontalAlignment = HorizontalAlignment.Center;
                        dd.ok.Margin = new Thickness(0, 0, 0, 16);
                        dd.ShowDialog();
                        activate.IsChecked = true;
                    }
                    else
                    {
                        if (count_product > 0)
                        {
                            dialog_confirm dd = new dialog_confirm();
                            dd.textBlock.Text = "Bahan baku masih memiliki produk aktif, produk akan dinonaktifkan";
                            dd.Title = "Warning";
                            dd.ShowDialog();
                            if (dd.conrfirmed == true)
                            {
                                Bahan_baku bb = db.Bahan_bakus.FirstOrDefault(c => c.Id == selected_bb);
                                bb.active = false;
                                var list_p = db.p_bbs.Where(c => c.bb == selected_bb);
                                foreach (p_bb pt in list_p)
                                {
                                    Produk pro = db.Produks.FirstOrDefault(c => c.Id == pt.produk);
                                    pro.active = false;
                                    db.SubmitChanges();
                                }
                                db.SubmitChanges();
                                refresh_bb();
                            }else
                            {
                                activate.IsChecked = true;
                            }
                        }
                        else
                        {
                            dialog_confirm dd = new dialog_confirm();
                            dd.textBlock.Text = "Bahan baku akan dinonaktifkan";
                            dd.Title = "Warning";
                            dd.ShowDialog();
                            if (dd.conrfirmed == true)
                            {
                                Bahan_baku bb = db.Bahan_bakus.FirstOrDefault(c => c.Id == selected_bb);
                                bb.active = false;
                                db.SubmitChanges();
                                refresh_bb();
                            }else
                            {
                                activate.IsChecked = true;
                            }
                        }

                    }
                }
            }
            
        }

        private void activate_Checked(object sender, RoutedEventArgs e)
        {
            
            if (db.Bahan_bakus.FirstOrDefault(c => c.Id == selected_bb).active == false)
            {
                if (db.master_users.FirstOrDefault(c => c.Id == Properties.Settings.Default.Active_user).role < 3)
                {
                    Bahan_baku bb = db.Bahan_bakus.FirstOrDefault(c => c.Id == selected_bb);
                    bb.active = true;
                    db.SubmitChanges();
                    refresh_bb();
                }
            }
                
        }

        private void edit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (db.master_users.FirstOrDefault(c => c.Id == Properties.Settings.Default.Active_user).role < 3)
            {
                Bahan_baku bb = db.Bahan_bakus.FirstOrDefault(c => c.Id == selected_bb);
                dialog_bb_baru dd = new dialog_bb_baru();
                dd.nama_f.Text = bb.nama;
                dd.Pesan.IsEnabled = true;
                var list = dd.comboBox.Items;
                foreach (Label s in list)
                {
                    string s_id = "_" + bb.satuan;
                    if (s.Name == s_id)
                    {
                        dd.comboBox.SelectedItem = s;
                    }
                }
                dd.ShowDialog();
                if (dd.done != false)
                {

                    bb.nama = dd.nama;
                    bb.Satuan_bb = db.Satuan_bbs.Single(c => c.Id == dd.satuan);
                    //bb.Satuan_bb = db.Satuan_bbs.Single(c => c.Id == dd.satuan);
                    db.SubmitChanges();
                    refresh_bb();
                }
            }
        }

        private void edit_hover(object sender, MouseEventArgs e)
        {
            edit.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF2196F3"));
        }

        private void edit_losthover(object sender, MouseEventArgs e)
        {
            edit.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));

        }

    }
}
