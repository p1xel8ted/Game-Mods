// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckButtonInput
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
public class CheckButtonInput : ConditionTask
{
  public PressTypes pressType;
  [RequiredField]
  public BBParameter<string> buttonName = (BBParameter<string>) "Fire1";

  public override string info => $"{this.pressType.ToString()} {this.buttonName.ToString()}";

  public override bool OnCheck()
  {
    if (this.pressType == PressTypes.Down)
      return Input.GetButtonDown(this.buttonName.value);
    if (this.pressType == PressTypes.Up)
      return Input.GetButtonUp(this.buttonName.value);
    return this.pressType == PressTypes.Pressed && Input.GetButton(this.buttonName.value);
  }
}
