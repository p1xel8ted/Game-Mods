// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetOtherBlackboardVariable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Description("Use this to set a variable on any blackboard by overriding the agent")]
public class SetOtherBlackboardVariable : ActionTask<Blackboard>
{
  [RequiredField]
  public BBParameter<string> targetVariableName;
  public BBObjectParameter newValue;

  public override string info
  {
    get
    {
      return $"<b>{this.targetVariableName.ToString()}</b> = {(this.newValue != null ? (object) this.newValue.ToString() : (object) "")}";
    }
  }

  public override void OnExecute()
  {
    this.agent.SetValue(this.targetVariableName.value, this.newValue.value);
    this.EndAction();
  }
}
