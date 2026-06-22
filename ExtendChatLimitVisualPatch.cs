// Decompiled with JetBrains decompiler
// Type: X.ExtendChatLimitVisualPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace X;

[HarmonyPatch(typeof (FreeChatInputField), "UpdateCharCount")]
public static class ExtendChatLimitVisualPatch
{
  public static void Postfix(FreeChatInputField __instance)
  {
    if (!ChocooPlugin.ExtendChatLimitEnabled)
      return;
    int length = __instance.textArea.text.Length;
    ((TMP_Text) __instance.charCountText).SetText($"{length}/120", true);
    if (length < 90)
      ((Graphic) __instance.charCountText).color = Color.black;
    else if (length < 120)
      ((Graphic) __instance.charCountText).color = new Color(1f, 1f, 0.0f, 1f);
    else
      ((Graphic) __instance.charCountText).color = Color.red;
  }
}
