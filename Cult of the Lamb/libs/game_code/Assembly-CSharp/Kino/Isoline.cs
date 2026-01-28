// Decompiled with JetBrains decompiler
// Type: Kino.Isoline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[AddComponentMenu("Kino Image Effects/Isoline")]
public class Isoline : MonoBehaviour
{
  [SerializeField]
  [ColorUsage(true, true)]
  public Color _lineColor = Color.white;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _luminanceBlending = 1f;
  [SerializeField]
  public float _fallOffDepth = 40f;
  [SerializeField]
  public Color _backgroundColor = Color.black;
  [SerializeField]
  public Vector3 _axis = Vector3.one * 0.577f;
  [SerializeField]
  public float _interval = 0.25f;
  [SerializeField]
  public Vector3 _offset;
  [SerializeField]
  public float _distortionFrequency = 1f;
  [SerializeField]
  public float _distortionAmount;
  [SerializeField]
  public Isoline.ModulationMode _modulationMode;
  [SerializeField]
  public Vector3 _modulationAxis = Vector3.forward;
  [SerializeField]
  public float _modulationFrequency = 0.2f;
  [SerializeField]
  public float _modulationSpeed = 1f;
  [SerializeField]
  [Range(1f, 50f)]
  public float _modulationExponent = 24f;
  [SerializeField]
  public Shader _shader;
  public Material _material;
  public float _modulationTime;

  public Color lineColor
  {
    get => this._lineColor;
    set => this._lineColor = value;
  }

  public float luminanceBlending
  {
    get => this._luminanceBlending;
    set => this._luminanceBlending = value;
  }

  public float fallOffDepth
  {
    get => this._fallOffDepth;
    set => this._fallOffDepth = value;
  }

  public Color backgroundColor
  {
    get => this._backgroundColor;
    set => this._backgroundColor = value;
  }

  public Vector3 axis
  {
    get => this._axis;
    set => this._axis = value;
  }

  public float interval
  {
    get => this._interval;
    set => this._interval = value;
  }

  public Vector3 offset
  {
    get => this._offset;
    set => this._offset = value;
  }

  public float distortionFrequency
  {
    get => this._distortionFrequency;
    set => this._distortionFrequency = value;
  }

  public float distortionAmount
  {
    get => this._distortionAmount;
    set => this._distortionAmount = value;
  }

  public Isoline.ModulationMode modulationMode
  {
    get => this._modulationMode;
    set => this._modulationMode = value;
  }

  public Vector3 modulationAxis
  {
    get => this._modulationAxis;
    set => this._modulationAxis = value;
  }

  public float modulationFrequency
  {
    get => this._modulationFrequency;
    set => this._modulationFrequency = value;
  }

  public float modulationSpeed
  {
    get => this._modulationSpeed;
    set => this._modulationSpeed = value;
  }

  public float modulationExponent
  {
    get => this._modulationExponent;
    set => this._modulationExponent = value;
  }

  public void OnEnable()
  {
    if (!((Object) this.GetComponent<Camera>() != (Object) null))
      return;
    this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
  }

  public void Update() => this._modulationTime += Time.deltaTime * this._modulationSpeed;

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    this._material.SetMatrix("_InverseView", this.GetComponent<Camera>().cameraToWorldMatrix);
    this._material.SetColor("_Color", this._lineColor);
    this._material.SetFloat("_FallOffDepth", this._fallOffDepth);
    this._material.SetFloat("_Blend", this._luminanceBlending);
    this._material.SetColor("_BgColor", this._backgroundColor);
    this._material.SetVector("_Axis", (Vector4) this._axis.normalized);
    this._material.SetFloat("_Density", 1f / this._interval);
    this._material.SetVector("_Offset", (Vector4) this._offset);
    this._material.SetFloat("_DistFreq", this._distortionFrequency);
    this._material.SetFloat("_DistAmp", this._distortionAmount);
    if ((double) this._distortionAmount > 0.0)
      this._material.EnableKeyword("DISTORTION");
    else
      this._material.DisableKeyword("DISTORTION");
    this._material.DisableKeyword("MODULATION_FRAC");
    this._material.DisableKeyword("MODULATION_SIN");
    this._material.DisableKeyword("MODULATION_NOISE");
    if (this._modulationMode == Isoline.ModulationMode.Frac)
      this._material.EnableKeyword("MODULATION_FRAC");
    else if (this._modulationMode == Isoline.ModulationMode.Sin)
      this._material.EnableKeyword("MODULATION_SIN");
    else if (this._modulationMode == Isoline.ModulationMode.Noise)
      this._material.EnableKeyword("MODULATION_NOISE");
    float modulationFrequency = this._modulationFrequency;
    if (this._modulationMode == Isoline.ModulationMode.Sin)
      modulationFrequency *= 6.28318548f;
    this._material.SetVector("_ModAxis", (Vector4) this._modulationAxis.normalized);
    this._material.SetFloat("_ModFreq", modulationFrequency);
    this._material.SetFloat("_ModTime", this._modulationTime);
    this._material.SetFloat("_ModExp", this._modulationExponent);
    Graphics.Blit((Texture) source, destination, this._material);
  }

  public enum ModulationMode
  {
    None,
    Frac,
    Sin,
    Noise,
  }
}
