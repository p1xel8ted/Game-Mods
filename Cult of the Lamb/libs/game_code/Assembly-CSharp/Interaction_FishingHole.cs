// Decompiled with JetBrains decompiler
// Type: Interaction_FishingHole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_FishingHole : Interaction
{
  public override void GetLabel()
  {
    base.GetLabel();
    if (DataManager.Instance.FishCaughtInsideWhaleToday >= 2)
    {
      this.Interactable = false;
      this.Label = ScriptLocalization.Interactions.NoFish;
    }
    else
    {
      this.Interactable = true;
      this.Label = ScriptLocalization.Interactions.Fish;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.playerFarming.GoToAndStop(this.transform.position + Vector3.right, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine(this.FishIE())));
  }

  public IEnumerator FishIE()
  {
    Interaction_FishingHole interactionFishingHole = this;
    if ((double) UnityEngine.Random.value < 0.75)
      ++DataManager.Instance.FishCaughtInsideWhaleToday;
    interactionFishingHole.playerFarming.state.facingAngle = Utils.GetAngle(interactionFishingHole.playerFarming.transform.position, interactionFishingHole.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFishingHole.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/seaice/fish_success", interactionFishingHole.playerFarming.transform.position);
    yield return (object) new WaitForSeconds(1f);
    interactionFishingHole.playerFarming.TimedAction(2f, (System.Action) null, "Fishing/fishing-loop");
    yield return (object) new WaitForSeconds(2f);
    interactionFishingHole.playerFarming.TimedAction(1f, (System.Action) null, "Fishing/fishing-catch");
    if (DataManager.Instance.OnboardedLegendaryWeapons && !DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Blunderbuss_Legendary))
    {
      yield return (object) interactionFishingHole.StartCoroutine(interactionFishingHole.FishOutWeapon());
    }
    else
    {
      for (int index = 0; index < UnityEngine.Random.Range(3, 6); ++index)
      {
        Interaction_Fishing.FishType randomFishType;
        do
        {
          randomFishType = Interaction_Fishing.Instance.GetRandomFishType();
        }
        while (randomFishType.Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION || randomFishType.Type == InventoryItem.ITEM_TYPE.RELIC || randomFishType.Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN);
        InventoryItem.Spawn(randomFishType.Type, 1, interactionFishingHole.playerFarming.transform.position + Vector3.left);
      }
      yield return (object) new WaitForSeconds(1f);
      interactionFishingHole.playerFarming.TimedAction(2.4f, (System.Action) null, "reactions/react-happy");
      yield return (object) new WaitForSeconds(2.4f);
      GameManager.GetInstance().OnConversationEnd();
      interactionFishingHole.HasChanged = true;
    }
  }

  public IEnumerator FishOutWeapon()
  {
    Interaction_FishingHole interactionFishingHole = this;
    GameManager.GetInstance().OnConversationNew();
    PickUp legendaryBlundebuss = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BROKEN_WEAPON_BLUNDERBUSS, 1, interactionFishingHole.transform.position + Vector3.back, 0.0f);
    Interaction_BrokenWeapon legendaryBlunderbussInteraction = legendaryBlundebuss.GetComponent<Interaction_BrokenWeapon>();
    legendaryBlunderbussInteraction.SetWeapon(EquipmentType.Blunderbuss_Legendary);
    legendaryBlundebuss.enabled = false;
    legendaryBlundebuss.transform.localScale = Vector3.zero;
    legendaryBlundebuss.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(legendaryBlundebuss.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", legendaryBlundebuss.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.5f);
    PlayerSimpleInventory component = interactionFishingHole.state.gameObject.GetComponent<PlayerSimpleInventory>();
    legendaryBlundebuss.transform.DOMove(new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(0.25f);
    interactionFishingHole.playerFarming.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionFishingHole.transform.position);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1);
    DataManager.Instance.AddLegendaryWeaponToUnlockQueue(EquipmentType.Blunderbuss_Legendary);
    DataManager.Instance.FoundLegendaryBlunderbuss = true;
    legendaryBlunderbussInteraction.StartBringWeaponToBlacksmithObjective();
    yield return (object) new WaitForSeconds(1.25f);
    UnityEngine.Object.Destroy((UnityEngine.Object) legendaryBlundebuss.gameObject);
    GameManager.GetInstance().OnConversationEnd();
    interactionFishingHole.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__1_0() => this.StartCoroutine(this.FishIE());
}
