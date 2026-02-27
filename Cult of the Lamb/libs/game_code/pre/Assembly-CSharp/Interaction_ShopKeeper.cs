// Decompiled with JetBrains decompiler
// Type: Interaction_ShopKeeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections.Generic;

#nullable disable
public class Interaction_ShopKeeper : Interaction
{
  private bool spoken;

  private void Start()
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
    if (this.spoken)
      return;
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "Hello, yes we are open From 9-5 come back than")
    }, (List<MMTools.Response>) null, (System.Action) null));
    this.spoken = true;
    this.Label = "";
  }

  private void TellMeMore()
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "Okay great buddy, just untie the rope and i'll give you all the bananas you've ever wanted!!")
    }, (List<MMTools.Response>) null, (System.Action) null));
  }

  private void EndConversation()
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "You will regret that...")
    }, (List<MMTools.Response>) null, (System.Action) null));
  }
}
