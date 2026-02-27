// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotCardAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TarotCardAnimator : BaseMonoBehaviour
{
  private const string kStaticAnimation = "menu-static";
  private const string kStaticBackAnimation = "menu-static-back";
  private const string kMenuRevealAnimation = "menu-reveal";
  [SerializeField]
  private SkeletonGraphic _spine;
  [SerializeField]
  private ParticleSystem _particleSystem;
  [Header("Materials")]
  [SerializeField]
  private Material _normalMaterial;
  [SerializeField]
  private Material _rareMaterial;
  [SerializeField]
  private Material _superRareMaterial;

  public SkeletonGraphic Spine => this._spine;

  public RectTransform RectTransform { private set; get; }

  private void Awake()
  {
    this.RectTransform = this.GetComponent<RectTransform>();
    if (!((Object) this._particleSystem != (Object) null))
      return;
    this._particleSystem.Stop();
  }

  public void Configure(TarotCards.TarotCard card)
  {
    if ((Object) this._particleSystem != (Object) null)
    {
      this._particleSystem.Stop();
      this._particleSystem.Clear();
    }
    this._spine.Skeleton.SetSkin(TarotCards.Skin(card.CardType));
    if (card.UpgradeIndex == 1)
    {
      this.Spine.material = this._rareMaterial;
      if (!((Object) this._particleSystem != (Object) null))
        return;
      this._particleSystem.Play();
    }
    else if (card.UpgradeIndex == 2)
    {
      this.Spine.material = this._superRareMaterial;
      if (!((Object) this._particleSystem != (Object) null))
        return;
      this._particleSystem.Play();
    }
    else
      this.Spine.material = this._normalMaterial;
  }

  public void Configure(TarotCards.Card card) => this.Configure(new TarotCards.TarotCard(card, 1));

  public void SetStaticBack()
  {
    this._spine.AnimationState.SetAnimation(0, "menu-static-back", false);
  }

  public void SetStaticFront() => this._spine.AnimationState.SetAnimation(0, "menu-static", false);

  public IEnumerator YieldForReveal()
  {
    yield return (object) this._spine.YieldForAnimation("menu-reveal");
  }
}
