// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportAttachment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportAttachment(string name, string fileName, string contentType, byte[] data)
{
  public string ContentType { get; set; } = contentType;

  public string DataBase64 { get; set; } = Convert.ToBase64String(data);

  public string DataIdentifier { get; set; } = (string) null;

  public string FileName { get; set; } = fileName;

  public string Name { get; set; } = name;
}
