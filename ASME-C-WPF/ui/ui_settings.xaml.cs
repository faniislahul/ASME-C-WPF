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
namespace ASME_C_WPF.ui
{
    /// <summary>
    /// Interaction logic for ui_settings.xaml
    /// </summary>
    public partial class ui_settings : Page
    {
        CoreDataContext db = new CoreDataContext();
        int active_user = ASME_C_WPF.Properties.Settings.Default.Active_user;
        public ui_settings()
        {
            InitializeComponent();
            security_barrier();

            if (active_user == 1)
            {
                settings_system.Visibility = Visibility.Visible;
            }
        }

        private void security_barrier()
        {
            int role = db.master_users.FirstOrDefault(c => c.Id == active_user).role;
            if (role == 3)
            {
                
                settings_user.Visibility = Visibility.Collapsed;
                
            }
            else
            {
                if (role > 3)
                {
                    settings_general.Visibility = Visibility.Collapsed;
                    settings_user.Visibility = Visibility.Collapsed;
                    settings_multi_user.Visibility = Visibility.Collapsed;
                    settings_database.Visibility = Visibility.Collapsed;
                    settings_backup.Visibility = Visibility.Collapsed;
                    settings_payment.Visibility = Visibility.Collapsed;
                    settings_table.Visibility = Visibility.Collapsed;
                    settings_unit.Visibility = Visibility.Collapsed;

                }

            }
        }

        private void settings_user_Selected(object sender, RoutedEventArgs e)
        {
            settings.SelectedItem = null;
            dialog_add_user dd = new dialog_add_user();
            dd.ShowDialog();
            

        }

        private void settings_multi_user_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void settings_table_Selected(object sender, RoutedEventArgs e)
        {
            settings.SelectedItem = null;
            dialog.dialog_new_table dd = new dialog.dialog_new_table();
            dd.ShowDialog();
           
        }

        private void settings_unit_Selected(object sender, RoutedEventArgs e)
        {
            settings.SelectedItem = null;
            dialog_new_unit dd = new dialog_new_unit();
            dd.ShowDialog();
        }

        private void settings_database_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void settings_payment_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void settings_general_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void settings_backup_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void settings_system_Selected(object sender, RoutedEventArgs e)
        {
            dialog_setting_system dd = new dialog_setting_system();
            dd.ShowDialog();
        }
    }
}
