// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.IState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace NodeCanvas.StateMachines;

public interface IState
{
  string name { get; }

  string tag { get; }

  float elapsedTime { get; }

  FSM FSM { get; }

  FSMConnection[] GetTransitions();

  bool CheckTransitions();

  void Finish(bool success);
}
