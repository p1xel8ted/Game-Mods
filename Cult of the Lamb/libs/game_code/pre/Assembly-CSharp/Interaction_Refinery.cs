// Decompiled with JetBrains decompiler
// Type: Interaction_Refinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.RefineryMenu;
using src.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_Refinery : Interaction
{
  private float Delay;
  public Canvas UICanvas;
  public Image UIProgress;
  public TextMeshProUGUI UIText;
  public TextMeshProUGUI UIQuantityText;
  public static List<Interaction_Refinery> Refineries = new List<Interaction_Refinery>();
  public Structure Structure;
  private Structures_Refinery _StructureInfo;
  public GameObject FollowerPosition;
  private bool beingMoved;
  public GameObject OnEffects;
  private string sDeposit;
  public Interaction_Refinery.State CurrentState;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Refinery StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Refinery;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  private void Start()
  {
    this.UICanvas.gameObject.SetActive(false);
    this.UpdateLocalisation();
  }

  public override void OnEnableInteraction()
  {
    this.ActivateDistance = 2.5f;
    base.OnEnableInteraction();
    Interaction_Refinery.Refineries.Add(this);
    PlacementRegion.OnBuildingBeganMoving += new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced += new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_Refinery.Refineries.Remove(this);
    PlacementRegion.OnBuildingBeganMoving -= new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced -= new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
    if ((bool) (UnityEngine.Object) this.Structure)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnCompleteRefining -= new System.Action(this.OnCompleteRefining);
  }

  private void OnBrainAssigned()
  {
    this.StructureBrain.OnCompleteRefining += new System.Action(this.OnCompleteRefining);
    this.CheckPhase();
  }

  private void OnBuildingBeganMoving(int structureID)
  {
    int num = structureID;
    int? id = this.Structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = true;
  }

  private void OnBuildingPlaced(int structureID)
  {
    int num = structureID;
    int? id = this.Structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sDeposit = ScriptLocalization.Interactions.SanctifyResources;
  }

  private void CheckPhase()
  {
    if (this.StructureInfo.QueuedResources.Count > 0)
      this.CurrentState = Interaction_Refinery.State.InProgress;
    else
      this.CurrentState = Interaction_Refinery.State.Available;
  }

  public void OnCompleteRefining()
  {
    if (this.StructureInfo.QueuedResources.Count > 0)
      return;
    this.UICanvas.gameObject.SetActive(false);
    this.OnEffects.SetActive(false);
    this.CurrentState = Interaction_Refinery.State.Available;
  }

  public override void GetLabel() => this.Label = this.sDeposit;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    HUD_Manager.Instance.Hide(false, 0);
    Time.timeScale = 0.0f;
    UIRefineryMenuController refineryMenuController = MonoSingleton<UIManager>.Instance.RefineryMenuTemplate.Instantiate<UIRefineryMenuController>();
    refineryMenuController.Show(this.StructureInfo, this);
    refineryMenuController.OnHide = refineryMenuController.OnHide + (System.Action) (() => HUD_Manager.Instance.Show(0));
    refineryMenuController.OnHidden = refineryMenuController.OnHidden + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
      foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
        locationFollower.Brain.CheckChangeTask();
      GameManager.GetInstance().OnConversationEnd();
    });
    refineryMenuController.OnItemQueued += (System.Action<InventoryItem.ITEM_TYPE>) (type => this.CurrentState = Interaction_Refinery.State.InProgress);
  }

  protected override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    if (this.StructureBrain == null)
    {
      Debug.Log((object) "Structure Brain Null!");
    }
    else
    {
      switch (this.CurrentState)
      {
        case Interaction_Refinery.State.Available:
          this.UICanvas.gameObject.SetActive(false);
          this.UIText.text = "";
          this.UIQuantityText.text = "";
          this.UIProgress.fillAmount = 0.0f;
          this.OnEffects.SetActive(false);
          break;
        case Interaction_Refinery.State.InProgress:
          this.DisplayUI();
          break;
      }
      if ((double) this.Delay <= 0.0)
        return;
      this.Delay = Mathf.Clamp(this.Delay - Time.deltaTime, 0.0f, float.MaxValue);
    }
  }

  private void DisplayUI()
  {
    if (!this.UICanvas.gameObject.activeSelf)
      this.UICanvas.gameObject.SetActive(true);
    this.OnEffects.SetActive(true);
    if (this.StructureInfo.QueuedResources.Count <= 0)
      return;
    this.UIText.text = FontImageNames.GetIconByType(this.StructureInfo.QueuedResources[0]);
    this.UIQuantityText.text = "x" + (object) this.StructureInfo.QueuedResources.Count;
    this.UIProgress.fillAmount = this.StructureInfo.Progress / ((Structures_Refinery) this.Structure.Brain).RefineryDuration(this.StructureInfo.QueuedResources[0]);
  }

  public enum State
  {
    Available,
    InProgress,
  }
}
