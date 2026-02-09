// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UniversalDelegateParam`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas.Nodes;

public class UniversalDelegateParam<TArray, TValue> : UniversalDelegateParam<TArray>
{
  public ValueInput<TValue>[] valueInputs;

  public override void RegisterAsInput(FlowNode node)
  {
    this.valueInputs = (ValueInput<TValue>[]) null;
    if (this.paramsArrayNeeded && this.paramsArrayCount >= 0)
    {
      this.valueInputs = new ValueInput<TValue>[this.paramsArrayCount];
      for (int index = 0; index <= this.paramsArrayCount - 1; ++index)
        this.valueInputs[index] = node.AddValueInput<TValue>($"{this.paramDef.portName} #{index.ToString()}", this.paramDef.portId + index.ToString());
    }
    else
      base.RegisterAsInput(node);
  }

  public override void SetFromInput()
  {
    if (this.paramsArrayNeeded && this.paramsArrayCount >= 0 && this.valueInputs != null && this.valueInputs.Length == this.paramsArrayCount)
    {
      TValue[] objArray = new TValue[this.paramsArrayCount];
      for (int index = 0; index <= this.paramsArrayCount - 1; ++index)
        objArray[index] = this.valueInputs[index].value;
      try
      {
        this.value = (TArray) objArray;
      }
      catch
      {
        base.SetFromInput();
      }
    }
    else
      base.SetFromInput();
  }
}
