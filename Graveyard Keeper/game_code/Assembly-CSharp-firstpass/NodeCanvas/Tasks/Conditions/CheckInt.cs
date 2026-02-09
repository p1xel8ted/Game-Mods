// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckInt
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Blackboard")]
public class CheckInt : ConditionTask
{
  [BlackboardOnly]
  public BBParameter<int> valueA;
  public CompareMethod checkType;
  public BBParameter<int> valueB;

  public override string info
  {
    get
    {
      return this.valueA?.ToString() + OperationTools.GetCompareString(this.checkType) + this.valueB?.ToString();
    }
  }

  public override bool OnCheck()
  {
    return OperationTools.Compare(this.valueA.value, this.valueB.value, this.checkType);
  }
}
