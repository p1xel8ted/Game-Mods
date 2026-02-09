// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.DungeonPattern
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace DungeonGenerator;

public class DungeonPattern
{
  public bool[] step_pattern;
  public bool[] hit_pattern;
  public int xmax;
  public int ymax;
  public static List<DungeonPattern> _patterns = (List<DungeonPattern>) null;
  public const int MAX_THICKNESS = 7;
  public const int STEP_PATTERN_SIZE_BITS = 8;
  public const int STEP_PATTERN_SIZE = 256 /*0x0100*/;
  public static int[][,] STEP_PATTERNS = new int[5][,]
  {
    new int[1, 1],
    new int[1, 1]{ { 1 } },
    new int[3, 3]{ { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } },
    new int[5, 5]
    {
      {
        1,
        1,
        1,
        1,
        1
      },
      {
        1,
        1,
        1,
        1,
        1
      },
      {
        1,
        1,
        1,
        1,
        1
      },
      {
        1,
        1,
        1,
        1,
        1
      },
      {
        1,
        1,
        1,
        1,
        1
      }
    },
    new int[7, 7]
    {
      {
        0,
        0,
        1,
        1,
        1,
        0,
        0
      },
      {
        0,
        1,
        1,
        1,
        1,
        1,
        0
      },
      {
        1,
        1,
        1,
        1,
        1,
        1,
        1
      },
      {
        1,
        1,
        1,
        1,
        1,
        1,
        1
      },
      {
        1,
        1,
        1,
        1,
        1,
        1,
        1
      },
      {
        0,
        1,
        1,
        1,
        1,
        1,
        0
      },
      {
        0,
        0,
        1,
        1,
        1,
        0,
        0
      }
    }
  };

  public static void InitPatternsCache()
  {
    DungeonPattern._patterns = new List<DungeonPattern>();
    for (int thickness = 0; thickness <= 7; ++thickness)
      DungeonPattern._patterns.Add(new DungeonPattern(thickness));
  }

  public static DungeonPattern GetPattern(int thickness) => DungeonPattern._patterns[thickness];

  public DungeonPattern(int thickness)
  {
    this.step_pattern = new bool[65536 /*0x010000*/];
    this.hit_pattern = new bool[65536 /*0x010000*/];
    for (int index = 0; index < this.step_pattern.Length; ++index)
      this.step_pattern[index] = this.hit_pattern[index] = false;
    int[,] numArray;
    if (thickness < DungeonPattern.STEP_PATTERNS.Length)
    {
      numArray = DungeonPattern.STEP_PATTERNS[thickness];
    }
    else
    {
      numArray = new int[2 * thickness - 1, 2 * thickness - 1];
      for (int index1 = 0; index1 < 2 * thickness - 1; ++index1)
      {
        for (int index2 = 0; index2 < 2 * thickness - 1; ++index2)
          numArray[index1, index2] = 1;
      }
    }
    this.xmax = numArray.GetUpperBound(0);
    this.ymax = numArray.GetUpperBound(1);
    for (int index3 = 0; index3 <= this.ymax; ++index3)
    {
      for (int index4 = 0; index4 <= this.xmax; ++index4)
        this.step_pattern[(index3 << 8) + index4] = numArray[index4, index3] == 1;
    }
    this.GenerateHitPattern(thickness);
  }

  public void GenerateHitPattern(int thickness)
  {
    if (thickness == 1)
    {
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
          this.hit_pattern[index1 + (index2 << 8)] = true;
      }
    }
    else
    {
      for (int index3 = 0; index3 < 2 * thickness - 1; ++index3)
      {
        for (int index4 = 0; index4 < 2 * thickness - 1; ++index4)
          this.hit_pattern[index3 + 1 + (index4 + 1 << 8)] = this.step_pattern[index3 + (index4 << 8)];
      }
      for (int index5 = 0; index5 < thickness + 1; ++index5)
      {
        for (int index6 = 0; index6 < thickness + 1; ++index6)
        {
          if (this.hit_pattern[index6 + 1 + (index5 << 8)] || this.hit_pattern[index6 + (index5 + 1 << 8)] || this.hit_pattern[index6 + 1 + (index5 + 1 << 8)])
          {
            this.hit_pattern[index6 + (index5 << 8)] = true;
            this.hit_pattern[2 * thickness - index6 + (index5 << 8)] = true;
            this.hit_pattern[index6 + (2 * thickness - index5 << 8)] = true;
            this.hit_pattern[2 * thickness - index6 + (2 * thickness - index5 << 8)] = true;
          }
          else
            this.hit_pattern[index6 + (index5 << 8)] = false;
        }
      }
    }
  }
}
