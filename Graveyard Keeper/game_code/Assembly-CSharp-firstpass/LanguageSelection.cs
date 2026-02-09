// Decompiled with JetBrains decompiler
// Type: LanguageSelection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Language Selection")]
[RequireComponent(typeof (UIPopupList))]
public class LanguageSelection : MonoBehaviour
{
  public UIPopupList mList;

  public void Awake()
  {
    this.mList = this.GetComponent<UIPopupList>();
    this.Refresh();
  }

  public void Start()
  {
    EventDelegate.Add(this.mList.onChange, (EventDelegate.Callback) (() => Localization.language = UIPopupList.current.value));
  }

  public void Refresh()
  {
    if (!((Object) this.mList != (Object) null) || Localization.knownLanguages == null)
      return;
    this.mList.Clear();
    int index = 0;
    for (int length = Localization.knownLanguages.Length; index < length; ++index)
      this.mList.items.Add(Localization.knownLanguages[index]);
    this.mList.value = Localization.language;
  }
}
