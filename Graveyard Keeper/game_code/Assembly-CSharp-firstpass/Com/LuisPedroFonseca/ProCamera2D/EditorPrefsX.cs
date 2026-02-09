// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.EditorPrefsX
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public static class EditorPrefsX
{
  public static int endianDiff1;
  public static int endianDiff2;
  public static int idx;
  public static byte[] byteBlock;

  public static bool SetBool(string name, bool value) => true;

  public static bool GetBool(string name) => true;

  public static bool GetBool(string name, bool defaultValue) => true;

  public static long GetLong(string key, long defaultValue) => 0;

  public static long GetLong(string key) => 0;

  public static void SplitLong(long input, out int lowBits, out int highBits)
  {
    lowBits = (int) (uint) input;
    highBits = (int) (uint) (input >> 32 /*0x20*/);
  }

  public static void SetLong(string key, long value)
  {
  }

  public static bool SetVector2(string key, Vector2 vector)
  {
    return EditorPrefsX.SetFloatArray(key, new float[2]
    {
      vector.x,
      vector.y
    });
  }

  public static Vector2 GetVector2(string key)
  {
    float[] floatArray = EditorPrefsX.GetFloatArray(key);
    return floatArray.Length < 2 ? Vector2.zero : new Vector2(floatArray[0], floatArray[1]);
  }

  public static Vector2 GetVector2(string key, Vector2 defaultValue) => Vector2.zero;

  public static bool SetVector3(string key, Vector3 vector)
  {
    return EditorPrefsX.SetFloatArray(key, new float[3]
    {
      vector.x,
      vector.y,
      vector.z
    });
  }

  public static Vector3 GetVector3(string key)
  {
    float[] floatArray = EditorPrefsX.GetFloatArray(key);
    return floatArray.Length < 3 ? Vector3.zero : new Vector3(floatArray[0], floatArray[1], floatArray[2]);
  }

  public static Vector3 GetVector3(string key, Vector3 defaultValue) => Vector3.zero;

  public static bool SetQuaternion(string key, Quaternion vector)
  {
    return EditorPrefsX.SetFloatArray(key, new float[4]
    {
      vector.x,
      vector.y,
      vector.z,
      vector.w
    });
  }

  public static Quaternion GetQuaternion(string key)
  {
    float[] floatArray = EditorPrefsX.GetFloatArray(key);
    return floatArray.Length < 4 ? Quaternion.identity : new Quaternion(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
  }

  public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
  {
    return Quaternion.identity;
  }

  public static bool SetColor(string key, Color color)
  {
    return EditorPrefsX.SetFloatArray(key, new float[4]
    {
      color.r,
      color.g,
      color.b,
      color.a
    });
  }

  public static Color GetColor(string key)
  {
    float[] floatArray = EditorPrefsX.GetFloatArray(key);
    return floatArray.Length < 4 ? new Color(0.0f, 0.0f, 0.0f, 0.0f) : new Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
  }

  public static Color GetColor(string key, Color defaultValue) => Color.white;

  public static bool SetBoolArray(string key, bool[] boolArray)
  {
    byte[] bytes = new byte[(boolArray.Length + 7) / 8 + 5];
    bytes[0] = Convert.ToByte((object) EditorPrefsX.ArrayType.Bool);
    int num = 1;
    int index1 = 5;
    for (int index2 = 0; index2 < boolArray.Length; ++index2)
    {
      if (boolArray[index2])
        bytes[index1] |= (byte) num;
      num <<= 1;
      if (num > 128 /*0x80*/)
      {
        num = 1;
        ++index1;
      }
    }
    EditorPrefsX.Initialize();
    EditorPrefsX.ConvertInt32ToBytes(boolArray.Length, bytes);
    return EditorPrefsX.SaveBytes(key, bytes);
  }

  public static bool[] GetBoolArray(string key)
  {
    if (!PlayerPrefs.HasKey(key))
      return new bool[0];
    byte[] bytes = Convert.FromBase64String(PlayerPrefs.GetString(key));
    if (bytes.Length < 5)
    {
      Debug.LogError((object) ("Corrupt preference file for " + key));
      return new bool[0];
    }
    if (bytes[0] != (byte) 2)
    {
      Debug.LogError((object) (key + " is not a boolean array"));
      return new bool[0];
    }
    EditorPrefsX.Initialize();
    bool[] boolArray = new bool[EditorPrefsX.ConvertBytesToInt32(bytes)];
    int num = 1;
    int index1 = 5;
    for (int index2 = 0; index2 < boolArray.Length; ++index2)
    {
      boolArray[index2] = ((uint) bytes[index1] & (uint) (byte) num) > 0U;
      num <<= 1;
      if (num > 128 /*0x80*/)
      {
        num = 1;
        ++index1;
      }
    }
    return boolArray;
  }

  public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize) => new bool[0];

  public static bool SetStringArray(string key, string[] stringArray) => true;

  public static string[] GetStringArray(string key) => new string[0];

  public static string[] GetStringArray(string key, string defaultValue, int defaultSize)
  {
    return new string[0];
  }

  public static bool SetIntArray(string key, int[] intArray)
  {
    return EditorPrefsX.SetValue<int[]>(key, intArray, EditorPrefsX.ArrayType.Int32, 1, new Action<int[], byte[], int>(EditorPrefsX.ConvertFromInt));
  }

  public static bool SetFloatArray(string key, float[] floatArray)
  {
    return EditorPrefsX.SetValue<float[]>(key, floatArray, EditorPrefsX.ArrayType.Float, 1, new Action<float[], byte[], int>(EditorPrefsX.ConvertFromFloat));
  }

  public static bool SetVector2Array(string key, Vector2[] vector2Array)
  {
    return EditorPrefsX.SetValue<Vector2[]>(key, vector2Array, EditorPrefsX.ArrayType.Vector2, 2, new Action<Vector2[], byte[], int>(EditorPrefsX.ConvertFromVector2));
  }

  public static bool SetVector3Array(string key, Vector3[] vector3Array)
  {
    return EditorPrefsX.SetValue<Vector3[]>(key, vector3Array, EditorPrefsX.ArrayType.Vector3, 3, new Action<Vector3[], byte[], int>(EditorPrefsX.ConvertFromVector3));
  }

  public static bool SetQuaternionArray(string key, Quaternion[] quaternionArray)
  {
    return EditorPrefsX.SetValue<Quaternion[]>(key, quaternionArray, EditorPrefsX.ArrayType.Quaternion, 4, new Action<Quaternion[], byte[], int>(EditorPrefsX.ConvertFromQuaternion));
  }

  public static bool SetColorArray(string key, Color[] colorArray)
  {
    return EditorPrefsX.SetValue<Color[]>(key, colorArray, EditorPrefsX.ArrayType.Color, 4, new Action<Color[], byte[], int>(EditorPrefsX.ConvertFromColor));
  }

  public static bool SetValue<T>(
    string key,
    T array,
    EditorPrefsX.ArrayType arrayType,
    int vectorNumber,
    Action<T, byte[], int> convert)
    where T : IList
  {
    byte[] bytes = new byte[4 * array.Count * vectorNumber + 1];
    bytes[0] = Convert.ToByte((object) arrayType);
    EditorPrefsX.Initialize();
    for (int index = 0; index < array.Count; ++index)
      convert(array, bytes, index);
    return EditorPrefsX.SaveBytes(key, bytes);
  }

  public static void ConvertFromInt(int[] array, byte[] bytes, int i)
  {
    EditorPrefsX.ConvertInt32ToBytes(array[i], bytes);
  }

  public static void ConvertFromFloat(float[] array, byte[] bytes, int i)
  {
    EditorPrefsX.ConvertFloatToBytes(array[i], bytes);
  }

  public static void ConvertFromVector2(Vector2[] array, byte[] bytes, int i)
  {
    EditorPrefsX.ConvertFloatToBytes(array[i].x, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].y, bytes);
  }

  public static void ConvertFromVector3(Vector3[] array, byte[] bytes, int i)
  {
    EditorPrefsX.ConvertFloatToBytes(array[i].x, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].y, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].z, bytes);
  }

  public static void ConvertFromQuaternion(Quaternion[] array, byte[] bytes, int i)
  {
    EditorPrefsX.ConvertFloatToBytes(array[i].x, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].y, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].z, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].w, bytes);
  }

  public static void ConvertFromColor(Color[] array, byte[] bytes, int i)
  {
    EditorPrefsX.ConvertFloatToBytes(array[i].r, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].g, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].b, bytes);
    EditorPrefsX.ConvertFloatToBytes(array[i].a, bytes);
  }

  public static int[] GetIntArray(string key)
  {
    List<int> list = new List<int>();
    EditorPrefsX.GetValue<List<int>>(key, list, EditorPrefsX.ArrayType.Int32, 1, new Action<List<int>, byte[]>(EditorPrefsX.ConvertToInt));
    return list.ToArray();
  }

  public static int[] GetIntArray(string key, int defaultValue, int defaultSize) => new int[0];

  public static float[] GetFloatArray(string key)
  {
    List<float> list = new List<float>();
    EditorPrefsX.GetValue<List<float>>(key, list, EditorPrefsX.ArrayType.Float, 1, new Action<List<float>, byte[]>(EditorPrefsX.ConvertToFloat));
    return list.ToArray();
  }

  public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
  {
    return new float[0];
  }

  public static Vector2[] GetVector2Array(string key)
  {
    List<Vector2> list = new List<Vector2>();
    EditorPrefsX.GetValue<List<Vector2>>(key, list, EditorPrefsX.ArrayType.Vector2, 2, new Action<List<Vector2>, byte[]>(EditorPrefsX.ConvertToVector2));
    return list.ToArray();
  }

  public static Vector2[] GetVector2Array(string key, Vector2 defaultValue, int defaultSize)
  {
    return new Vector2[0];
  }

  public static Vector3[] GetVector3Array(string key)
  {
    List<Vector3> list = new List<Vector3>();
    EditorPrefsX.GetValue<List<Vector3>>(key, list, EditorPrefsX.ArrayType.Vector3, 3, new Action<List<Vector3>, byte[]>(EditorPrefsX.ConvertToVector3));
    return list.ToArray();
  }

  public static Vector3[] GetVector3Array(string key, Vector3 defaultValue, int defaultSize)
  {
    return new Vector3[0];
  }

  public static Quaternion[] GetQuaternionArray(string key)
  {
    List<Quaternion> list = new List<Quaternion>();
    EditorPrefsX.GetValue<List<Quaternion>>(key, list, EditorPrefsX.ArrayType.Quaternion, 4, new Action<List<Quaternion>, byte[]>(EditorPrefsX.ConvertToQuaternion));
    return list.ToArray();
  }

  public static Quaternion[] GetQuaternionArray(
    string key,
    Quaternion defaultValue,
    int defaultSize)
  {
    return new Quaternion[0];
  }

  public static Color[] GetColorArray(string key)
  {
    List<Color> list = new List<Color>();
    EditorPrefsX.GetValue<List<Color>>(key, list, EditorPrefsX.ArrayType.Color, 4, new Action<List<Color>, byte[]>(EditorPrefsX.ConvertToColor));
    return list.ToArray();
  }

  public static Color[] GetColorArray(string key, Color defaultValue, int defaultSize)
  {
    return new Color[0];
  }

  public static void GetValue<T>(
    string key,
    T list,
    EditorPrefsX.ArrayType arrayType,
    int vectorNumber,
    Action<T, byte[]> convert)
    where T : IList
  {
  }

  public static void ConvertToInt(List<int> list, byte[] bytes)
  {
    list.Add(EditorPrefsX.ConvertBytesToInt32(bytes));
  }

  public static void ConvertToFloat(List<float> list, byte[] bytes)
  {
    list.Add(EditorPrefsX.ConvertBytesToFloat(bytes));
  }

  public static void ConvertToVector2(List<Vector2> list, byte[] bytes)
  {
    list.Add(new Vector2(EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes)));
  }

  public static void ConvertToVector3(List<Vector3> list, byte[] bytes)
  {
    list.Add(new Vector3(EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes)));
  }

  public static void ConvertToQuaternion(List<Quaternion> list, byte[] bytes)
  {
    list.Add(new Quaternion(EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes)));
  }

  public static void ConvertToColor(List<Color> list, byte[] bytes)
  {
    list.Add(new Color(EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes), EditorPrefsX.ConvertBytesToFloat(bytes)));
  }

  public static void ShowArrayType(string key)
  {
  }

  public static void Initialize()
  {
    if (BitConverter.IsLittleEndian)
    {
      EditorPrefsX.endianDiff1 = 0;
      EditorPrefsX.endianDiff2 = 0;
    }
    else
    {
      EditorPrefsX.endianDiff1 = 3;
      EditorPrefsX.endianDiff2 = 1;
    }
    if (EditorPrefsX.byteBlock == null)
      EditorPrefsX.byteBlock = new byte[4];
    EditorPrefsX.idx = 1;
  }

  public static bool SaveBytes(string key, byte[] bytes) => true;

  public static void ConvertFloatToBytes(float f, byte[] bytes)
  {
    EditorPrefsX.byteBlock = BitConverter.GetBytes(f);
    EditorPrefsX.ConvertTo4Bytes(bytes);
  }

  public static float ConvertBytesToFloat(byte[] bytes)
  {
    EditorPrefsX.ConvertFrom4Bytes(bytes);
    return BitConverter.ToSingle(EditorPrefsX.byteBlock, 0);
  }

  public static void ConvertInt32ToBytes(int i, byte[] bytes)
  {
    EditorPrefsX.byteBlock = BitConverter.GetBytes(i);
    EditorPrefsX.ConvertTo4Bytes(bytes);
  }

  public static int ConvertBytesToInt32(byte[] bytes)
  {
    EditorPrefsX.ConvertFrom4Bytes(bytes);
    return BitConverter.ToInt32(EditorPrefsX.byteBlock, 0);
  }

  public static void ConvertTo4Bytes(byte[] bytes)
  {
    bytes[EditorPrefsX.idx] = EditorPrefsX.byteBlock[EditorPrefsX.endianDiff1];
    bytes[EditorPrefsX.idx + 1] = EditorPrefsX.byteBlock[1 + EditorPrefsX.endianDiff2];
    bytes[EditorPrefsX.idx + 2] = EditorPrefsX.byteBlock[2 - EditorPrefsX.endianDiff2];
    bytes[EditorPrefsX.idx + 3] = EditorPrefsX.byteBlock[3 - EditorPrefsX.endianDiff1];
    EditorPrefsX.idx += 4;
  }

  public static void ConvertFrom4Bytes(byte[] bytes)
  {
    EditorPrefsX.byteBlock[EditorPrefsX.endianDiff1] = bytes[EditorPrefsX.idx];
    EditorPrefsX.byteBlock[1 + EditorPrefsX.endianDiff2] = bytes[EditorPrefsX.idx + 1];
    EditorPrefsX.byteBlock[2 - EditorPrefsX.endianDiff2] = bytes[EditorPrefsX.idx + 2];
    EditorPrefsX.byteBlock[3 - EditorPrefsX.endianDiff1] = bytes[EditorPrefsX.idx + 3];
    EditorPrefsX.idx += 4;
  }

  public enum ArrayType
  {
    Float,
    Int32,
    Bool,
    String,
    Vector2,
    Vector3,
    Quaternion,
    Color,
  }
}
