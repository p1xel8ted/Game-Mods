// Decompiled with JetBrains decompiler
// Type: DeadWorshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DeadWorshipper : BaseMonoBehaviour
{
  public static List<DeadWorshipper> DeadWorshippers = new List<DeadWorshipper>();
  public SkeletonAnimation Spine;
  public Structure Structure;
  public ParticleSystem RottenParticles;
  public FollowerInfo followerInfo;
  public GameObject[] Flowers;
  public GameObject ItemIndicator;
  public bool PlayAnimation;
  public string DeadAnimation = "dead";

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  private void Data() => Debug.Log((object) ("PlayAnimation " + this.PlayAnimation.ToString()));

  private void Start()
  {
    DeadWorshipper.DeadWorshippers.Add(this);
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.StructureModified);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.StructureModified);
  }

  private void OnEnable()
  {
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.HideBody();
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  private void OnDestroy()
  {
    DeadWorshipper.DeadWorshippers.Remove(this);
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.StructureModified);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.StructureModified);
  }

  private void OnBrainAssigned()
  {
    Debug.Log((object) "Brain assigned");
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.Spine.Initialize(false);
    this.Setup();
  }

  private void StructureModified(StructuresData s)
  {
    if (s == null || this.StructureInfo == null || s.FollowerID != this.StructureInfo.FollowerID)
      return;
    if (s.ID == this.StructureInfo.ID)
    {
      foreach (Structures_Prison structuresPrison in StructureManager.GetAllStructuresOfType<Structures_Prison>())
      {
        if (structuresPrison.Data.FollowerID == this.StructureInfo.FollowerID)
          structuresPrison.Data.FollowerID = -1;
      }
    }
    else
    {
      this.DeadAnimation = "dead";
      this.StructureInfo.Animation = this.DeadAnimation;
      s.FollowerID = -1;
      this.Setup();
    }
  }

  public void Setup()
  {
    Debug.Log((object) ("Set up ID: " + (object) this.StructureInfo.FollowerID));
    this.followerInfo = FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID);
    Debug.Log((object) ("Dead follower: " + (object) this.followerInfo));
    if (this.followerInfo == null)
    {
      this.followerInfo = FollowerManager.FindFollowerInfo(this.StructureInfo.FollowerID);
      Debug.Log((object) ("Living follower: " + (object) this.followerInfo));
    }
    if (this.StructureInfo.FollowerID == -1 || this.followerInfo == null)
    {
      this.gameObject.SetActive(false);
      StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.StructureModified);
      StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.StructureModified);
      StructureManager.RemoveStructure(this.Structure.Brain);
    }
    else
    {
      this.SetOutfit();
      this.Spine.skeleton.ScaleX = (float) this.StructureInfo.Dir;
      this.gameObject.name = "dead body " + this.followerInfo.Name;
      if (this.StructureInfo.Animation != "" && this.StructureInfo.Animation != "dead")
        this.DeadAnimation = this.StructureInfo.Animation;
      this.StructureInfo.Animation = this.DeadAnimation;
      if (this.PlayAnimation)
      {
        this.Spine.AnimationState.SetAnimation(0, "die", false);
        this.Spine.AnimationState.End += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_End);
        this.Spine.AnimationState.AddAnimation(0, this.DeadAnimation, true, 0.0f);
      }
      else if (this.StructureInfo.BodyWrapped)
      {
        this.Spine.AnimationState.SetAnimation(0, "corpse", true);
        this.WrapBody();
      }
      else if (this.StructureInfo.Rotten)
      {
        Debug.Log((object) "Set Body Rotten");
        this.Spine.AnimationState.SetAnimation(0, this.DeadAnimation + "-rotten", true);
        this.RottenParticles.gameObject.SetActive(true);
      }
      else
      {
        Debug.Log((object) "Set Body normal dead");
        this.Spine.AnimationState.SetAnimation(0, this.DeadAnimation, true);
        this.RottenParticles.gameObject.SetActive(false);
      }
      this.ShowBody();
    }
  }

  public void SetOutfit()
  {
    if (this.followerInfo == null || (UnityEngine.Object) this.Spine == (UnityEngine.Object) null || this.Spine.Skeleton == null)
      return;
    FollowerOutfit followerOutfit = new FollowerOutfit(this.followerInfo);
    Skin newSkin = new Skin("New Skin");
    Skin skin = this.Spine.Skeleton.Data.FindSkin(this.followerInfo.SkinName);
    if (skin != null)
    {
      newSkin.AddSkin(skin);
    }
    else
    {
      newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Cat"));
      this.followerInfo.SkinName = "Cat";
    }
    int outfit = (int) this.followerInfo.Outfit;
    string outfitSkinName = followerOutfit.GetOutfitSkinName((FollowerOutfitType) outfit);
    if (!string.IsNullOrEmpty(outfitSkinName))
      newSkin.AddSkin(this.Spine.skeleton.Data.FindSkin(outfitSkinName));
    if (this.followerInfo.Necklace != InventoryItem.ITEM_TYPE.NONE)
      newSkin.AddSkin(this.Spine.skeleton.Data.FindSkin("Necklaces/" + this.followerInfo.Necklace.ToString()));
    this.Spine.Skeleton.SetSkin(newSkin);
    this.Spine.skeleton.SetSlotsToSetupPose();
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(this.followerInfo.SkinName);
    if (colourData == null)
      return;
    foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(this.followerInfo.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
    {
      Slot slot = this.Spine.skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  public void SetOutfit(FollowerOutfitType outfit)
  {
    FollowerOutfit followerOutfit = new FollowerOutfit(this.followerInfo);
    if (this.followerInfo.CursedState == Thought.OldAge)
      followerOutfit.SetOutfit(this.Spine, FollowerOutfitType.Old, InventoryItem.ITEM_TYPE.NONE, false);
    else
      followerOutfit.SetOutfit(this.Spine, outfit, InventoryItem.ITEM_TYPE.NONE, false);
  }

  private void OnNewDayStarted()
  {
    this.Spine.enabled = false;
    this.StartCoroutine((IEnumerator) this.WaitRotten());
  }

  private IEnumerator WaitRotten()
  {
    yield return (object) new WaitForSeconds(0.1f);
    if (this.StructureInfo.Rotten && !this.StructureInfo.BodyWrapped)
    {
      this.Spine.enabled = true;
      yield return (object) new WaitForEndOfFrame();
      this.SetRotten();
      yield return (object) new WaitForEndOfFrame();
      this.Spine.enabled = false;
    }
  }

  private IEnumerator DeactivateSpine()
  {
    yield return (object) new WaitForSeconds(2.5f);
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
      this.Spine.enabled = false;
  }

  private void SetRotten()
  {
    if (this.StructureInfo.BodyWrapped)
      return;
    Debug.Log((object) "Set Body Rotten");
    this.Spine.AnimationState.SetAnimation(0, this.DeadAnimation + "-rotten", true);
    this.RottenParticles.gameObject.SetActive(true);
  }

  public void WrapBody()
  {
    this.Spine.enabled = true;
    this.StructureInfo.BodyWrapped = true;
    this.Spine.AnimationState.SetAnimation(0, "corpse", true);
    this.RottenParticles.gameObject.SetActive(false);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.DeactivateSpine());
  }

  public void HideBody()
  {
    this.Spine.gameObject.SetActive(false);
    this.RottenParticles.gameObject.SetActive(false);
    this.ItemIndicator.SetActive(false);
  }

  public void ShowBody()
  {
    this.ItemIndicator.SetActive(true);
    this.Spine.gameObject.SetActive(true);
  }

  private void AnimationState_End(TrackEntry trackEntry)
  {
    this.PlayAnimation = false;
    foreach (Follower follower in Follower.Followers)
    {
      if ((double) Vector3.Distance(this.transform.position, this.transform.position) < 12.0 && follower.Brain.Location == this.StructureInfo.Location && (follower.Brain.CurrentTask == null || !follower.Brain.CurrentTask.BlockReactTasks))
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ReactCorpse(this.StructureInfo.ID));
    }
  }

  public void AddWorshipper() => DeadWorshipper.DeadWorshippers.Add(this);

  public void RemoveWorshipper() => DeadWorshipper.DeadWorshippers.Remove(this);

  private void OnDisable()
  {
    this.Spine.AnimationState.End -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_End);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }
}
