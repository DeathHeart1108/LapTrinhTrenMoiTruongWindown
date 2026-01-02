using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Service
{
    public class BANservice
    {
        public List<Ban> GetAll()
        {
            using (var db = new Model1())
            {
                return db.Bans.ToList()
                 .OrderBy(t => int.TryParse(t.Soban, out int res) ? res : 0)
                 .ToList();
            }
        }

        public Ban GetById(int id)
        {
            using (var db = new Model1())
            {
                return db.Bans.Find(id);
            }
        }

        public void UpdateStatus(int id, string status)
        {
            using (var db = new Model1())
            {
                var ban = db.Bans.Find(id);
                if (ban != null)
                {
                    ban.Status = status;
                    db.SaveChanges();
                }
            }
        }
    }
}
