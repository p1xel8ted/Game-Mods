// Decompiled with JetBrains decompiler
// Type: EnemyModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private static EnemyModifier[] modifiers;
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
