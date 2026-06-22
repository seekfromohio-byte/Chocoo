// Decompiled with JetBrains decompiler
// Type: X.AccurateDisconnectReasonsPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;

#nullable disable
namespace X;

[HarmonyPatch(typeof (GameData), "ShowNotification")]
public static class AccurateDisconnectReasonsPatch
{
  public static bool Prefix(string playerName, DisconnectReasons reason)
  {
    if (!ChocooPlugin.AccurateDisconnectReasonsEnabled)
      return true;
    DisconnectReasons disconnectReasons = reason;
    if (disconnectReasons <= 103)
    {
      if (disconnectReasons <= 17)
      {
        switch ((int) disconnectReasons)
        {
          case 0:
          case 6:
          case 7:
            return true;
          case 1:
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " could not join. Lobby is full.");
            return false;
          case 2:
          case 3:
          case 4:
          case 8:
          case 9:
            goto label_26;
          case 5:
            break;
          case 10:
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was detected and removed for cheating.");
            return false;
          case 11:
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was disconnected. Not authorized.");
            return false;
          default:
            if (disconnectReasons == 17)
            {
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was disconnected due to an error.");
              return false;
            }
            goto label_26;
        }
      }
      else if (disconnectReasons != 21)
      {
        if (disconnectReasons == 103)
          goto label_24;
        goto label_26;
      }
      DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was disconnected due to a version mismatch.");
      return false;
    }
    if (disconnectReasons <= 115)
    {
      if (disconnectReasons != 104)
      {
        switch (disconnectReasons - 110)
        {
          case 0:
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was disconnected due to chat mode mismatch.");
            return false;
          case 2:
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was removed due to account sanctions.");
            return false;
          case 4:
            break;
          case 5:
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was removed for joining from multiple devices.");
            return false;
          default:
            goto label_26;
        }
      }
      else
      {
        DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was removed due to lobby inactivity.");
        return false;
      }
    }
    else
    {
      if (disconnectReasons != 208 /*0xD0*/)
      {
        if (disconnectReasons == 215)
        {
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " lost connection to the server.");
          return false;
        }
        goto label_26;
      }
      DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " left intentionally and received a penalty.");
      return false;
    }
label_24:
    DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " was disconnected due to platform restrictions.");
    return false;
label_26:
    DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"{playerName} was disconnected. Reason: {reason}");
    return false;
  }
}
