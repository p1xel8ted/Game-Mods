// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveButtonEventArgs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

public class InteractiveButtonEventArgs : InteractiveEventArgs
{
  [CompilerGenerated]
  public string \u003CControlID\u003Ek__BackingField;
  [CompilerGenerated]
  public InteractiveParticipant \u003CParticipant\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsPressed\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CTransactionID\u003Ek__BackingField;
  [CompilerGenerated]
  public uint \u003CCost\u003Ek__BackingField;

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

  public bool IsPressed
  {
    get => this.\u003CIsPressed\u003Ek__BackingField;
    set => this.\u003CIsPressed\u003Ek__BackingField = value;
  }

  public string TransactionID
  {
    get => this.\u003CTransactionID\u003Ek__BackingField;
    set => this.\u003CTransactionID\u003Ek__BackingField = value;
  }

  public uint Cost
  {
    get => this.\u003CCost\u003Ek__BackingField;
    set => this.\u003CCost\u003Ek__BackingField = value;
  }

  public void CaptureTransaction()
  {
    InteractivityManager.SingletonInstance.CaptureTransaction(this.TransactionID);
  }

  public InteractiveButtonEventArgs(
    InteractiveEventType type,
    string id,
    InteractiveParticipant participant,
    bool isPressed,
    uint cost,
    string transactionID)
    : base(type)
  {
    this.ControlID = id;
    this.Participant = participant;
    this.Cost = cost;
    this.IsPressed = isPressed;
    this.TransactionID = transactionID;
  }
}
