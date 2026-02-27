// Decompiled with JetBrains decompiler
// Type: TrapProjectileCross
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class TrapProjectileCross : BaseMonoBehaviour
{
  [SerializeField]
  private ProjectileCross projectileCross;
  private ProjectileCross currentProjectileCross;
  public GameObject trapOn;
  public GameObject trapOff;
  private Health health;
  private bool active;
  private bool deactivated;

  private void Awake() => this.health = this.GetComponent<Health>();

  private void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  private void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  private void RoomLockController_OnRoomCleared()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.DeactivateProjectiles();
  }

  private void Update()
  {
    if (!GameManager.RoomActive || this.active)
      return;
    this.ActivateProjectiles();
  }

  private void ActivateProjectiles()
  {
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

  private void DeactivateProjectiles()
  {
    this.deactivated = true;
    if ((Object) this.currentProjectileCross != (Object) null)
      this.StartCoroutine((IEnumerator) this.currentProjectileCross.DisableProjectiles());
    this.trapOn.SetActive(false);
    this.trapOff.SetActive(true);
  }

  private void OnDrawGizmos()
  {
    Utils.DrawLine(this.transform.position, this.transform.position + Vector3.up * 2.5f, Color.blue);
    Utils.DrawLine(this.transform.position, this.transform.position + Vector3.down * 2.5f, Color.blue);
    Utils.DrawLine(this.transform.position, this.transform.position + Vector3.right * 2.5f, Color.blue);
    Utils.DrawLine(this.transform.position, this.transform.position + Vector3.left * 2.5f, Color.blue);
  }
}
