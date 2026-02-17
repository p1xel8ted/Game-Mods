// Decompiled with JetBrains decompiler
// Type: Interaction_MonsterHeartPickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_MonsterHeartPickUp : Interaction
{
  public string LabelName;
  public float Delay = 0.5f;
  public SpriteRenderer Shadow;
  public SpriteRenderer Image;
  public Vector3 BookTargetPosition;
  public float Timer;

  public void Start()
  {
    this.UpdateLocalisation();
    this.StartCoroutine((IEnumerator) this.HeartBeart());
  }

  public IEnumerator HeartBeart()
  {
    Interaction_MonsterHeartPickUp monsterHeartPickUp = this;
    while (true)
    {
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/monster_heart/monster_heart_beat", monsterHeartPickUp.gameObject);
      monsterHeartPickUp.gameObject.transform.DOPunchScale(new Vector3(0.3f, -0.3f), 0.5f);
      yield return (object) null;
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.StopCoroutine((IEnumerator) this.HeartBeart());
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.LabelName = ScriptLocalization.Interactions.MonsterHeart;
  }

  public override void Update()
  {
    this.Delay -= Time.deltaTime;
    base.Update();
  }

  public override void GetLabel()
  {
    if ((double) this.Delay < 0.0)
      this.Label = this.LabelName;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.gameObject.GetComponent<PickUp>().enabled = false;
    this.StartCoroutine((IEnumerator) this.PlayerPickUpBook());
  }

  public IEnumerator PlayerPickUpBook()
  {
    Interaction_MonsterHeartPickUp monsterHeartPickUp = this;
    AudioManager.Instance.PlayOneShot("event:/monster_heart/monster_heart_sequence", monsterHeartPickUp.gameObject);
    monsterHeartPickUp.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(monsterHeartPickUp.state.gameObject, 5f);
    PlayerSimpleInventory component = monsterHeartPickUp.state.gameObject.GetComponent<PlayerSimpleInventory>();
    monsterHeartPickUp.BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    monsterHeartPickUp.Shadow.enabled = false;
    monsterHeartPickUp.state.CURRENT_STATE = StateMachine.State.FoundItem;
    while ((double) (monsterHeartPickUp.Timer += Time.deltaTime) < 0.5)
    {
      monsterHeartPickUp.transform.position = Vector3.Lerp(monsterHeartPickUp.transform.position, monsterHeartPickUp.BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    monsterHeartPickUp.transform.position = monsterHeartPickUp.BookTargetPosition;
    yield return (object) new WaitForSeconds(1f);
    monsterHeartPickUp.Image.enabled = false;
    monsterHeartPickUp.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    Inventory.AddItem(22, 1);
    Object.Destroy((Object) monsterHeartPickUp.gameObject);
  }
}
