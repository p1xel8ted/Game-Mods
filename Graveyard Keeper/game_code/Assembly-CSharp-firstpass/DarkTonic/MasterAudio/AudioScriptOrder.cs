// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AudioScriptOrder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace DarkTonic.MasterAudio;

public class AudioScriptOrder : Attribute
{
  public int Order;

  public AudioScriptOrder(int order) => this.Order = order;
}
