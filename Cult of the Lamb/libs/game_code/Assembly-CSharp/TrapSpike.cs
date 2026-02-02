// Decompiled with JetBrains decompiler
// Type: TrapSpike
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class TrapSpike : BaseMonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation skele;
  [SerializeField]
  public SpriteRenderer crossSprite;
  [SerializeField]
  public SpriteRenderer topSprite;
  [SerializeField]
  public SpriteRenderer bottomSprite;
  [SerializeField]
  public Sprite TopSpriteRed;
  [SerializeField]
  public Sprite TopSpriteWhite;
  [SerializeField]
  public Sprite TopSpriteDark;
  public bool disabled;

  public void AnimateSpike(string animationName, Color color, bool loop = false)
  {
    if ((Object) this.skele == (Object) null)
      this.skele = this.GetComponent<SkeletonAnimation>();
    this.skele.AnimationState.SetAnimation(0, animationName, loop);
    this.skele.skeleton.SetColor(color);
  }

  public void SetRedSprite()
  {
    if (this.disabled)
      this.DisableSpike();
    this.topSprite.sprite = this.TopSpriteRed;
  }

  public void SetWarningSprite()
  {
    if (this.disabled)
      this.DisableSpike();
    this.topSprite.sprite = this.TopSpriteWhite;
  }

  public void DisableSpike()
  {
    this.disabled = true;
    this.crossSprite.color = new Color(0.33f, 0.33f, 0.33f, 0.51f);
    this.topSprite.sprite = this.TopSpriteDark;
  }
}
