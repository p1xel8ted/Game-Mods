// Decompiled with JetBrains decompiler
// Type: SimpleAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class SimpleAnimator : BaseMonoBehaviour
{
  public bool reverseFacing;
  private float xScale = 1f;
  private float yScale = 1f;
  private float xScaleSpeed;
  private float yScaleSpeed;
  private bool flipX;
  private float moveSquish;
  private float moveSquishDelta;
  private SpriteRenderer spriterenderer;
  public Sprite IdleFrame;
  public Sprite AttackFrame;
  public Sprite PostAttackFrame;
  public Sprite PreAttackFrame;
  public Sprite DefendingFrame;
  public Sprite DodgingFrame;
  public Sprite FleeingFrame;
  public Sprite CustomAction0Frame;
  public Sprite RaiseAlarmFrame;
  public Sprite WorshippingFrame;
  public Sprite SleepingFrame;
  public Sprite InactiveFrame;
  public Sprite CustomAnimationFrame;
  private StateMachine state;
  private StateMachine.State prevState;
  public bool LookAtCamera;
  private Vector3 prevPosition;

  private void Start()
  {
    this.state = this.gameObject.GetComponentInParent<StateMachine>();
    this.spriterenderer = this.GetComponent<SpriteRenderer>();
    this.prevPosition = this.gameObject.transform.position;
  }

  private void Update()
  {
    if (this.LookAtCamera)
      this.transform.rotation = Quaternion.Euler(Utils.GetAngle(this.transform.position, Camera.main.transform.position) + 90f, 0.0f, 0.0f);
    if ((Object) this.state != (Object) null)
    {
      if (this.prevState != this.state.CURRENT_STATE)
      {
        this.transform.localPosition = Vector3.zero;
        this.transform.eulerAngles = new Vector3(-45f, 0.0f, 0.0f);
        switch (this.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            this.moveSquish = 0.0f;
            this.spriterenderer.sprite = this.IdleFrame;
            break;
          case StateMachine.State.Moving:
            this.SetScale(1.2f, 0.8f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.IdleFrame;
            break;
          case StateMachine.State.Attacking:
            this.moveSquish = 0.0f;
            this.spriterenderer.sprite = this.AttackFrame;
            break;
          case StateMachine.State.Defending:
            this.SetScale(2f, 0.6f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.DefendingFrame;
            break;
          case StateMachine.State.SignPostAttack:
            this.SetScale(0.7f, 2f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.PreAttackFrame;
            break;
          case StateMachine.State.RecoverFromAttack:
            this.SetScale(2f, 0.6f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.PostAttackFrame;
            break;
          case StateMachine.State.Dodging:
            this.transform.localPosition = new Vector3(0.0f, 0.0f, -0.2f);
            this.SetScale(2f, 0.6f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.DodgingFrame;
            break;
          case StateMachine.State.Fleeing:
            this.SetScale(0.8f, 1.2f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.FleeingFrame;
            break;
          case StateMachine.State.CustomAction0:
            this.SetScale(0.8f, 1.2f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.CustomAction0Frame;
            break;
          case StateMachine.State.InActive:
            this.SetScale(0.8f, 1.2f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.InactiveFrame;
            break;
          case StateMachine.State.RaiseAlarm:
            this.SetScale(0.8f, 1.2f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.RaiseAlarmFrame;
            break;
          case StateMachine.State.Worshipping:
            this.SetScale(0.8f, 1.2f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.WorshippingFrame;
            break;
          case StateMachine.State.Sleeping:
            this.SetScale(0.8f, 1.2f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.SleepingFrame;
            break;
          case StateMachine.State.CustomAnimation:
            this.SetScale(0.8f, 1.2f);
            this.moveSquishDelta = 0.0f;
            this.spriterenderer.sprite = this.CustomAnimationFrame;
            break;
        }
      }
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Moving:
          this.moveSquish = 0.05f * Mathf.Cos(this.moveSquishDelta += 0.4f);
          break;
        case StateMachine.State.Dodging:
          this.transform.Rotate(new Vector3(0.0f, 0.0f, (float) (20 * (this.flipX ? 1 : -1))));
          break;
        case StateMachine.State.Fleeing:
          this.moveSquish = 0.05f * Mathf.Cos(this.moveSquishDelta += 0.4f);
          break;
        case StateMachine.State.Sleeping:
          this.moveSquish = 0.1f * Mathf.Cos(this.moveSquishDelta += 0.01f);
          break;
      }
    }
    this.prevState = this.state.CURRENT_STATE;
    this.setFacing(((double) this.state.facingAngle >= 90.0 || (double) this.state.facingAngle <= -90.0 ? 1 : -1) * (this.reverseFacing ? -1 : 1));
    this.xScaleSpeed += (float) ((1.0 + (double) this.moveSquish - (double) this.xScale) * 0.20000000298023224);
    this.xScale += (this.xScaleSpeed *= 0.8f);
    this.yScaleSpeed += (float) ((1.0 - (double) this.moveSquish - (double) this.yScale) * 0.20000000298023224);
    this.yScale += (this.yScaleSpeed *= 0.8f);
    this.gameObject.transform.localScale = new Vector3(this.xScale * (this.flipX ? -1f : 1f), this.yScale, 1f);
    if (!Input.GetMouseButtonDown(1))
      return;
    this.SetScale(2f, 0.5f);
  }

  public void setFacing(int dir)
  {
    if (dir == 1 == this.flipX)
      return;
    this.flipX = dir == 1;
    this.SetScale(1.2f, 0.8f);
  }

  public void SetScale(float _xScale, float _yScale)
  {
    this.xScale = _xScale;
    this.yScale = _yScale;
  }
}
