// Decompiled with JetBrains decompiler
// Type: Interaction_Refinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.RefineryMenu;
using src.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class Interaction_Refinery : Interaction
{
  public float Delay;
  public GameObject radialProgressObj;
  public SpriteRenderer radialProgress;
  public InventoryItemDisplay itemDisplay;
  public TMP_Text UIQuantityText;
  public static List<Interaction_Refinery> Refineries = new List<Interaction_Refinery>();
  public Structure Structure;
  public Structures_Refinery _StructureInfo;
  public GameObject FollowerPosition;
  public bool beingMoved;
  public GameObject OnEffects;
  public string sDeposit;
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

  public void Start()
  {
    this.SetSpriteAtlasArcCenterOffset();
    this.radialProgressObj.SetActive(false);
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

  public void OnBrainAssigned()
  {
    this.StructureBrain.OnCompleteRefining += new System.Action(this.OnCompleteRefining);
    this.CheckPhase();
  }

  public void OnBuildingBeganMoving(int structureID)
  {
    int num = structureID;
    int? id = this.Structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = true;
  }

  public void OnBuildingPlaced(int structureID)
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

  public void CheckPhase()
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
    this.radialProgressObj.SetActive(false);
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
      PlayerFarming.SetStateForAllPlayers();
      foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
        locationFollower.Brain.CheckChangeTask();
      GameManager.GetInstance().OnConversationEnd();
    });
    refineryMenuController.OnItemQueued += (System.Action<InventoryItem.ITEM_TYPE>) (type => this.CurrentState = Interaction_Refinery.State.InProgress);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
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
          this.radialProgressObj.SetActive(false);
          this.UIQuantityText.text = "";
          this.SetProgress(0.0f);
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

  public void DisplayUI()
  {
    if (!this.radialProgressObj.activeSelf)
      this.radialProgressObj.SetActive(true);
    this.OnEffects.SetActive(true);
    if (this.StructureInfo.QueuedResources.Count <= 0)
      return;
    this.itemDisplay.SetImage(this.StructureInfo.QueuedResources[0], false);
    this.UIQuantityText.text = "x" + this.StructureInfo.QueuedResources.Count.ToString();
    this.SetProgress(this.StructureInfo.Progress / ((Structures_Refinery) this.Structure.Brain).RefineryDuration(this.StructureInfo.QueuedResources[0]));
  }

  public void SetProgress(float normalizedProgress)
  {
    this.radialProgress.material.SetFloat("_Arc2", (float) (360.0 * (1.0 - (double) normalizedProgress)));
  }

  public void SetSpriteAtlasArcCenterOffset()
  {
    Vector2 center = this.radialProgress.sprite.textureRect.center;
    this.radialProgress.material.SetVector("_ArcCenterOffset", (Vector4) (new Vector2(center.x / (float) this.radialProgress.sprite.texture.width, center.y / (float) this.radialProgress.sprite.texture.height) - Vector2.one * 0.5f));
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__29_2(InventoryItem.ITEM_TYPE type)
  {
    this.CurrentState = Interaction_Refinery.State.InProgress;
  }

  public enum State
  {
    Available,
    InProgress,
  }
}
