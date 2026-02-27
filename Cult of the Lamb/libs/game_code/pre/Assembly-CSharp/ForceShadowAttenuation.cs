// Decompiled with JetBrains decompiler
// Type: ForceShadowAttenuation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (MeshRenderer))]
[RequireComponent(typeof (MeshFilter))]
public class ForceShadowAttenuation : BaseMonoBehaviour
{
  private Material dummyMat;
  private Mesh dummyMesh;

  private MeshRenderer m_rend => this.gameObject.GetComponent<MeshRenderer>();

  private MeshFilter m_filter => this.gameObject.GetComponent<MeshFilter>();

  private void Awake() => this.Init();

  private void Start() => this.Init();

  private void Init()
  {
    if ((Object) this.dummyMat == (Object) null)
      this.dummyMat = new Material(Shader.Find("Hidden/ForwardBaseDummy"));
    if ((Object) this.m_rend.sharedMaterial == (Object) null)
      this.m_rend.sharedMaterial = this.dummyMat;
    if ((Object) this.dummyMesh == (Object) null)
      this.dummyMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
    this.m_rend.receiveShadows = true;
    this.m_rend.shadowCastingMode = ShadowCastingMode.On;
    this.dummyMesh.RecalculateBounds();
    this.m_filter.sharedMesh = this.dummyMesh;
  }

  public void OnDrawGizmos()
  {
    Bounds bounds1 = new Bounds();
    Bounds bounds2 = this.m_rend.bounds;
    Gizmos.color = Color.magenta;
    Gizmos.DrawWireCube(bounds2.center, bounds2.size);
  }
}
