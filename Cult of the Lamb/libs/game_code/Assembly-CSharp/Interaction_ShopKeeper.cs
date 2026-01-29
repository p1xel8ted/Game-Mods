// Decompiled with JetBrains decompiler
// Type: Interaction_ShopKeeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections.Generic;

#nullable disable
public class Interaction_ShopKeeper : Interaction
{
  public bool spoken;

  public void Start()
  {
    this.UpdateLocalisation();
    this.ActivateDistance = 2f;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.Label = ScriptLocalization.Interactions.Talk;
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (this.spoken)
      return;
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "Hello, yes we are open From 9-5 come back than")
    }, (List<MMTools.Response>) null, (System.Action) null));
    this.spoken = true;
    this.Label = "";
  }

  public void TellMeMore()
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "Okay great buddy, just untie the rope and i'll give you all the bananas you've ever wanted!!")
    }, (List<MMTools.Response>) null, (System.Action) null));
  }

  public void EndConversation()
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "You will regret that...")
    }, (List<MMTools.Response>) null, (System.Action) null));
  }
}
