// Decompiled with JetBrains decompiler
// Type: YoutubePlayerLivestream
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using YoutubeLight;

#nullable disable
public class YoutubePlayerLivestream : MonoBehaviour
{
  public string _livestreamUrl;
  public YoutubePlayerLivestream.DownloadUrlResponse downloadYoutubeUrlResponse;

  public void Start() => this.GetLivestreamUrl(this._livestreamUrl);

  public void GetLivestreamUrl(string url)
  {
    this.StartProcess(new Action<string>(this.OnLiveUrlLoaded), url);
  }

  public void StartProcess(Action<string> callback, string url)
  {
    this.StartCoroutine((IEnumerator) this.DownloadYoutubeUrl(url, callback));
  }

  public void OnLiveUrlLoaded(string url)
  {
    Debug.Log((object) "You can check how to use double clicking in that log");
    Debug.Log((object) ("This is the live url, pass to the player: " + url));
  }

  public IEnumerator DownloadYoutubeUrl(string url, Action<string> callback)
  {
    YoutubePlayerLivestream playerLivestream = this;
    playerLivestream.downloadYoutubeUrlResponse = new YoutubePlayerLivestream.DownloadUrlResponse();
    string videoId = url.Replace("https://youtube.com/watch?v=", "");
    string str = $"https://www.youtube.com/watch?v={videoId}&gl=US&hl=en&has_verified=1&bpctr=9999999999";
    UnityWebRequest request = UnityWebRequest.Get(url);
    request.SetRequestHeader("User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:10.0) Gecko/20100101 Firefox/10.0 (Chrome)");
    yield return (object) request.SendWebRequest();
    playerLivestream.downloadYoutubeUrlResponse.httpCode = request.responseCode;
    if (request.result == UnityWebRequest.Result.ConnectionError)
      Debug.Log((object) "Youtube UnityWebRequest isNetworkError!");
    else if (request.result == UnityWebRequest.Result.ProtocolError)
      Debug.Log((object) "Youtube UnityWebRequest isHttpError!");
    else if (request.responseCode == 200L)
    {
      if (request.downloadHandler != null && request.downloadHandler.text != null)
      {
        if (request.downloadHandler.isDone)
        {
          playerLivestream.downloadYoutubeUrlResponse.isValid = true;
          playerLivestream.downloadYoutubeUrlResponse.data = request.downloadHandler.text;
        }
      }
      else
        Debug.Log((object) "Youtube UnityWebRequest Null response");
    }
    else
      Debug.Log((object) ("Youtube UnityWebRequest responseCode:" + request.responseCode.ToString()));
    playerLivestream.StartCoroutine((IEnumerator) playerLivestream.GetUrlFromJson(callback, videoId, request.downloadHandler.text));
  }

  public IEnumerator GetUrlFromJson(Action<string> callback, string _videoID, string pageSource)
  {
    string str1 = _videoID.Replace("https://www.youtube.com/watch?v=", "").Replace("https://youtube.com/watch?v=", "");
    string json1 = string.Empty;
    bool flag = false;
    if (Regex.IsMatch(pageSource, "[\"\\']status[\"\\']\\s*:\\s*[\"\\']LOGIN_REQUIRED") | flag)
    {
      string str2 = $"https://www.docs.google.com/get_video_info?video_id={str1}&eurl=https://youtube.googleapis.com/v/{str1}&html5=1&c=TVHTML5&cver=6.20180913";
      Debug.Log((object) str2);
      UnityWebRequest request = UnityWebRequest.Get(str2);
      request.SetRequestHeader("User-Agent", pageSource);
      yield return (object) request.SendWebRequest();
      if (request.result == UnityWebRequest.Result.ConnectionError)
        Debug.Log((object) "Youtube UnityWebRequest isNetworkError!");
      else if (request.result == UnityWebRequest.Result.ProtocolError)
        Debug.Log((object) "Youtube UnityWebRequest isHttpError!");
      else if (request.responseCode != 200L)
        Debug.Log((object) ("Youtube UnityWebRequest responseCode:" + request.responseCode.ToString()));
      Debug.Log((object) request.downloadHandler.text);
      json1 = UnityWebRequest.UnEscapeURL(HTTPHelperYoutube.ParseQueryString(request.downloadHandler.text)["player_response"]);
      request = (UnityWebRequest) null;
    }
    else
    {
      Match match1 = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;\\s*(?:var\\s+meta|</script|\\n)", RegexOptions.Multiline).Match(pageSource);
      if (match1.Success)
      {
        string json2 = match1.Result("$1");
        if (!json2.Contains("raw_player_response:ytInitialPlayerResponse"))
          json1 = JObject.Parse(json2).ToString();
      }
      Match match2 = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;\\s*(?:var\\s+meta|</script|\\n)", RegexOptions.Multiline).Match(pageSource);
      if (match2.Success)
        json1 = match2.Result("$1");
      Match match3 = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;\\s*(?:var\\s+meta|</script|\\n)", RegexOptions.Multiline).Match(pageSource);
      if (match3.Success)
        json1 = match3.Result("$1");
      Match match4 = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;", RegexOptions.Multiline).Match(pageSource);
      if (match4.Success)
        json1 = match4.Result("$1");
    }
    JObject jobject = JObject.Parse(json1);
    if (jobject["videoDetails"][(object) "isLive"].Value<bool>())
    {
      string message = jobject["streamingData"][(object) "hlsManifestUrl"].ToString();
      Debug.Log((object) message);
      callback(message);
    }
    else
    {
      Debug.Log((object) "NO");
      Debug.Log((object) "This is not a livestream url");
    }
  }

  public static void WriteLog(string filename, string c)
  {
    string path = $"{Application.persistentDataPath}/{filename}_{DateTime.Now.ToString("ddMMyyyyhhmmssffff")}.txt";
    Debug.Log((object) ("Log written in: " + Application.persistentDataPath));
    string contents = c;
    File.WriteAllText(path, contents);
  }

  public class DownloadUrlResponse
  {
    public string data;
    public bool isValid;
    public long httpCode;

    public DownloadUrlResponse()
    {
      this.data = (string) null;
      this.isValid = false;
      this.httpCode = 0L;
    }
  }
}
