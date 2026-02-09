// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_PerformAttack
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Player")]
[Name("Perform Attack", 0)]
public class Action_PerformAttack : WGOBehaviourAction
{
  public BBParameter<int> type = new BBParameter<int>(0);
  public BBParameter<bool> look_at_player = new BBParameter<bool>(false);
  public BBParameter<bool> return_success_result;
  public BBParameter<bool> interruptable = new BBParameter<bool>(true);
  public BBParameter<global::Direction> direction = new BBParameter<global::Direction>(global::Direction.ToPlayer);

  public override string info => "Perform attack " + this.type?.ToString();

  public override void OnExecute()
  {
    if (this.look_at_player.value)
      this.self_ch.LookAt(MainGame.me.player);
    switch (this.direction.value)
    {
      case global::Direction.None:
        Debug.LogError((object) $"Wrong perform attack direction on {this.self_wgo.name}: dir={this.direction.value.ToString()}");
        break;
      case global::Direction.Right:
      case global::Direction.Up:
      case global::Direction.Left:
      case global::Direction.Down:
      case global::Direction.IgnoreDirection:
        this.self_ch.components.character.attack.Perform(this.direction.value, this.type.value, new BaseCharacterAttack.AttackResult(this.OnPerformed));
        break;
      case global::Direction.ToPlayer:
        this.self_ch.components.character.attack.Perform(MainGame.me.player, this.type.value, new BaseCharacterAttack.AttackResult(this.OnPerformed));
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this.self_ch.ChangeDamageFlagIgnoring(!this.interruptable.value);
  }

  public void OnPerformed(bool success)
  {
    if (!this.isRunning)
      return;
    this.EndAction(!this.return_success_result.value | success);
  }

  public override void OnStop()
  {
    base.OnStop();
    this.self_ch.ChangeDamageFlagIgnoring(false);
  }
}
