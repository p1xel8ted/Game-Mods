// Decompiled with JetBrains decompiler
// Type: Interaction_SelectWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using FMOD.Studio;
using I2.Loc;
using Rewired;
using System.Collections;
using Unify.Input;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class Interaction_SelectWeapon : Interaction
{
  [SerializeField]
  public TarotCards.Card weaponType;
  [SerializeField]
  public int requiredFollowers;
  [SerializeField]
  public SpriteRenderer lockIcon;
  [SerializeField]
  public InventoryItemDisplay itemDisplay;
  [SerializeField]
  public GameObject unlockableParticle;
  [SerializeField]
  public BlendModeEffect blendMode;
  [SerializeField]
  public int index;
  [SerializeField]
  public WeaponIcons[] weaponIcons;
  public Canvas canvas;
  public string sLabel;
  public bool locked;
  public bool withinDistance;
  public const float equipDuration = 1f;
  public const float holdDuration = 3f;
  public float holdTimer;
  public Interaction_SelectWeapon.WeaponState weaponState;
  public Player _RewiredController;
  public EventInstance LoopInstance;
  public bool createdLoop;
  public bool fadingOut;
  public float alphaOffset = 1f;

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

  public override void OnEnable()
  {
    base.OnEnable();
    this.StartCoroutine((IEnumerator) this.DelayedSet());
  }

  public IEnumerator DelayedSet()
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

  public void SetWeapon(TarotCards.Card card)
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

  public override void Update()
  {
    base.Update();
    this.itemDisplay.transform.localPosition = new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time) * 0.1f);
    this.lockIcon.gameObject.SetActive(this.locked);
    this.itemDisplay.spriteRenderer.color = this.locked ? Color.black : Color.white;
    float num1 = 0.0f;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((Object) player != (Object) null)
      {
        float a = Mathf.Lerp(0.0f, 1f, (float) (1.0 - ((double) Vector3.Distance(player.transform.position, this.transform.position) - 5.0) / 10.0));
        if ((double) a > (double) num1)
          num1 = a;
        else
          a = 0.0f;
        this.itemDisplay.spriteRenderer.color = new Color(this.itemDisplay.spriteRenderer.color.r, this.itemDisplay.spriteRenderer.color.g, this.itemDisplay.spriteRenderer.color.b, a * this.alphaOffset);
        this.lockIcon.color = new Color(this.lockIcon.color.r, this.lockIcon.color.g, this.lockIcon.color.b, a);
      }
    }
    if ((double) this.HoldProgress > 0.0)
    {
      if (this.createdLoop)
        return;
      this.LoopInstance = AudioManager.Instance.CreateLoop("event:/player/weapon_unlock_loop", this.gameObject, true);
      int num2 = (int) this.LoopInstance.setParameterByName("parameter:/unlock", this.HoldProgress);
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

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
  }

  public IEnumerator FadeOut()
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
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionSelectWeapon.fadingOut = true;
    interactionSelectWeapon.Interactable = false;
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
    this.playerFarming.state.GetComponent<PlayerWeapon>();
    this.blendMode.enabled = true;
    this.EndIndicateHighlighted(this.playerFarming);
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
