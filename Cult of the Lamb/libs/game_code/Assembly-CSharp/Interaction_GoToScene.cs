// Decompiled with JetBrains decompiler
// Type: Interaction_GoToScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
