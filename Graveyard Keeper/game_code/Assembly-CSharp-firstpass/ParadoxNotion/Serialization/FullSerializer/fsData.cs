// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

public sealed class fsData
{
  public object _value;
  public static fsData True = new fsData(true);
  public static fsData False = new fsData(false);
  public static fsData Null = new fsData();

  public fsData() => this._value = (object) null;

  public fsData(bool boolean) => this._value = (object) boolean;

  public fsData(double f) => this._value = (object) f;

  public fsData(long i) => this._value = (object) i;

  public fsData(string str) => this._value = (object) str;

  public fsData(Dictionary<string, fsData> dict) => this._value = (object) dict;

  public fsData(List<fsData> list) => this._value = (object) list;

  public static fsData CreateDictionary()
  {
    return new fsData(new Dictionary<string, fsData>(fsGlobalConfig.IsCaseSensitive ? (IEqualityComparer<string>) StringComparer.Ordinal : (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase));
  }

  public static fsData CreateList() => new fsData(new List<fsData>());

  public static fsData CreateList(int capacity) => new fsData(new List<fsData>(capacity));

  public void BecomeDictionary()
  {
    this._value = (object) new Dictionary<string, fsData>((IEqualityComparer<string>) StringComparer.Ordinal);
  }

  public fsData Clone()
  {
    return new fsData() { _value = this._value };
  }

  public fsDataType Type
  {
    get
    {
      if (this._value == null)
        return fsDataType.Null;
      if (this._value is double)
        return fsDataType.Double;
      if (this._value is long)
        return fsDataType.Int64;
      if (this._value is bool)
        return fsDataType.Boolean;
      if (this._value is string)
        return fsDataType.String;
      if (this._value is Dictionary<string, fsData>)
        return fsDataType.Object;
      if (this._value is List<fsData>)
        return fsDataType.Array;
      throw new InvalidOperationException("unknown JSON data type");
    }
  }

  public bool IsNull => this._value == null;

  public bool IsDouble => this._value is double;

  public bool IsInt64 => this._value is long;

  public bool IsBool => this._value is bool;

  public bool IsString => this._value is string;

  public bool IsDictionary => this._value is Dictionary<string, fsData>;

  public bool IsList => this._value is List<fsData>;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public double AsDouble => this.Cast<double>();

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public long AsInt64 => this.Cast<long>();

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public bool AsBool => this.Cast<bool>();

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public string AsString => this.Cast<string>();

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public Dictionary<string, fsData> AsDictionary => this.Cast<Dictionary<string, fsData>>();

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public List<fsData> AsList => this.Cast<List<fsData>>();

  public T Cast<T>()
  {
    return this._value is T ? (T) this._value : throw new InvalidCastException($"Unable to cast <{this?.ToString()}> (with type = {this._value.GetType()?.ToString()}) to type {typeof (T)?.ToString()}");
  }

  public override string ToString() => fsJsonPrinter.CompressedJson(this);

  public override bool Equals(object obj) => this.Equals(obj as fsData);

  public bool Equals(fsData other)
  {
    if (other == (fsData) null || this.Type != other.Type)
      return false;
    switch (this.Type)
    {
      case fsDataType.Array:
        List<fsData> asList1 = this.AsList;
        List<fsData> asList2 = other.AsList;
        if (asList1.Count != asList2.Count)
          return false;
        for (int index = 0; index < asList1.Count; ++index)
        {
          if (!asList1[index].Equals(asList2[index]))
            return false;
        }
        return true;
      case fsDataType.Object:
        Dictionary<string, fsData> asDictionary1 = this.AsDictionary;
        Dictionary<string, fsData> asDictionary2 = other.AsDictionary;
        if (asDictionary1.Count != asDictionary2.Count)
          return false;
        foreach (string key in asDictionary1.Keys)
        {
          if (!asDictionary2.ContainsKey(key) || !asDictionary1[key].Equals(asDictionary2[key]))
            return false;
        }
        return true;
      case fsDataType.Double:
        return this.AsDouble == other.AsDouble || Math.Abs(this.AsDouble - other.AsDouble) < double.Epsilon;
      case fsDataType.Int64:
        return this.AsInt64 == other.AsInt64;
      case fsDataType.Boolean:
        return this.AsBool == other.AsBool;
      case fsDataType.String:
        return this.AsString == other.AsString;
      case fsDataType.Null:
        return true;
      default:
        throw new Exception("Unknown data type");
    }
  }

  public static bool operator ==(fsData a, fsData b)
  {
    if ((object) a == (object) b)
      return true;
    if ((object) a == null || (object) b == null)
      return false;
    return a.IsDouble && b.IsDouble ? Math.Abs(a.AsDouble - b.AsDouble) < double.Epsilon : a.Equals(b);
  }

  public static bool operator !=(fsData a, fsData b) => !(a == b);

  public override int GetHashCode() => this._value.GetHashCode();
}
