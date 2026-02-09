// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_AnimateAndMove
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Animate and move", 0)]
[Category("Player")]
public class Action_AnimateAndMove : WGOBehaviourAction
{
  public BBParameter<CharAnimState> animation = new BBParameter<CharAnimState>(CharAnimState.Idle);
  public BBParameter<Action_AnimateAndMove.Direction> direction = new BBParameter<Action_AnimateAndMove.Direction>(Action_AnimateAndMove.Direction.None);
  public BBParameter<float> dist = new BBParameter<float>(1f);
  public BBParameter<float> min_dist = new BBParameter<float>(0.0f);
  public BBParameter<float> time = new BBParameter<float>(0.0f);
  public BBParameter<bool> use_astar = new BBParameter<bool>(true);
  public BBParameter<bool> interruptable = new BBParameter<bool>(true);
  public BBParameter<bool> look_at_player = new BBParameter<bool>(false);
  public bool finding_path;

  public override string info
  {
    get
    {
      string info = $"Animate {this.animation?.ToString()} and\nmove {this.direction?.ToString()}";
      if (this.animation.value == CharAnimState.Attack)
        info += "\n(For Attack use \"Perform attack\" block)";
      return info;
    }
  }

  public override void OnExecute()
  {
    this.self_ch.ChangeDamageFlagIgnoring(!this.interruptable.value);
    if (this.self_ch.anim_state == this.animation.value && this.self_ch.movement_state == MovementComponent.MovementState.AnimCurve)
    {
      this.EndAction(true);
    }
    else
    {
      if (this.look_at_player.value)
        this.self_ch.LookAt(MainGame.me.player);
      base.OnExecute();
      switch (this.direction.value)
      {
        case Action_AnimateAndMove.Direction.None:
          this.ProcessDir(Vector2.zero);
          break;
        case Action_AnimateAndMove.Direction.ToPlayer:
          Vector2 dir = this.self_wgo.tf.DirTo(this.player_wgo.tf);
          float current_dist = dir.magnitude;
          if ((double) current_dist < (double) this.min_dist.value)
          {
            this.EndAction(true);
            break;
          }
          bool flag = false;
          if (this.use_astar.value)
          {
            Physics2D.LinecastAll(this.self_wgo.pos, this.player_wgo.pos, 1);
            flag = false;
          }
          if ((double) this.dist.value + (double) this.min_dist.value > (double) current_dist)
          {
            if (this.use_astar.value & flag)
            {
              this.JumpToPlayerByAstar((GJCommons.VoidDelegate) (() => this.ProcessDir(dir, current_dist - this.min_dist.value)));
              break;
            }
            this.ProcessDir(dir, current_dist - this.min_dist.value);
            break;
          }
          if (this.use_astar.value & flag)
          {
            this.JumpToPlayerByAstar((GJCommons.VoidDelegate) (() => this.ProcessDir(dir)));
            break;
          }
          this.ProcessDir(dir);
          break;
        case Action_AnimateAndMove.Direction.FromPlayer:
          this.ProcessDir(this.player_wgo.tf.DirTo(this.self_wgo.tf));
          break;
        case Action_AnimateAndMove.Direction.Random:
          this.ProcessDir(Random.insideUnitCircle);
          break;
        case Action_AnimateAndMove.Direction.ToAnchor:
          if (!((Object) this.self_wgo != (Object) null) || this.self_wgo.components == null || !this.self_wgo.components.character.enabled || !((Object) this.self_wgo.components.character.anchor_obj != (Object) null))
            break;
          this.ProcessDir(this.self_wgo.tf.DirTo(this.self_wgo.components.character.anchor_obj.transform));
          break;
      }
    }
  }

  public void JumpToPlayerByAstar(GJCommons.VoidDelegate on_failed)
  {
    this.finding_path = true;
    this.self_ch.astar.Find(this.player_wgo.pos, (GJCommons.VoidDelegate) (() =>
    {
      if (!this.finding_path)
        return;
      this.ProcessAstar(on_failed);
    }), (GJCommons.VoidDelegate) (() =>
    {
      if (!this.finding_path)
        return;
      on_failed.TryInvoke();
    }));
  }

  public void ProcessAstar(GJCommons.VoidDelegate on_failed)
  {
    this.ProcessDir((Vector2) ((this.self_ch.cur_astar_path[1] - (Vector3) this.self_wgo.pos) / 96f));
  }

  public void ProcessDir(Vector2 dir, float dist = 0.0f)
  {
    if (dist.EqualsTo(0.0f))
      dist = this.dist.value;
    this.self_ch.SetAnimationState(this.animation.value);
    if ((double) this.time.value > 0.0)
      this.self_wgo.components.timer.Play(this.time.value);
    this.self_ch.CurveMove(dir, this.self_wgo.GetWOP().GetCurve(this.animation.value), dist, (GJCommons.VoidDelegate) (() =>
    {
      if (!this.isRunning)
        return;
      if (this.self_ch.anim_state == this.animation.value)
        this.self_ch.SetAnimationState(CharAnimState.Idle);
      this.EndAction(true);
    }), this.time.value.EqualsTo(0.0f), force_state_change: true, is_dir_change: true);
  }

  public override void OnStop()
  {
    base.OnStop();
    if (this.self_ch.anim_state == this.animation.value)
      this.self_ch.SetAnimationState(CharAnimState.Idle);
    this.self_ch.ChangeDamageFlagIgnoring(false);
    this.finding_path = false;
  }

  [CompilerGenerated]
  public void \u003CProcessDir\u003Eb__15_0()
  {
    if (!this.isRunning)
      return;
    if (this.self_ch.anim_state == this.animation.value)
      this.self_ch.SetAnimationState(CharAnimState.Idle);
    this.EndAction(true);
  }

  public enum Direction
  {
    None,
    ToPlayer,
    FromPlayer,
    Random,
    ToAnchor,
  }
}
