using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Service
{
    public class LOAIMONservice
    {
        public List<Loaimon> GetAll()
        {
            using (var db = new Model1())
            {
                return db.Loaimons.ToList();
            }

        }
    }
}
