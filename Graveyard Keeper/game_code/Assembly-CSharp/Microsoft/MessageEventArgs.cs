// Decompiled with JetBrains decompiler
// Type: Microsoft.MessageEventArgs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Microsoft;

public class MessageEventArgs : EventArgs
{
  public string Message;

  public MessageEventArgs(string message) => this.Message = message;
}
