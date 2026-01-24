// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.AttachmentExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
