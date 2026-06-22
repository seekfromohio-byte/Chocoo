// Decompiled with JetBrains decompiler
// Type: X.GradientTextHelper
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using System.Text;
using UnityEngine;

#nullable disable
namespace X;

public static class GradientTextHelper
{
  public static string GetGradientText(string text, Color startColor, Color endColor)
  {
    if (string.IsNullOrEmpty(text))
      return text;
    StringBuilder stringBuilder = new StringBuilder();
    int length = text.Length;
    for (int index = 0; index < length; ++index)
    {
      float num = length > 1 ? (float) index / (float) (length - 1) : 0.0f;
      string htmlStringRgb = ColorUtility.ToHtmlStringRGB(Color.Lerp(startColor, endColor, num));
      stringBuilder.Append($"<color=#{htmlStringRgb}>{text[index]}</color>");
    }
    return stringBuilder.ToString();
  }
}
