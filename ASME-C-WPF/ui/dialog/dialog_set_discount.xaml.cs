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
using System.Text.RegularExpressions;
namespace ASME_C_WPF.ui.dialog
{
    /// <summary>
    /// Interaction logic for dialog_set_discount.xaml
    /// </summary>
    public partial class dialog_set_discount : Window
    {
        public int qty = 0;
        Regex rg = new Regex("^[0-9]+$");
        public dialog_set_discount()
        {
            InitializeComponent();
            discount.Focus();
            discount.SelectAll();
            Pesan.IsEnabled = true;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pesan_Click(object sender, RoutedEventArgs e)
        {
            qty = Int32.Parse(discount.Text);
            this.Close();
        }

        private void discount_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void discount_KeyUp(object sender, KeyEventArgs e)
        {
            if (discount.Text != "")
            {
                if (rg.IsMatch(discount.Text))
                {
                    Pesan.IsEnabled = true;
                }
                else
                {
                    Pesan.IsEnabled = false;
                }

            }
            else
            {
                Pesan.IsEnabled = false;
            }
        }
    }
}
