// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathModifier
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding;

[Serializable]
public abstract class PathModifier : IPathModifier
{
  [NonSerialized]
  public Seeker seeker;

  public abstract int Order { get; }

  public void Awake(Seeker s)
  {
    this.seeker = s;
    if (!((UnityEngine.Object) s != (UnityEngine.Object) null))
      return;
    s.RegisterModifier((IPathModifier) this);
  }

  public void OnDestroy(Seeker s)
  {
    if (!((UnityEngine.Object) s != (UnityEngine.Object) null))
      return;
    s.DeregisterModifier((IPathModifier) this);
  }

  public virtual void PreProcess(Path p)
  {
  }

  public abstract void Apply(Path p);
}
