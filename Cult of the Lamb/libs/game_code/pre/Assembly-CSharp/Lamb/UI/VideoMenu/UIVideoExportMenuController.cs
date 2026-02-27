// Decompiled with JetBrains decompiler
// Type: Lamb.UI.VideoMenu.UIVideoExportMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.VideoioModule;
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.VideoMenu;

public class UIVideoExportMenuController : UIMenuBase
{
  [Header("Menu")]
  [SerializeField]
  private RawImage _imageHolder;
  [SerializeField]
  private CanvasGroup _progressCanvasGroup;
  [SerializeField]
  private CanvasGroup _buttonContainerCanvasGroup;
  [SerializeField]
  private UIMenuControlPrompts _controlIconsCanvasGroup;
  [SerializeField]
  private LoadingIcon _progressBar;
  [SerializeField]
  private TextMeshProUGUI _progressText;
  [SerializeField]
  private TextMeshProUGUI _completeText;
  [SerializeField]
  private TextMeshProUGUI _buttonText;
  [Header("Buttons")]
  [SerializeField]
  private MMButton _exportButton;
  private int screenWidth = 2560 /*0x0A00*/;
  private int screenHeight = 1440;
  private int amountOfScreenshots = 100;
  private bool _videoExported;
  private Texture2D tex;
  private Texture2D oldTex;
  private bool exportingVideo;

  private void Start() => this.StartCoroutine((IEnumerator) this.ShowImages());

  public void Show(int currentDay = 100, bool immediate = false)
  {
    this.amountOfScreenshots = currentDay;
    if ((UnityEngine.Object) this._canvas != (UnityEngine.Object) null && !UIMenuBase.ActiveMenus.Contains((UIMenuBase) this))
    {
      UIMenuBase.ActiveMenus.Add((UIMenuBase) this);
      this.UpdateSortingOrder();
    }
    this.gameObject.SetActive(true);
    this.StopAllCoroutines();
    this.Show(immediate);
  }

  protected override void OnShowStarted()
  {
    this._completeText.text = ScriptLocalization.UI_ExportVideo.Loading;
    this._buttonText.text = ScriptLocalization.UI_ExportVideo.Export;
    this._buttonContainerCanvasGroup.alpha = 0.0f;
    this._progressCanvasGroup.alpha = 0.0f;
    this._exportButton.onClick.AddListener(new UnityAction(this.exportVideo));
  }

  private void exportVideo()
  {
    if (this.exportingVideo)
      return;
    if (this._videoExported)
    {
      this.Hide();
    }
    else
    {
      this._progressCanvasGroup.alpha = 1f;
      this._buttonContainerCanvasGroup.alpha = 0.0f;
      this._imageHolder.color = StaticColors.GreyColor;
      this._controlIconsCanvasGroup.GetComponent<CanvasGroup>().alpha = 0.0f;
      this.StartCoroutine((IEnumerator) this.exportVideoRoutine());
    }
  }

  private IEnumerator ShowImages()
  {
    while (!this.exportingVideo)
    {
      for (int i = 0; i < this.amountOfScreenshots; ++i)
      {
        string path = Path.Combine(Application.persistentDataPath, "Screenshots", $"day_{(object) i}_{(object) SaveAndLoad.SAVE_SLOT}.jpeg");
        if (this.exportingVideo)
          yield break;
        if (File.Exists(path))
        {
          this._buttonContainerCanvasGroup.alpha = 1f;
          this._completeText.text = "";
          this._imageHolder.color = Color.white;
          if ((UnityEngine.Object) this.tex != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) this.tex);
          if ((UnityEngine.Object) this.oldTex != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) this.oldTex);
          this.tex = (Texture2D) null;
          this.oldTex = (Texture2D) null;
          Debug.Log((object) ("File does exist: " + path));
          byte[] data = File.ReadAllBytes(path);
          this.tex = new Texture2D(2560 /*0x0A00*/, 1440, TextureFormat.RGBA32, false);
          this.oldTex = new Texture2D(2, 2);
          this.oldTex.LoadImage(data);
          this.tex.SetPixels(this.oldTex.GetPixels());
          this.tex.Apply();
          this._imageHolder.texture = (Texture) this.tex;
          yield return (object) new WaitForSecondsRealtime(0.2f);
        }
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  private IEnumerator exportVideoRoutine()
  {
    this.exportingVideo = true;
    OpenCVForUnity.UnityUtils.Utils.setDebugMode(true, true);
    string encodedVideoFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{DataManager.Instance.CultName}_{(object) SaveAndLoad.SAVE_SLOT}.mp4");
    Size frameSize = new Size((double) this.screenWidth, (double) this.screenHeight);
    Mat frame = new Mat(this.screenHeight, this.screenWidth, CvType.CV_8UC4);
    VideoWriter outputVideo = new VideoWriter(encodedVideoFilePath, VideoWriter.fourcc('X', '2', '6', '4'), 5.0, frameSize);
    outputVideo.set(1900, 1.0);
    if (!outputVideo.isOpened())
      Debug.Log((object) "Could not open the output video for write");
    for (int i = 0; i < this.amountOfScreenshots; ++i)
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
        this._progressBar.UpdateProgress((float) i / (float) this.amountOfScreenshots);
        this._progressText.text = "%  " + (object) (float) ((double) i / (double) this.amountOfScreenshots * 100.0);
        Debug.Log((object) ("% Complete = " + (object) (float) ((double) i / (double) this.amountOfScreenshots)));
        yield return (object) new WaitForEndOfFrame();
      }
      else
        Debug.Log((object) ("File doesnt exist: " + encodedVideoFilePath));
    }
    this._progressCanvasGroup.alpha = 0.0f;
    this._buttonText.text = ScriptLocalization.Interactions.Done;
    this._buttonContainerCanvasGroup.alpha = 1f;
    this._controlIconsCanvasGroup.GetComponent<CanvasGroup>().alpha = 1f;
    outputVideo.release();
    outputVideo.Dispose();
    if (outputVideo.IsDisposed)
      this._completeText.text = string.Format(ScriptLocalization.UI_ExportVideo.Exported, (object) encodedVideoFilePath);
    else
      this._completeText.text = ScriptLocalization.UI_ExportVideo.Failed;
    this.exportingVideo = false;
    this._videoExported = true;
  }

  public override void OnCancelButtonInput()
  {
    if (this.exportingVideo || !this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
