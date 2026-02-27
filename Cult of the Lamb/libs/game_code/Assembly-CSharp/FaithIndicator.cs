// Decompiled with JetBrains decompiler
// Type: FaithIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FaithIndicator : BaseMonoBehaviour
{
  public Follower follower;
  public GameObject ArrowUp;
  public GameObject ArrowDoubleUp;
  public GameObject ArrowDown;
  public GameObject ArrowDoubleDown;
  public GameObject Container;
  public SpriteRenderer Icon;
  public Sprite FaithIcon;
  public Sprite SicknessIcon;
  public Sprite HungerIcon;
  public Sprite SleepIcon;
  public List<Sprite> DisplayQueue = new List<Sprite>();
  public List<GameObject> DisplayQueueArrow = new List<GameObject>();
  public Coroutine cShowIcon;

  public void OnEnable()
  {
    this.follower.OnFollowerBrainAssigned += new System.Action(this.OnFollowerBrainAssigned);
    FollowerBrainStats.OnIllnessChanged += new FollowerBrainStats.StatChangedEvent(this.OnIllness);
    this.HideAll();
    if (this.follower.Brain == null)
      return;
    this.OnFollowerBrainAssigned();
  }

  public void OnFollowerBrainAssigned()
  {
    this.follower.Brain.OnNewThought += new Action<float>(this.OnNewThought);
  }

  public void HideAll()
  {
    this.ArrowUp.SetActive(false);
    this.ArrowDoubleUp.SetActive(false);
    this.ArrowDown.SetActive(false);
    this.ArrowDoubleDown.SetActive(false);
    this.Container.SetActive(false);
  }

  public void OnDisable()
  {
    if (this.follower.Brain != null)
      this.follower.Brain.OnNewThought -= new Action<float>(this.OnNewThought);
    FollowerBrainStats.OnIllnessChanged -= new FollowerBrainStats.StatChangedEvent(this.OnIllness);
  }

  public void OnIllness(int followerID, float newValue, float oldValue, float change)
  {
    if (followerID != this.follower.Brain.Info.ID)
      return;
    if ((double) newValue < (double) oldValue)
      this.AddToQueue(this.SicknessIcon, this.ArrowUp);
    if ((double) newValue <= (double) oldValue)
      return;
    this.AddToQueue(this.SicknessIcon, this.ArrowDown);
  }

  public void OnNewThought(float Delta)
  {
    GameObject Arrow = (GameObject) null;
    if ((double) Delta <= -7.0)
      Arrow = this.ArrowDoubleDown;
    else if ((double) Delta < 0.0)
      Arrow = this.ArrowDown;
    else if ((double) Delta >= 7.0)
      Arrow = this.ArrowDoubleUp;
    else if ((double) Delta >= 0.0)
      Arrow = this.ArrowUp;
    this.AddToQueue(this.FaithIcon, Arrow);
  }

  public void AddToQueue(Sprite Icon, GameObject Arrow)
  {
    this.DisplayQueue.Add(Icon);
    this.DisplayQueueArrow.Add(Arrow);
    if (this.DisplayQueue.Count > 1)
      return;
    this.PlayQueue();
  }

  public void PlayQueue()
  {
    if (this.DisplayQueue.Count <= 0)
      return;
    if (this.cShowIcon != null)
      this.StopCoroutine(this.cShowIcon);
    this.cShowIcon = this.StartCoroutine(this.ShowIcon(this.DisplayQueue[0], this.DisplayQueueArrow[0]));
  }

  public IEnumerator ShowIcon(Sprite Sprite, GameObject Arrow)
  {
    this.HideAll();
    this.Container.SetActive(true);
    Arrow.SetActive(true);
    this.Icon.sprite = Sprite;
    this.Container.transform.localPosition = Vector3.zero;
    float Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.Container.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(2.5f);
    Progress = 0.0f;
    Duration = 0.1f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.Container.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.SmoothStep(1f, 0.0f, Progress / Duration));
      yield return (object) null;
    }
    this.Container.SetActive(false);
    this.DisplayQueue.RemoveAt(0);
    this.DisplayQueueArrow.RemoveAt(0);
    this.PlayQueue();
  }
}
