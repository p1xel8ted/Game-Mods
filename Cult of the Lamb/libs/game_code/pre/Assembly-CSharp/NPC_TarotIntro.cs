// Decompiled with JetBrains decompiler
// Type: NPC_TarotIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class NPC_TarotIntro : BaseMonoBehaviour
{
  public Interaction_SimpleConversation ConversationIntro;
  public Interaction_SimpleConversation ConversationGivenCards;
  public Transform SpawnPosition;
  public Sprite TarotCardSprite;

  private void Start()
  {
    this.ConversationIntro.enabled = true;
    this.ConversationGivenCards.enabled = false;
  }

  public void GiveCards()
  {
  }

  private IEnumerator GiveCardsRoutine()
  {
    NPC_TarotIntro npcTarotIntro = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: true);
    GameManager.GetInstance().OnConversationNext(npcTarotIntro.SpawnPosition.gameObject, 10f);
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) new WaitForSeconds(0.5f);
    GameObject Player = GameObject.FindWithTag("Player");
    int i = -1;
    while (++i < 3)
    {
      if (i == 2)
        ResourceCustomTarget.Create(Player, npcTarotIntro.SpawnPosition.position, npcTarotIntro.TarotCardSprite, new System.Action(npcTarotIntro.ContinueGiveCards));
      else
        ResourceCustomTarget.Create(Player, npcTarotIntro.SpawnPosition.position, npcTarotIntro.TarotCardSprite, (System.Action) null);
      yield return (object) new WaitForSeconds(0.5f);
    }
  }

  private void ContinueGiveCards()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.ConversationGivenCards.enabled = true;
  }
}
