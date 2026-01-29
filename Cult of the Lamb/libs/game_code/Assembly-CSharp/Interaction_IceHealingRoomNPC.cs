// Decompiled with JetBrains decompiler
// Type: Interaction_IceHealingRoomNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_IceHealingRoomNPC : Interaction
{
  [SerializeField]
  [TermsPopup("")]
  public string deliverLetterCharacterName;
  [Header("Conversations")]
  [SerializeField]
  public Interaction_SimpleConversation letter0Conversation;
  [SerializeField]
  public Interaction_SimpleConversation beforeBringingRodConversation;
  [SerializeField]
  public Vector3 listenPosition;
  [SerializeField]
  public GameObject npc;
  [Header("Barks")]
  [SerializeField]
  public SimpleBarkRepeating normalBarks;
  [Space]
  [SerializeField]
  public SkeletonAnimation charybdisSpine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "scyllaSpine")]
  public string sadIdleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "scyllaSpine")]
  public string happyIdleAnimation;

  public bool canGiveLetter
  {
    get
    {
      return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS) == 0 && !DataManager.Instance.DeliveredCharybisLetter && DataManager.Instance.RepairedLegendaryHammer && !this.letter0Conversation.Spoken;
    }
  }

  public bool canShowBeforeBringingRod
  {
    get
    {
      return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FISHING_ROD) == 0 && DataManager.Instance.DeliveredCharybisLetter && !this.beforeBringingRodConversation.Spoken;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.canGiveLetter)
      this.letter0Conversation.gameObject.SetActive(true);
    this.letter0Conversation.Callback.AddListener(new UnityAction(this.GiveLetter));
    this.beforeBringingRodConversation.Callback.AddListener(new UnityAction(this.BeforeShowRodCallback));
    if (DataManager.Instance.BroughtFishingRod)
      this.charybdisSpine.AnimationState.SetAnimation(0, this.happyIdleAnimation, true);
    else
      this.charybdisSpine.AnimationState.SetAnimation(0, this.sadIdleAnimation, true);
    this.CheckBarks();
  }

  public void BeforeShowRodCallback() => this.CheckBarks();

  public override void OnDisable()
  {
    base.OnDisable();
    this.letter0Conversation.Callback.RemoveListener(new UnityAction(this.GiveLetter));
    this.beforeBringingRodConversation.Callback.RemoveListener(new UnityAction(this.BeforeShowRodCallback));
  }

  public override void GetLabel()
  {
    if (this.canShowBeforeBringingRod)
    {
      this.Label = ScriptLocalization.Interactions.Talk;
      this.AutomaticallyInteract = true;
    }
    else
    {
      this.Label = "";
      this.AutomaticallyInteract = false;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!this.canShowBeforeBringingRod)
      return;
    this.beforeBringingRodConversation.gameObject.SetActive(true);
    this.beforeBringingRodConversation.Play(this.playerFarming.gameObject);
  }

  public void GiveLetter()
  {
    ResourceCustomTarget.Create(this.playerFarming.gameObject, this.npc.transform.position, InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS, (System.Action) (() =>
    {
      ObjectiveManager.Add((ObjectivesData) new Objectives_GiveItem("Objectives/GroupTitles/LegendaryDagger", this.deliverLetterCharacterName, 1, InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS, location: FollowerLocation.Dungeon1_5), true, true);
      Inventory.AddItem(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS, 1);
      GameManager.GetInstance().OnConversationEnd();
      this.CheckBarks();
    }));
  }

  public void CheckBarks()
  {
    Debug.Log((object) $"Checking Normal Bark Condition: canGiveLetter:{this.canGiveLetter}, canShowBeforeBringRod: {this.canShowBeforeBringingRod}");
    this.normalBarks?.gameObject.SetActive(!this.canGiveLetter && !this.canShowBeforeBringingRod);
  }

  [CompilerGenerated]
  public void \u003CGiveLetter\u003Eb__18_0()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_GiveItem("Objectives/GroupTitles/LegendaryDagger", this.deliverLetterCharacterName, 1, InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS, location: FollowerLocation.Dungeon1_5), true, true);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS, 1);
    GameManager.GetInstance().OnConversationEnd();
    this.CheckBarks();
  }
}
