// Decompiled with JetBrains decompiler
// Type: StencilLighting_DecalSprite_Fill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class StencilLighting_DecalSprite_Fill : BaseMonoBehaviour, IStencilLighting
{
  public bool OverrideHeight;
  [Range(1f, 15f)]
  public float BlendHeight = 1f;
  public bool useInfluenceOverride;
  [SerializeField]
  public float overrideInfluence = 1f;
  public float oldInfluence;
  public bool hideChild = true;
  public bool constrainChildTransform = true;
  public bool negativeScaleFix = true;
  public const float defaultHeight = 60f;
  public static int overrideInfluenceID = Shader.PropertyToID("_StencilInfluence");
  public Transform m_targetDecalObj;
  public Renderer m_renderer;
  public MeshRenderer m_DecalMeshRenderer;
  public MeshFilter m_meshFilter;
  public MaterialPropertyBlock m_propBlock;
  public bool isVisible;
  public float visibilityTimer;
  public int materialID = -1;
  public Material sharedMaterial;

  public MaterialPropertyBlock PropBlock
  {
    get
    {
      if (this.m_propBlock == null)
        this.m_propBlock = new MaterialPropertyBlock();
      return this.m_propBlock;
    }
  }

  public void Init()
  {
    this.oldInfluence = this.overrideInfluence;
    this.m_DecalMeshRenderer = this.gameObject.transform.GetComponentInChildren<MeshRenderer>(true);
    this.m_renderer = (Renderer) this.m_DecalMeshRenderer;
    this.m_targetDecalObj = this.m_renderer.transform;
    this.m_meshFilter = this.m_targetDecalObj.GetComponent<MeshFilter>();
  }

  public void ApplyProperties()
  {
    this.PropBlock.Clear();
    this.PropBlock.SetFloat(StencilLighting_DecalSprite_Fill.overrideInfluenceID, this.overrideInfluence);
    if ((Object) this.m_renderer != (Object) null)
      this.m_renderer.SetPropertyBlock(this.PropBlock, 0);
    this.oldInfluence = this.overrideInfluence;
  }

  public void NegativeScaleFix()
  {
    Vector3 vector3 = new Vector3(this.m_targetDecalObj.localScale.x, this.m_targetDecalObj.localScale.y, this.m_targetDecalObj.localScale.z);
    if ((double) this.m_targetDecalObj.lossyScale.x < 0.0)
      vector3.x *= -1f;
    if ((double) this.m_targetDecalObj.lossyScale.y < 0.0)
      vector3.y *= -1f;
    if ((double) this.m_targetDecalObj.lossyScale.z >= 0.0)
      return;
    vector3.z *= -1f;
  }

  public void ResetRotation()
  {
    if (!Application.isPlaying)
      return;
    Transform parent = this.m_targetDecalObj.parent;
    this.m_targetDecalObj.parent = (Transform) null;
    this.m_targetDecalObj.localRotation = Quaternion.Euler(new Vector3(-135f, 0.0f, 0.0f));
    this.m_targetDecalObj.parent = parent;
  }

  public void SetScale()
  {
    float num = 60f;
    if (this.OverrideHeight)
      num = this.BlendHeight;
    if (Mathf.Approximately(this.gameObject.transform.localScale.z, num))
      return;
    this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, num);
  }

  public void Awake()
  {
    this.Init();
    this.SetScale();
    if (this.negativeScaleFix)
      this.NegativeScaleFix();
    if (!this.constrainChildTransform)
      return;
    this.ResetRotation();
  }

  public void Start()
  {
    this.Init();
    if (!this.useInfluenceOverride)
      return;
    this.ApplyProperties();
  }

  public void UpdateVisibility()
  {
  }

  public void OnDisable()
  {
  }

  public void Update() => this.UpdateVisibility();

  public void OnDrawGizmos()
  {
    if ((Object) this.m_renderer == (Object) null)
      return;
    Gizmos.color = Color.yellow;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
  }

  public int GetMaterialID()
  {
    if (this.materialID == -1)
    {
      if ((Object) this.sharedMaterial == (Object) null)
        this.sharedMaterial = this.m_renderer.sharedMaterial;
      if ((Object) this.sharedMaterial != (Object) null)
        this.materialID = this.sharedMaterial.GetInstanceID();
    }
    return this.materialID;
  }

  public int GetGameObjectID() => this.m_targetDecalObj.GetInstanceID();

  public MeshFilter GetMeshFilter() => this.m_meshFilter;

  public Material GetSharedMaterial()
  {
    if ((Object) this.sharedMaterial == (Object) null)
      this.sharedMaterial = this.m_renderer.sharedMaterial;
    return this.sharedMaterial;
  }

  public Transform GetTransform() => this.m_targetDecalObj.transform;
}
