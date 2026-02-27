// Decompiled with JetBrains decompiler
// Type: UIFollowerXP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIFollowerXP : BaseMonoBehaviour
{
  public CanvasGroup canvasGroup;
  public GameObject IconPrefab;
  public Transform ContainerTransform;
  private List<UIFollowerXPIcon> Icons = new List<UIFollowerXPIcon>();
  private bool ReleaseButton;

  public void Play()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      UIFollowerXPIcon component = Object.Instantiate<GameObject>(this.IconPrefab, this.ContainerTransform).GetComponent<UIFollowerXPIcon>();
      this.Icons.Add(component);
      component.Play(allBrain);
    }
    this.StartCoroutine((IEnumerator) this.AwaitClose());
  }

  private IEnumerator AwaitClose()
  {
    UIFollowerXP uiFollowerXp = this;
    Time.timeScale = 0.0f;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      uiFollowerXp.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    float num = -0.1f;
    foreach (UIFollowerXPIcon icon in uiFollowerXp.Icons)
      icon.UpdateXP(num += 0.1f);
    while (!InputManager.UI.GetAcceptButtonHeld() && !InputManager.UI.GetCancelButtonHeld())
      yield return (object) null;
    Progress = 0.0f;
    Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      uiFollowerXp.canvasGroup.alpha = (float) (1.0 - (double) Progress / (double) Duration);
      yield return (object) null;
    }
    Time.timeScale = 1f;
    Object.Destroy((Object) uiFollowerXp.gameObject);
  }
}
