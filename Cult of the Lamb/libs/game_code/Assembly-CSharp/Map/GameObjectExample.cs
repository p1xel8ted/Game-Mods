// Decompiled with JetBrains decompiler
// Type: Map.GameObjectExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Malee;
using System;
using UnityEngine;

#nullable disable
namespace Map;

public class GameObjectExample : MonoBehaviour
{
  [Reorderable(paginate = true, pageSize = 2)]
  public GameObjectExample.GameObjectList list;

  public void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Space))
      return;
    this.list.Add(this.gameObject);
  }

  [Serializable]
  public class GameObjectList : ReorderableArray<GameObject>
  {
  }
}
