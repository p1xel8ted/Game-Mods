// Decompiled with JetBrains decompiler
// Type: src.Rendering.MMCombinedMeshRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.U2D;

#nullable disable
namespace src.Rendering;

[RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
public class MMCombinedMeshRenderer : MonoBehaviour
{
  [Header("Input")]
  [SerializeField]
  private SpriteAtlas _atlas;
  [SerializeField]
  private Material _srcMaterial;
  [SerializeField]
  private Shader _newShaderSrc;
  [Header("Output")]
  [SerializeField]
  private MeshFilter _meshFilter;
  [SerializeField]
  private MeshRenderer _meshRenderer;
  [SerializeField]
  private Mesh _mesh;
  [SerializeField]
  private Material _material;

  private void Reset()
  {
    this._meshFilter = this.GetComponent<MeshFilter>();
    this._meshRenderer = this.GetComponent<MeshRenderer>();
  }

  private void Clear()
  {
    if ((Object) this._mesh != (Object) null)
      Object.DestroyImmediate((Object) this._mesh);
    if (!((Object) this._material != (Object) null))
      return;
    Object.DestroyImmediate((Object) this._material);
  }
}
