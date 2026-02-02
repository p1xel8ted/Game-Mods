// Decompiled with JetBrains decompiler
// Type: UITraitManipulatorInProgressMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Spine.Unity;
using src.UINavigator;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UITraitManipulatorInProgressMenuController : UIMenuBase
{
  [SerializeField]
  public TextMeshProUGUI _followerNameText;
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [SerializeField]
  public IndoctrinationTraitItem traitItem;
  [SerializeField]
  public List<GameObject> traitSlots;
  public List<IndoctrinationTraitItem> traitItems = new List<IndoctrinationTraitItem>();
  public FollowerInfo followerInfo;
  public StructureBrain structureBrain;
  [SerializeField]
  public Image ProgressBar;

  public void Show(FollowerInfo config, StructureBrain structure)
  {
    this.Show();
    this.followerInfo = config;
    this.structureBrain = structure;
    if (this.structureBrain is Structures_TraitManipulator structureBrain)
      this.ProgressBar.fillAmount = this.structureBrain.Data.Progress / structureBrain.Duration;
    this._followerNameText.text = config.Name;
    this._followerSpine.ConfigureFollower(config);
    List<FollowerTrait.TraitType> traitTypeList = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) config.Traits);
    if (config.TraitManipulateType == UITraitManipulatorMenuController.Type.Add)
      traitTypeList.Add(config.TargetTraits[0]);
    else if (config.TraitManipulateType == UITraitManipulatorMenuController.Type.Remove)
    {
      traitTypeList.Remove(config.TargetTraits[0]);
      traitTypeList.Add(config.TargetTraits[0]);
    }
    int index1 = 0;
    for (int index2 = 0; index2 < traitTypeList.Count && index1 < this.traitSlots.Count; ++index2)
    {
      IndoctrinationTraitItem indoctrinationTraitItem = Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.traitSlots[index1].transform);
      ((RectTransform) indoctrinationTraitItem.transform).anchoredPosition = new Vector2(40f, 40f);
      indoctrinationTraitItem.transform.localScale = Vector3.one;
      indoctrinationTraitItem.Configure(traitTypeList[index2]);
      indoctrinationTraitItem.Selectable.Interactable = false;
      if (config.TraitManipulateType == UITraitManipulatorMenuController.Type.Shuffle || config.TargetTraits.Contains(traitTypeList[index2]))
      {
        if (FollowerTrait.UniqueTraits.Contains(traitTypeList[index2]))
        {
          indoctrinationTraitItem.SetState(UITraitManipulatorMenuController.Type.None);
          indoctrinationTraitItem.SetDeactivated(true);
        }
        else
          indoctrinationTraitItem.SetState(config.TraitManipulateType);
      }
      this.traitItems.Add(indoctrinationTraitItem);
      ++index1;
    }
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.traitItems[0].Selectable);
  }

  public override void OnCancelButtonInput()
  {
    if (!this.CanvasGroup.interactable)
      return;
    UIManager.PlayAudio("event:/ui/close_menu");
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    Object.Destroy((Object) this.gameObject);
  }

  public void ReleaseFollower()
  {
    UIManager.PlayAudio("event:/dlc/building/exorcismaltar/exorcism_follower_strap_on");
    Follower followerById = FollowerManager.FindFollowerByID(this.followerInfo.ID);
    followerById.Brain.CompleteCurrentTask();
    followerById.Brain._directInfoAccess.TargetTraits.Clear();
    followerById.LockToGround = true;
    this.structureBrain.Data.FollowerID = -1;
    this.structureBrain.Data.Progress = 0.0f;
    DataManager.Instance.Followers_TraitManipulating_IDs.Remove(followerById.Brain.Info.ID);
    foreach (Interaction_TraitManipulator traitManipulator in Interaction_TraitManipulator.TraitManipulators)
    {
      if (traitManipulator.StructureBrain == this.structureBrain)
        traitManipulator.TurnEffectsOff();
    }
    this.Hide();
  }
}
