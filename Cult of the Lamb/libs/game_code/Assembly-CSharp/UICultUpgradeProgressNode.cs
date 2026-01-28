// Decompiled with JetBrains decompiler
// Type: UICultUpgradeProgressNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Rituals;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UICultUpgradeProgressNode : MonoBehaviour
{
  public bool isIcon;
  [SerializeField]
  public Image connectionBar;
  [SerializeField]
  public GameObject unlockedObject;
  [SerializeField]
  public CanvasGroup lockedObject;
  [SerializeField]
  public CanvasGroup unlockIconBar;
  [SerializeField]
  public CultUpgradeItem associatedAestheticButton;
  [SerializeField]
  public TextMeshProUGUI levelText;
  public bool locked = true;

  public void SetUnlocked()
  {
    this.locked = false;
    if (this.isIcon)
    {
      this.lockedObject.gameObject.SetActive(false);
      this.unlockIconBar.gameObject.SetActive(true);
      this.unlockIconBar.transform.localScale = Vector3.zero;
      this.unlockIconBar.transform.DOKill();
      this.unlockIconBar.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      DOTweenModuleUI.DOColor(this.connectionBar, Color.white, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.connectionBar.transform.DOPunchPosition(new Vector3(0.0f, -2f, 0.0f), 0.5f).SetUpdate<Tweener>(true);
      AudioManager.Instance.PlayOneShot("event:/ui/objective_group_complete", this.gameObject);
      if ((bool) (Object) this.associatedAestheticButton)
      {
        this.associatedAestheticButton.transform.localScale = Vector3.zero;
        this.associatedAestheticButton.transform.DOKill();
        this.associatedAestheticButton.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.25f).OnStart<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.associatedAestheticButton.FadeIconImages(1f)));
      }
    }
    else
    {
      this.lockedObject.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      this.unlockIconBar.transform.DOKill();
      this.unlockIconBar.DOFade(1f, 0.0f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      this.unlockIconBar.transform.localScale = Vector3.zero;
      this.unlockIconBar.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      DOTweenModuleUI.DOColor(this.connectionBar, Color.white, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.connectionBar.transform.DOPunchPosition(new Vector3(0.0f, -2f, 0.0f), 0.5f).SetUpdate<Tweener>(true);
      AudioManager.Instance.PlayOneShot("event:/ui/objective_group_complete", this.gameObject);
    }
    this.levelText.color = Color.white;
    this.levelText.alpha = 1f;
  }

  public void Config(int upgradeLevel)
  {
    Debug.Log((object) $"Node Upgrade: {upgradeLevel.ToString()} is unlocked: {(upgradeLevel >= DataManager.Instance.TempleLevel).ToString()} Is Icon: {this.isIcon.ToString()}");
    this.levelText.text = (upgradeLevel + 1).ToNumeral();
    int num = 0;
    if (UICultUpgradeProgress.showCultUpgradeProgressSequence)
      num = 1;
    if (this.isIcon)
    {
      if (upgradeLevel < DataManager.Instance.TempleLevel - num)
      {
        this.lockedObject.gameObject.SetActive(false);
        this.unlockIconBar.gameObject.SetActive(true);
        this.locked = false;
        if ((Object) this.associatedAestheticButton != (Object) null)
          this.associatedAestheticButton.gameObject.SetActive(true);
        this.levelText.color = Color.white;
        this.levelText.alpha = 1f;
      }
      else
      {
        this.lockedObject.gameObject.SetActive(true);
        this.unlockIconBar.gameObject.SetActive(false);
        this.locked = true;
        this.levelText.color = Color.grey;
        this.levelText.alpha = 0.2f;
        if (!(bool) (Object) this.associatedAestheticButton)
          return;
        this.associatedAestheticButton.FadeIconImages();
      }
    }
    else
    {
      this.connectionBar.gameObject.SetActive(true);
      if (upgradeLevel < DataManager.Instance.TempleLevel - num)
      {
        this.connectionBar.color = Color.white;
        this.levelText.color = Color.white;
        this.levelText.alpha = 1f;
      }
      else
      {
        this.connectionBar.color = Color.grey;
        this.levelText.color = Color.grey;
        this.levelText.alpha = 0.2f;
      }
    }
  }

  public void AnimateUnlock()
  {
  }

  [CompilerGenerated]
  public void \u003CSetUnlocked\u003Eb__8_0() => this.associatedAestheticButton.FadeIconImages(1f);
}
