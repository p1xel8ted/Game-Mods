// Decompiled with JetBrains decompiler
// Type: AmplifyColorBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using AmplifyColor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

#nullable disable
[AddComponentMenu("")]
public class AmplifyColorBase : MonoBehaviour
{
  public const int LutSize = 32 /*0x20*/;
  public const int LutWidth = 1024 /*0x0400*/;
  public const int LutHeight = 32 /*0x20*/;
  public const int DepthCurveLutRange = 1024 /*0x0400*/;
  public Tonemapping Tonemapper;
  public float Exposure = 1f;
  public float LinearWhitePoint = 11.2f;
  [FormerlySerializedAs("UseDithering")]
  public bool ApplyDithering;
  public Quality QualityLevel = Quality.Standard;
  public float BlendAmount;
  public Texture LutTexture;
  public Texture LutBlendTexture;
  public Texture MaskTexture;
  public bool UseDepthMask;
  public AnimationCurve DepthMaskCurve = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 1f),
    new Keyframe(1f, 1f)
  });
  public bool UseVolumes;
  public float ExitVolumeBlendTime = 1f;
  public Transform TriggerVolumeProxy;
  public LayerMask VolumeCollisionMask = (LayerMask) -1;
  public Camera ownerCamera;
  public Shader shaderBase;
  public Shader shaderBlend;
  public Shader shaderBlendCache;
  public Shader shaderMask;
  public Shader shaderMaskBlend;
  public Shader shaderDepthMask;
  public Shader shaderDepthMaskBlend;
  public Shader shaderProcessOnly;
  public RenderTexture blendCacheLut;
  public Texture2D defaultLut;
  public Texture2D depthCurveLut;
  public Color32[] depthCurveColors;
  public ColorSpace colorSpace = ColorSpace.Uninitialized;
  public Quality qualityLevel = Quality.Standard;
  public Material materialBase;
  public Material materialBlend;
  public Material materialBlendCache;
  public Material materialMask;
  public Material materialMaskBlend;
  public Material materialDepthMask;
  public Material materialDepthMaskBlend;
  public Material materialProcessOnly;
  public bool blending;
  public float blendingTime;
  public float blendingTimeCountdown;
  public System.Action onFinishBlend;
  public AnimationCurve prevDepthMaskCurve = new AnimationCurve();
  public bool volumesBlending;
  public float volumesBlendingTime;
  public float volumesBlendingTimeCountdown;
  public Texture volumesLutBlendTexture;
  public float volumesBlendAmount;
  public Texture worldLUT;
  public AmplifyColorVolumeBase currentVolumeLut;
  public RenderTexture midBlendLUT;
  public bool blendingFromMidBlend;
  public VolumeEffect worldVolumeEffects;
  public VolumeEffect currentVolumeEffects;
  public VolumeEffect blendVolumeEffects;
  public float worldExposure = 1f;
  public float currentExposure = 1f;
  public float blendExposure = 1f;
  public float effectVolumesBlendAdjust;
  public List<AmplifyColorVolumeBase> enteredVolumes = new List<AmplifyColorVolumeBase>();
  public AmplifyColorTriggerProxyBase actualTriggerProxy;
  [HideInInspector]
  public VolumeEffectFlags EffectFlags = new VolumeEffectFlags();
  [SerializeField]
  [HideInInspector]
  public string sharedInstanceID = "";
  public bool silentError;

  public Texture2D DefaultLut
  {
    get => !((UnityEngine.Object) this.defaultLut == (UnityEngine.Object) null) ? this.defaultLut : this.CreateDefaultLut();
  }

  public bool IsBlending => this.blending;

  public float effectVolumesBlendAdjusted
  {
    get
    {
      return Mathf.Clamp01((double) this.effectVolumesBlendAdjust < 0.99000000953674316 ? (float) (((double) this.volumesBlendAmount - (double) this.effectVolumesBlendAdjust) / (1.0 - (double) this.effectVolumesBlendAdjust)) : 1f);
    }
  }

  public string SharedInstanceID => this.sharedInstanceID;

  public bool WillItBlend
  {
    get
    {
      return (UnityEngine.Object) this.LutTexture != (UnityEngine.Object) null && (UnityEngine.Object) this.LutBlendTexture != (UnityEngine.Object) null && !this.blending;
    }
  }

  public void NewSharedInstanceID() => this.sharedInstanceID = Guid.NewGuid().ToString();

  public void ReportMissingShaders()
  {
    Debug.LogError((object) "[AmplifyColor] Failed to initialize shaders. Please attempt to re-enable the Amplify Color Effect component. If that fails, please reinstall Amplify Color.");
    this.enabled = false;
  }

  public void ReportNotSupported()
  {
    Debug.LogError((object) "[AmplifyColor] This image effect is not supported on this platform.");
    this.enabled = false;
  }

  public bool CheckShader(Shader s)
  {
    if ((UnityEngine.Object) s == (UnityEngine.Object) null)
    {
      this.ReportMissingShaders();
      return false;
    }
    if (s.isSupported)
      return true;
    this.ReportNotSupported();
    return false;
  }

  public bool CheckShaders()
  {
    return this.CheckShader(this.shaderBase) && this.CheckShader(this.shaderBlend) && this.CheckShader(this.shaderBlendCache) && this.CheckShader(this.shaderMask) && this.CheckShader(this.shaderMaskBlend) && this.CheckShader(this.shaderProcessOnly);
  }

  public bool CheckSupport()
  {
    if (SystemInfo.supportsImageEffects)
      return true;
    this.ReportNotSupported();
    return false;
  }

  public void OnEnable()
  {
    if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
    {
      Debug.LogWarning((object) "[AmplifyColor] Null graphics device detected. Skipping effect silently.");
      this.silentError = true;
    }
    else
    {
      if (!this.CheckSupport() || !this.CreateMaterials())
        return;
      Texture2D lutTexture = this.LutTexture as Texture2D;
      Texture2D lutBlendTexture = this.LutBlendTexture as Texture2D;
      if ((!((UnityEngine.Object) lutTexture != (UnityEngine.Object) null) || lutTexture.mipmapCount <= 1) && (!((UnityEngine.Object) lutBlendTexture != (UnityEngine.Object) null) || lutBlendTexture.mipmapCount <= 1))
        return;
      Debug.LogError((object) "[AmplifyColor] Please disable \"Generate Mip Maps\" import settings on all LUT textures to avoid visual glitches. Change Texture Type to \"Advanced\" to access Mip settings.");
    }
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) this.actualTriggerProxy != (UnityEngine.Object) null)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.actualTriggerProxy.gameObject);
      this.actualTriggerProxy = (AmplifyColorTriggerProxyBase) null;
    }
    this.ReleaseMaterials();
    this.ReleaseTextures();
  }

  public void VolumesBlendTo(Texture blendTargetLUT, float blendTimeInSec)
  {
    this.volumesLutBlendTexture = blendTargetLUT;
    this.volumesBlendAmount = 0.0f;
    this.volumesBlendingTime = blendTimeInSec;
    this.volumesBlendingTimeCountdown = blendTimeInSec;
    this.volumesBlending = true;
  }

  public void BlendTo(Texture blendTargetLUT, float blendTimeInSec, System.Action onFinishBlend)
  {
    this.LutBlendTexture = blendTargetLUT;
    this.BlendAmount = 0.0f;
    this.onFinishBlend = onFinishBlend;
    this.blendingTime = blendTimeInSec;
    this.blendingTimeCountdown = blendTimeInSec;
    this.blending = true;
  }

  public void CheckCamera()
  {
    if ((UnityEngine.Object) this.ownerCamera == (UnityEngine.Object) null)
      this.ownerCamera = this.GetComponent<Camera>();
    if (!this.UseDepthMask || (this.ownerCamera.depthTextureMode & DepthTextureMode.Depth) != DepthTextureMode.None)
      return;
    this.ownerCamera.depthTextureMode |= DepthTextureMode.Depth;
  }

  public void Start()
  {
    if (this.silentError)
      return;
    this.CheckCamera();
    this.worldLUT = this.LutTexture;
    this.worldVolumeEffects = this.EffectFlags.GenerateEffectData(this);
    this.blendVolumeEffects = this.currentVolumeEffects = this.worldVolumeEffects;
    this.worldExposure = this.Exposure;
    this.blendExposure = this.currentExposure = this.worldExposure;
  }

  public void Update()
  {
    if (this.silentError)
      return;
    this.CheckCamera();
    bool flag = false;
    if (this.volumesBlending)
    {
      this.volumesBlendAmount = (this.volumesBlendingTime - this.volumesBlendingTimeCountdown) / this.volumesBlendingTime;
      this.volumesBlendingTimeCountdown -= Time.smoothDeltaTime;
      if ((double) this.volumesBlendAmount >= 1.0)
      {
        this.volumesBlendAmount = 1f;
        flag = true;
      }
    }
    else
      this.volumesBlendAmount = Mathf.Clamp01(this.volumesBlendAmount);
    if (this.blending)
    {
      this.BlendAmount = (this.blendingTime - this.blendingTimeCountdown) / this.blendingTime;
      this.blendingTimeCountdown -= Time.smoothDeltaTime;
      if ((double) this.BlendAmount >= 1.0)
      {
        this.LutTexture = this.LutBlendTexture;
        this.BlendAmount = 0.0f;
        this.blending = false;
        this.LutBlendTexture = (Texture) null;
        if (this.onFinishBlend != null)
          this.onFinishBlend();
      }
    }
    else
      this.BlendAmount = Mathf.Clamp01(this.BlendAmount);
    if (this.UseVolumes)
    {
      if ((UnityEngine.Object) this.actualTriggerProxy == (UnityEngine.Object) null)
      {
        GameObject gameObject1 = new GameObject(this.name + "+ACVolumeProxy");
        gameObject1.hideFlags = HideFlags.HideAndDontSave;
        GameObject gameObject2 = gameObject1;
        this.actualTriggerProxy = !((UnityEngine.Object) this.TriggerVolumeProxy != (UnityEngine.Object) null) || !((UnityEngine.Object) this.TriggerVolumeProxy.GetComponent<Collider2D>() != (UnityEngine.Object) null) ? (AmplifyColorTriggerProxyBase) gameObject2.AddComponent<AmplifyColorTriggerProxy>() : (AmplifyColorTriggerProxyBase) gameObject2.AddComponent<AmplifyColorTriggerProxy2D>();
        this.actualTriggerProxy.OwnerEffect = this;
      }
      this.UpdateVolumes();
    }
    else if ((UnityEngine.Object) this.actualTriggerProxy != (UnityEngine.Object) null)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.actualTriggerProxy.gameObject);
      this.actualTriggerProxy = (AmplifyColorTriggerProxyBase) null;
    }
    if (!flag)
      return;
    this.LutTexture = this.volumesLutBlendTexture;
    this.volumesBlendAmount = 0.0f;
    this.volumesBlending = false;
    this.volumesLutBlendTexture = (Texture) null;
    this.effectVolumesBlendAdjust = 0.0f;
    this.currentVolumeEffects = this.blendVolumeEffects;
    this.currentVolumeEffects.SetValues(this);
    this.currentExposure = this.blendExposure;
    if (this.blendingFromMidBlend && (UnityEngine.Object) this.midBlendLUT != (UnityEngine.Object) null)
      this.midBlendLUT.DiscardContents();
    this.blendingFromMidBlend = false;
  }

  public void EnterVolume(AmplifyColorVolumeBase volume)
  {
    if (this.enteredVolumes.Contains(volume))
      return;
    this.enteredVolumes.Insert(0, volume);
  }

  public void ExitVolume(AmplifyColorVolumeBase volume)
  {
    if (!this.enteredVolumes.Contains(volume))
      return;
    this.enteredVolumes.Remove(volume);
  }

  public void UpdateVolumes()
  {
    if (this.volumesBlending)
      this.currentVolumeEffects.BlendValues(this, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
    if (this.volumesBlending)
      this.Exposure = Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
    Transform transform = (UnityEngine.Object) this.TriggerVolumeProxy == (UnityEngine.Object) null ? this.transform : this.TriggerVolumeProxy;
    if ((UnityEngine.Object) this.actualTriggerProxy.transform.parent != (UnityEngine.Object) transform)
    {
      this.actualTriggerProxy.Reference = transform;
      this.actualTriggerProxy.gameObject.layer = transform.gameObject.layer;
    }
    AmplifyColorVolumeBase amplifyColorVolumeBase = (AmplifyColorVolumeBase) null;
    int num = int.MinValue;
    for (int index = 0; index < this.enteredVolumes.Count; ++index)
    {
      AmplifyColorVolumeBase enteredVolume = this.enteredVolumes[index];
      if (enteredVolume.Priority > num)
      {
        amplifyColorVolumeBase = enteredVolume;
        num = enteredVolume.Priority;
      }
    }
    if (!((UnityEngine.Object) amplifyColorVolumeBase != (UnityEngine.Object) this.currentVolumeLut))
      return;
    this.currentVolumeLut = amplifyColorVolumeBase;
    Texture blendTargetLUT = (UnityEngine.Object) amplifyColorVolumeBase == (UnityEngine.Object) null ? this.worldLUT : (Texture) amplifyColorVolumeBase.LutTexture;
    float blendTimeInSec = (UnityEngine.Object) amplifyColorVolumeBase == (UnityEngine.Object) null ? this.ExitVolumeBlendTime : amplifyColorVolumeBase.EnterBlendTime;
    if (this.volumesBlending && !this.blendingFromMidBlend && (UnityEngine.Object) blendTargetLUT == (UnityEngine.Object) this.LutTexture)
    {
      this.LutTexture = this.volumesLutBlendTexture;
      this.volumesLutBlendTexture = blendTargetLUT;
      this.volumesBlendingTimeCountdown = blendTimeInSec * ((this.volumesBlendingTime - this.volumesBlendingTimeCountdown) / this.volumesBlendingTime);
      this.volumesBlendingTime = blendTimeInSec;
      this.currentVolumeEffects = VolumeEffect.BlendValuesToVolumeEffect(this.EffectFlags, this.currentVolumeEffects, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
      this.currentExposure = Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
      this.effectVolumesBlendAdjust = 1f - this.volumesBlendAmount;
      this.volumesBlendAmount = 1f - this.volumesBlendAmount;
    }
    else
    {
      if (this.volumesBlending)
      {
        this.materialBlendCache.SetFloat("_LerpAmount", this.volumesBlendAmount);
        if (this.blendingFromMidBlend)
        {
          Graphics.Blit((Texture) this.midBlendLUT, this.blendCacheLut);
          this.materialBlendCache.SetTexture("_RgbTex", (Texture) this.blendCacheLut);
        }
        else
          this.materialBlendCache.SetTexture("_RgbTex", this.LutTexture);
        this.materialBlendCache.SetTexture("_LerpRgbTex", (UnityEngine.Object) this.volumesLutBlendTexture != (UnityEngine.Object) null ? this.volumesLutBlendTexture : (Texture) this.defaultLut);
        Graphics.Blit((Texture) this.midBlendLUT, this.midBlendLUT, this.materialBlendCache);
        this.blendCacheLut.DiscardContents();
        this.currentVolumeEffects = VolumeEffect.BlendValuesToVolumeEffect(this.EffectFlags, this.currentVolumeEffects, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
        this.currentExposure = Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
        this.effectVolumesBlendAdjust = 0.0f;
        this.blendingFromMidBlend = true;
      }
      this.VolumesBlendTo(blendTargetLUT, blendTimeInSec);
    }
    this.blendVolumeEffects = (UnityEngine.Object) amplifyColorVolumeBase == (UnityEngine.Object) null ? this.worldVolumeEffects : amplifyColorVolumeBase.EffectContainer.FindVolumeEffect(this);
    this.blendExposure = (UnityEngine.Object) amplifyColorVolumeBase == (UnityEngine.Object) null ? this.worldExposure : amplifyColorVolumeBase.Exposure;
    if (this.blendVolumeEffects != null)
      return;
    this.blendVolumeEffects = this.worldVolumeEffects;
  }

  public void SetupShader()
  {
    this.colorSpace = QualitySettings.activeColorSpace;
    this.qualityLevel = this.QualityLevel;
    this.shaderBase = Shader.Find("Hidden/Amplify Color/Base");
    this.shaderBlend = Shader.Find("Hidden/Amplify Color/Blend");
    this.shaderBlendCache = Shader.Find("Hidden/Amplify Color/BlendCache");
    this.shaderMask = Shader.Find("Hidden/Amplify Color/Mask");
    this.shaderMaskBlend = Shader.Find("Hidden/Amplify Color/MaskBlend");
    this.shaderDepthMask = Shader.Find("Hidden/Amplify Color/DepthMask");
    this.shaderDepthMaskBlend = Shader.Find("Hidden/Amplify Color/DepthMaskBlend");
    this.shaderProcessOnly = Shader.Find("Hidden/Amplify Color/ProcessOnly");
  }

  public void ReleaseMaterials()
  {
    this.SafeRelease<Material>(ref this.materialBase);
    this.SafeRelease<Material>(ref this.materialBlend);
    this.SafeRelease<Material>(ref this.materialBlendCache);
    this.SafeRelease<Material>(ref this.materialMask);
    this.SafeRelease<Material>(ref this.materialMaskBlend);
    this.SafeRelease<Material>(ref this.materialDepthMask);
    this.SafeRelease<Material>(ref this.materialDepthMaskBlend);
    this.SafeRelease<Material>(ref this.materialProcessOnly);
  }

  public Texture2D CreateDefaultLut()
  {
    Texture2D texture2D = new Texture2D(1024 /*0x0400*/, 32 /*0x20*/, TextureFormat.RGB24, false, true);
    texture2D.hideFlags = HideFlags.HideAndDontSave;
    this.defaultLut = texture2D;
    this.defaultLut.name = "DefaultLut";
    this.defaultLut.hideFlags = HideFlags.DontSave;
    this.defaultLut.anisoLevel = 1;
    this.defaultLut.filterMode = FilterMode.Bilinear;
    Color32[] colors = new Color32[32768 /*0x8000*/];
    for (int index1 = 0; index1 < 32 /*0x20*/; ++index1)
    {
      int num1 = index1 * 32 /*0x20*/;
      for (int index2 = 0; index2 < 32 /*0x20*/; ++index2)
      {
        int num2 = num1 + index2 * 1024 /*0x0400*/;
        for (int index3 = 0; index3 < 32 /*0x20*/; ++index3)
        {
          double num3 = (double) index3 / 31.0;
          float num4 = (float) index2 / 31f;
          float num5 = (float) index1 / 31f;
          byte r = (byte) (num3 * (double) byte.MaxValue);
          byte g = (byte) ((double) num4 * (double) byte.MaxValue);
          byte b = (byte) ((double) num5 * (double) byte.MaxValue);
          colors[num2 + index3] = new Color32(r, g, b, byte.MaxValue);
        }
      }
    }
    this.defaultLut.SetPixels32(colors);
    this.defaultLut.Apply();
    return this.defaultLut;
  }

  public Texture2D CreateDepthCurveLut()
  {
    this.SafeRelease<Texture2D>(ref this.depthCurveLut);
    Texture2D texture2D = new Texture2D(1024 /*0x0400*/, 1, TextureFormat.Alpha8, false, true);
    texture2D.hideFlags = HideFlags.HideAndDontSave;
    this.depthCurveLut = texture2D;
    this.depthCurveLut.name = "DepthCurveLut";
    this.depthCurveLut.hideFlags = HideFlags.DontSave;
    this.depthCurveLut.anisoLevel = 1;
    this.depthCurveLut.wrapMode = TextureWrapMode.Clamp;
    this.depthCurveLut.filterMode = FilterMode.Bilinear;
    this.depthCurveColors = new Color32[1024 /*0x0400*/];
    return this.depthCurveLut;
  }

  public void UpdateDepthCurveLut()
  {
    if ((UnityEngine.Object) this.depthCurveLut == (UnityEngine.Object) null)
      this.CreateDepthCurveLut();
    float time = 0.0f;
    int index = 0;
    while (index < 1024 /*0x0400*/)
    {
      this.depthCurveColors[index].a = (byte) Mathf.FloorToInt(Mathf.Clamp01(this.DepthMaskCurve.Evaluate(time)) * (float) byte.MaxValue);
      ++index;
      time += 0.0009775171f;
    }
    this.depthCurveLut.SetPixels32(this.depthCurveColors);
    this.depthCurveLut.Apply();
  }

  public void CheckUpdateDepthCurveLut()
  {
    bool flag = false;
    if (this.DepthMaskCurve.length != this.prevDepthMaskCurve.length)
    {
      flag = true;
    }
    else
    {
      float time = 0.0f;
      int num = 0;
      while (num < this.DepthMaskCurve.length)
      {
        if ((double) Mathf.Abs(this.DepthMaskCurve.Evaluate(time) - this.prevDepthMaskCurve.Evaluate(time)) > 1.4012984643248171E-45)
        {
          flag = true;
          break;
        }
        ++num;
        time += 0.0009775171f;
      }
    }
    if (!((UnityEngine.Object) this.depthCurveLut == (UnityEngine.Object) null | flag))
      return;
    this.UpdateDepthCurveLut();
    this.prevDepthMaskCurve = new AnimationCurve(this.DepthMaskCurve.keys);
  }

  public void CreateHelperTextures()
  {
    this.ReleaseTextures();
    RenderTexture renderTexture1 = new RenderTexture(1024 /*0x0400*/, 32 /*0x20*/, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
    renderTexture1.hideFlags = HideFlags.HideAndDontSave;
    this.blendCacheLut = renderTexture1;
    this.blendCacheLut.name = "BlendCacheLut";
    this.blendCacheLut.wrapMode = TextureWrapMode.Clamp;
    this.blendCacheLut.useMipMap = false;
    this.blendCacheLut.anisoLevel = 0;
    this.blendCacheLut.Create();
    RenderTexture renderTexture2 = new RenderTexture(1024 /*0x0400*/, 32 /*0x20*/, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
    renderTexture2.hideFlags = HideFlags.HideAndDontSave;
    this.midBlendLUT = renderTexture2;
    this.midBlendLUT.name = "MidBlendLut";
    this.midBlendLUT.wrapMode = TextureWrapMode.Clamp;
    this.midBlendLUT.useMipMap = false;
    this.midBlendLUT.anisoLevel = 0;
    this.midBlendLUT.Create();
    this.CreateDefaultLut();
    if (!this.UseDepthMask)
      return;
    this.CreateDepthCurveLut();
  }

  public bool CheckMaterialAndShader(Material material, string name)
  {
    if ((UnityEngine.Object) material == (UnityEngine.Object) null || (UnityEngine.Object) material.shader == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) $"[AmplifyColor] Error creating {name} material. Effect disabled.");
      this.enabled = false;
    }
    else if (!material.shader.isSupported)
    {
      Debug.LogWarning((object) $"[AmplifyColor] {name} shader not supported on this platform. Effect disabled.");
      this.enabled = false;
    }
    else
      material.hideFlags = HideFlags.HideAndDontSave;
    return this.enabled;
  }

  public bool CreateMaterials()
  {
    this.SetupShader();
    if (!this.CheckShaders())
      return false;
    this.ReleaseMaterials();
    this.materialBase = new Material(this.shaderBase);
    this.materialBlend = new Material(this.shaderBlend);
    this.materialBlendCache = new Material(this.shaderBlendCache);
    this.materialMask = new Material(this.shaderMask);
    this.materialMaskBlend = new Material(this.shaderMaskBlend);
    this.materialDepthMask = new Material(this.shaderDepthMask);
    this.materialDepthMaskBlend = new Material(this.shaderDepthMaskBlend);
    this.materialProcessOnly = new Material(this.shaderProcessOnly);
    if (((((((((false ? 0 : (this.CheckMaterialAndShader(this.materialBase, "BaseMaterial") ? 1 : 0)) == 0 ? 0 : (this.CheckMaterialAndShader(this.materialBlend, "BlendMaterial") ? 1 : 0)) == 0 ? 0 : (this.CheckMaterialAndShader(this.materialBlendCache, "BlendCacheMaterial") ? 1 : 0)) == 0 ? 0 : (this.CheckMaterialAndShader(this.materialMask, "MaskMaterial") ? 1 : 0)) == 0 ? 0 : (this.CheckMaterialAndShader(this.materialMaskBlend, "MaskBlendMaterial") ? 1 : 0)) == 0 ? 0 : (this.CheckMaterialAndShader(this.materialDepthMask, "DepthMaskMaterial") ? 1 : 0)) == 0 ? 0 : (this.CheckMaterialAndShader(this.materialDepthMaskBlend, "DepthMaskBlendMaterial") ? 1 : 0)) == 0 ? 0 : (this.CheckMaterialAndShader(this.materialProcessOnly, "ProcessOnlyMaterial") ? 1 : 0)) == 0)
      return false;
    this.CreateHelperTextures();
    return true;
  }

  public void SetMaterialKeyword(string keyword, bool state)
  {
    bool flag = this.materialBase.IsKeywordEnabled(keyword);
    if (state && !flag)
    {
      this.materialBase.EnableKeyword(keyword);
      this.materialBlend.EnableKeyword(keyword);
      this.materialBlendCache.EnableKeyword(keyword);
      this.materialMask.EnableKeyword(keyword);
      this.materialMaskBlend.EnableKeyword(keyword);
      this.materialDepthMask.EnableKeyword(keyword);
      this.materialDepthMaskBlend.EnableKeyword(keyword);
      this.materialProcessOnly.EnableKeyword(keyword);
    }
    else
    {
      if (state || !this.materialBase.IsKeywordEnabled(keyword))
        return;
      this.materialBase.DisableKeyword(keyword);
      this.materialBlend.DisableKeyword(keyword);
      this.materialBlendCache.DisableKeyword(keyword);
      this.materialMask.DisableKeyword(keyword);
      this.materialMaskBlend.DisableKeyword(keyword);
      this.materialDepthMask.DisableKeyword(keyword);
      this.materialDepthMaskBlend.DisableKeyword(keyword);
      this.materialProcessOnly.DisableKeyword(keyword);
    }
  }

  public void SafeRelease<T>(ref T obj) where T : UnityEngine.Object
  {
    if (!((UnityEngine.Object) obj != (UnityEngine.Object) null))
      return;
    if (System.Type.op_Equality(obj.GetType(), typeof (RenderTexture)))
      ((object) obj as RenderTexture).Release();
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) obj);
    obj = default (T);
  }

  public void ReleaseTextures()
  {
    RenderTexture.active = (RenderTexture) null;
    this.SafeRelease<RenderTexture>(ref this.blendCacheLut);
    this.SafeRelease<RenderTexture>(ref this.midBlendLUT);
    this.SafeRelease<Texture2D>(ref this.defaultLut);
    this.SafeRelease<Texture2D>(ref this.depthCurveLut);
  }

  public static bool ValidateLutDimensions(Texture lut)
  {
    bool flag = true;
    if ((UnityEngine.Object) lut != (UnityEngine.Object) null)
    {
      if (lut.width / lut.height != lut.height)
      {
        Debug.LogWarning((object) $"[AmplifyColor] Lut {lut.name} has invalid dimensions.");
        flag = false;
      }
      else if (lut.anisoLevel != 0)
        lut.anisoLevel = 0;
    }
    return flag;
  }

  public void UpdatePostEffectParams()
  {
    if (this.UseDepthMask)
      this.CheckUpdateDepthCurveLut();
    this.Exposure = Mathf.Max(this.Exposure, 0.0f);
  }

  public int ComputeShaderPass()
  {
    bool flag1 = this.QualityLevel == Quality.Mobile;
    bool flag2 = this.colorSpace == ColorSpace.Linear;
    int num1 = this.ownerCamera.allowHDR ? 1 : 0;
    int num2 = flag1 ? 18 : 0;
    return num1 == 0 ? num2 + (flag2 ? 1 : 0) : (int) (num2 + 2 + (flag2 ? 8 : 0) + (this.ApplyDithering ? 4 : 0) + this.Tonemapper);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (this.silentError)
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      this.BlendAmount = Mathf.Clamp01(this.BlendAmount);
      if (this.colorSpace != QualitySettings.activeColorSpace || this.qualityLevel != this.QualityLevel)
        this.CreateMaterials();
      this.UpdatePostEffectParams();
      int num1 = AmplifyColorBase.ValidateLutDimensions(this.LutTexture) ? 1 : 0;
      bool flag1 = AmplifyColorBase.ValidateLutDimensions(this.LutBlendTexture);
      bool flag2 = (UnityEngine.Object) this.LutTexture == (UnityEngine.Object) null && (UnityEngine.Object) this.LutBlendTexture == (UnityEngine.Object) null && (UnityEngine.Object) this.volumesLutBlendTexture == (UnityEngine.Object) null;
      Texture source1 = (UnityEngine.Object) this.LutTexture == (UnityEngine.Object) null ? (Texture) this.defaultLut : this.LutTexture;
      Texture lutBlendTexture = this.LutBlendTexture;
      int shaderPass = this.ComputeShaderPass();
      bool flag3 = (double) this.BlendAmount != 0.0 || this.blending;
      bool flag4 = flag3 || flag3 && (UnityEngine.Object) lutBlendTexture != (UnityEngine.Object) null;
      bool flag5 = flag4;
      int num2 = (num1 == 0 ? 1 : (!flag1 ? 1 : 0)) | (flag2 ? 1 : 0);
      Material mat = num2 == 0 ? (flag4 || this.volumesBlending ? (!this.UseDepthMask ? ((UnityEngine.Object) this.MaskTexture != (UnityEngine.Object) null ? this.materialMaskBlend : this.materialBlend) : this.materialDepthMaskBlend) : (!this.UseDepthMask ? ((UnityEngine.Object) this.MaskTexture != (UnityEngine.Object) null ? this.materialMask : this.materialBase) : this.materialDepthMask)) : this.materialProcessOnly;
      mat.SetFloat("_Exposure", this.Exposure);
      mat.SetFloat("_ShoulderStrength", 0.22f);
      mat.SetFloat("_LinearStrength", 0.3f);
      mat.SetFloat("_LinearAngle", 0.1f);
      mat.SetFloat("_ToeStrength", 0.2f);
      mat.SetFloat("_ToeNumerator", 0.01f);
      mat.SetFloat("_ToeDenominator", 0.3f);
      mat.SetFloat("_LinearWhite", this.LinearWhitePoint);
      mat.SetFloat("_LerpAmount", this.BlendAmount);
      if ((UnityEngine.Object) this.MaskTexture != (UnityEngine.Object) null)
        mat.SetTexture("_MaskTex", this.MaskTexture);
      if (this.UseDepthMask)
        mat.SetTexture("_DepthCurveLut", (Texture) this.depthCurveLut);
      if (num2 == 0)
      {
        if (this.volumesBlending)
        {
          this.volumesBlendAmount = Mathf.Clamp01(this.volumesBlendAmount);
          this.materialBlendCache.SetFloat("_LerpAmount", this.volumesBlendAmount);
          if (this.blendingFromMidBlend)
            this.materialBlendCache.SetTexture("_RgbTex", (Texture) this.midBlendLUT);
          else
            this.materialBlendCache.SetTexture("_RgbTex", source1);
          this.materialBlendCache.SetTexture("_LerpRgbTex", (UnityEngine.Object) this.volumesLutBlendTexture != (UnityEngine.Object) null ? this.volumesLutBlendTexture : (Texture) this.defaultLut);
          Graphics.Blit(source1, this.blendCacheLut, this.materialBlendCache);
        }
        if (flag5)
        {
          this.materialBlendCache.SetFloat("_LerpAmount", this.BlendAmount);
          RenderTexture renderTexture = (RenderTexture) null;
          if (this.volumesBlending)
          {
            renderTexture = RenderTexture.GetTemporary(this.blendCacheLut.width, this.blendCacheLut.height, this.blendCacheLut.depth, this.blendCacheLut.format, RenderTextureReadWrite.Linear);
            Graphics.Blit((Texture) this.blendCacheLut, renderTexture);
            this.materialBlendCache.SetTexture("_RgbTex", (Texture) renderTexture);
          }
          else
            this.materialBlendCache.SetTexture("_RgbTex", source1);
          this.materialBlendCache.SetTexture("_LerpRgbTex", (UnityEngine.Object) lutBlendTexture != (UnityEngine.Object) null ? lutBlendTexture : (Texture) this.defaultLut);
          Graphics.Blit(source1, this.blendCacheLut, this.materialBlendCache);
          if ((UnityEngine.Object) renderTexture != (UnityEngine.Object) null)
            RenderTexture.ReleaseTemporary(renderTexture);
          mat.SetTexture("_RgbBlendCacheTex", (Texture) this.blendCacheLut);
        }
        else if (this.volumesBlending)
        {
          mat.SetTexture("_RgbBlendCacheTex", (Texture) this.blendCacheLut);
        }
        else
        {
          if ((UnityEngine.Object) source1 != (UnityEngine.Object) null)
            mat.SetTexture("_RgbTex", source1);
          if ((UnityEngine.Object) lutBlendTexture != (UnityEngine.Object) null)
            mat.SetTexture("_LerpRgbTex", lutBlendTexture);
        }
      }
      Graphics.Blit((Texture) source, destination, mat, shaderPass);
      if (!flag5 && !this.volumesBlending)
        return;
      this.blendCacheLut.DiscardContents();
    }
  }
}
