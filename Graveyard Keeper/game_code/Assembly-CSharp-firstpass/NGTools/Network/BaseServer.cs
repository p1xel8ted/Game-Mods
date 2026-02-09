// Decompiled with JetBrains decompiler
// Type: NGTools.Network.BaseServer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NGTools.Network;

public abstract class BaseServer : MonoBehaviour
{
  public static List<BaseServer> instances = new List<BaseServer>();
  [Header("Starts the server when awaking.")]
  public bool autoStart = true;
  [Header("Keep the server alive between scenes.")]
  public bool dontDestroyOnLoad = true;
  [Header("[Required] A listener to communicate via network.")]
  public NetworkListener listener;
  [CompilerGenerated]
  public PacketExecuter \u003Cexecuter\u003Ek__BackingField;

  public PacketExecuter executer
  {
    get => this.\u003Cexecuter\u003Ek__BackingField;
    set => this.\u003Cexecuter\u003Ek__BackingField = value;
  }

  public virtual void Awake()
  {
    if (this.executer != null)
      return;
    for (int index = 0; index < BaseServer.instances.Count; ++index)
    {
      if (System.Type.op_Equality(BaseServer.instances[index].GetType(), this.GetType()))
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        return;
      }
    }
    this.executer = this.CreatePacketExecuter();
    if (!this.dontDestroyOnLoad)
      return;
    BaseServer.instances.Add(this);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.transform.root.gameObject);
  }

  public virtual void Start()
  {
    if ((UnityEngine.Object) this.listener == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "A NetworkListener is required.", (UnityEngine.Object) this);
    }
    else
    {
      this.listener.SetServer(this);
      if (!this.autoStart)
        return;
      this.StartServer();
    }
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this.listener != (UnityEngine.Object) null))
      return;
    this.listener.StopServer();
    this.listener = (NetworkListener) null;
  }

  public void StartServer() => this.listener.StartServer();

  public void StopServer() => this.OnDestroy();

  public abstract PacketExecuter CreatePacketExecuter();
}
