// Decompiled with JetBrains decompiler
// Type: NGTools.Network.PacketId
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NGTools.Network;

public class PacketId
{
  public const int ServerHasDisconnect = 1;
  public const int ClientHasDisconnect = 2;
  public const int ClientSendPing = 3;
  public const int ServerAnswerPing = 4;
  public const int Server_ErrorNotification = 5;
  public static Dictionary<int, string> packetNames;

  public static string GetPacketName(int id)
  {
    if (PacketId.packetNames == null)
    {
      PacketId.packetNames = new Dictionary<int, string>();
      FieldInfo[] fields = typeof (PacketId).GetFields();
      for (int index = 0; index < fields.Length; ++index)
        PacketId.packetNames.Add((int) fields[index].GetRawConstantValue(), fields[index].Name);
    }
    return PacketId.packetNames[id];
  }
}
