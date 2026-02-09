// Decompiled with JetBrains decompiler
// Type: EnemyModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "COTL/Enemy Modifier")]
public class EnemyModifier : ScriptableObject
{
  public Sprite ModifierIconSprite;
  public GameObject ModifierIcon;
  public EnemyModifier.ModifierType Modifier;
  public bool HasTimer;
  public float HealthMultiplier = 1.5f;
  [Space]
  [Range(0.0f, 1f)]
  public float Probability = 0.5f;
  [Space]
  public Color ColorTint = new Color(1f, 1f, 1f, 1f);
  public float Scale = 1f;
  public static float ChanceOfModifier = 0.035f;
  public static EnemyModifier[] modifiers;
  public static bool ForceModifiers = false;

  public static EnemyModifier GetModifier(float increaseChance = 0.0f)
  {
    if (DataManager.Instance != null && DataManager.Instance.dungeonRun <= 3 && (bool) (Object) BiomeGenerator.Instance && !BiomeGenerator.Instance.TestStartingLayer && !EnemyModifier.ForceModifiers)
      return (EnemyModifier) null;
    float num1 = Random.Range(0.0f, 1f - increaseChance);
    if ((double) EnemyModifier.ChanceOfModifier * (double) DataManager.Instance.EnemyModifiersChanceMultiplier >= (double) num1)
    {
      if (EnemyModifier.modifiers == null)
        EnemyModifier.modifiers = Resources.LoadAll<EnemyModifier>("Data/Enemy Modifiers");
      for (int index = 0; index < 100; ++index)
      {
        EnemyModifier modifier = EnemyModifier.modifiers[Random.Range(0, EnemyModifier.modifiers.Length)];
        float num2 = Random.Range(0.0f, 1f);
        if ((double) modifier.Probability >= (double) num2)
          return modifier;
      }
    }
    return (EnemyModifier) null;
  }

  public static EnemyModifier GetModifier(EnemyModifier.ModifierType modifierType)
  {
    if (EnemyModifier.modifiers == null)
      EnemyModifier.modifiers = Resources.LoadAll<EnemyModifier>("Data/Enemy Modifiers");
    foreach (EnemyModifier modifier in EnemyModifier.modifiers)
    {
      if (modifier.Modifier == modifierType)
        return modifier;
    }
    return (EnemyModifier) null;
  }

  public static EnemyModifier GetModifierExcluding(List<EnemyModifier.ModifierType> excludingTypes)
  {
    if (EnemyModifier.modifiers == null)
      EnemyModifier.modifiers = Resources.LoadAll<EnemyModifier>("Data/Enemy Modifiers");
    List<EnemyModifier> enemyModifierList = new List<EnemyModifier>();
    foreach (EnemyModifier modifier in EnemyModifier.modifiers)
    {
      if (!excludingTypes.Contains(modifier.Modifier))
        enemyModifierList.Add(modifier);
    }
    return enemyModifierList.Count > 0 ? enemyModifierList[Random.Range(0, enemyModifierList.Count)] : (EnemyModifier) null;
  }

  public enum ModifierType
  {
    DropPoison,
    DropProjectiles,
    DropBomb,
  }
}
