// Decompiled with JetBrains decompiler
// Type: SimpleSpineFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SimpleSpineFlash : BaseMonoBehaviour
{
  public SimpleSpineFlash.FollowAngleMode FollowAngle;
  public SimpleSpineFlash.SetFacingMode SetFacing;
  public float FacingAngle;
  [SerializeField]
  public StateMachine state;
  [CompilerGenerated]
  public SkeletonAnimation \u003CSpine\u003Ek__BackingField;
  [HideInInspector]
  public bool isFillWhite;
  public MaterialPropertyBlock BlockFlash;
  public MeshRenderer _meshRenderer;
  public int fillAlpha;
  public int fillColor;
  public int tintColor;
  public Color WarningColour = Color.white;
  public Color baseColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  public MaterialPropertyBlock block;
  public bool FlashingRed;
  public Coroutine cFlashFillRoutine;
  public float FlashRedMultiplier = 0.01f;
  public int NumColorToApply;
  public Color ColorToApply;
  public int _Dir;
  public Coroutine fadeRoutine;
  [SerializeField]
  public bool stateRequired = true;

  public SkeletonAnimation Spine
  {
    get => this.\u003CSpine\u003Ek__BackingField;
    set => this.\u003CSpine\u003Ek__BackingField = value;
  }

  public MeshRenderer meshRenderer
  {
    get
    {
      if ((Object) this._meshRenderer == (Object) null)
        this._meshRenderer = this.GetComponent<MeshRenderer>();
      return this._meshRenderer;
    }
  }

  public void OnEnable()
  {
    this.Spine = this.GetComponent<SkeletonAnimation>();
    if (!((Object) this.state == (Object) null))
      return;
    this.state = this.GetComponentInParent<StateMachine>();
  }

  public void Start()
  {
    this.fillColor = Shader.PropertyToID("_FillColor");
    this.fillAlpha = Shader.PropertyToID("_FillAlpha");
    this.tintColor = Shader.PropertyToID("_Color");
    this.BlockFlash = new MaterialPropertyBlock();
    this.block = new MaterialPropertyBlock();
    this.meshRenderer.GetPropertyBlock(this.BlockFlash);
    if (this.baseColor == new Color(0.0f, 0.0f, 0.0f, 0.0f))
      this.baseColor = this.BlockFlash.GetColor(this.fillColor);
    this.baseColor.a = this.BlockFlash.GetFloat(this.fillAlpha);
  }

  public void Flash(Color color, float duration)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.StartCoroutine(this.FlashRoutine(color, duration));
  }

  public IEnumerator FlashRoutine(Color color, float duration)
  {
    if (this.BlockFlash == null)
      this.BlockFlash = new MaterialPropertyBlock();
    float t = 0.0f;
    Color currentColor = this.BlockFlash.GetColor(this.fillColor);
    float currentAlpha = this.BlockFlash.GetFloat(this.fillAlpha);
    while ((double) t < (double) duration)
    {
      if (!PlayerRelic.TimeFrozen)
      {
        float t1 = t / duration;
        this.BlockFlash.SetColor(this.fillColor, Color.Lerp(color, currentColor, t1));
        this.BlockFlash.SetFloat(this.fillAlpha, Mathf.Lerp(1f, currentAlpha, t1));
        this.meshRenderer.SetPropertyBlock(this.BlockFlash);
        t += Time.deltaTime;
      }
      yield return (object) null;
    }
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
    this.SetColor((PlayerFarming.Location != FollowerLocation.Dungeon1_5 ? this.WarningColour : StaticColors.OrangeColor) with
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
    this.SetColor((PlayerFarming.Location != FollowerLocation.Dungeon1_5 ? this.WarningColour : StaticColors.OrangeColor) with
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

  public void FlashFillRed(float opacity = 1f)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.FlashingRed = true;
    if (this.cFlashFillRoutine != null)
      this.StopCoroutine(this.cFlashFillRoutine);
    if (!this.gameObject.activeSelf)
      return;
    this.cFlashFillRoutine = this.StartCoroutine(this.FlashOnHitRoutine(opacity));
  }

  public IEnumerator FlashOnHitRoutine(float opacity)
  {
    this.meshRenderer.receiveShadows = false;
    this.meshRenderer.SetPropertyBlock(this.block);
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
  }

  public void ResetColour()
  {
    this.NumColorToApply = 0;
    this.ColorToApply = this.baseColor;
    this.FlashingRed = false;
    MaterialPropertyBlock properties = new MaterialPropertyBlock();
    this.meshRenderer.GetPropertyBlock(properties);
    properties.SetColor(this.fillColor, this.ColorToApply);
    properties.SetFloat(this.fillAlpha, 0.0f);
  }

  public IEnumerator DoFlashFillRed()
  {
    this.meshRenderer.SetPropertyBlock(new MaterialPropertyBlock());
    this.SetColor(Color.red);
    yield return (object) new WaitForSeconds(0.1f);
    float Progress = 1f;
    while ((double) (Progress -= 0.05f * this.Spine.timeScale) >= 0.0)
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
    this.StopCoroutine(this.DoFlashFillGreen());
    this.StartCoroutine(this.DoFlashFillGreen());
  }

  public IEnumerator DoFlashFillGreen()
  {
    this.meshRenderer.SetPropertyBlock(new MaterialPropertyBlock());
    this.SetColor(Color.green);
    yield return (object) new WaitForSeconds(0.1f);
    float Progress = 1f;
    while ((double) (Progress -= 0.05f * this.Spine.timeScale) >= 0.0)
    {
      if ((double) Progress <= 0.0)
        Progress = 0.0f;
      this.SetColor(new Color(0.0f, 0.5f, 0.0f, Progress));
      yield return (object) null;
    }
  }

  public void FlashFillBlack(bool ignoreSpineTimescale = false)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.StopCoroutine(this.DoFlashFillBlack(ignoreSpineTimescale));
    this.StartCoroutine(this.DoFlashFillBlack(ignoreSpineTimescale));
  }

  public IEnumerator DoFlashFillBlack(bool ignoreSpineTimescale)
  {
    this.meshRenderer.SetPropertyBlock(new MaterialPropertyBlock());
    this.SetColor(Color.black);
    yield return (object) new WaitForSeconds(0.1f);
    float Progress = 1f;
    while ((double) (Progress -= (float) (0.05000000074505806 * (ignoreSpineTimescale ? 1.0 : (double) this.Spine.timeScale))) >= 0.0)
    {
      if ((double) Progress <= 0.0)
        Progress = 0.0f;
      this.SetColor(new Color(0.0f, 0.0f, 0.0f, Progress));
      yield return (object) null;
    }
  }

  public void FlashRedTint()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.StopCoroutine(this.DoFlashTintRed());
    this.StartCoroutine(this.DoFlashTintRed());
  }

  public IEnumerator DoFlashTintRed()
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime * this.Spine.timeScale) <= (double) Duration)
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

  public int Dir
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
    this.fadeRoutine = this.StartCoroutine(this.FadeTintAway(color1));
  }

  public IEnumerator FadeTintAway(Color color)
  {
    if (this.BlockFlash == null)
      this.BlockFlash = new MaterialPropertyBlock();
    Color currentColor = this.BlockFlash.GetColor(this.tintColor);
    float duration = 0.1f;
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      if (!PlayerRelic.TimeFrozen)
      {
        float t1 = t / duration;
        this.BlockFlash.SetColor(this.tintColor, Color.Lerp(currentColor, color, t1));
        this.meshRenderer.SetPropertyBlock(this.BlockFlash);
        t += Time.deltaTime * this.Spine.timeScale;
      }
      yield return (object) null;
    }
    this.BlockFlash.SetColor(this.tintColor, color);
    this.meshRenderer.SetPropertyBlock(this.BlockFlash);
    this.fadeRoutine = (Coroutine) null;
  }

  public void OverrideBaseColor(Color color) => this.baseColor = color;

  public void LateUpdate()
  {
    if ((Object) this.state == (Object) null && this.stateRequired)
      return;
    if ((double) this.Spine.timeScale == 9.9999997473787516E-05)
    {
      this.NumColorToApply = 0;
      this.ColorToApply = this.baseColor;
    }
    else
    {
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
      if ((Object) this.state == (Object) null || this.SetFacing == SimpleSpineFlash.SetFacingMode.Ignore)
        return;
      this.FacingAngle = this.FollowAngle == SimpleSpineFlash.FollowAngleMode.LookingAngle ? this.state.LookAngle : this.state.facingAngle;
      this.Dir = ((double) this.FacingAngle <= 90.0 || (double) this.FacingAngle >= 270.0 ? -1 : 1) * (this.SetFacing == SimpleSpineFlash.SetFacingMode.Reversed ? -1 : 1);
    }
  }

  public void SetFacingType(SimpleSpineFlash.SetFacingMode mode) => this.SetFacing = mode;

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
