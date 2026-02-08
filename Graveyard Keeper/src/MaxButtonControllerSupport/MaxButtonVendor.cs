namespace MaxButtonControllerSupport;

public static class MaxButtonVendor
{
    private const string MaxButtonName = "Max";
    private static string _caller = string.Empty;
    private static bool _fromPlayer;
    private static int _maximumQuantity = 1;
    private static Trading _trading;
    private static ItemCountGUI.PriceCalculateDelegate _priceCalculator;
    private static Transform _button1Transform;
    private static Transform _buttonMaxTransform;
    private static SmartSlider _slider;

    public static void SetCaller(string caller, bool fromPlayer, int maximumQuantity, Trading trading)
    {
        _caller = caller;
        _fromPlayer = fromPlayer;
        _maximumQuantity = maximumQuantity;
        _trading = trading;
    }

    private static void SetPriceCalculator(ItemCountGUI.PriceCalculateDelegate priceCalculateDelegate)
    {
        _priceCalculator = priceCalculateDelegate;
    }

    public static void AddMaxButton(ItemCountGUI itemCountGUI, ItemCountGUI.PriceCalculateDelegate priceCalculateDelegate)
    {
        SetPriceCalculator(priceCalculateDelegate);
        _button1Transform = itemCountGUI.transform.Find("window/dialog buttons/buttons table/button 1");
        _buttonMaxTransform = itemCountGUI.transform.Find("window/dialog buttons/buttons table/button max");

        var shouldHideButton = LazyInput.gamepad_active || !EqualityComparer<string>.Default.Equals(_caller, Plugin.VendorGui);
        if (shouldHideButton && _buttonMaxTransform != null)
        {
            _buttonMaxTransform.gameObject.SetActive(false);
            return;
        }

        if (_buttonMaxTransform != null)
        {
            _buttonMaxTransform.gameObject.SetActive(true);
            return;
        }

        var gameObject = Object.Instantiate(_button1Transform.gameObject, _button1Transform.parent);
        gameObject.name = "button max";
        Object.Destroy(gameObject.GetComponent<DialogButtonGUI>());
        gameObject.GetComponent<UILabel>().text = MaxButtonName;
        var uiButton = gameObject.AddComponent<UIButton>();
        gameObject.AddComponent<BoxCollider2D>().size = new Vector2(70f, 20f);
        _slider = itemCountGUI.transform.Find("window/Container/smart slider").GetComponent<SmartSlider>();
        uiButton.onClick = new List<EventDelegate>();
        EventDelegate.Add(uiButton.onClick, delegate { SetMaxPrice(_slider); });
    }

    private static void SetMaxPrice(SmartSlider slider)
    {
        var quantity = 0;

        if (!EqualityComparer<string>.Default.Equals(_caller, Plugin.VendorGui))
        {
            return;
        }

        var availableFunds = _fromPlayer ? _trading.trader.cur_money : _trading.player_money;
        availableFunds += _trading.GetTotalBalance();
        var priceForQuantity = _priceCalculator.Invoke(quantity);

        while (availableFunds >= priceForQuantity && quantity <= _maximumQuantity)
        {
            quantity++;
            priceForQuantity = _priceCalculator.Invoke(quantity);
        }

        quantity = Math.Max(quantity, 1);
        SetSliderValue(slider, quantity);
    }


    private static void SetSliderValue(SmartSlider slider, int quantity)
    {
        slider.SetValue(quantity);
    }
}