// Decompiled with JetBrains decompiler
// Type: FlipPolygonCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FlipPolygonCollider : MonoBehaviour
{
  public PolygonCollider2D p;
  public IslandPiece i;
  public List<Vector2> NewPoints;

  public void Play()
  {
    this.i = this.GetComponent<IslandPiece>();
    foreach (IslandConnector connector in this.i.Connectors)
    {
      connector.transform.position = new Vector3(connector.transform.position.x * -1f, connector.transform.position.y, connector.transform.position.z);
      if (connector.MyDirection == IslandConnector.Direction.East)
        connector.MyDirection = IslandConnector.Direction.West;
      else if (connector.MyDirection == IslandConnector.Direction.West)
        connector.MyDirection = IslandConnector.Direction.East;
    }
    this.p = this.i.Collider;
    this.NewPoints = new List<Vector2>();
    foreach (Vector2 vector2 in this.p.GetPath(0))
      this.NewPoints.Add(new Vector2(vector2.x * -1f, vector2.y));
    this.p.SetPath(0, (List<Vector2>) this.NewPoints);
  }
}
