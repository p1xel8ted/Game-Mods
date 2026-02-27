// Decompiled with JetBrains decompiler
// Type: Interaction_RandomiseMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private const int cost = 15;
  private bool firstInteraction = true;

  public override void GetLabel()
  {
    string str = "";
    if (this.Interactable)
    {
      if (this.firstInteraction)
        str = ScriptLocalization.Interactions.Talk;
      else if (Inventory.GetItemQuantity(20) < 15)
        str = $"{ScriptLocalization.Interactions.CantAfford} <color=red>{(object) 15} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.BLACK_GOLD)}";
      else
        str = $"{ScriptLocalization.Interactions.Give} {(object) 15} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.BLACK_GOLD)}";
    }
    this.Label = str;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    if (this.firstInteraction)
    {
      this.StartCoroutine((IEnumerator) this.FirstChatIE());
    }
    else
    {
      if (Inventory.GetItemQuantity(20) < 15)
        return;
      this.StartCoroutine((IEnumerator) this.InteractIE());
    }
  }

  private IEnumerator FirstChatIE()
  {
    Interaction_RandomiseMap interactionRandomiseMap = this;
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(interactionRandomiseMap.gameObject, "Conversation_NPC/RandomiseMapNPC/Line1"));
    Entries[0].CharacterName = "NAMES/Ratau";
    Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, interactionRandomiseMap.transform.position);
    PlayerFarming.Instance.state.LookAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, interactionRandomiseMap.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    interactionRandomiseMap.firstInteraction = false;
    interactionRandomiseMap.Interactable = true;
    interactionRandomiseMap.GetLabel();
    interactionRandomiseMap.HasChanged = true;
  }

  private IEnumerator InteractIE()
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
