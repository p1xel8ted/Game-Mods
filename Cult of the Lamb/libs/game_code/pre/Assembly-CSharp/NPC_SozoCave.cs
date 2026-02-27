// Decompiled with JetBrains decompiler
// Type: NPC_SozoCave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class NPC_SozoCave : BaseMonoBehaviour
{
  public Interaction_SimpleConversation ConversationQuestion;
  public Interaction_SimpleConversation ConversationNotEnough;
  public Interaction_SimpleConversation ConversationYes;
  public Interaction_SimpleConversation ConversationNo;

  private void OnEnable() => this.SetConversations();

  public void SetConversations()
  {
    this.ConversationNo.enabled = false;
    this.ConversationYes.enabled = false;
    if (Inventory.GetItemQuantity(29) >= 30)
    {
      this.ConversationQuestion.enabled = true;
      this.ConversationNotEnough.enabled = false;
    }
    else
    {
      this.ConversationQuestion.enabled = false;
      this.ConversationNotEnough.enabled = true;
    }
  }

  public void AnswerYes() => this.StartCoroutine((IEnumerator) this.AnswerYesRoutine());

  private IEnumerator AnswerYesRoutine()
  {
    NPC_SozoCave npcSozoCave = this;
    GameManager.GetInstance().OnConversationNew();
    int i = -1;
    while (++i < 30)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", npcSozoCave.ConversationQuestion.state.transform.position);
      ResourceCustomTarget.Create(npcSozoCave.gameObject, npcSozoCave.ConversationQuestion.state.transform.position, InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, new System.Action(npcSozoCave.RemoveMushroomFromInventort));
      yield return (object) new WaitForSeconds((float) (0.20000000298023224 - 0.10000000149011612 * (double) i / 20.0));
    }
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    npcSozoCave.ConversationYes.enabled = true;
  }

  private void RemoveMushroomFromInventort() => Inventory.ChangeItemQuantity(29, -1);

  public void QuestComplete()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BringSozoMushrooms);
    DataManager.Instance.SetVariable(DataManager.Variables.SozoQuestComplete, true);
  }

  public void AnswerNo() => this.ConversationNo.enabled = true;
}
