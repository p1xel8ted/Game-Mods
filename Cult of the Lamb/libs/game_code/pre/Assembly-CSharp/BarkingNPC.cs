// Decompiled with JetBrains decompiler
// Type: BarkingNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class BarkingNPC : MonoBehaviour
{
  public FollowerLocation Location;
  private SkeletonAnimation spine;
  private SimpleBarkRepeating simpleBarkRepeating;
  private CritterBee critter;

  private void Awake()
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
    if (DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_2) && this.Location == FollowerLocation.Hub1_Sozo)
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/Mushrooms"));
    if (DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_3))
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.spine.gameObject, "Conversation_NPC/Gossip/Midas"));
    foreach (ConversationEntry entry in this.simpleBarkRepeating.Entries)
    {
      entry.CharacterName = this.simpleBarkRepeating.Entries[0].CharacterName;
      entry.soundPath = this.simpleBarkRepeating.Entries[0].soundPath;
    }
  }

  private void Update() => this.critter.CanMove = !this.simpleBarkRepeating.IsSpeaking;
}
