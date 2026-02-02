// Decompiled with JetBrains decompiler
// Type: DLCFurnanceSequences
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.BuildMenu;
using MMTools;
using src.Extensions;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public static class DLCFurnanceSequences
{
  public const string FurnaceUpgradeToLevel02Sfx = "event:/dlc/building/furnace/upgrade_to_level_02_spatialized";
  public const string FurnaceUpgradeToLevel03Sfx = "event:/dlc/building/furnace/upgrade_to_level_03_spatialized";

  public static void PlayFurnaceUpgrade()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) DLCFurnanceSequences.PlayUpgradeSequenceIE());
  }

  public static IEnumerator PlayUpgradeSequenceIE()
  {
    GameManager.GetInstance().OnConversationNew();
    if (Interaction_DLCFurnace.Instance.Structure.Brain.Data.Type == StructureBrain.TYPES.FURNACE_2)
    {
      yield return (object) new WaitForSecondsRealtime(0.5f);
      StructuresData.CompleteResearch(StructureBrain.TYPES.PROXIMITY_FURNACE);
      StructuresData.SetRevealed(StructureBrain.TYPES.PROXIMITY_FURNACE);
      UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
      buildMenuController.Show(StructureBrain.TYPES.PROXIMITY_FURNACE);
      UIBuildMenuController buildMenuController1 = buildMenuController;
      buildMenuController1.OnHidden = buildMenuController1.OnHidden + (System.Action) (() => buildMenuController = (UIBuildMenuController) null);
      while ((UnityEngine.Object) buildMenuController != (UnityEngine.Object) null)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNew();
    }
    yield return (object) MMTransition.FadeIn(1f).YieldUntilCompleted();
    Vector3 fromPos = GameManager.GetInstance().CamFollowTarget.transform.position;
    Vector3 furnacePos = Interaction_DLCFurnace.Instance.transform.position;
    GameObject tempCameraTarget = new GameObject("TempFurnaceCameraTarget");
    tempCameraTarget.transform.position = furnacePos;
    BiomeBaseManager.Instance.ActivateRoom();
    GameManager.GetInstance().OnConversationNext(tempCameraTarget, 8f);
    GameManager.GetInstance().CamFollowTarget.targetPosition = furnacePos;
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -1.5f));
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    yield return (object) MMTransition.FadeOut();
    yield return (object) new WaitForSeconds(2f);
    StructureBrain brain = Interaction_DLCFurnace.Instance.Structure.Brain;
    StructureBrain.TYPES Type = brain.Data.Type == StructureBrain.TYPES.FURNACE_1 ? StructureBrain.TYPES.FURNACE_2 : StructureBrain.TYPES.FURNACE_3;
    string furnaceUpgradeSFX = Type == StructureBrain.TYPES.FURNACE_2 ? "event:/dlc/building/furnace/upgrade_to_level_02_spatialized" : "event:/dlc/building/furnace/upgrade_to_level_03_spatialized";
    StructureManager.RemoveStructure(brain);
    StructuresData newShrineData = StructuresData.GetInfoByType(Type, 0);
    newShrineData.Bounds = brain.Data.Bounds;
    newShrineData.GridTilePosition = brain.Data.GridTilePosition;
    newShrineData.PlacementRegionPosition = (Vector3Int) brain.Data.GridTilePosition;
    newShrineData.Fuel = brain.Data.Fuel;
    newShrineData.FullyFueled = brain.Data.FullyFueled;
    newShrineData.SoulCount = brain.Data.SoulCount;
    StructureManager.BuildStructure(FollowerLocation.Base, newShrineData, brain.Data.Position, new Vector2Int(2, 2), callback: (Action<GameObject>) (obj =>
    {
      BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
      PlacementRegion.Instance.structureBrain.AddStructureToGrid(newShrineData, true);
      AudioManager.Instance.PlayOneShot(furnaceUpgradeSFX);
      BiomeConstants.Instance.EmitDisplacementEffect(furnacePos);
      CameraManager.shakeCamera(10f, 0.0f);
      GameManager.GetInstance().CameraZoom(15f, 0.25f);
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    }));
    yield return (object) new WaitForSeconds(2f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.5f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    yield return (object) MMTransition.FadeIn(1f).YieldUntilCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) tempCameraTarget);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    BiomeBaseManager.Instance.ActivateDLCShrineRoom();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(fromPos);
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    yield return (object) MMTransition.FadeOut();
    GameManager.GetInstance().OnConversationEnd();
  }
}
