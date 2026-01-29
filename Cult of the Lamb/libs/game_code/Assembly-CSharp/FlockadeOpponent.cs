// Decompiled with JetBrains decompiler
// Type: FlockadeOpponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class FlockadeOpponent : MonoBehaviour
{
  [SerializeField]
  public FlockadeOpponentConfiguration config;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public Interaction_SimpleConversation firstWinConversation;
  [SerializeField]
  public Interaction_SimpleConversation winConversation;
  [SerializeField]
  public Interaction_SimpleConversation loseConversation;
  [SerializeField]
  public Interaction_SimpleConversation drawConversation;

  public FlockadeOpponentConfiguration Config => this.config;

  public SkeletonAnimation Spine => this.spine;

  public Interaction_SimpleConversation FirstWinConversation => this.firstWinConversation;

  public Interaction_SimpleConversation WinConversation => this.winConversation;

  public Interaction_SimpleConversation LoseConversation => this.loseConversation;

  public Interaction_SimpleConversation DrawConversation => this.drawConversation;
}
