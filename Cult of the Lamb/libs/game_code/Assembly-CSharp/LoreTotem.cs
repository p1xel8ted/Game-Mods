// Decompiled with JetBrains decompiler
// Type: LoreTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class LoreTotem : MonoBehaviour
{
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public GameObject totemActivated;
  [SerializeField]
  public GameObject totemDeactivated;
  [SerializeField]
  public GameObject beam;
  [SerializeField]
  public GameObject beamParticle;
  [SerializeField]
  public bool doorTotem;
  public ParticleSystem totemLeaveVFX;
  public Health health;
  public bool activated;
  public int requiredHits;

  public void Awake()
  {
    this.requiredHits = Random.Range(3, 6);
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
    this.totemActivated.SetActive(false);
    this.totemDeactivated.SetActive(true);
    this.beam.SetActive(false);
    this.beamParticle.SetActive(false);
    if (this.doorTotem)
      this.health.enabled = false;
    this.totemLeaveVFX.Stop();
  }

  public void OnDestroy() => this.health.OnHit -= new Health.HitAction(this.Health_OnHit);

  public void Activate()
  {
    if (this.activated)
      return;
    this.totemActivated.SetActive(true);
    this.totemDeactivated.SetActive(false);
    this.activated = true;
    this.totemLeaveVFX.Play();
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    if (this.doorTotem)
      return;
    foreach (Door componentsInChild in BiomeGenerator.Instance.CurrentRoom.generateRoom.GetComponentsInChildren<Door>(true))
    {
      if (componentsInChild.ConnectionType == GenerateRoom.ConnectionTypes.LoreStoneRoom)
      {
        componentsInChild.transform.parent.GetComponentInChildren<PolygonCollider2D>(true).gameObject.SetActive(true);
        BiomeGenerator.Instance.CurrentRoom.generateRoom.RevealDoor(componentsInChild, this);
        break;
      }
    }
  }

  public void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    --this.requiredHits;
    if (this.requiredHits <= 0)
      this.Activate();
    else
      BiomeConstants.Instance.EmitHitVFX(this.transform.position, (float) Random.Range(0, 360), "HitFX_Weak");
  }

  public void ActivateRay(LoreTotem target)
  {
    Vector3 vector3 = this.beam.transform.position + (target.beam.transform.position - this.beam.transform.position) / 2f;
    float num = Vector3.Distance(this.beam.transform.position, target.beam.transform.position);
    this.beam.transform.position = vector3;
    this.beam.transform.LookAt(target.beam.transform.position, Vector3.forward);
    this.beam.transform.Rotate(-90f, 0.0f, 90f);
    this.beam.transform.localScale = this.beam.transform.localScale with
    {
      x = num
    };
    this.beam.gameObject.SetActive(true);
    target.beamParticle.gameObject.SetActive(true);
  }

  public void DeactivateRay()
  {
    this.beam.gameObject.SetActive(false);
    this.beamParticle.gameObject.SetActive(false);
  }
}
