// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckKeyboardInput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("Input")]
public class CheckKeyboardInput : ConditionTask
{
  public PressTypes pressType;
  public KeyCode key = KeyCode.Space;

  public override string info => $"{this.pressType.ToString()} {this.key.ToString()}";

  public override bool OnCheck()
  {
    if (this.pressType == PressTypes.Down)
      return Input.GetKeyDown(this.key);
    if (this.pressType == PressTypes.Up)
      return Input.GetKeyUp(this.key);
    return this.pressType == PressTypes.Pressed && Input.GetKey(this.key);
  }
}
