// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TierLockIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private const float kLineBuffer = 125f;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [SerializeField]
  private TextMeshProUGUI _tierCountText;
  [SerializeField]
  private UpgradeTreeNode.TreeTier _tier;
  [SerializeField]
  private UpgradeTreeConfiguration _config;
  [SerializeField]
  private RectTransform _lockContainer;
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private MMUILineRenderer _leftLine;
  [SerializeField]
  private MMUILineRenderer _rightLine;
  private UpgradeTreeConfiguration.TreeTierConfig _tierConfig;

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

  private void OnDisable()
  {
    UpgradeSystem.OnAbilityUnlocked -= new Action<UpgradeSystem.Type>(this.OnAbilityUnlocked);
  }

  private void OnAbilityUnlocked(UpgradeSystem.Type type) => this.UpdateText();

  private void UpdateText()
  {
    this._tierCountText.text = $"{this._config.NumUnlockedUpgrades()} / {this._config.NumRequiredNodesForTier(this._tier)}";
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

  private IEnumerator ShrinkLock()
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
