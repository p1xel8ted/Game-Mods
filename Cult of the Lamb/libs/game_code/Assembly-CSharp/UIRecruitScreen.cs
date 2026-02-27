// Decompiled with JetBrains decompiler
// Type: UIRecruitScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UIRecruitScreen : BaseMonoBehaviour
{
  public Animator Animator;
  public UINavigator uINavigator;
  public System.Action Callback;
  public Follower follower;
  public TextMeshProUGUI NameText;
  public TextMeshProUGUI FactionText;
  public TMP_InputField ChangeName;
  public int NumSkins = 3;

  public void Start()
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
    string outfitName = FollowerBrain.GetOutfitName(this.follower.Brain.Info.Outfit);
    skin.AddSkin(this.follower.Spine.Skeleton.Data.FindSkin(outfitName));
    this.follower.Spine.Skeleton.SetSkin(skin);
  }

  public void PrevSkin()
  {
    this.follower.Brain.Info.PrevSkin();
    Skin skin = this.follower.Spine.Skeleton.Data.FindSkin(this.follower.Brain.Info.SkinName);
    string outfitName = FollowerBrain.GetOutfitName(this.follower.Brain.Info.Outfit);
    skin.AddSkin(this.follower.Spine.Skeleton.Data.FindSkin(outfitName));
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

  public void OnDisable()
  {
    this.uINavigator.OnButtonDown -= new UINavigator.ButtonDown(this.OnButtonDown);
    this.ChangeName.onValueChanged.RemoveAllListeners();
  }

  public void OnButtonDown(Buttons CurrentButton) => Debug.Log((object) "close!");

  public void Close()
  {
    GameManager.GetInstance()?.CameraSetOffset(Vector3.zero);
    this.StartCoroutine(this.CloseRoutine());
  }

  public IEnumerator CloseRoutine()
  {
    UIRecruitScreen uiRecruitScreen = this;
    uiRecruitScreen.Animator.Play("Base Layer.Out");
    yield return (object) new WaitForSeconds(0.5f);
    System.Action callback = uiRecruitScreen.Callback;
    if (callback != null)
      callback();
    UnityEngine.Object.Destroy((UnityEngine.Object) uiRecruitScreen.gameObject);
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__9_0(string _param1) => this.OnNameChanged();
}
