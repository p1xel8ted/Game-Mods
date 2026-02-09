// Decompiled with JetBrains decompiler
// Type: GJCommon
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

#nullable disable
public class GJCommon
{
  public static bool DEBUG_MEMORY = false;
  public static string LOREM_IPSUM_SHORT = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
  public static string LOREM_IPSUM = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
  public static bool USE_ETC_TEXTURES = false;
  public static bool PVR_SUPPORTED = true;
  public static string DEFAULT_SHADER_NAME = "ex2D/Alpha Blended";
  public static string BASE_62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
  public static string BASE_READABLE = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";

  public static byte[] MergeByteArrays(byte[] b1, byte[] b2)
  {
    byte[] numArray = new byte[b1.Length + b2.Length];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = index < b1.Length ? b1[index] : b2[index - b1.Length];
    return numArray;
  }

  public static byte[] MergeByteArrays(byte[] b1, List<byte> b2)
  {
    byte[] numArray = new byte[b1.Length + b2.Count];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = index < b1.Length ? b1[index] : b2[index - b1.Length];
    return numArray;
  }

  public static byte[] ReadBinaryDataFromStream(Stream stream)
  {
    BinaryReader binaryReader = new BinaryReader(stream);
    if (binaryReader == null)
    {
      Debug.LogError((object) $"ReadBinaryDataFromStream({stream?.ToString()}) error!");
      return (byte[]) null;
    }
    List<byte> byteList = new List<byte>();
    try
    {
      while (true)
      {
        byte num = binaryReader.ReadByte();
        byteList.Add(num);
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine((object) ex);
    }
    binaryReader.Close();
    return byteList.ToArray();
  }

  public static byte[] ReadBinaryResource(string resource_name)
  {
    TextAsset textAsset = Resources.Load(resource_name) as TextAsset;
    if ((UnityEngine.Object) textAsset == (UnityEngine.Object) null)
    {
      Debug.LogError((object) $"Error ReadBinaryResource '{resource_name}' !");
      return (byte[]) null;
    }
    Stream stream = (Stream) new MemoryStream(textAsset.bytes);
    byte[] numArray = GJCommon.ReadBinaryDataFromStream((Stream) (stream as MemoryStream));
    stream.Close();
    return numArray;
  }

  public static string Base64Encode(byte[] b)
  {
    return Convert.ToBase64String(b).Replace("+", "-").Replace("/", "_").Split("="[0])[0];
  }

  public static string Base64Encode(string s) => GJCommon.Base64Encode(Encoding.UTF8.GetBytes(s));

  public static byte[] Base64Decode(string s)
  {
    s = s.Replace("-", "+");
    s = s.Replace("_", "/");
    return Convert.FromBase64String(s);
  }

  public static int Checksum(byte[] b)
  {
    int num = 0;
    for (int index = 0; index < b.Length; ++index)
      num += (int) b[index];
    return num;
  }

  public static int CountTrailingNumbers(string s)
  {
    int num = 0;
    for (; (int) s.Substring(s.Length - 1)[0] >= (int) "0"[0] && (int) s.Substring(s.Length - 1)[0] <= (int) "9"[0]; s = s.Substring(0, s.Length - 1))
      ++num;
    return num;
  }

  public static string FormatTime(
    int seconds,
    string days_suffix,
    string hours_suffix,
    string min_suffix,
    string sec_suffix)
  {
    int num1 = (int) Mathf.Floor((float) (seconds / 86400));
    seconds -= num1 * 86400;
    int num2 = (int) Mathf.Floor((float) (seconds / 3600));
    seconds -= num2 * 3600;
    int num3 = (int) Mathf.Floor((float) (seconds / 60));
    seconds -= num3 * 60;
    int num4 = seconds;
    string str1 = num4.ToString() ?? "";
    string str2 = num3.ToString() ?? "";
    string str3 = num2.ToString() ?? "";
    string str4 = num1.ToString() ?? "";
    if (min_suffix == ":" && num4 < 10)
      str1 = "0" + str1;
    if (hours_suffix == ":" && num3 < 10)
      str2 = "0" + str2;
    if (num1 > 0)
      return str4 + days_suffix + str3 + hours_suffix + str2;
    if (num2 > 0)
      return str3 + hours_suffix + str2 + min_suffix + str1 + sec_suffix;
    if (num3 > 0)
      return str2 + min_suffix + str1 + sec_suffix;
    if (min_suffix == ":")
      str1 = "00:" + str1;
    return str1 + sec_suffix;
  }

  public static string FormatInt(int i)
  {
    string str1 = i.ToString() ?? "";
    string str2 = "";
    int num = 0;
    while (true)
    {
      do
      {
        str2 = str1[str1.Length - 1].ToString() + str2;
        if (str1.Length != 1)
          str1 = str1.Substring(0, str1.Length - 1);
        else
          goto label_4;
      }
      while (++num <= 2);
      str2 = " " + str2;
      num = 0;
    }
label_4:
    return str2;
  }

  public static List<Rect> FillRectWithSquares(List<int> available_squares, Vector2 rect)
  {
    List<Rect> rectList = new List<Rect>();
    int x = (int) rect.x;
    int y = (int) rect.y;
    List<List<bool>> boolListList = new List<List<bool>>(x);
    for (int index = 0; index < x; ++index)
      boolListList[index] = new List<bool>(y);
    for (int index1 = 0; (double) index1 < (double) rect.x; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) rect.y; ++index2)
        boolListList[index1][index2] = false;
    }
    List<int> intList = new List<int>();
    intList.AddRange((IEnumerable<int>) available_squares);
    intList.Sort();
    for (int index3 = 0; (double) index3 < (double) rect.x; ++index3)
    {
      for (int index4 = 0; (double) index4 < (double) rect.y; ++index4)
      {
        if (!boolListList[index3][index4])
        {
          int count = intList.Count;
          int num1 = 0;
          while (count > 0)
          {
            --count;
            num1 = intList[count];
            if ((double) (index3 + num1) <= (double) rect.x && (double) (index4 + num1) <= (double) rect.y)
            {
              bool flag = true;
              for (int index5 = 0; index5 < num1; ++index5)
              {
                for (int index6 = 0; index6 < num1; ++index6)
                {
                  if (boolListList[index3 + index5][index4 + index6])
                  {
                    flag = false;
                    break;
                  }
                }
                if (!flag)
                  break;
              }
              if (flag)
                break;
            }
            if (count == 0)
              Debug.LogWarning((object) $"FillRectWithSquares() couldn't fill rectangle correctly! (Maybe you don't have 1x1 square?)\nDidn't fill square at ({index3.ToString()}, {index4.ToString()}) of square {rect.x.ToString()}x{rect.y.ToString()}");
          }
          int num2 = 0;
          int num3 = 0;
          try
          {
            for (num2 = 0; num2 < num1; ++num2)
            {
              for (num3 = 0; num3 < num1; ++num3)
                boolListList[index3 + num2][index4 + num3] = true;
            }
          }
          catch (Exception ex)
          {
            Debug.LogError((object) $"Map is out of range: {(index3 + num2).ToString()}, {(index4 + num3).ToString()}");
          }
          rectList.Add(new Rect((float) index3, (float) index4, (float) num1, (float) num1));
        }
      }
    }
    return rectList;
  }

  public static byte[] StringToByteArray(string s)
  {
    byte[] byteArray = new byte[s.Length];
    for (int index = 0; index < s.Length; ++index)
      byteArray[index] = (byte) s[index];
    return byteArray;
  }

  public static string MemoryToString(long mem)
  {
    if (mem < 1024L /*0x0400*/)
      return mem.ToString() + " b";
    return mem < 1048576L /*0x100000*/ ? Mathf.Round((float) (mem / 1024L /*0x0400*/)).ToString() + " Kb" : Mathf.Round((float) (mem / 1048576L /*0x100000*/)).ToString() + " Mb";
  }

  public static UnityEngine.Object LoadResource(string res_name)
  {
    long usedHeapSize = (long) Profiler.usedHeapSize;
    UnityEngine.Object @object = Resources.Load(res_name);
    if (@object == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) $"Error loading resource \"{res_name}\" - resource not found!");
      return (UnityEngine.Object) null;
    }
    if (GJCommon.DEBUG_MEMORY)
      Debug.Log((object) $"Resource \"{res_name}\" loaded. Memory spent: {GJCommon.MemoryToString((long) Profiler.usedHeapSize - usedHeapSize)}");
    return @object;
  }

  public static UnityEngine.Object LoadResource(string res_name, System.Type res_class)
  {
    long usedHeapSize = (long) Profiler.usedHeapSize;
    UnityEngine.Object @object = Resources.Load(res_name, res_class);
    if (@object == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) $"Error loading resource \"{res_name}\" - resource not found!");
      return (UnityEngine.Object) null;
    }
    if (GJCommon.DEBUG_MEMORY)
      Debug.Log((object) $"Resource \"{res_name}\" loaded. Memory spent: {GJCommon.MemoryToString((long) Profiler.usedHeapSize - usedHeapSize)}");
    return @object;
  }

  public static bool ArrayContains(int[] ar, int item)
  {
    for (int index = ar.Length - 1; index >= 0; --index)
    {
      if (ar[index] == item)
        return true;
    }
    return false;
  }

  public static int ConvertStringDateToTimestamp(string s)
  {
    string[] strArray = s.Split("."[0]);
    if (strArray.Length != 3)
    {
      Debug.LogError((object) ("Unknown date format: " + s));
      return 0;
    }
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    return (int) (new DateTime(int.Parse(strArray[2]), int.Parse(strArray[1]), int.Parse(strArray[0]), 0, 0, 0, DateTimeKind.Utc) - dateTime).TotalSeconds;
  }

  public static string ToAnyBase(int n, string chars)
  {
    string anyBase = "";
    int length = chars.Length;
    do
    {
      anyBase = chars[n % length].ToString() + anyBase;
      n = (int) Mathf.Floor((float) n * 1f / (float) length);
    }
    while (n > 0);
    return anyBase;
  }

  public static int FromAnyBase(string s, string chars)
  {
    int num = 0;
    int length = chars.Length;
    for (int index = 0; index < s.Length; ++index)
      num = num * length + chars.IndexOf(s[index]);
    return num;
  }

  public static string ToBase62(int n) => GJCommon.ToAnyBase(n, GJCommon.BASE_62);

  public static int FromBase62(string s) => GJCommon.FromAnyBase(s, GJCommon.BASE_62);

  public static int GetNowTimestamp()
  {
    return (int) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
  }

  public static int GetYearDay() => DateTime.Now.DayOfYear;

  public static string MD5Sum(string strToEncrypt)
  {
    byte[] hash = new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(strToEncrypt));
    string str = "";
    for (int index = 0; index < hash.Length; ++index)
      str += Convert.ToString(hash[index], 16 /*0x10*/).PadLeft(2, "0"[0]);
    return str.PadLeft(32 /*0x20*/, "0"[0]);
  }

  public static string FixURLString(string s)
  {
    s = s.Replace("&quot;", "\"");
    s = s.Replace("&#xA;", "\n");
    return s;
  }

  public static string URLEncode(string s)
  {
    s = s.Replace(" ", "%20");
    s = s.Replace("?", "%3F");
    s = s.Replace("&", "%26");
    s = s.Replace("=", "%3D");
    s = s.Replace(",", "%2C");
    s = s.Replace("'", "%27");
    s = s.Replace("\"", "%22");
    s = s.Replace("$", "%24");
    s = s.Replace("\r", "%0D");
    s = s.Replace("\n", "%0A");
    s = s.Replace(":", "%2F");
    s = s.Replace("/", "%3A");
    return s;
  }
}
