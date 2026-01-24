// Decompiled with JetBrains decompiler
// Type: Interaction_MarkOfYngya
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_MarkOfYngya : Interaction
{
  public List<GameObject> LostSouls = new List<GameObject>();
  [SerializeField]
  public SpriteRenderer YngyaShrine;
  [SerializeField]
  public GameObject YngyaShrineEyes;
  [SerializeField]
  public ParticleSystem EyeParticles;
  [SerializeField]
  public SpriteRenderer YngyaSilhouette;
  [SerializeField]
  public SpriteRenderer YngyaSilhouette2;
  [SerializeField]
  public ParticleSystem YngyaParticles;
  [SerializeField]
  public ParticleSystem CircleBurst;
  [SerializeField]
  public Interaction_TeleportHome Teleporter;
  [SerializeField]
  public SimpleSetCamera SimpleSetCamera;
  public bool DisabledTeleporter;
  public int SoulCount;

  public override void OnEnable()
  {
    base.OnEnable();
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.YngyaShrine);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 1.5f;
    if (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Witness || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Witness)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.YngyaSilhouette.enabled = false;
      if (this.DisabledTeleporter)
        return;
      this.Teleporter.DisableTeleporter();
      this.DisabledTeleporter = true;
    }
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = this.Interactable ? LocalizationManager.GetTranslation("Interactions/YngyasMark") : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.ActivateYngyaMarkRoutine());
    PlayerReturnToBase.Disabled = true;
  }

  public IEnumerator ActivateYngyaMarkRoutine()
  {
    Interaction_MarkOfYngya interactionMarkOfYngya = this;
    interactionMarkOfYngya.Interactable = false;
    interactionMarkOfYngya.state.transform.DOMove(interactionMarkOfYngya.transform.position + new Vector3(0.0f, -0.2f, 0.0f), 0.5f);
    GameManager.GetInstance().OnConversationNew();
    SimpleBarkRepeating.CloseAllBarks(true);
    interactionMarkOfYngya.SimpleSetCamera.Play();
    if (DataManager.Instance.TotalShrineGhostJuice <= 0)
      AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/ghost_callhome_norotspread", interactionMarkOfYngya.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/ghost_callhome", interactionMarkOfYngya.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/music/yngya_shrine/ghost_callhome");
    interactionMarkOfYngya.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    if ((bool) (UnityEngine.Object) interactionMarkOfYngya.YngyaShrineEyes)
      interactionMarkOfYngya.YngyaShrineEyes.SetActive(true);
    if ((bool) (UnityEngine.Object) interactionMarkOfYngya.EyeParticles)
      interactionMarkOfYngya.EyeParticles.Play();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("collect-ghosts", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("collect-ghosts-loop", 0, true, 0.0f);
    interactionMarkOfYngya.StartCoroutine((IEnumerator) interactionMarkOfYngya.ShakeCameraWithRampUp(0.7f));
    yield return (object) new WaitForSeconds(0.2f);
    if ((UnityEngine.Object) interactionMarkOfYngya.YngyaSilhouette != (UnityEngine.Object) null)
    {
      interactionMarkOfYngya.YngyaSilhouette.transform.position = interactionMarkOfYngya.playerFarming.CameraBone.transform.position - new Vector3(0.0f, 0.0f, 3f);
      interactionMarkOfYngya.YngyaSilhouette.enabled = true;
      interactionMarkOfYngya.YngyaSilhouette.DOKill();
      interactionMarkOfYngya.YngyaSilhouette.transform.localScale = Vector3.zero;
      interactionMarkOfYngya.YngyaSilhouette.material.SetFloat("_MaxAlpha", 0.6f);
      interactionMarkOfYngya.YngyaSilhouette.transform.DOScale(0.5f, 0.5f);
    }
    if ((UnityEngine.Object) interactionMarkOfYngya.YngyaParticles != (UnityEngine.Object) null)
    {
      interactionMarkOfYngya.YngyaParticles.transform.position = interactionMarkOfYngya.playerFarming.CameraBone.transform.position - new Vector3(0.0f, 0.0f, 3f);
      interactionMarkOfYngya.YngyaParticles.Play();
    }
    if ((UnityEngine.Object) interactionMarkOfYngya.CircleBurst != (UnityEngine.Object) null)
    {
      interactionMarkOfYngya.CircleBurst.transform.position = interactionMarkOfYngya.playerFarming.CameraBone.transform.position - new Vector3(0.0f, 0.0f, 0.0f);
      interactionMarkOfYngya.CircleBurst.Play();
    }
    if ((UnityEngine.Object) interactionMarkOfYngya.YngyaSilhouette2 != (UnityEngine.Object) null)
    {
      interactionMarkOfYngya.YngyaSilhouette2.enabled = true;
      interactionMarkOfYngya.YngyaSilhouette.material.DOFloat(0.0f, "_MaxAlpha", 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
      yield return (object) new WaitForSeconds(0.35f);
      interactionMarkOfYngya.YngyaSilhouette.DOFade(0.7f, 1.5f);
      interactionMarkOfYngya.YngyaSilhouette2.DOFade(0.7f, 1.2f);
      yield return (object) new WaitForSeconds(0.45f);
    }
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 2f);
    foreach (GameObject lostSoul in interactionMarkOfYngya.LostSouls)
      lostSoul.GetComponent<LostSoulBark>().ActivateGhost();
    yield return (object) new WaitForSeconds(1f);
    if ((UnityEngine.Object) interactionMarkOfYngya.YngyaSilhouette != (UnityEngine.Object) null)
      interactionMarkOfYngya.YngyaSilhouette.enabled = false;
    if ((UnityEngine.Object) interactionMarkOfYngya.YngyaSilhouette2 != (UnityEngine.Object) null)
      interactionMarkOfYngya.YngyaSilhouette2.enabled = false;
    foreach (GameObject lostSoul in interactionMarkOfYngya.LostSouls)
    {
      lostSoul.SetActive(false);
      BiomeConstants.Instance.EmitHitVFXSoul(lostSoul.transform.position, Quaternion.identity);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/ghost_fly", lostSoul.transform.position);
      SoulCustomTarget.Create(interactionMarkOfYngya.playerFarming.CameraBone.gameObject, lostSoul.transform.position, Color.white, new System.Action(interactionMarkOfYngya.ReceiveSoul), 0.6f, sfxPath: " ", playDefaultSFX: false);
      yield return (object) new WaitForSeconds(0.1f);
    }
    while (interactionMarkOfYngya.SoulCount < interactionMarkOfYngya.LostSouls.Count)
      yield return (object) null;
    interactionMarkOfYngya.SimpleSetCamera.Reset();
    yield return (object) new WaitForSeconds(0.4f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("collect-ghosts-land", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.6f);
    interactionMarkOfYngya.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionMarkOfYngya.state.facingAngle = 270f;
    interactionMarkOfYngya.state.LookAngle = 270f;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionMarkOfYngya.Teleporter.EnableTeleporter();
    if ((bool) (UnityEngine.Object) YngyaRewardManager.Instance)
      YngyaRewardManager.Instance.Callback?.Invoke();
    if (DataManager.Instance.TotalShrineGhostJuice <= 0)
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/InteractYngyaShrine", Objectives.CustomQuestTypes.DepositGhosts), true, true);
  }

  public void ReceiveSoul()
  {
    ++this.SoulCount;
    if (Inventory.GetItemQuantity(209) < 4)
      Inventory.AddItem(InventoryItem.ITEM_TYPE.YNGYA_GHOST, 1);
    DataManager.Instance.ghostLostLambState = DataManager.GhostLostLambState.AppearInBase;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/ghost_deposit", PlayerFarming.Instance.transform.position);
  }

  public IEnumerator ShakeCameraWithRampUp(float Duration)
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < (double) Duration - 0.10000000149011612)
    {
      float t1 = t / (Duration - 0.1f);
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, 0.5f, t1), Mathf.Lerp(0.0f, 1.5f, t1), Duration - 0.1f, false);
      yield return (object) null;
    }
    CameraManager.instance.Stopshake();
  }

  public void EndGoToAndStop()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      player.EndGoToAndStop();
  }
}
