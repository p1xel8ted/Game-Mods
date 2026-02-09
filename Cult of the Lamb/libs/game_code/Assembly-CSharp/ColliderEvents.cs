// Decompiled with JetBrains decompiler
// Type: ColliderEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ColliderEvents : BaseMonoBehaviour
{
  public bool debug;
  public bool Performance;
  public bool DestroyOnComplete;
  public List<Collider2D> OnTriggerEnterColList;
  public float timedDisableTimer;
  public float disableTime;
  public int CountPerFrame = 2;
  public bool isTimedDisableActive;

  public event ColliderEvents.TriggerEvent OnTriggerEnterEvent;

  public event ColliderEvents.TriggerEvent OnTriggerExitEvent;

  public event ColliderEvents.TriggerEvent OnTriggerStayEvent;

  public event ColliderEvents.CollisionEvent OnCollisionEnterEvent;

  public event ColliderEvents.CollisionEvent OnCollisionExitEvent;

  public event ColliderEvents.CollisionEvent OnCollisionStayEvent;

  public void Awake() => this.OnTriggerEnterColList = new List<Collider2D>();

  public void BeginTriggerEnter(float time)
  {
    this.StartCoroutine((IEnumerator) this.StartTriggerEnter(time / (float) this.OnTriggerEnterColList.Count));
  }

  public IEnumerator StartTriggerEnter(float time)
  {
    ColliderEvents colliderEvents = this;
    if (colliderEvents.OnTriggerEnterColList.Count > 0)
    {
      int i = 0;
      while (i < colliderEvents.OnTriggerEnterColList.Count)
      {
        for (int index = 0; index < colliderEvents.CountPerFrame; ++index)
        {
          if (i >= colliderEvents.OnTriggerEnterColList.Count)
            i = 0;
          if ((Object) colliderEvents.OnTriggerEnterColList[i] != (Object) null)
          {
            try
            {
              ColliderEvents.TriggerEvent triggerEnterEvent = colliderEvents.OnTriggerEnterEvent;
              if (triggerEnterEvent != null)
                triggerEnterEvent(colliderEvents.OnTriggerEnterColList[i]);
            }
            catch
            {
              ++i;
              continue;
            }
          }
          ++i;
        }
        yield return (object) new WaitForSeconds(time);
      }
    }
    if (colliderEvents.DestroyOnComplete)
      Object.Destroy((Object) colliderEvents.gameObject);
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (this.Performance)
    {
      this.OnTriggerEnterColList.Add(collider);
    }
    else
    {
      ColliderEvents.TriggerEvent triggerEnterEvent = this.OnTriggerEnterEvent;
      if (triggerEnterEvent == null)
        return;
      triggerEnterEvent(collider);
    }
  }

  public void OnTriggerExit2D(Collider2D collider)
  {
    ColliderEvents.TriggerEvent triggerExitEvent = this.OnTriggerExitEvent;
    if (triggerExitEvent == null)
      return;
    triggerExitEvent(collider);
  }

  public void OnTriggerStay2D(Collider2D collider)
  {
    ColliderEvents.TriggerEvent triggerStayEvent = this.OnTriggerStayEvent;
    if (triggerStayEvent == null)
      return;
    triggerStayEvent(collider);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    ColliderEvents.CollisionEvent collisionEnterEvent = this.OnCollisionEnterEvent;
    if (collisionEnterEvent == null)
      return;
    collisionEnterEvent(collision);
  }

  public void OnCollisionExit2D(Collision2D collision)
  {
    ColliderEvents.CollisionEvent collisionExitEvent = this.OnCollisionExitEvent;
    if (collisionExitEvent == null)
      return;
    collisionExitEvent(collision);
  }

  public void OnCollisionStay2D(Collision2D collision)
  {
    ColliderEvents.CollisionEvent collisionStayEvent = this.OnCollisionStayEvent;
    if (collisionStayEvent == null)
      return;
    collisionStayEvent(collision);
  }

  public void SetActive(bool active)
  {
    if (this.debug)
      Debug.Log((object) $"ColliderEvents on {this.gameObject.name} active: {active}", (Object) this.gameObject);
    if ((Object) this.gameObject == (Object) null || this.gameObject.activeSelf == active)
      return;
    this.gameObject.SetActive(active);
  }

  public void StartTimedDisable(float duration)
  {
    this.isTimedDisableActive = true;
    this.disableTime = duration;
  }

  public void Update()
  {
    if (!this.isTimedDisableActive || (double) this.timedDisableTimer >= (double) this.disableTime)
      return;
    this.timedDisableTimer += Time.deltaTime;
    if ((double) this.timedDisableTimer <= (double) this.disableTime)
      return;
    this.isTimedDisableActive = false;
    this.timedDisableTimer = 0.0f;
    this.gameObject.SetActive(false);
  }

  public delegate void TriggerEvent(Collider2D collider);

  public delegate void CollisionEvent(Collision2D collision);
}
