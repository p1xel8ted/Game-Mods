// Decompiled with JetBrains decompiler
// Type: Map.Node
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Map;

[Serializable]
public class Node
{
  public Point point;
  public List<Point> incoming = new List<Point>();
  public List<Point> outgoing = new List<Point>();
  [JsonConverter(typeof (StringEnumConverter))]
  public NodeType nodeType;
  public NodeBlueprint blueprint;
  public Vector2 position;
  public DungeonModifier Modifier;
  public bool Hidden;
  public bool CanBeHidden = true;
  public FollowerLocation DungeonLocation = FollowerLocation.None;

  public Node(
    NodeType nodeType,
    NodeBlueprint blueprint,
    Point point,
    FollowerLocation location = FollowerLocation.None,
    DungeonModifier modifier = null)
  {
    this.nodeType = nodeType;
    this.blueprint = blueprint;
    this.point = point;
    this.Modifier = modifier;
    this.DungeonLocation = location;
    if (blueprint.ForcedDungeon != FollowerLocation.None)
      this.DungeonLocation = blueprint.ForcedDungeon;
    if (!blueprint.CanBeHidden || !this.CanBeHidden || (double) UnityEngine.Random.Range(0.0f, 1f) >= 0.10000000149011612 || !DataManager.Instance.MinimumRandomRoomsEncountered)
      return;
    this.Hidden = true;
  }

  public void AddIncoming(Point p)
  {
    if (this.incoming.Any<Point>((Func<Point, bool>) (element => element.Equals(p))))
      return;
    this.incoming.Add(p);
  }

  public void AddOutgoing(Point p)
  {
    if (this.outgoing.Any<Point>((Func<Point, bool>) (element => element.Equals(p))))
      return;
    this.outgoing.Add(p);
  }

  public void RemoveIncoming(Point p)
  {
    this.incoming.RemoveAll((Predicate<Point>) (element => element.Equals(p)));
  }

  public void RemoveOutgoing(Point p)
  {
    this.outgoing.RemoveAll((Predicate<Point>) (element => element.Equals(p)));
  }

  public bool HasNoConnections() => this.incoming.Count == 0 && this.outgoing.Count == 0;
}
