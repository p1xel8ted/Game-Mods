// Decompiled with JetBrains decompiler
// Type: Interaction_CrownStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_CrownStatue : Interaction
{
  [SerializeField]
  public GameObject cameraOffset;
  public string sLabel;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Read;
  }

  public override void GetLabel()
  {
    if (string.IsNullOrEmpty(this.sLabel))
      this.UpdateLocalisation();
    this.Label = this.sLabel;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.cameraOffset, "Conversation_NPC/DoorRoomStatue/0"),
      new ConversationEntry(this.cameraOffset, "Conversation_NPC/DoorRoomStatue/1")
    }, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
  }
}
