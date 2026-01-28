// Decompiled with JetBrains decompiler
// Type: StencilLighting_ExcludeSprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class StencilLighting_ExcludeSprite : BaseMonoBehaviour
{
  [Range(0.0f, 1f)]
  public float ExclusionAmount = 0.7f;
  public GameObject renderObject;
  public SpriteRenderer masterRenderer;
  public SpriteRenderer targetRenderer;
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

  public void Start()
  {
    this.masterRenderer = this.GetComponent<SpriteRenderer>();
    Material material = Resources.Load<Material>("Materials/StencilLighting_ExcludeSprite");
    if ((double) this.masterRenderer.sharedMaterial.GetFloat("_FadeIntoWoods") == 1.0)
      material.EnableKeyword("_FADEINTOWOODS_ON");
    this.renderObject = new GameObject("ExclusionRenderer");
    this.renderObject.layer = LayerMask.NameToLayer("Lighting_NoRender");
    this.renderObject.transform.parent = this.gameObject.transform;
    this.renderObject.transform.localPosition = Vector3.zero;
    this.renderObject.transform.localScale = Vector3.one;
    this.renderObject.transform.localRotation = Quaternion.identity;
    this.targetRenderer = this.renderObject.AddComponent<SpriteRenderer>();
    this.targetRenderer.sprite = this.masterRenderer.sprite;
    this.targetRenderer.flipX = this.masterRenderer.flipX;
    this.targetRenderer.flipY = this.masterRenderer.flipY;
    this.targetRenderer.sharedMaterial = material;
    this.targetRenderer.GetPropertyBlock(this.PropBlock);
    this.PropBlock.SetFloat("_ExcludeAmount", this.ExclusionAmount);
    this.targetRenderer.SetPropertyBlock(this.PropBlock);
    this.PropBlock.Clear();
  }
}
