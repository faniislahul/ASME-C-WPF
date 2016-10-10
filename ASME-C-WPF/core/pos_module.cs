using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASME_C_WPF.core
{
    class Pos_module
    {
        CoreDataContext db = new CoreDataContext();
        public static int user { set; get; }

        /* reserve_table
         * String details
         * int table_id
         * Datetime date
         */
        public int reserve_table(Dictionary<String, dynamic> data)
        {
            int result = 0;
            String details;
            int table_id;
            DateTime date;
            dynamic value;
            if(data.TryGetValue("details", out value));
            {
                details = value;
            }

            if (data.TryGetValue("table_id", out value)) ;
            {
                table_id = value;
            }

            if (data.TryGetValue("date", out value)) ;
            {
                date = value;
            }
            try
            {
                pos_reservation reserve = new pos_reservation();
                reserve.table_id = table_id;
                reserve.details = details;
                reserve.date = date;
                db.pos_reservations.InsertOnSubmit(reserve);

                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS CREATE RESERVATION";
                db.pos_logs.InsertOnSubmit(log);

                db.SubmitChanges();
                result = 4;
            }catch(Exception e)
            {
                throw e;
            }
            return result;
        }

        /* cancel_reserve
         * int id
         * 
         */
        public int cancel_reservation(int id)
        {
            int result = 0;
            
            try
            {
                db.pos_reservations.DeleteOnSubmit(db.pos_reservations.FirstOrDefault(c=>c.Id == id));
                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS DELETE RESERVATION";
                db.pos_logs.InsertOnSubmit(log);

                result = 4;

            }catch(Exception e)
            {
                throw e;
            }

            return result;
        }

        /* create_order
         * String details
         * int table_id
         * Datetime date
         */
        public int create_order(Dictionary<String, dynamic> data)
        {
            int result = 0;
            String details;
            int table_id;
            
            dynamic value;
            if (data.TryGetValue("details", out value)) ;
            {
                details = value;
            }

            if (data.TryGetValue("table_id", out value)) ;
            {
                table_id = value;
            }

            try
            {
                pos_order reserve = new pos_order();
                reserve.table_id = table_id;
                reserve.details = details;
                reserve.status = "PENDING";
                reserve.user = user;              

                pos_table table = db.pos_tables.FirstOrDefault(c=>c.Id == table_id);
                table.is_empty = false;
                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS CREATE ORDER";
                db.pos_logs.InsertOnSubmit(log);

                db.pos_orders.InsertOnSubmit(reserve);
                db.SubmitChanges();
                result = 4;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /* add_product
         * String details
         * int table_id
         * Datetime date
         */
        public int add_product(Dictionary<String, dynamic> data)
        {
            int result = 0;
            int quantity;
            int order_id;
            int produk;
            dynamic value;
            if (data.TryGetValue("quantity", out value)) ;
            {
                quantity = value;
            }

            if (data.TryGetValue("order_id", out value)) ;
            {
                order_id = value;
            }

            if (data.TryGetValue("produk", out value)) ;
            {
                produk = value;
            }
            try
            {
                pos_order_list list = new pos_order_list();
                list.order_id = order_id;
                list.peroduk = produk;
                list.quantity = quantity;
                list.status = "PENDING";

                Transaction trany = new Transaction();
                trany.details = "Penjualan " + db.Produks.FirstOrDefault(c => c.Id == produk).nama;
                trany.jumlah = db.Produks.FirstOrDefault(c => c.Id == produk).harga_jual * quantity * -1;
                trany.quantity = quantity;
                trany.type = "4.1.1." + produk;
                trany.user = user;
                db.Transactions.InsertOnSubmit(trany);

                Transaction trany2 = new Transaction();
                trany2.details = "Piutang Penjulan " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk);
                trany2.jumlah = db.Produks.FirstOrDefault(c => c.Id == produk).harga_jual * quantity;
                trany2.quantity = quantity;
                trany2.type = "1.1.5";
                trany2.user = user;
                db.Transactions.InsertOnSubmit(trany2);

                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS ADD PRODUCT";
                db.pos_logs.InsertOnSubmit(log);
                
                db.pos_order_lists.InsertOnSubmit(list);
                db.SubmitChanges();
                result = 4;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /* delete_produk
         * int id
         * 
         */
        public int delete_product(Dictionary<String, dynamic> data)
        {
            int result = 0;
            int order_id = 0;
            int quantity = 0;
            int produk = 0;
            dynamic value;

            if (data.TryGetValue("order_id", out value))
            {
                order_id = value;
            }
            if (data.TryGetValue("quantity", out value)) 
            {
                quantity = value;
            }
            
            if (data.TryGetValue("produk", out value)) 
            {
                produk = value;
            }
            try
            {
                int count = db.pos_order_lists.Where(c => c.order_id == order_id && c.peroduk == produk && c.status == "PENDING").Sum(c=>c.quantity);
                int temp = quantity;
                if (count >= quantity)
                {
                    
                        while (temp > 0)
                        {
                        pos_order_list en = db.pos_order_lists.FirstOrDefault(c => c.order_id == order_id && c.peroduk == produk && c.status == "PENDING");
                            {
                                if (temp >= en.quantity)
                                {
                                    en.status = "VOID";
                                    temp -= en.quantity;
                                    db.SubmitChanges();
                                }
                                else
                                {
                                    en.quantity -= temp;
                                    pos_order_list pol = new pos_order_list();
                                    pol.order_id = order_id;
                                    pol.peroduk = produk;
                                    pol.quantity = temp;
                                    pol.status = "VOID";
                                    db.pos_order_lists.InsertOnSubmit(pol);
                                    db.SubmitChanges();
                                    temp = 0;
                                }
                            }
                        }
                    Transaction trany = new Transaction();
                    trany.details = "Penjualan " + db.Produks.FirstOrDefault(c => c.Id == produk).nama;
                    trany.jumlah = db.Produks.FirstOrDefault(c => c.Id == produk).harga_jual * quantity;
                    trany.quantity = quantity;
                    trany.type = "4.1.1." + produk;
                    trany.user = user;
                    db.Transactions.InsertOnSubmit(trany);

                    Transaction trany2 = new Transaction();
                    trany2.details = "Piutang Penjualan " + db.Produks.FirstOrDefault(c => c.Id == produk).nama;
                    trany2.jumlah = db.Produks.FirstOrDefault(c => c.Id == produk).harga_jual * quantity * -1;
                    trany2.quantity = quantity;
                    trany2.type = "1.1.5";
                    trany2.user = user;
                    db.Transactions.InsertOnSubmit(trany2);

                    pos_log log = new pos_log();
                    log.user_id = user;
                    log.action = "POS DELETE PRODUCT";
                    db.pos_logs.InsertOnSubmit(log);
                    db.SubmitChanges();
                    result = 4;
                }else
                {
                    result = 5;
                }

                
                
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public int void_order(int id)
        {
            int result = 0;
            try
            {
                db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.pos_tables);
                pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == id);
                order.status = "VOID";
                order.pos_table.is_empty = true;
                db.SubmitChanges();


                var listorder = db.pos_order_lists.Where(c=>c.order_id==id&&c.status=="PENDING");

                foreach(pos_order_list list in listorder)
                {
                    list.status = "VOID";
                    Transaction trany = new Transaction();
                    trany.details = "Penjualan " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk).nama;
                    trany.jumlah = db.Produks.FirstOrDefault(c => c.Id == list.peroduk).harga_jual * list.quantity;
                    trany.quantity = list.quantity;
                    trany.type = "4.1.1." + list.peroduk;
                    trany.user = user;
                    db.Transactions.InsertOnSubmit(trany);

                    Transaction trany2 = new Transaction();
                    trany2.details = "Piutang Penjualan " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk);
                    trany2.jumlah = db.Produks.FirstOrDefault(c => c.Id == list.peroduk).harga_jual * list.quantity * -1;
                    trany2.quantity = list.quantity;
                    trany2.type = "1.1.5";
                    trany2.user = user;
                    db.Transactions.InsertOnSubmit(trany2);
                }

                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS VOID ORDER";
                db.pos_logs.InsertOnSubmit(log);

                db.SubmitChanges();

            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public int hold_order(int id)
        {
            int result = 0;
            try
            {
                db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.pos_tables);
                pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == id);
                order.status = "HOLD";
                order.pos_table.is_empty = true;
                db.SubmitChanges();

                var listorder = db.pos_order_lists.Where(c => c.order_id == id && c.status == "PENDING");

                foreach (pos_order_list list in listorder)
                {
                    list.status = "HOLD";
                    Transaction trany = new Transaction();
                    trany.details = "Beban Tak Tertagih " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk).nama;
                    trany.jumlah = db.Produks.FirstOrDefault(c => c.Id == list.peroduk).harga_jual * list.quantity;
                    trany.quantity = list.quantity;
                    trany.type = "5.1.3";
                    trany.user = user;
                    db.Transactions.InsertOnSubmit(trany);

                    Transaction trany2 = new Transaction();
                    trany2.details = "Piutang Penjualan " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk).nama;
                    trany2.jumlah = db.Produks.FirstOrDefault(c => c.Id == list.peroduk).harga_jual * list.quantity * -1;
                    trany2.quantity = list.quantity;
                    trany2.type = "1.1.5";
                    trany2.user = user;
                    db.Transactions.InsertOnSubmit(trany2);
                }

                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS HOLD ORDER";
                db.pos_logs.InsertOnSubmit(log);

                db.SubmitChanges();

            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public int set_discount(int id, int disc)
        {
            int result = 0;
            try
            {
                pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == id);
                order.discount = disc;

                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS SET DISC";
                db.pos_logs.InsertOnSubmit(log);

                db.SubmitChanges();
                result = 4;
            }
            catch (Exception e)
            {
                throw e;
            }


            return result;

        }

        public int checkout(int id)
        {
            int result = 0;
            try
            {
                db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.pos_tables);
                pos_order order = db.pos_orders.FirstOrDefault(c=>c.Id == id);
                order.status = "COMPLETED";
                order.pos_table.is_empty = true;
                db.SubmitChanges();

                var listorder = db.pos_order_lists.Where(c => c.order_id == id && c.status == "PENDING");

                foreach (pos_order_list list in listorder)
                {
                    list.status = "COMPLETED";
                    Transaction trany = new Transaction();
                    trany.details = "Kas Penjualan " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk).nama;
                    trany.jumlah = db.Produks.FirstOrDefault(c => c.Id == list.peroduk).harga_jual * list.quantity;
                    trany.quantity = list.quantity;
                    trany.type = "1.1.1";
                    trany.user = user;
                    db.Transactions.InsertOnSubmit(trany);

                    Transaction trany2 = new Transaction();
                    trany2.details = "Piutang Penjualan " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk).nama;
                    trany2.jumlah = db.Produks.FirstOrDefault(c => c.Id == list.peroduk).harga_jual * list.quantity * -1;
                    trany2.quantity = list.quantity;
                    trany2.type = "1.1.5";
                    trany2.user = user;
                    db.Transactions.InsertOnSubmit(trany2);
                }

                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS CHECKOUT";
                db.pos_logs.InsertOnSubmit(log);

                db.SubmitChanges();


            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public int checkout(int id, String cardnumber, String Transaction)
        {
            int result = 0;
            try
            {
                db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.pos_tables);
                pos_order order = db.pos_orders.FirstOrDefault(c => c.Id == id);
                order.status = "COMPLETED";
                order.pos_table.is_empty = true;
                db.SubmitChanges();

                var listorder = db.pos_order_lists.Where(c => c.order_id == id&&c.status=="PENDING");

                foreach (pos_order_list list in listorder)
                {
                    list.status = "COMPLETED";
                    Transaction trany = new Transaction();
                    trany.details = "Kas Penjualan " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk).nama;
                    trany.jumlah = db.Produks.FirstOrDefault(c => c.Id == list.peroduk).harga_jual * list.quantity;
                    trany.quantity = list.quantity;
                    trany.type = "1.1.1";
                    trany.user = user;
                    db.Transactions.InsertOnSubmit(trany);

                    Transaction trany2 = new Transaction();
                    trany2.details = "Piutang Penjualan " + db.Produks.FirstOrDefault(c => c.Id == list.peroduk).nama;
                    trany2.jumlah = db.Produks.FirstOrDefault(c => c.Id == list.peroduk).harga_jual * list.quantity * -1;
                    trany2.quantity = list.quantity;
                    trany2.type = "1.1.5";
                    trany2.user = user;
                    db.Transactions.InsertOnSubmit(trany2);
                }

                pos_log log = new pos_log();
                log.user_id = user;
                log.action = "POS CHECKOUT";
                db.pos_logs.InsertOnSubmit(log);

                db.SubmitChanges();


            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }





    }
}
