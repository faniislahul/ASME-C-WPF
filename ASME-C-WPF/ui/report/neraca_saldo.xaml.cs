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
namespace ASME_C_WPF.ui.report
{
    /// <summary>
    /// Interaction logic for neraca_saldo.xaml
    /// </summary>
    public partial class neraca_saldo : Window
    {
        public CoreDataContext db = new CoreDataContext();
        public String s { set; get; }
        public neraca_saldo()
        {
            
            InitializeComponent();
            datagrid.ItemsSource = db.Transactions;
            
            
        }

        //public String gettransaction()
        //{
         //   Transaction  s= db.Transactions.First();
           // return s.details;
        //}

    }
}
