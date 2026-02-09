// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.CameraMotionBlur
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camera/Camera Motion Blur")]
[RequireComponent(typeof (Camera))]
public class CameraMotionBlur : PostEffectsBase
{
  public static float MAX_RADIUS = 10f;
  public CameraMotionBlur.MotionBlurFilter filterType = CameraMotionBlur.MotionBlurFilter.Reconstruction;
  public bool preview;
  public Vector3 previewScale = Vector3.one;
  public float movementScale;
  public float rotationScale = 1f;
  public float maxVelocity = 8f;
  public float minVelocity = 0.1f;
  public float velocityScale = 0.375f;
  public float softZDistance = 0.005f;
  public int velocityDownsample = 1;
  public LayerMask excludeLayers = (LayerMask) 0;
  public GameObject tmpCam;
  public Shader shader;
  public Shader dx11MotionBlurShader;
  public Shader replacementClear;
  public Material motionBlurMaterial;
  public Material dx11MotionBlurMaterial;
  public Texture2D noiseTexture;
  public float jitter = 0.05f;
  public bool showVelocity;
  public float showVelocityScale = 1f;
  public Matrix4x4 currentViewProjMat;
  public Matrix4x4 prevViewProjMat;
  public int prevFrameCount;
  public bool wasActive;
  public Vector3 prevFrameForward = Vector3.forward;
  public Vector3 prevFrameUp = Vector3.up;
  public Vector3 prevFramePos = Vector3.zero;
  public Camera _camera;

