// Decompiled with JetBrains decompiler
// Type: SimpleSpineTint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleSpineTint : BaseMonoBehaviour
{
  [SerializeField]
  public List<SimpleSpineTint.MyDictionaryEntry> ItemImages;

  public void Start()
  {
    SkeletonAnimation component = this.GetComponent<SkeletonAnimation>();
    foreach (SimpleSpineTint.MyDictionaryEntry itemImage in this.ItemImages)
      component.skeleton.FindSlot(itemImage.Attachment).SetColor(itemImage.color);
  }

  [Serializable]
  public class MyDictionaryEntry
  {
    public string Attachment;
    public Color color = Color.white;
  }
}
