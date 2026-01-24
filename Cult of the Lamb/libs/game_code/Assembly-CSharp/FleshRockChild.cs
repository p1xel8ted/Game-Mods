// Decompiled with JetBrains decompiler
// Type: FleshRockChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMODUnity;
using MMBiomeGeneration;
using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class FleshRockChild : BaseMonoBehaviour
{
  [SerializeField]
  public SpriteRenderer[] rockSprites;
  [SerializeField]
  public Color activeColor;
  [SerializeField]
  public Color inactiveColor;
  [SerializeField]
  public float fadeTime;
  [SerializeField]
  public GameObject bombVisualSprite;
  [EventRef]
  public string DestroyFleshBlockSFX = "event:/dlc/dungeon06/trap/flesh_rock/block_fleshy_destroy";
  [EventRef]
  public string DestroyPetrifiedBlockSFX = "event:/dlc/dungeon06/trap/flesh_rock/block_petrified_destroy";
  public Health health;
  public bool dropBombOnDeath;
  public bool isAlive = true;
  public bool roomClearedPreventsBombs;

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    foreach (SpriteRenderer rockSprite in this.rockSprites)
      rockSprite.color = this.activeColor;
  }

  public void roomCleared() => this.roomClearedPreventsBombs = true;

  public void OnEnable()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.roomCleared);
  }

  public void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.roomCleared);
  }

  public void SetDropBombOnDeath(bool drop)
  {
    this.dropBombOnDeath = drop;
    this.bombVisualSprite.SetActive(drop);
  }

  public void OnDie(
    GameObject attacker,
    Vector3 attacklocation,
    Health victim,
    Health.AttackTypes attacktype,
    Health.AttackFlags attackflags)
  {
    this.PlayDeathSFX();
    if (!this.dropBombOnDeath || this.roomClearedPreventsBombs)
      return;
    if ((!((Object) BiomeGenerator.Instance != (Object) null) || BiomeGenerator.Instance.CurrentRoom == null || !BiomeGenerator.Instance.CurrentRoom.Completed ? (PlayerFarming.Location != FollowerLocation.Boss_Wolf ? 0 : (DataManager.Instance.BeatenWolf ? 1 : 0)) : 1) != 0)
      Debug.Log((object) "No bombs after room clear");
    else
      Bomb.CreateBomb(new Vector3(this.transform.position.x, this.transform.position.y, (Object) GenerateRoom.Instance != (Object) null ? GenerateRoom.Instance.transform.position.z : this.transform.position.z), this.health, this.transform.parent);
  }

  public void SetRockActive(bool active, float delay)
  {
    DOVirtual.DelayedCall(delay, (TweenCallback) (() =>
    {
      this.isAlive = false;
      foreach (SpriteRenderer rockSprite in this.rockSprites)
        rockSprite.DOColor(active ? this.activeColor : this.inactiveColor, this.fadeTime);
    }));
  }

  public void PlayDeathSFX()
  {
    string soundPath = this.isAlive ? this.DestroyFleshBlockSFX : this.DestroyPetrifiedBlockSFX;
    if (string.IsNullOrEmpty(soundPath))
      return;
    AudioManager.Instance.PlayOneShot(soundPath, this.gameObject);
  }
}
