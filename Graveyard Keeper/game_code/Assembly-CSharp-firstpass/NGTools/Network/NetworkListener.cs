// Decompiled with JetBrains decompiler
// Type: NGTools.Network.NetworkListener
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace NGTools.Network;

public abstract class NetworkListener : MonoBehaviour
{
  public int port;
  public BaseServer server;

  public void SetServer(BaseServer server) => this.server = server;

  public abstract void StartServer();

  public abstract void StopServer();
}
