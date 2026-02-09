// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_ThrowProjectilesAround
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Player")]
[Name("Throw projectiles around", 0)]
public class Action_ThrowProjectilesAround : WGOBehaviourAction
{
  public BBParameter<bool> look_at_player = new BBParameter<bool>(false);
  public BBParameter<bool> interruptable = new BBParameter<bool>(true);
  public BBParameter<int> projectiles_count = new BBParameter<int>(6);
  public BBParameter<global::Direction> direction = new BBParameter<global::Direction>(global::Direction.ToPlayer);
  public BBParameter<Action_ThrowProjectilesAround.ShotType> shot_type = new BBParameter<Action_ThrowProjectilesAround.ShotType>(Action_ThrowProjectilesAround.ShotType.Around);
  public BBParameter<float> fan_shot_angle = new BBParameter<float>(10f);
  public BBParameter<bool> do_round = new BBParameter<bool>(false);

  public override string info => $"Throw {this.projectiles_count?.ToString()} projectiles around";

  public override void OnExecute()
  {
    Debug.Log((object) $"#AI# Executing \"Throw projectiles around\" {{took_at_player={this.look_at_player.value.ToString()}; interruptable={this.interruptable.value.ToString()};projectiles_count={this.projectiles_count.value.ToString()}}}\n on WGO \"{this.self_wgo.name}\"", (UnityEngine.Object) this.self_wgo);
    if (this.projectiles_count.value < 2)
    {
      Debug.LogError((object) $"[{this.self_wgo.name}] projectiles_count < 2");
      this.EndAction(false);
    }
    else
    {
      if (this.look_at_player.value)
        this.self_ch.LookAt(MainGame.me.player);
      List<Vector2> dirs = new List<Vector2>();
      Vector2 pos = this.self_wgo.pos;
      switch (this.shot_type.value)
      {
        case Action_ThrowProjectilesAround.ShotType.Around:
          switch (this.direction.value)
          {
            case global::Direction.None:
              Debug.LogError((object) "Can not Throw Projectiles Around: direction is None");
              break;
            case global::Direction.Right:
            case global::Direction.Up:
            case global::Direction.Left:
            case global::Direction.Down:
              dirs.Add(this.direction.value.ToVec());
              break;
            case global::Direction.IgnoreDirection:
              dirs.Add(this.self_ch.direction.normalized);
              break;
            case global::Direction.ToPlayer:
              dirs.Add(pos.DirTo(MainGame.me.player.pos).normalized);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          if (this.do_round.value)
          {
            Vector2 vector2_1 = dirs[0];
            Vector2 vector2_2 = (double) Mathf.Abs(vector2_1.x) > (double) Mathf.Abs(vector2_1.y) ? new Vector2((double) vector2_1.x < 0.0 ? -1f : 1f, 0.0f) : new Vector2(0.0f, (double) vector2_1.y < 0.0 ? -1f : 1f);
            dirs[0] = vector2_2;
          }
          float num1 = Mathf.Atan2(dirs[0].y, dirs[0].x);
          float num2 = 6.28318548f / (float) this.projectiles_count.value;
          for (int index = 1; index < this.projectiles_count.value; ++index)
          {
            float f = num1 + num2 * (float) index;
            Vector2 vector2 = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
            dirs.Add(vector2);
          }
          break;
        case Action_ThrowProjectilesAround.ShotType.Fan:
          float num3 = this.fan_shot_angle.value * ((float) Math.PI / 180f);
          Vector2 vector2_3 = pos.DirTo(MainGame.me.player.pos).normalized;
          if (this.do_round.value)
            vector2_3 = (double) Mathf.Abs(vector2_3.x) > (double) Mathf.Abs(vector2_3.y) ? new Vector2((double) vector2_3.x < 0.0 ? -1f : 1f, 0.0f) : new Vector2(0.0f, (double) vector2_3.y < 0.0 ? -1f : 1f);
          if (this.projectiles_count.value % 2 == 1)
          {
            dirs.Add(vector2_3);
          }
          else
          {
            float f = Mathf.Atan2(vector2_3.y, vector2_3.x) - num3 / 2f;
            dirs.Add(new Vector2(Mathf.Cos(f), Mathf.Sin(f)));
          }
          for (int index = 1; index < this.projectiles_count.value; ++index)
          {
            bool flag = index % 2 == 0;
            float f = Mathf.Atan2(dirs[index - 1].y, dirs[index - 1].x) + num3 * (flag ? (float) -index : (float) index);
            dirs.Add(new Vector2(Mathf.Cos(f), Mathf.Sin(f)));
          }
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.projectile_emitter.DoShots(dirs, new ProjectileEmitter.ShootingEnded(this.OnPerformed));
      this.self_ch.ChangeDamageFlagIgnoring(!this.interruptable.value);
    }
  }

  public void OnPerformed(bool success)
  {
    if (!this.isRunning)
      return;
    Debug.Log((object) $"#AI# OnPerformed \"Throw projectiles around\" {{took_at_player={this.look_at_player.value.ToString()}; interruptable={this.interruptable.value.ToString()};projectiles_count={this.projectiles_count.value.ToString()}}}\n on WGO \"{this.self_wgo.name}\"", (UnityEngine.Object) this.self_wgo);
    this.EndAction(success);
  }

  public override void OnStop()
  {
    base.OnStop();
    this.self_ch.ChangeDamageFlagIgnoring(false);
  }

  public enum ShotType
  {
    Around,
    Fan,
  }
}
