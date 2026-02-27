// Decompiled with JetBrains decompiler
// Type: Interaction_PermanentHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_PermanentHeart : Interaction
{
  public System.Action OnFinishPickUp;
  public Vector3 BookTargetPosition;
  public float Timer;
  public SpriteRenderer Shadow;
  public ParticleSystem Particles;
  public SpriteRenderer Image;

  public void Start()
  {
    this.UpdateLocalisation();
    this.Particles.Stop();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.Label = ScriptLocalization.Inventory.RED_HEART;
  }

  public IEnumerator PlayerPickUpBook()
  {
    Interaction_PermanentHeart interactionPermanentHeart = this;
    interactionPermanentHeart.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPermanentHeart.state.gameObject, 5f);
    PlayerSimpleInventory component = interactionPermanentHeart.state.gameObject.GetComponent<PlayerSimpleInventory>();
    interactionPermanentHeart.BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    interactionPermanentHeart.BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    interactionPermanentHeart.Shadow.enabled = false;
    interactionPermanentHeart.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionPermanentHeart.Particles.Play();
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir", interactionPermanentHeart.transform.position);
    while ((double) (interactionPermanentHeart.Timer += Time.deltaTime) < 2.5)
    {
      interactionPermanentHeart.transform.position = Vector3.Lerp(interactionPermanentHeart.transform.position, interactionPermanentHeart.BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionPermanentHeart.transform.position = interactionPermanentHeart.BookTargetPosition;
    interactionPermanentHeart.Particles.Stop();
    interactionPermanentHeart.Image.enabled = false;
    interactionPermanentHeart.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    System.Action onFinishPickUp = interactionPermanentHeart.OnFinishPickUp;
    if (onFinishPickUp != null)
      onFinishPickUp();
    yield return (object) new WaitForSeconds(0.5f);
    interactionPermanentHeart.IncreaseHP();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionPermanentHeart.gameObject);
  }

  public void IncreaseHP()
  {
    foreach (Component player in PlayerFarming.players)
    {
      HealthPlayer component = player.GetComponent<HealthPlayer>();
      ++component.totalHP;
      component.HP = component.totalHP;
    }
    ++DataManager.Instance.RedHeartShrineLevel;
    ++DataManager.Instance.PLAYER_HEALTH_MODIFIED;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    CameraManager.shakeCamera(1f, (float) UnityEngine.Random.Range(0, 360));
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
    BiomeConstants.Instance.EmitBloodImpact(this.transform.position, 0.0f, "red", "BloodImpact_Large_0");
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.transform.position);
    this.StartCoroutine(this.PlayerPickUpBook());
    this.Interactable = false;
  }
}
