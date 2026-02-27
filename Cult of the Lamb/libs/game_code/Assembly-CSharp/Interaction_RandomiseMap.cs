// Decompiled with JetBrains decompiler
// Type: Interaction_RandomiseMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Map;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_RandomiseMap : Interaction
{
  public const int cost = 15;
  public bool firstInteraction = true;

  public override void GetLabel()
  {
    string str = "";
    if (this.Interactable)
    {
      if (this.firstInteraction)
        str = ScriptLocalization.Interactions.Talk;
      else if (Inventory.GetItemQuantity(20) < 15)
        str = $"{ScriptLocalization.Interactions.CantAfford} <color=red>{15.ToString()} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.BLACK_GOLD)}";
      else
        str = $"{ScriptLocalization.Interactions.Give} {15.ToString()} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.BLACK_GOLD)}";
    }
    this.Label = str;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    if (this.firstInteraction)
    {
      this.StartCoroutine(this.FirstChatIE());
    }
    else
    {
      if (Inventory.GetItemQuantity(20) < 15)
        return;
      this.StartCoroutine(this.InteractIE());
    }
  }

  public IEnumerator FirstChatIE()
  {
    Interaction_RandomiseMap interactionRandomiseMap = this;
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(interactionRandomiseMap.gameObject, "Conversation_NPC/RandomiseMapNPC/Line1"));
    Entries[0].CharacterName = "NAMES/Ratau";
    Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
    interactionRandomiseMap.playerFarming.state.facingAngle = Utils.GetAngle(interactionRandomiseMap.playerFarming.transform.position, interactionRandomiseMap.transform.position);
    interactionRandomiseMap.playerFarming.state.LookAngle = Utils.GetAngle(interactionRandomiseMap.playerFarming.transform.position, interactionRandomiseMap.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    interactionRandomiseMap.firstInteraction = false;
    interactionRandomiseMap.Interactable = true;
    interactionRandomiseMap.GetLabel();
    interactionRandomiseMap.HasChanged = true;
  }

  public IEnumerator InteractIE()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.RandomiseNextNodes();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }
}
