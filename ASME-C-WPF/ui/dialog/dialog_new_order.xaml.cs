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
    /// Interaction logic for dialog_new_order.xaml
    /// </summary>
    /// 


    public partial class dialog_new_order : Window
    {
        CoreDataContext db = new CoreDataContext();
        public int user { set; get; }

        public dialog_new_order()
        {
            InitializeComponent();
            try
            {
                var list = db.pos_tables;
                foreach (pos_table table in list)
                {

                    Thickness n = new Thickness(0, 0, 0, 0);
                    String nama = table.name;
                    int id = table.Id;

                    if (table.is_empty == true)
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
                        lbi.Name = "table_"+id.ToString();
                        lbi.Content = st;


                        table_list.Items.Add(lbi);
                    }

                }
            }catch(Exception e)
            {
                throw e;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            ui_cashier.return_state();

        }

        private void baru_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //pos_order order = new pos_order();
                Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
                data.Add("details", pemesan.Text);
                ListBoxItem table = table_list.SelectedItem as ListBoxItem;
                String extract = table.Name.Substring(6);
                //pemesan.Text = extract;
                data.Add("table_id", Int32.Parse(extract));
                

                Pos_module pos = new Pos_module();
                Pos_module.user = Properties.Settings.Default.Active_user;
                pos.create_order(data);

            }
            catch (Exception ex )
            {
                throw ex;
            }
            
            this.Close();
            
        }

        private void batal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void pemesan_KeyUp(object sender, KeyEventArgs e)
        {
            if (pemesan.Text != null && table_list.SelectedItem != null && pemesan.Text!="")
            {
                baru.IsEnabled = true;
            }
            else
            {
                baru.IsEnabled = false;
            }
        }

        private void table_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pemesan.Text != null && table_list.SelectedItem != null && pemesan.Text != "")
            {
                baru.IsEnabled = true;
            }
            else
            {
                baru.IsEnabled = false;
            }
        }
    }
}
