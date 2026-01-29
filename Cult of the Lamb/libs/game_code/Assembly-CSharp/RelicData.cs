// Decompiled with JetBrains decompiler
// Type: RelicData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using src.UINavigator;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Relic Data")]
public class RelicData : ScriptableObject
{
  public RelicType RelicType;
  public RelicInteractionType InteractionType;
  public RelicSubType RelicSubType;
  public RelicDamageType DamageType;
  public float DamageRequiredToCharge = 30f;
  [Space]
  public Sprite UISprite;
  public Sprite UISpriteOutline;
  public bool HasCleansedVersion;
  public Sprite CleansedSprite;
  public Sprite CleansedSpriteOutline;
  [Space]
  public float Weight = 1f;
  public bool RequiresCult;
  public bool ShowAnimationAbovePlayer = true;
  public UpgradeSystem.Type UpgradeType = UpgradeSystem.Type.Count;
  public VFXAbilitySequenceData VFXData;
  public Material Material;

  public Sprite Sprite
  {
    get
    {
      if (this.HasCleansedVersion)
      {
        switch (this.RelicType)
        {
          case RelicType.IncreaseDamageForDuration:
            return !DataManager.Instance.HeketHealed ? this.UISprite : this.CleansedSprite;
          case RelicType.GungeonBlank:
            return !DataManager.Instance.ShamuraHealed ? this.UISprite : this.CleansedSprite;
          case RelicType.SpawnCombatFollower:
            return !DataManager.Instance.KallamarHealed ? this.UISprite : this.CleansedSprite;
          case RelicType.DamageOnTouch_Familiar:
            return !DataManager.Instance.LeshyHealed ? this.UISprite : this.CleansedSprite;
        }
      }
      return this.UISprite;
    }
  }

  public Sprite SpriteOutline
  {
    get
    {
      if (this.HasCleansedVersion)
      {
        switch (this.RelicType)
        {
          case RelicType.IncreaseDamageForDuration:
            return !DataManager.Instance.HeketHealed ? this.UISpriteOutline : this.CleansedSpriteOutline;
          case RelicType.GungeonBlank:
            return !DataManager.Instance.KallamarHealed ? this.UISpriteOutline : this.CleansedSpriteOutline;
          case RelicType.SpawnCombatFollower:
            return !DataManager.Instance.ShamuraHealed ? this.UISpriteOutline : this.CleansedSpriteOutline;
          case RelicType.DamageOnTouch_Familiar:
            return !DataManager.Instance.LeshyHealed ? this.UISpriteOutline : this.CleansedSpriteOutline;
        }
      }
      return this.UISpriteOutline;
    }
  }

  public static string GetTitleLocalisation(RelicType relicType)
  {
    string str = "";
    if (relicType == RelicType.DamageOnTouch_Familiar && DataManager.Instance.LeshyHealed || relicType == RelicType.IncreaseDamageForDuration && DataManager.Instance.HeketHealed || relicType == RelicType.SpawnCombatFollower && DataManager.Instance.KallamarHealed || relicType == RelicType.GungeonBlank && DataManager.Instance.ShamuraHealed)
      str = "/Cleansed";
    return LocalizationManager.GetTermTranslation($"Relics/{relicType}{str}");
  }

