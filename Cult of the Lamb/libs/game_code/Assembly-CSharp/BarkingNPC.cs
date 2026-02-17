// Decompiled with JetBrains decompiler
// Type: BarkingNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class BarkingNPC : MonoBehaviour
{
  public FollowerLocation Location;
  public SkeletonAnimation spine;
  public SimpleBarkRepeating simpleBarkRepeating;
  public CritterBee critter;

  public bool IsWinter => DataManager.Instance.CurrentSeason == SeasonsManager.Season.Winter;

  public void Awake()
  {
    this.spine = this.GetComponentInChildren<SkeletonAnimation>();
    this.simpleBarkRepeating = this.GetComponentInChildren<SimpleBarkRepeating>();
    this.critter = this.GetComponent<CritterBee>();
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/LeshyKilled"));
    else if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2))
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/HeketKilled"));
    else if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3))
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/KallamarKilled"));
    else if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/ShamuraKilled"));
    if (DataManager.Instance.BossesCompleted.Count == 1)
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/AnyKilled"));
    else if (DataManager.Instance.BossesCompleted.Count >= 2)
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/AnyKilled"));
    if (DataManager.Instance.BossesCompleted.Count >= 3)
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/PraiseLamb"));
    if (DataManager.Instance.Lighthouse_Lit)
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/LighthouseLit"));
    if ((DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_2) || DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2)) && this.Location == FollowerLocation.Hub1_Sozo)
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/Mushrooms"));
    if (DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_3) || DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3))
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/Midas"));
    if (this.Location == FollowerLocation.Hub1_Sozo && DataManager.Instance.SozoDead)
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/MushroomFollower/DeadSozoBark/" + Random.Range(1, 5).ToString()));
    Debug.Log((object) $"Pilgrim's Passage? {{IsWinter?:{this.IsWinter}, Location:{this.Location}, CurrentSeason:{DataManager.Instance.CurrentSeason}}}");
    if (this.Location == FollowerLocation.HubShore && this.IsWinter)
    {
      Debug.Log((object) "Adding winter barks for Pilgrim's Passage...");
      for (int index = 0; index < 3; ++index)
        this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, $"Conversation_NPC/Lighthouse/LighthouseFollowersChant/Winter_{index}"));
    }
    if (this.Location == FollowerLocation.Hub1_Sozo && this.IsWinter)
    {
      Debug.Log((object) "Adding winter barks for Spore Grotto...");
      for (int index = 0; index < 3; ++index)
        this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, $"Conversation_NPC/MushroomFollower/GenericBark/Winter/{index}"));
    }
    foreach (ConversationEntry entry in this.simpleBarkRepeating.Entries)
    {
      entry.CharacterName = this.simpleBarkRepeating.Entries[0].CharacterName;
      entry.soundPath = this.simpleBarkRepeating.Entries[0].soundPath;
    }
  }

  public void Update() => this.critter.CanMove = !this.simpleBarkRepeating.IsSpeaking;
}
