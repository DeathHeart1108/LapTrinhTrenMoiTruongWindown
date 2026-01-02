using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BLL.Service
{
    public class MONservice
    {
        public List<Mon> GetAll()
        {
            using (var db = new Model1())
            {
                return db.Mons.Include(m => m.Loaimon).ToList();
            }
        }

        public List<Mon> GetByLoai(string tenLoai)
        {
            using (var db = new Model1())
            {
                var query = db.Mons.Include(m => m.Loaimon).AsQueryable();

                if (!string.IsNullOrEmpty(tenLoai))
                {
                    query = query.Where(m => m.Loaimon.Tenloai == tenLoai);
                }

                return query.ToList();
            }
        }

        public Mon GetById(int id)
        {
             using (var db = new Model1())
            {
                return db.Mons.Find(id);
            }
        }
    }
}
