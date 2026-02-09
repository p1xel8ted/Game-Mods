// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.TinyJsonDeserializer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.WindowsStore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

#nullable disable
namespace Pathfinding.Serialization;

public class TinyJsonDeserializer
{
  public TextReader reader;
  public static NumberFormatInfo numberFormat = NumberFormatInfo.InvariantInfo;
  public StringBuilder builder = new StringBuilder();

  public static object Deserialize(string text, System.Type type, object populate = null)
  {
    return new TinyJsonDeserializer()
    {
      reader = ((TextReader) new StringReader(text))
    }.Deserialize(type, populate);
  }

  public object Deserialize(System.Type tp, object populate = null)
  {
    System.Type typeInfo = WindowsStoreCompatibility.GetTypeInfo(tp);
    if (typeInfo.IsEnum)
      return Enum.Parse(tp, this.EatField());
    if (this.TryEat('n'))
    {
      this.Eat("ull");
      return (object) null;
    }
    if (object.Equals((object) tp, (object) typeof (float)))
      return (object) float.Parse(this.EatField(), (IFormatProvider) TinyJsonDeserializer.numberFormat);
    if (object.Equals((object) tp, (object) typeof (int)))
      return (object) int.Parse(this.EatField(), (IFormatProvider) TinyJsonDeserializer.numberFormat);
    if (object.Equals((object) tp, (object) typeof (uint)))
      return (object) uint.Parse(this.EatField(), (IFormatProvider) TinyJsonDeserializer.numberFormat);
    if (object.Equals((object) tp, (object) typeof (bool)))
      return (object) bool.Parse(this.EatField());
    if (object.Equals((object) tp, (object) typeof (string)))
      return (object) this.EatField();
    if (object.Equals((object) tp, (object) typeof (Version)))
      return (object) new Version(this.EatField());
    if (object.Equals((object) tp, (object) typeof (Vector2)))
    {
      this.Eat("{");
      Vector2 vector2 = new Vector2();
      this.EatField();
      vector2.x = float.Parse(this.EatField(), (IFormatProvider) TinyJsonDeserializer.numberFormat);
      this.EatField();
      vector2.y = float.Parse(this.EatField(), (IFormatProvider) TinyJsonDeserializer.numberFormat);
      this.Eat("}");
      return (object) vector2;
    }
    if (object.Equals((object) tp, (object) typeof (Vector3)))
    {
      this.Eat("{");
      Vector3 vector3 = new Vector3();
      this.EatField();
      vector3.x = float.Parse(this.EatField(), (IFormatProvider) TinyJsonDeserializer.numberFormat);
      this.EatField();
      vector3.y = float.Parse(this.EatField(), (IFormatProvider) TinyJsonDeserializer.numberFormat);
      this.EatField();
      vector3.z = float.Parse(this.EatField(), (IFormatProvider) TinyJsonDeserializer.numberFormat);
      this.Eat("}");
      return (object) vector3;
    }
    if (object.Equals((object) tp, (object) typeof (Pathfinding.Util.Guid)))
    {
      this.Eat("{");
      this.EatField();
      Pathfinding.Util.Guid guid = Pathfinding.Util.Guid.Parse(this.EatField());
      this.Eat("}");
      return (object) guid;
    }
    if (object.Equals((object) tp, (object) typeof (LayerMask)))
    {
      this.Eat("{");
      this.EatField();
      LayerMask layerMask = (LayerMask) int.Parse(this.EatField());
      this.Eat("}");
      return (object) layerMask;
    }
    if (object.Equals((object) tp, (object) typeof (List<string>)))
    {
      IList list = (IList) new List<string>();
      this.Eat("[");
      while (!this.TryEat(']'))
        list.Add(this.Deserialize(typeof (string)));
      return (object) list;
    }
    if (typeInfo.IsArray)
    {
      List<object> objectList = new List<object>();
      this.Eat("[");
      while (!this.TryEat(']'))
        objectList.Add(this.Deserialize(tp.GetElementType()));
      Array instance = Array.CreateInstance(tp.GetElementType(), objectList.Count);
      objectList.ToArray().CopyTo(instance, 0);
      return (object) instance;
    }
    if (object.Equals((object) tp, (object) typeof (Mesh)) || object.Equals((object) tp, (object) typeof (Texture2D)) || object.Equals((object) tp, (object) typeof (Transform)) || object.Equals((object) tp, (object) typeof (GameObject)))
      return (object) this.DeserializeUnityObject();
    object obj = populate ?? Activator.CreateInstance(tp);
    this.Eat("{");
    while (!this.TryEat('}'))
    {
      string name = this.EatField();
      FieldInfo field = tp.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (FieldInfo.op_Equality(field, (FieldInfo) null))
        this.SkipFieldData();
      else
        field.SetValue(obj, this.Deserialize(field.FieldType));
      this.TryEat(',');
    }
    return obj;
  }

