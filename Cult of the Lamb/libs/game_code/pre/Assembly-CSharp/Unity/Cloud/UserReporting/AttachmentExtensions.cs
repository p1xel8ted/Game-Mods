// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.AttachmentExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
