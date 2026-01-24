// Decompiled with JetBrains decompiler
// Type: LightShaft.Scripts.YoutubeSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Newtonsoft.Json.Linq;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Video;
using YoutubeLight;

#nullable disable
namespace LightShaft.Scripts;

[RequireComponent(typeof (YoutubeVideoController))]
[RequireComponent(typeof (YoutubeVideoEvents))]
public class YoutubeSettings : MonoBehaviour
{
  public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.41 Safari/537.36";
  [HideInInspector]
  public YoutubeVideoController _controller;
  [HideInInspector]
  public YoutubeVideoEvents _events;
  [Space]
  [Tooltip("You can put urls that start at a specific time example: 'https://youtu.be/1G1nCxxQMnA?t=67'")]
  public string youtubeUrl;
  [Space]
  [Space]
  [Tooltip("The desired video quality you want to play. It's in experimental mod, because we need to use 2 video players in qualities 720+, you can expect some desync, but we are working to find a definitive solution to that. Thanks to DASH format.")]
  public YoutubeSettings.YoutubeVideoQuality videoQuality;
  [Space]
  [Tooltip("If it is a 360 degree video")]
  public bool is360;
  [Space]
  [Tooltip("Untick if you want to force a fullscreen mode.")]
  public bool dontForceFullscreen;
  [Space]
  [Header("Playback Options")]
  [Space]
  [Tooltip("Play the video when the script initialize")]
  public bool autoPlayOnStart = true;
  [Space]
  [Tooltip("Start playing the video from a desired time")]
  public bool startFromSecond;
  [DrawIf("startFromSecond", true, DrawIfAttribute.DisablingType.DontDraw)]
  public int startFromSecondTime;
  public bool prepareVideoToPlayLater;
  [Space]
  public bool showThumbnailBeforeVideoLoad;
  [DrawIf("showThumbnailBeforeVideoLoad", true, DrawIfAttribute.DisablingType.DontDraw)]
  public Renderer thumbnailObject;
  public string thumbnailVideoID;
  [Space]
  public bool customPlaylist;
  [DrawIf("customPlaylist", true, DrawIfAttribute.DisablingType.DontDraw)]
  public bool autoPlayNextVideo;
  [Header("If is a custom playlist put urls here")]
  public string[] youtubeUrls;
  public int currentUrlIndex;
  public string youtubeVideoID;
  [Header("You can Try different formats")]
  public YoutubeSettings.VideoFormatType videoFormat;
  [Space]
  [Header("Start Load and Play Url On enable this gameobject")]
  [Tooltip("Play or continue when OnEnable is called")]
  public bool autoPlayOnEnable;
  [Space]
  [Header("Use Device Video player (Standard quality only)")]
  [Tooltip("Play video in mobiles using the mobile device video player not unity internal player")]
  public bool playUsingInternalDevicePlayer;
  [Space]
  [Header("Only load the url to use in a custom player.")]
  [Space]
  [Tooltip("If you want to use your custom player, you can enable this and set the callback OnYoutubeUrlLoaded to your custom function sending the loaded url.")]
  public bool loadYoutubeUrlsOnly;
  [Space]
  [Header("Render the same video to more objects")]
  [Tooltip("Render the same video player material to a different materials, if you want")]
  public GameObject[] objectsToRenderTheVideoImage;
  [Space]
  [Header("Option for 3D video Only.")]
  [Tooltip("If the video is a 3D video sidebyside or Over/Under")]
  public bool is3DLayoutVideo;
  [DrawIf("is3DLayoutVideo", true, DrawIfAttribute.DisablingType.DontDraw)]
  public YoutubeSettings.Layout3D layout3d;
  [Space]
  public Camera mainCamera;
  [Space]
  [Header("The unity video players")]
  [Tooltip("The unity video player")]
  public VideoPlayer videoPlayer;
  [Tooltip("The audio player, (Needed for videos that dont have audio included 720p+)")]
  public VideoPlayer audioPlayer;
  [Space]
  [Tooltip("Show the output in the console")]
  public bool debug;
  [Space]
  [Tooltip("Ignore timeout is good for very low connections")]
  public bool ignoreTimeout;
  [Header("If you are having issues with sync try check this and change video format to WEBM")]
  public bool _skipOnDrop;
  [HideInInspector]
  public string videoUrl;
  [HideInInspector]
  public string audioUrl;
  [HideInInspector]
  public bool ForceGetWebServer;
  [HideInInspector]
  public bool progressStartDrag;
  public int maxRequestTime = 5;
  public float currentRequestTime;
  public int retryTimeUntilToRequestFromServer = 1;
  public int currentRetryTime;
  public bool gettingYoutubeURL;
  public bool videoAreReadyToPlay;
  public float lastPlayTime;
  public bool audioDecryptDone;
  public bool videoDecryptDone;
  public bool isRetry;
  public string lastTryVideoId;
  public float lastStartedTime;
  public bool youtubeUrlReady;
  public bool fullscreenModeEnabled;
  public Material skyboxMaterial3D;
  public Material skyboxMaterialNormal;
  public Material skyboxMaterial3DSide;
  public bool loadingFromServer;
  public YoutubeResultIds newRequestResults;
  public static string jsUrl;
  public const string serverURI = "https://youtube-dl-server-docker2-sx7c45rkxa-uc.a.run.app/api/info?url=";
  public const string formatURI = "&format=best&flatten=true";
  public const string VIDEOURIFORWEBGLPLAYER = "https://lswebgldemo.herokuapp.com/download.php?mime=video/mp4&title=generatedvideo&token=";
  public string tmpv;
  public string tmpf;
  public JObject requestResult;
  public bool forceLocal;
  public bool alreadyGotUrls;
  public bool BACKUPSYSTEM;
  public static string _visitorData = "";
  public long lastFrame = -1;
  public bool videoEnded;
  public bool noAudioAtacched;
  [HideInInspector]
  public string videoTitle = "";
  public Material EACMaterial;
  public Material Material360;
  public bool decryptionNeeded;
  [Header("If your unity version audio desyncs try to play with .4f or other value.")]
  public float audioDelayOffset;
  public bool startedFromTime;
  public bool finishedCalled;
  public bool videoStarted;
  public float lastErrorTime;
  [HideInInspector]
  public bool pauseCalled;
  public float totalVideoDuration;
  public float currentVideoDuration;
  public bool videoSeekDone;
  public bool videoAudioSeekDone;
  public bool lowRes;
  public float hideScreenTime;
  public float audioDuration;
  public string lsigForVideo;
  public string lsigForAudio;
  public Thread thread1;
  public static string jsUrlDownloaded;
  public static bool jsDownloaded = false;
  public string testinguri = "";
  public YoutubeResultIds webGlResults;
  public bool startedPlayingWebgl;
  public string logTest = "/";
  [HideInInspector]
  public bool isSyncing;
  public const string RateBypassFlag = "ratebypass";
  [HideInInspector]
  public static string SignatureQuery = "sig";
  [HideInInspector]
  public string encryptedSignatureVideo;
  [HideInInspector]
  public string encryptedSignatureAudio;
  [HideInInspector]
  public string masterURLForVideo;
  [HideInInspector]
  public string masterURLForAudio;
  public string[] patternNames = new string[1]{ "" };
  [HideInInspector]
  public bool decryptedUrlForVideo;
  [HideInInspector]
  public bool decryptedUrlForAudio;
  [HideInInspector]
  public string decryptedVideoUrlResult = "";
  [HideInInspector]
  public string decryptedAudioUrlResult = "";
  public List<VideoInfo> youtubeVideoInfos;
  public string htmlVersion = "";
  public bool olderVersionEnable;
  public string currentJSName = "";
  public static string sp = "";
  public static string projectionType = "";
  [HideInInspector]
  public string EncryptUrlForVideo;
  [HideInInspector]
  public string EncryptUrlForAudio;
  public YoutubeSettings.DownloadUrlResponse downloadYoutubeUrlResponse;
  [HideInInspector]
  public string jsonForHtmlVersion = "";
  public bool waitAudioSeek;
  public float oldVolume;
  public const string NQuery = "n";
  [HideInInspector]
  public bool checkIfSync;
  public YoutubeSettings.MagicContent magicResult;
  public bool canUpdate = true;

  public void Skybox3DSettup()
  {
    if (!this.is3DLayoutVideo)
      return;
    if (this.layout3d == YoutubeSettings.Layout3D.OverUnder)
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkybox3DOverUnder");
    else if (this.layout3d == YoutubeSettings.Layout3D.SideBySide)
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkybox3Dside");
    else if (this.layout3d == YoutubeSettings.Layout3D.EAC)
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkyboxEAC");
    else if (this.layout3d == YoutubeSettings.Layout3D.EAC3D)
    {
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkybox3DEAC");
    }
    else
    {
      if (this.layout3d != YoutubeSettings.Layout3D.None)
        return;
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkybox3Dside");
    }
  }

  public void YoutubeGeneratorSys(string _videoUrl, string _formatCode)
  {
    this.tmpv = _videoUrl;
    this.tmpf = _formatCode;
    Debug.Log((object) "WTF");
    this.GetNewSystem();
  }

  public void LoadANon3DVideoFromServer(string _videoUrl, string _formatCode)
  {
    this.StartCoroutine((IEnumerator) this.YoutubeGeneratorSysCall(_videoUrl, _formatCode));
  }

  public IEnumerator GetVisitorData()
  {
    if (!string.IsNullOrWhiteSpace(YoutubeSettings._visitorData))
      yield return (object) null;
    UnityWebRequest request = UnityWebRequest.Get("https://www.youtube.com/sw.js_data");
    request.SetRequestHeader("User-Agent", "com.google.ios.youtube/19.45.4 (iPhone16,2; U; CPU iOS 18_1_0 like Mac OS X; US)");
    yield return (object) request.SendWebRequest();
    string aJSON = request.downloadHandler.text;
    Debug.Log((object) (request.downloadHandler.text ?? ""));
    if (aJSON.StartsWith(")]}'"))
    {
      string str = aJSON;
      aJSON = str.Substring(4, str.Length - 4);
    }
    string str1 = JSON.Parse(aJSON)[0][2][0][0][13].Value;
    YoutubeSettings._visitorData = !string.IsNullOrWhiteSpace(str1) ? str1 : throw new Exception("Failed to resolve visitor data.");
    Debug.Log((object) ("SET VISITOR DATA: " + str1));
  }

