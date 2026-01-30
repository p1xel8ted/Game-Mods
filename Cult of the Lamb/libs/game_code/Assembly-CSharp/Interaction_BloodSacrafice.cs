// Decompiled with JetBrains decompiler
// Type: Interaction_BloodSacrafice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_BloodSacrafice : Interaction
{
  public int rewardIndex;
  public SpriteRenderer sprite;

  public void Awake()
  {
    if (!PlayerFleeceManager.FleeceNoRedHeartsToUse())
      return;
    this.sprite.color = Color.black;
    this.Interactable = false;
  }

  public override void GetLabel()
  {
    if (!this.Interactable)
      this.Label = ScriptLocalization.Interactions.Recharging;
    else
      this.Label = !(bool) (UnityEngine.Object) this.playerFarming || (double) this.playerFarming.health.HP <= 0.0 ? "" : ScriptLocalization.Interactions.BloodSacrafice;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.InteractIE());
  }

  public IEnumerator InteractIE()
  {
    Interaction_BloodSacrafice interactionBloodSacrafice = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", interactionBloodSacrafice.playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBloodSacrafice.playerFarming.gameObject, 12f);
    interactionBloodSacrafice.playerFarming.CustomAnimation("pray", false);
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; i < 10; ++i)
    {
      SoulCustomTarget.Create(interactionBloodSacrafice.gameObject, interactionBloodSacrafice.playerFarming.transform.position, Color.red, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
    interactionBloodSacrafice.playerFarming.health.HP -= 2f;
    yield return (object) new WaitForSeconds(1f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(interactionBloodSacrafice.gameObject, "Interactions/BloodSacrafice/Message")
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", interactionBloodSacrafice.playerFarming.transform.position);
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(interactionBloodSacrafice.gameObject);
    interactionBloodSacrafice.playerFarming.simpleSpineAnimator.Animate("float-up-spin", 0, false);
    interactionBloodSacrafice.playerFarming.simpleSpineAnimator.AddAnimate("floating-land-spin", 0, false, 0.0f);
    interactionBloodSacrafice.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    interactionBloodSacrafice.StartCoroutine((IEnumerator) interactionBloodSacrafice.GiveReward());
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", interactionBloodSacrafice.playerFarming.transform.position);
    yield return (object) new WaitForSeconds(3.2f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(interactionBloodSacrafice.gameObject, "Interactions/BloodSacrafice/" + interactionBloodSacrafice.rewardIndex.ToString())
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    yield return (object) new WaitForEndOfFrame();
    interactionBloodSacrafice.sprite.DOColor(Color.black, 1f);
    while (MMConversation.isPlaying)
      yield return (object) null;
    interactionBloodSacrafice.Interactable = false;
    GameManager.GetInstance().OnConversationEnd();
    HealthPlayer health = interactionBloodSacrafice.playerFarming.health;
    health.invincible = false;
    if ((double) health.CurrentHP <= 0.0)
      interactionBloodSacrafice.playerFarming.health.DealDamage(1f, interactionBloodSacrafice.gameObject, interactionBloodSacrafice.transform.position, false, Health.AttackTypes.Melee, true, (Health.AttackFlags) 0);
  }

  public IEnumerator GiveReward()
  {
    Interaction_BloodSacrafice interactionBloodSacrafice = this;
    float num1 = Mathf.Clamp01(UnityEngine.Random.Range(0.0f, 1f) * DataManager.Instance.GetLuckMultiplier());
    if ((double) num1 <= 0.10000000149011612)
      interactionBloodSacrafice.rewardIndex = 0;
    if ((double) num1 <= 0.30000001192092896)
    {
      interactionBloodSacrafice.rewardIndex = 1;
      yield return (object) new WaitForSeconds(0.5f);
      DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL += 0.25f;
      BiomeConstants.Instance.EmitHeartPickUpVFX(interactionBloodSacrafice.playerFarming.CameraBone.transform.position, 0.0f, "strength", "strength");
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionBloodSacrafice.playerFarming.transform.position);
    }
    else if ((double) num1 <= 0.60000002384185791)
    {
      interactionBloodSacrafice.rewardIndex = 2;
      yield return (object) new WaitForSeconds(1f);
      BiomeConstants.Instance.EmitHeartPickUpVFX(interactionBloodSacrafice.transform.position, 0.0f, "blue", "burst_big");
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionBloodSacrafice.playerFarming.transform.position);
      yield return (object) new WaitForSeconds(3.5f);
      int num2 = UnityEngine.Random.Range(1, 4);
      interactionBloodSacrafice.playerFarming.health.BlueHearts += (float) (num2 * 2 * TrinketManager.GetHealthAmountMultiplier(interactionBloodSacrafice.playerFarming));
    }
    else if ((double) num1 <= 0.89999997615814209)
    {
      interactionBloodSacrafice.rewardIndex = 3;
      yield return (object) new WaitForSeconds(1f);
      BiomeConstants.Instance.EmitHeartPickUpVFX(interactionBloodSacrafice.transform.position, 0.0f, "black", "burst_big");
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionBloodSacrafice.playerFarming.transform.position);
      yield return (object) new WaitForSeconds(3.5f);
      int num3 = UnityEngine.Random.Range(1, 4);
      interactionBloodSacrafice.playerFarming.health.BlackHearts += (float) (num3 * 2);
    }
    else if ((double) num1 <= 0.949999988079071)
    {
      interactionBloodSacrafice.rewardIndex = 4;
      yield return (object) new WaitForSeconds(1f);
      BiomeConstants.Instance.EmitHeartPickUpVFX(interactionBloodSacrafice.transform.position, 0.0f, "red", "burst_big");
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionBloodSacrafice.playerFarming.transform.position);
      yield return (object) new WaitForSeconds(3.5f);
      int num4 = UnityEngine.Random.Range(1, 4);
      interactionBloodSacrafice.playerFarming.health.SpiritHearts += (float) (num4 * 2);
    }
    else if ((double) num1 <= 1.0)
    {
      interactionBloodSacrafice.rewardIndex = 5;
      yield return (object) new WaitForSeconds(0.5f);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, interactionBloodSacrafice.transform.position + Vector3.down * 2f, 0.0f);
    }
  }
}
