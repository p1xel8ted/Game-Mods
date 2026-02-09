// Decompiled with JetBrains decompiler
// Type: SmartCondition_Random
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class SmartCondition_Random : SmartCondition
{
  public float chance = 50f;

  public override bool CheckCondition() => (double) Random.Range(1, 100) < (double) this.chance;

  public override string GetName() => $"Random ({this.chance.ToString()})";
}
