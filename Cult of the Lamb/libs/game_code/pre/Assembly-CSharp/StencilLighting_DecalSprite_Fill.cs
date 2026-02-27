// Decompiled with JetBrains decompiler
// Type: StencilLighting_DecalSprite_Fill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class StencilLighting_DecalSprite_Fill : BaseMonoBehaviour
{
  public bool OverrideHeight;
  [Range(1f, 15f)]
  public float BlendHeight = 1f;
  public bool useInfluenceOverride;
  [SerializeField]
  public float overrideInfluence = 1f;
  private float oldInfluence;
  public bool hideChild = true;
  public bool constrainChildTransform = true;
  public bool negativeScaleFix = true;
  private const float defaultHeight = 60f;
  public static readonly int overrideInfluenceID = Shader.PropertyToID("_StencilInfluence");
  private Transform m_targetDecalObj;
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
    this.oldInfluence = this.overrideInfluence;
    this.m_targetDecalObj = this.gameObject.transform.GetChild(0);
    this.m_renderer = (Renderer) this.m_targetDecalObj.GetComponent<MeshRenderer>();
  }

  public void ApplyProperties()
  {
    this.PropBlock.Clear();
    this.PropBlock.SetFloat(StencilLighting_DecalSprite_Fill.overrideInfluenceID, this.overrideInfluence);
    if ((Object) this.m_renderer != (Object) null)
      this.m_renderer.SetPropertyBlock(this.PropBlock, 0);
    this.oldInfluence = this.overrideInfluence;
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

  private void ResetRotation()
  {
    if (!Application.isPlaying)
      return;
    Transform parent = this.m_targetDecalObj.parent;
    this.m_targetDecalObj.parent = (Transform) null;
    this.m_targetDecalObj.localRotation = Quaternion.Euler(new Vector3(-135f, 0.0f, 0.0f));
    this.m_targetDecalObj.parent = parent;
  }

  private void SetScale()
  {
    float num = 60f;
    if (this.OverrideHeight)
      num = this.BlendHeight;
    if (Mathf.Approximately(this.gameObject.transform.localScale.z, num))
      return;
    this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, num);
  }

  private void Awake()
  {
    this.Init();
    this.SetScale();
    if (this.negativeScaleFix)
      this.NegativeScaleFix();
    if (!this.constrainChildTransform)
      return;
    this.ResetRotation();
  }

  private void Start()
  {
    this.Init();
    if (!this.useInfluenceOverride)
      return;
    this.ApplyProperties();
  }

  private void OnDrawGizmos()
  {
    if ((Object) this.m_renderer == (Object) null)
      return;
    Gizmos.color = Color.yellow;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
  }
}
