// Decompiled with JetBrains decompiler
// Type: SeasonalEventData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.UI.Overlays.EventOverlay;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Seasonal Event Data")]
public class SeasonalEventData : ScriptableObject
{
  public SeasonalEventType EventType;
  public int StartDay;
  public int StartMonth;
  public int EndDay;
  public int EndMonth;
  public string[] Skins;
  public StructureBrain.TYPES[] Decorations;
  public bool ShowEventOverlay = true;
  [SerializeField]
  public AssetReferenceGameObject _eventOverlay;
  [SerializeField]
  [TermsPopup("")]
  public string _titleTerm;
  [SerializeField]
  [TermsPopup("")]
  public string _descriptionTerm;
  [SerializeField]
  [TermsPopup("")]
  public string _durationTerm;
  [SerializeField]
  [TermsPopup("")]
  public string _onboardingTitle;
  [SerializeField]
  public SeasonalEventData.OnboardingEntry _entry1;
  [SerializeField]
  public SeasonalEventData.OnboardingEntry _entry2;
  [SerializeField]
  public SeasonalEventData.OnboardingEntry _entry3;
  [CompilerGenerated]
  public UIEventOverlay \u003CEventOverlay\u003Ek__BackingField;

  public UIEventOverlay EventOverlay
  {
    set => this.\u003CEventOverlay\u003Ek__BackingField = value;
    get => this.\u003CEventOverlay\u003Ek__BackingField;
  }

  public string Title => LocalizationManager.GetTranslation(this._titleTerm);

  public string Description => LocalizationManager.GetTranslation(this._descriptionTerm);

  public string Duration
  {
    get
    {
      string translation = LocalizationManager.GetTranslation(this._durationTerm);
      string title = this.Title;
      DateTime dateTime = this.GetStartDate();
      string longDateString1 = dateTime.ToLongDateString();
      dateTime = this.GetEndDate();
      string longDateString2 = dateTime.ToLongDateString();
      return string.Format(translation, (object) title, (object) longDateString1, (object) longDateString2);
    }
  }

  public string OnboardingTitle => LocalizationManager.GetTranslation(this._onboardingTitle);

  public SeasonalEventData.OnboardingEntry Entry1 => this._entry1;

  public SeasonalEventData.OnboardingEntry Entry2 => this._entry2;

  public SeasonalEventData.OnboardingEntry Entry3 => this._entry3;

  public DateTime GetStartDate()
  {
    return new DateTime(DateTime.UtcNow.Year, this.StartMonth, this.StartDay);
  }

  public DateTime GetEndDate()
  {
    return new DateTime(this.EndMonth < this.StartMonth ? DateTime.UtcNow.Year + 1 : DateTime.UtcNow.Year, this.EndMonth, this.EndDay);
  }

  public bool IsEventActive()
  {
    return this.IsValid() && DateTime.UtcNow > this.GetStartDate() && DateTime.UtcNow < this.GetEndDate();
  }

  public bool IsValid() => DataManager.Instance.OnboardingFinished;

  public List<string> GetUnlockableSkins()
  {
    List<string> unlockableSkins = new List<string>();
    foreach (string skin in this.Skins)
    {
      if (!DataManager.GetFollowerSkinUnlocked(skin))
        unlockableSkins.Add(skin);
    }
    return unlockableSkins;
  }

  public List<StructureBrain.TYPES> GetUnlockableDecorations()
  {
    List<StructureBrain.TYPES> unlockableDecorations = new List<StructureBrain.TYPES>();
    foreach (StructureBrain.TYPES decoration in this.Decorations)
    {
      if (!DataManager.Instance.UnlockedStructures.Contains(decoration))
        unlockableDecorations.Add(decoration);
    }
    return unlockableDecorations;
  }

  public async System.Threading.Tasks.Task LoadUIAssets()
  {
    if (!(this._eventOverlay.Asset != (UnityEngine.Object) null))
      return;
    await System.Threading.Tasks.Task.WhenAll(this._entry1.Load(), this._entry2.Load(), this._entry3.Load());
    this.EventOverlay = await UIManager.LoadAsset<UIEventOverlay>(this._eventOverlay);
  }

  public void UnloadUIAssets()
  {
    if (this._eventOverlay == null)
      return;
    UIManager.UnloadAsset<UIEventOverlay>(this.EventOverlay);
    this._entry1.Unload();
    this._entry2.Unload();
    this._entry3.Unload();
  }

  [Serializable]
  public class OnboardingEntry
  {
    [SerializeField]
    [TermsPopup("")]
    public string _term;
    [SerializeField]
    public AssetReferenceSprite _imageReferenceSprite;
    [CompilerGenerated]
    public Sprite \u003CImage\u003Ek__BackingField;
    public AsyncOperationHandle<Sprite> _spriteAsyncOperationHandle;

    public string Text => LocalizationManager.GetTranslation(this._term);

    public Sprite Image
    {
      set => this.\u003CImage\u003Ek__BackingField = value;
      get => this.\u003CImage\u003Ek__BackingField;
    }

    public async System.Threading.Tasks.Task Load()
    {
      if (!((UnityEngine.Object) this.Image == (UnityEngine.Object) null))
        return;
      this._spriteAsyncOperationHandle = this._imageReferenceSprite.LoadAssetAsync<Sprite>();
      Sprite task = await this._spriteAsyncOperationHandle.Task;
      this.Image = this._spriteAsyncOperationHandle.Result;
    }

    public void Unload()
    {
      this.Image = (Sprite) null;
      if (!this._spriteAsyncOperationHandle.IsValid())
        return;
      Addressables.Release<Sprite>(this._spriteAsyncOperationHandle);
    }
  }
}
