// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Wait
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Wait for a certain amount of time before continueing")]
[Category("Time")]
public class Wait : LatentActionNode<float>
{
  [CompilerGenerated]
  public float \u003CtimeLeft\u003Ek__BackingField;

  public float timeLeft
  {
    get => this.\u003CtimeLeft\u003Ek__BackingField;
    set => this.\u003CtimeLeft\u003Ek__BackingField = value;
  }

  public override IEnumerator Invoke(float time = 1f)
  {
    this.timeLeft = time;
    while ((double) this.timeLeft > 0.0)
    {
      this.timeLeft -= Time.deltaTime;
      this.timeLeft = Mathf.Max(this.timeLeft, 0.0f);
      yield return (object) null;
    }
  }
}
