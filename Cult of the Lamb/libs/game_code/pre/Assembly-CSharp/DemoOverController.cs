// Decompiled with JetBrains decompiler
// Type: DemoOverController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using UnityEngine;

#nullable disable
public class DemoOverController : MonoBehaviour
{
  private float Timer;

  private void Update()
  {
    this.Timer += Time.deltaTime;
    if ((double) this.Timer <= 2.0 || !InputManager.Gameplay.GetInteractButtonDown() && !InputManager.Gameplay.GetAttackButtonDown())
      return;
    UIManager.PlayAudio("event:/ui/confirm_selection");
    UIManager.PlayAudio("event:/sermon/Sermon Upgrade Menu Appear");
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 3f, "", (System.Action) null);
  }
}
