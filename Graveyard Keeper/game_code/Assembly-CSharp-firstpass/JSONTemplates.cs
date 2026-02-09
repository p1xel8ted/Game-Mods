// Decompiled with JetBrains decompiler
// Type: JSONTemplates
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
public static class JSONTemplates
{
  public static HashSet<object> touched = new HashSet<object>();

  public static JSONObject TOJSON(object obj)
  {
    if (JSONTemplates.touched.Add(obj))
    {
      JSONObject jsonObject1 = JSONObject.obj;
      foreach (FieldInfo field in obj.GetType().GetFields())
      {
        JSONObject jsonObject2 = JSONObject.nullJO;
        if (!field.GetValue(obj).Equals((object) null))
        {
          MethodInfo method = typeof (JSONTemplates).GetMethod("From" + field.FieldType.Name);
          if (MethodInfo.op_Inequality(method, (MethodInfo) null))
          {
            object[] parameters = new object[1]
            {
              field.GetValue(obj)
            };
            jsonObject2 = (JSONObject) method.Invoke((object) null, parameters);
          }
          else
            jsonObject2 = !System.Type.op_Equality(field.FieldType, typeof (string)) ? JSONObject.Create(field.GetValue(obj).ToString()) : JSONObject.CreateStringObject(field.GetValue(obj).ToString());
        }
        if ((bool) jsonObject2)
        {
          if (jsonObject2.type != JSONObject.Type.NULL)
            jsonObject1.AddField(field.Name, jsonObject2);
          else
            Debug.LogWarning((object) $"Null for this non-null object, property {field.Name} of class {obj.GetType().Name}. Object type is {field.FieldType.Name}");
        }
      }
      foreach (PropertyInfo property in obj.GetType().GetProperties())
      {
        JSONObject jsonObject3 = JSONObject.nullJO;
        if (!property.GetValue(obj, (object[]) null).Equals((object) null))
        {
          MethodInfo method = typeof (JSONTemplates).GetMethod("From" + property.PropertyType.Name);
          if (MethodInfo.op_Inequality(method, (MethodInfo) null))
          {
            object[] parameters = new object[1]
            {
              property.GetValue(obj, (object[]) null)
            };
            jsonObject3 = (JSONObject) method.Invoke((object) null, parameters);
          }
          else
            jsonObject3 = !System.Type.op_Equality(property.PropertyType, typeof (string)) ? JSONObject.Create(property.GetValue(obj, (object[]) null).ToString()) : JSONObject.CreateStringObject(property.GetValue(obj, (object[]) null).ToString());
        }
        if ((bool) jsonObject3)
        {
          if (jsonObject3.type != JSONObject.Type.NULL)
            jsonObject1.AddField(property.Name, jsonObject3);
          else
            Debug.LogWarning((object) $"Null for this non-null object, property {property.Name} of class {obj.GetType().Name}. Object type is {property.PropertyType.Name}");
        }
      }
      return jsonObject1;
    }
    Debug.LogWarning((object) "trying to save the same data twice");
    return JSONObject.nullJO;
  }

  public static Vector2 ToVector2(JSONObject obj)
  {
    return new Vector2((bool) obj["x"] ? obj["x"].f : 0.0f, (bool) obj["y"] ? obj["y"].f : 0.0f);
  }

  public static JSONObject FromVector2(Vector2 v)
  {
    JSONObject jsonObject = JSONObject.obj;
    if ((double) v.x != 0.0)
      jsonObject.AddField("x", v.x);
    if ((double) v.y != 0.0)
      jsonObject.AddField("y", v.y);
    return jsonObject;
  }

  public static JSONObject FromVector3(Vector3 v)
  {
    JSONObject jsonObject = JSONObject.obj;
    if ((double) v.x != 0.0)
      jsonObject.AddField("x", v.x);
    if ((double) v.y != 0.0)
      jsonObject.AddField("y", v.y);
    if ((double) v.z != 0.0)
      jsonObject.AddField("z", v.z);
    return jsonObject;
  }

