// Decompiled with JetBrains decompiler
// Type: FollowAsTail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowAsTail : BaseMonoBehaviour
{
  public Transform FollowObject;
  public bool hasHealth;
  public List<Collider2D> collider2DList;
  public Collider2D DamageCollider;
  public Health health;
  public Health EnemyHealth;
  public Health mainHealth;
  public float Distance = 0.5f;
  public Vector3 Offset = new Vector3(0.0f, 0.0f, 0.0f);
  public SkeletonAnimation _Spine;
  public float Angle;
  public MeshRenderer meshRenderer;
  public MeshRenderer FollowObjectMeshRenderer;
  public SimpleSpineFlash simpleSpineFlash;
  public bool isTransformParentChanged;
  public UnitObject followObjectParentUnit;
  public bool Gravity;
  public float GravitySpeed = 0.3f;
  public float Grav;

  public SkeletonAnimation Spine
  {
    get
    {
      if ((Object) this._Spine == (Object) null)
        this._Spine = this.GetComponent<SkeletonAnimation>();
      return this._Spine;
    }
  }

  public void OnEnable()
  {
    if (this.isTransformParentChanged)
      return;
    this.StartCoroutine((IEnumerator) this.AssignToCorrectParent());
  }

  public void Start()
  {
    this.FollowObjectMeshRenderer = this.FollowObject.gameObject.GetComponent<MeshRenderer>();
    this.followObjectParentUnit = this.FollowObject.transform.parent.GetComponent<UnitObject>();
    if (this.hasHealth)
    {
      this.DamageCollider = this.GetComponent<Collider2D>();
      this.health = this.GetComponent<Health>();
    }
    this.meshRenderer = this.GetComponent<MeshRenderer>();
    this.simpleSpineFlash = this.GetComponent<SimpleSpineFlash>();
    if (!(bool) (Object) this.GetComponentInParent<UnitObject>() || !(bool) (Object) this.simpleSpineFlash)
      return;
    this.mainHealth = this.GetComponentInParent<UnitObject>().GetComponent<Health>();
    if (!(bool) (Object) this.mainHealth)
      return;
    this.mainHealth.OnPoisoned += new Health.StasisEvent(this.OnPoisoned);
    this.mainHealth.OnIced += new Health.StasisEvent(this.OnIced);
    this.mainHealth.OnFreezeTime += new Health.StasisEvent(this.OnFreezeTime);
    this.mainHealth.OnCharmed += new Health.StasisEvent(this.OnCharmed);
    this.mainHealth.OnStasisCleared += new Health.StasisEvent(this.OnStasisCleared);
  }

  public IEnumerator AssignToCorrectParent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowAsTail followAsTail = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      followAsTail.transform.parent = followAsTail.transform.parent.parent;
      followAsTail.isTransformParentChanged = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ForcePosition(Vector3 Direction)
  {
    if (!(bool) (Object) this.FollowObject)
      return;
    this.transform.position = this.FollowObject.position + Direction.normalized * this.Distance;
  }

  public void UpdatePosition()
  {
    if ((double) Vector3.Distance(this.transform.position, this.FollowObject.position) <= (double) this.Distance)
      return;
    this.transform.position = this.FollowObject.position + (this.transform.position - this.FollowObject.position).normalized * this.Distance;
    if (!((Object) this.Spine != (Object) null))
      return;
    this.Spine.skeleton.ScaleX = (double) this.Angle * 57.295780181884766 >= 90.0 || (double) this.Angle * 57.295780181884766 <= -90.0 ? -1f : 1f;
  }

  public void OnDestroy()
  {
    if (!(bool) (Object) this.mainHealth)
      return;
    this.mainHealth.OnPoisoned -= new Health.StasisEvent(this.OnPoisoned);
    this.mainHealth.OnIced -= new Health.StasisEvent(this.OnIced);
    this.mainHealth.OnFreezeTime -= new Health.StasisEvent(this.OnFreezeTime);
    this.mainHealth.OnCharmed -= new Health.StasisEvent(this.OnCharmed);
    this.mainHealth.OnStasisCleared -= new Health.StasisEvent(this.OnStasisCleared);
  }

  public void Update()
  {
    if ((Object) this.FollowObject == (Object) null)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      if ((Object) this.meshRenderer != (Object) null)
      {
        if (!this.FollowObjectMeshRenderer.enabled)
        {
          this.meshRenderer.enabled = false;
          return;
        }
        if ((Object) this.FollowObject.transform.parent != (Object) null && (Object) this.followObjectParentUnit != (Object) null)
          this.meshRenderer.enabled = this.FollowObject.gameObject.activeInHierarchy;
        else if ((Object) this.FollowObjectMeshRenderer != (Object) null)
          this.meshRenderer.enabled = this.FollowObjectMeshRenderer.enabled;
      }
      this.UpdatePosition();
      if (this.Gravity && (double) this.FollowObject.position.z >= 0.0)
      {
        if ((double) this.transform.position.z < 0.0)
        {
          this.Grav += -this.GravitySpeed * Time.deltaTime;
          this.transform.position += Vector3.back * this.Grav;
        }
        else
        {
          this.Grav = 0.0f;
          this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
        }
      }
      if (!this.hasHealth)
        return;
      this.collider2DList = new List<Collider2D>();
      this.DamageCollider.GetContacts((List<Collider2D>) this.collider2DList);
      Debug.Log((object) this.DamageCollider.GetContacts((List<Collider2D>) this.collider2DList));
      foreach (Component component in this.collider2DList)
      {
        this.EnemyHealth = component.gameObject.GetComponent<Health>();
        if ((Object) this.EnemyHealth != (Object) null && this.EnemyHealth.team != this.health.team)
        {
          Debug.Log((object) "DAMAGE");
          this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
        }
      }
    }
  }

  public void OnPoisoned() => this.simpleSpineFlash.Tint(Color.green);

  public void OnIced()
  {
    this.Spine.timeScale = 0.25f;
    this.simpleSpineFlash.Tint(Color.cyan);
  }

  public void OnFreezeTime() => this.Spine.timeScale = 0.0f;

  public void OnCharmed()
  {
    AudioManager.Instance.PlayOneShot("event:/player/Curses/enemy_charmed", this.gameObject);
    this.simpleSpineFlash.Tint(Color.red);
  }

  public void OnStasisCleared()
  {
    this.Spine.timeScale = 1f;
    this.simpleSpineFlash.Tint(Color.white);
  }
}
