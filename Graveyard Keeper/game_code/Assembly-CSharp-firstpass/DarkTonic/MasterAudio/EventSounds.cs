// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.EventSounds
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace DarkTonic.MasterAudio;

[AddComponentMenu("Dark Tonic/Master Audio/Event Sounds")]
[AudioScriptOrder(-30)]
public class EventSounds : MonoBehaviour, ICustomEventReceiver
{
  public bool showGizmo;
  public DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode soundSpawnMode = DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.AttachToCaller;
  public bool disableSounds;
  public bool showPoolManager;
  public bool showNGUI;
  public EventSounds.UnityUIVersion unityUIMode = EventSounds.UnityUIVersion.uGUI;
  public bool logMissingEvents = true;
  public static List<string> LayerTagFilterEvents = new List<string>()
  {
    EventSounds.EventType.OnCollision.ToString(),
    EventSounds.EventType.OnTriggerEnter.ToString(),
    EventSounds.EventType.OnTriggerExit.ToString(),
    EventSounds.EventType.OnCollision2D.ToString(),
    EventSounds.EventType.OnTriggerEnter2D.ToString(),
    EventSounds.EventType.OnTriggerExit2D.ToString(),
    EventSounds.EventType.OnParticleCollision.ToString(),
    EventSounds.EventType.OnCollisionExit.ToString(),
    EventSounds.EventType.OnCollisionExit2D.ToString()
  };
  public static List<DarkTonic.MasterAudio.MasterAudio.PlaylistCommand> PlaylistCommandsWithAll = new List<DarkTonic.MasterAudio.MasterAudio.PlaylistCommand>()
  {
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.FadeToVolume,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Pause,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.PlayNextSong,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.PlayRandomSong,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Resume,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Stop,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Mute,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Unmute,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.ToggleMute,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Restart,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.StopLoopingCurrentSong,
    DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.StopPlaylistAfterCurrentSong
  };
  public AudioEventGroup startSound;
  public AudioEventGroup visibleSound;
  public AudioEventGroup invisibleSound;
  public AudioEventGroup collisionSound;
  public AudioEventGroup collisionExitSound;
  public AudioEventGroup triggerSound;
  public AudioEventGroup triggerExitSound;
  public AudioEventGroup triggerStaySound;
  public AudioEventGroup mouseEnterSound;
  public AudioEventGroup mouseExitSound;
  public AudioEventGroup mouseClickSound;
  public AudioEventGroup mouseUpSound;
  public AudioEventGroup mouseDragSound;
  public AudioEventGroup spawnedSound;
  public AudioEventGroup despawnedSound;
  public AudioEventGroup enableSound;
  public AudioEventGroup disableSound;
  public AudioEventGroup collision2dSound;
  public AudioEventGroup collisionExit2dSound;
  public AudioEventGroup triggerEnter2dSound;
  public AudioEventGroup triggerStay2dSound;
  public AudioEventGroup triggerExit2dSound;
  public AudioEventGroup particleCollisionSound;
  public AudioEventGroup nguiOnClickSound;
  public AudioEventGroup nguiMouseDownSound;
  public AudioEventGroup nguiMouseUpSound;
  public AudioEventGroup nguiMouseEnterSound;
  public AudioEventGroup nguiMouseExitSound;
  public AudioEventGroup unitySliderChangedSound;
  public AudioEventGroup unityButtonClickedSound;
  public AudioEventGroup unityPointerDownSound;
  public AudioEventGroup unityDragSound;
  public AudioEventGroup unityPointerUpSound;
  public AudioEventGroup unityPointerEnterSound;
  public AudioEventGroup unityPointerExitSound;
  public AudioEventGroup unityDropSound;
  public AudioEventGroup unityScrollSound;
  public AudioEventGroup unityUpdateSelectedSound;
  public AudioEventGroup unitySelectSound;
  public AudioEventGroup unityDeselectSound;
  public AudioEventGroup unityMoveSound;
  public AudioEventGroup unityInitializePotentialDragSound;
  public AudioEventGroup unityBeginDragSound;
  public AudioEventGroup unityEndDragSound;
  public AudioEventGroup unitySubmitSound;
  public AudioEventGroup unityCancelSound;
  public AudioEventGroup unityToggleSound;
  public List<AudioEventGroup> userDefinedSounds = new List<AudioEventGroup>();
  public List<AudioEventGroup> mechanimStateChangedSounds = new List<AudioEventGroup>();
  public bool useStartSound;
  public bool useVisibleSound;
  public bool useInvisibleSound;
  public bool useCollisionSound;
  public bool useCollisionExitSound;
  public bool useTriggerEnterSound;
  public bool useTriggerExitSound;
  public bool useTriggerStaySound;
  public bool useMouseEnterSound;
  public bool useMouseExitSound;
  public bool useMouseClickSound;
  public bool useMouseUpSound;
  public bool useMouseDragSound;
  public bool useSpawnedSound;
  public bool useDespawnedSound;
  public bool useEnableSound;
  public bool useDisableSound;
  public bool useCollision2dSound;
  public bool useCollisionExit2dSound;
  public bool useTriggerEnter2dSound;
  public bool useTriggerStay2dSound;
  public bool useTriggerExit2dSound;
  public bool useParticleCollisionSound;
  public bool useNguiOnClickSound;
  public bool useNguiMouseDownSound;
  public bool useNguiMouseUpSound;
  public bool useNguiMouseEnterSound;
  public bool useNguiMouseExitSound;
  public bool useUnitySliderChangedSound;
  public bool useUnityButtonClickedSound;
  public bool useUnityPointerDownSound;
  public bool useUnityDragSound;
  public bool useUnityPointerUpSound;
  public bool useUnityPointerEnterSound;
  public bool useUnityPointerExitSound;
  public bool useUnityDropSound;
  public bool useUnityScrollSound;
  public bool useUnityUpdateSelectedSound;
  public bool useUnitySelectSound;
  public bool useUnityDeselectSound;
  public bool useUnityMoveSound;
  public bool useUnityInitializePotentialDragSound;
  public bool useUnityBeginDragSound;
  public bool useUnityEndDragSound;
  public bool useUnitySubmitSound;
  public bool useUnityCancelSound;
  public bool useUnityToggleSound;
  public Slider _slider;
  public Toggle _toggle;
  public Button _button;
  public bool _isVisible;
  public bool _needsCoroutine;
  public float? _triggerEnterTime;
  public float? _triggerEnter2dTime;
  public bool _mouseDragSoundPlayed;
  public PlaySoundResult _mouseDragResult;
  public Transform _trans;
  public List<AudioEventGroup> _validMechanimStateChangedSounds = new List<AudioEventGroup>();
  public Animator _anim;

  public void Awake()
  {
    this._trans = this.transform;
    this._anim = this.GetComponent<Animator>();
    this._slider = this.GetComponent<Slider>();
    this._button = this.GetComponent<Button>();
    this._toggle = this.GetComponent<Toggle>();
    if (this.IsSetToUGUI)
      this.AddUGUIComponents();
    this.SpawnedOrAwake();
  }

  public virtual void SpawnedOrAwake()
  {
    this._isVisible = false;
    this._validMechanimStateChangedSounds.Clear();
    this._needsCoroutine = false;
    if (this.disableSounds || (UnityEngine.Object) this._anim == (UnityEngine.Object) null)
      return;
    for (int index = 0; index < this.mechanimStateChangedSounds.Count; ++index)
    {
      AudioEventGroup stateChangedSound = this.mechanimStateChangedSounds[index];
      if (stateChangedSound.mechanimEventActive && !string.IsNullOrEmpty(stateChangedSound.mechanimStateName))
      {
        this._needsCoroutine = true;
        this._validMechanimStateChangedSounds.Add(stateChangedSound);
      }
    }
  }

