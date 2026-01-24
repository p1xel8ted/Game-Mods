// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportScreenshot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportScreenshot
{
  [CompilerGenerated]
  public string \u003CDataBase64\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CDataIdentifier\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CFrameNumber\u003Ek__BackingField;

  public string DataBase64
  {
    readonly get => this.\u003CDataBase64\u003Ek__BackingField;
    set => this.\u003CDataBase64\u003Ek__BackingField = value;
  }

  public string DataIdentifier
  {
    readonly get => this.\u003CDataIdentifier\u003Ek__BackingField;
    set => this.\u003CDataIdentifier\u003Ek__BackingField = value;
  }

  public int FrameNumber
  {
    readonly get => this.\u003CFrameNumber\u003Ek__BackingField;
    set => this.\u003CFrameNumber\u003Ek__BackingField = value;
  }

  public int Height => PngHelper.GetPngHeightFromBase64Data(this.DataBase64);

  public int Width => PngHelper.GetPngWidthFromBase64Data(this.DataBase64);
}
