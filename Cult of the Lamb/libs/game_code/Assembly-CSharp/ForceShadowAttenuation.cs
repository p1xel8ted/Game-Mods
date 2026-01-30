// Decompiled with JetBrains decompiler
// Type: ForceShadowAttenuation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (MeshRenderer))]
[RequireComponent(typeof (MeshFilter))]
public class ForceShadowAttenuation : BaseMonoBehaviour
{
  public Material dummyMat;
  public Mesh dummyMesh;

  public MeshRenderer m_rend => this.gameObject.GetComponent<MeshRenderer>();

  public MeshFilter m_filter => this.gameObject.GetComponent<MeshFilter>();

  public void Awake() => this.Init();

  public void Start() => this.Init();

  public void Init()
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
