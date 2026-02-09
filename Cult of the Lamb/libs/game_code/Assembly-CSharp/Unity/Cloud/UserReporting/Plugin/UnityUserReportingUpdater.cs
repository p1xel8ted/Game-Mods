// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.UnityUserReportingUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin;

public class UnityUserReportingUpdater : IEnumerator
{
  public int step;
  public WaitForEndOfFrame waitForEndOfFrame;
  [CompilerGenerated]
  public object \u003CCurrent\u003Ek__BackingField;

  public UnityUserReportingUpdater() => this.waitForEndOfFrame = new WaitForEndOfFrame();

  public object Current
  {
    get => this.\u003CCurrent\u003Ek__BackingField;
    set => this.\u003CCurrent\u003Ek__BackingField = value;
  }

  public bool MoveNext()
  {
    if (this.step == 0)
    {
      UnityUserReporting.CurrentClient.Update();
      this.Current = (object) null;
      this.step = 1;
      return true;
    }
    if (this.step == 1)
    {
      this.Current = (object) this.waitForEndOfFrame;
      this.step = 2;
      return true;
    }
    if (this.step != 2)
      return false;
    UnityUserReporting.CurrentClient.UpdateOnEndOfFrame();
    this.Current = (object) null;
    this.step = 3;
    return false;
  }

  public void Reset() => this.step = 0;
}
