﻿using System;
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
using System.Timers;

namespace ASME_C_WPF
{
    /// <summary>
    /// Interaction logic for splash.xaml
    /// </summary>
    public partial class splash : Window
    {
        public splash()
        {
            InitializeComponent();
            nteu();
            Login log = new Login();
            log.Show();
            this.Close();
            
        }

        public void nteu()
        {
            this.Show();
            System.Threading.Thread.Sleep(2500);
        }
    }
}
