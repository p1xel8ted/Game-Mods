// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponSelectionPodium
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_WeaponSelectionPodium : Interaction
{
  protected int WeaponLevel;
  [SerializeField]
  public SpriteRenderer IconSpriteRenderer;
  [SerializeField]
  protected SpriteRenderer LockedIcon;
  private WeaponPickUpUI weaponPickupUI;
  public ParticleSystem particleEffect;
  public GameObject podiumOn;
  public GameObject podiumOff;
  protected Canvas canvas;
  [SerializeField]
  public Animator AvailableGoop;
  [SerializeField]
  private NewWeaponEffect NewWeaponEffect;
  public static System.Action<bool> OnHighlightWeapon;
  public static System.Action<bool> OnHighlightCurse;
  public GameObject Lighting;
  public Material WeaponMaterial;
  public Material CurseMaterial;
  private LayerMask collisionMask;
  public SpriteRenderer weaponBetterIcon;
  public Sprite weaponUp;
  public Sprite weaponDown;
  public Interaction_WeaponSelectionPodium.Types Type;
  public bool RemoveIfNotFirstLayer;
  protected bool WeaponTaken;
  private bool initialDungeonEnter;
  private bool activated;
  private float WobbleTimer;
  private string sLabel;
  public bool ReadyToOpenDoor;
  public static List<Interaction_WeaponSelectionPodium> Podiums = new List<Interaction_WeaponSelectionPodium>();

  public EquipmentType TypeOfWeapon { get; protected set; } = EquipmentType.None;

  private void Awake()
  {
    this.initialDungeonEnter = GameManager.InitialDungeonEnter;
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_WeaponSelectionPodium.Podiums.Add(this);
    this.WobbleTimer = (float) UnityEngine.Random.Range(0, 360);
    if (this.Type == Interaction_WeaponSelectionPodium.Types.Random)
      this.Type = (double) UnityEngine.Random.value >= 0.5 ? Interaction_WeaponSelectionPodium.Types.Curse : Interaction_WeaponSelectionPodium.Types.Weapon;
    if (PlayerFarming.Location == FollowerLocation.IntroDungeon)
      this.Type = Interaction_WeaponSelectionPodium.Types.Weapon;
    if (this.RemoveIfNotFirstLayer && (GameManager.CurrentDungeonFloor > 1 || !this.initialDungeonEnter))
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else if (this.Type == Interaction_WeaponSelectionPodium.Types.Curse && !DataManager.Instance.EnabledSpells)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      if (!this.WeaponTaken && this.Type == Interaction_WeaponSelectionPodium.Types.Weapon)
        BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.LockDoors);
      Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.Reveal);
      if (!this.WeaponTaken)
      {
        this.AvailableGoop.Play("Show");
      }
      else
      {
        this.AvailableGoop.Play("Hidden");
        this.particleEffect.Stop();
      }
      this.weaponBetterIcon.enabled = false;
      this.CheckWeaponLevel();
    }
  }

  private void CheckWeaponLevel()
  {
    if (this.Type == Interaction_WeaponSelectionPodium.Types.Curse)
    {
      if (DataManager.Instance.CurrentCurseLevel > this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponDown;
        this.weaponBetterIcon.enabled = true;
      }
      else if (DataManager.Instance.CurrentCurseLevel < this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponUp;
        this.weaponBetterIcon.enabled = true;
      }
      if (DataManager.Instance.CurrentCurse != EquipmentType.None)
        return;
      this.weaponBetterIcon.enabled = false;
    }
    else
    {
      if (this.Type != Interaction_WeaponSelectionPodium.Types.Weapon)
        return;
      if (DataManager.Instance.CurrentWeaponLevel > this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponDown;
        this.weaponBetterIcon.enabled = true;
      }
      else if (DataManager.Instance.CurrentWeaponLevel < this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponUp;
        this.weaponBetterIcon.enabled = true;
      }
      if (DataManager.Instance.CurrentWeapon != EquipmentType.None)
        return;
      this.weaponBetterIcon.enabled = false;
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_WeaponSelectionPodium.Podiums.Remove(this);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.LockDoors);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.Reveal);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.LockDoors);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.Reveal);
  }

  public void Reveal()
  {
    Debug.Log((object) "Reveal()");
    if (PlayerFarming.Location == FollowerLocation.IntroDungeon)
      return;
    this.gameObject.SetActive(true);
    this.NewWeaponEffect.gameObject.SetActive(true);
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    if (this.TypeOfWeapon != EquipmentType.None)
      return;
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
    this.StartCoroutine((IEnumerator) this.SetItem());
    this.podiumOff.SetActive(false);
  }

  public void ResetRandom()
  {
    this.IconSpriteRenderer.enabled = true;
    this.WeaponTaken = false;
    this.StartCoroutine((IEnumerator) this.SetItem());
  }

  private IEnumerator SetItem()
  {
    yield return (object) new WaitForEndOfFrame();
    switch (this.Type)
    {
      case Interaction_WeaponSelectionPodium.Types.Weapon:
        this.SetWeapon();
        break;
      case Interaction_WeaponSelectionPodium.Types.Curse:
        if (DataManager.Instance.EnabledSpells)
        {
          this.SetCurse();
          break;
        }
        this.Type = Interaction_WeaponSelectionPodium.Types.Weapon;
        this.SetWeapon();
        break;
    }
    this.IconSpriteRenderer.sprite = this.GetIcon();
    this.LockedIcon.gameObject.SetActive(false);
    this.canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    if (!this.WeaponTaken)
      this.AvailableGoop.Play("Show");
    else
      this.AvailableGoop.Play("Hidden");
    this.CheckWeaponLevel();
  }

  private void LockDoors()
  {
    if (!this.RemoveIfNotFirstLayer || GameManager.CurrentDungeonFloor > 1)
      return;
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.LockDoors);
    RoomLockController.CloseAll();
  }

  protected virtual void SetWeapon()
  {
    this.IconSpriteRenderer.material = this.WeaponMaterial;
    this.TypeOfWeapon = DataManager.Instance.GetRandomWeaponInPool();
    this.WeaponLevel = DataManager.Instance.CurrentRunWeaponLevel + 1;
    if (DataManager.Instance.ForcedStartingWeapon != EquipmentType.None)
    {
      this.TypeOfWeapon = DataManager.Instance.ForcedStartingWeapon;
      DataManager.Instance.ForcedStartingWeapon = EquipmentType.None;
    }
    if (DataManager.Instance.CurrentWeapon == EquipmentType.None)
      this.WeaponLevel += DataManager.StartingEquipmentLevel;
    DataManager.Instance.CurrentRunWeaponLevel = this.WeaponLevel;
  }

  protected virtual void SetCurse()
  {
    this.IconSpriteRenderer.material = this.CurseMaterial;
    this.TypeOfWeapon = DataManager.Instance.GetRandomCurseInPool();
    this.WeaponLevel = DataManager.Instance.CurrentRunCurseLevel + 1;
    if (DataManager.Instance.ForcedStartingCurse != EquipmentType.None)
    {
      this.TypeOfWeapon = DataManager.Instance.ForcedStartingCurse;
      DataManager.Instance.ForcedStartingCurse = EquipmentType.None;
    }
    if (DataManager.Instance.CurrentCurse == EquipmentType.None)
      this.WeaponLevel += DataManager.StartingEquipmentLevel;
    DataManager.Instance.CurrentRunCurseLevel = this.WeaponLevel;
  }

  protected Sprite GetIcon() => EquipmentManager.GetEquipmentData(this.TypeOfWeapon).WorldSprite;

  private new void Update()
  {
    if (!this.WeaponTaken && (bool) (UnityEngine.Object) this.IconSpriteRenderer)
      this.IconSpriteRenderer.transform.localPosition = new Vector3(0.0f, 0.0f, Mathf.Sin(this.WobbleTimer += Time.deltaTime * 2f) * 0.1f);
    if (!((UnityEngine.Object) this.weaponPickupUI != (UnityEngine.Object) null) || MonoSingleton<Indicator>.Instance.gameObject.activeSelf)
      return;
    MonoSingleton<Indicator>.Instance.RectTransform.parent = this.canvas.transform;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.weaponPickupUI.gameObject);
    this.weaponPickupUI = (WeaponPickUpUI) null;
  }

  public override void IndicateHighlighted()
  {
    base.IndicateHighlighted();
    float damage = 0.0f;
    float speed = 0.0f;
    switch (this.Type)
    {
      case Interaction_WeaponSelectionPodium.Types.Weapon:
        System.Action<bool> onHighlightWeapon = Interaction_WeaponSelectionPodium.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(true);
          break;
        }
        break;
      case Interaction_WeaponSelectionPodium.Types.Curse:
        System.Action<bool> onHighlightCurse = Interaction_WeaponSelectionPodium.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(true);
          break;
        }
        break;
    }
    if (this.Type == Interaction_WeaponSelectionPodium.Types.Weapon)
    {
      damage = PlayerFarming.Instance.playerWeapon.GetAverageWeaponDamage(this.TypeOfWeapon, this.WeaponLevel);
      speed = PlayerFarming.Instance.playerWeapon.GetWeaponSpeed(this.TypeOfWeapon);
    }
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu", PlayerFarming.Instance.transform.position);
    this.weaponPickupUI = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Weapon Pickup")).GetComponent<WeaponPickUpUI>();
    this.weaponPickupUI.Play(this.TypeOfWeapon, this.WeaponLevel, this.IconSpriteRenderer.sprite, damage, speed, this.Type != Interaction_WeaponSelectionPodium.Types.Weapon || !((UnityEngine.Object) EquipmentManager.GetWeaponData(this.TypeOfWeapon) != (UnityEngine.Object) null) ? new List<WeaponAttachmentData>() : EquipmentManager.GetWeaponData(this.TypeOfWeapon).Attachments, this.gameObject, this.Type == Interaction_WeaponSelectionPodium.Types.Curse, this.Type);
    this.weaponPickupUI.transform.parent = MonoSingleton<Indicator>.Instance.transform;
    this.weaponPickupUI.transform.localScale = Vector3.one;
  }

  public override void EndIndicateHighlighted()
  {
    switch (this.Type)
    {
      case Interaction_WeaponSelectionPodium.Types.Weapon:
        System.Action<bool> onHighlightWeapon = Interaction_WeaponSelectionPodium.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(false);
          break;
        }
        break;
      case Interaction_WeaponSelectionPodium.Types.Curse:
        System.Action<bool> onHighlightCurse = Interaction_WeaponSelectionPodium.OnHighlightCurse;
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
    base.EndIndicateHighlighted();
    this.weaponPickupUI.canvasGroup.DOFade(0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.weaponPickupUI.gameObject, 1f)));
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Equip;
  }

  public override void GetLabel() => this.Label = this.WeaponTaken ? "" : this.sLabel;

  public override void OnInteract(StateMachine state)
  {
    if (this.activated)
      return;
    base.OnInteract(state);
    this.weaponBetterIcon.enabled = false;
    this.activated = true;
    switch (this.Type)
    {
      case Interaction_WeaponSelectionPodium.Types.Weapon:
        this.StartCoroutine((IEnumerator) this.PlayerShowWeaponRoutine());
        break;
      case Interaction_WeaponSelectionPodium.Types.Curse:
        this.StartCoroutine((IEnumerator) this.PlayerShowCurseRoutine());
        break;
    }
  }

  private IEnumerator PlayerShowWeaponRoutine()
  {
    Interaction_WeaponSelectionPodium weaponSelectionPodium = this;
    weaponSelectionPodium.AvailableGoop.Play("Hide");
    PlayerFarming.Instance.GoToAndStop(weaponSelectionPodium.transform.position + Vector3.down * 0.5f, weaponSelectionPodium.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    if (DataManager.Instance.CurrentWeapon != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), weaponSelectionPodium.state.transform.position, Quaternion.identity, weaponSelectionPodium.transform.parent) as GameObject;
      gameObject.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), 270f);
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(DataManager.Instance.CurrentWeapon, PlayerFarming.Instance.playerWeapon.CurrentWeaponLevel, Interaction_WeaponPickUp.Types.Weapon);
    }
    if (weaponSelectionPodium.TypeOfWeapon == EquipmentType.None)
      weaponSelectionPodium.TypeOfWeapon = EquipmentType.Sword;
    PlayerFarming.Instance.playerWeapon.SetWeapon(weaponSelectionPodium.TypeOfWeapon, weaponSelectionPodium.WeaponLevel);
    weaponSelectionPodium.WeaponTaken = true;
    weaponSelectionPodium.Lighting.SetActive(false);
    weaponSelectionPodium.IconSpriteRenderer.enabled = false;
    weaponSelectionPodium.podiumOn.SetActive(false);
    weaponSelectionPodium.podiumOff.SetActive(true);
    weaponSelectionPodium.particleEffect.Stop();
    if (!DataManager.Instance.WeaponPool.Contains(weaponSelectionPodium.TypeOfWeapon))
      DataManager.Instance.WeaponPool.Add(weaponSelectionPodium.TypeOfWeapon);
    weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", weaponSelectionPodium.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", weaponSelectionPodium.transform.position);
    yield return (object) new WaitForSeconds(PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.PickupAnimationKey, false).Animation.Duration);
    GameManager.GetInstance().CameraResetTargetZoom();
    weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.Idle;
    if (weaponSelectionPodium.RemoveIfNotFirstLayer && GameManager.CurrentDungeonFloor <= 1)
    {
      weaponSelectionPodium.ReadyToOpenDoor = true;
      bool flag = true;
      foreach (Interaction_WeaponSelectionPodium podium in Interaction_WeaponSelectionPodium.Podiums)
      {
        if (!podium.ReadyToOpenDoor)
          flag = false;
      }
      if (flag)
        RoomLockController.RoomCompleted();
    }
  }

  private IEnumerator PlayerShowCurseRoutine()
  {
    Interaction_WeaponSelectionPodium weaponSelectionPodium = this;
    weaponSelectionPodium.AvailableGoop.Play("Hide");
    if ((double) weaponSelectionPodium.transform.position.x < (double) weaponSelectionPodium.state.transform.position.x)
    {
      Vector3 TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) weaponSelectionPodium.transform.position, (Vector2) Vector3.right, 3f, (int) weaponSelectionPodium.collisionMask).collider != (UnityEngine.Object) null ? weaponSelectionPodium.transform.position + Vector3.left * 1.25f : weaponSelectionPodium.transform.position + Vector3.right * 1.25f;
      PlayerFarming.Instance.GoToAndStop(TargetPosition, weaponSelectionPodium.gameObject);
    }
    else
    {
      Vector3 TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) weaponSelectionPodium.transform.position, (Vector2) Vector3.left, 3f, (int) weaponSelectionPodium.collisionMask).collider != (UnityEngine.Object) null ? weaponSelectionPodium.transform.position + Vector3.right * 1.25f : weaponSelectionPodium.transform.position + Vector3.left * 1.25f;
      PlayerFarming.Instance.GoToAndStop(TargetPosition, weaponSelectionPodium.gameObject);
    }
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    FaithAmmo.Reload();
    weaponSelectionPodium.WeaponTaken = true;
    weaponSelectionPodium.IconSpriteRenderer.transform.DOShakePosition(2f, 0.25f);
    weaponSelectionPodium.IconSpriteRenderer.transform.DOShakeRotation(2f, new Vector3(0.0f, 0.0f, 15f));
    DG.Tweening.Sequence Sequence = DOTween.Sequence();
    Sequence.Append((Tween) weaponSelectionPodium.IconSpriteRenderer.transform.DOScale(Vector3.one * 1.2f, 0.3f));
    Sequence.Append((Tween) weaponSelectionPodium.IconSpriteRenderer.transform.DOScale(Vector3.one * 0.8f, 0.3f));
    Sequence.Play<DG.Tweening.Sequence>().SetLoops<DG.Tweening.Sequence>(-1);
    if (!DataManager.Instance.CursePool.Contains(weaponSelectionPodium.TypeOfWeapon))
      DataManager.Instance.CursePool.Add(weaponSelectionPodium.TypeOfWeapon);
    weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/absorb_curse", weaponSelectionPodium.gameObject);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Curses/curse-get", false);
    yield return (object) new WaitForSeconds(1.0333333f);
    weaponSelectionPodium.IconSpriteRenderer.enabled = false;
    Sequence.Kill();
    weaponSelectionPodium.Lighting.SetActive(false);
    weaponSelectionPodium.podiumOn.SetActive(false);
    weaponSelectionPodium.podiumOff.SetActive(true);
    weaponSelectionPodium.particleEffect.Stop();
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().CameraResetTargetZoom();
    weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.Idle;
    if (DataManager.Instance.CurrentCurse != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), weaponSelectionPodium.state.transform.position, Quaternion.identity, weaponSelectionPodium.transform.parent) as GameObject;
      gameObject.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), 270f);
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(DataManager.Instance.CurrentCurse, DataManager.Instance.CurrentCurseLevel, Interaction_WeaponPickUp.Types.Curse);
    }
    PlayerFarming.Instance.playerSpells.SetSpell(weaponSelectionPodium.TypeOfWeapon, weaponSelectionPodium.WeaponLevel);
    if (weaponSelectionPodium.RemoveIfNotFirstLayer && GameManager.CurrentDungeonFloor <= 1)
    {
      weaponSelectionPodium.ReadyToOpenDoor = true;
      bool flag = true;
      foreach (Interaction_WeaponSelectionPodium podium in Interaction_WeaponSelectionPodium.Podiums)
      {
        if (!podium.ReadyToOpenDoor)
          flag = false;
      }
      if (flag)
        RoomLockController.RoomCompleted();
    }
  }

  public enum Types
  {
    Random,
    Weapon,
    Curse,
  }

  [Serializable]
  public class WeaponIcons
  {
    public TarotCards.Card Weapon;
    public Sprite Sprite;
  }
}
