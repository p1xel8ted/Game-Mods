// Decompiled with JetBrains decompiler
// Type: src.Rendering.MMCombinedMeshRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.U2D;

#nullable disable
namespace src.Rendering;

[RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
public class MMCombinedMeshRenderer : MonoBehaviour
{
  [Header("Input")]
  [SerializeField]
  public SpriteAtlas _atlas;
  [SerializeField]
  public Material _srcMaterial;
  [SerializeField]
  public Shader _newShaderSrc;
  [Header("Output")]
  [SerializeField]
  public MeshFilter _meshFilter;
  [SerializeField]
  public MeshRenderer _meshRenderer;
  [SerializeField]
  public Mesh _mesh;
  [SerializeField]
  public Material _material;

  public void Reset()
  {
    this._meshFilter = this.GetComponent<MeshFilter>();
    this._meshRenderer = this.GetComponent<MeshRenderer>();
  }

  public void Clear()
  {
    if ((Object) this._mesh != (Object) null)
      Object.DestroyImmediate((Object) this._mesh);
    if (!((Object) this._material != (Object) null))
      return;
    Object.DestroyImmediate((Object) this._material);
  }
}
