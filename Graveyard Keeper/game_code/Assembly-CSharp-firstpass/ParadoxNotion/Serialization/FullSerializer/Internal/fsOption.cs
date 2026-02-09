// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsOption
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public static class fsOption
{
  public static fsOption<T> Just<T>(T value) => new fsOption<T>(value);
}
