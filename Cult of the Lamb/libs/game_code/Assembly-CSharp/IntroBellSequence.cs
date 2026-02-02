// Decompiled with JetBrains decompiler
// Type: IntroBellSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class IntroBellSequence : MonoBehaviour
{
  public GameObject distortionObject;
  public AnimationCurve MovementCurve;
  [SerializeField]
  public float ActivateDistance = 0.666f;
  [SerializeField]
  public SpriteRenderer bellSpriteRenderer;
  [SerializeField]
  public Sprite NormalBellSprite;
  [SerializeField]
  public GameObject LightingChildren;
  [SerializeField]
  public GameObject targetGameObject;
  [SerializeField]
  public Color ghostFlyColor = Color.magenta;
  public string bellSFX = "event:/dlc/env/cutscene/01/hanging_bell_ring";
  public string bellFoleySFX = "event:/dlc/env/cutscene/01/hanging_bell_ring";
  public UnitObject player;
  public Collider2D PlayerCollision;
  public UnityEvent eventTrigger;
  public bool Activated;
  public float Distance;
  public bool foundPlayer;
  public bool triggered;

  public void Start() => this.FindPlayer();

  public void FindPlayer()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((double) Vector3.Distance(this.gameObject.transform.position, player.gameObject.transform.position) < (double) this.ActivateDistance && !this.Activated)
      {
        this.player = player.unitObject;
        this.foundPlayer = true;
      }
    }
  }

  public void Update()
  {
    if (MMConversation.isPlaying)
      return;
    this.FindPlayer();
    if ((Object) this.player == (Object) null)
      return;
    this.Distance = Vector3.Distance(this.gameObject.transform.position, this.player.gameObject.transform.position);
    double num = (double) Vector3.Distance(this.gameObject.transform.position, this.player.gameObject.transform.position);
    if ((double) Vector3.Distance(this.gameObject.transform.position, this.player.gameObject.transform.position) < (double) this.ActivateDistance && !this.Activated)
      this.Activated = true;
    if (!this.Activated)
      return;
    this.Triggered();
  }

  public void Triggered()
  {
    if (this.triggered)
      return;
    this.eventTrigger?.Invoke();
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    DeviceLightingManager.FlashColor(Color.red);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.5f, 0.5f);
    AudioManager.Instance.PlayOneShot(this.bellSFX);
    AudioManager.Instance.PlayOneShot(this.bellFoleySFX);
    this.gameObject.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, 10f), 2f);
    this.bellSpriteRenderer.sprite = this.NormalBellSprite;
    this.LightingChildren.SetActive(true);
    this.StartCoroutine((IEnumerator) this.EmitParticles());
    this.PulseDisplacementObject();
    this.triggered = true;
  }

  public IEnumerator EmitParticles()
  {
    IntroBellSequence introBellSequence = this;
    for (int i = 0; i < 10; ++i)
    {
      SoulCustomTargetLerp.Create(introBellSequence.targetGameObject, introBellSequence.transform.position + new Vector3(Random.Range(-1f, 1f), (float) Random.Range(-1, 1), (float) Random.Range(-1, 1)), Random.Range(3f, 5f), introBellSequence.ghostFlyColor, _MovementCurve: introBellSequence.MovementCurve, sfx: "event:/dlc/env/cutscene/01/ghost_fly");
      yield return (object) new WaitForSeconds(Random.Range(0.05f, 0.2f));
    }
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = this.Activated ? Color.green : Color.red;
    Gizmos.DrawWireSphere(this.transform.position, this.ActivateDistance);
  }

  public void PulseDisplacementObject()
  {
    this.distortionObject.transform.position = this.transform.position with
    {
      z = -0.01f
    };
    this.distortionObject.SetActive(true);
    this.distortionObject.transform.DOKill();
    this.distortionObject.transform.localScale = Vector3.zero;
    this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.distortionObject.SetActive(false)));
  }

  [CompilerGenerated]
  public void \u003CPulseDisplacementObject\u003Eb__23_0()
  {
    this.distortionObject.SetActive(false);
  }
}
