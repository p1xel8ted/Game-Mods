// Decompiled with JetBrains decompiler
// Type: YoutubeLight.Downloader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace YoutubeLight;

public abstract class Downloader
{
  [CompilerGenerated]
  public int? \u003CBytesToDownload\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CSavePath\u003Ek__BackingField;
  [CompilerGenerated]
  public VideoInfo \u003CVideo\u003Ek__BackingField;

  public Downloader(VideoInfo video, string savePath, int? bytesToDownload = null)
  {
    if (video == null)
      throw new ArgumentNullException(nameof (video));
    if (savePath == null)
      throw new ArgumentNullException(nameof (savePath));
    this.Video = video;
    this.SavePath = savePath;
    this.BytesToDownload = bytesToDownload;
  }

  public event EventHandler DownloadFinished;

  public event EventHandler DownloadStarted;

  public int? BytesToDownload
  {
    get => this.\u003CBytesToDownload\u003Ek__BackingField;
    set => this.\u003CBytesToDownload\u003Ek__BackingField = value;
  }

  public string SavePath
  {
    get => this.\u003CSavePath\u003Ek__BackingField;
    set => this.\u003CSavePath\u003Ek__BackingField = value;
  }

  public VideoInfo Video
  {
    get => this.\u003CVideo\u003Ek__BackingField;
    set => this.\u003CVideo\u003Ek__BackingField = value;
  }

  public abstract void Execute();

  public void OnDownloadFinished(EventArgs e)
  {
    if (this.DownloadFinished == null)
      return;
    this.DownloadFinished((object) this, e);
  }

  public void OnDownloadStarted(EventArgs e)
  {
    if (this.DownloadStarted == null)
      return;
    this.DownloadStarted((object) this, e);
  }
}
