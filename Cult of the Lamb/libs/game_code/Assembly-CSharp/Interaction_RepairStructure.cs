// Decompiled with JetBrains decompiler
// Type: Interaction_RepairStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_RepairStructure : Interaction
{
  public new string label;
  public Structure structure;
  public int cost;
  public InventoryItem.ITEM_TYPE costType;
  public UIRebuildBedMinigameOverlayController _uiCookingMinigameOverlayController;

  public override void OnDestroy()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      this.EndIndicateHighlighted(player);
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureModified);
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.OnStructureModified);
    Interaction_DLCFurnace.OnFurnaceLit -= new Interaction_DLCFurnace.FurnaceEvent(this.OnFurnaceLit);
    base.OnDestroy();
  }

  public void Start()
  {
    this.UpdateLocalisation();
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureModified);
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.OnStructureModified);
    Interaction_DLCFurnace.OnFurnaceLit += new Interaction_DLCFurnace.FurnaceEvent(this.OnFurnaceLit);
  }

  public void Configure(Structure structure, InventoryItem.ITEM_TYPE costType, int cost)
  {
    this.structure = structure;
    this.cost = cost;
    this.costType = costType;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.label = ScriptLocalization.Interactions.Repair;
  }

  public override void Update()
  {
    base.Update();
    if (this.structure.Brain == null || !this.structure.Brain.Data.IsSnowedUnder || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      return;
    this.Defrost();
  }

  public override void GetLabel()
  {
    if (this.structure.Brain != null && this.structure.Brain.Data.IsSnowedUnder)
      this.Label = string.Format(LocalizationManager.GetTranslation("Interactions/RepairSnowedUnder"), (object) CostFormatter.FormatCost(this.costType, this.cost));
    else
      this.Label = string.Join(" ", this.label, CostFormatter.FormatCost(this.costType, this.cost));
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(this.costType) >= this.cost)
    {
      this.structure.Brain.ReservedByPlayer = true;
      this.StartCoroutine((IEnumerator) this.InteractRoutine());
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator InteractRoutine()
  {
    Interaction_RepairStructure kitchen = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(kitchen.playerFarming.CameraBone, 6f);
    yield return (object) new WaitForEndOfFrame();
    float i = (float) kitchen.cost;
    while ((double) --i >= 0.0)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", kitchen.gameObject);
      ResourceCustomTarget.Create(kitchen.gameObject, kitchen.playerFarming.CameraBone.transform.position, kitchen.costType, (System.Action) null);
      yield return (object) new WaitForSeconds((float) (0.10000000149011612 - 0.10000000149011612 * ((double) i / (double) kitchen.cost)));
    }
    yield return (object) new WaitForSeconds(0.2f);
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadRebuildBedMinigameAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    Structure component = kitchen.GetComponent<Structure>();
    kitchen._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.RebuildBedMinigameOverlayControllerTemplate.Instantiate<UIRebuildBedMinigameOverlayController>();
    kitchen._uiCookingMinigameOverlayController.Initialise(component.Structure_Info, (Interaction) kitchen);
    kitchen._uiCookingMinigameOverlayController.OnCook += new System.Action(kitchen.OnCook);
    kitchen._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(kitchen.OnUnderCook);
    kitchen._uiCookingMinigameOverlayController.OnBurn += new System.Action(kitchen.OnBurn);
    kitchen.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    kitchen.state.facingAngle = Utils.GetAngle(kitchen.state.transform.position, kitchen.transform.position);
    Inventory.ChangeItemQuantity((int) kitchen.costType, -kitchen.cost);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    AudioManager.Instance.PlayOneShot("event:/material/dirt_dig", kitchen.transform.position);
  }

  public void OnFurnaceLit() => this.OnStructureModified((StructuresData) null);

  public void OnStructureModified(StructuresData s)
  {
    if (this.structure.Brain == null || !this.structure.Brain.Data.IsSnowedUnder)
      return;
    List<Structures_ProximityFurnace> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_ProximityFurnace>();
    if (structuresOfType.Count <= 0 || !structuresOfType[0].NearbyHeatingStructure(this.structure.Brain.Data.Position))
      return;
    this.Defrost();
  }

  public void Defrost()
  {
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureDefrosted", $"<color=#FFD201>{this.structure.Brain.Data.GetLocalizedName()}</color>");
    this.structure.Brain.Defrost();
    UnityEngine.Object.Destroy((UnityEngine.Object) this);
  }

  public void OnCook()
  {
    this.GetComponent<Structure>().Brain.Repaired();
    this.Complete();
    UnityEngine.Object.Destroy((UnityEngine.Object) this);
  }

  public void OnBurn()
  {
    this.HasChanged = true;
    this.Complete();
  }

  public void OnUnderCook()
  {
    this.HasChanged = true;
    this.Complete();
  }

  public void Complete()
  {
    this.structure.Brain.ReservedByPlayer = false;
    AudioManager.Instance.PlayOneShot("event:/building/finished_wood", this.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this._uiCookingMinigameOverlayController.OnCook -= new System.Action(this.OnCook);
    this._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(this.OnUnderCook);
    this._uiCookingMinigameOverlayController.OnBurn -= new System.Action(this.OnBurn);
    this._uiCookingMinigameOverlayController = (UIRebuildBedMinigameOverlayController) null;
    GameManager.GetInstance().WaitForSeconds(0.3f, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
  }
}
