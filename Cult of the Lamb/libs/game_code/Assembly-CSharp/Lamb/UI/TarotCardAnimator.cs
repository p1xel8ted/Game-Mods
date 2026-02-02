// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotCardAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TarotCardAnimator : BaseMonoBehaviour
{
  public const string kStaticAnimation = "menu-static";
  public const string kStaticBackAnimation = "menu-static-back";
  public const string kMenuRevealAnimation = "menu-reveal";
  public string _skinName;
  [SerializeField]
  public SkeletonGraphic _spine;
  [SerializeField]
  public ParticleSystem _particleSystem;
  [Header("Materials")]
  [SerializeField]
  public Material _normalMaterial;
  [SerializeField]
  public Material _rareMaterial;
  [SerializeField]
  public Material _superRareMaterial;
  [SerializeField]
  public Material _rotMaterial;
  [CompilerGenerated]
  public RectTransform \u003CRectTransform\u003Ek__BackingField;

  public SkeletonGraphic Spine => this._spine;

  public RectTransform RectTransform
  {
    set => this.\u003CRectTransform\u003Ek__BackingField = value;
    get => this.\u003CRectTransform\u003Ek__BackingField;
  }

  public void Awake()
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
    this._skinName = TarotCards.Skin(card.CardType);
    this._spine.Skeleton.SetSkin(TarotCards.Skin(card.CardType));
    if (card.UpgradeIndex == 1)
    {
      this.Spine.material = this._rareMaterial;
      if ((Object) this._particleSystem != (Object) null)
        this._particleSystem.Play();
    }
    else if (card.UpgradeIndex == 2)
    {
      this.Spine.material = this._superRareMaterial;
      if ((Object) this._particleSystem != (Object) null)
        this._particleSystem.Play();
    }
    else
      this.Spine.material = this._normalMaterial;
    if (!TarotCards.IsRotCard(card.CardType) || !((Object) this._rotMaterial != (Object) null))
      return;
    this.Spine.material = this._rotMaterial;
  }

  public void Configure(TarotCards.Card card) => this.Configure(new TarotCards.TarotCard(card, 1));

  public void SetStaticBack()
  {
    this._spine.AnimationState.SetAnimation(0, "menu-static-back", false);
  }

  public void SetStaticFront() => this._spine.AnimationState.SetAnimation(0, "menu-static", false);

  public IEnumerator YieldForReveal(float timeScale = 1f)
  {
    this._spine.timeScale = timeScale;
    yield return (object) this._spine.YieldForAnimation("menu-reveal");
  }
}
