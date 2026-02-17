// Decompiled with JetBrains decompiler
// Type: HUD_Heart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Heart : BaseMonoBehaviour
{
  public Image Circle;
  public RectTransform rectTransform;
  public SkeletonGraphic Spine;
  [SpineSlot("", "", false, true, false, dataField = "Spine")]
  public string Slot;
  [SpineAttachment(true, false, false, "", "", "", true, false, dataField = "Spine")]
  public string Attachment = "";
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string RedHeartSkin = "";
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string BlueHeartSkin = "";
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string BlackHeartSkin = "";
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string FireHeartSkin = "";
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string IceHeartSkin = "";
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string halfSkin = "";
  public float _Shake;
  public float ShakeSpeed;
  public float _Scale;
  public float ScaleSpeed;
  public bool wasFull;
  public bool isEffectActivated;
  public bool isInScaleAnimation;
  public HUD_Heart.HeartType MyHeartType;
  public HUD_Heart.HeartState MyState;
  public bool Shaking;
  public float ShakeTimer;
  public float Timer;

  public void OnDisable()
  {
    if (!this.isInScaleAnimation)
      return;
    this.isInScaleAnimation = false;
    this.Scale = 1f;
  }

  public void Start() => this.rectTransform = this.GetComponent<RectTransform>();

  public float Shake
  {
    get => this._Shake;
    set
    {
      this._Shake = value;
      this.Spine.rectTransform.localPosition = new Vector3(0.0f, this.Shake);
    }
  }

  public float Scale
  {
    get => this._Scale;
    set
    {
      this._Scale = value;
      this.Spine.rectTransform.localScale = Vector3.one * this.Scale;
    }
  }

  public void Activate(bool Active, bool DoEffects)
  {
    if (!Active & DoEffects)
    {
      if (!this.gameObject.activeInHierarchy)
        return;
      this.StartCoroutine((IEnumerator) this.DeactivateWithEffect());
    }
    else
    {
      if (DoEffects || this.isEffectActivated)
        this.ClearCoroutines();
      this.gameObject.SetActive(Active);
    }
  }

  public void ActivateAndScale(float Delay)
  {
    this.gameObject.SetActive(true);
    this.ClearCoroutines();
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine((IEnumerator) this.DoScale(Delay));
  }

  public IEnumerator DeactivateWithEffect()
  {
    HUD_Heart hudHeart = this;
    hudHeart.isEffectActivated = true;
    hudHeart.Spine.AnimationState.SetAnimation(0, "disappear", false);
    yield return (object) hudHeart.StartCoroutine((IEnumerator) hudHeart.DoCircle());
    yield return (object) new WaitForSeconds(0.1f);
    hudHeart.gameObject.SetActive(false);
    hudHeart.isEffectActivated = false;
  }

  public void SetSprite(
    HUD_Heart.HeartState NewState,
    bool DoEffects = false,
    HUD_Heart.HeartType NewHeartType = HUD_Heart.HeartType.Red)
  {
    if ((Object) this.Spine == (Object) null || this.Spine.Skeleton == null)
      return;
    switch (NewHeartType)
    {
      case HUD_Heart.HeartType.Blue:
        this.Spine.Skeleton.SetSkin(this.BlueHeartSkin);
        break;
      case HUD_Heart.HeartType.Spirit:
        this.Spine.Skeleton.SetSkin(this.RedHeartSkin);
        this.Spine.Skeleton.SetAttachment(this.Slot, this.Attachment);
        if (NewState == HUD_Heart.HeartState.HeartHalf && !this.wasFull || NewState == HUD_Heart.HeartState.HeartContainer && !this.wasFull || NewState == HUD_Heart.HeartState.HalfHeartContainer && !this.wasFull)
          this.Spine.Skeleton.SetSkin(this.halfSkin);
        if (NewState == HUD_Heart.HeartState.HeartFull)
        {
          this.wasFull = true;
          break;
        }
        break;
      case HUD_Heart.HeartType.Black:
        this.Spine.Skeleton.SetSkin(this.BlackHeartSkin);
        this.Spine.Skeleton.SetAttachment(this.Slot, (string) null);
        break;
      case HUD_Heart.HeartType.Fire:
        this.Spine.Skeleton.SetSkin(this.FireHeartSkin);
        this.Spine.Skeleton.SetAttachment(this.Slot, (string) null);
        break;
      case HUD_Heart.HeartType.Ice:
        this.Spine.Skeleton.SetSkin(this.IceHeartSkin);
        this.Spine.Skeleton.SetAttachment(this.Slot, (string) null);
        break;
      default:
        if ((Object) this.Spine != (Object) null)
        {
          this.Spine.Skeleton.SetSkin(this.RedHeartSkin);
          this.Spine.Skeleton.SetAttachment(this.Slot, (string) null);
          if (NewState == HUD_Heart.HeartState.HeartHalf || NewState == HUD_Heart.HeartState.HalfHeartContainer)
            this.Spine.Skeleton.SetSkin(this.halfSkin);
          if (NewState == HUD_Heart.HeartState.HeartFull)
          {
            this.wasFull = true;
            break;
          }
          break;
        }
        break;
    }
    switch (NewState - 1)
    {
      case HUD_Heart.HeartState.None:
        switch (this.MyState)
        {
          case HUD_Heart.HeartState.HeartFull:
            this.Spine.AnimationState.SetAnimation(0, "full", false);
            break;
          case HUD_Heart.HeartState.HeartHalfFull:
          case HUD_Heart.HeartState.HeartHalf:
            this.Spine.AnimationState.SetAnimation(0, "fill-half-right", false);
            this.Spine.AnimationState.AddAnimation(0, "full", true, 0.0f);
            break;
          default:
            this.Spine.AnimationState.SetAnimation(0, "fill-whole", false);
            this.Spine.AnimationState.AddAnimation(0, "full", true, 0.0f);
            break;
        }
        break;
      case HUD_Heart.HeartState.HeartFull:
      case HUD_Heart.HeartState.HeartHalfFull:
        switch (this.MyState)
        {
          case HUD_Heart.HeartState.HeartFull:
          case HUD_Heart.HeartState.HeartHalfFull:
            if (this.MyState != NewState)
              this.Spine.AnimationState.SetAnimation(0, "lose-half", false);
            this.Spine.AnimationState.AddAnimation(0, "half-full", true, 0.0f);
            break;
          case HUD_Heart.HeartState.HeartHalf:
            this.Spine.AnimationState.AddAnimation(0, "half-full", true, 0.0f);
            break;
          default:
            this.Spine.AnimationState.SetAnimation(0, "fill-half-left", false);
            this.Spine.AnimationState.AddAnimation(0, "half-full", true, 0.0f);
            break;
        }
        break;
      case HUD_Heart.HeartState.HeartHalf:
      case HUD_Heart.HeartState.HeartContainer:
        switch (this.MyState)
        {
          case HUD_Heart.HeartState.HeartFull:
            this.Spine.AnimationState.SetAnimation(0, "lose-whole", false);
            this.Spine.AnimationState.AddAnimation(0, "empty", true, 0.0f);
            break;
          case HUD_Heart.HeartState.HeartHalfFull:
          case HUD_Heart.HeartState.HeartHalf:
            this.Spine.AnimationState.SetAnimation(0, "lose-whole", false);
            this.Spine.AnimationState.AddAnimation(0, "empty", true, 0.0f);
            break;
          default:
            this.Spine.AnimationState.AddAnimation(0, "empty", true, 0.0f);
            break;
        }
        break;
    }
    this.MyState = NewState;
    this.MyHeartType = NewHeartType;
    this.Spine.UpdateMesh();
  }

  public void ClearCoroutines()
  {
    this.Shake = 0.0f;
    this.Scale = 1f;
    this.StopAllCoroutines();
    this.isEffectActivated = false;
  }

  public IEnumerator DoShake()
  {
    HUD_Heart hudHeart = this;
    hudHeart.StartCoroutine((IEnumerator) hudHeart.DoCircle());
    hudHeart.Shaking = true;
    hudHeart.ShakeTimer = 0.0f;
    hudHeart.ShakeSpeed = 10f;
    while ((double) (hudHeart.ShakeTimer += Time.deltaTime) < 1.5)
    {
      hudHeart.ShakeSpeed += (float) ((0.0 - (double) hudHeart.Shake) * 0.30000001192092896);
      hudHeart.Shake += (hudHeart.ShakeSpeed *= 0.9f);
      yield return (object) null;
    }
    hudHeart.Shake = 0.0f;
    hudHeart.Shaking = false;
  }

  public IEnumerator DoCircle()
  {
    Color color = new Color(1f, 1f, 1f, 1f);
    Color colorOff = new Color(1f, 1f, 1f, 0.0f);
    this.Circle.transform.localScale = Vector3.zero;
    this.Circle.DOKill();
    this.Circle.color = color;
    DOTweenModuleUI.DOColor(this.Circle, colorOff, 1f);
    this.Circle.transform.DOScale(Vector3.one * 2f, 1f);
    yield return (object) new WaitForSeconds(1f);
    this.Circle.transform.localScale = Vector3.zero;
    this.Circle.color = colorOff;
  }

  public IEnumerator DoScale(float Delay)
  {
    this.isInScaleAnimation = true;
    this.Scale = 0.0f;
    while ((double) (Delay -= Time.deltaTime) > 0.0)
      yield return (object) null;
    this.Timer = 0.0f;
    this.Scale = 0.0f;
    while ((double) (this.Timer += Time.deltaTime) < 15.0)
    {
      this.ScaleSpeed += (float) ((1.0 - (double) this.Scale) * 0.20000000298023224);
      this.Scale += (this.ScaleSpeed *= 0.8f);
      yield return (object) null;
    }
    this.isInScaleAnimation = false;
    this.Scale = 1f;
  }

  public enum HeartType
  {
    Red,
    Blue,
    Spirit,
    Black,
    Fire,
    Ice,
  }

  public enum HeartState
  {
    None,
    HeartFull,
    HeartHalfFull,
    HeartHalf,
    HeartContainer,
    HalfHeartContainer,
  }
}
