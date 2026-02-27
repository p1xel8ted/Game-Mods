// Decompiled with JetBrains decompiler
// Type: SimpleSpineTint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleSpineTint : BaseMonoBehaviour
{
  [SerializeField]
  private List<SimpleSpineTint.MyDictionaryEntry> ItemImages;

  private void Start()
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
