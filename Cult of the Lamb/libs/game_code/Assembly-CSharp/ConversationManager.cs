// Decompiled with JetBrains decompiler
// Type: ConversationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ConversationManager : BaseMonoBehaviour
{
  public static ConversationManager instance;
  public bool IN_CONVERSATION;
  public GameObject Speaker;
  public GameObject OptionWheel;
  public Image SpeechBubble;
  public RectTransform SpeechBubbleRT;
  public TextMeshProUGUI SpeechText;
  public List<Response> Responses;
  public List<TextMeshProUGUI> ResponseText;
  public float Angle;
  public float PointerAngle;
  public float AngleApprox;
  public float ArrowAngle;
  public GameObject Arrow;
  public GameObject Pointer;
  public GameObject PointerDistance;
  public GameObject CurrentGameObject;
  public static int CURRENT_ANSWER;

  public void OnEnable()
  {
    ConversationManager.instance = this;
    this.HideAll();
  }

  public static ConversationManager GetInstance() => ConversationManager.instance;

  public void Update()
  {
    if (!this.IN_CONVERSATION)
      return;
    this.SpeechBubbleRT.position = Camera.main.WorldToScreenPoint(this.Speaker.transform.position);
    this.MoveOptionWheel();
  }

  public void OnDisable()
  {
    ConversationManager.instance = (ConversationManager) null;
    this.HideAll();
  }

  public void NewConversation(string text, string ID, List<Response> Responses)
  {
    this.ShowAll();
    this.SpeechText.text = text;
    float x = this.SpeechText.preferredWidth + 40f;
    if ((double) x > (double) this.SpeechText.rectTransform.sizeDelta.x)
      x = this.SpeechText.rectTransform.sizeDelta.x + 40f;
    float y = this.SpeechText.preferredHeight + 40f;
    this.SpeechBubble.rectTransform.sizeDelta = new Vector2(x, y);
    this.SpeechBubbleRT = this.SpeechBubble.GetComponent<RectTransform>();
    GameManager.GetInstance().CameraSetConversationMode(true);
    GameManager.GetInstance().RemoveAllFromCamera();
    StateMachine component = GameObject.FindWithTag("Player").GetComponent<StateMachine>();
    component.CURRENT_STATE = StateMachine.State.InActive;
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.CURRENT_STATE = StateMachine.State.InActive;
    this.Responses = Responses;
    for (int index = 0; index < Responses.Count; ++index)
      this.ResponseText[index].text = Responses[index].text;
    this.IN_CONVERSATION = true;
  }

  public void EndConversation()
  {
    this.IN_CONVERSATION = false;
    GameObject.FindWithTag("Player").GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().CameraSetConversationMode(false);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddPlayerToCamera();
    StateMachine component = this.Speaker.GetComponent<StateMachine>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.CURRENT_STATE = StateMachine.State.Idle;
    this.HideAll();
    if (this.Responses[ConversationManager.CURRENT_ANSWER] == null || this.Responses[ConversationManager.CURRENT_ANSWER].Callback == null)
      return;
    this.Responses[ConversationManager.CURRENT_ANSWER].Callback();
  }

  public void ShowAll()
  {
    ConversationManager.CURRENT_ANSWER = 0;
    this.Angle = this.PointerAngle = this.AngleApprox = 0.0f;
    this.Arrow.transform.eulerAngles = Vector3.zero;
    this.Pointer.transform.eulerAngles = Vector3.zero;
    this.SpeechBubble.gameObject.SetActive(true);
    this.OptionWheel.SetActive(true);
    for (int index = 0; index < this.ResponseText.Count; ++index)
      this.ResponseText[index].text = "";
  }

  public void HideAll()
  {
    this.IN_CONVERSATION = false;
    this.SpeechBubble.gameObject.SetActive(false);
    this.OptionWheel.SetActive(false);
  }

  public void MoveOptionWheel()
  {
    if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) > 0.20000000298023224 || (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) > 0.20000000298023224)
    {
      this.Angle = Utils.GetAngle(new Vector3(InputManager.UI.GetHorizontalAxis(), InputManager.UI.GetVerticalAxis()), Vector3.zero) + 90f;
      this.CheckDistance();
    }
    this.PointerAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.Angle - (double) this.PointerAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.Angle - (double) this.PointerAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / 3.0);
    this.Pointer.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.PointerAngle);
    if ((UnityEngine.Object) this.CurrentGameObject != (UnityEngine.Object) null)
    {
      this.AngleApprox = Utils.GetAngle(this.CurrentGameObject.transform.localPosition, this.Arrow.transform.localPosition) + 90f;
      this.ArrowAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.AngleApprox - (double) this.ArrowAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.AngleApprox - (double) this.ArrowAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / 3.0);
      this.Arrow.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.ArrowAngle);
      this.CurrentGameObject.transform.localScale = new Vector3(1.1f, 1.1f);
    }
    if (!InputManager.UI.GetAcceptButtonUp())
      return;
    this.EndConversation();
  }

  public void CheckDistance()
  {
    float num1 = float.MaxValue;
    for (int index = 0; index < this.ResponseText.Count; ++index)
    {
      TextMeshProUGUI textMeshProUgui = this.ResponseText[index];
      textMeshProUgui.transform.localScale = new Vector3(1f, 1f);
      float num2 = Vector2.Distance((Vector2) this.PointerDistance.transform.position, (Vector2) textMeshProUgui.transform.position);
      if ((double) num2 < (double) num1 && textMeshProUgui.text != "")
      {
        ConversationManager.CURRENT_ANSWER = index;
        this.CurrentGameObject = textMeshProUgui.gameObject;
        num1 = num2;
      }
    }
  }
}
