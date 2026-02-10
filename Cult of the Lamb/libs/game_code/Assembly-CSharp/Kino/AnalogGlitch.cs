// Decompiled with JetBrains decompiler
// Type: Kino.AnalogGlitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[AddComponentMenu("Kino Image Effects/Analog Glitch")]
public class AnalogGlitch : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _scanLineJitter;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _verticalJump;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _horizontalShake;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _colorDrift;
  [SerializeField]
  public Shader _shader;
  public Material _material;
  public float _verticalJumpTime;

  public float scanLineJitter
  {
    get => this._scanLineJitter;
    set => this._scanLineJitter = value;
  }

  public float verticalJump
  {
    get => this._verticalJump;
    set => this._verticalJump = value;
  }

  public float horizontalShake
  {
    get => this._horizontalShake;
    set => this._horizontalShake = value;
  }

  public float colorDrift
  {
    get => this._colorDrift;
    set => this._colorDrift = value;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    this._verticalJumpTime += (float) ((double) Time.deltaTime * (double) this._verticalJump * 11.300000190734863);
    float y = Mathf.Clamp01((float) (1.0 - (double) this._scanLineJitter * 1.2000000476837158));
    this._material.SetVector("_ScanLineJitter", (Vector4) new Vector2((float) (1.0 / 500.0 + (double) Mathf.Pow(this._scanLineJitter, 3f) * 0.05000000074505806), y));
    this._material.SetVector("_VerticalJump", (Vector4) new Vector2(this._verticalJump, this._verticalJumpTime));
    this._material.SetFloat("_HorizontalShake", this._horizontalShake * 0.2f);
    this._material.SetVector("_ColorDrift", (Vector4) new Vector2(this._colorDrift * 0.04f, Time.time * 606.11f));
    Graphics.Blit((Texture) source, destination, this._material);
  }
}
