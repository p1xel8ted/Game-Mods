// Decompiled with JetBrains decompiler
// Type: UIRecruitScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UIRecruitScreen : BaseMonoBehaviour
{
  public Animator Animator;
  public UINavigator uINavigator;
  private System.Action Callback;
  private Follower follower;
  public TextMeshProUGUI NameText;
  public TextMeshProUGUI FactionText;
  public TMP_InputField ChangeName;
  private readonly int NumSkins = 3;

  private void Start()
  {
    this.uINavigator.OnButtonDown += new UINavigator.ButtonDown(this.OnButtonDown);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-1.25f, 0.0f, 0.5f));
  }

  public void Play(System.Action Callback, Follower f)
  {
    this.Callback = Callback;
    this.follower = f;
    this.NameText.text = this.follower.Brain.Info.Name;
    this.FactionText.text = "Level " + this.follower.Brain.Info.XPLevel.ToNumeral();
    this.ChangeName.onValueChanged.AddListener((UnityAction<string>) (_param1 => this.OnNameChanged()));
  }

  public void OnNameChanged()
  {
    this.follower.Brain.Info.Name = this.ChangeName.text;
    this.NameText.text = this.follower.Brain.Info.Name;
  }

  public void NextSkin()
  {
    this.follower.Brain.Info.NextSkin();
    Skin skin = this.follower.Spine.Skeleton.Data.FindSkin(this.follower.Brain.Info.SkinName);
    string outfitSkinName = this.follower.Outfit.GetOutfitSkinName(this.follower.Brain.Info.Outfit);
    skin.AddSkin(this.follower.Spine.Skeleton.Data.FindSkin(outfitSkinName));
    this.follower.Spine.Skeleton.SetSkin(skin);
  }

  public void PrevSkin()
  {
    this.follower.Brain.Info.PrevSkin();
    Skin skin = this.follower.Spine.Skeleton.Data.FindSkin(this.follower.Brain.Info.SkinName);
    string outfitSkinName = this.follower.Outfit.GetOutfitSkinName(this.follower.Brain.Info.Outfit);
    skin.AddSkin(this.follower.Spine.Skeleton.Data.FindSkin(outfitSkinName));
    this.follower.Spine.Skeleton.SetSkin(skin);
  }

  public void NextColour()
  {
    this.follower.Brain.Info.NextSkinColor();
    foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData(this.follower.Brain.Info.SkinName).SlotAndColours[this.follower.Brain.Info.SkinColour].SlotAndColours)
    {
      Slot slot = this.follower.Spine.skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  public void PrevColour()
  {
    this.follower.Brain.Info.PrevSkinColor();
    foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData(this.follower.Brain.Info.SkinName).SlotAndColours[this.follower.Brain.Info.SkinColour].SlotAndColours)
    {
      Slot slot = this.follower.Spine.skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  private void OnDisable()
  {
    this.uINavigator.OnButtonDown -= new UINavigator.ButtonDown(this.OnButtonDown);
    this.ChangeName.onValueChanged.RemoveAllListeners();
  }

  private void OnButtonDown(Buttons CurrentButton) => Debug.Log((object) "close!");

  public void Close()
  {
    GameManager.GetInstance()?.CameraSetOffset(Vector3.zero);
    this.StartCoroutine((IEnumerator) this.CloseRoutine());
  }

  private IEnumerator CloseRoutine()
  {
    UIRecruitScreen uiRecruitScreen = this;
    uiRecruitScreen.Animator.Play("Base Layer.Out");
    yield return (object) new WaitForSeconds(0.5f);
    System.Action callback = uiRecruitScreen.Callback;
    if (callback != null)
      callback();
    UnityEngine.Object.Destroy((UnityEngine.Object) uiRecruitScreen.gameObject);
  }
}
