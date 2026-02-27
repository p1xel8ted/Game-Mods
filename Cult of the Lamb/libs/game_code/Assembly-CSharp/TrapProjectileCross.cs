// Decompiled with JetBrains decompiler
// Type: TrapProjectileCross
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TrapProjectileCross : BaseMonoBehaviour, IProjectileTrap
{
  [SerializeField]
  public ProjectileCross projectileCross;
  public ProjectileCross currentProjectileCross;
  public GameObject trapOn;
  public GameObject trapOff;
  public Health health;
  public bool active;
  public bool isDisarmed;

  public void Awake() => this.health = this.GetComponent<Health>();

  public void OnEnable()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
    this.active = false;
  }

  public void RoomLockController_OnRoomCleared()
  {
    if (this.gameObject.activeInHierarchy)
      this.DeactivateProjectiles();
    this.isDisarmed = true;
  }

  public void Update()
  {
    if (!GameManager.RoomActive || this.active || this.isDisarmed)
      return;
    this.ActivateProjectiles();
  }

  public void ActivateProjectiles()
  {
    this.health.enabled = true;
    this.active = true;
    this.trapOn.SetActive(true);
    this.trapOff.SetActive(false);
    Projectile component1 = Object.Instantiate<ProjectileCross>(this.projectileCross, this.transform).GetComponent<Projectile>();
    component1.transform.position = this.transform.position;
    component1.health = this.health;
    component1.team = Health.Team.Team2;
    ProjectileCross component2 = component1.GetComponent<ProjectileCross>();
    component2.InitDelayed();
    this.currentProjectileCross = component2;
  }

  public void DeactivateProjectiles()
  {
    if ((Object) this.currentProjectileCross != (Object) null)
      this.StartCoroutine(this.currentProjectileCross.DisableProjectiles(true));
    this.trapOn.SetActive(false);
    this.trapOff.SetActive(true);
    this.health.enabled = false;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawLine(this.transform.position, this.transform.position + Vector3.up * 2.5f, Color.blue);
    Utils.DrawLine(this.transform.position, this.transform.position + Vector3.down * 2.5f, Color.blue);
    Utils.DrawLine(this.transform.position, this.transform.position + Vector3.right * 2.5f, Color.blue);
    Utils.DrawLine(this.transform.position, this.transform.position + Vector3.left * 2.5f, Color.blue);
  }
}
