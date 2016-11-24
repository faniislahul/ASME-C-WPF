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
namespace ASME_C_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        CoreDataContext db = new CoreDataContext();
        public Login()
        {
            InitializeComponent();
            security();
        }

        /// <summary>
        /// hardwire security 
        /// </summary>
        private void security()
        {
            DateTime dt = new DateTime(2017, 1, 1);
            if (DateTime.Now > dt)
            {
                uname.IsEnabled = false;
                pass.IsEnabled = false;
                button.IsEnabled = false;
            }
        }

        private void clicked(object sender, RoutedEventArgs e)
        {
            Boolean state = false;
            if((uname.Text!=""&&uname.Text!=null) && (pass.Password != "" && pass.Password != null))
            {
                if (db.master_users.FirstOrDefault(c => c.name.ToLower() == uname.Text.ToLower()) != null && db.master_users.FirstOrDefault(c => c.name.ToLower() == uname.Text.ToLower()).active == true)
                {
                    if(db.master_users.FirstOrDefault(c=> c.name.ToLower() == uname.Text.ToLower()).pass == Core.calculateMD5(pass.Password))
                    {
                        state = true;
                    }else
                    {
                        dialog_login dd = new dialog_login();
                        dd.setmessage("Password Salah");
                        dd.ShowDialog();
                    }
                }else
                {
                    dialog_login dd = new dialog_login();
                    dd.setmessage("User Tidak ditemukan");
                    dd.ShowDialog();
                }
            }

            if(state == true)
            {
                ASME_C_WPF.Properties.Settings.Default.Active_user = db.master_users.FirstOrDefault(c => c.name.ToLower() == uname.Text.ToLower()).Id;
                ASME_C_WPF.Properties.Settings.Default.Save();
                db.master_users.FirstOrDefault(c => c.name.ToLower() == uname.Text.ToLower()).last_login = DateTime.Now;
                db.SubmitChanges();
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            
        }

       
    }
}
