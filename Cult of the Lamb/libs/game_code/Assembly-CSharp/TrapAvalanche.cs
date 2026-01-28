// Decompiled with JetBrains decompiler
// Type: TrapAvalanche
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class TrapAvalanche : TrapRockFall
{
  public float lifeTimer;
  public bool recycling;
  public Transform chaseTarget;
  public Color indicatorColor = Color.white;
  public float indicatorFlashTimer;
  public float lifeTime;
  [SerializeField]
  public Rigidbody2D rigidbody;
  [SerializeField]
  public SpriteRenderer indicatorIcon;
  public Collider2D[] _hitsBuffer = new Collider2D[32 /*0x20*/];

  public void Update()
  {
    this.TryStickToTarget();
    this.lifeTime = this.DropDelay + 2f;
    this.lifeTimer += Time.deltaTime;
    if (!this.recycling && (double) this.lifeTimer >= (double) this.DropDelay + 2.0)
      this.Recycle();
    if ((double) (this.indicatorFlashTimer += Time.deltaTime) < 0.11999999731779099 || !BiomeConstants.Instance.IsFlashLightsActive)
      return;
    this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
    this.indicatorIcon.material.SetColor("_Color", this.indicatorColor);
    this.indicatorFlashTimer = 0.0f;
  }

  public override void OnTriggerEnter2D(Collider2D other)
  {
  }

  public void TryStickToTarget()
  {
    if ((Object) this.chaseTarget == (Object) null)
      return;
    this.rigidbody.position = (Vector2) this.chaseTarget.position;
  }

  public void ResetAvalanche()
  {
    this.lifeTimer = 0.0f;
    this.recycling = false;
    this.debris.SetActive(false);
    this.shadow.DOKill();
    this.shadow.transform.DOKill();
    this.shadow.transform.localScale = Vector3.one;
    this.shadow.color = this.shadow.color with { a = 1f };
    this.shadowToggle.SetActive(false);
    this.rockContainer.transform.localPosition = new Vector3(0.0f, 0.0f, -10f);
    this.dropped = false;
    this.landed = false;
  }

  public void Recycle()
  {
    this.recycling = true;
    this.gameObject.Recycle();
    this.ResetAvalanche();
  }

  public override void Landed()
  {
    int num1 = Physics2D.OverlapCircleNonAlloc((Vector2) this.transform.position, this.collider.radius, this._hitsBuffer);
    for (int index = 0; index < num1; ++index)
    {
      Collider2D collider2D = this._hitsBuffer[index];
      Health component1 = collider2D.GetComponent<Health>();
      PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(collider2D.gameObject);
      TrapBellAvalanche component2 = collider2D.GetComponent<TrapBellAvalanche>();
      if ((bool) (Object) component1 && component1.team == Health.Team.PlayerTeam && !TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, farmingComponent) && !component1.ImmuneToTraps || (bool) (Object) component1 && (bool) (Object) component2)
        component1.DealDamage(1f, this.gameObject, this.transform.position);
    }
    AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/land_large", this.transform.position);
    this.rockContainer.SetActive(false);
    this.debris.SetActive(this.showDebris);
    this.marking.SetActive(false);
    this.shadowToggle.SetActive(false);
    int num2 = -1;
    if (this.particleChunks.Count > 0)
    {
      while (++num2 < 10)
      {
        if ((Object) this.particleMaterial == (Object) null)
          Particle_Chunk.AddNew(this.transform.position, (float) Random.Range(0, 360), this.particleChunks);
        else
          Particle_Chunk.AddNewMat(this.transform.position, (float) Random.Range(0, 360), this.particleChunks, mat: this.particleMaterial);
      }
    }
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.2f, false);
    this.aoeParticles.Play();
    this.onLand?.Invoke();
    this.onLand.RemoveAllListeners();
    this.landed = true;
  }
}
