// Decompiled with JetBrains decompiler
// Type: TypeAndPlacementObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
