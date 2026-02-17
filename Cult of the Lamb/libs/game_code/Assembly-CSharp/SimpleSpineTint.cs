// Decompiled with JetBrains decompiler
// Type: SimpleSpineTint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
