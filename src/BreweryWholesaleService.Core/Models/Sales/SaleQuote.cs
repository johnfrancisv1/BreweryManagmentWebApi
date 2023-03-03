using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryWholesaleService.Core.Models.Sales
{
    public class SaleQuote
    {
        public string ClientName { get; set; }
        List<QuoteItem> _Items { get; set; }

        int totalQuantity = 0;
        decimal totalPrice = 0;
        public List<QuoteItem> Items 
        {
            get { return _Items; }
        }

        public void AddQuoteItem(QuoteItem NewItem)
        {
            _Items.Add(NewItem);
            totalQuantity+= NewItem.Quantity;
            totalPrice += NewItem.TotalPrice;
        }


        public int TotalQuntity
        {
            get { return totalQuantity; }
        }
        public decimal Discount
        {
            get
            {
               
               if(TotalQuntity > 20)
                {
                    return 0.20m;
                }else if(TotalQuntity > 10)
                {
                    return 0.10m;
                }
                return 0;
            }
        }
        public decimal TotalPrice {
            get
            {
                return totalPrice;
            }
        }

        public decimal FinalPrice
        {
            get
            {
                decimal totalPriceNoDiscount = this.TotalPrice;
                return totalPriceNoDiscount -  Discount * this.TotalPrice;

            }
        }

    }
}
