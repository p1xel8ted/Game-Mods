// Decompiled with JetBrains decompiler
// Type: WorshipperBubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WorshipperBubble : BaseMonoBehaviour
{
  public SpriteRenderer spriteRenderer;
  public float Scale;
  public float ScaleSpeed;
  public GameObject Player;
  public float Timer;
  public Dictionary<WorshipperBubble.SPEECH_TYPE, Sprite> Bubbles = new Dictionary<WorshipperBubble.SPEECH_TYPE, Sprite>();
  [SerializeField]
  public List<WorshipperBubble.MyDictionaryEntry> BubbleImages;
  public Dictionary<WorshipperBubble.SPEECH_TYPE, Sprite> BubblesDictionary;
  public Worshipper worshipper;
  [CompilerGenerated]
  public bool \u003CActive\u003Ek__BackingField;
  public System.Action OnBubblePlay;
  public System.Action OnBubbleHide;
  public bool Closing;
  public Villager_Info v_i;
  public float Duration;

  public bool Active
  {
    get => this.\u003CActive\u003Ek__BackingField;
    set => this.\u003CActive\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    this.spriteRenderer.sprite = (Sprite) null;
    this.Player = GameObject.FindWithTag("Player");
    this.worshipper = this.GetComponentInParent<Worshipper>();
    this.BubblesDictionary = new Dictionary<WorshipperBubble.SPEECH_TYPE, Sprite>();
    foreach (WorshipperBubble.MyDictionaryEntry bubbleImage in this.BubbleImages)
      this.BubblesDictionary.Add(bubbleImage.key, bubbleImage.value);
  }

  public void Update()
  {
    if (!((UnityEngine.Object) this.spriteRenderer.sprite != (UnityEngine.Object) null))
      return;
    if (!this.Closing)
    {
      if ((double) Time.timeScale > 0.0)
      {
        this.ScaleSpeed += (float) ((1.0 - (double) this.Scale) * 0.30000001192092896) / Time.deltaTime;
        this.Scale += (this.ScaleSpeed *= 0.7f) * Time.deltaTime;
        if (!float.IsNaN(this.Scale))
        {
          this.Scale = Mathf.Min(this.Scale, 1.3f);
          this.transform.localScale = new Vector3(this.Scale, this.Scale);
        }
      }
      if ((double) (this.Timer += Time.deltaTime) > (double) this.Duration)
        this.Close();
      if (!((UnityEngine.Object) this.worshipper != (UnityEngine.Object) null) || !this.worshipper.BeingCarried)
        return;
      this.Close();
    }
    else
    {
      if ((double) Time.timeScale <= 0.0)
        return;
      this.ScaleSpeed -= (float) (0.0099999997764825821 * (double) Time.deltaTime * 60.0);
      this.Scale += this.ScaleSpeed;
      if (float.IsNaN(this.Scale))
        this.Scale = 0.0f;
      if (!float.IsNaN(this.Scale))
      {
        this.Scale = Mathf.Min(this.Scale, 1.3f);
        this.transform.localScale = new Vector3(this.Scale, this.Scale);
      }
      if ((double) this.Scale > 0.0)
        return;
      this.spriteRenderer.sprite = (Sprite) null;
      this.Active = false;
    }
  }

  public void OnDisable() => this.Active = false;

  public void Close()
  {
    if (this.Closing)
      return;
    this.ScaleSpeed = 0.07f;
    this.Closing = true;
    System.Action onBubbleHide = this.OnBubbleHide;
    if (onBubbleHide == null)
      return;
    onBubbleHide();
  }

  public void Play(WorshipperBubble.SPEECH_TYPE Type, float Duration = 4f, float Delay = 0.0f)
  {
    if (CheatConsole.HidingUI || !this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine((IEnumerator) this.PlayRoutine(Type, Duration, Delay));
    this.Active = true;
    System.Action onBubblePlay = this.OnBubblePlay;
    if (onBubblePlay == null)
      return;
    onBubblePlay();
  }

  public IEnumerator PlayRoutine(WorshipperBubble.SPEECH_TYPE Type, float Duration = 4f, float Delay = 0.0f)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    WorshipperBubble worshipperBubble = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/followers/speech_bubble", worshipperBubble.transform.position);
      worshipperBubble.spriteRenderer.sprite = worshipperBubble.BubblesDictionary[Type];
      worshipperBubble.Scale = 0.0f;
      worshipperBubble.Timer = 0.0f;
      worshipperBubble.Closing = false;
      worshipperBubble.Duration = Duration - 1.6f - Delay;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(Delay);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public enum SPEECH_TYPE
  {
    FOOD,
    HOME,
    HELP,
    LOVE,
    ENEMIES,
    FRIENDS,
    DISSENTER1,
    DISSENTER2,
    DISSENTER3,
    BOSSCROWN1,
    BOSSCROWN2,
    BOSSCROWN3,
    BOSSCROWN4,
    DISSENTARGUE,
    FOLLOWERMEAT,
    READY,
    TWITCH,
    SIN,
    STARVING,
    ILL,
  }

  [Serializable]
  public class MyDictionaryEntry
  {
    public WorshipperBubble.SPEECH_TYPE key;
    public Sprite value;
  }
}
