// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradePlayerTreeMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIUpgradePlayerTreeMenuController : 
  UIUpgradeTreeMenuBase<UIPlayerUpgradeUnlockOverlayController>,
  ITreeMenuController
{
  public float cachedVoBusVolume;

  public override void OnShowCompleted()
  {
    this.ActivateNavigation();
    this.cachedVoBusVolume = AudioManager.Instance.GetVOBusVolume();
    DOVirtual.Float(this.cachedVoBusVolume, 0.0f, 0.3f, (TweenCallback<float>) (value => AudioManager.Instance.SetVOBusVolume(value))).SetUpdate<Tweener>(true);
    if (this.revealType == UpgradeSystem.Type.Count)
      return;
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
    {
      if (!treeNode.IsAvailable() && treeNode.RequiresUpgrade == this.revealType)
      {
        UpgradeSystem.UnlockAbility(this.revealType);
        this.StartCoroutine((IEnumerator) this.DoRevealAnimation(this.revealType, 1f, false));
        break;
      }
    }
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    AudioManager.Instance.SetVOBusVolume(this.cachedVoBusVolume);
  }

  public override Selectable GetDefaultSelectable()
  {
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
    {
      if (treeNode.Upgrade == DataManager.Instance.MostRecentPlayerTreeUpgrade)
        return (Selectable) treeNode.Button;
    }
    return (Selectable) null;
  }

  public override int UpgradePoints() => UpgradeSystem.DisciplePoints;

  public override string GetPointsText() => "";

  public override void DoUnlock(UpgradeSystem.Type upgrade)
  {
    UpgradeSystem.UnlockAbility(upgrade);
    DataManager.Instance.MostRecentPlayerTreeUpgrade = upgrade;
    DataManager.Instance.LastDaySincePlayerUpgrade = TimeManager.CurrentDay;
    this._didUpgraded = true;
  }

  public override void DoRelease()
  {
  }

  public override UpgradeTreeNode.TreeTier TreeTier()
  {
    return DataManager.Instance.CurrentPlayerUpgradeTreeTier;
  }

  public override void UpdateTier(UpgradeTreeNode.TreeTier tier)
  {
    DataManager.Instance.CurrentPlayerUpgradeTreeTier = tier;
  }

  public override void OnCancelButtonInput()
  {
  }

  public override void Awake()
  {
    this.disableBackPrompt = this.gameObject.transform.Find("UpgradeTreeMenuContainer/Control Prompts/Back Prompt").gameObject;
    this.disableBackPrompt.SetActive(false);
    base.Awake();
  }

  public List<NodeConnectionLine> GetConnectionLines() => this.NodeConnections;
}
