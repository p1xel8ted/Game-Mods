// Decompiled with JetBrains decompiler
// Type: SimpleState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
