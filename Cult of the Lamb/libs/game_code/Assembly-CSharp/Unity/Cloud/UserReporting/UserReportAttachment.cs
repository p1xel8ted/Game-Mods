// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportAttachment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportAttachment(string name, string fileName, string contentType, byte[] data)
{
  [CompilerGenerated]
  public string \u003CContentType\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CDataBase64\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CDataIdentifier\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CFileName\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CName\u003Ek__BackingField;

  public string ContentType
  {
    readonly get => this.\u003CContentType\u003Ek__BackingField;
    set => this.\u003CContentType\u003Ek__BackingField = value;
  }

  public string DataBase64
  {
    readonly get => this.\u003CDataBase64\u003Ek__BackingField;
    set => this.\u003CDataBase64\u003Ek__BackingField = value;
  }

  public string DataIdentifier
  {
    readonly get => this.\u003CDataIdentifier\u003Ek__BackingField;
    set => this.\u003CDataIdentifier\u003Ek__BackingField = value;
  }

  public string FileName
  {
    readonly get => this.\u003CFileName\u003Ek__BackingField;
    set => this.\u003CFileName\u003Ek__BackingField = value;
  }

  public string Name
  {
    readonly get => this.\u003CName\u003Ek__BackingField;
    set => this.\u003CName\u003Ek__BackingField = value;
  }
}
