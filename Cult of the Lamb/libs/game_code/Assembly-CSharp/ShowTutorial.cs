// Decompiled with JetBrains decompiler
// Type: ShowTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UI.Overlays.TutorialOverlay;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class ShowTutorial : BaseMonoBehaviour
{
  public TutorialTopic TutorialTopic;
  public bool ForcePlay;
  public UnityEvent Callback;

  public void Play()
  {
    Debug.Log((object) "PLAY!");
    Debug.Log((object) ("DataManager.Instance.TryRevealTutorialTopic(TutorialTopic): " + DataManager.Instance.TryRevealTutorialTopic(this.TutorialTopic).ToString()));
    if (!DataManager.Instance.TryRevealTutorialTopic(this.TutorialTopic) && !this.ForcePlay)
      return;
    UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(this.TutorialTopic);
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => this.Callback?.Invoke());
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__3_0() => this.Callback?.Invoke();
}
