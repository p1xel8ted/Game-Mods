// Decompiled with JetBrains decompiler
// Type: HUD_Heart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  public string halfSkin = "";
  private float _Shake;
  private float ShakeSpeed;
  private float _Scale;
  private float ScaleSpeed;
  private bool wasFull;
  public HUD_Heart.HeartType MyHeartType;
  public HUD_Heart.HeartState MyState;
  private bool Shaking;
  private float ShakeTimer;
  private float Timer;

  private void Start() => this.rectTransform = this.GetComponent<RectTransform>();

  private float Shake
  {
    get => this._Shake;
    set
    {
      this._Shake = value;
      this.Spine.rectTransform.localPosition = new Vector3(0.0f, this.Shake);
    }
  }

  private float Scale
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
      this.gameObject.SetActive(Active);
      if (!DoEffects)
        return;
      this.ClearCoroutines();
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

  private IEnumerator DeactivateWithEffect()
  {
    HUD_Heart hudHeart = this;
    hudHeart.Spine.AnimationState.SetAnimation(0, "disappear", false);
    yield return (object) hudHeart.StartCoroutine((IEnumerator) hudHeart.DoCircle());
    yield return (object) new WaitForSeconds(0.1f);
    hudHeart.gameObject.SetActive(false);
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

  private void ClearCoroutines()
  {
    this.Shake = 0.0f;
    this.Scale = 1f;
    this.StopAllCoroutines();
  }

  private IEnumerator DoShake()
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

  private IEnumerator DoCircle()
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

  private IEnumerator DoScale(float Delay)
  {
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
    this.Scale = 1f;
  }

  public enum HeartType
  {
    Red,
    Blue,
    Spirit,
    Black,
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
