// Decompiled with JetBrains decompiler
// Type: EnemyConversation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using UnityEngine;

#nullable disable
public class EnemyConversation : BaseMonoBehaviour
{
  [SerializeField]
  private GameObject speechEffectCalm;
  [SerializeField]
  private GameObject speechEffectStressed;
  private static float speechDurationCalm = 1f;
  private static float speechDurationStressed = 0.4f;
  private static float minDelayBetweenSpeechesCalm = 3f;
  private static float minDelayBetweenSpeechesStressed = 2f;
  private float nextSpeechTimestamp;
  private EnemyConversation.EnemySpeech currentSpeech;
  private EnemyConversation.EnemySpeech lastSpeech;
  private GameManager gm;

  private void OnEnable()
  {
    this.gm = GameManager.GetInstance();
    this.nextSpeechTimestamp = EnemyConversation.minDelayBetweenSpeechesCalm * Random.Range(1f, 3f);
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
    if ((Object) this.speechEffectCalm != (Object) null)
      this.speechEffectCalm.SetActive(false);
    if (!((Object) this.speechEffectStressed != (Object) null))
      return;
    this.speechEffectStressed.SetActive(false);
  }

  private void OnDisable()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  private void Update()
  {
    if ((Object) this.gm == (Object) null)
      this.gm = GameManager.GetInstance();
    else if (this.currentSpeech != null)
    {
      if ((double) this.gm.TimeSince(this.currentSpeech.Timestamp) >= (this.currentSpeech.SpeechType == EnemyConversation.SpeechType.Calm ? (double) EnemyConversation.speechDurationCalm : (double) EnemyConversation.speechDurationStressed))
        this.StopSpeech();
      else if ((Object) this.currentSpeech.Speaker == (Object) null)
        this.StopSpeech();
      else if (this.currentSpeech.SpeechType == EnemyConversation.SpeechType.Calm)
      {
        if (!((Object) this.speechEffectCalm != (Object) null))
          return;
        this.speechEffectCalm.transform.position = this.currentSpeech.Speaker.position;
      }
      else
      {
        if (!((Object) this.speechEffectStressed != (Object) null))
          return;
        this.speechEffectStressed.transform.position = this.currentSpeech.Speaker.position;
      }
    }
    else
    {
      if (Health.team2.Count <= 1 || (double) this.gm.CurrentTime < (double) this.nextSpeechTimestamp)
        return;
      Health health = (Health) null;
      int num = Random.Range(0, Health.team2.Count - 1);
      for (int index1 = 0; index1 < Health.team2.Count; ++index1)
      {
        int index2 = (index1 + num) % Health.team2.Count;
        if (index2 >= Health.team2.Count)
          index2 -= Health.team2.Count;
        if (!((Object) Health.team2[index2] == (Object) null) && (double) Health.team2[index2].HP != 0.0 && (this.lastSpeech == null || !((Object) this.lastSpeech.Speaker != (Object) null) || !((Object) this.lastSpeech.Speaker == (Object) health)) && (Object) health != (Object) null)
          this.Speak(health.transform, health.Unaware ? EnemyConversation.SpeechType.Calm : EnemyConversation.SpeechType.Stressed);
      }
    }
  }

  private void Speak(Transform speaker, EnemyConversation.SpeechType speechType)
  {
    if ((Object) speaker == (Object) null)
      return;
    this.currentSpeech = new EnemyConversation.EnemySpeech(speaker, this.gm.CurrentTime, speechType);
    this.lastSpeech = this.currentSpeech;
    if (this.currentSpeech.SpeechType != EnemyConversation.SpeechType.Calm)
    {
      double durationStressed = (double) EnemyConversation.speechDurationStressed;
    }
    else
    {
      double speechDurationCalm = (double) EnemyConversation.speechDurationCalm;
    }
    this.nextSpeechTimestamp = this.currentSpeech.Timestamp + (this.currentSpeech.SpeechType == EnemyConversation.SpeechType.Calm ? EnemyConversation.minDelayBetweenSpeechesCalm : EnemyConversation.minDelayBetweenSpeechesStressed) * Random.Range(1f, 2f);
    if (speechType == EnemyConversation.SpeechType.Calm)
    {
      if ((Object) this.speechEffectCalm != (Object) null)
        this.speechEffectCalm?.SetActive(true);
      if ((Object) this.speechEffectStressed != (Object) null)
        this.speechEffectStressed?.SetActive(false);
      Debug.Log((object) (speaker.gameObject.name + " calmly says 'blah blah blah'"));
    }
    else
    {
      if ((Object) this.speechEffectStressed != (Object) null)
        this.speechEffectStressed.SetActive(true);
      if ((Object) this.speechEffectCalm != (Object) null)
        this.speechEffectCalm.SetActive(false);
      Debug.Log((object) (speaker.gameObject.name + " shouts 'BLERGH!'"));
    }
  }

  private void StopSpeech()
  {
    if (this.currentSpeech == null)
      return;
    if ((Object) this.speechEffectCalm != (Object) null)
      this.speechEffectCalm?.SetActive(false);
    if ((Object) this.speechEffectStressed != (Object) null)
      this.speechEffectStressed?.SetActive(false);
    this.currentSpeech = (EnemyConversation.EnemySpeech) null;
  }

  private void OnBiomeChangeRoom() => this.StopSpeech();

  private enum SpeechType
  {
    Calm,
    Stressed,
  }

  private class EnemySpeech
  {
    public Transform Speaker;
    public float Timestamp;
    public EnemyConversation.SpeechType SpeechType;

    public EnemySpeech(Transform speaker, float timestamp, EnemyConversation.SpeechType speechType)
    {
      this.Speaker = speaker;
      this.Timestamp = timestamp;
      this.SpeechType = speechType;
    }
  }
}
