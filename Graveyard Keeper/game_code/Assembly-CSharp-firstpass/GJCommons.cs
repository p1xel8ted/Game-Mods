// Decompiled with JetBrains decompiler
// Type: GJCommons
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class GJCommons
{
  public static long initial_ticks;
  public static float ticks_in_seconds;
  public static int current_timestamp;
  public static float _paused_time;
  public static float _pause_shift;
  public static GJCommons.GJCommonsHelper _helper;
  public static bool _helper_set;
  public static Transform _main_camera_tf;
  public static bool _main_camera_tf_set;

  public static string ListToString<T>(List<T> list)
  {
    string str = $"[List of <{typeof (T)?.ToString()}>: ";
    for (int index = 0; index < list.Count; ++index)
    {
      if (index > 0)
        str += ", ";
      str += list[index]?.ToString();
    }
    return str + "]";
  }

  public static string GetRomeNumber(int n, string one_char = "I")
  {
    switch (n)
    {
      case 1:
        return one_char;
      case 2:
        return one_char + one_char;
      case 3:
        return one_char + one_char + one_char;
      case 4:
        return one_char + "V";
      case 5:
        return "V";
      case 6:
        return "V" + one_char;
      case 7:
        return $"V{one_char}{one_char}";
      case 8:
        return $"{one_char}{one_char}X";
      case 9:
        return one_char + "X";
      case 10:
        return "X";
      default:
        return n.ToString();
    }
  }

  public static string GetTimeDebug() => $"<color=#006b70>[{Time.time.ToString("f3")}]</color> ";

  public static string FormatNumber(int n)
  {
    if (n < 1000)
      return n.ToString();
    return n < 1000000 ? $"'{n:### ###}'" : $"'{n:### ### ###}'";
  }

  public static float GetTicksInSeconds()
  {
    if (GJCommons.initial_ticks == 0L)
      GJCommons.initial_ticks = DateTime.Now.Ticks;
    return (float) (DateTime.Now.Ticks - GJCommons.initial_ticks) / 1E+07f - GJCommons._pause_shift;
  }

  public static int GetDateTicksInSeconds()
  {
    return (int) (DateTime.UtcNow - new DateTime(2000, 1, 1, 8, 0, 0, DateTimeKind.Utc)).TotalSeconds;
  }

  public static bool BracketValidator(string s)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (int num3 in s)
    {
      if (num3 == 40)
        ++num1;
      if (num3 == 41)
        ++num2;
    }
    return num1 == num2;
  }

  public static float RoundFloatXX(float a) => Mathf.Round(a * 10f) / 10f;

  public static void OnApplicationPaused()
  {
    GJCommons._paused_time = GJCommons.GetTicksInSeconds();
  }

  public static void OnApplicationUnpaused()
  {
    double ticksInSeconds = (double) GJCommons.GetTicksInSeconds();
  }

  public static bool IsStringOnlyNumbers(ref string s)
  {
    if (s.Length == 0)
      return false;
    for (int index = 0; index < s.Length; ++index)
    {
      char ch = s[index];
      if (ch < '0' || ch > '9')
        return false;
    }
    return true;
  }

  public static string RemoveLineDashes(string s) => s.Replace("- ", "").Replace("-\n", "");

  public static string MyEscape(string a) => WWW.EscapeURL(a).Replace("+", "%20");

  public static void SendEmail(string addr, string subject, string body)
  {
    Application.OpenURL($"mailto:{addr}?subject={GJCommons.MyEscape(subject)}&body={GJCommons.MyEscape(body)}");
  }

  public static float GetDialonalSize()
  {
    if ((double) Screen.dpi == 0.0)
      return 10f;
    float dialonalSize = Mathf.Sqrt((float) (Screen.width * Screen.width + Screen.height * Screen.height)) / Screen.dpi;
    Debug.Log((object) $"Screen.dpi = {Screen.dpi.ToString()}, diagonal = {dialonalSize.ToString()}");
    return dialonalSize;
  }

  public static void SetLayerRecursively(this GameObject obj, int layer)
  {
    obj.layer = layer;
    foreach (Transform transform in obj.transform)
    {
      if (!((UnityEngine.Object) transform == (UnityEngine.Object) null))
        transform.gameObject.SetLayerRecursively(layer);
    }
  }

  public static GameObject GameObjectHardFind(string str)
  {
    GameObject gameObject = (GameObject) null;
    foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
    {
      if ((UnityEngine.Object) transform.parent == (UnityEngine.Object) null)
      {
        gameObject = GJCommons.GameObjectHardFind(transform.gameObject, str, 0);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          break;
      }
    }
    return gameObject;
  }

  public static GameObject GameObjectHardFind(string str, string parent)
  {
    GameObject gameObject = GJCommons.GameObjectHardFind(parent);
    return (UnityEngine.Object) gameObject == (UnityEngine.Object) null ? (GameObject) null : GJCommons.GameObjectHardFind(gameObject, str, 0);
  }

  public static GameObject GameObjectHardFind(GameObject item, string str, int index)
  {
    if (item.name == str)
      return item;
    if (index >= item.transform.childCount)
      return (GameObject) null;
    GameObject gameObject = GJCommons.GameObjectHardFind(item.transform.GetChild(index).gameObject, str, 0);
    return (UnityEngine.Object) gameObject == (UnityEngine.Object) null ? GJCommons.GameObjectHardFind(item, str, ++index) : gameObject;
  }

  public static void Destroy(UnityEngine.Object o)
  {
    if (Application.isPlaying)
      UnityEngine.Object.Destroy(o);
    else
      UnityEngine.Object.DestroyImmediate(o);
  }

  public static Transform main_camera_tf
  {
    get
    {
      if (!GJCommons._main_camera_tf_set)
      {
        GJCommons._main_camera_tf_set = true;
        GJCommons._main_camera_tf = Camera.main.transform;
      }
      return GJCommons._main_camera_tf;
    }
  }

  public static GJCommons.GJCommonsHelper helper
  {
    get
    {
      if (GJCommons._helper_set)
        return GJCommons._helper;
      GJCommons._helper = UnityEngine.Object.FindObjectOfType<GJCommons.GJCommonsHelper>();
      if ((UnityEngine.Object) GJCommons._helper != (UnityEngine.Object) null)
      {
        GJCommons._helper_set = true;
        return GJCommons._helper;
      }
      GJCommons._helper = new GameObject("* GJCommonsHelper")
      {
        transform = {
          parent = GJCommons.main_camera_tf
        }
      }.AddComponent<GJCommons.GJCommonsHelper>();
      GJCommons._helper_set = true;
      return GJCommons._helper;
    }
  }

  public static Vector3 main_camera_pos => GJCommons.helper.main_camera_pos;

  public static Vector2 main_camera_pos_v2 => GJCommons.helper.main_camera_pos2;

  public delegate void VoidDelegate();

  public delegate bool BoolDelegate();

  public class GJCommonsHelper : MonoBehaviour
  {
    public Vector3 main_camera_pos = Vector3.zero;
    public Vector2 main_camera_pos2 = Vector2.zero;

    public void Update()
    {
      this.main_camera_pos = GJCommons.main_camera_tf.position;
      this.main_camera_pos2 = (Vector2) this.main_camera_pos;
    }
  }
}
