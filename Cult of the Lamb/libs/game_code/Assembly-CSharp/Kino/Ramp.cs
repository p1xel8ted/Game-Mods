// Decompiled with JetBrains decompiler
// Type: Kino.Ramp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Kino Image Effects/Ramp")]
public class Ramp : MonoBehaviour
{
  [SerializeField]
  public Color _color1 = Color.blue;
  [SerializeField]
  public Color _color2 = Color.red;
  [SerializeField]
  [Range(-180f, 180f)]
  public float _angle = 90f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _opacity = 1f;
  [SerializeField]
  public Ramp.BlendMode _blendMode = Ramp.BlendMode.Overlay;
  [SerializeField]
  public bool _debug;
  [SerializeField]
  public Shader _shader;
  public Material _material;
  public static string[] _blendModeKeywords = new string[5]
  {
    "_MULTIPLY",
    "_SCREEN",
    "_OVERLAY",
    "_HARDLIGHT",
    "_SOFTLIGHT"
  };

  public Color color1
  {
    get => this._color1;
    set => this._color1 = value;
  }

  public Color color2
  {
    get => this._color2;
    set => this._color2 = value;
  }

  public float angle
  {
    get => this._angle;
    set => this._angle = value;
  }

  public float opacity
  {
    get => this._opacity;
    set => this._opacity = value;
  }

  public int blendMode
  {
    get => (int) this._blendMode;
    set => this._blendMode = (Ramp.BlendMode) (value % 5);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((UnityEngine.Object) this._material == (UnityEngine.Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    Color a = this._blendMode != Ramp.BlendMode.Multiply ? (this._blendMode != Ramp.BlendMode.Screen ? Color.gray : Color.black) : Color.white;
    float t = this._debug ? 1f : this._opacity;
    this._material.SetColor("_Color1", Color.Lerp(a, this._color1, t));
    this._material.SetColor("_Color2", Color.Lerp(a, this._color2, t));
    float f = (float) Math.PI / 180f * this._angle;
    this._material.SetVector("_Direction", (Vector4) new Vector2(Mathf.Cos(f), Mathf.Sin(f)));
    this._material.shaderKeywords = (string[]) null;
    this._material.EnableKeyword(Ramp._blendModeKeywords[(int) this._blendMode]);
    if (QualitySettings.activeColorSpace == ColorSpace.Linear)
      this._material.EnableKeyword("_LINEAR");
    else
      this._material.DisableKeyword("_LINEAR");
    if (this._debug)
      this._material.EnableKeyword("_DEBUG");
    else
      this._material.DisableKeyword("_DEBUG");
    Graphics.Blit((Texture) source, destination, this._material, 0);
  }

  public enum BlendMode
  {
    Multiply,
    Screen,
    Overlay,
    HardLight,
    SoftLight,
    NumValues,
  }
}
