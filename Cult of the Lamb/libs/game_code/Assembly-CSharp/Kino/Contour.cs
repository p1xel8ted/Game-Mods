// Decompiled with JetBrains decompiler
// Type: Kino.Contour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Kino Image Effects/Contour")]
public class Contour : MonoBehaviour
{
  [SerializeField]
  public Color _lineColor = Color.black;
  [SerializeField]
  public Color _backgroundColor = new Color(1f, 1f, 1f, 0.0f);
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _lowerThreshold = 0.05f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _upperThreshold = 0.5f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _colorSensitivity;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _depthSensitivity = 0.5f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _normalSensitivity;
  [SerializeField]
  public float _fallOffDepth = 40f;
  [SerializeField]
  [HideInInspector]
  public Shader _shader;
  public Material _material;

  public Color lineColor
  {
    get => this._lineColor;
    set => this._lineColor = value;
  }

  public Color backgroundColor
  {
    get => this._backgroundColor;
    set => this._backgroundColor = value;
  }

  public float lowerThreshold
  {
    get => this._lowerThreshold;
    set => this._lowerThreshold = value;
  }

  public float upperThreshold
  {
    get => this._upperThreshold;
    set => this._upperThreshold = value;
  }

  public float colorSensitivity
  {
    get => this._colorSensitivity;
    set => this._colorSensitivity = value;
  }

  public float depthSensitivity
  {
    get => this._depthSensitivity;
    set => this._depthSensitivity = value;
  }

  public float normalSensitivity
  {
    get => this._normalSensitivity;
    set => this._normalSensitivity = value;
  }

  public float fallOffDepth
  {
    get => this._fallOffDepth;
    set => this._fallOffDepth = value;
  }

  public void OnValidate()
  {
    this._lowerThreshold = Mathf.Min(this._lowerThreshold, this._upperThreshold);
  }

  public void OnDestroy()
  {
    if (!((Object) this._material != (Object) null))
      return;
    if (Application.isPlaying)
      Object.Destroy((Object) this._material);
    else
      Object.DestroyImmediate((Object) this._material);
  }

  public void Update()
  {
    if ((double) this._depthSensitivity <= 0.0)
      return;
    this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    this._material.SetColor("_Color", this._lineColor);
    this._material.SetColor("_Background", this._backgroundColor);
    this._material.SetFloat("_Threshold", this._lowerThreshold);
    this._material.SetFloat("_InvRange", (float) (1.0 / ((double) this._upperThreshold - (double) this._lowerThreshold)));
    this._material.SetFloat("_ColorSensitivity", this._colorSensitivity);
    this._material.SetFloat("_DepthSensitivity", this._depthSensitivity * 2f);
    this._material.SetFloat("_NormalSensitivity", this._normalSensitivity);
    this._material.SetFloat("_InvFallOff", 1f / this._fallOffDepth);
    if ((double) this._colorSensitivity > 0.0)
      this._material.EnableKeyword("_CONTOUR_COLOR");
    else
      this._material.DisableKeyword("_CONTOUR_COLOR");
    if ((double) this._depthSensitivity > 0.0)
      this._material.EnableKeyword("_CONTOUR_DEPTH");
    else
      this._material.DisableKeyword("_CONTOUR_DEPTH");
    if ((double) this._normalSensitivity > 0.0)
      this._material.EnableKeyword("_CONTOUR_NORMAL");
    else
      this._material.DisableKeyword("_CONTOUR_NORMAL");
    Graphics.Blit((Texture) source, destination, this._material);
  }
}
