// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVOSquareObstacle
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding.RVO;

[AddComponentMenu("Pathfinding/Local Avoidance/Square Obstacle")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_square_obstacle.php")]
public class RVOSquareObstacle : RVOObstacle
{
  public float height = 1f;
  public Vector2 size = (Vector2) Vector3.one;
  public Vector2 center = (Vector2) Vector3.one;

  public override bool StaticObstacle => false;

  public override bool ExecuteInEditor => true;

  public override bool LocalCoordinates => true;

  public override float Height => this.height;

  public override bool AreGizmosDirty() => false;

  public override void CreateObstacles()
  {
    this.size.x = Mathf.Abs(this.size.x);
    this.size.y = Mathf.Abs(this.size.y);
    this.height = Mathf.Abs(this.height);
    Vector3[] vertices = new Vector3[4]
    {
      new Vector3(1f, 0.0f, -1f),
      new Vector3(1f, 0.0f, 1f),
      new Vector3(-1f, 0.0f, 1f),
      new Vector3(-1f, 0.0f, -1f)
    };
    for (int index = 0; index < vertices.Length; ++index)
    {
      vertices[index].Scale(new Vector3(this.size.x * 0.5f, 0.0f, this.size.y * 0.5f));
      vertices[index] += new Vector3(this.center.x, 0.0f, this.center.y);
    }
    this.AddObstacle(vertices, this.height);
  }
}
