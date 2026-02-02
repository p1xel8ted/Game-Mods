// Decompiled with JetBrains decompiler
// Type: DemoOverController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using UnityEngine;

#nullable disable
public class DemoOverController : MonoBehaviour
{
  public float Timer;

  public void Update()
  {
    this.Timer += Time.deltaTime;
    if ((double) this.Timer <= 2.0 || !InputManager.Gameplay.GetInteractButtonDown() && !InputManager.Gameplay.GetAttackButtonDown())
      return;
    UIManager.PlayAudio("event:/ui/confirm_selection");
    UIManager.PlayAudio("event:/sermon/Sermon Upgrade Menu Appear");
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 3f, "", (System.Action) null);
  }
}
