// Decompiled with JetBrains decompiler
// Type: JSONObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using UnityEngine;

#nullable disable
public class JSONObject
{
  public const int MAX_DEPTH = 100;
  public const string INFINITY = "\"INFINITY\"";
  public const string NEGINFINITY = "\"NEGINFINITY\"";
  public const string NaN = "\"NaN\"";
  public static char[] WHITESPACE = new char[6]
  {
    ' ',
    '\r',
    '\n',
    '\t',
    '\uFEFF',
    '\t'
  };
  public JSONObject.Type type;
  public List<JSONObject> list;
  public List<string> keys;
  public string str;
  public double n;
  public bool b;
  public const float maxFrameTime = 0.008f;
  public static Stopwatch printWatch = new Stopwatch();

  public bool isContainer
  {
    get => this.type == JSONObject.Type.ARRAY || this.type == JSONObject.Type.OBJECT;
  }

  public int Count => this.list == null ? -1 : this.list.Count;

  public float f => (float) this.n;

  public int i => Mathf.RoundToInt(this.f);

  public static JSONObject nullJO => JSONObject.Create(JSONObject.Type.NULL);

  public static JSONObject obj => JSONObject.Create(JSONObject.Type.OBJECT);

  public static JSONObject arr => JSONObject.Create(JSONObject.Type.ARRAY);

  public JSONObject(JSONObject.Type t)
  {
    this.type = t;
    if (t != JSONObject.Type.OBJECT)
    {
      if (t != JSONObject.Type.ARRAY)
        return;
      this.list = new List<JSONObject>();
    }
    else
    {
      this.list = new List<JSONObject>();
      this.keys = new List<string>();
    }
  }

  public JSONObject(bool b)
  {
    this.type = JSONObject.Type.BOOL;
    this.b = b;
  }

  public JSONObject(double d)
  {
    this.type = JSONObject.Type.NUMBER;
    this.n = d;
  }

  public JSONObject(Dictionary<string, string> dic)
  {
    this.type = JSONObject.Type.OBJECT;
    this.keys = new List<string>();
    this.list = new List<JSONObject>();
    foreach (KeyValuePair<string, string> keyValuePair in dic)
    {
      this.keys.Add(keyValuePair.Key);
      this.list.Add(JSONObject.CreateStringObject(keyValuePair.Value));
    }
  }

  public JSONObject(Dictionary<string, JSONObject> dic)
  {
    this.type = JSONObject.Type.OBJECT;
    this.keys = new List<string>();
    this.list = new List<JSONObject>();
    foreach (KeyValuePair<string, JSONObject> keyValuePair in dic)
    {
      this.keys.Add(keyValuePair.Key);
      this.list.Add(keyValuePair.Value);
    }
  }

  public JSONObject(JSONObject.AddJSONConents content) => content(this);

  public JSONObject(JSONObject[] objs)
  {
    this.type = JSONObject.Type.ARRAY;
    this.list = new List<JSONObject>((IEnumerable<JSONObject>) objs);
  }

  public static JSONObject StringObject(string val) => JSONObject.CreateStringObject(val);

  public void Absorb(JSONObject obj)
  {
    this.list.AddRange((IEnumerable<JSONObject>) obj.list);
    this.keys.AddRange((IEnumerable<string>) obj.keys);
    this.str = obj.str;
    this.n = obj.n;
    this.b = obj.b;
    this.type = obj.type;
  }

  public static JSONObject Create() => new JSONObject();

  public static JSONObject Create(JSONObject.Type t)
  {
    JSONObject jsonObject = JSONObject.Create();
    jsonObject.type = t;
    switch (t)
    {
      case JSONObject.Type.OBJECT:
        jsonObject.list = new List<JSONObject>();
        jsonObject.keys = new List<string>();
        break;
      case JSONObject.Type.ARRAY:
        jsonObject.list = new List<JSONObject>();
        break;
    }
    return jsonObject;
  }

  public static JSONObject Create(bool val)
  {
    JSONObject jsonObject = JSONObject.Create();
    jsonObject.type = JSONObject.Type.BOOL;
    jsonObject.b = val;
    return jsonObject;
  }

