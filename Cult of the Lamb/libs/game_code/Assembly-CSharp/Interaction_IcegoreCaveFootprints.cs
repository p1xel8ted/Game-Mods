// Decompiled with JetBrains decompiler
// Type: Interaction_IcegoreCaveFootprints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_IcegoreCaveFootprints : Interaction
{
  public Transform playerTarget;
  public bool playFootSounds;
  public AudioSource audioSource;
  public float originalZoom;
  [SerializeField]
  public Animator cameraPanAnimator;
  [SerializeField]
  public GameObject cameraPanTarget;
  [SerializeField]
  public GameObject iceGoreGameObject;

  public CameraFollowTarget cameraFollowTarget => CameraFollowTarget.Instance;

  public void Start()
  {
    this.label = LocalizationManager.GetTranslation("Conversation_NPC/IceGore/Cave/Footprints");
    this.Interactable = true;
    this.audioSource = this.gameObject.AddComponent<AudioSource>();
    this.audioSource.playOnAwake = false;
    this.originalZoom = CameraFollowTarget.Instance.targetDistance;
    this.init();
  }

  public new void OnEnable()
  {
    base.OnEnable();
    this.init();
  }

  public void init()
  {
    if (!DataManager.Instance.IceGoreShown)
      return;
    this.label = "";
    this.cameraPanAnimator.enabled = false;
    this.iceGoreGameObject.SetActive(true);
    this.Interactable = false;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.PanThroughFootprints(state));
    DataManager.Instance.IceGoreShown = true;
    this.label = "";
    this.Interactable = false;
  }

  public IEnumerator PanThroughFootprints(StateMachine state)
  {
    Interaction_IcegoreCaveFootprints icegoreCaveFootprints = this;
    GameManager.GetInstance().OnConversationNew();
    PlayerFarming playerFarming = state.GetComponent<PlayerFarming>();
    if ((Object) playerFarming != (Object) null)
    {
      playerFarming.GoToAndStop(icegoreCaveFootprints.playerTarget.position);
      while (playerFarming.GoToAndStopping)
        yield return (object) null;
    }
    float zoomedInValue = icegoreCaveFootprints.originalZoom * 0.5f;
    icegoreCaveFootprints.cameraPanAnimator.enabled = true;
    icegoreCaveFootprints.cameraPanAnimator.Play("IcegoreFootprintsPan", 0);
    AudioManager.Instance.PlayOneShot("event:/Stings/lore_sting", icegoreCaveFootprints.transform.position);
    yield return (object) new WaitForSeconds(0.1f);
    while ((double) icegoreCaveFootprints.cameraPanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0)
    {
      icegoreCaveFootprints.cameraFollowTarget.ClearAllTargets();
      icegoreCaveFootprints.cameraFollowTarget.AddTarget(icegoreCaveFootprints.cameraPanTarget, 1f);
      GameManager.GetInstance().CameraSetTargetZoom(zoomedInValue);
      yield return (object) null;
    }
    icegoreCaveFootprints.cameraFollowTarget.ClearAllTargets();
    foreach (PlayerFarming player in PlayerFarming.players)
      icegoreCaveFootprints.cameraFollowTarget.AddTarget(player.gameObject, 1f);
    GameManager.GetInstance().CameraSetTargetZoom(icegoreCaveFootprints.originalZoom);
    if ((Object) playerFarming != (Object) null)
    {
      while (playerFarming.GoToAndStopping)
        yield return (object) null;
    }
    GameManager.GetInstance().OnConversationEnd();
    icegoreCaveFootprints.Interactable = false;
    icegoreCaveFootprints.cameraPanAnimator.enabled = false;
  }
}
