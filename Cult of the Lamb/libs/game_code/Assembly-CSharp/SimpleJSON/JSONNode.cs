// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace SimpleJSON;

public abstract class JSONNode
{
  public static StringBuilder m_EscapeBuilder = new StringBuilder();

  public virtual JSONNode this[int aIndex]
  {
    get => (JSONNode) null;
    set
    {
    }
  }

  public virtual JSONNode this[string aKey]
  {
    get => (JSONNode) null;
    set
    {
    }
  }

  public virtual string Value
  {
    get => "";
    set
    {
    }
  }

  public virtual int Count => 0;

  public virtual bool IsNumber => false;

  public virtual bool IsString => false;

  public virtual bool IsBoolean => false;

  public virtual bool IsNull => false;

  public virtual bool IsArray => false;

  public virtual bool IsObject => false;

  public virtual void Add(string aKey, JSONNode aItem)
  {
  }

  public virtual void Add(JSONNode aItem) => this.Add("", aItem);

  public virtual JSONNode Remove(string aKey) => (JSONNode) null;

  public virtual JSONNode Remove(int aIndex) => (JSONNode) null;

  public virtual JSONNode Remove(JSONNode aNode) => aNode;

  public virtual IEnumerable<JSONNode> Children
  {
    get
    {
      yield break;
    }
  }

  public IEnumerable<JSONNode> DeepChildren
  {
    get
    {
      foreach (JSONNode child in this.Children)
      {
        foreach (JSONNode deepChild in child.DeepChildren)
          yield return deepChild;
      }
    }
  }

  public override string ToString()
  {
    StringBuilder aSB = new StringBuilder();
    this.WriteToStringBuilder(aSB, 0, 0, JSONTextMode.Compact);
    return aSB.ToString();
  }

  public virtual string ToString(int aIndent)
  {
    StringBuilder aSB = new StringBuilder();
    this.WriteToStringBuilder(aSB, 0, aIndent, JSONTextMode.Indent);
    return aSB.ToString();
  }

  public abstract void WriteToStringBuilder(
    StringBuilder aSB,
    int aIndent,
    int aIndentInc,
    JSONTextMode aMode);

  public abstract JSONNodeType Tag { get; }

  public virtual double AsDouble
  {
    get
    {
      double result = 0.0;
      return double.TryParse(this.Value, out result) ? result : 0.0;
    }
    set => this.Value = value.ToString();
  }

  public virtual int AsInt
  {
    get => (int) this.AsDouble;
    set => this.AsDouble = (double) value;
  }

  public virtual float AsFloat
  {
    get => (float) this.AsDouble;
    set => this.AsDouble = (double) value;
  }

  public virtual bool AsBool
  {
    get
    {
      bool result = false;
      return bool.TryParse(this.Value, out result) ? result : !string.IsNullOrEmpty(this.Value);
    }
    set => this.Value = value ? "true" : "false";
  }

  public virtual JSONArray AsArray => this as JSONArray;

  public virtual JSONObject AsObject => this as JSONObject;

  public static implicit operator JSONNode(string s) => (JSONNode) new JSONString(s);

  public static implicit operator string(JSONNode d)
  {
    return !(d == (object) null) ? d.Value : (string) null;
  }

  public static implicit operator JSONNode(double n) => (JSONNode) new JSONNumber(n);

  public static implicit operator double(JSONNode d) => !(d == (object) null) ? d.AsDouble : 0.0;

  public static implicit operator JSONNode(float n) => (JSONNode) new JSONNumber((double) n);

  public static implicit operator float(JSONNode d) => !(d == (object) null) ? d.AsFloat : 0.0f;

  public static implicit operator JSONNode(int n) => (JSONNode) new JSONNumber((double) n);

  public static implicit operator int(JSONNode d) => !(d == (object) null) ? d.AsInt : 0;

  public static implicit operator JSONNode(bool b) => (JSONNode) new JSONBool(b);

  public static implicit operator bool(JSONNode d) => !(d == (object) null) && d.AsBool;

  public static bool operator ==(JSONNode a, object b)
  {
    if ((object) a == b)
      return true;
    int num1 = a is JSONNull || (object) a == null ? 1 : (a is JSONLazyCreator ? 1 : 0);
    int num2;
    switch (b)
    {
      case JSONNull _:
      case null:
        num2 = 1;
        break;
      default:
        num2 = b is JSONLazyCreator ? 1 : 0;
        break;
    }
    int num3 = num2 != 0 ? 1 : 0;
    return (num1 & num3) != 0 || a.Equals(b);
  }

