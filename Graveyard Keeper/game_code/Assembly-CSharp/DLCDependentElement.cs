// Decompiled with JetBrains decompiler
// Type: DLCDependentElement
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DLCDependentElement : MonoBehaviour
{
  [Header("Enable if")]
  [SerializeField]
  public List<DLCDependentElement.ConditionAtom> conditions;

  public void Start()
  {
    bool flag1 = false;
    bool flag2 = true;
    for (int index = 0; index < this.conditions.Count; ++index)
    {
      flag1 = true;
      flag2 &= DLCEngine.IsDLCAvailable(this.conditions[index].DLCVersion) == this.conditions[index].is_enable;
    }
    this.gameObject.SetActive(flag1 & flag2);
  }

  [Serializable]
  public class ConditionAtom
  {
    public DLCEngine.DLCVersion DLCVersion;
    public bool is_enable;
  }
}
