// Decompiled with JetBrains decompiler
// Type: OnScreenKeyboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public bool keyboardShown;
  public static bool isSubmit;
  public bool keepInputSelect;
  public MMInputField inputField;
  public GameObject prevSelectable;
  public bool SubmitOnClose = true;
  public Callback<GamepadTextInputDismissed_t> m_GamepadTextInputDismissed;
  public string prevText = "";
  public bool Selectable;

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

  public void Start()
  {
    Debug.Log((object) "OSK:Start");
    Debug.Log((object) "Registered native keyboard callbacks.");
    this.inputField = this.GetComponent<MMInputField>();
  }

  public void OnDestroy()
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
    Debug.Log((object) ("Current Selectable: " + ((object) this.prevSelectable)?.ToString()));
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

  public void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t pCallback)
  {
  }
}
