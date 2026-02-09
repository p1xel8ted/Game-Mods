// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetBoolean
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Description("Set a blackboard boolean variable")]
public class SetBoolean : ActionTask
{
  [RequiredField]
  [BlackboardOnly]
  public BBParameter<bool> boolVariable;
  public SetBoolean.BoolSetModes setTo = SetBoolean.BoolSetModes.True;

  public override string info
  {
    get
    {
      return this.setTo == SetBoolean.BoolSetModes.Toggle ? "Toggle " + this.boolVariable.ToString() : $"Set {this.boolVariable.ToString()} to {this.setTo.ToString()}";
    }
  }

  public override void OnExecute()
  {
    this.boolVariable.value = this.setTo != SetBoolean.BoolSetModes.Toggle ? this.setTo == SetBoolean.BoolSetModes.True : !this.boolVariable.value;
    this.EndAction();
  }

  public enum BoolSetModes
  {
    False,
    True,
    Toggle,
  }
}
