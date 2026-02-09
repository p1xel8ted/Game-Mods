// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.Quads
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

public class Quads
{
  public static Mesh[] meshes;
  public static int currentQuads;

  public static bool HasMeshes()
  {
    if (Quads.meshes == null)
      return false;
    foreach (Object mesh in Quads.meshes)
    {
      if ((Object) null == mesh)
        return false;
    }
    return true;
  }

  public static void Cleanup()
  {
    if (Quads.meshes == null)
      return;
    for (int index = 0; index < Quads.meshes.Length; ++index)
    {
      if ((Object) null != (Object) Quads.meshes[index])
      {
        Object.DestroyImmediate((Object) Quads.meshes[index]);
        Quads.meshes[index] = (Mesh) null;
      }
    }
    Quads.meshes = (Mesh[]) null;
  }

  public static Mesh[] GetMeshes(int totalWidth, int totalHeight)
  {
    if (Quads.HasMeshes() && Quads.currentQuads == totalWidth * totalHeight)
      return Quads.meshes;
    int max = 10833;
    int num = totalWidth * totalHeight;
    Quads.currentQuads = num;
    Quads.meshes = new Mesh[Mathf.CeilToInt((float) (1.0 * (double) num / (1.0 * (double) max)))];
    int index = 0;
    for (int triOffset = 0; triOffset < num; triOffset += max)
    {
      int triCount = Mathf.FloorToInt((float) Mathf.Clamp(num - triOffset, 0, max));
      Quads.meshes[index] = Quads.GetMesh(triCount, triOffset, totalWidth, totalHeight);
      ++index;
    }
    return Quads.meshes;
  }

  public static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
  {
    Mesh mesh = new Mesh();
    mesh.hideFlags = HideFlags.DontSave;
    Vector3[] vector3Array = new Vector3[triCount * 4];
    Vector2[] vector2Array1 = new Vector2[triCount * 4];
    Vector2[] vector2Array2 = new Vector2[triCount * 4];
    int[] numArray = new int[triCount * 6];
    for (int index1 = 0; index1 < triCount; ++index1)
    {
      int index2 = index1 * 4;
      int index3 = index1 * 6;
      int num = triOffset + index1;
      float x = Mathf.Floor((float) (num % totalWidth)) / (float) totalWidth;
      float y = Mathf.Floor((float) (num / totalWidth)) / (float) totalHeight;
      Vector3 vector3 = new Vector3((float) ((double) x * 2.0 - 1.0), (float) ((double) y * 2.0 - 1.0), 1f);
      vector3Array[index2] = vector3;
      vector3Array[index2 + 1] = vector3;
      vector3Array[index2 + 2] = vector3;
      vector3Array[index2 + 3] = vector3;
      vector2Array1[index2] = new Vector2(0.0f, 0.0f);
      vector2Array1[index2 + 1] = new Vector2(1f, 0.0f);
      vector2Array1[index2 + 2] = new Vector2(0.0f, 1f);
      vector2Array1[index2 + 3] = new Vector2(1f, 1f);
      vector2Array2[index2] = new Vector2(x, y);
      vector2Array2[index2 + 1] = new Vector2(x, y);
      vector2Array2[index2 + 2] = new Vector2(x, y);
      vector2Array2[index2 + 3] = new Vector2(x, y);
      numArray[index3] = index2;
      numArray[index3 + 1] = index2 + 1;
      numArray[index3 + 2] = index2 + 2;
      numArray[index3 + 3] = index2 + 1;
      numArray[index3 + 4] = index2 + 2;
      numArray[index3 + 5] = index2 + 3;
    }
    mesh.vertices = vector3Array;
    mesh.triangles = numArray;
    mesh.uv = vector2Array1;
    mesh.uv2 = vector2Array2;
    return mesh;
  }
}
