// Decompiled with JetBrains decompiler
// Type: Interaction_Bed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Bed : Interaction
{
  public Structure Structure;
  [SerializeField]
  private interaction_CollapsedBed collapsedBed;
  [SerializeField]
  private GameObject uncollapsedBed;
  [SerializeField]
  private Dwelling dwelling;
  private Structures_Bed _StructureInfo;
  public SpriteXPBar XPBar;
  public bool cacheCollapse;
  private bool Activated;
  private FollowerInfo OldFollower;
  private Follower follower;
  private bool Activating;

  public virtual Structures_Bed StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Bed;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Collapse()
  {
    this.StructureBrain.Collapse();
    this.UpdateBed();
  }

  private void Start() => this.dwelling = this.GetComponent<Dwelling>();

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    FollowerBrain.OnDwellingAssigned += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingAssignedAwaitClaim += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingCleared += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    else
      this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  private void OnBrainAssigned()
  {
    this.OldFollower = FollowerInfo.GetInfoByID(this.StructureBrain.Data.FollowerID);
    if (this.StructureBrain.Data.Type == global::StructureBrain.TYPES.BED_3)
    {
      this.HasSecondaryInteraction = true;
      this.UpdateBar();
    }
    this.StructureBrain.OnBedCollapsed += new Structures_Bed.BedEvent(this.UpdateBed);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnSoulsGained += new System.Action<int>(this.OnSoulsGained);
    this.UpdateBed();
  }

  private void OnSoulsGained(int count) => this.UpdateBar();

  private void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    this.XPBar.gameObject.SetActive(true);
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f));
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain != null)
      this.StructureBrain.OnBedCollapsed -= new Structures_Bed.BedEvent(this.UpdateBed);
    if (this.StructureBrain != null)
      this.StructureBrain.OnSoulsGained -= new System.Action<int>(this.OnSoulsGained);
    FollowerBrain.OnDwellingAssigned -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingAssignedAwaitClaim -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingCleared -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
  }

  public void UpdateBed()
  {
    if (this.StructureBrain.Data.Type == global::StructureBrain.TYPES.BED_3)
      return;
    if (this.cacheCollapse != this.StructureBrain.IsCollapsed)
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
      AudioManager.Instance.PlayOneShot("event:/building/finished_fabric", this.transform.position);
      this.cacheCollapse = this.StructureBrain.IsCollapsed;
    }
    this.collapsedBed.gameObject.SetActive(this.StructureBrain.IsCollapsed);
    this.uncollapsedBed.gameObject.SetActive(!this.StructureBrain.IsCollapsed);
    this.enabled = !this.StructureBrain.IsCollapsed;
  }

  private void OnDwellingAssignmentChanged(int followerID, Dwelling.DwellingAndSlot d)
  {
    if (!((UnityEngine.Object) this.Structure != (UnityEngine.Object) null) || this.Structure.Structure_Info == null || d.ID != this.Structure.Structure_Info.ID)
      return;
    this.OldFollower = FollowerInfo.GetInfoByID(this.StructureBrain.Data.FollowerID);
  }

  public override void GetLabel()
  {
    if (this.StructureBrain == null)
      return;
    if (this.OldFollower != null && this.StructureBrain.Data.FollowerID != -1)
    {
      this.Label = ScriptLocalization.Interactions_Bed.Re_Assign + " | ";
      this.SecondaryLabel = " " + ScriptLocalization.Interactions_Bed.LivesHere.Replace("{0}", this.OldFollower.Name);
    }
    else
    {
      this.Label = ScriptLocalization.Interactions_Bed.Assign + " | ";
      this.SecondaryLabel = " " + ScriptLocalization.Interactions_Bed.Unoccupied;
    }
    if (this.StructureBrain.Data.Type != global::StructureBrain.TYPES.BED_3)
      return;
    this.XPBar.gameObject.SetActive(true);
    if (this.StructureBrain.SoulCount <= 0)
      return;
    this.SecondaryLabel = $"{ScriptLocalization.Interactions.ReceiveDevotion} {(GameManager.HasUnlockAvailable() ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">")} {(object) this._StructureInfo.SoulCount}{StaticColors.GreyColorHex} / {(object) this.StructureBrain.SoulMax}";
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    this.Activated = true;
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    UIFollowerSelectMenuController followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    List<FollowerInfo> blackList = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (FollowerManager.FollowerLocked(follower.ID))
        blackList.Add(follower);
    }
    if (this.OldFollower != null)
    {
      blackList.Add(this.OldFollower);
      Debug.Log((object) " FOLLOWER FOR BLACK LIST: ! ".Colour(Color.red));
    }
    else
      Debug.Log((object) "NULL FOLLOWER! ".Colour(Color.red));
    followerSelectMenu.Show(DataManager.Instance.Followers, blackList, false, UpgradeSystem.Type.Count, true, true, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectMenu;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + new System.Action<FollowerInfo>(this.OnFollowerChosenForConversion);
    UIFollowerSelectMenuController selectMenuController2 = followerSelectMenu;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() =>
    {
      followerSelectMenu = (UIFollowerSelectMenuController) null;
      this.OnHidden();
      this.HasChanged = true;
    });
  }

  private void WakeUpFollower()
  {
    this.OldFollower = FollowerInfo.GetInfoByID(this.StructureBrain.Data.FollowerID);
    this.follower = FollowerManager.FindFollowerByID(this.OldFollower.ID);
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null)
      return;
    this.follower.Brain.ClearPersonalOverrideTaskProvider();
    this.follower.Brain.AddThought(Thought.SleepInterrupted);
    this.follower.Brain.CompleteCurrentTask();
    this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    this.follower.Brain._directInfoAccess.WakeUpDay = TimeManager.CurrentDay;
    CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, this.follower.Brain.Info.ID);
    this.follower.TimedAnimation("tantrum", 3.16666675f, (System.Action) (() => this.follower.Brain.CompleteCurrentTask()));
  }

  private void OnFollowerChosenForConversion(FollowerInfo followerInfo)
  {
    int followerId = this.StructureBrain.Data.FollowerID;
    if (FollowerInfo.GetInfoByID(followerId) != null)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerId);
      if ((bool) (UnityEngine.Object) followerById)
      {
        if (followerById.Brain.CurrentTaskType == FollowerTaskType.ClaimDwelling)
          followerById.Brain.CurrentTask.Abort();
        followerById.Brain.ClearDwelling();
      }
    }
    int ID = Dwelling.NO_HOME;
    if (followerInfo != null)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
      if (followerById.Brain.CurrentTaskType == FollowerTaskType.ClaimDwelling)
        followerById.Brain.CurrentTask.Abort();
      ID = followerById.Brain.GetDwellingAndSlot() != null ? followerById.Brain.GetDwellingAndSlot().ID : Dwelling.NO_HOME;
      followerById.Brain.ClearDwelling();
      followerById.Brain.AssignDwelling(new Dwelling.DwellingAndSlot(this.dwelling.StructureInfo.ID, 0, 0), followerInfo.ID, false);
      followerById.Brain._directInfoAccess.PreviousDwellingID = Dwelling.NO_HOME;
      followerById.Brain._directInfoAccess.WakeUpDay = -1;
      followerById.Brain.CheckChangeTask();
    }
    if (FollowerInfo.GetInfoByID(followerId) != null)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerId);
      if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
      {
        this.HasChanged = true;
        return;
      }
      if (followerById.Brain.CurrentTaskType == FollowerTaskType.ClaimDwelling)
        followerById.Brain.CurrentTask.Abort();
      if (ID != Dwelling.NO_HOME)
      {
        followerById.Brain.AssignDwelling(new Dwelling.DwellingAndSlot(ID, 0, 0), followerById.Brain.Info.ID, false);
        StructureManager.GetStructureByID<Structures_Bed>(ID).ReservedForTask = true;
      }
      else
      {
        Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(FollowerLocation.Base, this.OldFollower);
        if (freeDwellingAndSlot != null)
        {
          followerById.Brain.AssignDwelling(new Dwelling.DwellingAndSlot(freeDwellingAndSlot.ID, 0, 0), followerById.Brain.Info.ID, false);
          StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID);
        }
      }
      followerById.Brain._directInfoAccess.PreviousDwellingID = Dwelling.NO_HOME;
      followerById.Brain._directInfoAccess.WakeUpDay = -1;
      followerById.Brain.CheckChangeTask();
    }
    this.HasChanged = true;
  }

  private void OnHidden()
  {
    this.Activated = false;
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show();
    GameManager.GetInstance().OnConversationEnd();
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (this.StructureBrain.SoulCount > 0)
    {
      if (this.Activating)
        return;
      this.Activating = true;
      this.StartCoroutine((IEnumerator) this.GiveReward());
    }
    else
      MonoSingleton<Indicator>.Instance.PlayShake();
  }

  private IEnumerator GiveReward()
  {
    Interaction_Bed interactionBed = this;
    int Souls = interactionBed.StructureBrain.SoulCount;
    for (int i = 0; i < Souls; ++i)
    {
      if (GameManager.HasUnlockAvailable())
      {
        // ISSUE: reference to a compiler-generated method
        SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, interactionBed.transform.position, Color.white, new System.Action(interactionBed.\u003CGiveReward\u003Eb__29_0));
      }
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionBed.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      float num = Mathf.Clamp((float) (Souls - i) / (float) interactionBed.StructureBrain.SoulMax, 0.0f, 1f);
      interactionBed.XPBar.UpdateBar(num);
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionBed.XPBar.UpdateBar(0.0f);
    interactionBed.StructureBrain.SoulCount = 0;
    interactionBed.GetSecondaryLabel();
    interactionBed.HasChanged = true;
    interactionBed.Activating = false;
  }
}
