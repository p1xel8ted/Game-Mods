// Decompiled with JetBrains decompiler
// Type: Interaction_DemonSummoner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_DemonSummoner : Interaction
{
  public static Interaction_DemonSummoner Instance;
  public Canvas UICanvas;
  public Image UIProgress;
  [SerializeField]
  public SkeletonAnimation[] demons;
  [CompilerGenerated]
  public Structure \u003CStructure\u003Ek__BackingField;
  public Structures_Demon_Summoner _StructureInfo;
  public FollowerBrain follower;
  public GameObject OnEffects;
  public static string[] DemonSkins = new string[14]
  {
    "Projectile",
    "Chomp",
    "Arrows",
    "Heart",
    "Explode",
    "Spirit",
    "Baal",
    "Aym",
    "Leshy",
    "Heket",
    "Kallamar",
    "Shamura",
    "ChosenChild",
    "RotDemon"
  };

  public Structure Structure
  {
    get => this.\u003CStructure\u003Ek__BackingField;
    set => this.\u003CStructure\u003Ek__BackingField = value;
  }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Demon_Summoner structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Demon_Summoner;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Awake() => Interaction_DemonSummoner.Instance = this;

  public void Start() => this.UpdateLocalisation();

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!(bool) (UnityEngine.Object) this.Structure)
      return;
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.Structure.OnBrainRemoved -= new System.Action(this.OnBrainRemoved);
  }

  public void OnBrainAssigned()
  {
    foreach (Component demon in this.demons)
      demon.gameObject.SetActive(false);
    for (int index1 = this.StructureInfo.MultipleFollowerIDs.Count - 1; index1 >= 0; --index1)
    {
      if (this.StructureInfo.MultipleFollowerIDs[index1] != -1 && DataManager.Instance.Followers_Demons_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index1]))
      {
        this.OnEffects.SetActive(true);
        this.demons[index1].gameObject.SetActive(true);
        for (int index2 = 0; index2 < DataManager.Instance.Followers_Demons_IDs.Count; ++index2)
        {
          if (DataManager.Instance.Followers_Demons_IDs[index2] == this.StructureInfo.MultipleFollowerIDs[index1])
          {
            FollowerInfo infoById = FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index2]);
            if (infoById != null)
            {
              string skinName = Interaction_DemonSummoner.DemonSkins[DataManager.Instance.Followers_Demons_Types[index2]] + (infoById.GetDemonLevel() <= 1 || DataManager.Instance.Followers_Demons_Types[index2] >= 6 ? "" : "+");
              this.demons[index1].Skeleton.SetSkin(skinName);
              break;
            }
          }
        }
      }
    }
    this.DemonPreserved(false);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (this.StructureInfo == null)
      return;
    for (int index = this.StructureInfo.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Transitioning_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]))
      {
        Follower followerById = FollowerManager.FindFollowerByID(this.StructureInfo.MultipleFollowerIDs[index]);
        int demonType = DemonModel.GetDemonType(followerById.Brain._directInfoAccess);
        this.demons[index].gameObject.SetActive(true);
        string skinName = Interaction_DemonSummoner.DemonSkins[demonType] + (!followerById.Brain.Stats.HasLevelledUp || demonType >= 6 ? "" : "+");
        this.demons[index].Skeleton.SetSkin(skinName);
        this.demons[index].AnimationState.SetAnimation(0, "reveal", false);
        this.demons[index].AnimationState.AddAnimation(0, "idle", true, 0.0f);
        followerById.Brain.CompleteCurrentTask();
        followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_IsDemon());
        followerById.Brain.CurrentTask.Arrive();
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.DemonConverted, followerById.Brain.Info, NotificationFollower.Animation.Normal);
        DataManager.Instance.Followers_Demons_IDs.Add(followerById.Brain.Info.ID);
        DataManager.Instance.Followers_Demons_Types.Add(demonType);
        DataManager.Instance.Followers_Transitioning_IDs.Remove(followerById.Brain.Info.ID);
      }
    }
  }

  public void OnBrainRemoved()
  {
    DataManager.Instance.Followers_Demons_IDs.Clear();
    DataManager.Instance.Followers_Demons_Types.Clear();
    this.DemonPreserved(true);
  }

  public void DemonPreserved(bool structureRemoved)
  {
    for (int index = this.StructureInfo.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (this.StructureInfo.MultipleFollowerIDs[index] != -1 && !DataManager.Instance.Followers_Demons_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]) && !DataManager.Instance.Followers_Transitioning_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]))
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureInfo.MultipleFollowerIDs[index]);
        if (infoById != null)
        {
          FollowerBrain brain = FollowerBrain.GetOrCreateBrain(infoById);
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.SetFollowerPosition(brain));
          NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.DemonPreserved, brain.Info, NotificationFollower.Animation.Normal);
          if (!structureRemoved)
          {
            brain.MakeExhausted();
            brain._directInfoAccess.MissionaryExhaustion += 3f;
            ++brain._directInfoAccess.TimesTurnedIntoADemon;
          }
        }
        this.StructureInfo.MultipleFollowerIDs.RemoveAt(index);
      }
    }
  }

  public IEnumerator SetFollowerPosition(FollowerBrain brain)
  {
    Vector3 pos = this.transform.position;
    while ((double) Time.timeScale == 0.0)
      yield return (object) null;
    brain.HardSwapToTask((FollowerTask) new FollowerTask_Idle());
    brain.CurrentTask.Arrive();
    while ((UnityEngine.Object) FollowerManager.FindFollowerByID(brain.Info.ID) == (UnityEngine.Object) null)
      yield return (object) null;
    Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
    if ((bool) (UnityEngine.Object) followerById)
    {
      brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      followerById.transform.position = pos;
      brain.CompleteCurrentTask();
    }
  }

  public override void OnEnableInteraction()
  {
    this.Structure = this.GetComponentInParent<Structure>();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.Structure.OnBrainRemoved += new System.Action(this.OnBrainRemoved);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    base.OnEnableInteraction();
  }

  public override void GetLabel()
  {
    if (!this.AtDemonLimit())
      this.Label = ScriptLocalization.Interactions.SummonDemon;
    else
      this.Label = "";
  }

  public override void GetSecondaryLabel()
  {
    if (this.StructureInfo.MultipleFollowerIDs.Count > 0)
      this.SecondaryLabel = ScriptLocalization.UI_Settings_Controls.Interact;
    else
      this.SecondaryLabel = "";
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    this.Interactable = !this.AtDemonLimit();
    this.HasSecondaryInteraction = this.StructureInfo.MultipleFollowerIDs.Count > 0;
    base.OnBecomeCurrent(playerFarming);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.AtDemonLimit())
      return;
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    UIDemonSummonMenuController demonSummonMenuInstance = MonoSingleton<UIManager>.Instance.DemonSummonTemplate.Instantiate<UIDemonSummonMenuController>();
    demonSummonMenuInstance.Show(DemonModel.AvailableFollowersForDemonConversion(), false, UpgradeSystem.Type.Count, true, true, true, false, true);
    demonSummonMenuInstance.UpdateDemonCounts(this.StructureInfo.MultipleFollowerIDs);
    UIDemonSummonMenuController summonMenuController1 = demonSummonMenuInstance;
    summonMenuController1.OnFollowerSelected = summonMenuController1.OnFollowerSelected + new System.Action<FollowerInfo>(this.OnFollowerChosenForConversion);
    UIDemonSummonMenuController summonMenuController2 = demonSummonMenuInstance;
    summonMenuController2.OnHidden = summonMenuController2.OnHidden + (System.Action) (() =>
    {
      demonSummonMenuInstance = (UIDemonSummonMenuController) null;
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show();
      GameManager.GetInstance().OnConversationEnd();
    });
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    GameManager.GetInstance().OnConversationNew();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (int multipleFollowerId in this.StructureInfo.MultipleFollowerIDs)
      followerSelectEntries.Add(new FollowerSelectEntry(multipleFollowerId));
    UIDemonMenuController demonMenuInstance = MonoSingleton<UIManager>.Instance.DemonMenuTemplate.Instantiate<UIDemonMenuController>();
    demonMenuInstance.Show(followerSelectEntries);
    UIDemonMenuController demonMenuController = demonMenuInstance;
    demonMenuController.OnHidden = demonMenuController.OnHidden + (System.Action) (() =>
    {
      demonMenuInstance = (UIDemonMenuController) null;
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show();
      GameManager.GetInstance().OnConversationEnd();
    });
  }

  public bool AtDemonLimit()
  {
    return this.StructureInfo.MultipleFollowerIDs.Count >= this.structureBrain.DemonSlots;
  }

  public void OnFollowerChosenForConversion(FollowerInfo followerInfo)
  {
    this.StructureInfo.MultipleFollowerIDs.Add(followerInfo.ID);
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(followerInfo);
    Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
    FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
    brain.HardSwapToTask((FollowerTask) nextTask);
    followerById.transform.position = this.transform.position;
    DataManager.Instance.Followers_Transitioning_IDs.Add(followerInfo.ID);
    this.StartCoroutine((IEnumerator) this.SentFollowerIE(followerById));
  }

  public IEnumerator SentFollowerIE(Follower follower)
  {
    follower.HideAllFollowerIcons();
    int demonType = DemonModel.GetDemonType(follower.Brain._directInfoAccess);
    yield return (object) new WaitForSeconds(0.5f);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.OnEffects.SetActive(true);
    double num = (double) follower.SetBodyAnimation("summon-demon", false);
    AudioManager.Instance.PlayOneShot("event:/followers/follower_to_demon_sequence", follower.transform.position);
    yield return (object) new WaitForSeconds(3f);
    int index1 = 0;
    for (int index2 = 0; index2 < this.StructureInfo.MultipleFollowerIDs.Count; ++index2)
    {
      if (this.StructureInfo.MultipleFollowerIDs[index2] == follower.Brain.Info.ID)
      {
        index1 = index2;
        break;
      }
    }
    this.demons[index1].gameObject.SetActive(true);
    string skinName = Interaction_DemonSummoner.DemonSkins[demonType] + (follower.Brain.Info.XPLevel <= 1 || demonType >= 6 ? "" : "+");
    this.demons[index1].Skeleton.SetSkin(skinName);
    this.demons[index1].AnimationState.SetAnimation(0, "reveal", false);
    this.demons[index1].AnimationState.AddAnimation(0, "idle", true, 0.0f);
    follower.Brain.CompleteCurrentTask();
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_IsDemon());
    follower.Brain.CurrentTask.Arrive();
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.DemonConverted, follower.Brain.Info, NotificationFollower.Animation.Normal);
    DataManager.Instance.Followers_Demons_IDs.Add(follower.Brain.Info.ID);
    DataManager.Instance.Followers_Demons_Types.Add(demonType);
    DataManager.Instance.Followers_Transitioning_IDs.Remove(follower.Brain.Info.ID);
    yield return (object) new WaitForSeconds(0.8f);
  }
}
