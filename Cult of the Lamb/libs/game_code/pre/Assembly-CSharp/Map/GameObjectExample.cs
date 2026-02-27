// Decompiled with JetBrains decompiler
// Type: Map.GameObjectExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Malee;
using System;
using UnityEngine;

#nullable disable
namespace Map;

public class GameObjectExample : MonoBehaviour
{
  [Reorderable(paginate = true, pageSize = 2)]
  public GameObjectExample.GameObjectList list;

  private void Update()
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
