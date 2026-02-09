// Decompiled with JetBrains decompiler
// Type: GyzmoGrid
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GyzmoGrid : MonoBehaviour
{
  public bool enabled_grid = true;
  public float gridScale = 1f;
  public int minX = -20;
  public int minY = -35;
  public int maxX = 15;
  public int maxY = 15;
  public Vector3 gridOffset = Vector3.zero;
  public bool topDownGrid = true;
  public int gizmoMajorLines = 5;
  public Color gizmoLineColor = new Color(0.4f, 0.4f, 0.3f, 1f);
  public Color build_grid_color = new Color(0.4f, 0.4f, 0.3f, 0.3f);

  public void OnDrawGizmos()
  {
    if (!this.enabled_grid)
      return;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Color color1 = new Color(this.gizmoLineColor.r, this.gizmoLineColor.g, this.gizmoLineColor.b, 0.25f * this.gizmoLineColor.a);
    Color color2 = Color.Lerp(Color.white, this.gizmoLineColor, 0.75f);
    for (int minX = this.minX; minX < this.maxX + 1; ++minX)
    {
      Gizmos.color = minX % this.gizmoMajorLines == 0 ? this.gizmoLineColor : color1;
      if (minX == 0)
        Gizmos.color = color2;
      for (int index = 0; index < 3; ++index)
      {
        double x = (double) minX + (double) index * 0.3333333432674408;
        if (index != 0)
          Gizmos.color = this.build_grid_color;
        Vector3 vector3_1 = new Vector3((float) x, (float) this.minY, 0.0f) * this.gridScale;
        Vector3 vector3_2 = new Vector3((float) x, (float) this.maxY, 0.0f) * this.gridScale;
        if (this.topDownGrid)
        {
          vector3_1 = new Vector3(vector3_1.x, 0.0f, vector3_1.y);
          vector3_2 = new Vector3(vector3_2.x, 0.0f, vector3_2.y);
        }
        Gizmos.DrawLine(this.gridOffset + vector3_1, this.gridOffset + vector3_2);
      }
    }
    for (int minY = this.minY; minY < this.maxY + 1; ++minY)
    {
      Gizmos.color = minY % this.gizmoMajorLines == 0 ? this.gizmoLineColor : color1;
      if (minY == 0)
        Gizmos.color = color2;
      for (int index = 0; index < 3; ++index)
      {
        float y = (float) minY + (float) index * 0.333333343f;
        if (index != 0)
          Gizmos.color = this.build_grid_color;
        Vector3 vector3_3 = new Vector3((float) this.minX, y, 0.0f) * this.gridScale;
        Vector3 vector3_4 = new Vector3((float) this.maxX, y, 0.0f) * this.gridScale;
        if (this.topDownGrid)
        {
          vector3_3 = new Vector3(vector3_3.x, 0.0f, vector3_3.y);
          vector3_4 = new Vector3(vector3_4.x, 0.0f, vector3_4.y);
        }
        Gizmos.DrawLine(this.gridOffset + vector3_3, this.gridOffset + vector3_4);
      }
    }
  }
}
