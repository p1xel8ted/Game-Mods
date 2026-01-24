// Decompiled with JetBrains decompiler
// Type: LightShaft.Scripts.YoutubeSimplifiedRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.Video;
using YoutubeLight;

#nullable disable
namespace LightShaft.Scripts;

[RequireComponent(typeof (YoutubeVideoController))]
[RequireComponent(typeof (YoutubeVideoEvents))]
public class YoutubeSimplifiedRequest : MonoBehaviour
{
  public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.41 Safari/537.36";
  [HideInInspector]
  public YoutubeVideoController controller;
  [HideInInspector]
  public YoutubeVideoEvents events;
  [Space]
  [Tooltip("You can put urls that start at a specific time example: 'https://youtu.be/1G1nCxxQMnA?t=67'")]
  public string youtubeUrl;
  [Space]
  [Space]
  [Tooltip("The desired video quality you want to play. It's in experimental mod, because we need to use 2 video players in qualities 720+, you can expect some desync, but we are working to find a definitive solution to that. Thanks to DASH format.")]
  public YoutubeSimplifiedRequest.YoutubeVideoQuality videoQuality;
  [SerializeField]
  [Tooltip("This option force the video output to be mp4 if is available (Some users with Windows11 have issues in the editor )")]
  public bool _forceMP4;
  [Space]
  [Tooltip("If it is a 360 degree video")]
  public bool is360;
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
  public bool PrepareVideoToPlayLater;
  [Space]
  public bool showThumbnailBeforeVideoLoad;
  [DrawIf("showThumbnailBeforeVideoLoad", true, DrawIfAttribute.DisablingType.DontDraw)]
  public Renderer thumbnailObject;
  [Space]
  public bool customPlaylist;
  [DrawIf("customPlaylist", true, DrawIfAttribute.DisablingType.DontDraw)]
  public bool autoPlayNextVideo;
  [Header("If is a custom playlist put urls here")]
  public string[] youtubeUrls;
  public int CurrentUrlIndex;
  public string youtubeVideoID;
  public bool PLAYING;
  [Header("You can Try different formats")]
  public YoutubeSimplifiedRequest.VideoFormatType videoFormat;
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
  public YoutubeSimplifiedRequest.Layout3D layout3d;
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
  [FormerlySerializedAs("_skipOnDrop")]
  [Header("If you are having issues with sync try check this and change video format to WEBM")]
  public bool skipOnDrop;
  public MMButton StopButton;
  public MMButton[] buttons;
  [HideInInspector]
  public string videoUrl;
  [HideInInspector]
  public string audioUrl;
  [HideInInspector]
  public bool progressStartDrag;
  public int maxRequestTime = 5;
  public float currentRequestTime;
  public int retryTimeUntilToRequestFromServer = 1;
  public int currentRetryTime;
  public bool gettingYoutubeURL;
  public bool videoAreReadyToPlay;
  public bool YoutubeUrlReady;
  public string lastTryVideoId;
  public bool videoStarted;
  public bool decryptionNeeded;
  public bool startPlaying;
  public float oldVolume;
  public bool waitAudioSeek;
  public const string RateBypassFlag = "ratebypass";
  public static string _signatureQuery = "sig";
  public const string Sp = "";
  public static string _projectionType = "";
  public float totalVideoDuration;
  public float currentVideoDuration;
  public bool lowRes;
  public float hideScreenTime;
  public float audioDuration;
  public Material skyboxMaterialNormal;
  public Material skyboxMaterial3DSide;
  public bool loadingFromServer;
  public static string _jsUrlDownloaded;
  public static bool _jsDownloaded;
  public static string _jsUrl;
  public bool FullscreenModeEnabled;
  public bool videoEnded;
  [HideInInspector]
  public string videoTitle = "";
  [FormerlySerializedAs("EACMaterial")]
  public Material eacMaterial;
  [FormerlySerializedAs("Material360")]
  public Material material360;
  public List<VideoInfo> _youtubeVideoInfos;
  public bool FinishedCalled;
  public bool StartedFromTime;
  public string tmpv;
  public JObject requestResult;
  public bool alreadyGotUrls;
  public static string _visitorData = "";
  public long lastFrame = -1;
  [Header("If your unity version audio desyncs try to play with .4f or other value.")]
  public float audioDelayOffset;
  [HideInInspector]
  public bool pauseCalled;

  public void Skybox3DSettup()
  {
    if (!this.is3DLayoutVideo)
      return;
    if (this.layout3d == YoutubeSimplifiedRequest.Layout3D.OverUnder)
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkybox3DOverUnder");
    else if (this.layout3d == YoutubeSimplifiedRequest.Layout3D.SideBySide)
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkybox3Dside");
    else if (this.layout3d == YoutubeSimplifiedRequest.Layout3D.EAC)
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkyboxEAC");
    else if (this.layout3d == YoutubeSimplifiedRequest.Layout3D.EAC3D)
    {
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkybox3DEAC");
    }
    else
    {
      if (this.layout3d != YoutubeSimplifiedRequest.Layout3D.None)
        return;
      RenderSettings.skybox = (Material) Resources.Load("Materials/PanoramicSkybox3Dside");
    }
  }

