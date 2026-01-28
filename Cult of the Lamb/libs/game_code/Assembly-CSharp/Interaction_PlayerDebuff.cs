// Decompiled with JetBrains decompiler
// Type: Interaction_PlayerDebuff
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public class Interaction_PlayerDebuff : Interaction
{
  public ColorGrading colorGrading;
  public TarotCards.TarotCard removedCard;

  public override void OnEnable()
  {
    base.OnEnable();
    BiomeConstants.Instance.ppv.profile.TryGetSettings<ColorGrading>(out this.colorGrading);
    this.StartCoroutine((IEnumerator) this.DebuffIE());
  }

  public IEnumerator DebuffIE()
  {
    Interaction_PlayerDebuff interactionPlayerDebuff = this;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) interactionPlayerDebuff.playerFarming == (UnityEngine.Object) null || interactionPlayerDebuff.playerFarming.GoToAndStopping || LetterBox.IsPlaying || MMTransition.IsPlaying)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    interactionPlayerDebuff.FadeRedIn();
    interactionPlayerDebuff.playerFarming.GoToAndStop(interactionPlayerDebuff.transform.position);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPlayerDebuff.gameObject, 6f);
    while (interactionPlayerDebuff.playerFarming.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(interactionPlayerDebuff.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/pentagram_platform/pentagram_platform_start");
    interactionPlayerDebuff.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionPlayerDebuff.playerFarming.Spine.AnimationState.SetAnimation(0, "float-up", false);
    interactionPlayerDebuff.playerFarming.Spine.AnimationState.AddAnimation(0, "floating", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/door/cross_disappear", interactionPlayerDebuff.gameObject);
    string str1 = ScriptLocalization.NAMES_CultLeaders.Dungeon1;
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_2:
        str1 = ScriptLocalization.NAMES_CultLeaders.Dungeon2;
        break;
      case FollowerLocation.Dungeon1_3:
        str1 = ScriptLocalization.NAMES_CultLeaders.Dungeon3;
        break;
      case FollowerLocation.Dungeon1_4:
        str1 = ScriptLocalization.NAMES_CultLeaders.Dungeon4;
        break;
    }
    string TermToSpeak = string.Format(LocalizationManager.GetTranslation("Interactions/NegativeRoom/Message"), (object) str1);
    int num = interactionPlayerDebuff.GiveDebuff();
    string str2 = LocalizationManager.GetTranslation("Interactions/NegativeRoom/Debuff" + num.ToString());
    if (num == 2)
      str2 = string.Format(str2, (object) TarotCards.LocalisedName(interactionPlayerDebuff.removedCard.CardType, 0));
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(interactionPlayerDebuff.gameObject, TermToSpeak));
    Entries.Add(new ConversationEntry(interactionPlayerDebuff.gameObject, str2));
    Entries[0].Offset = new Vector3(0.0f, 3f, 0.0f);
    Entries[1].Offset = new Vector3(0.0f, 3f, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/pentagram_platform/pentagram_platform_curse");
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.isPlaying)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    interactionPlayerDebuff.playerFarming.Spine.AnimationState.SetAnimation(0, "floating-land", false);
    AudioManager.Instance.PlayOneShot("event:/pentagram_platform/pentagram_platform_end");
    interactionPlayerDebuff.FadeRedAway();
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public int GiveDebuff()
  {
    float num = UnityEngine.Random.Range(0.0f, 1f);
    HealthPlayer health = this.playerFarming.health;
    if ((double) num > 0.89999997615814209 && (double) health.HP > 0.0)
    {
      this.playerFarming.RedHeartsTemporarilyRemoved += Mathf.RoundToInt(health.totalHP);
      int hp = (int) health.HP;
      health.totalHP = 0.0f;
      health.BlueHearts += (float) hp;
      return 1;
    }
    if ((double) num > 0.699999988079071)
    {
      DataManager.Instance.ProjectileMoveSpeedMultiplier += 0.5f;
      return 3;
    }
    if ((double) num > 0.60000002384185791)
    {
      ++DataManager.Instance.EnemyModifiersChanceMultiplier;
      return 4;
    }
    if ((double) num > 0.5)
    {
      DataManager.Instance.EnemyHealthMultiplier += 0.5f;
      return 5;
    }
    if ((double) num > 0.40000000596046448 && DataManager.Instance.Followers_Demons_IDs.Count > 0)
    {
      if (DataManager.Instance.HealingLeshyQuestActive || DataManager.Instance.HealingHeketQuestActive || DataManager.Instance.HealingKallamarQuestActive || DataManager.Instance.HealingShamuraQuestActive)
        return 5;
      DataManager.Instance.Followers_Demons_IDs.Clear();
      DataManager.Instance.Followers_Demons_Types.Clear();
      for (int index = Demon_Arrows.Demons.Count - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) Demon_Arrows.Demons[index]);
      return 6;
    }
    if ((double) num > 0.30000001192092896)
    {
      Inventory.RemoveDungeonItemsFromInventory();
      return 7;
    }
    if ((double) num > 0.20000000298023224)
    {
      DataManager.Instance.DodgeDistanceMultiplier -= 0.5f;
      return 8;
    }
    if ((double) num > 0.10000000149011612)
    {
      ++DataManager.Instance.CurseFeverMultiplier;
      return 9;
    }
    if ((double) num <= 0.0 || (double) health.BlueHearts <= 0.0 && (double) health.SpiritHearts <= 0.0 && (double) health.BlackHearts <= 0.0 && (double) health.FireHearts <= 0.0 && (double) health.IceHearts <= 0.0 || DataManager.Instance.PlayerFleece == 5)
      return this.GiveDebuff();
    HealthPlayer.LoseAllSpecialHearts();
    return 10;
  }

  public void FadeRedAway()
  {
    DOTween.To((DOGetter<Color>) (() => this.colorGrading.colorFilter.value), (DOSetter<Color>) (x => this.colorGrading.colorFilter.value = x), Color.white, 1f);
  }

  public void FadeRedIn()
  {
    DOTween.To((DOGetter<Color>) (() => this.colorGrading.colorFilter.value), (DOSetter<Color>) (x => this.colorGrading.colorFilter.value = x), new Color(1f, 0.25f, 0.25f, 1f), 2f);
  }

  [CompilerGenerated]
  public Color \u003CFadeRedAway\u003Eb__5_0() => this.colorGrading.colorFilter.value;

  [CompilerGenerated]
  public void \u003CFadeRedAway\u003Eb__5_1(Color x) => this.colorGrading.colorFilter.value = x;

  [CompilerGenerated]
  public Color \u003CFadeRedIn\u003Eb__6_0() => this.colorGrading.colorFilter.value;

  [CompilerGenerated]
  public void \u003CFadeRedIn\u003Eb__6_1(Color x) => this.colorGrading.colorFilter.value = x;
}
