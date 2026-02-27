// Decompiled with JetBrains decompiler
// Type: Interaction_DigGrave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_DigGrave : Interaction
{
  public GameObject GraveMound;
  public GameObject GraveHole;
  public GameObject ZombiePrefab;
  private bool Activated;
  private string sString;

  private void Start()
  {
    this.UpdateLocalisation();
    this.HoldToInteract = true;
  }

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sString;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Activated = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 4f);
    PlayerFarming.Instance.GoToAndStop(this.transform.position + new Vector3((double) this.transform.position.x < (double) state.transform.position.x ? 0.4f : -0.4f, 0.2f), this.gameObject, GoToCallback: (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 4f);
      PlayerFarming.Instance.TimedAction(3f, (System.Action) (() => this.StartCoroutine((IEnumerator) this.InteractRoutine())), "actions/dig");
    }));
  }

  private IEnumerator InteractRoutine()
  {
    Interaction_DigGrave interactionDigGrave = this;
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    switch (UnityEngine.Random.Range(0, 6))
    {
      case 0:
      case 1:
        switch (UnityEngine.Random.Range(0, 7))
        {
          case 0:
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
            break;
          case 1:
          case 2:
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.HALF_BLUE_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
            break;
          case 3:
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RED_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
            break;
          case 4:
          case 5:
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.HALF_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
            break;
        }
        break;
      case 2:
      case 3:
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, UnityEngine.Random.Range(3, 6), interactionDigGrave.GraveHole.transform.position);
        break;
      case 4:
      case 5:
      case 6:
        FormationFighter component = UnityEngine.Object.Instantiate<GameObject>(interactionDigGrave.ZombiePrefab, interactionDigGrave.transform.position, Quaternion.identity, interactionDigGrave.transform.parent).GetComponent<FormationFighter>();
        component.GraveSpawn();
        component.CombatTask.CannotLoseTarget = true;
        break;
    }
    interactionDigGrave.GraveMound.SetActive(false);
    interactionDigGrave.GraveHole.SetActive(true);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
  }
}
