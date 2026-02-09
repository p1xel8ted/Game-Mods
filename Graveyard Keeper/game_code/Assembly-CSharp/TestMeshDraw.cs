// Decompiled with JetBrains decompiler
// Type: TestMeshDraw
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TestMeshDraw : MonoBehaviour
{
  public void OnDrawGizmos()
  {
    Mesh mesh = new Mesh()
    {
      vertices = new Vector3[3]
      {
        new Vector3(1f, 1f, 0.0f),
        new Vector3(-1f, 1f, 0.0f),
        new Vector3(0.0f, -1f, 0.0f)
      },
      triangles = new int[3]{ 0, 1, 2 }
    };
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    Gizmos.DrawWireMesh(mesh, this.transform.position, Quaternion.identity, this.transform.lossyScale);
  }
}
