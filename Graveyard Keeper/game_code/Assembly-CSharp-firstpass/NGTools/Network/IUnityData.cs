// Decompiled with JetBrains decompiler
// Type: NGTools.Network.IUnityData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace NGTools.Network;

public interface IUnityData
{
  Client Client { get; }

  string[] Layers { get; }

  void GetResources(Type type, out string[] resourceNames, out int[] resourceInstanceIds);

  string GetGameObjectName(int instanceID);

  string GetBehaviourName(int gameObjectInstanceID, int instanceID);

  string GetResourceName(Type type, int instanceID);

  void AddPacket(Packet packet);
}
