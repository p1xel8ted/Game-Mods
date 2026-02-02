// Decompiled with JetBrains decompiler
// Type: UIVoodooStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UIVoodooStatue : UIHeartStatue
{
  public override bool CanUpgrade() => DataManager.Instance.ShrineVoodo < 4;

  public override void Repair() => DataManager.Instance.ShrineVoodo = 1;

  public override void Upgrade()
  {
    if (DataManager.Instance.ShrineVoodo >= 4 || this.Upgrading)
      return;
    Debug.Log((object) "Upgrade!");
    this.StartCoroutine((IEnumerator) this.DoUpgrade());
  }

  public override IEnumerator DoUpgrade()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIVoodooStatue uiVoodooStatue = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      uiVoodooStatue.ShowButton();
      uiVoodooStatue.Upgrading = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    uiVoodooStatue.Upgrading = true;
    uiVoodooStatue.HideButton();
    DataManager.Instance.ShrineVoodo = Mathf.Min(++DataManager.Instance.ShrineVoodo, 4);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.3f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
