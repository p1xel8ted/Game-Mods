// Decompiled with JetBrains decompiler
// Type: BeholderEye
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class BeholderEye : Interaction
{
  public ParticleSystem Particles;
  public SpriteRenderer Image;
  public static BeholderEye Instance;
  private float Delay = 0.5f;
  private string LabelName;
  private Vector3 BookTargetPosition;
  private float Timer;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.LabelName = ScriptLocalization.Inventory.BEHOLDER_EYE;
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
  }

  protected override void Update()
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
    this.StartCoroutine((IEnumerator) this.PlayerPickUpBook());
  }

  private IEnumerator PlayerPickUpBook()
  {
    BeholderEye beholderEye = this;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", beholderEye.transform.position);
    beholderEye.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: true);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
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
    Inventory.AddItem(101, 1);
    beholderEye.Particles.Stop();
    yield return (object) new WaitForSeconds(0.5f);
    beholderEye.Image.enabled = false;
    beholderEye.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    Object.Destroy((Object) beholderEye.gameObject);
  }
}
