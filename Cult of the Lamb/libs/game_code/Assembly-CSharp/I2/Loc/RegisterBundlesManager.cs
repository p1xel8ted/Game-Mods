// Decompiled with JetBrains decompiler
// Type: I2.Loc.RegisterBundlesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class RegisterBundlesManager : MonoBehaviour, IResourceManager_Bundles
{
  public void OnEnable()
  {
    if (ResourceManager.pInstance.mBundleManagers.Contains((IResourceManager_Bundles) this))
      return;
    ResourceManager.pInstance.mBundleManagers.Add((IResourceManager_Bundles) this);
  }

  public void OnDisable()
  {
    ResourceManager.pInstance.mBundleManagers.Remove((IResourceManager_Bundles) this);
  }

  public virtual UnityEngine.Object LoadFromBundle(string path, System.Type assetType)
  {
    return (UnityEngine.Object) null;
  }
}
