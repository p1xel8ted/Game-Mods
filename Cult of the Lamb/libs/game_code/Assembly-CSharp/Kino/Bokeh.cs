// Decompiled with JetBrains decompiler
// Type: Kino.Bokeh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class Bokeh : MonoBehaviour
{
  [SerializeField]
  [FormerlySerializedAs("_subject")]
  public Transform _pointOfFocus;
  [SerializeField]
  [FormerlySerializedAs("_distance")]
  public float _focusDistance = 10f;
  [SerializeField]
  public float _fNumber = 1.4f;
  [SerializeField]
  public bool _useCameraFov = true;
  [SerializeField]
  public float _focalLength = 0.05f;
  [SerializeField]
  [FormerlySerializedAs("_sampleCount")]
  public Bokeh.KernelSize _kernelSize = Bokeh.KernelSize.Medium;
  public const float kFilmHeight = 0.024f;
  [SerializeField]
  public Shader _shader;
  public Material _material;

  public Transform pointOfFocus
  {
    get => this._pointOfFocus;
    set => this._pointOfFocus = value;
  }

  public float focusDistance
  {
    get => this._focusDistance;
    set => this._focusDistance = value;
  }

  public float fNumber
  {
    get => this._fNumber;
    set => this._fNumber = value;
  }

  public bool useCameraFov
  {
    get => this._useCameraFov;
    set => this._useCameraFov = value;
  }

  public float focalLength
  {
    get => this._focalLength;
    set => this._focalLength = value;
  }

  public Bokeh.KernelSize kernelSize
  {
    get => this._kernelSize;
    set => this._kernelSize = value;
  }

  public Camera TargetCamera => this.GetComponent<Camera>();

  public float CalculateFocusDistance()
  {
    if ((UnityEngine.Object) this._pointOfFocus == (UnityEngine.Object) null)
      return this._focusDistance;
    Transform transform = this.TargetCamera.transform;
    return Vector3.Dot(this._pointOfFocus.position - transform.position, transform.forward);
  }

  public float CalculateFocalLength()
  {
    return !this._useCameraFov ? this._focalLength : 0.012f / Mathf.Tan(0.5f * (this.TargetCamera.fieldOfView * ((float) Math.PI / 180f)));
  }

  public float CalculateMaxCoCRadius(int screenHeight)
  {
    return Mathf.Min(0.05f, (float) ((double) this._kernelSize * 4.0 + 6.0) / (float) screenHeight);
  }

  public void SetUpShaderParameters(RenderTexture source)
  {
    float focusDistance = this.CalculateFocusDistance();
    float focalLength = this.CalculateFocalLength();
    float num = Mathf.Max(focusDistance, focalLength);
    this._material.SetFloat("_Distance", num);
    this._material.SetFloat("_LensCoeff", (float) ((double) focalLength * (double) focalLength / ((double) this._fNumber * ((double) num - (double) focalLength) * 0.024000000208616257 * 2.0)));
    float maxCoCradius = this.CalculateMaxCoCRadius(source.height);
    this._material.SetFloat("_MaxCoC", maxCoCradius);
    this._material.SetFloat("_RcpMaxCoC", 1f / maxCoCradius);
    this._material.SetFloat("_RcpAspect", (float) source.height / (float) source.width);
  }

  public void OnEnable()
  {
    Shader shader = Shader.Find("Hidden/Kino/Bokeh");
    if (!shader.isSupported || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
      return;
    if ((UnityEngine.Object) this._material == (UnityEngine.Object) null)
    {
      this._material = new Material(shader);
      this._material.hideFlags = HideFlags.HideAndDontSave;
    }
    this.TargetCamera.depthTextureMode |= DepthTextureMode.Depth;
  }

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) this._material != (UnityEngine.Object) null))
      return;
    if (Application.isPlaying)
      UnityEngine.Object.Destroy((UnityEngine.Object) this._material);
    else
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this._material);
  }

  public void Update()
  {
    if ((double) this._focusDistance < 0.0099999997764825821)
      this._focusDistance = 0.01f;
    if ((double) this._fNumber >= 0.10000000149011612)
      return;
    this._fNumber = 0.1f;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((UnityEngine.Object) this._material == (UnityEngine.Object) null)
    {
      Graphics.Blit((Texture) source, destination);
      if (!Application.isPlaying)
        return;
      this.enabled = false;
    }
    else
    {
      int width = source.width;
      int height = source.height;
      RenderTextureFormat format = RenderTextureFormat.ARGBHalf;
      this.SetUpShaderParameters(source);
      RenderTexture temporary1 = RenderTexture.GetTemporary(width / 2, height / 2, 0, format);
      source.filterMode = FilterMode.Point;
      Graphics.Blit((Texture) source, temporary1, this._material, 0);
      RenderTexture temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, format);
      temporary1.filterMode = FilterMode.Bilinear;
      Graphics.Blit((Texture) temporary1, temporary2, this._material, (int) (1 + this._kernelSize));
      temporary2.filterMode = FilterMode.Bilinear;
      Graphics.Blit((Texture) temporary2, temporary1, this._material, 5);
      this._material.SetTexture("_BlurTex", (Texture) temporary1);
      Graphics.Blit((Texture) source, destination, this._material, 6);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
    }
  }

  public enum KernelSize
  {
    Small,
    Medium,
    Large,
    VeryLarge,
  }
}
