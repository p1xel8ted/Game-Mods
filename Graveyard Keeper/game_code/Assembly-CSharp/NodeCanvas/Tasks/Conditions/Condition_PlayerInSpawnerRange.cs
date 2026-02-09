// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Condition_PlayerInSpawnerRange
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("Player")]
[Name("Player In Spawner Range", 0)]
public class Condition_PlayerInSpawnerRange : WGOBehaviourCondition
{
  public BBParameter<float> range = new BBParameter<float>(1f);

  public override string info => "Player In Spawner Range " + this.range?.ToString();

  public override bool OnCheck()
  {
    if (this.self_ch == null)
    {
      Debug.LogError((object) $"Character of {this.self_wgo.name} is null!");
      return false;
    }
    if ((Object) this.self_ch.spawner == (Object) null)
    {
      Debug.LogError((object) $"Spawner of {this.self_wgo.name} is null!");
      return false;
    }
    return !this.player_wgo.is_dead && this.player_wgo.IsInRange(this.self_ch.spawner.gameObject, this.range.value);
  }
}
