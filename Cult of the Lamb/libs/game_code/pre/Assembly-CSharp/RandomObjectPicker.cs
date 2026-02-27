// Decompiled with JetBrains decompiler
// Type: RandomObjectPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class RandomObjectPicker : BaseMonoBehaviour
{
  public List<string> Objects = new List<string>();
  public UnityAction ObjectCreated;
  public Transform CreatedObject;
  public RandomObjectPicker.SelectionMode selectionMode;
  [Range(0.0f, 100f)]
  public int chanceToEnable;

  private void Start()
  {
    switch (this.selectionMode)
    {
      case RandomObjectPicker.SelectionMode.RandomChance:
        if (this.Objects == null || this.Objects.Count <= 0)
          break;
        for (int index = 0; index < this.Objects.Count; ++index)
        {
          if (UnityEngine.Random.Range(0, 100) <= this.chanceToEnable)
            ObjectPool.Spawn(this.Objects[index], this.transform.position, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
            {
              this.CreatedObject = obj.transform;
              this.CreatedObject.localPosition = Vector3.zero;
              this.CreatedObject.gameObject.SetActive(true);
              UnityAction objectCreated = this.ObjectCreated;
              if (objectCreated == null)
                return;
              objectCreated();
            }));
        }
        break;
      case RandomObjectPicker.SelectionMode.RandomSingle:
        if (this.Objects == null || this.Objects.Count <= 0)
          break;
        ObjectPool.Spawn(this.Objects[UnityEngine.Random.Range(0, this.Objects.Count)], this.transform.position, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
        {
          this.CreatedObject = obj.transform;
          this.CreatedObject.localPosition = Vector3.zero;
          this.CreatedObject.gameObject.SetActive(true);
          UnityAction objectCreated = this.ObjectCreated;
          if (objectCreated == null)
            return;
          objectCreated();
        }));
        break;
    }
  }

  public enum SelectionMode
  {
    RandomChance,
    RandomSingle,
  }
}
