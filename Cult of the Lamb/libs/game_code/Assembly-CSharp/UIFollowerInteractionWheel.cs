// Decompiled with JetBrains decompiler
// Type: UIFollowerInteractionWheel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFollowerInteractionWheel : BaseMonoBehaviour
{
  public Transform Container;
  public static Color TextColor = new Color(0.9960784f, 0.9411765f, 0.827451f);
  public CanvasGroup canvasGroup;
  public Action<FollowerCommands> CallbackClose;
  public System.Action CallbackCancel;
  public CanvasScaler canvas;
  public static UIFollowerInteractionWheel Instance = (UIFollowerInteractionWheel) null;
  public RectTransform Pointer;
  public float PointerSpeed = 25f;
  public TextMeshProUGUI SermonName;
  public TextMeshProUGUI SermonDescription;
  public List<UIFollowerInteractionWheel.ActivityChoice> ActivityChoices = new List<UIFollowerInteractionWheel.ActivityChoice>();
  public float ActivityDistance = 200f;
  public Follower Follower;
  public string TitleText;
  public UIFollowerInteractionWheel.ActivityChoice Closest;
  public static float Angle = 4.712389f;
  public float PointerDistance = 50f;

  public void GetAngles()
  {
    foreach (UIFollowerInteractionWheel.ActivityChoice activityChoice in this.ActivityChoices)
    {
      activityChoice.Angle = Utils.GetAngle(Vector3.zero, activityChoice.rectTransform.localPosition);
      activityChoice.Init((Follower) null, FollowerCommands.Talk, false);
    }
  }

  public void SetPositions()
  {
    foreach (UIFollowerInteractionWheel.ActivityChoice activityChoice in this.ActivityChoices)
      activityChoice.rectTransform.localPosition = Vector3.Lerp(activityChoice.rectTransform.localPosition, new Vector3(this.ActivityDistance * Mathf.Cos(activityChoice.Angle * ((float) Math.PI / 180f)), this.ActivityDistance * Mathf.Sin(activityChoice.Angle * ((float) Math.PI / 180f))), 25f * Time.unscaledDeltaTime);
  }

  public void OnEnable() => UIFollowerInteractionWheel.Instance = this;

  public void OnDisable()
  {
    if (!((UnityEngine.Object) UIFollowerInteractionWheel.Instance == (UnityEngine.Object) this))
      return;
    UIFollowerInteractionWheel.Instance = (UIFollowerInteractionWheel) null;
  }

  public void Play(
    Follower Follower,
    List<CommandPosition> CommandsAndPositions,
    Action<FollowerCommands> CallbackClose,
    System.Action CallbackCancel,
    bool ResetAngle = false,
    FollowerCommands Title = FollowerCommands.None)
  {
    this.Follower = Follower;
    this.TitleText = Title == FollowerCommands.None ? "" : LocalizationManager.GetTranslation($"FollowerInteractions/{Title}");
    this.canvas = this.GetComponentInParent<CanvasScaler>();
    foreach (UIFollowerInteractionWheel.ActivityChoice activityChoice in this.ActivityChoices)
      activityChoice.rectTransform.gameObject.SetActive(false);
    foreach (CommandPosition commandsAndPosition in CommandsAndPositions)
    {
      if (commandsAndPosition != null)
      {
        foreach (UIFollowerInteractionWheel.ActivityChoice activityChoice in this.ActivityChoices)
        {
          if (activityChoice.WheelPosition == commandsAndPosition.WheelPosition)
            activityChoice.Init(Follower, commandsAndPosition.FollowerCommand, commandsAndPosition.ShowNotification);
        }
      }
    }
    this.CallbackClose = CallbackClose;
    this.CallbackCancel = CallbackCancel;
    this.StartCoroutine((IEnumerator) this.DoLoop());
    this.SermonName.text = this.SermonDescription.text = "";
    if (!ResetAngle)
      return;
    UIFollowerInteractionWheel.Angle = 4.712389f;
  }

  public IEnumerator DoLoop()
  {
    UIFollowerInteractionWheel interactionWheel = this;
    float Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      interactionWheel.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    interactionWheel.canvasGroup.alpha = 1f;
    while (true)
    {
      if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) > 0.30000001192092896 || (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) > 0.30000001192092896)
        UIFollowerInteractionWheel.Angle = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.UI.GetHorizontalAxis(), InputManager.UI.GetVerticalAxis())) * ((float) Math.PI / 180f);
      interactionWheel.Pointer.localPosition = Vector3.Lerp(interactionWheel.Pointer.localPosition, new Vector3(interactionWheel.PointerDistance * Mathf.Cos(UIFollowerInteractionWheel.Angle), interactionWheel.PointerDistance * Mathf.Sin(UIFollowerInteractionWheel.Angle)), Time.unscaledDeltaTime * interactionWheel.PointerSpeed);
      interactionWheel.Pointer.eulerAngles = new Vector3(0.0f, 0.0f, Utils.GetAngle(Vector3.zero, interactionWheel.Pointer.localPosition) - 90f);
      interactionWheel.Closest = (UIFollowerInteractionWheel.ActivityChoice) null;
      float num1 = float.MaxValue;
      foreach (UIFollowerInteractionWheel.ActivityChoice activityChoice in interactionWheel.ActivityChoices)
      {
        float num2 = Vector3.Distance(interactionWheel.Pointer.position, activityChoice.StartingPosition);
        if (activityChoice.rectTransform.gameObject.activeSelf && (double) num2 < (double) num1)
        {
          num1 = num2;
          interactionWheel.Closest = activityChoice;
        }
      }
      float num3 = -1f;
      foreach (UIFollowerInteractionWheel.ActivityChoice activityChoice in interactionWheel.ActivityChoices)
      {
        Vector3 b = Vector3.one * (interactionWheel.Closest == activityChoice ? 1.1f : 1f);
        activityChoice.rectTransform.localScale = Vector3.Lerp(activityChoice.rectTransform.localScale, b, (++num3 + 25f) * Time.unscaledDeltaTime);
        float num4 = interactionWheel.Closest == activityChoice ? 220f : 200f;
        activityChoice.Text.color = interactionWheel.Closest == activityChoice ? Color.yellow : UIFollowerInteractionWheel.TextColor;
        activityChoice.rectTransform.localPosition = Vector3.Lerp(activityChoice.rectTransform.localPosition, new Vector3(num4 * Mathf.Cos(activityChoice.Angle * ((float) Math.PI / 180f)), num4 * Mathf.Sin(activityChoice.Angle * ((float) Math.PI / 180f))), 25f * Time.unscaledDeltaTime);
      }
      if (interactionWheel.Closest != null)
      {
        interactionWheel.SermonName.text = interactionWheel.TitleText == "" ? interactionWheel.Closest.Text.text : interactionWheel.TitleText;
        interactionWheel.SermonDescription.text = interactionWheel.Closest.Description;
      }
      if (interactionWheel.Closest == null || !InputManager.UI.GetAcceptButtonDown())
      {
        if (!InputManager.UI.GetCancelButtonDown() || interactionWheel.CallbackCancel == null)
          yield return (object) null;
        else
          break;
      }
      else
        goto label_24;
    }
    System.Action callbackCancel = interactionWheel.CallbackCancel;
    if (callbackCancel != null)
    {
      callbackCancel();
      goto label_26;
    }
    goto label_26;
