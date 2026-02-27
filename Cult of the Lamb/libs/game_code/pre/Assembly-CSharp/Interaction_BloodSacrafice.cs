// Decompiled with JetBrains decompiler
// Type: Interaction_BloodSacrafice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_BloodSacrafice : Interaction
{
  private int rewardIndex;
  public SpriteRenderer sprite;

  public override void GetLabel()
  {
    if (!this.Interactable)
      this.Label = ScriptLocalization.Interactions.Recharging;
    else
      this.Label = !(bool) (UnityEngine.Object) PlayerFarming.Instance || (double) PlayerFarming.Instance.health.HP <= 0.0 ? "" : ScriptLocalization.Interactions.BloodSacrafice;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.InteractIE());
  }

  private IEnumerator InteractIE()
  {
    Interaction_BloodSacrafice interactionBloodSacrafice = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", PlayerFarming.Instance.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 12f);
    PlayerFarming.Instance.CustomAnimation("pray", false);
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; i < 10; ++i)
    {
      SoulCustomTarget.Create(interactionBloodSacrafice.gameObject, PlayerFarming.Instance.transform.position, Color.red, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
    PlayerFarming.Instance.health.HP -= 2f;
    yield return (object) new WaitForSeconds(1f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(interactionBloodSacrafice.gameObject, "Interactions/BloodSacrafice/Message")
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", PlayerFarming.Instance.transform.position);
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(interactionBloodSacrafice.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("float-up-spin", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("floating-land-spin", 0, false, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    interactionBloodSacrafice.StartCoroutine((IEnumerator) interactionBloodSacrafice.GiveReward());
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", PlayerFarming.Instance.transform.position);
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
    HealthPlayer health = PlayerFarming.Instance.health as HealthPlayer;
    health.invincible = false;
    if ((double) health.HP + (double) health.BlueHearts + (double) health.SpiritHearts + (double) health.BlackHearts <= 0.0)
      PlayerFarming.Instance.health.DealDamage(1f, interactionBloodSacrafice.gameObject, interactionBloodSacrafice.transform.position, dealDamageImmediately: true);
  }

  private IEnumerator GiveReward()
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
      BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "strength", "strength");
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", PlayerFarming.Instance.transform.position);
    }
    else if ((double) num1 <= 0.60000002384185791)
    {
      interactionBloodSacrafice.rewardIndex = 2;
      yield return (object) new WaitForSeconds(1f);
      BiomeConstants.Instance.EmitHeartPickUpVFX(interactionBloodSacrafice.transform.position, 0.0f, "blue", "burst_big");
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", PlayerFarming.Instance.transform.position);
      yield return (object) new WaitForSeconds(3.5f);
      int num2 = UnityEngine.Random.Range(1, 4);
      PlayerFarming.Instance.health.BlueHearts += (float) (num2 * 2 * TrinketManager.GetHealthAmountMultiplier());
    }
    else if ((double) num1 <= 0.89999997615814209)
    {
      interactionBloodSacrafice.rewardIndex = 3;
      yield return (object) new WaitForSeconds(1f);
      BiomeConstants.Instance.EmitHeartPickUpVFX(interactionBloodSacrafice.transform.position, 0.0f, "black", "burst_big");
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", PlayerFarming.Instance.transform.position);
      yield return (object) new WaitForSeconds(3.5f);
      int num3 = UnityEngine.Random.Range(1, 4);
      PlayerFarming.Instance.health.BlackHearts += (float) (num3 * 2);
    }
    else if ((double) num1 <= 0.949999988079071)
    {
      interactionBloodSacrafice.rewardIndex = 4;
      yield return (object) new WaitForSeconds(1f);
      BiomeConstants.Instance.EmitHeartPickUpVFX(interactionBloodSacrafice.transform.position, 0.0f, "red", "burst_big");
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", PlayerFarming.Instance.transform.position);
      yield return (object) new WaitForSeconds(3.5f);
      int num4 = UnityEngine.Random.Range(1, 4);
      PlayerFarming.Instance.health.SpiritHearts += (float) (num4 * 2);
    }
    else if ((double) num1 <= 1.0)
    {
      interactionBloodSacrafice.rewardIndex = 5;
      yield return (object) new WaitForSeconds(0.5f);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, interactionBloodSacrafice.transform.position + Vector3.down * 2f, 0.0f);
    }
  }
}
