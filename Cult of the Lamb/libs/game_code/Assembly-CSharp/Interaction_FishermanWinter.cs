// Decompiled with JetBrains decompiler
// Type: Interaction_FishermanWinter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_FishermanWinter : Interaction
{
  public const int WOOL_TO_RECEIVE = 5;
  [SerializeField]
  [TermsPopup("")]
  public string characterName;
  [Header("Legednary Dagger")]
  [SerializeField]
  public Interaction_SimpleConversation bringWoolConversation;
  [SerializeField]
  public Interaction_SimpleConversation receivedWoolConversation;
  public List<InventoryItem.ITEM_TYPE> itemsToBring = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.WOOL
  };
  public string sLabel;

  public bool canGiveItem
  {
    get
    {
      return DataManager.Instance.DeliveredCharybisLetter && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FISHING_ROD) == 0 && !DataManager.Instance.BroughtFishingRod && ObjectiveManager.GroupExists("Objectives/GroupTitles/LegendaryDagger") && (this.HasPlayerAnyItem() || DataManager.Instance.FishermanGaveWoolAmount >= 5) && DataManager.Instance.BringFishermanWoolStarted;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Interactable = this.canGiveItem;
    if (DataManager.Instance.DeliveredCharybisLetter && !DataManager.Instance.BringFishermanWoolStarted)
      this.bringWoolConversation.gameObject.SetActive(true);
    this.bringWoolConversation.Callback.AddListener(new UnityAction(this.GiveCollectWoolObjective));
    this.receivedWoolConversation.Callback.AddListener(new UnityAction(this.ReceivedWoolCallback));
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.bringWoolConversation.Callback.RemoveListener(new UnityAction(this.GiveCollectWoolObjective));
    this.receivedWoolConversation.Callback.RemoveListener(new UnityAction(this.ReceivedWoolCallback));
  }

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Give;
  }

  public override void GetLabel()
  {
    if (this.canGiveItem)
    {
      if (DataManager.Instance.FishermanGaveWoolAmount >= 5)
      {
        this.Label = ScriptLocalization.Interactions.Talk;
        this.AutomaticallyInteract = true;
        this.ActivateDistance = 3f;
      }
      else
        this.Label = $"{ScriptLocalization.Interactions.Give}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.WOOL, 5)}";
    }
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (DataManager.Instance.FishermanGaveWoolAmount >= 5 && this.canGiveItem)
    {
      this.CheckObjectiveInteractions();
      this.AutomaticallyInteract = false;
    }
    else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) >= 5)
    {
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.gameObject, 7f);
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.WOOL, -5);
      for (int index = 0; index < 5; ++index)
      {
        ++DataManager.Instance.FishermanGaveWoolAmount;
        ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, InventoryItem.ITEM_TYPE.WOOL, (System.Action) null);
      }
      GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => this.CheckObjectiveInteractions()));
    }
    else
      this._playerFarming.indicator.PlayShake();
  }

  public void GiveCollectWoolObjective()
  {
    this.Interactable = true;
    DataManager.Instance.BringFishermanWoolStarted = true;
    this.bringWoolConversation.gameObject.SetActive(false);
  }

  public void CheckObjectiveInteractions()
  {
    if (DataManager.Instance.FishermanGaveWoolAmount < 5)
      return;
    this.receivedWoolConversation.gameObject.SetActive(true);
    this.receivedWoolConversation.Play(this.playerFarming.gameObject);
  }

  public void ReceivedWoolCallback()
  {
    ResourceCustomTarget.Create(this.playerFarming.gameObject, this.transform.position, InventoryItem.ITEM_TYPE.FISHING_ROD, (System.Action) (() => Inventory.AddItem(InventoryItem.ITEM_TYPE.FISHING_ROD, 1)));
  }

  public bool HasPlayerAnyItem()
  {
    foreach (InventoryItem.ITEM_TYPE itemType in this.itemsToBring)
    {
      if (Inventory.GetItemQuantity(itemType) > 0)
        return true;
    }
    return false;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__13_0() => this.CheckObjectiveInteractions();
}
