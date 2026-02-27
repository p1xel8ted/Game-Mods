// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_WeaponItem : Interaction
{
  public EquipmentType TypeOfWeapon;
  public WeaponShopKeeperManager ShopKeeper;
  private int WeaponLevel;
  private int Cost;
  private WeaponPickUpUI weaponPickupUI;
  private Canvas canvas;
  [SerializeField]
  public SpriteRenderer IconSpriteRenderer;
  public static System.Action<bool> OnHighlightWeapon;
  public static System.Action<bool> OnHighlightCurse;
  private bool Activated;
  public Interaction_WeaponItem.Types Type;
  private float WobbleTimer;
  private string BuyString;

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

  private void Start()
  {
    this.WobbleTimer = (float) UnityEngine.Random.Range(0, 360);
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
  }

  private Sprite GetIcon() => EquipmentManager.GetEquipmentData(this.TypeOfWeapon).WorldSprite;

  private new void Update()
  {
    this.IconSpriteRenderer.transform.localPosition = new Vector3(0.0f, 0.0f, (float) ((double) Mathf.Sin(this.WobbleTimer += Time.deltaTime * 2f) * 0.10000000149011612 - 0.75));
  }

  public override void IndicateHighlighted()
  {
    base.IndicateHighlighted();
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
    Interaction_WeaponSelectionPodium.Types type = Interaction_WeaponSelectionPodium.Types.Curse;
    if (this.Type == Interaction_WeaponItem.Types.Weapon)
    {
      damage = PlayerFarming.Instance.playerWeapon.GetAverageWeaponDamage(this.TypeOfWeapon, this.WeaponLevel);
      speed = PlayerFarming.Instance.playerWeapon.GetWeaponSpeed(this.TypeOfWeapon);
      type = Interaction_WeaponSelectionPodium.Types.Weapon;
    }
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu", PlayerFarming.Instance.transform.position);
    this.weaponPickupUI = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Weapon Pickup"), GameObject.FindWithTag("Canvas").transform).GetComponent<WeaponPickUpUI>();
    this.weaponPickupUI.Play(this.TypeOfWeapon, this.WeaponLevel, this.IconSpriteRenderer.sprite, damage, speed, this.Type == Interaction_WeaponItem.Types.Weapon ? EquipmentManager.GetWeaponData(this.TypeOfWeapon).Attachments : new List<WeaponAttachmentData>(), this.gameObject, this.Type == Interaction_WeaponItem.Types.Curse, type);
    MonoSingleton<Indicator>.Instance.RectTransform.parent = this.weaponPickupUI.transform;
    MonoSingleton<Indicator>.Instance.UpdatePosition = false;
    MonoSingleton<Indicator>.Instance.RectTransform.anchoredPosition = (Vector2) new Vector3(0.0f, -50f, 0.0f);
  }

  public override void EndIndicateHighlighted()
  {
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
    if (!(bool) (UnityEngine.Object) this.weaponPickupUI || !((UnityEngine.Object) MonoSingleton<Indicator>.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.weaponPickupUI.gameObject != (UnityEngine.Object) null) || !((UnityEngine.Object) this.weaponPickupUI.canvasGroup != (UnityEngine.Object) null) || !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) MonoSingleton<Indicator>.Instance.RectTransform != (UnityEngine.Object) null) || !((UnityEngine.Object) MonoSingleton<Indicator>.Instance.RectTransform.parent != (UnityEngine.Object) null))
      return;
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu", PlayerFarming.Instance.transform.position);
    if ((UnityEngine.Object) this.canvas == (UnityEngine.Object) null)
      this.canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    MonoSingleton<Indicator>.Instance.gameObject.SetActive(false);
    MonoSingleton<Indicator>.Instance.RectTransform.parent = this.canvas.transform;
    MonoSingleton<Indicator>.Instance.RectTransform.anchoredPosition = MonoSingleton<Indicator>.Instance.CachedPosition;
    MonoSingleton<Indicator>.Instance.UpdatePosition = true;
    base.EndIndicateHighlighted();
    this.weaponPickupUI.canvasGroup.DOFade(0.0f, 0.5f);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.weaponPickupUI.gameObject, 1f);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.BuyString = ScriptLocalization.Interactions.Buy;
  }

  private string GetAffordColor()
  {
    return Inventory.GetItemQuantity(20) >= this.Cost - 1 ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void GetLabel()
  {
    this.Label = $"{string.Format(this.BuyString, (object) $"{EquipmentManager.GetEquipmentData(this.TypeOfWeapon).GetLocalisedTitle()} {this.WeaponLevel.ToNumeral()}")} | {this.GetAffordColor()}{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.BLACK_GOLD)} {(object) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD)} / {(object) this.Cost}</color>";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(20) >= this.Cost - 1 && !this.Activated)
    {
      this.Activated = true;
      for (int index = 0; index < this.Cost; ++index)
      {
        if (index < 10)
        {
          Inventory.GetItemByType(20);
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.Instance.transform.position);
          ResourceCustomTarget.Create(this.gameObject, PlayerFarming.Instance.gameObject.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
        }
        Inventory.ChangeItemQuantity(20, -1);
      }
      CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360));
      switch (this.Type)
      {
        case Interaction_WeaponItem.Types.Weapon:
          this.StartCoroutine((IEnumerator) this.PlayerShowWeaponRoutine());
          break;
        case Interaction_WeaponItem.Types.Curse:
          this.StartCoroutine((IEnumerator) this.PlayerShowCurseRoutine());
          break;
      }
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
      MonoSingleton<Indicator>.Instance.PlayShake();
      if (!((UnityEngine.Object) this.ShopKeeper.CantAffordBark != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.ShopKeeper.NormalBark != (UnityEngine.Object) null)
        this.ShopKeeper.NormalBark.SetActive(false);
      this.ShopKeeper.CantAffordBark.SetActive(true);
    }
  }

  private IEnumerator PlayerShowWeaponRoutine()
  {
    Interaction_WeaponItem interactionWeaponItem = this;
    PlayerFarming.Instance.GoToAndStop(interactionWeaponItem.transform.position + Vector3.down * 0.5f, interactionWeaponItem.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    if (DataManager.Instance.CurrentWeapon != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), interactionWeaponItem.state.transform.position, Quaternion.identity, interactionWeaponItem.transform.parent) as GameObject;
      gameObject.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), 270f);
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(DataManager.Instance.CurrentWeapon, PlayerFarming.Instance.playerWeapon.CurrentWeaponLevel, Interaction_WeaponPickUp.Types.Weapon);
    }
    PlayerFarming.Instance.playerWeapon.SetWeapon(interactionWeaponItem.TypeOfWeapon, interactionWeaponItem.WeaponLevel);
    interactionWeaponItem.IconSpriteRenderer.enabled = false;
    if (!DataManager.Instance.WeaponPool.Contains(interactionWeaponItem.TypeOfWeapon))
      DataManager.Instance.WeaponPool.Add(interactionWeaponItem.TypeOfWeapon);
    interactionWeaponItem.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", interactionWeaponItem.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", interactionWeaponItem.transform.position);
    yield return (object) new WaitForSeconds(PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.PickupAnimationKey, false).Animation.Duration);
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionWeaponItem.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWeaponItem.gameObject);
  }

  private IEnumerator PlayerShowCurseRoutine()
  {
    Interaction_WeaponItem interactionWeaponItem = this;
    if ((double) interactionWeaponItem.transform.position.x < (double) interactionWeaponItem.state.transform.position.x)
      PlayerFarming.Instance.GoToAndStop(interactionWeaponItem.transform.position + Vector3.right * 1.25f, interactionWeaponItem.gameObject);
    else
      PlayerFarming.Instance.GoToAndStop(interactionWeaponItem.transform.position + Vector3.left * 1.25f, interactionWeaponItem.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    FaithAmmo.Reload();
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
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Curses/curse-get", false);
    yield return (object) new WaitForSeconds(1.0333333f);
    interactionWeaponItem.IconSpriteRenderer.enabled = false;
    Sequence.Kill();
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionWeaponItem.state.CURRENT_STATE = StateMachine.State.Idle;
    if (DataManager.Instance.CurrentCurse != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), interactionWeaponItem.state.transform.position, Quaternion.identity, interactionWeaponItem.transform.parent) as GameObject;
      gameObject.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), 270f);
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(DataManager.Instance.CurrentCurse, DataManager.Instance.CurrentCurseLevel, Interaction_WeaponPickUp.Types.Curse);
    }
    PlayerFarming.Instance.playerSpells.SetSpell(interactionWeaponItem.TypeOfWeapon, interactionWeaponItem.WeaponLevel);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWeaponItem.gameObject);
  }

  public enum Types
  {
    Weapon,
    Curse,
  }
}
