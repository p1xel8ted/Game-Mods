// Decompiled with JetBrains decompiler
// Type: SimpleStateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
