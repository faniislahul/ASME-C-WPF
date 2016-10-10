using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ASME_C_WPF.core;
using System.Resources;
using System.Text.RegularExpressions;
namespace ASME_C_WPF.ui.dialog
{
    /// <summary>
    /// Interaction logic for dialog_produk_baru.xaml
    /// </summary>
    public partial class dialog_produk_baru : Window
    {
        public List<p_bb> pbb = new List<p_bb>();
        private CoreDataContext db = new CoreDataContext();
        private Regex rg = new Regex("^[0-9]+$");
        public Produk prd = new Produk();
        public bool done = false;
       
        public dialog_produk_baru()
        {
            InitializeComponent();
            nama.Focus();
            setbb();
            

            var caten = db.Categories;
            int count = 0;
            foreach(Category ct in caten)
            {
                Label ne = new Label();
                ne.Content = ct.nama;
                ne.Name = "cat__" + ct.Id;
                cat.Items.Add(ne);
                count += 1;
            }
            if (count > 0)
            {
                cat.SelectedItem = cat.Items.GetItemAt(0);
            }

            OK.IsEnabled = false;
        }

        private void setbb()
        {
            var list = db.Bahan_bakus.Where(c=>c.active==true);
            foreach(Bahan_baku bb in list)
            {
                StackPanel st = new StackPanel();
                st.Orientation = Orientation.Horizontal;
                st.Margin = new Thickness(5, 5, 0, 0);
                st.Name = "__" + bb.nama.Replace(' ', '_');
                StackPanel sta = new StackPanel();
                

                ToggleButton tg = new ToggleButton();
                tg.SetValue(FrameworkElement.NameProperty, ("__" + bb.Id.ToString()));
                tg.Checked += Tg_Checked;
                tg.Unchecked += Tg_Unchecked;
                this.RegisterName(("__" + bb.Id.ToString()),tg);

                Label nw = new Label();
                nw.Content = bb.nama;
  
                sta.Children.Add(nw);
                
                st.Children.Add(tg);
                st.Children.Add(sta);

                bb_list.Children.Add(st);

            }
            
        }

        private void Tg_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton tgb = sender as ToggleButton;
            p_bb link = new p_bb();
            link.bb = Int32.Parse(tgb.Name.Substring(2).ToString());
            pbb.Remove(pbb.FirstOrDefault(c => c.bb == link.bb));

            if (nama.Text != null && harga.Text != null && nama.Text != "" && harga.Text != "" && pbb.Count>0)
            {
                OK.IsEnabled = true;
            }
            else
            {
                OK.IsEnabled = false;
            }
        }

        private void Tg_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton tgb = sender as ToggleButton;
            p_bb link = new p_bb();
            link.bb = Int32.Parse(tgb.Name.Substring(2).ToString());
            pbb.Add(link);

            if (nama.Text != null && harga.Text!= null && nama.Text != "" && harga.Text != "")
            {
                OK.IsEnabled = true;
            }
            else
            {
                OK.IsEnabled = false;
            }


        }

        private void search_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != "")
            {
                foreach (StackPanel s in bb_list.Children)
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
                foreach (StackPanel s in bb_list.Children)
                {
                    s.Visibility = Visibility.Visible;
                }
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            prd.nama = nama.Text;
            prd.harga_jual = Int32.Parse(harga.Text);
            prd.SKU = "";
            if(active.IsChecked  == true)
            {
                prd.active = true;
            }else
            {
                prd.active = false;
            }
            prd.category = Int32.Parse((cat.SelectedItem as Label).Name.Substring(5).ToString());
            done = true;
            this.Close();
        }

        private void nama_KeyUp(object sender, KeyEventArgs e)
        {
            if (nama.Text != null && pbb.Count != 0 && nama.Text != "" && harga.Text!=""&& harga.Text != null)
            {
                OK.IsEnabled = true;
            }
            else
            {
                OK.IsEnabled = false;
            }
        }

        private void harga_KeyUp(object sender, KeyEventArgs e)
        {
            if (harga.Text != "")
            {
                if (rg.IsMatch(harga.Text))
                {
                    if (pbb.Count != 0 && nama.Text != "")
                    {
                        OK.IsEnabled = true;
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
    }
}
