// Decompiled with JetBrains decompiler
// Type: StencilLighting_DecalSprite_Additive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class StencilLighting_DecalSprite_Additive : BaseMonoBehaviour, IStencilLighting
{
  [Range(1f, 15f)]
  public float BlendHeight = 1f;
  [Range(1f, 2f)]
  public float projScale = 1.27f;
  [ColorUsage(true, true)]
  public Color AdditiveColor = Color.white;
  public Texture TextureOverride;
  public bool renderToStencilTexture = true;
  public bool hideChild = true;
  public bool constrainChildTransform = true;
  public bool negativeScaleFix = true;
  public static int overrideInfluenceID = Shader.PropertyToID("_StencilInfluence");
  public static int overrideColorID = Shader.PropertyToID("_ColorInstanceOverride");
  public static int overrideProjScaleID = Shader.PropertyToID("_ProjectionScale");
  public static int overrideTextureID = Shader.PropertyToID("_MainTex");
  public Transform m_targetDecalObj;
  public Transform m_targetAddObj;
  public Renderer m_DecalRenderer;
  public MeshRenderer m_DecalMeshRenderer;
  public MeshFilter m_meshFilter;
  public Renderer m_AddRenderer;
  public bool isVisible;
  public float visibilityTimer;
  public int materialID = -1;
  public Material sharedMaterial;
  public MaterialPropertyBlock m_propBlock;

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
    this.m_targetAddObj = this.m_targetDecalObj.GetChild(0);
    this.m_AddRenderer = (Renderer) this.m_targetAddObj.GetComponent<MeshRenderer>();
  }

  public void ApplyProperties()
  {
    this.m_DecalRenderer.enabled = this.renderToStencilTexture;
    this.PropBlock.Clear();
    this.PropBlock.SetFloat(StencilLighting_DecalSprite_Additive.overrideInfluenceID, this.AdditiveColor.a);
    this.PropBlock.SetFloat(StencilLighting_DecalSprite_Additive.overrideProjScaleID, this.projScale);
    if ((Object) this.TextureOverride != (Object) null)
      this.PropBlock.SetTexture(StencilLighting_DecalSprite_Additive.overrideTextureID, this.TextureOverride);
    if ((Object) this.m_DecalRenderer != (Object) null && this.m_DecalRenderer.enabled)
      this.m_DecalRenderer.SetPropertyBlock(this.PropBlock, 0);
    this.PropBlock.Clear();
    this.PropBlock.SetColor(StencilLighting_DecalSprite_Additive.overrideColorID, this.AdditiveColor);
    this.PropBlock.SetFloat(StencilLighting_DecalSprite_Additive.overrideInfluenceID, 1f);
    this.PropBlock.SetFloat(StencilLighting_DecalSprite_Additive.overrideProjScaleID, this.projScale);
    if ((Object) this.TextureOverride != (Object) null)
      this.PropBlock.SetTexture(StencilLighting_DecalSprite_Additive.overrideTextureID, this.TextureOverride);
    if (!((Object) this.m_AddRenderer != (Object) null))
      return;
    this.m_AddRenderer.SetPropertyBlock(this.PropBlock, 0);
  }

  public void Awake()
  {
    this.Init();
    if (!this.negativeScaleFix)
      return;
    this.NegativeScaleFix();
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
