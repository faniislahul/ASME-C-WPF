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
    /// Interaction logic for dialog_new_table.xaml
    /// </summary>
    public partial class dialog_new_table : Window
    {
        CoreDataContext db = new CoreDataContext();
        public dialog_new_table()
        {
            InitializeComponent();
            refresh_table();
        }

        public void refresh_table()
        {
            try
            {
                db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.pos_tables);
                var list = db.pos_tables;
                foreach (pos_table table in list)
                {

                    Thickness n = new Thickness(0, 0, 0, 0);
                    String nama = table.name;
                    int id = table.Id;

                    if (table.is_empty == true || table.is_empty == false)
                    {
                        Label ln = new Label();

                        ln.Content = nama;
                        ln.Padding = n;
                        ln.Margin = n;

                        Label ls = new Label();

                        int reservation = db.pos_reservations.Where(c => c.table_id == id).Count();
                        if (reservation > 0)
                        {
                            pos_reservation rv = db.pos_reservations.FirstOrDefault(c => c.table_id == id);
                            ls.Content = "Status : Reserved by" + rv.details + " at " + rv.date;
                        }
                        else
                        {
                            ls.Content = "Status : Available";
                        }


                        ls.FontSize = 10;
                        ls.Foreground = SystemColors.GrayTextBrush;
                        ls.Margin = n;
                        ls.Padding = n;


                        StackPanel st = new StackPanel();
                        st.Children.Add(ln);
                        st.Children.Add(ls);

                        ListBoxItem lbi = new ListBoxItem();
                        lbi.Name = "table_" + id.ToString();
                        lbi.Content = st;


                        table_list.Items.Add(lbi);
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Pesan_Click(object sender, RoutedEventArgs e)
        {
            if (nama.Text != null && nama.Text != "")
            {
                pos_table pt = new pos_table();
                pt.name = nama.Text;
                pt.is_empty = true;
                db.pos_tables.InsertOnSubmit(pt);
                db.SubmitChanges();
                this.Close();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
