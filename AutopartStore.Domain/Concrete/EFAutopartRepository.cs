using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutopartStore.Domain.Abstract;
using AutopartStore.Domain.Entities;

namespace AutopartStore.Domain.Concrete
{
    public class EFAutopartRepository : IAutopartRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Autopart> Autoparts
        {
            get { return context.Autoparts; }
        }

        public void SaveAutopart(Autopart autopart)
        {
            if (autopart.AutopartId == 0)
                context.Autoparts.Add(autopart);
            else
            {
                Autopart dbEntry = context.Autoparts.Find(autopart.AutopartId);
                if (dbEntry != null)
                {
                    dbEntry.Name = autopart.Name;
                    dbEntry.Price = autopart.Price;
                    dbEntry.Description = autopart.Description;
                    dbEntry.Amount = autopart.Amount;
                    dbEntry.Category = autopart.Category;
                }
            }
            context.SaveChanges();
        }
        public Autopart DeleteAutopart(int autopartId)
        {
            Autopart dbEntry = context.Autoparts.Find(autopartId);
            if (dbEntry != null)
            {
                context.Autoparts.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
