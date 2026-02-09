// Decompiled with JetBrains decompiler
// Type: UITableOrGrid
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UITableOrGrid : MonoBehaviour
{
  public bool _inited;
  public UITableOrGrid.Type _type;
  public UITable _table;
  public UIGrid _grid;
  public SimpleUITable _stable;

  public void CheckInit()
  {
    if (this._inited)
      return;
    this._inited = true;
    this._table = this.GetComponent<UITable>();
    this._grid = this.GetComponent<UIGrid>();
    this._stable = this.GetComponent<SimpleUITable>();
    if ((UnityEngine.Object) this._table != (UnityEngine.Object) null)
      this._type = UITableOrGrid.Type.Table;
    else if ((UnityEngine.Object) this._grid != (UnityEngine.Object) null)
      this._type = UITableOrGrid.Type.Grid;
    else if ((UnityEngine.Object) this._stable != (UnityEngine.Object) null)
      this._type = UITableOrGrid.Type.SimpleTable;
    else
      Debug.LogException(new Exception("No table/grid component found on object " + this.name), (UnityEngine.Object) this);
  }

  public void Reposition()
  {
    this.CheckInit();
    switch (this._type)
    {
      case UITableOrGrid.Type.Table:
        this._table.Reposition();
        this._table.repositionNow = true;
        break;
      case UITableOrGrid.Type.Grid:
        this._grid.Reposition();
        this._grid.repositionNow = true;
        break;
      case UITableOrGrid.Type.SimpleTable:
        this._stable.Reposition();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void DestroyChildren(Transform[] exceptions = null)
  {
    this.DestroyChildren<Transform>(exceptions);
  }

  public void DestroyChildren<T>(T[] exceptions = null) where T : Component
  {
    List<Transform> transformList = new List<Transform>();
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      Transform child = this.transform.GetChild(index);
      if (!((UnityEngine.Object) child.GetComponent<T>() == (UnityEngine.Object) null))
      {
        if (exceptions != null)
        {
          bool flag = false;
          foreach (T exception in exceptions)
          {
            if ((UnityEngine.Object) exception.transform == (UnityEngine.Object) child)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            continue;
        }
        transformList.Add(child);
      }
    }
    foreach (UnityEngine.Object @object in transformList)
      NGUITools.Destroy(@object);
  }

  public enum Type
  {
    Table,
    Grid,
    SimpleTable,
  }
}
