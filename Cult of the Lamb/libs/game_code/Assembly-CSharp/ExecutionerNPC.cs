// Decompiled with JetBrains decompiler
// Type: ExecutionerNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ExecutionerNPC : UnitObject
{
  public static ExecutionerNPC Instance;
  [SerializeField]
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleBarkRepeating pardonedBark;
  [SerializeField]
  public SimpleBarkRepeating damnedBark;
  [SerializeField]
  public GameObject[] graves;
  public SimpleBarkRepeating barkRepeating;
  public Coroutine pathingRoutine;
  public float directionTimer = 0.5f;
  public Vector3 previousPos;
  public bool convoPlaying;
  public float delay;

  public override void OnDisable()
  {
    base.OnDisable();
    this.StopPathfinding();
  }

  public void Start()
  {
    ExecutionerNPC.Instance = this;
    this.pardonedBark.gameObject.SetActive(!DataManager.Instance.ExecutionerDamned);
    this.damnedBark.gameObject.SetActive(DataManager.Instance.ExecutionerDamned);
    this.barkRepeating = DataManager.Instance.ExecutionerPardoned ? this.pardonedBark : this.damnedBark;
    if (DataManager.Instance.BeatenExecutioner)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    ExecutionerNPC.Instance = (ExecutionerNPC) null;
  }

  public override void Update()
  {
    base.Update();
    if (this.convoPlaying)
      return;
    if ((double) this.delay <= 0.0)
    {
      this.delay = 10f;
      if (this.barkRepeating.IsSpeaking && this.pathingRoutine != null)
      {
        this.Spine.AnimationState.SetAnimation(0, "idle_beaten", true);
        this.ClearPaths();
        this.StopCoroutine(this.pathingRoutine);
        this.pathingRoutine = (Coroutine) null;
      }
      else if (!this.barkRepeating.IsSpeaking && this.pathingRoutine == null && !MMConversation.isPlaying)
        this.pathingRoutine = this.StartCoroutine((IEnumerator) this.PathfindingIE());
    }
    else
      this.delay -= Time.deltaTime;
    this.directionTimer -= Time.deltaTime;
    if ((double) this.directionTimer <= 0.0)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.previousPos, this.transform.position);
      this.directionTimer = 0.5f;
    }
    this.previousPos = this.transform.position;
  }

  public IEnumerator PathfindingIE()
  {
    ExecutionerNPC executionerNpc = this;
    while (true)
    {
      executionerNpc.graves = ((IEnumerable<GameObject>) executionerNpc.graves).OrderByDescending<GameObject, float>(new Func<GameObject, float>(executionerNpc.\u003CPathfindingIE\u003Eb__15_0)).ToArray<GameObject>();
      int index = Mathf.RoundToInt(UnityEngine.Random.Range(0.0f, (float) (executionerNpc.graves.Length - 1) * 0.3f));
      executionerNpc.givePath(executionerNpc.graves[index].transform.position);
      executionerNpc.Spine.AnimationState.SetAnimation(0, "run-beaten", true);
      yield return (object) new WaitForSeconds(1f);
      while (executionerNpc.pathToFollow != null)
        yield return (object) null;
      executionerNpc.Spine.AnimationState.SetAnimation(0, "idle_beaten", true);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(6f, 10f));
    }
  }

  public void StopPathfinding()
  {
    if (this.pathingRoutine != null)
      this.StopCoroutine(this.pathingRoutine);
    this.pathingRoutine = (Coroutine) null;
    this.ClearPaths();
    this.Spine.AnimationState.SetAnimation(0, "idle_beaten", true);
  }

  [CompilerGenerated]
  public float \u003CPathfindingIE\u003Eb__15_0(GameObject x)
  {
    return Vector3.Distance(x.transform.position, this.transform.position);
  }
}
