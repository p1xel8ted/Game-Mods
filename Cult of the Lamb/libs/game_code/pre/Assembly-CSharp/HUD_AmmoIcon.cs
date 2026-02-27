// Decompiled with JetBrains decompiler
// Type: HUD_AmmoIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_AmmoIcon : BaseMonoBehaviour
{
  public Sprite On;
  public Sprite Empty;
  public Sprite SpiritOn;
  public Sprite SpiritEmpty;
  private RectTransform _rectTransform;
  public SkeletonGraphic Spine;
  [SpineSlot("", "", false, true, false, dataField = "Spine")]
  public string Slot;
  [SpineAttachment(true, false, false, "", "", "", true, false, dataField = "Spine")]
  public string Attachment = "images/AmmoSpiritOutline";
  private Image _Image;
  private HUD_AmmoIcon.Mode mode;
  private HUD_AmmoIcon.Mode PrevMode;
  private Coroutine A;
  private Coroutine D;
  private Coroutine AE;
  private float ScaleTimer;
  private float _Scale;
  private float ScaleSpeed;
  private bool Shaking;
  private float ShakeTimer;
  private float Shake;
  private float ShakeSpeed;
  private Vector3 InitPos;

  private RectTransform rectTransform
  {
    set => this.rectTransform = value;
    get
    {
      if ((Object) this._rectTransform == (Object) null)
        this._rectTransform = this.GetComponent<RectTransform>();
      return this._rectTransform;
    }
  }

  private Image Image
  {
    get
    {
      if ((Object) this._Image == (Object) null)
        this._Image = this.GetComponent<Image>();
      return this._Image;
    }
  }

  public void SetMode(HUD_AmmoIcon.Mode mode, float Delay)
  {
    if (this.A != null)
      this.StopCoroutine(this.A);
    if (this.D != null)
      this.StopCoroutine(this.D);
    if (this.AE != null)
      this.StopCoroutine(this.AE);
    this.mode = mode;
    switch (mode)
    {
      case HUD_AmmoIcon.Mode.ON:
      case HUD_AmmoIcon.Mode.ON_SPIRIT:
        this.Spine.gameObject.SetActive(true);
        this.A = this.StartCoroutine((IEnumerator) this.Activate(Delay));
        break;
      case HUD_AmmoIcon.Mode.EMPTY:
      case HUD_AmmoIcon.Mode.EMPTY_SPIRIT:
        this.Spine.gameObject.SetActive(true);
        this.AE = this.StartCoroutine((IEnumerator) this.ActivateEmpty(this.PrevMode == HUD_AmmoIcon.Mode.OFF ? Delay : 0.0f));
        break;
      case HUD_AmmoIcon.Mode.OFF:
        this.D = this.StartCoroutine((IEnumerator) this.Deactivate(Delay));
        break;
    }
    this.PrevMode = mode;
  }

  private IEnumerator Activate(float Delay)
  {
    HUD_AmmoIcon hudAmmoIcon = this;
    while ((double) (Delay -= Time.deltaTime) > 0.0)
      yield return (object) null;
    switch (hudAmmoIcon.mode)
    {
      case HUD_AmmoIcon.Mode.ON:
        hudAmmoIcon.Spine.Skeleton.SetAttachment(hudAmmoIcon.Slot, (string) null);
        if (!(hudAmmoIcon.Spine.AnimationState.GetCurrent(0).Animation.Name != "full"))
          break;
        hudAmmoIcon.Spine.AnimationState.SetAnimation(0, "fill", false);
        hudAmmoIcon.Spine.AnimationState.AddAnimation(0, "full", true, 0.0f);
        break;
      case HUD_AmmoIcon.Mode.ON_SPIRIT:
        if (hudAmmoIcon.Spine.AnimationState.GetCurrent(0).Animation.Name != "full")
        {
          hudAmmoIcon.Spine.AnimationState.SetAnimation(0, "fill", false);
          hudAmmoIcon.Spine.AnimationState.AddAnimation(0, "full", true, 0.0f);
        }
        hudAmmoIcon.Spine.Skeleton.SetAttachment(hudAmmoIcon.Slot, hudAmmoIcon.Attachment);
        hudAmmoIcon.StartCoroutine((IEnumerator) hudAmmoIcon.DoScale());
        break;
    }
  }

  private IEnumerator ActivateEmpty(float Delay)
  {
    while ((double) (Delay -= Time.deltaTime) > 0.0)
      yield return (object) null;
    if ((Object) this.Spine != (Object) null)
    {
      if (this.Spine.AnimationState.GetCurrent(0).Animation.Name != "empty")
      {
        this.Spine.AnimationState.SetAnimation(0, "shoot", false);
        this.Spine.AnimationState.AddAnimation(0, "empty", true, 0.0f);
      }
      switch (this.mode)
      {
        case HUD_AmmoIcon.Mode.EMPTY:
          this.Spine.Skeleton.SetAttachment(this.Slot, (string) null);
          break;
        case HUD_AmmoIcon.Mode.EMPTY_SPIRIT:
          this.Spine.Skeleton.SetAttachment(this.Slot, this.Attachment);
          break;
      }
    }
  }

  private IEnumerator Deactivate(float Delay)
  {
    if (this.Spine.gameObject.activeSelf)
    {
      while ((double) (Delay -= Time.deltaTime) > 0.0)
        yield return (object) null;
      this.Spine.gameObject.SetActive(false);
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

  private IEnumerator DoScale()
  {
    this.ScaleTimer = 0.0f;
    this.Scale = 1.3f;
    while ((double) (this.ScaleTimer += Time.deltaTime) < 15.0)
    {
      this.ScaleSpeed += (float) ((1.0 - (double) this.Scale) * 0.20000000298023224);
      this.Scale += (this.ScaleSpeed *= 0.8f);
      yield return (object) null;
    }
    this.Scale = 1f;
  }

  public void StartShake()
  {
    if (this.Shaking)
      return;
    this.StartCoroutine((IEnumerator) this.DoShake());
  }

  private IEnumerator DoShake()
  {
    this.Shaking = true;
    this.InitPos = this.Spine.rectTransform.localPosition;
    this.ShakeTimer = 0.0f;
    this.Shake = 0.0f;
    this.ShakeSpeed = 10f;
    while ((double) (this.ShakeTimer += Time.deltaTime) < 1.2000000476837158)
    {
      this.ShakeSpeed += (float) ((0.0 - (double) this.Shake) * 0.30000001192092896);
      this.Shake += (this.ShakeSpeed *= 0.9f);
      this.Spine.rectTransform.localPosition = this.InitPos + new Vector3(0.0f, this.Shake);
      yield return (object) null;
    }
    this.Shake = 0.0f;
    this.Spine.rectTransform.localPosition = this.InitPos + new Vector3(0.0f, this.Shake);
    this.Shaking = false;
  }

  public enum Mode
  {
    ON,
    EMPTY,
    OFF,
    ON_SPIRIT,
    EMPTY_SPIRIT,
  }
}
