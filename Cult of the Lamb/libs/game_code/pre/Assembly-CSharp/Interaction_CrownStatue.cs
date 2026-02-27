// Decompiled with JetBrains decompiler
// Type: Interaction_CrownStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_CrownStatue : Interaction
{
  [SerializeField]
  private GameObject cameraOffset;
  private string sLabel;

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
