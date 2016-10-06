using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASME_C_WPF.core
{
    class Abc_module
    {
        CoreDataContext db = new CoreDataContext();
        public dynamic calculate()
        {
            var result = 0;
           
            var costlist = bb_cost();
            var produklist = db.Produks;
            double hpp_total = 0.0;
            DateTime date = DateTime.Now;
            
            foreach(Produk some in produklist)
            {
                double cost = 0.0;
                int count = 0;
                var pbb = db.p_bbs.Where(c => c.produk == some.Id);
                double value = 0.0;
                double hpp = 0.0;
                foreach (p_bb bb_cost in pbb)
                {
                    if(costlist.TryGetValue(bb_cost.bb,out value))
                    {
                        cost += value;
                    }
                    
                }
                cost_driver csd = db.cost_drivers.FirstOrDefault(c=>c.Id == 1);
                var sales = db.Transactions.Where(c => c.type == ("4.1.1." + some.Id) && c.date.Month == date.Month);
                foreach (Transaction p in sales)
                {
                    if (p.jumlah < 0)
                    {
                        count += p.quantity;
                    }
                    else
                    {
                        count -= p.quantity;
                    }
                }



                Console.WriteLine("debug cost = " + cost);
                abc_log write = new abc_log();
                write.produk = some.Id;
                write.cost = cost*1.0;
                write.costdriver = csd.Id;
                write.month = date.Month.ToString();
                write.year = date.Year;
                write.unit_produksi = count;
                
                db.abc_logs.InsertOnSubmit(write);
                db.SubmitChanges();

                hpp_total += hpp*count;
                
            }

            if (hpp_total > 0)
            {
                Transaction tr = new Transaction();
                tr.details = "Harga Pokok Produksi";
                tr.jumlah = (long)hpp_total;
                tr.type = "4.1.2";
                tr.user = 1;
                db.Transactions.InsertOnSubmit(tr);

                Transaction tr2 = new Transaction();
                tr2.details = "Biaya Bahan Baku";
                tr2.type = "5.1.2";
                tr2.jumlah = (long)(hpp_total) * -1;
                tr2.user = 1;
                db.Transactions.InsertOnSubmit(tr2);
                db.SubmitChanges();
            }


            return result;
        }

        private Dictionary<int, double> bb_cost()
        {
            Dictionary<int, double> result = new Dictionary<int, double>();
            var bblist = db.Bahan_bakus;
            DateTime date = DateTime.Now;
            long biaya_bb = 0;
            int qty = 0;
            foreach(Bahan_baku bb in bblist)
            {
                var produklist = db.p_bbs.Where(c => c.bb == bb.Id);
                var unit = 0;
                foreach(p_bb some in produklist)
                {
                    var sales = db.Transactions.Where(c=> c.type == ("4.1.1."+some.produk) && c.date.Month == date.Month);
                    foreach(Transaction p in sales)
                    {
                        if (p.jumlah < 0)
                        {
                            unit += p.quantity;
                        }
                        else
                        {
                            unit -= p.quantity;
                        }
                    }
                }

                long used = 0;
                int count = 0;
               
                double cost = 0.0;
                var loglist = db.bb_logs.Where(c=> c.tipe == bb.Id && c.date.Month == date.Month);
                foreach(bb_log log in loglist)
                {
                    used += log.used*log.hpp_digunakan;
                    count += log.used;
                }
                

                if (unit > 0)
                {
                    
                    cost = (used*1.0) / (unit*1.0);
                    Console.WriteLine("debug = " + used + "/" + unit+"= "+cost);

                }
                if (count > 0)
                {
                    Transaction tr = new Transaction();
                    tr.details = "Penggunaan " + bb.nama;
                    tr.type = "1.1.3." + bb.Id;
                    tr.jumlah = used*-1;
                    tr.quantity = count;
                    tr.user = 1;
                    db.Transactions.InsertOnSubmit(tr);
                    db.SubmitChanges();
                }

                biaya_bb += used;
                qty += count;
                
                result.Add(bb.Id,cost);

            }

            if (biaya_bb > 0)
            {
                Transaction tr = new Transaction();
                tr.details = "Biaya Bahan Baku";
                tr.type = "5.1.2";
                tr.jumlah = biaya_bb;
                tr.quantity = qty;
                tr.user = 1;
                db.Transactions.InsertOnSubmit(tr);
                db.SubmitChanges();
            }

            return result;
        }

        

    }
}
