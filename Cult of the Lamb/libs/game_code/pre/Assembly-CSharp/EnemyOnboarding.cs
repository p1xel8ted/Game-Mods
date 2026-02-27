// Decompiled with JetBrains decompiler
// Type: EnemyOnboarding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyOnboarding : BaseMonoBehaviour
{
  private UnitObject[] enemies;
  private Coroutine waitingRoutine;
  private GameObject chef;

  private void OnEnable()
  {
    if (this.waitingRoutine != null)
      this.StopCoroutine(this.waitingRoutine);
    this.waitingRoutine = this.StartCoroutine((IEnumerator) this.WaitForTileToLoad());
  }

  private IEnumerator WaitForTileToLoad()
  {
    while (!BiomeGenerator.Instance.CurrentRoom.generateRoom.GeneratedDecorations)
      yield return (object) null;
    this.waitingRoutine = (Coroutine) null;
    this.OnboardEnemy();
    this.SpawnShopKeeperChef();
  }

  private void OnboardEnemy()
  {
    if (DataManager.Instance == null)
      return;
    UnitObject unitObject = (UnitObject) null;
    this.enemies = this.GetComponentsInChildren<UnitObject>(true);
    foreach (UnitObject enemy in this.enemies)
    {
      if (!DataManager.Instance.HasEncounteredEnemy(enemy.name) && (bool) (Object) enemy.GetComponent<EnemyRequiresOnboarding>() && enemy.gameObject.activeSelf)
        unitObject = enemy;
    }
    if (!((Object) unitObject != (Object) null))
      return;
    Interaction_Chest instance = Interaction_Chest.Instance;
    foreach (UnitObject enemy in this.enemies)
    {
      if ((Object) enemy != (Object) unitObject)
      {
        instance.Enemies.Remove(enemy.health);
        Object.Destroy((Object) enemy.gameObject);
      }
    }
    foreach (RaycastHit2D raycastHit2D in Physics2D.CircleCastAll((Vector2) instance.transform.position, 2f, Vector2.zero))
    {
      Health component = raycastHit2D.collider.GetComponent<Health>();
      if ((Object) component != (Object) null && component.team != Health.Team.PlayerTeam && component.team != Health.Team.Team2 && (Object) component != (Object) unitObject.health)
        component.DealDamage((float) int.MaxValue, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    }
    BreakableSpiderNest[] componentsInChildren1 = this.GetComponentsInChildren<BreakableSpiderNest>();
    if (componentsInChildren1.Length != 0)
    {
      for (int index = componentsInChildren1.Length - 1; index >= 0; --index)
        componentsInChildren1[index].GetComponent<Health>().DealDamage(float.MaxValue, this.gameObject, this.transform.position);
    }
    SpiderNest[] componentsInChildren2 = this.GetComponentsInChildren<SpiderNest>();
    if (componentsInChildren2.Length != 0)
    {
      for (int index = componentsInChildren2.Length - 1; index >= 0; --index)
        Object.Destroy((Object) componentsInChildren2[index].gameObject);
    }
    TrapCharger[] componentsInChildren3 = this.GetComponentsInChildren<TrapCharger>();
    if (componentsInChildren3.Length != 0)
    {
      for (int index = componentsInChildren3.Length - 1; index >= 0; --index)
        Object.Destroy((Object) componentsInChildren3[index].gameObject);
    }
    TrapSpikes[] componentsInChildren4 = this.GetComponentsInChildren<TrapSpikes>();
    if (componentsInChildren4.Length != 0)
    {
      for (int index = componentsInChildren4.Length - 1; index >= 0; --index)
        Object.Destroy((Object) componentsInChildren4[index].ParentToDestroy);
    }
    TrapProjectileCross[] componentsInChildren5 = this.GetComponentsInChildren<TrapProjectileCross>();
    if (componentsInChildren5.Length != 0)
    {
      for (int index = componentsInChildren5.Length - 1; index >= 0; --index)
        Object.Destroy((Object) componentsInChildren5[index].gameObject);
    }
    TrapRockFall[] componentsInChildren6 = this.GetComponentsInChildren<TrapRockFall>();
    if (componentsInChildren6.Length != 0)
    {
      for (int index = componentsInChildren6.Length - 1; index >= 0; --index)
        Object.Destroy((Object) componentsInChildren6[index].gameObject);
    }
    unitObject.transform.position = Interaction_Chest.Instance.transform.position;
    unitObject.RemoveModifier();
    Health.team2.Clear();
    Health.team2.Add(unitObject.GetComponent<Health>());
    DataManager.Instance.AddEncounteredEnemy(unitObject.name);
  }

  private void SpawnShopKeeperChef()
  {
    bool flag = (Object) BiomeGenerator.Instance.CurrentRoom.generateRoom.GetComponentInChildren<DungeonLeaderMechanics>(true) != (Object) null;
    if (DataManager.Instance.ShopKeeperChefState != 1 || flag || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) || DataManager.Instance.playerDeathsInARow != 0 || (double) Random.Range(0.0f, 1f) >= 0.05000000074505806)
      return;
    Interaction_Chest instance = Interaction_Chest.Instance;
    this.enemies = this.GetComponentsInChildren<UnitObject>(true);
    if (this.enemies.Length <= 1)
      return;
    int index = 0;
    if (index >= this.enemies.Length)
      return;
    Vector3 position = this.enemies[index].transform.position;
    Transform parent = this.enemies[index].transform.parent;
    instance.Enemies.Remove(this.enemies[index].health);
    Object.Destroy((Object) this.enemies[index].gameObject);
    this.chef = Object.Instantiate<GameObject>(BiomeConstants.Instance.ShopKeeperChef, position, Quaternion.identity, parent);
    this.chef.GetComponent<Health>().OnDie += new Health.DieAction(this.ChefDied);
    instance.AddEnemy(this.chef.GetComponent<Health>());
    if (RoomLockController.DoorsOpen)
      this.chef.GetComponentInChildren<Interaction_SimpleConversation>(true).Interactable = true;
    else
      RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.OnRoomCompleted);
  }

  private void OnRoomCompleted()
  {
    this.chef.GetComponentInChildren<Interaction_SimpleConversation>(true).Interactable = true;
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.OnRoomCompleted);
  }

  private void ChefDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    DataManager.Instance.ShopKeeperChefState = 2;
  }

  private void OnDisable()
  {
    if (!((Object) this.chef != (Object) null))
      return;
    Object.Destroy((Object) this.chef.gameObject);
  }
}
