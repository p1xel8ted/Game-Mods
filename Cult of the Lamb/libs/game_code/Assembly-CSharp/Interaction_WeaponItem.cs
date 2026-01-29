// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using src.UI.Prompts;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_WeaponItem : Interaction
{
  public EquipmentType TypeOfWeapon;
  public WeaponShopKeeperManager ShopKeeper;
  public int WeaponLevel;
  public int Cost;
  public Canvas canvas;
  [SerializeField]
  public SpriteRenderer IconSpriteRenderer;
  public static System.Action<bool> OnHighlightWeapon;
  public static System.Action<bool> OnHighlightCurse;
  public bool Activated;
  public Interaction_WeaponItem.Types Type;
  public float WobbleTimer;
  public UIWeaponPickupPromptController _weaponPickupUI;
  public string BuyString;

  public void Init(
    EquipmentType _TypeOfWeapon,
    int _WeaponLevel,
    int _Cost,
    Interaction_WeaponItem.Types _type)
  {
    this.TypeOfWeapon = _TypeOfWeapon;
    this.WeaponLevel = _WeaponLevel;
    this.Cost = _Cost;
    this.Type = _type;
    this.IconSpriteRenderer.sprite = this.GetIcon();
  }

  public void Start()
  {
    this.WobbleTimer = (float) UnityEngine.Random.Range(0, 360);
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
  }

  public Sprite GetIcon() => EquipmentManager.GetEquipmentData(this.TypeOfWeapon).WorldSprite;

  public override void Update()
  {
    base.Update();
    this.IconSpriteRenderer.transform.localPosition = new Vector3(0.0f, 0.0f, (float) ((double) Mathf.Sin(this.WobbleTimer += Time.deltaTime * 2f) * 0.10000000149011612 - 0.75));
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
    float damage = 0.0f;
    float speed = 0.0f;
    switch (this.Type)
    {
      case Interaction_WeaponItem.Types.Weapon:
        System.Action<bool> onHighlightWeapon = Interaction_WeaponItem.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(true);
          break;
        }
        break;
      case Interaction_WeaponItem.Types.Curse:
        System.Action<bool> onHighlightCurse = Interaction_WeaponItem.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(true);
          break;
        }
        break;
    }
    if (this.Type == Interaction_WeaponItem.Types.Weapon)
    {
      damage = playerFarming.playerWeapon.GetAverageWeaponDamage(this.TypeOfWeapon, this.WeaponLevel);
      speed = playerFarming.playerWeapon.GetWeaponSpeed(this.TypeOfWeapon);
    }
    if ((UnityEngine.Object) this._weaponPickupUI == (UnityEngine.Object) null)
    {
      this._weaponPickupUI = MonoSingleton<UIManager>.Instance.WeaponPickPromptControllerTemplate.Instantiate<UIWeaponPickupPromptController>();
      this._weaponPickupUI.Init(playerFarming);
      UIWeaponPickupPromptController weaponPickupUi = this._weaponPickupUI;
      weaponPickupUi.OnHidden = weaponPickupUi.OnHidden + (System.Action) (() => this._weaponPickupUI = (UIWeaponPickupPromptController) null);
    }
    this._weaponPickupUI.Show(playerFarming, this.TypeOfWeapon, damage, speed, this.WeaponLevel);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
    switch (this.Type)
    {
      case Interaction_WeaponItem.Types.Weapon:
        System.Action<bool> onHighlightWeapon = Interaction_WeaponItem.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(false);
          break;
        }
        break;
      case Interaction_WeaponItem.Types.Curse:
        System.Action<bool> onHighlightCurse = Interaction_WeaponItem.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(false);
          break;
        }
        break;
    }
    if (!((UnityEngine.Object) this._weaponPickupUI != (UnityEngine.Object) null))
      return;
    this._weaponPickupUI.Hide();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.BuyString = ScriptLocalization.Interactions.Buy;
  }

  public string GetAffordColor()
  {
    return Inventory.GetItemQuantity(20) >= this.Cost - 1 ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void GetLabel()
  {
    this.Label = $"{string.Format(this.BuyString, (object) $"{EquipmentManager.GetEquipmentData(this.TypeOfWeapon).GetLocalisedTitle()} {this.WeaponLevel.ToNumeral()}")} | {this.GetAffordColor()}{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.BLACK_GOLD)} {LocalizeIntegration.FormatCurrentMax(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD).ToString(), this.Cost.ToString())}</color>";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    PlayerFarming component = state.GetComponent<PlayerFarming>();
    if (Inventory.GetItemQuantity(20) >= this.Cost - 1 && !this.Activated)
    {
      this.Activated = true;
      for (int index = 0; index < this.Cost; ++index)
      {
        if (index < 10)
        {
          Inventory.GetItemByType(20);
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", component.transform.position);
          ResourceCustomTarget.Create(this.gameObject, component.gameObject.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
        }
        Inventory.ChangeItemQuantity(20, -1);
      }
      CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360));
      switch (this.Type)
      {
        case Interaction_WeaponItem.Types.Weapon:
          this.StartCoroutine((IEnumerator) this.PlayerShowWeaponRoutine(component));
          break;
        case Interaction_WeaponItem.Types.Curse:
          this.StartCoroutine((IEnumerator) this.PlayerShowCurseRoutine(component));
          break;
      }
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
      component.indicator.PlayShake();
      if (!((UnityEngine.Object) this.ShopKeeper.CantAffordBark != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.ShopKeeper.NormalBark != (UnityEngine.Object) null)
        this.ShopKeeper.NormalBark.SetActive(false);
      this.ShopKeeper.CantAffordBark.SetActive(true);
    }
  }

  public IEnumerator PlayerShowWeaponRoutine(PlayerFarming playerFarming)
  {
    Interaction_WeaponItem interactionWeaponItem = this;
    playerFarming.GoToAndStop(interactionWeaponItem.transform.position + Vector3.down * 0.5f, interactionWeaponItem.gameObject);
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    if (playerFarming.currentWeapon != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), interactionWeaponItem.state.transform.position, Quaternion.identity, interactionWeaponItem.transform.parent) as GameObject;
      gameObject.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), 270f);
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(playerFarming.currentWeapon, playerFarming.playerWeapon.CurrentWeaponLevel, Interaction_WeaponPickUp.Types.Weapon);
    }
    playerFarming.playerWeapon.SetWeapon(interactionWeaponItem.TypeOfWeapon, interactionWeaponItem.WeaponLevel);
    interactionWeaponItem.IconSpriteRenderer.enabled = false;
    if (!DataManager.Instance.WeaponPool.Contains(interactionWeaponItem.TypeOfWeapon))
      DataManager.Instance.WeaponPool.Add(interactionWeaponItem.TypeOfWeapon);
    interactionWeaponItem.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", interactionWeaponItem.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", interactionWeaponItem.transform.position);
    yield return (object) new WaitForSeconds(playerFarming.Spine.AnimationState.SetAnimation(0, playerFarming.CurrentWeaponInfo.WeaponData.PickupAnimationKey, false).Animation.Duration);
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionWeaponItem.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWeaponItem.gameObject);
  }

  public IEnumerator PlayerShowCurseRoutine(PlayerFarming playerFarming)
  {
    Interaction_WeaponItem interactionWeaponItem = this;
    if ((double) interactionWeaponItem.transform.position.x < (double) interactionWeaponItem.state.transform.position.x)
      playerFarming.GoToAndStop(interactionWeaponItem.transform.position + Vector3.right * 1.25f, interactionWeaponItem.gameObject);
    else
      playerFarming.GoToAndStop(interactionWeaponItem.transform.position + Vector3.left * 1.25f, interactionWeaponItem.gameObject);
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    PlayerFarming.ReloadAllFaith();
    interactionWeaponItem.IconSpriteRenderer.transform.DOShakePosition(2f, 0.25f);
    interactionWeaponItem.IconSpriteRenderer.transform.DOShakeRotation(2f, new Vector3(0.0f, 0.0f, 15f));
    Sequence Sequence = DOTween.Sequence();
    Sequence.Append((Tween) interactionWeaponItem.IconSpriteRenderer.transform.DOScale(Vector3.one * 1.2f, 0.3f));
    Sequence.Append((Tween) interactionWeaponItem.IconSpriteRenderer.transform.DOScale(Vector3.one * 0.8f, 0.3f));
    Sequence.Play<Sequence>().SetLoops<Sequence>(-1);
    if (!DataManager.Instance.CursePool.Contains(interactionWeaponItem.TypeOfWeapon))
      DataManager.Instance.CursePool.Add(interactionWeaponItem.TypeOfWeapon);
    interactionWeaponItem.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/absorb_curse", interactionWeaponItem.gameObject);
    playerFarming.Spine.AnimationState.SetAnimation(0, "Curses/curse-get", false);
    yield return (object) new WaitForSeconds(1.0333333f);
    interactionWeaponItem.IconSpriteRenderer.enabled = false;
    Sequence.Kill();
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionWeaponItem.state.CURRENT_STATE = StateMachine.State.Idle;
    if (playerFarming.currentCurse != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), interactionWeaponItem.state.transform.position, Quaternion.identity, interactionWeaponItem.transform.parent) as GameObject;
      gameObject.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), 270f);
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(playerFarming.currentCurse, playerFarming.currentCurseLevel, Interaction_WeaponPickUp.Types.Curse);
    }
    playerFarming.playerSpells.SetSpell(interactionWeaponItem.TypeOfWeapon, interactionWeaponItem.WeaponLevel);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWeaponItem.gameObject);
  }

  [CompilerGenerated]
  public void \u003CIndicateHighlighted\u003Eb__17_0()
  {
    this._weaponPickupUI = (UIWeaponPickupPromptController) null;
  }

  public enum Types
  {
    Weapon,
    Curse,
  }
}
