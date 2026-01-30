// Decompiled with JetBrains decompiler
// Type: MeshCutter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MeshCutter : MonoBehaviour
{
  public SkeletonGraphic skeletonGraphic;
  public Plane slicingPlane;
  public Material slicedMaterial;
  public Vector3 sliceRotation;
  public Vector3 sliceScale = Vector3.one;

  public void SliceSkeleton()
  {
    if ((Object) this.skeletonGraphic == (Object) null)
    {
      Debug.LogError((object) "SkeletonGraphic is not assigned.");
    }
    else
    {
      Mesh lastMesh = this.skeletonGraphic.GetLastMesh();
      if ((Object) lastMesh == (Object) null)
      {
        Debug.LogError((object) "SkeletonMesh is null.");
      }
      else
      {
        Vector3[] vertices = lastMesh.vertices;
        int[] triangles = lastMesh.triangles;
        Debug.Log((object) ("Number of triangles in skeleton mesh: " + (triangles.Length / 3).ToString()));
        List<Mesh> slicedMeshes = new List<Mesh>();
        int num = 0;
        for (int index1 = 0; index1 < triangles.Length; index1 += 3)
        {
          int index2 = triangles[index1];
          int index3 = triangles[index1 + 1];
          int index4 = triangles[index1 + 2];
          Vector3 vector3_1 = vertices[index2];
          Vector3 vector3_2 = vertices[index3];
          Vector3 vector3_3 = vertices[index4];
          slicedMeshes.Add(new Mesh()
          {
            vertices = new Vector3[3]
            {
              vector3_1,
              vector3_2,
              vector3_3
            },
            triangles = new int[3]{ 0, 1, 2 }
          });
          ++num;
        }
        Debug.Log((object) ("Number of triangles sliced: " + num.ToString()));
        this.ProcessSlicedMeshes(slicedMeshes);
      }
    }
  }

  public bool IsVertexInFrontOfPlane(Vector3 vertex)
  {
    return (double) this.slicingPlane.GetDistanceToPoint(vertex) > 9.9999997473787516E-05;
  }

  public void ProcessSlicedMeshes(List<Mesh> slicedMeshes)
  {
    foreach (Mesh slicedMesh in slicedMeshes)
    {
      GameObject gameObject = new GameObject("SlicedObject");
      gameObject.AddComponent<MeshFilter>().mesh = slicedMesh;
      gameObject.AddComponent<MeshRenderer>().material = this.slicedMaterial;
      gameObject.AddComponent<SkeletonGraphic>();
      gameObject.transform.position = Vector3.zero;
      gameObject.transform.rotation = Quaternion.Euler(this.sliceRotation);
      gameObject.transform.localScale = this.sliceScale;
    }
  }
}
