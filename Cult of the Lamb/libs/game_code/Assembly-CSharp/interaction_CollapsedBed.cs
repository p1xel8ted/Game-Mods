// Decompiled with JetBrains decompiler
// Type: interaction_CollapsedBed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class interaction_CollapsedBed : Interaction
{
  [SerializeField]
  public Interaction_Bed bed;
  public float GoldCost = 1f;
  public float WoodCost = 3f;
  public UIRebuildBedMinigameOverlayController _uiCookingMinigameOverlayController;
  public string sRepair;

  public void Start() => this.UpdateLocalisation();

  public string GetAffordColor()
  {
    return (double) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LOG) >= (double) this.WoodCost ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sRepair = ScriptLocalization.Interactions.Repair;
  }

  public override void GetLabel()
  {
    this.Label = string.Join(" ", this.sRepair, CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.LOG, (int) this.WoodCost));
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if ((double) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LOG) >= (double) this.WoodCost)
      this.StartCoroutine((IEnumerator) this.InteractRoutine());
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator InteractRoutine()
  {
    interaction_CollapsedBed kitchen = this;
    Debug.Log((object) "AAAA");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(kitchen.playerFarming.CameraBone, 6f);
    yield return (object) new WaitForEndOfFrame();
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadRebuildBedMinigameAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    kitchen._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.RebuildBedMinigameOverlayControllerTemplate.Instantiate<UIRebuildBedMinigameOverlayController>();
    kitchen._uiCookingMinigameOverlayController.Initialise(kitchen.bed.StructureBrain.Data, (Interaction) kitchen);
    kitchen._uiCookingMinigameOverlayController.OnCook += new System.Action(kitchen.OnCook);
    kitchen._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(kitchen.OnUnderCook);
    kitchen._uiCookingMinigameOverlayController.OnBurn += new System.Action(kitchen.OnBurn);
    kitchen.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    kitchen.state.facingAngle = Utils.GetAngle(kitchen.state.transform.position, kitchen.transform.position);
    float i = kitchen.WoodCost;
    while ((double) --i >= 0.0)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", kitchen.gameObject);
      ResourceCustomTarget.Create(kitchen.gameObject, kitchen.playerFarming.CameraBone.transform.position, InventoryItem.ITEM_TYPE.LOG, (System.Action) null);
      yield return (object) new WaitForSeconds((float) (0.10000000149011612 - 0.10000000149011612 * ((double) i / (double) kitchen.WoodCost)));
    }
    Inventory.ChangeItemQuantity(1, (int) -(double) kitchen.WoodCost);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    AudioManager.Instance.PlayOneShot("event:/material/dirt_dig", kitchen.transform.position);
  }

  public void OnCook()
  {
    this.bed.StructureBrain.Rebuild();
    this.bed.UpdateBed();
    this.Complete();
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
    AudioManager.Instance.PlayOneShot("event:/building/finished_wood", this.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this._uiCookingMinigameOverlayController.OnCook -= new System.Action(this.OnCook);
    this._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(this.OnUnderCook);
    this._uiCookingMinigameOverlayController.OnBurn -= new System.Action(this.OnBurn);
    this._uiCookingMinigameOverlayController = (UIRebuildBedMinigameOverlayController) null;
    Sequence s = DOTween.Sequence();
    s.AppendInterval(0.3f);
    s.AppendCallback((TweenCallback) (() => GameManager.GetInstance().OnConversationEnd()));
  }
}
