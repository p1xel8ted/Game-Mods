// Decompiled with JetBrains decompiler
// Type: SimpleRandomSpineSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;

#nullable disable
public class SimpleRandomSpineSkin : BaseMonoBehaviour
{
  public SkeletonAnimation SkeletonData;
  public List<SimpleRandomSpineSkin.SkinNames> Skins = new List<SimpleRandomSpineSkin.SkinNames>();

  private void Start()
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
