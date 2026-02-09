// Decompiled with JetBrains decompiler
// Type: SavedWorkersList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SavedWorkersList
{
  [SerializeField]
  public List<Worker> _workers = new List<Worker>();
  [SerializeField]
  public long last_unique_id;

  public Worker CreateNewWorker(WorldGameObject zombie_wgo, string worker_id, Item base_body)
  {
    ++this.last_unique_id;
    Worker newWorker = new Worker(zombie_wgo, this.last_unique_id);
    newWorker.id = worker_id;
    this._workers.Add(newWorker);
    zombie_wgo.data.inventory = base_body.inventory;
    return newWorker;
  }

  public Worker GetWorker(long worker_unique_id)
  {
    foreach (Worker worker in this._workers)
    {
      if (worker.worker_unique_id == worker_unique_id)
        return worker;
    }
    return (Worker) null;
  }
}
