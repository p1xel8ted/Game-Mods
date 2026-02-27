// Decompiled with JetBrains decompiler
// Type: Interaction_ChaliceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_ChaliceManager : Interaction
{
  public List<Interaction_Chalice> Chalices = new List<Interaction_Chalice>();
  private bool Activated;

  private void Start()
  {
    this.AutomaticallyInteract = true;
    int index = Random.Range(0, this.Chalices.Count);
    foreach (Interaction_Chalice chalice in this.Chalices)
      chalice.Drink = Interaction_Chalice.DrinkType.Poison;
    this.Chalices[index].Drink = Interaction_Chalice.DrinkType.Vitality;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Activated = true;
    this.StartCoroutine((IEnumerator) this.MixDrinksUp());
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : "   ";

  private IEnumerator MixDrinksUp()
  {
    Interaction_ChaliceManager interactionChaliceManager = this;
    foreach (Interaction_Chalice chalice in interactionChaliceManager.Chalices)
      chalice.Activating = true;
    yield return (object) interactionChaliceManager.StartCoroutine((IEnumerator) interactionChaliceManager.ShowPoison());
    int Loops = 5;
    while (Loops > 0)
    {
      yield return (object) interactionChaliceManager.StartCoroutine((IEnumerator) interactionChaliceManager.MixDrink());
      --Loops;
      yield return (object) null;
    }
    foreach (Interaction_Chalice chalice in interactionChaliceManager.Chalices)
      chalice.Activating = false;
  }

  private IEnumerator ShowPoison()
  {
    yield return (object) new WaitForSeconds(1f);
    foreach (Interaction_Chalice chalice in this.Chalices)
      chalice.Spine.AnimationState.SetAnimation(0, chalice.Drink == Interaction_Chalice.DrinkType.Poison ? "show-bad" : "show-good", true);
    yield return (object) new WaitForSeconds(1.33333337f);
    foreach (Interaction_Chalice chalice in this.Chalices)
      chalice.Spine.AnimationState.SetAnimation(0, "idle", true);
    yield return (object) new WaitForSeconds(0.2f);
  }

  private IEnumerator MixDrink()
  {
    Interaction_Chalice C1 = this.Chalices[Random.Range(0, this.Chalices.Count)];
    Interaction_Chalice C2 = this.Chalices[Random.Range(0, this.Chalices.Count)];
    while ((Object) C1 == (Object) C2)
      C2 = this.Chalices[Random.Range(0, this.Chalices.Count)];
    float Progress = 0.0f;
    float Duration = 0.2f;
    Vector3 C1Position = C1.transform.position;
    Vector3 C2Position = C2.transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      C1.transform.position = Vector3.Lerp(C1Position, C2Position, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      C2.transform.position = Vector3.Lerp(C2Position, C1Position, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    C1.transform.position = C2Position;
    C2.transform.position = C1Position;
    Debug.Log((object) "Complete mixing");
    yield return (object) new WaitForSeconds(0.2f);
  }
}
