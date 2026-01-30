// Decompiled with JetBrains decompiler
// Type: VFXMaterialOverride
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
[Serializable]
public class VFXMaterialOverride
{
  public string _materialPropertyName = "_MainColor";
  [SerializeField]
  public ShaderPropertyType _type;
  [SerializeField]
  [ColorUsage(true, true)]
  public Color _color;
  [SerializeField]
  public Texture _texture;
  [SerializeField]
  public float _float;
  [SerializeField]
  public Vector4 _vector;
  [SerializeField]
  public List<Renderer> _renderers;

  public VFXMaterialOverride()
  {
  }

  public VFXMaterialOverride(string materialPropertyName, List<Renderer> renderers, Color color)
  {
    this._materialPropertyName = materialPropertyName;
    this._renderers = renderers;
    this._type = ShaderPropertyType.Color;
    this._color = color;
  }

  public VFXMaterialOverride(
    string materialPropertyName,
    List<Renderer> renderers,
    Texture texture)
  {
    this._materialPropertyName = materialPropertyName;
    this._renderers = renderers;
    this._type = ShaderPropertyType.Texture;
    this._texture = texture;
  }

  public VFXMaterialOverride(string materialPropertyName, List<Renderer> renderers, float f)
  {
    this._materialPropertyName = materialPropertyName;
    this._renderers = renderers;
    this._type = ShaderPropertyType.Float;
    this._float = f;
  }

  public void Apply(ref MaterialPropertyBlock propertyBlock)
  {
    switch (this._type)
    {
      case ShaderPropertyType.Color:
        propertyBlock.SetColor(this._materialPropertyName, this._color);
        break;
      case ShaderPropertyType.Vector:
        propertyBlock.SetVector(this._materialPropertyName, this._vector);
        break;
      case ShaderPropertyType.Float:
      case ShaderPropertyType.Range:
        propertyBlock.SetFloat(this._materialPropertyName, this._float);
        break;
      case ShaderPropertyType.Texture:
        propertyBlock.SetTexture(this._materialPropertyName, this._texture);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    foreach (Renderer renderer in this._renderers)
      renderer.SetPropertyBlock(propertyBlock);
  }
}