  public UnityEngine.Object DeserializeUnityObject()
  {
    this.Eat("{");
    UnityEngine.Object @object = this.DeserializeUnityObjectInner();
    this.Eat("}");
    return @object;
  }

  public UnityEngine.Object DeserializeUnityObjectInner()
  {
    if (this.EatField() != "Name")
      throw new Exception("Expected 'Name' field");
    string path = this.EatField();
    if (path == null)
      return (UnityEngine.Object) null;
    if (this.EatField() != "Type")
      throw new Exception("Expected 'Type' field");
    string name = this.EatField();
    if (name.IndexOf(',') != -1)
      name = name.Substring(0, name.IndexOf(','));
    System.Type type = WindowsStoreCompatibility.GetTypeInfo(typeof (AstarPath)).Assembly.GetType(name) ?? WindowsStoreCompatibility.GetTypeInfo(typeof (Transform)).Assembly.GetType(name);
    if (object.Equals((object) type, (object) null))
    {
      Debug.LogError((object) $"Could not find type '{name}'. Cannot deserialize Unity reference");
      return (UnityEngine.Object) null;
    }
    this.EatWhitespace();
    if ((ushort) this.reader.Peek() == (ushort) 34)
    {
      if (this.EatField() != "GUID")
        throw new Exception("Expected 'GUID' field");
      string str = this.EatField();
      foreach (UnityReferenceHelper unityReferenceHelper in UnityEngine.Object.FindObjectsOfType<UnityReferenceHelper>())
      {
        if (unityReferenceHelper.GetGUID() == str)
          return object.Equals((object) type, (object) typeof (GameObject)) ? (UnityEngine.Object) unityReferenceHelper.gameObject : (UnityEngine.Object) unityReferenceHelper.GetComponent(type);
      }
    }
    UnityEngine.Object[] objectArray = Resources.LoadAll(path, type);
    for (int index = 0; index < objectArray.Length; ++index)
    {
      if (objectArray[index].name == path || objectArray.Length == 1)
        return objectArray[index];
    }
    return (UnityEngine.Object) null;
  }

  public void EatWhitespace()
  {
    while (char.IsWhiteSpace((char) this.reader.Peek()))
      this.reader.Read();
  }

  public void Eat(string s)
  {
    this.EatWhitespace();
    for (int index = 0; index < s.Length; ++index)
    {
      char ch = (char) this.reader.Read();
      if ((int) ch != (int) s[index])
        throw new Exception($"Expected '{s[index].ToString()}' found '{ch.ToString()}'\n\n...{this.reader.ReadLine()}");
    }
  }

  public string EatUntil(string c, bool inString)
  {
    this.builder.Length = 0;
    bool flag = false;
    while (true)
    {
      int num = this.reader.Peek();
      if (!flag && (ushort) num == (ushort) 34)
        inString = !inString;
      char ch = (char) num;
      if (num != -1)
      {
        if (!flag && ch == '\\')
        {
          flag = true;
          this.reader.Read();
        }
        else if (inString || c.IndexOf(ch) == -1)
        {
          this.builder.Append(ch);
          this.reader.Read();
          flag = false;
        }
        else
          goto label_9;
      }
      else
        break;
    }
    throw new Exception("Unexpected EOF");
label_9:
    return this.builder.ToString();
  }

  public bool TryEat(char c)
  {
    this.EatWhitespace();
    if ((int) (ushort) this.reader.Peek() != (int) c)
      return false;
    this.reader.Read();
    return true;
  }

  public string EatField()
  {
    string str = this.EatUntil("\",}]", this.TryEat('"'));
    this.TryEat('"');
    this.TryEat(':');
    this.TryEat(',');
    return str;
  }

  public void SkipFieldData()
  {
    int num = 0;
    while (true)
    {
      this.EatUntil(",{}[]", false);
      switch ((char) this.reader.Peek())
      {
        case ',':
          if (num != 0)
            break;
          goto label_6;
        case '[':
        case '{':
          ++num;
          break;
        case ']':
        case '}':
          --num;
          if (num >= 0)
            break;
          goto label_2;
        default:
          goto label_7;
      }
      this.reader.Read();
    }
label_2:
    return;
label_6:
    this.reader.Read();
    return;
label_7:
    throw new Exception("Should not reach this part");
  }
}
