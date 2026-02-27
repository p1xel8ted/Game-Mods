// Decompiled with JetBrains decompiler
// Type: HideIfUIDisabled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class HideIfUIDisabled : BaseMonoBehaviour
{
  private SpriteRenderer[] SpriteRenderers;
  private Coroutine cHideAllRoutine;

  private void OnEnable()
  {
    if (CheatConsole.HidingUI)
      this.HideUI();
    CheatConsole.OnHideUI += new System.Action(this.HideUI);
    CheatConsole.OnShowUI += new System.Action(this.ShowUI);
  }

  private void OnDisable()
  {
    CheatConsole.OnHideUI -= new System.Action(this.HideUI);
    CheatConsole.OnShowUI -= new System.Action(this.ShowUI);
  }

  private void HideUI()
  {
    if (this.cHideAllRoutine != null)
      this.StopCoroutine(this.cHideAllRoutine);
    this.cHideAllRoutine = this.StartCoroutine((IEnumerator) this.HideAllRoutine());
  }

  private IEnumerator HideAllRoutine()
  {
    HideIfUIDisabled hideIfUiDisabled = this;
    while (true)
    {
      hideIfUiDisabled.SpriteRenderers = hideIfUiDisabled.GetComponentsInChildren<SpriteRenderer>();
      foreach (Renderer spriteRenderer in hideIfUiDisabled.SpriteRenderers)
        spriteRenderer.enabled = false;
      yield return (object) null;
    }
  }

  private void ShowUI()
  {
    if (this.cHideAllRoutine != null)
      this.StopCoroutine(this.cHideAllRoutine);
    foreach (Renderer spriteRenderer in this.SpriteRenderers)
      spriteRenderer.enabled = true;
  }
}
