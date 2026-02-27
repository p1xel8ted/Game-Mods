// Decompiled with JetBrains decompiler
// Type: I2.Loc.CustomLocalizeCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
