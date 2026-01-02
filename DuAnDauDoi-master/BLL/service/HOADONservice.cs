using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BLL.Service
{
    public class HOADONservice
    {
        private readonly Random _random = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public Hoadon GetById(string id)
        {
            using (var db = new Model1())
            {
                 return db.Hoadons
                    .Include(h => h.Cthds.Select(c => c.Mon))
                    .FirstOrDefault(h => h.Mahd == id);
            }
        }

        public Hoadon GetUnpaidInvoiceByTable(int tableId)
        {
            using (var db = new Model1())
            {
                return db.Hoadons
                    .Include(h => h.Cthds.Select(c => c.Mon))
                    .FirstOrDefault(h => h.Maban == tableId && h.Status == 0);
            }
        }

        // Logic to Create Invoice if not exists, add detail, update total
        public void AddOrder(int tableId, List<Tuple<Mon, int>> items)
        {
             using (var db = new Model1())
             {
                 using (var transaction = db.Database.BeginTransaction())
                 {
                     try
                     {
                         var hoadon = db.Hoadons.FirstOrDefault(h => h.Maban == tableId && h.Status == 0);
                         string currentMahd;
                         
                         if (hoadon == null)
                         {
                             currentMahd = GenerateUniqueMahd(db, "HD");
                             hoadon = new Hoadon
                             {
                                 Mahd = currentMahd,
                                 Ngaylap = DateTime.Now,
                                 Status = 0,
                                 Maban = tableId,
                                 Tongtien = 0
                             };
                             db.Hoadons.Add(hoadon);
                         }
                         else
                         {
                             currentMahd = hoadon.Mahd;
                         }

                         foreach (var item in items)
                         {
                             var mon = item.Item1;
                             var qty = item.Item2;
                             
                             var existingCthd = db.Cthds.FirstOrDefault(ct => ct.Mahd == currentMahd && ct.Mamon == mon.Mamon);

                             if (existingCthd != null)
                             {
                                 existingCthd.Sl += qty;
                             }
                             else
                             {
                                 string randomSuffix = GenerateRandomString(3);
                                 string maCTHD = $"CT{currentMahd.Substring(currentMahd.Length - 3)}{randomSuffix}";
                                 if (maCTHD.Length > 10) maCTHD = maCTHD.Substring(0, 10);
                                 
                                 // Ensure unique ID loop could be here but keeping simple for now
                                 while(db.Cthds.Any(x => x.Macthd == maCTHD))
                                 {
                                      randomSuffix = GenerateRandomString(3);
                                      maCTHD = $"CT{currentMahd.Substring(currentMahd.Length - 3)}{randomSuffix}";
                                      if (maCTHD.Length > 10) maCTHD = maCTHD.Substring(0, 10);
                                 }

                                 db.Cthds.Add(new Cthd
                                 {
                                     Macthd = maCTHD,
                                     Mahd = currentMahd,
                                     Mamon = mon.Mamon,
                                     Sl = qty,
                                     Khuyenmai = 0
                                 });
                             }
                         }

                         db.SaveChanges(); // Save changes to calculate total

                         // Recalculate Total
                         var allDetails = db.Cthds.Where(ct => ct.Mahd == currentMahd).ToList();
                         decimal totalAmount = 0;
                         foreach (var detail in allDetails)
                         {
                            // Need to fetch price again or assume Mon is loaded. Safer to fetch.
                            var price = db.Mons.Where(m => m.Mamon == detail.Mamon).Select(m => m.Giamon).FirstOrDefault() ?? 0;
                            totalAmount += price * (decimal)(detail.Sl ?? 0);
                         }
                         hoadon.Tongtien = totalAmount;

                         // Update Table Status
                         var ban = db.Bans.Find(tableId);
                         if (ban != null) ban.Status = "Có khách";

                         db.SaveChanges();
                         transaction.Commit();
                     }
                     catch
                     {
                         transaction.Rollback();
                         throw;
                     }
                 }
             }
        }
        
        public void UpdateOrder(string invoiceId, int tableId, List<Tuple<int, int>> newItems)
        {
            using (var db = new Model1())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var hoadon = db.Hoadons.FirstOrDefault(h => h.Mahd == invoiceId);
                        if (hoadon == null) return;

                        // 1. Remove old details
                        var oldCthds = db.Cthds.Where(c => c.Mahd == invoiceId).ToList();
                        db.Cthds.RemoveRange(oldCthds);

                        if (newItems == null || newItems.Count == 0)
                        {
                            // If no items, remove invoice and set table to empty
                            db.Hoadons.Remove(hoadon);
                            var ban = db.Bans.Find(tableId);
                            if (ban != null) ban.Status = "Trống";
                        }
                        else
                        {
                            // 2. Add new details
                            int i = 1;
                            decimal total = 0;
                            foreach (var item in newItems)
                            {
                                int mamonId = item.Item1;
                                int qty = item.Item2;
                                var mon = db.Mons.Find(mamonId);
                                decimal price = mon?.Giamon ?? 0;
                                total += price * qty;
                                
                                string randomSuffix = GenerateRandomString(3);
                                string maCTHD = $"CT{invoiceId.Substring(invoiceId.Length - 3)}{randomSuffix}";
                                 while(db.Cthds.Any(x => x.Macthd == maCTHD))
                                 {
                                      randomSuffix = GenerateRandomString(3);
                                      maCTHD = $"CT{invoiceId.Substring(invoiceId.Length - 3)}{randomSuffix}";
                                 }

                                db.Cthds.Add(new Cthd
                                {
                                    Macthd = maCTHD,
                                    Mahd = invoiceId,
                                    Mamon = mamonId,
                                    Sl = qty
                                });
                            }
                            hoadon.Tongtien = total;
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void PayOrder(string invoiceId, decimal givenAmount)
        {
             using (var db = new Model1())
            {
                var hoadon = db.Hoadons.Find(invoiceId);
                if (hoadon != null)
                {
                    hoadon.Status = 1;
                    hoadon.Ngayxuat = DateTime.Now;
                    // Ensure Tongtien is correct or re-calculate? define flow. Assuming updated.
                    
                    var ban = db.Bans.Find(hoadon.Maban);
                    if (ban != null) ban.Status = "Trống";

                    db.SaveChanges();
                }
            }
        }

        public object GetHistory(string searchText)
        {
             using (var db = new Model1())
            {
                var query = db.Hoadons.Where(h => h.Status == 1); 

                if (!string.IsNullOrEmpty(searchText))
                {
                    query = query.Where(h => h.Mahd.Contains(searchText) || h.Maban.ToString().Contains(searchText));
                }

                return query.OrderByDescending(h => h.Ngayxuat)
                    .Select(h => new
                    {
                        MaHD = h.Mahd,
                        NgayLap = h.Ngaylap,
                        NgayThanhToan = h.Ngayxuat,
                        SoBan = h.Maban,
                        TongTien = h.Tongtien,
                        GiamGia = h.KHUYENMAI_HD ?? 0
                    }).ToList();
            }
        }

        private string GenerateRandomString(int length)
        {
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++) stringChars[i] = Chars[_random.Next(Chars.Length)];
            return new string(stringChars);
        }

        private string GenerateUniqueMahd(Model1 context, string prefix = "HD")
        {
            string newMahd;
            do
            {
                newMahd = $"{prefix}{GenerateRandomString(3)}";
            } while (context.Hoadons.Any(h => h.Mahd == newMahd));
            return newMahd;
        }
    }
}
