// Decompiled with JetBrains decompiler
// Type: Interaction_HealingBay
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
public class Interaction_HealingBay : Interaction
{
  public static List<Interaction_HealingBay> HealingBays = new List<Interaction_HealingBay>();
  public Structure Structure;
  private Structures_HealingBay _StructureInfo;
  public GameObject FollowerPosition;
  public GameObject EntrancePosition;
  private bool Activated;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_HealingBay structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_HealingBay;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public int PatientID => this.StructureInfo.FollowerID;

  private int Cost => this.structureBrain.Data.Type != StructureBrain.TYPES.HEALING_BAY ? 10 : 15;

  private void Start() => Interaction_HealingBay.HealingBays.Add(this);

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = ScriptLocalization.Interactions_Bed.Assign;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.Activated)
      return;
    this.Activated = true;
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    UIFollowerSelectMenuController followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    List<FollowerInfo> blackList = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (FollowerManager.FollowerLocked(follower.ID) || follower.CursedState != Thought.Ill)
        blackList.Add(follower);
    }
    followerSelectMenu.Show(DataManager.Instance.Followers, blackList, false, UpgradeSystem.Type.Count, true, true, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectMenu;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + new System.Action<FollowerInfo>(this.OnFollowerChosenForConversion);
    UIFollowerSelectMenuController selectMenuController2 = followerSelectMenu;
    selectMenuController2.OnCancel = selectMenuController2.OnCancel + (System.Action) (() =>
    {
      followerSelectMenu = (UIFollowerSelectMenuController) null;
      this.OnHidden();
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectMenu;
    selectMenuController3.OnShow = selectMenuController3.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
        followerInfoBox.ShowCostItem(InventoryItem.ITEM_TYPE.FLOWER_RED, this.Cost, false);
    });
  }

  private void OnFollowerChosenForConversion(FollowerInfo followerInfo)
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FLOWER_RED) >= this.Cost)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
      if ((bool) (UnityEngine.Object) followerById)
        this.StartCoroutine((IEnumerator) this.HealingRoutine(followerById));
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.FLOWER_RED, -this.Cost);
    }
    else
      this.OnHidden();
  }

  private IEnumerator HealingRoutine(Follower follower)
  {
    Time.timeScale = 1f;
    PlayerFarming.Instance.GoToAndStop(this.EntrancePosition.transform.position + Vector3.right * 2f, follower.gameObject);
    FollowerTask_ManualControl task = new FollowerTask_ManualControl();
    follower.Brain.HardSwapToTask((FollowerTask) task);
    follower.transform.position = this.EntrancePosition.transform.position;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    yield return (object) new WaitForSeconds(1f);
    bool waiting = true;
    task.GoToAndStop(follower, this.FollowerPosition.transform.position, (System.Action) (() => waiting = false));
    SimulationManager.Pause();
    while (waiting)
      yield return (object) null;
    follower.SimpleAnimator.Animate("sleep_bedrest_justhead", 1, true, 0.0f);
    float illness = follower.Brain._directInfoAccess.Illness;
    float t = 0.0f;
    float duration = 3f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      follower.Brain.Stats.Illness = Mathf.Lerp(illness, 0.0f, t / duration);
      if (Time.frameCount % 10 == 0 && (double) t > 0.5 && (double) t < (double) duration - 0.5)
        ResourceCustomTarget.Create(follower.gameObject, PlayerFarming.Instance.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 0.5f), InventoryItem.ITEM_TYPE.FLOWER_RED, (System.Action) null);
      yield return (object) null;
    }
    FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
    if (illnessStateChanged != null)
      illnessStateChanged(follower.Brain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
    yield return (object) new WaitForSeconds(0.5f);
    follower.SimpleAnimator.Animate("idle", 1, true, 0.0f);
    double num = (double) follower.SetBodyAnimation("Reactions/react-happy1", false);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.shakeCamera(1.5f, (float) UnityEngine.Random.Range(0, 360));
    BiomeConstants.Instance.EmitHeartPickUpVFX(follower.gameObject.transform.position, 0.0f, "black", "burst_big");
    yield return (object) new WaitForSeconds(1.6f);
    SimulationManager.UnPause();
    follower.Brain.CompleteCurrentTask();
    GameManager.GetInstance().OnConversationEnd();
    ObjectiveManager.CheckObjectives(Objectives.TYPES.SEND_FOLLOWER_BED_REST);
    this.Activated = false;
  }

  private void OnHidden()
  {
    this.Activated = false;
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show();
    GameManager.GetInstance().OnConversationEnd();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_HealingBay.HealingBays.Remove(this);
  }
}
