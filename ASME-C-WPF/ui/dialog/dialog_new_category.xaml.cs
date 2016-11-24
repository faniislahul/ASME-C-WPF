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
using MaterialIcons;

namespace ASME_C_WPF.ui.dialog
{
    /// <summary>
    /// Interaction logic for dialog_new_category.xaml
    /// </summary>
    public partial class dialog_new_category : Window
    {
        CoreDataContext db = new CoreDataContext();
        public dialog_new_category()
        {
            InitializeComponent();
            refresh_cat();
            if (icon_list.Items.Count > 0)
            {
                icon_list.SelectedIndex = 0;
            }
        }

        private void refresh_cat()
        {
            StackPanel listv = new StackPanel();
            var catl = db.Categories;
            foreach(Category s in catl)
            {
                Label name = new Label();
                name.Content = s.nama.ToString();

                MaterialIcons.MaterialIcon circle = new MaterialIcons.MaterialIcon();
                circle.Icon = Core.iconlist.ElementAt(s.pic);
                circle.Height = 25;
                circle.Width = 25;
                circle.Margin = new Thickness(5, 0, 5, 0);
                circle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF404040"));

                StackPanel stp = new StackPanel();
                stp.Orientation = Orientation.Horizontal;

                stp.Children.Add(circle);
                stp.Children.Add(name);

                listv.Children.Add(stp);
            }

            cat_list.Content = listv;

            foreach(MaterialIcons.MaterialIconType cz in Core.iconlist)
            {
                ListBoxItem lbt = new ListBoxItem();
                MaterialIcons.MaterialIcon circle = new MaterialIcons.MaterialIcon();
                circle.Icon = cz;
                circle.Height = 50;
                circle.Width = 50;
                circle.Margin = new Thickness(5, 0, 5, 0);
                circle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF404040"));

                lbt.Content = circle;
                icon_list.Items.Add(lbt);
            }

            
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (cat_name.Text != "" & cat_name != null)
            {
                Category cat = new Category();
                cat.nama = cat_name.Text;
                cat.pic = icon_list.SelectedIndex;
                db.Categories.InsertOnSubmit(cat);
                db.SubmitChanges();
                this.Close();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
