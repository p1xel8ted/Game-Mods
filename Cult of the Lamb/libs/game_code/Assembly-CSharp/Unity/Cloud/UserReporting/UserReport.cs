// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace Unity.Cloud.UserReporting;

public class UserReport : UserReportPreview
{
  [CompilerGenerated]
  public List<UserReportAttachment> \u003CAttachments\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportMetric> \u003CClientMetrics\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportNamedValue> \u003CDeviceMetadata\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportEvent> \u003CEvents\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportNamedValue> \u003CFields\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportMeasure> \u003CMeasures\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportScreenshot> \u003CScreenshots\u003Ek__BackingField;

  public UserReport()
  {
    this.AggregateMetrics = new List<UserReportMetric>();
    this.Attachments = new List<UserReportAttachment>();
    this.ClientMetrics = new List<UserReportMetric>();
    this.DeviceMetadata = new List<UserReportNamedValue>();
    this.Events = new List<UserReportEvent>();
    this.Fields = new List<UserReportNamedValue>();
    this.Measures = new List<UserReportMeasure>();
    this.Screenshots = new List<UserReportScreenshot>();
  }

  public List<UserReportAttachment> Attachments
  {
    get => this.\u003CAttachments\u003Ek__BackingField;
    set => this.\u003CAttachments\u003Ek__BackingField = value;
  }

  public List<UserReportMetric> ClientMetrics
  {
    get => this.\u003CClientMetrics\u003Ek__BackingField;
    set => this.\u003CClientMetrics\u003Ek__BackingField = value;
  }

  public List<UserReportNamedValue> DeviceMetadata
  {
    get => this.\u003CDeviceMetadata\u003Ek__BackingField;
    set => this.\u003CDeviceMetadata\u003Ek__BackingField = value;
  }

  public List<UserReportEvent> Events
  {
    get => this.\u003CEvents\u003Ek__BackingField;
    set => this.\u003CEvents\u003Ek__BackingField = value;
  }

  public List<UserReportNamedValue> Fields
  {
    get => this.\u003CFields\u003Ek__BackingField;
    set => this.\u003CFields\u003Ek__BackingField = value;
  }

  public List<UserReportMeasure> Measures
  {
    get => this.\u003CMeasures\u003Ek__BackingField;
    set => this.\u003CMeasures\u003Ek__BackingField = value;
  }

  public List<UserReportScreenshot> Screenshots
  {
    get => this.\u003CScreenshots\u003Ek__BackingField;
    set => this.\u003CScreenshots\u003Ek__BackingField = value;
  }

  public UserReport Clone()
  {
    UserReport userReport = new UserReport();
    userReport.AggregateMetrics = this.AggregateMetrics != null ? this.AggregateMetrics.ToList<UserReportMetric>() : (List<UserReportMetric>) null;
    userReport.Attachments = this.Attachments != null ? this.Attachments.ToList<UserReportAttachment>() : (List<UserReportAttachment>) null;
    userReport.ClientMetrics = this.ClientMetrics != null ? this.ClientMetrics.ToList<UserReportMetric>() : (List<UserReportMetric>) null;
    userReport.ContentLength = this.ContentLength;
    userReport.DeviceMetadata = this.DeviceMetadata != null ? this.DeviceMetadata.ToList<UserReportNamedValue>() : (List<UserReportNamedValue>) null;
    userReport.Dimensions = this.Dimensions.ToList<UserReportNamedValue>();
    userReport.Events = this.Events != null ? this.Events.ToList<UserReportEvent>() : (List<UserReportEvent>) null;
    userReport.ExpiresOn = this.ExpiresOn;
    userReport.Fields = this.Fields != null ? this.Fields.ToList<UserReportNamedValue>() : (List<UserReportNamedValue>) null;
    userReport.Identifier = this.Identifier;
    userReport.IPAddress = this.IPAddress;
    userReport.Measures = this.Measures != null ? this.Measures.ToList<UserReportMeasure>() : (List<UserReportMeasure>) null;
    userReport.ProjectIdentifier = this.ProjectIdentifier;
    userReport.ReceivedOn = this.ReceivedOn;
    userReport.Screenshots = this.Screenshots != null ? this.Screenshots.ToList<UserReportScreenshot>() : (List<UserReportScreenshot>) null;
    userReport.Summary = this.Summary;
    userReport.Thumbnail = this.Thumbnail;
    return userReport;
  }

