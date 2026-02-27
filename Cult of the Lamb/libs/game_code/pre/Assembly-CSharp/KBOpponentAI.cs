// Decompiled with JetBrains decompiler
// Type: KBOpponentAI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using KnuckleBones;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KBOpponentAI
{
  private float _randomStupidity;

  public KBOpponentAI(float stupidity) => this._randomStupidity = stupidity;

  public int Evaluate(List<KBDiceTub> tubs, Dice newDice)
  {
    if ((double) Random.value * 10.0 > (double) this._randomStupidity)
    {
      List<int> intList = new List<int>();
      int num1 = -1;
      while (++num1 < tubs.Count)
      {
        Debug.Log((object) num1);
        if (tubs[num1].Dice.Count < 3)
          intList.Add(num1);
      }
      int num2 = intList[Random.Range(0, intList.Count)];
      Debug.Log((object) ("BE STUPID! " + (object) num2));
      return num2;
    }
    Debug.Log((object) "BE SMART");
    List<int> intList1 = new List<int>() { 0, 0, 0 };
    int index1 = -1;
    while (++index1 < tubs.Count)
    {
      KBDiceTub tub = tubs[index1];
      if (tub.Dice.Count >= 3)
      {
        intList1[index1] = int.MinValue;
      }
      else
      {
        switch (tub.OpponentTub.NumMatchingDice(newDice.Num))
        {
          case 0:
            List<int> intList2 = intList1;
            int index2 = index1;
            intList2[index2] = intList2[index2];
            break;
          case 1:
            ++intList1[index1];
            break;
          case 2:
            intList1[index1] += 2;
            break;
          case 3:
            intList1[index1] += 5;
            break;
        }
        switch (tub.NumMatchingDice(newDice.Num))
        {
          case 0:
            List<int> intList3 = intList1;
            int index3 = index1;
            intList3[index3] = intList3[index3];
            continue;
          case 1:
            ++intList1[index1];
            continue;
          case 2:
            intList1[index1] += 2;
            continue;
          case 3:
            intList1[index1] += 5;
            continue;
          default:
            continue;
        }
      }
    }
    int minValue = int.MinValue;
    int index4 = -1;
    while (++index4 < intList1.Count)
    {
      if (intList1[index4] > minValue)
        minValue = intList1[index4];
    }
    List<int> intList4 = new List<int>();
    int index5 = -1;
    while (++index5 < intList1.Count)
    {
      if (intList1[index5] == minValue)
        intList4.Add(index5);
    }
    return intList4[Random.Range(0, intList4.Count)];
  }
}
