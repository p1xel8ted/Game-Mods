// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Unity.Cloud.Authorization;

#nullable disable
namespace Unity.Cloud.UserReporting;

public class UserReportPreview
{
  public UserReportPreview() => this.Dimensions = new List<UserReportNamedValue>();

  public List<UserReportMetric> AggregateMetrics { get; set; }

  public UserReportAppearanceHint AppearanceHint { get; set; }

  public long ContentLength { get; set; }

  public List<UserReportNamedValue> Dimensions { get; set; }

  public DateTime ExpiresOn { get; set; }

  public string GeoCountry { get; set; }

  public string Identifier { get; set; }

  public string IPAddress { get; set; }

  public bool IsHiddenWithoutDimension { get; set; }

  public bool IsSilent { get; set; }

  public bool IsTemporary { get; set; }

  public LicenseLevel LicenseLevel { get; set; }

  public string ProjectIdentifier { get; set; }

  public DateTime ReceivedOn { get; set; }

  public string Summary { get; set; }

  public UserReportScreenshot Thumbnail { get; set; }
}
