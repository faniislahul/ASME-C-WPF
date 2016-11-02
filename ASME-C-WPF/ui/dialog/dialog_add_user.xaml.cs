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
using ASME_C_WPF.ui.dialog;
using System.Windows.Controls.Primitives;

namespace ASME_C_WPF.ui.dialog
{
    /// <summary>
    /// Interaction logic for dialog_add_user.xaml
    /// </summary>
    public partial class dialog_add_user : Window
    {
        CoreDataContext db = new CoreDataContext();
        private int selection = 0;
        public dialog_add_user()
        {
            InitializeComponent();
            refresh_user();
        }

        public void refresh_user()
        {
            var users = db.master_users.Where(c=>c.role>1);
            foreach (master_user user in users)
            {
                StackPanel st = new StackPanel();
                st.Orientation = Orientation.Horizontal;
                st.Margin = new Thickness(5, 5, 0, 0);
                StackPanel sta = new StackPanel();

                ToggleButton tg = new ToggleButton();
                tg.SetValue(FrameworkElement.NameProperty, ("__" + user.Id.ToString()));
                if (user.active == true)
                {
                    tg.IsChecked = true;
                }else
                {
                    tg.IsChecked = false;
                }
                tg.Checked += Tg_Checked;
                tg.Unchecked += Tg_Unchecked;
                this.RegisterName(("__" + user.Id.ToString()), tg);

                Label nw = new Label();
                if (user.role == 2)
                {
                    nw.Content = user.name+" (Super User)";
                }
                else
                {
                    if(user.role == 3)
                    {
                        nw.Content = user.name + " (Administrator)";
                    }else
                    {
                        if (user.role == 4)
                        {
                            nw.Content = user.name + " (Operator)";
                        }else
                        {
                            if (user.role == 5)
                            {
                                nw.Content = user.name + " (Cashier)";
                            }else
                            {
                                if (user.role == 1)
                                {
                                    nw.Content = user.name + " (SYSTEM)";
                                }
                            }
                        }
                    }
                }

                sta.Children.Add(nw);

                st.Children.Add(tg);
                st.Children.Add(sta);

                user_list.Children.Add(st);
            }
        }

        private void Tg_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton tgb = sender as ToggleButton;
            int id = Int32.Parse(tgb.Name.Substring(2).ToString());
            db.master_users.FirstOrDefault(c => c.Id == id).active = false;
            db.SubmitChanges();
        }

        private void Tg_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton tgb = sender as ToggleButton;
            int id = Int32.Parse(tgb.Name.Substring(2).ToString());
            db.master_users.FirstOrDefault(c => c.Id == id).active = true;
            db.SubmitChanges();
        }

        private void Pesan_Click(object sender, RoutedEventArgs e)
        {
            if (selection > 0 && (nama.Text != null && nama.Text!="") && (Pass.Password!=null && Pass.Password!=""))
            {
                master_user mt = new master_user();
                mt.name = nama.Text;
                mt.pass = Core.calculateMD5(Pass.Password);
                mt.role = selection;
                mt.active = true;

                db.master_users.InsertOnSubmit(mt);
                db.SubmitChanges();

                this.Close();
            }

            
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


       

        private void comboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                selection = 2;
            }else
            {
                if(comboBox.SelectedIndex == 1)
                {
                    selection = 3;
                }else
                {
                    if(comboBox.SelectedIndex == 2)
                    {
                        selection = 4;
                    }else
                    {
                        selection = 5;
                    }
                }
            }
        }
    }
}
