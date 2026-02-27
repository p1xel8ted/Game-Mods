// Decompiled with JetBrains decompiler
// Type: Interaction_CoinGamble
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
public class Interaction_CoinGamble : Interaction
{
  [SerializeField]
  private GameObject cameraBone;
  [SerializeField]
  private Health[] goldSacks;
  private const int maxRolls = 3;
  private int rolls;
  private float costMultiplier = 1f;
  private string resultText;
  public int goldSacksCacheLength;

  private int cost => Mathf.RoundToInt(20f * this.costMultiplier);

  private void Start() => this.goldSacksCacheLength = this.goldSacks.Length;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.StartCoroutine((IEnumerator) this.CheckGoldSacks());
  }

  private IEnumerator CheckGoldSacks()
  {
    Interaction_CoinGamble interactionCoinGamble = this;
    while (true)
    {
      int num = 0;
      foreach (Health goldSack in interactionCoinGamble.goldSacks)
      {
        if ((UnityEngine.Object) goldSack == (UnityEngine.Object) null || (double) goldSack.HP <= 0.0)
          ++num;
      }
      if (interactionCoinGamble.goldSacksCacheLength != num)
      {
        Debug.Log((object) "Shaking");
        interactionCoinGamble.transform.DOKill();
        if (num != interactionCoinGamble.goldSacks.Length)
          interactionCoinGamble.transform.DOShakePosition(0.5f, Vector3.left * 0.1f);
        else
          interactionCoinGamble.transform.DOShakePosition(1f, Vector3.left * 0.5f);
        interactionCoinGamble.goldSacksCacheLength = num;
      }
      yield return (object) new WaitForSeconds(0.5f);
    }
  }

  private string GetAffordColor() => Inventory.GetItemQuantity(20) < this.cost ? "<color=red>" : "";

  public override void GetLabel()
  {
    this.Label = this.Interactable ? $"{ScriptLocalization.Interactions.MakeOffering} {CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, this.cost)}" : ScriptLocalization.Interactions.Recharging;
  }

  public override void OnInteract(StateMachine state)
  {
    if (Inventory.GetItemQuantity(20) >= this.cost)
    {
      base.OnInteract(state);
      this.Interactable = false;
      this.StartCoroutine((IEnumerator) this.GambleIE());
      this.costMultiplier += 0.5f;
      ++this.rolls;
    }
    else
      MonoSingleton<Indicator>.Instance.PlayShake();
  }

  private IEnumerator GambleIE()
  {
    Interaction_CoinGamble interactionCoinGamble = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionCoinGamble.cameraBone, 8f);
    yield return (object) new WaitForSeconds(0.5f);
    if (!DataManager.Instance.EncounteredGambleRoom)
    {
      DataManager.Instance.EncounteredGambleRoom = true;
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(interactionCoinGamble.cameraBone, "Interactions/CoinGamble/Message/0")
      }, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = 400f;
      yield return (object) new WaitForEndOfFrame();
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    float increment = 1f / (float) interactionCoinGamble.cost;
    for (int i = 0; i < interactionCoinGamble.cost; ++i)
    {
      Inventory.GetItemByType(20);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.Instance.gameObject.transform.position);
      ResourceCustomTarget.Create(interactionCoinGamble.gameObject, PlayerFarming.Instance.gameObject.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      Inventory.ChangeItemQuantity(20, -1);
      yield return (object) new WaitForSeconds(increment);
    }
    yield return (object) interactionCoinGamble.StartCoroutine((IEnumerator) interactionCoinGamble.Shake());
    yield return (object) interactionCoinGamble.StartCoroutine((IEnumerator) interactionCoinGamble.GiveReward());
    yield return (object) new WaitForSeconds(1f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(interactionCoinGamble.cameraBone, interactionCoinGamble.resultText)
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 400f;
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    if (interactionCoinGamble.rolls >= 3)
      interactionCoinGamble.Interactable = false;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationEnd();
  }

  private IEnumerator GiveReward()
  {
    Interaction_CoinGamble interactionCoinGamble = this;
    float num1 = Mathf.Clamp01(UnityEngine.Random.Range(0.0f, 1f) * DataManager.Instance.GetLuckMultiplier());
    bool interactable = true;
    int num2 = 0;
    foreach (Health goldSack in interactionCoinGamble.goldSacks)
    {
      if ((UnityEngine.Object) goldSack == (UnityEngine.Object) null || (double) goldSack.HP <= 0.0)
        ++num2;
    }
    int amount;
    int i;
    if ((double) num2 / (double) interactionCoinGamble.goldSacks.Length > 0.60000002384185791)
    {
      AudioManager.Instance.PlayOneShot("event:/Stings/gamble_lose", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Failure, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      interactionCoinGamble.resultText = LocalizationManager.GetTranslation("Interactions/CoinGamble/0");
      interactable = false;
    }
    else if ((double) num1 <= 0.40000000596046448)
    {
      yield return (object) new WaitForSeconds(0.5f);
      amount = UnityEngine.Random.Range(5, 50);
      if (amount > 20)
      {
        AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", PlayerFarming.Instance.transform.position);
        MMVibrate.Haptic(MMVibrate.HapticTypes.Success, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
        BiomeConstants.Instance.EmitConfettiVFX(interactionCoinGamble.transform.position);
      }
      else
      {
        AudioManager.Instance.PlayOneShot("event:/Stings/gamble_lose", PlayerFarming.Instance.transform.position);
        MMVibrate.Haptic(MMVibrate.HapticTypes.Failure, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      }
      float increment = 1f / (float) amount;
      string translation = LocalizationManager.GetTranslation("Interactions/CoinGamble/1");
      interactionCoinGamble.resultText = string.Format(translation, (object) $"<color=#FFD201>x{amount.ToString()}{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.BLACK_GOLD)}");
      for (i = 0; i < amount; ++i)
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionCoinGamble.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(3f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        yield return (object) new WaitForSeconds(increment);
      }
    }
    else if ((double) num1 <= 0.60000002384185791)
    {
      amount = UnityEngine.Random.Range(7, 12);
      i = UnityEngine.Random.Range(7, 12);
      string translation = LocalizationManager.GetTranslation("Interactions/CoinGamble/1");
      interactionCoinGamble.resultText = string.Format(translation, (object) $"<color=#FFD201>{amount.ToString()}{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.LOG)}");
      interactionCoinGamble.resultText = $"{interactionCoinGamble.resultText} <color=#FFD201>{i.ToString()}{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.STONE)}";
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      BiomeConstants.Instance.EmitConfettiVFX(interactionCoinGamble.transform.position);
      for (int index = 0; index < amount; ++index)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LOG, 1, interactionCoinGamble.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(3f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      for (int index = 0; index < i; ++index)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.STONE, 1, interactionCoinGamble.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(3f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    }
    else if ((double) num1 <= 0.9)
    {
      interactionCoinGamble.resultText = LocalizationManager.GetTranslation("Interactions/CoinGamble/2");
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      BiomeConstants.Instance.EmitConfettiVFX(interactionCoinGamble.transform.position);
      for (int index = 0; index < 15; ++index)
      {
        InventoryItem.ITEM_TYPE[] itemTypeArray = new InventoryItem.ITEM_TYPE[4]
        {
          InventoryItem.ITEM_TYPE.BERRY,
          InventoryItem.ITEM_TYPE.FISH,
          InventoryItem.ITEM_TYPE.MEAT,
          InventoryItem.ITEM_TYPE.PUMPKIN
        };
        InventoryItem.Spawn(itemTypeArray[UnityEngine.Random.Range(0, itemTypeArray.Length)], 1, interactionCoinGamble.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(3f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      }
    }
    else if ((double) num1 <= 0.949999988079071 && DataManager.CheckIfThereAreSkinsAvailable())
    {
      interactionCoinGamble.resultText = LocalizationManager.GetTranslation("Interactions/CoinGamble/4");
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      BiomeConstants.Instance.EmitConfettiVFX(interactionCoinGamble.transform.position);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN, 1, interactionCoinGamble.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(3f, 270f);
    }
    else if ((double) num1 <= 1.0)
    {
      interactionCoinGamble.resultText = LocalizationManager.GetTranslation("Interactions/CoinGamble/5");
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      BiomeConstants.Instance.EmitConfettiVFX(interactionCoinGamble.transform.position);
      for (i = 0; i < UnityEngine.Random.Range(5, 7); ++i)
      {
        InventoryItem.Spawn(UnityEngine.Random.Range(0, 2) == 0 ? InventoryItem.ITEM_TYPE.BLUE_HEART : InventoryItem.ITEM_TYPE.HALF_BLUE_HEART, 1, interactionCoinGamble.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(3f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f));
      }
    }
    interactionCoinGamble.Interactable = interactable;
  }

  private IEnumerator Shake()
  {
    Interaction_CoinGamble interactionCoinGamble = this;
    AudioManager.Instance.PlayOneShot("event:/locations/light_house/fireplace_shake", interactionCoinGamble.transform.position);
    Vector3 pos = interactionCoinGamble.transform.position;
    float progress = 0.0f;
    float duration = 2f;
    while ((double) progress < (double) duration)
    {
      interactionCoinGamble.transform.position = pos + (Vector3) UnityEngine.Random.insideUnitCircle * progress * 0.05f;
      progress = Mathf.Clamp(progress + Time.unscaledDeltaTime, 0.0f, duration);
      yield return (object) null;
    }
    while ((double) progress > 0.0)
    {
      interactionCoinGamble.transform.position = pos + (Vector3) UnityEngine.Random.insideUnitCircle * progress * 0.05f;
      progress = Mathf.Clamp(progress - Time.unscaledDeltaTime * 3f, 0.0f, duration);
      yield return (object) null;
    }
    interactionCoinGamble.transform.position = pos;
  }
}
