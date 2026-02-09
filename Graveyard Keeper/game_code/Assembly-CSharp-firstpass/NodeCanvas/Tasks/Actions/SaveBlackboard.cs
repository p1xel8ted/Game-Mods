// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SaveBlackboard
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Saves the blackboard variables in the provided key and to be loaded later on")]
[Category("✫ Blackboard")]
public class SaveBlackboard : ActionTask<Blackboard>
{
  [RequiredField]
  public BBParameter<string> saveKey;

  public override string info => $"Save Blackboard [{this.saveKey.ToString()}]";

  public override void OnExecute()
  {
    this.agent.Save(this.saveKey.value);
    this.EndAction();
  }
}
