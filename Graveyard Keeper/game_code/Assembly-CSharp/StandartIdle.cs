// Decompiled with JetBrains decompiler
// Type: StandartIdle
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StandartIdle : BaseCharacterIdle
{
  [Space]
  public float speed = 1f;
  public float max_moving_range = 1f;

  public override void MoveToRandomPos()
  {
    Vector2 dest = this.GetNextDest();
    if (this.ch == null || (Object) this.ch.wgo == (Object) null)
      return;
    Vector2 vector2 = this.ch.wgo.pos - dest;
    if ((double) vector2.magnitude > 96.0 * (double) this.max_moving_range)
    {
      vector2 *= 96f * this.max_moving_range / vector2.magnitude;
      dest = this.ch.wgo.pos - vector2;
    }
    this._state = BaseCharacterIdle.IdleState.Moving;
    this.ch.GoTo(dest, on_complete: new GJCommons.VoidDelegate(((BaseCharacterIdle) this).ChangeState), on_failed: new GJCommons.VoidDelegate(((BaseCharacterIdle) this).ChangeState));
    this.ch.SetSpeed(this.speed);
  }
}
