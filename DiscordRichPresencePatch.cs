// Decompiled with JetBrains decompiler
// Type: X.DiscordRichPresencePatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using Discord;
using HarmonyLib;
using System;

#nullable disable
namespace X;

[HarmonyPatch(typeof (ActivityManager), "UpdateActivity")]
public static class DiscordRichPresencePatch
{
  public static ActivityManager cachedManager;

  [HarmonyPrefix]
  [HarmonyPriority(800)]
  public static bool Prefix(ActivityManager __instance, [HarmonyArgument(0)] Activity activity)
  {
    try
    {
      if (DiscordRichPresencePatch.cachedManager == null)
        DiscordRichPresencePatch.cachedManager = __instance;
      if (activity == null)
        return true;
      if (ChocooPlugin.StealthMode)
      {
        activity.Details = "";
        activity.State = "";
        return true;
      }
      int num = !ChocooPlugin.SpoofMenuEnabled || ChocooPlugin.selectedSpoofMenuIndex < 0 ? 0 : (ChocooPlugin.selectedSpoofMenuIndex < ChocooPlugin.spoofMenuDiscordMessages.Length ? 1 : 0);
      activity.Details = num == 0 ? "ChocooMenu v1.0.8" : ChocooPlugin.spoofMenuDiscordMessages[ChocooPlugin.selectedSpoofMenuIndex];
      return true;
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("DiscordRichPresencePatch error: " + ex.Message));
      return true;
    }
  }

  public static void ForceDiscordUpdate()
  {
    try
    {
      if (DiscordRichPresencePatch.cachedManager == null)
        return;
      Activity activity = new Activity()
      {
        Details = !ChocooPlugin.SpoofMenuEnabled || ChocooPlugin.selectedSpoofMenuIndex < 0 || ChocooPlugin.selectedSpoofMenuIndex >= ChocooPlugin.spoofMenuDiscordMessages.Length ? "ChocooMenu v1.0.8" : ChocooPlugin.spoofMenuDiscordMessages[ChocooPlugin.selectedSpoofMenuIndex]
      };
      DiscordRichPresencePatch.cachedManager.UpdateActivity(activity, (ActivityManager.UpdateActivityHandler) null);
    }
    catch
    {
    }
  }
}
