// Decompiled with JetBrains decompiler
// Type: TrapSpike
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class TrapSpike : BaseMonoBehaviour
{
  [SerializeField]
  private SkeletonAnimation skele;
  [SerializeField]
  private SpriteRenderer crossSprite;
  [SerializeField]
  private SpriteRenderer topSprite;
  [SerializeField]
  private SpriteRenderer bottomSprite;
  [SerializeField]
  private Sprite TopSpriteRed;
  [SerializeField]
  private Sprite TopSpriteWhite;
  [SerializeField]
  private Sprite TopSpriteDark;
  private bool disabled;

  public void AnimateSpike(string animationName, Color color)
  {
    this.skele.AnimationState.SetAnimation(0, animationName, false);
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
