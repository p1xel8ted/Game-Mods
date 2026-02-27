// Decompiled with JetBrains decompiler
// Type: Interaction_PlacementRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_PlacementRegion : Interaction
{
  private string sString;
  private string sTutorial;
  private string sShrineBuilding;
  public PlacementRegion placementRegion;
  public GameObject CameraTarget;
  public GameObject ActiveObject;
  public GameObject InactiveObject;
  public GameObject TutorialObject;
  private List<int> Costs = new List<int>()
  {
    0,
    5,
    10,
    20,
    30,
    50
  };
  public bool UseWhiteList;
  public List<StructureBrain.TYPES> WhiteList;
  public GameObject NewBuildingAvailableObject;
  public Animator newBuildingAnimator;
  public GameObject TechTree;

  public int Cost
  {
    get
    {
      int b = 0;
      foreach (PlacementRegion placementRegion in PlacementRegion.PlacementRegions)
      {
        if (placementRegion.StructureInfo.Purchased)
          ++b;
      }
      return this.Costs[Mathf.Min(this.Costs.Count, b)];
    }
  }

  private void Start()
  {
    this.IgnoreTutorial = true;
    this.HasSecondaryInteraction = true;
    this.UpdateLocalisation();
    this.ActiveObject.SetActive(true);
    this.InactiveObject.SetActive(false);
    this.HasSecondaryInteraction = false;
    this.OnBuildingUnlocked();
  }

  private void OnBuildingUnlocked()
  {
    if (DataManager.Instance.NewBuildings)
    {
      this.NewBuildingAvailableObject.SetActive(true);
    }
    else
    {
      this.newBuildingAnimator.Play("Hide");
      this.StartCoroutine((IEnumerator) this.WaitToTurnOffBuilding());
    }
  }

  private IEnumerator WaitToTurnOffBuilding()
  {
    yield return (object) new WaitForSeconds(1.5f);
    this.NewBuildingAvailableObject.SetActive(false);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    UpgradeSystem.OnBuildingUnlocked += new System.Action(this.OnBuildingUnlocked);
  }

  public override void OnDisableInteraction()
  {
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    UpgradeSystem.OnBuildingUnlocked -= new System.Action(this.OnBuildingUnlocked);
  }

  private void OnStructuresPlaced()
  {
    if (this.Cost != 0 || !((UnityEngine.Object) this.placementRegion != (UnityEngine.Object) null))
      return;
    this.placementRegion.StructureInfo.Purchased = true;
  }

  private void SetGameObjects()
  {
    if (this.placementRegion.StructureInfo.Purchased)
    {
      this.ActiveObject.SetActive(true);
      this.InactiveObject.SetActive(false);
    }
    else
    {
      this.ActiveObject.SetActive(false);
      this.InactiveObject.SetActive(true);
    }
  }

  public override void GetLabel()
  {
    if (!DataManager.Instance.AllowBuilding)
    {
      this.Interactable = false;
      this.Label = this.sTutorial;
    }
    else if (BuildSitePlot.StructureOfTypeUnderConstruction(StructureBrain.TYPES.SHRINE) || BuildSitePlotProject.StructureOfTypeUnderConstruction(StructureBrain.TYPES.SHRINE))
    {
      this.Interactable = false;
      this.Label = this.sShrineBuilding;
    }
    else
    {
      this.Interactable = true;
      this.Label = this.sString;
    }
  }

  public override void GetSecondaryLabel()
  {
    int shrineLevel = DataManager.Instance.ShrineLevel;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Build;
    this.sTutorial = ScriptLocalization.Interactions.IndoctrinateFollowerBeforeBuilding;
    this.sShrineBuilding = ScriptLocalization.Interactions.ShrineUnderConstruction;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    state.CURRENT_STATE = StateMachine.State.InActive;
    DataManager.Instance.NewBuildings = false;
    UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
    buildMenuController.Show();
    buildMenuController.OnBuildingChosen += new System.Action<StructureBrain.TYPES>(this.PlaceBuilding);
    buildMenuController.OnCancel = buildMenuController.OnCancel + (System.Action) (() =>
    {
      HUD_Manager.Instance.Show(0);
      Time.timeScale = 1f;
      this.Cancel();
    });
    HUD_Manager.Instance.Hide(false, 0);
    Time.timeScale = 0.0f;
    if (!((UnityEngine.Object) this.TutorialObject != (UnityEngine.Object) null))
      return;
    this.TutorialObject.SetActive(false);
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
  }

  private IEnumerator PurchaseRoutine()
  {
    Interaction_PlacementRegion interactionPlacementRegion = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPlacementRegion.gameObject, 7f);
    int i = -1;
    while (++i < interactionPlacementRegion.Cost)
    {
      SoulCustomTarget.Create(interactionPlacementRegion.gameObject, interactionPlacementRegion.state.gameObject.transform.position, Color.white, (System.Action) null);
      PlayerFarming.Instance.GetSoul(-1);
      yield return (object) new WaitForSeconds((float) (0.10000000149011612 - 0.10000000149011612 * (double) (i / interactionPlacementRegion.Cost)));
    }
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(interactionPlacementRegion.transform.position, interactionPlacementRegion.state.transform.position));
    interactionPlacementRegion.placementRegion.StructureInfo.Purchased = true;
    interactionPlacementRegion.SetGameObjects();
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
  }

  private void PlaceBuilding(StructureBrain.TYPES structureType)
  {
    NotificationCentreScreen.Instance.Stop();
    this.OnBuildingUnlocked();
    this.placementRegion.PlacementGameObject = TypeAndPlacementObjects.GetByType(structureType).PlacementObject;
    this.placementRegion.StructureType = structureType;
    this.placementRegion.Play();
  }

  private void Cancel()
  {
    this.OnBuildingUnlocked();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }
}
