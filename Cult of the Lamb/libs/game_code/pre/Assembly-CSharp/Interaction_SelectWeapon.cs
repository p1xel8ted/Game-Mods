// Decompiled with JetBrains decompiler
// Type: Interaction_SelectWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using BlendModes;
using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Rewired;
using System.Collections;
using Unify.Input;
using UnityEngine;

#nullable disable
public class Interaction_SelectWeapon : Interaction
{
  [SerializeField]
  private TarotCards.Card weaponType;
  [SerializeField]
  private int requiredFollowers;
  [SerializeField]
  private SpriteRenderer lockIcon;
  [SerializeField]
  private InventoryItemDisplay itemDisplay;
  [SerializeField]
  private GameObject unlockableParticle;
  [SerializeField]
  private BlendModeEffect blendMode;
  [SerializeField]
  private int index;
  [SerializeField]
  private WeaponIcons[] weaponIcons;
  private WeaponPickUpUI weaponPickupUI;
  private Canvas canvas;
  private string sLabel;
  private bool locked;
  private bool withinDistance;
  private const float equipDuration = 1f;
  private const float holdDuration = 3f;
  private float holdTimer;
  private Interaction_SelectWeapon.WeaponState weaponState;
  private Player _RewiredController;
  private EventInstance LoopInstance;
  private bool createdLoop;
  private bool fadingOut;
  private float alphaOffset = 1f;

  public static event Interaction_SelectWeapon.WeaponSelectEvent OnWeaponSelect;

  [HideInInspector]
  public Player RewiredController
  {
    get
    {
      if (this._RewiredController == null)
        this._RewiredController = RewiredInputManager.MainPlayer;
      return this._RewiredController;
    }
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.StartCoroutine((IEnumerator) this.DelayedSet());
  }

  private IEnumerator DelayedSet()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_SelectWeapon interactionSelectWeapon = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionSelectWeapon.blendMode.enabled = false;
      interactionSelectWeapon.ActivateDistance = 2f;
      interactionSelectWeapon.UpdateLocalisation();
      interactionSelectWeapon.canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
      interactionSelectWeapon.unlockableParticle.SetActive(false);
      interactionSelectWeapon.SetWeapon(interactionSelectWeapon.weaponType);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void SetWeapon(TarotCards.Card card)
  {
    foreach (WeaponIcons weaponIcon in this.weaponIcons)
    {
      if (weaponIcon.Weapon == card)
      {
        this.itemDisplay.SetImage(weaponIcon.Sprite);
        break;
      }
    }
    this.weaponType = card;
    DataManager.Instance.WeaponSelectionPositions[this.index] = this.weaponType;
  }

  protected override void Update()
  {
    base.Update();
    this.itemDisplay.transform.localPosition = new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time) * 0.1f);
    this.lockIcon.gameObject.SetActive(this.locked);
    this.itemDisplay.spriteRenderer.color = this.locked ? Color.black : Color.white;
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    float a = Mathf.Lerp(0.0f, 1f, (float) (1.0 - ((double) Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position) - 5.0) / 10.0));
    this.itemDisplay.spriteRenderer.color = new Color(this.itemDisplay.spriteRenderer.color.r, this.itemDisplay.spriteRenderer.color.g, this.itemDisplay.spriteRenderer.color.b, a * this.alphaOffset);
    this.lockIcon.color = new Color(this.lockIcon.color.r, this.lockIcon.color.g, this.lockIcon.color.b, a);
    if ((bool) (Object) this.weaponPickupUI)
      this.weaponPickupUI.Shake(this.HoldProgress, this.HoldProgress);
    if ((double) this.HoldProgress > 0.0)
    {
      if (this.createdLoop)
        return;
      this.LoopInstance = AudioManager.Instance.CreateLoop("event:/player/weapon_unlock_loop", this.gameObject, true);
      int num = (int) this.LoopInstance.setParameterByName("parameter:/unlock", this.HoldProgress);
      this.createdLoop = true;
    }
    else
    {
      AudioManager.Instance.StopLoop(this.LoopInstance);
      this.createdLoop = false;
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.LoopInstance);
  }

  public override void IndicateHighlighted()
  {
    base.IndicateHighlighted();
    this.weaponPickupUI = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Weapon Pickup"), GameObject.FindWithTag("Canvas").transform).GetComponent<WeaponPickUpUI>();
    MonoSingleton<Indicator>.Instance.RectTransform.parent = this.weaponPickupUI.transform;
    MonoSingleton<Indicator>.Instance.UpdatePosition = false;
    MonoSingleton<Indicator>.Instance.RectTransform.anchoredPosition = (Vector2) new Vector3(0.0f, -50f, 0.0f);
  }

  public override void EndIndicateHighlighted()
  {
    if (!(bool) (Object) this.weaponPickupUI)
      return;
    MonoSingleton<Indicator>.Instance.gameObject.SetActive(false);
    MonoSingleton<Indicator>.Instance.RectTransform.parent = this.canvas.transform;
    MonoSingleton<Indicator>.Instance.RectTransform.anchoredPosition = MonoSingleton<Indicator>.Instance.CachedPosition;
    MonoSingleton<Indicator>.Instance.UpdatePosition = true;
    base.EndIndicateHighlighted();
    this.weaponPickupUI.canvasGroup.DOFade(0.0f, 0.5f);
    Object.Destroy((Object) this.weaponPickupUI.gameObject, 1f);
  }

  private IEnumerator FadeOut()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_SelectWeapon interactionSelectWeapon = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionSelectWeapon.fadingOut = false;
      Object.Destroy((Object) interactionSelectWeapon.weaponPickupUI.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionSelectWeapon.fadingOut = true;
    interactionSelectWeapon.Interactable = false;
    interactionSelectWeapon.weaponPickupUI.canvasGroup.DOFade(0.0f, 0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.holdTimer = 0.0f;
    AudioManager.Instance.StopLoop(this.LoopInstance);
    if (this.locked)
      return;
    TarotCards.Card weaponType = this.weaponType;
    PlayerFarming.Instance.state.GetComponent<PlayerWeapon>();
    this.blendMode.enabled = true;
    this.EndIndicateHighlighted();
    AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", this.gameObject);
    Interaction_SelectWeapon.WeaponSelectEvent onWeaponSelect = Interaction_SelectWeapon.OnWeaponSelect;
    if (onWeaponSelect == null)
      return;
    onWeaponSelect(weaponType);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    switch (this.weaponState)
    {
      case Interaction_SelectWeapon.WeaponState.Locked:
        this.sLabel = "";
        break;
      case Interaction_SelectWeapon.WeaponState.Unlockable:
        this.sLabel = "";
        break;
      case Interaction_SelectWeapon.WeaponState.Unlocked:
        this.sLabel = ScriptLocalization.Interactions.Equip;
        break;
      case Interaction_SelectWeapon.WeaponState.Equipped:
        this.sLabel = ScriptLocalization.Interactions.Equipped;
        break;
    }
    this.Label = this.sLabel;
    this.Interactable = this.weaponState == Interaction_SelectWeapon.WeaponState.Unlockable || this.weaponState == Interaction_SelectWeapon.WeaponState.Unlocked;
    this.HoldToInteract = this.weaponState == Interaction_SelectWeapon.WeaponState.Unlockable;
  }

  public enum WeaponState
  {
    Locked,
    Unlockable,
    Unlocked,
    Equipped,
  }

  public delegate void WeaponSelectEvent(TarotCards.Card weaponType);
}
