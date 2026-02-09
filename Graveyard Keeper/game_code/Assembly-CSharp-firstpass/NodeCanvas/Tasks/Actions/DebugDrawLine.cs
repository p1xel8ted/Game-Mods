// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.DebugDrawLine
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
public class DebugDrawLine : ActionTask
{
  public BBParameter<Vector3> from;
  public BBParameter<Vector3> to;
  public Color color = Color.white;
  public float timeToShow = 0.1f;

  public override void OnExecute()
  {
    Debug.DrawLine(this.from.value, this.to.value, this.color, this.timeToShow);
    this.EndAction(true);
  }
}
