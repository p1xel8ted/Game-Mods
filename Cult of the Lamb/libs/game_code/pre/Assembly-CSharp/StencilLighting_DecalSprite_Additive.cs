// Decompiled with JetBrains decompiler
// Type: StencilLighting_DecalSprite_Additive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class StencilLighting_DecalSprite_Additive : BaseMonoBehaviour
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
  public static readonly int overrideInfluenceID = Shader.PropertyToID("_StencilInfluence");
  public static readonly int overrideColorID = Shader.PropertyToID("_ColorInstanceOverride");
  public static readonly int overrideProjScaleID = Shader.PropertyToID("_ProjectionScale");
  public static readonly int overrideTextureID = Shader.PropertyToID("_MainTex");
  private Transform m_targetDecalObj;
  private Transform m_targetAddObj;
  private Renderer m_DecalRenderer;
  private Renderer m_AddRenderer;
  private MaterialPropertyBlock m_propBlock;

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
    this.m_targetDecalObj = this.gameObject.transform.GetChild(0);
    this.m_targetAddObj = this.m_targetDecalObj.GetChild(0);
    this.m_DecalRenderer = (Renderer) this.m_targetDecalObj.GetComponent<MeshRenderer>();
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

  private void Awake()
  {
    this.Init();
    if (!this.negativeScaleFix)
      return;
    this.NegativeScaleFix();
  }

  private void NegativeScaleFix()
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

  private void Start()
  {
    this.Init();
    this.ApplyProperties();
  }

  private void OnDrawGizmos()
  {
    if ((Object) this.m_DecalRenderer == (Object) null)
      return;
    Gizmos.color = Color.red;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
  }
}