  public static Vector3 ToVector3(JSONObject obj)
  {
    double x = (bool) obj["x"] ? (double) obj["x"].f : 0.0;
    float num1 = (bool) obj["y"] ? obj["y"].f : 0.0f;
    float num2 = (bool) obj["z"] ? obj["z"].f : 0.0f;
    double y = (double) num1;
    double z = (double) num2;
    return new Vector3((float) x, (float) y, (float) z);
  }

  public static JSONObject FromVector4(Vector4 v)
  {
    JSONObject jsonObject = JSONObject.obj;
    if ((double) v.x != 0.0)
      jsonObject.AddField("x", v.x);
    if ((double) v.y != 0.0)
      jsonObject.AddField("y", v.y);
    if ((double) v.z != 0.0)
      jsonObject.AddField("z", v.z);
    if ((double) v.w != 0.0)
      jsonObject.AddField("w", v.w);
    return jsonObject;
  }

  public static Vector4 ToVector4(JSONObject obj)
  {
    double x = (bool) obj["x"] ? (double) obj["x"].f : 0.0;
    float num1 = (bool) obj["y"] ? obj["y"].f : 0.0f;
    float num2 = (bool) obj["z"] ? obj["z"].f : 0.0f;
    float num3 = (bool) obj["w"] ? obj["w"].f : 0.0f;
    double y = (double) num1;
    double z = (double) num2;
    double w = (double) num3;
    return new Vector4((float) x, (float) y, (float) z, (float) w);
  }

  public static JSONObject FromMatrix4x4(Matrix4x4 m)
  {
    JSONObject jsonObject = JSONObject.obj;
    if ((double) m.m00 != 0.0)
      jsonObject.AddField("m00", m.m00);
    if ((double) m.m01 != 0.0)
      jsonObject.AddField("m01", m.m01);
    if ((double) m.m02 != 0.0)
      jsonObject.AddField("m02", m.m02);
    if ((double) m.m03 != 0.0)
      jsonObject.AddField("m03", m.m03);
    if ((double) m.m10 != 0.0)
      jsonObject.AddField("m10", m.m10);
    if ((double) m.m11 != 0.0)
      jsonObject.AddField("m11", m.m11);
    if ((double) m.m12 != 0.0)
      jsonObject.AddField("m12", m.m12);
    if ((double) m.m13 != 0.0)
      jsonObject.AddField("m13", m.m13);
    if ((double) m.m20 != 0.0)
      jsonObject.AddField("m20", m.m20);
    if ((double) m.m21 != 0.0)
      jsonObject.AddField("m21", m.m21);
    if ((double) m.m22 != 0.0)
      jsonObject.AddField("m22", m.m22);
    if ((double) m.m23 != 0.0)
      jsonObject.AddField("m23", m.m23);
    if ((double) m.m30 != 0.0)
      jsonObject.AddField("m30", m.m30);
    if ((double) m.m31 != 0.0)
      jsonObject.AddField("m31", m.m31);
    if ((double) m.m32 != 0.0)
      jsonObject.AddField("m32", m.m32);
    if ((double) m.m33 != 0.0)
      jsonObject.AddField("m33", m.m33);
    return jsonObject;
  }

