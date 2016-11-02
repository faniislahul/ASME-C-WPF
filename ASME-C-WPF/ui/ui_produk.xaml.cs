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
using ASME_C_WPF.ui.dialog;
using ASME_C_WPF.core;
using System.Windows.Controls.Primitives;
namespace ASME_C_WPF.ui
{
    /// <summary>
    /// Interaction logic for ui_produk.xaml
    /// </summary>
    public partial class ui_produk : Page
    {
        CoreDataContext db = new CoreDataContext();
        Core core = new Core();
        int selected_product = 0;
        
        public ui_produk()
        {
            InitializeComponent();
            refresh_product();
            activate.IsEnabled = false;
            edit.IsEnabled = false;
            
        }

        
        private void refresh_product()
        {
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Produks);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Categories);
            active_list.Items.Clear();
            Thickness tn = new Thickness(0, 0, 0, 0);
            var catlist = db.Categories;
            ItemsPanelTemplate n = new ItemsPanelTemplate();
            var temp = new FrameworkElementFactory(typeof(UniformGrid));
            temp.SetValue(UniformGrid.ColumnsProperty, 2);
            temp.SetValue(UniformGrid.VerticalAlignmentProperty, VerticalAlignment.Top);
            n.VisualTree = temp;
            active_list.ItemsPanel = n;
            foreach (Category cat in catlist)
            {
                Label la = new Label();
                la.Content = cat.nama.ToUpper();
                la.Foreground = SystemColors.GrayTextBrush;
                la.FontSize = 11;
                la.Padding = tn;
                la.Margin = new Thickness(5,0,0,0);

                ListBoxItem lbcat = new ListBoxItem();
                lbcat.Name = "__000__"+cat.nama.Replace(' ','_');
                lbcat.Content = la;
                lbcat.IsEnabled = false;

                ListBoxItem empty1 = new ListBoxItem();
                empty1.Name = "__0000__" + cat.nama.Replace(' ', '_');
                empty1.Content = "";
                empty1.IsEnabled = false;
                ListBoxItem empty2 = new ListBoxItem();
                empty2.Name = "__00000__" + cat.nama.Replace(' ', '_');
                empty2.Content = "";
                empty2.IsEnabled = false;

                
               
                
                    active_list.Items.Add(lbcat);
                    active_list.Items.Add(empty1);
                
                var pt = db.Produks.Where(c => c.category == cat.Id && c.active ==true);
                foreach(Produk product in pt)
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

                    Label lx = new Label();
                    if (product.active == true)
                    {
                        lx.Content = "AKTIF";

                    }else
                    {
                        lx.Content = "NONAKTIF";
                    }
                    lx.FontSize = 10;
                    lx.Foreground = SystemColors.GrayTextBrush;
                    lx.Margin = tn;
                    lx.Padding = tn;

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
                    st.Children.Add(lx);

                    stp.Children.Add(circle);
                    stp.Children.Add(st);

                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Name = "__" + id + "__" + nama.Replace(' ', '_')+"__"+cat.nama.Replace(' ','_');
                    lbi.Content = stp;
                    lbi.Selected += Lbi_Selected;


                    active_list.Items.Add(lbi);
                }

