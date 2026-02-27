// Decompiled with JetBrains decompiler
// Type: SpiderNest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class SpiderNest : BaseMonoBehaviour
{
  [SerializeField]
  public SpiderNest.DropType dropType;
  [SerializeField]
  public float dropGravity;
  [SerializeField]
  public AnimationCurve gravityCurve;
  [SerializeField]
  public float radius;
  [SerializeField]
  public AssetReferenceGameObject[] enemiesList;
  [SerializeField]
  public AssetReferenceGameObject[] ExtraEnemyList;
  [SerializeField]
  public bool random = true;
  [SerializeField]
  public Vector2 amount;
  [Space]
  [SerializeField]
  public GameObject unbroken;
  [SerializeField]
  public GameObject broken;
  [SerializeField]
  public GameObject breakParticle;
  public float gravity = -1f;
  public float dropTimer;
  public float shakeTimer;
  public bool dropping;
  public bool dropped;
  public bool enteredRoom;
  public List<KeyValuePair<EnemySpider, Vector3>> spawnedEnemies = new List<KeyValuePair<EnemySpider, Vector3>>();
  public static List<SpiderNest> activeNests = new List<SpiderNest>();
  public Queue<Health> spawnedEnemiesInDisabledMode = new Queue<Health>();
  public Vector2 shakeDelay = new Vector2(2f, 5f);
  public Vector2 tryToDropDelay = new Vector2(5f, 12f);
  [CompilerGenerated]
  public bool \u003CDroppable\u003Ek__BackingField = true;
  public bool shouldSpawnEnemies = true;

  public bool Droppable
  {
    get => this.\u003CDroppable\u003Ek__BackingField;
    set => this.\u003CDroppable\u003Ek__BackingField = value;
  }

  public void Awake() => this.breakParticle.CreatePool(6, true);

  public void Start()
  {
    if (this.dropType != SpiderNest.DropType.None)
      this.PrewarmEnemies();
    this.dropTimer = Time.time + UnityEngine.Random.Range(this.tryToDropDelay.x, this.tryToDropDelay.y);
    this.shakeTimer = Time.time + UnityEngine.Random.Range(this.shakeDelay.x, this.shakeDelay.y);
  }

  public void OnEnable()
  {
    if (!this.dropped && this.dropping)
      this.dropping = false;
    SpiderNest.activeNests.Add(this);
    while (this.spawnedEnemiesInDisabledMode.Count > 0)
    {
      Health health = this.spawnedEnemiesInDisabledMode.Dequeue();
      Health.team2.Add(health);
    }
    Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  public void OnChestRevealed() => this.shouldSpawnEnemies = false;

  public void OnDisable()
  {
    SpiderNest.activeNests.Remove(this);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  public void PrewarmEnemies()
  {
    Vector3 vector3 = new Vector3(this.transform.position.x, this.transform.position.y + this.transform.parent.position.z, this.transform.position.z - this.transform.parent.position.z);
    int num = (int) UnityEngine.Random.Range(this.amount.x, this.amount.y);
    if (this.random)
    {
      for (int index = 0; index < num; ++index)
        Addressables_wrapper.InstantiateAsync((object) this.enemiesList[UnityEngine.Random.Range(0, this.enemiesList.Length)], vector3 + (Vector3) UnityEngine.Random.insideUnitCircle * this.radius, Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          EnemySpider component1 = obj.Result.GetComponent<EnemySpider>();
          Health component2 = component1.GetComponent<Health>();
          component1.enabled = false;
          component1.gameObject.SetActive(false);
          component2.enabled = false;
          if (this.gameObject.activeInHierarchy)
            Health.team2.Add(component2);
          else
            this.spawnedEnemiesInDisabledMode.Enqueue(component2);
          Interaction_Chest.Instance?.AddEnemy(component2);
          this.spawnedEnemies.Add(new KeyValuePair<EnemySpider, Vector3>(component1, Vector3.zero));
        }));
    }
    else
    {
      for (int index = 0; index < this.enemiesList.Length; ++index)
        Addressables_wrapper.InstantiateAsync((object) this.enemiesList[index], vector3 + (Vector3) UnityEngine.Random.insideUnitCircle * this.radius, Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          EnemySpider component3 = obj.Result.GetComponent<EnemySpider>();
          Health component4 = component3.GetComponent<Health>();
          component3.enabled = false;
          component3.gameObject.SetActive(false);
          component4.enabled = false;
          if (this.gameObject.activeInHierarchy)
            Health.team2.Add(component4);
          else
            this.spawnedEnemiesInDisabledMode.Enqueue(component4);
          Interaction_Chest.Instance?.AddEnemy(component4);
          this.spawnedEnemies.Add(new KeyValuePair<EnemySpider, Vector3>(component3, Vector3.zero));
        }));
    }
    if (this.ExtraEnemyList == null || this.ExtraEnemyList.Length == 0)
      return;
    Addressables_wrapper.InstantiateAsync((object) this.ExtraEnemyList[UnityEngine.Random.Range(0, this.ExtraEnemyList.Length)], vector3 + (Vector3) UnityEngine.Random.insideUnitCircle * this.radius, Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemySpider component5 = obj.Result.GetComponent<EnemySpider>();
      Health component6 = component5.GetComponent<Health>();
      component5.enabled = false;
      component5.gameObject.SetActive(false);
      component6.enabled = false;
      if (this.gameObject.activeInHierarchy)
        Health.team2.Add(component6);
      else
        this.spawnedEnemiesInDisabledMode.Enqueue(component6);
      Interaction_Chest.Instance?.AddEnemy(component6);
      this.spawnedEnemies.Add(new KeyValuePair<EnemySpider, Vector3>(component5, Vector3.zero));
    }));
  }

  public void Update()
  {
    if (Health.isGlobalTimeFreeze || !GameManager.RoomActive)
      return;
    if (!this.enteredRoom)
    {
      this.Shake();
      this.enteredRoom = true;
      this.dropTimer = Time.time + UnityEngine.Random.Range(this.tryToDropDelay.x, this.tryToDropDelay.y);
      this.shakeTimer = Time.time + UnityEngine.Random.Range(this.shakeDelay.x, this.shakeDelay.y);
    }
    if (!this.dropping && (double) Time.time > (double) this.shakeTimer)
      this.Shake();
    else if (!this.dropping && (double) Time.time > (double) this.dropTimer)
      this.DropEnemies();
    if (this.dropping && (double) this.gravity != -1.0)
    {
      this.gravity += this.dropGravity * Time.deltaTime;
      float time = this.gravity / 1f;
      for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
      {
        KeyValuePair<EnemySpider, Vector3> spawnedEnemy = this.spawnedEnemies[index];
        if (!((UnityEngine.Object) spawnedEnemy.Key == (UnityEngine.Object) null))
        {
          bool flag = false;
          spawnedEnemy = this.spawnedEnemies[index];
          Vector3 b = spawnedEnemy.Key.transform.TransformPoint(Vector3.zero);
          spawnedEnemy = this.spawnedEnemies[index];
          Transform transform1 = spawnedEnemy.Key.Spine.transform;
          spawnedEnemy = this.spawnedEnemies[index];
          Vector3 vector3_1 = Vector3.Lerp(spawnedEnemy.Value, b, this.gravityCurve.Evaluate(time));
          transform1.position = vector3_1;
          spawnedEnemy = this.spawnedEnemies[index];
          if ((double) Vector3.Distance(spawnedEnemy.Key.Spine.transform.position, b) < 0.5)
          {
            spawnedEnemy = this.spawnedEnemies[index];
            spawnedEnemy.Key.enabled = true;
            spawnedEnemy = this.spawnedEnemies[index];
            Health component1 = spawnedEnemy.Key.GetComponent<Health>();
            if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
            {
              component1.enabled = true;
              Interaction_Chest.Instance?.AddEnemy(component1);
            }
            spawnedEnemy = this.spawnedEnemies[index];
            if (spawnedEnemy.Key.Spine.AnimationState != null)
            {
              spawnedEnemy = this.spawnedEnemies[index];
              spawnedEnemy.Key.Spine.AnimationState.SetAnimation(0, "land", false);
              spawnedEnemy = this.spawnedEnemies[index];
              spawnedEnemy.Key.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
              spawnedEnemy = this.spawnedEnemies[index];
              Transform parent = spawnedEnemy.Key.Spine.transform.parent;
              spawnedEnemy = this.spawnedEnemies[index];
              EnemySpider key = spawnedEnemy.Key;
              if ((UnityEngine.Object) parent == (UnityEngine.Object) key)
              {
                spawnedEnemy = this.spawnedEnemies[index];
                spawnedEnemy.Key.Spine.transform.position = Vector3.zero;
              }
              else
              {
                spawnedEnemy = this.spawnedEnemies[index];
                Transform transform2 = spawnedEnemy.Key.Spine.transform;
                spawnedEnemy = this.spawnedEnemies[index];
                Vector3 vector3_2 = spawnedEnemy.Key.transform.TransformPoint(Vector3.zero);
                transform2.position = vector3_2;
              }
              spawnedEnemy = this.spawnedEnemies[index];
              Transform transform3 = spawnedEnemy.Key.transform;
              spawnedEnemy = this.spawnedEnemies[index];
              Vector3 position1 = spawnedEnemy.Key.Spine.transform.position;
              transform3.position = position1;
              spawnedEnemy = this.spawnedEnemies[index];
              spawnedEnemy.Key.Spine.transform.localPosition = Vector3.zero;
              spawnedEnemy = this.spawnedEnemies[index];
              foreach (Component component2 in Physics2D.OverlapCircleAll((Vector2) spawnedEnemy.Key.transform.position, 0.5f))
              {
                Health component3 = component2.GetComponent<Health>();
                if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && component3.team == Health.Team.Neutral)
                {
                  Health health = component3;
                  spawnedEnemy = this.spawnedEnemies[index];
                  GameObject gameObject = spawnedEnemy.Key.gameObject;
                  Vector3 position2 = component3.transform.position;
                  spawnedEnemy = this.spawnedEnemies[index];
                  Vector3 position3 = spawnedEnemy.Key.transform.position;
                  Vector3 AttackLocation = Vector3.Lerp(position2, position3, 0.7f);
                  health.DealDamage((float) int.MaxValue, gameObject, AttackLocation, dealDamageImmediately: true);
                }
              }
              flag = true;
            }
            else
              continue;
          }
          if (!this.shouldSpawnEnemies)
          {
            spawnedEnemy = this.spawnedEnemies[index];
            Health component = spawnedEnemy.Key.GetComponent<Health>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
              component.DealDamage(1000f, (GameObject) null, Vector3.zero, dealDamageImmediately: true);
            flag = true;
          }
          if (flag)
            this.spawnedEnemies.Remove(this.spawnedEnemies[index]);
        }
      }
      if (this.spawnedEnemies.Count != 0)
        return;
      this.enabled = false;
    }
    else
    {
      if (this.dropping || this.dropType == SpiderNest.DropType.Manual || !this.AllEnemiesDefeated())
        return;
      if (this.dropType != SpiderNest.DropType.None)
      {
        this.DropEnemies();
      }
      else
      {
        this.dropping = true;
        this.StartCoroutine(this.ShakeIE());
      }
    }
  }

  public bool AllEnemiesDefeated()
  {
    int num1 = 0;
    foreach (SpiderNest activeNest in SpiderNest.activeNests)
      num1 += activeNest.spawnedEnemies.Count;
    int num2 = 0;
    foreach (UnityEngine.Object @object in Health.team2)
    {
      if (@object != (UnityEngine.Object) null)
        ++num2;
    }
    return num2 <= num1;
  }

  public void DropEnemies()
  {
    if (!this.Droppable)
      return;
    this.StartCoroutine(this.DropEnemiesIE());
  }

  public void Shake() => this.StartCoroutine(this.ShakeIE());

  public IEnumerator ShakeIE(float delay = 0.0f)
  {
    this.shakeTimer = Time.time + UnityEngine.Random.Range(this.shakeDelay.x, this.shakeDelay.y);
    Vector3 ogPosition = this.unbroken.transform.localPosition;
    Vector3 ogScale = this.unbroken.transform.localScale;
    yield return (object) new WaitForSeconds(delay);
    this.unbroken.transform.DOShakeScale(1f, 0.025f, fadeOut: false).SetEase<Tweener>(Ease.Linear);
    this.unbroken.transform.DOShakePosition(1f, 0.025f, fadeOut: false).SetEase<Tweener>(Ease.Linear);
    yield return (object) new WaitForSeconds(0.25f);
    this.unbroken.transform.localPosition = ogPosition;
    this.unbroken.transform.localScale = ogScale;
  }

  public IEnumerator DropEnemiesIE()
  {
    SpiderNest spiderNest = this;
    spiderNest.dropping = true;
    yield return (object) spiderNest.StartCoroutine(spiderNest.ShakeIE());
    yield return (object) new WaitForSeconds(0.3f);
    spiderNest.unbroken.transform.DOScaleX(1.25f, 0.6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce);
    spiderNest.unbroken.transform.DOScaleY(0.75f, 0.6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce);
    yield return (object) new WaitForSeconds(0.6f);
    spiderNest.gravity = 0.0f;
    spiderNest.broken.SetActive(true);
    spiderNest.unbroken.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_impact", spiderNest.gameObject);
    if (!BiomeGenerator.Instance.CurrentRoom.Completed && GameManager.RoomActive && spiderNest.shouldSpawnEnemies)
    {
      for (int i = 0; i < spiderNest.spawnedEnemies.Count; ++i)
      {
        KeyValuePair<EnemySpider, Vector3> spawnedEnemy = spiderNest.spawnedEnemies[i];
        if ((UnityEngine.Object) spawnedEnemy.Key != (UnityEngine.Object) null)
        {
          spawnedEnemy = spiderNest.spawnedEnemies[i];
          spawnedEnemy.Key.Spine.transform.position = spiderNest.transform.position;
          spawnedEnemy = spiderNest.spawnedEnemies[i];
          spawnedEnemy.Key.gameObject.SetActive(true);
          spawnedEnemy = spiderNest.spawnedEnemies[i];
          Health component = spawnedEnemy.Key.GetComponent<Health>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            component.enabled = true;
            Interaction_Chest.Instance?.AddEnemy(component);
          }
          while (true)
          {
            spawnedEnemy = spiderNest.spawnedEnemies[i];
            if (spawnedEnemy.Key.Spine.AnimationState == null)
              yield return (object) null;
            else
              break;
          }
          spawnedEnemy = spiderNest.spawnedEnemies[i];
          spawnedEnemy.Key.Spine.AnimationState.SetAnimation(0, "falling", true);
          List<KeyValuePair<EnemySpider, Vector3>> spawnedEnemies = spiderNest.spawnedEnemies;
          int index = i;
          spawnedEnemy = spiderNest.spawnedEnemies[i];
          KeyValuePair<EnemySpider, Vector3> keyValuePair = new KeyValuePair<EnemySpider, Vector3>(spawnedEnemy.Key, spiderNest.transform.position);
          spawnedEnemies[index] = keyValuePair;
        }
      }
    }
    else
    {
      for (int index = 0; index < spiderNest.spawnedEnemies.Count; ++index)
      {
        KeyValuePair<EnemySpider, Vector3> spawnedEnemy = spiderNest.spawnedEnemies[index];
        if ((UnityEngine.Object) spawnedEnemy.Key != (UnityEngine.Object) null)
        {
          spawnedEnemy = spiderNest.spawnedEnemies[index];
          Health component = spawnedEnemy.Key.gameObject.GetComponent<Health>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            component.invincible = false;
            component.enabled = true;
            component.DealDamage(float.PositiveInfinity, spiderNest.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile, dealDamageImmediately: true);
          }
        }
      }
    }
    ObjectPool.Spawn(spiderNest.breakParticle, spiderNest.transform.position, Quaternion.identity);
    spiderNest.dropped = true;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(new Vector3(this.transform.position.x, this.transform.position.y + this.transform.parent.position.z, this.transform.position.z - this.transform.parent.position.z)
    {
      z = 0.0f
    }, this.radius, Color.red);
  }

  [CompilerGenerated]
  public void \u003CPrewarmEnemies\u003Eb__33_1(AsyncOperationHandle<GameObject> obj)
  {
    EnemySpider component1 = obj.Result.GetComponent<EnemySpider>();
    Health component2 = component1.GetComponent<Health>();
    component1.enabled = false;
    component1.gameObject.SetActive(false);
    component2.enabled = false;
    if (this.gameObject.activeInHierarchy)
      Health.team2.Add(component2);
    else
      this.spawnedEnemiesInDisabledMode.Enqueue(component2);
    Interaction_Chest.Instance?.AddEnemy(component2);
    this.spawnedEnemies.Add(new KeyValuePair<EnemySpider, Vector3>(component1, Vector3.zero));
  }

  [CompilerGenerated]
  public void \u003CPrewarmEnemies\u003Eb__33_2(AsyncOperationHandle<GameObject> obj)
  {
    EnemySpider component1 = obj.Result.GetComponent<EnemySpider>();
    Health component2 = component1.GetComponent<Health>();
    component1.enabled = false;
    component1.gameObject.SetActive(false);
    component2.enabled = false;
    if (this.gameObject.activeInHierarchy)
      Health.team2.Add(component2);
    else
      this.spawnedEnemiesInDisabledMode.Enqueue(component2);
    Interaction_Chest.Instance?.AddEnemy(component2);
    this.spawnedEnemies.Add(new KeyValuePair<EnemySpider, Vector3>(component1, Vector3.zero));
  }

  [CompilerGenerated]
  public void \u003CPrewarmEnemies\u003Eb__33_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemySpider component1 = obj.Result.GetComponent<EnemySpider>();
    Health component2 = component1.GetComponent<Health>();
    component1.enabled = false;
    component1.gameObject.SetActive(false);
    component2.enabled = false;
    if (this.gameObject.activeInHierarchy)
      Health.team2.Add(component2);
    else
      this.spawnedEnemiesInDisabledMode.Enqueue(component2);
    Interaction_Chest.Instance?.AddEnemy(component2);
    this.spawnedEnemies.Add(new KeyValuePair<EnemySpider, Vector3>(component1, Vector3.zero));
  }

  [Serializable]
  public enum DropType
  {
    None,
    Timed,
    Manual,
  }
}
