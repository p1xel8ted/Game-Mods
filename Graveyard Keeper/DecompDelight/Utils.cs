namespace DecompDelight;

public static class Utils
{
    internal static void AddToTooltip(List<BubbleWidgetData> __result, ElementMaps.Element element)
    {
        var color = GetTextColorForElement(element);
        var name = LangDicts.GetElementName(element);
        __result.Insert(5, new BubbleWidgetTextData($"{color}{name}[-][/c]", BubbleWidgetTextData.Font.TinyFont, NGUIText.Alignment.Center, -1));
    }

    internal static string GetDecomposeOutput(string itemId)
    {
        var craftData = GameBalance.me.craft_data
            .Where(cd => cd.craft_type == CraftDefinition.CraftType.AlchemyDecompose && cd.needs.Count > 0 && cd.needs[0].id.Equals(itemId))
            .SelectMany(cd => cd.output)
            .Select(output => output.id.Split('_'))
            .Select(split => split.Any(s => s.Equals("d", StringComparison.OrdinalIgnoreCase)) ? $"d_{split.Last()}" : split.Last())
            .Distinct()
            .ToList();

        return craftData.FirstOrDefault();
    }

    internal static ElementMaps.Element GetElementForItem(string decomposeOutput)
    {
        return ElementMaps.ElementMappings.TryGetValue(decomposeOutput, out var element) ? element : ElementMaps.Element.None;
    }

    private static string GetTextColorForElement(ElementMaps.Element element)
    {
        return element switch
        {
            ElementMaps.Element.Slowing => $"[c][{Plugin.SlowingColorHex}]",
            ElementMaps.Element.Acceleration => $"[c][{Plugin.AccelerationColorHex}]",
            ElementMaps.Element.Health => $"[c][{Plugin.HealthColorHex}]",
            ElementMaps.Element.Death => $"[c][{Plugin.DeathColorHex}]",
            ElementMaps.Element.Order => $"[c][{Plugin.OrderColorHex}]",
            ElementMaps.Element.Toxic => $"[c][{Plugin.ToxicColorHex}]",
            ElementMaps.Element.Chaos => $"[c][{Plugin.ChaosColorHex}]",
            ElementMaps.Element.Life => $"[c][{Plugin.LifeColorHex}]",
            ElementMaps.Element.Electric => $"[c][{Plugin.ElectricColorHex}]",
            ElementMaps.Element.Silver => $"[c][{Plugin.SilverColorHex}]",
            ElementMaps.Element.White => $"[c][{Plugin.WhiteColorHex}]",
            ElementMaps.Element.Water => $"[c][{Plugin.WaterColorHex}]",
            ElementMaps.Element.Oil => $"[c][{Plugin.OilColorHex}]",
            ElementMaps.Element.Blood => $"[c][{Plugin.BloodColorHex}]",
            ElementMaps.Element.Salt => $"[c][{Plugin.SaltColorHex}]",
            ElementMaps.Element.Ash => $"[c][{Plugin.AshColorHex}]",
            ElementMaps.Element.Alcohol => $"[c][{Plugin.AlcoholColorHex}]",
            _ => string.Empty
        };
    }

    internal static string ColorToHex(Color color)
    {
        var r = Mathf.RoundToInt(color.r * 255);
        var g = Mathf.RoundToInt(color.g * 255);
        var b = Mathf.RoundToInt(color.b * 255);
        return $"{r:X2}{g:X2}{b:X2}";
    }

    internal static bool SurveyCompleted(ItemDefinition itemDefinition, bool fullDetail)
    {
        var surveyCraft = itemDefinition.GetSurveyCraft();
        return fullDetail && surveyCraft != null && MainGame.me.save.completed_one_time_crafts.Contains(surveyCraft.id);
    }
}