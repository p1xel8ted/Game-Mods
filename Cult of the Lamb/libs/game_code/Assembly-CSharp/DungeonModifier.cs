// Decompiled with JetBrains decompiler
// Type: DungeonModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "COTL/Dungeon Modifier")]
public class DungeonModifier : ScriptableObject
{
  public Sprite modifierIcon;
  public DungeonPositiveModifier PositiveModifier;
  public DungeonNegativeModifier NegativeModifier;
  public DungeonNeutralModifier NeutralModifier;
  [Space]
  [Range(0.0f, 1f)]
  public float Probability = 0.5f;
  public static float ChanceOfModifier = 0.3f;
  public static DungeonModifier currentModifier;
  public static DungeonModifier[] dungeonModifiers;

  public bool noNeutralModifier => this.NeutralModifier == DungeonNeutralModifier.None;

  public static DungeonModifier GetModifier(float increaseChance = 0.0f)
  {
    if ((double) increaseChance == -1.0 || !DataManager.Instance.EnabledSpells)
      return (DungeonModifier) null;
    if (DungeonSandboxManager.Active)
      increaseChance += 0.3f;
    float num1 = Random.Range(0.0f, 1f - increaseChance);
    if ((double) DungeonModifier.ChanceOfModifier >= (double) num1)
    {
      if (DungeonModifier.dungeonModifiers == null)
        DungeonModifier.dungeonModifiers = Resources.LoadAll<DungeonModifier>("Data/Dungeon Modifiers");
      int num2 = 0;
      while (num2 < 100)
      {
        DungeonModifier dungeonModifier = DungeonModifier.dungeonModifiers[Random.Range(0, DungeonModifier.dungeonModifiers.Length)];
        if (dungeonModifier.NeutralModifier == DungeonNeutralModifier.LoseRedGainTarot && (!DataManager.Instance.CanFindTarotCards || PlayerFleeceManager.FleecePreventTarotCards()))
          ++num2;
        else if (dungeonModifier.NeutralModifier == DungeonNeutralModifier.ChestsDropFoodNotGold && DungeonSandboxManager.Active)
          ++num2;
        else if ((dungeonModifier.NeutralModifier == DungeonNeutralModifier.LoseRedGainTarot || dungeonModifier.NeutralModifier == DungeonNeutralModifier.LoseRedGainBlackHeart) && PlayerFleeceManager.FleeceNoRedHeartsToUse())
          ++num2;
        else if (dungeonModifier.PositiveModifier == DungeonPositiveModifier.DoubleGold && DungeonSandboxManager.Active)
          ++num2;
        else if (dungeonModifier.NeutralModifier == DungeonNeutralModifier.LoseRedGainTarot && DungeonSandboxManager.Active && DungeonSandboxManager.CurrentScenario.ScenarioType == DungeonSandboxManager.ScenarioType.BossRushMode)
        {
          ++num2;
        }
        else
        {
          float num3 = Random.Range(0.0f, 1f);
          if ((double) dungeonModifier.Probability >= (double) num3)
            return dungeonModifier;
          ++num2;
        }
      }
    }
    return (DungeonModifier) null;
  }

  public static void SetActiveModifier(DungeonModifier modifier)
  {
    DungeonModifier.currentModifier = modifier;
    if ((bool) (Object) DungeonModifier.currentModifier)
    {
      Debug.Log((object) $"MODIFIER SET {DungeonModifier.currentModifier.PositiveModifier}-Positive    {DungeonModifier.currentModifier.NegativeModifier}-Negative,    {DungeonModifier.currentModifier.NeutralModifier}-Neutral");
    }
    else
    {
      if (!((Object) HUD_Manager.Instance != (Object) null))
        return;
      HUD_Manager.Instance.CurrentDungeonModifierText.text = "";
      HUD_Manager.Instance.CurrentDungeonModifierText.gameObject.SetActive(false);
    }
  }

  public static bool HasModifierActive()
  {
    return (Object) DungeonModifier.currentModifier != (Object) null;
  }

  public static bool HasPositiveModifier(DungeonPositiveModifier positiveModifier)
  {
    return (Object) DungeonModifier.currentModifier != (Object) null && DungeonModifier.currentModifier.NeutralModifier == DungeonNeutralModifier.None && DungeonModifier.currentModifier.PositiveModifier == positiveModifier;
  }

  public static bool HasNegativeModifier(DungeonNegativeModifier negativeModifier)
  {
    return (Object) DungeonModifier.currentModifier != (Object) null && DungeonModifier.currentModifier.NeutralModifier == DungeonNeutralModifier.None && DungeonModifier.currentModifier.NegativeModifier == negativeModifier;
  }

  public static bool HasNeutralModifier(DungeonNeutralModifier neutralModifier)
  {
    return (Object) DungeonModifier.currentModifier != (Object) null && DungeonModifier.currentModifier.NeutralModifier == neutralModifier;
  }

  public static float HasPositiveModifier(
    DungeonPositiveModifier positiveModifier,
    float trueResult,
    float falseResult)
  {
    return !((Object) DungeonModifier.currentModifier != (Object) null) || DungeonModifier.currentModifier.NeutralModifier != DungeonNeutralModifier.None || DungeonModifier.currentModifier.PositiveModifier != positiveModifier ? falseResult : trueResult;
  }

  public static float HasNegativeModifier(
    DungeonNegativeModifier negativeModifier,
    float trueResult,
    float falseResult)
  {
    return !((Object) DungeonModifier.currentModifier != (Object) null) || DungeonModifier.currentModifier.NeutralModifier != DungeonNeutralModifier.None || DungeonModifier.currentModifier.NegativeModifier != negativeModifier ? falseResult : trueResult;
  }

  public static float HasNeutralModifier(
    DungeonNeutralModifier neutralModifier,
    float trueResult,
    float falseResult)
  {
    return !((Object) DungeonModifier.currentModifier != (Object) null) || DungeonModifier.currentModifier.NeutralModifier != neutralModifier ? falseResult : trueResult;
  }

  public static string GetCurrentModifierText()
  {
    if ((Object) DungeonModifier.currentModifier == (Object) null)
      return "";
    string currentModifierText = "";
    if (DungeonModifier.currentModifier.PositiveModifier != DungeonPositiveModifier.None)
      currentModifierText = $"{currentModifierText}{LocalizationManager.GetTranslation("UI/DungeonModifier/Positive/" + DungeonModifier.currentModifier.PositiveModifier.ToString())}<br>";
    if (DungeonModifier.currentModifier.NegativeModifier != DungeonNegativeModifier.None)
      currentModifierText = $"{currentModifierText}{LocalizationManager.GetTranslation("UI/DungeonModifier/Negative/" + DungeonModifier.currentModifier.NegativeModifier.ToString())}<br>";
    if (DungeonModifier.currentModifier.NeutralModifier != DungeonNeutralModifier.None)
      currentModifierText = $"{currentModifierText}{LocalizationManager.GetTranslation("UI/DungeonModifier/Neutral/" + DungeonModifier.currentModifier.NeutralModifier.ToString())}<br>";
    return currentModifierText;
  }
}
