// Decompiled with JetBrains decompiler
// Type: StencilLighting_DecalSprite_Effects_A
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class StencilLighting_DecalSprite_Effects_A : BaseMonoBehaviour
{
  [Range(1f, 15f)]
  public float BlendHeight = 1f;
  [Range(1f, 2f)]
  public float projScale = 1.27f;
  [Range(0.0f, 360f)]
  public float maskRotation;
  public Texture MaskShape;
  public bool constrainChildTransform = true;
  public static readonly int scalePosOffsetID = Shader.PropertyToID("_ScalePosOffset");
  public static readonly int maskRotationID = Shader.PropertyToID("_MaskRotation");
  public static readonly int overrideProjScaleID = Shader.PropertyToID("_ProjectionScale");
  public static readonly int overrideMaskID = Shader.PropertyToID("_MaskTex");
  private Transform m_targetDecalObj;
  private Renderer m_DecalRenderer;
  private Vector4 scalePosOffset;
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
    this.m_DecalRenderer = (Renderer) this.m_targetDecalObj.GetComponent<MeshRenderer>();
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

  private void Awake() => this.Init();

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
