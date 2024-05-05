using flanne;
using HarmonyLib;

namespace LagLess;

[Harmony]
public class SummonLayersPatch
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(Summon),nameof(Summon.Start))]
	private static void Summon_Start(ref Summon __instance)
	{
			var summonTypeID = __instance.SummonTypeID;
			var text = summonTypeID;
			var text2 = text;
			if (!(text2 == "MagicLens"))
			{
				if (!(text2 == "Knife"))
				{
					if (!(text2 == "Scythe"))
					{
						if (text2 == "Spirit")
						{
							__instance.gameObject.layer = LLLayers.bulletLayer;
						}
					}
					else
					{
						__instance.gameObject.layer = LLLayers.bulletLayer;
					}
				}
				else
				{
					__instance.gameObject.layer = LLLayers.bulletLayer;
				}
			}
			else
			{
				__instance.gameObject.layer = LLLayers.summonCollideOnlyBullet;
			}
		}

}