  public IEnumerator CoUpdate()
  {
label_1:
    yield return (object) DarkTonic.MasterAudio.MasterAudio.EndOfFrameDelay;
    for (int index = 0; index < this._validMechanimStateChangedSounds.Count; ++index)
    {
      AudioEventGroup stateChangedSound = this._validMechanimStateChangedSounds[index];
      if (!this._anim.GetCurrentAnimatorStateInfo(0).IsName(stateChangedSound.mechanimStateName))
        stateChangedSound.mechEventPlayedForState = false;
      else if (!stateChangedSound.mechEventPlayedForState)
      {
        stateChangedSound.mechEventPlayedForState = true;
        this.PlaySounds(stateChangedSound, EventSounds.EventType.MechanimStateChanged);
      }
    }
    goto label_1;
  }

  public void Start()
  {
    this.CheckForIllegalCustomEvents();
    if (!this.useStartSound)
      return;
    this.PlaySounds(this.startSound, EventSounds.EventType.OnStart);
  }

  public void OnBecameVisible()
  {
    if (!this.useVisibleSound || this._isVisible)
      return;
    this._isVisible = true;
    this.PlaySounds(this.visibleSound, EventSounds.EventType.OnVisible);
  }

  public void OnBecameInvisible()
  {
    if (!this.useInvisibleSound)
      return;
    this._isVisible = false;
    this.PlaySounds(this.invisibleSound, EventSounds.EventType.OnInvisible);
  }

  public void OnEnable()
  {
    if ((UnityEngine.Object) this._slider != (UnityEngine.Object) null)
    {
      this._slider.onValueChanged.AddListener(new UnityAction<float>(this.SliderChanged));
      this.RestorePersistentSliders();
    }
    if ((UnityEngine.Object) this._button != (UnityEngine.Object) null)
      this._button.onClick.AddListener(new UnityAction(this.ButtonClicked));
    if ((UnityEngine.Object) this._toggle != (UnityEngine.Object) null)
      this._toggle.onValueChanged.AddListener(new UnityAction<bool>(this.ToggleChanged));
    this._mouseDragResult = (PlaySoundResult) null;
    this.RegisterReceiver();
    if (this._needsCoroutine)
    {
      this.StopAllCoroutines();
      this.StartCoroutine(this.CoUpdate());
    }
    if (!this.useEnableSound)
      return;
    this.PlaySounds(this.enableSound, EventSounds.EventType.OnEnable);
  }

  public void RestorePersistentSliders()
  {
    if (!this.useUnitySliderChangedSound)
      return;
    foreach (AudioEvent soundEvent in this.unitySliderChangedSound.SoundEvents)
    {
      if (soundEvent.currentSoundFunctionType == DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.PersistentSettingsControl && soundEvent.targetVolMode == AudioEvent.TargetVolumeMode.UseSliderValue)
      {
        switch (soundEvent.currentPersistentSettingsCommand)
        {
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.SetBusVolume:
            if (!soundEvent.allSoundTypesForBusCmd)
            {
              float? busVolume = PersistentAudioSettings.GetBusVolume(soundEvent.busName);
              if (busVolume.HasValue)
              {
                this._slider.value = busVolume.Value;
                continue;
              }
              continue;
            }
            continue;
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.SetGroupVolume:
            if (!soundEvent.allSoundTypesForGroupCmd)
            {
              float? groupVolume = PersistentAudioSettings.GetGroupVolume(soundEvent.soundType);
              if (groupVolume.HasValue)
              {
                this._slider.value = groupVolume.Value;
                continue;
              }
              continue;
            }
            continue;
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.SetMixerVolume:
            float? mixerVolume = PersistentAudioSettings.MixerVolume;
            if (mixerVolume.HasValue)
            {
              this._slider.value = mixerVolume.Value;
              continue;
            }
            continue;
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.SetMusicVolume:
            float? musicVolume = PersistentAudioSettings.MusicVolume;
            if (musicVolume.HasValue)
            {
              this._slider.value = musicVolume.Value;
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    if ((UnityEngine.Object) this._slider != (UnityEngine.Object) null)
      this._slider.onValueChanged.RemoveListener(new UnityAction<float>(this.SliderChanged));
    if ((UnityEngine.Object) this._button != (UnityEngine.Object) null)
      this._button.onClick.RemoveListener(new UnityAction(this.ButtonClicked));
    if ((UnityEngine.Object) this._toggle != (UnityEngine.Object) null)
      this._toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.ToggleChanged));
    this.UnregisterReceiver();
    if (!this.useDisableSound || DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    this.PlaySounds(this.disableSound, EventSounds.EventType.OnDisable);
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    this._triggerEnter2dTime = new float?(Time.realtimeSinceStartup);
    if (!this.useTriggerEnter2dSound || this.triggerEnter2dSound.useLayerFilter && !this.triggerEnter2dSound.matchingLayers.Contains(other.gameObject.layer) || this.triggerEnter2dSound.useTagFilter && !this.triggerEnter2dSound.matchingTags.Contains(other.gameObject.tag))
      return;
    this.PlaySounds(this.triggerEnter2dSound, EventSounds.EventType.OnTriggerEnter2D);
  }

  public void OnTriggerStay2D(Collider2D other)
  {
    if (!this.useTriggerStay2dSound)
      return;
    if ((double) this.triggerStay2dSound.triggerStayForTime > (this._triggerEnter2dTime.HasValue ? (double) (Time.realtimeSinceStartup - this._triggerEnter2dTime.Value) : 0.0) || !this.PlaySounds(this.triggerStay2dSound, EventSounds.EventType.OnTriggerStay2D) || !this.triggerStay2dSound.doesTriggerStayRepeat)
      return;
    this._triggerEnter2dTime = new float?(Time.realtimeSinceStartup);
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    this._triggerEnter2dTime = new float?();
    if (!this.useTriggerExit2dSound || this.triggerExit2dSound.useLayerFilter && !this.triggerExit2dSound.matchingLayers.Contains(other.gameObject.layer) || this.triggerExit2dSound.useTagFilter && !this.triggerExit2dSound.matchingTags.Contains(other.gameObject.tag))
      return;
    this.PlaySounds(this.triggerExit2dSound, EventSounds.EventType.OnTriggerExit2D);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.useCollision2dSound || this.collision2dSound.useLayerFilter && !this.collision2dSound.matchingLayers.Contains(collision.gameObject.layer) || this.collision2dSound.useTagFilter && !this.collision2dSound.matchingTags.Contains(collision.gameObject.tag))
      return;
    this.PlaySounds(this.collision2dSound, EventSounds.EventType.OnCollision2D);
  }

  public void OnCollisionExit2D(Collision2D collision)
  {
    if (!this.useCollisionExit2dSound || this.collisionExit2dSound.useLayerFilter && !this.collisionExit2dSound.matchingLayers.Contains(collision.gameObject.layer) || this.collisionExit2dSound.useTagFilter && !this.collisionExit2dSound.matchingTags.Contains(collision.gameObject.tag))
      return;
    this.PlaySounds(this.collisionExit2dSound, EventSounds.EventType.OnCollisionExit2D);
  }

  public void OnCollisionEnter(Collision collision)
  {
    if (!this.useCollisionSound || this.collisionSound.useLayerFilter && !this.collisionSound.matchingLayers.Contains(collision.gameObject.layer) || this.collisionSound.useTagFilter && !this.collisionSound.matchingTags.Contains(collision.gameObject.tag))
      return;
    this.PlaySounds(this.collisionSound, EventSounds.EventType.OnCollision);
  }

  public void OnCollisionExit(Collision collision)
  {
    if (!this.useCollisionExitSound || this.collisionExitSound.useLayerFilter && !this.collisionExitSound.matchingLayers.Contains(collision.gameObject.layer) || this.collisionExitSound.useTagFilter && !this.collisionExitSound.matchingTags.Contains(collision.gameObject.tag))
      return;
    this.PlaySounds(this.collisionExitSound, EventSounds.EventType.OnCollisionExit);
  }

  public void OnTriggerEnter(Collider other)
  {
    this._triggerEnterTime = new float?(Time.realtimeSinceStartup);
    if (!this.useTriggerEnterSound || this.triggerSound.useLayerFilter && !this.triggerSound.matchingLayers.Contains(other.gameObject.layer) || this.triggerSound.useTagFilter && !this.triggerSound.matchingTags.Contains(other.gameObject.tag))
      return;
    this.PlaySounds(this.triggerSound, EventSounds.EventType.OnTriggerEnter);
  }

  public void OnTriggerStay(Collider other)
  {
    if (!this.useTriggerStaySound)
      return;
    if ((double) this.triggerStaySound.triggerStayForTime > (this._triggerEnterTime.HasValue ? (double) (Time.realtimeSinceStartup - this._triggerEnterTime.Value) : 0.0) || !this.PlaySounds(this.triggerStaySound, EventSounds.EventType.OnTriggerStay) || !this.triggerStaySound.doesTriggerStayRepeat)
      return;
    this._triggerEnterTime = new float?(Time.realtimeSinceStartup);
  }

  public void OnTriggerExit(Collider other)
  {
    this._triggerEnterTime = new float?();
    if (!this.useTriggerExitSound || this.triggerExitSound.useLayerFilter && !this.triggerExitSound.matchingLayers.Contains(other.gameObject.layer) || this.triggerExitSound.useTagFilter && !this.triggerExitSound.matchingTags.Contains(other.gameObject.tag))
      return;
    this.PlaySounds(this.triggerExitSound, EventSounds.EventType.OnTriggerExit);
  }

  public void OnParticleCollision(GameObject other)
  {
    if (!this.useParticleCollisionSound || this.particleCollisionSound.useLayerFilter && !this.particleCollisionSound.matchingLayers.Contains(other.gameObject.layer) || this.particleCollisionSound.useTagFilter && !this.particleCollisionSound.matchingTags.Contains(other.gameObject.tag))
      return;
    this.PlaySounds(this.particleCollisionSound, EventSounds.EventType.OnParticleCollision);
  }

  public void OnPointerEnter(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityPointerEnterSound)
      return;
    this.PlaySounds(this.unityPointerEnterSound, EventSounds.EventType.UnityPointerEnter);
  }

  public void OnPointerExit(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityPointerExitSound)
      return;
    this.PlaySounds(this.unityPointerExitSound, EventSounds.EventType.UnityPointerExit);
  }

