using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using ASME_C_WPF.core;
using ASME_C_WPF.ui.dialog;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace ASME_C_WPF
{
    /// <summary>
    /// Interaction logic for ui_dash.xaml
    /// </summary>
    public partial class ui_cashier : Page
    {
        static int active=0;
        CoreDataContext db = new CoreDataContext();
        Pos_module pos = new Pos_module();
        private int selected_order = 0;
        private long total = 0;
        private long subtotal = 0;
        private long service_value = 0;
        public ui_cashier()
        {
            InitializeComponent();
            

            refresh_product();
            refresh_order();
            Pos_module.user = Properties.Settings.Default.Active_user;
        }

        private void refresh_product()
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Produks);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Categories);
            product_list.Items.Clear();
            Thickness tn = new Thickness(0, 0, 0, 0);
            var catlist = db.Categories;
            ItemsPanelTemplate n = new ItemsPanelTemplate();
            var temp = new FrameworkElementFactory(typeof(UniformGrid));
            temp.SetValue(UniformGrid.ColumnsProperty, 2);
            temp.SetValue(UniformGrid.VerticalAlignmentProperty, VerticalAlignment.Top);
            n.VisualTree = temp;
            product_list.ItemsPanel = n;
            foreach (Category cat in catlist)
            {
                Label la = new Label();
                la.Content = cat.nama.ToUpper();
                la.Foreground = SystemColors.GrayTextBrush;
                la.FontSize = 11;
                la.Padding = tn;
                la.Margin = new Thickness(5, 0, 0, 0);

                ListBoxItem lbcat = new ListBoxItem();
                lbcat.Name = "__000__" + cat.nama.Replace(' ', '_').Replace('&', '_'); ;
                lbcat.Content = la;
                lbcat.IsEnabled = false;

                ListBoxItem empty1 = new ListBoxItem();
                empty1.Name = "__0000__" + cat.nama.Replace(' ', '_').Replace('&', '_'); ;
                empty1.Content = "";
                empty1.IsEnabled = false;
                ListBoxItem empty2 = new ListBoxItem();
                empty2.Name = "__00000__" + cat.nama.Replace(' ', '_').Replace('&', '_'); ;
                empty2.Content = "";
                empty2.IsEnabled = false;




                product_list.Items.Add(lbcat);
                product_list.Items.Add(empty1);

                var pt = db.Produks.Where(c => c.category == cat.Id && c.active == true);
                foreach (Produk product in pt)
                {
                    String nama = product.nama;
                    int id = product.Id;
                    long harga = product.harga_jual;

                    Label ln = new Label();
                    ln.Content = nama;
                    ln.Padding = tn;
                    ln.Margin = tn;

                    Label ls = new Label();
                    ls.Content = harga.ToString("C");
                    ls.FontSize = 10;
                    ls.Foreground = SystemColors.GrayTextBrush;
                    ls.Margin = tn;
                    ls.Padding = tn;

                    

                    StackPanel stp = new StackPanel();
                    stp.Orientation = Orientation.Horizontal;

                    MaterialIcons.MaterialIcon circle = new MaterialIcons.MaterialIcon();
                    circle.Icon = Core.iconlist.ElementAt(cat.pic);
                    circle.Height = 25;
                    circle.Width = 25;
                    circle.Margin = new Thickness(5, 0, 5, 0);
                    circle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF404040"));

                    StackPanel st = new StackPanel();
                    st.Children.Add(ln);
                    st.Children.Add(ls);
                    

                    stp.Children.Add(circle);
                    stp.Children.Add(st);

                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Name = "__" + id + "__" + nama.Replace(' ', '_').Replace('&', '_') + "__" + cat.nama.Replace(' ', '_').Replace('&', '_'); 
                    lbi.Content = stp;
                    


                    product_list.Items.Add(lbi);
                }

                
                if (db.Produks.Where(c => c.category == cat.Id&&c.active==true).Count() % 2 == 1)
                {
                    product_list.Items.Add(empty2);
                }
                else
                {

                }




            }



        }

        public static void return_state()
        {
            active = 0;
        }
        public void refresh_order()
        {
            order_list.Items.Clear();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.pos_orders);
            var list = db.pos_orders;
            foreach (pos_order order in list)
            {
                if (order.status == "PENDING")
                {
                    Thickness n = new Thickness(0, 0, 0, 0);
                    String nama = order.pos_table.name;
                    String details = order.details;
                    String ordeid = "#"+order.Id;
                    int id = order.Id;
                    String created = order.date.ToString();

                    Label ln = new Label();
                    ln.Content = details;
                    ln.Padding = n;
                    ln.Margin = n;

                    Label ls = new Label();
                    ls.Content = nama;
                    ls.FontSize = 12;
                    ls.Foreground = SystemColors.GrayTextBrush;
                    ls.Margin = n;
                    ls.Padding = n;

                    Label lq = new Label();
                    lq.Content = "Order #"+ordeid;
                    lq.FontSize = 10;
                    lq.Foreground = SystemColors.GrayTextBrush;
                    lq.Margin = n;
                    lq.Padding = n;

                    Label lt = new Label();
                    lt.Content = created;
                    lt.FontSize = 10;
                    lt.Foreground = SystemColors.GrayTextBrush;
                    lt.Margin = n;
                    lt.Padding = n;

                    
                    
                    StackPanel st = new StackPanel();
                    st.Children.Add(ln);
                    st.Children.Add(ls);
                    st.Children.Add(lq);
                    st.Children.Add(lt);

                    //stp.Children.Add(st);

                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Name = "order_" + id;
                    lbi.Content = st;


                    order_list.Items.Add(lbi);
                }

                product_list.Items.IsLiveFiltering = true;
                
            }
        }

        
        private void create_order_Click(object sender, RoutedEventArgs e)
        {
            if (active == 0)
            {
                dialog_new_order new_order = new dialog_new_order();
                new_order.ShowDialog();
                refresh_order();
                if (order_list.Items.Count != 0)
                {
                    order_list.SelectedItem = order_list.Items.GetItemAt(order_list.Items.Count - 1);
                    ListBoxItem selected = order_list.SelectedItem as ListBoxItem;
                    //reciept_head.Content = selected.Name.ToString();
                    selected_order = Int32.Parse(selected.Name.Substring(6));
                    receipt_refresh(selected_order);
                }
                
            }
            
        }

        private void order_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ListBoxItem order = order_list.SelectedItem as ListBoxItem;
            if (order != null)
            {
                int id = Int32.Parse(order.Name.Substring(6));
                void_button.IsEnabled = true;
                hold_button.IsEnabled = true;
                switch_button.IsEnabled = true;
                
                product_list.SelectedItem = null;
                receipt_refresh(id);
                selected_order = id;
            }

        }

        private void receipt_refresh(int o_id)
        {
            reciept_list.SelectedItem = null; 
            reciept_list.Items.Clear();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues,db.pos_order_lists);
            pos_order oz = db.pos_orders.FirstOrDefault(c => c.Id == o_id);
            reciept_head.Content = "#" + o_id + " " + oz.details;
            receipt_details.Content = oz.pos_table.name + " " + oz.date;
            var list = db.pos_order_lists.Where(c=>c.order_id == o_id&& c.status=="PENDING").GroupBy(c=>c.peroduk);
            long stotal = 0;
            Thickness n = new Thickness(0, 0, 0, 0);
            foreach ( var enlist in list)
            {
                String name = "";
                long harga = 0;
                int qty = 0;
                int p_id = 0;
                foreach(pos_order_list nested in enlist)
                {
                    name = nested.Produk.nama;
                    harga = nested.Produk.harga_jual;
                    qty += nested.quantity;
                    p_id = nested.peroduk;
                }

                
                Label ln = new Label();
                ln.Content = qty+" x "+name;
                ln.Padding = n;
                ln.Margin = n;

                Label ls = new Label();
                ls.Content = harga.ToString("C");
                ls.FontSize = 10;
                ls.Foreground = SystemColors.GrayTextBrush;
                ls.Margin = n;
                ls.Padding = n;

                Label lq = new Label();
                lq.Content = (qty*harga).ToString("C");
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
                lbi.Name = "__"+p_id+"__"+name.Replace(' ','_').Replace('&', '_');
                lbi.Content = line;
                lbi.Selected += delete_product;
                reciept_list.Items.Add(lbi);

                stotal += harga*qty;
                
                
            }
            subtotal = stotal;

            ///static portion
            n = new Thickness(0, 0, 3, 0);
            pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == o_id);
            long discount = (order.discount * stotal) / 100;
            long serv_total = order.service;
            long ppn_total = ((Properties.Settings.Default.tax_ppn)*stotal) / 100;
            Label dsc = new Label();
            dsc.Content = "("+(discount).ToString("C")+")";
            dsc.Margin = n;
            dsc.Padding = n;
            dsc.HorizontalAlignment = HorizontalAlignment.Right;

            Label dscText = new Label();
            dscText.Content = "Discount "+order.discount+"%";
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
            tot.Content = ((stotal-discount)+ppn_total).ToString("C");
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
            dislis.Selected += setdiscount;

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


            reciept_list.Items.Add(dev2);
            reciept_list.Items.Add(subt);
            reciept_list.Items.Add(dislis);
            reciept_list.Items.Add(ppnlis);
            reciept_list.Items.Add(servlis);
            reciept_list.Items.Add(dev1);
            reciept_list.Items.Add(totlist);
            long total_value = (stotal - discount) + serv_total + ppn_total;
            receipt_total.Content = total_value.ToString("C");
            if (total_value > 0)
            {
                total = total_value;
                checkout_button.IsEnabled = true;
            }else
            {
                checkout_button.IsEnabled = false;
            }
        }

        private void receipt_null()
        {
            reciept_list.Items.Clear();
            reciept_head.Content = "No Order Selected";
            receipt_details.Content = "";
            receipt_total.Content = "";
            checkout_button.IsEnabled = false; 
        }

        private void setdiscount(object sender, RoutedEventArgs e)
        {
            if(selected_order != 0)
            {
                dialog_set_discount dd = new dialog_set_discount();
                dd.ShowDialog();
                    pos_order ord = db.pos_orders.FirstOrDefault(c => c.Id == selected_order);
                    ord.discount = dd.qty;
                    db.SubmitChanges();
                    receipt_refresh(selected_order);
                
            }
        }

        private void delete_product(object sender, RoutedEventArgs e)
        {
            if(selected_order != 0)
            {
                ListBoxItem en = sender as ListBoxItem;

                int p_id = Int32.Parse(en.Name.Split('_')[2]);
                int count = db.pos_order_lists.Where(c => c.order_id == selected_order && c.peroduk == p_id && c.status == "PENDING").Sum(c=>c.quantity);
                dialog_delete_quantity dd = new dialog_delete_quantity();
                dd.count = count;
                dd.ShowDialog();
                if (dd.qty > 0)
                {
                    Dictionary<String, dynamic> data = new Dictionary<string, dynamic>();
                    data.Add("order_id", selected_order);
                    data.Add("produk", p_id);
                    data.Add("quantity", dd.qty);
                    pos.delete_product(data);
                    receipt_refresh(selected_order);
                }

            }
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (search.Text != "")
            {
                foreach(ListBoxItem s in product_list.Items)
                {
                    if (s.Name.ToLower().Replace('_',' ').Contains(search.Text.ToLower()))
                    {
                        s.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        s.Visibility = Visibility.Collapsed;
                    }
                }
            }else
            {
                foreach (ListBoxItem s in product_list.Items)
                {
                    s.Visibility = Visibility.Visible;
                }
            }
        }

        private void product_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem produk = product_list.SelectedItem as ListBoxItem;
            if (produk != null)
            {
                if (order_list.SelectedItem!= null)
                {
                    int id = Int32.Parse(produk.Name.Split('_')[2]);
                    dialog_product_quantity dd = new dialog_product_quantity();
                    dd.ShowDialog();
                    if (dd.qty > 0)
                    {
                        Dictionary<String, dynamic> data = new Dictionary<string, dynamic>();
                        ListBoxItem order = order_list.SelectedItem as ListBoxItem;
                        int o_id = Int32.Parse(order.Name.Substring(6));
                        data.Add("order_id", o_id);
                        data.Add("produk", id);
                        data.Add("quantity", dd.qty);
                        pos.add_product(data);
                        receipt_refresh(o_id);
                    }

                    product_list.SelectedItem = null;
                }
                
                

            }
        }

        private void void_Click(object sender, RoutedEventArgs e)
        {
            if (order_list.SelectedItem != null)
            {
                dialog_confirm dd = new dialog_confirm();
                dd.ShowDialog();
                bool check = dd.conrfirmed;
                if (check == true)
                {
                    pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == selected_order);
                    order.disc_amount = order.discount * subtotal;
                    order.total = total;
                    order.ppn = (Properties.Settings.Default.tax_ppn * subtotal) / 100;
                    order.service = service_value;
                    db.SubmitChanges();
                    pos.void_order(selected_order);
                    refresh_order();
                    if (order_list.Items.Count > 0)
                    {
                        order_list.SelectedItem = order_list.Items.GetItemAt(0);

                    }
                    else
                    {
                        selected_order = 0;
                        receipt_null();
                    }
                }
                else
                {

                }
            }
        }

        private void hold_Click(object sender, RoutedEventArgs e)
        {
            if (order_list.SelectedItem != null)
            {
                dialog_confirm dd = new dialog_confirm();
                dd.ShowDialog();
                bool check = dd.conrfirmed;
                if (check == true)
                {
                    pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == selected_order);
                    order.disc_amount = order.discount * subtotal;
                    order.total = total;
                    order.ppn = (Properties.Settings.Default.tax_ppn * subtotal) / 100;
                    order.service = service_value;
                    db.SubmitChanges();
                    pos.hold_order(selected_order);
                    refresh_order();
                    if (order_list.Items.Count > 0)
                    {
                        order_list.SelectedItem = order_list.Items.GetItemAt(0);

                    }
                    else
                    {
                        selected_order = 0;
                        receipt_null();
                    }
                }
                else
                {

                }
            }
        }

        private void checkout_button_Click(object sender, RoutedEventArgs e)
        {
            if (selected_order > 0)
            {
                dialog_checkout dd = new dialog_checkout();
                dd.receipt_refresh(selected_order);
                dd.ShowDialog();
                if (dd.choice == 1)
                {
                    pos_order order = db.pos_orders.FirstOrDefault(c=>c.Id==selected_order);
                    order.disc_amount = (order.discount * subtotal)/100;
                    order.total = total;
                    order.ppn = (Properties.Settings.Default.tax_ppn*subtotal)/100;
                    order.service = service_value;
                    order.pembayaran = dd.pembayaran;
                    db.SubmitChanges();
                    pos.checkout(selected_order);
                    refresh_order();
                   
                    receipt_null();
                }
                else
                {
                    if (dd.choice == 2)
                    {
                        pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == selected_order);
                        order.disc_amount = (order.discount * subtotal)/100;
                        order.total = total;
                        order.ppn = (Properties.Settings.Default.tax_ppn * subtotal) / 100;
                        order.service = service_value;
                        order.pembayaran = dd.pembayaran;
                        db.SubmitChanges();
                        pos.checkout(selected_order,dd.cardnumber,dd.transaction);
                        refresh_order();
                        receipt_null();
                    }
                }
            }
            
        }

        private void history_Click(object sender, RoutedEventArgs e)
        {
            dialog_order_list dd = new dialog_order_list();
            dd.ShowDialog();
        }

        private void switch_Click(object sender, RoutedEventArgs e)
        {
            dialog_switch_table dd = new dialog_switch_table();
            
            dd.order = selected_order;
            dd.ShowDialog();
            refresh_order();
            receipt_refresh(selected_order);
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            if (selected_order > 0 && ASME_C_WPF.Properties.Settings.Default.print_enable==true)
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
                add.Padding = new Thickness(0,0,20,0);
                add.Foreground = SystemColors.ActiveCaptionTextBrush;
                add.HorizontalAlignment = HorizontalAlignment.Center;
                add.Content = ASME_C_WPF.Properties.Settings.Default.Client_address_1.ToString(); ;

                Label telp = new Label();
                telp.FontSize = 12;
                telp.Padding = new Thickness(0, 0, 20, 0);
                telp.Foreground = SystemColors.ActiveCaptionTextBrush;
                telp.HorizontalAlignment = HorizontalAlignment.Center;
                telp.Content = ASME_C_WPF.Properties.Settings.Default.Client_address_2.ToString(); ;

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
                a2.Content = "#"+po.Id;

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
                aw.Margin = new Thickness(20,0,50,0);

                lv.Width = pd.PrintableAreaWidth;
                lv.Margin = new Thickness(15,0,35,0);
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
                    ln.Content =  name;
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
                subtotal = stotal;

                ///static portion
                n = new Thickness(10,0,25,0);
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

                Label subfooter = new Label();
                subfooter.Content = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")+" "+db.master_users.FirstOrDefault(c=>c.Id==ASME_C_WPF.Properties.Settings.Default.Active_user).name;
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
                lv.Children.Add(subfooter);
                lv.Children.Add(subfooter2);





                //lv.Height = pd.PrintableAreaHeight;


                pd.PrintVisual(lv, "Lets Print Something");
            }
            
        }
    }
}
