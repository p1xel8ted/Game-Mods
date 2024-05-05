using System.Globalization;
using System.Linq;
using System.Threading;
using Helper;

namespace TheSeedEqualizer;

public static partial class MainPatcher
{
    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    private static void Log(string message, bool error = false)
    {
        if (error)
            Tools.Log("TheSeedEqualizer", $"{message}", true);
        else if (_cfg.Debug)
            Tools.Log("TheSeedEqualizer", $"{message}");
    }

    private static void ModifyOutput(ObjectDefinition obj)
    {
        foreach (var output in obj.drop_items.Where(a => a.id.Contains("seed")))
        {
            Log($"Initial craft def: {obj.id}");
            string craft;
            if (output.id.EndsWith(":3"))
            {
                craft = output.id.Replace("_seed:3", $"_planting_3");
            }
            else if (output.id.EndsWith(":2"))
            {
                craft = output.id.Replace("_seed:2", $"_planting_2");
            }
            else if (output.id.EndsWith(":1"))
            {
                craft = output.id.Replace("_seed:1", $"_planting_1");
            }
            else
            {
                craft = output.id.Replace("_seed", "_planting_1");
            }

            craft = craft.Replace("hamp", "cannabis");
            craft = $"garden_{craft}";

            Log($"CraftDef for {output.id}: {craft}");


            var craftDef = GameBalance.me.GetDataOrNull<CraftDefinition>(craft);

            if (craftDef != null)
            {
                Log($"Found corresponding craft, setting min_value of {output.id} to {craftDef.needs[0].value}");
                output.min_value = SmartExpression.ParseExpression(craftDef.needs[0].value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                Log($"Did not find corresponding craft, setting min_value of {output.id} to 4.");
                output.min_value = SmartExpression.ParseExpression(4.ToString(CultureInfo.InvariantCulture));
            }

            var normalBoost = output.max_value.EvaluateFloat() + 2;
            var extraBoost = output.max_value.EvaluateFloat() + 4;
            var boost = _cfg.BoostPotentialSeedOutput ? extraBoost : normalBoost;
            output.max_value = SmartExpression.ParseExpression(boost.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void ModifyOutput(CraftDefinition craft)
    {
        foreach (var output in craft.output.Where(a => a.id.Contains("seed")))
        {
            output.min_value = SmartExpression.ParseExpression(craft.needs[0].value.ToString(CultureInfo.InvariantCulture));
            var normalBoost = output.max_value.EvaluateFloat() + 2;
            var extraBoost = output.max_value.EvaluateFloat() + 4;
            var boost = _cfg.BoostPotentialSeedOutput ? extraBoost : normalBoost;
            output.max_value = SmartExpression.ParseExpression(boost.ToString(CultureInfo.InvariantCulture));
        }
    }
}