  public static Matrix4x4 ToMatrix4x4(JSONObject obj)
  {
    Matrix4x4 matrix4x4 = new Matrix4x4();
    if ((bool) obj["m00"])
      matrix4x4.m00 = obj["m00"].f;
    if ((bool) obj["m01"])
      matrix4x4.m01 = obj["m01"].f;
    if ((bool) obj["m02"])
      matrix4x4.m02 = obj["m02"].f;
    if ((bool) obj["m03"])
      matrix4x4.m03 = obj["m03"].f;
    if ((bool) obj["m10"])
      matrix4x4.m10 = obj["m10"].f;
    if ((bool) obj["m11"])
      matrix4x4.m11 = obj["m11"].f;
    if ((bool) obj["m12"])
      matrix4x4.m12 = obj["m12"].f;
    if ((bool) obj["m13"])
      matrix4x4.m13 = obj["m13"].f;
    if ((bool) obj["m20"])
      matrix4x4.m20 = obj["m20"].f;
    if ((bool) obj["m21"])
      matrix4x4.m21 = obj["m21"].f;
    if ((bool) obj["m22"])
      matrix4x4.m22 = obj["m22"].f;
    if ((bool) obj["m23"])
      matrix4x4.m23 = obj["m23"].f;
    if ((bool) obj["m30"])
      matrix4x4.m30 = obj["m30"].f;
    if ((bool) obj["m31"])
      matrix4x4.m31 = obj["m31"].f;
    if ((bool) obj["m32"])
      matrix4x4.m32 = obj["m32"].f;
    if ((bool) obj["m33"])
      matrix4x4.m33 = obj["m33"].f;
    return matrix4x4;
  }

  public static JSONObject FromQuaternion(Quaternion q)
  {
    JSONObject jsonObject = JSONObject.obj;
    if ((double) q.w != 0.0)
      jsonObject.AddField("w", q.w);
    if ((double) q.x != 0.0)
      jsonObject.AddField("x", q.x);
    if ((double) q.y != 0.0)
      jsonObject.AddField("y", q.y);
    if ((double) q.z != 0.0)
      jsonObject.AddField("z", q.z);
    return jsonObject;
  }

  public static Quaternion ToQuaternion(JSONObject obj)
  {
    double x = (bool) obj["x"] ? (double) obj["x"].f : 0.0;
    float num1 = (bool) obj["y"] ? obj["y"].f : 0.0f;
    float num2 = (bool) obj["z"] ? obj["z"].f : 0.0f;
    float num3 = (bool) obj["w"] ? obj["w"].f : 0.0f;
    double y = (double) num1;
    double z = (double) num2;
    double w = (double) num3;
    return new Quaternion((float) x, (float) y, (float) z, (float) w);
  }

  public static JSONObject FromColor(Color c)
  {
    JSONObject jsonObject = JSONObject.obj;
    if ((double) c.r != 0.0)
      jsonObject.AddField("r", c.r);
    if ((double) c.g != 0.0)
      jsonObject.AddField("g", c.g);
    if ((double) c.b != 0.0)
      jsonObject.AddField("b", c.b);
    if ((double) c.a != 0.0)
      jsonObject.AddField("a", c.a);
    return jsonObject;
  }

  public static Color ToColor(JSONObject obj)
  {
    Color color = new Color();
    for (int index = 0; index < obj.Count; ++index)
    {
      switch (obj.keys[index])
      {
        case "r":
          color.r = obj[index].f;
          break;
        case "g":
          color.g = obj[index].f;
          break;
        case "b":
          color.b = obj[index].f;
          break;
        case "a":
          color.a = obj[index].f;
          break;
      }
    }
    return color;
  }

  public static JSONObject FromLayerMask(LayerMask l)
  {
    JSONObject jsonObject = JSONObject.obj;
    jsonObject.AddField("value", l.value);
    return jsonObject;
  }

  public static LayerMask ToLayerMask(JSONObject obj)
  {
    return new LayerMask() { value = (int) obj["value"].n };
  }

  public static JSONObject FromRect(Rect r)
  {
    JSONObject jsonObject = JSONObject.obj;
    if ((double) r.x != 0.0)
      jsonObject.AddField("x", r.x);
    if ((double) r.y != 0.0)
      jsonObject.AddField("y", r.y);
    if ((double) r.height != 0.0)
      jsonObject.AddField("height", r.height);
    if ((double) r.width != 0.0)
      jsonObject.AddField("width", r.width);
    return jsonObject;
  }

