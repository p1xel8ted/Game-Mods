// Decompiled with JetBrains decompiler
// Type: CheatChainRunner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CheatChainRunner : MonoBehaviour
{
  private CheatConsole cheatConsole;
  private int i;

  public void RunChain(string[] CheatChain, float[] timer)
  {
    this.cheatConsole = UnityEngine.Object.FindObjectOfType<CheatConsole>();
    this.StartCoroutine((IEnumerator) this.ChainRun(CheatChain, timer));
  }

  private IEnumerator ChainRun(string[] CheatChain, float[] timer)
  {
    CheatChainRunner cheatChainRunner = this;
    for (cheatChainRunner.i = 0; cheatChainRunner.i < CheatChain.Length; ++cheatChainRunner.i)
    {
      while ((UnityEngine.Object) cheatChainRunner.cheatConsole == (UnityEngine.Object) null)
      {
        cheatChainRunner.cheatConsole = UnityEngine.Object.FindObjectOfType<CheatConsole>();
        yield return (object) new WaitForSecondsRealtime(1f);
      }
      System.Action action;
      if (cheatChainRunner.cheatConsole.Cheats.TryGetValue(CheatChain[cheatChainRunner.i], out action))
      {
        Debug.Log((object) ("running Cheat " + CheatChain[cheatChainRunner.i]));
        action();
      }
      yield return (object) new WaitForSecondsRealtime(timer[cheatChainRunner.i]);
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) cheatChainRunner);
  }
}
