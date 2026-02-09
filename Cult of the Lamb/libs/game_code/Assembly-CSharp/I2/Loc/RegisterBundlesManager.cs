// Decompiled with JetBrains decompiler
// Type: I2.Loc.RegisterBundlesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
