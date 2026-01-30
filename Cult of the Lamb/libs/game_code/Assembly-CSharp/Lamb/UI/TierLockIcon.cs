// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TierLockIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TierLockIcon : MonoBehaviour
{
  public const float kLineBuffer = 125f;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public TextMeshProUGUI _tierCountText;
  [SerializeField]
  public UpgradeTreeNode.TreeTier _tier;
  [SerializeField]
  public UpgradeTreeConfiguration _config;
  [SerializeField]
  public RectTransform _lockContainer;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public MMUILineRenderer _leftLine;
  [SerializeField]
  public MMUILineRenderer _rightLine;
  public UpgradeTreeConfiguration.TreeTierConfig _tierConfig;

  public UpgradeTreeNode.TreeTier Tier => this._tier;

  public RectTransform RectTransform => this._rectTransform;

  public void Configure(UpgradeTreeNode.TreeTier tier)
  {
    if (this._tierConfig == null)
      this._tierConfig = this._config.GetConfigForTier(this._tier);
    if (tier >= this._tier)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      int num = this._config.NumRequiredNodesForTier(this._tier);
      if (this._config.NumUnlockedUpgrades() >= num)
      {
        this._lockContainer.gameObject.SetActive(false);
        this._leftLine.Fill = (float) (1.0 - 125.0 / (double) this._leftLine.Root.TotalLength / 2.0);
        this._rightLine.Fill = this._leftLine.Fill;
      }
      else
      {
        this.UpdateText();
        UpgradeSystem.OnAbilityUnlocked += new Action<UpgradeSystem.Type>(this.OnAbilityUnlocked);
      }
    }
  }

  public void OnDisable()
  {
    UpgradeSystem.OnAbilityUnlocked -= new Action<UpgradeSystem.Type>(this.OnAbilityUnlocked);
  }

  public void OnAbilityUnlocked(UpgradeSystem.Type type) => this.UpdateText();

  public void UpdateText()
  {
    this._tierCountText.text = $"{this._config.NumUnlockedUpgrades()} / {this._config.NumRequiredNodesForTier(this._tier)}";
    this._tierCountText.isRightToLeftText = false;
  }

  public IEnumerator DestroyTierLock()
  {
    yield return (object) this.ShrinkLock();
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_unlock");
    float size = (float) (125.0 / (double) this._leftLine.Root.TotalLength / 2.0);
    float t = 0.0f;
    float tt = 0.5f;
    while ((double) t < (double) tt)
    {
      t += Time.unscaledDeltaTime;
      this._leftLine.Fill = (float) (1.0 - (double) size * ((double) t / (double) tt));
      this._rightLine.Fill = this._leftLine.Fill;
      yield return (object) null;
    }
  }

  public IEnumerator ShrinkLock()
  {
    UIManager.PlayAudio("event:/door/door_unlock");
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    this._lockContainer.DOScale(Vector3.one * 1.25f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    this._lockContainer.DOScale(Vector3.zero, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    this._lockContainer.gameObject.SetActive(false);
  }

  public IEnumerator RevealTier()
  {
    if (this._lockContainer.gameObject.activeSelf)
      yield return (object) this.ShrinkLock();
    while ((double) this._leftLine.Fill > 0.0)
    {
      this._leftLine.Fill -= Time.unscaledDeltaTime * 1.5f;
      this._rightLine.Fill = this._leftLine.Fill;
      yield return (object) null;
    }
  }
}
