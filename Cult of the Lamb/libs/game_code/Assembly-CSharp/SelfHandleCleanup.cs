// Decompiled with JetBrains decompiler
// Type: SelfHandleCleanup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
