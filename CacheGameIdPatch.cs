// Decompiled with JetBrains decompiler
// Type: X.CacheGameIdPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using InnerNet;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (AmongUsClient), "OnGameEnd")]
public static class CacheGameIdPatch
{
  public static void Prefix()
  {
    if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ChocooPlugin.AutoRejoinEnabled)
      ChocooPlugin.CachedGameId = ((InnerNetClient) AmongUsClient.Instance).GameId;
    ChocooPlugin.SpeedrunTimer = 0.0f;
  }
}
