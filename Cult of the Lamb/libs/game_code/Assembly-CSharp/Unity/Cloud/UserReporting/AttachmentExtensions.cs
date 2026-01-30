// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.AttachmentExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Unity.Cloud.UserReporting;

public static class AttachmentExtensions
{
  public static void AddJson(
    this List<UserReportAttachment> instance,
    string name,
    string fileName,
    string contents)
  {
    instance?.Add(new UserReportAttachment(name, fileName, "application/json", Encoding.UTF8.GetBytes(contents)));
  }

  public static void AddText(
    this List<UserReportAttachment> instance,
    string name,
    string fileName,
    string contents)
  {
    instance?.Add(new UserReportAttachment(name, fileName, "text/plain", Encoding.UTF8.GetBytes(contents)));
  }
}
