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
using Microsoft.Office.Interop;
namespace ASME_C_WPF
{
    /// <summary>
    /// Interaction logic for ui_dash.xaml
    /// </summary>
    public partial class ui_dash : Page
    {
        CoreDataContext db = new CoreDataContext();
        List<SalesAdapter> sales = new List<SalesAdapter>();
        List<SalesAdapter> sales2 = new List<SalesAdapter>();
        public ui_dash()
        {
            InitializeComponent();
            refresh_penjualan();
            refresh_history();
            long sum = 0;
            foreach(SalesAdapter ss in sales)
            {
                if (ss.status == "COMPLETED")
                {
                    sum += ss.total;
                }
            }
            revenue.Content = sum.ToString("C");
            date.SelectedDate = DateTime.Now;
        }

        private void refresh_history()
        {
            var history = db.Transactions.Where(c => (c.type.StartsWith("1.1.2") && c.jumlah < 0) || (c.type.StartsWith("2.1.1") && c.jumlah < 0) || (c.type.StartsWith("3.1.1") && c.jumlah < 0) || (c.type.StartsWith("4.1.1") && c.jumlah < 0) || (c.type.StartsWith("4.1.3") && c.jumlah < 0 )).OrderByDescending(c => c.date).Take(15);
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
                lc.Content = (tr.jumlah * -1).ToString("C");
                lc.Foreground = SystemColors.GrayTextBrush;
                lc.FontSize = 11;
                lc.Padding = new Thickness(0, 0, 0, 0);
                lc.Margin = new Thickness(5, 0, 0, 0);
                stc.Children.Add(la);
                stc.Children.Add(lb);
                stc.Children.Add(lc);


                container.Children.Add(stc);


            }
            transaction_list.Content = container;
        }
        private void refresh_penjualan()
        {
            var salesx = db.pos_order_lists.Where(c => (c.status == "COMPLETED" || c.status == "HOLD") && c.date.Day == DateTime.Now.Day);
            var pt = db.Produks.Where(c=>c.active==true);

            foreach(Produk prod in pt)
            {
                int count = 0;
                int count_hold = 0;
                foreach(pos_order_list sale in salesx)
                {
                    if(sale.peroduk == prod.Id && sale.status=="COMPLETED")
                    {
                        
                        count += sale.quantity;
                    }else
                    {
                        if (sale.peroduk == prod.Id && sale.status == "HOLD")
                        {
                            count_hold += sale.quantity;
                        }
                    }
                }
                SalesAdapter sally = new SalesAdapter(prod, count, "COMPLETED");
                sales.Add(sally);
                if (count_hold > 0)
                {
                    SalesAdapter sally_hold = new SalesAdapter(prod, count_hold, "HOLD");
                    sales.Add(sally_hold);
                }
                
            }

            StackPanel st = new StackPanel();
            foreach(SalesAdapter ss in sales.ToArray())
            {
                if (ss.count > 0)
                {
                    Label aa = new Label();
                    Label cc = new Label();
                    cc.HorizontalAlignment = HorizontalAlignment.Right;
                    if (ss.status == "HOLD")
                    {
                        aa.Content = ss.product.nama+" (HOLD)";
                        cc.Content = (ss.total*-1).ToString("C");
                    } else
                    {
                        aa.Content = ss.product.nama;
                        cc.Content = ss.total.ToString("C");
                    }

                    Label bb = new Label();
                    bb.Content = ss.count + " x ";

                    
                    
                    

                    StackPanel stp = new StackPanel();
                    stp.Orientation = Orientation.Horizontal;
                    stp.HorizontalAlignment = HorizontalAlignment.Left;
                    stp.Children.Add(bb);
                    stp.Children.Add(aa);

                    Grid gd = new Grid();
                    gd.HorizontalAlignment = HorizontalAlignment.Stretch;
                    gd.Children.Add(stp);
                    gd.Children.Add(cc);

                    st.Children.Add(gd);
                }
            }
               
            penjualan_list.Content = st;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            export.IsEnabled = true;
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            
            DateTime dt = (DateTime) date.SelectedDate;
            sales2.Clear();
            var salesx = db.pos_order_lists.Where(c => (c.status == "COMPLETED" || c.status=="HOLD") && c.date.Day == dt.Day);
            var pt = db.Produks.Where(c => c.active == true);

            foreach (Produk prod in pt)
            {
                int count = 0;
                int count_hold = 0;
                foreach (pos_order_list sale in salesx)
                {
                    if (sale.peroduk == prod.Id && sale.status == "COMPLETED")
                    {

                        count += sale.quantity;
                    }
                    else
                    {
                        if (sale.peroduk == prod.Id && sale.status == "HOLD")
                        {
                            count_hold += sale.quantity;
                        }
                    }
                }
                SalesAdapter sally = new SalesAdapter(prod, count, "COMPLETED");
                sales2.Add(sally);
                if (count_hold > 0)
                {
                    SalesAdapter sally_hold = new SalesAdapter(prod, count_hold, "HOLD");
                    sales2.Add(sally_hold);
                }

            }

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if(xlApp == null)
            {
                MessageBox.Show("Excel isn't available");
                return;
            }

            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Cells[1, 1] = "PENJUALAN "+dt.ToString("dd/MMM/yyyy");
            xlWorkSheet.Cells[3, 1] = "NAMA PRODUK";
            xlWorkSheet.Cells[3, 2] = "Quantity";
            xlWorkSheet.Cells[3, 3] = "Total";
            int sum1 = 0;
            long sum2 = 0;
            int row = 4;
            for(int i = 0; i < sales2.Count; i++)
            {
                if (sales2[i].count > 0)
                {
                    if (sales2[i].status == "HOLD")
                    {
                        xlWorkSheet.Cells[row, 1] = sales2[i].product.nama+"(HOLD)";
                    }else
                    {
                        xlWorkSheet.Cells[row, 1] = sales2[i].product.nama;
                    }
                    xlWorkSheet.Cells[row, 2] = sales2[i].count;
                    xlWorkSheet.Cells[row, 3] = sales2[i].total*-1;
                    row += 1;
                    sum1 += sales2[i].count;
                    sum2 += sales2[i].total;
                }

            }
            xlWorkSheet.Cells[row, 1] = "Total";
            xlWorkSheet.Cells[row, 2] = sum1;
            xlWorkSheet.Cells[row, 3] = sum2;

            xlWorkBook.SaveAs(("ASME_BACKUP_" + dt.ToString("ddMMMyyyy")),Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,misValue,misValue,misValue,misValue,Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,misValue,misValue,misValue,misValue,misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("Export Success");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }catch(Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
