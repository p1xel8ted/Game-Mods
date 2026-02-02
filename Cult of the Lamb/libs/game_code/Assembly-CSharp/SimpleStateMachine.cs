// Decompiled with JetBrains decompiler
// Type: SimpleStateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class SimpleStateMachine
{
  public SimpleState currentState;

  public void Update()
  {
    if (this.currentState == null)
      return;
    this.currentState.Update();
  }

  public void SetState(SimpleState newState)
  {
    if (this.currentState != null)
      this.currentState.OnExit();
    this.currentState = newState;
    this.currentState.Init(this);
    if (this.currentState == null)
      return;
    this.currentState.OnEnter();
  }

  public SimpleState GetCurrentState() => this.currentState;
}