  public void YoutubeGetPlayableURL()
  {
    this.decryptionNeeded = true;
    this.StartCoroutine((IEnumerator) this.YoutubeGenerateUrlUsingClient());
  }

  public IEnumerator GetVisitorData()
  {
    if (!string.IsNullOrWhiteSpace(YoutubeSimplifiedRequest._visitorData))
      yield return (object) null;
    UnityWebRequest request = UnityWebRequest.Get("https://www.youtube.com/sw.js_data");
    request.SetRequestHeader("User-Agent", "com.google.ios.youtube/19.45.4 (iPhone16,2; U; CPU iOS 18_1_0 like Mac OS X; US)");
    yield return (object) request.SendWebRequest();
    string aJSON = request.downloadHandler.text;
    if (aJSON.StartsWith(")]}'"))
    {
      string str = aJSON;
      aJSON = str.Substring(4, str.Length - 4);
    }
    string str1 = JSON.Parse(aJSON)[0][2][0][0][13].Value;
    YoutubeSimplifiedRequest._visitorData = !string.IsNullOrWhiteSpace(str1) ? str1 : throw new Exception("Failed to resolve visitor data.");
  }

  public IEnumerator PreventFinishToBeCalledTwoTimes()
  {
    yield return (object) new WaitForSeconds(1f);
    this.FinishedCalled = false;
  }

