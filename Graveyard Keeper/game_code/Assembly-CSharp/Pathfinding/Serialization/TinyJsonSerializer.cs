// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.TinyJsonSerializer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.WindowsStore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

#nullable disable
namespace Pathfinding.Serialization;

public class TinyJsonSerializer
{
  public StringBuilder output = new StringBuilder();
  public Dictionary<System.Type, Action<object>> serializers = new Dictionary<System.Type, Action<object>>();
  public static CultureInfo invariantCulture = CultureInfo.InvariantCulture;

  public static void Serialize(object obj, StringBuilder output)
  {
    new TinyJsonSerializer() { output = output }.Serialize(obj);
  }

  public TinyJsonSerializer()
  {
    this.serializers[typeof (float)] = (Action<object>) (v => this.output.Append(((float) v).ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture)));
    this.serializers[typeof (Version)] = this.serializers[typeof (bool)] = this.serializers[typeof (uint)] = this.serializers[typeof (int)] = (Action<object>) (v => this.output.Append(v.ToString()));
    this.serializers[typeof (string)] = (Action<object>) (v => this.output.AppendFormat("\"{0}\"", v));
    this.serializers[typeof (Vector2)] = (Action<object>) (v =>
    {
      StringBuilder output = this.output;
      Vector2 vector2 = (Vector2) v;
      string str1 = vector2.x.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
      vector2 = (Vector2) v;
      string str2 = vector2.y.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
      output.AppendFormat("{{ \"x\": {0}, \"y\": {1} }}", (object) str1, (object) str2);
    });
    this.serializers[typeof (Vector3)] = (Action<object>) (v =>
    {
      StringBuilder output = this.output;
      Vector3 vector3 = (Vector3) v;
      string str3 = vector3.x.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
      vector3 = (Vector3) v;
      string str4 = vector3.y.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
      vector3 = (Vector3) v;
      string str5 = vector3.z.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
      output.AppendFormat("{{ \"x\": {0}, \"y\": {1}, \"z\": {2} }}", (object) str3, (object) str4, (object) str5);
    });
    this.serializers[typeof (Pathfinding.Util.Guid)] = (Action<object>) (v => this.output.AppendFormat("{{ \"value\": \"{0}\" }}", (object) v.ToString()));
    this.serializers[typeof (LayerMask)] = (Action<object>) (v => this.output.AppendFormat("{{ \"value\": {0} }}", (object) ((int) (LayerMask) v).ToString()));
  }

  public void Serialize(object obj)
  {
    if (obj == null)
    {
      this.output.Append("null");
    }
    else
    {
      System.Type type = obj.GetType();
      System.Type typeInfo = WindowsStoreCompatibility.GetTypeInfo(type);
      if (this.serializers.ContainsKey(type))
        this.serializers[type](obj);
      else if (typeInfo.IsEnum)
        this.output.Append($"\"{obj.ToString()}\"");
      else if (obj is IList)
      {
        this.output.Append("[");
        IList list = obj as IList;
        for (int index = 0; index < list.Count; ++index)
        {
          if (index != 0)
            this.output.Append(", ");
          this.Serialize(list[index]);
        }
        this.output.Append("]");
      }
      else if ((object) (obj as UnityEngine.Object) != null)
      {
        this.SerializeUnityObject(obj as UnityEngine.Object);
      }
      else
      {
        bool flag1 = typeInfo.GetCustomAttributes(typeof (JsonOptInAttribute), true).Length != 0;
        this.output.Append("{");
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        bool flag2 = false;
        foreach (FieldInfo fieldInfo in fields)
        {
          if (!flag1 && fieldInfo.IsPublic || fieldInfo.GetCustomAttributes(typeof (JsonMemberAttribute), true).Length != 0)
          {
            if (flag2)
              this.output.Append(", ");
            flag2 = true;
            this.output.AppendFormat("\"{0}\": ", (object) fieldInfo.Name);
            this.Serialize(fieldInfo.GetValue(obj));
          }
        }
        this.output.Append("}");
      }
    }
  }

  public void QuotedField(string name, string contents)
  {
    this.output.AppendFormat("\"{0}\": \"{1}\"", (object) name, (object) contents);
  }

  public void SerializeUnityObject(UnityEngine.Object obj)
  {
    if (obj == (UnityEngine.Object) null)
    {
      this.Serialize((object) null);
    }
    else
    {
      this.output.Append("{");
      this.QuotedField("Name", obj.name);
      this.output.Append(", ");
      this.QuotedField("Type", obj.GetType().FullName);
      Component component = obj as Component;
      GameObject gameObject = obj as GameObject;
      if ((UnityEngine.Object) component != (UnityEngine.Object) null || (UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) gameObject == (UnityEngine.Object) null)
          gameObject = component.gameObject;
        UnityReferenceHelper unityReferenceHelper = gameObject.GetComponent<UnityReferenceHelper>();
        if ((UnityEngine.Object) unityReferenceHelper == (UnityEngine.Object) null)
        {
          Debug.Log((object) $"Adding UnityReferenceHelper to Unity Reference '{obj.name}'");
          unityReferenceHelper = gameObject.AddComponent<UnityReferenceHelper>();
        }
        unityReferenceHelper.Reset();
        this.output.Append(", ");
        this.QuotedField("GUID", unityReferenceHelper.GetGUID().ToString());
      }
      this.output.Append("}");
    }
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__4_0(object v)
  {
    this.output.Append(((float) v).ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture));
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__4_6(object v) => this.output.Append(v.ToString());

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__4_1(object v) => this.output.AppendFormat("\"{0}\"", v);

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__4_2(object v)
  {
    StringBuilder output = this.output;
    Vector2 vector2 = (Vector2) v;
    string str1 = vector2.x.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
    vector2 = (Vector2) v;
    string str2 = vector2.y.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
    output.AppendFormat("{{ \"x\": {0}, \"y\": {1} }}", (object) str1, (object) str2);
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__4_3(object v)
  {
    StringBuilder output = this.output;
    Vector3 vector3 = (Vector3) v;
    string str1 = vector3.x.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
    vector3 = (Vector3) v;
    string str2 = vector3.y.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
    vector3 = (Vector3) v;
    string str3 = vector3.z.ToString("R", (IFormatProvider) TinyJsonSerializer.invariantCulture);
    output.AppendFormat("{{ \"x\": {0}, \"y\": {1}, \"z\": {2} }}", (object) str1, (object) str2, (object) str3);
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__4_4(object v)
  {
    this.output.AppendFormat("{{ \"value\": \"{0}\" }}", (object) v.ToString());
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__4_5(object v)
  {
    this.output.AppendFormat("{{ \"value\": {0} }}", (object) ((int) (LayerMask) v).ToString());
  }
}
