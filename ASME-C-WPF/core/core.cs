using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;

namespace ASME_C_WPF.core
{

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
        public int user = Properties.Settings.Default.Active_user;

        public static List<MaterialIcons.MaterialIconType> iconlist = new List<MaterialIcons.MaterialIconType>(new MaterialIcons.MaterialIconType[] 
        {
            MaterialIcons.MaterialIconType.ic_local_bar,
            MaterialIcons.MaterialIconType.ic_local_dining,
            MaterialIcons.MaterialIconType.ic_local_pizza,
            MaterialIcons.MaterialIconType.ic_local_cafe,

        });

        public static Dictionary<String, String> accountlist = new Dictionary<string, string>()
        {
            {"1.1.1","Kas" },
            {"1.1.2","Piutang Usaha" },
            {"1.1.3","Persediaan Bahan Baku" },
            {"1.1.4","Perlengkapan" },
            {"1.1.5","Piutang Penjualan" },
            {"1.2.1","Peralatan" },
            {"2.1.1","Utang Lancar" },
            {"3.1.1","Modal Pemilik" },
            {"3.1.2","Prive" },
            {"4.1.1","Penjualan" },
            {"4.1.2","Harga Pokok Produksi" },
            {"5.1.1","Beban Umum" },
            {"5.1.2","Biaya Bahan Baku" },
            {"5.1.3","Beban Tak Tertagih" },
            {"5.1.4","PPN" },
            {"5.1.5","Pajak" },
            {"5.1.6","Biaya Lain-Lain" },
            {"6.1.1","Laba" },
            {"6.1.2","Rugi" },


        };


