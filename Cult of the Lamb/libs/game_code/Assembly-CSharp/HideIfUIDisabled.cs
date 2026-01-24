// Decompiled with JetBrains decompiler
// Type: HideIfUIDisabled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HideIfUIDisabled : BaseMonoBehaviour
{
  public Renderer[] SpriteRenderers;
  public List<Renderer> HiddenRenderers = new List<Renderer>();
  public Coroutine cHideAllRoutine;

  public void OnEnable()
  {
    if (CheatConsole.HidingUI)
      this.HideUI();
    CheatConsole.OnHideUI += new System.Action(this.HideUI);
    CheatConsole.OnShowUI += new System.Action(this.ShowUI);
    PhotoModeManager.OnPhotoModeEnabled += new PhotoModeManager.PhotoEvent(this.HideUI);
    PhotoModeManager.OnPhotoModeDisabled += new PhotoModeManager.PhotoEvent(this.ShowUI);
  }

  public void OnDisable()
  {
    CheatConsole.OnHideUI -= new System.Action(this.HideUI);
    CheatConsole.OnShowUI -= new System.Action(this.ShowUI);
    PhotoModeManager.OnPhotoModeEnabled -= new PhotoModeManager.PhotoEvent(this.HideUI);
    PhotoModeManager.OnPhotoModeDisabled -= new PhotoModeManager.PhotoEvent(this.ShowUI);
  }

  public void HideUI()
  {
    if (this.cHideAllRoutine != null)
      this.StopCoroutine(this.cHideAllRoutine);
    this.cHideAllRoutine = this.StartCoroutine((IEnumerator) this.HideAllRoutine());
  }

  public IEnumerator HideAllRoutine()
  {
    HideIfUIDisabled hideIfUiDisabled = this;
    while (true)
    {
      hideIfUiDisabled.SpriteRenderers = hideIfUiDisabled.GetComponentsInChildren<Renderer>();
      foreach (Renderer spriteRenderer in hideIfUiDisabled.SpriteRenderers)
      {
        if (spriteRenderer.enabled)
        {
          spriteRenderer.enabled = false;
          if (!hideIfUiDisabled.HiddenRenderers.Contains(spriteRenderer))
            hideIfUiDisabled.HiddenRenderers.Add(spriteRenderer);
        }
      }
      yield return (object) null;
    }
  }

  public void ShowUI()
  {
    if (this.cHideAllRoutine != null)
      this.StopCoroutine(this.cHideAllRoutine);
    foreach (Renderer hiddenRenderer in this.HiddenRenderers)
      hiddenRenderer.enabled = true;
    this.HiddenRenderers.Clear();
  }
}
