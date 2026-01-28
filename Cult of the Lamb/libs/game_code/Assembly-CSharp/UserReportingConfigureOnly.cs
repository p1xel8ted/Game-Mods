// Decompiled with JetBrains decompiler
// Type: UserReportingConfigureOnly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
