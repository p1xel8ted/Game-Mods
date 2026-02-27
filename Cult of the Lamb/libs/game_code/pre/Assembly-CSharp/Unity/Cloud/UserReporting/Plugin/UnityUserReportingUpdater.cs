// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.UnityUserReportingUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin;

public class UnityUserReportingUpdater : IEnumerator
{
  private int step;
  private WaitForEndOfFrame waitForEndOfFrame;

  public UnityUserReportingUpdater() => this.waitForEndOfFrame = new WaitForEndOfFrame();

  public object Current { get; private set; }

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
