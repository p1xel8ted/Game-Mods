// Decompiled with JetBrains decompiler
// Type: UISavedOption
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
  public string keyName;
  public UIPopupList mList;
  public UIToggle mCheck;
  public UIProgressBar mSlider;

  public string key
  {
    get => !string.IsNullOrEmpty(this.keyName) ? this.keyName : "NGUI State: " + this.name;
  }

  public void Awake()
  {
    this.mList = this.GetComponent<UIPopupList>();
    this.mCheck = this.GetComponent<UIToggle>();
    this.mSlider = this.GetComponent<UIProgressBar>();
  }

  public void OnEnable()
  {
    if ((Object) this.mList != (Object) null)
    {
      EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
      string str = PlayerPrefs.GetString(this.key);
      if (string.IsNullOrEmpty(str))
        return;
      this.mList.value = str;
    }
    else if ((Object) this.mCheck != (Object) null)
    {
      EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
      this.mCheck.value = PlayerPrefs.GetInt(this.key, this.mCheck.startsActive ? 1 : 0) != 0;
    }
    else if ((Object) this.mSlider != (Object) null)
    {
      EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
      this.mSlider.value = PlayerPrefs.GetFloat(this.key, this.mSlider.value);
    }
    else
    {
      string str = PlayerPrefs.GetString(this.key);
      UIToggle[] componentsInChildren = this.GetComponentsInChildren<UIToggle>(true);
      int index = 0;
      for (int length = componentsInChildren.Length; index < length; ++index)
      {
        UIToggle uiToggle = componentsInChildren[index];
        uiToggle.value = uiToggle.name == str;
      }
    }
  }

  public void OnDisable()
  {
    if ((Object) this.mCheck != (Object) null)
      EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
    else if ((Object) this.mList != (Object) null)
      EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
    else if ((Object) this.mSlider != (Object) null)
    {
      EventDelegate.Remove(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
    }
    else
    {
      UIToggle[] componentsInChildren = this.GetComponentsInChildren<UIToggle>(true);
      int index = 0;
      for (int length = componentsInChildren.Length; index < length; ++index)
      {
        UIToggle uiToggle = componentsInChildren[index];
        if (uiToggle.value)
        {
          PlayerPrefs.SetString(this.key, uiToggle.name);
          break;
        }
      }
    }
  }

  public void SaveSelection() => PlayerPrefs.SetString(this.key, UIPopupList.current.value);

  public void SaveState() => PlayerPrefs.SetInt(this.key, UIToggle.current.value ? 1 : 0);

  public void SaveProgress() => PlayerPrefs.SetFloat(this.key, UIProgressBar.current.value);
}
