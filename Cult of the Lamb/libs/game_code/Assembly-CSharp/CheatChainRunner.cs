// Decompiled with JetBrains decompiler
// Type: CheatChainRunner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using Unify.Input;
using UnityEngine;

#nullable disable
public class CheatChainRunner : MonoBehaviour
{
  public CheatConsole cheatConsole;
  public int i;

  public void RunChain(string[] CheatChain, float[] timer)
  {
    this.cheatConsole = UnityEngine.Object.FindObjectOfType<CheatConsole>();
    this.StartCoroutine((IEnumerator) this.ChainRun(CheatChain, timer));
  }

  public void RunChainForEver(string[] CheatChain, float timer)
  {
    this.cheatConsole = UnityEngine.Object.FindObjectOfType<CheatConsole>();
    this.StartCoroutine((IEnumerator) this.ChainRun(CheatChain, timer));
  }

  public IEnumerator ChainRun(string[] CheatChain, float[] timer)
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

  public IEnumerator ChainRun(string[] CheatChain, float timer)
  {
    for (this.i = 0; this.i <= CheatChain.Length; ++this.i)
    {
      if (this.i == CheatChain.Length)
        this.i = 0;
      while ((UnityEngine.Object) this.cheatConsole == (UnityEngine.Object) null)
      {
        this.cheatConsole = UnityEngine.Object.FindObjectOfType<CheatConsole>();
        yield return (object) new WaitForSecondsRealtime(1f);
      }
      System.Action action;
      if (this.cheatConsole.Cheats.TryGetValue(CheatChain[this.i], out action))
      {
        Debug.Log((object) ("running Cheat " + CheatChain[this.i]));
        action();
      }
      yield return (object) new WaitForSecondsRealtime(timer);
    }
  }

  public void Update()
  {
    if (!RewiredInputManager.MainPlayer.GetButtonDown("Bleat"))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this);
  }
}
