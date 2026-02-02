// Decompiled with JetBrains decompiler
// Type: Interaction_RanchChoppingBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.RanchSelect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_RanchChoppingBlock : Interaction
{
  [SerializeField]
  public GameObject animalPos;
  [SerializeField]
  public GameObject playerPos;
  [SerializeField]
  public ParticleSystem sacrificeVFX;

  public override void GetLabel()
  {
    if (Interaction_WolfBase.WolvesActive)
    {
      this.Interactable = false;
      this.Label = "";
    }
    else
    {
      this.Interactable = true;
      this.Label = LocalizationManager.GetTranslation("UI/KillAnimal");
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    List<Structures_Ranch> ranches = StructureManager.GetAllStructuresOfType<Structures_Ranch>();
    List<RanchSelectEntry> animal1 = new List<RanchSelectEntry>();
    foreach (StructureBrain structureBrain in ranches)
    {
      foreach (StructuresData.Ranchable_Animal animal2 in structureBrain.Data.Animals)
      {
        if (animal2.State != Interaction_Ranchable.State.Dead)
        {
          RanchSelectEntry ranchSelectEntry = new RanchSelectEntry(animal2);
          animal1.Add(ranchSelectEntry);
        }
      }
    }
    UIRanchSelectMenuController itemSelector = MonoSingleton<UIManager>.Instance.ShowRanchSacrificeMenu(animal1);
    itemSelector.SetHoldText(LocalizationManager.GetTranslation("UI/KillAnimal"));
    UIRanchSelectMenuController selectMenuController = itemSelector;
    selectMenuController.OnAnimalSelected = selectMenuController.OnAnimalSelected + (System.Action<RanchSelectEntry>) (sacrificedRanchable =>
    {
      int id = sacrificedRanchable.AnimalInfo.ID;
      int type = (int) sacrificedRanchable.AnimalInfo.Type;
      int age = sacrificedRanchable.AnimalInfo.Age;
      Interaction_Ranchable animal3 = Interaction_Ranchable.Ranchables.Find((Predicate<Interaction_Ranchable>) (x => x.Animal.ID == id));
      foreach (Structures_Ranch structuresRanch in ranches)
      {
        int num1 = structuresRanch.IsOvercrowded ? 1 : 0;
        structuresRanch.RemoveAnimal(sacrificedRanchable.AnimalInfo);
        int num2 = structuresRanch.IsOvercrowded ? 1 : 0;
        if (num1 != num2 && (UnityEngine.Object) animal3.ranch != (UnityEngine.Object) null)
          animal3.NotifyOvercrowded(animal3.ranch);
      }
      this.StartCoroutine((IEnumerator) this.SacrificeRanchableRoutine(animal3));
      itemSelector.Hide();
    });
  }

  public IEnumerator SacrificeRanchableRoutine(Interaction_Ranchable animal)
  {
    Interaction_RanchChoppingBlock ranchChoppingBlock = this;
    ranchChoppingBlock.Outliner.OutlineLayers[0].Clear();
    animal.CurrentState = Interaction_Ranchable.State.Default;
    animal.UnitObject.enabled = false;
    animal.gameObject.SetActive(true);
    animal.transform.SetParent(ranchChoppingBlock.transform);
    ranchChoppingBlock.sacrificeVFX.Play();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SlaughterAnimal);
    ranchChoppingBlock.playerFarming.GoToAndStop(ranchChoppingBlock.playerPos, ranchChoppingBlock.gameObject);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(ranchChoppingBlock.animalPos.gameObject, 6f);
    animal.CurrentState = Interaction_Ranchable.State.Overcrowded;
    animal.transform.position = ranchChoppingBlock.animalPos.transform.position;
    animal.UnitObject.LockToGround = false;
    animal.playerFarming = ranchChoppingBlock.playerFarming;
    yield return (object) new WaitForSeconds(1f);
    ranchChoppingBlock.playerFarming.EndGoToAndStop();
    if (animal.Animal.Age < 2)
      CultFaithManager.AddThought(Thought.Cult_BabyRanchAnimalButchered);
    else
      CultFaithManager.AddThought(Thought.Cult_RanchAnimalButchered);
    yield return (object) ranchChoppingBlock.StartCoroutine((IEnumerator) animal.SlaughterIE());
  }
}
