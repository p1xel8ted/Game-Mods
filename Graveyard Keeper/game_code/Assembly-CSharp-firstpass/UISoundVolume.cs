// Decompiled with JetBrains decompiler
// Type: UISoundVolume
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Sound Volume")]
[RequireComponent(typeof (UISlider))]
public class UISoundVolume : MonoBehaviour
{
  public void Awake()
  {
    UISlider component = this.GetComponent<UISlider>();
    component.value = NGUITools.soundVolume;
    EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnChange));
  }

  public void OnChange() => NGUITools.soundVolume = UIProgressBar.current.value;
}
