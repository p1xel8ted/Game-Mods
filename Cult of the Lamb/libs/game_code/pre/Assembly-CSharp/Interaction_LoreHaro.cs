// Decompiled with JetBrains decompiler
// Type: Interaction_LoreHaro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_LoreHaro : Interaction
{
  private string convoText = "Conversation_NPC/Haro/Conversation_{0}/Line{1}";
  private string convoFinalText = "Conversation_NPC/Haro/Conversation_Final/Line{0}";
  private List<ConversationEntry> entries = new List<ConversationEntry>();
  public UnityEvent Callback;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 3f;
  }

  public override void GetLabel()
  {
    this.Label = this.Interactable ? ScriptLocalization.Interactions.Talk : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.InteractIE());
  }

  private IEnumerator InteractIE()
  {
    Interaction_LoreHaro interactionLoreHaro = this;
    PlayerFarming.Instance.GoToAndStop(interactionLoreHaro.transform.position + Vector3.left * 2f);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, interactionLoreHaro.transform.position);
    PlayerFarming.Instance.state.LookAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, interactionLoreHaro.transform.position);
    for (int index = 1; LocalizationManager.GetTermData(string.Format(interactionLoreHaro.convoText, (object) DataManager.Instance.HaroConversationIndex, (object) index)) != null; ++index)
    {
      string TermToSpeak = string.Format(interactionLoreHaro.convoText, (object) DataManager.Instance.HaroConversationIndex, (object) index);
      interactionLoreHaro.entries.Add(new ConversationEntry(interactionLoreHaro.gameObject, TermToSpeak)
      {
        CharacterName = "NAMES/Haro",
        soundPath = "event:/dialogue/haro/standard_haro"
      });
    }
    MMConversation.Play(new ConversationObject(interactionLoreHaro.entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    ++DataManager.Instance.HaroConversationIndex;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    interactionLoreHaro.Callback?.Invoke();
  }
}
