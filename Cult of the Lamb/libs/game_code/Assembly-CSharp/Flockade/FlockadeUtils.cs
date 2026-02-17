// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Flockade;

public static class FlockadeUtils
{
  public const float STEP = 0.0166666675f;

  public static Sequence Combine(IEnumerable<Tween> tweens)
  {
    Sequence s = DOTween.Sequence();
    foreach (Tween t in (IEnumerable<Tween>) ((object) tweens ?? (object) Array.Empty<Tween>()))
    {
      if (t != null)
        s.Join(t);
    }
    return s;
  }

  public static int Modulo(int value, int modulo) => (value % modulo + modulo) % modulo;

  public static int NextMultipleOf(int multiple, int from)
  {
    return (from + multiple - 1) / multiple * multiple;
  }

  public static int Rank(int index, int origin, int length, bool forward = true)
  {
    return ((forward ? 1 : -1) * (index - origin) + length) % length;
  }

  public static void RunCoroutineSynchronously(IEnumerator coroutine)
  {
    Stack<IEnumerator> enumeratorStack = new Stack<IEnumerator>();
    enumeratorStack.Push(coroutine);
    while (enumeratorStack.Count > 0)
    {
      IEnumerator enumerator = enumeratorStack.Peek();
      if (!enumerator.MoveNext())
        enumeratorStack.Pop();
      else if (enumerator.Current is IEnumerator current && !(current is CustomYieldInstruction))
        enumeratorStack.Push(current);
    }
  }

  public static IEnumerator WaitForCompletion(IEnumerable<Tween> tweens)
  {
    foreach (Tween tween in tweens)
    {
      if (tween != null && tween.IsActive() && !tween.IsComplete())
        yield return (object) tween.WaitForCompletion();
    }
  }
}
