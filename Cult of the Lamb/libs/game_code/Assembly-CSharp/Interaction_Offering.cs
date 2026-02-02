// Decompiled with JetBrains decompiler
// Type: Interaction_Offering
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Offering : Interaction
{
  public GameObject PlayerGoTo;
  public DataManager.Variables VariableOnComplete;
  public string sLabel;
  public bool Activated;
  public EnemyRounds enemyRounds;
  public InventoryItem.ITEM_TYPE Resource;
  public List<InventoryItem.ITEM_TYPE> Resources = new List<InventoryItem.ITEM_TYPE>();
  public InventoryItemDisplay[] inventoryItems;

  public void Start()
  {
    this.HoldToInteract = true;
    this.Resource = !((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null) ? this.Resources[UnityEngine.Random.Range(0, this.Resources.Count)] : this.Resources[BiomeGenerator.Instance.CurrentRoom.RandomSeed.Next(0, this.Resources.Count)];
    this.UpdateLocalisation();
    this.inventoryItems = this.GetComponentsInChildren<InventoryItemDisplay>();
    foreach (InventoryItemDisplay inventoryItem in this.inventoryItems)
      inventoryItem.SetImage(this.Resource);
  }

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sLabel;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Activated = true;
    this.StartCombat();
  }

  public IEnumerator InteractRoutine()
  {
    Interaction_Offering interactionOffering = this;
    int ResourcesToGive = 10;
    int NumResouces = ResourcesToGive;
    foreach (Component inventoryItem in interactionOffering.inventoryItems)
      inventoryItem.gameObject.SetActive(false);
    while (--NumResouces >= 0)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionOffering.transform.position);
      ResourceCustomTarget.Create(interactionOffering.state.gameObject, interactionOffering.transform.position, interactionOffering.Resource, NumResouces == 0 ? new System.Action(interactionOffering.CompleteCollection) : new System.Action(interactionOffering.GiveResource));
      yield return (object) new WaitForSeconds((float) (0.10000000149011612 + 0.20000000298023224 * (double) (NumResouces / ResourcesToGive)));
    }
  }

  public void GiveResource() => Inventory.AddItem((int) this.Resource, 1);

  public void StartCombat() => this.StartCoroutine((IEnumerator) this.StartCombatRoutine());

  public IEnumerator StartCombatRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_Offering interactionOffering = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionOffering.enemyRounds.BeginCombat(false, new System.Action(interactionOffering.CombatComplete));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    BlockingDoor.CloseAll();
    interactionOffering.state.CURRENT_STATE = StateMachine.State.Idle;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.OfferingCombat);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void CombatComplete()
  {
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardAmbience);
    this.StartCoroutine((IEnumerator) this.DelayGoTo());
  }

  public IEnumerator DelayGoTo()
  {
    Interaction_Offering interactionOffering = this;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionOffering.state.gameObject, 8f);
    yield return (object) new WaitForSeconds(0.5f);
    interactionOffering.playerFarming.GoToAndStop(interactionOffering.PlayerGoTo, interactionOffering.gameObject, GoToCallback: new System.Action(interactionOffering.CollectResources));
  }

  public void CollectResources() => this.StartCoroutine((IEnumerator) this.InteractRoutine());

  public void CompleteCollection()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    BlockingDoor.OpenAll();
    DataManager.Instance.SetVariable(this.VariableOnComplete, true);
  }
}
