// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerSelect.UIFollowerSelectMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.FollowerSelect;

public class UIFollowerSelectMenuController : UIFollowerSelectBase<FollowerInformationBox>
{
  [SerializeField]
  private TMP_Text _header;
  private UpgradeSystem.Type _followerSelectionType;
  private List<ObjectivesData> _cachedObjectives;

  public void Show(
    List<FollowerBrain> followerBrains,
    List<FollowerBrain> blackList = null,
    bool instant = false,
    UpgradeSystem.Type followerSelectionType = UpgradeSystem.Type.Count,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    this._followerSelectionType = followerSelectionType;
    this.Show(followerBrains, blackList, instant, hideOnSelection, cancellable, hasSelection);
  }

  public void Show(
    List<Follower> followers,
    List<Follower> blackList = null,
    bool instant = false,
    UpgradeSystem.Type followerSelectionType = UpgradeSystem.Type.Count,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    this._followerSelectionType = followerSelectionType;
    this.Show(followers, blackList, instant, hideOnSelection, cancellable, hasSelection);
  }

  public void Show(
    List<SimFollower> simFollowers,
    List<SimFollower> blackList = null,
    bool instant = false,
    UpgradeSystem.Type followerSelectionType = UpgradeSystem.Type.Count,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    this._followerSelectionType = followerSelectionType;
    this.Show(simFollowers, blackList, instant, hideOnSelection, cancellable, hasSelection);
  }

  public void Show(
    List<FollowerInfo> followerInfo,
    List<FollowerInfo> blackList = null,
    bool instant = false,
    UpgradeSystem.Type followerSelectionType = UpgradeSystem.Type.Count,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    this._followerSelectionType = followerSelectionType;
    this.Show(followerInfo, blackList, instant, hideOnSelection, cancellable, hasSelection);
  }

  protected override void OnShowStarted()
  {
    if (this._cachedObjectives != null)
      this._cachedObjectives.Clear();
    if (!this._hasSelection)
      this._header.text = ScriptLocalization.Inventory.FOLLOWERS;
    base.OnShowStarted();
  }

  protected override void OnShowCompleted()
  {
    base.OnShowCompleted();
    foreach (FollowerInformationBox followerInfoBox in this._followerInfoBoxes)
    {
      if (this.DoesFollowerHaveObjective(followerInfoBox.FollowerInfo))
        followerInfoBox.ShowObjective();
    }
  }

  private bool DoesFollowerHaveObjective(FollowerInfo followerInfo)
  {
    if (this._cachedObjectives == null)
    {
      this._cachedObjectives = new List<ObjectivesData>();
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective is Objectives_PerformRitual objectivesPerformRitual && objectivesPerformRitual.Ritual == this._followerSelectionType)
          this._cachedObjectives.Add(objective);
        else if (objective is Objectives_Custom objectivesCustom)
        {
          switch (this._followerSelectionType)
          {
            case UpgradeSystem.Type.Building_Prison:
              if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison)
              {
                this._cachedObjectives.Add(objective);
                continue;
              }
              continue;
            case UpgradeSystem.Type.Building_Missionary:
              if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerOnMissionary)
              {
                this._cachedObjectives.Add(objective);
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }
    foreach (ObjectivesData cachedObjective in this._cachedObjectives)
    {
      if (cachedObjective is Objectives_PerformRitual objectivesPerformRitual && (objectivesPerformRitual.TargetFollowerID_1 == followerInfo.ID || objectivesPerformRitual.TargetFollowerID_2 == followerInfo.ID) || cachedObjective is Objectives_Custom objectivesCustom && objectivesCustom.TargetFollowerID == followerInfo.ID)
        return true;
    }
    return false;
  }

  protected override FollowerInformationBox PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.FollowerInformationBoxTemplate;
  }
}
