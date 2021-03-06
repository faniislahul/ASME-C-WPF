﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    /// Interaction logic for dialog_penggunaan_bb.xaml
    /// </summary>
    public partial class dialog_penggunaan_bb : Window
    {
        public int qty = 0;
        public int count = 0;
        public bool done = false;
        Regex rg = new Regex("^[0-9]+$");
        public dialog_penggunaan_bb()
        {
            InitializeComponent();
            quantity.Focus();
            quantity.SelectAll();
            Pesan.IsEnabled = true;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pesan_Click(object sender, RoutedEventArgs e)
        {
            qty = Int32.Parse(quantity.Text);
            done = true;
            this.Close();
        }


        private void quantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (quantity.Text != "")
            {
                if (rg.IsMatch(quantity.Text))
                {
                    if (Int32.Parse(quantity.Text) <= count)
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
            else
            {
                Pesan.IsEnabled = false;
            }
        }
    }
}
