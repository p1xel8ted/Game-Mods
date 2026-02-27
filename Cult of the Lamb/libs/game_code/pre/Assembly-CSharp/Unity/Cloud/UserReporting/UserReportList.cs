// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Unity.Cloud.UserReporting;

public class UserReportList
{
  public UserReportList() => this.UserReportPreviews = new List<UserReportPreview>();

  public string ContinuationToken { get; set; }

  public string Error { get; set; }

  public bool HasMore { get; set; }

  public List<UserReportPreview> UserReportPreviews { get; set; }

  public void Complete(int originalLimit, string continuationToken)
  {
    if (this.UserReportPreviews.Count <= 0 || this.UserReportPreviews.Count <= originalLimit)
      return;
    while (this.UserReportPreviews.Count > originalLimit)
      this.UserReportPreviews.RemoveAt(this.UserReportPreviews.Count - 1);
    this.ContinuationToken = continuationToken;
    this.HasMore = true;
  }
}