  public static string GetDescriptionLocalisation(RelicType relicType)
  {
    string input = LocalizationManager.GetTermTranslation($"Relics/{relicType}/Description");
    switch (relicType)
    {
      case RelicType.DealDamagePerFollower:
      case RelicType.HealPerFollower:
        input = $"{input}<br>{string.Format(LocalizationManager.GetTranslation("UI/Followers"), (object) LocalizeIntegration.ReverseText(DataManager.Instance.Followers.Count.ToString()))}";
        break;
      case RelicType.DealDamagePerFollower_Blessed:
      case RelicType.HealPerFollower_Blessed:
        int num1 = 0;
        for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
        {
          if (DataManager.Instance.Followers[index] != null && DataManager.Instance.Followers[index].CursedState == Thought.OldAge)
            ++num1;
        }
        input = $"{input}<br>{string.Format(LocalizationManager.GetTranslation("UI/ElderlyFollowers"), (object) LocalizeIntegration.ReverseText(num1.ToString()))}";
        break;
      case RelicType.DealDamagePerFollower_Dammed:
      case RelicType.HealPerFollower_Dammed:
        int num2 = 0;
        for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
        {
          if (DataManager.Instance.Followers[index] != null && DataManager.Instance.Followers[index].CursedState == Thought.Dissenter)
            ++num2;
        }
        input = $"{input}<br>{string.Format(LocalizationManager.GetTranslation("UI/DissentingFollowers"), (object) LocalizeIntegration.ReverseText(num2.ToString()))}";
        break;
      case RelicType.FrozenGhosts:
        int num3 = 0;
        for (int index = 0; index < DataManager.Instance.Followers_Dead.Count; ++index)
        {
          if (DataManager.Instance.Followers_Dead[index] != null && DataManager.Instance.Followers_Dead[index].FrozeToDeath)
            ++num3;
        }
        input = $"{input}<br>{string.Format(LocalizationManager.GetTranslation("UI/FrozenFollowers"), (object) LocalizeIntegration.ReverseText(num3.ToString()))}";
        break;
    }
    if (EquipmentManager.GetRelicData(relicType).RelicSubType == RelicSubType.Corrupted)
    {
      string str1 = LocalizationManager.GetTermTranslation($"Relics/{relicType}/Positive");
      string str2 = LocalizationManager.GetTermTranslation($"Relics/{relicType}/Negative");
      string format = LocalizationManager.GetTermTranslation($"Relics/{relicType}/Description");
      if (TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      {
        str2 = $"<s>{str2}</s>";
        format = $"{format}<br><color=#FFD201>{TarotCards.LocalisedName(TarotCards.Card.NoCorruption)}";
      }
      else if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      {
        str1 = $"<s>{str1}</s>";
        format = $"{format}<br><color=#FFD201>{TarotCards.LocalisedName(TarotCards.Card.CorruptedFullCorruption)}";
      }
      input = string.Format(format, (object) str1, (object) str2);
    }
    if (EquipmentManager.GetRelicData(relicType).DamageType == RelicDamageType.DealsDamage && TrinketManager.HasTrinket(TarotCards.Card.CorruptedRelicCharge, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      input = $"{input}<br><color=#FFD201>{LocalizationManager.GetTranslation("UI/DamageIncreased")} | {TarotCards.LocalisedName(TarotCards.Card.CorruptedRelicCharge)}";
    if (relicType == RelicType.DamageOnTouch_Familiar && DataManager.Instance.LeshyHealed || relicType == RelicType.IncreaseDamageForDuration && DataManager.Instance.HeketHealed || relicType == RelicType.SpawnCombatFollower && DataManager.Instance.KallamarHealed || relicType == RelicType.GungeonBlank && DataManager.Instance.ShamuraHealed)
      input = LocalizationManager.GetTermTranslation($"Relics/{relicType}/Cleansed/Description");
    return LocalizationManager.CurrentLanguage == "French (Canadian)" || LocalizationManager.CurrentLanguage == "Italian" ? Regex.Replace(input, "<sprite name=\"(.*?)\">", "<size=70%><sprite name=\"$1\"><size=100%>") : input;
  }

  public static string GetLoreLocalization(RelicType relicType)
  {
    return LocalizationManager.GetTermTranslation($"Relics/{relicType}/Lore");
  }

  public static RelicChargeCategory GetChargeCategory(RelicType relicType)
  {
    return RelicData.GetChargeCategory(EquipmentManager.GetRelicData(relicType));
  }

  public static RelicChargeCategory GetChargeCategory(RelicData relicData)
  {
    float requiredToCharge = relicData.DamageRequiredToCharge;
    if ((double) requiredToCharge < 50.0)
      return RelicChargeCategory.Fast;
    return (double) requiredToCharge < 80.0 ? RelicChargeCategory.Average : RelicChargeCategory.Slow;
  }

  public static bool GetRelicDLC(RelicType relicType)
  {
    switch (relicType)
    {
      case RelicType.IgniteOnTouch_Familiar:
      case RelicType.IgniteAll:
      case RelicType.FireballsRain:
      case RelicType.FieryBlood:
      case RelicType.FieryBurrow:
      case RelicType.FrozenGhosts:
      case RelicType.IceyBlood:
      case RelicType.IceyBurrow:
      case RelicType.IceyCoat:
      case RelicType.IceSpikes:
        return true;
      default:
        return false;
    }
  }

  public static string GetChargeCategoryColor(RelicChargeCategory category)
  {
    switch (category)
    {
      case RelicChargeCategory.Fast:
        return "<color=#00FFC2>";
      case RelicChargeCategory.Average:
        return "<color=#FF8C2F>";
      case RelicChargeCategory.Slow:
        return "<color=#FD1D03>";
      default:
        return "<color=#00FFC2>";
    }
  }
}
