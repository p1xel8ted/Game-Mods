// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MecanimSetFloat
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Set Parameter Float", 0)]
[Category("Animator")]
[Description("You can either use a parameter name OR hashID. Leave the parameter name empty or none to use hashID instead.")]
public class MecanimSetFloat : ActionTask<Animator>
{
  public BBParameter<string> parameter;
  public BBParameter<int> parameterHashID;
  public BBParameter<float> setTo;
  [SliderField(0, 1)]
  public float transitTime = 0.25f;
  public float currentValue;

  public override string info
  {
    get
    {
      return $"Mec.SetFloat {(string.IsNullOrEmpty(this.parameter.value) ? (object) this.parameterHashID.ToString() : (object) this.parameter.ToString())} to {this.setTo}";
    }
  }

  public override void OnExecute()
  {
    if ((double) this.transitTime <= 0.0)
    {
      this.Set(this.setTo.value);
      this.EndAction();
    }
    else
      this.currentValue = this.Get();
  }

  public override void OnUpdate()
  {
    this.Set(Mathf.Lerp(this.currentValue, this.setTo.value, this.elapsedTime / this.transitTime));
    if ((double) this.elapsedTime < (double) this.transitTime)
      return;
    this.EndAction(true);
  }

  public float Get()
  {
    return !string.IsNullOrEmpty(this.parameter.value) ? this.agent.GetFloat(this.parameter.value) : this.agent.GetFloat(this.parameterHashID.value);
  }

  public void Set(float newValue)
  {
    if (!string.IsNullOrEmpty(this.parameter.value))
      this.agent.SetFloat(this.parameter.value, newValue);
    else
      this.agent.SetFloat(this.parameterHashID.value, newValue);
  }
}