label_24:
    Action<FollowerCommands> callbackClose = interactionWheel.CallbackClose;
    if (callbackClose != null)
      callbackClose(interactionWheel.Closest.Activity);
label_26:
    Progress = 0.0f;
    Duration = 0.2f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      interactionWheel.canvasGroup.alpha = (float) (1.0 - (double) Progress / (double) Duration);
      yield return (object) null;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionWheel.gameObject);
  }

  [Serializable]
  public class ActivityChoice
  {
    public FollowerCommands Activity;
    public WheelPosition WheelPosition;
    public RectTransform rectTransform;
    public TextMeshProUGUI Text;
    public GameObject NotificationIcon;
    public Image Image;
    public float Angle;
    public Vector3 StartingPosition;
    public string Description;

    public void Init(Follower Follower, FollowerCommands Activity, bool showNotification)
    {
      this.rectTransform.gameObject.SetActive(true);
      this.Activity = Activity;
      this.rectTransform.gameObject.name = this.WheelPosition.ToString();
      this.Text = this.rectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>();
      this.Text.color = UIFollowerInteractionWheel.TextColor;
      this.Text.text = LocalizationManager.GetTranslation($"FollowerInteractions/{Activity}");
      this.Description = LocalizationManager.GetTranslation($"FollowerInteractions/{Activity}/Description");
      if (Activity == FollowerCommands.GiveWorkerCommand_2 && !Follower.Brain.Stats.WorkerBeenGivenOrders)
        this.Text.text = $"<color=yellow>{LocalizationManager.GetTranslation($"FollowerInteractions/{Activity}")}</color>";
      if ((UnityEngine.Object) this.NotificationIcon != (UnityEngine.Object) null)
        this.NotificationIcon.SetActive(showNotification);
      this.StartingPosition = this.rectTransform.position;
    }
  }
}
