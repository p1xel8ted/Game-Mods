// Decompiled with JetBrains decompiler
// Type: UserReportingConfigureOnly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

#nullable disable
public class UserReportingConfigureOnly : MonoBehaviour
{
  public void Start()
  {
    if (UnityUserReporting.CurrentClient != null)
      return;
    UnityUserReporting.Configure();
  }
}
