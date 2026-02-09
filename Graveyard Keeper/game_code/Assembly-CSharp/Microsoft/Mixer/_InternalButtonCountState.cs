// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer._InternalButtonCountState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Microsoft.Mixer;

public struct _InternalButtonCountState
{
  public uint PreviousCountOfButtonDownEvents;
  public uint PreviousCountOfButtonPressEvents;
  public uint PreviousCountOfButtonUpEvents;
  public uint CountOfButtonDownEvents;
  public uint CountOfButtonPressEvents;
  public uint CountOfButtonUpEvents;
  public uint NextCountOfButtonDownEvents;
  public uint NextCountOfButtonPressEvents;
  public uint NextCountOfButtonUpEvents;
  public string PreviousTransactionID;
  public string TransactionID;
  public string NextTransactionID;
}
