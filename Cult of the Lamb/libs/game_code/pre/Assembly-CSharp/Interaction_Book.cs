// Decompiled with JetBrains decompiler
// Type: Interaction_Book
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Book : Interaction
{
  public ParticleSystem Particles;
  public SpriteRenderer Shadow;
  public GameObject BuildMenuTutorial;
  public GameObject BookImage;
  private Vector3 BookTargetPosition;
  private float Timer;

  private void Start()
  {
    this.Particles.Stop();
    if (!DataManager.Instance.BuildingTome)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Label = "";
    this.StartCoroutine((IEnumerator) this.PlayerPickUpBook());
  }

  private IEnumerator PlayerPickUpBook()
  {
    Interaction_Book interactionBook = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBook.state.gameObject, 5f);
    PlayerSimpleInventory component = interactionBook.state.gameObject.GetComponent<PlayerSimpleInventory>();
    interactionBook.BookTargetPosition = component.ItemImage.transform.position;
    interactionBook.Shadow.enabled = false;
    interactionBook.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionBook.Particles.Play();
    while ((double) (interactionBook.Timer += Time.deltaTime) < 2.5)
    {
      interactionBook.transform.position = Vector3.Lerp(interactionBook.transform.position, interactionBook.BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionBook.Particles.Stop();
    interactionBook.End();
  }

  private void End() => this.StartCoroutine((IEnumerator) this.EndCoroutine());

  private IEnumerator EndCoroutine()
  {
    Interaction_Book interactionBook = this;
    yield return (object) new WaitForSeconds(0.5f);
    interactionBook.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    interactionBook.BookImage.SetActive(false);
    yield return (object) new WaitForSeconds(0.5f);
    DataManager.Instance.BuildingTome = true;
    GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Canvas");
    Object.Instantiate<GameObject>(interactionBook.BuildMenuTutorial, gameObjectWithTag.transform).GetComponent<global::BuildMenuTutorial>().Target = interactionBook.state.transform;
    Object.Destroy((Object) interactionBook.gameObject);
  }
}
