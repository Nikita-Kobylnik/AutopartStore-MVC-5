using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutopartStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Autopart autopart, int quantity)
        {
            CartLine line = lineCollection
                .Where(a => a.Autopart.AutopartId == autopart.AutopartId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Autopart = autopart,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Autopart autopart)
        {
            lineCollection.RemoveAll(l => l.Autopart.AutopartId == autopart.AutopartId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Autopart.Price * e.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

    }
    public class CartLine
    {
        public Autopart Autopart { get; set; }
        public int Quantity { get; set; }
    }
}
