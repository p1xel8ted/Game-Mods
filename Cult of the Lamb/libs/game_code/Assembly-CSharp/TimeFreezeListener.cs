// Decompiled with JetBrains decompiler
// Type: TimeFreezeListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TimeFreezeListener : BaseMonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation spine;
  public bool timeFrozen;

  public event System.Action OnTimeFrozen;

  public event System.Action OnTimeUnfrozen;

  public void Start() => this.CheckFrozen();

  public void Update() => this.CheckFrozen();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void CheckFrozen()
  {
    if (this.timeFrozen == PlayerRelic.TimeFrozen)
      return;
    this.timeFrozen = PlayerRelic.TimeFrozen;
    this.FreezeTime(this.timeFrozen);
  }

  public void FreezeTime(bool freeze)
  {
    if ((UnityEngine.Object) this.spine != (UnityEngine.Object) null)
      this.spine.timeScale = freeze ? 0.0001f : 1f;
    System.Action action = freeze ? this.OnTimeFrozen : this.OnTimeUnfrozen;
    if (action == null)
      return;
    action();
  }
}
