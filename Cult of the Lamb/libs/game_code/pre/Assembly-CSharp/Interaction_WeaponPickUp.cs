// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponPickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_WeaponPickUp : Interaction
{
  public EquipmentType TypeOfWeapon;
  public Interaction_WeaponPickUp.Types Type;
  public int WeaponLevel = -1;
  public int DurabilityLevel = -1;
  private WeaponPickUpUI weaponPickupUI;
  private Canvas canvas;
  private float DamageDifference;
  private string DamageDifferenceString;
  public SpriteRenderer IconSpriteRenderer;
  public SpriteRenderer ShadowSpriteRenderer;
  public SpriteRenderer weaponBetterIcon;
  public Sprite weaponUp;
  public Sprite weaponDown;
  private string sLabel;
  private string sRecycle;
  private EquipmentType cacheCurse;
  private EquipmentType cacheWeapon;
  private bool Activated;
  private int RecycleCost = 1;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.IconSpriteRenderer.transform.localScale = Vector3.one * 1.5f;
    this.canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    this.UpdateLocalisation();
    this.HasSecondaryInteraction = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Equip;
    this.sRecycle = ScriptLocalization.Interactions.Recycle;
  }

  protected override void Update()
  {
    base.Update();
    if (this.cacheWeapon == DataManager.Instance.CurrentWeapon && this.cacheCurse == DataManager.Instance.CurrentWeapon)
      return;
    this.CheckWeaponLevel();
  }

  private void CheckWeaponLevel()
  {
    this.weaponBetterIcon.enabled = false;
    this.cacheCurse = DataManager.Instance.CurrentCurse;
    this.cacheCurse = DataManager.Instance.CurrentWeapon;
    if (this.Type == Interaction_WeaponPickUp.Types.Curse)
    {
      if (DataManager.Instance.CurrentCurseLevel > this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponDown;
        this.weaponBetterIcon.enabled = true;
      }
      else
      {
        if (DataManager.Instance.CurrentCurseLevel >= this.WeaponLevel)
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
      if (DataManager.Instance.CurrentWeaponLevel > this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponDown;
        this.weaponBetterIcon.enabled = true;
      }
      else
      {
        if (DataManager.Instance.CurrentWeaponLevel >= this.WeaponLevel)
          return;
        this.weaponBetterIcon.sprite = this.weaponUp;
        this.weaponBetterIcon.enabled = true;
      }
    }
  }

  private Sprite GetIcon() => EquipmentManager.GetEquipmentData(this.TypeOfWeapon).WorldSprite;

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

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sLabel;

  public override void GetSecondaryLabel()
  {
    this.SecondaryLabel = this.Activated ? "" : this.sRecycle;
  }

  public override void OnInteract(StateMachine state)
  {
    this.Activated = true;
    base.OnInteract(state);
    this.weaponBetterIcon.enabled = false;
    switch (this.Type)
    {
      case Interaction_WeaponPickUp.Types.Weapon:
        this.StartCoroutine((IEnumerator) this.PlayerShowWeaponRoutine());
        break;
      case Interaction_WeaponPickUp.Types.Curse:
        this.StartCoroutine((IEnumerator) this.PlayerShowCurseRoutine());
        break;
    }
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    this.Activated = true;
    base.OnSecondaryInteract(state);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, this.RecycleCost, this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/shop/buy", this.transform.position);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private IEnumerator PlayerShowWeaponRoutine()
  {
    Interaction_WeaponPickUp interactionWeaponPickUp = this;
    if (DataManager.Instance.CurrentWeapon != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), interactionWeaponPickUp.state.transform.position, Quaternion.identity, interactionWeaponPickUp.transform.parent) as GameObject;
      gameObject.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), Utils.GetAngle(interactionWeaponPickUp.transform.position, Vector3.zero));
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(DataManager.Instance.CurrentWeapon, PlayerFarming.Instance.playerWeapon.CurrentWeaponLevel, interactionWeaponPickUp.Type);
    }
    PlayerFarming.Instance.playerWeapon.SetWeapon(interactionWeaponPickUp.TypeOfWeapon, interactionWeaponPickUp.WeaponLevel);
    interactionWeaponPickUp.IconSpriteRenderer.enabled = false;
    interactionWeaponPickUp.ShadowSpriteRenderer.enabled = false;
    interactionWeaponPickUp.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", interactionWeaponPickUp.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", interactionWeaponPickUp.transform.position);
    yield return (object) new WaitForSeconds(PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, EquipmentManager.GetEquipmentData(interactionWeaponPickUp.TypeOfWeapon).PickupAnimationKey, false).Animation.Duration);
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionWeaponPickUp.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWeaponPickUp.gameObject);
  }

  private IEnumerator PlayerShowCurseRoutine()
  {
    Interaction_WeaponPickUp interactionWeaponPickUp = this;
    if ((double) interactionWeaponPickUp.transform.position.x < (double) interactionWeaponPickUp.state.transform.position.x)
      PlayerFarming.Instance.GoToAndStop(interactionWeaponPickUp.transform.position + Vector3.right * 1.25f, interactionWeaponPickUp.gameObject);
    else
      PlayerFarming.Instance.GoToAndStop(interactionWeaponPickUp.transform.position + Vector3.left * 1.25f, interactionWeaponPickUp.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    interactionWeaponPickUp.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/absorb_curse", interactionWeaponPickUp.gameObject);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Curses/curse-get", false);
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
    if (DataManager.Instance.CurrentCurse != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), interactionWeaponPickUp.state.transform.position, Quaternion.identity, interactionWeaponPickUp.transform.parent) as GameObject;
      gameObject.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), Utils.GetAngle(interactionWeaponPickUp.transform.position, Vector3.zero));
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(DataManager.Instance.CurrentCurse, DataManager.Instance.CurrentCurseLevel, interactionWeaponPickUp.Type);
    }
    PlayerFarming.Instance.playerSpells.SetSpell(interactionWeaponPickUp.TypeOfWeapon, interactionWeaponPickUp.WeaponLevel);
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionWeaponPickUp.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWeaponPickUp.gameObject);
  }

  public override void IndicateHighlighted()
  {
    base.IndicateHighlighted();
    switch (this.Type)
    {
      case Interaction_WeaponPickUp.Types.Weapon:
        System.Action<bool> onHighlightWeapon = Interaction_WeaponSelectionPodium.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(true);
          break;
        }
        break;
      case Interaction_WeaponPickUp.Types.Curse:
        System.Action<bool> onHighlightCurse = Interaction_WeaponSelectionPodium.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(true);
          break;
        }
        break;
    }
    float damage = 0.0f;
    float speed = 0.0f;
    Interaction_WeaponSelectionPodium.Types type = Interaction_WeaponSelectionPodium.Types.Curse;
    if (this.Type == Interaction_WeaponPickUp.Types.Weapon)
    {
      damage = PlayerFarming.Instance.playerWeapon.GetAverageWeaponDamage(this.TypeOfWeapon, this.WeaponLevel);
      speed = PlayerFarming.Instance.playerWeapon.GetWeaponSpeed(this.TypeOfWeapon);
      type = Interaction_WeaponSelectionPodium.Types.Weapon;
    }
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu", PlayerFarming.Instance.transform.position);
    this.weaponPickupUI = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Weapon Pickup")).GetComponent<WeaponPickUpUI>();
    this.weaponPickupUI.Play(this.TypeOfWeapon, this.WeaponLevel, this.IconSpriteRenderer.sprite, damage, speed, this.Type == Interaction_WeaponPickUp.Types.Weapon ? EquipmentManager.GetWeaponData(this.TypeOfWeapon).Attachments : (List<WeaponAttachmentData>) null, this.gameObject, this.Type == Interaction_WeaponPickUp.Types.Curse, type);
    this.weaponPickupUI.transform.parent = MonoSingleton<Indicator>.Instance.transform;
    this.weaponPickupUI.transform.localScale = Vector3.one;
  }

  public override void EndIndicateHighlighted()
  {
    switch (this.Type)
    {
      case Interaction_WeaponPickUp.Types.Weapon:
        System.Action<bool> onHighlightWeapon = Interaction_WeaponSelectionPodium.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(false);
          break;
        }
        break;
      case Interaction_WeaponPickUp.Types.Curse:
        System.Action<bool> onHighlightCurse = Interaction_WeaponSelectionPodium.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(false);
          break;
        }
        break;
    }
    if (!(bool) (UnityEngine.Object) this.weaponPickupUI)
      return;
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu", PlayerFarming.Instance.transform.position);
    base.EndIndicateHighlighted();
    this.weaponPickupUI.canvasGroup.DOFade(0.0f, 0.5f);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.weaponPickupUI.gameObject, 1f);
  }

  public enum Types
  {
    Weapon,
    Curse,
  }
}
