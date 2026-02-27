// Decompiled with JetBrains decompiler
// Type: SimpleSpineAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class SimpleSpineAnimator : BaseMonoBehaviour
{
  public int AnimationTrack;
  private StateMachine state;
  private SkeletonAnimation _anim;
  public bool AutomaticallySetFacing = true;
  public AnimationReferenceAsset DefaultLoop;
  public AnimationReferenceAsset StartMoving;
  public AnimationReferenceAsset StopMoving;
  public AnimationReferenceAsset TurnAnimation;
  public AnimationReferenceAsset NorthMoving;
  public AnimationReferenceAsset NorthDiagonalMoving;
  public AnimationReferenceAsset SouthMoving;
  public AnimationReferenceAsset SouthDiagonalMoving;
  public AnimationReferenceAsset NorthIdle;
  public AnimationReferenceAsset Dodging;
  public AnimationReferenceAsset NorthDodging;
  public AnimationReferenceAsset SouthDodging;
  public AnimationReferenceAsset Aiming;
  public List<SimpleSpineAnimator.SpineChartacterAnimationData> Animations = new List<SimpleSpineAnimator.SpineChartacterAnimationData>();
  public AnimationReferenceAsset Idle;
  public AnimationReferenceAsset IdleWithItem;
  public AnimationReferenceAsset MovingWithItem;
  public AnimationReferenceAsset Moving;
  public AnimationReferenceAsset Inactive;
  public AnimationReferenceAsset Action;
  public AnimationReferenceAsset Sleeping;
  public AnimationReferenceAsset PreAttack;
  public AnimationReferenceAsset PostAttack;
  public AnimationReferenceAsset Defending;
  public AnimationReferenceAsset HitLeft;
  public AnimationReferenceAsset HitRight;
  public AnimationReferenceAsset Dodge;
  public SimpleInventory inventory;
  private TrackEntry Track;
  private StateMachine.State cs;
  private TrackEntry t;
  public bool isFillWhite;
  public bool UpdateAnimsOnStateChange = true;
  private MaterialPropertyBlock BlockWhite;
  private MeshRenderer meshRenderer;
  private int fillAlpha;
  private int fillColor;
  private Color WarningColour = new Color(0.7490196f, 0.6862745f, 0.1372549f, 1f);
  private bool FlashingRed;
  private Coroutine cFlashFillRoutine;
  private float FlashRedMultiplier = 0.01f;
  private float xScaleSpeed;
  private float yScaleSpeed;
  private float moveSquish;
  private float xScale;
  private float yScale;
  private bool flipX;
  public bool ReverseFacing;
  public bool StartOnDefault = true;
  private int _Dir;
  public bool ForceDirectionalMovement;

  private SkeletonAnimation anim
  {
    get
    {
      if ((UnityEngine.Object) this._anim == (UnityEngine.Object) null)
        this._anim = this.GetComponent<SkeletonAnimation>();
      return this._anim;
    }
  }

  public bool IsVisible
  {
    get => (UnityEngine.Object) this.meshRenderer == (UnityEngine.Object) null || this.meshRenderer.isVisible;
  }

  public void Initialize(bool overwrite) => this.anim.Initialize(overwrite);

  public SimpleSpineAnimator.SpineChartacterAnimationData GetAnimationData(StateMachine.State State)
  {
    foreach (SimpleSpineAnimator.SpineChartacterAnimationData animation in this.Animations)
    {
      if (animation.State == State)
        return animation;
    }
    return (SimpleSpineAnimator.SpineChartacterAnimationData) null;
  }

  private StateMachine.State CurrentState
  {
    get => this.cs;
    set
    {
      if (this.cs == value)
        return;
      this.cs = value;
      this.UpdateAnimFromState();
    }
  }

  private void UpdateAnimFromState()
  {
    if ((UnityEngine.Object) this.anim == (UnityEngine.Object) null || !this.UpdateAnimsOnStateChange)
      return;
    this.cs = this.state.CURRENT_STATE;
    if (this.Animations.Count > 0)
    {
      foreach (SimpleSpineAnimator.SpineChartacterAnimationData animation in this.Animations)
      {
        if (animation.State == this.cs)
        {
          if (!((UnityEngine.Object) animation.Animation != (UnityEngine.Object) null))
            return;
          if (animation.State == StateMachine.State.Idle)
          {
            if ((UnityEngine.Object) this.StopMoving != (UnityEngine.Object) null)
            {
              this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.StopMoving, false);
              this.Track = this.anim.AnimationState.AddAnimation(this.AnimationTrack, (Spine.Animation) animation.Animation, true, 0.0f);
              return;
            }
            this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) animation.Animation, true);
            return;
          }
          if (animation.State == StateMachine.State.Moving)
          {
            if ((UnityEngine.Object) this.StartMoving != (UnityEngine.Object) null)
            {
              this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.StartMoving, false);
              this.Track = this.anim.AnimationState.AddAnimation(this.AnimationTrack, (Spine.Animation) animation.Animation, true, 0.0f);
              return;
            }
            this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) animation.Animation, animation.Looping);
            return;
          }
          if ((UnityEngine.Object) animation.AddAnimation == (UnityEngine.Object) null)
          {
            this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) animation.Animation, animation.Looping);
            if (!animation.DisableMixDuration)
              return;
            this.Track.MixDuration = 0.0f;
            return;
          }
          this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) animation.Animation, false);
          if (animation.DisableMixDuration)
            this.Track.MixDuration = 0.0f;
          this.anim.AnimationState.AddAnimation(this.AnimationTrack, (Spine.Animation) animation.AddAnimation, animation.Looping, 0.0f);
          return;
        }
      }
      this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.DefaultLoop, true);
    }
    else
    {
      switch (this.cs)
      {
        case StateMachine.State.Idle:
          if ((UnityEngine.Object) this.inventory != (UnityEngine.Object) null && this.inventory.Item != InventoryItem.ITEM_TYPE.NONE && (UnityEngine.Object) this.IdleWithItem != (UnityEngine.Object) null)
          {
            this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.IdleWithItem, true);
            break;
          }
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Idle, true);
          break;
        case StateMachine.State.Moving:
          if ((UnityEngine.Object) this.inventory != (UnityEngine.Object) null && this.inventory.Item != InventoryItem.ITEM_TYPE.NONE && (UnityEngine.Object) this.MovingWithItem != (UnityEngine.Object) null)
          {
            this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.MovingWithItem, true);
            break;
          }
          if ((UnityEngine.Object) this.StartMoving != (UnityEngine.Object) null)
          {
            this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.StartMoving, true);
            this.anim.AnimationState.AddAnimation(this.AnimationTrack, (Spine.Animation) this.Moving, true, 0.0f);
            break;
          }
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Moving, true);
          break;
        case StateMachine.State.Defending:
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Defending, true);
          break;
        case StateMachine.State.SignPostAttack:
          if (!((UnityEngine.Object) this.PreAttack != (UnityEngine.Object) null))
            break;
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.PreAttack, false);
          break;
        case StateMachine.State.RecoverFromAttack:
          if (!((UnityEngine.Object) this.PostAttack != (UnityEngine.Object) null))
            break;
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.PostAttack, false);
          break;
        case StateMachine.State.Dodging:
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Dodge, true);
          break;
        case StateMachine.State.CustomAction0:
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Action, true);
          break;
        case StateMachine.State.TimedAction:
          break;
        case StateMachine.State.Sleeping:
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Sleeping, true);
          break;
        case StateMachine.State.BeingCarried:
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Idle, true);
          break;
        case StateMachine.State.HitLeft:
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.HitLeft, true);
          break;
        case StateMachine.State.HitRight:
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.HitRight, true);
          break;
        case StateMachine.State.Aiming:
          break;
        default:
          if (!((UnityEngine.Object) this.Idle != (UnityEngine.Object) null) || this.anim.AnimationState == null)
            break;
          this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Idle, true);
          break;
      }
    }
  }

  public string CurrentAnimation() => this.anim.AnimationName;

  public TrackEntry Animate(string Animation, int Track, bool Loop)
  {
    return this.anim.AnimationState.Data.SkeletonData.FindAnimation(Animation) != null ? this.anim.AnimationState.SetAnimation(Track, Animation, Loop) : (TrackEntry) null;
  }

  public TrackEntry Animate(string Animation, int Track, bool Loop, float MixTime)
  {
    if (this.anim.AnimationState.Data.SkeletonData.FindAnimation(Animation) == null)
      return (TrackEntry) null;
    this.t = this.anim.AnimationState.SetAnimation(Track, Animation, Loop);
    this.t.MixTime = MixTime;
    return this.t;
  }

  public void AddAnimate(string Animation, int Track, bool Loop, float Delay)
  {
    this.anim.AnimationState.AddAnimation(Track, Animation, Loop, Delay);
  }

  public void ClearTrackAfterAnimation(int Track)
  {
    this.anim.AnimationState.AddEmptyAnimation(Track, 0.1f, 0.0f);
  }

  public void SetAttachment(string slotName, string attachmentName)
  {
    this.anim.skeleton.SetAttachment(slotName, attachmentName);
  }

  public void SetSkin(string Skin)
  {
    this.anim.skeleton.SetSkin(Skin);
    this.anim.skeleton.SetSlotsToSetupPose();
  }

  public void SetColor(Color color) => this.anim.skeleton.SetColor(color);

  public void FlashMeWhite()
  {
    if (Time.frameCount % 5 != 0)
      return;
    this.FlashWhite(!this.isFillWhite);
  }

  public void FlashWhite(bool toggle)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || (UnityEngine.Object) this.meshRenderer == (UnityEngine.Object) null)
      return;
    if (this.BlockWhite == null)
      this.BlockWhite = new MaterialPropertyBlock();
    this.BlockWhite.SetColor(this.fillColor, this.WarningColour);
    this.BlockWhite.SetFloat(this.fillAlpha, toggle ? 0.33f : 0.0f);
    this.meshRenderer.SetPropertyBlock(this.BlockWhite);
    this.isFillWhite = toggle;
  }

  public void FillWhite(bool toggle)
  {
    if ((UnityEngine.Object) this.meshRenderer == (UnityEngine.Object) null)
      return;
    if (this.BlockWhite == null)
      this.BlockWhite = new MaterialPropertyBlock();
    this.BlockWhite.SetColor(this.fillColor, Color.white);
    this.BlockWhite.SetFloat(this.fillAlpha, toggle ? 1f : 0.0f);
    this.meshRenderer.SetPropertyBlock(this.BlockWhite);
    this.isFillWhite = toggle;
  }

  public void FillColor(Color color, float Alpha = 1f)
  {
    if ((UnityEngine.Object) this.meshRenderer == (UnityEngine.Object) null)
      return;
    if (this.BlockWhite == null)
      this.BlockWhite = new MaterialPropertyBlock();
    this.BlockWhite.SetColor(this.fillColor, color);
    this.BlockWhite.SetFloat(this.fillAlpha, Alpha);
    this.meshRenderer.SetPropertyBlock(this.BlockWhite);
  }

  public void FlashFillRed(float opacity = 0.5f)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.FlashingRed = true;
    if (this.cFlashFillRoutine != null)
      this.StopCoroutine(this.cFlashFillRoutine);
    this.cFlashFillRoutine = this.StartCoroutine((IEnumerator) this.FlashOnHitRoutine(opacity));
  }

  private IEnumerator FlashOnHitRoutine(float opacity)
  {
    MaterialPropertyBlock properties = new MaterialPropertyBlock();
    this.meshRenderer.receiveShadows = false;
    this.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
    this.meshRenderer.SetPropertyBlock(properties);
    this.SetColor(new Color(1f, 1f, 1f, opacity));
    yield return (object) new WaitForSeconds(6f * this.FlashRedMultiplier);
    this.SetColor(new Color(0.0f, 0.0f, 0.0f, opacity));
    yield return (object) new WaitForSeconds(3f * this.FlashRedMultiplier);
    this.SetColor(new Color(1f, 0.0f, 0.0f, opacity));
    yield return (object) new WaitForSeconds(2f * this.FlashRedMultiplier);
    this.SetColor(new Color(0.0f, 0.0f, 0.0f, opacity));
    yield return (object) new WaitForSeconds(2f * this.FlashRedMultiplier);
    this.SetColor(new Color(1f, 0.0f, 0.0f, opacity));
    yield return (object) new WaitForSeconds(2f * this.FlashRedMultiplier);
    this.SetColor(new Color(1f, 1f, 1f, 1f));
    this.FlashingRed = false;
    this.meshRenderer.receiveShadows = true;
    this.meshRenderer.shadowCastingMode = ShadowCastingMode.On;
  }

  private IEnumerator DoFlashFillRed()
  {
    if (!((UnityEngine.Object) this.meshRenderer == (UnityEngine.Object) null))
    {
      MaterialPropertyBlock block = new MaterialPropertyBlock();
      this.meshRenderer.SetPropertyBlock(block);
      block.SetColor(this.fillColor, Color.red);
      block.SetFloat(this.fillAlpha, 1f);
      this.meshRenderer.SetPropertyBlock(block);
      yield return (object) new WaitForSeconds(0.1f);
      float Progress = 1f;
      while ((double) (Progress -= 0.05f) >= 0.0)
      {
        if ((double) Progress <= 0.0)
          Progress = 0.0f;
        block.SetFloat(this.fillAlpha, Progress);
        this.meshRenderer.SetPropertyBlock(block);
        yield return (object) null;
      }
    }
  }

  public void FlashRedTint()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.StopCoroutine((IEnumerator) this.DoFlashTintRed());
    this.StartCoroutine((IEnumerator) this.DoFlashTintRed());
  }

  private IEnumerator DoFlashTintRed()
  {
    float Progress = 0.0f;
    while ((double) (Progress += 0.05f) <= 1.0)
    {
      this.SetColor(Color.Lerp(Color.red, Color.white, Progress));
      yield return (object) null;
    }
    this.SetColor(Color.white);
  }

  public void FlashFillBlack()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.StopCoroutine((IEnumerator) this.DoFlashFillBlack());
    this.StartCoroutine((IEnumerator) this.DoFlashFillBlack());
  }

  private IEnumerator DoFlashFillBlack()
  {
    MaterialPropertyBlock block = new MaterialPropertyBlock();
    this.meshRenderer.SetPropertyBlock(block);
    this.SetColor(Color.black);
    yield return (object) new WaitForSeconds(0.1f);
    this.SetColor(Color.white);
    float Progress = 1f;
    while ((double) Progress > 0.0)
    {
      Progress -= Time.deltaTime;
      block.SetFloat(this.fillAlpha, Progress);
      this.meshRenderer.SetPropertyBlock(block);
      yield return (object) null;
    }
  }

  public void FlashFillGreen()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.StopCoroutine((IEnumerator) this.DoFlashFillGreen());
    this.StartCoroutine((IEnumerator) this.DoFlashFillGreen());
  }

  private IEnumerator DoFlashFillGreen()
  {
    MaterialPropertyBlock block = new MaterialPropertyBlock();
    this.meshRenderer.SetPropertyBlock(block);
    this.SetColor(Color.green);
    yield return (object) new WaitForSeconds(0.1f);
    this.SetColor(Color.white);
    float Progress = 1f;
    while ((double) Progress > 0.0)
    {
      Progress -= Time.deltaTime;
      block.SetFloat(this.fillAlpha, Progress);
      this.meshRenderer.SetPropertyBlock(block);
      yield return (object) null;
    }
  }

  public void ChangeStateAnimation(StateMachine.State state, string NewAnimation)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = this.GetAnimationData(state);
    AnimationReferenceAsset instance = ScriptableObject.CreateInstance<AnimationReferenceAsset>();
    instance.Animation = this.anim.skeleton.Data.FindAnimation(NewAnimation);
    instance.name = NewAnimation;
    AnimationReferenceAsset animationReferenceAsset = instance;
    animationData.Animation = animationReferenceAsset;
    if (this.CurrentState != state || this.state.CURRENT_STATE != state)
      return;
    this.UpdateAnimFromState();
  }

  public AnimationReferenceAsset GetAnimationReference(string AnimationName)
  {
    AnimationReferenceAsset animationReference = new AnimationReferenceAsset();
    animationReference.Animation = this.anim.skeleton.Data.FindAnimation(AnimationName);
    animationReference.name = AnimationName;
    return animationReference;
  }

  public void ResetAnimationsToDefaults()
  {
    foreach (SimpleSpineAnimator.SpineChartacterAnimationData animation in this.Animations)
      animation.Animation = animation.DefaultAnimation;
    this.UpdateAnimFromState();
  }

  public float Duration()
  {
    return this.anim.AnimationState.GetCurrent(this.AnimationTrack).Animation.Duration;
  }

  private void Awake()
  {
    this.state = this.GetComponentInParent<StateMachine>();
    this.UpdateIdleAndMoving();
    if (this.StartOnDefault && (UnityEngine.Object) this.anim != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.DefaultLoop == (UnityEngine.Object) null && (UnityEngine.Object) this.Idle == (UnityEngine.Object) null)
        return;
      if (this.anim.AnimationState.Data.SkeletonData.FindAnimation((UnityEngine.Object) this.DefaultLoop != (UnityEngine.Object) null ? this.DefaultLoop.Animation.Name : this.Idle.Animation.Name) != null)
        this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) ((UnityEngine.Object) this.DefaultLoop != (UnityEngine.Object) null ? this.DefaultLoop : this.Idle), true);
    }
    else
      this.Track = this.anim.AnimationState.GetCurrent(0);
    this.Dir = 1;
    this.meshRenderer = this.GetComponent<MeshRenderer>();
    this.fillColor = Shader.PropertyToID("_FillColor");
    this.fillAlpha = Shader.PropertyToID("_FillAlpha");
  }

  private void OnEnable()
  {
    this.anim.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.SpineEventHandler);
  }

  private void OnDisable()
  {
    this.anim.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.SpineEventHandler);
  }

  public void SetDefault(StateMachine.State State, string Animation)
  {
    foreach (SimpleSpineAnimator.SpineChartacterAnimationData animation in this.Animations)
    {
      if (animation.State == State)
      {
        animation.Animation = this.GetAnimationReference(Animation);
        animation.DefaultAnimation = this.GetAnimationReference(Animation);
      }
    }
  }

  public void UpdateIdleAndMoving()
  {
    foreach (SimpleSpineAnimator.SpineChartacterAnimationData animation in this.Animations)
    {
      animation.InitDefaults();
      if (animation.State == StateMachine.State.Moving)
        this.Moving = animation.Animation;
      if (animation.State == StateMachine.State.Idle)
        this.Idle = animation.Animation;
    }
  }

  private void OnDestroy()
  {
    this.anim.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.SpineEventHandler);
  }

  public event SimpleSpineAnimator.SpineEvent OnSpineEvent;

  private void SpineEventHandler(TrackEntry trackEntry, Spine.Event e)
  {
    if (this.OnSpineEvent == null)
      return;
    this.OnSpineEvent(e.Data.Name);
  }

  public int Dir
  {
    get => this._Dir;
    set
    {
      if (this._Dir == value)
        return;
      if ((UnityEngine.Object) this.state != (UnityEngine.Object) null && this.state.CURRENT_STATE == StateMachine.State.Moving && (UnityEngine.Object) this.TurnAnimation != (UnityEngine.Object) null)
      {
        this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.TurnAnimation, true);
        this.anim.AnimationState.AddAnimation(this.AnimationTrack, (Spine.Animation) this.Moving, true, 0.0f);
      }
      this._Dir = value;
      this.anim.skeleton.ScaleX = (float) this.Dir;
    }
  }

  private void Update()
  {
    if (!((UnityEngine.Object) this.state != (UnityEngine.Object) null))
      return;
    this.CurrentState = this.state.CURRENT_STATE;
    if (this.AutomaticallySetFacing)
      this.Dir = ((double) this.state.facingAngle <= 90.0 || (double) this.state.facingAngle >= 270.0 ? -1 : 1) * (this.ReverseFacing ? -1 : 1);
    if ((UnityEngine.Object) this.NorthIdle != (UnityEngine.Object) null && (this.CurrentState == StateMachine.State.InActive || this.CurrentState == StateMachine.State.Idle))
    {
      if ((double) this.state.facingAngle > 40.0 && (double) this.state.facingAngle < 140.0)
      {
        if (this.Track != null && this.Track.Animation != this.NorthIdle.Animation)
          this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.NorthIdle, true);
      }
      else if (this.Track != null && this.Track.Animation != this.Idle.Animation)
        this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Idle, true);
    }
    if ((UnityEngine.Object) this.NorthDodging != (UnityEngine.Object) null && this.CurrentState == StateMachine.State.Dodging)
    {
      if ((double) this.state.facingAngle > 40.0 && (double) this.state.facingAngle < 140.0)
      {
        if (this.Track.Animation != this.NorthDodging.Animation)
          this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.NorthDodging, true);
      }
      else if ((double) this.state.facingAngle > 220.0 && (double) this.state.facingAngle < 320.0)
      {
        if (this.Track.Animation != this.SouthDodging.Animation)
          this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.SouthDodging, true);
      }
      else if (this.Track.Animation != this.Dodging.Animation)
        this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Dodging, true);
    }
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = this.GetAnimationData(StateMachine.State.Moving);
    if (animationData != null && !((UnityEngine.Object) animationData.Animation == (UnityEngine.Object) animationData.DefaultAnimation) && !this.ForceDirectionalMovement)
      return;
    if ((UnityEngine.Object) this.NorthMoving != (UnityEngine.Object) null && this.CurrentState == StateMachine.State.Moving)
    {
      if ((double) this.state.facingAngle > 40.0 && (double) this.state.facingAngle < 140.0)
      {
        if ((UnityEngine.Object) this.NorthDiagonalMoving != (UnityEngine.Object) null)
        {
          if ((double) this.state.facingAngle > 70.0 && (double) this.state.facingAngle < 110.0)
          {
            if (this.Track.Animation != this.NorthMoving.Animation)
              this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.NorthMoving, true);
          }
          else if (this.Track.Animation != this.NorthDiagonalMoving.Animation)
            this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.NorthDiagonalMoving, true);
        }
        else if (this.Track.Animation != this.NorthMoving.Animation)
          this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.NorthMoving, true);
      }
      else if (((UnityEngine.Object) this.SouthMoving == (UnityEngine.Object) null || (UnityEngine.Object) this.SouthMoving != (UnityEngine.Object) null && (double) this.state.facingAngle < 220.0 && (double) this.state.facingAngle > 320.0) && this.Track.Animation != this.Moving.Animation)
        this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Moving, true);
    }
    if (!((UnityEngine.Object) this.SouthMoving != (UnityEngine.Object) null) || this.CurrentState != StateMachine.State.Moving)
      return;
    if ((double) this.state.facingAngle > 220.0 && (double) this.state.facingAngle < 320.0)
    {
      if ((UnityEngine.Object) this.SouthDiagonalMoving != (UnityEngine.Object) null)
      {
        if ((double) this.state.facingAngle > 250.0 && (double) this.state.facingAngle < 290.0)
        {
          if (this.Track.Animation == this.SouthMoving.Animation)
            return;
          this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.SouthMoving, true);
        }
        else
        {
          if (this.Track.Animation == this.SouthDiagonalMoving.Animation)
            return;
          this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.SouthDiagonalMoving, true);
        }
      }
      else
      {
        if (this.Track.Animation == this.SouthMoving.Animation)
          return;
        this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.SouthMoving, true);
      }
    }
    else
    {
      if (!((UnityEngine.Object) this.NorthMoving == (UnityEngine.Object) null) && (!((UnityEngine.Object) this.NorthMoving != (UnityEngine.Object) null) || (double) this.state.facingAngle >= 40.0 && (double) this.state.facingAngle <= 140.0) || this.Track.Animation == this.Moving.Animation)
        return;
      this.Track = this.anim.AnimationState.SetAnimation(this.AnimationTrack, (Spine.Animation) this.Moving, true);
    }
  }

  public delegate void SpineEvent(string EventName);

  [Serializable]
  public class SpineChartacterAnimationData
  {
    public StateMachine.State State;
    [HideInInspector]
    public AnimationReferenceAsset DefaultAnimation;
    public AnimationReferenceAsset Animation;
    public AnimationReferenceAsset AddAnimation;
    public bool DisableMixDuration;
    public bool Looping;

    public void InitDefaults() => this.DefaultAnimation = this.Animation;
  }
}
