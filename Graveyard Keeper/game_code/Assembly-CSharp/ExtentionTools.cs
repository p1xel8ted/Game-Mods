// Decompiled with JetBrains decompiler
// Type: ExtentionTools
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public static class ExtentionTools
{
  public static bool EqualsTo(this float a, float b, float epsilon = 1E-05f)
  {
    return (double) Mathf.Abs(a - b) < (double) epsilon;
  }

  public static bool EqualsOrMore(this float a, float b, float epsilon = 1E-05f)
  {
    return (double) Mathf.Abs(a - b) < (double) epsilon || (double) a > (double) b;
  }

  public static bool EqualsTo(this Vector2 a, Vector2 b, float epsilon = 1E-05f)
  {
    return a.x.EqualsTo(b.x, epsilon) && a.y.EqualsTo(b.y, epsilon);
  }

  public static Vector2 Round(this Vector2 vec, float value)
  {
    if (value.EqualsTo(0.0f))
      return vec;
    vec.x = Mathf.Round(vec.x / value) * value;
    vec.y = Mathf.Round(vec.y / value) * value;
    return vec;
  }

  public static Vector3 Round(this Vector3 vec, float value = 1f)
  {
    return value.EqualsTo(0.0f) ? vec : new Vector3(Mathf.Round(vec.x / value) * value, Mathf.Round(vec.y / value) * value, Mathf.Round(vec.z / value) * value);
  }

  public static void RemoveNulls<T>(this List<T> list)
  {
    for (int index = 0; index < list.Count; ++index)
    {
      if ((object) list[index] == null)
      {
        list.RemoveAt(index);
        --index;
      }
    }
  }

  public static void RemoveUnityNulls<T>(this List<T> list) where T : UnityEngine.Object
  {
    for (int index = 0; index < list.Count; ++index)
    {
      if (!((UnityEngine.Object) list[index] != (UnityEngine.Object) null))
      {
        list.RemoveAt(index);
        --index;
      }
    }
  }

  public static T Copy<T>(this T source, Transform parent = null, bool activate = true, string name = "") where T : MonoBehaviour
  {
    if ((UnityEngine.Object) source == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Null prefab");
      return default (T);
    }
    T obj = UnityEngine.Object.Instantiate<T>(source);
    obj.gameObject.transform.SetParent(parent ?? source.gameObject.transform.parent, false);
    obj.gameObject.SetActive(activate);
    if (!string.IsNullOrEmpty(name))
      obj.name = name;
    return obj;
  }

  public static GameObject Copy(
    this GameObject source,
    Transform parent = null,
    bool activate = true,
    string name = "")
  {
    if ((UnityEngine.Object) source == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Null prefab");
      return (GameObject) null;
    }
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(source);
    gameObject.transform.SetParent(parent ?? source.transform.parent, false);
    gameObject.SetActive(activate);
    if (!string.IsNullOrEmpty(name))
      gameObject.name = name;
    return gameObject;
  }

  public static void Activate<T>(this T behaviour) where T : MonoBehaviour
  {
    if (!((UnityEngine.Object) behaviour != (UnityEngine.Object) null))
      return;
    behaviour.gameObject.SetActive(true);
  }

  public static void Deactivate<T>(this T behaviour) where T : MonoBehaviour
  {
    if (!((UnityEngine.Object) behaviour != (UnityEngine.Object) null))
      return;
    behaviour.gameObject.SetActive(false);
  }

  public static void Activate(this GameObject obj)
  {
    if (!((UnityEngine.Object) obj != (UnityEngine.Object) null))
      return;
    obj.SetActive(true);
  }

  public static void Deactivate(this GameObject obj)
  {
    if (!((UnityEngine.Object) obj != (UnityEngine.Object) null))
      return;
    obj.SetActive(false);
  }

  public static void SetActive(this MonoBehaviour behaviour, bool active)
  {
    if (!(bool) (UnityEngine.Object) behaviour)
      return;
    behaviour.gameObject.SetActive(active);
  }

  public static bool HasParam(this Animator animator, string param_name)
  {
    foreach (AnimatorControllerParameter parameter in animator.parameters)
    {
      if (parameter.name == param_name)
        return true;
    }
    return false;
  }

  public static string ColorizeText(this string text, Color color)
  {
    return $"[c][{color.ToHex()}]{text}[-][/c]";
  }

  public static string ToHex(this Color color, bool alpha = false)
  {
    Color32 color32 = (Color32) color;
    string hex = color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");
    if (alpha)
      hex += color32.a.ToString("X2");
    return hex;
  }

  public static void TryInvoke(this GJCommons.VoidDelegate callback)
  {
    if (callback == null)
      return;
    callback();
  }

  public static void TryInvoke(this System.Action callback)
  {
    if (callback == null)
      return;
    callback();
  }

  public static void TryInvoke<T>(this Action<T> callback, T obj)
  {
    if (callback == null)
      return;
    callback(obj);
  }

  public static void TryExecute(this EventDelegate event_delegate) => event_delegate?.Execute();

  public static void TrySetActive(this GameObject go, bool active)
  {
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    go.SetActive(active);
  }

  public static void RoundCamPos(this Transform tf, Camera cam, Vector3 offset = default (Vector3), float pixel_k = 1f)
  {
    tf.localPosition = offset;
    tf.position = tf.position.ReturnRoundedPos(cam, cam, pixel_k: pixel_k);
  }

  public static void SetGUIPosToWorldPos(
    this Transform tf,
    Vector3 position,
    Camera world_cam,
    Camera gui_cam,
    Vector2 shift,
    bool halfres_magic = true)
  {
    Vector3 vector3 = position.ReturnRoundedPos(world_cam, gui_cam, true, halfres_magic: halfres_magic);
    tf.position = vector3 + (Vector3) shift;
  }

  public static void SetGUIPosToWorldPos(
    this Transform tf,
    Vector3 position,
    Camera world_cam,
    Camera gui_cam)
  {
    tf.SetGUIPosToWorldPos(position, world_cam, gui_cam, Vector2.zero);
  }

  public static Vector3 ReturnRoundedPos(
    this Vector3 position,
    Camera world_cam,
    Camera obj_cam,
    bool zero_z = false,
    float pixel_k = 1f,
    bool halfres_magic = false)
  {
    float num1 = zero_z ? 0.0f : position.z;
    position = world_cam.WorldToScreenPoint(position);
    position.x = Mathf.Round(position.x / pixel_k) * pixel_k;
    position.y = Mathf.Round(position.y / pixel_k) * pixel_k;
    int num2 = halfres_magic ? 1 : 0;
    position = obj_cam.ScreenToWorldPoint(position);
    position.z = num1;
    return position;
  }

  public static T Cache<T>(this MonoBehaviour beh, out T cached, out bool flag, bool deep) where T : Component
  {
    cached = default (T);
    foreach (T component in beh.GetComponents<T>())
    {
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
      {
        cached = component;
        break;
      }
    }
    if ((UnityEngine.Object) cached == (UnityEngine.Object) null & deep)
    {
      foreach (T componentsInChild in beh.GetComponentsInChildren<T>(true))
      {
        if (!((UnityEngine.Object) componentsInChild == (UnityEngine.Object) null))
        {
          cached = componentsInChild;
          break;
        }
      }
    }
    flag = (UnityEngine.Object) cached != (UnityEngine.Object) null;
    return cached;
  }

  public static T AddComponentNotDuplicate<T>(this GameObject obj) where T : Component
  {
    T component = obj.GetComponent<T>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? component : obj.AddComponent<T>();
  }

  public static void DestroyComponentIfExists<T>(this GameObject obj) where T : MonoBehaviour
  {
    if (!((UnityEngine.Object) obj.GetComponent<T>() != (UnityEngine.Object) null))
      return;
    obj.Destroy<T>();
  }

  public static void Destroy(this GameObject go, float delay = 0.0f)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      Debug.LogError((object) "Can't destroy null obj");
    else if (!Application.isPlaying)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go);
    else if ((double) delay > 0.0)
      UnityEngine.Object.Destroy((UnityEngine.Object) go, delay);
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) go);
  }

  public static void Destroy<T>(this GameObject go, float delay = 0.0f) where T : MonoBehaviour
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      Debug.LogError((object) "Can't destroy null obj");
    else
      go.GetComponent<T>().DestroyComponent();
  }

  public static void DestroyComponent(this MonoBehaviour component, float delay = 0.0f)
  {
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      Debug.LogError((object) "Can't destroy null component");
    else if (!Application.isPlaying)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component);
    else if ((double) delay > 0.0)
      UnityEngine.Object.Destroy((UnityEngine.Object) component, delay);
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) component);
  }

  public static void DestroyGO<T>(this T component, float delay = 0.0f) where T : MonoBehaviour
  {
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      Debug.LogError((object) "Can't destroy null component");
    else
      component.gameObject.Destroy(delay);
  }

  public static void SetXY(this Transform tf, Vector2 pos)
  {
    Vector3 vector3 = new Vector3(pos.x, pos.y, tf.position.z);
    tf.position = vector3;
  }

  public static GameObject GetFirstParent(this GameObject go)
  {
    GameObject firstParent = go;
    while ((UnityEngine.Object) firstParent.transform.parent != (UnityEngine.Object) null)
      firstParent = firstParent.transform.parent.gameObject;
    return firstParent;
  }

  public static void MakeStatic(this Rigidbody2D body)
  {
    if (body.bodyType == RigidbodyType2D.Static)
      return;
    body.bodyType = RigidbodyType2D.Static;
  }

  public static void MakeDynamic(this Rigidbody2D body)
  {
    if (body.bodyType == RigidbodyType2D.Dynamic)
      return;
    body.bodyType = RigidbodyType2D.Dynamic;
  }

  public static T RandomElement<T>(this List<T> list, bool remove_element = false)
  {
    if (list == null || list.Count == 0)
      return default (T);
    int index = UnityEngine.Random.Range(0, list.Count);
    T obj = list[index];
    if (!remove_element)
      return obj;
    list.RemoveAt(index);
    return obj;
  }

  public static T LastElement<T>(this List<T> list)
  {
    return list == null || list.Count == 0 ? default (T) : list[list.Count - 1];
  }

  public static string JoinToString<T>(this List<T> list, string separator = ",")
  {
    if (list == null || list.Count == 0)
      return "";
    StringBuilder stringBuilder = new StringBuilder();
    foreach (T obj in list)
    {
      if (stringBuilder.Length > 0)
        stringBuilder.Append(separator);
      stringBuilder.Append((object) obj);
    }
    return stringBuilder.ToString();
  }

  public static bool PolygonContainsPoint(Vector2[] polygon, Vector2 point)
  {
    int length = polygon.Length;
    int num1 = 0;
    bool flag = false;
    float x1 = point.x;
    float y1 = point.y;
    Vector2 vector2_1 = polygon[length - 1];
    float x2 = vector2_1.x;
    float y2 = vector2_1.y;
    while (num1 < length)
    {
      float num2 = x2;
      float num3 = y2;
      Vector2 vector2_2 = polygon[num1++];
      x2 = vector2_2.x;
      y2 = vector2_2.y;
      flag = ((flag ? 1 : 0) ^ (!((double) y2 > (double) y1 ^ (double) num3 > (double) y1) ? 0 : ((double) x1 - (double) x2 < ((double) y1 - (double) y2) * ((double) num2 - (double) x2) / ((double) num3 - (double) y2) ? 1 : 0))) != 0;
    }
    return flag;
  }

  public static void SetAlpha(this Color c, float a) => c.a = a;

  public static void SetX(this Vector3 v, float x) => v.x = x;

  public static void SetY(this Vector3 v, float y) => v.y = y;

  public static string ConcatWithSeparator(this string s, string ss, string separator = "\n")
  {
    string str = s;
    if (!string.IsNullOrEmpty(s))
      str += separator;
    return str + ss;
  }
}
