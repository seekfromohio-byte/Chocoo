// Decompiled with JetBrains decompiler
// Type: X.VersionShowerUpdater
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace X;

public class VersionShowerUpdater : MonoBehaviour
{
  public VersionShower versionShower;
  public string originalText;
  private int _gradientOffset = 0;
  private float _gradientTimer = 0.0f;

  private string ApplyFlowingGradient(string text, int offset)
  {
    if (text.Length <= 1)
      return $"<color=#FFD700>{text}</color>";
    string str = "";
    int length = text.Length;
    for (int index = 0; index < length; ++index)
    {
      int num1 = (index + offset) % (2 * (length - 1));
      float num2 = num1 <= length - 1 ? (float) num1 / (float) (length - 1) : (float) (2 * (length - 1) - num1) / (float) (length - 1);
      byte num3 = (byte) Mathf.Lerp(85f, 85f, num2);
      byte num4 = (byte) Mathf.Lerp(0.0f, 102f, num2);
      byte maxValue = byte.MaxValue;
      str += $"<color=#{num3:X2}{num4:X2}{maxValue:X2}>{text[index]}</color>";
    }
    return str;
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.versionShower, (Object) null) || Object.op_Equality((Object) this.versionShower.text, (Object) null))
    {
      Object.Destroy((Object) this);
    }
    else
    {
      this._gradientTimer += Time.deltaTime;
      if ((double) this._gradientTimer >= 0.10000000149011612)
      {
        ++this._gradientOffset;
        this._gradientTimer = 0.0f;
      }
      ((TMP_Text) this.versionShower.text).text = $"<b>{this.ApplyFlowingGradient("ChocooMenu", this._gradientOffset)} {this.ApplyFlowingGradient("v1.0.8", this._gradientOffset)}</b> - {this.originalText}";
    }
  }
}
