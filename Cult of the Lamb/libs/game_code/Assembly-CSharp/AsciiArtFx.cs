// Decompiled with JetBrains decompiler
// Type: AsciiArtFx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
[ExecuteInEditMode]
public class AsciiArtFx : MonoBehaviour
{
  public Color colorTint = Color.white;
  public Dictionary<int, float> knobValues = new Dictionary<int, float>();
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _blendRatio = 1f;
  [SerializeField]
  [Range(0.5f, 10f)]
  public float _scaleFactor = 1f;
  [SerializeField]
  public Shader shader;
  public Material _material;
  public AsciiArtFx.FloatEvent OnKnobValueChanged;

  public float blendRatio
  {
    get => this._blendRatio;
    set => this._blendRatio = value;
  }

  public float scaleFactor
  {
    get => this._scaleFactor;
    set => this._scaleFactor = value;
  }

  public Material material
  {
    get
    {
      if ((Object) this._material == (Object) null)
      {
        this._material = new Material(this.shader);
        this._material.hideFlags = HideFlags.HideAndDontSave;
      }
      return this._material;
    }
  }

  public void Start()
  {
    if (this.OnKnobValueChanged != null)
      return;
    this.OnKnobValueChanged = new AsciiArtFx.FloatEvent();
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.material.color = this.colorTint;
    this.material.SetFloat("_Alpha", this.blendRatio);
    this.material.SetFloat("_Scale", this.scaleFactor);
    Graphics.Blit((Texture) source, destination, this.material);
  }

  public void OnDisable()
  {
    if (!(bool) (Object) this._material)
      return;
    Object.DestroyImmediate((Object) this._material);
  }

  public class FloatEvent : UnityEvent<float>
  {
  }
}
