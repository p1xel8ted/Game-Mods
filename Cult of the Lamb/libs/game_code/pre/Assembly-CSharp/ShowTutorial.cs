// Decompiled with JetBrains decompiler
// Type: ShowTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using src.UI.Overlays.TutorialOverlay;
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
}
