// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud.UserReporting;

public class UserReportList
{
  [CompilerGenerated]
  public string \u003CContinuationToken\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CError\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CHasMore\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportPreview> \u003CUserReportPreviews\u003Ek__BackingField;

  public UserReportList() => this.UserReportPreviews = new List<UserReportPreview>();

  public string ContinuationToken
  {
    get => this.\u003CContinuationToken\u003Ek__BackingField;
    set => this.\u003CContinuationToken\u003Ek__BackingField = value;
  }

  public string Error
  {
    get => this.\u003CError\u003Ek__BackingField;
    set => this.\u003CError\u003Ek__BackingField = value;
  }

  public bool HasMore
  {
    get => this.\u003CHasMore\u003Ek__BackingField;
    set => this.\u003CHasMore\u003Ek__BackingField = value;
  }

  public List<UserReportPreview> UserReportPreviews
  {
    get => this.\u003CUserReportPreviews\u003Ek__BackingField;
    set => this.\u003CUserReportPreviews\u003Ek__BackingField = value;
  }

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
