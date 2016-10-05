using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

namespace ASME_C_WPF.core
{
    partial class Transaction
    {
    }

    class Core
    {
        /* response
         * 0 : General Error
         * 1 : {{reserved}}
         * 2 : Missing Reference
         * 3 : 
         * 4 : Success
         * 5 : Not Enough Stock
         */
        CoreDataContext db = new CoreDataContext();
        /* Management Pengeluaran
         * 
         */

        /* Pembayaran Utang
        *  
        * var_input Dictionary<String, dynamic>
        * user = master_user
        * id = String
        * details = String
        * jumlah = long
        */
        public dynamic Pembayaran_utang(Dictionary<String, dynamic> Data)
        {
            var result = 0;
            dynamic value;
            String detail = null;
            String id = null;
            master_user user = null ;
            long jumlah = -1;

            if (Data.TryGetValue("user", out value))
            {
                user = value;
            };

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            if (Data.TryGetValue("id", out value))
            {
                id ="2.1.1."+value;
            };
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user != null))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user.Id;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user.Id;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch(Exception e){ };

            }
            else {
                ///missing reference
                result = 2;
            };
            
            return result;
            }

        /* Prive
        *  
        * var_input Dictionary<String, dynamic>
        * user = master_user
        * details = String
        * jumlah = long
        */
        public dynamic Prive(Dictionary<String, dynamic> Data)
        {
            dynamic result = 0;
            dynamic value;
            String details = null;
            master_user user = null; 
            long jumlah = -1;

            if (Data.TryGetValue("user", out value))
            {
                user = value;
                Console.WriteLine(user);
            };

            if (Data.TryGetValue("details", out value))
            {
                details = value;
                Console.WriteLine(details);
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
                Console.WriteLine(jumlah);
            };

         
            if ((details != null) && (jumlah >= 0) && (user !=null))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = details;
                    trans1.jumlah = jumlah;
                    trans1.type = "3.1.2";
                    trans1.user = user.Id;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = details;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user.Id;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch (Exception e){ };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        /* Pembayaran Beban 
         * 
         * var_input Dictionary<String, dynamic>
         * user = master_user
         * id = String
         * details = String
         * jumlah = long
         */
        public dynamic Pembayaran_beban(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            String detail = null;
            String id = null;
            master_user user = null;
            long jumlah = -1;

            if (Data.TryGetValue("user", out value))
            {
                user = value;
            };

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            if (Data.TryGetValue("id", out value))
            {
                id = "5.1.1." + value;
            };
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user != null))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user.Id;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user.Id;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch(Exception e){ };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        /* Pembelian_bahan_baku 
        * 
        * var_input Dictionary<String, dynamic>
        * user = master_user
        * id = String
        * details = String
        * quantity = int
        * jumlah = long
        */
        public dynamic Pembelian_bahan_baku(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            int bb_id =-1; 
            String id = null;
            master_user user = null;
            long jumlah = -1;
            int quantity = -1;


            if (Data.TryGetValue("user", out value))
            {
                user = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            if (Data.TryGetValue("quantity", out value))
            {
                quantity = value;
            };

            if (Data.TryGetValue("id", out value))
            {
                id = "1.1.3." + value;
                bb_id = value;
            };
            if ((jumlah >= 0) && (id != null) && (user != null)&&(quantity>=0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = "Pembelian "+db.Bahan_bakus.FirstOrDefault(c=>c.Id==bb_id).nama;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user.Id;
                    trans1.quantity = quantity;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = "Pembelian " + db.Bahan_bakus.FirstOrDefault(c => c.Id == bb_id).nama;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user.Id;
                    trans2.quantity = quantity;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();

                    long hpp = jumlah / quantity;
                    Bahan_baku bb = db.Bahan_bakus.FirstOrDefault(c => c.Id == bb_id);
                    if (bb != null)
                    {
                        bb_stock stock = new bb_stock();
                        stock.tipe = bb.Id;
                        stock.harga_beli = jumlah;
                        stock.quantity = quantity;
                        stock.hpp_digunakan = hpp;
                        stock.used = 0;
                        stock.satuan = bb.satuan;
                        db.bb_stocks.InsertOnSubmit(stock);
                        db.SubmitChanges();

                        bb_log log = new bb_log();
                        log.nama = bb.nama;
                        log.kode_stock = stock.Id;
                        log.harga_beli = jumlah;
                        log.hpp_digunakan = hpp;
                        log.satuan = bb.satuan;
                        log.add = quantity;
                        log.used = 0;
                        db.bb_logs.InsertOnSubmit(log);
                        db.SubmitChanges();

                        bb.quantity = bb.quantity + quantity;
                        bb.harga_beli = db.bb_stocks.First(c=>c.tipe == bb.Id).harga_beli;
                        bb.hpp_digunakan = db.bb_stocks.First(c => c.tipe == bb.Id).hpp_digunakan;
                        db.SubmitChanges();
                    }
                    
                    ///success
                    result = 4;
                }
                catch(Exception e)
                {

                    throw e;
                        }

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        /* Stock Management
         * 
         */


        /* Pengurangan_stock 
        * 
        * var_input Dictionary<String, dynamic>
        * user = master_user
        * id = int
        * quantity = int
        */
        public dynamic Pengurangan_stock(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            int bb_id = -1;
            master_user user = null;
            int quantity = -1;
            

            if (Data.TryGetValue("user", out value))
            {
                user = value;
            };

            if (Data.TryGetValue("quantity", out value))
            {
                quantity = value;
            };

            if (Data.TryGetValue("id", out value))
            {
                bb_id = value;
            };
            if ((bb_id > 0) && (user != null) && (quantity >= 0))
            {
                try
                {
                    Bahan_baku bb = db.Bahan_bakus.FirstOrDefault(c => c.Id == bb_id);
                    int count = db.bb_stocks.Count(c => c.tipe == bb_id);
                    int sum = db.bb_stocks.Where(c => c.tipe == bb_id).Sum(c => c.quantity);
                    if ((count > 0)&&(quantity<=sum))
                    {


                        bb_stock some;
                        bb.used += quantity;
                       
                            while (quantity > 0)
                            {
                                some = db.bb_stocks.First(c => c.tipe == bb.Id);
                                if (some.quantity > quantity)
                                {

                                    
                                    bb_stock st = db.bb_stocks.FirstOrDefault(c=>c.Id==some.Id);
                                    st.quantity -= quantity;
                                    st.used += quantity;
                                    bb_log log = new bb_log();
                                    log.used = quantity;
                                    log.Satuan_bb = some.Satuan_bb;
                                    log.kode_stock = some.Id;
                                    log.hpp_digunakan = some.hpp_digunakan;
                                    log.harga_beli = some.harga_beli;
                                    log.nama = db.Bahan_bakus.FirstOrDefault(c=>c.Id == some.tipe).nama;
                                    db.bb_logs.InsertOnSubmit(log);
                                    db.SubmitChanges();
                                    quantity = 0;
                                }
                                else
                                {
                                    quantity -= some.quantity;
                                    db.bb_stocks.DeleteOnSubmit(db.bb_stocks.FirstOrDefault(c => c.Id == some.Id));
                                    bb_log log = new bb_log();
                                    log.used = some.quantity;
                                    log.Satuan_bb = some.Satuan_bb;
                                    log.kode_stock = some.Id;
                                    log.hpp_digunakan = some.hpp_digunakan;
                                    log.harga_beli = some.harga_beli;
                                    log.nama = db.Bahan_bakus.FirstOrDefault(c => c.Id == some.tipe).nama;
                                    db.bb_logs.InsertOnSubmit(log);
                                    db.SubmitChanges();
                                    
                                }
                               
                            }
                        
                      
                        bb.quantity = db.bb_stocks.Where(c => c.tipe == bb.Id).Sum(c => c.quantity);
                        bb.harga_beli = db.bb_stocks.First(c => c.tipe == bb.Id).harga_beli;
                        bb.hpp_digunakan = db.bb_stocks.First(c => c.tipe == bb.Id).hpp_digunakan;
                        db.SubmitChanges();
                    }
                    else
                    {
                        result = 5;
                    }
                         


                    ///success
                    result = 4;
                }
                catch(Exception e) {Console.WriteLine(e.ToString()); };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        /* Management Pemasukan
         * 
         */

        /* Pemasukan Piutang
        *  
        * var_input Dictionary<String, dynamic>
        * user = master_user
        * id = int
        * details = String
        * jumlah = long
        */
        public dynamic Penerimaan_piutang(Dictionary<String, dynamic> Data)
        {
            var result = 0;
            dynamic value;
            String detail = null;
            String id = null;
            int pt_id = -1;
            master_user user = null;
            long jumlah = -1;

            if (Data.TryGetValue("user", out value))
            {
                user = value;
            };

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            if (Data.TryGetValue("id", out value))
            {
                id = "1.1.2." + value;
                pt_id = value;
            };
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user != null))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah*-1;
                    trans1.type = id;
                    trans1.user = user.Id;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah;
                    trans2.type = "1.1.1";
                    trans2.user = user.Id;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch(Exception e){ };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        /* Pemasukan Utang
        *  
        * var_input Dictionary<String, dynamic>
        * user = master_user
        * id = int
        * details = String
        * jumlah = long
        */
        public dynamic Penerimaan_utang(Dictionary<String, dynamic> Data)
        {
            var result = 0;
            dynamic value;
            String detail = null;
            String id = null;
            int pt_id = -1;
            master_user user = null;
            long jumlah = -1;

            if (Data.TryGetValue("user", out value))
            {
                user = value;
            };

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            if (Data.TryGetValue("id", out value))
            {
                id = "2.1.1." + value;
                pt_id = value;
            };
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user != null))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah * -1;
                    trans1.type = id;
                    trans1.user = user.Id;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah;
                    trans2.type = "1.1.1";
                    trans2.user = user.Id;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch(Exception e){ };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        /* Pemasukan modal
        *  
        * var_input Dictionary<String, dynamic>
        * user = master_user
        * details = String
        * jumlah = long
        */
        public dynamic Penerimaan_modal(Dictionary<String, dynamic> Data)
        {
            var result = 0;
            dynamic value;
            String detail = null;
            master_user user = null;
            long jumlah = -1;

            if (Data.TryGetValue("user", out value))
            {
                user = value;
            };

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            
            if ((detail != null) && (jumlah >= 0) && (user != null))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah * -1;
                    trans1.type = "3.1.1";
                    trans1.user = user.Id;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah;
                    trans2.type = "1.1.1";
                    trans2.user = user.Id;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch(Exception e){ };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        /* Pemasukan penjualan
        *  
        * var_input Dictionary<String, dynamic>
        * user = master_user
        * id = int
        * quantity = int
        * jumlah = long
        */
        public dynamic Penerimaan_penjualan(Dictionary<String, dynamic> Data)
        {
            var result = 0;
            dynamic value;
            String id = null;
            int pt_id = -1;
            master_user user = null;
            int qty = -1;

            if (Data.TryGetValue("user", out value))
            {
                user = value;
            };

            if (Data.TryGetValue("quantity", out value))
            {
                qty = value;
            };

            if (Data.TryGetValue("id", out value))
            {
                id = "4.1.1." + value;
                pt_id = value;
            };
            if ((qty >= 0) && (id != null) && (user != null))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = "Penjualan "+db.Produks.FirstOrDefault(c=>c.Id == pt_id).nama;
                    trans1.jumlah = db.Produks.FirstOrDefault(c => c.Id == pt_id).harga_jual* qty * -1;
                    trans1.type = id;
                    trans1.user = user.Id;
                    trans1.quantity = qty;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = "Penjualan " + db.Produks.FirstOrDefault(c => c.Id == pt_id).nama;
                    trans2.jumlah = db.Produks.FirstOrDefault(c => c.Id == pt_id).harga_jual*qty;
                    trans2.type = "1.1.1";
                    trans2.user = user.Id;
                    trans2.quantity = qty;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch(Exception e) {  };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

    }
}