  public static bool operator !=(JSONNode a, object b) => !(a == b);

  public override bool Equals(object obj) => (object) this == obj;

  public override int GetHashCode() => base.GetHashCode();

  public static string Escape(string aText)
  {
    JSONNode.m_EscapeBuilder.Length = 0;
    if (JSONNode.m_EscapeBuilder.Capacity < aText.Length + aText.Length / 10)
      JSONNode.m_EscapeBuilder.Capacity = aText.Length + aText.Length / 10;
    foreach (char ch in aText)
    {
      switch (ch)
      {
        case '\b':
          JSONNode.m_EscapeBuilder.Append("\\b");
          break;
        case '\t':
          JSONNode.m_EscapeBuilder.Append("\\t");
          break;
        case '\n':
          JSONNode.m_EscapeBuilder.Append("\\n");
          break;
        case '\f':
          JSONNode.m_EscapeBuilder.Append("\\f");
          break;
        case '\r':
          JSONNode.m_EscapeBuilder.Append("\\r");
          break;
        case '"':
          JSONNode.m_EscapeBuilder.Append("\\\"");
          break;
        case '\\':
          JSONNode.m_EscapeBuilder.Append("\\\\");
          break;
        default:
          JSONNode.m_EscapeBuilder.Append(ch);
          break;
      }
    }
    string str = JSONNode.m_EscapeBuilder.ToString();
    JSONNode.m_EscapeBuilder.Length = 0;
    return str;
  }

  public static void ParseElement(JSONNode ctx, string token, string tokenName, bool quoted)
  {
    if (quoted)
    {
      ctx.Add(tokenName, (JSONNode) token);
    }
    else
    {
      string lower = token.ToLower();
      switch (lower)
      {
        case "false":
        case "true":
          ctx.Add(tokenName, (JSONNode) (lower == "true"));
          break;
        case "null":
          ctx.Add(tokenName, (JSONNode) null);
          break;
        default:
          double result;
          if (double.TryParse(token, out result))
          {
            ctx.Add(tokenName, (JSONNode) result);
            break;
          }
          ctx.Add(tokenName, (JSONNode) token);
          break;
      }
    }
  }

  public static JSONNode Parse(string aJSON)
  {
    Stack<JSONNode> jsonNodeStack = new Stack<JSONNode>();
    JSONNode ctx = (JSONNode) null;
    int index = 0;
    StringBuilder stringBuilder = new StringBuilder();
    string str = "";
    bool flag = false;
    bool quoted = false;
    for (; index < aJSON.Length; ++index)
    {
      switch (aJSON[index])
      {
        case '\t':
        case ' ':
          if (flag)
          {
            stringBuilder.Append(aJSON[index]);
            continue;
          }
          continue;
        case '\n':
        case '\r':
          continue;
        case '"':
          flag = !flag;
          quoted |= flag;
          continue;
        case ',':
          if (flag)
          {
            stringBuilder.Append(aJSON[index]);
            continue;
          }
          if (stringBuilder.Length > 0 | quoted)
            JSONNode.ParseElement(ctx, stringBuilder.ToString(), str, quoted);
          str = "";
          stringBuilder.Length = 0;
          quoted = false;
          continue;
        case ':':
          if (flag)
          {
            stringBuilder.Append(aJSON[index]);
            continue;
          }
          str = stringBuilder.ToString();
          stringBuilder.Length = 0;
          quoted = false;
          continue;
        case '[':
          if (flag)
          {
            stringBuilder.Append(aJSON[index]);
            continue;
          }
          jsonNodeStack.Push((JSONNode) new JSONArray());
          if (ctx != (object) null)
            ctx.Add(str, jsonNodeStack.Peek());
          str = "";
          stringBuilder.Length = 0;
          ctx = jsonNodeStack.Peek();
          continue;
        case '\\':
          ++index;
          if (flag)
          {
            char ch = aJSON[index];
            switch (ch)
            {
              case 'b':
                stringBuilder.Append('\b');
                continue;
              case 'f':
                stringBuilder.Append('\f');
                continue;
              case 'n':
                stringBuilder.Append('\n');
                continue;
              case 'r':
                stringBuilder.Append('\r');
                continue;
              case 't':
                stringBuilder.Append('\t');
                continue;
              case 'u':
                string s = aJSON.Substring(index + 1, 4);
                stringBuilder.Append((char) int.Parse(s, NumberStyles.AllowHexSpecifier));
                index += 4;
                continue;
              default:
                stringBuilder.Append(ch);
                continue;
            }
          }
          else
            continue;
        case ']':
        case '}':
          if (flag)
          {
            stringBuilder.Append(aJSON[index]);
            continue;
          }
          if (jsonNodeStack.Count == 0)
            throw new Exception("JSON Parse: Too many closing brackets");
          jsonNodeStack.Pop();
          if (stringBuilder.Length > 0 | quoted)
          {
            JSONNode.ParseElement(ctx, stringBuilder.ToString(), str, quoted);
            quoted = false;
          }
          str = "";
          stringBuilder.Length = 0;
          if (jsonNodeStack.Count > 0)
          {
            ctx = jsonNodeStack.Peek();
            continue;
          }
          continue;
        case '{':
          if (flag)
          {
            stringBuilder.Append(aJSON[index]);
            continue;
          }
          jsonNodeStack.Push((JSONNode) new JSONObject());
          if (ctx != (object) null)
            ctx.Add(str, jsonNodeStack.Peek());
          str = "";
          stringBuilder.Length = 0;
          ctx = jsonNodeStack.Peek();
          continue;
        default:
          stringBuilder.Append(aJSON[index]);
          continue;
      }
    }
    if (flag)
      throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
    return ctx;
  }

