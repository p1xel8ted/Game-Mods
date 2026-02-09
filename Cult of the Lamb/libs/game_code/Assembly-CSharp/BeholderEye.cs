// Decompiled with JetBrains decompiler
// Type: BeholderEye
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class BeholderEye : Interaction
{
  public InventoryItem.ITEM_TYPE type;
  public ParticleSystem Particles;
  public SpriteRenderer Image;
  public static BeholderEye Instance;
  public float Delay = 0.5f;
  public string LabelName;
  public Vector3 BookTargetPosition;
  public float Timer;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.LabelName = this.type == InventoryItem.ITEM_TYPE.BEHOLDER_EYE ? ScriptLocalization.Inventory.BEHOLDER_EYE : ScriptLocalization.Inventory.BEHOLDER_EYE_ROT;
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
    this.Particles.Stop();
    BeholderEye.Instance = this;
    this.StartCoroutine((IEnumerator) this.DelayDoTween());
  }

  public IEnumerator DelayDoTween()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BeholderEye beholderEye = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      beholderEye.gameObject.GetComponent<PickUp>().enabled = false;
      beholderEye.Image.gameObject.transform.DOLocalMoveZ(-0.33f, 1.5f).SetLoops<TweenerCore<Vector3, Vector3, VectorOptions>>(-1, DG.Tweening.LoopType.Yoyo).SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
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
    if (!((Object) BeholderEye.Instance == (Object) this))
      return;
    BeholderEye.Instance = (BeholderEye) null;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.gameObject.GetComponent<PickUp>().enabled = false;
    this.Image.gameObject.transform.DOKill();
    this.StartCoroutine((IEnumerator) this.PlayerPickUpBook());
  }

  public IEnumerator PlayerPickUpBook()
  {
    BeholderEye beholderEye = this;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", beholderEye.transform.position);
    beholderEye.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(beholderEye.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = beholderEye.state.gameObject.GetComponent<PlayerSimpleInventory>();
    beholderEye.BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    beholderEye.state.CURRENT_STATE = StateMachine.State.FoundItem;
    beholderEye.Particles.Play();
    while ((double) (beholderEye.Timer += Time.deltaTime) < 2.0)
    {
      beholderEye.transform.position = Vector3.Lerp(beholderEye.transform.position, beholderEye.BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    beholderEye.transform.position = beholderEye.BookTargetPosition;
    Inventory.AddItem((int) beholderEye.type, 1);
    beholderEye.Particles.Stop();
    yield return (object) new WaitForSeconds(0.5f);
    beholderEye.Image.enabled = false;
    beholderEye.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    Object.Destroy((Object) beholderEye.gameObject);
  }
}
