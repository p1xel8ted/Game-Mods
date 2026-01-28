// Decompiled with JetBrains decompiler
// Type: UIDaycareMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine.Unity;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UIDaycareMenu : UIMenuBase
{
  public Action<FollowerInfo> OnFollowerReleased;
  public System.Action OnNurtured;
  [Header("Prisoner Menu")]
  [SerializeField]
  public TextMeshProUGUI _nameText;
  [SerializeField]
  public SkeletonGraphic[] _followerSkeleton;
  [SerializeField]
  public GameObject _notification;
  [SerializeField]
  public TMP_Text _descriptionText;
  [SerializeField]
  public TMP_Text _occupiedText;
  [Header("Buttons")]
  [SerializeField]
  public MMButton _assignButton;
  [SerializeField]
  public MMButton _releaseButton;
  [SerializeField]
  public MMButton _reeducateButton;
  [SerializeField]
  public TextMeshProUGUI _nurtureButtonText;
  public Structures_Daycare _structuresBrain;
  public StructuresData _structuresData;
  public bool _didCancel;

  public void Show(Structures_Daycare structuresBrain, StructuresData structuresData, bool instant = false)
  {
    this._structuresBrain = structuresBrain;
    this._structuresData = structuresData;
    this.Show(instant);
    if (structuresBrain.Capacity == this._followerSkeleton.Length)
      return;
    Debug.LogError((object) "Invalid capacity to followers' skeltons!");
  }

  public override void OnShowStarted()
  {
    this._releaseButton.onClick.AddListener(new UnityAction(this.OnReleaseClicked));
    this._reeducateButton.onClick.AddListener(new UnityAction(this.OnNurturePressed));
    this._reeducateButton.OnSelected += new System.Action(this.OnNurtureSelected);
    this._reeducateButton.OnDeselected += new System.Action(this.OnNurtureDeselected);
    this._reeducateButton.OnConfirmDenied += new System.Action(this.ShakeNurtured);
    this._assignButton.onClick.AddListener(new UnityAction(this.OnAssignClicked));
    this._reeducateButton.Confirmable = false;
    foreach (int multipleFollowerId in this._structuresData.MultipleFollowerIDs)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(multipleFollowerId);
      if (infoById != null && !infoById.Cuddled)
        this._reeducateButton.Confirmable = true;
    }
    this._nurtureButtonText.gameObject.SetActive(!this._reeducateButton.Confirmable);
    this._notification.gameObject.SetActive(this._reeducateButton.Confirmable);
    this._descriptionText.text = ScriptLocalization.UI_Daycare.Description;
    this._descriptionText.text = string.Format(this._descriptionText.text, (object) this._structuresBrain.Capacity);
    this._occupiedText.text = ScriptLocalization.UI_Daycare.OccupiedSlots;
    this._occupiedText.text = string.Format(this._occupiedText.text, (object) this._structuresData.MultipleFollowerIDs.Count, (object) this._structuresBrain.Capacity);
    this.UpdateStats();
  }

  public override void SetActiveStateForMenu(bool state)
  {
    base.SetActiveStateForMenu(state);
    if (this._structuresBrain.IsFull)
    {
      if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == this._assignButton)
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._reeducateButton);
      this._assignButton.Interactable = false;
    }
    else
      this._assignButton.Interactable = state;
  }

  public void UpdateStats()
  {
    foreach (Component component in this._followerSkeleton)
      component.gameObject.SetActive(false);
    for (int index = 0; index < this._structuresData.MultipleFollowerIDs.Count; ++index)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this._structuresData.MultipleFollowerIDs[index]);
      this._followerSkeleton[index].gameObject.SetActive(true);
      this._followerSkeleton[index].ConfigureFollower(infoById);
      if (infoById.Traits.Contains(FollowerTrait.TraitType.Zombie))
      {
        this._followerSkeleton[index].AnimationState.SetAnimation(0, index >= 1 ? "Baby/Baby-zombie/baby-idle-sit-zombie" : "Baby/Baby-zombie/baby-idle-stand-zombie", true);
      }
      else
      {
        Follower followerById = FollowerManager.FindFollowerByID(infoById.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        {
          if (infoById.Age >= 10)
          {
            if (followerById.IsBabyAngry())
              this._followerSkeleton[index].AnimationState.SetAnimation(0, "Baby/Baby-angry/baby-idle-stand-angry", true);
            else if (followerById.IsBabySad())
              this._followerSkeleton[index].AnimationState.SetAnimation(0, "Baby/Baby-sad/baby-idle-stand-sad", true);
            else
              this._followerSkeleton[index].AnimationState.SetAnimation(0, "Baby/baby-idle-stand", true);
          }
          else if (followerById.IsBabyAngry())
            this._followerSkeleton[index].AnimationState.SetAnimation(0, "Baby/Baby-angry/baby-idle-sit-angry", true);
          else if (followerById.IsBabySad())
            this._followerSkeleton[index].AnimationState.SetAnimation(0, "Baby/Baby-sad/baby-idle-sit-sad", true);
          else
            this._followerSkeleton[index].AnimationState.SetAnimation(0, "Baby/baby-idle-sit", true);
        }
      }
    }
  }

  public void OnAssignClicked()
  {
    List<FollowerSelectEntry> followerSelectEntries = Interaction_Daycare.GetFollowerSelectEntries();
    UIFollowerSelectMenuController menu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    menu.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    menu.SetHeaderText(LocalizationManager.GetTranslation("UI/Generic/SelectChild"));
    UIFollowerSelectMenuController selectMenuController1 = menu;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (info =>
    {
      Follower followerById = FollowerManager.FindFollowerByID(info.ID);
      followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
      {
        if ((UnityEngine.Object) daycare != (UnityEngine.Object) null && daycare.Structure.Brain.Data.ID == this._structuresBrain.Data.ID)
          followerById.transform.position = daycare.MiddlePosition + (Vector3) UnityEngine.Random.insideUnitCircle * ((Structures_Daycare) daycare.Structure.Brain).BoundariesRadius;
      }
      if (followerById.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
        followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChildZombie(StructureBrain.GetOrCreateBrain(this._structuresData)));
      else
        followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Child(StructureBrain.GetOrCreateBrain(this._structuresData)));
      followerById.Interaction_FollowerInteraction.Interactable = false;
      followerById.HideAllFollowerIcons();
      Interaction_Daycare.RemoveFromDaycare(info.ID);
      this._structuresData.MultipleFollowerIDs.Add(info.ID);
      foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
        daycare.itemGauge.SetPosition((float) daycare.Structure.Brain.Data.MultipleFollowerIDs.Count / (float) ((Structures_Daycare) daycare.structureBrain).Capacity);
      this._occupiedText.text = string.Format(ScriptLocalization.UI_Daycare.OccupiedSlots, (object) this._structuresData.MultipleFollowerIDs.Count, (object) this._structuresBrain.Capacity);
      this.UpdateStats();
      foreach (int multipleFollowerId in this._structuresData.MultipleFollowerIDs)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(multipleFollowerId);
        if (infoById != null && !infoById.Cuddled)
          this._reeducateButton.Confirmable = true;
      }
      this._nurtureButtonText.gameObject.SetActive(!this._reeducateButton.Confirmable);
      this._notification.gameObject.SetActive(this._reeducateButton.Confirmable);
      if (this._structuresData.MultipleFollowerIDs.Count > 0)
        return;
      this.Hide();
    });
    UIFollowerSelectMenuController selectMenuController2 = menu;
    selectMenuController2.OnHide = selectMenuController2.OnHide + (System.Action) (() => MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._assignButton));
    this.PushInstance<UIFollowerSelectMenuController>(menu);
  }

  public void OnReleaseClicked()
  {
    if (this._structuresData.MultipleFollowerIDs.Count == 1)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this._structuresData.MultipleFollowerIDs[0]);
      Interaction_Daycare.RemoveFromDaycare(infoById.ID);
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(infoById);
      if (brain.HasTrait(FollowerTrait.TraitType.Zombie))
        brain.HardSwapToTask((FollowerTask) new FollowerTask_ChildZombie(infoById.ID));
      else
        brain.HardSwapToTask((FollowerTask) new FollowerTask_Child(infoById.ID));
      this._occupiedText.text = string.Format(ScriptLocalization.UI_Daycare.OccupiedSlots, (object) this._structuresData.MultipleFollowerIDs.Count, (object) this._structuresBrain.Capacity);
      this.UpdateStats();
      this.Hide();
    }
    else
    {
      List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
      foreach (int multipleFollowerId in this._structuresData.MultipleFollowerIDs)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(multipleFollowerId);
        if (infoById != null)
          followerSelectEntries.Add(new FollowerSelectEntry(infoById));
      }
      UIFollowerSelectMenuController menu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
      menu.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
      menu.SetHeaderText(LocalizationManager.GetTranslation("UI/Generic/SelectChild"));
      UIFollowerSelectMenuController selectMenuController1 = menu;
      selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (info =>
      {
        Interaction_Daycare.RemoveFromDaycare(info.ID);
        FollowerBrain brain = FollowerBrain.GetOrCreateBrain(info);
        if (brain.HasTrait(FollowerTrait.TraitType.Zombie))
          brain.HardSwapToTask((FollowerTask) new FollowerTask_ChildZombie(info.ID));
        else
          brain.HardSwapToTask((FollowerTask) new FollowerTask_Child(info.ID));
        this._occupiedText.text = string.Format(ScriptLocalization.UI_Daycare.OccupiedSlots, (object) this._structuresData.MultipleFollowerIDs.Count, (object) this._structuresBrain.Capacity);
        this.UpdateStats();
        if (this._structuresData.MultipleFollowerIDs.Count > 0)
          return;
        this.Hide();
      });
      UIFollowerSelectMenuController selectMenuController2 = menu;
      selectMenuController2.OnHide = selectMenuController2.OnHide + (System.Action) (() => MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._releaseButton));
      this.PushInstance<UIFollowerSelectMenuController>(menu);
    }
  }

  public void OnNurtureSelected()
  {
  }

  public void OnNurtureDeselected()
  {
  }

  public void OnNurturePressed()
  {
    System.Action onNurtured = this.OnNurtured;
    if (onNurtured != null)
      onNurtured();
    this.UpdateStats();
    this.Hide();
  }

  public void ShakeNurtured()
  {
    this._nurtureButtonText.rectTransform.DOKill(true);
    this._nurtureButtonText.rectTransform.localPosition = (Vector3) Vector2.zero;
    this._nurtureButtonText.rectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public override void OnCancelButtonInput()
  {
    this._didCancel = true;
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003COnAssignClicked\u003Eb__18_0(FollowerInfo info)
  {
    Follower followerById = FollowerManager.FindFollowerByID(info.ID);
    followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
    {
      if ((UnityEngine.Object) daycare != (UnityEngine.Object) null && daycare.Structure.Brain.Data.ID == this._structuresBrain.Data.ID)
        followerById.transform.position = daycare.MiddlePosition + (Vector3) UnityEngine.Random.insideUnitCircle * ((Structures_Daycare) daycare.Structure.Brain).BoundariesRadius;
    }
    if (followerById.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChildZombie(StructureBrain.GetOrCreateBrain(this._structuresData)));
    else
      followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Child(StructureBrain.GetOrCreateBrain(this._structuresData)));
    followerById.Interaction_FollowerInteraction.Interactable = false;
    followerById.HideAllFollowerIcons();
    Interaction_Daycare.RemoveFromDaycare(info.ID);
    this._structuresData.MultipleFollowerIDs.Add(info.ID);
    foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
      daycare.itemGauge.SetPosition((float) daycare.Structure.Brain.Data.MultipleFollowerIDs.Count / (float) ((Structures_Daycare) daycare.structureBrain).Capacity);
    this._occupiedText.text = string.Format(ScriptLocalization.UI_Daycare.OccupiedSlots, (object) this._structuresData.MultipleFollowerIDs.Count, (object) this._structuresBrain.Capacity);
    this.UpdateStats();
    foreach (int multipleFollowerId in this._structuresData.MultipleFollowerIDs)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(multipleFollowerId);
      if (infoById != null && !infoById.Cuddled)
        this._reeducateButton.Confirmable = true;
    }
    this._nurtureButtonText.gameObject.SetActive(!this._reeducateButton.Confirmable);
    this._notification.gameObject.SetActive(this._reeducateButton.Confirmable);
    if (this._structuresData.MultipleFollowerIDs.Count > 0)
      return;
    this.Hide();
  }

  [CompilerGenerated]
  public void \u003COnAssignClicked\u003Eb__18_1()
  {
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._assignButton);
  }

  [CompilerGenerated]
  public void \u003COnReleaseClicked\u003Eb__19_0(FollowerInfo info)
  {
    Interaction_Daycare.RemoveFromDaycare(info.ID);
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(info);
    if (brain.HasTrait(FollowerTrait.TraitType.Zombie))
      brain.HardSwapToTask((FollowerTask) new FollowerTask_ChildZombie(info.ID));
    else
      brain.HardSwapToTask((FollowerTask) new FollowerTask_Child(info.ID));
    this._occupiedText.text = string.Format(ScriptLocalization.UI_Daycare.OccupiedSlots, (object) this._structuresData.MultipleFollowerIDs.Count, (object) this._structuresBrain.Capacity);
    this.UpdateStats();
    if (this._structuresData.MultipleFollowerIDs.Count > 0)
      return;
    this.Hide();
  }

  [CompilerGenerated]
  public void \u003COnReleaseClicked\u003Eb__19_1()
  {
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._releaseButton);
  }
}
