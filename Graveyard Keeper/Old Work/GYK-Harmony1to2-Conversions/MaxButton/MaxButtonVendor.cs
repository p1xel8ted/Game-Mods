using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using Rewired;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaxButton
{
    public static class MaxButtonVendor
    {
        public static string MaxButtonName = "Max";
        private static MethodInfo _methodSliderSetValue;
        private static string _caller = string.Empty;
        private static bool _fromPlayer;
        private static int _maximumQuantity = 1;
        private static Trading _trading;
        private static ItemCountGUI.PriceCalculateDelegate _priceCalculator;

        public static void SetCaller(
          string Caller,
          bool FromPlayer,
          int MaximumQuantity,
          Item Item,
          Trading Trading)
        {
            _caller = Caller;
            _fromPlayer = FromPlayer;
            _maximumQuantity = MaximumQuantity;
            _trading = Trading;
        }

        private static void SetPriceCalculator(
          ItemCountGUI.PriceCalculateDelegate PriceCalculateDelegate)
        {
            _priceCalculator = PriceCalculateDelegate;
        }

        public static void AddMaxButton(
          ref ItemCountGUI ItemCountGUI,
          ItemCountGUI.PriceCalculateDelegate PriceCalculateDelegate)
        {
            SetPriceCalculator(PriceCalculateDelegate);
            var transform1 = ItemCountGUI.transform.Find("window/dialog buttons/buttons table/button 1");
            var transform2 = ItemCountGUI.transform.Find("window/dialog buttons/buttons table/button max");
            if (LazyInput.gamepad_active)
            {
                if (!(transform2 != null))
                    return;
                transform2.gameObject.SetActive(false);
            }
            else if (!_caller.Equals("VendorGUI"))
            {
                if (!(transform2 != null))
                    return;
                transform2.gameObject.SetActive(false);
            }
            else if (transform2 != null)
            {
                transform2.gameObject.SetActive(true);
            }
            else
            {
                var gameObject = Object.Instantiate(transform1.gameObject, transform1.parent);
                gameObject.name = "button max";
                Object.Destroy(gameObject.GetComponent<DialogButtonGUI>());
                gameObject.GetComponent<UILabel>().text = MaxButtonName;
                var uiButton = gameObject.AddComponent<UIButton>();
                gameObject.AddComponent<BoxCollider2D>().size = new Vector2(70f, 20f);
                var slider = ItemCountGUI.transform.Find("window/Container/smart slider").GetComponent<SmartSlider>();
                uiButton.onClick = new List<EventDelegate>();
                EventDelegate.Add(uiButton.onClick, () => SetMaxPrice(ref slider));
            }
        }

        private static void SetMaxPrice(ref SmartSlider Slider)
        {
            var val1 = 0;
            if (!_caller.Equals("VendorGUI"))
                return;
            var num1 = (!_fromPlayer ? _trading.player_money : _trading.trader.cur_money) + _trading.GetTotalBalance();
            for (var amount = 1; amount <= _maximumQuantity; ++amount)
            {
                var num2 = _priceCalculator(amount);
                if (num1 >= (double)num2)
                    ++val1;
                else
                    break;
            }
            var quantity = Math.Max(val1, 1);
            SetSliderValue(ref Slider, quantity);
        }

        private static void SetSliderValue(ref SmartSlider Slider, int Quantity)
        {
            if (_methodSliderSetValue == null)
                _methodSliderSetValue = typeof(SmartSlider).GetMethod("SetValue", AccessTools.all);
            _methodSliderSetValue?.Invoke(Slider, new object[2]
            {
        Quantity,
        true
            });
        }
    }
}