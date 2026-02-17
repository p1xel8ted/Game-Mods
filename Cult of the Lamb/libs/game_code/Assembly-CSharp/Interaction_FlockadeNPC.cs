// Decompiled with JetBrains decompiler
// Type: Interaction_FlockadeNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using I2.Loc;
using Lamb.UI;
using MMTools;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FlockadeNPC : Interaction
{
  [SerializeField]
  [TermsPopup("")]
  public string characterName;
  [SerializeField]
  public int hintCost = 20;
  [Space]
  [SerializeField]
  public SimpleBark newPiecesBark;
  [SerializeField]
  public SimpleBark collectedAllBark;
  [SerializeField]
  public bool TestFinalFlockadePiece;
  public string sRequires;
  public string sRemind;
  public bool onePieceLeft;

  public FlockadePieceType hintedPiece
  {
    get => DataManager.Instance.HintedPieceType;
    set => DataManager.Instance.HintedPieceType = value;
  }

  public bool didUnlockAllPieces => FlockadePieceManager.GetLockedPieceConfigurations().Count == 0;

  public bool canRemindHint
  {
    get
    {
      return !FlockadePieceManager.IsPieceUnlocked(DataManager.Instance.HintedPieceType) && DataManager.Instance.LastDayUsedFollowerShop == TimeManager.CurrentDay && !this.didUnlockAllPieces;
    }
  }

  public bool canPurchaseHint
  {
    get
    {
      return (DataManager.Instance.LastDayUsedFollowerShop != TimeManager.CurrentDay || CheatConsole.BuildingsFree) && !this.didUnlockAllPieces;
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sRequires = ScriptLocalization.Interactions.Requires;
    this.sRemind = ScriptLocalization.Interactions.RemindHint;
  }

  public override void GetLabel()
  {
    if (this.TestFinalFlockadePiece || FlockadePieceManager.GetLockedPieceConfigurations().Count == 1)
    {
      this.onePieceLeft = true;
      this.Label = "";
    }
    else if (this.canRemindHint)
      this.Label = this.sRemind;
    else if (this.canPurchaseHint)
    {
      this.SetBarkActive();
      this.Label = $"{this.sRequires} {LocalizeIntegration.ReverseText(this.hintCost.ToString())} <sprite name=\"icon_blackgold\">";
    }
    else
    {
      if (this.didUnlockAllPieces)
        this.SetAllPiecesUnlockedBark();
      else if (DataManager.Instance.HasNewFlockadePieces)
        this.SetNewPiecesUnlockedBark();
      this.Label = "";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.onePieceLeft)
      this.StartCoroutine((IEnumerator) this.RewardLastFlockadePiece());
    else if (this.canRemindHint)
    {
      this.ShowHint();
    }
    else
    {
      if (!this.canPurchaseHint)
        return;
      this.StartCoroutine((IEnumerator) this.Purchase());
    }
  }

  public void ShowHint()
  {
    this.CloseBark();
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, FlockadePieceManager.GetPiecesPool().GetPiece(this.hintedPiece).Hint)
    }, (List<MMTools.Response>) null, (System.Action) (() => GameManager.GetInstance().OnConversationEnd())), false);
  }

  public IEnumerator RewardLastFlockadePiece()
  {
    Interaction_FlockadeNPC interactionFlockadeNpc = this;
    interactionFlockadeNpc.Interactable = false;
    yield return (object) new WaitForSeconds(0.1f);
    interactionFlockadeNpc.CloseBark();
    FlockadePieceManager.GetPiecesPool().GetPiece(interactionFlockadeNpc.hintedPiece);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(interactionFlockadeNpc.gameObject, LocalizationManager.GetTranslation("LambwarNPC/CollectionComplete/0")),
      new ConversationEntry(interactionFlockadeNpc.gameObject, LocalizationManager.GetTranslation("LambwarNPC/CollectionComplete/1")),
      new ConversationEntry(interactionFlockadeNpc.gameObject, LocalizationManager.GetTranslation("LambwarNPC/CollectionComplete/2"))
    }, (List<MMTools.Response>) null, (System.Action) (() => GameManager.GetInstance().OnConversationEnd())), false);
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    yield return (object) interactionFlockadeNpc.GiveFlockadePiece(FlockadePieceType.ShepherdGolden);
    interactionFlockadeNpc.Interactable = true;
    interactionFlockadeNpc.onePieceLeft = false;
  }

  public IEnumerator GiveFlockadePiece(FlockadePieceType pieceType)
  {
    Interaction_FlockadeNPC interactionFlockadeNpc = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFlockadeNpc.gameObject, 4f);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionFlockadeNpc.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFlockadeNpc.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    FlockadePieceManager.UnlockPiece(pieceType);
    PlayerSimpleInventory component = interactionFlockadeNpc.state.gameObject.GetComponent<PlayerSimpleInventory>();
    Vector3 pieceTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y + 1.5f, -1f);
    interactionFlockadeNpc.state.CURRENT_STATE = StateMachine.State.FoundItem;
    float Timer = 0.0f;
    while ((double) (Timer += Time.unscaledDeltaTime) < 2.0)
    {
      interactionFlockadeNpc.transform.position = Vector3.Lerp(interactionFlockadeNpc.transform.position, pieceTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionFlockadeNpc.transform.position = pieceTargetPosition;
    yield return (object) new WaitForSeconds(0.5f);
    bool waiting = false;
    System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadFlockadePiecesAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => task.IsCompleted));
    UIFlockadePiecesMenuController piecesMenuController = MonoSingleton<UIManager>.Instance.FlockadePiecesMenuTemplate.Instantiate<UIFlockadePiecesMenuController>();
    piecesMenuController.Show(pieceType, interactionFlockadeNpc.playerFarming);
    piecesMenuController.OnHide = piecesMenuController.OnHide + (System.Action) (() => waiting = false);
    while (waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  public void SetBarkActive()
  {
    this.collectedAllBark.gameObject.SetActive(false);
    this.newPiecesBark.gameObject.SetActive(false);
  }

  public void SetAllPiecesUnlockedBark()
  {
    this.newPiecesBark.gameObject.SetActive(false);
    this.collectedAllBark.gameObject.SetActive(true);
  }

  public void SetNewPiecesUnlockedBark()
  {
    this.newPiecesBark.gameObject.SetActive(true);
    this.collectedAllBark.gameObject.SetActive(false);
  }

  public void CloseBark()
  {
    this.collectedAllBark.Close();
    this.collectedAllBark.gameObject.SetActive(false);
    this.collectedAllBark.gameObject.SetActive(false);
  }

  public IEnumerator Purchase()
  {
    Interaction_FlockadeNPC interactionFlockadeNpc = this;
    interactionFlockadeNpc.DrawLockedPiece();
    interactionFlockadeNpc.CloseBark();
    GameManager.GetInstance().OnConversationNew(true, true, interactionFlockadeNpc.playerFarming);
    for (int i = 0; i < interactionFlockadeNpc.hintCost; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionFlockadeNpc.transform.position);
      ResourceCustomTarget.Create(interactionFlockadeNpc.gameObject, interactionFlockadeNpc.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(0.1f);
    }
    Inventory.ChangeItemQuantity(20, -interactionFlockadeNpc.hintCost);
    interactionFlockadeNpc.ShowHint();
  }

  public void DrawLockedPiece()
  {
    DataManager.Instance.LastDayUsedFollowerShop = TimeManager.CurrentDay;
    List<FlockadeGamePieceConfiguration> pieceConfigurations = FlockadePieceManager.GetLockedPieceConfigurations();
    List<string> stringList = new List<string>();
    foreach (FlockadeGamePieceConfiguration pieceConfiguration in pieceConfigurations)
    {
      if (!string.IsNullOrEmpty(pieceConfiguration.Hint) && pieceConfiguration.Type != this.hintedPiece && !stringList.Contains(pieceConfiguration.Hint))
        stringList.Add(pieceConfiguration.Hint);
    }
    string str = stringList[UnityEngine.Random.Range(0, stringList.Count)];
    foreach (FlockadeGamePieceConfiguration pieceConfiguration in pieceConfigurations)
    {
      if (pieceConfiguration.Hint.Equals(str))
      {
        this.hintedPiece = pieceConfiguration.Type;
        break;
      }
    }
  }
}