  public static Rect ToRect(JSONObject obj)
  {
    Rect rect = new Rect();
    for (int index = 0; index < obj.Count; ++index)
    {
      switch (obj.keys[index])
      {
        case "x":
          rect.x = obj[index].f;
          break;
        case "y":
          rect.y = obj[index].f;
          break;
        case "height":
          rect.height = obj[index].f;
          break;
        case "width":
          rect.width = obj[index].f;
          break;
      }
    }
    return rect;
  }

  public static JSONObject FromRectOffset(RectOffset r)
  {
    JSONObject jsonObject = JSONObject.obj;
    if (r.bottom != 0)
      jsonObject.AddField("bottom", r.bottom);
    if (r.left != 0)
      jsonObject.AddField("left", r.left);
    if (r.right != 0)
      jsonObject.AddField("right", r.right);
    if (r.top != 0)
      jsonObject.AddField("top", r.top);
    return jsonObject;
  }

  public static RectOffset ToRectOffset(JSONObject obj)
  {
    RectOffset rectOffset = new RectOffset();
    for (int index = 0; index < obj.Count; ++index)
    {
      switch (obj.keys[index])
      {
        case "bottom":
          rectOffset.bottom = (int) obj[index].n;
          break;
        case "left":
          rectOffset.left = (int) obj[index].n;
          break;
        case "right":
          rectOffset.right = (int) obj[index].n;
          break;
        case "top":
          rectOffset.top = (int) obj[index].n;
          break;
      }
    }
    return rectOffset;
  }

  public static AnimationCurve ToAnimationCurve(JSONObject obj)
  {
    AnimationCurve animationCurve = new AnimationCurve();
    if (obj.HasField("keys"))
    {
      JSONObject field = obj.GetField("keys");
      for (int index = 0; index < field.list.Count; ++index)
        animationCurve.AddKey(JSONTemplates.ToKeyframe(field[index]));
    }
    if (obj.HasField("preWrapMode"))
      animationCurve.preWrapMode = (WrapMode) obj.GetField("preWrapMode").n;
    if (obj.HasField("postWrapMode"))
      animationCurve.postWrapMode = (WrapMode) obj.GetField("postWrapMode").n;
    return animationCurve;
  }

  public static JSONObject FromAnimationCurve(AnimationCurve a)
  {
    JSONObject jsonObject1 = JSONObject.obj;
    jsonObject1.AddField("preWrapMode", a.preWrapMode.ToString());
    jsonObject1.AddField("postWrapMode", a.postWrapMode.ToString());
    if (a.keys.Length != 0)
    {
      JSONObject jsonObject2 = JSONObject.Create();
      for (int index = 0; index < a.keys.Length; ++index)
        jsonObject2.Add(JSONTemplates.FromKeyframe(a.keys[index]));
      jsonObject1.AddField("keys", jsonObject2);
    }
    return jsonObject1;
  }

  public static Keyframe ToKeyframe(JSONObject obj)
  {
    Keyframe keyframe = new Keyframe(obj.HasField("time") ? (float) obj.GetField("time").n : 0.0f, obj.HasField("value") ? (float) obj.GetField("value").n : 0.0f);
    if (obj.HasField("inTangent"))
      keyframe.inTangent = (float) obj.GetField("inTangent").n;
    if (obj.HasField("outTangent"))
      keyframe.outTangent = (float) obj.GetField("outTangent").n;
    if (obj.HasField("tangentMode"))
      keyframe.tangentMode = (int) obj.GetField("tangentMode").n;
    return keyframe;
  }

  public static JSONObject FromKeyframe(Keyframe k)
  {
    JSONObject jsonObject = JSONObject.obj;
    if ((double) k.inTangent != 0.0)
      jsonObject.AddField("inTangent", k.inTangent);
    if ((double) k.outTangent != 0.0)
      jsonObject.AddField("outTangent", k.outTangent);
    if (k.tangentMode != 0)
      jsonObject.AddField("tangentMode", k.tangentMode);
    if ((double) k.time != 0.0)
      jsonObject.AddField("time", k.time);
    if ((double) k.value != 0.0)
      jsonObject.AddField("value", k.value);
    return jsonObject;
  }
}
