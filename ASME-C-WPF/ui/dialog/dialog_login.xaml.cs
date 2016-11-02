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
    /// Interaction logic for dialog_login.xaml
    /// </summary>
    public partial class dialog_login : Window
    {
        
        public dialog_login()
        {
            InitializeComponent();            
        }

        public void setmessage(String s)
        {
            warn.Content = s;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