  public virtual void Serialize(BinaryWriter aWriter)
  {
  }

  public void SaveToStream(Stream aData) => this.Serialize(new BinaryWriter(aData));

  public void SaveToCompressedStream(Stream aData)
  {
    throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
  }

  public void SaveToCompressedFile(string aFileName)
  {
    throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
  }

  public string SaveToCompressedBase64()
  {
    throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
  }

  public void SaveToFile(string aFileName)
  {
    Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
    using (FileStream aData = File.OpenWrite(aFileName))
      this.SaveToStream((Stream) aData);
  }

  public string SaveToBase64()
  {
    using (MemoryStream aData = new MemoryStream())
    {
      this.SaveToStream((Stream) aData);
      aData.Position = 0L;
      return Convert.ToBase64String(aData.ToArray());
    }
  }

  public static JSONNode Deserialize(BinaryReader aReader)
  {
    JSONNodeType jsonNodeType = (JSONNodeType) aReader.ReadByte();
    switch (jsonNodeType)
    {
      case JSONNodeType.Array:
        int num1 = aReader.ReadInt32();
        JSONArray jsonArray = new JSONArray();
        for (int index = 0; index < num1; ++index)
          jsonArray.Add(JSONNode.Deserialize(aReader));
        return (JSONNode) jsonArray;
      case JSONNodeType.Object:
        int num2 = aReader.ReadInt32();
        JSONObject jsonObject = new JSONObject();
        for (int index = 0; index < num2; ++index)
        {
          string aKey = aReader.ReadString();
          JSONNode aItem = JSONNode.Deserialize(aReader);
          jsonObject.Add(aKey, aItem);
        }
        return (JSONNode) jsonObject;
      case JSONNodeType.String:
        return (JSONNode) new JSONString(aReader.ReadString());
      case JSONNodeType.Number:
        return (JSONNode) new JSONNumber(aReader.ReadDouble());
      case JSONNodeType.NullValue:
        return (JSONNode) new JSONNull();
      case JSONNodeType.Boolean:
        return (JSONNode) new JSONBool(aReader.ReadBoolean());
      default:
        throw new Exception("Error deserializing JSON. Unknown tag: " + jsonNodeType.ToString());
    }
  }

  public static JSONNode LoadFromCompressedFile(string aFileName)
  {
    throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
  }

  public static JSONNode LoadFromCompressedStream(Stream aData)
  {
    throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
  }

  public static JSONNode LoadFromCompressedBase64(string aBase64)
  {
    throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
  }

  public static JSONNode LoadFromStream(Stream aData)
  {
    using (BinaryReader aReader = new BinaryReader(aData))
      return JSONNode.Deserialize(aReader);
  }

  public static JSONNode LoadFromFile(string aFileName)
  {
    using (FileStream aData = File.OpenRead(aFileName))
      return JSONNode.LoadFromStream((Stream) aData);
  }

  public static JSONNode LoadFromBase64(string aBase64)
  {
    MemoryStream aData = new MemoryStream(Convert.FromBase64String(aBase64));
    aData.Position = 0L;
    return JSONNode.LoadFromStream((Stream) aData);
  }
}
