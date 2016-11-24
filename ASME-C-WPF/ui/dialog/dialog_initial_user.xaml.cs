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

namespace ASME_C_WPF.ui.dialog
{
    /// <summary>
    /// Interaction logic for dialog_initial_user.xaml
    /// </summary>
    public partial class dialog_initial_user : Window
    {
        CoreDataContext db = new CoreDataContext();
        public dialog_initial_user()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if ((uname.Text != null && uname.Text != "") && (pass.Password != null && pass.Password != ""))
            {
                master_user user = new master_user();
                user.name = uname.Text;
                user.pass = Core.calculateMD5(pass.Password);
                user.role = 2;
                user.active = true;

                db.master_users.InsertOnSubmit(user);
                db.SubmitChanges();
                this.Close();
            }
        }
    }
}
