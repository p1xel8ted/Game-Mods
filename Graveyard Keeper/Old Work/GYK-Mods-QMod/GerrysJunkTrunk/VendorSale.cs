using System.Collections.Generic;

namespace GerrysJunkTrunk;

internal class VendorSale
{
    internal class Sale
    {
        private readonly Item _item;
        private readonly float _qty;
        private readonly float _price;

        internal Sale(Item item, float qty, float price)
        {
            _item = item;
            _qty = qty;
            _price = price;
        }

        public Item GetItem()
        {
            return _item;
        }

        public float GetQty()
        {
            return _qty;
        }

        public float GetPrice()
        {
            return _price;
        }
    }

    private Vendor Vendor { get; }
    private readonly List<Sale> _saleList = new();

    public VendorSale(Vendor vendor)
    {
        Vendor = vendor;
    }

    public void AddSale(Item item, float qty, float price)
    {
        _saleList.Add(new Sale(item, qty, price));
    }

    public List<Sale> GetSales()
    {
        return _saleList;
    }

    public Vendor GetVendor()
    {
        return Vendor;
    }
}