  public IEnumerator YoutubeGenerateUrlUsingClient(string _videoUrl, string _formatCode)
  {
    this.alreadyGotUrls = false;
    Debug.Log((object) "WW");
    yield return (object) this.GetVisitorData();
    this.CheckVideoUrlAndExtractThevideoId(this.youtubeUrl);
    WWWForm formData1 = new WWWForm();
    string str1 = $"{{\"context\": {{\"client\": {{\"clientName\": \"IOS\",\"clientVersion\": \"19.29.1\",\"deviceMake\": \"Apple\",\"deviceModel\": \"iPhone16,2\",\"hl\": \"en\",\"osName\": \"iPhone\",\"osVersion\": \"17.5.1.21F90\",\"timeZone\": \"UTC\",\"gl\": \"US\",\"userAgent\": \"com.google.ios.youtube/19.29.1 (iPhone16,2; U; CPU iOS 17_5_1 like Mac OS X;)\"}}}},\"videoId\": \"{this.youtubeVideoID}\",\"contentCheckOk\": \"true\",}}";
    string s = $"{{\"context\": {{\"client\": {{\"clientName\": \"ANDROID\",\"clientVersion\": \"20.10.38\",\"androidSdkVersion\": \"30\",\"osName\": \"Android\",\"osVersion\": \"11\",\"visitorData\": \"{YoutubeSettings._visitorData}\",\"userAgent\": \"com.google.android.youtube/19.29.37 (Linux; U; Android 11) gzip\", \"hl\": \"en\",\"gl\": \"US\",\"utcOffsetMinutes\": \"0\"}}}},\"videoId\": \"{this.youtubeVideoID}\",}}";
    string str2 = $"{{\"context\": {{\"client\": {{\"clientName\": \"WEB\",\"clientVersion\": \"2.20220801.00.00\"}}}},\"videoId\": \"{this.youtubeVideoID}\",}}";
    byte[] bytes1 = Encoding.UTF8.GetBytes(s);
    UnityWebRequest request = UnityWebRequest.Post("https://www.youtube.com/youtubei/v1/player", formData1);
    request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bytes1);
    request.SetRequestHeader("Content-Type", "application/json");
    request.SetRequestHeader("User-Agent", "com.google.android.youtube/20.10.38 (Linux; U; Android 11) gzip");
    yield return (object) request.SendWebRequest();
    request.uploadHandler.Dispose();
    JObject jobject = JObject.Parse(request.downloadHandler.text);
    Debug.Log((object) request.downloadHandler.text);
    JToken formats = jobject["streamingData"][(object) "formats"];
    WWWForm formData2 = new WWWForm();
    byte[] bytes2 = Encoding.UTF8.GetBytes($"{{\"context\": {{\"client\": {{\"clientName\": \"IOS\",\"clientVersion\": \"19.45.4\",\"deviceMake\": \"Apple\",\"deviceModel\": \"iPhone16,2\",\"osName\": \"IOS\",\"osVersion\": \"18.1.0.22B83\",\"visitorData\": \"{YoutubeSettings._visitorData}\",\"platform\": \"MOBILE\",\"hl\": \"en\",\"osName\": \"iPhone\",\"osVersion\": \"18.1.0.22B83\",\"utcOffsetMinutes\": \"0\",\"timeZone\": \"UTC\",\"gl\": \"US\",\"userAgent\": \"com.google.ios.youtube/19.29.1 (iPhone16,2; U; CPU iOS 17_5_1 like Mac OS X;)\"}}}},\"videoId\": \"{this.youtubeVideoID}\",\"contentCheckOk\": \"true\",}}");
    UnityWebRequest nrequest = UnityWebRequest.Post("https://www.youtube.com/youtubei/v1/player", formData2);
    nrequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(bytes2);
    nrequest.SetRequestHeader("Content-Type", "application/json");
    nrequest.SetRequestHeader("User-Agent", "com.google.ios.youtube/19.45.4 (iPhone16,2; U; CPU iOS 18_1_0 like Mac OS X; US)");
    yield return (object) nrequest.SendWebRequest();
    nrequest.uploadHandler.Dispose();
    if (nrequest.error != null)
    {
      Debug.Log((object) ("Error: " + request.error));
    }
    else
    {
      if (this.decryptionNeeded)
      {
        this.requestResult = JObject.Parse(nrequest.downloadHandler.text);
        this.requestResult["streamingData"][(object) "formats"] = formats;
        if (this.debug)
          Debug.Log((object) "want to write log?");
        this.youtubeVideoInfos = YoutubeSettings.GetVideoInfos(YoutubeSettings.ExtractDownloadUrls(this.requestResult), this.videoTitle).ToList<VideoInfo>();
        this.videoTitle = YoutubeSettings.GetVideoTitle(this.requestResult);
        this.is360 = false;
        this.alreadyGotUrls = true;
        this.UrlsLoaded();
      }
      if (this.is360)
      {
        if (request.downloadHandler.text.Contains("EQUIRECTANGULAR_THREED_TOP_BOTTOM"))
        {
          if (this.debug)
            Debug.Log((object) "IS 3D");
          RenderSettings.skybox = this.skyboxMaterial3DSide;
          if (!this.alreadyGotUrls)
            this.UrlsLoaded();
        }
        else if (!this.loadingFromServer)
        {
          this.loadingFromServer = true;
          if (this.debug)
            Debug.Log((object) "Not a 3D");
          RenderSettings.skybox = this.skyboxMaterialNormal;
          if (!this.alreadyGotUrls)
            this.UrlsLoaded();
        }
      }
      else if (!this.alreadyGotUrls)
      {
        this.requestResult = JObject.Parse(request.downloadHandler.text);
        this.youtubeVideoInfos = YoutubeSettings.GetVideoInfos(YoutubeSettings.ExtractDownloadUrls(this.requestResult), this.videoTitle).ToList<VideoInfo>();
        request.downloadHandler.Dispose();
        if (!this.alreadyGotUrls)
          this.UrlsLoaded();
      }
    }
  }

  public IEnumerator YoutubeGeneratorSysCall(string _videoUrl, string _formatCode)
  {
    YoutubeSettings youtubeSettings = this;
    UnityWebRequest request = !youtubeSettings.is360 ? UnityWebRequest.Get($"https://youtube-dl-server-docker2-sx7c45rkxa-uc.a.run.app/api/utubePlay?url={_videoUrl}&format=best&flatten=true&formatId={_formatCode}") : UnityWebRequest.Get($"https://youtube-dl-server-docker2-sx7c45rkxa-uc.a.run.app/api/schoolupdate?url={_videoUrl}&flatten=true&format=bestvideo[height<=2160]");
    Debug.Log((object) request.url);
    yield return (object) request.SendWebRequest();
    youtubeSettings.loadingFromServer = false;
    youtubeSettings.gettingYoutubeURL = false;
    if (request.result == UnityWebRequest.Result.ConnectionError)
    {
      Debug.Log((object) request.error);
      youtubeSettings.StartCoroutine((IEnumerator) youtubeSettings.WaitAndTryConnectToServerAgain());
    }
    else
    {
      JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
      Debug.Log((object) request.downloadHandler.text);
      youtubeSettings.videoTitle = (string) jsonNode["title"];
      youtubeSettings.audioUrl = (string) jsonNode["audio"];
      youtubeSettings.videoUrl = (string) jsonNode["video"];
      if (!youtubeSettings.is360)
      {
        if (string.IsNullOrEmpty(youtubeSettings.videoUrl))
        {
          youtubeSettings.videoUrl = youtubeSettings.audioUrl;
          youtubeSettings.videoQuality = YoutubeSettings.YoutubeVideoQuality.STANDARD;
        }
      }
      else if (!youtubeSettings.is3DLayoutVideo)
        youtubeSettings.videoPlayer.targetMaterialRenderer.material = youtubeSettings.Material360;
      youtubeSettings.OnYoutubeUrlsLoaded();
    }
  }

  public IEnumerator WaitAndTryConnectToServerAgain()
  {
    yield return (object) new WaitForSeconds(2f);
    this.RetryPlayYoutubeVideo();
  }

  public void FixCameraEvent()
  {
    if ((UnityEngine.Object) this.mainCamera == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) Camera.main != (UnityEngine.Object) null)
      {
        this.mainCamera = Camera.main;
      }
      else
      {
        this.mainCamera = UnityEngine.Object.FindObjectOfType<Camera>();
        Debug.Log((object) "Add the main camera to the mainCamera field");
      }
    }
    if (this.videoPlayer.renderMode != VideoRenderMode.CameraFarPlane && this.videoPlayer.renderMode != VideoRenderMode.CameraNearPlane)
      return;
    this.videoPlayer.targetCamera = this.mainCamera;
  }

  public string CheckVideoUrlAndExtractThevideoId(string url)
  {
    if (url.Contains("?t="))
    {
      int num = url.LastIndexOf("?t=");
      string s = url.Remove(0, num).Replace("?t=", "");
      this.startFromSecond = true;
      this.startFromSecondTime = int.Parse(s);
      url = url.Remove(num);
    }
    if (!this.TryNormalizeYoutubeUrlLocal(url, out url))
    {
      url = "none";
      this.OnYoutubeError("Not a Youtube Url");
    }
    return url;
  }

  public void OnYoutubeError(string errorType)
  {
    Debug.Log((object) $"<color=red>{errorType}</color>");
  }

  public bool TryNormalizeYoutubeUrlLocal(string url, out string normalizedUrl)
  {
    url = url.Trim();
    url = url.Replace("youtu.be/", "youtube.com/watch?v=");
    url = url.Replace("www.youtube", "youtube");
    url = url.Replace("youtube.com/embed/", "youtube.com/watch?v=");
    if (url.Contains("/v/"))
      url = "https://youtube.com" + new Uri(url).AbsolutePath.Replace("/v/", "/watch?v=");
    url = url.Replace("/watch#", "/watch?");
    string str;
    if (!HTTPHelperYoutube.ParseQueryString(url).TryGetValue("v", out str))
    {
      normalizedUrl = (string) null;
      return false;
    }
    this.youtubeVideoID = str;
    normalizedUrl = "https://youtube.com/watch?v=" + str;
    return true;
  }

  public void Awake()
  {
    this._skipOnDrop = true;
    if (!this.loadYoutubeUrlsOnly)
    {
      if ((UnityEngine.Object) this.GetComponent<YoutubeVideoController>() == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "You need a VideoController attached to YoutubePlayer");
        return;
      }
      if ((UnityEngine.Object) this.GetComponent<YoutubeVideoEvents>() == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "You need a VidepoEvents attached to YoutubePlayer");
        return;
      }
      this._controller = this.GetComponent<YoutubeVideoController>();
      this._events = this.GetComponent<YoutubeVideoEvents>();
    }
    if (this.loadYoutubeUrlsOnly)
    {
      this._controller = this.GetComponent<YoutubeVideoController>();
      this._events = this.GetComponent<YoutubeVideoEvents>();
    }
    this.magicResult = new YoutubeSettings.MagicContent();
    if (this.playUsingInternalDevicePlayer || this.loadYoutubeUrlsOnly)
      return;
    if (this.is360 && this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.videoQuality = YoutubeSettings.YoutubeVideoQuality.HD;
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
    {
      if (this.videoFormat == YoutubeSettings.VideoFormatType.WEBM)
        this.videoPlayer.skipOnDrop = this._skipOnDrop;
      if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
        this.audioPlayer.transform.gameObject.SetActive(false);
    }
    if (this.videoPlayer.renderMode == VideoRenderMode.CameraFarPlane || this.videoPlayer.renderMode == VideoRenderMode.CameraNearPlane)
      this.fullscreenModeEnabled = true;
    else
      this.fullscreenModeEnabled = false;
  }

  public void VerifyFrames()
  {
    if (this.playUsingInternalDevicePlayer || !this.videoPlayer.isPlaying)
      return;
    if (this.lastFrame == this.videoPlayer.frame)
    {
      this.audioPlayer.Pause();
      this.videoPlayer.Pause();
      this.StartCoroutine((IEnumerator) this.WaitSync());
    }
    this.lastFrame = this.videoPlayer.frame;
    this.Invoke(nameof (VerifyFrames), 2f);
  }

  public virtual void Start()
  {
    if ((UnityEngine.Object) this.videoPlayer != (UnityEngine.Object) null && (UnityEngine.Object) this.videoPlayer.targetTexture != (UnityEngine.Object) null)
    {
      switch (this.videoQuality)
      {
        case YoutubeSettings.YoutubeVideoQuality.STANDARD:
          this.videoPlayer.targetTexture.width = 640;
          this.videoPlayer.targetTexture.height = 360;
          break;
        case YoutubeSettings.YoutubeVideoQuality.HD:
          this.videoPlayer.targetTexture.width = 1280 /*0x0500*/;
          this.videoPlayer.targetTexture.height = 720;
          break;
        case YoutubeSettings.YoutubeVideoQuality.FULLHD:
          this.videoPlayer.targetTexture.width = 1920;
          this.videoPlayer.targetTexture.height = 1080;
          break;
        case YoutubeSettings.YoutubeVideoQuality.UHD1440:
          this.videoPlayer.targetTexture.width = 2560 /*0x0A00*/;
          this.videoPlayer.targetTexture.height = 1440;
          break;
        case YoutubeSettings.YoutubeVideoQuality.UHD2160:
          this.videoPlayer.targetTexture.width = 3840 /*0x0F00*/;
          this.videoPlayer.targetTexture.height = 2160;
          break;
      }
    }
    if (this.playUsingInternalDevicePlayer)
      this.loadYoutubeUrlsOnly = true;
    this.videoFormat = YoutubeSettings.VideoFormatType.WEBM;
    if (!this.loadYoutubeUrlsOnly)
    {
      this.Invoke("VerifyFrames", 2f);
      this.FixCameraEvent();
      this.Skybox3DSettup();
      if (this.videoFormat == YoutubeSettings.VideoFormatType.WEBM)
      {
        this.videoPlayer.skipOnDrop = this._skipOnDrop;
        if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
          this.audioPlayer.skipOnDrop = this._skipOnDrop;
      }
      this.audioPlayer.seekCompleted += new VideoPlayer.EventHandler(this.AudioSeeked);
      this.videoPlayer.seekCompleted += new VideoPlayer.EventHandler(this.VideoSeeked);
    }
    this.PrepareVideoPlayerCallbacks();
    if (this.autoPlayOnStart)
    {
      if (this.customPlaylist)
        this.PlayYoutubeVideo(this.youtubeUrls[this.currentUrlIndex]);
      else
        this.PlayYoutubeVideo(this.youtubeUrl);
    }
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.lowRes = true;
    else
      this.lowRes = false;
  }

  public void DisableThumbnailObject()
  {
    if (!((UnityEngine.Object) this.thumbnailObject != (UnityEngine.Object) null))
      return;
    this.thumbnailObject.gameObject.SetActive(false);
  }

  public void EnableThumbnailObject()
  {
    if ((UnityEngine.Object) this.thumbnailObject != (UnityEngine.Object) null)
      this.thumbnailObject.gameObject.SetActive(true);
    else
      Debug.Log((object) "Thumbnail object is null");
  }

  public void TryToLoadThumbnailBeforeOpenVideo(string id)
  {
    this.StartCoroutine((IEnumerator) this.DownloadThumbnail(id.Replace("https://youtube.com/watch?v=", "")));
  }

  public IEnumerator DownloadThumbnail(string videoId)
  {
    UnityWebRequest request = UnityWebRequestTexture.GetTexture($"https://img.youtube.com/vi/{videoId}/0.jpg");
    yield return (object) request.SendWebRequest();
    this.EnableThumbnailObject();
    this.thumbnailObject.material.mainTexture = (Texture) DownloadHandlerTexture.GetContent(request);
  }

  public void FixedUpdate()
  {
    if ((UnityEngine.Object) this.videoPlayer != (UnityEngine.Object) null && this.videoPlayer.isPlaying)
    {
      if (!this.lowRes)
      {
        if ((UnityEngine.Object) this._controller.volumeSlider != (UnityEngine.Object) null)
        {
          if ((double) this.videoPlayer.GetTargetAudioSource((ushort) 0).volume <= 0.0)
            this.videoPlayer.GetTargetAudioSource((ushort) 0).volume = this._controller.volumeSlider.value;
        }
        else
          this.videoPlayer.GetTargetAudioSource((ushort) 0).volume = 1f;
      }
      else if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this._controller.volumeSlider != (UnityEngine.Object) null)
        {
          if ((double) this.audioPlayer.GetTargetAudioSource((ushort) 0).volume <= 0.0)
            this.audioPlayer.GetTargetAudioSource((ushort) 0).volume = this._controller.volumeSlider.value;
        }
        else
          this.audioPlayer.GetTargetAudioSource((ushort) 0).volume = 1f;
      }
    }
    if (!this.loadYoutubeUrlsOnly && !this.playUsingInternalDevicePlayer)
    {
      if (this.videoPlayer.isPlaying)
        this.HideLoading();
      else if (!this.pauseCalled && !this.prepareVideoToPlayLater)
        this.ShowLoading();
    }
    if (!this.loadYoutubeUrlsOnly)
    {
      if (this._controller.showPlayerControl && this.videoPlayer.isPlaying)
      {
        this.totalVideoDuration = (float) Mathf.RoundToInt((float) this.videoPlayer.frameCount / this.videoPlayer.frameRate);
        if (!this.lowRes)
        {
          this.audioDuration = (float) Mathf.RoundToInt((float) this.audioPlayer.frameCount / this.audioPlayer.frameRate);
          this.currentVideoDuration = (double) this.audioDuration >= (double) this.totalVideoDuration || !(this.audioPlayer.url != "") ? (float) Mathf.RoundToInt((float) this.videoPlayer.frame / this.videoPlayer.frameRate) : (float) Mathf.RoundToInt((float) this.audioPlayer.frame / this.audioPlayer.frameRate);
        }
        else
          this.currentVideoDuration = (float) Mathf.RoundToInt((float) this.videoPlayer.frame / this.videoPlayer.frameRate);
      }
      if (this.videoPlayer.frameCount > 0UL && (UnityEngine.Object) this._controller != (UnityEngine.Object) null && this._controller.showPlayerControl)
      {
        if (this._controller.useSliderToProgressVideo)
        {
          if (!this.progressStartDrag)
            this._controller.playbackSlider.value = (float) this.videoPlayer.time;
        }
        else if ((UnityEngine.Object) this._controller.progressRectangle != (UnityEngine.Object) null)
          this._controller.progressRectangle.fillAmount = (float) this.videoPlayer.frame / (float) this.videoPlayer.frameCount;
      }
    }
    if (this.gettingYoutubeURL)
    {
      this.currentRequestTime += Time.deltaTime;
      if ((double) this.currentRequestTime >= (double) this.maxRequestTime && !this.ignoreTimeout)
      {
        this.gettingYoutubeURL = false;
        if (this.debug)
          Debug.Log((object) "<color=blue>Max time reached, trying again!</color>");
        this.RetryPlayYoutubeVideo();
      }
    }
    if (this.videoAreReadyToPlay)
      this.videoAreReadyToPlay = false;
    this.ErrorCheck();
    if (!this.loadYoutubeUrlsOnly)
    {
      if (this._controller.showPlayerControl)
      {
        this.lowRes = this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD;
        if ((UnityEngine.Object) this._controller.currentTime != (UnityEngine.Object) null && (UnityEngine.Object) this._controller.totalTime != (UnityEngine.Object) null)
        {
          this._controller.currentTime.text = this.FormatTime(Mathf.RoundToInt(this.currentVideoDuration));
          this._controller.totalTime.text = this.lowRes ? this.FormatTime(Mathf.RoundToInt(this.totalVideoDuration)) : ((double) this.audioDuration >= (double) this.totalVideoDuration || !(this.audioPlayer.url != "") ? this.FormatTime(Mathf.RoundToInt(this.totalVideoDuration)) : this.FormatTime(Mathf.RoundToInt(this.audioDuration)));
        }
      }
      if (!this._controller.showPlayerControl)
      {
        if ((UnityEngine.Object) this._controller.controllerMainUI != (UnityEngine.Object) null)
          this._controller.controllerMainUI.SetActive(false);
      }
      else
        this._controller.controllerMainUI.SetActive(true);
    }
    if (this.decryptedUrlForAudio)
    {
      this.decryptedUrlForAudio = false;
      this.StartCoroutine((IEnumerator) this.ExtractorGetNParamAudio(this.decryptedAudioUrlResult, this.decryptedVideoUrlResult));
    }
    if (this.decryptedUrlForVideo)
    {
      this.decryptedUrlForVideo = false;
      this.DecryptVideoDone(this.decryptedVideoUrlResult);
    }
    if (!this.loadYoutubeUrlsOnly && this.videoPlayer.isPrepared && !this.videoPlayer.isPlaying)
    {
      if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
      {
        if (this.audioPlayer.isPrepared && !this.videoStarted)
        {
          this.videoStarted = true;
          this.VideoStarted(this.videoPlayer);
        }
      }
      else if (!this.videoStarted)
      {
        this.videoStarted = true;
        this.VideoStarted(this.videoPlayer);
      }
    }
    if (this.loadYoutubeUrlsOnly)
      return;
    if (this.videoPlayer.frame != 0L && !this.videoEnded && this.videoPlayer.isPlaying && this.videoPlayer.frame >= (long) this.videoPlayer.frameCount - 1L)
    {
      this.videoEnded = true;
      this.PlaybackDone(this.videoPlayer);
    }
    if (!this.videoPlayer.isPrepared)
      return;
    if (this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD)
    {
      if (!this.audioPlayer.isPrepared || this.startedPlayingWebgl)
        return;
      this.startedPlayingWebgl = true;
      this.StartPlayingWebgl();
    }
    else
    {
      if (this.startedPlayingWebgl)
        return;
      this.startedPlayingWebgl = true;
      this.StartPlayingWebgl();
    }
  }

  public void PrepareVideoPlayerCallbacks()
  {
    this.videoPlayer.errorReceived += new VideoPlayer.ErrorEventHandler(this.VideoErrorReceived);
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      return;
    this.audioPlayer.errorReceived += new VideoPlayer.ErrorEventHandler(this.VideoErrorReceived);
  }

  public void ShowLoading()
  {
    if (!((UnityEngine.Object) this._controller.loading != (UnityEngine.Object) null))
      return;
    this._controller.loading.SetActive(true);
  }

  public void HideLoading()
  {
    if (!((UnityEngine.Object) this._controller.loading != (UnityEngine.Object) null))
      return;
    this._controller.loading.SetActive(false);
  }

  public void ResetThings()
  {
    this.gettingYoutubeURL = false;
    this.progressStartDrag = false;
    this.videoAreReadyToPlay = false;
    this.audioDecryptDone = false;
    this.videoDecryptDone = false;
    this.isRetry = false;
    this.youtubeUrlReady = false;
    if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
      this.audioPlayer.seekCompleted += new VideoPlayer.EventHandler(this.AudioSeeked);
    this.videoPlayer.seekCompleted += new VideoPlayer.EventHandler(this.VideoSeeked);
    this.videoPlayer.frameDropped += new VideoPlayer.EventHandler(this.VideoPlayer_frameDropped);
    if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
      this.audioPlayer.frameDropped += new VideoPlayer.EventHandler(this.AudioPlayer_frameDropped);
    this.waitAudioSeek = false;
  }

  public string GetFormatCode()
  {
    if (!this.is360)
    {
      switch (this.videoQuality)
      {
        case YoutubeSettings.YoutubeVideoQuality.STANDARD:
          return "18";
        case YoutubeSettings.YoutubeVideoQuality.HD:
          return this.videoFormat != YoutubeSettings.VideoFormatType.MP4 ? "247" : "136";
        case YoutubeSettings.YoutubeVideoQuality.FULLHD:
          return this.videoFormat != YoutubeSettings.VideoFormatType.MP4 ? "248" : "137";
        case YoutubeSettings.YoutubeVideoQuality.UHD1440:
          return this.videoFormat != YoutubeSettings.VideoFormatType.MP4 ? "271" : "264";
        case YoutubeSettings.YoutubeVideoQuality.UHD2160:
          return this.videoFormat != YoutubeSettings.VideoFormatType.MP4 ? "313" : "266";
      }
    }
    else
    {
      switch (this.videoQuality)
      {
        case YoutubeSettings.YoutubeVideoQuality.STANDARD:
          return "134";
        case YoutubeSettings.YoutubeVideoQuality.HD:
          return this.videoFormat != YoutubeSettings.VideoFormatType.MP4 ? "247" : "136";
        case YoutubeSettings.YoutubeVideoQuality.FULLHD:
          return this.videoFormat != YoutubeSettings.VideoFormatType.MP4 ? "248" : "137";
        case YoutubeSettings.YoutubeVideoQuality.UHD1440:
          return this.videoFormat != YoutubeSettings.VideoFormatType.MP4 ? "271" : "264";
        case YoutubeSettings.YoutubeVideoQuality.UHD2160:
          return this.videoFormat != YoutubeSettings.VideoFormatType.MP4 ? "313" : "266";
      }
    }
    return "18";
  }

  public void PlayYoutubeVideo(string _videoId)
  {
    Debug.Log((object) "LOL");
    this.lowRes = this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD;
    this.ResetThings();
    _videoId = this.CheckVideoUrlAndExtractThevideoId(_videoId);
    if (_videoId == "none")
      return;
    if (this.showThumbnailBeforeVideoLoad)
      this.TryToLoadThumbnailBeforeOpenVideo(_videoId);
    this.youtubeUrlReady = false;
    this.ShowLoading();
    this.youtubeUrl = _videoId;
    this.isRetry = false;
    this.lastTryVideoId = _videoId;
    this.lastPlayTime = Time.time;
    Debug.Log((object) "WTFXX");
    if (!this.ForceGetWebServer)
    {
      this.currentRequestTime = 0.0f;
      this.gettingYoutubeURL = true;
      this.YoutubeGeneratorSys(this.youtubeUrl, this.GetFormatCode());
    }
    else
      this.StartCoroutine((IEnumerator) this.WebRequest(this.youtubeUrl));
  }

  public void DecryptAudioDone(string url)
  {
    this.audioUrl = url;
    this.audioDecryptDone = true;
    if (!this.videoDecryptDone)
      return;
    if (string.IsNullOrEmpty(this.decryptedAudioUrlResult))
    {
      this.RetryPlayYoutubeVideo();
    }
    else
    {
      this.videoAreReadyToPlay = true;
      this.OnYoutubeUrlsLoaded();
    }
  }

  public void DecryptVideoDone(string url)
  {
    this.videoUrl = url;
    this.videoDecryptDone = true;
    if (this.audioDecryptDone)
    {
      if (string.IsNullOrEmpty(this.decryptedVideoUrlResult))
      {
        this.RetryPlayYoutubeVideo();
      }
      else
      {
        this.videoAreReadyToPlay = true;
        this.OnYoutubeUrlsLoaded();
      }
    }
    else
    {
      if (this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD)
        return;
      if (string.IsNullOrEmpty(this.decryptedVideoUrlResult))
      {
        this.RetryPlayYoutubeVideo();
      }
      else
      {
        this.videoAreReadyToPlay = true;
        this.OnYoutubeUrlsLoaded();
      }
    }
  }

  public void UrlsLoaded()
  {
    this.gettingYoutubeURL = false;
    List<VideoInfo> youtubeVideoInfos = this.youtubeVideoInfos;
    this.videoDecryptDone = false;
    this.audioDecryptDone = false;
    this.decryptedUrlForVideo = false;
    this.decryptedUrlForAudio = false;
    if (this.is360 && (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.UHD1440 || this.videoQuality == YoutubeSettings.YoutubeVideoQuality.UHD2160))
      this.videoFormat = YoutubeSettings.VideoFormatType.WEBM;
    bool flag1 = false;
    string encrytedUrlAudio = "";
    string encryptedUrlVideo = "";
    string html = "";
    if (this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
    youtubeVideoInfos.Reverse();
    using (IEnumerator<VideoInfo> enumerator = youtubeVideoInfos.Where<VideoInfo>((Func<VideoInfo, bool>) (info => info.FormatCode == 18)).GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        VideoInfo current = enumerator.Current;
        this.StartCoroutine((IEnumerator) this.WaitJSDownload(current, YoutubeSettings.jsUrlDownloaded));
        if (current.RequiresDecryption)
        {
          flag1 = true;
          html = current.HtmlPlayerVersion;
          encrytedUrlAudio = current.DownloadUrl;
        }
        else
        {
          encrytedUrlAudio = current.DownloadUrl;
          this.audioUrl = current.DownloadUrl;
        }
      }
    }
    int message = 360;
    switch (this.videoQuality)
    {
      case YoutubeSettings.YoutubeVideoQuality.STANDARD:
        message = 360;
        break;
      case YoutubeSettings.YoutubeVideoQuality.HD:
        message = 720;
        break;
      case YoutubeSettings.YoutubeVideoQuality.FULLHD:
        message = 1080;
        break;
      case YoutubeSettings.YoutubeVideoQuality.UHD1440:
        message = 1440;
        break;
      case YoutubeSettings.YoutubeVideoQuality.UHD2160:
        message = 2160;
        break;
    }
    bool flag2 = false;
    youtubeVideoInfos.Reverse();
    foreach (VideoInfo videoInfo in youtubeVideoInfos)
    {
      VideoType videoType = this.videoFormat == YoutubeSettings.VideoFormatType.MP4 ? VideoType.Mp4 : VideoType.WebM;
      if (videoInfo.VideoType == videoType && videoInfo.Resolution == message)
      {
        if (this.debug)
          Debug.Log((object) message);
        if (this.is360)
        {
          if (!string.IsNullOrEmpty(YoutubeSettings.projectionType) && this.videoPlayer.renderMode != VideoRenderMode.RenderTexture)
          {
            switch (YoutubeSettings.projectionType)
            {
              case "MESH":
                foreach (UnityEngine.Object material in this.videoPlayer.targetMaterialRenderer.materials)
                {
                  if (material.name == "SphereEAC")
                  {
                    this.videoPlayer.targetMaterialRenderer.material = this.EACMaterial;
                    this.videoPlayer.targetMaterialRenderer.gameObject.transform.localScale = new Vector3(344.9097f, 344.9097f, -344.9097f);
                    this.videoPlayer.targetMaterialRenderer.gameObject.transform.localRotation = Quaternion.Euler(90f, -10f, -280f);
                  }
                }
                break;
              case "EQUIRECTANGULAR":
              case "RECTANGULAR":
                this.videoPlayer.targetMaterialRenderer.material = this.Material360;
                this.videoPlayer.targetMaterialRenderer.gameObject.transform.localScale = new Vector3(344.9097f, 344.9097f, 344.9097f);
                this.videoPlayer.targetMaterialRenderer.gameObject.transform.localRotation = Quaternion.Euler(0.0f, 90f, -90f);
                break;
              default:
                Debug.Log((object) "Untested projection type, report to support email.");
                break;
            }
          }
          bool flag3 = false;
          switch (videoInfo.Resolution)
          {
            case 720:
              if (videoType == VideoType.Mp4)
              {
                if (videoInfo.FormatCode == 136 || videoInfo.FormatCode == 298)
                {
                  flag3 = true;
                  break;
                }
                break;
              }
              if (videoInfo.FormatCode == 247 || videoInfo.FormatCode == 302)
              {
                flag3 = true;
                break;
              }
              break;
            case 1080:
              if (videoType == VideoType.Mp4)
              {
                if (videoInfo.FormatCode == 137 || videoInfo.FormatCode == 299)
                {
                  flag3 = true;
                  break;
                }
                break;
              }
              if (videoInfo.FormatCode == 248 || videoInfo.FormatCode == 303)
              {
                flag3 = true;
                break;
              }
              break;
            case 1440:
              if (videoType == VideoType.Mp4)
              {
                if (videoInfo.FormatCode == 264)
                {
                  flag3 = true;
                  break;
                }
                break;
              }
              if (videoInfo.FormatCode == 271 || videoInfo.FormatCode == 308)
              {
                flag3 = true;
                break;
              }
              break;
            case 2160:
              if (videoType == VideoType.Mp4)
              {
                if (videoInfo.FormatCode == 266)
                {
                  flag3 = true;
                  break;
                }
                break;
              }
              if (videoInfo.FormatCode == 313 || videoInfo.FormatCode == 315)
              {
                flag3 = true;
                break;
              }
              break;
          }
          if (flag3)
          {
            if (this.debug)
              Debug.Log((object) videoInfo.FormatCode);
            if (videoInfo.RequiresDecryption)
            {
              if (this.debug)
                Debug.Log((object) "REQUIRE DECRYPTION!");
              this.logTest = "Decry";
              flag1 = true;
              encryptedUrlVideo = videoInfo.DownloadUrl;
            }
            else
            {
              if (this.debug)
                Debug.Log((object) videoInfo.DownloadUrl);
              encryptedUrlVideo = videoInfo.DownloadUrl;
              this.StartCoroutine((IEnumerator) this.GetNParamAudio(videoInfo.DownloadUrl, videoInfo.HtmlPlayerVersion));
            }
            flag2 = true;
            break;
          }
        }
        else
        {
          if (videoInfo.RequiresDecryption)
          {
            if (this.debug)
              Debug.Log((object) "REQUIRE DECRYPTION!");
            this.logTest = "Decry";
            flag1 = true;
            encryptedUrlVideo = videoInfo.DownloadUrl;
          }
          else
          {
            encryptedUrlVideo = videoInfo.DownloadUrl;
            if (this.decryptionNeeded)
            {
              this.decryptionNeeded = false;
              this.videoUrl = videoInfo.DownloadUrl;
              this.StartCoroutine((IEnumerator) this.GetNParamAudio(videoInfo.DownloadUrl, videoInfo.HtmlPlayerVersion));
            }
            else
              this.StartCoroutine((IEnumerator) this.GetNParamAudio(videoInfo.DownloadUrl, videoInfo.HtmlPlayerVersion));
          }
          flag2 = true;
          break;
        }
      }
    }
    if (!flag2 && message == 1440)
    {
      foreach (VideoInfo videoInfo in youtubeVideoInfos)
      {
        if (videoInfo.FormatCode == 271)
        {
          string[] strArray = new string[6];
          strArray[0] = "FIXING!! ";
          int num = videoInfo.Resolution;
          strArray[1] = num.ToString();
          strArray[2] = " | ";
          strArray[3] = videoInfo.VideoType.ToString();
          strArray[4] = " | ";
          num = videoInfo.FormatCode;
          strArray[5] = num.ToString();
          Debug.Log((object) string.Concat(strArray));
          if (videoInfo.RequiresDecryption)
          {
            flag1 = true;
            encryptedUrlVideo = videoInfo.DownloadUrl;
          }
          else
          {
            encryptedUrlVideo = videoInfo.DownloadUrl;
            this.videoUrl = videoInfo.DownloadUrl;
            this.videoAreReadyToPlay = true;
            this.OnYoutubeUrlsLoaded();
          }
          flag2 = true;
          break;
        }
      }
    }
    if (!flag2 && message == 2160)
    {
      foreach (VideoInfo videoInfo in youtubeVideoInfos)
      {
        if (videoInfo.FormatCode == 313)
        {
          if (this.debug)
            Debug.Log((object) "Found but with unknow format in results, check to see if the video works normal.");
          if (videoInfo.RequiresDecryption)
          {
            flag1 = true;
            encryptedUrlVideo = videoInfo.DownloadUrl;
          }
          else
          {
            encryptedUrlVideo = videoInfo.DownloadUrl;
            this.videoUrl = videoInfo.DownloadUrl;
            this.videoAreReadyToPlay = true;
            this.OnYoutubeUrlsLoaded();
          }
          flag2 = true;
          break;
        }
      }
    }
    if (!flag2)
    {
      if (this.debug)
        Debug.Log((object) $"Desired quality not found, playing with low quality, check if the video id: {this.youtubeUrl} support that quality!");
      bool flag4 = false;
      VideoType videoType = this.videoFormat == YoutubeSettings.VideoFormatType.MP4 ? VideoType.Mp4 : VideoType.WebM;
      foreach (VideoInfo videoInfo in youtubeVideoInfos)
      {
        if (this.debug)
        {
          string[] strArray = new string[6];
          strArray[0] = "RES: ";
          int num = videoInfo.Resolution;
          strArray[1] = num.ToString();
          strArray[2] = " | ";
          num = videoInfo.FormatCode;
          strArray[3] = num.ToString();
          strArray[4] = " | ";
          strArray[5] = videoInfo.VideoType.ToString();
          Debug.Log((object) string.Concat(strArray));
        }
      }
      if (videoType == VideoType.WebM)
        videoType = VideoType.Mp4;
      foreach (VideoInfo videoInfo in youtubeVideoInfos)
      {
        if (videoInfo.VideoType == videoType && videoInfo.Resolution == 1080)
        {
          flag4 = true;
          if (videoInfo.RequiresDecryption)
          {
            Debug.Log((object) "yes");
            this.videoQuality = YoutubeSettings.YoutubeVideoQuality.STANDARD;
            flag1 = true;
            encryptedUrlVideo = videoInfo.DownloadUrl;
            break;
          }
          encryptedUrlVideo = videoInfo.DownloadUrl;
          this.videoUrl = videoInfo.DownloadUrl;
          this.StartCoroutine((IEnumerator) this.GetNParamAudio(videoInfo.DownloadUrl, videoInfo.HtmlPlayerVersion));
          break;
        }
      }
      if (!flag4)
      {
        foreach (VideoInfo videoInfo in youtubeVideoInfos)
        {
          if (videoInfo.VideoType == videoType && videoInfo.Resolution == 360)
          {
            if (videoInfo.RequiresDecryption)
            {
              this.videoQuality = YoutubeSettings.YoutubeVideoQuality.STANDARD;
              flag1 = true;
              encryptedUrlVideo = videoInfo.DownloadUrl;
              break;
            }
            encryptedUrlVideo = videoInfo.DownloadUrl;
            this.videoUrl = videoInfo.DownloadUrl;
            this.StartCoroutine((IEnumerator) this.GetNParamAudio(videoInfo.DownloadUrl, videoInfo.HtmlPlayerVersion));
            break;
          }
        }
      }
    }
    if (!flag1)
      return;
    this.decryptionNeeded = true;
    this.DecryptDownloadUrl(encryptedUrlVideo, encrytedUrlAudio, html, false);
  }

  public void StartPlayingWebgl()
  {
    this._events.OnVideoReadyToStart.Invoke();
    if (this.playUsingInternalDevicePlayer && Application.isMobilePlatform)
      this.StartCoroutine((IEnumerator) this.HandHeldPlayback());
    else
      this.StartPlayback();
  }

  public IEnumerator HandHeldPlayback()
  {
    Debug.Log((object) "This runs in mobile devices only!");
    yield return (object) new WaitForSeconds(1f);
    this.PlaybackDone(this.videoPlayer);
  }

  public IEnumerator DelayPlay()
  {
    yield return (object) new WaitForSeconds(this.audioDelayOffset);
    this.audioPlayer.Play();
  }

  public void StartPlayback()
  {
    if (this.objectsToRenderTheVideoImage.Length != 0)
    {
      foreach (GameObject gameObject in this.objectsToRenderTheVideoImage)
        gameObject.GetComponent<Renderer>().material.mainTexture = this.videoPlayer.texture;
    }
    this.videoEnded = false;
    this._events.OnVideoStarted.Invoke();
    int videoQuality1 = (int) this.videoQuality;
    this.HideLoading();
    this.waitAudioSeek = true;
    if ((this.is360 || (UnityEngine.Object) this.videoPlayer.targetTexture != (UnityEngine.Object) null) && this.videoPlayer.renderMode == VideoRenderMode.RenderTexture)
    {
      this.videoPlayer.targetTexture.width = (int) this.videoPlayer.width;
      this.videoPlayer.targetTexture.height = (int) this.videoPlayer.height;
    }
    if (this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD && !this.noAudioAtacched)
    {
      this.audioPlayer.Pause();
      this.videoPlayer.Pause();
      this.audioPlayer.time = 1.0;
      this.videoPlayer.time = 0.0;
    }
    if (!this.prepareVideoToPlayLater)
      this.DisableThumbnailObject();
    if (!this.prepareVideoToPlayLater)
    {
      this._events.OnVideoStarted.Invoke();
      if (this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD)
      {
        this.videoPlayer.Play();
        this.StartCoroutine((IEnumerator) this.DelayPlay());
      }
      else
        this.videoPlayer.Play();
    }
    if (this.startFromSecond)
    {
      this.startedFromTime = true;
      if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
        this.videoPlayer.time = (double) this.startFromSecondTime;
      else
        this.audioPlayer.time = (double) this.startFromSecondTime;
    }
    int videoQuality2 = (int) this.videoQuality;
  }

  public void ErrorCheck()
  {
    if (this.ForceGetWebServer || this.isRetry || (double) this.lastStartedTime >= (double) this.lastErrorTime || (double) this.lastErrorTime <= (double) this.lastPlayTime)
      return;
    if (this.debug)
      Debug.Log((object) "Error detected!, retry with low quality!");
    this.isRetry = true;
  }

  public int GetMaxQualitySupportedByDevice()
  {
    return Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation != ScreenOrientation.Portrait ? Screen.currentResolution.height : Screen.currentResolution.width;
  }

  public IEnumerator WebRequest(string videoID)
  {
    UnityWebRequest request = UnityWebRequest.Get($"https://youtube-dl-server-docker2-sx7c45rkxa-uc.a.run.app/api/info?url={videoID}&format=best&flatten=true");
    yield return (object) request.SendWebRequest();
    this.newRequestResults = new YoutubeResultIds();
    JSONNode jsonNode1 = JSON.Parse(request.downloadHandler.text);
    JSONNode jsonNode2 = jsonNode1["videos"][0]["formats"];
    this.newRequestResults.bestFormatWithAudioIncluded = (string) jsonNode1["videos"][0]["url"];
    for (int aIndex = 0; aIndex < jsonNode2.Count; ++aIndex)
    {
      if (jsonNode2[aIndex]["format_id"] == (object) "160")
        this.newRequestResults.lowQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "133")
        this.newRequestResults.lowQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "134")
        this.newRequestResults.standardQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "136")
        this.newRequestResults.hdQuality = this.newRequestResults.bestFormatWithAudioIncluded;
      else if (jsonNode2[aIndex]["format_id"] == (object) "137")
        this.newRequestResults.fullHdQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "266")
        this.newRequestResults.ultraHdQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "139")
        this.newRequestResults.audioUrl = (string) jsonNode2[aIndex]["url"];
    }
    this.audioUrl = this.newRequestResults.bestFormatWithAudioIncluded;
    this.videoUrl = this.newRequestResults.bestFormatWithAudioIncluded;
    switch (this.videoQuality)
    {
      case YoutubeSettings.YoutubeVideoQuality.STANDARD:
        this.videoUrl = this.newRequestResults.bestFormatWithAudioIncluded;
        break;
      case YoutubeSettings.YoutubeVideoQuality.HD:
        this.videoUrl = this.newRequestResults.hdQuality;
        break;
      case YoutubeSettings.YoutubeVideoQuality.FULLHD:
        this.videoUrl = this.newRequestResults.fullHdQuality;
        break;
      case YoutubeSettings.YoutubeVideoQuality.UHD1440:
        this.videoUrl = this.newRequestResults.fullHdQuality;
        break;
      case YoutubeSettings.YoutubeVideoQuality.UHD2160:
        this.videoUrl = this.newRequestResults.ultraHdQuality;
        break;
    }
    if (this.videoUrl == "")
      this.videoUrl = this.newRequestResults.bestFormatWithAudioIncluded;
    this.videoAreReadyToPlay = true;
    this.OnYoutubeUrlsLoaded();
  }

  public string ConvertToWebglUrl(string url)
  {
    string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(url));
    if (this.debug)
      Debug.Log((object) url);
    return "https://lswebgldemo.herokuapp.com/download.php?mime=video/mp4&title=generatedvideo&token=" + base64String;
  }

  public void RetryPlayYoutubeVideo()
  {
    Debug.Log((object) "Retry Load Video");
    this.Stop();
    ++this.currentRetryTime;
    this.logTest = "Retry!!";
    if (this.currentRetryTime < this.retryTimeUntilToRequestFromServer)
    {
      if (this.ForceGetWebServer)
        return;
      this.StopIfPlaying();
      if (this.debug)
        Debug.Log((object) ("Youtube Retrying...:" + this.lastTryVideoId));
      this.logTest = "retry";
      this.isRetry = true;
      this.ShowLoading();
      this.youtubeUrl = this.lastTryVideoId;
      this.PlayYoutubeVideo(this.youtubeUrl);
    }
    else
    {
      this.currentRetryTime = 0;
      this.StopIfPlaying();
      if (this.debug)
        Debug.Log((object) ("Youtube Retrying...:" + this.lastTryVideoId));
      this.logTest = "retry";
      this.isRetry = true;
      this.ShowLoading();
      this.youtubeUrl = this.lastTryVideoId;
      this.PlayYoutubeVideo(this.youtubeUrl);
    }
  }

  public void StopIfPlaying()
  {
    if (this.loadYoutubeUrlsOnly)
      return;
    if (this.debug)
      Debug.Log((object) "Stopping video");
    if (this.videoPlayer.isPlaying)
      this.videoPlayer.Stop();
    if (!((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null) || !this.audioPlayer.isPlaying)
      return;
    this.audioPlayer.Stop();
  }

  public void OnYoutubeUrlsLoaded()
  {
    this.youtubeUrlReady = true;
    int num = this.decryptionNeeded ? 1 : 0;
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.videoUrl = this.audioUrl;
    if (this.loadYoutubeUrlsOnly)
    {
      Debug.Log((object) ("Url Generated to play, you can use the event callback: " + this.videoUrl));
      if ((UnityEngine.Object) this._events != (UnityEngine.Object) null)
        this._events.OnYoutubeUrlAreReady.Invoke(this.videoUrl);
    }
    if (!this.loadYoutubeUrlsOnly)
    {
      if (this.debug)
        Debug.Log((object) ("Url Generated to play!!" + this.videoUrl));
      this.startedPlayingWebgl = false;
      this.videoPlayer.source = VideoSource.Url;
      this.videoPlayer.url = this.videoUrl;
      this.videoPlayer.EnableAudioTrack((ushort) 0, true);
      this.videoPlayer.SetTargetAudioSource((ushort) 0, this.videoPlayer.GetComponent<AudioSource>());
      this.videoPlayer.Prepare();
      if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
        return;
      this.audioPlayer.source = VideoSource.Url;
      this.audioPlayer.url = this.audioUrl;
      this.audioPlayer.Prepare();
    }
    else
    {
      if (!this.playUsingInternalDevicePlayer)
        return;
      this.StartCoroutine((IEnumerator) this.HandHeldPlayback());
    }
  }

  public void CheckIfCanPlayUrl(string url, System.Action callback)
  {
    this.StartCoroutine((IEnumerator) this.VideoRequest(url, callback));
  }

  public IEnumerator VideoRequest(string url, System.Action callback)
  {
    UnityWebRequest request = UnityWebRequest.Head(url);
    yield return (object) request.SendWebRequest();
    if (int.Parse(request.GetResponseHeader("Content-Length")) > 0)
    {
      callback();
    }
    else
    {
      Debug.Log((object) "Retry... WindowsMediaXError!");
      this.RetryPlayYoutubeVideo();
    }
  }

  public void PrepareVideoAfterCheck()
  {
    this.videoPlayer.Prepare();
    this._events.OnYoutubeUrlAreReady.Invoke(this.videoUrl);
  }

  public void PrepareAudioAfterCheck() => this.audioPlayer.Prepare();

  public IEnumerator PreventFinishToBeCalledTwoTimes()
  {
    yield return (object) new WaitForSeconds(1f);
    this.finishedCalled = false;
  }

  public void PlaybackDone(VideoPlayer vPlayer)
  {
    this.videoStarted = false;
    this._events.OnVideoFinished.Invoke();
  }

  public void VideoStarted(VideoPlayer source)
  {
    if (this.videoStarted)
      return;
    this.lastStartedTime = Time.time;
    this.lastErrorTime = this.lastStartedTime;
    if (!this.debug)
      return;
    Debug.Log((object) "Youtube Video Started");
  }

  public void VideoErrorReceived(VideoPlayer source, string message)
  {
  }

  public void Pause()
  {
    this.pauseCalled = true;
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
    {
      this.videoPlayer.Pause();
    }
    else
    {
      this.audioPlayer.GetTargetAudioSource((ushort) 0).volume = 0.0f;
      this.videoPlayer.Pause();
      this.audioPlayer.Pause();
      this.audioPlayer.time = this.videoPlayer.time;
    }
    this._events.OnVideoPaused.Invoke();
  }

  public void Update()
  {
    if (this.loadYoutubeUrlsOnly || !this._controller.showPlayerControl || this._controller.hideScreenControlTime <= 0)
      return;
    if (this.UserInteract())
    {
      this.hideScreenTime = 0.0f;
      if (!((UnityEngine.Object) this._controller.controllerMainUI != (UnityEngine.Object) null))
        return;
      this._controller.controllerMainUI.SetActive(true);
    }
    else
    {
      this.hideScreenTime += Time.deltaTime;
      if ((double) this.hideScreenTime < (double) this._controller.hideScreenControlTime)
        return;
      this.hideScreenTime = (float) this._controller.hideScreenControlTime;
      this._controller.HideControllers();
    }
  }

  public void Seek(float time)
  {
    this.waitAudioSeek = true;
    this.Pause();
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.videoPlayer.time = (double) time;
    else
      this.audioPlayer.time = (double) time;
  }

  public void VideoPreparedSeek(VideoPlayer p)
  {
  }

  public void AudioPreparedSeek(VideoPlayer p)
  {
  }

  public void Stop()
  {
    this.prepareVideoToPlayLater = false;
    this.startedPlayingWebgl = false;
    if (this.playUsingInternalDevicePlayer)
      return;
    if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
      this.audioPlayer.seekCompleted -= new VideoPlayer.EventHandler(this.AudioSeeked);
    this.videoPlayer.seekCompleted -= new VideoPlayer.EventHandler(this.VideoSeeked);
    this.videoPlayer.frameDropped -= new VideoPlayer.EventHandler(this.VideoPlayer_frameDropped);
    if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
      this.audioPlayer.frameDropped -= new VideoPlayer.EventHandler(this.AudioPlayer_frameDropped);
    this.videoPlayer.Stop();
    if (this.lowRes || !((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null))
      return;
    this.audioPlayer.Stop();
  }

  public void SeekVideoDone(VideoPlayer vp)
  {
    this.videoSeekDone = true;
    this.videoPlayer.seekCompleted -= new VideoPlayer.EventHandler(this.SeekVideoDone);
    if (!this.lowRes)
    {
      if (!this.videoSeekDone || !this.videoAudioSeekDone)
        return;
      this.isSyncing = false;
      this.StartCoroutine((IEnumerator) this.SeekFinished());
    }
    else
    {
      this.isSyncing = false;
      this.HideLoading();
    }
  }

  public void SeekVideoAudioDone(VideoPlayer vp)
  {
    Debug.Log((object) "NAAN");
    this.videoAudioSeekDone = true;
    this.audioPlayer.seekCompleted -= new VideoPlayer.EventHandler(this.SeekVideoAudioDone);
    if (this.lowRes || !this.videoSeekDone || !this.videoAudioSeekDone)
      return;
    this.isSyncing = false;
    this.StartCoroutine((IEnumerator) this.SeekFinished());
  }

  public IEnumerator SeekFinished()
  {
    yield return (object) new WaitForEndOfFrame();
    this.HideLoading();
  }

  public string FormatTime(int time)
  {
    int num1 = time / 3600;
    int num2 = time % 3600 / 60;
    int num3 = time % 3600 % 60;
    if (num1 == 0 && num2 != 0)
      return $"{num2.ToString("00")}:{num3.ToString("00")}";
    if (num1 == 0 && num2 == 0)
      return "00:" + num3.ToString("00");
    return $"{num1.ToString("00")}:{num2.ToString("00")}:{num3.ToString("00")}";
  }

  public bool UserInteract()
  {
    return Application.isMobilePlatform ? Input.touches.Length >= 1 : Input.GetMouseButtonDown(0) || (double) Input.GetAxis("Mouse X") != 0.0 || (double) Input.GetAxis("Mouse Y") != 0.0;
  }

  public void DecryptDownloadUrl(
    string encryptedUrlVideo,
    string encrytedUrlAudio,
    string html,
    bool videoOnly)
  {
    YoutubeSettings.jsDownloaded = false;
    this.EncryptUrlForAudio = encrytedUrlAudio;
    this.EncryptUrlForVideo = encryptedUrlVideo;
    HTTPHelperYoutube.ParseQueryString(this.EncryptUrlForVideo);
    if (videoOnly)
    {
      string[] strArray = this.EncryptUrlForVideo.Replace("&sig=", "|").Replace("lsig=", "|").Replace("&ratebypass=yes", "").Split('|', StringSplitOptions.None);
      this.lsigForVideo = strArray[strArray.Length - 2];
      this.encryptedSignatureVideo = strArray[strArray.Length - 1];
      this.StartCoroutine((IEnumerator) this.Downloader(YoutubeSettings.jsUrl, false, false));
    }
    else
    {
      string encryptUrlForVideo = this.EncryptUrlForVideo;
      if (this.debug)
      {
        Debug.Log((object) ("Encrypted Normal: " + encryptUrlForVideo));
        Debug.Log((object) ("Encrypted Changed: " + encryptUrlForVideo));
      }
      string[] strArray1 = encryptUrlForVideo.Replace("&sig=", "|").Replace("lsig=", "|").Replace("&ratebypass=yes", "").Split('|', StringSplitOptions.None);
      this.lsigForVideo = strArray1[strArray1.Length - 2];
      this.encryptedSignatureVideo = strArray1[strArray1.Length - 1];
      string[] strArray2 = this.EncryptUrlForAudio.Replace("&sig=", "|").Replace("lsig=", "|").Replace("&ratebypass=yes", "").Split('|', StringSplitOptions.None);
      this.lsigForAudio = strArray2[strArray2.Length - 2];
      this.encryptedSignatureAudio = strArray2[strArray2.Length - 1];
      this.StartCoroutine((IEnumerator) this.Downloader(YoutubeSettings.jsUrl, true, false));
    }
  }

  public void ReadyForExtract(string r, bool audioExtract)
  {
    if (audioExtract)
    {
      this.SetMasterUrlForAudio(r);
      if (SystemInfo.processorCount > 1)
      {
        this.thread1 = new Thread((ThreadStart) (() => this.DoRegexFunctionsForAudio(r)));
        this.thread1.Start();
      }
      else
        this.DoRegexFunctionsForAudio(r);
    }
    else
    {
      this.SetMasterUrlForVideo(r);
      if (SystemInfo.processorCount > 1)
      {
        this.thread1 = new Thread((ThreadStart) (() => this.DoRegexFunctionsForAudio(r)));
        this.thread1.Start();
      }
      else
        this.DoRegexFunctionsForAudio(r);
    }
  }

  public void SaveToCache(string name, string js)
  {
    string path = Application.persistentDataPath + "/utubejs/";
    if (!Directory.Exists(Path.GetDirectoryName(path)))
    {
      Directory.CreateDirectory(Path.GetDirectoryName(path));
      Debug.Log((object) "Creating now");
    }
    else
      Debug.Log((object) (path + " does exist"));
    try
    {
      File.WriteAllText($"{path}/{name}.js", js);
      Debug.Log((object) ("Saved Data to: " + path.Replace("/", "\\")));
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ("Failed To Save Data to: " + path.Replace("/", "\\")));
      Debug.LogWarning((object) ("Error: " + ex.Message));
    }
  }

  public string LoadFromCache(string name)
  {
    string path = $"{Application.persistentDataPath}/utubejs/{name}.js";
    return File.Exists(Path.GetDirectoryName(path)) ? File.ReadAllText(path) ?? (string) null : (string) null;
  }

  public IEnumerator Downloader(string uri, bool audio, bool loadJsOnly)
  {
    UnityWebRequest request;
    if (loadJsOnly)
    {
      request = UnityWebRequest.Get(uri);
      yield return (object) request.SendWebRequest();
      YoutubeSettings.jsUrlDownloaded = request.downloadHandler.text;
      YoutubeSettings.jsDownloaded = true;
      request = (UnityWebRequest) null;
    }
    else
    {
      if (this.debug)
        Debug.Log((object) YoutubeSettings.jsDownloaded);
      if (!YoutubeSettings.jsDownloaded)
      {
        request = UnityWebRequest.Get(uri);
        yield return (object) request.SendWebRequest();
        YoutubeSettings.jsUrlDownloaded = request.downloadHandler.text;
        YoutubeSettings.jsDownloaded = true;
        this.ReadyForExtract(YoutubeSettings.jsUrlDownloaded, audio);
        request = (UnityWebRequest) null;
      }
      else
      {
        yield return (object) new WaitForSeconds(0.1f);
        this.ReadyForExtract(YoutubeSettings.jsUrlDownloaded, audio);
      }
    }
  }

  public IEnumerator WebGlRequest(string videoID)
  {
    UnityWebRequest request = UnityWebRequest.Get($"https://youtube-dl-server-docker2-sx7c45rkxa-uc.a.run.app/api/info?url={videoID}&format=best&flatten=true");
    yield return (object) request.SendWebRequest();
    this.startedPlayingWebgl = false;
    this.webGlResults = new YoutubeResultIds();
    JSONNode jsonNode1 = JSON.Parse(request.downloadHandler.text);
    JSONNode jsonNode2 = jsonNode1["videos"][0]["formats"];
    this.webGlResults.bestFormatWithAudioIncluded = (string) jsonNode1["videos"][0]["url"];
    for (int aIndex = 0; aIndex < jsonNode2.Count; ++aIndex)
    {
      if (jsonNode2[aIndex]["format_id"] == (object) "160")
        this.webGlResults.lowQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "133")
        this.webGlResults.lowQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "134")
        this.webGlResults.standardQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "136")
        this.webGlResults.hdQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "137")
        this.webGlResults.fullHdQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "266")
        this.webGlResults.ultraHdQuality = (string) jsonNode2[aIndex]["url"];
      else if (jsonNode2[aIndex]["format_id"] == (object) "139")
        this.webGlResults.audioUrl = (string) jsonNode2[aIndex]["url"];
    }
    this.WebGlGetVideo(this.webGlResults.bestFormatWithAudioIncluded);
  }

  public void WebGlGetVideo(string url)
  {
    this.logTest = "Getting Url Player";
    this.videoUrl = "https://lswebgldemo.herokuapp.com/download.php?mime=video/mp4&title=generatedvideo&token=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(url));
    this.videoQuality = YoutubeSettings.YoutubeVideoQuality.STANDARD;
    this.logTest = this.videoUrl + " Done";
    Debug.Log((object) ("Play!! " + this.videoUrl));
    this.videoPlayer.source = VideoSource.Url;
    this.videoPlayer.url = this.videoUrl;
    this.videoPlayer.Prepare();
    this.videoPlayer.prepareCompleted += new VideoPlayer.EventHandler(this.WeblPrepared);
  }

  public void WeblPrepared(VideoPlayer source)
  {
    this.startedPlayingWebgl = true;
    this.StartCoroutine((IEnumerator) this.WebGLPlay());
    this.logTest = "Playing!!";
  }

  public IEnumerator WebGLPlay()
  {
    yield return (object) new WaitForSeconds(2f);
    this.StartPlayingWebgl();
  }

  public void OnGUI()
  {
    if (!this.debug)
      return;
    GUI.Label(new Rect(0.0f, 0.0f, 400f, 30f), this.logTest);
  }

  public void SetMasterUrlForAudio(string url) => this.masterURLForAudio = url;

  public void SetMasterUrlForVideo(string url) => this.masterURLForVideo = url;

  public static int GetOpIndex(string op) => int.Parse(new Regex(".(\\d+)").Match(op).Result("$1"));

  public static char[] SpliceFunction(char[] a, int b) => a.Splice<char>(b);

  public static char[] SwapFunction(char[] a, int b)
  {
    char ch = a[0];
    a[0] = a[b % a.Length];
    a[b % a.Length] = ch;
    return a;
  }

  public static char[] ReverseFunction(char[] a)
  {
    Array.Reverse<char>(a);
    return a;
  }

  public void DoRegexFunctionsForAudio(string jsF)
  {
    this.masterURLForAudio = jsF;
    string masterUrlForAudio = this.masterURLForAudio;
    Debug.Log((object) ("CIPHER URL: " + this.masterURLForAudio));
    YoutubeSettings.WriteLog("CIPHER", masterUrlForAudio);
    string pattern = "(?:\\.get\\(\"n\"\\)\\)&&\\(b=|(?:b=String\\.fromCharCode\\(110\\)|(?<str_idx>[a-zA-Z0-9_$.]+)&&\\(b=\"nn\"\\[\\+(?<str_idx>)\\])(?:,[a-zA-Z0-9_$]+\\(a\\))?,c=a\\.(?:get\\(b\\)|[a-zA-Z0-9_$]+\\[b\\]\\|\\|null)\\)&&\\(c=|\\b(?<var>[a-zA-Z0-9_$]+)=)(?<nfunc>[a-zA-Z0-9_$]+)(?:\\[(?<idx>\\d+)\\])?\\([a-zA-Z]\\)(?(var),[a-zA-Z0-9_$]+\\.set\\((?:\"n+\"|[a-zA-Z0-9_$]+),\\k<var>\\))";
    string input1 = Regex.Match(masterUrlForAudio, pattern).Groups[0].Value;
    Debug.Log((object) ("CIPHER SITE: " + input1));
    string str1 = Regex.Match(input1, "([$_\\w]+)\\.[$_\\w]+\\([$_\\w]+,[$_\\w]+\\)").Groups[1].Value;
    Debug.Log((object) ("Container Name: " + str1));
    Debug.Log((object) ("CIPHER ESCAPE: " + Regex.Escape(str1)));
    string input2 = Regex.Match(masterUrlForAudio, $"var {Regex.Escape(str1)}={{.*?}};", RegexOptions.Singleline).Groups[0].Value;
    Debug.Log((object) ("CIPHER Defi: " + input2));
    string b1 = Regex.Match(input2, "([$_\\w]+):function\\([$_\\w]+,[$_\\w]+\\){+[^}]*?%[^}]*?}", RegexOptions.Singleline).Groups[1].Value;
    string b2 = Regex.Match(input2, "([$_\\w]+):function\\([$_\\w]+,[$_\\w]+\\){+[^}]*?splice[^}]*?}", RegexOptions.Singleline).Groups[1].Value;
    string b3 = Regex.Match(input2, "([$_\\w]+):function\\([$_\\w]+\\){+[^}]*?reverse[^}]*?}", RegexOptions.Singleline).Groups[1].Value;
    input2.Split(';', StringSplitOptions.None);
    Debug.Log((object) $"{b3} {b2} {b1}");
    string operations = "";
    foreach (string input3 in input1.Split(';', StringSplitOptions.None))
    {
      string a = Regex.Match(input3, "[$_\\w]+\\.([$_\\w]+)\\([$_\\w]+,\\d+\\)").Groups[1].Value;
      if (!string.IsNullOrWhiteSpace(a))
      {
        if (string.Equals(a, b1, StringComparison.Ordinal))
        {
          string str2 = Regex.Match(input3, "\\([$_\\w]+,(\\d+)\\)").Groups[1].Value;
          operations = $"{operations}w{str2} ";
        }
        else if (string.Equals(a, b2, StringComparison.Ordinal))
        {
          string str3 = Regex.Match(input3, "\\([$_\\w]+,(\\d+)\\)").Groups[1].Value;
          operations = $"{operations}s{str3} ";
        }
        else if (string.Equals(a, b3, StringComparison.Ordinal))
          operations += "r ";
      }
    }
    if (string.IsNullOrEmpty(operations))
    {
      if (this.canUpdate)
        this.canUpdate = false;
      this.decryptedAudioUrlResult = (string) null;
      this.decryptedVideoUrlResult = (string) null;
    }
    else
    {
      if (this.encryptedSignatureAudio != "")
      {
        string newValue = MagicHands.DecipherWithOperations(this.encryptedSignatureAudio, operations);
        this.decryptedAudioUrlResult = HTTPHelperYoutube.ReplaceQueryStringParameter(this.EncryptUrlForAudio, YoutubeSettings.SignatureQuery, newValue, this.lsigForAudio);
        this.decryptedUrlForAudio = true;
      }
      string newValue1 = MagicHands.DecipherWithOperations(this.encryptedSignatureVideo, operations);
      this.decryptedVideoUrlResult = HTTPHelperYoutube.ReplaceQueryStringParameter(this.EncryptUrlForVideo, YoutubeSettings.SignatureQuery, newValue1, this.lsigForVideo);
      this.decryptedUrlForVideo = true;
    }
  }

  public void DelayForAudio() => this.decryptedUrlForVideo = true;

  public static string GetFunctionFromLine(string currentLine)
  {
    return new Regex("\\w+\\.(?<functionID>\\w+)\\(").Match(currentLine).Groups["functionID"].Value;
  }

  public IEnumerator WebGlRequest(Action<string> callback, string id, string host)
  {
    UnityWebRequest request = UnityWebRequest.Get($"{host}getvideo.php?videoid={id}&type=Download");
    yield return (object) request.SendWebRequest();
    callback(request.downloadHandler.text);
  }

  public void GetDownloadUrls(System.Action callback, string videoUrl, YoutubeSettings player)
  {
    if (videoUrl == null)
      throw new ArgumentNullException(nameof (videoUrl));
    if (!YoutubeSettings.TryNormalizeYoutubeUrl(videoUrl, out videoUrl))
      throw new ArgumentException("URL is not a valid youtube URL!");
    this.StartCoroutine((IEnumerator) this.DownloadYoutubeUrl(videoUrl, callback, player));
  }

  public IEnumerator YoutubeURLDownloadFinished(System.Action callback, YoutubeSettings player)
  {
    YoutubeSettings youtubeSettings = this;
    string str = youtubeSettings.youtubeUrl.Replace("https://youtube.com/watch?v=", "");
    string json1 = string.Empty;
    bool flag = false;
    if (Regex.IsMatch(youtubeSettings.jsonForHtmlVersion, "[\"\\']status[\"\\']\\s*:\\s*[\"\\']LOGIN_REQUIRED") | flag)
    {
      UnityWebRequest request = UnityWebRequest.Get($"https://www.docs.google.com/get_video_info?video_id={str}&eurl=https://youtube.googleapis.com/v/{str}&html5=1&c=TVHTML5&cver=6.20180913");
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
      Match match1 = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;\\s*(?:var\\s+meta|</script|\\n)", RegexOptions.Multiline).Match(youtubeSettings.jsonForHtmlVersion);
      if (match1.Success)
      {
        string json2 = match1.Result("$1");
        if (!json2.Contains("raw_player_response:ytInitialPlayerResponse"))
          json1 = JObject.Parse(json2).ToString();
      }
      Match match2 = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;\\s*(?:var\\s+meta|</script|\\n)", RegexOptions.Multiline).Match(youtubeSettings.jsonForHtmlVersion);
      if (match2.Success)
        json1 = match2.Result("$1");
      Match match3 = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;\\s*(?:var\\s+meta|</script|\\n)", RegexOptions.Multiline).Match(youtubeSettings.jsonForHtmlVersion);
      if (match3.Success)
        json1 = match3.Result("$1");
      Match match4 = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;", RegexOptions.Multiline).Match(youtubeSettings.jsonForHtmlVersion);
      if (match4.Success)
        json1 = match4.Result("$1");
    }
    JObject json3 = JObject.Parse(json1);
    if (youtubeSettings.downloadYoutubeUrlResponse.isValid)
    {
      if (YoutubeSettings.IsVideoUnavailable(youtubeSettings.downloadYoutubeUrlResponse.data))
        throw new VideoNotAvailableException();
      try
      {
        List<Regex> regexList = new List<Regex>();
        regexList.Add(new Regex("\\/s\\/player\\/([a-zA-Z0-9_-]{8,})\\/player"));
        string input = "";
        foreach (Regex regex in regexList)
        {
          if (regex.IsMatch(youtubeSettings.jsonForHtmlVersion))
          {
            input = regex.Match(youtubeSettings.jsonForHtmlVersion).Result("$1").Replace("\\/", "/");
            break;
          }
        }
        if (input.Contains("player-plasma-ias-phone-en_US.vflset/base.js"))
          input = input.Replace("player-plasma-ias-phone-en_US.vflset/base.js", "player_ias.vflset/en_US/base.js");
        youtubeSettings.currentJSName = input;
        YoutubeSettings.jsUrl = $"https://www.youtube.com/s/player/{input}/player_ias.vflset/en_US/base.js";
        if (youtubeSettings.debug)
          Debug.Log((object) YoutubeSettings.jsUrl);
        youtubeSettings.testinguri = YoutubeSettings.jsUrl;
        if (youtubeSettings.olderVersionEnable)
        {
          youtubeSettings.StartCoroutine((IEnumerator) youtubeSettings.Downloader(YoutubeSettings.jsUrl, false, true));
          string message = YoutubeSettings.TryMatchHtmlVersion(input, youtubeSettings.magicResult.defaultHtmlPlayerVersion);
          if (youtubeSettings.debug)
            Debug.Log((object) message);
          youtubeSettings.htmlVersion = message;
          string videoTitle = YoutubeSettings.GetVideoTitle(json3);
          if (youtubeSettings.debug)
            Debug.Log((object) videoTitle);
          List<VideoInfo> list = YoutubeSettings.GetVideoInfos(YoutubeSettings.ExtractDownloadUrls(json3), videoTitle).ToList<VideoInfo>();
          if (string.IsNullOrEmpty(youtubeSettings.htmlVersion))
            youtubeSettings.RetryPlayYoutubeVideo();
          youtubeSettings.youtubeVideoInfos = list;
          foreach (VideoInfo youtubeVideoInfo in youtubeSettings.youtubeVideoInfos)
            youtubeVideoInfo.HtmlPlayerVersion = message;
          callback();
        }
        else
        {
          List<VideoInfo> list = YoutubeSettings.GetVideoInfos(YoutubeSettings.ExtractDownloadUrls(json3), youtubeSettings.videoTitle).ToList<VideoInfo>();
          youtubeSettings.youtubeVideoInfos = list;
          foreach (VideoInfo youtubeVideoInfo in youtubeSettings.youtubeVideoInfos)
            youtubeVideoInfo.DownloadUrl = UnityWebRequest.UnEscapeURL(youtubeVideoInfo.DownloadUrl);
          if (youtubeSettings.debug)
            Debug.Log((object) "I have the js uri");
          callback();
        }
      }
      catch (Exception ex)
      {
        if (!youtubeSettings.loadYoutubeUrlsOnly)
        {
          if (youtubeSettings.debug)
            Debug.Log((object) $"Error: {ex}");
          if ((UnityEngine.Object) player != (UnityEngine.Object) null)
            player.RetryPlayYoutubeVideo();
          else
            Debug.LogError((object) "Connection to Youtube Server Error! Try Again");
        }
      }
    }
  }

  public static bool TryNormalizeYoutubeUrl(string url, out string normalizedUrl)
  {
    url = url.Trim();
    url = url.Replace("youtu.be/", "youtube.com/watch?v=");
    url = url.Replace("www.youtube", "youtube");
    url = url.Replace("youtube.com/embed/", "youtube.com/watch?v=");
    if (url.Contains("/v/"))
      url = "https://youtube.com" + new Uri(url).AbsolutePath.Replace("/v/", "/watch?v=");
    url = url.Replace("/watch#", "/watch?");
    string str;
    if (!HTTPHelperYoutube.ParseQueryString(url).TryGetValue("v", out str))
    {
      normalizedUrl = (string) null;
      return false;
    }
    normalizedUrl = "https://youtube.com/watch?v=" + str;
    return true;
  }

  public static IEnumerable<YoutubeSettings.ExtractionInfo> ExtractDownloadUrls(JObject json)
  {
    List<string> urls = new List<string>();
    List<string> stringList = new List<string>();
    JObject jobject = json;
    if (jobject["streamingData"][(object) "formats"] != null)
    {
      if (jobject["streamingData"][(object) "formats"][(object) 0][(object) "cipher"] != null)
      {
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["streamingData"][(object) "formats"])
          stringList.Add(jtoken[(object) "cipher"].ToString());
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["streamingData"][(object) "adaptiveFormats"])
          stringList.Add(jtoken[(object) "cipher"].ToString());
      }
      else if (jobject["streamingData"][(object) "formats"][(object) 0][(object) "signatureCipher"] != null)
      {
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["streamingData"][(object) "formats"])
          stringList.Add(jtoken[(object) "signatureCipher"].ToString());
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["streamingData"][(object) "adaptiveFormats"])
          stringList.Add(jtoken[(object) "signatureCipher"].ToString());
      }
      else
      {
        if (jobject["streamingData"][(object) "formats"] != null)
        {
          foreach (JToken jtoken in (IEnumerable<JToken>) jobject["streamingData"][(object) "formats"])
            urls.Add(jtoken[(object) "url"].ToString());
        }
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["streamingData"][(object) "adaptiveFormats"])
        {
          if (jtoken[(object) "itag"].ToString() == "134" && jtoken[(object) "projectionType"] != null)
            YoutubeSettings.projectionType = jtoken[(object) "projectionType"].ToString();
          urls.Add(jtoken[(object) "url"].ToString());
        }
      }
    }
    else
    {
      if (jobject["streamingData"][(object) "formats"] != null)
      {
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["streamingData"][(object) "formats"])
          urls.Add(jtoken[(object) "url"].ToString());
      }
      foreach (JToken jtoken in (IEnumerable<JToken>) jobject["streamingData"][(object) "adaptiveFormats"])
      {
        if (jtoken[(object) "itag"].ToString() == "134" && jtoken[(object) "projectionType"] != null)
          YoutubeSettings.projectionType = jtoken[(object) "projectionType"].ToString();
        urls.Add(jtoken[(object) "url"].ToString());
      }
    }
    foreach (string s in stringList)
    {
      IDictionary<string, string> queryString = HTTPHelperYoutube.ParseQueryString(s);
      bool flag = false;
      YoutubeSettings.SignatureQuery = !queryString.ContainsKey("sp") ? "signatures" : "sig";
      string url;
      if (queryString.ContainsKey("s") || queryString.ContainsKey("signature"))
      {
        flag = queryString.ContainsKey("s");
        string str = queryString.ContainsKey("s") ? queryString["s"] : queryString["signature"];
        url = (!(YoutubeSettings.sp != "none") ? $"{queryString["url"]}&{YoutubeSettings.SignatureQuery}={str}" : $"{queryString["url"]}&{YoutubeSettings.SignatureQuery}={str}") + (queryString.ContainsKey("fallback_host") ? "&fallback_host=" + queryString["fallback_host"] : string.Empty);
      }
      else
        url = queryString["url"];
      string str1 = HTTPHelperYoutube.UrlDecode(url);
      if (!HTTPHelperYoutube.ParseQueryString(str1).ContainsKey("ratebypass"))
        str1 += $"&{"ratebypass"}={"yes"}";
      yield return new YoutubeSettings.ExtractionInfo()
      {
        RequiresDecryption = flag,
        Uri = new Uri(str1)
      };
    }
    foreach (string url in urls)
    {
      string str = HTTPHelperYoutube.UrlDecode(HTTPHelperYoutube.UrlDecode(url));
      if (!HTTPHelperYoutube.ParseQueryString(str).ContainsKey("ratebypass"))
        str += $"&{"ratebypass"}={"yes"}";
      yield return new YoutubeSettings.ExtractionInfo()
      {
        RequiresDecryption = false,
        Uri = new Uri(str)
      };
    }
  }

  public static string GetAdaptiveStreamMap(JObject json)
  {
    return (json["args"][(object) "adaptive_fmts"] ?? json["args"][(object) "url_encoded_fmt_stream_map"] ?? JObject.Parse(json["args"][(object) "player_response"].ToString())["streamingData"][(object) "adaptiveFormats"]).ToString();
  }

  public static string GetHtml5PlayerVersion(JObject json, string regexForHtmlPVersions)
  {
    Regex regex = new Regex(regexForHtmlPVersions);
    string input1 = json["assets"][(object) "js"].ToString();
    YoutubeSettings.jsUrl = "https://www.youtube.com" + input1;
    string input2 = input1;
    Match match1 = regex.Match(input2);
    if (match1.Success)
      return match1.Result("$1");
    Match match2 = new Regex("player_ias(.+?).js").Match(input1);
    return match2.Success ? match2.Result("$1") : new Regex("player-(.+?).js").Match(input1).Result("$1");
  }

  public static string TryMatchHtmlVersion(string input, string regexForHtmlPVersions)
  {
    Match match1 = new Regex(regexForHtmlPVersions).Match(input);
    if (match1.Success)
      return match1.Result("$1");
    Match match2 = new Regex("player_ias(.+?).js").Match(input);
    return match2.Success ? match2.Result("$1") : new Regex("player-(.+?).js").Match(input).Result("$1");
  }

  public static string GetStreamMap(JObject json)
  {
    JToken jtoken = json["args"][(object) "url_encoded_fmt_stream_map"] ?? JObject.Parse(json["args"][(object) "player_response"].ToString())["streamingData"][(object) "formats"];
    string streamMap = jtoken == null ? (string) null : jtoken.ToString();
    if (streamMap != null && !streamMap.Contains("been+removed"))
      return streamMap;
    if (streamMap.Contains("been+removed"))
      throw new VideoNotAvailableException("Video is removed or has an age restriction.");
    return (string) null;
  }

  public static IEnumerable<VideoInfo> GetVideoInfos(
    IEnumerable<YoutubeSettings.ExtractionInfo> extractionInfos,
    string videoTitle)
  {
    List<VideoInfo> videoInfos = new List<VideoInfo>();
    foreach (YoutubeSettings.ExtractionInfo extractionInfo in extractionInfos)
    {
      int formatCode = int.Parse(HTTPHelperYoutube.ParseQueryString(extractionInfo.Uri.Query)["itag"]);
      VideoInfo info = VideoInfo.Defaults.SingleOrDefault<VideoInfo>((Func<VideoInfo, bool>) (videoInfo => videoInfo.FormatCode == formatCode));
      VideoInfo videoInfo1;
      if (info != null)
        videoInfo1 = new VideoInfo(info)
        {
          DownloadUrl = extractionInfo.Uri.ToString(),
          Title = videoTitle,
          RequiresDecryption = extractionInfo.RequiresDecryption
        };
      else
        videoInfo1 = new VideoInfo(formatCode)
        {
          DownloadUrl = extractionInfo.Uri.ToString()
        };
      videoInfos.Add(videoInfo1);
    }
    return (IEnumerable<VideoInfo>) videoInfos;
  }

  public static string GetVideoTitle(JObject json)
  {
    JToken jtoken = json["videoDetails"][(object) "title"];
    return jtoken != null ? jtoken.ToString() : string.Empty;
  }

  public static bool IsVideoUnavailable(string pageSource)
  {
    return pageSource.Contains("<div id=\"watch-player-unavailable\">");
  }

  public IEnumerator DownloadUrl(string url, Action<string> callback, VideoInfo videoInfo)
  {
    UnityWebRequest request = UnityWebRequest.Get(url);
    yield return (object) request.SendWebRequest();
    if (request.result == UnityWebRequest.Result.ConnectionError)
      Debug.Log((object) "Youtube UnityWebRequest isNetworkError!");
    else if (request.result == UnityWebRequest.Result.ProtocolError)
      Debug.Log((object) "Youtube UnityWebRequest isHttpError!");
    else if (request.responseCode != 200L)
      Debug.Log((object) ("Youtube UnityWebRequest responseCode:" + request.responseCode.ToString()));
  }

  public IEnumerator DownloadYoutubeUrl(string url, System.Action callback, YoutubeSettings player)
  {
    YoutubeSettings youtubeSettings = this;
    youtubeSettings.downloadYoutubeUrlResponse = new YoutubeSettings.DownloadUrlResponse();
    UnityWebRequest request = UnityWebRequest.Get($"https://www.youtube.com/watch?v={url.Replace("https://youtube.com/watch?v=", "")}&has_verified=1&bpctr=9999999999");
    request.SetRequestHeader("X-YouTube-Client-Name", "1");
    request.SetRequestHeader("X-YouTube-Client-Version", "2.20220801.00.00");
    yield return (object) request.SendWebRequest();
    youtubeSettings.downloadYoutubeUrlResponse.httpCode = request.responseCode;
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
          youtubeSettings.downloadYoutubeUrlResponse.isValid = true;
          youtubeSettings.jsonForHtmlVersion = request.downloadHandler.text;
          youtubeSettings.downloadYoutubeUrlResponse.data = request.downloadHandler.text;
        }
      }
      else
        Debug.Log((object) "Youtube UnityWebRequest Null response");
    }
    else
      Debug.Log((object) ("Youtube UnityWebRequest responseCode:" + request.responseCode.ToString()));
    youtubeSettings.StartCoroutine((IEnumerator) youtubeSettings.YoutubeURLDownloadFinished(callback, player));
  }

  public static void WriteLog(string filename, string c)
  {
    string path = $"C:/log/{filename}_{DateTime.Now.ToString("ddMMyyyyhhmmssffff")}.txt";
    Debug.Log((object) ("Log written in: " + path));
    File.WriteAllText(path, c);
  }

  public static void ThrowYoutubeParseException(Exception innerException, string videoUrl)
  {
    throw new YoutubeParseException($"Could not parse the Youtube page for URL {videoUrl}\nThis may be due to a change of the Youtube page structure.\nPlease report this bug at kelvinparkour@gmail.com with a subject message 'Parse Error' ", innerException);
  }

  public void TrySkip(PointerEventData eventData)
  {
    Vector2 localPoint;
    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._controller.progressRectangle.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
      return;
    this.SkipToPercent(Mathf.InverseLerp(this._controller.progressRectangle.rectTransform.rect.xMin, this._controller.progressRectangle.rectTransform.rect.xMax, localPoint.x));
  }

  public void SkipToPercent(float pct)
  {
    if (this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD)
    {
      this.oldVolume = this.videoPlayer.GetComponent<AudioSource>().volume;
      this.videoPlayer.GetComponent<AudioSource>().volume = 0.0f;
    }
    float num1 = (float) this.videoPlayer.frameCount * pct;
    float num2 = this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD ? (float) this.audioPlayer.frameCount * pct : (float) this.videoPlayer.frameCount * pct;
    this.videoPlayer.Pause();
    if (this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.audioPlayer.Pause();
    this.waitAudioSeek = true;
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.videoPlayer.frame = (long) num2;
    else
      this.audioPlayer.frame = (long) num2;
    this.videoPlayer.Pause();
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      return;
    this.audioPlayer.Pause();
  }

  public void CustomSeek(float time)
  {
    if (!this.videoPlayer.isPrepared)
      return;
    this.videoPlayer.Pause();
    if (this.videoQuality != YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.audioPlayer.Pause();
    this.waitAudioSeek = true;
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      this.videoPlayer.time = (double) time;
    else
      this.audioPlayer.time = (double) time;
    this.videoPlayer.Pause();
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      return;
    this.audioPlayer.Pause();
  }

  public IEnumerator VideoSeekCall()
  {
    yield return (object) new WaitForSeconds(1f);
    this.videoPlayer.time = this.audioPlayer.time;
  }

  public void VideoSeeked(VideoPlayer source)
  {
    if (!this.waitAudioSeek)
    {
      if (this.startedFromTime)
        this.StartCoroutine((IEnumerator) this.PlayNowFromTime(2f));
      else
        this.StartCoroutine((IEnumerator) this.PlayNow());
    }
    else if (this.startedFromTime)
      this.StartCoroutine((IEnumerator) this.PlayNowFromTime(2f));
    else
      this.StartCoroutine((IEnumerator) this.PlayNow());
  }

  public bool CheckIfJSIsDownloaded() => YoutubeSettings.jsDownloaded;

  public IEnumerator WaitJSDownload(VideoInfo videoInfo, string js)
  {
    YoutubeSettings youtubeSettings = this;
    if (youtubeSettings.debug)
      Debug.Log((object) "waiting js");
    yield return (object) new WaitUntil((Func<bool>) new Func<bool>(youtubeSettings.CheckIfJSIsDownloaded));
  }

  public void GetNewSystem()
  {
    this.decryptionNeeded = true;
    Debug.Log((object) "NEW SYSTEM");
    if (!this.BACKUPSYSTEM)
      this.StartCoroutine((IEnumerator) this.YoutubeGenerateUrlUsingClient(this.tmpv, this.GetFormatCode()));
    else
      this.GetDownloadUrls(new System.Action(this.UrlsLoaded), this.youtubeUrl, this);
  }

  public void GetTheJS()
  {
    if (this.debug)
      Debug.Log((object) "GOT THE JSURL");
    this.StartCoroutine((IEnumerator) this.YoutubeGenerateUrlUsingClient(this.tmpv, this.tmpf));
  }

  public IEnumerator GetNParamAudio(string dUrl, string htmlpv)
  {
    YoutubeSettings youtubeSettings = this;
    if (youtubeSettings.debug)
      Debug.Log((object) ("Descamble N :" + youtubeSettings.testinguri));
    IDictionary<string, string> queryString = HTTPHelperYoutube.ParseQueryString(youtubeSettings.audioUrl);
    if (queryString.ContainsKey("n"))
    {
      string str1 = queryString["n"];
      UnityWebRequest request = UnityWebRequest.Get($"https://yt-dlp-online-utils.vercel.app/youtube/nparams/decrypt?player={youtubeSettings.testinguri}&n={str1}");
      yield return (object) request.SendWebRequest();
      string newValue = (string) JSON.Parse(request.downloadHandler.text)["data"];
      string str2 = HTTPHelperYoutube.ReplaceQueryStringParameterx(youtubeSettings.audioUrl, "n", newValue);
      youtubeSettings.audioUrl = str2;
      youtubeSettings.audioUrl = youtubeSettings.audioUrl.Replace("googlevideo.com:443", "googlevideo.com");
      youtubeSettings.StartCoroutine((IEnumerator) youtubeSettings.GetNParam(dUrl, htmlpv));
      request = (UnityWebRequest) null;
    }
    else
    {
      if (youtubeSettings.debug)
        Debug.Log((object) "Dont have N");
      youtubeSettings.OnYoutubeUrlsLoaded();
    }
  }

  public IEnumerator GetNParam(string dUrl, string htmlpv)
  {
    UnityWebRequest request = UnityWebRequest.Get($"https://yt-dlp-online-utils.vercel.app/youtube/nparams/decrypt?player={this.testinguri}&n={HTTPHelperYoutube.ParseQueryString(dUrl)["n"]}");
    yield return (object) request.SendWebRequest();
    dUrl = HTTPHelperYoutube.ReplaceQueryStringParameterx(dUrl, "n", (string) JSON.Parse(request.downloadHandler.text)["data"]);
    this.videoUrl = dUrl;
    this.videoUrl = this.videoUrl.Replace("googlevideo.com:443", "googlevideo.com");
    this.videoUrl = this.videoUrl.Replace("&ratebypass=yes", "");
    this.videoAreReadyToPlay = true;
    this.OnYoutubeUrlsLoaded();
  }

  public IEnumerator ExtractorGetNParamAudio(string dAUrl, string dUrl)
  {
    YoutubeSettings youtubeSettings = this;
    Debug.Log((object) dUrl);
    IDictionary<string, string> queryString = HTTPHelperYoutube.ParseQueryString(dAUrl);
    if (queryString.ContainsKey("n"))
    {
      string str1 = queryString["n"];
      Debug.Log((object) $"https://yt-dlp-online-utils.vercel.app/youtube/nparams/decrypt?player={youtubeSettings.testinguri}&n={str1}");
      UnityWebRequest request = UnityWebRequest.Get($"https://yt-dlp-online-utils.vercel.app/youtube/nparams/decrypt?player={youtubeSettings.testinguri}&n={str1}");
      yield return (object) request.SendWebRequest();
      string str2 = HTTPHelperYoutube.ReplaceQueryStringParameterx(dAUrl, "n", (string) JSON.Parse(request.downloadHandler.text)["data"]);
      youtubeSettings.audioUrl = str2;
      youtubeSettings.audioUrl = youtubeSettings.audioUrl.Replace("googlevideo.com:443", "googlevideo.com");
      youtubeSettings.StartCoroutine((IEnumerator) youtubeSettings.ExtractorGetNParam(dUrl));
      request = (UnityWebRequest) null;
    }
  }

  public IEnumerator ExtractorGetNParam(string dUrl)
  {
    IDictionary<string, string> strs = HTTPHelperYoutube.ParseQueryString(dUrl);
    if (!strs.ContainsKey("n"))
    {
      this.RetryPlayYoutubeVideo();
      yield return (object) null;
    }
    UnityWebRequest request = UnityWebRequest.Get($"https://yt-dlp-online-utils.vercel.app/youtube/nparams/decrypt?player={this.testinguri}&n={strs["n"]}");
    yield return (object) request.SendWebRequest();
    dUrl = HTTPHelperYoutube.ReplaceQueryStringParameterx(dUrl, "n", (string) JSON.Parse(request.downloadHandler.text)["data"]);
    this.videoUrl = dUrl;
    this.videoUrl = this.videoUrl.Replace("googlevideo.com:443", "googlevideo.com");
    this.videoUrl = this.videoUrl.Replace("&ratebypass=yes", "");
    this.videoAreReadyToPlay = true;
    this.OnYoutubeUrlsLoaded();
  }

  public static void DecryptDownloadUrlNParam(VideoInfo videoInfo, string js)
  {
    string newValue = string.Empty;
    string n_param = HTTPHelperYoutube.ParseQueryString(videoInfo.DownloadUrl)["n"];
    try
    {
      newValue = YoutubeSettings.NDescramble(js, n_param);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
    videoInfo.DownloadUrl = HTTPHelperYoutube.ReplaceQueryStringParameter(videoInfo.DownloadUrl, "n", newValue);
  }

  public static string NDescramble(string js, string n_param)
  {
    js = YoutubeSettings.jsUrlDownloaded;
    if (string.IsNullOrEmpty(js))
    {
      Debug.Log((object) "is null");
      return n_param;
    }
    Debug.Log((object) $"js: {js}|n: {n_param}");
    Debug.Log((object) "tryibng");
    string message = new Regex("\\.get\\(\"n\"\\)\\)&&\\(b=(?<nfunc>[a-zA-Z0-9$]+)(\\[(?<idx>\\d+)\\])?\\([a-zA-Z0-9]\\)").Match(js).Groups["nfunc"].Value;
    Debug.Log((object) Regex.Match(js, "(?:function\\s+qha|[{;,]\\s*qha\\s*=\\s*function|var\\s+qha\\s*=\\s*function)\\s*\\(([^)]*)\\)\\s*(\\{(?:(?!};)[^\"]|\"([^\"]|\\\\\")*\")+\\})").Groups[0].Value);
    Debug.Log((object) message);
    Debug.Log((object) $"{message}('{n_param}')");
    return n_param;
  }

  public void AudioSeeked(VideoPlayer source)
  {
    if (!this.waitAudioSeek)
      this.StartCoroutine((IEnumerator) this.VideoSeekCall());
    else
      this.StartCoroutine((IEnumerator) this.VideoSeekCall());
  }

  public virtual void Play()
  {
    if (this.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      return;
    this.videoPlayer.GetComponent<AudioSource>().volume = this.oldVolume;
  }

  public IEnumerator WaitSync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    YoutubeSettings youtubeSettings = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      youtubeSettings.Play();
      youtubeSettings.Invoke("VerifyFrames", 2f);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator PlayNow()
  {
    YoutubeSettings youtubeSettings = this;
    if (youtubeSettings.videoQuality == YoutubeSettings.YoutubeVideoQuality.STANDARD)
      yield return (object) new WaitForSeconds(0.0f);
    else
      yield return (object) new WaitForSeconds(1f);
    if (!youtubeSettings.pauseCalled)
    {
      youtubeSettings.Play();
      youtubeSettings.StartCoroutine((IEnumerator) youtubeSettings.ReleaseDrop());
    }
    else
      youtubeSettings.StopCoroutine(nameof (PlayNow));
  }

  public void CheckIfIsSync()
  {
  }

  public IEnumerator ReleaseDrop()
  {
    yield return (object) new WaitForSeconds(2f);
  }

  public IEnumerator PlayNowFromTime(float time)
  {
    YoutubeSettings youtubeSettings = this;
    yield return (object) new WaitForSeconds(time);
    youtubeSettings.startedFromTime = false;
    if (!youtubeSettings.pauseCalled)
      youtubeSettings.Play();
    else
      youtubeSettings.StopCoroutine(nameof (PlayNowFromTime));
  }

  public void AudioPlayer_frameDropped(VideoPlayer source)
  {
  }

  public void VideoPlayer_frameDropped(VideoPlayer source)
  {
  }

  public void ChacheSystem()
  {
    PlayerPrefs.SetString("id", "url");
    PlayerPrefs.SetInt("id-expire", 1634364304);
  }

  public enum YoutubeVideoQuality
  {
    STANDARD,
    HD,
    FULLHD,
    UHD1440,
    UHD2160,
  }

  public enum VideoFormatType
  {
    MP4,
    WEBM,
  }

  public enum Layout3D
  {
    SideBySide,
    OverUnder,
    None,
    EAC,
    EAC3D,
  }

  public class Html5PlayerResult
  {
    public string scriptName;
    public string result;
    public bool isValid;

    public Html5PlayerResult(string _script, string _result, bool _valid)
    {
      this.scriptName = _script;
      this.result = _result;
      this.isValid = _valid;
    }
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

  public class ExtractionInfo
  {
    [CompilerGenerated]
    public bool \u003CRequiresDecryption\u003Ek__BackingField;
    [CompilerGenerated]
    public Uri \u003CUri\u003Ek__BackingField;

    public bool RequiresDecryption
    {
      get => this.\u003CRequiresDecryption\u003Ek__BackingField;
      set => this.\u003CRequiresDecryption\u003Ek__BackingField = value;
    }

    public Uri Uri
    {
      get => this.\u003CUri\u003Ek__BackingField;
      set => this.\u003CUri\u003Ek__BackingField = value;
    }
  }

  public class MagicContent
  {
    public string[] defaultFuncName = new string[3]
    {
      "(?:\\b|[^\\w$])([\\w$]{2})\\s*=\\s*function\\(\\s*a\\s*\\)\\s*{\\s*a\\s*=\\s*a\\.split\\(\\s*\"\"\\s*\\)",
      "(\\w+)=function\\(\\w+\\){(\\w+)=\\2\\.split\\(\\x22{2}\\);.*?return\\s+\\2\\.join\\(\\x22{2}\\)}",
      "\\b[cs]\\s*&&\\s*[adf]\\.set\\([^,]+\\s*,\\s*encodeURIComponent\\s*\\(\\s*([\\w$]+)\\("
    };
    public string defaultHtmlJson = "ytplayer\\.config\\s*=\\s*(\\{.+?\\});ytplayer";
    public string defaultHtmlPlayerVersion = "player_ias-(.+?).js";
  }
}
