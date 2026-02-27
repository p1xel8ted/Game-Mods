// Decompiled with JetBrains decompiler
// Type: ScreenshotCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.VideoioModule;
using System.Collections;
using System.IO;
using UnityEngine;

#nullable disable
public class ScreenshotCamera : MonoBehaviour
{
  private static ScreenshotCamera _Instance;
  public Camera ScreenshotCameraObject;
  private int screenWidth = 2560 /*0x0A00*/;
  private int screenHeight = 1440;
  private Coroutine screenshotRoutine;

  public static ScreenshotCamera Instance
  {
    get => ScreenshotCamera._Instance;
    set => ScreenshotCamera._Instance = value;
  }

  private void Start()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhase);
    this.ScreenshotCameraObject.gameObject.SetActive(false);
    ScreenshotCamera.Instance = this;
    this.OnNewPhase();
  }

  private void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhase);

  private void OnNewPhase()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || DataManager.Instance.DateLastScreenshot == TimeManager.CurrentDay || !TimeManager.IsDay || this.screenshotRoutine != null)
      return;
    this.screenshotRoutine = this.StartCoroutine((IEnumerator) this.WaitToTakeScreenshot());
  }

  private IEnumerator WaitToTakeScreenshot()
  {
    while (PlayerFarming.Location != FollowerLocation.Base)
      yield return (object) null;
    this.screenshotRoutine = (Coroutine) null;
    this.TakeScreenshot();
  }

  public void ExportVideoCV() => this.StartCoroutine((IEnumerator) this.ExportVideoCVRoutine());

  private IEnumerator ExportVideoCVRoutine()
  {
    OpenCVForUnity.UnityUtils.Utils.setDebugMode(true, true);
    string encodedVideoFilePath = Path.Combine(Application.persistentDataPath, $"{DataManager.Instance.CultName}_{(object) SaveAndLoad.SAVE_SLOT}.mp4");
    Size frameSize = new Size((double) this.screenWidth, (double) this.screenHeight);
    Mat frame = new Mat(this.screenHeight, this.screenWidth, CvType.CV_8UC4);
    VideoWriter outputVideo = new VideoWriter(encodedVideoFilePath, VideoWriter.fourcc('X', '2', '6', '4'), 5.0, frameSize);
    outputVideo.set(1900, 1.0);
    outputVideo.set(47, 8000.0);
    outputVideo.set(1, 50.0);
    if (!outputVideo.isOpened())
      Debug.Log((object) "Could not open the output video for write");
    for (int i = 0; i < TimeManager.CurrentDay; ++i)
    {
      string path = Path.Combine(Application.persistentDataPath, "Screenshots", $"day_{(object) i}_{(object) SaveAndLoad.SAVE_SLOT}.jpeg");
      if (File.Exists(path))
      {
        Debug.Log((object) ("File does exist: " + path));
        byte[] data = File.ReadAllBytes(path);
        Texture2D texture2D = new Texture2D(this.screenHeight, this.screenWidth, TextureFormat.RGBA32, false);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(data);
        texture2D.SetPixels(tex.GetPixels());
        texture2D.Apply();
        OpenCVForUnity.UnityUtils.Utils.fastTexture2DToMat(texture2D, frame);
        Imgproc.cvtColor(frame, frame, 2);
        Imgproc.putText(frame, i.ToString(), new Point((double) (frame.cols() - 70), 30.0), 0, 1.0, new Scalar((double) byte.MaxValue, (double) byte.MaxValue, (double) byte.MaxValue), 2, 16 /*0x10*/, false);
        Imgproc.putText(frame, DataManager.Instance.CultName, new Point((double) (frame.cols() / 2), 30.0), 0, 1.0, new Scalar((double) byte.MaxValue, (double) byte.MaxValue, (double) byte.MaxValue), 2, 16 /*0x10*/, false);
        outputVideo.write(frame);
        UnityEngine.Object.Destroy((UnityEngine.Object) texture2D);
        UnityEngine.Object.Destroy((UnityEngine.Object) tex);
        Debug.Log((object) ("% Complete = " + (object) (float) ((double) i / (double) TimeManager.CurrentDay)));
        yield return (object) new WaitForEndOfFrame();
      }
      else
        Debug.Log((object) ("File doesnt exist: " + encodedVideoFilePath));
    }
    outputVideo.release();
    outputVideo.Dispose();
    if (outputVideo.IsDisposed)
      Debug.Log((object) ("Video Exported: " + encodedVideoFilePath));
  }

  public static string ScreenShotName()
  {
    string str = Path.Combine(Application.persistentDataPath, "Screenshots");
    if (!Directory.Exists(str))
      Directory.CreateDirectory(str);
    return string.Format(Path.Combine(str, "day_{0}_{1}.jpeg"), (object) TimeManager.CurrentDay.ToString(), (object) SaveAndLoad.SAVE_SLOT);
  }

  public void TakeScreenshotCV()
  {
    DataManager.Instance.DateLastScreenshot = TimeManager.CurrentDay;
    this.ScreenshotCameraObject.gameObject.SetActive(true);
    RenderTexture renderTexture = new RenderTexture(this.screenWidth, this.screenHeight, 24);
    this.ScreenshotCameraObject.targetTexture = renderTexture;
    Texture2D texture2D = new Texture2D(this.screenWidth, this.screenHeight, TextureFormat.RGB24, false);
    this.ScreenshotCameraObject.Render();
    RenderTexture.active = renderTexture;
    texture2D.ReadPixels(new UnityEngine.Rect(0.0f, 0.0f, (float) this.screenWidth, (float) this.screenHeight), 0, 0);
    this.ScreenshotCameraObject.targetTexture = (RenderTexture) null;
    RenderTexture.active = (RenderTexture) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) renderTexture);
    Mat mat = new Mat(this.screenHeight, this.screenWidth, CvType.CV_8UC3);
    OpenCVForUnity.UnityUtils.Utils.fastTexture2DToMat(texture2D, mat);
    Imgproc.cvtColor(mat, mat, 2);
    MatOfInt _params = new MatOfInt();
    _params.push_back((Mat) new MatOfInt(new int[1]
    {
      16 /*0x10*/
    }));
    _params.push_back((Mat) new MatOfInt(new int[1]{ 9 }));
    _params.push_back((Mat) new MatOfInt(new int[1]{ 17 }));
    _params.push_back((Mat) new MatOfInt(new int[1]{ 3 }));
    _params.push_back((Mat) new MatOfInt(new int[1]{ 18 }));
    _params.push_back((Mat) new MatOfInt(new int[1]));
    OpenCVForUnity.UnityUtils.Utils.setDebugMode(true, true);
    if (Imgcodecs.imwrite(ScreenshotCamera.ScreenShotName(), mat, _params))
      Debug.Log((object) $"Took screenshot to: {ScreenshotCamera.ScreenShotName()}");
    else
      Debug.Log((object) $"Failed to take screenshot to: {ScreenshotCamera.ScreenShotName()}");
    this.ScreenshotCameraObject.gameObject.SetActive(false);
  }

  public void TakeScreenshot() => this.StartCoroutine((IEnumerator) this.TakeScreenshotRoutine());

  private IEnumerator TakeScreenshotRoutine()
  {
    Debug.Log((object) "Taking Screenshit");
    DataManager.Instance.DateLastScreenshot = TimeManager.CurrentDay;
    this.ScreenshotCameraObject.gameObject.SetActive(true);
    RenderTexture renderTexture = new RenderTexture(this.screenWidth, this.screenHeight, 24);
    this.ScreenshotCameraObject.targetTexture = renderTexture;
    Texture2D screenshot = new Texture2D(this.screenWidth, this.screenHeight, TextureFormat.RGB24, false);
    this.ScreenshotCameraObject.Render();
    RenderTexture.active = renderTexture;
    screenshot.ReadPixels(new UnityEngine.Rect(0.0f, 0.0f, (float) this.screenWidth, (float) this.screenHeight), 0, 0);
    this.ScreenshotCameraObject.targetTexture = (RenderTexture) null;
    RenderTexture.active = (RenderTexture) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) renderTexture);
    yield return (object) null;
    byte[] jpg = screenshot.EncodeToJPG(75);
    string path = ScreenshotCamera.ScreenShotName();
    File.WriteAllBytes(path, jpg);
    Debug.Log((object) $"Took screenshot to: {path}");
    yield return (object) null;
    this.ScreenshotCameraObject.gameObject.SetActive(false);
    UnityEngine.Object.Destroy((UnityEngine.Object) screenshot);
    screenshot = (Texture2D) null;
  }
}
