// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportScreenshot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportScreenshot
{
  public string DataBase64 { get; set; }

  public string DataIdentifier { get; set; }

  public int FrameNumber { get; set; }

  public int Height => PngHelper.GetPngHeightFromBase64Data(this.DataBase64);

  public int Width => PngHelper.GetPngWidthFromBase64Data(this.DataBase64);
}
