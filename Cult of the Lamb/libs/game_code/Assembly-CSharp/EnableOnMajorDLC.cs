// Decompiled with JetBrains decompiler
// Type: EnableOnMajorDLC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class EnableOnMajorDLC : MonoBehaviour
{
  public TextMeshProUGUI text;
  public bool disableInsteadOfEnable;
  public bool instantCheck;

  public void Start()
  {
    if ((Object) this.text != (Object) null)
      this.text.alpha = 0.0f;
    this.StartCoroutine(this.CheckForDLC());
  }

  public IEnumerator CheckForDLC()
  {
    EnableOnMajorDLC enableOnMajorDlc = this;
    if (!enableOnMajorDlc.instantCheck)
      yield return (object) new WaitForSecondsRealtime(1f);
    if (!enableOnMajorDlc.disableInsteadOfEnable)
    {
      if (GameManager.AuthenticateMajorDLC())
      {
        enableOnMajorDlc.gameObject.SetActive(true);
        if ((Object) enableOnMajorDlc.text != (Object) null)
          ShortcutExtensionsTMPText.DOFade(enableOnMajorDlc.text, 1f, 1f);
      }
      else
        enableOnMajorDlc.gameObject.SetActive(false);
    }
    else if (GameManager.AuthenticateMajorDLC())
    {
      enableOnMajorDlc.gameObject.SetActive(false);
    }
    else
    {
      enableOnMajorDlc.gameObject.SetActive(true);
      if ((Object) enableOnMajorDlc.text != (Object) null)
        ShortcutExtensionsTMPText.DOFade(enableOnMajorDlc.text, 1f, 1f);
    }
  }

  public void OnEnable() => this.StartCoroutine(this.CheckForDLC());
}
