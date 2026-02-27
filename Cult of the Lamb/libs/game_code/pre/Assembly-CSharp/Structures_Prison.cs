// Decompiled with JetBrains decompiler
// Type: Structures_Prison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Prison : StructureBrain
{
  public override void OnAdded()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public override void OnRemoved()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  private void OnNewPhaseStarted()
  {
    if (this.Data.FollowerID == -1)
      return;
    FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Data.FollowerID);
    if (infoById == null)
      return;
    FollowerBrain.GetOrCreateBrain(infoById)?.AddThought(Thought.PrisonReEducation, forced: true);
  }

  public void Reeducate(FollowerBrain brain)
  {
    for (int index = 0; index < 4; ++index)
      brain.AddThought(Thought.PrisonReEducation, forced: true);
    brain.Stats.Reeducation -= 25f;
    if ((double) brain.Stats.Reeducation <= 0.0 || (double) brain.Stats.Reeducation >= 2.0)
      return;
    brain.Stats.Reeducation = 0.0f;
  }

  public static void RemoveFollower(int ID)
  {
    List<Structures_Prison> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Prison>();
    for (int index = 0; index < structuresOfType.Count; ++index)
    {
      if (structuresOfType[index].Data.FollowerID == ID)
        structuresOfType[index].Data.FollowerID = -1;
    }
    FollowerInfo infoById = FollowerInfo.GetInfoByID(ID, true);
    if (!DataManager.Instance.Followers_Imprisoned_IDs.Contains(ID) || infoById != null && infoById.DiedInPrison)
      return;
    DataManager.Instance.Followers_Imprisoned_IDs.Remove(ID);
  }
}
