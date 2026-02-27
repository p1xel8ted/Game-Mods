// Decompiled with JetBrains decompiler
// Type: OnScreenKeyboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class OnScreenKeyboard : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public UnityEvent OnSkip;
  public GameObject debugKeyboard;
  public string title;
  public string description;
  private bool keyboardShown;
  private static bool isSubmit;
  public bool keepInputSelect;
  private MMInputField inputField;
  private GameObject prevSelectable;
  public bool SubmitOnClose = true;
  protected Callback<GamepadTextInputDismissed_t> m_GamepadTextInputDismissed;
  private string prevText = "";
  private bool Selectable;

  public static bool IsSubmit
  {
    get
    {
      if (!OnScreenKeyboard.isSubmit)
        return false;
      OnScreenKeyboard.isSubmit = false;
      return true;
    }
  }

  private void Start()
  {
    Debug.Log((object) "OSK:Start");
    Debug.Log((object) "Registered native keyboard callbacks.");
    this.inputField = this.GetComponent<MMInputField>();
  }

  private void OnDestroy()
  {
  }

  public void ShowKeyboard()
  {
    Debug.Log((object) "Show keyboard");
    OnScreenKeyboard.isSubmit = false;
    this.keepInputSelect = true;
    if (this.keyboardShown)
      return;
    string str = this.GetComponent<MMInputField>().text;
    if (str == null)
      str = "";
    else
      this.prevText = str;
    if ((Object) this.debugKeyboard != (Object) null)
    {
      this.debugKeyboard.SetActive(true);
      this.debugKeyboard.transform.Find("CurrentText").gameObject.GetComponent<Text>().text = str;
      this.keyboardShown = true;
    }
    this.prevSelectable = EventSystem.current.currentSelectedGameObject;
    Debug.Log((object) ("Current Selectable: " + (object) this.prevSelectable));
    Debug.Log((object) ("Shown keyboard: " + this.keyboardShown.ToString()));
  }

  public void KeyboardFinished(string message)
  {
    Debug.Log((object) ("Keyboard Text Entry Finished: " + message));
    this.keyboardShown = false;
    this.keepInputSelect = false;
    if (message == "")
      this.inputField.text = this.prevText;
    else
      this.inputField.text = message;
    if (this.SubmitOnClose)
    {
      OnScreenKeyboard.isSubmit = true;
      this.inputField.onEndEdit.Invoke(message);
    }
    if (!((Object) this.prevSelectable != (Object) null))
      return;
    this.prevSelectable.GetComponent<UnityEngine.UI.Selectable>().Select();
  }

  public void OnSelect(BaseEventData eventData)
  {
    if (!this.Selectable)
    {
      this.Selectable = true;
    }
    else
    {
      Debug.Log((object) ("Input Element selected: " + this.gameObject.name));
      this.ShowKeyboard();
    }
  }

  public void Update()
  {
    if (!this.keyboardShown || !this.keepInputSelect || !((Object) EventSystem.current.currentSelectedGameObject != (Object) this.inputField.gameObject))
      return;
    this.inputField.Select();
  }

  private void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t pCallback)
  {
  }
}
