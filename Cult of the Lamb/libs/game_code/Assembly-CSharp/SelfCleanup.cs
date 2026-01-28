// Decompiled with JetBrains decompiler
// Type: SelfCleanup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class SelfCleanup : MonoBehaviour
{
  public void OnDestroy()
  {
    Debug.Log((object) ("Releasing gameobject Instance" + this.gameObject.name));
    Addressables.ReleaseInstance(this.gameObject);
  }
}
