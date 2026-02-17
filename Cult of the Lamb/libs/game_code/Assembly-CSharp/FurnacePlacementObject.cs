// Decompiled with JetBrains decompiler
// Type: FurnacePlacementObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

#nullable disable
public class FurnacePlacementObject : PlacementObject
{
  public override void Start()
  {
    if (string.IsNullOrEmpty(this.ToBuildAsset))
      return;
    this.ToBuildAsset = $"Assets/{this.ToBuildAsset}.prefab";
    Addressables_wrapper.InstantiateAsync((object) this.ToBuildAsset, Vector3.zero, Quaternion.identity, this.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.ChildObject = obj.Result?.transform;
      foreach (MonoBehaviour componentsInChild in this.ChildObject.GetComponentsInChildren<MonoBehaviour>())
      {
        switch (componentsInChild)
        {
          case SkeletonRenderer _:
          case SpriteShapeController _:
          case Interaction_RacingGate _:
          case Interaction_FarmCropGrower _:
label_3:
            if (!DataManager.Instance.OnboardedSnowedUnder)
            {
              switch (componentsInChild)
              {
                case Interaction_DLCFurnace interactionDlcFurnace2:
                  interactionDlcFurnace2.HideVisuals();
                  continue;
                case Interaction_DLCProximityFurnace proximityFurnace2:
                  proximityFurnace2.HideVisuals();
                  continue;
                default:
                  continue;
              }
            }
            else
              continue;
          default:
            componentsInChild.enabled = false;
            goto label_3;
        }
      }
    }));
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__0_0(AsyncOperationHandle<GameObject> obj)
  {
    this.ChildObject = obj.Result?.transform;
    foreach (MonoBehaviour componentsInChild in this.ChildObject.GetComponentsInChildren<MonoBehaviour>())
    {
      switch (componentsInChild)
      {
        case SkeletonRenderer _:
        case SpriteShapeController _:
        case Interaction_RacingGate _:
        case Interaction_FarmCropGrower _:
label_3:
          if (!DataManager.Instance.OnboardedSnowedUnder)
          {
            switch (componentsInChild)
            {
              case Interaction_DLCFurnace interactionDlcFurnace:
                interactionDlcFurnace.HideVisuals();
                continue;
              case Interaction_DLCProximityFurnace proximityFurnace:
                proximityFurnace.HideVisuals();
                continue;
              default:
                continue;
            }
          }
          else
            continue;
        default:
          componentsInChild.enabled = false;
          goto label_3;
      }
    }
  }
}
