// Decompiled with JetBrains decompiler
// Type: Interaction_Apothecary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Apothecary : Interaction_Cook
{
  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sCook = "Brew";
    this.sCancel = ScriptLocalization.Interactions.Cancel;
  }

  public List<InventoryItem> item => this.structure.Structure_Info.Inventory;

  public override IEnumerator CookFood()
  {
    Interaction_Apothecary interactionApothecary = this;
    interactionApothecary.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/cooking/cooking_loop", interactionApothecary.gameObject, true);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) new WaitForEndOfFrame();
    interactionApothecary.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionApothecary.state.facingAngle = Utils.GetAngle(interactionApothecary.state.transform.position, interactionApothecary.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    if ((double) interactionApothecary.CookingDuration > 0.5)
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) new WaitForSeconds(interactionApothecary.CookingDuration);
    InventoryItem.ITEM_TYPE MealToCreate = InventoryItem.ITEM_TYPE.BLACK_GOLD;
    if (interactionApothecary.item[0].type == 55 && interactionApothecary.item[1].type == 55 && interactionApothecary.item[2].type == 55)
      MealToCreate = InventoryItem.ITEM_TYPE.Necklace_1;
    if (interactionApothecary.item[0].type == 56 && interactionApothecary.item[1].type == 56 && interactionApothecary.item[2].type == 56)
      MealToCreate = InventoryItem.ITEM_TYPE.Necklace_2;
    if (interactionApothecary.item[0].type == 9 && interactionApothecary.item[1].type == 9 && interactionApothecary.item[2].type == 9)
      MealToCreate = InventoryItem.ITEM_TYPE.Necklace_1;
    ResourceCustomTarget.Create(interactionApothecary.state.gameObject, interactionApothecary.transform.position, MealToCreate, (System.Action) (() => Inventory.AddItem((int) MealToCreate, 1)));
    AudioManager.Instance.PlayOneShot("event:/cooking/meal_cooked", interactionApothecary.transform.position);
    AudioManager.Instance.StopLoop(interactionApothecary.loopingSoundInstance);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    interactionApothecary.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionApothecary.structure.Structure_Info.Inventory.Clear();
    interactionApothecary.cauldron.enabled = true;
    interactionApothecary.enabled = false;
  }
}
