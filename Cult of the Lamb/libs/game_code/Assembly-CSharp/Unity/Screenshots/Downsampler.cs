// Decompiled with JetBrains decompiler
// Type: Unity.Screenshots.Downsampler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Unity.Screenshots;

public static class Downsampler
{
  public static byte[] Downsample(
    byte[] dataRgba,
    int stride,
    int maximumWidth,
    int maximumHeight,
    out int downsampledStride)
  {
    if (stride == 0)
      throw new ArgumentException("The stride must be greater than 0.");
    if (stride % 4 != 0)
      throw new ArgumentException("The stride must be evenly divisible by 4.");
    if (dataRgba == null)
      throw new ArgumentNullException(nameof (dataRgba));
    if (dataRgba.Length == 0)
      throw new ArgumentException("The data length must be greater than 0.");
    if (dataRgba.Length % 4 != 0)
      throw new ArgumentException("The data must be evenly divisible by 4.");
    if (dataRgba.Length % stride != 0)
      throw new ArgumentException("The data must be evenly divisible by the stride.");
    int num1 = stride / 4;
    int num2 = dataRgba.Length / stride;
    float num3 = Math.Min((float) maximumWidth / (float) num1, (float) maximumHeight / (float) num2);
    if ((double) num3 < 1.0)
    {
      int num4 = (int) Math.Round((double) num1 * (double) num3);
      int num5 = (int) Math.Round((double) num2 * (double) num3);
      float[] numArray1 = new float[num4 * num5 * 4];
      float d1 = (float) num1 / (float) num4;
      float d2 = (float) num2 / (float) num5;
      int num6 = (int) Math.Floor((double) d1);
      int num7 = (int) Math.Floor((double) d2);
      int num8 = num6 * num7;
      for (int index1 = 0; index1 < num5; ++index1)
      {
        for (int index2 = 0; index2 < num4; ++index2)
        {
          int index3 = index1 * num4 * 4 + index2 * 4;
          int num9 = (int) Math.Floor((double) index2 * (double) d1);
          int num10 = (int) Math.Floor((double) index1 * (double) d2);
          int num11 = num9 + num6;
          int num12 = num10 + num7;
          for (int index4 = num10; index4 < num12; ++index4)
          {
            if (index4 < num2)
            {
              for (int index5 = num9; index5 < num11; ++index5)
              {
                if (index5 < num1)
                {
                  int index6 = index4 * num1 * 4 + index5 * 4;
                  numArray1[index3] += (float) dataRgba[index6];
                  numArray1[index3 + 1] += (float) dataRgba[index6 + 1];
                  numArray1[index3 + 2] += (float) dataRgba[index6 + 2];
                  numArray1[index3 + 3] += (float) dataRgba[index6 + 3];
                }
              }
            }
          }
          numArray1[index3] /= (float) num8;
          numArray1[index3 + 1] /= (float) num8;
          numArray1[index3 + 2] /= (float) num8;
          numArray1[index3 + 3] /= (float) num8;
        }
      }
      byte[] numArray2 = new byte[num4 * num5 * 4];
      for (int index7 = 0; index7 < num5; ++index7)
      {
        for (int index8 = 0; index8 < num4; ++index8)
        {
          int index9 = (num5 - 1 - index7) * num4 * 4 + index8 * 4;
          int index10 = index7 * num4 * 4 + index8 * 4;
          numArray2[index10] += (byte) numArray1[index9];
          numArray2[index10 + 1] += (byte) numArray1[index9 + 1];
          numArray2[index10 + 2] += (byte) numArray1[index9 + 2];
          numArray2[index10 + 3] += (byte) numArray1[index9 + 3];
        }
      }
      downsampledStride = num4 * 4;
      return numArray2;
    }
    byte[] numArray = new byte[dataRgba.Length];
    for (int index11 = 0; index11 < num2; ++index11)
    {
      for (int index12 = 0; index12 < num1; ++index12)
      {
        int index13 = (num2 - 1 - index11) * num1 * 4 + index12 * 4;
        int index14 = index11 * num1 * 4 + index12 * 4;
        numArray[index14] += dataRgba[index13];
        numArray[index14 + 1] += dataRgba[index13 + 1];
        numArray[index14 + 2] += dataRgba[index13 + 2];
        numArray[index14 + 3] += dataRgba[index13 + 3];
      }
    }
    downsampledStride = num1 * 4;
    return numArray;
  }
}
