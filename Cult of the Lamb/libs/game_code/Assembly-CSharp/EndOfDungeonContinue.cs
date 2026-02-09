// Decompiled with JetBrains decompiler
// Type: EndOfDungeonContinue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public GameObject door;
  [SerializeField]
  public GameObject chest;
  [SerializeField]
  public GameObject teleporter;
  [SerializeField]
  public List<GameObject> DoorObjectsToHide = new List<GameObject>();
  [SerializeField]
  public ParticleSystemRenderer doorParticle;
  [SerializeField]
  public Material oldMaterial;
  [SerializeField]
  public Material newMaterial;
  [SerializeField]
  public SkeletonRendererCustomMaterials[] torchCustomMaterials;

  public void Awake()
  {
    if (DungeonSandboxManager.Active)
    {
      this.enabled = false;
      if ((Object) MapManager.Instance != (Object) null && MapManager.Instance.CurrentMap.GetFinalBossNode() == MapManager.Instance.CurrentNode)
      {
        this.teleporter.SetActive(true);
        this.chest.gameObject.SetActive(false);
        this.HideDoor();
      }
      else
        this.teleporter.SetActive(false);
    }
    else
    {
      if ((DataManager.Instance.DungeonCompleted(BiomeGenerator.Instance.DungeonLocation, GameManager.Layer2) ? 1 : (DungeonSandboxManager.Active ? 1 : 0)) != 0 && (Object) MapManager.Instance != (Object) null && MapManager.Instance.CurrentNode != null && MapManager.Instance.CurrentNode == MapManager.Instance.CurrentMap.GetFinalBossNode())
      {
        this.chest.SetActive(false);
        this.teleporter.SetActive(true);
        if (GameManager.DungeonEndlessLevel >= 3 || DungeonSandboxManager.Active)
          this.HideDoor();
      }
      else
      {
        this.chest.SetActive(true);
        this.teleporter.SetActive(false);
      }
      if (!((Object) this.doorParticle != (Object) null))
        return;
      this.doorParticle.material = this.oldMaterial;
    }
  }

  public void HideDoor()
  {
    Debug.Log((object) ("GameManager.DungeonEndlessLevel:" + GameManager.DungeonEndlessLevel.ToString()));
    Debug.Log((object) ("BiomeGenerator.MAX_ENDLESS_LEVELS: " + 3.ToString()));
    foreach (GameObject gameObject in this.DoorObjectsToHide)
    {
      if ((Object) gameObject != (Object) null)
        gameObject.SetActive(false);
    }
  }

  public void OnEnable() => this.StartCoroutine((IEnumerator) this.MovePlayerToMiddle());

  public void SetEverythingGreen()
  {
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    int num1 = 4;
    int num2 = 0;
    foreach (SkeletonRendererCustomMaterials torchCustomMaterial in this.torchCustomMaterials)
    {
      if ((Object) torchCustomMaterial != (Object) null)
      {
        ++num2;
        if (num2 < num1)
          AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", torchCustomMaterial.gameObject);
        torchCustomMaterial.enabled = true;
      }
    }
    if (!((Object) this.doorParticle != (Object) null))
      return;
    this.doorParticle.material = this.newMaterial;
  }

  public IEnumerator MovePlayerToMiddle()
  {
    EndOfDungeonContinue ofDungeonContinue = this;
    yield return (object) new WaitForEndOfFrame();
    if (!((Object) BiomeGenerator.Instance == (Object) null) && BiomeGenerator.Instance.CurrentRoom != null && !((Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (Object) ofDungeonContinue.GetComponentInParent<GenerateRoom>()) && !((Object) MapManager.Instance == (Object) null) && MapManager.Instance.CurrentNode != null && MapManager.Instance.CurrentNode.nodeType == Map.NodeType.MiniBossFloor)
    {
      bool shownTutorial = !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.ContinueAdventureMap);
      if (shownTutorial)
      {
        ofDungeonContinue.SetEverythingGreen();
      }
      else
      {
        GameManager.GetInstance().OnConversationNew((PlayerFarming) null);
        GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
        float t = 0.0f;
        Vector3 endPosition = new Vector3(-0.5f, 2.5f, 0.0f);
        while ((double) (t += Time.deltaTime) < 1.7999999523162842)
        {
          PlayerFarming.Instance.GoToAndStop(endPosition, groupAction: true);
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
        foreach (SkeletonRendererCustomMaterials torchCustomMaterial in ofDungeonContinue.torchCustomMaterials)
        {
          if ((bool) (Object) torchCustomMaterial)
            torchCustomMaterial.enabled = true;
        }
        Material m = ofDungeonContinue.doorParticle.material;
        float time = 0.0f;
        while ((double) (time += Time.deltaTime) < 1.0)
        {
          if ((Object) ofDungeonContinue.doorParticle != (Object) null)
            ofDungeonContinue.doorParticle.material.Lerp(m, ofDungeonContinue.newMaterial, time / 1f);
          yield return (object) null;
        }
        yield return (object) new WaitForSeconds(1.5f);
        GameManager.GetInstance().OnConversationEnd();
      }
    }
  }
}
