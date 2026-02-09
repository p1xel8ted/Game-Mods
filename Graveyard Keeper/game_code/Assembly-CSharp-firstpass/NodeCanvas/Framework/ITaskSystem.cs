// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.ITaskSystem
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

public interface ITaskSystem
{
  Component agent { get; }

  IBlackboard blackboard { get; }

  float elapsedTime { get; }

  Object contextObject { get; }

  void SendTaskOwnerDefaults();

  void SendEvent(EventData eventData);

  void RecordUndo(string name);
}
