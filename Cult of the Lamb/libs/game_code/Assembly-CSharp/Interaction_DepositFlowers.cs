// Decompiled with JetBrains decompiler
// Type: Interaction_DepositFlowers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_DepositFlowers : Interaction
{
  [SerializeField]
  public int cost = 10;
  [SerializeField]
  public InventoryItem.ITEM_TYPE costItem;
  [SerializeField]
  public SimpleBark idleBark;
  [SerializeField]
  public int rewardQuantity = 1;
  [SerializeField]
  public InventoryItem.ITEM_TYPE rewardItem;
  [SerializeField]
  public SimpleBark rewardedBark;

  public void Start()
  {
    if (DataManager.Instance.CompletedInfectedNPCQuest)
      this.Interactable = false;
    else if (DataManager.Instance.TalkedToInfectedNPC)
    {
      this.Interactable = true;
      this.idleBark.gameObject.SetActive(true);
    }
    else
      this.Interactable = false;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (!this.Interactable)
      this.Label = "";
    else
      this.Label = string.Format(ScriptLocalization.UI_ItemSelector_Context.Give, (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.FLOWER_RED, this.cost));
  }

  public void IntroConversationEnd()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.Interactable = true;
    this.HasChanged = true;
    this.idleBark.gameObject.SetActive(true);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(this.costItem) >= this.cost)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.gameObject);
      this.rewardedBark.gameObject.SetActive(true);
      Inventory.ChangeItemQuantity(this.costItem, this.cost);
      GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
      {
        InventoryItem.Spawn(this.rewardItem, this.rewardQuantity, this.transform.position);
        this.Interactable = false;
        this.HasChanged = true;
        GameManager.GetInstance().OnConversationEnd();
        DataManager.Instance.CompletedInfectedNPCQuest = true;
      }));
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__9_0()
  {
    InventoryItem.Spawn(this.rewardItem, this.rewardQuantity, this.transform.position);
    this.Interactable = false;
    this.HasChanged = true;
    GameManager.GetInstance().OnConversationEnd();
    DataManager.Instance.CompletedInfectedNPCQuest = true;
  }
}
