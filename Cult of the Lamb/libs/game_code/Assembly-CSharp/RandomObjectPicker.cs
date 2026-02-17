// Decompiled with JetBrains decompiler
// Type: RandomObjectPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

  public void Start()
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
              if ((UnityEngine.Object) this == (UnityEngine.Object) null)
              {
                ObjectPool.Recycle<Transform>(obj.transform);
              }
              else
              {
                this.CreatedObject = obj.transform;
                this.CreatedObject.localPosition = Vector3.zero;
                this.CreatedObject.gameObject.SetActive(true);
                UnityAction objectCreated = this.ObjectCreated;
                if (objectCreated == null)
                  return;
                objectCreated();
              }
            }));
        }
        break;
      case RandomObjectPicker.SelectionMode.RandomSingle:
        if (this.Objects == null || this.Objects.Count <= 0)
          break;
        ObjectPool.Spawn(this.Objects[UnityEngine.Random.Range(0, this.Objects.Count)], this.transform.position, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
        {
          if ((UnityEngine.Object) this == (UnityEngine.Object) null)
          {
            ObjectPool.Recycle<Transform>(obj.transform);
          }
          else
          {
            this.CreatedObject = obj.transform;
            this.CreatedObject.localPosition = Vector3.zero;
            this.CreatedObject.gameObject.SetActive(true);
            UnityAction objectCreated = this.ObjectCreated;
            if (objectCreated == null)
              return;
            objectCreated();
          }
        }));
        break;
    }
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__6_0(GameObject obj)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
    {
      ObjectPool.Recycle<Transform>(obj.transform);
    }
    else
    {
      this.CreatedObject = obj.transform;
      this.CreatedObject.localPosition = Vector3.zero;
      this.CreatedObject.gameObject.SetActive(true);
      UnityAction objectCreated = this.ObjectCreated;
      if (objectCreated == null)
        return;
      objectCreated();
    }
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__6_1(GameObject obj)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
    {
      ObjectPool.Recycle<Transform>(obj.transform);
    }
    else
    {
      this.CreatedObject = obj.transform;
      this.CreatedObject.localPosition = Vector3.zero;
      this.CreatedObject.gameObject.SetActive(true);
      UnityAction objectCreated = this.ObjectCreated;
      if (objectCreated == null)
        return;
      objectCreated();
    }
  }

  public enum SelectionMode
  {
    RandomChance,
    RandomSingle,
  }
}
