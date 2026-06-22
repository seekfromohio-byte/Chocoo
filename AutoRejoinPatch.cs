// Decompiled with JetBrains decompiler
// Type: X.AutoRejoinPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using Il2CppSystem;
using InnerNet;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace X;

[HarmonyPatch(typeof (EndGameManager), "SetEverythingUp")]
public static class AutoRejoinPatch
{
  public static bool Cancelled;

  public static void Postfix(EndGameManager __instance)
  {
    if (!ChocooPlugin.AutoRejoinEnabled || ChocooPlugin.CachedGameId == 0 || ((InnerNetClient) AmongUsClient.Instance).AmHost)
      return;
    AutoRejoinPatch.Cancelled = false;
    GameObject textObj = Object.Instantiate<GameObject>(((Component) __instance.WinText).gameObject);
    textObj.transform.position = new Vector3(0.0f, -2.5f, -15f);
    textObj.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    TextMeshPro tmp = textObj.GetComponent<TextMeshPro>();
    ((TMP_Text) tmp).alignment = (TextAlignmentOptions) 514;
    ((Graphic) tmp).color = Color.white;
    int cachedId = ChocooPlugin.CachedGameId;
    ((TMP_Text) tmp).text = "Rejoining lobby in 3s";
    ((MonoBehaviour) AmongUsClient.Instance).StartCoroutine(Effects.Lerp(3f, Action<float>.op_Implicit((Action<float>) (t =>
    {
      if (AutoRejoinPatch.Cancelled)
      {
        if (!Object.op_Inequality((Object) tmp, (Object) null))
          return;
        Object.Destroy((Object) textObj);
      }
      else
      {
        if (Object.op_Inequality((Object) tmp, (Object) null))
        {
          int num = Mathf.CeilToInt((float) (3.0 - (double) t * 3.0));
          ((TMP_Text) tmp).text = (double) t < 1.0 ? $"Rejoining lobby in {num}s" : "Rejoining...";
        }
        if ((double) t < 1.0 || AutoRejoinPatch.Cancelled)
          return;
        ((MonoBehaviour) AmongUsClient.Instance).StartCoroutine(AmongUsClient.Instance.CoJoinOnlineGameFromCode(cachedId, false));
      }
    }))));
  }
}
