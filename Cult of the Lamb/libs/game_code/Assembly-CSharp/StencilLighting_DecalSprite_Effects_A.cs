// Decompiled with JetBrains decompiler
// Type: StencilLighting_DecalSprite_Effects_A
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class StencilLighting_DecalSprite_Effects_A : BaseMonoBehaviour, IStencilLighting
{
  [Range(1f, 15f)]
  public float BlendHeight = 1f;
  [Range(1f, 2f)]
  public float projScale = 1.27f;
  [Range(0.0f, 360f)]
  public float maskRotation;
  public Texture MaskShape;
  public bool constrainChildTransform = true;
  public static int scalePosOffsetID = Shader.PropertyToID("_ScalePosOffset");
  public static int maskRotationID = Shader.PropertyToID("_MaskRotation");
  public static int overrideProjScaleID = Shader.PropertyToID("_ProjectionScale");
  public static int overrideMaskID = Shader.PropertyToID("_MaskTex");
  public Transform m_targetDecalObj;
  public Renderer m_DecalRenderer;
  public MeshRenderer m_DecalMeshRenderer;
  public MeshFilter m_meshFilter;
  public Vector4 scalePosOffset;
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
    this.m_DecalMeshRenderer = this.gameObject.transform.GetComponentInChildren<MeshRenderer>(true);
    this.m_DecalRenderer = (Renderer) this.m_DecalMeshRenderer;
    this.m_targetDecalObj = this.m_DecalRenderer.transform;
    this.m_meshFilter = this.m_targetDecalObj.GetComponent<MeshFilter>();
  }

  public void ApplyProperties()
  {
    this.PropBlock.Clear();
    this.scalePosOffset = new Vector4(this.transform.localScale.x, this.transform.localScale.y, this.transform.position.x, this.transform.position.y);
    this.PropBlock.SetVector(StencilLighting_DecalSprite_Effects_A.scalePosOffsetID, this.scalePosOffset);
    this.PropBlock.SetFloat(StencilLighting_DecalSprite_Effects_A.overrideProjScaleID, this.projScale);
    this.PropBlock.SetFloat(StencilLighting_DecalSprite_Effects_A.maskRotationID, this.maskRotation);
    if ((Object) this.MaskShape != (Object) null)
      this.PropBlock.SetTexture(StencilLighting_DecalSprite_Effects_A.overrideMaskID, this.MaskShape);
    if (!((Object) this.m_DecalRenderer != (Object) null))
      return;
    this.m_DecalRenderer.SetPropertyBlock(this.PropBlock, 0);
  }

  public void Awake() => this.Init();

  public void Start()
  {
    this.Init();
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
    if ((Object) this.m_DecalRenderer == (Object) null)
      return;
    Gizmos.color = Color.red;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
  }

  public int GetMaterialID()
  {
    if (this.materialID == -1)
    {
      if ((Object) this.sharedMaterial == (Object) null)
        this.sharedMaterial = this.m_DecalRenderer.sharedMaterial;
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
      this.sharedMaterial = this.m_DecalRenderer.sharedMaterial;
    return this.sharedMaterial;
  }

  public Transform GetTransform() => this.m_targetDecalObj.transform;
}
