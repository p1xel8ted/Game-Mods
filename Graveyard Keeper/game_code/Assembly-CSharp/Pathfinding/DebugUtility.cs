// Decompiled with JetBrains decompiler
// Type: Pathfinding.DebugUtility
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_debug_utility.php")]
public class DebugUtility : MonoBehaviour
{
  public Material defaultMaterial;
  public static DebugUtility active;
  public float offset = 0.2f;
  public bool optimizeMeshes;

  public void Awake() => DebugUtility.active = this;

  public static void DrawCubes(
    Vector3[] topVerts,
    Vector3[] bottomVerts,
    Color[] vertexColors,
    float width)
  {
    if ((UnityEngine.Object) DebugUtility.active == (UnityEngine.Object) null)
      DebugUtility.active = UnityEngine.Object.FindObjectOfType(typeof (DebugUtility)) as DebugUtility;
    if ((UnityEngine.Object) DebugUtility.active == (UnityEngine.Object) null)
      throw new NullReferenceException();
    if (topVerts.Length != bottomVerts.Length || topVerts.Length != vertexColors.Length)
    {
      Debug.LogError((object) "Array Lengths are not the same");
    }
    else
    {
      if (topVerts.Length > 2708)
      {
        Vector3[] topVerts1 = new Vector3[topVerts.Length - 2708];
        Vector3[] bottomVerts1 = new Vector3[topVerts.Length - 2708];
        Color[] vertexColors1 = new Color[topVerts.Length - 2708];
        for (int index = 2708; index < topVerts.Length; ++index)
        {
          topVerts1[index - 2708] = topVerts[index];
          bottomVerts1[index - 2708] = bottomVerts[index];
          vertexColors1[index - 2708] = vertexColors[index];
        }
        Vector3[] vector3Array1 = new Vector3[2708];
        Vector3[] vector3Array2 = new Vector3[2708];
        Color[] colorArray = new Color[2708];
        for (int index = 0; index < 2708; ++index)
        {
          vector3Array1[index] = topVerts[index];
          vector3Array2[index] = bottomVerts[index];
          colorArray[index] = vertexColors[index];
        }
        DebugUtility.DrawCubes(topVerts1, bottomVerts1, vertexColors1, width);
        topVerts = vector3Array1;
        bottomVerts = vector3Array2;
        vertexColors = colorArray;
      }
      width /= 2f;
      Vector3[] vector3Array = new Vector3[topVerts.Length * 4 * 6];
      int[] numArray = new int[topVerts.Length * 6 * 6];
      Color[] colorArray1 = new Color[topVerts.Length * 4 * 6];
      for (int index1 = 0; index1 < topVerts.Length; ++index1)
      {
        Vector3 vector3_1 = topVerts[index1] + new Vector3(0.0f, DebugUtility.active.offset, 0.0f);
        Vector3 vector3_2 = bottomVerts[index1] - new Vector3(0.0f, DebugUtility.active.offset, 0.0f);
        Vector3 vector3_3 = vector3_1 + new Vector3(-width, 0.0f, -width);
        Vector3 vector3_4 = vector3_1 + new Vector3(width, 0.0f, -width);
        Vector3 vector3_5 = vector3_1 + new Vector3(width, 0.0f, width);
        Vector3 vector3_6 = vector3_1 + new Vector3(-width, 0.0f, width);
        Vector3 vector3_7 = vector3_2 + new Vector3(-width, 0.0f, -width);
        Vector3 vector3_8 = vector3_2 + new Vector3(width, 0.0f, -width);
        Vector3 vector3_9 = vector3_2 + new Vector3(width, 0.0f, width);
        Vector3 vector3_10 = vector3_2 + new Vector3(-width, 0.0f, width);
        int index2 = index1 * 4 * 6;
        int num1 = index1 * 6 * 6;
        Color vertexColor = vertexColors[index1];
        for (int index3 = index2; index3 < index2 + 24; ++index3)
          colorArray1[index3] = vertexColor;
        vector3Array[index2] = vector3_3;
        vector3Array[index2 + 1] = vector3_6;
        vector3Array[index2 + 2] = vector3_5;
        vector3Array[index2 + 3] = vector3_4;
        int index4 = index2 + 4;
        vector3Array[index4 + 3] = vector3_7;
        vector3Array[index4 + 2] = vector3_10;
        vector3Array[index4 + 1] = vector3_9;
        vector3Array[index4] = vector3_8;
        int index5 = index4 + 4;
        vector3Array[index5] = vector3_8;
        vector3Array[index5 + 1] = vector3_4;
        vector3Array[index5 + 2] = vector3_5;
        vector3Array[index5 + 3] = vector3_9;
        int index6 = index5 + 4;
        vector3Array[index6 + 3] = vector3_7;
        vector3Array[index6 + 2] = vector3_3;
        vector3Array[index6 + 1] = vector3_6;
        vector3Array[index6] = vector3_10;
        int index7 = index6 + 4;
        vector3Array[index7 + 3] = vector3_9;
        vector3Array[index7 + 2] = vector3_10;
        vector3Array[index7 + 1] = vector3_6;
        vector3Array[index7] = vector3_5;
        int index8 = index7 + 4;
        vector3Array[index8] = vector3_8;
        vector3Array[index8 + 1] = vector3_7;
        vector3Array[index8 + 2] = vector3_3;
        vector3Array[index8 + 3] = vector3_4;
        for (int index9 = 0; index9 < 6; ++index9)
        {
          int num2 = index8 + index9 * 4;
          int index10 = num1 + index9 * 6;
          numArray[index10] = num2;
          numArray[index10 + 1] = num2 + 1;
          numArray[index10 + 2] = num2 + 2;
          numArray[index10 + 3] = num2;
          numArray[index10 + 4] = num2 + 2;
          numArray[index10 + 5] = num2 + 3;
        }
      }
      Mesh mesh = new Mesh();
      mesh.vertices = vector3Array;
      mesh.triangles = numArray;
      mesh.colors = colorArray1;
      mesh.name = "VoxelMesh";
      mesh.RecalculateNormals();
      mesh.RecalculateBounds();
      int num = DebugUtility.active.optimizeMeshes ? 1 : 0;
      GameObject gameObject = new GameObject("DebugMesh");
      (gameObject.AddComponent(typeof (MeshRenderer)) as MeshRenderer).material = DebugUtility.active.defaultMaterial;
      (gameObject.AddComponent(typeof (MeshFilter)) as MeshFilter).mesh = mesh;
    }
  }

