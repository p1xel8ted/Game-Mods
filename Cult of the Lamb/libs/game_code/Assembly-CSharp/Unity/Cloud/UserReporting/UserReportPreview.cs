// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Cloud.Authorization;

#nullable disable
namespace Unity.Cloud.UserReporting;

public class UserReportPreview
{
  [CompilerGenerated]
  public List<UserReportMetric> \u003CAggregateMetrics\u003Ek__BackingField;
  [CompilerGenerated]
  public UserReportAppearanceHint \u003CAppearanceHint\u003Ek__BackingField;
  [CompilerGenerated]
  public long \u003CContentLength\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportNamedValue> \u003CDimensions\u003Ek__BackingField;
  [CompilerGenerated]
  public DateTime \u003CExpiresOn\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CGeoCountry\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CIdentifier\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CIPAddress\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsHiddenWithoutDimension\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsSilent\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsTemporary\u003Ek__BackingField;
  [CompilerGenerated]
  public LicenseLevel \u003CLicenseLevel\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CProjectIdentifier\u003Ek__BackingField;
  [CompilerGenerated]
  public DateTime \u003CReceivedOn\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CSummary\u003Ek__BackingField;
  [CompilerGenerated]
  public UserReportScreenshot \u003CThumbnail\u003Ek__BackingField;

  public UserReportPreview() => this.Dimensions = new List<UserReportNamedValue>();

  public List<UserReportMetric> AggregateMetrics
  {
    get => this.\u003CAggregateMetrics\u003Ek__BackingField;
    set => this.\u003CAggregateMetrics\u003Ek__BackingField = value;
  }

  public UserReportAppearanceHint AppearanceHint
  {
    get => this.\u003CAppearanceHint\u003Ek__BackingField;
    set => this.\u003CAppearanceHint\u003Ek__BackingField = value;
  }

  public long ContentLength
  {
    get => this.\u003CContentLength\u003Ek__BackingField;
    set => this.\u003CContentLength\u003Ek__BackingField = value;
  }

  public List<UserReportNamedValue> Dimensions
  {
    get => this.\u003CDimensions\u003Ek__BackingField;
    set => this.\u003CDimensions\u003Ek__BackingField = value;
  }

  public DateTime ExpiresOn
  {
    get => this.\u003CExpiresOn\u003Ek__BackingField;
    set => this.\u003CExpiresOn\u003Ek__BackingField = value;
  }

  public string GeoCountry
  {
    get => this.\u003CGeoCountry\u003Ek__BackingField;
    set => this.\u003CGeoCountry\u003Ek__BackingField = value;
  }

  public string Identifier
  {
    get => this.\u003CIdentifier\u003Ek__BackingField;
    set => this.\u003CIdentifier\u003Ek__BackingField = value;
  }

  public string IPAddress
  {
    get => this.\u003CIPAddress\u003Ek__BackingField;
    set => this.\u003CIPAddress\u003Ek__BackingField = value;
  }

  public bool IsHiddenWithoutDimension
  {
    get => this.\u003CIsHiddenWithoutDimension\u003Ek__BackingField;
    set => this.\u003CIsHiddenWithoutDimension\u003Ek__BackingField = value;
  }

  public bool IsSilent
  {
    get => this.\u003CIsSilent\u003Ek__BackingField;
    set => this.\u003CIsSilent\u003Ek__BackingField = value;
  }

  public bool IsTemporary
  {
    get => this.\u003CIsTemporary\u003Ek__BackingField;
    set => this.\u003CIsTemporary\u003Ek__BackingField = value;
  }

  public LicenseLevel LicenseLevel
  {
    get => this.\u003CLicenseLevel\u003Ek__BackingField;
    set => this.\u003CLicenseLevel\u003Ek__BackingField = value;
  }

  public string ProjectIdentifier
  {
    get => this.\u003CProjectIdentifier\u003Ek__BackingField;
    set => this.\u003CProjectIdentifier\u003Ek__BackingField = value;
  }

  public DateTime ReceivedOn
  {
    get => this.\u003CReceivedOn\u003Ek__BackingField;
    set => this.\u003CReceivedOn\u003Ek__BackingField = value;
  }

  public string Summary
  {
    get => this.\u003CSummary\u003Ek__BackingField;
    set => this.\u003CSummary\u003Ek__BackingField = value;
  }

  public UserReportScreenshot Thumbnail
  {
    get => this.\u003CThumbnail\u003Ek__BackingField;
    set => this.\u003CThumbnail\u003Ek__BackingField = value;
  }
}