  public IEnumerator YoutubeGenerateUrlUsingClient()
  {
    this.alreadyGotUrls = false;
    yield return (object) this.GetVisitorData();
    this.CheckVideoUrlAndExtractThevideoId(this.youtubeUrl);
    WWWForm formData1 = new WWWForm();
    byte[] bytes1 = Encoding.UTF8.GetBytes($"{{\"context\": {{\"client\": {{\"clientName\": \"ANDROID\",\"clientVersion\": \"19.29.37\",\"androidSdkVersion\": \"30\",\"osName\": \"Android\",\"osVersion\": \"11\",\"visitorData\":\"{YoutubeSimplifiedRequest._visitorData}\",\"userAgent\": \"com.google.android.youtube/19.29.37 (Linux; U; Android 11) gzip\", \"hl\": \"en\",\"gl\": \"US\",\"utcOffsetMinutes\": \"0\"}}}},\"videoId\": \"{this.youtubeVideoID}\",}}");
    UnityWebRequest request = UnityWebRequest.Post("https://www.youtube.com/youtubei/v1/player?key=AIzaSyA8eiZmM1FaDVjRy-df2KTyQ_vz_yYM39w", formData1);
    request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bytes1);
    request.SetRequestHeader("Content-Type", "application/json");
    request.SetRequestHeader("User-Agent", "com.google.android.youtube/19.29.37 (Linux; U; Android 11) gzip");
    yield return (object) request.SendWebRequest();
    request.uploadHandler.Dispose();
    JToken formats = JObject.Parse(request.downloadHandler.text)["streamingData"]?[(object) "formats"];
    WWWForm formData2 = new WWWForm();
    byte[] bytes2 = Encoding.UTF8.GetBytes($"{{\"context\": {{\"client\": {{\"clientName\": \"IOS\",\"clientVersion\": \"19.45.4\",\"deviceMake\": \"Apple\",\"deviceModel\": \"iPhone16,2\",\"osName\": \"IOS\",\"osVersion\": \"18.1.0.22B83\",\"visitorData\": \"{YoutubeSimplifiedRequest._visitorData}\",\"platform\": \"MOBILE\",\"hl\": \"en\",\"osName\": \"iPhone\",\"osVersion\": \"17.5.1.21F90\",\"timeZone\": \"UTC\",\"gl\": \"US\",\"userAgent\": \"com.google.ios.youtube/19.29.1 (iPhone16,2; U; CPU iOS 17_5_1 like Mac OS X;)\"}}}},\"videoId\": \"{this.youtubeVideoID}\",\"contentCheckOk\": \"true\",}}");
    UnityWebRequest nrequest = UnityWebRequest.Post("https://www.youtube.com/youtubei/v1/player?key=AIzaSyA8eiZmM1FaDVjRy-df2KTyQ_vz_yYM39w", formData2);
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
        JToken jtoken = this.requestResult["streamingData"];
        if (jtoken != null)
          jtoken[(object) "formats"] = formats;
        if (this.debug)
          Debug.Log((object) "want to write log?");
        this._youtubeVideoInfos = YoutubeSimplifiedRequest.GetVideoInfos(YoutubeSimplifiedRequest.ExtractDownloadUrls(this.requestResult), this.videoTitle).ToList<VideoInfo>();
        this.videoTitle = YoutubeSimplifiedRequest.GetVideoTitle(this.requestResult);
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
        this._youtubeVideoInfos = YoutubeSimplifiedRequest.GetVideoInfos(YoutubeSimplifiedRequest.ExtractDownloadUrls(this.requestResult), this.videoTitle).ToList<VideoInfo>();
        request.downloadHandler.Dispose();
        if (!this.alreadyGotUrls)
          this.UrlsLoaded();
      }
    }
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
      int num = url.LastIndexOf("?t=", StringComparison.Ordinal);
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
    this.skipOnDrop = true;
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
      this.controller = this.GetComponent<YoutubeVideoController>();
      this.events = this.GetComponent<YoutubeVideoEvents>();
      this.FullscreenModeEnabled = this.videoPlayer.renderMode == VideoRenderMode.CameraFarPlane || this.videoPlayer.renderMode == VideoRenderMode.CameraNearPlane;
    }
    if (this.loadYoutubeUrlsOnly)
    {
      this.controller = this.GetComponent<YoutubeVideoController>();
      this.events = this.GetComponent<YoutubeVideoEvents>();
    }
    if (this.playUsingInternalDevicePlayer || this.loadYoutubeUrlsOnly)
      return;
    if (this.is360 && this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      this.videoQuality = YoutubeSimplifiedRequest.YoutubeVideoQuality.Hd;
    if (this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      return;
    if (this.videoFormat == YoutubeSimplifiedRequest.VideoFormatType.Webm)
      this.videoPlayer.skipOnDrop = this.skipOnDrop;
    if (!((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null))
      return;
    this.audioPlayer.transform.gameObject.SetActive(false);
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
        case YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard:
          this.videoPlayer.targetTexture.width = 640;
          this.videoPlayer.targetTexture.height = 360;
          break;
        case YoutubeSimplifiedRequest.YoutubeVideoQuality.Hd:
          this.videoPlayer.targetTexture.width = 1280 /*0x0500*/;
          this.videoPlayer.targetTexture.height = 720;
          break;
        case YoutubeSimplifiedRequest.YoutubeVideoQuality.Fullhd:
          this.videoPlayer.targetTexture.width = 1920;
          this.videoPlayer.targetTexture.height = 1080;
          break;
        case YoutubeSimplifiedRequest.YoutubeVideoQuality.Uhd1440:
          this.videoPlayer.targetTexture.width = 2560 /*0x0A00*/;
          this.videoPlayer.targetTexture.height = 1440;
          break;
        case YoutubeSimplifiedRequest.YoutubeVideoQuality.Uhd2160:
          this.videoPlayer.targetTexture.width = 3840 /*0x0F00*/;
          this.videoPlayer.targetTexture.height = 2160;
          break;
      }
    }
    if (this.playUsingInternalDevicePlayer)
      this.loadYoutubeUrlsOnly = true;
    this.videoFormat = this._forceMP4 ? YoutubeSimplifiedRequest.VideoFormatType.Mp4 : YoutubeSimplifiedRequest.VideoFormatType.Webm;
    if (!this.loadYoutubeUrlsOnly)
    {
      this.Invoke("VerifyFrames", 2f);
      this.FixCameraEvent();
      this.Skybox3DSettup();
      if (this.videoFormat == YoutubeSimplifiedRequest.VideoFormatType.Webm)
      {
        this.videoPlayer.skipOnDrop = this.skipOnDrop;
        if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
          this.audioPlayer.skipOnDrop = this.skipOnDrop;
      }
      this.audioPlayer.seekCompleted += new VideoPlayer.EventHandler(this.AudioSeeked);
      this.videoPlayer.seekCompleted += new VideoPlayer.EventHandler(this.VideoSeeked);
    }
    this.PrepareVideoPlayerCallbacks();
    if (this.autoPlayOnStart)
      this.PlayYoutubeVideo(this.customPlaylist ? this.youtubeUrls[this.CurrentUrlIndex] : this.youtubeUrl);
    this.lowRes = this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard;
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
    if (!this.PLAYING)
    {
      this.videoPlayer.Stop();
      this.audioPlayer.Stop();
    }
    else
    {
      if (Application.internetReachability == NetworkReachability.NotReachable)
      {
        this.PLAYING = false;
        this.GetComponentInParent<UIRoadmapOverlayController>().OnCancelButtonInput();
      }
      if ((bool) (UnityEngine.Object) this.videoPlayer && this.videoPlayer.isPlaying)
      {
        if (!this.lowRes)
        {
          if ((bool) (UnityEngine.Object) this.controller.volumeSlider)
          {
            if ((double) this.videoPlayer.GetTargetAudioSource((ushort) 0).volume <= 0.0)
              this.videoPlayer.GetTargetAudioSource((ushort) 0).volume = this.controller.volumeSlider.value;
          }
          else
            this.videoPlayer.GetTargetAudioSource((ushort) 0).volume = 1f;
        }
        else if ((bool) (UnityEngine.Object) this.audioPlayer && (UnityEngine.Object) this.audioPlayer.GetTargetAudioSource((ushort) 0) != (UnityEngine.Object) null)
        {
          if ((bool) (UnityEngine.Object) this.controller.volumeSlider)
          {
            if ((double) this.audioPlayer.GetTargetAudioSource((ushort) 0).volume <= 0.0)
              this.audioPlayer.GetTargetAudioSource((ushort) 0).volume = this.controller.volumeSlider.value;
          }
          else
            this.audioPlayer.GetTargetAudioSource((ushort) 0).volume = 1f;
        }
      }
      if (!this.loadYoutubeUrlsOnly && !this.playUsingInternalDevicePlayer)
      {
        if (this.videoPlayer.isPlaying)
          this.HideLoading();
        else if (!this.pauseCalled && !this.PrepareVideoToPlayLater)
          this.ShowLoading();
      }
      if (!this.loadYoutubeUrlsOnly)
      {
        if (this.controller.showPlayerControl && this.videoPlayer.isPlaying)
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
        if (this.videoPlayer.frameCount > 0UL && (bool) (UnityEngine.Object) this.controller && this.controller.showPlayerControl)
        {
          if (this.controller.useSliderToProgressVideo)
          {
            if (!this.progressStartDrag)
              this.controller.playbackSlider.value = (float) this.videoPlayer.time;
          }
          else if ((bool) (UnityEngine.Object) this.controller.progressRectangle)
            this.controller.progressRectangle.fillAmount = (float) this.videoPlayer.frame / (float) this.videoPlayer.frameCount;
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
      if (!this.loadYoutubeUrlsOnly)
      {
        if (this.controller.showPlayerControl)
        {
          this.lowRes = this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard;
          if ((bool) (UnityEngine.Object) this.controller.currentTime && (bool) (UnityEngine.Object) this.controller.totalTime)
          {
            this.controller.currentTime.text = YoutubeSimplifiedRequest.FormatTime(Mathf.RoundToInt(this.currentVideoDuration));
            this.controller.totalTime.text = this.lowRes ? YoutubeSimplifiedRequest.FormatTime(Mathf.RoundToInt(this.totalVideoDuration)) : ((double) this.audioDuration >= (double) this.totalVideoDuration || !(this.audioPlayer.url != "") ? YoutubeSimplifiedRequest.FormatTime(Mathf.RoundToInt(this.totalVideoDuration)) : YoutubeSimplifiedRequest.FormatTime(Mathf.RoundToInt(this.audioDuration)));
          }
        }
        if (!this.controller.showPlayerControl)
        {
          if ((bool) (UnityEngine.Object) this.controller.controllerMainUI)
            this.controller.controllerMainUI.SetActive(false);
        }
        else
          this.controller.controllerMainUI.SetActive(true);
      }
      if (!this.loadYoutubeUrlsOnly && this.videoPlayer.isPrepared && !this.videoPlayer.isPlaying)
      {
        if ((bool) (UnityEngine.Object) this.audioPlayer)
        {
          if (this.audioPlayer.isPrepared && !this.videoStarted)
          {
            this.videoStarted = true;
            this.VideoStarted();
          }
        }
        else if (!this.videoStarted)
        {
          this.videoStarted = true;
          this.VideoStarted();
        }
      }
      if (this.loadYoutubeUrlsOnly)
        return;
      if (this.videoPlayer.frame != 0L && !this.videoEnded && this.videoPlayer.isPlaying && this.videoPlayer.frame >= (long) this.videoPlayer.frameCount - 1L)
      {
        this.videoEnded = true;
        this.PlaybackDone();
      }
      if (!this.videoPlayer.isPrepared || this.startPlaying)
        return;
      if (this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      {
        if (!this.audioPlayer.isPrepared)
          return;
        this.StartPlayingWebgl();
      }
      else
        this.StartPlayingWebgl();
    }
  }

  public void PrepareVideoPlayerCallbacks()
  {
    this.videoPlayer.errorReceived += new VideoPlayer.ErrorEventHandler(this.VideoErrorReceived);
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      return;
    this.audioPlayer.errorReceived += new VideoPlayer.ErrorEventHandler(this.VideoErrorReceived);
  }

  public void ShowLoading()
  {
    if (!((UnityEngine.Object) this.controller.loading != (UnityEngine.Object) null))
      return;
    this.controller.loading.SetActive(true);
  }

  public void HideLoading()
  {
    if (!((UnityEngine.Object) this.controller.loading != (UnityEngine.Object) null))
      return;
    this.controller.loading.SetActive(false);
  }

  public void ResetThings()
  {
    this.startPlaying = false;
    this.gettingYoutubeURL = false;
    this.progressStartDrag = false;
    this.videoAreReadyToPlay = false;
    this.YoutubeUrlReady = false;
    if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
      this.audioPlayer.seekCompleted += new VideoPlayer.EventHandler(this.AudioSeeked);
    this.videoPlayer.seekCompleted += new VideoPlayer.EventHandler(this.VideoSeeked);
    this.waitAudioSeek = false;
  }

  public void PlayYoutubeVideo(string videoId)
  {
    this.lowRes = this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard;
    this.ResetThings();
    videoId = this.CheckVideoUrlAndExtractThevideoId(videoId);
    if (!(videoId != "none"))
      return;
    if (this.showThumbnailBeforeVideoLoad)
      this.TryToLoadThumbnailBeforeOpenVideo(videoId);
    this.YoutubeUrlReady = false;
    this.ShowLoading();
    this.youtubeUrl = videoId;
    this.lastTryVideoId = videoId;
    this.currentRequestTime = 0.0f;
    this.gettingYoutubeURL = true;
    this.YoutubeGetPlayableURL();
  }

  public void UrlsLoaded()
  {
    this.gettingYoutubeURL = false;
    List<VideoInfo> youtubeVideoInfos = this._youtubeVideoInfos;
    if (this.is360 && (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Uhd1440 || this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Uhd2160))
      this.videoFormat = YoutubeSimplifiedRequest.VideoFormatType.Webm;
    bool flag1 = false;
    if (this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      this.videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
    youtubeVideoInfos.Reverse();
    using (IEnumerator<VideoInfo> enumerator = youtubeVideoInfos.Where<VideoInfo>((Func<VideoInfo, bool>) (info => info.FormatCode == 18)).GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        VideoInfo current = enumerator.Current;
        this.StartCoroutine((IEnumerator) this.WaitJsDownload());
        if (current.RequiresDecryption)
          flag1 = true;
        else
          this.audioUrl = current.DownloadUrl;
      }
    }
    int message = 360;
    switch (this.videoQuality)
    {
      case YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard:
        message = 360;
        break;
      case YoutubeSimplifiedRequest.YoutubeVideoQuality.Hd:
        message = 720;
        break;
      case YoutubeSimplifiedRequest.YoutubeVideoQuality.Fullhd:
        message = 1080;
        break;
      case YoutubeSimplifiedRequest.YoutubeVideoQuality.Uhd1440:
        message = 1440;
        break;
      case YoutubeSimplifiedRequest.YoutubeVideoQuality.Uhd2160:
        message = 2160;
        break;
    }
    bool flag2 = false;
    youtubeVideoInfos.Reverse();
    foreach (VideoInfo videoInfo in youtubeVideoInfos)
    {
      VideoType videoType = this.videoFormat == YoutubeSimplifiedRequest.VideoFormatType.Mp4 ? VideoType.Mp4 : VideoType.WebM;
      if (videoInfo.VideoType == videoType && videoInfo.Resolution == message)
      {
        if (this.debug)
          Debug.Log((object) message);
        if (this.is360)
        {
          if (!string.IsNullOrEmpty(YoutubeSimplifiedRequest._projectionType) && this.videoPlayer.renderMode != VideoRenderMode.RenderTexture)
          {
            switch (YoutubeSimplifiedRequest._projectionType)
            {
              case "MESH":
                foreach (UnityEngine.Object material in this.videoPlayer.targetMaterialRenderer.materials)
                {
                  if (material.name == "SphereEAC")
                  {
                    this.videoPlayer.targetMaterialRenderer.material = this.eacMaterial;
                    this.videoPlayer.targetMaterialRenderer.gameObject.transform.localScale = new Vector3(344.9097f, 344.9097f, -344.9097f);
                    this.videoPlayer.targetMaterialRenderer.gameObject.transform.localRotation = Quaternion.Euler(90f, -10f, -280f);
                  }
                }
                break;
              case "EQUIRECTANGULAR":
              case "RECTANGULAR":
                this.videoPlayer.targetMaterialRenderer.material = this.material360;
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
              flag1 = true;
            }
            else if (this.debug)
              Debug.Log((object) videoInfo.DownloadUrl);
            flag2 = true;
            break;
          }
        }
        else
        {
          if (videoInfo.Resolution == message)
          {
            this.videoUrl = videoInfo.DownloadUrl;
            this.OnYoutubeUrlsLoaded();
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
          if (videoInfo.RequiresDecryption)
          {
            flag1 = true;
          }
          else
          {
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
          }
          else
          {
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
      VideoType videoType = this.videoFormat == YoutubeSimplifiedRequest.VideoFormatType.Mp4 ? VideoType.Mp4 : VideoType.WebM;
      foreach (VideoInfo videoInfo in youtubeVideoInfos)
      {
        if (this.debug)
          Debug.Log((object) $"RES: {videoInfo.Resolution.ToString()} | {videoInfo.FormatCode.ToString()} | {videoInfo.VideoType.ToString()}");
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
            this.videoQuality = YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard;
            flag1 = true;
            break;
          }
          this.videoUrl = videoInfo.DownloadUrl;
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
              this.videoQuality = YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard;
              flag1 = true;
              break;
            }
            this.videoUrl = videoInfo.DownloadUrl;
            break;
          }
        }
      }
    }
    if (!flag1)
      return;
    this.decryptionNeeded = true;
  }

  public void StartPlayingWebgl()
  {
    if (this.startPlaying)
      return;
    this.startPlaying = true;
    this.events.OnVideoReadyToStart.Invoke();
    if (this.playUsingInternalDevicePlayer && Application.isMobilePlatform)
      this.StartCoroutine((IEnumerator) this.HandHeldPlayback());
    else
      this.StartPlayback();
  }

  public IEnumerator HandHeldPlayback()
  {
    Debug.Log((object) "This runs in mobile devices only!");
    yield return (object) new WaitForSeconds(1f);
    this.PlaybackDone();
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
    this.events.OnVideoStarted.Invoke();
    this.HideLoading();
    this.waitAudioSeek = true;
    if ((this.is360 || (UnityEngine.Object) this.videoPlayer.targetTexture != (UnityEngine.Object) null) && this.videoPlayer.renderMode == VideoRenderMode.RenderTexture)
    {
      this.videoPlayer.targetTexture.width = (int) this.videoPlayer.width;
      this.videoPlayer.targetTexture.height = (int) this.videoPlayer.height;
    }
    if (this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
    {
      this.audioPlayer.Pause();
      this.videoPlayer.Pause();
      this.audioPlayer.time = 1.0;
      this.videoPlayer.time = 0.0;
    }
    if (!this.PrepareVideoToPlayLater)
      this.DisableThumbnailObject();
    if (!this.PrepareVideoToPlayLater)
    {
      this.events.OnVideoStarted.Invoke();
      if (this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      {
        this.videoPlayer.Play();
        this.StartCoroutine((IEnumerator) this.DelayPlay());
      }
      else
        this.videoPlayer.Play();
    }
    if (!this.startFromSecond)
      return;
    this.StartedFromTime = true;
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      this.videoPlayer.time = (double) this.startFromSecondTime;
    else
      this.audioPlayer.time = (double) this.startFromSecondTime;
  }

  public int GetMaxQualitySupportedByDevice()
  {
    return Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation != ScreenOrientation.Portrait ? Screen.currentResolution.height : Screen.currentResolution.width;
  }

  public void RetryPlayYoutubeVideo()
  {
    this.Stop();
    ++this.currentRetryTime;
    if (this.currentRetryTime < this.retryTimeUntilToRequestFromServer)
    {
      this.StopIfPlaying();
      if (this.debug)
        Debug.Log((object) ("Youtube Retrying...:" + this.lastTryVideoId));
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
    this.YoutubeUrlReady = true;
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      this.videoUrl = this.audioUrl;
    if (this.loadYoutubeUrlsOnly)
    {
      Debug.Log((object) ("Url Generated to play, you can use the event callback: " + this.videoUrl));
      if ((UnityEngine.Object) this.events != (UnityEngine.Object) null)
        this.events.OnYoutubeUrlAreReady.Invoke(this.videoUrl);
    }
    if (!this.loadYoutubeUrlsOnly)
    {
      if (this.debug)
        Debug.Log((object) ("Url Generated to play!!" + this.videoUrl));
      this.videoPlayer.source = VideoSource.Url;
      this.videoPlayer.url = this.videoUrl;
      this.videoPlayer.EnableAudioTrack((ushort) 0, true);
      this.videoPlayer.SetTargetAudioSource((ushort) 0, this.videoPlayer.GetComponent<AudioSource>());
      this.videoPlayer.Prepare();
      if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
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

  public void PlaybackDone()
  {
    this.videoStarted = false;
    this.events.OnVideoFinished.Invoke();
  }

  public void VideoStarted()
  {
    if (this.videoStarted || !this.debug)
      return;
    Debug.Log((object) "Youtube Video Started");
  }

  public void VideoErrorReceived(VideoPlayer source, string message)
  {
    Debug.Log((object) ("Video Error: " + message));
  }

  public void Pause()
  {
    Debug.Log((object) "PAUSE");
    this.pauseCalled = true;
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
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
    this.events.OnVideoPaused.Invoke();
  }

  public void Update()
  {
    if (this.loadYoutubeUrlsOnly || !this.controller.showPlayerControl || this.controller.hideScreenControlTime <= 0)
      return;
    if (this.UserInteract())
    {
      this.hideScreenTime = 0.0f;
      if (!(bool) (UnityEngine.Object) this.controller.controllerMainUI)
        return;
      this.controller.controllerMainUI.SetActive(true);
    }
    else
    {
      this.hideScreenTime += Time.deltaTime;
      if ((double) this.hideScreenTime < (double) this.controller.hideScreenControlTime)
        return;
      this.hideScreenTime = (float) this.controller.hideScreenControlTime;
      this.controller.HideControllers();
    }
  }

  public void Stop()
  {
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.StopActiveLoops();
    this.PLAYING = false;
    this.PrepareVideoToPlayLater = false;
    if (this.playUsingInternalDevicePlayer)
      return;
    if ((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null)
      this.audioPlayer.seekCompleted -= new VideoPlayer.EventHandler(this.AudioSeeked);
    this.videoPlayer.seekCompleted -= new VideoPlayer.EventHandler(this.VideoSeeked);
    this.videoPlayer.Stop();
    if (this.lowRes || !((UnityEngine.Object) this.audioPlayer != (UnityEngine.Object) null))
      return;
    this.audioPlayer.Stop();
  }

  public static string FormatTime(int time)
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

  public static IEnumerable<YoutubeSimplifiedRequest.ExtractionInfo> ExtractDownloadUrls(
    JObject json)
  {
    List<string> urls = new List<string>();
    List<string> stringList = new List<string>();
    JObject jobject = json;
    JToken jtoken1 = jobject["streamingData"][(object) "adaptiveFormats"];
    if (jobject["streamingData"][(object) "formats"] != null)
    {
      if (jobject["streamingData"][(object) "formats"][(object) 0]?[(object) "cipher"] != null)
      {
        foreach (JToken jtoken2 in (IEnumerable<JToken>) jobject["streamingData"][(object) "formats"])
          stringList.Add(jtoken2[(object) "cipher"]?.ToString());
        if (jtoken1 != null)
        {
          foreach (JToken jtoken3 in (IEnumerable<JToken>) jtoken1)
            stringList.Add(jtoken3[(object) "cipher"]?.ToString());
        }
      }
      else if (jobject["streamingData"][(object) "formats"][(object) 0]?[(object) "signatureCipher"] != null)
      {
        foreach (JToken jtoken4 in (IEnumerable<JToken>) jobject["streamingData"][(object) "formats"])
          stringList.Add(jtoken4[(object) "signatureCipher"]?.ToString());
        if (jtoken1 != null)
        {
          foreach (JToken jtoken5 in (IEnumerable<JToken>) jtoken1)
            stringList.Add(jtoken5[(object) "signatureCipher"]?.ToString());
        }
      }
      else
      {
        if (jobject["streamingData"][(object) "formats"] != null)
        {
          foreach (JToken jtoken6 in (IEnumerable<JToken>) jobject["streamingData"][(object) "formats"])
            urls.Add(jtoken6[(object) "url"]?.ToString());
        }
        if (jtoken1 != null)
        {
          foreach (JToken jtoken7 in (IEnumerable<JToken>) jtoken1)
          {
            if (jtoken7[(object) "itag"]?.ToString() == "134" && jtoken7[(object) "projectionType"] != null)
              YoutubeSimplifiedRequest._projectionType = jtoken7[(object) "projectionType"].ToString();
            urls.Add(jtoken7[(object) "url"]?.ToString());
          }
        }
      }
    }
    else
    {
      if (jobject["streamingData"][(object) "formats"] != null)
      {
        foreach (JToken jtoken8 in (IEnumerable<JToken>) jobject["streamingData"][(object) "formats"])
          urls.Add(jtoken8[(object) "url"]?.ToString());
      }
      if (jtoken1 != null)
      {
        foreach (JToken jtoken9 in (IEnumerable<JToken>) jtoken1)
        {
          if (jtoken9[(object) "itag"]?.ToString() == "134" && jtoken9[(object) "projectionType"] != null)
            YoutubeSimplifiedRequest._projectionType = jtoken9[(object) "projectionType"].ToString();
          urls.Add(jtoken9[(object) "url"]?.ToString());
        }
      }
    }
    foreach (string s in stringList)
    {
      IDictionary<string, string> queryString = HTTPHelperYoutube.ParseQueryString(s);
      bool flag = false;
      YoutubeSimplifiedRequest._signatureQuery = queryString.ContainsKey("sp") ? "sig" : "signatures";
      string url;
      if (queryString.ContainsKey("s") || queryString.ContainsKey("signature"))
      {
        flag = queryString.ContainsKey("s");
        string str1;
        string str2 = queryString.TryGetValue("s", out str1) ? str1 : queryString["signature"];
        string str3;
        url = $"{queryString["url"]}&{YoutubeSimplifiedRequest._signatureQuery}={str2}" + (queryString.TryGetValue("fallback_host", out str3) ? "&fallback_host=" + str3 : string.Empty);
      }
      else
        url = queryString["url"];
      string str = HTTPHelperYoutube.UrlDecode(url);
      if (!HTTPHelperYoutube.ParseQueryString(str).ContainsKey("ratebypass"))
        str += "&ratebypass=yes";
      yield return new YoutubeSimplifiedRequest.ExtractionInfo()
      {
        RequiresDecryption = flag,
        Uri = new Uri(str)
      };
    }
    foreach (string url in urls)
    {
      string str = HTTPHelperYoutube.UrlDecode(HTTPHelperYoutube.UrlDecode(url));
      if (!HTTPHelperYoutube.ParseQueryString(str).ContainsKey("ratebypass"))
        str += "&ratebypass=yes";
      yield return new YoutubeSimplifiedRequest.ExtractionInfo()
      {
        RequiresDecryption = false,
        Uri = new Uri(str)
      };
    }
  }

  public static IEnumerable<VideoInfo> GetVideoInfos(
    IEnumerable<YoutubeSimplifiedRequest.ExtractionInfo> extractionInfos,
    string videoTitle)
  {
    List<VideoInfo> videoInfos = new List<VideoInfo>();
    foreach (YoutubeSimplifiedRequest.ExtractionInfo extractionInfo in extractionInfos)
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

  public static void WriteLog(string filename, string c)
  {
    string path = $"C:/log/{filename}_{DateTime.Now.ToString("ddMMyyyyhhmmssffff")}.txt";
    Debug.Log((object) ("Log written in: " + path));
    File.WriteAllText(path, c);
  }

  public void TrySkip(PointerEventData eventData)
  {
    Vector2 localPoint;
    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.controller.progressRectangle.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
      return;
    this.SkipToPercent(Mathf.InverseLerp(this.controller.progressRectangle.rectTransform.rect.xMin, this.controller.progressRectangle.rectTransform.rect.xMax, localPoint.x));
  }

  public void SkipToPercent(float pct)
  {
    if (this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
    {
      this.oldVolume = this.videoPlayer.GetComponent<AudioSource>().volume;
      this.videoPlayer.GetComponent<AudioSource>().volume = 0.0f;
    }
    float num = this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard ? (float) this.audioPlayer.frameCount * pct : (float) this.videoPlayer.frameCount * pct;
    this.videoPlayer.Pause();
    if (this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      this.audioPlayer.Pause();
    this.waitAudioSeek = true;
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      this.videoPlayer.frame = (long) num;
    else
      this.audioPlayer.frame = (long) num;
    this.videoPlayer.Pause();
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
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
      this.StartCoroutine(this.StartedFromTime ? (IEnumerator) this.PlayNowFromTime(2f) : (IEnumerator) this.PlayNow());
    else
      this.StartCoroutine(this.StartedFromTime ? (IEnumerator) this.PlayNowFromTime(2f) : (IEnumerator) this.PlayNow());
  }

  public bool CheckIfJsIsDownloaded() => YoutubeSimplifiedRequest._jsDownloaded;

  public IEnumerator WaitJsDownload()
  {
    YoutubeSimplifiedRequest simplifiedRequest = this;
    if (simplifiedRequest.debug)
      Debug.Log((object) "waiting js");
    yield return (object) new WaitUntil((Func<bool>) new Func<bool>(simplifiedRequest.CheckIfJsIsDownloaded));
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
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      return;
    this.videoPlayer.GetComponent<AudioSource>().volume = this.oldVolume;
  }

  public IEnumerator WaitSync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    YoutubeSimplifiedRequest simplifiedRequest = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      simplifiedRequest.Play();
      simplifiedRequest.Invoke("VerifyFrames", 2f);
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

  public IEnumerator EnableStopButton()
  {
    yield return (object) new WaitForSecondsRealtime(1f);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    foreach (MMButton button in this.buttons)
      button.Interactable = true;
  }

  public IEnumerator PlayNow()
  {
    YoutubeSimplifiedRequest simplifiedRequest = this;
    if (simplifiedRequest.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      yield return (object) new WaitForSeconds(0.0f);
    else
      yield return (object) new WaitForSeconds(1f);
    if (!simplifiedRequest.pauseCalled)
    {
      simplifiedRequest.Play();
      simplifiedRequest.StartCoroutine((IEnumerator) simplifiedRequest.ReleaseDrop());
    }
    else
      simplifiedRequest.StopCoroutine(nameof (PlayNow));
  }

  public IEnumerator ReleaseDrop()
  {
    yield return (object) new WaitForSeconds(2f);
  }

  public IEnumerator PlayNowFromTime(float time)
  {
    YoutubeSimplifiedRequest simplifiedRequest = this;
    yield return (object) new WaitForSeconds(time);
    simplifiedRequest.StartedFromTime = false;
    if (!simplifiedRequest.pauseCalled)
      simplifiedRequest.Play();
    else
      simplifiedRequest.StopCoroutine(nameof (PlayNowFromTime));
  }

  public enum YoutubeVideoQuality
  {
    Standard,
    Hd,
    Fullhd,
    Uhd1440,
    Uhd2160,
  }

  public enum VideoFormatType
  {
    Mp4,
    Webm,
  }

  public enum Layout3D
  {
    SideBySide,
    OverUnder,
    None,
    EAC,
    EAC3D,
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
}
