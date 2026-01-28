// Decompiled with JetBrains decompiler
// Type: Interaction_GambleStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_GambleStatue : Interaction
{
  public bool Activating;
  public GameObject PayResourceReceivePosition;
  public int Cost = 5;
  public List<GameObject> ResourceListLeft;
  public List<GameObject> ResourceListRight;
  public List<InventoryItem.ITEM_TYPE> RandomResourceTypes = new List<InventoryItem.ITEM_TYPE>();
  public InventoryItem.ITEM_TYPE ResourceType = InventoryItem.ITEM_TYPE.LOG;
  public CameraInclude CameraInclude;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "Spine", true, false)]
  public string IdleAnimation;
  [SpineAnimation("", "Spine", true, false)]
  public string FailedAnimation;
  [SpineAnimation("", "Spine", true, false)]
  public string SingleRewardAnimation;
  [SpineAnimation("", "Spine", true, false)]
  public string SingleRewardAnimationLoop;
  [SpineAnimation("", "Spine", true, false)]
  public string DoubleRewardAnimation;
  [SpineAnimation("", "Spine", true, false)]
  public string DoubleRewardAnimationLoop;
  [SpineAnimation("", "Spine", true, false)]
  public string CalculatingLoop;
  public string sString;

  public void Start() => this.SetResources();

  public void SetResources()
  {
    this.ResourceType = this.RandomResourceTypes[UnityEngine.Random.Range(0, this.RandomResourceTypes.Count)];
    foreach (GameObject gameObject in this.ResourceListLeft)
      gameObject.GetComponent<InventoryItemDisplay>().SetImage(this.ResourceType);
    foreach (GameObject gameObject in this.ResourceListRight)
      gameObject.GetComponent<InventoryItemDisplay>().SetImage(this.ResourceType);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.MakeOffering;
  }

  public override void GetLabel()
  {
    this.Label = this.Activating ? "" : $"{this.sString} {CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.SOUL, this.Cost)}";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.Souls > this.Cost)
      this.StartCoroutine((IEnumerator) this.PayDevotion());
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator PayDevotion()
  {
    Interaction_GambleStatue interactionGambleStatue = this;
    interactionGambleStatue.Activating = true;
    interactionGambleStatue.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    interactionGambleStatue.playerFarming.simpleSpineAnimator.Animate("devotion/devotion-start", 0, false);
    interactionGambleStatue.playerFarming.simpleSpineAnimator.AddAnimate("devotion/devotion-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    float SoulsToGive = (float) interactionGambleStatue.Cost;
    float NumSouls = 0.0f;
    while ((double) ++NumSouls <= (double) SoulsToGive)
    {
      if ((double) NumSouls == (double) SoulsToGive)
      {
        SoulCustomTarget.Create(interactionGambleStatue.PayResourceReceivePosition, interactionGambleStatue.playerFarming.CameraBone.transform.position, Color.white, new System.Action(interactionGambleStatue.\u003CPayDevotion\u003Eb__22_0));
        yield return (object) new WaitForSeconds(0.5f);
      }
      else
      {
        SoulCustomTarget.Create(interactionGambleStatue.PayResourceReceivePosition, interactionGambleStatue.playerFarming.CameraBone.transform.position, Color.white, (System.Action) null);
        yield return (object) new WaitForSeconds((float) (0.10000000149011612 - 0.10000000149011612 * ((double) NumSouls / (double) SoulsToGive)));
      }
    }
    interactionGambleStatue.playerFarming.simpleSpineAnimator.Animate("devotion/devotion-stop", 0, false);
    interactionGambleStatue.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    interactionGambleStatue.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void Gamble() => this.StartCoroutine((IEnumerator) this.GambleRoutine());

  public IEnumerator GambleRoutine()
  {
    Interaction_GambleStatue interactionGambleStatue = this;
    interactionGambleStatue.CameraInclude.enabled = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionGambleStatue.PayResourceReceivePosition, 7f);
    interactionGambleStatue.Spine.AnimationState.SetAnimation(0, interactionGambleStatue.CalculatingLoop, true);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.3f, 1.5f);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.shakeCamera(0.5f);
    interactionGambleStatue.Spine.AnimationState.SetAnimation(0, interactionGambleStatue.IdleAnimation, true);
    yield return (object) new WaitForSeconds(1f);
    switch (UnityEngine.Random.Range(0, 4))
    {
      case 0:
      case 1:
        interactionGambleStatue.StartCoroutine((IEnumerator) interactionGambleStatue.FailRoutine());
        break;
      case 2:
        interactionGambleStatue.StartCoroutine((IEnumerator) interactionGambleStatue.SingleRewardRoutine());
        break;
      case 3:
        interactionGambleStatue.StartCoroutine((IEnumerator) interactionGambleStatue.DoubleRewardRoutine());
        break;
    }
  }

  public IEnumerator SingleRewardRoutine()
  {
    this.Spine.AnimationState.SetAnimation(0, this.SingleRewardAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.SingleRewardAnimationLoop, true, 0.0f);
    yield return (object) new WaitForSeconds(0.4f);
    GameManager.GetInstance().OnConversationEnd();
    foreach (GameObject gameObject in this.ResourceListLeft)
    {
      gameObject.SetActive(false);
      InventoryItem.Spawn(this.ResourceType, 1, gameObject.transform.position + new Vector3(0.0f, -0.1f, 0.0f), 0.0f);
      yield return (object) new WaitForSeconds(0.05f);
    }
    yield return (object) new WaitForSeconds(0.5f);
  }

  public IEnumerator DoubleRewardRoutine()
  {
    this.Spine.AnimationState.SetAnimation(0, this.DoubleRewardAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.DoubleRewardAnimationLoop, true, 0.0f);
    yield return (object) new WaitForSeconds(0.4f);
    GameManager.GetInstance().OnConversationEnd();
    int i = -1;
    while (++i < this.ResourceListLeft.Count)
    {
      this.ResourceListLeft[i].SetActive(false);
      InventoryItem.Spawn(this.ResourceType, 1, this.ResourceListLeft[i].transform.position + new Vector3(0.0f, -0.1f, 0.0f), 0.0f);
      this.ResourceListRight[i].SetActive(false);
      InventoryItem.Spawn(this.ResourceType, 1, this.ResourceListRight[i].transform.position + new Vector3(0.0f, -0.1f, 0.0f), 0.0f);
      yield return (object) new WaitForSeconds(0.05f);
    }
    yield return (object) new WaitForSeconds(0.5f);
  }

  public IEnumerator FailRoutine()
  {
    ++this.Cost;
    this.Spine.AnimationState.SetAnimation(0, this.FailedAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.IdleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    this.Activating = false;
    GameManager.GetInstance().OnConversationEnd();
    this.CameraInclude.enabled = true;
  }

  [CompilerGenerated]
  public void \u003CPayDevotion\u003Eb__22_0() => this.Gamble();
}
