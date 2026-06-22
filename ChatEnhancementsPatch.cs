// Decompiled with JetBrains decompiler
// Type: X.ChatEnhancementsPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;

#nullable disable
namespace X;

[HarmonyPatch(typeof (ChatController), "Update")]
public static class ChatEnhancementsPatch
{
  public static void Postfix(ChatController __instance)
  {
    if (ChocooPlugin.ReduceChatCooldownEnabled && (double) __instance.timeSinceLastMessage < 0.89999997615814209)
      __instance.timeSinceLastMessage = 0.9f;
    __instance.freeChatField.textArea.characterLimit = !ChocooPlugin.ExtendChatLimitEnabled ? 100 : 120;
    if (ChocooPlugin.AllowAllCharactersEnabled)
    {
      __instance.freeChatField.textArea.AllowSymbols = true;
      __instance.freeChatField.textArea.AllowEmail = true;
      __instance.freeChatField.textArea.allowAllCharacters = true;
    }
    else
    {
      __instance.freeChatField.textArea.AllowSymbols = false;
      __instance.freeChatField.textArea.AllowEmail = false;
      __instance.freeChatField.textArea.allowAllCharacters = false;
    }
  }
}
