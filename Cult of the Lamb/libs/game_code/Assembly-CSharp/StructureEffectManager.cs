// Decompiled with JetBrains decompiler
// Type: StructureEffectManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;

#nullable disable
public class StructureEffectManager
{
  public static Action<int, StructureEffectManager.EffectType, StructureEffectManager.State> OnEffectChange;

  public static List<StructureEffect> StructureEffects
  {
    get => DataManager.Instance.StructureEffects;
    set => DataManager.Instance.StructureEffects = value;
  }

  public static string GetLocalizedName(StructureEffectManager.EffectType Type)
  {
    return LocalizationManager.GetTranslation($"Structures/StructureEffect/{Type}");
  }

  public static string GetLocalizedKey(StructureEffectManager.EffectType type)
  {
    return $"Structures/StructureEffect/{type}";
  }

  public static string GetLocalizedDescription(StructureEffectManager.EffectType Type)
  {
    return LocalizationManager.GetTranslation($"Structures/StructureEffect/{Type}/Description");
  }

  public static string GetLocalizedDescriptionKey(StructureEffectManager.EffectType type)
  {
    return $"Structures/StructureEffect/{type}/Description";
  }

  public static void Tick()
  {
    int index = -1;
    while (++index < StructureEffectManager.StructureEffects.Count)
    {
      if (!StructureEffectManager.StructureEffects[index].CoolingDown && (double) TimeManager.TotalElapsedGameTime - (double) StructureEffectManager.StructureEffects[index].TimeStarted >= (double) StructureEffectManager.StructureEffects[index].DurationInGameMinutes)
        StructureEffectManager.StructureEffects[index].BeginCooldown();
      if ((double) TimeManager.TotalElapsedGameTime - (double) StructureEffectManager.StructureEffects[index].TimeStarted >= (double) StructureEffectManager.StructureEffects[index].DurationInGameMinutes + (double) StructureEffectManager.StructureEffects[index].CoolDownInGameMinutes)
      {
        Action<int, StructureEffectManager.EffectType, StructureEffectManager.State> onEffectChange = StructureEffectManager.OnEffectChange;
        if (onEffectChange != null)
          onEffectChange(StructureEffectManager.StructureEffects[index].StructureID, StructureEffectManager.StructureEffects[index].Type, StructureEffectManager.State.Off);
        StructureEffectManager.StructureEffects.RemoveAt(index);
        --index;
      }
    }
  }

  public static StructureEffectManager.State GetEffectAvailability(
    int StructureID,
    StructureEffectManager.EffectType Type)
  {
    foreach (StructureEffect structureEffect in StructureEffectManager.StructureEffects)
    {
      if (structureEffect.StructureID == StructureID && structureEffect.Type == Type)
      {
        if ((double) TimeManager.TotalElapsedGameTime - (double) structureEffect.TimeStarted < (double) structureEffect.DurationInGameMinutes)
          return StructureEffectManager.State.Active;
        if ((double) TimeManager.TotalElapsedGameTime - (double) structureEffect.TimeStarted >= (double) structureEffect.DurationInGameMinutes && (double) TimeManager.TotalElapsedGameTime - (double) structureEffect.TimeStarted < (double) structureEffect.DurationInGameMinutes + (double) structureEffect.CoolDownInGameMinutes)
          return StructureEffectManager.State.Cooldown;
      }
    }
    return StructureEffectManager.State.DoesntExist;
  }

  public static float GetEffectCoolDownProgress(
    int StructureID,
    StructureEffectManager.EffectType Type)
  {
    foreach (StructureEffect structureEffect in StructureEffectManager.StructureEffects)
    {
      if (structureEffect.StructureID == StructureID && structureEffect.Type == Type)
        return structureEffect.CoolDownProgress;
    }
    return 0.0f;
  }

  public static void CreateEffect(int StructureID, StructureEffectManager.EffectType Type)
  {
    StructureEffect structureEffect = new StructureEffect();
    structureEffect.Create(StructureID, Type);
    StructureEffectManager.StructureEffects.Add(structureEffect);
    Action<int, StructureEffectManager.EffectType, StructureEffectManager.State> onEffectChange = StructureEffectManager.OnEffectChange;
    if (onEffectChange == null)
      return;
    onEffectChange(StructureID, Type, StructureEffectManager.State.On);
  }

  public enum EffectType
  {
    Shrine_DevotionEffeciency,
    Shrine_ExtendedShift,
    Farm_ExtendedShift,
    Farm_BloodFertilizer,
    Farm_CorpseFertilizer,
    Cooking_FoodScraps,
    Cooking_BulkierMeals,
    Cooking_Fast,
    Cooking_MindAlteringMeals,
  }

  public enum State
  {
    DoesntExist,
    Active,
    Cooldown,
    On,
    Off,
  }
}
