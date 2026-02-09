// Decompiled with JetBrains decompiler
// Type: FollowerBonfire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerBonfire : BaseMonoBehaviour
{
  public DataManager.Variables VariableOnComplete;
  public Vector3 TriggerArea = Vector3.zero;
  public float TriggerRadius = 5f;
  public GameObject Player;
  public BarricadeLine barricadeLine;
  public EnemyRounds enemyRounds;
  public Interaction interaction;
  public LocalizedString _MyLocalizedString;
  public GameObject BonfireOn;
  public GameObject BonfireOff;
  public GameObject Follower;
  public GameObject Bag;
  public Collider2D CutTheRopeCollider;

  public void OnEnable()
  {
    if (DataManager.Instance.GetVariable(this.VariableOnComplete))
      return;
    this.interaction.Interactable = false;
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
    this.interaction.Label = (string) this._MyLocalizedString;
  }

  public void OnDisable() => this.StopAllCoroutines();

  public void Start() => this.CutTheRopeCollider.enabled = false;

  public IEnumerator WaitForPlayer()
  {
    FollowerBonfire followerBonfire = this;
    while ((UnityEngine.Object) (followerBonfire.Player = GameObject.FindGameObjectWithTag("Player")) == (UnityEngine.Object) null)
      yield return (object) null;
    while ((double) Vector3.Distance(followerBonfire.transform.position + followerBonfire.TriggerArea, followerBonfire.Player.transform.position) > (double) followerBonfire.TriggerRadius)
      yield return (object) null;
    BiomeGenerator.Instance.CurrentRoom.Active = true;
    followerBonfire.barricadeLine.Close();
    BlockingDoor.CloseAll();
    RoomLockController.CloseAll();
    followerBonfire.enemyRounds.BeginCombat(false, new System.Action(followerBonfire.Close));
  }

  public void ReleaseFollower()
  {
    this.BonfireOn.SetActive(false);
    this.Bag.SetActive(false);
    this.Follower.SetActive(true);
    BlockingDoor.OpenAll();
    DataManager.Instance.SetVariable(DataManager.Variables.ForestRescueWorshipper, true);
  }

  public void Close() => this.StartCoroutine((IEnumerator) this.CloseRoutine());

  public IEnumerator CloseRoutine()
  {
    DataManager.Instance.SetVariable(this.VariableOnComplete, true);
    this.barricadeLine.Open();
    this.CutTheRopeCollider.enabled = true;
    this.interaction.Interactable = true;
    yield return (object) new WaitForSeconds(0.5f);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.TriggerArea, this.TriggerRadius, Color.yellow);
  }
}
