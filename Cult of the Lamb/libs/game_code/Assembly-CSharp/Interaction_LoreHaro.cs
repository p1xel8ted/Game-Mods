// Decompiled with JetBrains decompiler
// Type: Interaction_LoreHaro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_LoreHaro : Interaction
{
  public string convoText = "Conversation_NPC/Haro/Conversation_{0}/Line{1}";
  public string convoFinalText = "Conversation_NPC/Haro/Conversation_Final/Line{0}";
  public List<ConversationEntry> entries = new List<ConversationEntry>();
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

  public IEnumerator InteractIE()
  {
    Interaction_LoreHaro interactionLoreHaro = this;
    interactionLoreHaro.playerFarming.GoToAndStop(interactionLoreHaro.transform.position + Vector3.left * 2f);
    while (interactionLoreHaro.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionLoreHaro.playerFarming.state.facingAngle = Utils.GetAngle(interactionLoreHaro.playerFarming.transform.position, interactionLoreHaro.transform.position);
    interactionLoreHaro.playerFarming.state.LookAngle = Utils.GetAngle(interactionLoreHaro.playerFarming.transform.position, interactionLoreHaro.transform.position);
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
