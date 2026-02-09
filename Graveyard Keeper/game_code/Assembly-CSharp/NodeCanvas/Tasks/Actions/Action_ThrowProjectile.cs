// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_ThrowProjectile
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
[Name("Throw projectile", 0)]
public class Action_ThrowProjectile : WGOBehaviourAction
{
  public BBParameter<bool> look_at_player = new BBParameter<bool>(false);
  public BBParameter<bool> return_success_result;
  public BBParameter<bool> interruptable = new BBParameter<bool>(true);
  public BBParameter<bool> round_dir = new BBParameter<bool>(false);
  public BBParameter<global::Direction> direction = new BBParameter<global::Direction>(global::Direction.ToPlayer);

  public override string info => "Throw projectile ";

  public override void OnExecute()
  {
    if (this.look_at_player.value)
      this.self_ch.LookAt(MainGame.me.player);
    switch (this.direction.value)
    {
      case global::Direction.None:
        Debug.LogError((object) $"Wrong Throw projectile direction on {this.self_wgo.name}: dir={this.direction.value.ToString()}");
        break;
      case global::Direction.Right:
      case global::Direction.Up:
      case global::Direction.Left:
      case global::Direction.Down:
        this.projectile_emitter.DoShot(this.direction.value.ToVec(), new ProjectileEmitter.ShootingEnded(this.OnPerformed));
        break;
      case global::Direction.IgnoreDirection:
        this.projectile_emitter.DoShot(this.self_ch.direction.normalized, new ProjectileEmitter.ShootingEnded(this.OnPerformed));
        break;
      case global::Direction.ToPlayer:
        if (this.round_dir.value)
        {
          Vector2 normalized = this.self_wgo.pos.DirTo(MainGame.me.player.pos).normalized;
          this.projectile_emitter.DoShot((double) Mathf.Abs(normalized.x) > (double) Mathf.Abs(normalized.y) ? new Vector2((double) normalized.x < 0.0 ? -1f : 1f, 0.0f) : new Vector2(0.0f, (double) normalized.y < 0.0 ? -1f : 1f), new ProjectileEmitter.ShootingEnded(this.OnPerformed));
          break;
        }
        this.projectile_emitter.DoShot(MainGame.me.player.tf, new ProjectileEmitter.ShootingEnded(this.OnPerformed));
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
