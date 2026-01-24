// Decompiled with JetBrains decompiler
// Type: I2.Loc.CustomLocalizeCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace I2.Loc;

[AddComponentMenu("I2/Localization/I2 Localize Callback")]
public class CustomLocalizeCallback : MonoBehaviour
{
  public UnityEvent _OnLocalize = new UnityEvent();

  public void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
  }

  public void OnLocalize() => this._OnLocalize.Invoke();
}
