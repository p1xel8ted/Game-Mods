// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer._InternalControlPropertyUpdateData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Mixer;

public struct _InternalControlPropertyUpdateData
{
  public Dictionary<string, _InternalControlPropertyMetaData> properties;

  public _InternalControlPropertyUpdateData(
    string name,
    _KnownControlPropertyPrimitiveTypes type,
    object value)
  {
    this.properties = new Dictionary<string, _InternalControlPropertyMetaData>();
    _InternalControlPropertyMetaData propertyMetaData = new _InternalControlPropertyMetaData();
    propertyMetaData.type = type;
    try
    {
      switch (type)
      {
        case _KnownControlPropertyPrimitiveTypes.Boolean:
          propertyMetaData.boolValue = (bool) value;
          break;
        case _KnownControlPropertyPrimitiveTypes.Number:
          propertyMetaData.numberValue = (double) value;
          break;
        default:
          propertyMetaData.stringValue = value.ToString();
          break;
      }
    }
    catch (Exception ex)
    {
      InteractivityManager.SingletonInstance._LogError("Failed to cast the value to a known type. Exception: " + ex.Message);
    }
    this.properties.Add(name, propertyMetaData);
  }
}
