// Decompiled with JetBrains decompiler
// Type: Interaction_GoToScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;

#nullable disable
public class Interaction_GoToScene : Interaction
{
  public string SceneToLoad;
  public float FadeDuration;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, this.SceneToLoad, this.FadeDuration, "", (System.Action) null);
  }
}
