// Decompiled with JetBrains decompiler
// Type: SimpleSpineFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class SimpleSpineFlash : BaseMonoBehaviour
{
  public SimpleSpineFlash.FollowAngleMode FollowAngle;
  public SimpleSpineFlash.SetFacingMode SetFacing;
  public float FacingAngle;
  private StateMachine state;
  [HideInInspector]
  public bool isFillWhite;
  private MaterialPropertyBlock BlockFlash;
  private MeshRenderer _meshRenderer;
  private int fillAlpha;
  private int fillColor;
  private int tintColor;
  private Color WarningColour = Color.white;
  private Color baseColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  private bool FlashingRed;
  private Coroutine cFlashFillRoutine;
  private float FlashRedMultiplier = 0.01f;
  private int NumColorToApply;
  private Color ColorToApply;
  private int _Dir;
  private Coroutine fadeRoutine;

  public SkeletonAnimation Spine { get; private set; }

  private MeshRenderer meshRenderer
  {
    get
    {
      if ((Object) this._meshRenderer == (Object) null)
        this._meshRenderer = this.GetComponent<MeshRenderer>();
      return this._meshRenderer;
    }
  }

  private void OnEnable()
  {
    this.Spine = this.GetComponent<SkeletonAnimation>();
    if (!((Object) this.state == (Object) null))
      return;
    this.state = this.GetComponentInParent<StateMachine>();
  }

  private void Start()
  {
    this.fillColor = Shader.PropertyToID("_FillColor");
    this.fillAlpha = Shader.PropertyToID("_FillAlpha");
    this.tintColor = Shader.PropertyToID("_Color");
    this.BlockFlash = new MaterialPropertyBlock();
    this.meshRenderer.GetPropertyBlock(this.BlockFlash);
    if (this.baseColor == new Color(0.0f, 0.0f, 0.0f, 0.0f))
      this.baseColor = this.BlockFlash.GetColor(this.fillColor);
    this.baseColor.a = this.BlockFlash.GetFloat(this.fillAlpha);
  }

  public void FlashMeWhite(float alpha = 0.5f, int frameCount = 5)
  {
    if (Time.frameCount % frameCount != 0)
      return;
    this.FlashWhite(!this.isFillWhite, alpha);
  }

  public void FlashMeWhite()
  {
    if (Time.frameCount % 5 != 0)
      return;
    this.FlashWhite(!this.isFillWhite);
  }

  public void FlashWhite(bool toggle, float alpha = 0.5f)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || this.FlashingRed)
      return;
    this.SetColor(this.WarningColour with
    {
      a = toggle ? alpha : 0.0f
    });
    this.isFillWhite = toggle;
  }

  public void FlashWhite(float amt)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || this.FlashingRed)
      return;
    this.isFillWhite = (double) amt > 0.0;
    amt *= 0.44f;
    this.SetColor(this.WarningColour with
    {
      a = Mathf.Lerp(0.0f, 1f, amt)
    });
  }

  public void FlashRed(float amt)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.SetColor(Color.red with
    {
      a = Mathf.Lerp(0.0f, 1f, amt)
    });
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
    this.SetColor(new Color(1f, 0.0f, 0.0f, 0.0f));
    this.FlashingRed = false;
    this.meshRenderer.receiveShadows = true;
    this.meshRenderer.shadowCastingMode = ShadowCastingMode.On;
  }

  private IEnumerator DoFlashFillRed()
  {
    this.meshRenderer.SetPropertyBlock(new MaterialPropertyBlock());
    this.SetColor(Color.red);
    yield return (object) new WaitForSeconds(0.1f);
    float Progress = 1f;
    while ((double) (Progress -= 0.05f) >= 0.0)
    {
      if ((double) Progress <= 0.0)
        Progress = 0.0f;
      this.SetColor(new Color(1f, 0.0f, 0.0f, Progress));
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
    this.meshRenderer.SetPropertyBlock(new MaterialPropertyBlock());
    this.SetColor(Color.green);
    yield return (object) new WaitForSeconds(0.1f);
    float Progress = 1f;
    while ((double) (Progress -= 0.05f) >= 0.0)
    {
      if ((double) Progress <= 0.0)
        Progress = 0.0f;
      this.SetColor(new Color(0.0f, 0.5f, 0.0f, Progress));
      yield return (object) null;
    }
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
    this.meshRenderer.SetPropertyBlock(new MaterialPropertyBlock());
    this.SetColor(Color.black);
    yield return (object) new WaitForSeconds(0.1f);
    float Progress = 1f;
    while ((double) (Progress -= 0.05f) >= 0.0)
    {
      if ((double) Progress <= 0.0)
        Progress = 0.0f;
      this.SetColor(new Color(0.0f, 0.0f, 0.0f, Progress));
      yield return (object) null;
    }
  }

  private void FlashRedTint()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.StopCoroutine((IEnumerator) this.DoFlashTintRed());
    this.StartCoroutine((IEnumerator) this.DoFlashTintRed());
  }

  private IEnumerator DoFlashTintRed()
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) <= (double) Duration)
    {
      this.SetColor(new Color(1f, 0.0f, 0.0f, (float) (1.0 - (double) Progress / (double) Duration)));
      yield return (object) null;
    }
    this.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
  }

  public void SetColor(Color color)
  {
    this.ColorToApply += color;
    ++this.NumColorToApply;
  }

  private int Dir
  {
    set
    {
      if (this._Dir == value)
        return;
      this.Spine.skeleton.ScaleX = (float) (this._Dir = value);
    }
  }

  public void Tint(Color color)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    Color color1 = !(color == Color.white) || !(this.baseColor != new Color(0.0f, 0.0f, 0.0f, 0.0f)) ? color : new Color(this.baseColor.r, this.baseColor.g, this.baseColor.b);
    if (this.fadeRoutine != null)
      this.StopCoroutine(this.fadeRoutine);
    this.fadeRoutine = this.StartCoroutine((IEnumerator) this.FadeTintAway(color1));
  }

  private IEnumerator FadeTintAway(Color color)
  {
    if (this.BlockFlash == null)
      this.BlockFlash = new MaterialPropertyBlock();
    Color currentColor = this.BlockFlash.GetColor(this.tintColor);
    float duration = 0.1f;
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < (double) duration)
    {
      float t1 = t / duration;
      this.BlockFlash.SetColor(this.tintColor, Color.Lerp(currentColor, color, t1));
      this.meshRenderer.SetPropertyBlock(this.BlockFlash);
      yield return (object) null;
    }
    this.BlockFlash.SetColor(this.tintColor, color);
    this.meshRenderer.SetPropertyBlock(this.BlockFlash);
    this.fadeRoutine = (Coroutine) null;
  }

  public void OverrideBaseColor(Color color) => this.baseColor = color;

  private void LateUpdate()
  {
    if ((Object) this.state == (Object) null)
      return;
    if (this.NumColorToApply > 0)
    {
      if (this.BlockFlash == null)
        this.BlockFlash = new MaterialPropertyBlock();
      this.BlockFlash.SetColor(this.fillColor, this.ColorToApply / (float) this.NumColorToApply);
      this.BlockFlash.SetFloat(this.fillAlpha, this.ColorToApply.a / (float) this.NumColorToApply);
      this.meshRenderer.SetPropertyBlock(this.BlockFlash);
      this.NumColorToApply = 0;
      this.ColorToApply = this.baseColor;
    }
    if (this.SetFacing == SimpleSpineFlash.SetFacingMode.Ignore)
      return;
    this.FacingAngle = this.FollowAngle == SimpleSpineFlash.FollowAngleMode.LookingAngle ? this.state.LookAngle : this.state.facingAngle;
    this.Dir = ((double) this.FacingAngle <= 90.0 || (double) this.FacingAngle >= 270.0 ? -1 : 1) * (this.SetFacing == SimpleSpineFlash.SetFacingMode.Reversed ? -1 : 1);
  }

  public void SetFacingType(int mode) => this.SetFacing = (SimpleSpineFlash.SetFacingMode) mode;

  public enum FollowAngleMode
  {
    LookingAngle,
    FacingAngle,
  }

  public enum SetFacingMode
  {
    None,
    Normal,
    Reversed,
    Ignore,
  }
}
