// Decompiled with JetBrains decompiler
// Type: Interaction_EntrySignPost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_EntrySignPost : Interaction
{
  [SerializeField]
  private GameObject cameraOffset;
  [SerializeField]
  private FollowerLocation Location;
  [SerializeField]
  private SpriteRenderer SpriteFront;
  [SerializeField]
  private SpriteRenderer SpriteBack;
  private bool LocationHasBeenSet;
  public List<Interaction_EntrySignPost.LocationAndSprite> LocationAndSprites = new List<Interaction_EntrySignPost.LocationAndSprite>();
  private string sLabel;
  private string sBrokenLabel;
  private bool BrokenSign;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if (GameManager.CurrentDungeonFloor != 1)
      this.gameObject.SetActive(false);
    if (this.LocationHasBeenSet)
      return;
    LocationManager.OnPlayerLocationSet += new System.Action(this.OnPlayerLocationSet);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
  }

  private void OnPlayerLocationSet()
  {
    this.LocationHasBeenSet = true;
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
    foreach (Interaction_EntrySignPost.LocationAndSprite locationAndSprite in this.LocationAndSprites)
    {
      if (locationAndSprite.Location == PlayerFarming.Location)
      {
        this.SpriteFront.sprite = locationAndSprite.Sprite;
        this.SpriteBack.sprite = locationAndSprite.Sprite;
        break;
      }
    }
    if (!DataManager.Instance.SignPostsRead.Contains(PlayerFarming.Location))
      return;
    this.gameObject.SetActive(false);
  }

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation() => this.sLabel = ScriptLocalization.Interactions.Read;

  public override void GetLabel()
  {
    if (this.BrokenSign)
    {
      this.Label = "";
    }
    else
    {
      if (string.IsNullOrEmpty(this.sLabel))
        this.UpdateLocalisation();
      this.Label = this.sLabel;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!DataManager.Instance.SignPostsRead.Contains(PlayerFarming.Location))
      DataManager.Instance.SignPostsRead.Add(PlayerFarming.Location);
    if (!DataManager.Instance.SignPostsRead.Contains(PlayerFarming.Location))
      DataManager.Instance.SignPostsRead.Add(PlayerFarming.Location);
    string Term = "";
    switch (this.Location)
    {
      case FollowerLocation.Dungeon1_1:
        Term = ScriptLocalization.NAMES_CultLeaders.Dungeon1;
        break;
      case FollowerLocation.Dungeon1_2:
        Term = ScriptLocalization.NAMES_CultLeaders.Dungeon2;
        break;
      case FollowerLocation.Dungeon1_3:
        Term = ScriptLocalization.NAMES_CultLeaders.Dungeon3;
        break;
      case FollowerLocation.Dungeon1_4:
        Term = ScriptLocalization.NAMES_CultLeaders.Dungeon4;
        break;
    }
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(this.cameraOffset, ScriptLocalization.Conversation_NPC_NewDungeonSign._0)
    };
    Entries[0].TermToSpeak = string.Format(LocalizationManager.GetTranslation(ScriptLocalization.Conversation_NPC_NewDungeonSign._0), (object) $"<color=#FFD201>{LocalizationManager.GetTranslation(Term)}</color>");
    if (this.BrokenSign)
    {
      string termToSpeak = Entries[0].TermToSpeak;
      int startIndex = termToSpeak.IndexOf("will be");
      Entries[0].TermToSpeak = "..." + termToSpeak.Substring(startIndex);
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), Translate: false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 350f;
  }

  public void OnDie()
  {
    foreach (Interaction_EntrySignPost.LocationAndSprite locationAndSprite in this.LocationAndSprites)
    {
      if (locationAndSprite.Location == PlayerFarming.Location)
        locationAndSprite.Rubble.SetActive(true);
    }
    this.BrokenSign = true;
  }

  [Serializable]
  public class LocationAndSprite
  {
    public FollowerLocation Location;
    public Sprite Sprite;
    public GameObject Rubble;
  }
}
