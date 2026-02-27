// Decompiled with JetBrains decompiler
// Type: EnemyModifierExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class EnemyModifierExtension
{
  public static bool HasModifier(
    this EnemyModifier enemyModifier,
    EnemyModifier.ModifierType modifierType)
  {
    return (Object) enemyModifier != (Object) null && enemyModifier.Modifier == modifierType;
  }
}
