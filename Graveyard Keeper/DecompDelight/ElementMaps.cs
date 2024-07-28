namespace DecompDelight;

internal static class ElementMaps
{
    internal readonly static Dictionary<string, Element> ItemElementCache = new();

    internal readonly static Dictionary<string, Element> ElementMappings = new()
    {
        {"brown", Element.Slowing},
        {"d_blue", Element.Acceleration},
        {"gold", Element.Acceleration},
        {"silver", Element.Silver},
        {"graphite", Element.Acceleration},
        {"d_green", Element.Health},
        {"d_violet", Element.Death},
        {"green", Element.Order},
        {"red", Element.Toxic},
        {"violet", Element.Chaos},
        {"yellow", Element.Life},
        {"electro", Element.Electric},
        {"white", Element.White},
        {"water", Element.Water},
        {"oil", Element.Oil},
        {"alchohol", Element.Alcohol},
        {"alcohol", Element.Alcohol},
        {"blood", Element.Blood},
        {"salt", Element.Salt},
        {"ash", Element.Ash},
    };
    
    internal enum Element
    {
        None,
        Slowing,
        Acceleration,
        Health,
        Death,
        Order,
        Toxic,
        Chaos,
        Life,
        Electric,
        Silver,
        White,
        Water,
        Oil,
        Blood,
        Salt,
        Ash,
        Alcohol
    }
}