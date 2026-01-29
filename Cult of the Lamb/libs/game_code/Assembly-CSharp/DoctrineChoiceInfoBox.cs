// Decompiled with JetBrains decompiler
// Type: DoctrineChoiceInfoBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMTools;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class DoctrineChoiceInfoBox : 
  MonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public RectTransform rectTransform;
  public RectTransform containerRectTransform;
  public Image Icon;
  public TextMeshProUGUI ResponseText;
  public TextMeshProUGUI UnlockName;
  public TextMeshProUGUI UnlockDescription;
  public TextMeshProUGUI UnlockType;
  public TextMeshProUGUI UnlockTypeIcon;
  public TextMeshProUGUI UnlockTypeDescription;
  public GameObject HoldToUnlock;
  public Image RadialProgress;
  public Selectable Selectable;
  public GameObject DoctrineUnlocked;
  public Image SelectedSymbol;
  public Sprite SelectedSymbolSprite;
  public Sprite UnSelectedSymbolSprite;
  public Image UnselectedOverlay;
  public Image RedOutline;
  public Image WhiteFlash;

  public void Init(DoctrineResponse d)
  {
    this.RedOutline.rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    this.ResponseText.text = $"\"{LocalizationManager.Sources[0].GetTranslation($"DoctrineUpgradeSystem/{d.SermonCategory.ToString()}{d.RewardLevel.ToString()}_Response{(d.isFirstChoice ? "A" : "B")}")}\"";
    DoctrineUpgradeSystem.DoctrineType sermonReward = DoctrineUpgradeSystem.GetSermonReward(d.SermonCategory, d.RewardLevel, d.isFirstChoice);
    this.Icon.sprite = DoctrineUpgradeSystem.GetIcon(sermonReward);
    this.UnlockName.text = DoctrineUpgradeSystem.GetLocalizedName(sermonReward);
    this.UnlockDescription.text = DoctrineUpgradeSystem.GetLocalizedDescription(sermonReward);
    this.UnlockType.text = DoctrineUpgradeSystem.GetDoctrineUnlockString(sermonReward);
    this.UnlockTypeIcon.text = DoctrineUpgradeSystem.GetDoctrineUnlockIcon(sermonReward);
    this.Selectable.enabled = true;
  }

  public void OnEnable()
  {
    this.HoldToUnlock.SetActive(false);
    this.RadialProgress.fillAmount = 0.0f;
    this.DoctrineUnlocked.SetActive(false);
  }

  public void OnSelect(BaseEventData eventData)
  {
    Debug.Log((object) ("ON SELECT! " + this.gameObject.name));
    this.rectTransform.DOKill();
    this.rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    this.RedOutline.rectTransform.DOKill();
    this.RedOutline.rectTransform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad);
    this.UnselectedOverlay.DOKill();
    this.UnselectedOverlay.color = (Color) new Vector4(1f, 1f, 1f, 1f);
    DOTweenModuleUI.DOFade(this.UnselectedOverlay, 0.0f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCirc);
    this.SelectedSymbol.sprite = this.SelectedSymbolSprite;
    this.SelectedSymbol.color = StaticColors.RedColor;
  }

  public void OnDeselect(BaseEventData eventData)
  {
    Debug.Log((object) ("ON DESELECT! " + this.gameObject.name));
    this.ResetTweens();
    this.UnselectedOverlay.DOKill();
    this.UnselectedOverlay.color = (Color) new Vector4(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(this.UnselectedOverlay, 1f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCirc);
    this.SelectedSymbol.sprite = this.UnSelectedSymbolSprite;
    this.SelectedSymbol.color = StaticColors.OffWhiteColor;
  }

  public void ResetTweens()
  {
    this.RedOutline.rectTransform.DOKill();
    this.RedOutline.rectTransform.DOScale(0.8f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad);
    this.rectTransform.DOKill();
    this.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
  }
}
