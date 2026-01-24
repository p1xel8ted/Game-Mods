// Decompiled with JetBrains decompiler
// Type: SummonGhostLambNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class SummonGhostLambNPC : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject FXPosition;
  [SerializeField]
  public GhostNPC ghostNPC;
  [SerializeField]
  public Transform ghostSpawnPoint;
  [SerializeField]
  public GameObject CameraTarget;
  [SerializeField]
  public float cameraDistance = 4f;
  [SerializeField]
  public Interaction_Generic graveGhostInteraction;
  [SerializeField]
  public Interaction_PurchasableFleece fleeceInteraction;

  public void OnEnable()
  {
    this.UpdateInteractions();
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().WaitForSeconds(1f, new System.Action(this.UpdateInteractions));
  }

  public void UpdateInteractions() => this.fleeceInteraction.UpdateInteractions();

  public void RevealNPC(System.Action summonedCallback)
  {
    this.StartCoroutine((IEnumerator) this.RevealNPCRoutine(summonedCallback));
  }

  public IEnumerator RevealNPCRoutine(System.Action summonedCallback)
  {
    this.ghostNPC.IsRevealed = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.CameraTarget, this.cameraDistance);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(1.3f, 1.4f, 1.5f);
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.CameraTarget, 7f);
    BiomeConstants.Instance.EmitGroundSmashVFXParticles(this.FXPosition.transform.position + Vector3.back * 0.05f);
    AudioManager.Instance.PlayOneShot("event:/followers/break_free");
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuryLostGhost);
    if ((UnityEngine.Object) this.ghostNPC != (UnityEngine.Object) null)
    {
      this.ghostNPC.gameObject.SetActive(true);
      yield return (object) this.ghostNPC.Summon(this.ghostSpawnPoint.position);
      System.Action action = summonedCallback;
      if (action != null)
        action();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.ghostNPC.gameObject);
      this.ghostNPC.ReturnHome();
      yield return (object) new WaitForSeconds(2f);
      if (this.ghostNPC.IsRancher)
      {
        yield return (object) new WaitForSeconds(2f);
        this.ghostNPC.BrokenShopInteraction.Play();
        if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) <= 0)
          BaseGoopDoor.WoolhavenDoor.CheckWoolhavenDoor();
      }
      else
        GameManager.GetInstance().OnConversationEnd();
      this.UpdateInteractions();
    }
  }
}