  public static JSONObject Create(float val)
  {
    JSONObject jsonObject = JSONObject.Create();
    jsonObject.type = JSONObject.Type.NUMBER;
    jsonObject.n = (double) val;
    return jsonObject;
  }

  public static JSONObject Create(int val)
  {
    JSONObject jsonObject = JSONObject.Create();
    jsonObject.type = JSONObject.Type.NUMBER;
    jsonObject.n = (double) val;
    return jsonObject;
  }

  public static JSONObject CreateStringObject(string val)
  {
    JSONObject stringObject = JSONObject.Create();
    stringObject.type = JSONObject.Type.STRING;
    stringObject.str = val;
    return stringObject;
  }

  public static JSONObject CreateBakedObject(string val)
  {
    JSONObject bakedObject = JSONObject.Create();
    bakedObject.type = JSONObject.Type.BAKED;
    bakedObject.str = val;
    return bakedObject;
  }

  public static JSONObject Create(string val, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
  {
    JSONObject jsonObject = JSONObject.Create();
    jsonObject.Parse(val, maxDepth, storeExcessLevels, strict);
    return jsonObject;
  }

  public static JSONObject Create(JSONObject.AddJSONConents content)
  {
    JSONObject self = JSONObject.Create();
    content(self);
    return self;
  }

  public static JSONObject Create(Dictionary<string, string> dic)
  {
    JSONObject jsonObject = JSONObject.Create();
    jsonObject.type = JSONObject.Type.OBJECT;
    jsonObject.keys = new List<string>();
    jsonObject.list = new List<JSONObject>();
    foreach (KeyValuePair<string, string> keyValuePair in dic)
    {
      jsonObject.keys.Add(keyValuePair.Key);
      jsonObject.list.Add(JSONObject.CreateStringObject(keyValuePair.Value));
    }
    return jsonObject;
  }

  public JSONObject()
  {
  }

  public JSONObject(string str, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
  {
    this.Parse(str, maxDepth, storeExcessLevels, strict);
  }

  public void Parse(string str, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
  {
    if (!string.IsNullOrEmpty(str))
    {
      str = str.Trim(JSONObject.WHITESPACE);
      if (strict && str[0] != '[' && str[0] != '{')
      {
        this.type = JSONObject.Type.NULL;
        UnityEngine.Debug.LogWarning((object) "Improper (strict) JSON formatting.  First character must be [ or {");
      }
      else if (str.Length > 0)
      {
        if (string.Compare(str, "true", true) == 0)
        {
          this.type = JSONObject.Type.BOOL;
          this.b = true;
        }
        else if (string.Compare(str, "false", true) == 0)
        {
          this.type = JSONObject.Type.BOOL;
          this.b = false;
        }
        else if (string.Compare(str, "null", true) == 0)
        {
          this.type = JSONObject.Type.NULL;
        }
        else
        {
          switch (str)
          {
            case "\"INFINITY\"":
              this.type = JSONObject.Type.NUMBER;
              this.n = double.PositiveInfinity;
              break;
            case "\"NEGINFINITY\"":
              this.type = JSONObject.Type.NUMBER;
              this.n = double.NegativeInfinity;
              break;
            case "\"NaN\"":
              this.type = JSONObject.Type.NUMBER;
              this.n = double.NaN;
              break;
            default:
              if (str[0] == '"')
              {
                this.type = JSONObject.Type.STRING;
                this.str = str.Substring(1, str.Length - 2);
                break;
              }
              int startIndex = 1;
              int index = 0;
              switch (str[index])
              {
                case '[':
                  this.type = JSONObject.Type.ARRAY;
                  this.list = new List<JSONObject>();
                  break;
                case '{':
                  this.type = JSONObject.Type.OBJECT;
                  this.keys = new List<string>();
                  this.list = new List<JSONObject>();
                  break;
                default:
                  try
                  {
                    this.n = Convert.ToDouble(str, (IFormatProvider) CultureInfo.InvariantCulture);
                    this.type = JSONObject.Type.NUMBER;
                    return;
                  }
                  catch (FormatException ex)
                  {
                    this.type = JSONObject.Type.NULL;
                    UnityEngine.Debug.LogWarning((object) ("improper JSON formatting:" + str));
                    return;
                  }
              }
              string str1 = "";
              bool flag1 = false;
              bool flag2 = false;
              int num = 0;
              while (++index < str.Length)
              {
                if (Array.IndexOf<char>(JSONObject.WHITESPACE, str[index]) <= -1)
                {
                  if (str[index] == '\\')
                  {
                    ++index;
                  }
                  else
                  {
                    if (str[index] == '"')
                    {
                      if (flag1)
                      {
                        if (!flag2 && num == 0 && this.type == JSONObject.Type.OBJECT)
                          str1 = str.Substring(startIndex + 1, index - startIndex - 1);
                        flag1 = false;
                      }
                      else
                      {
                        if (num == 0 && this.type == JSONObject.Type.OBJECT)
                          startIndex = index;
                        flag1 = true;
                      }
                    }
                    if (!flag1)
                    {
                      if (this.type == JSONObject.Type.OBJECT && num == 0 && str[index] == ':')
                      {
                        startIndex = index + 1;
                        flag2 = true;
                      }
                      if (str[index] == '[' || str[index] == '{')
                        ++num;
                      else if (str[index] == ']' || str[index] == '}')
                        --num;
                      if (str[index] == ',' && num == 0 || num < 0)
                      {
                        flag2 = false;
                        string val = str.Substring(startIndex, index - startIndex).Trim(JSONObject.WHITESPACE);
                        if (val.Length > 0)
                        {
                          if (this.type == JSONObject.Type.OBJECT)
                            this.keys.Add(str1);
                          if (maxDepth != -1)
                            this.list.Add(JSONObject.Create(val, maxDepth < -1 ? -2 : maxDepth - 1));
                          else if (storeExcessLevels)
                            this.list.Add(JSONObject.CreateBakedObject(val));
                        }
                        startIndex = index + 1;
                      }
                    }
                  }
                }
              }
              break;
          }
        }
      }
      else
        this.type = JSONObject.Type.NULL;
    }
    else
      this.type = JSONObject.Type.NULL;
  }

  public bool IsNumber => this.type == JSONObject.Type.NUMBER;

  public bool IsNull => this.type == JSONObject.Type.NULL;

  public bool IsString => this.type == JSONObject.Type.STRING;

  public bool IsBool => this.type == JSONObject.Type.BOOL;

  public bool IsArray => this.type == JSONObject.Type.ARRAY;

  public bool IsObject => this.type == JSONObject.Type.OBJECT || this.type == JSONObject.Type.BAKED;

  public void Add(bool val) => this.Add(JSONObject.Create(val));

  public void Add(float val) => this.Add(JSONObject.Create(val));

  public void Add(int val) => this.Add(JSONObject.Create(val));

  public void Add(string str) => this.Add(JSONObject.CreateStringObject(str));

  public void Add(JSONObject.AddJSONConents content) => this.Add(JSONObject.Create(content));

  public void Add(JSONObject obj)
  {
    if (!(bool) obj)
      return;
    if (this.type != JSONObject.Type.ARRAY)
    {
      this.type = JSONObject.Type.ARRAY;
      if (this.list == null)
        this.list = new List<JSONObject>();
    }
    this.list.Add(obj);
  }

  public void AddField(string name, bool val) => this.AddField(name, JSONObject.Create(val));

  public void AddField(string name, float val) => this.AddField(name, JSONObject.Create(val));

  public void AddField(string name, int val) => this.AddField(name, JSONObject.Create(val));

  public void AddField(string name, JSONObject.AddJSONConents content)
  {
    this.AddField(name, JSONObject.Create(content));
  }

  public void AddField(string name, string val)
  {
    this.AddField(name, JSONObject.CreateStringObject(val));
  }

  public void AddField(string name, JSONObject obj)
  {
    if (!(bool) obj)
      return;
    if (this.type != JSONObject.Type.OBJECT)
    {
      if (this.keys == null)
        this.keys = new List<string>();
      if (this.type == JSONObject.Type.ARRAY)
      {
        for (int index = 0; index < this.list.Count; ++index)
          this.keys.Add(index.ToString() ?? "");
      }
      else if (this.list == null)
        this.list = new List<JSONObject>();
      this.type = JSONObject.Type.OBJECT;
    }
    this.keys.Add(name);
    this.list.Add(obj);
  }

  public void SetField(string name, string val)
  {
    this.SetField(name, JSONObject.CreateStringObject(val));
  }

  public void SetField(string name, bool val) => this.SetField(name, JSONObject.Create(val));

  public void SetField(string name, float val) => this.SetField(name, JSONObject.Create(val));

  public void SetField(string name, int val) => this.SetField(name, JSONObject.Create(val));

  public void SetField(string name, JSONObject obj)
  {
    if (this.HasField(name))
    {
      this.list.Remove(this[name]);
      this.keys.Remove(name);
    }
    this.AddField(name, obj);
  }

  public void RemoveField(string name)
  {
    if (this.keys.IndexOf(name) <= -1)
      return;
    this.list.RemoveAt(this.keys.IndexOf(name));
    this.keys.Remove(name);
  }

  public bool GetField(ref bool field, string name, bool fallback)
  {
    if (this.GetField(ref field, name))
      return true;
    field = fallback;
    return false;
  }

  public bool GetField(ref bool field, string name, JSONObject.FieldNotFound fail = null)
  {
    if (this.type == JSONObject.Type.OBJECT)
    {
      int index = this.keys.IndexOf(name);
      if (index >= 0)
      {
        field = this.list[index].b;
        return true;
      }
    }
    if (fail != null)
      fail(name);
    return false;
  }

  public bool GetField(ref double field, string name, double fallback)
  {
    if (this.GetField(ref field, name))
      return true;
    field = fallback;
    return false;
  }

  public bool GetField(ref double field, string name, JSONObject.FieldNotFound fail = null)
  {
    if (this.type == JSONObject.Type.OBJECT)
    {
      int index = this.keys.IndexOf(name);
      if (index >= 0)
      {
        field = this.list[index].n;
        return true;
      }
    }
    if (fail != null)
      fail(name);
    return false;
  }

  public bool GetField(ref int field, string name, int fallback)
  {
    if (this.GetField(ref field, name))
      return true;
    field = fallback;
    return false;
  }

  public bool GetField(ref int field, string name, JSONObject.FieldNotFound fail = null)
  {
    if (this.IsObject)
    {
      int index = this.keys.IndexOf(name);
      if (index >= 0)
      {
        field = (int) this.list[index].n;
        return true;
      }
    }
    if (fail != null)
      fail(name);
    return false;
  }

  public bool GetField(ref uint field, string name, uint fallback)
  {
    if (this.GetField(ref field, name))
      return true;
    field = fallback;
    return false;
  }

  public bool GetField(ref uint field, string name, JSONObject.FieldNotFound fail = null)
  {
    if (this.IsObject)
    {
      int index = this.keys.IndexOf(name);
      if (index >= 0)
      {
        field = (uint) this.list[index].n;
        return true;
      }
    }
    if (fail != null)
      fail(name);
    return false;
  }

  public bool GetField(ref string field, string name, string fallback)
  {
    if (this.GetField(ref field, name))
      return true;
    field = fallback;
    return false;
  }

  public bool GetField(ref string field, string name, JSONObject.FieldNotFound fail = null)
  {
    if (this.IsObject)
    {
      int index = this.keys.IndexOf(name);
      if (index >= 0)
      {
        field = this.list[index].str;
        return true;
      }
    }
    if (fail != null)
      fail(name);
    return false;
  }

  public void GetField(
    string name,
    JSONObject.GetFieldResponse response,
    JSONObject.FieldNotFound fail = null)
  {
    if (response != null && this.IsObject)
    {
      int index = this.keys.IndexOf(name);
      if (index >= 0)
      {
        response(this.list[index]);
        return;
      }
    }
    if (fail == null)
      return;
    fail(name);
  }

  public JSONObject GetField(string name)
  {
    if (this.IsObject)
    {
      for (int index = 0; index < this.keys.Count; ++index)
      {
        if (this.keys[index] == name)
          return this.list[index];
      }
    }
    return (JSONObject) null;
  }

  public bool HasFields(string[] names)
  {
    if (!this.IsObject)
      return false;
    for (int index = 0; index < names.Length; ++index)
    {
      if (!this.keys.Contains(names[index]))
        return false;
    }
    return true;
  }

  public bool HasField(string name)
  {
    if (!this.IsObject)
      return false;
    for (int index = 0; index < this.keys.Count; ++index)
    {
      if (this.keys[index] == name)
        return true;
    }
    return false;
  }

  public void Clear()
  {
    this.type = JSONObject.Type.NULL;
    if (this.list != null)
      this.list.Clear();
    if (this.keys != null)
      this.keys.Clear();
    this.str = "";
    this.n = 0.0;
    this.b = false;
  }

  public JSONObject Copy() => JSONObject.Create(this.Print());

  public void Merge(JSONObject obj) => JSONObject.MergeRecur(this, obj);

  public static void MergeRecur(JSONObject left, JSONObject right)
  {
    if (left.type == JSONObject.Type.NULL)
      left.Absorb(right);
    else if (left.type == JSONObject.Type.OBJECT && right.type == JSONObject.Type.OBJECT)
    {
      for (int index = 0; index < right.list.Count; ++index)
      {
        string key = right.keys[index];
        if (right[index].isContainer)
        {
          if (left.HasField(key))
            JSONObject.MergeRecur(left[key], right[index]);
          else
            left.AddField(key, right[index]);
        }
        else if (left.HasField(key))
          left.SetField(key, right[index]);
        else
          left.AddField(key, right[index]);
      }
    }
    else
    {
      if (left.type != JSONObject.Type.ARRAY || right.type != JSONObject.Type.ARRAY)
        return;
      if (right.Count > left.Count)
      {
        UnityEngine.Debug.LogError((object) "Cannot merge arrays when right object has more elements");
      }
      else
      {
        for (int index = 0; index < right.list.Count; ++index)
        {
          if (left[index].type == right[index].type)
          {
            if (left[index].isContainer)
              JSONObject.MergeRecur(left[index], right[index]);
            else
              left[index] = right[index];
          }
        }
      }
    }
  }

  public void Bake()
  {
    if (this.type == JSONObject.Type.BAKED)
      return;
    this.str = this.Print();
    this.type = JSONObject.Type.BAKED;
  }

  public IEnumerable BakeAsync()
  {
    if (this.type != JSONObject.Type.BAKED)
    {
      foreach (string str in this.PrintAsync())
      {
        if (str == null)
          yield return (object) str;
        else
          this.str = str;
      }
      this.type = JSONObject.Type.BAKED;
    }
  }

  public string Print(bool pretty = false)
  {
    StringBuilder builder = new StringBuilder();
    this.Stringify(0, builder, pretty);
    return builder.ToString();
  }

  public IEnumerable<string> PrintAsync(bool pretty = false)
  {
    StringBuilder builder = new StringBuilder();
    JSONObject.printWatch.Reset();
    JSONObject.printWatch.Start();
    foreach (IEnumerable enumerable in this.StringifyAsync(0, builder, pretty))
      yield return (string) null;
    yield return builder.ToString();
  }

  public IEnumerable StringifyAsync(int depth, StringBuilder builder, bool pretty = false)
  {
    if (depth++ > 100)
    {
      UnityEngine.Debug.Log((object) "reached max depth!");
    }
    else
    {
      if (JSONObject.printWatch.Elapsed.TotalSeconds > 0.00800000037997961)
      {
        JSONObject.printWatch.Reset();
        yield return (object) null;
        JSONObject.printWatch.Start();
      }
      int i;
      switch (this.type)
      {
        case JSONObject.Type.NULL:
          builder.Append("null");
          break;
        case JSONObject.Type.STRING:
          builder.AppendFormat("\"{0}\"", (object) this.str);
          break;
        case JSONObject.Type.NUMBER:
          if (double.IsInfinity(this.n))
          {
            builder.Append("\"INFINITY\"");
            break;
          }
          if (double.IsNegativeInfinity(this.n))
          {
            builder.Append("\"NEGINFINITY\"");
            break;
          }
          if (double.IsNaN(this.n))
          {
            builder.Append("\"NaN\"");
            break;
          }
          builder.Append(this.n.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JSONObject.Type.OBJECT:
          builder.Append("{");
          if (this.list.Count > 0)
          {
            if (pretty)
              builder.Append("\n");
            for (i = 0; i < this.list.Count; ++i)
            {
              string key = this.keys[i];
              JSONObject jsonObject = this.list[i];
              if ((bool) jsonObject)
              {
                if (pretty)
                {
                  for (int index = 0; index < depth; ++index)
                    builder.Append("\t");
                }
                builder.AppendFormat("\"{0}\":", (object) key);
                foreach (IEnumerable enumerable in jsonObject.StringifyAsync(depth, builder, pretty))
                  yield return (object) enumerable;
                builder.Append(",");
                if (pretty)
                  builder.Append("\n");
              }
            }
            if (pretty)
              builder.Length -= 2;
            else
              --builder.Length;
          }
          if (pretty && this.list.Count > 0)
          {
            builder.Append("\n");
            for (int index = 0; index < depth - 1; ++index)
              builder.Append("\t");
          }
          builder.Append("}");
          break;
        case JSONObject.Type.ARRAY:
          builder.Append("[");
          if (this.list.Count > 0)
          {
            if (pretty)
              builder.Append("\n");
            for (i = 0; i < this.list.Count; ++i)
            {
              if ((bool) this.list[i])
              {
                if (pretty)
                {
                  for (int index = 0; index < depth; ++index)
                    builder.Append("\t");
                }
                foreach (IEnumerable enumerable in this.list[i].StringifyAsync(depth, builder, pretty))
                  yield return (object) enumerable;
                builder.Append(",");
                if (pretty)
                  builder.Append("\n");
              }
            }
            if (pretty)
              builder.Length -= 2;
            else
              --builder.Length;
          }
          if (pretty && this.list.Count > 0)
          {
            builder.Append("\n");
            for (int index = 0; index < depth - 1; ++index)
              builder.Append("\t");
          }
          builder.Append("]");
          break;
        case JSONObject.Type.BOOL:
          if (this.b)
          {
            builder.Append("true");
            break;
          }
          builder.Append("false");
          break;
        case JSONObject.Type.BAKED:
          builder.Append(this.str);
          break;
      }
    }
  }

  public void Stringify(int depth, StringBuilder builder, bool pretty = false)
  {
    if (depth++ > 100)
    {
      UnityEngine.Debug.Log((object) "reached max depth!");
    }
    else
    {
      switch (this.type)
      {
        case JSONObject.Type.NULL:
          builder.Append("null");
          break;
        case JSONObject.Type.STRING:
          builder.AppendFormat("\"{0}\"", (object) this.str);
          break;
        case JSONObject.Type.NUMBER:
          if (double.IsInfinity(this.n))
          {
            builder.Append("\"INFINITY\"");
            break;
          }
          if (double.IsNegativeInfinity(this.n))
          {
            builder.Append("\"NEGINFINITY\"");
            break;
          }
          if (double.IsNaN(this.n))
          {
            builder.Append("\"NaN\"");
            break;
          }
          builder.Append(this.n.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JSONObject.Type.OBJECT:
          builder.Append("{");
          if (this.list.Count > 0)
          {
            if (pretty)
              builder.Append("\n");
            for (int index1 = 0; index1 < this.list.Count; ++index1)
            {
              string key = this.keys[index1];
              JSONObject jsonObject = this.list[index1];
              if ((bool) jsonObject)
              {
                if (pretty)
                {
                  for (int index2 = 0; index2 < depth; ++index2)
                    builder.Append("\t");
                }
                builder.AppendFormat("\"{0}\":", (object) key);
                jsonObject.Stringify(depth, builder, pretty);
                builder.Append(",");
                if (pretty)
                  builder.Append("\n");
              }
            }
            if (pretty)
              builder.Length -= 2;
            else
              --builder.Length;
          }
          if (pretty && this.list.Count > 0)
          {
            builder.Append("\n");
            for (int index = 0; index < depth - 1; ++index)
              builder.Append("\t");
          }
          builder.Append("}");
          break;
        case JSONObject.Type.ARRAY:
          builder.Append("[");
          if (this.list.Count > 0)
          {
            if (pretty)
              builder.Append("\n");
            for (int index3 = 0; index3 < this.list.Count; ++index3)
            {
              if ((bool) this.list[index3])
              {
                if (pretty)
                {
                  for (int index4 = 0; index4 < depth; ++index4)
                    builder.Append("\t");
                }
                this.list[index3].Stringify(depth, builder, pretty);
                builder.Append(",");
                if (pretty)
                  builder.Append("\n");
              }
            }
            if (pretty)
              builder.Length -= 2;
            else
              --builder.Length;
          }
          if (pretty && this.list.Count > 0)
          {
            builder.Append("\n");
            for (int index = 0; index < depth - 1; ++index)
              builder.Append("\t");
          }
          builder.Append("]");
          break;
        case JSONObject.Type.BOOL:
          if (this.b)
          {
            builder.Append("true");
            break;
          }
          builder.Append("false");
          break;
        case JSONObject.Type.BAKED:
          builder.Append(this.str);
          break;
      }
    }
  }

  public static implicit operator WWWForm(JSONObject obj)
  {
    WWWForm wwwForm = new WWWForm();
    for (int index = 0; index < obj.list.Count; ++index)
    {
      string fieldName = index.ToString() ?? "";
      if (obj.type == JSONObject.Type.OBJECT)
        fieldName = obj.keys[index];
      string str = obj.list[index].ToString();
      if (obj.list[index].type == JSONObject.Type.STRING)
        str = str.Replace("\"", "");
      wwwForm.AddField(fieldName, str);
    }
    return wwwForm;
  }

  public JSONObject this[int index]
  {
    get => this.list.Count > index ? this.list[index] : (JSONObject) null;
    set
    {
      if (this.list.Count <= index)
        return;
      this.list[index] = value;
    }
  }

  public JSONObject this[string index]
  {
    get => this.GetField(index);
    set => this.SetField(index, value);
  }

  public override string ToString() => this.Print();

  public string ToString(bool pretty) => this.Print(pretty);

  public Dictionary<string, string> ToDictionary()
  {
    if (this.type == JSONObject.Type.OBJECT)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      for (int index = 0; index < this.list.Count; ++index)
      {
        JSONObject jsonObject = this.list[index];
        switch (jsonObject.type)
        {
          case JSONObject.Type.STRING:
            dictionary.Add(this.keys[index], jsonObject.str);
            break;
          case JSONObject.Type.NUMBER:
            dictionary.Add(this.keys[index], jsonObject.n.ToString() ?? "");
            break;
          case JSONObject.Type.BOOL:
            dictionary.Add(this.keys[index], jsonObject.b.ToString() ?? "");
            break;
          default:
            UnityEngine.Debug.LogWarning((object) $"Omitting object: {this.keys[index]} in dictionary conversion");
            break;
        }
      }
      return dictionary;
    }
    UnityEngine.Debug.LogWarning((object) "Tried to turn non-Object JSONObject into a dictionary");
    return (Dictionary<string, string>) null;
  }

  public static implicit operator bool(JSONObject o) => o != null;

  public enum Type
  {
    NULL,
    STRING,
    NUMBER,
    OBJECT,
    ARRAY,
    BOOL,
    BAKED,
  }

  public delegate void AddJSONConents(JSONObject self);

  public delegate void FieldNotFound(string name);

  public delegate void GetFieldResponse(JSONObject obj);
}
