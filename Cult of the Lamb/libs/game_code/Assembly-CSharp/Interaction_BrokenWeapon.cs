// Decompiled with JetBrains decompiler
// Type: Interaction_BrokenWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_BrokenWeapon : Interaction
{
  [Header("Broken Weapon Settings")]
  [SerializeField]
  public SpriteRenderer sprite;
  [SerializeField]
  public GameObject outline;
  [SerializeField]
  public EquipmentType weaponType;
  [SerializeField]
  public List<Interaction_BrokenWeapon.WeaponIcons> weaponIcons;
  public PickUp pickUp;

  public void Awake()
  {
    this.pickUp = this.GetComponent<PickUp>();
    if (this.weaponType != EquipmentType.Gauntlet_Legendary || !DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(this.weaponType) && !DataManager.Instance.RepairedLegendaryGauntlet && DataManager.Instance.OnboardedLegendaryWeapons)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = ScriptLocalization.Interactions.PickUp;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine(this.PickUpRoutine());
  }

  public void SetWeapon(EquipmentType weapon) => this.weaponType = weapon;

  public void SetWeaponVisual()
  {
    for (int index = 0; index < this.weaponIcons.Count; ++index)
    {
      if (this.weaponIcons[index].WeaponType == this.weaponType)
      {
        this.sprite.sprite = this.weaponIcons[index].Sprite;
        this.outline.SetActive(false);
        break;
      }
    }
  }

  public IEnumerator PickUpRoutine()
  {
    Interaction_BrokenWeapon interactionBrokenWeapon = this;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionBrokenWeapon.transform.position);
    GameManager.GetInstance().OnConversationNew();
    interactionBrokenWeapon.pickUp.enabled = false;
    interactionBrokenWeapon.transform.localScale = Vector3.zero;
    interactionBrokenWeapon.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(interactionBrokenWeapon.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", interactionBrokenWeapon.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.5f);
    PlayerSimpleInventory component = interactionBrokenWeapon.state.gameObject.GetComponent<PlayerSimpleInventory>();
    Vector3 endValue = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    interactionBrokenWeapon.transform.DOMove(endValue, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(0.2f);
    interactionBrokenWeapon.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionBrokenWeapon.transform.position);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1);
    DataManager.Instance.AddLegendaryWeaponToUnlockQueue(interactionBrokenWeapon.weaponType);
    interactionBrokenWeapon.StartBringWeaponToBlacksmithObjective();
    if (interactionBrokenWeapon.weaponType == EquipmentType.Hammer_Legendary)
      DataManager.Instance.FindBrokenHammerWeapon = false;
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationEnd();
    RoomLockController.RoomCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionBrokenWeapon.gameObject);
  }

  public void StartBringWeaponToBlacksmithObjective()
  {
    if (ObjectiveManager.GetGiveItemObjectives(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT).Count > 0)
      return;
    ObjectiveManager.Add((ObjectivesData) new Objectives_GiveItem("Objectives/GroupTitles/LegendaryWeapons", "NAMES/BlacksmithNPC", 1, InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT), true, true);
  }

  [Serializable]
  public struct WeaponIcons
  {
    public EquipmentType WeaponType;
    public Sprite Sprite;
  }
}
