// Decompiled with JetBrains decompiler
// Type: PlatformLocaliser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Unify;
using UnityEngine;

#nullable disable
public class PlatformLocaliser : MonoBehaviour
{
  public Localize _ISSLocalize;
  public bool set;

  public void Start()
  {
    if (this.set)
      return;
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.PS4:
      case UnifyManager.Platform.PS5:
        this._ISSLocalize.Term = "UI/PressAnyButtonToStart_PLAYSTATION";
        break;
      case UnifyManager.Platform.Switch:
        this._ISSLocalize.Term = "UI/PressAnyButtonToStart_SWITCH";
        break;
      default:
        this._ISSLocalize.Term = "UI/PressAnyButtonToStart";
        break;
    }
    this._ISSLocalize.OnLocalize(true);
    this.set = true;
  }
}
