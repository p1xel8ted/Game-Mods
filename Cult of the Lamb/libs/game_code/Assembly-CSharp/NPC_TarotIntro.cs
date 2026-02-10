// Decompiled with JetBrains decompiler
// Type: NPC_TarotIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class NPC_TarotIntro : BaseMonoBehaviour
{
  public Interaction_SimpleConversation ConversationIntro;
  public Interaction_SimpleConversation ConversationGivenCards;
  public Transform SpawnPosition;
  public Sprite TarotCardSprite;

  public void Start()
  {
    this.ConversationIntro.enabled = true;
    this.ConversationGivenCards.enabled = false;
  }

  public void GiveCards()
  {
  }

  public IEnumerator GiveCardsRoutine()
  {
    NPC_TarotIntro npcTarotIntro = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
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

  public void ContinueGiveCards()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.ConversationGivenCards.enabled = true;
  }
}
