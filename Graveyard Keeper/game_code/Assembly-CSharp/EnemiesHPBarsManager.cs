// Decompiled with JetBrains decompiler
// Type: EnemiesHPBarsManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemiesHPBarsManager : MonoBehaviour
{
  public static EnemiesHPBarsManager me;
  public OverheadGUI enemy_bar_prefab;
  public List<OverheadGUI> _bars = new List<OverheadGUI>();
  public bool _initialized;

  public void Awake()
  {
    if (this._initialized)
      return;
    EnemiesHPBarsManager.me = this;
    this.enemy_bar_prefab.Deactivate<OverheadGUI>();
    this._initialized = true;
  }

  public void LateUpdate()
  {
    for (int index = 0; index < this._bars.Count; ++index)
    {
      if (this._bars[index].IsNotNeededAnymore())
      {
        this._bars[index].DestroyGO<OverheadGUI>();
        this._bars.RemoveAt(index);
        int num = index - 1;
        break;
      }
      this._bars[index].UpdateForLinkedObj();
    }
  }

  public void AddIfNeeded(WorldGameObject obj)
  {
    if ((Object) obj == (Object) null || (double) obj.hp < 0.0)
      return;
    long uniqueId = obj.unique_id;
    foreach (OverheadGUI bar in this._bars)
    {
      if (bar.linked_obj_id == uniqueId)
        return;
    }
    OverheadGUI overheadGui = this.enemy_bar_prefab.Copy<OverheadGUI>(name: "hp bar " + obj.obj_id);
    overheadGui.LinkToObj(obj);
    this._bars.Add(overheadGui);
  }
}
