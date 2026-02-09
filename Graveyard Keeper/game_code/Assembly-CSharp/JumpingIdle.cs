// Decompiled with JetBrains decompiler
// Type: JumpingIdle
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class JumpingIdle : BaseCharacterIdle
{
  [Range(0.0f, 1f)]
  [Space]
  public float at_place_probability = 0.2f;
  public float jump_dist = 0.5f;

  public override void StartIdle()
  {
    if (this._state != BaseCharacterIdle.IdleState.None)
      return;
    this.ch = this.wgo.components.character;
    this.start_pos = this.wgo.pos;
    this.Wait(this.ch.anim_state != CharAnimState.Jump && this.ch.movement_state != MovementComponent.MovementState.AnimCurve);
  }

  public override void MoveToRandomPos()
  {
    if ((double) Random.value < (double) this.at_place_probability)
    {
      this.ProcessDir(Vector2.zero);
    }
    else
    {
      Vector2 nextDest = this.GetNextDest();
      if ((double) nextDest.magnitude > 0.0)
        this.ProcessDir(this.wgo.pos.DirTo(nextDest).normalized);
      else
        this.Wait(true);
    }
  }

  public override void Wait(bool stop = true)
  {
    base.Wait(stop);
    this.ch.SetAnimationState(CharAnimState.Idle);
  }

  public void ProcessDir(Vector2 dir)
  {
    this.ch.SetAnimationState(CharAnimState.Jump);
    this.ch.CurveMove(dir, this.wgo.wop.GetCurve(CharAnimState.Jump), this.jump_dist, new GJCommons.VoidDelegate(((BaseCharacterIdle) this).ChangeState));
    this._state = BaseCharacterIdle.IdleState.Moving;
  }
}
