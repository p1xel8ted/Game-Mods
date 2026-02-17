// Decompiled with JetBrains decompiler
// Type: ChangeAnimationOnCollidePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class ChangeAnimationOnCollidePlayer : MonoBehaviour
{
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string NormalAnimationName = "idle-lost";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ChangeToAnimationName = "idle-found";
  public CritterBee CritterBee;

  public void OnEnable() => this.CritterBee = this.GetComponent<CritterBee>();

  public void Update()
  {
    bool flag = false;
    foreach (Component player in PlayerFarming.players)
    {
      if ((double) Vector3.Distance(player.transform.position, this.transform.position) < 1.5)
      {
        flag = true;
        break;
      }
    }
    if (flag)
    {
      if (!(this.CritterBee.Spine.AnimationName != this.ChangeToAnimationName))
        return;
      this.CritterBee.Spine.AnimationState.SetAnimation(0, this.ChangeToAnimationName, true);
    }
    else
    {
      if (!(this.CritterBee.Spine.AnimationName != this.NormalAnimationName))
        return;
      this.CritterBee.Spine.AnimationState.SetAnimation(0, this.NormalAnimationName, true);
    }
  }
}
