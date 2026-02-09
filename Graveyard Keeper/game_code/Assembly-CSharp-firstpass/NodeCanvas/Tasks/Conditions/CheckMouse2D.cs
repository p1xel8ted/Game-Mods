// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckMouse2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("System Events")]
[Name("Check Mouse 2D", 0)]
[Task.EventReceiver(new string[] {"OnMouseEnter", "OnMouseExit", "OnMouseOver"})]
public class CheckMouse2D : ConditionTask<Collider2D>
{
  public MouseInteractionTypes checkType;

  public override string info => this.checkType.ToString();

  public override bool OnCheck() => false;

  public void OnMouseEnter()
  {
    if (this.checkType != MouseInteractionTypes.MouseEnter)
      return;
    this.YieldReturn(true);
  }

  public void OnMouseExit()
  {
    if (this.checkType != MouseInteractionTypes.MouseExit)
      return;
    this.YieldReturn(true);
  }

  public void OnMouseOver()
  {
    if (this.checkType != MouseInteractionTypes.MouseOver)
      return;
    this.YieldReturn(true);
  }
}
