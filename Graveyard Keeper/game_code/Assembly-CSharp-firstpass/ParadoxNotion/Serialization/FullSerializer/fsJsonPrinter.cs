// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsJsonPrinter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

public static class fsJsonPrinter
{
  public static void InsertSpacing(TextWriter stream, int count)
  {
    for (int index = 0; index < count; ++index)
      stream.Write("    ");
  }

  public static string EscapeString(string str)
  {
    bool flag = false;
    for (int index = 0; index < str.Length; ++index)
    {
      char ch = str[index];
      int int32 = Convert.ToInt32(ch);
      if (int32 < 0 || int32 > (int) sbyte.MaxValue)
      {
        flag = true;
        break;
      }
      switch (ch)
      {
        case char.MinValue:
        case '\a':
        case '\b':
        case '\t':
        case '\n':
        case '\f':
        case '\r':
        case '"':
        case '\\':
          flag = true;
          break;
      }
      if (flag)
        break;
    }
    if (!flag)
      return str;
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < str.Length; ++index)
    {
      char ch = str[index];
      int int32 = Convert.ToInt32(ch);
      if (int32 < 0 || int32 > (int) sbyte.MaxValue)
      {
        stringBuilder.Append($"\\u{int32:x4} ".Trim());
      }
      else
      {
        switch (ch)
        {
          case char.MinValue:
            stringBuilder.Append("\\0");
            continue;
          case '\a':
            stringBuilder.Append("\\a");
            continue;
          case '\b':
            stringBuilder.Append("\\b");
            continue;
          case '\t':
            stringBuilder.Append("\\t");
            continue;
          case '\n':
            stringBuilder.Append("\\n");
            continue;
          case '\f':
            stringBuilder.Append("\\f");
            continue;
          case '\r':
            stringBuilder.Append("\\r");
            continue;
          case '"':
            stringBuilder.Append("\\\"");
            continue;
          case '\\':
            stringBuilder.Append("\\\\");
            continue;
          default:
            stringBuilder.Append(ch);
            continue;
        }
      }
    }
    return stringBuilder.ToString();
  }

  public static void BuildCompressedString(fsData data, TextWriter stream)
  {
    switch (data.Type)
    {
      case fsDataType.Array:
        stream.Write('[');
        bool flag1 = false;
        foreach (fsData data1 in data.AsList)
        {
          if (flag1)
            stream.Write(',');
          flag1 = true;
          TextWriter stream1 = stream;
          fsJsonPrinter.BuildCompressedString(data1, stream1);
        }
        stream.Write(']');
        break;
      case fsDataType.Object:
        stream.Write('{');
        bool flag2 = false;
        foreach (KeyValuePair<string, fsData> keyValuePair in data.AsDictionary)
        {
          if (flag2)
            stream.Write(',');
          flag2 = true;
          stream.Write('"');
          stream.Write(keyValuePair.Key);
          stream.Write('"');
          stream.Write(":");
          fsJsonPrinter.BuildCompressedString(keyValuePair.Value, stream);
        }
        stream.Write('}');
        break;
      case fsDataType.Double:
        stream.Write(fsJsonPrinter.ConvertDoubleToString(data.AsDouble));
        break;
      case fsDataType.Int64:
        stream.Write(data.AsInt64);
        break;
      case fsDataType.Boolean:
        if (data.AsBool)
        {
          stream.Write("true");
          break;
        }
        stream.Write("false");
        break;
      case fsDataType.String:
        stream.Write('"');
        stream.Write(fsJsonPrinter.EscapeString(data.AsString));
        stream.Write('"');
        break;
      case fsDataType.Null:
        stream.Write("null");
        break;
    }
  }

  public static void BuildPrettyString(fsData data, TextWriter stream, int depth)
  {
    switch (data.Type)
    {
      case fsDataType.Array:
        if (data.AsList.Count == 0)
        {
          stream.Write("[]");
          break;
        }
        bool flag1 = false;
        stream.Write('[');
        stream.WriteLine();
        foreach (fsData data1 in data.AsList)
        {
          if (flag1)
          {
            stream.Write(',');
            stream.WriteLine();
          }
          flag1 = true;
          fsJsonPrinter.InsertSpacing(stream, depth + 1);
          TextWriter stream1 = stream;
          int depth1 = depth + 1;
          fsJsonPrinter.BuildPrettyString(data1, stream1, depth1);
        }
        stream.WriteLine();
        fsJsonPrinter.InsertSpacing(stream, depth);
        stream.Write(']');
        break;
      case fsDataType.Object:
        stream.Write('{');
        stream.WriteLine();
        bool flag2 = false;
        foreach (KeyValuePair<string, fsData> keyValuePair in data.AsDictionary)
        {
          if (flag2)
          {
            stream.Write(',');
            stream.WriteLine();
          }
          flag2 = true;
          fsJsonPrinter.InsertSpacing(stream, depth + 1);
          stream.Write('"');
          stream.Write(keyValuePair.Key);
          stream.Write('"');
          stream.Write(": ");
          fsJsonPrinter.BuildPrettyString(keyValuePair.Value, stream, depth + 1);
        }
        stream.WriteLine();
        fsJsonPrinter.InsertSpacing(stream, depth);
        stream.Write('}');
        break;
      case fsDataType.Double:
        stream.Write(fsJsonPrinter.ConvertDoubleToString(data.AsDouble));
        break;
      case fsDataType.Int64:
        stream.Write(data.AsInt64);
        break;
      case fsDataType.Boolean:
        if (data.AsBool)
        {
          stream.Write("true");
          break;
        }
        stream.Write("false");
        break;
      case fsDataType.String:
        stream.Write('"');
        stream.Write(fsJsonPrinter.EscapeString(data.AsString));
        stream.Write('"');
        break;
      case fsDataType.Null:
        stream.Write("null");
        break;
    }
  }

  public static void PrettyJson(fsData data, TextWriter outputStream)
  {
    fsJsonPrinter.BuildPrettyString(data, outputStream, 0);
  }

  public static string PrettyJson(fsData data)
  {
    StringBuilder sb = new StringBuilder();
    using (StringWriter stream = new StringWriter(sb))
    {
      fsJsonPrinter.BuildPrettyString(data, (TextWriter) stream, 0);
      return sb.ToString();
    }
  }

  public static void CompressedJson(fsData data, StreamWriter outputStream)
  {
    fsJsonPrinter.BuildCompressedString(data, (TextWriter) outputStream);
  }

  public static string CompressedJson(fsData data)
  {
    StringBuilder sb = new StringBuilder();
    using (StringWriter stream = new StringWriter(sb))
    {
      fsJsonPrinter.BuildCompressedString(data, (TextWriter) stream);
      return sb.ToString();
    }
  }

  public static string ConvertDoubleToString(double d)
  {
    if (double.IsInfinity(d) || double.IsNaN(d))
      return d.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    string str = d.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    if (!str.Contains(".") && !str.Contains("e") && !str.Contains("E"))
      str += ".0";
    return str;
  }
}
