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
namespace ASME_C_WPF.ui
{
    /// <summary>
    /// Interaction logic for ui_pengeluaran.xaml
    /// </summary>
    /// 
    public partial class ui_pengeluaran : Page
    {
        int selected = 0;
        CoreDataContext db = new CoreDataContext();
        Core core = new Core();
        Regex rg = new Regex("^[0-9]+$");
        public ui_pengeluaran()
        {
            InitializeComponent();
            refresh_list();
            refresh_history();
        }

        private void refresh_list()
        {
            jenis.Items.Clear();
            Label lx = new Label();
            lx.Name = "b__1";
            lx.Content = "Pembayaran Utang";

            Label lz = new Label();
            lz.Name = "b__2";
            lz.Content = "Prive";

            Label la = new Label();
            la.Name = "b__3";
            la.Content = "Beban Pembayaran PPN";

            Label lb = new Label();
            lb.Name = "b__4";
            lb.Content = "Beban Pembayaran Pajak";

            Label lc = new Label();
            lc.Name = "b__5";
            lc.Content = "Beban Pemeliharaan Aset";

            jenis.Items.Add(lx);
            jenis.Items.Add(lz);
            jenis.Items.Add(la);
            jenis.Items.Add(lb);
            jenis.Items.Add(lc);

            var beban = db.Bebans;
            foreach(Beban x in beban)
            {
                Label lo = new Label();
                lo.Name = "b__6_"+x.Id;
                lo.Content = x.nama;

                jenis.Items.Add(la);

            }

            Label ld = new Label();
            ld.Name = "b__7";
            ld.Content = "Beban Lain-Lain";
            jenis.Items.Add(ld);
            jenis.SelectedItem = jenis.Items.GetItemAt(selected);
            total.Clear();
            detail.Clear();
        }
        private void refresh_history()
        {
            var history = db.Transactions.Where(c => c.type.StartsWith("5.1.")||(c.type.StartsWith("1.1.3.")&&c.jumlah>0)).OrderByDescending(c=>c.date).Take(15);
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
                lb.Content =String.Format(tr.date.ToString(),"DD/MM/YYYY");
                lb.Foreground = SystemColors.GrayTextBrush;
                lb.FontSize = 11;
                lb.Padding = new Thickness(0, 0, 0, 0);
                lb.Margin = new Thickness(5, 0, 0, 0);
                Label lc = new Label();
                lc.Content = tr.jumlah;
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
        private void detail_KeyUp(object sender, KeyEventArgs e)
        {
            if (detail.Text != "" && total.Text != "")
            {
                        bayar.IsEnabled = true;
               
            }
            else
            {
                bayar.IsEnabled = false;
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
                        bayar.IsEnabled = true;
                    }
                    else
                    {
                        bayar.IsEnabled = false;
                    }

                }
                else
                {
                    bayar.IsEnabled = false;
                }

            }
            else
            {
                bayar.IsEnabled = false;
            }
        }

        private void bayar_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, dynamic> data = new Dictionary<string, dynamic>();
            int sel_jenis = Int32.Parse((jenis.SelectedItem as Label).Name.Substring(3).ToString());
            if (sel_jenis == 1)
            {
                ///s1 utang
                data.Add("jumlah", Int64.Parse(total.Text));
                data.Add("details", detail.Text);
                core.Pembayaran_utang(data);
                refresh_list();
                refresh_history();
            }
            else
            {
                if (sel_jenis == 2)
                {
                    ///s2 prive
                    data.Add("jumlah", Int64.Parse(total.Text));
                    data.Add("details", detail.Text);
                    core.Prive(data);
                    refresh_list();
                    refresh_history();
                }
                else
                {
                    if(sel_jenis == 3)
                    {
                        ///s3 ppn 
                        data.Add("jumlah", Int64.Parse(total.Text));
                        data.Add("details", detail.Text);
                        core.Pembayaran_beban_ppn(data);
                        refresh_list();
                        refresh_history();
                    }
                    else
                    {
                        if (sel_jenis == 4)
                        {
                            ///s4 pajak
                            data.Add("jumlah", Int64.Parse(total.Text));
                            data.Add("details", detail.Text);
                            core.Pembayaran_beban_pajak(data);
                            refresh_list();
                            refresh_history();
                        }
                        else
                        {
                            if (sel_jenis == 5)
                            {
                                ////s5 aset
                                data.Add("jumlah", Int64.Parse(total.Text));
                                data.Add("details", detail.Text);
                                core.Pembayaran_beban_pemeliharaan(data);
                                refresh_list();
                                refresh_history();
                            }
                            else
                            {
                                if (sel_jenis == 6)
                                {
                                    ///s6
                                }
                                else
                                {
                                    if (sel_jenis == 7)
                                    {
                                        ///s7 lain
                                        data.Add("jumlah", Int64.Parse(total.Text));
                                        data.Add("details", detail.Text);
                                        core.Pembayaran_beban_lain(data);
                                        refresh_list();
                                        refresh_history();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
