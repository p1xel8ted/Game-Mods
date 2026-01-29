// Decompiled with JetBrains decompiler
// Type: Interaction_Daycare
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Daycare : Interaction
{
  public static List<Interaction_Daycare> Daycares = new List<Interaction_Daycare>();
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject center;
  public ItemGauge itemGauge;
  [SerializeField]
  public GameObject[] poopStates;
  public StructureBrain structureBrain;
  public float m_ChildCheckDuration = 3f;
  public float m_ChildCheckTimer;

  public Structure Structure => this.structure;

  public Vector3 MiddlePosition => this.center.transform.position;

  public void Start()
  {
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null)
      this.OnBrainAssigned();
    Interaction_Daycare.Daycares.Add(this);
  }

  public void OnBrainAssigned()
  {
    this.structureBrain = this.structure.Brain;
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structure.Brain.OnItemDeposited += new System.Action(this.UpdatePoopStates);
    this.UpdatePoopStates();
    this.itemGauge.SetPosition((float) this.structure.Brain.Data.MultipleFollowerIDs.Count / (float) ((Structures_Daycare) this.structureBrain).Capacity);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null)
      this.structure.Brain.OnItemDeposited -= new System.Action(this.UpdatePoopStates);
    Interaction_Daycare.Daycares.Remove(this);
  }

  public override void Update()
  {
    base.Update();
    this.m_ChildCheckTimer += Time.deltaTime;
    if ((double) this.m_ChildCheckTimer < (double) this.m_ChildCheckDuration)
      return;
    this.m_ChildCheckTimer = 0.0f;
    for (int index = 0; index < this.Structure.Brain.Data.MultipleFollowerIDs.Count; ++index)
    {
      Follower followerById = FollowerManager.FindFollowerByID(this.Structure.Brain.Data.MultipleFollowerIDs[index]);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null && followerById.Brain.Info.Age < 14)
      {
        if (followerById.Brain.CurrentTask is FollowerTask_Idle)
        {
          Debug.Log((object) $"Nursery: Detected truant child, bringing them back! {followerById.Brain.Info.ID}, Name: {followerById.Brain.Info.Name}");
          followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Child(this.Structure.Brain));
        }
        else if ((double) Vector3.Distance(followerById.Brain.LastPosition, this.transform.position) > 3.0)
          followerById.transform.position = this.center.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle;
      }
      else
      {
        this.Structure.Brain.Data.MultipleFollowerIDs.Remove(this.Structure.Brain.Data.MultipleFollowerIDs[index]);
        break;
      }
    }
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.label = ScriptLocalization.FollowerInteractions.MakeDemand;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    if (this.structureBrain.Data.MultipleFollowerIDs.Count <= 0)
    {
      UIFollowerSelectMenuController selectMenuController = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
      selectMenuController.VotingType = TwitchVoting.VotingType.DAYCARE;
      selectMenuController.Show(Interaction_Daycare.GetFollowerSelectEntries(), false, UpgradeSystem.Type.Count, true, true, true, false, true);
      selectMenuController.SetHeaderText(LocalizationManager.GetTranslation("UI/Generic/SelectChild"));
      selectMenuController.OnFollowerSelected = selectMenuController.OnFollowerSelected + (System.Action<FollowerInfo>) (info => this.OnFollowerSelected(info));
      selectMenuController.OnHidden = selectMenuController.OnHidden + (System.Action) (() =>
      {
        Time.timeScale = 1f;
        GameManager.GetInstance().OnConversationEnd();
        this.HasChanged = true;
      });
    }
    else
    {
      bool nurturing = false;
      UIDaycareMenu uiDaycareMenu = MonoSingleton<UIManager>.Instance.DaycareMenuTemplate.Instantiate<UIDaycareMenu>();
      uiDaycareMenu.Show(this.structure.Brain as Structures_Daycare, this.structure.Brain.Data);
      uiDaycareMenu.OnNurtured += (System.Action) (() =>
      {
        nurturing = true;
        Time.timeScale = 1f;
        this.StartCoroutine((IEnumerator) this.NurtureIE());
      });
      uiDaycareMenu.OnHidden = uiDaycareMenu.OnHidden + (System.Action) (() =>
      {
        if (nurturing)
          return;
        Time.timeScale = 1f;
        GameManager.GetInstance().OnConversationEnd();
        this.HasChanged = true;
      });
    }
  }

  public IEnumerator NurtureIE()
  {
    Interaction_Daycare interactionDaycare1 = this;
    GameManager.GetInstance().OnConversationNext(interactionDaycare1.playerFarming.gameObject, 4f);
    interactionDaycare1.playerFarming.GoToAndStop(interactionDaycare1.MiddlePosition, interactionDaycare1.gameObject, GoToCallback: new System.Action(interactionDaycare1.\u003CNurtureIE\u003Eb__18_0));
    List<Follower> targetFollowers = new List<Follower>();
    for (int index = 0; index < interactionDaycare1.structureBrain.Data.MultipleFollowerIDs.Count; ++index)
    {
      Follower followerById = FollowerManager.FindFollowerByID(interactionDaycare1.structureBrain.Data.MultipleFollowerIDs[index]);
      if (!followerById.Brain._directInfoAccess.Cuddled && followerById.Brain.Info.Age < 18)
        targetFollowers.Add(followerById);
    }
    for (int index = 0; index < targetFollowers.Count; ++index)
    {
      Interaction_Daycare interactionDaycare = interactionDaycare1;
      Follower f = targetFollowers[index];
      FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
      f.Brain.HardSwapToTask((FollowerTask) nextTask);
      nextTask.GoToAndStop(f, interactionDaycare1.GetCirclePosition(targetFollowers.Count, index, 1f), (System.Action) (() => f.FacePosition(interactionDaycare.playerFarming.transform.position)));
      f.HideAllFollowerIcons();
    }
    yield return (object) new WaitForSeconds(2f);
    interactionDaycare1.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (interactionDaycare1.playerFarming.isLamb && !interactionDaycare1.playerFarming.IsGoat)
    {
      interactionDaycare1.playerFarming.Spine.AnimationState.SetAnimation(0, "bleat", false);
      interactionDaycare1.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      if (interactionDaycare1._playerFarming.isLamb)
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
      else
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
      yield return (object) new WaitForSeconds(0.4f);
    }
    else
    {
      interactionDaycare1.playerFarming.Spine.AnimationState.SetAnimation(0, "bleat-goat3", false);
      interactionDaycare1.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_bleat", interactionDaycare1.gameObject);
      CameraManager.shakeCamera(4f);
      yield return (object) new WaitForSeconds(1.5f);
    }
    foreach (Follower follower in targetFollowers)
    {
      if (!follower.Brain.Stats.Cuddled)
      {
        CultFaithManager.AddThought(Thought.ChildCuddle_0, follower.Brain.Info.ID);
        follower.Brain.AddThought((Thought) UnityEngine.Random.Range(393, 397));
        AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", follower.gameObject.transform.position);
        BiomeConstants.Instance.EmitHeartPickUpVFX(follower.gameObject.transform.position, 0.0f, "red", "burst_big", false);
        follower.Brain.Stats.Cuddled = true;
        ++follower.Brain._directInfoAccess.CuddledAmount;
        follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.CuddleBaby);
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.CuddleBaby, (System.Action) (() => { }));
      }
    }
    yield return (object) new WaitForSeconds(2.5f);
    foreach (Follower follower in targetFollowers)
    {
      if (follower.Brain.CanLevelUp())
      {
        follower.FacePosition(interactionDaycare1.playerFarming.transform.position);
        interactionDaycare1.StartCoroutine((IEnumerator) follower.Interaction_FollowerInteraction.LevelUpRoutineTemple(interactionDaycare1._playerFarming));
      }
    }
    yield return (object) new WaitForSeconds(3f);
    foreach (Follower follower in targetFollowers)
    {
      if (follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      {
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChildZombie(interactionDaycare1.structureBrain));
        follower.Brain.CurrentState = (FollowerState) new FollowerState_ChildZombie();
      }
      else
      {
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Child(interactionDaycare1.structureBrain));
        follower.Brain.CurrentState = (FollowerState) new FollowerState_Child();
      }
      follower.ResetStateAnimations();
      follower.Brain.CurrentState.SetStateAnimations(follower);
      if (!interactionDaycare1.structureBrain.Data.MultipleFollowerIDs.Contains(follower.Brain.Info.ID))
        interactionDaycare1.structureBrain.Data.MultipleFollowerIDs.Add(follower.Brain.Info.ID);
    }
    GameManager.GetInstance().OnConversationEnd();
  }

  public void OnFollowerSelected(FollowerInfo info)
  {
    Follower followerById = FollowerManager.FindFollowerByID(info.ID);
    followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    followerById.transform.position = this.center.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle;
    if (followerById.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChildZombie(this.structureBrain));
    else
      followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Child(this.structureBrain));
    followerById.Interaction_FollowerInteraction.Interactable = false;
    followerById.HideAllFollowerIcons();
    Interaction_Daycare.RemoveFromDaycare(info.ID);
    this.structureBrain.Data.MultipleFollowerIDs.Add(info.ID);
    this.itemGauge.SetPosition((float) this.structure.Brain.Data.MultipleFollowerIDs.Count / (float) ((Structures_Daycare) this.structureBrain).Capacity);
  }

  public static List<FollowerSelectEntry> GetFollowerSelectEntries()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.Info.CursedState == Thought.Child)
      {
        bool flag = false;
        foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Daycare>())
        {
          if (structureBrain.Data.MultipleFollowerIDs.Contains(follower.Brain.Info.ID))
            flag = true;
        }
        if (flag)
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableInNursery));
        else if (follower.Brain.Info.ID == 100000)
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.Unavailable));
        else if (follower.Brain.Info.Age >= 14)
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableTooOldForDaycare));
        else if (follower.Brain.Info.ID == 100000)
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.Unavailable));
        else
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess));
      }
    }
    return followerSelectEntries;
  }

  public void UpdatePoopStates()
  {
    int num = Mathf.Clamp(Mathf.FloorToInt((float) ((this.structure.Inventory.Count > 0 ? (double) this.structure.Inventory[0].quantity : 0.0) / 20.0 * 5.0)), 0, this.poopStates.Length);
    for (int index = 0; index < this.poopStates.Length; ++index)
      this.poopStates[index].gameObject.SetActive(index == num - 1);
  }

  public void MaxOutPoop()
  {
    this.structureBrain.DepositItem(InventoryItem.ITEM_TYPE.POOP, 20 - (this.structure.Inventory.Count > 0 ? this.structure.Inventory[0].quantity : 0));
  }

  public Vector3 GetCirclePosition(int followersCount, int index, float targetDistance)
  {
    float f = (float) ((double) index * (360.0 / (double) followersCount) * (Math.PI / 180.0));
    return this.MiddlePosition + new Vector3(targetDistance * Mathf.Cos(f), targetDistance * Mathf.Sin(f)) * ((Structures_Daycare) this.structureBrain).BoundariesRadius;
  }

  public static bool HasAvailableDaycare()
  {
    foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
    {
      if (daycare.structureBrain != null && !daycare.structureBrain.IsFull)
        return true;
    }
    return false;
  }

  public static bool IsInDaycare(int followerID)
  {
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.DAYCARE))
    {
      if (structureBrain.Data.MultipleFollowerIDs.Contains(followerID))
        return true;
    }
    return false;
  }

  public static void RemoveFromDaycare(int followerID)
  {
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.DAYCARE))
    {
      if (structureBrain.Data.MultipleFollowerIDs.Contains(followerID))
      {
        structureBrain.Data.MultipleFollowerIDs.Remove(followerID);
        break;
      }
    }
    foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
      daycare.itemGauge.SetPosition((float) daycare.structure.Brain.Data.MultipleFollowerIDs.Count / (float) ((Structures_Daycare) daycare.structureBrain).Capacity);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__17_0(FollowerInfo info) => this.OnFollowerSelected(info);

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__17_1()
  {
    Time.timeScale = 1f;
    GameManager.GetInstance().OnConversationEnd();
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003CNurtureIE\u003Eb__18_0()
  {
    this.playerFarming.transform.DOMove(this.MiddlePosition, 0.25f);
  }
}
