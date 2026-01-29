// Decompiled with JetBrains decompiler
// Type: SelfHandleCleanup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class SelfHandleCleanup : MonoBehaviour
{
  public AsyncOperationHandle<GameObject> Handle;

  public void OnDestroy()
  {
    Debug.Log((object) ("Releasing gameobject Handle" + this.gameObject.name));
    if (!this.Handle.IsValid())
      return;
    Addressables.Release<GameObject>(this.Handle);
  }
}
