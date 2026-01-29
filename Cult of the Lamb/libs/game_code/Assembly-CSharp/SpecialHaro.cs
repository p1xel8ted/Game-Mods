// Decompiled with JetBrains decompiler
// Type: SpecialHaro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpecialHaro : MonoBehaviour
{
  [SerializeField]
  public Interaction_SimpleConversation[] conversations;

  public void Awake()
  {
    for (int index = 0; index < this.conversations.Length; ++index)
      this.conversations[index].gameObject.SetActive(DataManager.Instance.SpecialHaroConversationIndex == index);
    DataManager.Instance.ShowSpecialHaroRoom = false;
  }

  public void IncrementVariable()
  {
    DataManager.Instance.HaroSpecialEncounteredLocations.Add(PlayerFarming.Location);
    ++DataManager.Instance.SpecialHaroConversationIndex;
  }
}
