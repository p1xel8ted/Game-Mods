// Decompiled with JetBrains decompiler
// Type: UserReportingScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Client;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UserReportingScript : MonoBehaviour
{
  [Tooltip("The category dropdown.")]
  public Dropdown CategoryDropdown;
  [Tooltip("The description input on the user report form.")]
  public InputField DescriptionInput;
  [Tooltip("The UI shown when there's an error.")]
  public Canvas ErrorPopup;
  public bool isCreatingUserReport;
  [Tooltip("A value indicating whether the hotkey is enabled (Left Alt + Left Shift + B).")]
  public bool IsHotkeyEnabled;
  [Tooltip("A value indicating whether the prefab is in silent mode. Silent mode does not show the user report form.")]
  public bool IsInSilentMode;
  [Tooltip("A value indicating whether the user report client reports metrics about itself.")]
  public bool IsSelfReporting;
  public bool isShowingError;
  public bool isSubmitting;
  [Tooltip("The display text for the progress text.")]
  public UnityEngine.UI.Text ProgressText;
  [Tooltip("A value indicating whether the user report client send events to analytics.")]
  public bool SendEventsToAnalytics;
  [Tooltip("The UI shown while submitting.")]
  public Canvas SubmittingPopup;
  [Tooltip("The summary input on the user report form.")]
  public InputField SummaryInput;
  [Tooltip("The thumbnail viewer on the user report form.")]
  public Image ThumbnailViewer;
  public UnityUserReportingUpdater unityUserReportingUpdater;
  [Tooltip("The user report button used to create a user report.")]
  public Button UserReportButton;
  [Tooltip("The UI for the user report form. Shown after a user report is created.")]
  public Canvas UserReportForm;
  [Tooltip("The User Reporting platform. Different platforms have different features but may require certain Unity versions or target platforms. The Async platform adds async screenshotting and report creation, but requires Unity 2018.3 and above, the package manager version of Unity User Reporting, and a target platform that supports asynchronous GPU readback such as DirectX.")]
  public UserReportingPlatformType UserReportingPlatform;
  [Tooltip("The event raised when a user report is submitting.")]
  public UnityEvent UserReportSubmitting;
  [CompilerGenerated]
  public UserReport \u003CCurrentUserReport\u003Ek__BackingField;

  public UserReportingScript()
  {
    this.UserReportSubmitting = new UnityEvent();
    this.unityUserReportingUpdater = new UnityUserReportingUpdater();
  }

  public UserReport CurrentUserReport
  {
    get => this.\u003CCurrentUserReport\u003Ek__BackingField;
    set => this.\u003CCurrentUserReport\u003Ek__BackingField = value;
  }

  public UserReportingState State
  {
    get
    {
      if (this.CurrentUserReport != null)
      {
        if (this.IsInSilentMode)
          return UserReportingState.Idle;
        return this.isSubmitting ? UserReportingState.SubmittingForm : UserReportingState.ShowingForm;
      }
      return this.isCreatingUserReport ? UserReportingState.CreatingUserReport : UserReportingState.Idle;
    }
  }

  public void CancelUserReport()
  {
    this.CurrentUserReport = (UserReport) null;
    this.ClearForm();
    Time.timeScale = 1f;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  public IEnumerator ClearError()
  {
    yield return (object) new WaitForSeconds(10f);
    this.isShowingError = false;
  }

  public void ClearForm()
  {
    this.SummaryInput.text = (string) null;
    this.DescriptionInput.text = (string) null;
  }

  public void CreateUserReport()
  {
    if (this.isCreatingUserReport)
      return;
    Time.timeScale = 0.0f;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    this.isCreatingUserReport = true;
    UnityUserReporting.CurrentClient.TakeScreenshot(2048 /*0x0800*/, 2048 /*0x0800*/, (Action<UserReportScreenshot>) (s => { }));
    UnityUserReporting.CurrentClient.TakeScreenshot(512 /*0x0200*/, 512 /*0x0200*/, (Action<UserReportScreenshot>) (s => { }));
    UnityUserReporting.CurrentClient.CreateUserReport((Action<UserReport>) (br =>
    {
      if (string.IsNullOrEmpty(br.ProjectIdentifier))
        Debug.LogWarning((object) "The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
      string path1 = Path.Combine(Application.persistentDataPath, "saves");
      COTLDataReadWriter<DataManager> cotlDataReadWriter1 = new COTLDataReadWriter<DataManager>();
      string path2_1 = SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT);
      string filename1 = path2_1;
      if (cotlDataReadWriter1.FileExists(filename1))
      {
        byte[] data = Compression.Compress(File.ReadAllBytes(Path.Combine(path1, path2_1)));
        br.Attachments.Add(new UserReportAttachment("SaveFile", "SaveFile.zip", "application/zip", data));
      }
      COTLDataReadWriter<SettingsData> cotlDataReadWriter2 = new COTLDataReadWriter<SettingsData>();
      string path2_2 = "settings.json";
      string filename2 = path2_2;
      if (cotlDataReadWriter2.FileExists(filename2))
      {
        byte[] data = Compression.Compress(File.ReadAllBytes(Path.Combine(path1, path2_2)));
        br.Attachments.Add(new UserReportAttachment("SettingsFile", "SettingsFile.zip", "application/zip", data));
      }
      string path = Path.Combine(Application.persistentDataPath, "Player.log");
      if (File.Exists(path))
      {
        byte[] data = Compression.Compress(File.ReadAllBytes(path));
        br.Attachments.Add(new UserReportAttachment("Player Log", "PlayerLog.zip", "application/log", data));
      }
      string str1 = "Unknown";
      string str2 = "0.0";
      foreach (UserReportNamedValue reportNamedValue in br.DeviceMetadata)
      {
        if (reportNamedValue.Name == "Platform")
          str1 = reportNamedValue.Value;
        if (reportNamedValue.Name == "Version")
          str2 = reportNamedValue.Value;
      }
      br.Dimensions.Add(new UserReportNamedValue("Platform.Version", $"{str1}.{str2}"));
      this.CurrentUserReport = br;
      this.isCreatingUserReport = false;
      this.SetThumbnail(br);
      if (!this.IsInSilentMode)
        return;
      this.SubmitUserReport();
    }));
  }

  public UserReportingClientConfiguration GetConfiguration()
  {
    return new UserReportingClientConfiguration();
  }

  public bool IsSubmitting() => this.isSubmitting;

  public void SetThumbnail(UserReport userReport)
  {
    if (userReport == null || !((UnityEngine.Object) this.ThumbnailViewer != (UnityEngine.Object) null))
      return;
    byte[] data = Convert.FromBase64String(userReport.Thumbnail.DataBase64);
    Texture2D texture2D = new Texture2D(1, 1);
    texture2D.LoadImage(data);
    this.ThumbnailViewer.sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height), new Vector2(0.5f, 0.5f));
    this.ThumbnailViewer.preserveAspect = true;
  }

  public void Start()
  {
    if (Application.isPlaying && (UnityEngine.Object) UnityEngine.Object.FindObjectOfType<EventSystem>() == (UnityEngine.Object) null)
    {
      GameObject gameObject = new GameObject("EventSystem");
      gameObject.AddComponent<EventSystem>();
      gameObject.AddComponent<StandaloneInputModule>();
    }
    bool flag = false;
    if (this.UserReportingPlatform == UserReportingPlatformType.Async)
    {
      System.Type type = Assembly.GetExecutingAssembly().GetType("Unity.Cloud.UserReporting.Plugin.Version2018_3.AsyncUnityUserReportingPlatform");
      if (type != (System.Type) null && Activator.CreateInstance(type) is IUserReportingPlatform instance)
      {
        UnityUserReporting.Configure(instance, this.GetConfiguration());
        flag = true;
      }
    }
    if (!flag)
      UnityUserReporting.Configure(this.GetConfiguration());
    UnityUserReporting.CurrentClient.Platform.Post($"https://userreporting.cloud.unity3d.com/api/userreporting/projects/{UnityUserReporting.CurrentClient.ProjectIdentifier}/ping", "application/json", Encoding.UTF8.GetBytes("\"Ping\""), (Action<float, float>) ((upload, download) => { }), (Action<bool, byte[]>) ((result, bytes) => { }));
  }

  public void SubmitUserReport()
  {
    if (this.isSubmitting || this.CurrentUserReport == null)
      return;
    Time.timeScale = 1f;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    this.isSubmitting = true;
    if ((UnityEngine.Object) this.SummaryInput != (UnityEngine.Object) null)
      this.CurrentUserReport.Summary = this.SummaryInput.text;
    if ((UnityEngine.Object) this.CategoryDropdown != (UnityEngine.Object) null)
    {
      string text = this.CategoryDropdown.options[this.CategoryDropdown.value].text;
      this.CurrentUserReport.Dimensions.Add(new UserReportNamedValue("Category", text));
      this.CurrentUserReport.Fields.Add(new UserReportNamedValue("Category", text));
    }
    if ((UnityEngine.Object) this.DescriptionInput != (UnityEngine.Object) null)
      this.CurrentUserReport.Fields.Add(new UserReportNamedValue()
      {
        Name = "Description",
        Value = this.DescriptionInput.text
      });
    this.ClearForm();
    this.RaiseUserReportSubmitting();
    UnityUserReporting.CurrentClient.SendUserReport(this.CurrentUserReport, (Action<float, float>) ((uploadProgress, downloadProgress) =>
    {
      if (!((UnityEngine.Object) this.ProgressText != (UnityEngine.Object) null))
        return;
      this.ProgressText.text = $"{uploadProgress:P}";
    }), (Action<bool, UserReport>) ((success, br2) =>
    {
      if (!success)
      {
        this.isShowingError = true;
        this.StartCoroutine((IEnumerator) this.ClearError());
      }
      this.CurrentUserReport = (UserReport) null;
      this.isSubmitting = false;
    }));
  }

  public void Update()
  {
    if (this.IsHotkeyEnabled && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.B))
      this.CreateUserReport();
    UnityUserReporting.CurrentClient.IsSelfReporting = this.IsSelfReporting;
    UnityUserReporting.CurrentClient.SendEventsToAnalytics = this.SendEventsToAnalytics;
    if ((UnityEngine.Object) this.UserReportButton != (UnityEngine.Object) null)
      this.UserReportButton.interactable = this.State == UserReportingState.Idle;
    if ((UnityEngine.Object) this.UserReportForm != (UnityEngine.Object) null)
      this.UserReportForm.enabled = this.State == UserReportingState.ShowingForm;
    if ((UnityEngine.Object) this.SubmittingPopup != (UnityEngine.Object) null)
      this.SubmittingPopup.enabled = this.State == UserReportingState.SubmittingForm;
    if ((UnityEngine.Object) this.ErrorPopup != (UnityEngine.Object) null)
      this.ErrorPopup.enabled = this.isShowingError;
    this.unityUserReportingUpdater.Reset();
    this.StartCoroutine((IEnumerator) this.unityUserReportingUpdater);
  }

  public virtual void RaiseUserReportSubmitting()
  {
    if (this.UserReportSubmitting == null)
      return;
    this.UserReportSubmitting.Invoke();
  }

  [CompilerGenerated]
  public void \u003CCreateUserReport\u003Eb__29_2(UserReport br)
  {
    if (string.IsNullOrEmpty(br.ProjectIdentifier))
      Debug.LogWarning((object) "The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
    string path1 = Path.Combine(Application.persistentDataPath, "saves");
    COTLDataReadWriter<DataManager> cotlDataReadWriter1 = new COTLDataReadWriter<DataManager>();
    string path2_1 = SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT);
    string filename1 = path2_1;
    if (cotlDataReadWriter1.FileExists(filename1))
    {
      byte[] data = Compression.Compress(File.ReadAllBytes(Path.Combine(path1, path2_1)));
      br.Attachments.Add(new UserReportAttachment("SaveFile", "SaveFile.zip", "application/zip", data));
    }
    COTLDataReadWriter<SettingsData> cotlDataReadWriter2 = new COTLDataReadWriter<SettingsData>();
    string path2_2 = "settings.json";
    string filename2 = path2_2;
    if (cotlDataReadWriter2.FileExists(filename2))
    {
      byte[] data = Compression.Compress(File.ReadAllBytes(Path.Combine(path1, path2_2)));
      br.Attachments.Add(new UserReportAttachment("SettingsFile", "SettingsFile.zip", "application/zip", data));
    }
    string path = Path.Combine(Application.persistentDataPath, "Player.log");
    if (File.Exists(path))
    {
      byte[] data = Compression.Compress(File.ReadAllBytes(path));
      br.Attachments.Add(new UserReportAttachment("Player Log", "PlayerLog.zip", "application/log", data));
    }
    string str1 = "Unknown";
    string str2 = "0.0";
    foreach (UserReportNamedValue reportNamedValue in br.DeviceMetadata)
    {
      if (reportNamedValue.Name == "Platform")
        str1 = reportNamedValue.Value;
      if (reportNamedValue.Name == "Version")
        str2 = reportNamedValue.Value;
    }
    br.Dimensions.Add(new UserReportNamedValue("Platform.Version", $"{str1}.{str2}"));
    this.CurrentUserReport = br;
    this.isCreatingUserReport = false;
    this.SetThumbnail(br);
    if (!this.IsInSilentMode)
      return;
    this.SubmitUserReport();
  }

  [CompilerGenerated]
  public void \u003CSubmitUserReport\u003Eb__34_0(float uploadProgress, float downloadProgress)
  {
    if (!((UnityEngine.Object) this.ProgressText != (UnityEngine.Object) null))
      return;
    this.ProgressText.text = $"{uploadProgress:P}";
  }

  [CompilerGenerated]
  public void \u003CSubmitUserReport\u003Eb__34_1(bool success, UserReport br2)
  {
    if (!success)
    {
      this.isShowingError = true;
      this.StartCoroutine((IEnumerator) this.ClearError());
    }
    this.CurrentUserReport = (UserReport) null;
    this.isSubmitting = false;
  }
}
