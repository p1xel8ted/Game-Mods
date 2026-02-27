// Decompiled with JetBrains decompiler
// Type: Interaction_PlayerDebuff
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public class Interaction_PlayerDebuff : Interaction
{
  private ColorGrading colorGrading;
  private TarotCards.TarotCard removedCard;

  protected override void OnEnable()
  {
    base.OnEnable();
    BiomeConstants.Instance.ppv.profile.TryGetSettings<ColorGrading>(out this.colorGrading);
    this.StartCoroutine((IEnumerator) this.DebuffIE());
  }

  private IEnumerator DebuffIE()
  {
    Interaction_PlayerDebuff interactionPlayerDebuff = this;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || PlayerFarming.Instance.GoToAndStopping || LetterBox.IsPlaying)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    interactionPlayerDebuff.FadeRedIn();
    PlayerFarming.Instance.GoToAndStop(interactionPlayerDebuff.transform.position);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPlayerDebuff.gameObject, 6f);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(interactionPlayerDebuff.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/pentagram_platform/pentagram_platform_start");
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "float-up", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "floating", true, 0.0f);
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
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "floating-land", false);
    AudioManager.Instance.PlayOneShot("event:/pentagram_platform/pentagram_platform_end");
    interactionPlayerDebuff.FadeRedAway();
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
  }

  private int GiveDebuff()
  {
    float num = UnityEngine.Random.Range(0.0f, 1f);
    HealthPlayer health = PlayerFarming.Instance.health as HealthPlayer;
    if ((double) num > 0.89999997615814209 && (double) health.HP > 0.0)
    {
      DataManager.Instance.RedHeartsTemporarilyRemoved += Mathf.RoundToInt(health.totalHP);
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
      DataManager.Instance.Followers_Demons_IDs.Clear();
      DataManager.Instance.Followers_Demons_Types.Clear();
      for (int index = Demon_Arrows.Demons.Count - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) Demon_Arrows.Demons[index]);
      return 6;
    }
    if ((double) num > 0.30000001192092896)
    {
      Inventory.ClearDungeonItems(false);
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
    if ((double) num <= 0.0 || (double) health.BlueHearts <= 0.0 && (double) health.SpiritHearts <= 0.0 && (double) health.BlackHearts <= 0.0)
      return this.GiveDebuff();
    HealthPlayer.LoseAllSpecialHearts();
    return 10;
  }

  public void FadeRedAway()
  {
    DOTween.To((DOGetter<Color>) (() => this.colorGrading.colorFilter.value), (DOSetter<Color>) (x => this.colorGrading.colorFilter.value = x), Color.white, 1f);
  }

  private void FadeRedIn()
  {
    DOTween.To((DOGetter<Color>) (() => this.colorGrading.colorFilter.value), (DOSetter<Color>) (x => this.colorGrading.colorFilter.value = x), new Color(1f, 0.25f, 0.25f, 1f), 2f);
  }
}
