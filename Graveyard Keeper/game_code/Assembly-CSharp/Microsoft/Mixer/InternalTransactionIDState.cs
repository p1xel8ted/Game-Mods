// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InternalTransactionIDState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Microsoft.Mixer;

public struct InternalTransactionIDState(string newTransactionID)
{
  public string previousTransactionID = (string) null;
  public string transactionID = (string) null;
  public string nextTransactionID = newTransactionID;
}