        public static string calculateMD5(string text)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

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
            String detail = "2.1.1";
            String id = null;
            long jumlah = -1;


            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user >0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch{ };

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
            long jumlah = -1;


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

         
            if ((details != null) && (jumlah >= 0) && (user > 0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = details;
                    trans1.jumlah = jumlah;
                    trans1.type = "3.1.2";
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = details;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch{ };

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
        public dynamic Pembayaran_beban_umum(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            String detail = null;
            String id = null;
            long jumlah = -1;

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
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user >0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch{ };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        public dynamic Pembayaran_beban_pajak(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            String detail = null;
            String id = "5.1.5";
            long jumlah = -1;

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user > 0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch { };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }

        public dynamic Pembayaran_beban_ppn(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            String detail = null;
            String id = "5.1.4";
            long jumlah = -1;

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

           
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user > 0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch { };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }
        public dynamic Pembayaran_beban_pemeliharaan(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            String detail = null;
            String id = "5.1.6";
            long jumlah = -1;

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user > 0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch { };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }
        public dynamic Pembayaran_beban_lain(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            String detail = null;
            String id = "5.1.7";
            long jumlah = -1;

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user > 0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch { };

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
        * id = int
        * quantity = int
        */
        public dynamic Pengurangan_stock(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            int bb_id = -1;
            int quantity = -1;
            

            if (Data.TryGetValue("quantity", out value))
            {
                quantity = value;
            };

            if (Data.TryGetValue("id", out value))
            {
                bb_id = value;
            };
            if ((bb_id > 0) && (user >0) && (quantity >= 0))
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
                                    log.kode_stock = some.Id;
                                    log.hpp_digunakan = some.hpp_digunakan;
                                    log.harga_beli = some.harga_beli;
                                    log.satuan = some.satuan;
                                    log.tipe = db.Bahan_bakus.FirstOrDefault(c=>c.Id == some.tipe).Id;
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
                                    log.kode_stock = some.Id;
                                    log.hpp_digunakan = some.hpp_digunakan;
                                    log.harga_beli = some.harga_beli;
                                    log.satuan = some.satuan;
                                    log.tipe = db.Bahan_bakus.FirstOrDefault(c => c.Id == some.tipe).Id;
                                    db.bb_logs.InsertOnSubmit(log);
                                    db.SubmitChanges();
                                    
                                }
                               
                            }

                        if( db.bb_stocks.Where(c => c.tipe == bb.Id).Count() == 0){
                            bb.quantity = 0;
                            bb.harga_beli = 0;
                            bb.hpp_digunakan = 0;
                        }
                        else
                        {
                            bb.quantity = db.bb_stocks.Where(c => c.tipe == bb.Id).Sum(c => c.quantity);
                            bb.harga_beli = db.bb_stocks.First(c => c.tipe == bb.Id).harga_beli;
                            bb.hpp_digunakan = db.bb_stocks.First(c => c.tipe == bb.Id).hpp_digunakan;
                        }
                        
                        db.SubmitChanges();
                    }
                    else
                    {
                        result = 5;
                    }
                         


                    ///success
                    result = 4;
                }
                catch(Exception e){ throw e; };

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
       * id = String
       * details = String
       * quantity = int
       * jumlah = long
       */
        public dynamic Penambahan_stock(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            int bb_id = -1;
            String id = null;
            long jumlah = -1;
            int quantity = -1;

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
            if ((jumlah >= 0) && (id != null) && (user > 0) && (quantity >= 0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = "Pembelian " + db.Bahan_bakus.FirstOrDefault(c => c.Id == bb_id).nama;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = quantity;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = "Pembelian " + db.Bahan_bakus.FirstOrDefault(c => c.Id == bb_id).nama;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "1.1.1";
                    trans2.user = user;
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
                        log.tipe = bb.Id;
                        log.kode_stock = stock.Id;
                        log.harga_beli = jumlah;
                        log.hpp_digunakan = hpp;
                        log.add = quantity;
                        log.used = 0;
                        log.satuan = bb.satuan;
                        db.bb_logs.InsertOnSubmit(log);
                        db.SubmitChanges();

                        bb.quantity += quantity;
                        bb.harga_beli = db.bb_stocks.First(c => c.tipe == bb.Id).harga_beli;
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

        public dynamic Penambahan_stock_modal(Dictionary<String, dynamic> Data)
        {
            ///result initialized
            var result = 0;
            dynamic value;
            int bb_id = -1;
            String id = null;
            long jumlah = -1;
            int quantity = -1;

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
            if ((jumlah >= 0) && (id != null) && (user > 0) && (quantity >= 0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = "Pembelian " + db.Bahan_bakus.FirstOrDefault(c => c.Id == bb_id).nama;
                    trans1.jumlah = jumlah;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = quantity;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = "Modal " + db.Bahan_bakus.FirstOrDefault(c => c.Id == bb_id).nama;
                    trans2.jumlah = jumlah * -1;
                    trans2.type = "3.1.1";
                    trans2.user = user;
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
                        log.tipe = bb.Id;
                        log.kode_stock = stock.Id;
                        log.harga_beli = jumlah;
                        log.hpp_digunakan = hpp;
                        log.add = quantity;
                        log.used = 0;
                        log.satuan = bb.satuan;
                        db.bb_logs.InsertOnSubmit(log);
                        db.SubmitChanges();

                        bb.quantity += quantity;
                        bb.harga_beli = db.bb_stocks.First(c => c.tipe == bb.Id).harga_beli;
                        bb.hpp_digunakan = db.bb_stocks.First(c => c.tipe == bb.Id).hpp_digunakan;
                        db.SubmitChanges();
                    }

                    ///success
                    result = 4;
                }
                catch (Exception e)
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
            String id = "1.1.2";
            int pt_id = -1;
            long jumlah = -1;
            

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

           
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user >0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah*-1;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch{ };

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
            String detail = "2.1.1";
            String id = null;
            int pt_id = -1;
            long jumlah = -1;
            

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };
            if ((detail != null) && (jumlah >= 0) && (id != null) && (user >0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah * -1;
                    trans1.type = id;
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch{ };

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
            long jumlah = -1;
            
            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };

            
            if ((detail != null) && (jumlah >= 0) && (user >0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah * -1;
                    trans1.type = "3.1.1";
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch{ };

            }
            else
            {
                ///missing reference
                result = 2;
            };

            return result;
        }
        public dynamic Penerimaan_lain(Dictionary<String, dynamic> Data)
        {
            var result = 0;
            dynamic value;
            String detail = null;
            long jumlah = -1;

            if (Data.TryGetValue("details", out value))
            {
                detail = value;
            };

            if (Data.TryGetValue("jumlah", out value))
            {
                jumlah = value;
            };


            if ((detail != null) && (jumlah >= 0) && (user > 0))
            {
                try
                {
                    Transaction trans1 = new Transaction();
                    trans1.details = detail;
                    trans1.jumlah = jumlah * -1;
                    trans1.type = "4.1.3";
                    trans1.user = user;
                    trans1.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans1);
                    db.SubmitChanges();

                    Transaction trans2 = new Transaction();
                    trans2.details = detail;
                    trans2.jumlah = jumlah;
                    trans2.type = "1.1.1";
                    trans2.user = user;
                    trans2.quantity = 1;
                    db.Transactions.InsertOnSubmit(trans2);
                    db.SubmitChanges();
                    ///success
                    result = 4;
                }
                catch { };

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
