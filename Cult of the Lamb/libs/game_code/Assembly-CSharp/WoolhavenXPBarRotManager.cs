// Decompiled with JetBrains decompiler
// Type: WoolhavenXPBarRotManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WoolhavenXPBarRotManager : BaseMonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation skeleton;
  [SpineSkin("", "", true, false, false, dataField = "skeleton")]
  [SerializeField]
  public List<string> rotSkins = new List<string>();
  [SpineSkin("", "", true, false, false, dataField = "skeleton")]
  [SerializeField]
  public string beatenYngyaSkin;
  [CompilerGenerated]
  public static WoolhavenXPBarRotManager \u003CInstance\u003Ek__BackingField;

  public static WoolhavenXPBarRotManager Instance
  {
    get => WoolhavenXPBarRotManager.\u003CInstance\u003Ek__BackingField;
    set => WoolhavenXPBarRotManager.\u003CInstance\u003Ek__BackingField = value;
  }

  public void Awake() => WoolhavenXPBarRotManager.Instance = this;

  public void OnEnable() => this.SetCurrentXPBarRot();

  public void SetCurrentXPBarRot()
  {
    int winterServerity = DataManager.Instance.WinterServerity;
    if (DataManager.Instance.BeatenYngya)
      this.SetBeatenYngya();
    else
      this.SetRotLevel(winterServerity);
  }

  public void ClearRot() => this.SetRotLevel(0);

  public void SetBeatenYngya()
  {
    this.skeleton.skeleton.SetSkin(this.beatenYngyaSkin);
    this.skeleton.skeleton.SetSlotsToSetupPose();
  }

  public void SetRotLevel(int level)
  {
    if (level < 0)
      level = 0;
    if (level >= this.rotSkins.Count)
      level = this.rotSkins.Count - 1;
    this.skeleton.skeleton.SetSkin(this.rotSkins[level]);
    this.skeleton.skeleton.SetSlotsToSetupPose();
  }
}
