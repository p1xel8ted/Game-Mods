// Decompiled with JetBrains decompiler
// Type: EyeTrailBlit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class EyeTrailBlit : MonoBehaviour
{
  [Header("Quality")]
  public EyeTrailBlit.TrailQuality quality = EyeTrailBlit.TrailQuality.High;
  [Tooltip("Extra perf: pre-scale the trail RT. Overridden by quality each frame.")]
  [Range(0.25f, 1f)]
  public float manualResolutionScale = 1f;
  [Header("Preview (optional)")]
  public RawImage preview;
  [Header("Shader & Look")]
  public Shader trailShader;
  public Color drawColor = Color.white;
  [Tooltip("Radius in pixels (short-side). 0 → use uvRadiusSSU.")]
  public float pixelRadius = 12f;
  public float uvRadiusSSU = 0.02f;
  public float pixelSoftness = 3f;
  public float pixelThickness;
  [Header("Persistence (Half-life)")]
  [Tooltip("Seconds to half the brightness. Larger = longer trails.")]
  public float trailHalfLifeSeconds = 2f;
  [Header("Noise / Flow")]
  public Texture2D noiseTex;
  public bool noiseIsNormalMap;
  [Tooltip("Displacement in pixels (short-side).")]
  public float noiseAmountPixels = 2f;
  public float noiseScale1 = 12f;
  public float noiseScale2 = 20f;
  public float noiseSpeed1 = 0.25f;
  public float noiseSpeed2 = -0.17f;
  [Range(0.0f, 1f)]
  public float layer2Weight = 0.5f;
  public bool normalizeFlow = true;
  [Header("Domain Warp (optional)")]
  public float warpAmountPixels;
  public float warpScale = 8f;
  public float warpSpeed = 0.15f;
  [Header("Directional Drift")]
  public bool driftEnabled = true;
  [Tooltip("0=Right, 90=Up")]
  public float driftAngleDeg = 90f;
  public float driftSpeedPixelsPerSec = 12f;
  [Header("Visibility/Culling")]
  public bool onlyIfVisible = true;
  [Header("Preview Auto-Toggle")]
  public EyeTrailBlit.PreviewVisibility previewVisibility = EyeTrailBlit.PreviewVisibility.AnyAnchorsExist;
  [Tooltip("Fade the RawImage in/out instead of hard toggle.")]
  public bool previewFade = true;
  [Tooltip("Max alpha when shown.")]
  [Range(0.0f, 1f)]
  public float previewMaxAlpha = 1f;
  [Tooltip("Seconds for fade in/out.")]
  [Min(0.0f)]
  public float previewFadeDuration = 0.2f;
  [Tooltip("Actually disable the RawImage when fully hidden (saves a tiny bit).")]
  public bool disableWhenFullyHidden = true;
  public float _previewAlpha;
  public Material _mat;
  public RenderTexture _rtA;
  public RenderTexture _rtB;
  public Camera _cam;
  public int _maxStamps = 64 /*0x40*/;
  public float _resolutionScale = 1f;
  public bool _noiseOn = true;
  public bool _noiseLayer2 = true;
  public bool _domainWarp = true;
  public bool _flowNormalize = true;
  public bool _driftOn = true;
  public static int ID_MainTex = Shader.PropertyToID("_MainTex");
  public static int ID_Color = Shader.PropertyToID("_Color");
  public static int ID_Radius = Shader.PropertyToID("_Radius");
  public static int ID_Softness = Shader.PropertyToID("_Softness");
  public static int ID_Thickness = Shader.PropertyToID("_Thickness");
  public static int ID_Fade = Shader.PropertyToID("_Fade");
  public static int ID_NoiseTex = Shader.PropertyToID("_NoiseTex");
  public static int ID_NoiseAmountSS = Shader.PropertyToID("_NoiseAmountSS");
  public static int ID_NoiseScale1 = Shader.PropertyToID("_NoiseScale1");
  public static int ID_NoiseScale2 = Shader.PropertyToID("_NoiseScale2");
  public static int ID_NoiseSpeed1 = Shader.PropertyToID("_NoiseSpeed1");
  public static int ID_NoiseSpeed2 = Shader.PropertyToID("_NoiseSpeed2");
  public static int ID_Layer2Weight = Shader.PropertyToID("_Layer2Weight");
  public static int ID_NoiseRotation = Shader.PropertyToID("_NoiseRotationDeg");
  public static int ID_NoiseTimeScale = Shader.PropertyToID("_NoiseTimeScale");
  public static int ID_WarpAmountSS = Shader.PropertyToID("_WarpAmountSS");
  public static int ID_WarpScale = Shader.PropertyToID("_WarpScale");
  public static int ID_WarpSpeed = Shader.PropertyToID("_WarpSpeed");
  public static int ID_DriftStepSS = Shader.PropertyToID("_DriftStepSS");
  public static int ID_StampCentersSS = Shader.PropertyToID("_StampCentersSS");
  public static int ID_StampCount = Shader.PropertyToID("_StampCount");
  public Vector4[] _centersSSU;

  public void Awake()
  {
    this._cam = this.GetComponent<Camera>();
    if ((bool) (UnityEngine.Object) this._cam)
      return;
    this._cam = Camera.main;
  }

  public void Start()
  {
    if (!(bool) (UnityEngine.Object) this.trailShader)
      this.trailShader = Shader.Find("Hidden/CircleTrailOptimized");
    this._mat = new Material(this.trailShader);
    this.ApplyQualityProfile();
    this.AllocateRTs();
    this.Clear();
    if (!(bool) (UnityEngine.Object) this.preview)
      return;
    this.preview.texture = (Texture) this._rtA;
  }

  public void UpdatePreviewVisibilityUI(int visibleCount, int totalAnchors)
  {
    if (!(bool) (UnityEngine.Object) this.preview)
      return;
    bool flag1;
    switch (this.previewVisibility)
    {
      case EyeTrailBlit.PreviewVisibility.AlwaysOn:
        flag1 = true;
        break;
      case EyeTrailBlit.PreviewVisibility.AnyAnchorsExist:
        flag1 = totalAnchors > 0;
        break;
      case EyeTrailBlit.PreviewVisibility.AnyVisibleAnchors:
        flag1 = visibleCount > 0;
        break;
      default:
        flag1 = true;
        break;
    }
    bool flag2 = flag1;
    float target = flag2 ? this.previewMaxAlpha : 0.0f;
    if (this.previewFade && (double) this.previewFadeDuration > 0.0)
    {
      float num = (double) this.previewMaxAlpha <= 0.0 ? 0.0f : this.previewMaxAlpha / this.previewFadeDuration;
      this._previewAlpha = Mathf.MoveTowards(this._previewAlpha, target, num * Time.unscaledDeltaTime);
      if (!this.preview.enabled)
        this.preview.enabled = true;
      this.preview.color = this.preview.color with
      {
        a = this._previewAlpha
      };
      if (!this.disableWhenFullyHidden || (double) this._previewAlpha > 1.0 / 1000.0)
        return;
      this.preview.enabled = false;
    }
    else
    {
      this._previewAlpha = target;
      this.preview.color = this.preview.color with
      {
        a = this._previewAlpha
      };
      this.preview.enabled = flag2;
    }
  }

  public void OnValidate()
  {
    if (!Application.isPlaying)
      return;
    this.ApplyQualityProfile();
  }

  public void ApplyQualityProfile()
  {
    switch (this.quality)
    {
      case EyeTrailBlit.TrailQuality.Low:
        this._resolutionScale = 0.5f;
        this._maxStamps = 16 /*0x10*/;
        this._noiseOn = true;
        this._noiseLayer2 = false;
        this._domainWarp = false;
        this._flowNormalize = false;
        this._driftOn = true;
        break;
      case EyeTrailBlit.TrailQuality.Medium:
        this._resolutionScale = 0.75f;
        this._maxStamps = 32 /*0x20*/;
        this._noiseOn = true;
        this._noiseLayer2 = true;
        this._domainWarp = false;
        this._flowNormalize = true;
        this._driftOn = true;
        break;
      case EyeTrailBlit.TrailQuality.High:
        this._resolutionScale = 1f;
        this._maxStamps = 64 /*0x40*/;
        this._noiseOn = true;
        this._noiseLayer2 = true;
        this._domainWarp = true;
        this._flowNormalize = true;
        this._driftOn = true;
        break;
      case EyeTrailBlit.TrailQuality.Ultra:
        this._resolutionScale = 1f;
        this._maxStamps = 128 /*0x80*/;
        this._noiseOn = true;
        this._noiseLayer2 = true;
        this._domainWarp = true;
        this._flowNormalize = true;
        this._driftOn = true;
        break;
    }
    this._resolutionScale = Mathf.Clamp(this.manualResolutionScale, 0.25f, 1f);
    if (this._centersSSU == null || this._centersSSU.Length != this._maxStamps)
      this._centersSSU = new Vector4[this._maxStamps];
    this.SetKW("_NOISE_ON", this._noiseOn);
    this.SetKW("_NOISE_LAYER2", this._noiseLayer2);
    this.SetKW("_FLOW_NORMALIZE_ON", this._flowNormalize);
    this.SetKW("_DOMAIN_WARP", this._domainWarp);
    this.SetKW("_DRIFT_ON", this._driftOn);
    this.SetKW("_NOISE_NORMALMAP", this.noiseIsNormalMap);
  }

  public void SetKW(string kw, bool on)
  {
    if (!((UnityEngine.Object) this._mat != (UnityEngine.Object) null))
      return;
    if (on)
      this._mat.EnableKeyword(kw);
    else
      this._mat.DisableKeyword(kw);
  }

  public void AllocateRTs()
  {
    this.ReleaseRTs();
    int width = Mathf.Max(1, Mathf.RoundToInt((float) Screen.width * this._resolutionScale));
    int height = Mathf.Max(1, Mathf.RoundToInt((float) Screen.height * this._resolutionScale));
    RenderTexture renderTexture1 = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
    renderTexture1.useMipMap = false;
    renderTexture1.autoGenerateMips = false;
    renderTexture1.wrapMode = TextureWrapMode.Clamp;
    renderTexture1.filterMode = FilterMode.Bilinear;
    this._rtA = renderTexture1;
    this._rtA.Create();
    RenderTexture renderTexture2 = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
    renderTexture2.useMipMap = false;
    renderTexture2.autoGenerateMips = false;
    renderTexture2.wrapMode = TextureWrapMode.Clamp;
    renderTexture2.filterMode = FilterMode.Bilinear;
    this._rtB = renderTexture2;
    this._rtB.Create();
  }

  public void ReleaseRTs()
  {
    if ((bool) (UnityEngine.Object) this._rtA)
      this._rtA.Release();
    if ((bool) (UnityEngine.Object) this._rtB)
      this._rtB.Release();
    this._rtA = this._rtB = (RenderTexture) null;
  }

  public void OnDestroy() => this.ReleaseRTs();

  public void Update()
  {
    if (UnifyManager.platform == UnifyManager.Platform.Switch)
    {
      this.preview.enabled = false;
      this.enabled = false;
    }
    int graphicsPreset = SettingsManager.Settings.Graphics.GraphicsPreset;
    if (!DataManager.Instance.OnboardedYngyaAwoken || graphicsPreset == 0)
    {
      this.Clear();
    }
    else
    {
      List<EyeAnchor> all = EyeAnchor.All;
      if (all.Count == 0)
      {
        this.Clear();
      }
      else
      {
        int num1 = Mathf.RoundToInt((float) Screen.width * this._resolutionScale);
        int num2 = Mathf.RoundToInt((float) Screen.height * this._resolutionScale);
        if ((UnityEngine.Object) this._rtA == (UnityEngine.Object) null || this._rtA.width != num1 || this._rtA.height != num2)
        {
          this.AllocateRTs();
          this.Clear();
          if ((bool) (UnityEngine.Object) this.preview)
            this.preview.texture = (Texture) this._rtA;
        }
        float num3 = (float) Mathf.Max(1, Mathf.Min(this._rtA.width, this._rtA.height));
        float num4 = (double) this.pixelRadius > 0.0 ? this.pixelRadius / num3 : this.uvRadiusSSU;
        float num5 = Mathf.Max(0.0001f, this.pixelSoftness / num3);
        float num6 = Mathf.Max(0.0f, this.pixelThickness / num3);
        float num7 = Mathf.Max(0.0f, this.noiseAmountPixels / num3);
        float num8 = Mathf.Max(0.0f, this.warpAmountPixels / num3);
        float num9 = Mathf.Exp(Mathf.Log(0.5f) * (Time.unscaledDeltaTime / Mathf.Max(0.0001f, this.trailHalfLifeSeconds)));
        int visibleCount = 0;
        this.UpdatePreviewVisibilityUI(visibleCount, all.Count);
        int count = all.Count;
        int num10 = Mathf.Max(1, Mathf.CeilToInt((float) count / (float) this._maxStamps));
        for (int index = 0; index < count && visibleCount < this._maxStamps; index += num10)
        {
          Transform transform = (bool) (UnityEngine.Object) all[index] ? all[index].transform : (Transform) null;
          if ((bool) (UnityEngine.Object) transform)
          {
            Vector3 viewportPoint = this._cam.WorldToViewportPoint(transform.position);
            if (!this.onlyIfVisible || (double) viewportPoint.z > 0.0 && (double) viewportPoint.x >= 0.0 && (double) viewportPoint.x <= 1.0 && (double) viewportPoint.y >= 0.0 && (double) viewportPoint.y <= 1.0)
            {
              float x = viewportPoint.x * (float) this._rtA.width / num3;
              float y = viewportPoint.y * (float) this._rtA.height / num3;
              this._centersSSU[visibleCount++] = new Vector4(x, y, 0.0f, 0.0f);
            }
          }
        }
        Vector2 vector2 = Vector2.zero;
        if (this._driftOn && this.driftEnabled && (double) this.driftSpeedPixelsPerSec > 0.0)
        {
          float num11 = this.driftSpeedPixelsPerSec / num3 * Time.unscaledDeltaTime;
          float f = this.driftAngleDeg * ((float) Math.PI / 180f);
          vector2 = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * num11;
        }
        this._mat.SetTexture(EyeTrailBlit.ID_MainTex, (Texture) this._rtA);
        this._mat.SetColor(EyeTrailBlit.ID_Color, this.drawColor);
        this._mat.SetFloat(EyeTrailBlit.ID_Radius, num4);
        this._mat.SetFloat(EyeTrailBlit.ID_Softness, num5);
        this._mat.SetFloat(EyeTrailBlit.ID_Thickness, num6);
        this._mat.SetFloat(EyeTrailBlit.ID_Fade, num9);
        this._mat.SetTexture(EyeTrailBlit.ID_NoiseTex, (bool) (UnityEngine.Object) this.noiseTex ? (Texture) this.noiseTex : (Texture) Texture2D.grayTexture);
        this._mat.SetFloat(EyeTrailBlit.ID_NoiseAmountSS, this._noiseOn ? num7 : 0.0f);
        this._mat.SetFloat(EyeTrailBlit.ID_NoiseScale1, Mathf.Max(0.0001f, this.noiseScale1));
        this._mat.SetFloat(EyeTrailBlit.ID_NoiseScale2, Mathf.Max(0.0001f, this.noiseScale2));
        this._mat.SetFloat(EyeTrailBlit.ID_NoiseSpeed1, this.noiseSpeed1);
        this._mat.SetFloat(EyeTrailBlit.ID_NoiseSpeed2, this.noiseSpeed2);
        this._mat.SetFloat(EyeTrailBlit.ID_Layer2Weight, this.layer2Weight);
        this._mat.SetFloat(EyeTrailBlit.ID_NoiseRotation, 0.0f);
        this._mat.SetFloat(EyeTrailBlit.ID_NoiseTimeScale, 1f);
        this._mat.SetFloat(EyeTrailBlit.ID_WarpAmountSS, this._domainWarp ? num8 : 0.0f);
        this._mat.SetFloat(EyeTrailBlit.ID_WarpScale, Mathf.Max(0.0001f, this.warpScale));
        this._mat.SetFloat(EyeTrailBlit.ID_WarpSpeed, this.warpSpeed);
        this._mat.SetVector(EyeTrailBlit.ID_DriftStepSS, new Vector4(vector2.x, vector2.y, 0.0f, 0.0f));
        if (visibleCount > 0)
          this._mat.SetVectorArray(EyeTrailBlit.ID_StampCentersSS, this._centersSSU);
        this._mat.SetFloat(EyeTrailBlit.ID_StampCount, (float) visibleCount);
        Graphics.Blit((Texture) this._rtA, this._rtB, this._mat);
        this.Swap();
        if (!(bool) (UnityEngine.Object) this.preview)
          return;
        this.preview.texture = (Texture) this._rtA;
      }
    }
  }

  public void Swap()
  {
    RenderTexture rtA = this._rtA;
    this._rtA = this._rtB;
    this._rtB = rtA;
  }

  public void Clear()
  {
    if ((bool) (UnityEngine.Object) this._rtA)
      Graphics.Blit((Texture) Texture2D.blackTexture, this._rtA);
    if (!(bool) (UnityEngine.Object) this._rtB)
      return;
    Graphics.Blit((Texture) Texture2D.blackTexture, this._rtB);
  }

  public enum TrailQuality
  {
    Low,
    Medium,
    High,
    Ultra,
  }

  public enum PreviewVisibility
  {
    AlwaysOn,
    AnyAnchorsExist,
    AnyVisibleAnchors,
  }
}
