// Decompiled with JetBrains decompiler
// Type: Interaction_DivineCrystal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_DivineCrystal : Interaction
{
  public SpriteRenderer Image;
  [SerializeField]
  public ParticleSystem ps_PickUp;
  public static Interaction_DivineCrystal Instance;
  public float Delay = 0.5f;
  public string LabelName;
  public Vector3 BookTargetPosition;
  public float Timer;
  public EventInstance loopedSound;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.LabelName = $"{ScriptLocalization.Interactions.PickUp} {ScriptLocalization.Inventory.GOD_TEAR}";
  }

  public override void GetLabel()
  {
    if ((double) this.Delay < 0.0)
      this.Label = this.LabelName;
    else
      this.Label = "";
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_DivineCrystal.Instance = this;
    this.StartCoroutine((IEnumerator) this.DelayDoTween());
  }

  public IEnumerator DelayDoTween()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_DivineCrystal interactionDivineCrystal = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionDivineCrystal.gameObject.GetComponent<PickUp>().enabled = false;
      interactionDivineCrystal.Image.gameObject.transform.DOLocalMoveZ(-0.33f, 1.5f).SetLoops<TweenerCore<Vector3, Vector3, VectorOptions>>(-1, DG.Tweening.LoopType.Yoyo).SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void Update()
  {
    base.Update();
    this.Delay -= Time.deltaTime;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (!((Object) Interaction_DivineCrystal.Instance == (Object) this))
      return;
    Interaction_DivineCrystal.Instance = (Interaction_DivineCrystal) null;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.gameObject.GetComponent<PickUp>().enabled = false;
    this.Image.gameObject.transform.DOKill();
    this.ps_PickUp.Play();
    this.StartCoroutine((IEnumerator) this.PlayerPickUpBook());
    if (DungeonSandboxManager.Active)
      return;
    this.IncrementGodTears();
  }

  public void IncrementGodTears()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        ++DataManager.Instance.Dungeon1GodTears;
        break;
      case FollowerLocation.Dungeon1_2:
        ++DataManager.Instance.Dungeon2GodTears;
        break;
      case FollowerLocation.Dungeon1_3:
        ++DataManager.Instance.Dungeon3GodTears;
        break;
      case FollowerLocation.Dungeon1_4:
        ++DataManager.Instance.Dungeon4GodTears;
        break;
    }
  }

  public IEnumerator PlayerPickUpBook()
  {
    Interaction_DivineCrystal interactionDivineCrystal = this;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionDivineCrystal.transform.position);
    interactionDivineCrystal.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDivineCrystal.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = interactionDivineCrystal.state.gameObject.GetComponent<PlayerSimpleInventory>();
    interactionDivineCrystal.BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    interactionDivineCrystal.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionDivineCrystal.Image.transform.DOShakeScale(2.5f, 0.2f);
    while ((double) (interactionDivineCrystal.Timer += Time.deltaTime) < 2.0)
    {
      interactionDivineCrystal.transform.position = Vector3.Lerp(interactionDivineCrystal.transform.position, interactionDivineCrystal.BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionDivineCrystal.transform.position = interactionDivineCrystal.BookTargetPosition;
    Inventory.AddItem(119, 1);
    yield return (object) new WaitForSeconds(0.5f);
    interactionDivineCrystal.Image.enabled = false;
    interactionDivineCrystal.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    Object.Destroy((Object) interactionDivineCrystal.gameObject);
  }
}
