// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Condition_WasDamaged
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("Player")]
[Name("Was Damaged", 0)]
public class Condition_WasDamaged : WGOBehaviourCondition
{
  public override bool OnCheck() => this.self_ch.WasDamaged(false);
}
