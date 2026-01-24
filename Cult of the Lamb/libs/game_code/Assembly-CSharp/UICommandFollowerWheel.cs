// Decompiled with JetBrains decompiler
// Type: UICommandFollowerWheel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unify.Input;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UICommandFollowerWheel : BaseMonoBehaviour
{
  public CanvasGroup canvasGroup;
  public Action<UICommandFollowerWheel.ActivityChoice.AvailableCommands> CallbackClose;
  public CanvasScaler canvas;
  public static UICommandFollowerWheel Instance;
  public GameObject FollowerPortraitPrefab;
  public RectTransform Pointer;
  public List<UICommandFollowerWheel.ActivityChoice> ActivityChoices = new List<UICommandFollowerWheel.ActivityChoice>();
  public float ActivityDistance = 300f;
  public UICommandFollowerWheel.ActivityChoice Closest;
  public float Angle = 4.712389f;
  public float PointerDistance = 310f;
  public float PointerSpeed = 25f;
  public bool Released;

  public Player Input => RewiredInputManager.MainPlayer;

  public void GetAngles()
  {
    foreach (UICommandFollowerWheel.ActivityChoice activityChoice in this.ActivityChoices)
    {
      activityChoice.Angle = Utils.GetAngle(Vector3.zero, activityChoice.rectTransform.localPosition);
      activityChoice.Init();
    }
  }

  public void SetPositions()
  {
    foreach (UICommandFollowerWheel.ActivityChoice activityChoice in this.ActivityChoices)
      activityChoice.rectTransform.localPosition = Vector3.Lerp(activityChoice.rectTransform.localPosition, new Vector3(this.ActivityDistance * Mathf.Cos(activityChoice.Angle * ((float) Math.PI / 180f)), this.ActivityDistance * Mathf.Sin(activityChoice.Angle * ((float) Math.PI / 180f))), 25f * Time.unscaledDeltaTime);
  }

  public void AddPortrait(FollowerBrainInfo f)
  {
    FollowerCommandWheelPortrait component = UnityEngine.Object.Instantiate<GameObject>(this.FollowerPortraitPrefab, this.FollowerPortraitPrefab.transform.parent).GetComponent<FollowerCommandWheelPortrait>();
    component.gameObject.SetActive(true);
    component.Play(f);
  }

  public void OnEnable() => UICommandFollowerWheel.Instance = this;

  public void OnDisable()
  {
    if (!((UnityEngine.Object) UICommandFollowerWheel.Instance == (UnityEngine.Object) this))
      return;
    UICommandFollowerWheel.Instance = (UICommandFollowerWheel) null;
  }

  public void Start()
  {
    this.canvas = this.GetComponentInParent<CanvasScaler>();
    foreach (UICommandFollowerWheel.ActivityChoice activityChoice in this.ActivityChoices)
      activityChoice.Init();
    this.StartCoroutine((IEnumerator) this.DoLoop());
  }

  public IEnumerator DoLoop()
  {
    UICommandFollowerWheel commandFollowerWheel = this;
    commandFollowerWheel.Pointer.gameObject.SetActive(false);
    while (InputManager.UI.GetPageNavigateLeftHeld())
    {
      foreach (UICommandFollowerWheel.ActivityChoice activityChoice in commandFollowerWheel.ActivityChoices)
        activityChoice.rectTransform.localScale = Vector3.zero;
      yield return (object) null;
    }
    commandFollowerWheel.Pointer.gameObject.SetActive(true);
    Time.timeScale = 0.1f;
    PlayerFarming.Instance.Spine.UseDeltaTime = false;
    while (true)
    {
      if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) > 0.30000001192092896 || (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) > 0.30000001192092896)
        commandFollowerWheel.Angle = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.UI.GetHorizontalAxis(), InputManager.UI.GetVerticalAxis())) * ((float) Math.PI / 180f);
      commandFollowerWheel.Pointer.localPosition = Vector3.Lerp(commandFollowerWheel.Pointer.localPosition, new Vector3(commandFollowerWheel.PointerDistance * Mathf.Cos(commandFollowerWheel.Angle), commandFollowerWheel.PointerDistance * Mathf.Sin(commandFollowerWheel.Angle)), Time.unscaledDeltaTime * commandFollowerWheel.PointerSpeed);
      commandFollowerWheel.Pointer.eulerAngles = new Vector3(0.0f, 0.0f, Utils.GetAngle(Vector3.zero, commandFollowerWheel.Pointer.localPosition) - 90f);
      commandFollowerWheel.Closest = (UICommandFollowerWheel.ActivityChoice) null;
      float num1 = float.MaxValue;
      foreach (UICommandFollowerWheel.ActivityChoice activityChoice in commandFollowerWheel.ActivityChoices)
      {
        float num2 = Vector3.Distance(commandFollowerWheel.Pointer.position, activityChoice.StartingPosition);
        if (activityChoice.rectTransform.gameObject.activeSelf && (double) num2 < (double) num1)
        {
          num1 = num2;
          commandFollowerWheel.Closest = activityChoice;
        }
      }
      float num3 = -1f;
      foreach (UICommandFollowerWheel.ActivityChoice activityChoice in commandFollowerWheel.ActivityChoices)
      {
        Vector3 b = Vector3.one * (commandFollowerWheel.Closest == activityChoice ? 1.1f : 1f);
        activityChoice.rectTransform.localScale = Vector3.Lerp(activityChoice.rectTransform.localScale, b, (++num3 + 25f) * Time.unscaledDeltaTime);
        float num4 = commandFollowerWheel.Closest == activityChoice ? 220f : 200f;
        activityChoice.Text.color = commandFollowerWheel.Closest == activityChoice ? Color.yellow : Color.white;
        activityChoice.rectTransform.localPosition = Vector3.Lerp(activityChoice.rectTransform.localPosition, new Vector3(num4 * Mathf.Cos(activityChoice.Angle * ((float) Math.PI / 180f)), num4 * Mathf.Sin(activityChoice.Angle * ((float) Math.PI / 180f))), 25f * Time.unscaledDeltaTime);
      }
      if (commandFollowerWheel.Closest == null || !commandFollowerWheel.Released || !InputManager.UI.GetAcceptButtonDown())
      {
        if (!InputManager.UI.GetAcceptButtonDown())
          commandFollowerWheel.Released = true;
        yield return (object) null;
      }
      else
        break;
    }
    int activity = (int) commandFollowerWheel.Closest.Activity;
    Debug.Log((object) commandFollowerWheel.Closest.Activity);
    foreach (UICommandFollowerWheel.ActivityChoice activityChoice in commandFollowerWheel.ActivityChoices)
    {
      activityChoice.Init();
      if ((UnityEngine.Object) activityChoice.Image != (UnityEngine.Object) null)
        activityChoice.Image.color = commandFollowerWheel.Closest.Activity == activityChoice.Activity ? Color.white : Color.black;
    }
    Time.timeScale = 1f;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    commandFollowerWheel.StartCoroutine((IEnumerator) commandFollowerWheel.CallBackRoutine());
    float Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      if ((UnityEngine.Object) commandFollowerWheel.Closest.Image != (UnityEngine.Object) null && Time.frameCount % 15 == 0)
        commandFollowerWheel.Closest.Image.color = commandFollowerWheel.Closest.Image.color == Color.white ? Color.black : Color.white;
      commandFollowerWheel.canvasGroup.alpha = (float) (1.0 - (double) Progress / (double) Duration);
      yield return (object) null;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) commandFollowerWheel.gameObject);
  }

  public IEnumerator CallBackRoutine()
  {
    yield return (object) new WaitForSeconds(0.1f);
    Action<UICommandFollowerWheel.ActivityChoice.AvailableCommands> callbackClose = this.CallbackClose;
    if (callbackClose != null)
      callbackClose(this.Closest.Activity);
  }

  [Serializable]
  public class ActivityChoice
  {
    public UICommandFollowerWheel.ActivityChoice.AvailableCommands Activity;
    public RectTransform rectTransform;
    public TextMeshProUGUI Text;
    public Image Image;
    public float Angle;
    public Vector3 StartingPosition;

    public void Init()
    {
      this.rectTransform.gameObject.name = this.Activity.ToString();
      this.Text = this.rectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>();
      this.Text.color = Color.white;
      this.Text.text = this.Activity.ToString();
      this.StartingPosition = this.rectTransform.position;
    }

    public enum AvailableCommands
    {
      ChopTrees,
      ClearWeeds,
      ClearRubble,
      Cancel,
    }
  }
}
