// Decompiled with JetBrains decompiler
// Type: TypeAndPlacementObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
[Serializable]
public class TypeAndPlacementObject
{
  public StructureBrain.TYPES Type;
  [HideInInspector]
  public StructureBrain.Categories Category;
  public GameObject _placementObject;
  public AssetReferenceGameObject Addr_PlacementObject;
  public AsyncOperationHandle<GameObject> AddrHandle;
  public Sprite IconImage;
  [HideInInspector]
  public TypeAndPlacementObjects.Tier Tier;

  public GameObject PlacementObject
  {
    get
    {
      if ((UnityEngine.Object) this._placementObject == (UnityEngine.Object) null)
      {
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.Addr_PlacementObject);
        asyncOperationHandle.WaitForCompletion();
        this._placementObject = asyncOperationHandle.Result;
        this.AddrHandle = asyncOperationHandle;
      }
      return this._placementObject;
    }
    set => this._placementObject = value;
  }
}
