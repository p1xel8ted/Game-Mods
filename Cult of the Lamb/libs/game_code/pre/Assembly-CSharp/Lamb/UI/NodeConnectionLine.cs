// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NodeConnectionLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class NodeConnectionLine : BaseMonoBehaviour
{
  [SerializeField]
  private MMUILineRenderer _nodeLines;
  [SerializeField]
  [SerializeReference]
  private List<ConnectionListener> _connections = new List<ConnectionListener>();
  [SerializeField]
  private UpgradeTreeConfiguration _configuration;
  [SerializeField]
  private List<UpgradeTreeNode> _nodes = new List<UpgradeTreeNode>();
  private List<ConnectionListener> _dirtyListeners = new List<ConnectionListener>();

  public List<UpgradeTreeNode> Nodes => this._nodes;

  public bool IsDirty => this._dirtyListeners.Count > 0;

  private void Awake()
  {
    List<ConnectionListener> connectionListenerList = new List<ConnectionListener>((IEnumerable<ConnectionListener>) this._connections);
    connectionListenerList.Reverse();
    foreach (ConnectionListener connectionListener in connectionListenerList)
    {
      connectionListener.Configure(this._nodeLines.Root);
      connectionListener.OnStateChanged += new Action<ConnectionListener>(this.OnConnectionStateChanged);
    }
  }

  private void OnConnectionStateChanged(ConnectionListener connectionListener)
  {
    this._dirtyListeners.Add(connectionListener);
  }

  public void PerformLineAnimation() => this.StartCoroutine((IEnumerator) this.DoLineAnimation());

  private IEnumerator DoLineAnimation()
  {
    int num = int.MaxValue;
    foreach (ConnectionListener dirtyListener in this._dirtyListeners)
    {
      if (dirtyListener.Depth < num)
        num = dirtyListener.Depth;
    }
    foreach (ConnectionListener dirtyListener in this._dirtyListeners)
    {
      if (dirtyListener.Depth == num)
        dirtyListener.PerformFillAnimation();
    }
    bool completed = false;
    while (!completed)
    {
      completed = true;
      yield return (object) null;
      foreach (ConnectionListener dirtyListener in this._dirtyListeners)
      {
        if (dirtyListener.IsDirty)
        {
          completed = false;
          break;
        }
      }
    }
    yield return (object) null;
    this._dirtyListeners.Clear();
  }
}
