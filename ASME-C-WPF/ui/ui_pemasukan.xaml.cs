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
using ASME_C_WPF.core;
using ASME_C_WPF.ui.dialog;
using System.Text.RegularExpressions;
using LiveCharts;
namespace ASME_C_WPF.ui
{
    /// <summary>
    /// Interaction logic for ui_pemasukan.xaml
    /// </summary>
    public partial class ui_pemasukan : Page
    {
        CoreDataContext db = new CoreDataContext();
        Core core = new Core();
        Regex rg = new Regex("^[0-9]+$");
        public ChartValues<long> v1 { set; get; }
        public ui_pemasukan()
        {
            InitializeComponent();
            refresh_list();
            refresh_history();
            set_penjualan_chart();
            z.Values = v1;
            
            
        }
        private void refresh_list()
        {
            jenis.Items.Clear();
            Label lx = new Label();
            lx.Name = "p__1";
            lx.Content = "Penerimaan Piutang";

            Label lz = new Label();
            lz.Name = "p__2";
            lz.Content = "Penerimaan Utang";

            Label la = new Label();
            la.Name = "p__3";
            la.Content = "Setoran Modal";

            Label lb = new Label();
            lb.Name = "p__4";
            lb.Content = "Pemasukan Lain";

            jenis.Items.Add(lx);
            jenis.Items.Add(lz);
            jenis.Items.Add(la);
            jenis.Items.Add(lb);

            
         
            jenis.SelectedItem = jenis.Items.GetItemAt(0);
            total.Clear();
            detail.Clear();
        }

        private void refresh_history()
        {
            var history = db.Transactions.Where(c => (c.type.StartsWith("1.1.2")&&c.jumlah<0) || (c.type.StartsWith("2.1.1") && c.jumlah < 0)||(c.type.StartsWith("3.1.1") && c.jumlah < 0) || (c.type.StartsWith("4.1.1") && c.jumlah < 0)|| (c.type.StartsWith("4.1.3") && c.jumlah < 0)).OrderByDescending(c => c.date).Take(15);
            StackPanel container = new StackPanel();
            foreach (Transaction tr in history)
            {
                StackPanel stc = new StackPanel();
                stc.Margin = new Thickness(5, 0, 0, 0);
                Label la = new Label();
                la.Content = tr.details;
                la.FontSize = 12;
                la.Padding = new Thickness(0, 0, 0, 0);
                la.Margin = new Thickness(0, 0, 0, 0);

                Label lb = new Label();
                lb.Content = String.Format(tr.date.ToString(), "DD/MM/YYYY");
                lb.Foreground = SystemColors.GrayTextBrush;
                lb.FontSize = 11;
                lb.Padding = new Thickness(0, 0, 0, 0);
                lb.Margin = new Thickness(5, 0, 0, 0);
                Label lc = new Label();
                lc.Content = (tr.jumlah*-1).ToString("C");
                lc.Foreground = SystemColors.GrayTextBrush;
                lc.FontSize = 11;
                lc.Padding = new Thickness(0, 0, 0, 0);
                lc.Margin = new Thickness(5, 0, 0, 0);
                stc.Children.Add(la);
                stc.Children.Add(lb);
                stc.Children.Add(lc);


                container.Children.Add(stc);


            }
            history_channel.Content = container;
        }

        private void set_penjualan_chart()
        {
            DateTime dt = DateTime.Now;
            v1 = new ChartValues<long>();
            v1.Add(0);
            for (int i = 1;i<= dt.Day;i++)
            {
                if(db.Transactions.Where(c => c.type.StartsWith("4.1.1.") && c.date.Day == i &&c.date.Month == dt.Month) != null)
                {
                    var soe = db.Transactions.Where(c => c.type.StartsWith("4.1.1.") && c.date.Day == i && c.date.Month == dt.Month);
                    long sum = 0;
                    foreach(Transaction tr in soe)
                    {
                        sum += (tr.jumlah*-1);
                    }
                    v1.Add(sum);
                }
                else
                {
                    long sum = 0;
                    v1.Add(sum);
                }
                
            }
           


        }

        private void terima_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, dynamic> data = new Dictionary<string, dynamic>();
            int selection = Int32.Parse((jenis.SelectedItem as Label).Name.Substring(3).ToString());
            if (selection == 1)
            {
                data.Add("jumlah", Int64.Parse(total.Text));
                data.Add("details", detail.Text);
                core.Penerimaan_piutang(data);
                refresh_list();
                refresh_history();
                terima.IsEnabled = false;
            }
            else
            {
                if (selection == 2)
                {
                    data.Add("jumlah", Int64.Parse(total.Text));
                    data.Add("details", detail.Text);
                    core.Penerimaan_utang(data);
                    refresh_list();
                    refresh_history();
                    terima.IsEnabled = false;
                }
                else
                {
                    if(selection == 3)
                    {
                        data.Add("jumlah", Int64.Parse(total.Text));
                        data.Add("details", detail.Text);
                        core.Penerimaan_modal(data);
                        refresh_list();
                        refresh_history();
                        terima.IsEnabled = false;
                    }
                    else
                    {
                        if (selection == 4)
                        {
                            data.Add("jumlah", Int64.Parse(total.Text));
                            data.Add("details", detail.Text);
                            core.Penerimaan_lain(data);
                            refresh_list();
                            refresh_history();
                            terima.IsEnabled = false;
                        }
                    }
                }
            }
        }

        private void detail_KeyUp(object sender, KeyEventArgs e)
        {
            if (detail.Text != "" && total.Text != "")
            {
                terima.IsEnabled = true;

            }
            else
            {
                terima.IsEnabled = false;
            }
        }

        private void total_KeyUp(object sender, KeyEventArgs e)
        {
            if (detail.Text != "" && total.Text != "")
            {
                if (rg.IsMatch(total.Text))
                {
                    if (Int32.Parse(total.Text) < 999999999 && Int32.Parse(total.Text) < 999999999)
                    {
                        terima.IsEnabled = true;
                    }
                    else
                    {
                        terima.IsEnabled = false;
                    }

                }
                else
                {
                    terima.IsEnabled = false;
                }

            }
            else
            {
                terima.IsEnabled = false;
            }
        }
    }

    public class rt_penjualan
    {
        public long value { set; get; }
        public int sales { set; get; }
        public rt_penjualan(long v)
        {
            value = v;
        }
        public rt_penjualan(int s)
        {
            sales = s;
        }
    }

}
