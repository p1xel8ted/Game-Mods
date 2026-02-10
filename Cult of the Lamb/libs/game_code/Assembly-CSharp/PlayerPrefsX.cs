// Decompiled with JetBrains decompiler
// Type: PlayerPrefsX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlayerPrefsX
{
  public static int endianDiff1;
  public static int endianDiff2;
  public static int idx;
  public static byte[] byteBlock;

  public static bool SetBool(string name, bool value)
  {
    try
    {
      PlayerPrefs.SetInt(name, value ? 1 : 0);
    }
    catch
    {
      return false;
    }
    return true;
  }

  public static bool GetBool(string name) => PlayerPrefs.GetInt(name) == 1;

  public static bool GetBool(string name, bool defaultValue)
  {
    return 1 == PlayerPrefs.GetInt(name, defaultValue ? 1 : 0);
  }

  public static long GetLong(string key, long defaultValue)
  {
    int lowBits;
    int highBits;
    PlayerPrefsX.SplitLong(defaultValue, out lowBits, out highBits);
    int num = PlayerPrefs.GetInt(key + "_lowBits", lowBits);
    return (long) (uint) PlayerPrefs.GetInt(key + "_highBits", highBits) << 32 /*0x20*/ | (long) (uint) num;
  }

  public static long GetLong(string key)
  {
    int num = PlayerPrefs.GetInt(key + "_lowBits");
    return (long) (uint) PlayerPrefs.GetInt(key + "_highBits") << 32 /*0x20*/ | (long) (uint) num;
  }

  public static void SplitLong(long input, out int lowBits, out int highBits)
  {
    lowBits = (int) (uint) input;
    highBits = (int) (uint) (input >> 32 /*0x20*/);
  }

  public static void SetLong(string key, long value)
  {
    int lowBits;
    int highBits;
    PlayerPrefsX.SplitLong(value, out lowBits, out highBits);
    PlayerPrefs.SetInt(key + "_lowBits", lowBits);
    PlayerPrefs.SetInt(key + "_highBits", highBits);
  }

  public static bool SetVector2(string key, Vector2 vector)
  {
    return PlayerPrefsX.SetFloatArray(key, new float[2]
    {
      vector.x,
      vector.y
    });
  }

  public static Vector2 GetVector2(string key)
  {
    float[] floatArray = PlayerPrefsX.GetFloatArray(key);
    return floatArray.Length < 2 ? Vector2.zero : new Vector2(floatArray[0], floatArray[1]);
  }

  public static Vector2 GetVector2(string key, Vector2 defaultValue)
  {
    return PlayerPrefs.HasKey(key) ? PlayerPrefsX.GetVector2(key) : defaultValue;
  }

  public static bool SetVector3(string key, Vector3 vector)
  {
    return PlayerPrefsX.SetFloatArray(key, new float[3]
    {
      vector.x,
      vector.y,
      vector.z
    });
  }

  public static Vector3 GetVector3(string key)
  {
    float[] floatArray = PlayerPrefsX.GetFloatArray(key);
    return floatArray.Length < 3 ? Vector3.zero : new Vector3(floatArray[0], floatArray[1], floatArray[2]);
  }

  public static Vector3 GetVector3(string key, Vector3 defaultValue)
  {
    return PlayerPrefs.HasKey(key) ? PlayerPrefsX.GetVector3(key) : defaultValue;
  }

  public static bool SetQuaternion(string key, Quaternion vector)
  {
    return PlayerPrefsX.SetFloatArray(key, new float[4]
    {
      vector.x,
      vector.y,
      vector.z,
      vector.w
    });
  }

  public static Quaternion GetQuaternion(string key)
  {
    float[] floatArray = PlayerPrefsX.GetFloatArray(key);
    return floatArray.Length < 4 ? Quaternion.identity : new Quaternion(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
  }

  public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
  {
    return PlayerPrefs.HasKey(key) ? PlayerPrefsX.GetQuaternion(key) : defaultValue;
  }

  public static bool SetColor(string key, Color color)
  {
    return PlayerPrefsX.SetFloatArray(key, new float[4]
    {
      color.r,
      color.g,
      color.b,
      color.a
    });
  }

  public static Color GetColor(string key)
  {
    float[] floatArray = PlayerPrefsX.GetFloatArray(key);
    return floatArray.Length < 4 ? new Color(0.0f, 0.0f, 0.0f, 0.0f) : new Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
  }

  public static Color GetColor(string key, Color defaultValue)
  {
    return PlayerPrefs.HasKey(key) ? PlayerPrefsX.GetColor(key) : defaultValue;
  }

  public static bool SetBoolArray(string key, bool[] boolArray)
  {
    byte[] bytes = new byte[(boolArray.Length + 7) / 8 + 5];
    bytes[0] = Convert.ToByte((object) PlayerPrefsX.ArrayType.Bool);
    new BitArray(boolArray).CopyTo((Array) bytes, 5);
    PlayerPrefsX.Initialize();
    PlayerPrefsX.ConvertInt32ToBytes(boolArray.Length, bytes);
    return PlayerPrefsX.SaveBytes(key, bytes);
  }

  public static bool[] GetBoolArray(string key)
  {
    if (!PlayerPrefs.HasKey(key))
      return new bool[0];
    byte[] numArray1 = Convert.FromBase64String(PlayerPrefs.GetString(key));
    if (numArray1.Length < 5)
    {
      Debug.LogError((object) ("Corrupt preference file for " + key));
      return new bool[0];
    }
    if (numArray1[0] != (byte) 2)
    {
      Debug.LogError((object) (key + " is not a boolean array"));
      return new bool[0];
    }
    PlayerPrefsX.Initialize();
    byte[] numArray2 = new byte[numArray1.Length - 5];
    Array.Copy((Array) numArray1, 5, (Array) numArray2, 0, numArray2.Length);
    BitArray bitArray = new BitArray(numArray2);
    bitArray.Length = PlayerPrefsX.ConvertBytesToInt32(numArray1);
    bool[] boolArray = new bool[bitArray.Count];
    bitArray.CopyTo((Array) boolArray, 0);
    return boolArray;
  }

  public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
  {
    if (PlayerPrefs.HasKey(key))
      return PlayerPrefsX.GetBoolArray(key);
    bool[] boolArray = new bool[defaultSize];
    for (int index = 0; index < defaultSize; ++index)
      boolArray[index] = defaultValue;
    return boolArray;
  }

  public static bool SetStringArray(string key, string[] stringArray)
  {
    byte[] inArray = new byte[stringArray.Length + 1];
    inArray[0] = Convert.ToByte((object) PlayerPrefsX.ArrayType.String);
    PlayerPrefsX.Initialize();
    for (int index = 0; index < stringArray.Length; ++index)
    {
      if (stringArray[index] == null)
      {
        Debug.LogError((object) ("Can't save null entries in the string array when setting " + key));
        return false;
      }
      if (stringArray[index].Length > (int) byte.MaxValue)
      {
        Debug.LogError((object) ("Strings cannot be longer than 255 characters when setting " + key));
        return false;
      }
      inArray[PlayerPrefsX.idx++] = (byte) stringArray[index].Length;
    }
    try
    {
      PlayerPrefs.SetString(key, $"{Convert.ToBase64String(inArray)}|{string.Join("", stringArray)}");
    }
    catch
    {
      return false;
    }
    return true;
  }

  public static string[] GetStringArray(string key)
  {
    if (!PlayerPrefs.HasKey(key))
      return new string[0];
    string str = PlayerPrefs.GetString(key);
    int length1 = str.IndexOf("|"[0]);
    if (length1 < 4)
    {
      Debug.LogError((object) ("Corrupt preference file for " + key));
      return new string[0];
    }
    byte[] numArray = Convert.FromBase64String(str.Substring(0, length1));
    if (numArray[0] != (byte) 3)
    {
      Debug.LogError((object) (key + " is not a string array"));
      return new string[0];
    }
    PlayerPrefsX.Initialize();
    int length2 = numArray.Length - 1;
    string[] stringArray = new string[length2];
    int startIndex = length1 + 1;
    for (int index = 0; index < length2; ++index)
    {
      int length3 = (int) numArray[PlayerPrefsX.idx++];
      if (startIndex + length3 > str.Length)
      {
        Debug.LogError((object) ("Corrupt preference file for " + key));
        return new string[0];
      }
      stringArray[index] = str.Substring(startIndex, length3);
      startIndex += length3;
    }
    return stringArray;
  }

  public static string[] GetStringArray(string key, string defaultValue, int defaultSize)
  {
    if (PlayerPrefs.HasKey(key))
      return PlayerPrefsX.GetStringArray(key);
    string[] stringArray = new string[defaultSize];
    for (int index = 0; index < defaultSize; ++index)
      stringArray[index] = defaultValue;
    return stringArray;
  }

  public static bool SetIntArray(string key, int[] intArray)
  {
    return PlayerPrefsX.SetValue<int[]>(key, intArray, PlayerPrefsX.ArrayType.Int32, 1, new Action<int[], byte[], int>(PlayerPrefsX.ConvertFromInt));
  }

  public static bool SetFloatArray(string key, float[] floatArray)
  {
    return PlayerPrefsX.SetValue<float[]>(key, floatArray, PlayerPrefsX.ArrayType.Float, 1, new Action<float[], byte[], int>(PlayerPrefsX.ConvertFromFloat));
  }

  public static bool SetVector2Array(string key, Vector2[] vector2Array)
  {
    return PlayerPrefsX.SetValue<Vector2[]>(key, vector2Array, PlayerPrefsX.ArrayType.Vector2, 2, new Action<Vector2[], byte[], int>(PlayerPrefsX.ConvertFromVector2));
  }

  public static bool SetVector3Array(string key, Vector3[] vector3Array)
  {
    return PlayerPrefsX.SetValue<Vector3[]>(key, vector3Array, PlayerPrefsX.ArrayType.Vector3, 3, new Action<Vector3[], byte[], int>(PlayerPrefsX.ConvertFromVector3));
  }

  public static bool SetQuaternionArray(string key, Quaternion[] quaternionArray)
  {
    return PlayerPrefsX.SetValue<Quaternion[]>(key, quaternionArray, PlayerPrefsX.ArrayType.Quaternion, 4, new Action<Quaternion[], byte[], int>(PlayerPrefsX.ConvertFromQuaternion));
  }

  public static bool SetColorArray(string key, Color[] colorArray)
  {
    return PlayerPrefsX.SetValue<Color[]>(key, colorArray, PlayerPrefsX.ArrayType.Color, 4, new Action<Color[], byte[], int>(PlayerPrefsX.ConvertFromColor));
  }

  public static bool SetValue<T>(
    string key,
    T array,
    PlayerPrefsX.ArrayType arrayType,
    int vectorNumber,
    Action<T, byte[], int> convert)
    where T : IList
  {
    byte[] bytes = new byte[4 * array.Count * vectorNumber + 1];
    bytes[0] = Convert.ToByte((object) arrayType);
    PlayerPrefsX.Initialize();
    for (int index = 0; index < array.Count; ++index)
      convert(array, bytes, index);
    return PlayerPrefsX.SaveBytes(key, bytes);
  }

  public static void ConvertFromInt(int[] array, byte[] bytes, int i)
  {
    PlayerPrefsX.ConvertInt32ToBytes(array[i], bytes);
  }

  public static void ConvertFromFloat(float[] array, byte[] bytes, int i)
  {
    PlayerPrefsX.ConvertFloatToBytes(array[i], bytes);
  }

  public static void ConvertFromVector2(Vector2[] array, byte[] bytes, int i)
  {
    PlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
  }

  public static void ConvertFromVector3(Vector3[] array, byte[] bytes, int i)
  {
    PlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].z, bytes);
  }

  public static void ConvertFromQuaternion(Quaternion[] array, byte[] bytes, int i)
  {
    PlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].z, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].w, bytes);
  }

  public static void ConvertFromColor(Color[] array, byte[] bytes, int i)
  {
    PlayerPrefsX.ConvertFloatToBytes(array[i].r, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].g, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].b, bytes);
    PlayerPrefsX.ConvertFloatToBytes(array[i].a, bytes);
  }

  public static int[] GetIntArray(string key)
  {
    List<int> list = new List<int>();
    PlayerPrefsX.GetValue<List<int>>(key, list, PlayerPrefsX.ArrayType.Int32, 1, new Action<List<int>, byte[]>(PlayerPrefsX.ConvertToInt));
    return list.ToArray();
  }

  public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
  {
    if (PlayerPrefs.HasKey(key))
      return PlayerPrefsX.GetIntArray(key);
    int[] intArray = new int[defaultSize];
    for (int index = 0; index < defaultSize; ++index)
      intArray[index] = defaultValue;
    return intArray;
  }

  public static float[] GetFloatArray(string key)
  {
    List<float> list = new List<float>();
    PlayerPrefsX.GetValue<List<float>>(key, list, PlayerPrefsX.ArrayType.Float, 1, new Action<List<float>, byte[]>(PlayerPrefsX.ConvertToFloat));
    return list.ToArray();
  }

  public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
  {
    if (PlayerPrefs.HasKey(key))
      return PlayerPrefsX.GetFloatArray(key);
    float[] floatArray = new float[defaultSize];
    for (int index = 0; index < defaultSize; ++index)
      floatArray[index] = defaultValue;
    return floatArray;
  }

  public static Vector2[] GetVector2Array(string key)
  {
    List<Vector2> list = new List<Vector2>();
    PlayerPrefsX.GetValue<List<Vector2>>(key, list, PlayerPrefsX.ArrayType.Vector2, 2, new Action<List<Vector2>, byte[]>(PlayerPrefsX.ConvertToVector2));
    return list.ToArray();
  }

  public static Vector2[] GetVector2Array(string key, Vector2 defaultValue, int defaultSize)
  {
    if (PlayerPrefs.HasKey(key))
      return PlayerPrefsX.GetVector2Array(key);
    Vector2[] vector2Array = new Vector2[defaultSize];
    for (int index = 0; index < defaultSize; ++index)
      vector2Array[index] = defaultValue;
    return vector2Array;
  }

  public static Vector3[] GetVector3Array(string key)
  {
    List<Vector3> list = new List<Vector3>();
    PlayerPrefsX.GetValue<List<Vector3>>(key, list, PlayerPrefsX.ArrayType.Vector3, 3, new Action<List<Vector3>, byte[]>(PlayerPrefsX.ConvertToVector3));
    return list.ToArray();
  }

  public static Vector3[] GetVector3Array(string key, Vector3 defaultValue, int defaultSize)
  {
    if (PlayerPrefs.HasKey(key))
      return PlayerPrefsX.GetVector3Array(key);
    Vector3[] vector3Array = new Vector3[defaultSize];
    for (int index = 0; index < defaultSize; ++index)
      vector3Array[index] = defaultValue;
    return vector3Array;
  }

  public static Quaternion[] GetQuaternionArray(string key)
  {
    List<Quaternion> list = new List<Quaternion>();
    PlayerPrefsX.GetValue<List<Quaternion>>(key, list, PlayerPrefsX.ArrayType.Quaternion, 4, new Action<List<Quaternion>, byte[]>(PlayerPrefsX.ConvertToQuaternion));
    return list.ToArray();
  }

  public static Quaternion[] GetQuaternionArray(
    string key,
    Quaternion defaultValue,
    int defaultSize)
  {
    if (PlayerPrefs.HasKey(key))
      return PlayerPrefsX.GetQuaternionArray(key);
    Quaternion[] quaternionArray = new Quaternion[defaultSize];
    for (int index = 0; index < defaultSize; ++index)
      quaternionArray[index] = defaultValue;
    return quaternionArray;
  }

  public static Color[] GetColorArray(string key)
  {
    List<Color> list = new List<Color>();
    PlayerPrefsX.GetValue<List<Color>>(key, list, PlayerPrefsX.ArrayType.Color, 4, new Action<List<Color>, byte[]>(PlayerPrefsX.ConvertToColor));
    return list.ToArray();
  }

  public static Color[] GetColorArray(string key, Color defaultValue, int defaultSize)
  {
    if (PlayerPrefs.HasKey(key))
      return PlayerPrefsX.GetColorArray(key);
    Color[] colorArray = new Color[defaultSize];
    for (int index = 0; index < defaultSize; ++index)
      colorArray[index] = defaultValue;
    return colorArray;
  }

  public static void GetValue<T>(
    string key,
    T list,
    PlayerPrefsX.ArrayType arrayType,
    int vectorNumber,
    Action<T, byte[]> convert)
    where T : IList
  {
    if (!PlayerPrefs.HasKey(key))
      return;
    byte[] numArray = Convert.FromBase64String(PlayerPrefs.GetString(key));
    if ((numArray.Length - 1) % (vectorNumber * 4) != 0)
      Debug.LogError((object) ("Corrupt preference file for " + key));
    else if ((PlayerPrefsX.ArrayType) numArray[0] != arrayType)
    {
      Debug.LogError((object) $"{key} is not a {arrayType.ToString()} array");
    }
    else
    {
      PlayerPrefsX.Initialize();
      int num = (numArray.Length - 1) / (vectorNumber * 4);
      for (int index = 0; index < num; ++index)
        convert(list, numArray);
    }
  }

  public static void ConvertToInt(List<int> list, byte[] bytes)
  {
    list.Add(PlayerPrefsX.ConvertBytesToInt32(bytes));
  }

  public static void ConvertToFloat(List<float> list, byte[] bytes)
  {
    list.Add(PlayerPrefsX.ConvertBytesToFloat(bytes));
  }

  public static void ConvertToVector2(List<Vector2> list, byte[] bytes)
  {
    list.Add(new Vector2(PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes)));
  }

  public static void ConvertToVector3(List<Vector3> list, byte[] bytes)
  {
    list.Add(new Vector3(PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes)));
  }

  public static void ConvertToQuaternion(List<Quaternion> list, byte[] bytes)
  {
    list.Add(new Quaternion(PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes)));
  }

  public static void ConvertToColor(List<Color> list, byte[] bytes)
  {
    list.Add(new Color(PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes)));
  }

  public static void ShowArrayType(string key)
  {
    byte[] numArray = Convert.FromBase64String(PlayerPrefs.GetString(key));
    if (numArray.Length == 0)
      return;
    PlayerPrefsX.ArrayType arrayType = (PlayerPrefsX.ArrayType) numArray[0];
    Debug.Log((object) $"{key} is a {arrayType.ToString()} array");
  }

  public static void Initialize()
  {
    if (BitConverter.IsLittleEndian)
    {
      PlayerPrefsX.endianDiff1 = 0;
      PlayerPrefsX.endianDiff2 = 0;
    }
    else
    {
      PlayerPrefsX.endianDiff1 = 3;
      PlayerPrefsX.endianDiff2 = 1;
    }
    if (PlayerPrefsX.byteBlock == null)
      PlayerPrefsX.byteBlock = new byte[4];
    PlayerPrefsX.idx = 1;
  }

  public static bool SaveBytes(string key, byte[] bytes)
  {
    try
    {
      PlayerPrefs.SetString(key, Convert.ToBase64String(bytes));
    }
    catch
    {
      return false;
    }
    return true;
  }

  public static void ConvertFloatToBytes(float f, byte[] bytes)
  {
    PlayerPrefsX.byteBlock = BitConverter.GetBytes(f);
    PlayerPrefsX.ConvertTo4Bytes(bytes);
  }

  public static float ConvertBytesToFloat(byte[] bytes)
  {
    PlayerPrefsX.ConvertFrom4Bytes(bytes);
    return BitConverter.ToSingle(PlayerPrefsX.byteBlock, 0);
  }

  public static void ConvertInt32ToBytes(int i, byte[] bytes)
  {
    PlayerPrefsX.byteBlock = BitConverter.GetBytes(i);
    PlayerPrefsX.ConvertTo4Bytes(bytes);
  }

  public static int ConvertBytesToInt32(byte[] bytes)
  {
    PlayerPrefsX.ConvertFrom4Bytes(bytes);
    return BitConverter.ToInt32(PlayerPrefsX.byteBlock, 0);
  }

  public static void ConvertTo4Bytes(byte[] bytes)
  {
    bytes[PlayerPrefsX.idx] = PlayerPrefsX.byteBlock[PlayerPrefsX.endianDiff1];
    bytes[PlayerPrefsX.idx + 1] = PlayerPrefsX.byteBlock[1 + PlayerPrefsX.endianDiff2];
    bytes[PlayerPrefsX.idx + 2] = PlayerPrefsX.byteBlock[2 - PlayerPrefsX.endianDiff2];
    bytes[PlayerPrefsX.idx + 3] = PlayerPrefsX.byteBlock[3 - PlayerPrefsX.endianDiff1];
    PlayerPrefsX.idx += 4;
  }

  public static void ConvertFrom4Bytes(byte[] bytes)
  {
    PlayerPrefsX.byteBlock[PlayerPrefsX.endianDiff1] = bytes[PlayerPrefsX.idx];
    PlayerPrefsX.byteBlock[1 + PlayerPrefsX.endianDiff2] = bytes[PlayerPrefsX.idx + 1];
    PlayerPrefsX.byteBlock[2 - PlayerPrefsX.endianDiff2] = bytes[PlayerPrefsX.idx + 2];
    PlayerPrefsX.byteBlock[3 - PlayerPrefsX.endianDiff1] = bytes[PlayerPrefsX.idx + 3];
    PlayerPrefsX.idx += 4;
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
