// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.GraphSerializationData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[Serializable]
public class GraphSerializationData
{
  public const float FRAMEWORK_VERSION = 2.7f;
  public float version;
  public System.Type type;
  public string name = string.Empty;
  public string category = string.Empty;
  public string comments = string.Empty;
  public Vector2 translation = new Vector2(-5000f, -5000f);
  public float zoomFactor = 1f;
  public List<NodeCanvas.Framework.Node> nodes = new List<NodeCanvas.Framework.Node>();
  public List<Connection> connections = new List<Connection>();
  public NodeCanvas.Framework.Node primeNode;
  public List<NodeCanvas.Framework.CanvasGroup> canvasGroups;
  public BlackboardSource localBlackboard;
  public object derivedData;

  public GraphSerializationData()
  {
  }

  public GraphSerializationData(Graph graph)
  {
    this.version = 2.7f;
    this.type = graph.GetType();
    this.category = graph.category;
    this.comments = graph.graphComments;
    this.translation = graph.translation;
    this.zoomFactor = graph.zoomFactor;
    this.nodes = graph.allNodes;
    this.canvasGroups = graph.canvasGroups;
    this.localBlackboard = graph.localBlackboard;
    List<Connection> connectionList = new List<Connection>();
    for (int index1 = 0; index1 < this.nodes.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.nodes[index1].outConnections.Count; ++index2)
        connectionList.Add(this.nodes[index1].outConnections[index2]);
    }
    this.connections = connectionList;
    this.primeNode = graph.primeNode;
    this.derivedData = graph.OnDerivedDataSerialization();
  }

  public void Reconstruct(Graph graph)
  {
    for (int index = 0; index < this.connections.Count; ++index)
    {
      this.connections[index].sourceNode.outConnections.Add(this.connections[index]);
      this.connections[index].targetNode.inConnections.Add(this.connections[index]);
    }
    for (int index = 0; index < this.nodes.Count; ++index)
    {
      this.nodes[index].graph = graph;
      this.nodes[index].ID = index + 1;
    }
    graph.OnDerivedDataDeserialization(this.derivedData);
  }
}