  public static void DrawQuads(Vector3[] verts, float width)
  {
    if (verts.Length >= 16250)
    {
      Vector3[] verts1 = new Vector3[verts.Length - 16250];
      for (int index = 16250; index < verts.Length; ++index)
        verts1[index - 16250] = verts[index];
      Vector3[] vector3Array = new Vector3[16250];
      for (int index = 0; index < 16250; ++index)
        vector3Array[index] = verts[index];
      DebugUtility.DrawQuads(verts1, width);
      verts = vector3Array;
    }
    width /= 2f;
    Vector3[] vector3Array1 = new Vector3[verts.Length * 4];
    int[] numArray = new int[verts.Length * 6];
    for (int index1 = 0; index1 < verts.Length; ++index1)
    {
      Vector3 vert = verts[index1];
      int index2 = index1 * 4;
      vector3Array1[index2] = vert + new Vector3(-width, 0.0f, -width);
      vector3Array1[index2 + 1] = vert + new Vector3(-width, 0.0f, width);
      vector3Array1[index2 + 2] = vert + new Vector3(width, 0.0f, width);
      vector3Array1[index2 + 3] = vert + new Vector3(width, 0.0f, -width);
      int index3 = index1 * 6;
      numArray[index3] = index2;
      numArray[index3 + 1] = index2 + 1;
      numArray[index3 + 2] = index2 + 2;
      numArray[index3 + 3] = index2;
      numArray[index3 + 4] = index2 + 2;
      numArray[index3 + 5] = index2 + 3;
    }
    Mesh mesh = new Mesh();
    mesh.vertices = vector3Array1;
    mesh.triangles = numArray;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    GameObject gameObject = new GameObject("DebugMesh");
    (gameObject.AddComponent(typeof (MeshRenderer)) as MeshRenderer).material = DebugUtility.active.defaultMaterial;
    (gameObject.AddComponent(typeof (MeshFilter)) as MeshFilter).mesh = mesh;
  }
}
