// Decompiled with JetBrains decompiler
// Type: ObjectPoolObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ObjectPoolObject
{
  public GameObject gameObject;
  public int AmountToPool;
  public ObjectPoolObject.PoolLocation PoolingLocation;

  public enum PoolLocation
  {
    None,
    Base,
    Dungeon,
    Both,
  }
}
