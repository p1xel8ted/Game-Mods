// Decompiled with JetBrains decompiler
// Type: UICrownAbilitySelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UICrownAbilitySelect : BaseMonoBehaviour
{
  public bool WaitForNoConversation;
  public CanvasGroup canvasGroup;

  public void OnEnable() => this.StartCoroutine((IEnumerator) this.DoRoutine());

  public IEnumerator DoRoutine()
  {
    UICrownAbilitySelect crownAbilitySelect = this;
    if (crownAbilitySelect.WaitForNoConversation)
    {
      GameManager gameManager = GameManager.GetInstance();
      while (gameManager.CamFollowTarget.IN_CONVERSATION)
        yield return (object) null;
      gameManager = (GameManager) null;
    }
    Time.timeScale = 0.0f;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      crownAbilitySelect.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    crownAbilitySelect.canvasGroup.alpha = 1f;
    while (!InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    Progress = 0.0f;
    Duration = 0.5f;
    Time.timeScale = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      crownAbilitySelect.canvasGroup.alpha = (float) (1.0 - (double) Progress / (double) Duration);
      yield return (object) null;
    }
    Object.Destroy((Object) crownAbilitySelect.gameObject);
  }
}
