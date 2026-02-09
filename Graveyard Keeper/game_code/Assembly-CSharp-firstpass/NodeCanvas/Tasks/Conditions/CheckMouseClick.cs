// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckMouseClick
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Task.EventReceiver(new string[] {"OnMouseDown", "OnMouseUp"})]
[Category("System Events")]
public class CheckMouseClick : ConditionTask<Collider>
{
  public MouseClickEvent checkType;

  public override string info => this.checkType.ToString();

  public override bool OnCheck() => false;

  public void OnMouseDown()
  {
    if (this.checkType != MouseClickEvent.MouseDown)
      return;
    this.YieldReturn(true);
  }

  public void OnMouseUp()
  {
    if (this.checkType != MouseClickEvent.MouseUp)
      return;
    this.YieldReturn(true);
  }
}
