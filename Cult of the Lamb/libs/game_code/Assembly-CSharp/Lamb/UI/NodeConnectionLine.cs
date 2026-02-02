// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NodeConnectionLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public MMUILineRenderer _nodeLines;
  [SerializeField]
  [SerializeReference]
  public List<ConnectionListener> _connections = new List<ConnectionListener>();
  [SerializeField]
  public UpgradeTreeConfiguration _configuration;
  [SerializeField]
  public List<UpgradeTreeNode> _nodes = new List<UpgradeTreeNode>();
  public List<ConnectionListener> _dirtyListeners = new List<ConnectionListener>();

  public List<UpgradeTreeNode> Nodes => this._nodes;

  public MMUILineRenderer Line => this._nodeLines;

  public bool IsDirty => this._dirtyListeners.Count > 0;

  public void Start()
  {
    List<ConnectionListener> connectionListenerList = new List<ConnectionListener>((IEnumerable<ConnectionListener>) this._connections);
    connectionListenerList.Reverse();
    foreach (ConnectionListener connectionListener in connectionListenerList)
    {
      connectionListener.Configure(this._nodeLines.Root);
      connectionListener.OnStateChanged += new Action<ConnectionListener>(this.OnConnectionStateChanged);
    }
  }

  public void OnConnectionStateChanged(ConnectionListener connectionListener)
  {
    this._dirtyListeners.Add(connectionListener);
  }

  public void PerformLineAnimation() => this.StartCoroutine((IEnumerator) this.DoLineAnimation());

  public IEnumerator DoLineAnimation()
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

  public void ClearListeners() => this._dirtyListeners.Clear();
}
