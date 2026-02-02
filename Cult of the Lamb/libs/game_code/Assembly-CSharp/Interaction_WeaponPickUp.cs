// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponPickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
public class Interaction_WeaponPickUp : Interaction
{
  public EquipmentType TypeOfWeapon;
  public Interaction_WeaponPickUp.Types Type;
  public int WeaponLevel = -1;
  public int DurabilityLevel = -1;
  public Canvas canvas;
  public float DamageDifference;
  public string DamageDifferenceString;
  public SpriteRenderer IconSpriteRenderer;
  public SpriteRenderer ShadowSpriteRenderer;
  public SpriteRenderer weaponBetterIcon;
  public Sprite weaponUp;
  public Sprite weaponDown;
  public UIWeaponPickupPromptController _weaponPickupUI;
  public string sLabel;
  public string sRecycle;
  public string sGain;
  public EquipmentType cacheCurse;
  public EquipmentType cacheWeapon;
  public bool Activated;
  public int RecycleCost = 1;

  public override bool InactiveAfterStopMoving => false;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.IconSpriteRenderer.transform.localScale = Vector3.one * 1.5f;
    this.canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    this.UpdateLocalisation();
    if (CoopManager.PreventRecycleInCurrentRoom)
    {
      this.HasSecondaryInteraction = false;
      this.SecondaryInteractable = false;
    }
    else
    {
      this.HasSecondaryInteraction = true;
      this.SecondaryInteractable = true;
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Equip;
    this.sRecycle = ScriptLocalization.Interactions.Recycle;
    this.sGain = ScriptLocalization.Interactions.Gain + " <sprite name=\"icon_UIHeartBlueHalf\">";
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.cacheWeapon == this.playerFarming.currentWeapon && this.cacheCurse == this.playerFarming.currentCurse)
      return;
    this.CheckWeaponLevel();
  }

  public void CheckWeaponLevel()
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      return;
    this.weaponBetterIcon.enabled = false;
    this.cacheCurse = this.playerFarming.currentCurse;
    this.cacheWeapon = this.playerFarming.currentWeapon;
    if (this.Type == Interaction_WeaponPickUp.Types.Curse)
    {
      if (this.playerFarming.currentCurseLevel > this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponDown;
        this.weaponBetterIcon.enabled = true;
      }
      else
      {
        if (this.playerFarming.currentCurseLevel >= this.WeaponLevel)
          return;
        this.weaponBetterIcon.sprite = this.weaponUp;
        this.weaponBetterIcon.enabled = true;
      }
    }
    else
    {
      if (this.Type != Interaction_WeaponPickUp.Types.Weapon)
        return;
      this.cacheCurse = this.TypeOfWeapon;
      if (this.playerFarming.currentWeaponLevel > this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponDown;
        this.weaponBetterIcon.enabled = true;
      }
      else
      {
        if (this.playerFarming.currentWeaponLevel >= this.WeaponLevel)
          return;
        this.weaponBetterIcon.sprite = this.weaponUp;
        this.weaponBetterIcon.enabled = true;
      }
    }
  }

  public Sprite GetIcon() => EquipmentManager.GetEquipmentData(this.TypeOfWeapon).WorldSprite;

  public void SetWeapon(
    EquipmentType TypeOfWeapon,
    int WeaponLevel,
    Interaction_WeaponPickUp.Types Type)
  {
    this.TypeOfWeapon = TypeOfWeapon;
    this.WeaponLevel = WeaponLevel;
    this.Type = Type;
    this.IconSpriteRenderer.sprite = this.GetIcon();
    this.CheckWeaponLevel();
  }

  public override void GetLabel()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!((UnityEngine.Object) player.interactor.TempInteraction != (UnityEngine.Object) this))
      {
        if (this.Activated)
        {
          this.Label = "";
          this.Interactable = false;
        }
        else
        {
          this.Label = this.sLabel;
          this.Interactable = true;
        }
      }
    }
  }

  public override void GetSecondaryLabel()
  {
    string str = this.sRecycle;
    if (this.Type == Interaction_WeaponPickUp.Types.Curse)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.currentCurse == EquipmentType.None)
          str = "";
      }
    }
    else if (this.Type == Interaction_WeaponPickUp.Types.Weapon)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.currentWeapon == EquipmentType.None)
          str = "";
      }
    }
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player.interactor.TempInteraction == (UnityEngine.Object) this || (UnityEngine.Object) player.interactor.CurrentInteraction == (UnityEngine.Object) this)
      {
        if (TrinketManager.HasTrinket(TarotCards.Card.Recycle, player))
        {
          str = $"{str} + {this.sGain}";
          break;
        }
        break;
      }
    }
    this.SecondaryLabel = this.Activated ? "" : str;
  }

  public override void OnInteract(StateMachine state)
  {
    this.Activated = true;
    base.OnInteract(state);
    this.weaponBetterIcon.enabled = false;
    switch (this.Type)
    {
      case Interaction_WeaponPickUp.Types.Weapon:
        this.StartCoroutine((IEnumerator) this.PlayerShowWeaponRoutine(state));
        break;
      case Interaction_WeaponPickUp.Types.Curse:
        this.StartCoroutine((IEnumerator) this.PlayerShowCurseRoutine(state));
        break;
    }
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    this.Activated = true;
    base.OnSecondaryInteract(state);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, this.RecycleCost, this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/shop/buy", this.transform.position);
    if (TrinketManager.HasTrinket(TarotCards.Card.Recycle, state.GetComponent<PlayerFarming>()))
      this.AddSpiritHearts();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public IEnumerator PlayerShowWeaponRoutine(StateMachine state)
  {
    Interaction_WeaponPickUp interactionWeaponPickUp = this;
    PlayerFarming component1 = state.GetComponent<PlayerFarming>();
    if (component1.currentWeapon != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), state.transform.position, Quaternion.identity, interactionWeaponPickUp.transform.parent) as GameObject;
      PickUp component2 = gameObject.GetComponent<PickUp>();
      component2.SetIgnoreBoundsCheck();
      component2.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), Utils.GetAngle(interactionWeaponPickUp.transform.position, Vector3.zero));
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(component1.currentWeapon, component1.playerWeapon.CurrentWeaponLevel, interactionWeaponPickUp.Type);
    }
    component1.playerWeapon.SetWeapon(interactionWeaponPickUp.TypeOfWeapon, interactionWeaponPickUp.WeaponLevel);
    interactionWeaponPickUp.IconSpriteRenderer.enabled = false;
    interactionWeaponPickUp.ShadowSpriteRenderer.enabled = false;
    state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", interactionWeaponPickUp.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", interactionWeaponPickUp.transform.position);
    yield return (object) new WaitForSeconds(component1.Spine.AnimationState.SetAnimation(0, EquipmentManager.GetEquipmentData(interactionWeaponPickUp.TypeOfWeapon).PickupAnimationKey, false).Animation.Duration);
    GameManager.GetInstance().CameraResetTargetZoom();
    state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWeaponPickUp.gameObject);
  }

  public IEnumerator PlayerShowCurseRoutine(StateMachine state)
  {
    Interaction_WeaponPickUp interactionWeaponPickUp = this;
    PlayerFarming playerFarming = state.GetComponent<PlayerFarming>();
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Obstacles")) | 1 << LayerMask.NameToLayer("Island"));
    bool flag1 = (bool) Physics2D.Raycast((Vector2) interactionWeaponPickUp.transform.position, (Vector2) Vector3.right, 1.5f, (int) layerMask);
    bool flag2 = (bool) Physics2D.Raycast((Vector2) interactionWeaponPickUp.transform.position, (Vector2) Vector3.left, 1.5f, (int) layerMask);
    if ((((double) interactionWeaponPickUp.transform.position.x >= (double) state.transform.position.x ? 0 : (!flag1 ? 1 : 0)) | (flag2 ? 1 : 0)) != 0)
      playerFarming.GoToAndStop(interactionWeaponPickUp.transform.position + Vector3.right * 1.25f, interactionWeaponPickUp.gameObject);
    else if ((((double) interactionWeaponPickUp.transform.position.x < (double) state.transform.position.x ? 0 : (!flag2 ? 1 : 0)) | (flag1 ? 1 : 0)) != 0)
      playerFarming.GoToAndStop(interactionWeaponPickUp.transform.position + Vector3.left * 1.25f, interactionWeaponPickUp.gameObject);
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/absorb_curse", interactionWeaponPickUp.gameObject);
    playerFarming.Spine.AnimationState.SetAnimation(0, "Curses/curse-get", false);
    interactionWeaponPickUp.IconSpriteRenderer.transform.DOShakePosition(2f, 0.25f);
    interactionWeaponPickUp.IconSpriteRenderer.transform.DOShakeRotation(2f, new Vector3(0.0f, 0.0f, 15f));
    Sequence sequence = DOTween.Sequence();
    sequence.Append((Tween) interactionWeaponPickUp.IconSpriteRenderer.transform.DOScale(Vector3.one * 1.25f, 0.2f));
    sequence.Append((Tween) interactionWeaponPickUp.IconSpriteRenderer.transform.DOScale(Vector3.one * 0.75f, 0.2f));
    sequence.Play<Sequence>().SetLoops<Sequence>(-1);
    yield return (object) new WaitForSeconds(1.0333333f);
    interactionWeaponPickUp.IconSpriteRenderer.enabled = false;
    interactionWeaponPickUp.ShadowSpriteRenderer.enabled = false;
    yield return (object) new WaitForSeconds(0.3f);
    if (playerFarming.currentCurse != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), state.transform.position, Quaternion.identity, interactionWeaponPickUp.transform.parent) as GameObject;
      PickUp component = gameObject.GetComponent<PickUp>();
      component.SetIgnoreBoundsCheck();
      component.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), Utils.GetAngle(interactionWeaponPickUp.transform.position, Vector3.zero));
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(playerFarming.currentCurse, playerFarming.currentCurseLevel, interactionWeaponPickUp.Type);
    }
    playerFarming.playerSpells.SetSpell(interactionWeaponPickUp.TypeOfWeapon, interactionWeaponPickUp.WeaponLevel);
    GameManager.GetInstance().CameraResetTargetZoom();
    state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWeaponPickUp.gameObject);
  }

  public void AddSpiritHearts()
  {
    if (PlayerFleeceManager.FleecePreventsHealthPickups())
      return;
    ++this.playerFarming.health.BlueHearts;
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.playerFarming.CameraBone.transform.position, 0.0f, "blue", "burst_big");
    AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", this.playerFarming.transform.position);
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
    switch (this.Type)
    {
      case Interaction_WeaponPickUp.Types.Weapon:
        System.Action<bool, PlayerFarming> onHighlightWeapon = Interaction_WeaponSelectionPodium.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(true, playerFarming);
          break;
        }
        break;
      case Interaction_WeaponPickUp.Types.Curse:
        System.Action<bool, PlayerFarming> onHighlightCurse = Interaction_WeaponSelectionPodium.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(true, playerFarming);
          break;
        }
        break;
    }
    float damage = 0.0f;
    float speed = 0.0f;
    if (this.Type == Interaction_WeaponPickUp.Types.Weapon)
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
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu", playerFarming.transform.position);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
    switch (this.Type)
    {
      case Interaction_WeaponPickUp.Types.Weapon:
        System.Action<bool, PlayerFarming> onHighlightWeapon = Interaction_WeaponSelectionPodium.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(false, playerFarming);
          break;
        }
        break;
      case Interaction_WeaponPickUp.Types.Curse:
        System.Action<bool, PlayerFarming> onHighlightCurse = Interaction_WeaponSelectionPodium.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(false, playerFarming);
          break;
        }
        break;
    }
    if (!((UnityEngine.Object) this._weaponPickupUI != (UnityEngine.Object) null))
      return;
    this._weaponPickupUI.Hide();
  }

  [CompilerGenerated]
  public void \u003CIndicateHighlighted\u003Eb__36_0()
  {
    this._weaponPickupUI = (UIWeaponPickupPromptController) null;
  }

  public enum Types
  {
    Weapon,
    Curse,
  }
}
