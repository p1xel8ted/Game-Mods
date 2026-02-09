// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Condition_PlayerInRange
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Name("Player In Range", 0)]
[Category("Player")]
public class Condition_PlayerInRange : WGOBehaviourCondition
{
  public BBParameter<float> range = new BBParameter<float>(1f);

  public override string info => "Player in range " + this.range?.ToString();

  public override bool OnCheck()
  {
    return this.self_wgo.IsInRange(this.player_wgo, this.range.value) && !this.player_wgo.is_dead;
  }
}
