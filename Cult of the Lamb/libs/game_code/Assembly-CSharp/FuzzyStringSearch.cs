// Decompiled with JetBrains decompiler
// Type: FuzzyStringSearch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Pool;

#nullable disable
public static class FuzzyStringSearch
{
  public static FuzzyStringSearch.FuzzySearchResult GreedyFuzzyMatchScore(
    string queryStr,
    string candidateStr,
    int highlightThreshold,
    Func<string, int, int, string> highlightFunc)
  {
    FuzzyStringSearch.FuzzySearchResult fuzzySearchResult = new FuzzyStringSearch.FuzzySearchResult();
    fuzzySearchResult.OriginalString = candidateStr;
    string lowerInvariant1 = queryStr.ToLowerInvariant();
    string lowerInvariant2 = candidateStr.ToLowerInvariant();
    int length1 = lowerInvariant1.Length;
    int length2 = lowerInvariant2.Length;
    List<int> positions;
    using (CollectionPool<List<int>, int>.Get(out positions))
    {
      int index1 = 0;
      for (int index2 = 0; index1 < length1 && index2 < length2; ++index2)
      {
        if ((int) lowerInvariant1[index1] == (int) lowerInvariant2[index2])
        {
          positions.Add(index2);
          ++index1;
        }
      }
      if (index1 < length1)
      {
        fuzzySearchResult.Match = false;
        fuzzySearchResult.Score = 0;
        fuzzySearchResult.Highlighted = (Func<string>) (() => candidateStr);
        fuzzySearchResult.Segments = new List<FuzzyStringSearch.SequenceRun>();
        return fuzzySearchResult;
      }
      fuzzySearchResult.Match = true;
      fuzzySearchResult.Score = 0;
      int num1 = -2;
      for (int index3 = 0; index3 < positions.Count; ++index3)
      {
        int index4 = positions[index3];
        int num2 = 10;
        int num3;
        if (index4 == num1 + 1)
        {
          num3 = num2 + 5;
        }
        else
        {
          int num4 = index4 - num1 - 1;
          num3 = num2 - num4;
        }
        if (index4 > 0 && FuzzyStringSearch.IsWordBoundary(candidateStr[index4 - 1], candidateStr[index4]))
          num3 += 7;
        if (index4 == 0)
          num3 += 15;
        fuzzySearchResult.Score += num3;
        num1 = index4;
      }
      List<FuzzyStringSearch.SequenceRun> runs = new List<FuzzyStringSearch.SequenceRun>();
      FuzzyStringSearch.CompressPositionList(positions, runs);
      fuzzySearchResult.Segments = runs;
      fuzzySearchResult.Highlighted = fuzzySearchResult.Score < highlightThreshold || highlightFunc == null ? (Func<string>) (() => candidateStr) : (Func<string>) (() =>
      {
        string str = candidateStr;
        for (int index5 = runs.Count - 1; index5 >= 0; --index5)
        {
          FuzzyStringSearch.SequenceRun sequenceRun = runs[index5];
          str = highlightFunc(str, sequenceRun.Start, sequenceRun.Length);
        }
        return str;
      });
    }
    return fuzzySearchResult;
  }

  public static bool IsWordBoundary(char prevChar, char currentChar)
  {
    if ("/_- ".Contains(prevChar))
      return true;
    return char.IsLower(prevChar) && char.IsUpper(currentChar);
  }

  public static void CompressPositionList(
    List<int> positions,
    List<FuzzyStringSearch.SequenceRun> runs)
  {
    if (positions == null || runs == null || positions.Count <= 0)
      return;
    int num1 = positions[0];
    int num2 = positions[0];
    int num3 = 1;
    FuzzyStringSearch.SequenceRun sequenceRun1;
    for (int index = 1; index < positions.Count; ++index)
    {
      int position = positions[index];
      if (position == num2 + 1)
      {
        num2 = position;
        ++num3;
      }
      else
      {
        List<FuzzyStringSearch.SequenceRun> sequenceRunList = runs;
        sequenceRun1 = new FuzzyStringSearch.SequenceRun();
        sequenceRun1.Start = num1;
        sequenceRun1.Length = num3;
        FuzzyStringSearch.SequenceRun sequenceRun2 = sequenceRun1;
        sequenceRunList.Add(sequenceRun2);
        num3 = 1;
        num1 = position;
        num2 = position;
      }
    }
    List<FuzzyStringSearch.SequenceRun> sequenceRunList1 = runs;
    sequenceRun1 = new FuzzyStringSearch.SequenceRun();
    sequenceRun1.Start = num1;
    sequenceRun1.Length = num3;
    FuzzyStringSearch.SequenceRun sequenceRun3 = sequenceRun1;
    sequenceRunList1.Add(sequenceRun3);
  }

  public class FuzzySearchResult
  {
    public bool Match;
    public int Score;
    public string OriginalString = string.Empty;
    public List<FuzzyStringSearch.SequenceRun> Segments;
    public Func<string> Highlighted;
  }

  public struct SequenceRun
  {
    public int Start;
    public int Length;
  }

  public static class Const
  {
    public const string s_WordBoundaryChars = "/_- ";
  }
}
