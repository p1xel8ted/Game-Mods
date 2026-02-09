// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportNamedValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportNamedValue(string name, string value)
{
  [CompilerGenerated]
  public string \u003CName\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CValue\u003Ek__BackingField;

  public string Name
  {
    readonly get => this.\u003CName\u003Ek__BackingField;
    set => this.\u003CName\u003Ek__BackingField = value;
  }

  public string Value
  {
    readonly get => this.\u003CValue\u003Ek__BackingField;
    set => this.\u003CValue\u003Ek__BackingField = value;
  }
}
