// Decompiled with JetBrains decompiler
// Type: StencilLighting_Volumetric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class StencilLighting_Volumetric : BaseMonoBehaviour
{
  public bool useAdditiveColor;
  [SerializeField]
  [ColorUsage(false, true)]
  public Color AdditiveColor = Color.grey;
  [Range(0.0f, 1f)]
  public float AdditiveScale = 1f;
  public bool hideChild = true;
  public static readonly int overrideColorID = Shader.PropertyToID("_Color");
  private Transform m_targetObj;
  private Renderer m_renderer;
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
    this.m_targetObj = this.gameObject.transform.GetChild(0);
    this.m_targetObj.transform.localScale = new Vector3(this.AdditiveScale, this.AdditiveScale, this.AdditiveScale);
    this.m_renderer = (Renderer) this.m_targetObj.GetComponent<MeshRenderer>();
    this.m_targetObj.gameObject.SetActive(this.useAdditiveColor);
    if (!this.useAdditiveColor)
      return;
    this.ApplyProperties();
  }

  public void ApplyProperties()
  {
    this.PropBlock.Clear();
    this.PropBlock.SetColor(StencilLighting_Volumetric.overrideColorID, this.AdditiveColor);
    if (!((Object) this.m_renderer != (Object) null))
      return;
    this.m_renderer.SetPropertyBlock(this.PropBlock, 0);
  }

  private void Awake()
  {
    this.Init();
    this.ApplyProperties();
    if (this.gameObject.layer == 15 || this.gameObject.layer == 20)
      return;
    this.gameObject.layer = 20;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
  }
}
