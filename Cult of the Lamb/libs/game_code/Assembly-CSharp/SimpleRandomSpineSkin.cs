// Decompiled with JetBrains decompiler
// Type: SimpleRandomSpineSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;

#nullable disable
public class SimpleRandomSpineSkin : BaseMonoBehaviour
{
  public SkeletonAnimation SkeletonData;
  public List<SimpleRandomSpineSkin.SkinNames> Skins = new List<SimpleRandomSpineSkin.SkinNames>();

  public void Start()
  {
    this.SkeletonData.skeleton.SetSkin(this.Skins[UnityEngine.Random.Range(0, this.Skins.Count)].Skin);
  }

  [Serializable]
  public class SkinNames
  {
    [SpineSkin("", "SkeletonData", true, false, false)]
    public string Skin;
  }
}
