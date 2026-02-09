// Decompiled with JetBrains decompiler
// Type: Interaction_Missionaries
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Lamb.UI.Mission;
using src.Extensions;
using src.UI.Menus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_Missionaries : Interaction
{
  public static Interaction_Missionaries Instance;
  public Canvas UICanvas;
  public Image UIProgress;
  [CompilerGenerated]
  public Structure \u003CStructure\u003Ek__BackingField;
  public Structures_Missionary _StructureInfo;
  public Interaction_Missionaries.MissionarySlot[] MissionarySlots;
  public static List<Interaction_Missionaries> Missionaries = new List<Interaction_Missionaries>();
  public List<int> transitoningIDs = new List<int>();

  public Structure Structure
  {
    get => this.\u003CStructure\u003Ek__BackingField;
    set => this.\u003CStructure\u003Ek__BackingField = value;
  }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Missionary structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Missionary;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Awake() => Interaction_Missionaries.Instance = this;

  public void Start()
  {
    this.UpdateLocalisation();
    Interaction_Missionaries.Missionaries.Add(this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((bool) (UnityEngine.Object) this.Structure)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    for (int index = this.transitoningIDs.Count - 1; index >= 0; --index)
    {
      Follower followerById = FollowerManager.FindFollowerByID(this.transitoningIDs[index]);
      if ((bool) (UnityEngine.Object) followerById)
      {
        followerById.SetOutfit(FollowerOutfitType.Follower, false);
        followerById.Interaction_FollowerInteraction.Interactable = true;
      }
      DataManager.Instance.Followers_Transitioning_IDs.Remove(this.transitoningIDs[index]);
    }
    Interaction_Missionaries.Missionaries.Remove(this);
  }

  public void OnBrainAssigned() => this.UpdateSlots();

  public override void OnEnableInteraction()
  {
    this.Structure = this.GetComponentInParent<Structure>();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    base.OnEnableInteraction();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (this.StructureInfo == null)
      return;
    foreach (int multipleFollowerId in this.StructureInfo.MultipleFollowerIDs)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(multipleFollowerId);
      if (infoById != null)
      {
        FollowerBrain brain = FollowerBrain.GetOrCreateBrain(infoById);
        if (brain != null && brain != null && brain.CurrentTask != null && brain.CurrentTask is FollowerTask_ManualControl)
        {
          brain._directInfoAccess.MissionarySuccessful = MissionaryManager.GetReward((InventoryItem.ITEM_TYPE) brain._directInfoAccess.MissionaryType, brain._directInfoAccess.MissionaryChance, brain.Info.ID).Length != 0;
          brain.HardSwapToTask((FollowerTask) new FollowerTask_OnMissionary());
          DataManager.Instance.Followers_OnMissionary_IDs.Add(brain.Info.ID);
          DataManager.Instance.Followers_Transitioning_IDs.Remove(brain.Info.ID);
        }
      }
    }
  }

  public void UpdateSlots()
  {
    foreach (Interaction_Missionaries.MissionarySlot missionarySlot in this.MissionarySlots)
    {
      missionarySlot.Free.SetActive(true);
      missionarySlot.Occupied.SetActive(false);
    }
    if (this.StructureInfo == null)
      return;
    for (int index = 0; index < this.StructureInfo.MultipleFollowerIDs.Count; ++index)
    {
      this.MissionarySlots[index].Free.SetActive(false);
      this.MissionarySlots[index].Occupied.SetActive(true);
    }
  }

  public override void GetLabel()
  {
    if (!this.AtMissionaryLimit())
      this.Label = ScriptLocalization.Structures.MISSIONARY;
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
    this.Interactable = !this.AtMissionaryLimit();
    this.HasSecondaryInteraction = this.StructureInfo.MultipleFollowerIDs.Count > 0;
    base.OnBecomeCurrent(playerFarming);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.AtMissionaryLimit())
      return;
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    for (int index = this.StructureInfo.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (!DataManager.Instance.Followers_OnMissionary_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]) && !this.transitoningIDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]))
        this.StructureInfo.MultipleFollowerIDs.Remove(this.StructureInfo.MultipleFollowerIDs[index]);
    }
    UIMissionaryMenuController missionaryMenuInstance = MonoSingleton<UIManager>.Instance.MissionaryMenuTemplate.Instantiate<UIMissionaryMenuController>();
    missionaryMenuInstance.Show(MissionaryManager.FollowersAvailableForMission(), followerSelectionType: UpgradeSystem.Type.Building_Missionary);
    missionaryMenuInstance.OnMissionaryChosen += new System.Action<FollowerInfo, InventoryItem.ITEM_TYPE>(this.OnSentFollowerOnMissionary);
    UIMissionaryMenuController missionaryMenuController = missionaryMenuInstance;
    missionaryMenuController.OnHidden = missionaryMenuController.OnHidden + (System.Action) (() =>
    {
      missionaryMenuInstance = (UIMissionaryMenuController) null;
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show();
      GameManager.GetInstance().OnConversationEnd();
    });
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    for (int index = this.StructureInfo.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureInfo.MultipleFollowerIDs[index]);
      if (infoById != null && infoById.IsSnowman)
        followerSelectEntries.Add(new FollowerSelectEntry(infoById, FollowerSelectEntry.Status.Unavailable));
      else if (!DataManager.Instance.Followers_OnMissionary_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]) && !this.transitoningIDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]))
        this.StructureInfo.MultipleFollowerIDs.Remove(this.StructureInfo.MultipleFollowerIDs[index]);
      else
        followerSelectEntries.Add(new FollowerSelectEntry(this.StructureInfo.MultipleFollowerIDs[index]));
    }
    UIMissionMenuController missionMenuInstance = MonoSingleton<UIManager>.Instance.MissionMenuTemplate.Instantiate<UIMissionMenuController>();
    missionMenuInstance.Show(followerSelectEntries);
    UIMissionMenuController missionMenuController = missionMenuInstance;
    missionMenuController.OnHidden = missionMenuController.OnHidden + (System.Action) (() =>
    {
      missionMenuInstance = (UIMissionMenuController) null;
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show();
      GameManager.GetInstance().OnConversationEnd();
    });
  }

  public bool AtMissionaryLimit()
  {
    HashSet<int> intSet = new HashSet<int>((IEnumerable<int>) this.transitoningIDs);
    intSet.UnionWith((IEnumerable<int>) this.StructureInfo.MultipleFollowerIDs);
    int count = intSet.Count;
    return this.StructureInfo.Type == StructureBrain.TYPES.MISSIONARY && count >= 1 || this.StructureInfo.Type == StructureBrain.TYPES.MISSIONARY_II && count >= 2 || this.StructureInfo.Type == StructureBrain.TYPES.MISSIONARY_III && count >= 3;
  }

  public override void Update() => base.Update();

  public void OnSentFollowerOnMissionary(FollowerInfo followerInfo, InventoryItem.ITEM_TYPE type)
  {
    this.transitoningIDs.Add(followerInfo.ID);
    this.StructureInfo.MultipleFollowerIDs.Add(followerInfo.ID);
    Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
    FollowerBrain brain = followerById.Brain;
    float num = 1f;
    if (this.StructureInfo.Type == StructureBrain.TYPES.MISSIONARY_II)
      num = 0.8f;
    else if (this.StructureInfo.Type == StructureBrain.TYPES.MISSIONARY_II)
      num = 0.6f;
    List<Structures_Missionary> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Missionary>(brain.Location);
    brain._directInfoAccess.MissionaryTimestamp = TimeManager.TotalElapsedGameTime;
    brain._directInfoAccess.MissionaryDuration = (float) (MissionaryManager.GetDurationDeterministic(followerInfo, type) * 1200) / num;
    brain._directInfoAccess.MissionaryIndex = structuresOfType.IndexOf(this.structureBrain);
    brain._directInfoAccess.MissionaryType = (int) type;
    brain._directInfoAccess.MissionaryChance = MissionaryManager.GetChance(type, brain._directInfoAccess, this.StructureInfo.Type);
    DataManager.Instance.NextMissionarySuccessful = false;
    DataManager.Instance.Followers_Transitioning_IDs.Add(followerInfo.ID);
    brain.CompleteCurrentTask();
    brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    followerById.OverridingOutfit = true;
    followerById.transform.position = this.MissionarySlots[this.StructureInfo.MultipleFollowerIDs.Count - 1].Free.transform.position;
    followerById.Interaction_FollowerInteraction.Interactable = false;
    this.StartCoroutine((IEnumerator) this.SentFollowerIE(followerById));
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SendFollowerOnMissionary, followerById.Brain.Info.ID);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SendSpouseOnMissionary, followerById.Brain.Info.ID);
  }

  public IEnumerator SentFollowerIE(Follower follower)
  {
    follower.HideAllFollowerIcons();
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/followers/backpack", follower.transform.position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num1 = (double) follower.SetBodyAnimation("missionary-start", false);
    this.UpdateSlots();
    yield return (object) new WaitForSeconds(0.1f);
    follower.Brain.Info.Outfit = FollowerOutfitType.Sherpa;
    follower.SetOutfit(FollowerOutfitType.Sherpa, false);
    yield return (object) new WaitForSeconds(1f);
    double num2 = (double) follower.SetBodyAnimation("wave", false);
    yield return (object) new WaitForSeconds(1.8f);
    follower.AddBodyAnimation("idle", false, 0.0f);
    follower.Brain._directInfoAccess.MissionarySuccessful = MissionaryManager.GetReward((InventoryItem.ITEM_TYPE) follower.Brain._directInfoAccess.MissionaryType, follower.Brain._directInfoAccess.MissionaryChance, follower.Brain.Info.ID).Length != 0;
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_OnMissionary());
    follower.OverridingOutfit = false;
    this.transitoningIDs.Remove(follower.Brain.Info.ID);
    DataManager.Instance.Followers_Transitioning_IDs.Remove(follower.Brain.Info.ID);
    follower.Brain._directInfoAccess.MissionaryExhaustion += 3f;
    DataManager.Instance.Followers_OnMissionary_IDs.Add(follower.Brain.Info.ID);
  }

  [Serializable]
  public struct MissionarySlot
  {
    public GameObject Free;
    public GameObject Occupied;
  }
}
