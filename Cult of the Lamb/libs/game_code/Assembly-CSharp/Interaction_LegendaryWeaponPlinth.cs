// Decompiled with JetBrains decompiler
// Type: Interaction_LegendaryWeaponPlinth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Extensions;
using src.UI.Prompts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_LegendaryWeaponPlinth : Interaction
{
  public static List<Interaction_LegendaryWeaponPlinth> WeaponPlinths = new List<Interaction_LegendaryWeaponPlinth>();
  [SerializeField]
  public Interaction_LegendaryWeaponPlinth.WeaponCost cost;
  [SerializeField]
  public SpriteRenderer weaponSprite;
  [SerializeField]
  public GameObject activeEffect;
  [SerializeField]
  public GameObject activeEffect2;
  [SerializeField]
  public EquipmentType legendaryWeapon;
  public EquipmentType[] weaponItems = new EquipmentType[7]
  {
    EquipmentType.Hammer_Legendary,
    EquipmentType.Sword_Legendary,
    EquipmentType.Dagger_Legendary,
    EquipmentType.Gauntlet_Legendary,
    EquipmentType.Blunderbuss_Legendary,
    EquipmentType.Axe_Legendary,
    EquipmentType.Chain_Legendary
  };
  public UIWeaponPickupPromptController _weaponPickupUI;
  public bool isPaying;

  public bool canShowWeapon => DataManager.Instance.WeaponPool.Contains(this.legendaryWeapon);

  public bool canActivate
  {
    get => this.canShowWeapon && DataManager.Instance.ForcedStartingWeapon == EquipmentType.None;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.activeEffect.SetActive(false);
    this.activeEffect2.SetActive(false);
    if (this.canShowWeapon)
    {
      this.weaponSprite.gameObject.SetActive(true);
      if (DataManager.Instance.ForcedStartingWeapon == this.legendaryWeapon)
        this.Activate();
    }
    else
      this.weaponSprite.gameObject.SetActive(false);
    Interaction_LegendaryWeaponPlinth.WeaponPlinths.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_LegendaryWeaponPlinth.WeaponPlinths.Remove(this);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = true;
    if ((DataManager.Instance.LegendaryWeaponsJobBoardCompleted.Contains(this.legendaryWeapon) || this.legendaryWeapon == EquipmentType.Chain_Legendary && DataManager.IsAllJobBoardsComplete && DataManager.Instance.WeaponPool.Contains(EquipmentType.Chain_Legendary)) && DataManager.Instance.ForcedStartingWeapon != this.legendaryWeapon && !this.isPaying)
      this.Label = $"{LocalizationManager.GetTranslation("UI/Settings/Accessibility/GameplayModifiers/ForceWeapon")}: {InventoryItem.CapacityString(this.cost.CostType, this.cost.Cost)}";
    else if (this.canShowWeapon)
    {
      this.Interactable = false;
      this.Label = EquipmentManager.GetWeaponData(this.legendaryWeapon).GetLocalisedTitle();
    }
    else
    {
      this.Interactable = false;
      this.Label = "";
    }
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.IndicateHighlighted(playerFarming);
    if ((UnityEngine.Object) this._weaponPickupUI == (UnityEngine.Object) null)
    {
      this._weaponPickupUI = MonoSingleton<UIManager>.Instance.WeaponPickPromptControllerTemplate.Instantiate<UIWeaponPickupPromptController>();
      this._weaponPickupUI.Init(playerFarming);
      UIWeaponPickupPromptController weaponPickupUi = this._weaponPickupUI;
      weaponPickupUi.OnHidden = weaponPickupUi.OnHidden + (System.Action) (() => this._weaponPickupUI = (UIWeaponPickupPromptController) null);
    }
    EquipmentManager.GetWeaponData(this.legendaryWeapon);
    float averageWeaponDamage = playerFarming.playerWeapon.GetAverageWeaponDamage(this.legendaryWeapon, 1);
    float weaponSpeed = playerFarming.playerWeapon.GetWeaponSpeed(this.legendaryWeapon);
    this._weaponPickupUI.Show(playerFarming, this.legendaryWeapon, averageWeaponDamage, weaponSpeed, 0);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.EndIndicateHighlighted(playerFarming);
    if (!((UnityEngine.Object) this._weaponPickupUI != (UnityEngine.Object) null))
      return;
    this._weaponPickupUI.Hide();
  }

  public override void OnInteract(StateMachine state)
  {
    if (Inventory.GetItemQuantity(this.cost.CostType) < this.cost.Cost)
    {
      this.playerFarming.indicator.PlayShake();
    }
    else
    {
      base.OnInteract(state);
      this.StartCoroutine((IEnumerator) this.Pay());
    }
  }

  public static Interaction_LegendaryWeaponPlinth GetWeaponPlinth(EquipmentType legendaryWeapon)
  {
    foreach (Interaction_LegendaryWeaponPlinth weaponPlinth in Interaction_LegendaryWeaponPlinth.WeaponPlinths)
    {
      if (weaponPlinth.legendaryWeapon == legendaryWeapon)
        return weaponPlinth;
    }
    return (Interaction_LegendaryWeaponPlinth) null;
  }

  public void SetVisual()
  {
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.weaponSprite.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/legendary_weapon_repair/weapon_appear");
    this.weaponSprite.gameObject.SetActive(true);
  }

  public IEnumerator Pay()
  {
    Interaction_LegendaryWeaponPlinth legendaryWeaponPlinth = this;
    legendaryWeaponPlinth.isPaying = true;
    legendaryWeaponPlinth.HasChanged = true;
    AudioManager.Instance.PlayOneShot("event:/shop/buy");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(legendaryWeaponPlinth.gameObject);
    Debug.Log((object) $"LegendaryWeaponPlinth: Paying {{Item:{legendaryWeaponPlinth.cost.CostType},Quantity:{legendaryWeaponPlinth.cost.Cost}}}");
    Inventory.ChangeItemQuantity(legendaryWeaponPlinth.cost.CostType, -legendaryWeaponPlinth.cost.Cost);
    float timeBetweenResources = legendaryWeaponPlinth.cost.Cost == 0 ? 0.0f : 1f / (float) legendaryWeaponPlinth.cost.Cost;
    for (int x = 0; x < legendaryWeaponPlinth.cost.Cost; ++x)
    {
      ResourceCustomTarget.Create(legendaryWeaponPlinth.gameObject, legendaryWeaponPlinth.playerFarming.transform.position, legendaryWeaponPlinth.cost.CostType, (System.Action) null);
      yield return (object) new WaitForSeconds(timeBetweenResources);
    }
    Interaction_LegendaryWeaponPlinth.DeactivateAllPlinthEffects();
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short");
    legendaryWeaponPlinth.Activate();
    DataManager.Instance.ForcedStartingWeapon = legendaryWeaponPlinth.legendaryWeapon;
    legendaryWeaponPlinth.isPaying = false;
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void Activate()
  {
    this.activeEffect.SetActive(true);
    this.activeEffect2.SetActive(true);
  }

  public void Deactivate()
  {
    this.activeEffect.SetActive(false);
    this.activeEffect2.SetActive(false);
  }

  public static void DeactivateAllPlinthEffects()
  {
    foreach (Interaction_LegendaryWeaponPlinth weaponPlinth in Interaction_LegendaryWeaponPlinth.WeaponPlinths)
      weaponPlinth.Deactivate();
  }

  [CompilerGenerated]
  public void \u003CIndicateHighlighted\u003Eb__17_0()
  {
    this._weaponPickupUI = (UIWeaponPickupPromptController) null;
  }

  [Serializable]
  public struct WeaponCost
  {
    public InventoryItem.ITEM_TYPE CostType;
    public int Cost;
  }
}
