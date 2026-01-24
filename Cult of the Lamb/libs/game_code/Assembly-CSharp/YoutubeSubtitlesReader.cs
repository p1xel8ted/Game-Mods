// Decompiled with JetBrains decompiler
// Type: YoutubeSubtitlesReader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

#nullable disable
public class YoutubeSubtitlesReader : MonoBehaviour
{
  public string[] _delimiters = new string[3]
  {
    "-->",
    "- >",
    "->"
  };
  public string videoID;
  public string langCode;
  public VideoPlayer videoPlayer;
  public bool subtitleLoaded;
  public string currentTextLine;
  public UnityEngine.UI.Text uiSubtitle;
  public List<SubtitleItem> subtitleList;

  public void Start() => this.LoadSubtitle();

  public void LoadSubtitle()
  {
    this.subtitleLoaded = false;
    this.StartCoroutine((IEnumerator) this.DownloadSubtitle());
  }

  public void WhenSubtitleLoadAreReady(List<SubtitleItem> result)
  {
    this.subtitleList = result;
    Debug.Log((object) "Subtitle Loaded");
    this.subtitleLoaded = true;
  }

  public IEnumerator DownloadSubtitle()
  {
    YoutubeSubtitlesReader youtubeSubtitlesReader = this;
    UnityWebRequest request = UnityWebRequest.Get("https://flask-service.e1ist6a3bu9ba.us-east-2.cs.amazonlightsail.com/api/subtitle?url=https://www.youtube.com/watch?v=" + youtubeSubtitlesReader.videoID);
    Debug.Log((object) request.url);
    yield return (object) request.SendWebRequest();
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text)["subtitles"][0][youtubeSubtitlesReader.langCode];
    if (jsonNode.Count > 0)
    {
      for (int aIndex = 0; aIndex < jsonNode.Count; ++aIndex)
      {
        if (jsonNode[aIndex]["ext"] == (object) "vtt")
        {
          Debug.Log((object) "loading subtitle");
          youtubeSubtitlesReader.StartCoroutine((IEnumerator) youtubeSubtitlesReader.DownloadSubtitleFile((string) jsonNode[aIndex]["url"]));
          break;
        }
      }
    }
  }

  public IEnumerator DownloadSubtitleFile(string url)
  {
    UnityWebRequest request = UnityWebRequest.Get(url);
    yield return (object) request.SendWebRequest();
    this.subtitleList = this.ParseStream(request.downloadHandler.data);
    this.WhenSubtitleLoadAreReady(this.subtitleList);
  }

  public void FixedUpdate()
  {
    if (!this.videoPlayer.isPlaying || !this.subtitleLoaded)
      return;
    foreach (SubtitleItem subtitle in this.subtitleList)
    {
      if (this.videoPlayer.time >= subtitle.StartTime && this.videoPlayer.time <= subtitle.EndTime)
      {
        this.currentTextLine = subtitle.text;
        this.uiSubtitle.text = this.currentTextLine;
        break;
      }
      this.currentTextLine = "";
      this.uiSubtitle.text = this.currentTextLine;
    }
  }

  public List<SubtitleItem> ParseStream(byte[] subtitleBytes)
  {
    List<SubtitleItem> source = new List<SubtitleItem>();
    List<string> list1 = this.GetVttSubTitleParts(subtitleBytes).ToList<string>();
    if (!list1.Any<string>())
      throw new FormatException("Parsing as VTT returned no VTT part.");
    foreach (string str in list1)
    {
      string[] separator = new string[1]
      {
        Environment.NewLine
      };
      List<string> list2 = ((IEnumerable<string>) str.Split(separator, StringSplitOptions.None)).Select<string, string>((Func<string, string>) (s => s.Trim())).Where<string>((Func<string, bool>) (l => !string.IsNullOrEmpty(l))).ToList<string>();
      SubtitleItem subtitleItem1 = new SubtitleItem();
      foreach (string line in list2)
      {
        if (subtitleItem1.StartTime == 0.0 && subtitleItem1.EndTime == 0.0)
        {
          double startTc;
          double endTc;
          if (this.TryParseTimecodeLine(line, out startTc, out endTc))
          {
            subtitleItem1.StartTime = startTc / 1000.0;
            subtitleItem1.EndTime = endTc / 1000.0;
          }
        }
        else
        {
          SubtitleItem subtitleItem2 = subtitleItem1;
          subtitleItem2.text = $"{subtitleItem2.text}{line} \n";
        }
      }
      if ((subtitleItem1.StartTime != 0.0 || subtitleItem1.EndTime != 0.0) && subtitleItem1.text.Any<char>())
        source.Add(subtitleItem1);
    }
    return source.Any<SubtitleItem>() ? source : throw new ArgumentException("Stream is not in a valid VTT format");
  }

  public IEnumerable<string> GetVttSubTitleParts(byte[] r)
  {
    StringBuilder stringBuilder = new StringBuilder();
    StreamReader reader = new StreamReader((Stream) new MemoryStream(r));
    string str;
    while ((str = reader.ReadLine()) != null)
    {
      if (string.IsNullOrEmpty(str.Trim()))
      {
        string vttSubTitlePart = stringBuilder.ToString().TrimEnd();
        if (!string.IsNullOrEmpty(vttSubTitlePart))
          yield return vttSubTitlePart;
        stringBuilder = new StringBuilder();
      }
      else
        stringBuilder.AppendLine(str);
    }
    if (stringBuilder.Length > 0)
      yield return stringBuilder.ToString();
  }

  public bool TryParseTimecodeLine(string line, out double startTc, out double endTc)
  {
    string[] strArray = line.Split(this._delimiters, StringSplitOptions.None);
    if (strArray.Length != 2)
    {
      startTc = -1.0;
      endTc = -1.0;
      return false;
    }
    startTc = (double) this.ParseVttTimecode(strArray[0]);
    endTc = (double) this.ParseVttTimecode(strArray[1]);
    return true;
  }

  public int ParseVttTimecode(string s)
  {
    string str = string.Empty;
    Match match1 = Regex.Match(s, "[0-9]+:[0-9]+:[0-9]+[,\\.][0-9]+");
    if (match1.Success)
    {
      str = match1.Value;
    }
    else
    {
      Match match2 = Regex.Match(s, "[0-9]+:[0-9]+[,\\.][0-9]+");
      if (match2.Success)
        str = "00:" + match2.Value;
    }
    TimeSpan result;
    return !string.IsNullOrEmpty(str) && TimeSpan.TryParse(str.Replace(',', '.'), out result) ? (int) result.TotalMilliseconds : -1;
  }
}
