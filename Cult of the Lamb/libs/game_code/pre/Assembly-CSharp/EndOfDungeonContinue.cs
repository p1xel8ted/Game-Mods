// Decompiled with JetBrains decompiler
// Type: EndOfDungeonContinue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using Map;
using MMBiomeGeneration;
using MMRoomGeneration;
using Spine.Unity;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EndOfDungeonContinue : MonoBehaviour
{
  [SerializeField]
  private GameObject door;
  [SerializeField]
  private GameObject chest;
  [SerializeField]
  private GameObject teleporter;
  [SerializeField]
  private List<GameObject> DoorObjectsToHide = new List<GameObject>();
  [SerializeField]
  private ParticleSystemRenderer doorParticle;
  [SerializeField]
  private Material oldMaterial;
  [SerializeField]
  private Material newMaterial;
  [SerializeField]
  private SkeletonRendererCustomMaterials[] torchCustomMaterials;

  private void Awake()
  {
    Debug.Log((object) ("GameManager.DungeonEndlessLevel: " + (object) GameManager.DungeonEndlessLevel));
    if (DataManager.Instance.DungeonCompleted(BiomeGenerator.Instance.DungeonLocation) && (Object) MapManager.Instance != (Object) null && MapManager.Instance.CurrentNode != null && MapManager.Instance.CurrentNode.nodeType == NodeType.MiniBossFloor)
    {
      this.chest.SetActive(false);
      this.teleporter.SetActive(true);
      if (GameManager.DungeonEndlessLevel >= 3)
        this.HideDoor();
    }
    else
    {
      this.chest.SetActive(true);
      this.teleporter.SetActive(false);
    }
    this.doorParticle.material = this.oldMaterial;
  }

  private void HideDoor()
  {
    Debug.Log((object) ("GameManager.DungeonEndlessLevel:" + (object) GameManager.DungeonEndlessLevel));
    Debug.Log((object) ("BiomeGenerator.MAX_ENDLESS_LEVELS: " + (object) 3));
    foreach (GameObject gameObject in this.DoorObjectsToHide)
      gameObject.SetActive(false);
  }

  private void OnEnable() => this.StartCoroutine((IEnumerator) this.MovePlayerToMiddle());

  private void SetEverythingGreen()
  {
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    foreach (SkeletonRendererCustomMaterials torchCustomMaterial in this.torchCustomMaterials)
    {
      AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", torchCustomMaterial.gameObject);
      torchCustomMaterial.enabled = true;
    }
    this.doorParticle.material = this.newMaterial;
  }

  private IEnumerator MovePlayerToMiddle()
  {
    EndOfDungeonContinue ofDungeonContinue = this;
    yield return (object) new WaitForEndOfFrame();
    if (!((Object) BiomeGenerator.Instance == (Object) null) && BiomeGenerator.Instance.CurrentRoom != null && !((Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (Object) ofDungeonContinue.GetComponentInParent<GenerateRoom>()) && !((Object) MapManager.Instance == (Object) null) && MapManager.Instance.CurrentNode != null && MapManager.Instance.CurrentNode.nodeType == NodeType.MiniBossFloor)
    {
      bool shownTutorial = !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.ContinueAdventureMap);
      if (shownTutorial)
      {
        ofDungeonContinue.SetEverythingGreen();
      }
      else
      {
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
        float t = 0.0f;
        while ((double) (t += Time.deltaTime) < 1.7999999523162842)
        {
          PlayerFarming.Instance.GoToAndStop(new Vector3(-0.5f, 2.5f, 0.0f));
          yield return (object) null;
        }
        UITutorialOverlayController tutorialOverlay = (UITutorialOverlayController) null;
        if (!shownTutorial)
          tutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.ContinueAdventureMap);
        while ((Object) tutorialOverlay != (Object) null)
          yield return (object) null;
        yield return (object) new WaitForEndOfFrame();
        while (UIMenuBase.ActiveMenus.Count > 0)
          yield return (object) null;
        GameManager.GetInstance().OnConversationNext(ofDungeonContinue.door);
        yield return (object) new WaitForSeconds(1.5f);
        foreach (Behaviour torchCustomMaterial in ofDungeonContinue.torchCustomMaterials)
          torchCustomMaterial.enabled = true;
        Material m = ofDungeonContinue.doorParticle.material;
        float time = 0.0f;
        while ((double) (time += Time.deltaTime) < 1.0)
        {
          ofDungeonContinue.doorParticle.material.Lerp(m, ofDungeonContinue.newMaterial, time / 1f);
          yield return (object) null;
        }
        yield return (object) new WaitForSeconds(1.5f);
        GameManager.GetInstance().OnConversationEnd();
      }
    }
  }
}
