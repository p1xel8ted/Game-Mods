// Decompiled with JetBrains decompiler
// Type: LocaliseForPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Unify;
using UnityEngine;

#nullable disable
public class LocaliseForPlatform : MonoBehaviour
{
  public Localize _Localize;
  public string SwitchTerm = "";
  public string PS4Term = "";
  public string PS5Term = "";
  public string GCTerm = "";
  public bool set;

  public void Start()
  {
    if (this.set)
      return;
    this._Localize = this.GetComponent<Localize>();
    if (UnifyManager.platform == UnifyManager.Platform.Switch && this.SwitchTerm != "")
      this._Localize.Term = this.SwitchTerm;
    else if (UnifyManager.platform == UnifyManager.Platform.PS4 && this.PS4Term != "")
      this._Localize.Term = this.PS4Term;
    else if (UnifyManager.platform == UnifyManager.Platform.PS5 && this.PS5Term != "")
      this._Localize.Term = this.PS5Term;
    else if (UnifyManager.platform == UnifyManager.Platform.GameCoreConsole && this.GCTerm != "")
    {
      this._Localize.Term = this.GCTerm;
    }
    else
    {
      this.set = true;
      return;
    }
    this._Localize.OnLocalize(true);
    this.set = true;
  }
}
