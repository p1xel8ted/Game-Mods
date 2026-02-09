// Decompiled with JetBrains decompiler
// Type: Pathfinding.ObjImporter
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class ObjImporter
{
  public static Mesh ImportFile(string filePath)
  {
    if (!File.Exists(filePath))
    {
      Debug.LogError((object) $"No file was found at '{filePath}'");
      return (Mesh) null;
    }
    ObjImporter.meshStruct meshStruct = ObjImporter.createMeshStruct(filePath);
    ObjImporter.populateMeshStruct(ref meshStruct);
    Vector3[] vector3Array1 = new Vector3[meshStruct.faceData.Length];
    Vector2[] vector2Array = new Vector2[meshStruct.faceData.Length];
    Vector3[] vector3Array2 = new Vector3[meshStruct.faceData.Length];
    int index = 0;
    foreach (Vector3 vector3 in meshStruct.faceData)
    {
      vector3Array1[index] = meshStruct.vertices[(int) vector3.x - 1];
      if ((double) vector3.y >= 1.0)
        vector2Array[index] = meshStruct.uv[(int) vector3.y - 1];
      if ((double) vector3.z >= 1.0)
        vector3Array2[index] = meshStruct.normals[(int) vector3.z - 1];
      ++index;
    }
    Mesh mesh = new Mesh();
    mesh.vertices = vector3Array1;
    mesh.uv = vector2Array;
    mesh.normals = vector3Array2;
    mesh.triangles = meshStruct.triangles;
    mesh.RecalculateBounds();
    return mesh;
  }

  public static ObjImporter.meshStruct createMeshStruct(string filename)
  {
    int length1 = 0;
    int length2 = 0;
    int length3 = 0;
    int length4 = 0;
    int length5 = 0;
    ObjImporter.meshStruct meshStruct = new ObjImporter.meshStruct();
    meshStruct.fileName = filename;
    StreamReader streamReader = File.OpenText(filename);
    string end = streamReader.ReadToEnd();
    streamReader.Dispose();
    using (StringReader stringReader = new StringReader(end))
    {
      string str = stringReader.ReadLine();
      char[] separator = new char[1]{ ' ' };
      while (str != null)
      {
        if (!str.StartsWith("f ") && !str.StartsWith("v ") && !str.StartsWith("vt ") && !str.StartsWith("vn "))
        {
          str = stringReader.ReadLine();
          if (str != null)
            str = str.Replace("  ", " ");
        }
        else
        {
          string[] strArray = str.Trim().Split(separator, 50);
          switch (strArray[0])
          {
            case "v":
              ++length2;
              break;
            case "vt":
              ++length3;
              break;
            case "vn":
              ++length4;
              break;
            case "f":
              length5 = length5 + strArray.Length - 1;
              length1 += 3 * (strArray.Length - 2);
              break;
          }
          str = stringReader.ReadLine();
          if (str != null)
            str = str.Replace("  ", " ");
        }
      }
    }
    meshStruct.triangles = new int[length1];
    meshStruct.vertices = new Vector3[length2];
    meshStruct.uv = new Vector2[length3];
    meshStruct.normals = new Vector3[length4];
    meshStruct.faceData = new Vector3[length5];
    return meshStruct;
  }

  public static void populateMeshStruct(ref ObjImporter.meshStruct mesh)
  {
    StreamReader streamReader = File.OpenText(mesh.fileName);
    string end = streamReader.ReadToEnd();
    streamReader.Close();
    using (StringReader stringReader = new StringReader(end))
    {
      string str = stringReader.ReadLine();
      char[] separator1 = new char[1]{ ' ' };
      char[] separator2 = new char[1]{ '/' };
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      int index4 = 0;
      int index5 = 0;
      int index6 = 0;
      int index7 = 0;
      while (str != null)
      {
        if (!str.StartsWith("f ") && !str.StartsWith("v ") && !str.StartsWith("vt ") && !str.StartsWith("vn ") && !str.StartsWith("g ") && !str.StartsWith("usemtl ") && !str.StartsWith("mtllib ") && !str.StartsWith("vt1 ") && !str.StartsWith("vt2 ") && !str.StartsWith("vc ") && !str.StartsWith("usemap "))
        {
          str = stringReader.ReadLine();
          if (str != null)
            str = str.Replace("  ", " ");
        }
        else
        {
          string[] strArray1 = str.Trim().Split(separator1, 50);
          switch (strArray1[0])
          {
            case "f":
              int index8 = 1;
              List<int> intList = new List<int>();
              while (index8 < strArray1.Length && (strArray1[index8] ?? "").Length > 0)
              {
                Vector3 vector3 = new Vector3();
                string[] strArray2 = strArray1[index8].Split(separator2, 3);
                vector3.x = (float) Convert.ToInt32(strArray2[0]);
                if (strArray2.Length > 1)
                {
                  if (strArray2[1] != "")
                    vector3.y = (float) Convert.ToInt32(strArray2[1]);
                  vector3.z = (float) Convert.ToInt32(strArray2[2]);
                }
                ++index8;
                mesh.faceData[index2] = vector3;
                intList.Add(index2);
                ++index2;
              }
              for (int index9 = 1; index9 + 2 < strArray1.Length; ++index9)
              {
                mesh.triangles[index1] = intList[0];
                int index10 = index1 + 1;
                mesh.triangles[index10] = intList[index9];
                int index11 = index10 + 1;
                mesh.triangles[index11] = intList[index9 + 1];
                index1 = index11 + 1;
              }
              break;
            case "v":
              mesh.vertices[index3] = new Vector3(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]), Convert.ToSingle(strArray1[3]));
              ++index3;
              break;
            case "vn":
              mesh.normals[index4] = new Vector3(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]), Convert.ToSingle(strArray1[3]));
              ++index4;
              break;
            case "vt":
              mesh.uv[index5] = new Vector2(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]));
              ++index5;
              break;
            case "vt1":
              mesh.uv[index6] = new Vector2(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]));
              ++index6;
              break;
            case "vt2":
              mesh.uv[index7] = new Vector2(Convert.ToSingle(strArray1[1]), Convert.ToSingle(strArray1[2]));
              ++index7;
              break;
          }
          str = stringReader.ReadLine();
          if (str != null)
            str = str.Replace("  ", " ");
        }
      }
    }
  }

  public struct meshStruct
  {
    public Vector3[] vertices;
    public Vector3[] normals;
    public Vector2[] uv;
    public Vector2[] uv1;
    public Vector2[] uv2;
    public int[] triangles;
    public int[] faceVerts;
    public int[] faceUVs;
    public Vector3[] faceData;
    public string name;
    public string fileName;
  }
}