  public void OnPointerDown(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityPointerDownSound)
      return;
    this.PlaySounds(this.unityPointerDownSound, EventSounds.EventType.UnityPointerDown);
  }

  public void OnPointerUp(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityPointerUpSound)
      return;
    this.PlaySounds(this.unityPointerUpSound, EventSounds.EventType.UnityPointerUp);
  }

  public void OnDrag(Vector2 delta)
  {
  }

  public void OnDrag(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityDragSound)
      return;
    this.PlaySounds(this.unityDragSound, EventSounds.EventType.UnityDrag);
  }

  public void OnDrop(GameObject go)
  {
  }

  public void OnDrop(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityDropSound)
      return;
    this.PlaySounds(this.unityDropSound, EventSounds.EventType.UnityDrop);
  }

  public void OnScroll(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityScrollSound)
      return;
    this.PlaySounds(this.unityScrollSound, EventSounds.EventType.UnityScroll);
  }

  public void OnUpdateSelected(BaseEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityUpdateSelectedSound)
      return;
    this.PlaySounds(this.unityUpdateSelectedSound, EventSounds.EventType.UnityUpdateSelected);
  }

  public void OnSelect(bool isSelected)
  {
  }

  public void OnSelect(BaseEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnitySelectSound)
      return;
    this.PlaySounds(this.unitySelectSound, EventSounds.EventType.UnitySelect);
  }

  public void OnDeselect(BaseEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityDeselectSound)
      return;
    this.PlaySounds(this.unityDeselectSound, EventSounds.EventType.UnityDeselect);
  }

  public void OnMove(AxisEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityMoveSound)
      return;
    this.PlaySounds(this.unityMoveSound, EventSounds.EventType.UnityMove);
  }

  public void OnInitializePotentialDrag(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityInitializePotentialDragSound)
      return;
    this.PlaySounds(this.unityInitializePotentialDragSound, EventSounds.EventType.UnityInitializePotentialDrag);
  }

  public void OnBeginDrag(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityBeginDragSound)
      return;
    this.PlaySounds(this.unityBeginDragSound, EventSounds.EventType.UnityBeginDrag);
  }

  public void OnEndDrag(PointerEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityEndDragSound)
      return;
    this.PlaySounds(this.unityEndDragSound, EventSounds.EventType.UnityEndDrag);
  }

  public void OnSubmit(BaseEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnitySubmitSound)
      return;
    this.PlaySounds(this.unitySubmitSound, EventSounds.EventType.UnitySubmit);
  }

  public void OnCancel(BaseEventData data)
  {
    if (!this.IsSetToUGUI || !this.useUnityCancelSound)
      return;
    this.PlaySounds(this.unityCancelSound, EventSounds.EventType.UnityCancel);
  }

  public void SliderChanged(float newValue)
  {
    if (!this.useUnitySliderChangedSound)
      return;
    this.unitySliderChangedSound.sliderValue = newValue;
    this.PlaySounds(this.unitySliderChangedSound, EventSounds.EventType.UnitySliderChanged);
  }

  public void ToggleChanged(bool newValue)
  {
    if (!this.useUnityToggleSound)
      return;
    this.PlaySounds(this.unityToggleSound, EventSounds.EventType.UnityToggle);
  }

  public void ButtonClicked()
  {
    if (!this.useUnityButtonClickedSound)
      return;
    this.PlaySounds(this.unityButtonClickedSound, EventSounds.EventType.UnityButtonClicked);
  }

  public bool IsSetToUGUI => this.unityUIMode != 0;

  public bool IsSetToLegacyUI => this.unityUIMode == EventSounds.UnityUIVersion.Legacy;

  public void OnMouseEnter()
  {
    if (!this.IsSetToLegacyUI || !this.useMouseEnterSound)
      return;
    this.PlaySounds(this.mouseEnterSound, EventSounds.EventType.OnMouseEnter);
  }

  public void OnMouseExit()
  {
    if (!this.IsSetToLegacyUI || !this.useMouseExitSound)
      return;
    this.PlaySounds(this.mouseExitSound, EventSounds.EventType.OnMouseExit);
  }

  public void OnMouseDown()
  {
    if (!this.IsSetToLegacyUI || !this.useMouseClickSound)
      return;
    this.PlaySounds(this.mouseClickSound, EventSounds.EventType.OnMouseClick);
  }

  public void OnMouseUp()
  {
    if (this.IsSetToLegacyUI && this.useMouseUpSound)
      this.PlaySounds(this.mouseUpSound, EventSounds.EventType.OnMouseUp);
    if (this.useMouseDragSound)
    {
      switch (this.mouseUpSound.mouseDragStopMode)
      {
        case EventSounds.PreviousSoundStopMode.Stop:
          if (this._mouseDragResult != null && (this._mouseDragResult.SoundPlayed || this._mouseDragResult.SoundScheduled))
          {
            this._mouseDragResult.ActingVariation.Stop(true);
            break;
          }
          break;
        case EventSounds.PreviousSoundStopMode.FadeOut:
          if (this._mouseDragResult != null && (this._mouseDragResult.SoundPlayed || this._mouseDragResult.SoundScheduled))
          {
            this._mouseDragResult.ActingVariation.FadeToVolume(0.0f, this.mouseUpSound.mouseDragFadeOutTime);
            break;
          }
          break;
      }
      this._mouseDragResult = (PlaySoundResult) null;
    }
    this._mouseDragSoundPlayed = false;
  }

  public void OnMouseDrag()
  {
    if (!this.IsSetToLegacyUI || !this.useMouseDragSound || this._mouseDragSoundPlayed)
      return;
    this.PlaySounds(this.mouseDragSound, EventSounds.EventType.OnMouseDrag);
    this._mouseDragSoundPlayed = true;
  }

  public void OnPress(bool isDown)
  {
    if (!this.showNGUI)
      return;
    if (isDown)
    {
      if (!this.useNguiMouseDownSound)
        return;
      this.PlaySounds(this.nguiMouseDownSound, EventSounds.EventType.NGUIMouseDown);
    }
    else
    {
      if (!this.useNguiMouseUpSound)
        return;
      this.PlaySounds(this.nguiMouseUpSound, EventSounds.EventType.NGUIMouseUp);
    }
  }

  public void OnClick()
  {
    if (!this.showNGUI || !this.useNguiOnClickSound)
      return;
    this.PlaySounds(this.nguiOnClickSound, EventSounds.EventType.NGUIOnClick);
  }

  public void OnHover(bool isOver)
  {
    if (!this.showNGUI)
      return;
    if (isOver)
    {
      if (!this.useNguiMouseEnterSound)
        return;
      this.PlaySounds(this.nguiMouseEnterSound, EventSounds.EventType.NGUIMouseEnter);
    }
    else
    {
      if (!this.useNguiMouseExitSound)
        return;
      this.PlaySounds(this.nguiMouseExitSound, EventSounds.EventType.NGUIMouseExit);
    }
  }

  public void OnSpawned()
  {
    this.SpawnedOrAwake();
    if (!this.showPoolManager || !this.useSpawnedSound)
      return;
    this.PlaySounds(this.spawnedSound, EventSounds.EventType.OnSpawned);
  }

  public void OnDespawned()
  {
    if (!this.showPoolManager || !this.useDespawnedSound)
      return;
    this.PlaySounds(this.despawnedSound, EventSounds.EventType.OnDespawned);
  }

  public AudioEventGroup GetMechanimAudioEventGroup(string stateName)
  {
    return this._validMechanimStateChangedSounds.Find((Predicate<AudioEventGroup>) (grp => grp.mechanimStateName == stateName));
  }

  public bool PlaySounds(AudioEventGroup eventGrp, EventSounds.EventType eType)
  {
    if (!EventSounds.CheckForRetriggerLimit(eventGrp) || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return false;
    switch (eventGrp.retriggerLimitMode)
    {
      case EventSounds.RetriggerLimMode.FrameBased:
        eventGrp.triggeredLastFrame = AudioUtil.FrameCount;
        break;
      case EventSounds.RetriggerLimMode.TimeBased:
        eventGrp.triggeredLastTime = AudioUtil.Time;
        break;
    }
    if (!DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown && DarkTonic.MasterAudio.MasterAudio.IsWarming)
    {
      AudioEvent aEvent = new AudioEvent();
      this.PerformSingleAction(eventGrp, aEvent, eType);
      return true;
    }
    for (int index = 0; index < eventGrp.SoundEvents.Count; ++index)
      this.PerformSingleAction(eventGrp, eventGrp.SoundEvents[index], eType);
    return true;
  }

  public void OnDrawGizmos()
  {
    if (!this.showGizmo)
      return;
    Gizmos.DrawIcon(this.transform.position, "MasterAudio/MasterAudio Icon.png", true);
  }

  public static bool CheckForRetriggerLimit(AudioEventGroup grp)
  {
    switch (grp.retriggerLimitMode)
    {
      case EventSounds.RetriggerLimMode.FrameBased:
        if (grp.triggeredLastFrame > 0 && AudioUtil.FrameCount - grp.triggeredLastFrame < grp.limitPerXFrm)
          return false;
        break;
      case EventSounds.RetriggerLimMode.TimeBased:
        if ((double) grp.triggeredLastTime > 0.0 && (double) AudioUtil.Time - (double) grp.triggeredLastTime < (double) grp.limitPerXSec)
          return false;
        break;
    }
    return true;
  }

  public void PerformSingleAction(
    AudioEventGroup grp,
    AudioEvent aEvent,
    EventSounds.EventType eType)
  {
    if (this.disableSounds || DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return;
    bool flag1 = eType == EventSounds.EventType.UnitySliderChanged && aEvent.targetVolMode == AudioEvent.TargetVolumeMode.UseSliderValue;
    float volumePercentage = aEvent.volume;
    string soundType = aEvent.soundType;
    float? pitch = new float?(aEvent.pitch);
    if (!aEvent.useFixedPitch)
      pitch = new float?();
    PlaySoundResult playSoundResult1 = (PlaySoundResult) null;
    DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode spawnLocationMode = this.soundSpawnMode;
    if (eType == EventSounds.EventType.OnDisable || eType == EventSounds.EventType.OnDespawned)
      spawnLocationMode = DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.CallerLocation;
    bool flag2 = eType == EventSounds.EventType.OnMouseDrag || aEvent.glidePitchType != 0;
    switch (aEvent.currentSoundFunctionType)
    {
      case DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.PlaySound:
        string variationName = (string) null;
        if (aEvent.variationType == EventSounds.VariationType.PlaySpecific)
          variationName = aEvent.variationName;
        if (flag1)
          volumePercentage = grp.sliderValue;
        switch (spawnLocationMode)
        {
          case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.MasterAudioLocation:
            if (flag2)
            {
              playSoundResult1 = DarkTonic.MasterAudio.MasterAudio.PlaySound(soundType, volumePercentage, pitch, aEvent.delaySound, variationName);
              break;
            }
            DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(soundType, volumePercentage, pitch, aEvent.delaySound, variationName);
            break;
          case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.CallerLocation:
            if (flag2)
            {
              playSoundResult1 = DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform(soundType, this._trans, volumePercentage, pitch, aEvent.delaySound, variationName);
              break;
            }
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(soundType, this._trans, volumePercentage, pitch, aEvent.delaySound, variationName);
            break;
          case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.AttachToCaller:
            if (flag2)
            {
              playSoundResult1 = DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransform(soundType, this._trans, volumePercentage, pitch, aEvent.delaySound, variationName);
              break;
            }
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(soundType, this._trans, volumePercentage, pitch, aEvent.delaySound, variationName);
            break;
        }
        if (playSoundResult1 != null && (UnityEngine.Object) playSoundResult1.ActingVariation != (UnityEngine.Object) null && aEvent.glidePitchType != EventSounds.GlidePitchType.None)
        {
          switch (aEvent.glidePitchType)
          {
            case EventSounds.GlidePitchType.RaisePitch:
              playSoundResult1.ActingVariation.GlideByPitch(aEvent.targetGlidePitch, aEvent.pitchGlideTime);
              break;
            case EventSounds.GlidePitchType.LowerPitch:
              playSoundResult1.ActingVariation.GlideByPitch(-aEvent.targetGlidePitch, aEvent.pitchGlideTime);
              break;
          }
        }
        if (eType != EventSounds.EventType.OnMouseDrag)
          break;
        this._mouseDragResult = playSoundResult1;
        break;
      case DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.GroupControl:
        PlaySoundResult playSoundResult2 = new PlaySoundResult()
        {
          ActingVariation = (SoundGroupVariation) null,
          SoundPlayed = true,
          SoundScheduled = false
        };
        List<string> stringList1 = new List<string>();
        if (!aEvent.allSoundTypesForGroupCmd || DarkTonic.MasterAudio.MasterAudio.GroupCommandsWithNoAllGroupSelector.Contains(aEvent.currentSoundGroupCommand))
          stringList1.Add(aEvent.soundType);
        else
          stringList1.AddRange((IEnumerable<string>) DarkTonic.MasterAudio.MasterAudio.RuntimeSoundGroupNames);
        for (int index = 0; index < stringList1.Count; ++index)
        {
          string sType = stringList1[index];
          switch (aEvent.currentSoundGroupCommand)
          {
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.None:
              playSoundResult2.SoundPlayed = false;
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeToVolume:
              float newVolume = flag1 ? grp.sliderValue : aEvent.fadeVolume;
              DarkTonic.MasterAudio.MasterAudio.FadeSoundGroupToVolume(sType, newVolume, aEvent.fadeTime, willStopAfterFade: aEvent.stopAfterFade, willResetVolumeAfterFade: aEvent.restoreVolumeAfterFade);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeOutAllOfSound:
              DarkTonic.MasterAudio.MasterAudio.FadeOutAllOfSound(sType, aEvent.fadeTime);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.Mute:
              DarkTonic.MasterAudio.MasterAudio.MuteGroup(sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.Pause:
              DarkTonic.MasterAudio.MasterAudio.PauseSoundGroup(sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.Solo:
              DarkTonic.MasterAudio.MasterAudio.SoloGroup(sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.StopAllOfSound:
              DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.Unmute:
              DarkTonic.MasterAudio.MasterAudio.UnmuteGroup(sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.Unpause:
              DarkTonic.MasterAudio.MasterAudio.UnpauseSoundGroup(sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.Unsolo:
              DarkTonic.MasterAudio.MasterAudio.UnsoloGroup(sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.StopAllSoundsOfTransform:
              DarkTonic.MasterAudio.MasterAudio.StopAllSoundsOfTransform(this._trans);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.PauseAllSoundsOfTransform:
              DarkTonic.MasterAudio.MasterAudio.PauseAllSoundsOfTransform(this._trans);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.UnpauseAllSoundsOfTransform:
              DarkTonic.MasterAudio.MasterAudio.UnpauseAllSoundsOfTransform(this._trans);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.StopSoundGroupOfTransform:
              DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(this._trans, sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.PauseSoundGroupOfTransform:
              DarkTonic.MasterAudio.MasterAudio.PauseSoundGroupOfTransform(this._trans, sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.UnpauseSoundGroupOfTransform:
              DarkTonic.MasterAudio.MasterAudio.UnpauseSoundGroupOfTransform(this._trans, sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeOutSoundGroupOfTransform:
              DarkTonic.MasterAudio.MasterAudio.FadeOutSoundGroupOfTransform(this._trans, sType, aEvent.fadeTime);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.RefillSoundGroupPool:
              DarkTonic.MasterAudio.MasterAudio.RefillSoundGroupPool(sType);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.RouteToBus:
              string busName = aEvent.busName;
              if (busName == "[None]")
                busName = (string) null;
              DarkTonic.MasterAudio.MasterAudio.RouteGroupToBus(sType, busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.GlideByPitch:
              switch (aEvent.glidePitchType)
              {
                case EventSounds.GlidePitchType.RaisePitch:
                  DarkTonic.MasterAudio.MasterAudio.GlideSoundGroupByPitch(sType, aEvent.targetGlidePitch, aEvent.pitchGlideTime);
                  continue;
                case EventSounds.GlidePitchType.LowerPitch:
                  DarkTonic.MasterAudio.MasterAudio.GlideSoundGroupByPitch(sType, -aEvent.targetGlidePitch, aEvent.pitchGlideTime);
                  continue;
                default:
                  continue;
              }
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.ToggleSoundGroup:
              if (DarkTonic.MasterAudio.MasterAudio.IsSoundGroupPlaying(sType))
              {
                DarkTonic.MasterAudio.MasterAudio.FadeOutAllOfSound(sType, aEvent.fadeTime);
                break;
              }
              switch (spawnLocationMode)
              {
                case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.MasterAudioLocation:
                  DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(soundType, volumePercentage, pitch, aEvent.delaySound);
                  continue;
                case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.CallerLocation:
                  DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(soundType, this._trans, volumePercentage, pitch, aEvent.delaySound);
                  continue;
                case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.AttachToCaller:
                  DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(soundType, this._trans, volumePercentage, pitch, aEvent.delaySound);
                  continue;
                default:
                  continue;
              }
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.ToggleSoundGroupOfTransform:
              if (DarkTonic.MasterAudio.MasterAudio.IsTransformPlayingSoundGroup(sType, this._trans))
              {
                DarkTonic.MasterAudio.MasterAudio.FadeOutSoundGroupOfTransform(this._trans, sType, aEvent.fadeTime);
                break;
              }
              switch (spawnLocationMode)
              {
                case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.MasterAudioLocation:
                  DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(soundType, volumePercentage, pitch, aEvent.delaySound);
                  continue;
                case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.CallerLocation:
                  DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(soundType, this._trans, volumePercentage, pitch, aEvent.delaySound);
                  continue;
                case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.AttachToCaller:
                  DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(soundType, this._trans, volumePercentage, pitch, aEvent.delaySound);
                  continue;
                default:
                  continue;
              }
            case DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeOutAllSoundsOfTransform:
              DarkTonic.MasterAudio.MasterAudio.FadeOutAllSoundsOfTransform(this._trans, aEvent.fadeTime);
              break;
          }
        }
        break;
      case DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.BusControl:
        PlaySoundResult playSoundResult3 = new PlaySoundResult()
        {
          ActingVariation = (SoundGroupVariation) null,
          SoundPlayed = true,
          SoundScheduled = false
        };
        List<string> stringList2 = new List<string>();
        if (!aEvent.allSoundTypesForBusCmd)
          stringList2.Add(aEvent.busName);
        else
          stringList2.AddRange((IEnumerable<string>) DarkTonic.MasterAudio.MasterAudio.RuntimeBusNames);
        for (int index = 0; index < stringList2.Count; ++index)
        {
          string busName = stringList2[index];
          switch (aEvent.currentBusCommand)
          {
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.None:
              playSoundResult3.SoundPlayed = false;
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.FadeToVolume:
              float newVolume = flag1 ? grp.sliderValue : aEvent.fadeVolume;
              DarkTonic.MasterAudio.MasterAudio.FadeBusToVolume(busName, newVolume, aEvent.fadeTime, willStopAfterFade: aEvent.stopAfterFade, willResetVolumeAfterFade: aEvent.restoreVolumeAfterFade);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.Mute:
              DarkTonic.MasterAudio.MasterAudio.MuteBus(busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.Pause:
              DarkTonic.MasterAudio.MasterAudio.PauseBus(busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.Solo:
              DarkTonic.MasterAudio.MasterAudio.SoloBus(busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.Unmute:
              DarkTonic.MasterAudio.MasterAudio.UnmuteBus(busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.Unpause:
              DarkTonic.MasterAudio.MasterAudio.UnpauseBus(busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.Unsolo:
              DarkTonic.MasterAudio.MasterAudio.UnsoloBus(busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.Stop:
              DarkTonic.MasterAudio.MasterAudio.StopBus(busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.ChangePitch:
              DarkTonic.MasterAudio.MasterAudio.ChangeBusPitch(busName, aEvent.pitch);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.ToggleMute:
              DarkTonic.MasterAudio.MasterAudio.ToggleMuteBus(busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.StopBusOfTransform:
              DarkTonic.MasterAudio.MasterAudio.StopBusOfTransform(this._trans, aEvent.busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.PauseBusOfTransform:
              DarkTonic.MasterAudio.MasterAudio.PauseBusOfTransform(this._trans, aEvent.busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.UnpauseBusOfTransform:
              DarkTonic.MasterAudio.MasterAudio.UnpauseBusOfTransform(this._trans, aEvent.busName);
              break;
            case DarkTonic.MasterAudio.MasterAudio.BusCommand.GlideByPitch:
              switch (aEvent.glidePitchType)
              {
                case EventSounds.GlidePitchType.RaisePitch:
                  DarkTonic.MasterAudio.MasterAudio.GlideBusByPitch(busName, aEvent.targetGlidePitch, aEvent.pitchGlideTime);
                  continue;
                case EventSounds.GlidePitchType.LowerPitch:
                  DarkTonic.MasterAudio.MasterAudio.GlideBusByPitch(busName, -aEvent.targetGlidePitch, aEvent.pitchGlideTime);
                  continue;
                default:
                  continue;
              }
          }
        }
        break;
      case DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.PlaylistControl:
        PlaySoundResult playSoundResult4 = new PlaySoundResult()
        {
          ActingVariation = (SoundGroupVariation) null,
          SoundPlayed = true,
          SoundScheduled = false
        };
        if (string.IsNullOrEmpty(aEvent.playlistControllerName))
          aEvent.playlistControllerName = "~only~";
        switch (aEvent.currentPlaylistCommand)
        {
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.None:
            playSoundResult4.SoundPlayed = false;
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.ChangePlaylist:
            if (string.IsNullOrEmpty(aEvent.playlistName))
            {
              Debug.Log((object) $"You have not specified a Playlist name for Event Sounds on '{this._trans.name}'.");
              playSoundResult4.SoundPlayed = false;
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.ChangePlaylistByName(aEvent.playlistControllerName, aEvent.playlistName, aEvent.startPlaylist);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.FadeToVolume:
            float targetVolume = flag1 ? grp.sliderValue : aEvent.fadeVolume;
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.FadeAllPlaylistsToVolume(targetVolume, aEvent.fadeTime);
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.FadePlaylistToVolume(aEvent.playlistControllerName, targetVolume, aEvent.fadeTime);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.PlaySong:
            if (string.IsNullOrEmpty(aEvent.clipName))
            {
              Debug.Log((object) $"You have not specified a song name for Event Sounds on '{this._trans.name}'.");
              playSoundResult4.SoundPlayed = false;
              return;
            }
            if (aEvent.playlistControllerName == "[None]" || DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip(aEvent.playlistControllerName, aEvent.clipName))
              return;
            playSoundResult4.SoundPlayed = false;
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.PlayRandomSong:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.TriggerRandomClipAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.TriggerRandomPlaylistClip(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.PlayNextSong:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.TriggerNextClipAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.TriggerNextPlaylistClip(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Pause:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.PauseAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.PausePlaylist(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Resume:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.UnpauseAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.UnpausePlaylist(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Stop:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.StopPlaylist(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Mute:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.MuteAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.MutePlaylist(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Unmute:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.UnmuteAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.UnmutePlaylist(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.ToggleMute:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.ToggleMuteAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.ToggleMutePlaylist(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Restart:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.RestartAllPlaylists();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.RestartPlaylist(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.Start:
            if (aEvent.playlistControllerName == "[None]" || aEvent.playlistName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.StartPlaylist(aEvent.playlistControllerName, aEvent.playlistName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.StopLoopingCurrentSong:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.StopLoopingAllCurrentSongs();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.StopLoopingCurrentSong(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.StopPlaylistAfterCurrentSong:
            if (aEvent.allPlaylistControllersForGroupCmd)
            {
              DarkTonic.MasterAudio.MasterAudio.StopAllPlaylistsAfterCurrentSongs();
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.StopPlaylistAfterCurrentSong(aEvent.playlistControllerName);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.AddSongToQueue:
            playSoundResult4.SoundPlayed = false;
            if (string.IsNullOrEmpty(aEvent.clipName))
            {
              Debug.Log((object) $"You have not specified a song name for Event Sounds on '{this._trans.name}'.");
              return;
            }
            if (aEvent.playlistControllerName == "[None]")
              return;
            DarkTonic.MasterAudio.MasterAudio.QueuePlaylistClip(aEvent.playlistControllerName, aEvent.clipName);
            playSoundResult4.SoundPlayed = true;
            return;
          default:
            return;
        }
      case DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.CustomEventControl:
        if (eType == EventSounds.EventType.UserDefinedEvent)
        {
          Debug.LogError((object) $"Custom Event Receivers cannot fire events. Occured in Transform '{this.name}'.");
          break;
        }
        if (aEvent.currentCustomEventCommand != DarkTonic.MasterAudio.MasterAudio.CustomEventCommand.FireEvent)
          break;
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(aEvent.theCustomEventName, this._trans);
        break;
      case DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.GlobalControl:
        switch (aEvent.currentGlobalCommand)
        {
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.PauseMixer:
            DarkTonic.MasterAudio.MasterAudio.PauseMixer();
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.UnpauseMixer:
            DarkTonic.MasterAudio.MasterAudio.UnpauseMixer();
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.StopMixer:
            DarkTonic.MasterAudio.MasterAudio.StopMixer();
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.StopEverything:
            DarkTonic.MasterAudio.MasterAudio.StopEverything();
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.PauseEverything:
            DarkTonic.MasterAudio.MasterAudio.PauseEverything();
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.UnpauseEverything:
            DarkTonic.MasterAudio.MasterAudio.UnpauseEverything();
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.MuteEverything:
            DarkTonic.MasterAudio.MasterAudio.MuteEverything();
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.UnmuteEverything:
            DarkTonic.MasterAudio.MasterAudio.UnmuteEverything();
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.SetMasterMixerVolume:
            DarkTonic.MasterAudio.MasterAudio.MasterVolumeLevel = flag1 ? grp.sliderValue : aEvent.volume;
            return;
          case DarkTonic.MasterAudio.MasterAudio.GlobalCommand.SetMasterPlaylistVolume:
            DarkTonic.MasterAudio.MasterAudio.PlaylistMasterVolume = flag1 ? grp.sliderValue : aEvent.volume;
            return;
          default:
            return;
        }
      case DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.UnityMixerControl:
        switch (aEvent.currentMixerCommand)
        {
          case DarkTonic.MasterAudio.MasterAudio.UnityMixerCommand.TransitionToSnapshot:
            AudioMixerSnapshot snapshotToTransitionTo = aEvent.snapshotToTransitionTo;
            if (!((UnityEngine.Object) snapshotToTransitionTo != (UnityEngine.Object) null))
              return;
            snapshotToTransitionTo.audioMixer.TransitionToSnapshots(new AudioMixerSnapshot[1]
            {
              snapshotToTransitionTo
            }, new float[1]{ 1f }, aEvent.snapshotTransitionTime);
            return;
          case DarkTonic.MasterAudio.MasterAudio.UnityMixerCommand.TransitionToSnapshotBlend:
            List<AudioMixerSnapshot> audioMixerSnapshotList = new List<AudioMixerSnapshot>();
            List<float> floatList = new List<float>();
            AudioMixer audioMixer = (AudioMixer) null;
            for (int index = 0; index < aEvent.snapshotsToBlend.Count; ++index)
            {
              AudioEvent.MA_SnapshotInfo maSnapshotInfo = aEvent.snapshotsToBlend[index];
              if (!((UnityEngine.Object) maSnapshotInfo.snapshot == (UnityEngine.Object) null))
              {
                if ((UnityEngine.Object) audioMixer == (UnityEngine.Object) null)
                  audioMixer = maSnapshotInfo.snapshot.audioMixer;
                else if ((UnityEngine.Object) audioMixer != (UnityEngine.Object) maSnapshotInfo.snapshot.audioMixer)
                {
                  Debug.LogError((object) $"Snapshot '{maSnapshotInfo.snapshot.name}' isn't in the same Audio Mixer as the previous snapshot in EventSounds on GameObject '{this.name}'. Please make sure all the Snapshots to blend are on the same mixer.");
                  break;
                }
                audioMixerSnapshotList.Add(maSnapshotInfo.snapshot);
                floatList.Add(maSnapshotInfo.weight);
              }
            }
            if (audioMixerSnapshotList.Count <= 0)
              return;
            Debug.Log((object) "trans");
            audioMixer.TransitionToSnapshots(audioMixerSnapshotList.ToArray(), floatList.ToArray(), aEvent.snapshotTransitionTime);
            return;
          default:
            return;
        }
      case DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.PersistentSettingsControl:
        switch (aEvent.currentPersistentSettingsCommand)
        {
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.SetBusVolume:
            List<string> stringList3 = new List<string>();
            if (!aEvent.allSoundTypesForBusCmd)
              stringList3.Add(aEvent.busName);
            else
              stringList3.AddRange((IEnumerable<string>) DarkTonic.MasterAudio.MasterAudio.RuntimeBusNames);
            for (int index = 0; index < stringList3.Count; ++index)
              PersistentAudioSettings.SetBusVolume(stringList3[index], flag1 ? grp.sliderValue : aEvent.volume);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.SetGroupVolume:
            List<string> stringList4 = new List<string>();
            if (!aEvent.allSoundTypesForGroupCmd)
              stringList4.Add(aEvent.soundType);
            else
              stringList4.AddRange((IEnumerable<string>) DarkTonic.MasterAudio.MasterAudio.RuntimeSoundGroupNames);
            for (int index = 0; index < stringList4.Count; ++index)
              PersistentAudioSettings.SetGroupVolume(stringList4[index], flag1 ? grp.sliderValue : aEvent.volume);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.SetMixerVolume:
            PersistentAudioSettings.MixerVolume = new float?(flag1 ? grp.sliderValue : aEvent.volume);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.SetMusicVolume:
            PersistentAudioSettings.MusicVolume = new float?(flag1 ? grp.sliderValue : aEvent.volume);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.MixerMuteToggle:
            bool? mixerMuted = PersistentAudioSettings.MixerMuted;
            if (mixerMuted.HasValue)
            {
              mixerMuted = PersistentAudioSettings.MixerMuted;
              PersistentAudioSettings.MixerMuted = new bool?(!mixerMuted.Value);
              return;
            }
            PersistentAudioSettings.MixerMuted = new bool?(true);
            return;
          case DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand.MusicMuteToggle:
            bool? musicMuted = PersistentAudioSettings.MusicMuted;
            if (musicMuted.HasValue)
            {
              musicMuted = PersistentAudioSettings.MusicMuted;
              PersistentAudioSettings.MusicMuted = new bool?(!musicMuted.Value);
              return;
            }
            PersistentAudioSettings.MusicMuted = new bool?(true);
            return;
          default:
            return;
        }
    }
  }

  public void LogIfCustomEventMissing(AudioEventGroup eventGroup)
  {
    if (!this.logMissingEvents || eventGroup.isCustomEvent && (!eventGroup.customSoundActive || string.IsNullOrEmpty(eventGroup.customEventName)))
      return;
    for (int index = 0; index < eventGroup.SoundEvents.Count; ++index)
    {
      AudioEvent soundEvent = eventGroup.SoundEvents[index];
      if (soundEvent.currentSoundFunctionType == DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.CustomEventControl)
      {
        string theCustomEventName = soundEvent.theCustomEventName;
        if (!DarkTonic.MasterAudio.MasterAudio.CustomEventExists(theCustomEventName))
          DarkTonic.MasterAudio.MasterAudio.LogWarning($"Transform '{this.name}' is set up to receive or fire Custom Event '{theCustomEventName}', which does not exist in Master Audio.");
      }
    }
  }

  public void CheckForIllegalCustomEvents()
  {
    if (this.useStartSound)
      this.LogIfCustomEventMissing(this.startSound);
    if (this.useVisibleSound)
      this.LogIfCustomEventMissing(this.visibleSound);
    if (this.useInvisibleSound)
      this.LogIfCustomEventMissing(this.invisibleSound);
    if (this.useCollisionSound)
      this.LogIfCustomEventMissing(this.collisionSound);
    if (this.useCollisionExitSound)
      this.LogIfCustomEventMissing(this.collisionExitSound);
    if (this.useTriggerEnterSound)
      this.LogIfCustomEventMissing(this.triggerSound);
    if (this.useTriggerExitSound)
      this.LogIfCustomEventMissing(this.triggerExitSound);
    if (this.useMouseEnterSound)
      this.LogIfCustomEventMissing(this.mouseEnterSound);
    if (this.useMouseExitSound)
      this.LogIfCustomEventMissing(this.mouseExitSound);
    if (this.useMouseClickSound)
      this.LogIfCustomEventMissing(this.mouseClickSound);
    if (this.useMouseDragSound)
      this.LogIfCustomEventMissing(this.mouseDragSound);
    if (this.useMouseUpSound)
      this.LogIfCustomEventMissing(this.mouseUpSound);
    if (this.useNguiMouseDownSound)
      this.LogIfCustomEventMissing(this.nguiMouseDownSound);
    if (this.useNguiMouseUpSound)
      this.LogIfCustomEventMissing(this.nguiMouseUpSound);
    if (this.useNguiOnClickSound)
      this.LogIfCustomEventMissing(this.nguiOnClickSound);
    if (this.useNguiMouseEnterSound)
      this.LogIfCustomEventMissing(this.nguiMouseEnterSound);
    if (this.useNguiMouseExitSound)
      this.LogIfCustomEventMissing(this.nguiMouseExitSound);
    if (this.useSpawnedSound)
      this.LogIfCustomEventMissing(this.spawnedSound);
    if (this.useDespawnedSound)
      this.LogIfCustomEventMissing(this.despawnedSound);
    if (this.useEnableSound)
      this.LogIfCustomEventMissing(this.enableSound);
    if (this.useDisableSound)
      this.LogIfCustomEventMissing(this.disableSound);
    if (this.useCollision2dSound)
      this.LogIfCustomEventMissing(this.collision2dSound);
    if (this.useCollisionExit2dSound)
      this.LogIfCustomEventMissing(this.collisionExit2dSound);
    if (this.useTriggerEnter2dSound)
      this.LogIfCustomEventMissing(this.triggerEnter2dSound);
    if (this.useTriggerExit2dSound)
      this.LogIfCustomEventMissing(this.triggerExit2dSound);
    if (this.useParticleCollisionSound)
      this.LogIfCustomEventMissing(this.particleCollisionSound);
    if (this.useUnitySliderChangedSound)
      this.LogIfCustomEventMissing(this.unitySliderChangedSound);
    if (this.useUnityButtonClickedSound)
      this.LogIfCustomEventMissing(this.unityButtonClickedSound);
    if (this.useUnityPointerDownSound)
      this.LogIfCustomEventMissing(this.unityPointerDownSound);
    if (this.useUnityDragSound)
      this.LogIfCustomEventMissing(this.unityDragSound);
    if (this.useUnityDropSound)
      this.LogIfCustomEventMissing(this.unityDropSound);
    if (this.useUnityPointerUpSound)
      this.LogIfCustomEventMissing(this.unityPointerUpSound);
    if (this.useUnityPointerEnterSound)
      this.LogIfCustomEventMissing(this.unityPointerEnterSound);
    if (this.useUnityPointerExitSound)
      this.LogIfCustomEventMissing(this.unityPointerExitSound);
    if (this.useUnityScrollSound)
      this.LogIfCustomEventMissing(this.unityScrollSound);
    if (this.useUnityUpdateSelectedSound)
      this.LogIfCustomEventMissing(this.unityUpdateSelectedSound);
    if (this.useUnitySelectSound)
      this.LogIfCustomEventMissing(this.unitySelectSound);
    if (this.useUnityDeselectSound)
      this.LogIfCustomEventMissing(this.unityDeselectSound);
    if (this.useUnityMoveSound)
      this.LogIfCustomEventMissing(this.unityMoveSound);
    if (this.useUnityInitializePotentialDragSound)
      this.LogIfCustomEventMissing(this.unityInitializePotentialDragSound);
    if (this.useUnityBeginDragSound)
      this.LogIfCustomEventMissing(this.unityBeginDragSound);
    if (this.useUnityEndDragSound)
      this.LogIfCustomEventMissing(this.unityEndDragSound);
    if (this.useUnitySubmitSound)
      this.LogIfCustomEventMissing(this.unitySubmitSound);
    if (this.useUnityCancelSound)
      this.LogIfCustomEventMissing(this.unityCancelSound);
    for (int index = 0; index < this.userDefinedSounds.Count; ++index)
      this.LogIfCustomEventMissing(this.userDefinedSounds[index]);
  }

  public void ReceiveEvent(string customEventName, Vector3 originPoint)
  {
    for (int index = 0; index < this.userDefinedSounds.Count; ++index)
    {
      AudioEventGroup userDefinedSound = this.userDefinedSounds[index];
      if (userDefinedSound.customSoundActive && !string.IsNullOrEmpty(userDefinedSound.customEventName) && userDefinedSound.customEventName.Equals(customEventName))
        this.PlaySounds(userDefinedSound, EventSounds.EventType.UserDefinedEvent);
    }
  }

  public bool SubscribesToEvent(string customEventName)
  {
    for (int index = 0; index < this.userDefinedSounds.Count; ++index)
    {
      AudioEventGroup userDefinedSound = this.userDefinedSounds[index];
      if (userDefinedSound.customSoundActive && !string.IsNullOrEmpty(userDefinedSound.customEventName) && userDefinedSound.customEventName.Equals(customEventName))
        return true;
    }
    return false;
  }

  public void RegisterReceiver()
  {
    if (this.userDefinedSounds.Count <= 0)
      return;
    DarkTonic.MasterAudio.MasterAudio.AddCustomEventReceiver((ICustomEventReceiver) this, this._trans);
  }

  public void UnregisterReceiver()
  {
    if (this.userDefinedSounds.Count <= 0)
      return;
    DarkTonic.MasterAudio.MasterAudio.RemoveCustomEventReceiver((ICustomEventReceiver) this);
  }

  public IList<AudioEventGroup> GetAllEvents()
  {
    return (IList<AudioEventGroup>) this.userDefinedSounds.AsReadOnly();
  }

  public void AddUGUIComponents()
  {
    this.AddUGUIHandler<EventSoundsPointerEnterHandler>(this.useUnityPointerEnterSound);
    this.AddUGUIHandler<EventSoundsPointerExitHandler>(this.useUnityPointerExitSound);
    this.AddUGUIHandler<EventSoundsPointerDownHandler>(this.useUnityPointerDownSound);
    this.AddUGUIHandler<EventSoundsPointerUpHandler>(this.useUnityPointerUpSound);
    this.AddUGUIHandler<EventSoundsDragHandler>(this.useUnityDragSound);
    this.AddUGUIHandler<EventSoundsDropHandler>(this.useUnityDropSound);
    this.AddUGUIHandler<EventSoundsScrollHandler>(this.useUnityScrollSound);
    this.AddUGUIHandler<EventSoundsUpdateSelectedHandler>(this.useUnityUpdateSelectedSound);
    this.AddUGUIHandler<EventSoundsSelectHandler>(this.useUnitySelectSound);
    this.AddUGUIHandler<EventSoundsDeselectHandler>(this.useUnityDeselectSound);
    this.AddUGUIHandler<EventSoundsMoveHandler>(this.useUnityMoveSound);
    this.AddUGUIHandler<EventSoundsInitializePotentialDragHandler>(this.useUnityInitializePotentialDragSound);
    this.AddUGUIHandler<EventSoundsBeginDragHandler>(this.useUnityBeginDragSound);
    this.AddUGUIHandler<EventSoundsEndDragHandler>(this.useUnityEndDragSound);
    this.AddUGUIHandler<EventSoundsSubmitHandler>(this.useUnitySubmitSound);
    this.AddUGUIHandler<EventSoundsCancelHandler>(this.useUnityCancelSound);
  }

  public void AddUGUIHandler<T>(bool useSound) where T : EventSoundsUGUIHandler
  {
    if (!useSound)
      return;
    this.gameObject.AddComponent<T>().eventSounds = this;
  }

  public enum UnityUIVersion
  {
    Legacy,
    uGUI,
  }

  public enum EventType
  {
    OnStart,
    OnVisible,
    OnInvisible,
    OnCollision,
    OnTriggerEnter,
    OnTriggerExit,
    OnMouseEnter,
    OnMouseClick,
    OnSpawned,
    OnDespawned,
    OnEnable,
    OnDisable,
    OnCollision2D,
    OnTriggerEnter2D,
    OnTriggerExit2D,
    OnParticleCollision,
    UserDefinedEvent,
    OnCollisionExit,
    OnCollisionExit2D,
    OnMouseUp,
    OnMouseExit,
    OnMouseDrag,
    NGUIOnClick,
    NGUIMouseDown,
    NGUIMouseUp,
    NGUIMouseEnter,
    NGUIMouseExit,
    MechanimStateChanged,
    UnitySliderChanged,
    UnityButtonClicked,
    UnityPointerDown,
    UnityPointerUp,
    UnityPointerEnter,
    UnityPointerExit,
    UnityDrag,
    UnityDrop,
    UnityScroll,
    UnityUpdateSelected,
    UnitySelect,
    UnityDeselect,
    UnityMove,
    UnityInitializePotentialDrag,
    UnityBeginDrag,
    UnityEndDrag,
    UnitySubmit,
    UnityCancel,
    UnityToggle,
    OnTriggerStay,
    OnTriggerStay2D,
  }

  public enum GlidePitchType
  {
    None,
    RaisePitch,
    LowerPitch,
  }

  public enum VariationType
  {
    PlaySpecific,
    PlayRandom,
  }

  public enum PreviousSoundStopMode
  {
    None,
    Stop,
    FadeOut,
  }

  public enum RetriggerLimMode
  {
    None,
    FrameBased,
    TimeBased,
  }
}
