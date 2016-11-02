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
using System.Windows.Shapes;
using ASME_C_WPF.core;
using System.Windows.Controls.Primitives;

namespace ASME_C_WPF.ui.dialog
{
    /// <summary>
    /// Interaction logic for dialog_new_unit.xaml
    /// </summary>
    public partial class dialog_new_unit : Window
    {
        CoreDataContext db = new CoreDataContext();
        public dialog_new_unit()
        {
            InitializeComponent();
            refresh_user();
        }

        public void refresh_user()
        {
            var units = db.Satuan_bbs;
            foreach (Satuan_bb unit in units)
            {
                StackPanel st = new StackPanel();
                st.Orientation = Orientation.Horizontal;
                st.Margin = new Thickness(5, 5, 0, 0);
                StackPanel sta = new StackPanel();

                ToggleButton tg = new ToggleButton();
                tg.SetValue(FrameworkElement.NameProperty, ("__" + unit.Id.ToString()));
                if (unit.active == true)
                {
                    tg.IsChecked = true;
                }
                else
                {
                    tg.IsChecked = false;
                }
                tg.Checked += Tg_Checked;
                tg.Unchecked += Tg_Unchecked;
                this.RegisterName(("__" + unit.Id.ToString()), tg);

                Label nw = new Label();
                nw.Content = unit.nama;

                sta.Children.Add(nw);

                st.Children.Add(tg);
                st.Children.Add(sta);

                unit_list.Children.Add(st);
            }
        }

        private void Tg_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton tgb = sender as ToggleButton;
            int id = Int32.Parse(tgb.Name.Substring(2).ToString());
            db.Satuan_bbs.FirstOrDefault(c => c.Id == id).active = false;
            db.SubmitChanges();
        }

        private void Tg_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton tgb = sender as ToggleButton;
            int id = Int32.Parse(tgb.Name.Substring(2).ToString());
            db.Satuan_bbs.FirstOrDefault(c => c.Id == id).active = true;
            db.SubmitChanges();
        }
        private void Pesan_Click(object sender, RoutedEventArgs e)
        {
            if(nama.Text!=null && nama.Text != "")
            {
                Satuan_bb sat = new Satuan_bb();
                sat.nama = nama.Text;
                sat.active = true;
                db.Satuan_bbs.InsertOnSubmit(sat);
                db.SubmitChanges();
                this.Close();
            }
            
        }
    }
}
