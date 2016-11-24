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
    /// Interaction logic for dialog_checkout.xaml
    /// </summary>
    /// 

    
    public partial class dialog_checkout : Window
    {
        CoreDataContext db = new CoreDataContext();
        Regex rg = new Regex("^[0-9]+$");
        long total_val = 0;
        int selected = 0;
        public bool done = false;
        public int choice = 0;
        public long pembayaran = 0;
        public long change = 0;
        public String cardnumber = "";
        public String transaction = "";
        public dialog_checkout()
        {
            InitializeComponent();
            cardlist.Visibility = Visibility.Collapsed;
            cashlist.Visibility = Visibility.Visible;

        }

        public void receipt_refresh(int o_id)
        {
            header.Content = "#"+o_id+" "+db.pos_orders.FirstOrDefault(c => c.Id == o_id).details;
            detail.Content = db.pos_orders.FirstOrDefault(c => c.Id == o_id).pos_table.name;
            receipt.SelectedItem = null;
            receipt.Items.Clear();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.pos_order_lists);
            var list = db.pos_order_lists.Where(c => c.order_id == o_id && c.status == "PENDING").GroupBy(c => c.peroduk);
            long stotal = 0;
            Thickness n = new Thickness(0, 0, 0, 0);
            foreach (var enlist in list)
            {
                String name = "";
                long harga = 0;
                int qty = 0;
                long st = 0;
                int p_id = 0;
                foreach (pos_order_list nested in enlist)
                {
                    name = nested.Produk.nama;
                    harga = nested.Produk.harga_jual;
                    qty += nested.quantity;
                    p_id = nested.peroduk;
                }


                Label ln = new Label();
                ln.Content = qty + " x " + name;
                ln.Padding = n;
                ln.Margin = n;

                Label ls = new Label();
                ls.Content = harga.ToString("C");
                ls.FontSize = 10;
                ls.Foreground = SystemColors.GrayTextBrush;
                ls.Margin = n;
                ls.Padding = n;

                Label lq = new Label();
                lq.Content = (qty * harga).ToString("C");
                lq.FontSize = 14;

                lq.HorizontalAlignment = HorizontalAlignment.Right;

                StackPanel strait = new StackPanel();
                strait.Children.Add(ln);
                strait.Children.Add(ls);
                strait.HorizontalAlignment = HorizontalAlignment.Left;

                Grid line = new Grid();
                line.HorizontalAlignment = HorizontalAlignment.Stretch;
                //line.Orientation = Orientation.Horizontal;
                line.Width = 300;
                line.Children.Add(strait);
                line.Children.Add(lq);


                //toplist;

                ListBoxItem lbi = new ListBoxItem();
                lbi.Name = "__" + p_id + "__" + name.Replace(' ', '_').Replace('&', '_');
                lbi.Content = line;
                
                receipt.Items.Add(lbi);

                stotal += harga * qty;

            }


            ///static portion
            pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == o_id);
            long discount = (order.discount * stotal) / 100;
            long serv_total = order.service;
            long ppn_total = ((Properties.Settings.Default.tax_ppn) * stotal) / 100;
            Label dsc = new Label();
            dsc.Content = "(" + (discount).ToString("C") + ")";
            dsc.Margin = n;
            dsc.Padding = n;
            dsc.HorizontalAlignment = HorizontalAlignment.Right;

            Label dscText = new Label();
            dscText.Content = "Discount " + order.discount + "%";
            dscText.Margin = n;
            dscText.Padding = n;
            dscText.HorizontalAlignment = HorizontalAlignment.Left;

            Grid dsc_temp = new Grid();
            dsc_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
            dsc_temp.Width = 300;
            dsc_temp.Children.Add(dscText);
            dsc_temp.Children.Add(dsc);

            Label service = new Label();
            service.Content = (serv_total).ToString("C");
            service.Margin = n;
            service.Padding = n;
            service.HorizontalAlignment = HorizontalAlignment.Right;

            Label servText = new Label();
            servText.Content = "Service";
            servText.Margin = n;
            servText.Padding = n;
            servText.HorizontalAlignment = HorizontalAlignment.Left;

            Grid serv_temp = new Grid();
            serv_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
            serv_temp.Width = 300;
            serv_temp.Children.Add(servText);
            serv_temp.Children.Add(service);

            Label ppn = new Label();
            ppn.Content = (ppn_total).ToString("C");
            ppn.Margin = n;
            ppn.Padding = n;
            ppn.HorizontalAlignment = HorizontalAlignment.Right;

            Label ppnText = new Label();
            ppnText.Content = "PPN " + Properties.Settings.Default.tax_ppn + "%";
            ppnText.Margin = n;
            ppnText.Padding = n;
            ppnText.HorizontalAlignment = HorizontalAlignment.Left;

            Grid ppn_temp = new Grid();
            ppn_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
            ppn_temp.Width = 300;
            ppn_temp.Children.Add(ppnText);
            ppn_temp.Children.Add(ppn);

            Label sub = new Label();
            sub.Content = (stotal).ToString("C");
            sub.FontSize = 15;
            sub.Margin = n;
            sub.Padding = n;
            sub.HorizontalAlignment = HorizontalAlignment.Right;

            Label subText = new Label();
            subText.Content = "SUBTOTAL";
            subText.FontSize = 11;
            subText.Foreground = SystemColors.GrayTextBrush;
            subText.Margin = n;
            subText.Padding = n;
            subText.HorizontalAlignment = HorizontalAlignment.Left;

            Grid sub_temp = new Grid();
            sub_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
            sub_temp.Width = 300;
            sub_temp.Children.Add(subText);
            sub_temp.Children.Add(sub);

            Label tot = new Label();
            tot.Content = ((stotal - discount) + ppn_total).ToString("C");
            tot.FontSize = 15;
            tot.Margin = n;
            tot.Padding = n;
            tot.HorizontalAlignment = HorizontalAlignment.Right;

            Label totText = new Label();
            totText.Content = "TOTAL";
            totText.FontSize = 11;
            totText.Foreground = SystemColors.GrayTextBrush;
            totText.Margin = n;
            totText.Padding = n;
            totText.HorizontalAlignment = HorizontalAlignment.Left;

            Grid tot_temp = new Grid();
            tot_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
            tot_temp.Width = 300;
            tot_temp.Children.Add(totText);
            tot_temp.Children.Add(tot);

            Rectangle devider = new Rectangle();
            devider.Margin = n;
            devider.MinHeight = 1;
            devider.Width = 300;
            devider.Fill = SystemColors.GrayTextBrush;

            ListBoxItem dev1 = new ListBoxItem();
            dev1.Content = devider;
            dev1.IsEnabled = false;

            ListBoxItem subt = new ListBoxItem();
            subt.Content = sub_temp;
            subt.IsEnabled = false;

            ListBoxItem dislis = new ListBoxItem();
            dislis.Content = dsc_temp;

            ListBoxItem ppnlis = new ListBoxItem();
            ppnlis.Content = ppn_temp;

            ListBoxItem servlis = new ListBoxItem();
            servlis.Content = serv_temp;

            ListBoxItem dev2 = new ListBoxItem();
            dev2.Content = devider;
            dev2.IsEnabled = false;
            ListBoxItem totlist = new ListBoxItem();
            totlist.Content = tot_temp;
            totlist.IsEnabled = false;


            receipt.Items.Add(dev2);
            receipt.Items.Add(subt);
            receipt.Items.Add(dislis);
            receipt.Items.Add(ppnlis);
            receipt.Items.Add(servlis);
            receipt.Items.Add(dev1);
            receipt.Items.Add(totlist);
            long total_value = (stotal - discount) + serv_total + ppn_total;
            total_val = total_value;
            total_disp.Content = total_value.ToString("C");
            selected = o_id;
        }

        private void payment_KeyUp(object sender, KeyEventArgs e)
        {
            if (payment.Text != "")
            {
                if (rg.IsMatch(payment.Text))
                {
                    if (Int32.Parse(payment.Text) < 999999999)
                    {
                        OK.IsEnabled = true;
                        kembalian.Content = (Int64.Parse(payment.Text) - total_val).ToString("C");
                    }
                    else
                    {
                        OK.IsEnabled = false;
                    }
                    
                    
                    
                }
                else
                {
                    OK.IsEnabled = false;
                }

            }
            else
            {
                OK.IsEnabled = false;
            }
        }

        private void cashradio_Checked(object sender, RoutedEventArgs e)
        {
            if (cardlist != null)
            {
                cardlist.Visibility = Visibility.Collapsed;
                cashlist.Visibility = Visibility.Visible;
            }
        }

        private void cardradio_Checked(object sender, RoutedEventArgs e)
        {
            if (cashlist != null)
            {
                cashlist.Visibility = Visibility.Collapsed;
                cardlist.Visibility = Visibility.Visible;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (cashradio.IsChecked == true)
            {
                done = true;
                choice = 1;
                pembayaran = Int64.Parse(payment.Text);
                change = Int64.Parse(payment.Text) - total_val;
                print();
                this.Close();
            }
            else
            {
                done = true;
                choice = 2;
                pembayaran = total_val;
                change = 0;
                cardnumber = card_no.Text;
                transaction = tx_id.Text;
                this.Close();
            }
        }

        private void print()
        {
            int selected_order = selected;
            if (selected_order > 0 && ASME_C_WPF.Properties.Settings.Default.print_enable == true)
            {
                var orders = db.pos_order_lists.Where(c => c.Id == selected_order && c.status == "PENDING");
                pos_order po = db.pos_orders.FirstOrDefault(c => c.Id == selected_order);

                PrintDialog pd = new PrintDialog();
                StackPanel lv = new StackPanel();
                Label title = new Label();
                title.Padding = new Thickness(0, 0, 20, 0);
                title.FontSize = 20;
                title.Foreground = SystemColors.ActiveCaptionTextBrush;
                title.HorizontalAlignment = HorizontalAlignment.Center;
                title.Content = ASME_C_WPF.Properties.Settings.Default.Client_Name.ToString();
                title.FontWeight = FontWeights.Bold;

                Label add = new Label();
                add.FontSize = 12;
                add.Padding = new Thickness(0, 0, 20, 0);
                add.Foreground = SystemColors.ActiveCaptionTextBrush;
                add.HorizontalAlignment = HorizontalAlignment.Center;
                add.Content = ASME_C_WPF.Properties.Settings.Default.Client_address_1.ToString();

                Label telp = new Label();
                telp.FontSize = 12;
                telp.Padding = new Thickness(0, 0, 20, 0);
                telp.Foreground = SystemColors.ActiveCaptionTextBrush;
                telp.HorizontalAlignment = HorizontalAlignment.Center;
                telp.Content = ASME_C_WPF.Properties.Settings.Default.Client_address_2.ToString();

                Rectangle devider3 = new Rectangle();
                devider3.Margin = new Thickness(0);
                devider3.MinHeight = 1;
                devider3.Width = 300;
                devider3.Fill = SystemColors.GrayTextBrush;

                Rectangle devider4 = new Rectangle();
                devider4.Margin = new Thickness(0);
                devider4.MinHeight = 1;
                devider4.Width = 300;
                devider4.Fill = SystemColors.GrayTextBrush;

                Label a1 = new Label();
                a1.FontSize = 11;
                a1.Padding = new Thickness(0);
                a1.Foreground = SystemColors.ActiveCaptionTextBrush;
                a1.HorizontalAlignment = HorizontalAlignment.Left;
                a1.Content = po.date.ToString("dd/MM/yyyy");

                Label a2 = new Label();
                a2.FontSize = 11;
                a2.Padding = new Thickness(0);
                a2.Foreground = SystemColors.ActiveCaptionTextBrush;
                a2.HorizontalAlignment = HorizontalAlignment.Left;
                a2.Content = "#" + po.Id;

                Label a3 = new Label();
                a3.FontSize = 11;
                a3.Padding = new Thickness(0);
                a3.Foreground = SystemColors.ActiveCaptionTextBrush;
                a3.HorizontalAlignment = HorizontalAlignment.Right;
                a3.Content = po.details;

                Label a4 = new Label();
                a4.FontSize = 11;
                a4.Padding = new Thickness(0);
                a4.Foreground = SystemColors.ActiveCaptionTextBrush;
                a4.HorizontalAlignment = HorizontalAlignment.Right;
                a4.Content = po.pos_table.name;

                StackPanel al = new StackPanel();
                al.Children.Add(a1);
                al.Children.Add(a2);
                al.HorizontalAlignment = HorizontalAlignment.Right;

                StackPanel az = new StackPanel();
                az.Children.Add(a3);
                az.Children.Add(a4);
                al.HorizontalAlignment = HorizontalAlignment.Left;

                Grid aw = new Grid();
                aw.Children.Add(al);
                aw.Children.Add(az);
                aw.HorizontalAlignment = HorizontalAlignment.Stretch;
                aw.Margin = new Thickness(20, 0, 50, 0);

                lv.Width = pd.PrintableAreaWidth;
                lv.Margin = new Thickness(15, 0, 35, 0);
                lv.Children.Add(title);
                lv.Children.Add(add);
                lv.Children.Add(telp);
                lv.Children.Add(devider3);
                lv.Children.Add(aw);
                lv.Children.Add(devider4);



                var list = db.pos_order_lists.Where(c => c.order_id == selected_order && c.status == "PENDING").GroupBy(c => c.peroduk);
                long stotal = 0;
                Thickness n = new Thickness(0, 0, 0, 0);
                foreach (var enlist in list)
                {
                    String name = "";
                    long harga = 0;
                    int qty = 0;
                    int p_id = 0;
                    foreach (pos_order_list nested in enlist)
                    {
                        name = nested.Produk.nama;
                        harga = nested.Produk.harga_jual;
                        qty += nested.quantity;
                        p_id = nested.peroduk;
                    }


                    Label ln = new Label();
                    ln.Content = name;
                    ln.FontSize = 11;
                    ln.Foreground = SystemColors.ActiveCaptionTextBrush;
                    ln.HorizontalAlignment = HorizontalAlignment.Left;


                    Label ls = new Label();
                    ls.Content = qty + " x " + harga.ToString("C");
                    ls.FontSize = 11;
                    ls.Foreground = SystemColors.ActiveCaptionTextBrush;
                    ls.HorizontalAlignment = HorizontalAlignment.Center;

                    Label lq = new Label();
                    lq.Margin = new Thickness(0, 0, 10, 0);
                    lq.Content = (qty * harga).ToString("C");
                    lq.FontSize = 11;
                    lq.Foreground = SystemColors.ActiveCaptionTextBrush;

                    lq.HorizontalAlignment = HorizontalAlignment.Right;



                    Grid line3 = new Grid();
                    line3.HorizontalAlignment = HorizontalAlignment.Stretch;
                    //line.Orientation = Orientation.Horizontal;
                    line3.Children.Add(ln);
                    line3.Children.Add(ls);
                    line3.Children.Add(lq);
                    line3.Margin = new Thickness(15, 0, 40, 0);


                    //toplist;

                    lv.Children.Add(line3);
                    stotal += harga * qty;


                }

                ///static portion
                n = new Thickness(10, 0, 25, 0);
                pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == selected_order);
                long discount = (order.discount * stotal) / 100;
                long serv_total = order.service;
                long ppn_total = ((Properties.Settings.Default.tax_ppn) * stotal) / 100;

                Label dsc = new Label();
                dsc.Content = "(" + (discount).ToString("C") + ")";
                dsc.Margin = n;
                dsc.Padding = n;
                dsc.HorizontalAlignment = HorizontalAlignment.Right;
                dsc.Foreground = SystemColors.ActiveCaptionTextBrush;

                Label dscText = new Label();
                dscText.Content = "Discount " + order.discount + "%";
                dscText.Margin = n;
                dscText.Padding = n;
                dscText.HorizontalAlignment = HorizontalAlignment.Left;
                dscText.Foreground = SystemColors.ActiveCaptionTextBrush;

                Grid dsc_temp = new Grid();
                dsc_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
                dsc_temp.Children.Add(dscText);
                dsc_temp.Children.Add(dsc);

                Label service = new Label();
                service.Content = (serv_total).ToString("C");
                service.Margin = n;
                service.Padding = n;
                service.HorizontalAlignment = HorizontalAlignment.Right;
                service.Foreground = SystemColors.ActiveCaptionTextBrush;

                Label servText = new Label();
                servText.Content = "Service";
                servText.Margin = n;
                servText.Padding = n;
                servText.HorizontalAlignment = HorizontalAlignment.Left;
                servText.Foreground = SystemColors.ActiveCaptionTextBrush;

                Grid serv_temp = new Grid();
                serv_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
                serv_temp.Children.Add(servText);
                serv_temp.Children.Add(service);

                Label ppn = new Label();
                ppn.Content = (ppn_total).ToString("C");
                ppn.Margin = n;
                ppn.Padding = n;
                ppn.HorizontalAlignment = HorizontalAlignment.Right;
                ppn.Foreground = SystemColors.ActiveCaptionTextBrush;

                Label ppnText = new Label();
                ppnText.Content = "PPN " + Properties.Settings.Default.tax_ppn + "%";
                ppnText.Margin = n;
                ppnText.Padding = n;
                ppnText.HorizontalAlignment = HorizontalAlignment.Left;
                ppnText.Foreground = SystemColors.ActiveCaptionTextBrush;

                Grid ppn_temp = new Grid();
                ppn_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
                ppn_temp.Children.Add(ppnText);
                ppn_temp.Children.Add(ppn);

                Label sub = new Label();
                sub.Content = (stotal).ToString("C");
                sub.FontSize = 13;
                sub.Margin = n;
                sub.Padding = n;
                sub.HorizontalAlignment = HorizontalAlignment.Right;
                sub.Foreground = SystemColors.ActiveCaptionTextBrush;

                Label subText = new Label();
                subText.Margin = n;
                subText.Padding = n;
                subText.Content = "SUBTOTAL";
                subText.FontSize = 13;
                subText.Foreground = SystemColors.ActiveCaptionTextBrush;
                subText.HorizontalAlignment = HorizontalAlignment.Left;

                Grid sub_temp = new Grid();
                sub_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
                sub_temp.Children.Add(subText);
                sub_temp.Children.Add(sub);

                Label tot = new Label();
                tot.Content = ((stotal - discount) + ppn_total).ToString("C");
                tot.FontSize = 13;
                tot.Margin = n;
                tot.Padding = n;
                tot.HorizontalAlignment = HorizontalAlignment.Right;
                tot.Foreground = SystemColors.ActiveCaptionTextBrush;

                Label totText = new Label();
                totText.Content = "TOTAL";
                totText.FontSize = 13;
                totText.Foreground = SystemColors.ActiveCaptionTextBrush;
                totText.Margin = n;
                totText.Padding = n;
                totText.HorizontalAlignment = HorizontalAlignment.Left;

                Grid tot_temp = new Grid();
                tot_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
                tot_temp.Children.Add(totText);
                tot_temp.Children.Add(tot);

                Label pem = new Label();
                pem.Content = pembayaran.ToString("C");
                pem.FontSize = 13;
                pem.Margin = n;
                pem.Padding = n;
                pem.HorizontalAlignment = HorizontalAlignment.Right;
                pem.Foreground = SystemColors.ActiveCaptionTextBrush;

                Label pemText = new Label();
                pemText.Content = "PEMBAYARAN";
                pemText.FontSize = 13;
                pemText.Foreground = SystemColors.ActiveCaptionTextBrush;
                pemText.Margin = n;
                pemText.Padding = n;
                pemText.HorizontalAlignment = HorizontalAlignment.Left;

                Grid pem_temp = new Grid();
                pem_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
                pem_temp.Children.Add(pemText);
                pem_temp.Children.Add(pem);

                Label kem = new Label();
                kem.Content = change.ToString("C");
                kem.FontSize = 13;
                kem.Margin = n;
                kem.Padding = n;
                kem.HorizontalAlignment = HorizontalAlignment.Right;
                kem.Foreground = SystemColors.ActiveCaptionTextBrush;

                Label kemText = new Label();
                kemText.Content = "KEMBALIAN";
                kemText.FontSize = 13;
                kemText.Foreground = SystemColors.ActiveCaptionTextBrush;
                kemText.Margin = n;
                kemText.Padding = n;
                kemText.HorizontalAlignment = HorizontalAlignment.Left;

                Grid kem_temp = new Grid();
                kem_temp.HorizontalAlignment = HorizontalAlignment.Stretch;
                kem_temp.Children.Add(kemText);
                kem_temp.Children.Add(kem);

                Rectangle devider = new Rectangle();
                devider.Margin = n;
                devider.MinHeight = 1;
                devider.Width = 300;
                devider.Fill = SystemColors.GrayTextBrush;
                Rectangle devider2 = new Rectangle();
                devider2.Margin = n;
                devider2.MinHeight = 1;
                devider2.Width = 300;
                devider2.Fill = SystemColors.GrayTextBrush;

                Rectangle devider5 = new Rectangle();
                devider5.Margin = n;
                devider5.MinHeight = 1;
                devider5.Width = 300;
                devider5.Fill = SystemColors.GrayTextBrush;
                

                Label subfooter = new Label();
                subfooter.Content = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + db.master_users.FirstOrDefault(c => c.Id == ASME_C_WPF.Properties.Settings.Default.Active_user).name;
                subfooter.FontSize = 11;
                subfooter.Foreground = SystemColors.ActiveCaptionTextBrush;
                subfooter.HorizontalAlignment = HorizontalAlignment.Left;

                Label subfooter2 = new Label();

                subfooter2.Content = "Terima kasih atas kunjungan Anda";
                subfooter2.FontSize = 11;
                subfooter2.Foreground = SystemColors.ActiveCaptionTextBrush;
                subfooter2.HorizontalAlignment = HorizontalAlignment.Center;

                lv.Children.Add(devider);
                lv.Children.Add(sub_temp);
                lv.Children.Add(dsc_temp);
                lv.Children.Add(ppn_temp);
                lv.Children.Add(serv_temp);
                lv.Children.Add(tot_temp);
                lv.Children.Add(devider2);
                lv.Children.Add(pem_temp);
                lv.Children.Add(kem_temp);
                lv.Children.Add(devider5);
                lv.Children.Add(subfooter);
                lv.Children.Add(subfooter2);
                

                pd.PrintVisual(lv, "Lets Print Something");
            }

        }
    }
}
