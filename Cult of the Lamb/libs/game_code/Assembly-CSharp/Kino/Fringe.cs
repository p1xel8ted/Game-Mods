// Decompiled with JetBrains decompiler
// Type: Kino.Fringe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Kino Image Effects/Fringe")]
public class Fringe : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _lateralShift = 0.3f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _axialStrength = 0.8f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _axialShift = 0.3f;
  [SerializeField]
  public Fringe.QualityLevel _axialQuality;
  [SerializeField]
  public Shader _shader;
  public Material _material;

  public float lateralShift
  {
    get => this._lateralShift;
    set => this._lateralShift = value;
  }

  public float axialStrength
  {
    get => this._axialStrength;
    set => this._axialStrength = value;
  }

  public float axialShift
  {
    get => this._axialShift;
    set => this._axialShift = value;
  }

  public Fringe.QualityLevel axialQuality
  {
    get => this._axialQuality;
    set => this._axialQuality = value;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    Camera component = this.GetComponent<Camera>();
    this._material.SetVector("_CameraAspect", new Vector4(component.aspect, 1f / component.aspect, 1f, 0.0f));
    this._material.SetFloat("_LateralShift", this._lateralShift);
    this._material.SetFloat("_AxialStrength", this._axialStrength);
    this._material.SetFloat("_AxialShift", this._axialShift);
    if ((double) this._axialStrength == 0.0)
    {
      this._material.DisableKeyword("AXIAL_SAMPLE_LOW");
      this._material.DisableKeyword("AXIAL_SAMPLE_HIGH");
    }
    else if (this._axialQuality == Fringe.QualityLevel.Low)
    {
      this._material.EnableKeyword("AXIAL_SAMPLE_LOW");
      this._material.DisableKeyword("AXIAL_SAMPLE_HIGH");
    }
    else
    {
      this._material.DisableKeyword("AXIAL_SAMPLE_LOW");
      this._material.EnableKeyword("AXIAL_SAMPLE_HIGH");
    }
    Graphics.Blit((Texture) source, destination, this._material, 0);
  }

  public enum QualityLevel
  {
    Low,
    High,
  }
}
