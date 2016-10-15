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
    public partial class buku_besar : Window
    {
        public CoreDataContext db = new CoreDataContext();
        List<TransactionParser> parser = new List<TransactionParser>();
        int selected_month = 0;
        int selected_year = 2016;
        String selected_akun = "1.1.1";
        public buku_besar()
        {
            
            InitializeComponent();

            foreach(KeyValuePair<String,String> acc in Core.accountlist){
                Label type = new Label();
                type.Name = "_"+acc.Key.Replace('.','_');
                type.Content = acc.Value;

                akun_type.Items.Add(type);

            }
            akun_type.SelectedItem = akun_type.Items.GetItemAt(0);

            DateTime nt = DateTime.Now;
            for(int i = Properties.Settings.Default.Initial_month; i <= nt.Month; i++)
            {
                Label sel = new Label();
                DateTime nn = new DateTime(selected_year,i,1);
                sel.Name = "__" + nn.Month + "_" + nn.Year;
                sel.Content = nn.ToString("MMMM yyyy");
                month.Items.Add(sel);
                
            }

            month.SelectedItem = month.Items.GetItemAt(0);
            setdatagrid((akun_type.SelectedItem as Label).Name.Substring(1).Replace('_', '.'), Properties.Settings.Default.Initial_month, Properties.Settings.Default.Initial_year);
        }

        public void setdatagrid(String akun, int bulan, int tahun)
        {
            parser.Clear();
            
            var Translist = db.Transactions.Where(c => c.type.StartsWith(akun) && c.date.Month == bulan && c.date.Year == tahun);

            foreach (Transaction tr in Translist)
            {
                TransactionParser trp = new TransactionParser(tr.date, tr.details, tr.jumlah);
                parser.Add(trp);
            }

            datagrid.ItemsSource = parser;
            datagrid.Items.Refresh();
        }

        private void show_Click(object sender, RoutedEventArgs e)
        {
            selected_akun = (akun_type.SelectedItem as Label).Name.Substring(1).Replace('_', '.');
            selected_month = Int32.Parse((month.SelectedItem as Label).Name.Split('_')[2].ToString());
            selected_year = Int32.Parse((month.SelectedItem as Label).Name.Split('_')[3].ToString());
            setdatagrid(selected_akun, selected_month, selected_year);
        }
    }



    public class TransactionParser{
        public TransactionParser(DateTime dt,String dtl,long val)
        {
            Date = dt;
            Details = dtl;
            if (val < 0)
            {
                Kredit = val * -1;
                Debit = 0;
            }
            else
            {
                Debit = val;
                Kredit = 0;
            }
        }

        public DateTime Date { set; get; }
       

        public String Details { set; get; }

        public long Debit { set; get; }
        
        public long Kredit { set; get; }
        

    }
}
