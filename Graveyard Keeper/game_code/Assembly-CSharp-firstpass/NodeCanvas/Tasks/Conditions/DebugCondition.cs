// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.DebugCondition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("Simply use to debug return true or false by inverting the condition if needed")]
[Category("✫ Utility")]
public class DebugCondition : ConditionTask
{
  public override bool OnCheck() => false;
}
