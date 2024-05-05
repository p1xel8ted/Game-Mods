namespace GerrysJunkTrunk;

internal class ItemPrice
{
    private readonly Item _item;
    private readonly int _qty;
    private readonly float _price;

    public ItemPrice(Item item, int qty, float price)
    {
        _item = item;
        _qty = qty;
        _price = price;
    }

    public float GetPrice()
    {
        return _price;
    }

    public Item GetItem()
    {
        return _item;
    }

    public int GetQty()
    {
        return _qty;
    }
}