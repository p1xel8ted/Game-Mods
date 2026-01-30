// Decompiled with JetBrains decompiler
// Type: SimpleState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
