// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.GlobalBlackboard
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[ExecuteInEditMode]
public class GlobalBlackboard : Blackboard
{
  public static List<GlobalBlackboard> allGlobals = new List<GlobalBlackboard>();
  public bool dontDestroy = true;

  public new string name
  {
    get => base.name;
    set
    {
      if (!(base.name != value))
        return;
      base.name = value;
      if (this.IsUnique())
        return;
      Debug.LogError((object) "Another Blackboard has the same name. Please rename either.", (UnityEngine.Object) this.gameObject);
    }
  }

  public static GlobalBlackboard Create()
  {
    GlobalBlackboard globalBlackboard = new GameObject("@GlobalBlackboard").AddComponent<GlobalBlackboard>();
    globalBlackboard.name = "Global";
    return globalBlackboard;
  }

  public static GlobalBlackboard Find(string name)
  {
    if (!Application.isPlaying)
      return ((IEnumerable<GlobalBlackboard>) UnityEngine.Object.FindObjectsOfType<GlobalBlackboard>()).Where<GlobalBlackboard>((Func<GlobalBlackboard, bool>) (b => b.name == name)).FirstOrDefault<GlobalBlackboard>();
    GlobalBlackboard globalBlackboard = GlobalBlackboard.allGlobals.Find((Predicate<GlobalBlackboard>) (b => b.name == name));
    if ((UnityEngine.Object) globalBlackboard == (UnityEngine.Object) null)
    {
      globalBlackboard = ((IEnumerable<GlobalBlackboard>) UnityEngine.Object.FindObjectsOfType<GlobalBlackboard>()).Where<GlobalBlackboard>((Func<GlobalBlackboard, bool>) (b => b.name == name)).FirstOrDefault<GlobalBlackboard>();
      if ((UnityEngine.Object) globalBlackboard != (UnityEngine.Object) null)
        GlobalBlackboard.allGlobals.Add(globalBlackboard);
    }
    return globalBlackboard;
  }

  public override void Awake()
  {
    base.Awake();
    if (!GlobalBlackboard.allGlobals.Contains(this))
      GlobalBlackboard.allGlobals.Add(this);
    if (Application.isPlaying)
    {
      if (this.IsUnique())
      {
        if (this.dontDestroy)
          UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
      }
      else
      {
        Debug.Log((object) $"There exist more than one Global Blackboards with same name '{this.name}'. The old one will be destroyed and replaced with the new one.");
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
    }
    if (Application.isPlaying || this.IsUnique())
      return;
    Debug.LogError((object) $"There is a duplicate <b>GlobalBlackboard</b> named '{this.name}' in the scene. Please rename it.", (UnityEngine.Object) this);
  }

  public void OnDestroy() => GlobalBlackboard.allGlobals.Remove(this);

  public bool IsUnique()
  {
    return (UnityEngine.Object) GlobalBlackboard.allGlobals.Find((Predicate<GlobalBlackboard>) (b => b.name == this.name && (UnityEngine.Object) b != (UnityEngine.Object) this)) == (UnityEngine.Object) null;
  }

  [CompilerGenerated]
  public bool \u003CIsUnique\u003Eb__9_0(GlobalBlackboard b)
  {
    return b.name == this.name && (UnityEngine.Object) b != (UnityEngine.Object) this;
  }
}
