// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIBugReportingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using src.UINavigator;
using src.Utilities;
using System;
using System.Collections;
using System.IO;
using System.Text;
using TMPro;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Client;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIBugReportingOverlayController : UIMenuBase
{
  private const int kMaxSpamCount = 2;
  [Header("Bug Reporting")]
  [SerializeField]
  private GameObject _buttonHighlight;
  [SerializeField]
  private UserReportingPlatformType UserReportingPlatform;
  [Header("Loading")]
  [SerializeField]
  private GameObject _loadingContent;
  [Header("Form")]
  [SerializeField]
  private GameObject _formContent;
  [SerializeField]
  private MMInputField _summaryField;
  [SerializeField]
  private BugInputField _summarySelector;
  [SerializeField]
  private MMInputField _contactField;
  [SerializeField]
  private BugInputField _contactSelector;
  [SerializeField]
  private MMInputField _descriptionField;
  [SerializeField]
  private BugInputField _descriptionSelector;
  [SerializeField]
  private MMButton _cancelButton;
  [SerializeField]
  private MMButton _acceptButton;
  [Header("Completed")]
  [SerializeField]
  private GameObject _sendingContent;
  [SerializeField]
  private TextMeshProUGUI _sendingStatus;
  [Header("Completed")]
  [SerializeField]
  private GameObject _completedContent;
  [SerializeField]
  private MMButton _completedConfirmButton;
  [Header("Eror")]
  [SerializeField]
  private TextMeshProUGUI _errorHeader;
  [SerializeField]
  private TextMeshProUGUI _erroDescription;
  [SerializeField]
  private GameObject _errorContent;
  [SerializeField]
  private MMButton _errorConfirmButton;
  private UserReport _currentUserReport;
  private UIBugReportingOverlayController.State _currentState;
  private UnityUserReportingUpdater _unityUserReportingUpdater;
  private string _summary;
  private string _contact;
  private string _description;
  private static int _spamCount;

  public UIBugReportingOverlayController()
  {
    this._unityUserReportingUpdater = new UnityUserReportingUpdater();
  }

  protected override void OnShowStarted()
  {
    this._acceptButton.onClick.AddListener(new UnityAction(this.SubmitUserReport));
    this._cancelButton.onClick.AddListener(new UnityAction(((UIMenuBase) this).OnCancelButtonInput));
    this._completedConfirmButton.onClick.AddListener(new UnityAction(((UIMenuBase) this).OnCancelButtonInput));
    this._errorConfirmButton.onClick.AddListener(new UnityAction(((UIMenuBase) this).OnCancelButtonInput));
    this._summaryField.OnDeselected += new System.Action(this._summarySelector.ShowNormal);
    this._summaryField.OnSelected += new System.Action(this._summarySelector.ShowSelected);
    this._descriptionField.OnDeselected += new System.Action(this._descriptionSelector.ShowNormal);
    this._descriptionField.OnSelected += new System.Action(this._descriptionSelector.ShowSelected);
    this._contactField.OnDeselected += new System.Action(this._contactSelector.ShowNormal);
    this._contactField.OnSelected += new System.Action(this._contactSelector.ShowSelected);
    this._summary = this._summaryField.text;
    this._contact = this._contactField.text;
    this._description = this._descriptionField.text;
    if (UIBugReportingOverlayController._spamCount > 2)
      this.ChangeState(UIBugReportingOverlayController.State.Error, ScriptLocalization.UI_BugReporting.TooManyReports, ScriptLocalization.UI_BugReporting_TooManyReports.Description);
    else
      this.ChangeState(UIBugReportingOverlayController.State.Loading);
  }

  private void ChangeState(
    UIBugReportingOverlayController.State newState,
    string header = "",
    string description = "")
  {
    if (newState == this._currentState)
      return;
    this._currentState = newState;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this._loadingContent.SetActive(this._currentState == UIBugReportingOverlayController.State.Loading);
    this._formContent.SetActive(this._currentState == UIBugReportingOverlayController.State.Form);
    this._sendingContent.SetActive(this._currentState == UIBugReportingOverlayController.State.Sending);
    this._completedContent.SetActive(this._currentState == UIBugReportingOverlayController.State.Completed);
    this._errorContent.SetActive(this._currentState == UIBugReportingOverlayController.State.Error);
    this._buttonHighlight.SetActive(this._currentState != UIBugReportingOverlayController.State.Loading && this._currentState != UIBugReportingOverlayController.State.Sending);
    if (this._currentState == UIBugReportingOverlayController.State.Loading)
      this.EnterLoadingState();
    else if (this._currentState == UIBugReportingOverlayController.State.Form)
      this.EnterFormState();
    else if (this._currentState == UIBugReportingOverlayController.State.Sending)
      this.EnterSendingState();
    else if (this._currentState == UIBugReportingOverlayController.State.Completed)
    {
      this.EnterCompletedState();
    }
    else
    {
      if (this._currentState != UIBugReportingOverlayController.State.Error)
        return;
      this.EnterErrorState();
      if (!string.IsNullOrEmpty(header))
        this._errorHeader.text = header;
      if (string.IsNullOrEmpty(description))
        return;
      this._erroDescription.text = description;
    }
  }

  public override void OnCancelButtonInput()
  {
    if (this._summaryField.isFocused || this._contactField.isFocused || this._descriptionField.isFocused || !this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void Update()
  {
    this._unityUserReportingUpdater.Reset();
    this.StartCoroutine((IEnumerator) this._unityUserReportingUpdater);
  }

  private void EnterLoadingState()
  {
    UnityUserReporting.CurrentClient.IsSelfReporting = false;
    UnityUserReporting.CurrentClient.SendEventsToAnalytics = false;
    UnityUserReporting.Configure(this.GetConfiguration());
    UnityUserReporting.CurrentClient.Platform.Post($"https://userreporting.cloud.unity3d.com/api/userreporting/projects/{UnityUserReporting.CurrentClient.ProjectIdentifier}/ping", "application/json", Encoding.UTF8.GetBytes("\"Ping\""), (Action<float, float>) ((upload, download) => { }), (Action<bool, byte[]>) ((result, bytes) => { }));
    this.StartCoroutine((IEnumerator) this.WaitForReport());
    this.CreateReport();
  }

  private IEnumerator WaitForReport()
  {
    float timeout = 0.0f;
    while (this._currentUserReport == null)
    {
      timeout += Time.unscaledDeltaTime;
      if ((double) timeout >= 10.0)
      {
        this.ChangeState(UIBugReportingOverlayController.State.Error);
        yield break;
      }
      yield return (object) null;
    }
    this.ChangeState(UIBugReportingOverlayController.State.Form);
  }

  private void EnterFormState()
  {
    this.OverrideDefault((Selectable) this._cancelButton);
    this.ActivateNavigation();
  }

  private void SubmitUserReport()
  {
    UIBugReportingOverlayController.ValidationResult result;
    if (!this.IsReportValid(out result))
    {
      if (result == UIBugReportingOverlayController.ValidationResult.Spam)
      {
        ++UIBugReportingOverlayController._spamCount;
        this.ChangeState(UIBugReportingOverlayController.State.Error, "Invalid Report!", "If you do not enter a valid summary and description of your issue you will be locked from sending bug reports");
        return;
      }
      if (result == UIBugReportingOverlayController.ValidationResult.Invalid)
      {
        this.ChangeState(UIBugReportingOverlayController.State.Error, "Invalid Report!", "Please make sure you're entering a correct summary and description of the issue");
        return;
      }
    }
    this._currentUserReport.Summary = this._summaryField.text;
    this._currentUserReport.Fields.Add(new UserReportNamedValue("Description", this._descriptionField.text));
    if (this._contact != this._contactField.text)
      this._currentUserReport.Fields.Add(new UserReportNamedValue("Contact", this._contactField.text));
    this._currentUserReport.Fields.Add(new UserReportNamedValue("Category", "Bug"));
    this.ChangeState(UIBugReportingOverlayController.State.Sending);
  }

  private bool IsReportValid(
    out UIBugReportingOverlayController.ValidationResult result)
  {
    string str1 = this._summary.StripWhitespace();
    string str2 = this._description.StripWhitespace();
    string str3 = this._summaryField.text.StripWhitespace();
    string str4 = this._descriptionField.text.StripWhitespace();
    if (string.IsNullOrEmpty(str3) || string.IsNullOrEmpty(str4))
    {
      result = UIBugReportingOverlayController.ValidationResult.Invalid;
      return false;
    }
    if (str1 == str3 && str2 == str4)
    {
      result = UIBugReportingOverlayController.ValidationResult.Spam;
      return false;
    }
    result = UIBugReportingOverlayController.ValidationResult.Valid;
    return true;
  }

  private void EnterSendingState()
  {
    UnityUserReporting.CurrentClient.SendUserReport(this._currentUserReport, (Action<float, float>) ((uploadProgress, downloadProgress) => this._sendingStatus.text = $"SENDING {uploadProgress:P}"), (Action<bool, UserReport>) ((success, br2) =>
    {
      if (!success)
        this.ChangeState(UIBugReportingOverlayController.State.Error);
      else
        this.ChangeState(UIBugReportingOverlayController.State.Completed);
    }));
  }

  private void EnterCompletedState()
  {
    this.OverrideDefault((Selectable) this._completedConfirmButton);
    this.ActivateNavigation();
  }

  private void EnterErrorState()
  {
    this.OverrideDefault((Selectable) this._errorConfirmButton);
    this.ActivateNavigation();
  }

  private void CreateReport()
  {
    UnityUserReporting.CurrentClient.TakeScreenshotFromSource(Screen.width, Screen.height, (object) Camera.main, (Action<UserReportScreenshot>) (s => { }));
    UnityUserReporting.CurrentClient.TakeScreenshotFromSource(Screen.width, Screen.height, (object) Camera.main, (Action<UserReportScreenshot>) (s => { }));
    UnityUserReporting.CurrentClient.CreateUserReport((Action<UserReport>) (userReport =>
    {
      Debug.Log((object) "Created user report".Colour(Color.yellow));
      if (string.IsNullOrEmpty(userReport.ProjectIdentifier))
        Debug.LogWarning((object) "The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
      string path1 = Path.Combine(Application.persistentDataPath, "saves");
      COTLDataReadWriter<DataManager> cotlDataReadWriter1 = new COTLDataReadWriter<DataManager>();
      string path2_1 = SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT);
      string filename1 = path2_1;
      if (cotlDataReadWriter1.FileExists(filename1))
      {
        byte[] data = Compression.Compress(File.ReadAllBytes(Path.Combine(path1, path2_1)));
        userReport.Attachments.Add(new UserReportAttachment("SaveFile", "SaveFile.zip", "application/zip", data));
      }
      COTLDataReadWriter<SettingsData> cotlDataReadWriter2 = new COTLDataReadWriter<SettingsData>();
      string path2_2 = "settings.json";
      string filename2 = path2_2;
      if (cotlDataReadWriter2.FileExists(filename2))
      {
        byte[] data = Compression.Compress(File.ReadAllBytes(Path.Combine(path1, path2_2)));
        userReport.Attachments.Add(new UserReportAttachment("SettingsFile", "SettingsFile.zip", "application/zip", data));
      }
      MonoSingleton<MMLogger>.Instance.Enabled = false;
      if (File.Exists(MMLogger.GetFileDirectory()))
      {
        byte[] data = Compression.Compress(File.ReadAllBytes(MMLogger.GetFileDirectory()));
        userReport.Attachments.Add(new UserReportAttachment("MMLog", "MMLog.zip", "application/log", data));
      }
      MonoSingleton<MMLogger>.Instance.Enabled = true;
      string str1 = "Unknown";
      string str2 = "0.0";
      foreach (UserReportNamedValue reportNamedValue in userReport.DeviceMetadata)
      {
        if (reportNamedValue.Name == "Platform")
          str1 = reportNamedValue.Value;
        if (reportNamedValue.Name == "Version")
          str2 = reportNamedValue.Value;
      }
      userReport.Dimensions.Add(new UserReportNamedValue("Platform.Version", $"{str1}.{str2}"));
      this._currentUserReport = userReport;
    }));
  }

  private UserReportingClientConfiguration GetConfiguration()
  {
    return new UserReportingClientConfiguration();
  }

  private enum State
  {
    Undefined,
    Loading,
    Form,
    Sending,
    Completed,
    Error,
  }

  private enum ValidationResult
  {
    Valid,
    Invalid,
    Spam,
  }
}
