// Decompiled with JetBrains decompiler
// Type: YoutubeLight.VideoInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace YoutubeLight;

public class VideoInfo
{
  public static IEnumerable<VideoInfo> Defaults = (IEnumerable<VideoInfo>) new List<VideoInfo>()
  {
    new VideoInfo(5, VideoType.Flash, 240 /*0xF0*/, false, AudioType.Mp3, 64 /*0x40*/, AdaptiveType.None),
    new VideoInfo(6, VideoType.Flash, 270, false, AudioType.Mp3, 64 /*0x40*/, AdaptiveType.None),
    new VideoInfo(13, VideoType.Mobile, 0, false, AudioType.Aac, 0, AdaptiveType.None),
    new VideoInfo(17, VideoType.Mobile, 144 /*0x90*/, false, AudioType.Aac, 24, AdaptiveType.None),
    new VideoInfo(18, VideoType.Mp4, 360, false, AudioType.Aac, 96 /*0x60*/, AdaptiveType.None),
    new VideoInfo(22, VideoType.Mp4, 720, false, AudioType.Aac, 192 /*0xC0*/, AdaptiveType.None),
    new VideoInfo(34, VideoType.Flash, 360, false, AudioType.Aac, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(35, VideoType.Flash, 480, false, AudioType.Aac, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(36, VideoType.Mobile, 240 /*0xF0*/, false, AudioType.Aac, 38, AdaptiveType.None),
    new VideoInfo(37, VideoType.Mp4, 1080, false, AudioType.Aac, 192 /*0xC0*/, AdaptiveType.None),
    new VideoInfo(38, VideoType.Mp4, 3072 /*0x0C00*/, false, AudioType.Aac, 192 /*0xC0*/, AdaptiveType.None),
    new VideoInfo(43, VideoType.WebM, 360, false, AudioType.Vorbis, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(44, VideoType.WebM, 480, false, AudioType.Vorbis, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(45, VideoType.WebM, 720, false, AudioType.Vorbis, 192 /*0xC0*/, AdaptiveType.None),
    new VideoInfo(46, VideoType.WebM, 1080, false, AudioType.Vorbis, 192 /*0xC0*/, AdaptiveType.None),
    new VideoInfo(59, VideoType.Mp4, 480, false, AudioType.Aac, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(78, VideoType.Mp4, 480, false, AudioType.Aac, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(82, VideoType.Mp4, 360, true, AudioType.Aac, 96 /*0x60*/, AdaptiveType.None),
    new VideoInfo(83, VideoType.Mp4, 240 /*0xF0*/, true, AudioType.Aac, 96 /*0x60*/, AdaptiveType.None),
    new VideoInfo(84, VideoType.Mp4, 720, true, AudioType.Aac, 152, AdaptiveType.None),
    new VideoInfo(85, VideoType.Mp4, 520, true, AudioType.Aac, 152, AdaptiveType.None),
    new VideoInfo(100, VideoType.WebM, 360, true, AudioType.Vorbis, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(101, VideoType.WebM, 360, true, AudioType.Vorbis, 192 /*0xC0*/, AdaptiveType.None),
    new VideoInfo(102, VideoType.WebM, 720, true, AudioType.Vorbis, 192 /*0xC0*/, AdaptiveType.None),
    new VideoInfo(91, VideoType.Mp4, 144 /*0x90*/, false, AudioType.Aac, 48 /*0x30*/, AdaptiveType.None),
    new VideoInfo(92, VideoType.Mp4, 240 /*0xF0*/, false, AudioType.Aac, 48 /*0x30*/, AdaptiveType.None),
    new VideoInfo(93, VideoType.Mp4, 360, false, AudioType.Aac, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(94, VideoType.Mp4, 480, false, AudioType.Aac, 128 /*0x80*/, AdaptiveType.None),
    new VideoInfo(95, VideoType.Mp4, 720, false, AudioType.Aac, 256 /*0x0100*/, AdaptiveType.None),
    new VideoInfo(96 /*0x60*/, VideoType.Mp4, 1080, false, AudioType.Aac, 256 /*0x0100*/, AdaptiveType.None),
    new VideoInfo(132, VideoType.Mp4, 240 /*0xF0*/, false, AudioType.Aac, 48 /*0x30*/, AdaptiveType.None),
    new VideoInfo(151, VideoType.Mp4, 72, false, AudioType.Aac, 24, AdaptiveType.None),
    new VideoInfo(133, VideoType.Mp4, 240 /*0xF0*/, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(134, VideoType.Mp4, 360, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(135, VideoType.Mp4, 480, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(136, VideoType.Mp4, 720, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(137, VideoType.Mp4, 1080, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(138, VideoType.Mp4, 2160, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(160 /*0xA0*/, VideoType.Mp4, 144 /*0x90*/, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(212, VideoType.Mp4, 480, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(264, VideoType.Mp4, 1440, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(298, VideoType.Mp4, 720, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(299, VideoType.Mp4, 1080, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(266, VideoType.Mp4, 2160, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(139, VideoType.Mp4, 0, false, AudioType.Aac, 48 /*0x30*/, AdaptiveType.Audio),
    new VideoInfo(140, VideoType.Mp4, 0, false, AudioType.Aac, 128 /*0x80*/, AdaptiveType.Audio),
    new VideoInfo(141, VideoType.Mp4, 0, false, AudioType.Aac, 256 /*0x0100*/, AdaptiveType.Audio),
    new VideoInfo(256 /*0x0100*/, VideoType.Mp4, 0, false, AudioType.Aac, 0, AdaptiveType.Audio),
    new VideoInfo(258, VideoType.Mp4, 0, false, AudioType.Aac, 0, AdaptiveType.Audio),
    new VideoInfo(325, VideoType.Mp4, 0, false, AudioType.Aac, 0, AdaptiveType.Audio),
    new VideoInfo(328, VideoType.Mp4, 0, false, AudioType.Aac, 0, AdaptiveType.Audio),
    new VideoInfo(167, VideoType.WebM, 360, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(168, VideoType.WebM, 480, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(169, VideoType.WebM, 720, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(170, VideoType.WebM, 1080, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(218, VideoType.WebM, 480, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(219, VideoType.WebM, 480, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(278, VideoType.WebM, 144 /*0x90*/, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(242, VideoType.WebM, 240 /*0xF0*/, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(243, VideoType.WebM, 360, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(244, VideoType.WebM, 480, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(245, VideoType.WebM, 480, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(246, VideoType.WebM, 480, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(247, VideoType.WebM, 720, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(248, VideoType.WebM, 1080, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(271, VideoType.WebM, 1440, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(272, VideoType.WebM, 2160, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(302, VideoType.WebM, 720, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(303, VideoType.WebM, 1080, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(308, VideoType.WebM, 1440, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(313, VideoType.WebM, 2160, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(315, VideoType.WebM, 2160, false, AudioType.Unknown, 0, AdaptiveType.Video),
    new VideoInfo(171, VideoType.WebM, 0, false, AudioType.Vorbis, 128 /*0x80*/, AdaptiveType.Audio),
    new VideoInfo(172, VideoType.WebM, 0, false, AudioType.Vorbis, 192 /*0xC0*/, AdaptiveType.Audio)
  };
  [CompilerGenerated]
  public AdaptiveType \u003CAdaptiveType\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CAudioBitrate\u003Ek__BackingField;
  [CompilerGenerated]
  public AudioType \u003CAudioType\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CDownloadUrl\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CFormatCode\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIs3D\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CHDR\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CRequiresDecryption\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CResolution\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CTitle\u003Ek__BackingField;
  [CompilerGenerated]
  public VideoType \u003CVideoType\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CHtmlPlayerVersion\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CHtmlscriptName\u003Ek__BackingField;

  public VideoInfo(int formatCode)
    : this(formatCode, VideoType.Unknown, 0, false, AudioType.Unknown, 0, AdaptiveType.None)
  {
  }

  public VideoInfo(VideoInfo info)
    : this(info.FormatCode, info.VideoType, info.Resolution, info.Is3D, info.AudioType, info.AudioBitrate, info.AdaptiveType)
  {
  }

  public VideoInfo(
    int formatCode,
    VideoType videoType,
    int resolution,
    bool is3D,
    AudioType audioType,
    int audioBitrate,
    AdaptiveType adaptiveType)
  {
    this.FormatCode = formatCode;
    this.VideoType = videoType;
    this.Resolution = resolution;
    this.Is3D = is3D;
    this.AudioType = audioType;
    this.AudioBitrate = audioBitrate;
    this.AdaptiveType = adaptiveType;
  }

  public AdaptiveType AdaptiveType
  {
    get => this.\u003CAdaptiveType\u003Ek__BackingField;
    set => this.\u003CAdaptiveType\u003Ek__BackingField = value;
  }

  public int AudioBitrate
  {
    get => this.\u003CAudioBitrate\u003Ek__BackingField;
    set => this.\u003CAudioBitrate\u003Ek__BackingField = value;
  }

  public string AudioExtension
  {
    get
    {
      switch (this.AudioType)
      {
        case AudioType.Aac:
          return ".aac";
        case AudioType.Mp3:
          return ".mp3";
        case AudioType.Opus:
          return ".webm";
        case AudioType.Vorbis:
          return ".ogg";
        default:
          return (string) null;
      }
    }
  }

  public AudioType AudioType
  {
    get => this.\u003CAudioType\u003Ek__BackingField;
    set => this.\u003CAudioType\u003Ek__BackingField = value;
  }

  public bool CanExtractAudio => this.VideoType == VideoType.Flash;

  public string DownloadUrl
  {
    get => this.\u003CDownloadUrl\u003Ek__BackingField;
    set => this.\u003CDownloadUrl\u003Ek__BackingField = value;
  }

  public int FormatCode
  {
    get => this.\u003CFormatCode\u003Ek__BackingField;
    set => this.\u003CFormatCode\u003Ek__BackingField = value;
  }

  public bool Is3D
  {
    get => this.\u003CIs3D\u003Ek__BackingField;
    set => this.\u003CIs3D\u003Ek__BackingField = value;
  }

  public bool HDR
  {
    get => this.\u003CHDR\u003Ek__BackingField;
    set => this.\u003CHDR\u003Ek__BackingField = value;
  }

  public bool RequiresDecryption
  {
    get => this.\u003CRequiresDecryption\u003Ek__BackingField;
    set => this.\u003CRequiresDecryption\u003Ek__BackingField = value;
  }

  public int Resolution
  {
    get => this.\u003CResolution\u003Ek__BackingField;
    set => this.\u003CResolution\u003Ek__BackingField = value;
  }

  public string Title
  {
    get => this.\u003CTitle\u003Ek__BackingField;
    set => this.\u003CTitle\u003Ek__BackingField = value;
  }

  public string VideoExtension
  {
    get
    {
      switch (this.VideoType)
      {
        case VideoType.Mobile:
          return ".3gp";
        case VideoType.Flash:
          return ".flv";
        case VideoType.Mp4:
          return ".mp4";
        case VideoType.WebM:
          return ".webm";
        default:
          return (string) null;
      }
    }
  }

  public VideoType VideoType
  {
    get => this.\u003CVideoType\u003Ek__BackingField;
    set => this.\u003CVideoType\u003Ek__BackingField = value;
  }

  public string HtmlPlayerVersion
  {
    get => this.\u003CHtmlPlayerVersion\u003Ek__BackingField;
    set => this.\u003CHtmlPlayerVersion\u003Ek__BackingField = value;
  }

  public string HtmlscriptName
  {
    get => this.\u003CHtmlscriptName\u003Ek__BackingField;
    set => this.\u003CHtmlscriptName\u003Ek__BackingField = value;
  }

  public override string ToString()
  {
    return $"Full Title: {this.Title + this.VideoExtension}, Type: {this.VideoType}, Resolution: {this.Resolution}p";
  }
}