  public void Complete()
  {
    if (this.Screenshots.Count > 0)
      this.Thumbnail = this.Screenshots[this.Screenshots.Count - 1];
    Dictionary<string, UserReportMetric> dictionary = new Dictionary<string, UserReportMetric>();
    foreach (UserReportMeasure measure in this.Measures)
    {
      foreach (UserReportMetric metric in measure.Metrics)
      {
        if (!dictionary.ContainsKey(metric.Name))
          dictionary.Add(metric.Name, new UserReportMetric()
          {
            Name = metric.Name
          });
        UserReportMetric userReportMetric = dictionary[metric.Name];
        userReportMetric.Sample(metric.Average);
        dictionary[metric.Name] = userReportMetric;
      }
    }
    if (this.AggregateMetrics == null)
      this.AggregateMetrics = new List<UserReportMetric>();
    foreach (KeyValuePair<string, UserReportMetric> keyValuePair in dictionary)
      this.AggregateMetrics.Add(keyValuePair.Value);
    this.AggregateMetrics.Sort((IComparer<UserReportMetric>) new UserReport.UserReportMetricSorter());
  }

  public void Fix()
  {
    this.AggregateMetrics = this.AggregateMetrics ?? new List<UserReportMetric>();
    this.Attachments = this.Attachments ?? new List<UserReportAttachment>();
    this.ClientMetrics = this.ClientMetrics ?? new List<UserReportMetric>();
    this.DeviceMetadata = this.DeviceMetadata ?? new List<UserReportNamedValue>();
    this.Dimensions = this.Dimensions ?? new List<UserReportNamedValue>();
    this.Events = this.Events ?? new List<UserReportEvent>();
    this.Fields = this.Fields ?? new List<UserReportNamedValue>();
    this.Measures = this.Measures ?? new List<UserReportMeasure>();
    this.Screenshots = this.Screenshots ?? new List<UserReportScreenshot>();
  }

  public string GetDimensionsString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < this.Dimensions.Count; ++index)
    {
      UserReportNamedValue dimension = this.Dimensions[index];
      stringBuilder.Append(dimension.Name);
      stringBuilder.Append(": ");
      stringBuilder.Append(dimension.Value);
      if (index != this.Dimensions.Count - 1)
        stringBuilder.Append(", ");
    }
    return stringBuilder.ToString();
  }

  public void RemoveScreenshots(
    int maximumWidth,
    int maximumHeight,
    int totalBytes,
    int ignoreCount)
  {
    int num = 0;
    for (int count = this.Screenshots.Count; count > 0; --count)
    {
      if (count >= ignoreCount)
      {
        UserReportScreenshot screenshot = this.Screenshots[count];
        num += screenshot.DataBase64.Length;
        if (num > totalBytes)
          break;
        if (screenshot.Width > maximumWidth || screenshot.Height > maximumHeight)
          this.Screenshots.RemoveAt(count);
      }
    }
  }

  public UserReportPreview ToPreview()
  {
    return new UserReportPreview()
    {
      AggregateMetrics = this.AggregateMetrics != null ? this.AggregateMetrics.ToList<UserReportMetric>() : (List<UserReportMetric>) null,
      ContentLength = this.ContentLength,
      Dimensions = this.Dimensions != null ? this.Dimensions.ToList<UserReportNamedValue>() : (List<UserReportNamedValue>) null,
      ExpiresOn = this.ExpiresOn,
      Identifier = this.Identifier,
      IPAddress = this.IPAddress,
      ProjectIdentifier = this.ProjectIdentifier,
      ReceivedOn = this.ReceivedOn,
      Summary = this.Summary,
      Thumbnail = this.Thumbnail
    };
  }

  public class UserReportMetricSorter : IComparer<UserReportMetric>
  {
    public int Compare(UserReportMetric x, UserReportMetric y)
    {
      return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
  }
}
