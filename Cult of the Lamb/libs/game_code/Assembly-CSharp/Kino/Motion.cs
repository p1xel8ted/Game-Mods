// Decompiled with JetBrains decompiler
// Type: Kino.Motion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Kino;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("Kino Image Effects/Motion")]
public class Motion : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 360f)]
  [Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
  public float _shutterAngle = 270f;
  [SerializeField]
  [Tooltip("The amount of sample points, which affects quality and performance.")]
  public int _sampleCount = 8;
  [SerializeField]
  [Range(0.0f, 1f)]
  [Tooltip("The strength of multiple frame blending")]
  public float _frameBlending;
  [SerializeField]
  public Shader _reconstructionShader;
  [SerializeField]
  public Shader _frameBlendingShader;
  public Motion.ReconstructionFilter _reconstructionFilter;
  public Motion.FrameBlendingFilter _frameBlendingFilter;

  public float shutterAngle
  {
    get => this._shutterAngle;
    set => this._shutterAngle = value;
  }

  public int sampleCount
  {
    get => this._sampleCount;
    set => this._sampleCount = value;
  }

  public float frameBlending
  {
    get => this._frameBlending;
    set => this._frameBlending = value;
  }

  public void OnEnable()
  {
    this._reconstructionFilter = new Motion.ReconstructionFilter();
    this._frameBlendingFilter = new Motion.FrameBlendingFilter();
  }

  public void OnDisable()
  {
    this._reconstructionFilter.Release();
    this._frameBlendingFilter.Release();
    this._reconstructionFilter = (Motion.ReconstructionFilter) null;
    this._frameBlendingFilter = (Motion.FrameBlendingFilter) null;
  }

  public void Update()
  {
    if ((double) this._shutterAngle <= 0.0)
      return;
    this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((double) this._shutterAngle > 0.0 && (double) this._frameBlending > 0.0)
    {
      RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
      this._reconstructionFilter.ProcessImage(this._shutterAngle, this._sampleCount, source, temporary);
      this._frameBlendingFilter.BlendFrames(this._frameBlending, temporary, destination);
      this._frameBlendingFilter.PushFrame(temporary);
      RenderTexture.ReleaseTemporary(temporary);
    }
    else if ((double) this._shutterAngle > 0.0)
      this._reconstructionFilter.ProcessImage(this._shutterAngle, this._sampleCount, source, destination);
    else if ((double) this._frameBlending > 0.0)
    {
      this._frameBlendingFilter.BlendFrames(this._frameBlending, source, destination);
      this._frameBlendingFilter.PushFrame(source);
    }
    else
      Graphics.Blit((Texture) source, destination);
  }

  public class FrameBlendingFilter
  {
    public bool _useCompression;
    public RenderTextureFormat _rawTextureFormat;
    public Material _material;
    public Motion.FrameBlendingFilter.Frame[] _frameList;
    public int _lastFrameCount;

    public FrameBlendingFilter()
    {
      this._useCompression = Motion.FrameBlendingFilter.CheckSupportCompression();
      this._rawTextureFormat = Motion.FrameBlendingFilter.GetPreferredRenderTextureFormat();
      this._material = new Material(Shader.Find("Hidden/Kino/Motion/FrameBlending"));
      this._material.hideFlags = HideFlags.DontSave;
      this._frameList = new Motion.FrameBlendingFilter.Frame[4];
    }

    public void Release()
    {
      Object.DestroyImmediate((Object) this._material);
      this._material = (Material) null;
      foreach (Motion.FrameBlendingFilter.Frame frame in this._frameList)
        frame.Release();
      this._frameList = (Motion.FrameBlendingFilter.Frame[]) null;
    }

    public void PushFrame(RenderTexture source)
    {
      int frameCount = Time.frameCount;
      if (frameCount == this._lastFrameCount)
        return;
      int index = frameCount % this._frameList.Length;
      if (this._useCompression)
        this._frameList[index].MakeRecord(source, this._material);
      else
        this._frameList[index].MakeRecordRaw(source, this._rawTextureFormat);
      this._lastFrameCount = frameCount;
    }

    public void BlendFrames(float strength, RenderTexture source, RenderTexture destination)
    {
      float time = Time.time;
      Motion.FrameBlendingFilter.Frame frameRelative1 = this.GetFrameRelative(-1);
      Motion.FrameBlendingFilter.Frame frameRelative2 = this.GetFrameRelative(-2);
      Motion.FrameBlendingFilter.Frame frameRelative3 = this.GetFrameRelative(-3);
      Motion.FrameBlendingFilter.Frame frameRelative4 = this.GetFrameRelative(-4);
      this._material.SetTexture("_History1LumaTex", (Texture) frameRelative1.lumaTexture);
      this._material.SetTexture("_History2LumaTex", (Texture) frameRelative2.lumaTexture);
      this._material.SetTexture("_History3LumaTex", (Texture) frameRelative3.lumaTexture);
      this._material.SetTexture("_History4LumaTex", (Texture) frameRelative4.lumaTexture);
      this._material.SetTexture("_History1ChromaTex", (Texture) frameRelative1.chromaTexture);
      this._material.SetTexture("_History2ChromaTex", (Texture) frameRelative2.chromaTexture);
      this._material.SetTexture("_History3ChromaTex", (Texture) frameRelative3.chromaTexture);
      this._material.SetTexture("_History4ChromaTex", (Texture) frameRelative4.chromaTexture);
      this._material.SetFloat("_History1Weight", frameRelative1.CalculateWeight(strength, time));
      this._material.SetFloat("_History2Weight", frameRelative2.CalculateWeight(strength, time));
      this._material.SetFloat("_History3Weight", frameRelative3.CalculateWeight(strength, time));
      this._material.SetFloat("_History4Weight", frameRelative4.CalculateWeight(strength, time));
      Graphics.Blit((Texture) source, destination, this._material, this._useCompression ? 1 : 2);
    }

    public static bool CheckSupportCompression()
    {
      return SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) && SystemInfo.supportedRenderTargetCount > 1;
    }

    public static RenderTextureFormat GetPreferredRenderTextureFormat()
    {
      RenderTextureFormat[] renderTextureFormatArray = new RenderTextureFormat[3]
      {
        RenderTextureFormat.RGB565,
        RenderTextureFormat.ARGB1555,
        RenderTextureFormat.ARGB4444
      };
      foreach (RenderTextureFormat format in renderTextureFormatArray)
      {
        if (SystemInfo.SupportsRenderTextureFormat(format))
          return format;
      }
      return RenderTextureFormat.Default;
    }

    public Motion.FrameBlendingFilter.Frame GetFrameRelative(int offset)
    {
      return this._frameList[(Time.frameCount + this._frameList.Length + offset) % this._frameList.Length];
    }

    public struct Frame
    {
      public RenderTexture lumaTexture;
      public RenderTexture chromaTexture;
      public float time;
      public RenderBuffer[] _mrt;

      public float CalculateWeight(float strength, float currentTime)
      {
        if ((double) this.time == 0.0)
          return 0.0f;
        float num = Mathf.Lerp(80f, 16f, strength);
        return Mathf.Exp((this.time - currentTime) * num);
      }

      public void Release()
      {
        if ((Object) this.lumaTexture != (Object) null)
          RenderTexture.ReleaseTemporary(this.lumaTexture);
        if ((Object) this.chromaTexture != (Object) null)
          RenderTexture.ReleaseTemporary(this.chromaTexture);
        this.lumaTexture = (RenderTexture) null;
        this.chromaTexture = (RenderTexture) null;
      }

      public void MakeRecord(RenderTexture source, Material material)
      {
        this.Release();
        this.lumaTexture = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
        this.chromaTexture = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
        this.lumaTexture.filterMode = FilterMode.Point;
        this.chromaTexture.filterMode = FilterMode.Point;
        if (this._mrt == null)
          this._mrt = new RenderBuffer[2];
        this._mrt[0] = this.lumaTexture.colorBuffer;
        this._mrt[1] = this.chromaTexture.colorBuffer;
        Graphics.SetRenderTarget(this._mrt, this.lumaTexture.depthBuffer);
        Graphics.Blit((Texture) source, material, 0);
        this.time = Time.time;
      }

      public void MakeRecordRaw(RenderTexture source, RenderTextureFormat format)
      {
        this.Release();
        this.lumaTexture = RenderTexture.GetTemporary(source.width, source.height, 0, format);
        this.lumaTexture.filterMode = FilterMode.Point;
        Graphics.Blit((Texture) source, this.lumaTexture);
        this.time = Time.time;
      }
    }
  }

  public class ReconstructionFilter
  {
    public const float kMaxBlurRadius = 5f;
    public Material _material;
    public RenderTextureFormat _vectorRTFormat = RenderTextureFormat.RGHalf;
    public RenderTextureFormat _packedRTFormat = RenderTextureFormat.ARGB2101010;

    public ReconstructionFilter()
    {
      Shader shader = Shader.Find("Hidden/Kino/Motion/Reconstruction");
      if (!shader.isSupported || !this.CheckTextureFormatSupport())
        return;
      this._material = new Material(shader);
      this._material.hideFlags = HideFlags.DontSave;
    }

    public void Release()
    {
      if ((Object) this._material != (Object) null)
        Object.DestroyImmediate((Object) this._material);
      this._material = (Material) null;
    }

    public void ProcessImage(
      float shutterAngle,
      int sampleCount,
      RenderTexture source,
      RenderTexture destination)
    {
      if ((Object) this._material == (Object) null)
      {
        Graphics.Blit((Texture) source, destination);
      }
      else
      {
        int num = (int) (5.0 * (double) source.height / 100.0);
        int divider = ((num - 1) / 8 + 1) * 8;
        this._material.SetFloat("_VelocityScale", shutterAngle / 360f);
        this._material.SetFloat("_MaxBlurRadius", (float) num);
        this._material.SetFloat("_RcpMaxBlurRadius", 1f / (float) num);
        RenderTexture temporaryRt1 = this.GetTemporaryRT((Texture) source, 1, this._packedRTFormat);
        Graphics.Blit((Texture) null, temporaryRt1, this._material, 0);
        RenderTexture temporaryRt2 = this.GetTemporaryRT((Texture) source, 2, this._vectorRTFormat);
        Graphics.Blit((Texture) temporaryRt1, temporaryRt2, this._material, 1);
        RenderTexture temporaryRt3 = this.GetTemporaryRT((Texture) source, 4, this._vectorRTFormat);
        Graphics.Blit((Texture) temporaryRt2, temporaryRt3, this._material, 2);
        this.ReleaseTemporaryRT(temporaryRt2);
        RenderTexture temporaryRt4 = this.GetTemporaryRT((Texture) source, 8, this._vectorRTFormat);
        Graphics.Blit((Texture) temporaryRt3, temporaryRt4, this._material, 2);
        this.ReleaseTemporaryRT(temporaryRt3);
        this._material.SetVector("_TileMaxOffs", (Vector4) (Vector2.one * (float) ((double) divider / 8.0 - 1.0) * -0.5f));
        this._material.SetInt("_TileMaxLoop", divider / 8);
        RenderTexture temporaryRt5 = this.GetTemporaryRT((Texture) source, divider, this._vectorRTFormat);
        Graphics.Blit((Texture) temporaryRt4, temporaryRt5, this._material, 3);
        this.ReleaseTemporaryRT(temporaryRt4);
        RenderTexture temporaryRt6 = this.GetTemporaryRT((Texture) source, divider, this._vectorRTFormat);
        Graphics.Blit((Texture) temporaryRt5, temporaryRt6, this._material, 4);
        this.ReleaseTemporaryRT(temporaryRt5);
        this._material.SetFloat("_LoopCount", (float) (Mathf.Clamp(sampleCount, 2, 64 /*0x40*/) / 2));
        this._material.SetTexture("_NeighborMaxTex", (Texture) temporaryRt6);
        this._material.SetTexture("_VelocityTex", (Texture) temporaryRt1);
        Graphics.Blit((Texture) source, destination, this._material, 5);
        this.ReleaseTemporaryRT(temporaryRt1);
        this.ReleaseTemporaryRT(temporaryRt6);
      }
    }

    public bool CheckTextureFormatSupport()
    {
      if (!SystemInfo.SupportsRenderTextureFormat(this._vectorRTFormat))
        return false;
      if (!SystemInfo.SupportsRenderTextureFormat(this._packedRTFormat))
        this._packedRTFormat = RenderTextureFormat.ARGB32;
      return true;
    }

    public RenderTexture GetTemporaryRT(Texture source, int divider, RenderTextureFormat format)
    {
      int width = source.width / divider;
      int num = source.height / divider;
      RenderTextureReadWrite textureReadWrite = RenderTextureReadWrite.Linear;
      int height = num;
      int format1 = (int) format;
      int readWrite = (int) textureReadWrite;
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, (RenderTextureFormat) format1, (RenderTextureReadWrite) readWrite);
      temporary.filterMode = FilterMode.Point;
      return temporary;
    }

    public void ReleaseTemporaryRT(RenderTexture rt) => RenderTexture.ReleaseTemporary(rt);
  }
}
