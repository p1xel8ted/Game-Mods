// Decompiled with JetBrains decompiler
// Type: SimpleState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public abstract class SimpleState
{
  public SimpleStateMachine parentStateMachine;

  public void Init(SimpleStateMachine parentStateMachine)
  {
    this.parentStateMachine = parentStateMachine;
  }

  public abstract void OnEnter();

  public abstract void Update();

  public abstract void OnExit();

  public virtual bool IsComplete() => false;
}