  public void CalculateViewProjection()
  {
    Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
    this.currentViewProjMat = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true) * worldToCameraMatrix;
  }

  public new void Start()
  {
    this.CheckResources();
    if ((UnityEngine.Object) this._camera == (UnityEngine.Object) null)
      this._camera = this.GetComponent<Camera>();
    this.wasActive = this.gameObject.activeInHierarchy;
    this.CalculateViewProjection();
    this.Remember();
    this.wasActive = false;
  }

  public new void OnEnable()
  {
    if ((UnityEngine.Object) this._camera == (UnityEngine.Object) null)
      this._camera = this.GetComponent<Camera>();
    this._camera.depthTextureMode |= DepthTextureMode.Depth;
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) null != (UnityEngine.Object) this.motionBlurMaterial)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.motionBlurMaterial);
      this.motionBlurMaterial = (Material) null;
    }
    if ((UnityEngine.Object) null != (UnityEngine.Object) this.dx11MotionBlurMaterial)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.dx11MotionBlurMaterial);
      this.dx11MotionBlurMaterial = (Material) null;
    }
    if (!((UnityEngine.Object) null != (UnityEngine.Object) this.tmpCam))
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.tmpCam);
    this.tmpCam = (GameObject) null;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true, true);
    this.motionBlurMaterial = this.CheckShaderAndCreateMaterial(this.shader, this.motionBlurMaterial);
    if (this.supportDX11 && this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11)
      this.dx11MotionBlurMaterial = this.CheckShaderAndCreateMaterial(this.dx11MotionBlurShader, this.dx11MotionBlurMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
        this.StartFrame();
      RenderTextureFormat format = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.RGHalf : RenderTextureFormat.ARGBHalf;
      RenderTexture temporary1 = RenderTexture.GetTemporary(CameraMotionBlur.divRoundUp(source.width, this.velocityDownsample), CameraMotionBlur.divRoundUp(source.height, this.velocityDownsample), 0, format);
      this.maxVelocity = Mathf.Max(2f, this.maxVelocity);
      float maxVelocity = this.maxVelocity;
      bool flag = this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && (UnityEngine.Object) this.dx11MotionBlurMaterial == (UnityEngine.Object) null;
      int width;
      int height;
      float num1;
      if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction | flag || this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
      {
        this.maxVelocity = Mathf.Min(this.maxVelocity, CameraMotionBlur.MAX_RADIUS);
        width = CameraMotionBlur.divRoundUp(temporary1.width, (int) this.maxVelocity);
        height = CameraMotionBlur.divRoundUp(temporary1.height, (int) this.maxVelocity);
        num1 = (float) (temporary1.width / width);
      }
      else
      {
        width = CameraMotionBlur.divRoundUp(temporary1.width, (int) this.maxVelocity);
        height = CameraMotionBlur.divRoundUp(temporary1.height, (int) this.maxVelocity);
        num1 = (float) (temporary1.width / width);
      }
      RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, format);
      RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, 0, format);
      temporary1.filterMode = FilterMode.Point;
      temporary2.filterMode = FilterMode.Point;
      temporary3.filterMode = FilterMode.Point;
      if ((bool) (UnityEngine.Object) this.noiseTexture)
        this.noiseTexture.filterMode = FilterMode.Point;
      source.wrapMode = TextureWrapMode.Clamp;
      temporary1.wrapMode = TextureWrapMode.Clamp;
      temporary3.wrapMode = TextureWrapMode.Clamp;
      temporary2.wrapMode = TextureWrapMode.Clamp;
      this.CalculateViewProjection();
      if (this.gameObject.activeInHierarchy && !this.wasActive)
        this.Remember();
      this.wasActive = this.gameObject.activeInHierarchy;
      Matrix4x4 matrix4x4 = Matrix4x4.Inverse(this.currentViewProjMat);
      this.motionBlurMaterial.SetMatrix("_InvViewProj", matrix4x4);
      this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
      this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x4);
      this.motionBlurMaterial.SetFloat("_MaxVelocity", num1);
      this.motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num1);
      this.motionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
      this.motionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
      this.motionBlurMaterial.SetFloat("_Jitter", this.jitter);
      this.motionBlurMaterial.SetTexture("_NoiseTex", (Texture) this.noiseTexture);
      this.motionBlurMaterial.SetTexture("_VelTex", (Texture) temporary1);
      this.motionBlurMaterial.SetTexture("_NeighbourMaxTex", (Texture) temporary3);
      this.motionBlurMaterial.SetTexture("_TileTexDebug", (Texture) temporary2);
      if (this.preview)
      {
        Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
        Matrix4x4 identity = Matrix4x4.identity;
        identity.SetTRS(this.previewScale * 0.3333f, Quaternion.identity, Vector3.one);
        this.prevViewProjMat = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true) * identity * worldToCameraMatrix;
        this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
        this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x4);
      }
      if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
      {
        Vector4 zero = Vector4.zero;
        float num2 = Vector3.Dot(this.transform.up, Vector3.up);
        Vector3 rhs = this.prevFramePos - this.transform.position;
        double magnitude = (double) rhs.magnitude;
        float num3 = (float) ((double) Vector3.Angle(this.transform.up, this.prevFrameUp) / (double) this._camera.fieldOfView * ((double) source.width * 0.75));
        zero.x = this.rotationScale * num3;
        float num4 = (float) ((double) Vector3.Angle(this.transform.forward, this.prevFrameForward) / (double) this._camera.fieldOfView * ((double) source.width * 0.75));
        zero.y = this.rotationScale * num2 * num4;
        float num5 = (float) ((double) Vector3.Angle(this.transform.forward, this.prevFrameForward) / (double) this._camera.fieldOfView * ((double) source.width * 0.75));
        zero.z = this.rotationScale * (1f - num2) * num5;
        double epsilon = (double) Mathf.Epsilon;
        if (magnitude > epsilon && (double) this.movementScale > (double) Mathf.Epsilon)
        {
          zero.w = (float) ((double) this.movementScale * (double) Vector3.Dot(this.transform.forward, rhs) * ((double) source.width * 0.5));
          zero.x += (float) ((double) this.movementScale * (double) Vector3.Dot(this.transform.up, rhs) * ((double) source.width * 0.5));
          zero.y += (float) ((double) this.movementScale * (double) Vector3.Dot(this.transform.right, rhs) * ((double) source.width * 0.5));
        }
        if (this.preview)
          this.motionBlurMaterial.SetVector("_BlurDirectionPacked", new Vector4(this.previewScale.y, this.previewScale.x, 0.0f, this.previewScale.z) * 0.5f * this._camera.fieldOfView);
        else
          this.motionBlurMaterial.SetVector("_BlurDirectionPacked", zero);
      }
      else
      {
        Graphics.Blit((Texture) source, temporary1, this.motionBlurMaterial, 0);
        Camera camera = (Camera) null;
        if (this.excludeLayers.value != 0)
          camera = this.GetTmpCam();
        if ((bool) (UnityEngine.Object) camera && this.excludeLayers.value != 0 && (bool) (UnityEngine.Object) this.replacementClear && this.replacementClear.isSupported)
        {
          camera.targetTexture = temporary1;
          camera.cullingMask = (int) this.excludeLayers;
          camera.RenderWithShader(this.replacementClear, "");
        }
      }
      if (!this.preview && Time.frameCount != this.prevFrameCount)
      {
        this.prevFrameCount = Time.frameCount;
        this.Remember();
      }
      source.filterMode = FilterMode.Bilinear;
      if (this.showVelocity)
      {
        this.motionBlurMaterial.SetFloat("_DisplayVelocityScale", this.showVelocityScale);
        Graphics.Blit((Texture) temporary1, destination, this.motionBlurMaterial, 1);
      }
      else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && !flag)
      {
        this.dx11MotionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
        this.dx11MotionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
        this.dx11MotionBlurMaterial.SetFloat("_Jitter", this.jitter);
        this.dx11MotionBlurMaterial.SetTexture("_NoiseTex", (Texture) this.noiseTexture);
        this.dx11MotionBlurMaterial.SetTexture("_VelTex", (Texture) temporary1);
        this.dx11MotionBlurMaterial.SetTexture("_NeighbourMaxTex", (Texture) temporary3);
        this.dx11MotionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
        this.dx11MotionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num1);
        Graphics.Blit((Texture) temporary1, temporary2, this.dx11MotionBlurMaterial, 0);
        Graphics.Blit((Texture) temporary2, temporary3, this.dx11MotionBlurMaterial, 1);
        Graphics.Blit((Texture) source, destination, this.dx11MotionBlurMaterial, 2);
      }
      else if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction | flag)
      {
        this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
        Graphics.Blit((Texture) temporary1, temporary2, this.motionBlurMaterial, 2);
        Graphics.Blit((Texture) temporary2, temporary3, this.motionBlurMaterial, 3);
        Graphics.Blit((Texture) source, destination, this.motionBlurMaterial, 4);
      }
      else if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
        Graphics.Blit((Texture) source, destination, this.motionBlurMaterial, 6);
      else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
      {
        this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
        Graphics.Blit((Texture) temporary1, temporary2, this.motionBlurMaterial, 2);
        Graphics.Blit((Texture) temporary2, temporary3, this.motionBlurMaterial, 3);
        Graphics.Blit((Texture) source, destination, this.motionBlurMaterial, 7);
      }
      else
        Graphics.Blit((Texture) source, destination, this.motionBlurMaterial, 5);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
    }
  }

  public void Remember()
  {
    this.prevViewProjMat = this.currentViewProjMat;
    this.prevFrameForward = this.transform.forward;
    this.prevFrameUp = this.transform.up;
    this.prevFramePos = this.transform.position;
  }

  public Camera GetTmpCam()
  {
    if ((UnityEngine.Object) this.tmpCam == (UnityEngine.Object) null)
    {
      string name = $"_{this._camera.name}_MotionBlurTmpCam";
      GameObject gameObject = GameObject.Find(name);
      if ((UnityEngine.Object) null == (UnityEngine.Object) gameObject)
        this.tmpCam = new GameObject(name, new System.Type[1]
        {
          typeof (Camera)
        });
      else
        this.tmpCam = gameObject;
    }
    this.tmpCam.hideFlags = HideFlags.DontSave;
    this.tmpCam.transform.position = this._camera.transform.position;
    this.tmpCam.transform.rotation = this._camera.transform.rotation;
    this.tmpCam.transform.localScale = this._camera.transform.localScale;
    this.tmpCam.GetComponent<Camera>().CopyFrom(this._camera);
    this.tmpCam.GetComponent<Camera>().enabled = false;
    this.tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
    this.tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
    return this.tmpCam.GetComponent<Camera>();
  }

  public void StartFrame()
  {
    this.prevFramePos = Vector3.Slerp(this.prevFramePos, this.transform.position, 0.75f);
  }

  public static int divRoundUp(int x, int d) => (x + d - 1) / d;

  public enum MotionBlurFilter
  {
    CameraMotion,
    LocalBlur,
    Reconstruction,
    ReconstructionDX11,
    ReconstructionDisc,
  }
}
