// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.LoadBlackboard
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Loads the blackboard variables previously saved in the provided PlayerPrefs key if at all. Returns false if no saves found or load was failed")]
[Category("✫ Blackboard")]
public class LoadBlackboard : ActionTask<Blackboard>
{
  [RequiredField]
  public BBParameter<string> saveKey;

  public override string info => $"Load Blackboard [{this.saveKey.ToString()}]";

  public override void OnExecute() => this.EndAction(this.agent.Load(this.saveKey.value));
}
