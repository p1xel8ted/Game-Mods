// Decompiled with JetBrains decompiler
// Type: I2.Loc.RegisterBundlesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
