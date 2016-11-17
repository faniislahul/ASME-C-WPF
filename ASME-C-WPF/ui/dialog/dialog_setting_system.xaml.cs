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

namespace ASME_C_WPF.ui.dialog
{
    /// <summary>
    /// Interaction logic for dialog_setting_system.xaml
    /// </summary>
    public partial class dialog_setting_system : Window
    {
        public dialog_setting_system()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (client_name.Text != null && client_name.Text != "")
            {
                ASME_C_WPF.Properties.Settings.Default.Client_Name = client_name.Text;
                this.Close();
            }
        }
    }
}
