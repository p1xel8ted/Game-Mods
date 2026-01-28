// Decompiled with JetBrains decompiler
// Type: UIWeaponCardFeature
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIWeaponCardFeature : BaseMonoBehaviour
{
  public Coroutine cFadeIn;
  public CanvasGroup canvasGroup;
  public RectTransform rectTransform;
  public TextMeshProUGUI Name;
  public TextMeshProUGUI Lore;
  public TextMeshProUGUI Description;
  public SkeletonGraphic Spine;
  [SerializeField]
  public Image BG;
  [SerializeField]
  public CanvasGroup rightCanvasGroup;
  public Vector3 StartingPos;

  public void Selected(Buttons Button, string Skin)
  {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.canvasGroup.DOKill();
    this.rectTransform.DOKill();
    UICardManager.ButtonsUICardManager buttonsUiCardManager = Button as UICardManager.ButtonsUICardManager;
    if (!buttonsUiCardManager.Card.Unlocked)
    {
      if ((double) this.canvasGroup.alpha == 0.0)
        return;
      this.canvasGroup.DOFade(0.0f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
    }
    else
    {
      this.canvasGroup.alpha = 0.0f;
      this.canvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
      this.rectTransform.DOShakePosition(0.5f, new Vector3(10f, 0.0f, 0.0f));
      this.Spine.Skeleton.SetSkin(Skin);
      this.Name.text = TarotCards.LocalisedName(buttonsUiCardManager.Card.Type);
      this.Lore.text = TarotCards.LocalisedLore(buttonsUiCardManager.Card.Type);
      this.Description.text = TarotCards.LocalisedDescription(buttonsUiCardManager.Card.Type, 0, PlayerFarming.Instance);
    }
  }

  public void Reveal() => this.StartCoroutine((IEnumerator) this.RevealRoutine());

  public IEnumerator RevealRoutine()
  {
    UIWeaponCardFeature weaponCardFeature = this;
    weaponCardFeature.Spine.gameObject.transform.DOPunchScale(new Vector3(1f, 1f), 0.5f);
    weaponCardFeature.Spine.AnimationState.SetAnimation(0, "menu-static-back", false);
    weaponCardFeature.StartingPos = weaponCardFeature.Spine.transform.position;
    weaponCardFeature.BG.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    weaponCardFeature.rightCanvasGroup.alpha = 0.0f;
    weaponCardFeature.Spine.transform.DOMove(weaponCardFeature.gameObject.transform.position, 0.0f);
    yield return (object) new WaitForSecondsRealtime(1f);
    weaponCardFeature.Spine.AnimationState.AddAnimation(0, "menu-reveal", false, 0.0f);
    weaponCardFeature.Spine.AnimationState.AddAnimation(0, "menu-static", true, 0.0f);
    yield return (object) new WaitForSecondsRealtime(1f);
    weaponCardFeature.Spine.transform.DOMove(weaponCardFeature.StartingPos, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    DOTweenModuleUI.DOFade(weaponCardFeature.BG, 1f, 0.75f);
    weaponCardFeature.rightCanvasGroup.DOFade(1f, 0.75f);
  }

  public void Selected(UICardManagerCard Card, string Skin)
  {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.canvasGroup.DOKill();
    this.rectTransform.DOKill();
    if (!Card.Card.Unlocked)
    {
      Debug.Log((object) "Card is not unlocked");
      if ((double) this.canvasGroup.alpha == 0.0)
        return;
      this.canvasGroup.DOFade(0.0f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
    }
    else
    {
      this.canvasGroup.alpha = 0.0f;
      this.canvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
      this.rectTransform.DOShakePosition(0.5f, new Vector3(10f, 0.0f, 0.0f));
      this.Spine.Skeleton.SetSkin(Skin);
      this.Name.text = TarotCards.LocalisedName(Card.Card.Type);
      this.Lore.text = TarotCards.LocalisedLore(Card.Card.Type);
      this.Description.text = TarotCards.LocalisedDescription(Card.Card.Type, 0, PlayerFarming.Instance);
    }
  }
}
