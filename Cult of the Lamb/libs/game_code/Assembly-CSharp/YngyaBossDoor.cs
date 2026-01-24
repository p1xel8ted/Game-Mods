// Decompiled with JetBrains decompiler
// Type: YngyaBossDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class YngyaBossDoor : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation doorSpine;
  [SerializeField]
  public SkeletonAnimation yngyaShrineSpine;
  [SerializeField]
  public GameObject blockingCollider;
  public bool used;

  public void Awake()
  {
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_5))
    {
      this.doorSpine.AnimationState.SetAnimation(0, "open", true);
      this.blockingCollider.gameObject.SetActive(false);
    }
    this.yngyaShrineSpine.AnimationState.SetAnimation(0, Mathf.Clamp(SeasonsManager.WinterSeverity, 0, 3).ToString(), true);
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_5) || !collision.gameObject.CompareTag("Player") || this.used)
      return;
    this.used = true;
    GameManager.NewRun("", false);
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon Boss Yngya", 1f, "", new System.Action(this.FadeSave));
  }

  public void FadeSave() => SaveAndLoad.Save();
}
