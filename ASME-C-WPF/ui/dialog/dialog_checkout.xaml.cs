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
                lbi.Name = "__" + p_id + "__" + name.Replace(' ', '_');
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
    }
}
