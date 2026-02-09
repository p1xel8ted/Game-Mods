// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveTextEventArgs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

public class InteractiveTextEventArgs : InteractiveEventArgs
{
  [CompilerGenerated]
  public string \u003CControlID\u003Ek__BackingField;
  [CompilerGenerated]
  public InteractiveParticipant \u003CParticipant\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CText\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CTransactionID\u003Ek__BackingField;

  public string ControlID
  {
    get => this.\u003CControlID\u003Ek__BackingField;
    set => this.\u003CControlID\u003Ek__BackingField = value;
  }

  public InteractiveParticipant Participant
  {
    get => this.\u003CParticipant\u003Ek__BackingField;
    set => this.\u003CParticipant\u003Ek__BackingField = value;
  }

  public string Text
  {
    get => this.\u003CText\u003Ek__BackingField;
    set => this.\u003CText\u003Ek__BackingField = value;
  }

  public string TransactionID
  {
    get => this.\u003CTransactionID\u003Ek__BackingField;
    set => this.\u003CTransactionID\u003Ek__BackingField = value;
  }

  public void CaptureTransaction()
  {
    InteractivityManager.SingletonInstance.CaptureTransaction(this.TransactionID);
  }

  public InteractiveTextEventArgs(
    InteractiveEventType type,
    string id,
    InteractiveParticipant participant,
    string text,
    string transactionID)
    : base(type)
  {
    this.ControlID = id;
    this.Participant = participant;
    this.Text = text;
    this.TransactionID = transactionID;
  }
}