                var ptnon = db.Produks.Where(c => c.category == cat.Id && c.active == false);
                foreach (Produk product in ptnon)
                {
                    String nama = product.nama;
                    int id = product.Id;
                    long harga = product.harga_jual;

                    Label ln = new Label();
                    ln.Content = nama;
                    ln.Padding = tn;
                    ln.Margin = tn;
                    ln.Foreground = SystemColors.GrayTextBrush;

                    Label ls = new Label();
                    ls.Content = harga.ToString("C");
                    ls.FontSize = 10;
                    ls.Foreground = SystemColors.GrayTextBrush;
                    ls.Margin = tn;
                    ls.Padding = tn;

                    Label lx = new Label();
                    if (product.active == true)
                    {
                        lx.Content = "AKTIF";

                    }
                    else
                    {
                        lx.Content = "NONAKTIF";
                    }
                    lx.FontSize = 10;
                    lx.Foreground = SystemColors.GrayTextBrush;
                    lx.Margin = tn;
                    lx.Padding = tn;

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
                    st.Children.Add(lx);

                    stp.Children.Add(circle);
                    stp.Children.Add(st);

                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Name = "__" + id + "__" + nama.Replace(' ', '_') + "__" + cat.nama.Replace(' ', '_');
                    lbi.Content = stp;
                    lbi.Selected += Lbi_Selected;


                    active_list.Items.Add(lbi);
                }
                if (db.Produks.Where(c => c.category == cat.Id).Count() % 2 == 1)
                {
                    active_list.Items.Add(empty2);
                }else
                {

                }

                    


            }
            


        }

        private void Lbi_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem list = sender as ListBoxItem;
            selected_product = Int32.Parse(list.Name.Split('_')[2].ToString());
            activate.IsEnabled = true;
            det_refresh();
        }

        private void det_refresh()
        {
            if (db.master_users.FirstOrDefault(c => c.Id == Properties.Settings.Default.Active_user).role < 3)
            {
                edit.IsEnabled = true;
            }
            else
            {
                edit.IsEnabled = false;
            }

            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Produks);
            Produk pt = db.Produks.FirstOrDefault(c => c.Id == selected_product);
            product_title.Content = pt.nama;
            det_cat.Content =pt.Category1.nama;
            det_price.Content =pt.harga_jual.ToString("C");
            det_sku.Content =pt.SKU;
            if (pt.active == false)
            {
                activate.IsChecked = false;
            }
            else
            {
                activate.IsChecked = true;
            }
            bb_list.Children.Clear();
            var pbb = db.p_bbs.Where(c => c.produk == selected_product);
            foreach (p_bb bb in pbb)
            {
                Label bee = new Label();
               // bee.Foreground = SystemColors.GrayTextBrush;
                bee.Content = bb.Bahan_baku.nama;
                

                bb_list.Children.Add(bee);
            }
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (search.Text != "")
            {
                foreach (ListBoxItem s in active_list.Items)
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
                foreach (ListBoxItem s in active_list.Items)
                {
                    s.Visibility = Visibility.Visible;
                }
            }
        }

        private void create_category_Click(object sender, RoutedEventArgs e)
        {

        }

        private void product_select(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void create_product_Click(object sender, RoutedEventArgs e)
        {
            dialog_produk_baru dd = new dialog_produk_baru();
            dd.ShowDialog();
            if(dd.done == true)
            {
                db.Produks.InsertOnSubmit(dd.prd);
                db.SubmitChanges();
                Produk pt = db.Produks.FirstOrDefault(c=>c.Id == dd.prd.Id);
                if(pt !=null)
                {
                    string s = pt.nama + pt.Id;
                    pt.SKU = Core.calculateMD5(s);
                    db.SubmitChanges();
                }
                foreach(p_bb link in dd.pbb)
                {
                    link.produk = pt.Id;
                    db.p_bbs.InsertOnSubmit(link);
                    db.SubmitChanges();
                }
                refresh_product();
                
            }
        }

        private void toggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (selected_product > 0)
            {
                Produk pt = db.Produks.FirstOrDefault(c => c.Id == selected_product);
                pt.active = true;
                db.SubmitChanges();
                refresh_product();
                det_refresh();
            }
            
        }

        private void toggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (selected_product > 0)
            {
                Produk pt = db.Produks.FirstOrDefault(c => c.Id == selected_product);
                pt.active = false;
                db.SubmitChanges();
                refresh_product();
                det_refresh();
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

        private void edit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dialog_produk_baru dd = new dialog_produk_baru();
            Produk pt = db.Produks.FirstOrDefault(c => c.Id == selected_product);
            dd.header.Content = pt.nama;
            dd.nama.Text = pt.nama;
            dd.harga.Text = pt.harga_jual.ToString();
            dd.cat.SelectedItem = dd.cat.Items.GetItemAt(pt.category-1);
            if (pt.active == true)
            {
                dd.active.IsChecked = true;
            }else
            {
                dd.active.IsChecked = false;
            }
            var list = db.p_bbs.Where(c=>c.produk == pt.Id);
            foreach (p_bb bb in list)
            {
                if (dd.FindName("__" + bb.bb) != null)
                {
                    ToggleButton tg = dd.FindName("__" + bb.bb) as ToggleButton;
                    tg.IsChecked = true;

                    dd.pbb.FirstOrDefault(c => c.bb == bb.bb).produk = bb.produk;
                }
            }

            dd.ShowDialog();
            if (dd.done == true)
            {
                pt.nama = dd.prd.nama;
                pt.harga_jual = dd.prd.harga_jual;
                pt.Category1 = db.Categories.Single(c=> c.Id==dd.prd.category);
                foreach (p_bb bb in list)
                {
                    int c = 0;
                    foreach(p_bb nn in dd.pbb)
                    {
                        if(nn.bb == bb.bb)
                        {
                            c += 1;
                        }
                    }
                    if (c == 0)
                    {
                        db.p_bbs.DeleteOnSubmit(bb);
                    }
                    db.SubmitChanges();                    
                }
                foreach(p_bb xx in dd.pbb)
                {
                    if(db.p_bbs.FirstOrDefault(c=>c.bb == xx.bb&&c.produk == xx.produk) == null)
                    {
                        xx.produk = pt.Id;
                        db.p_bbs.InsertOnSubmit(xx);
                        db.SubmitChanges();
                    }
                }
                db.SubmitChanges();

                refresh_product();
                det_refresh();

            }

        
        }
    }
}
