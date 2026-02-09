// Decompiled with JetBrains decompiler
// Type: DropCollectGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DropCollectGUI : MonoBehaviour
{
  public DropCollectItem item_prefab;
  public static DropCollectGUI _me;
  public Transform root_for_items;
  public UIGrid grid;
  public List<DropCollectItem> _items = new List<DropCollectItem>();

  public void Start()
  {
    DropCollectGUI._me = this;
    if ((Object) this.item_prefab == (Object) null)
      Debug.LogError((object) "Drop item_prefab is null");
    else
      this.item_prefab.gameObject.SetActive(false);
  }

  public static void OnMoneyCollected(float money)
  {
    DropCollectGUI.OnDropCollected(new Item(nameof (money), Mathf.FloorToInt(money * 100f)));
  }

  public static void OnDropCollected(Item item)
  {
    if ((Object) DropCollectGUI._me == (Object) null || item.definition != null && item.definition.item_size > 1)
      return;
    foreach (DropCollectItem dropCollectItem in DropCollectGUI._me._items)
    {
      if (dropCollectItem.item_id == item.id)
      {
        dropCollectItem.AddMoreItems(item.value);
        return;
      }
    }
    DropCollectItem dropCollectItem1 = DropCollectGUI._me.item_prefab.Copy<DropCollectItem>(DropCollectGUI._me.root_for_items);
    Vector3 position = dropCollectItem1.transform.position with
    {
      y = -1f
    };
    dropCollectItem1.transform.position = position;
    dropCollectItem1.Draw(item);
    DropCollectGUI._me._items.Add(dropCollectItem1);
  }

  public static void RedrawGrid(Transform t = null)
  {
    if ((Object) DropCollectGUI._me == (Object) null)
      return;
    DropCollectGUI._me.grid.AddChild(t);
  }

  public static void Despawn(DropCollectItem i)
  {
    if ((Object) DropCollectGUI._me == (Object) null)
      return;
    DropCollectGUI._me._items.Remove(i);
    DropCollectGUI._me.grid.RemoveChild(i.transform);
    UIWidget w = i.gameObject.GetComponent<UIWidget>();
    DOTween.To((DOGetter<float>) (() => w.alpha), (DOSetter<float>) (x => w.alpha = x), 0.0f, 0.2f);
    i.gameObject.transform.DOMoveY(-1f, 0.2f).OnComplete<Tweener>((TweenCallback) (() => Object.Destroy((Object) i.gameObject)));
  }
}
