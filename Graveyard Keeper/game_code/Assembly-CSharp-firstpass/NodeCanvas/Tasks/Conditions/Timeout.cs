// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Timeout
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("Will return true after a specific amount of time has passed and false while still counting down")]
[Category("✫ Utility")]
public class Timeout : ConditionTask
{
  public BBParameter<float> timeout = (BBParameter<float>) 1f;
  public float currentTime;
  public Coroutine coroutine;

  public override string info
  {
    get => $"Timeout {this.currentTime.ToString("0.00")}/{this.timeout.ToString()}";
  }

  public override void OnDisable()
  {
    if (this.coroutine == null)
      return;
    this.StopCoroutine(this.coroutine);
    this.coroutine = (Coroutine) null;
  }

  public override bool OnCheck()
  {
    if (this.coroutine == null)
    {
      this.currentTime = 0.0f;
      this.coroutine = this.StartCoroutine(this.Do());
    }
    if ((double) this.currentTime < (double) this.timeout.value)
      return false;
    this.coroutine = (Coroutine) null;
    return true;
  }

  public IEnumerator Do()
  {
    while ((double) this.currentTime < (double) this.timeout.value)
    {
      this.currentTime += Time.deltaTime;
      yield return (object) null;
    }
  }
}
