// Decompiled with JetBrains decompiler
// Type: chocoomenu.HostCommandPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Collections.Generic;
using InnerNet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace chocoomenu;

[HarmonyPatch(typeof (ChatController), "SendChat")]
public static class HostCommandPatch
{
  public static bool Prefix(ChatController __instance)
  {
    if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
      return true;
    string str1 = __instance.freeChatField.textArea.text.Trim();
    if (!str1.StartsWith("s!", StringComparison.OrdinalIgnoreCase))
      return true;
    string[] strArray = str1.Substring(2).Trim().Split(new char[1]
    {
      ' '
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length == 0)
      return true;
    string lowerInvariant = strArray[0].ToLowerInvariant();
    switch (lowerInvariant)
    {
      case "id":
        string str2 = "=== Player IDs ===\n";
        foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
        {
          if (Object.op_Inequality((Object) allPlayerControl, (Object) null) && Object.op_Inequality((Object) allPlayerControl.Data, (Object) null))
          {
            string str3 = allPlayerControl.Data.PlayerName ?? "Unknown";
            str2 = $"{str2}ID {allPlayerControl.PlayerId.ToString()} - {str3}\n";
          }
        }
        DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, str2, true);
        return false;
      case "commands":
        DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "=== Available Commands ===\ns! id - Show all player IDs\ns! kick <id> <reason> - Kick a player\ns! ban <id> <reason> - Ban a player\ns! warn <id> <reason> - Warn a player", true);
        return false;
      case "warn":
        if (strArray.Length < 2)
        {
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "Usage: s! warn <player_id> <reason>", true);
          return false;
        }
        byte result1;
        if (!byte.TryParse(strArray[1], out result1))
        {
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "Invalid ID: " + strArray[1], true);
          return false;
        }
        PlayerControl playerControl1 = (PlayerControl) null;
        foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
        {
          if (Object.op_Inequality((Object) allPlayerControl, (Object) null) && (int) allPlayerControl.PlayerId == (int) result1)
          {
            playerControl1 = allPlayerControl;
            break;
          }
        }
        if (Object.op_Equality((Object) playerControl1, (Object) null))
        {
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"No player with ID {result1}.", true);
          return false;
        }
        string str4 = strArray.Length >= 3 ? string.Join(" ", strArray, 2, strArray.Length - 2) : "No reason provided";
        string str5 = $"{playerControl1.Data?.PlayerName ?? ((Object) playerControl1).name} was warned\nReason: {str4}";
        PlayerControl localPlayer1 = PlayerControl.LocalPlayer;
        string playerName1 = localPlayer1.Data.PlayerName;
        MessageWriter messageWriter1 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer1).NetId, (byte) 6, (SendOption) 1, -1);
        messageWriter1.Write(((InnerNetObject) localPlayer1.Data).NetId);
        messageWriter1.Write("[Host]");
        ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter1);
        MessageWriter messageWriter2 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer1).NetId, (byte) 13, (SendOption) 1, -1);
        messageWriter2.Write(str5);
        ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter2);
        MessageWriter messageWriter3 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer1).NetId, (byte) 6, (SendOption) 1, -1);
        messageWriter3.Write(((InnerNetObject) localPlayer1.Data).NetId);
        messageWriter3.Write(playerName1);
        ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter3);
        DestroyableSingleton<HudManager>.Instance.Chat.AddChat(localPlayer1, str5, true);
        return false;
      default:
        if (lowerInvariant != "ban" && lowerInvariant != "kick")
          return true;
        bool flag = lowerInvariant == "ban";
        if (strArray.Length < 2)
        {
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, flag ? "Usage: s! ban <player_id> <reason>" : "Usage: s! kick <player_id> <reason>", true);
          return false;
        }
        byte result2;
        if (!byte.TryParse(strArray[1], out result2))
        {
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "Invalid ID: " + strArray[1], true);
          return false;
        }
        PlayerControl playerControl2 = (PlayerControl) null;
        foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
        {
          if (Object.op_Inequality((Object) allPlayerControl, (Object) null) && (int) allPlayerControl.PlayerId == (int) result2)
          {
            playerControl2 = allPlayerControl;
            break;
          }
        }
        if (Object.op_Equality((Object) playerControl2, (Object) null))
        {
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"No player with ID {result2}.", true);
          return false;
        }
        if ((int) playerControl2.PlayerId == (int) PlayerControl.LocalPlayer.PlayerId)
        {
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "You can't ban/kick yourself.", true);
          return false;
        }
        string str6 = strArray.Length >= 3 ? string.Join(" ", strArray, 2, strArray.Length - 2) : "No reason provided";
        string str7 = playerControl2.Data?.PlayerName ?? ((Object) playerControl2).name;
        ClientData client = ((InnerNetClient) AmongUsClient.Instance).GetClient(((InnerNetObject) playerControl2).OwnerId);
        if (client == null)
        {
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "Client not found.", true);
          return false;
        }
        string str8 = flag ? "banned" : "kicked";
        string str9 = $"{str7} was {str8}\nReason: {str6}";
        PlayerControl localPlayer2 = PlayerControl.LocalPlayer;
        string playerName2 = localPlayer2.Data.PlayerName;
        MessageWriter messageWriter4 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer2).NetId, (byte) 6, (SendOption) 1, -1);
        messageWriter4.Write(((InnerNetObject) localPlayer2.Data).NetId);
        messageWriter4.Write("[Host]");
        ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter4);
        MessageWriter messageWriter5 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer2).NetId, (byte) 13, (SendOption) 1, -1);
        messageWriter5.Write(str9);
        ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter5);
        MessageWriter messageWriter6 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer2).NetId, (byte) 6, (SendOption) 1, -1);
        messageWriter6.Write(((InnerNetObject) localPlayer2.Data).NetId);
        messageWriter6.Write(playerName2);
        ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter6);
        DestroyableSingleton<HudManager>.Instance.Chat.AddChat(localPlayer2, str9, true);
        ((InnerNetClient) AmongUsClient.Instance).KickPlayer(client.Id, flag);
        return false;
    }
  }

  public class RolesUI(IntPtr ptr) : MonoBehaviour(ptr)
  {
    private Vector2 scrollPosition = Vector2.zero;
    private Rect windowRect = new Rect(600f, 100f, 450f, 500f);
    private int windowId = 667;
    private bool isResizing = false;
    private Vector2 resizeStart;
    private Rect resizeHandleRect;
    private const float MIN_WIDTH = 400f;
    private const float MIN_HEIGHT = 400f;
    private const float MAX_WIDTH = 800f;
    private const float MAX_HEIGHT = 800f;
    private const float RESIZE_HANDLE_SIZE = 15f;
    private Vector2 roleDropdownScrollPosition = Vector2.zero;
    private const float CLOSE_BUTTON_SIZE = 32f;
    private Rect closeButtonRect;
    private static readonly RoleTypes[] availableRoles;
    private static readonly Dictionary<Color, Texture2D> _texCache;

    private void Update()
    {
      if (!Input.GetKeyDown((KeyCode) 27) || !ChocooPlugin.ShowForceRolesMenu)
        return;
      ChocooPlugin.ShowForceRolesMenu = false;
      ChocooPlugin.showRoleDropdown = false;
    }

    private void OnGUI()
    {
      if (!ChocooPlugin.ShowForceRolesMenu)
        return;
      this.HandleResize();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      this.windowRect = GUI.Window(this.windowId, this.windowRect, GUI.WindowFunction.op_Implicit(new Action<int>(this.DrawWindowContents)), "");
      this.DrawResizeHandle();
      this.DrawCloseButton();
    }

    private void DrawWindowContents(int id)
    {
      GUILayout.BeginArea(new Rect(10f, 10f, ((Rect) ref this.windowRect).width - 20f, 30f));
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label("Force Roles Menu", new GUIStyle(GUI.skin.label)
      {
        fontSize = 16 /*0x10*/,
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.EndArea();
      GUILayout.BeginArea(new Rect(10f, 50f, ((Rect) ref this.windowRect).width - 20f, ((Rect) ref this.windowRect).height - 90f));
      if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.yellow;
        GUILayout.Label("⚠️ Must be Host to Force Roles", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 12,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      else
        this.DrawRoleAssignments();
      GUILayout.EndArea();
      GUILayout.BeginArea(new Rect(10f, ((Rect) ref this.windowRect).height - 55f, ((Rect) ref this.windowRect).width - 20f, 50f));
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = Color.cyan;
      GUILayout.Label("\uD83D\uDCA1 Roles will be assigned when the game starts", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : Color.gray;
      GUILayout.Label("Press ESC to close", new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4,
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.EndArea();
      if (this.isResizing || ((Rect) ref this.resizeHandleRect).Contains(Event.current.mousePosition) || ((Rect) ref this.closeButtonRect).Contains(Event.current.mousePosition))
        return;
      Rect rect;
      // ISSUE: explicit constructor call
      ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((double) ((Rect) ref this.windowRect).width - 32.0 - 10.0), ((Rect) ref this.windowRect).height);
      GUI.DragWindow(rect);
    }

    private void DrawCloseButton()
    {
      this.closeButtonRect = new Rect((float) ((double) ((Rect) ref this.windowRect).x + (double) ((Rect) ref this.windowRect).width - 32.0 - 5.0), ((Rect) ref this.windowRect).y + 5f, 32f, 32f);
      Color col = ((Rect) ref this.closeButtonRect).Contains(Event.current.mousePosition) ? new Color(0.8f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.2f, 0.2f, 0.8f);
      GUIStyle guiStyle = new GUIStyle(GUI.skin.box);
      guiStyle.normal.background = this.MakeTex(2, 2, col);
      GUI.backgroundColor = col;
      GUI.Box(this.closeButtonRect, "", guiStyle);
      GUI.Label(this.closeButtonRect, "✕", new GUIStyle(GUI.skin.label)
      {
        fontSize = 16 /*0x10*/,
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 4,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        }
      });
      if (GUI.Button(this.closeButtonRect, "", GUIStyle.none))
      {
        ChocooPlugin.ShowForceRolesMenu = false;
        ChocooPlugin.showRoleDropdown = false;
        ChocooPlugin.dropdownPlayerIndex = -1;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void DrawRoleAssignments()
    {
      this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      List<PlayerControl> list = ((IEnumerable<PlayerControl>) PlayerControl.AllPlayerControls.ToArray()).Where<PlayerControl>((Func<PlayerControl, bool>) (p => Object.op_Inequality((Object) p, (Object) null) && Object.op_Inequality((Object) p.Data, (Object) null) && !p.Data.Disconnected)).ToList<PlayerControl>();
      if (list.Count == 0)
      {
        GUI.contentColor = Color.gray;
        GUILayout.Label("No players in lobby", new GUIStyle(GUI.skin.label)
        {
          fontSize = 11,
          alignment = (TextAnchor) 4,
          fontStyle = (FontStyle) 2
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else
      {
        for (int index = 0; index < list.Count; ++index)
        {
          PlayerControl playerControl = list[index];
          int playerId = (int) playerControl.PlayerId;
          string str1 = playerControl.Data.PlayerName ?? "Unknown";
          Color color = Color32.op_Implicit(((Il2CppArrayBase<Color32>) Palette.PlayerColors)[playerControl.Data.DefaultOutfit.ColorId]);
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
          GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
          GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
          GUI.contentColor = color;
          string str2 = str1;
          GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
          guiStyle.fontSize = 12;
          guiStyle.fontStyle = (FontStyle) 1;
          GUILayoutOption[] guiLayoutOptionArray = new GUILayoutOption[1]
          {
            GUILayout.Width(140f)
          };
          GUILayout.Label(str2, guiStyle, guiLayoutOptionArray);
          GUI.contentColor = ChocooPlugin.GetRGBText();
          GUILayout.FlexibleSpace();
          RoleTypes forcedRole = ChocooPlugin.forcedRoles.ContainsKey(playerId) ? ChocooPlugin.forcedRoles[playerId] : (RoleTypes) (object) 0;
          string roleName1 = this.GetRoleName(forcedRole);
          bool isOpen = ChocooPlugin.showRoleDropdown && ChocooPlugin.dropdownPlayerIndex == index;
          GUI.backgroundColor = isOpen ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.4f, 1f);
          if (GUILayout.Button("▼ " + roleName1, new GUILayoutOption[2]
          {
            GUILayout.Height(25f),
            GUILayout.Width(150f)
          }))
          {
            if (isOpen)
            {
              ChocooPlugin.showRoleDropdown = false;
              ChocooPlugin.dropdownPlayerIndex = -1;
            }
            else
            {
              ChocooPlugin.showRoleDropdown = true;
              ChocooPlugin.dropdownPlayerIndex = index;
              ChocooPlugin.selectedForceRolePlayerId = playerId;
            }
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
          GUILayout.Space(5f);
          GUI.backgroundColor = new Color(0.6f, 0.1f, 0.1f, 1f);
          if (GUILayout.Button("✖", new GUILayoutOption[2]
          {
            GUILayout.Width(30f),
            GUILayout.Height(25f)
          }) && ChocooPlugin.forcedRoles.ContainsKey(playerId))
          {
            ChocooPlugin.forcedRoles.Remove(playerId);
            ChocooPlugin.Logger.LogInfo((object) ("Removed forced role for " + str1));
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
          GUILayout.EndHorizontal();
          float dropdownHeight = ChocooPlugin.GetDropdownHeight("forceRole_" + index.ToString(), isOpen, 150f);
          if ((double) dropdownHeight > 1.0)
          {
            GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
            this.roleDropdownScrollPosition = GUILayout.BeginScrollView(this.roleDropdownScrollPosition, new GUILayoutOption[1]
            {
              GUILayout.Height(dropdownHeight)
            });
            foreach (ushort availableRole in HostCommandPatch.RolesUI.availableRoles)
            {
              RoleTypes role = (RoleTypes) (int) availableRole;
              GUI.backgroundColor = forcedRole == role ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
              string roleName2 = this.GetRoleName(role);
              if (GUILayout.Button(roleName2, new GUILayoutOption[1]
              {
                GUILayout.Height(25f)
              }))
              {
                ChocooPlugin.forcedRoles[playerId] = role;
                ChocooPlugin.showRoleDropdown = false;
                ChocooPlugin.dropdownPlayerIndex = -1;
                ChocooPlugin.Logger.LogInfo((object) $"Set {str1} to {roleName2}");
              }
              GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
          }
          GUILayout.EndVertical();
          GUI.backgroundColor = ChocooPlugin.GetRGBColor();
          GUILayout.Space(3f);
        }
      }
      GUILayout.Space(10f);
      GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
      if (GUILayout.Button("CLEAR ALL ROLES", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.forcedRoles.Clear();
        ChocooPlugin.Logger.LogInfo((object) "Cleared all forced roles");
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndScrollView();
    }

    private string GetRoleName(RoleTypes role)
    {
      switch ((int) role)
      {
        case 0:
          return "Crewmate";
        case 1:
          return "Impostor";
        case 2:
          return "Scientist";
        case 3:
          return "Engineer";
        case 4:
          return "Guardian Angel";
        case 5:
          return "Shapeshifter";
        case 8:
          return "Noisemaker";
        case 9:
          return "Phantom";
        case 10:
          return "Tracker";
        case 12:
          return "Detective";
        case 18:
          return "Viper";
        default:
          return role.ToString();
      }
    }

    private void HandleResize()
    {
      Event current = Event.current;
      this.resizeHandleRect = new Rect((float) ((double) ((Rect) ref this.windowRect).x + (double) ((Rect) ref this.windowRect).width - 15.0), (float) ((double) ((Rect) ref this.windowRect).y + (double) ((Rect) ref this.windowRect).height - 15.0), 15f, 15f);
      if (current.type == null && current.button == 0 && ((Rect) ref this.resizeHandleRect).Contains(current.mousePosition))
      {
        this.isResizing = true;
        this.resizeStart = current.mousePosition;
        current.Use();
      }
      if (!this.isResizing)
        return;
      if (current.type == 3)
      {
        Vector2 vector2 = Vector2.op_Subtraction(current.mousePosition, this.resizeStart);
        float num1 = ((Rect) ref this.windowRect).width + vector2.x;
        float num2 = ((Rect) ref this.windowRect).height + vector2.y;
        ((Rect) ref this.windowRect).width = Mathf.Clamp(num1, 400f, 800f);
        ((Rect) ref this.windowRect).height = Mathf.Clamp(num2, 400f, 800f);
        this.resizeStart = current.mousePosition;
        current.Use();
      }
      if (current.type == 1 && current.button == 0)
      {
        this.isResizing = false;
        current.Use();
      }
    }

    private void DrawResizeHandle()
    {
      GUI.backgroundColor = this.isResizing ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.8f);
      GUI.Box(this.resizeHandleRect, "", new GUIStyle(GUI.skin.box)
      {
        normal = {
          background = this.MakeTex(2, 2, this.isResizing ? new Color(1f, 1f, 0.0f, 0.5f) : new Color(0.3f, 0.3f, 0.3f, 0.5f))
        }
      });
      GUI.contentColor = Color.white;
      GUI.Label(this.resizeHandleRect, "⋰", new GUIStyle(GUI.skin.label)
      {
        fontSize = 12,
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 4
      });
      GUI.contentColor = Color.white;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
      Texture2D texture2D;
      if (!HostCommandPatch.RolesUI._texCache.TryGetValue(col, out texture2D) || Object.op_Equality((Object) texture2D, (Object) null))
      {
        texture2D = new Texture2D(width, height);
        Color[] colorArray = new Color[width * height];
        for (int index = 0; index < colorArray.Length; ++index)
          colorArray[index] = col;
        texture2D.SetPixels(Il2CppStructArray<Color>.op_Implicit(colorArray));
        texture2D.Apply();
        HostCommandPatch.RolesUI._texCache[col] = texture2D;
      }
      return texture2D;
    }

    static RolesUI()
    {
      // ISSUE: unable to decompile the method.
    }
  }

  public class ForceColorUI(IntPtr ptr) : MonoBehaviour(ptr)
  {
    private Vector2 scrollPosition = Vector2.zero;
    private Rect windowRect = new Rect(600f, 100f, 450f, 550f);
    private int windowId = 668;
    private bool isResizing = false;
    private Vector2 resizeStart;
    private Rect resizeHandleRect;
    private const float MIN_WIDTH = 400f;
    private const float MIN_HEIGHT = 400f;
    private const float MAX_WIDTH = 800f;
    private const float MAX_HEIGHT = 800f;
    private const float RESIZE_HANDLE_SIZE = 15f;
    private Vector2 colorDropdownScrollPosition = Vector2.zero;
    private bool showGlobalColorDropdown = false;
    private Dictionary<int, Vector2> playerColorScrollPositions = new Dictionary<int, Vector2>();
    private const float CLOSE_BUTTON_SIZE = 32f;
    private Rect closeButtonRect;
    private static readonly string[] colorNames = new string[18]
    {
      "Red",
      "Blue",
      "Green",
      "Pink",
      "Orange",
      "Yellow",
      "Black",
      "White",
      "Purple",
      "Brown",
      "Cyan",
      "Lime",
      "Maroon",
      "Rose",
      "Banana",
      "Gray",
      "Tan",
      "Coral"
    };
    private GUIStyle _titleStyle;
    private GUIStyle _boldStyle11;
    private GUIStyle _warningStyle;
    private GUIStyle _infoStyle10;
    private GUIStyle _escStyle;
    private GUIStyle _playerNameStyle;
    private GUIStyle _italicStyle11;
    private GUIStyle _closeXStyle;
    private GUIStyle _resizeHandleStyle;
    private GUIStyle[] _colorSwatchStyles;
    private GUIStyle _closeBoxStyleNormal;
    private GUIStyle _closeBoxStyleHover;
    private GUIStyle _resizeHandleBoxIdle;
    private GUIStyle _resizeHandleBoxActive;
    private bool _stylesInitialized = false;
    private List<PlayerControl> _cachedPlayers = new List<PlayerControl>();
    private float _playerCacheTimer = 0.0f;
    private const float PLAYER_CACHE_INTERVAL = 0.5f;
    private static readonly Dictionary<Color, Texture2D> _texCache = new Dictionary<Color, Texture2D>();

    private void InitStyles()
    {
      if (this._stylesInitialized)
        return;
      this._titleStyle = new GUIStyle(GUI.skin.label)
      {
        fontSize = 16 /*0x10*/,
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 4
      };
      this._boldStyle11 = new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 11
      };
      this._warningStyle = new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 12,
        alignment = (TextAnchor) 4
      };
      this._infoStyle10 = new GUIStyle(GUI.skin.label)
      {
        fontSize = 10,
        alignment = (TextAnchor) 4
      };
      this._escStyle = new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4,
        fontSize = 9
      };
      this._playerNameStyle = new GUIStyle(GUI.skin.label)
      {
        fontSize = 12,
        fontStyle = (FontStyle) 1
      };
      this._italicStyle11 = new GUIStyle(GUI.skin.label)
      {
        fontSize = 11,
        alignment = (TextAnchor) 4,
        fontStyle = (FontStyle) 2
      };
      this._closeXStyle = new GUIStyle(GUI.skin.label)
      {
        fontSize = 16 /*0x10*/,
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 4,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        }
      };
      this._resizeHandleStyle = new GUIStyle(GUI.skin.label)
      {
        fontSize = 12,
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 4
      };
      this._colorSwatchStyles = new GUIStyle[18];
      for (int index = 0; index < 18; ++index)
        this._colorSwatchStyles[index] = new GUIStyle(GUI.skin.box)
        {
          normal = {
            background = this.MakeTex(2, 2, Color32.op_Implicit(((Il2CppArrayBase<Color32>) Palette.PlayerColors)[index]))
          }
        };
      this._closeBoxStyleNormal = new GUIStyle(GUI.skin.box);
      this._closeBoxStyleNormal.normal.background = this.MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 0.8f));
      this._closeBoxStyleHover = new GUIStyle(GUI.skin.box);
      this._closeBoxStyleHover.normal.background = this.MakeTex(2, 2, new Color(0.8f, 0.2f, 0.2f, 1f));
      this._resizeHandleBoxIdle = new GUIStyle(GUI.skin.box);
      this._resizeHandleBoxIdle.normal.background = this.MakeTex(2, 2, new Color(0.3f, 0.3f, 0.3f, 0.5f));
      this._resizeHandleBoxActive = new GUIStyle(GUI.skin.box);
      this._resizeHandleBoxActive.normal.background = this.MakeTex(2, 2, new Color(1f, 1f, 0.0f, 0.5f));
      this._stylesInitialized = true;
    }

    private void RefreshPlayerCache()
    {
      this._cachedPlayers.Clear();
      List<PlayerControl> allPlayerControls = PlayerControl.AllPlayerControls;
      if (allPlayerControls == null)
        return;
      foreach (PlayerControl playerControl in allPlayerControls)
      {
        if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl.Data, (Object) null) && !playerControl.Data.Disconnected)
          this._cachedPlayers.Add(playerControl);
      }
    }

    private void Update()
    {
      if (Input.GetKeyDown((KeyCode) 27) && ChocooPlugin.ShowForceColorMenu)
      {
        ChocooPlugin.ShowForceColorMenu = false;
        ChocooPlugin.showColorDropdown = false;
        this.showGlobalColorDropdown = false;
      }
      if (!ChocooPlugin.ShowForceColorMenu)
        return;
      this._playerCacheTimer -= Time.deltaTime;
      if ((double) this._playerCacheTimer <= 0.0)
      {
        this.RefreshPlayerCache();
        this._playerCacheTimer = 0.5f;
      }
    }

    private void OnGUI()
    {
      if (!ChocooPlugin.ShowForceColorMenu)
        return;
      this.HandleResize();
      this.InitStyles();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      this.windowRect = GUI.Window(this.windowId, this.windowRect, GUI.WindowFunction.op_Implicit(new Action<int>(this.DrawWindowContents)), "");
      this.DrawResizeHandle();
      this.DrawCloseButton();
    }

    private void DrawWindowContents(int id)
    {
      GUILayout.BeginArea(new Rect(10f, 10f, ((Rect) ref this.windowRect).width - 20f, 30f));
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label("Force Color Menu", this._titleStyle, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.EndArea();
      GUILayout.BeginArea(new Rect(10f, 50f, ((Rect) ref this.windowRect).width - 20f, ((Rect) ref this.windowRect).height - 90f));
      if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.yellow;
        GUILayout.Label("⚠️ Must be Host to Force Colors", this._warningStyle, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      else
        this.DrawColorControls();
      GUILayout.EndArea();
      GUILayout.BeginArea(new Rect(10f, ((Rect) ref this.windowRect).height - 55f, ((Rect) ref this.windowRect).width - 20f, 50f));
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = Color.cyan;
      GUILayout.Label("\uD83C\uDFA8 Colors will be applied immediately", this._infoStyle10, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : Color.gray;
      GUILayout.Label("Press ESC to close", this._escStyle, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.EndArea();
      if (this.isResizing || ((Rect) ref this.resizeHandleRect).Contains(Event.current.mousePosition) || ((Rect) ref this.closeButtonRect).Contains(Event.current.mousePosition))
        return;
      Rect rect;
      // ISSUE: explicit constructor call
      ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((double) ((Rect) ref this.windowRect).width - 32.0 - 10.0), ((Rect) ref this.windowRect).height);
      GUI.DragWindow(rect);
    }

    private void DrawCloseButton()
    {
      this.closeButtonRect = new Rect((float) ((double) ((Rect) ref this.windowRect).x + (double) ((Rect) ref this.windowRect).width - 32.0 - 5.0), ((Rect) ref this.windowRect).y + 5f, 32f, 32f);
      bool flag = ((Rect) ref this.closeButtonRect).Contains(Event.current.mousePosition);
      Color color = flag ? new Color(0.8f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.2f, 0.2f, 0.8f);
      GUIStyle guiStyle = flag ? this._closeBoxStyleHover : this._closeBoxStyleNormal;
      GUI.backgroundColor = color;
      GUI.Box(this.closeButtonRect, "", guiStyle);
      GUI.Label(this.closeButtonRect, "✕", this._closeXStyle);
      if (GUI.Button(this.closeButtonRect, "", GUIStyle.none))
      {
        ChocooPlugin.ShowForceColorMenu = false;
        ChocooPlugin.showColorDropdown = false;
        this.showGlobalColorDropdown = false;
        ChocooPlugin.dropdownPlayerIndexColor = -1;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void DrawColorControls()
    {
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.2f, 0.7f, 0.9f);
      GUILayout.Label("Select Global Color:", this._boldStyle11, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      Color32.op_Implicit(((Il2CppArrayBase<Color32>) Palette.PlayerColors)[(int) ChocooPlugin.selectedGlobalColor]);
      string colorName = HostCommandPatch.ForceColorUI.colorNames[(int) ChocooPlugin.selectedGlobalColor];
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Box("", this._colorSwatchStyles[(int) ChocooPlugin.selectedGlobalColor], new GUILayoutOption[2]
      {
        GUILayout.Width(30f),
        GUILayout.Height(30f)
      });
      GUILayout.Space(5f);
      if (GUILayout.Button("▼ " + colorName, new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.showGlobalColorDropdown = !this.showGlobalColorDropdown;
      GUILayout.EndHorizontal();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      float dropdownHeight = ChocooPlugin.GetDropdownHeight("globalColor", this.showGlobalColorDropdown, 150f);
      if ((double) dropdownHeight > 1.0)
      {
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        this.colorDropdownScrollPosition = GUILayout.BeginScrollView(this.colorDropdownScrollPosition, new GUILayoutOption[1]
        {
          GUILayout.Height(dropdownHeight)
        });
        for (byte index = 0; index < (byte) 18; ++index)
        {
          GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
          bool flag = (int) ChocooPlugin.selectedGlobalColor == (int) index;
          GUILayout.Box("", this._colorSwatchStyles[(int) index], new GUILayoutOption[2]
          {
            GUILayout.Width(25f),
            GUILayout.Height(25f)
          });
          GUILayout.Space(5f);
          GUI.backgroundColor = flag ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
          if (GUILayout.Button(HostCommandPatch.ForceColorUI.colorNames[(int) index], new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
          {
            ChocooPlugin.selectedGlobalColor = index;
            this.showGlobalColorDropdown = false;
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
          GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
      }
      GUILayout.Space(10f);
      GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
      if (GUILayout.Button("\uD83C\uDFA8 FORCE COLOR TO ALL", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        this.ForceColorToAll(ChocooPlugin.selectedGlobalColor);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      this.DrawPlayerColorAssignments();
    }

    private void DrawPlayerColorAssignments()
    {
      this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      List<PlayerControl> cachedPlayers = this._cachedPlayers;
      if (cachedPlayers.Count == 0)
      {
        GUI.contentColor = Color.gray;
        GUILayout.Label("No players in lobby", this._italicStyle11, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else
      {
        for (int index1 = 0; index1 < cachedPlayers.Count; ++index1)
        {
          PlayerControl playerControl = cachedPlayers[index1];
          if (!Object.op_Equality((Object) playerControl, (Object) null) && !Object.op_Equality((Object) playerControl.Data, (Object) null))
          {
            int playerId = (int) playerControl.PlayerId;
            string str = playerControl.Data.PlayerName ?? "Unknown";
            Color color = Color32.op_Implicit(((Il2CppArrayBase<Color32>) Palette.PlayerColors)[playerControl.Data.DefaultOutfit.ColorId]);
            GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
            GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
            GUI.contentColor = color;
            GUILayout.Label(str, this._playerNameStyle, new GUILayoutOption[1]
            {
              GUILayout.Width(140f)
            });
            GUI.contentColor = ChocooPlugin.GetRGBText();
            GUILayout.FlexibleSpace();
            byte index2 = ChocooPlugin.forcedColors.ContainsKey(playerId) ? ChocooPlugin.forcedColors[playerId] : (byte) playerControl.Data.DefaultOutfit.ColorId;
            string colorName = HostCommandPatch.ForceColorUI.colorNames[(int) index2];
            bool isOpen = ChocooPlugin.showColorDropdown && ChocooPlugin.dropdownPlayerIndexColor == index1;
            GUI.backgroundColor = isOpen ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.4f, 1f);
            GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
            GUILayout.Box("", this._colorSwatchStyles[(int) index2], new GUILayoutOption[2]
            {
              GUILayout.Width(25f),
              GUILayout.Height(25f)
            });
            GUILayout.Space(3f);
            if (GUILayout.Button("▼ " + colorName, new GUILayoutOption[2]
            {
              GUILayout.Height(25f),
              GUILayout.Width(120f)
            }))
            {
              if (isOpen)
              {
                ChocooPlugin.showColorDropdown = false;
                ChocooPlugin.dropdownPlayerIndexColor = -1;
              }
              else
              {
                ChocooPlugin.showColorDropdown = true;
                ChocooPlugin.dropdownPlayerIndexColor = index1;
                ChocooPlugin.selectedForceColorId = playerId;
              }
            }
            GUILayout.EndHorizontal();
            GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
            GUILayout.Space(5f);
            GUI.backgroundColor = new Color(0.6f, 0.1f, 0.1f, 1f);
            if (GUILayout.Button("✖", new GUILayoutOption[2]
            {
              GUILayout.Width(30f),
              GUILayout.Height(25f)
            }) && ChocooPlugin.forcedColors.ContainsKey(playerId))
            {
              ChocooPlugin.forcedColors.Remove(playerId);
              ChocooPlugin.Logger.LogInfo((object) ("Removed forced color for " + str));
            }
            GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
            GUILayout.EndHorizontal();
            float dropdownHeight = ChocooPlugin.GetDropdownHeight("forceColor_" + index1.ToString(), isOpen, 150f);
            if ((double) dropdownHeight > 1.0)
            {
              GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
              if (!this.playerColorScrollPositions.ContainsKey(index1))
                this.playerColorScrollPositions[index1] = Vector2.zero;
              this.playerColorScrollPositions[index1] = GUILayout.BeginScrollView(this.playerColorScrollPositions[index1], new GUILayoutOption[1]
              {
                GUILayout.Height(dropdownHeight)
              });
              for (byte index3 = 0; index3 < (byte) 18; ++index3)
              {
                GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
                bool flag = (int) index2 == (int) index3;
                GUILayout.Box("", this._colorSwatchStyles[(int) index3], new GUILayoutOption[2]
                {
                  GUILayout.Width(25f),
                  GUILayout.Height(25f)
                });
                GUILayout.Space(5f);
                GUI.backgroundColor = flag ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
                if (GUILayout.Button(HostCommandPatch.ForceColorUI.colorNames[(int) index3], new GUILayoutOption[1]
                {
                  GUILayout.Height(25f)
                }))
                {
                  ChocooPlugin.forcedColors[playerId] = index3;
                  playerControl.RpcSetColor(index3);
                  ChocooPlugin.showColorDropdown = false;
                  ChocooPlugin.dropdownPlayerIndexColor = -1;
                  ChocooPlugin.Logger.LogInfo((object) $"Set {str} to {HostCommandPatch.ForceColorUI.colorNames[(int) index3]}");
                }
                GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
                GUILayout.EndHorizontal();
              }
              GUILayout.EndScrollView();
              GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            GUI.backgroundColor = ChocooPlugin.GetRGBColor();
            GUILayout.Space(3f);
          }
        }
      }
      GUILayout.EndScrollView();
    }

    private void ForceColorToAll(byte colorId)
    {
      try
      {
        int num = 0;
        foreach (PlayerControl cachedPlayer in this._cachedPlayers)
        {
          if (!Object.op_Equality((Object) cachedPlayer, (Object) null) && !Object.op_Equality((Object) cachedPlayer.Data, (Object) null) && !cachedPlayer.Data.Disconnected)
          {
            cachedPlayer.RpcSetColor(colorId);
            ChocooPlugin.forcedColors[(int) cachedPlayer.PlayerId] = colorId;
            ++num;
          }
        }
        ChocooPlugin.Logger.LogInfo((object) $"Forced {HostCommandPatch.ForceColorUI.colorNames[(int) colorId]} to {num.ToString()} players");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForceColorToAll error: " + ex.Message));
      }
    }

    private void HandleResize()
    {
      Event current = Event.current;
      this.resizeHandleRect = new Rect((float) ((double) ((Rect) ref this.windowRect).x + (double) ((Rect) ref this.windowRect).width - 15.0), (float) ((double) ((Rect) ref this.windowRect).y + (double) ((Rect) ref this.windowRect).height - 15.0), 15f, 15f);
      if (current.type == null && current.button == 0 && ((Rect) ref this.resizeHandleRect).Contains(current.mousePosition))
      {
        this.isResizing = true;
        this.resizeStart = current.mousePosition;
        current.Use();
      }
      if (!this.isResizing)
        return;
      if (current.type == 3)
      {
        Vector2 vector2 = Vector2.op_Subtraction(current.mousePosition, this.resizeStart);
        float num1 = ((Rect) ref this.windowRect).width + vector2.x;
        float num2 = ((Rect) ref this.windowRect).height + vector2.y;
        ((Rect) ref this.windowRect).width = Mathf.Clamp(num1, 400f, 800f);
        ((Rect) ref this.windowRect).height = Mathf.Clamp(num2, 400f, 800f);
        this.resizeStart = current.mousePosition;
        current.Use();
      }
      if (current.type == 1 && current.button == 0)
      {
        this.isResizing = false;
        current.Use();
      }
    }

    private void DrawResizeHandle()
    {
      GUI.backgroundColor = this.isResizing ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.8f);
      GUI.Box(this.resizeHandleRect, "", this.isResizing ? this._resizeHandleBoxActive : this._resizeHandleBoxIdle);
      GUI.contentColor = Color.white;
      GUI.Label(this.resizeHandleRect, "⋰", this._resizeHandleStyle);
      GUI.contentColor = Color.white;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
      Texture2D texture2D;
      if (!HostCommandPatch.ForceColorUI._texCache.TryGetValue(col, out texture2D) || Object.op_Equality((Object) texture2D, (Object) null))
      {
        texture2D = new Texture2D(width, height);
        Color[] colorArray = new Color[width * height];
        for (int index = 0; index < colorArray.Length; ++index)
          colorArray[index] = col;
        texture2D.SetPixels(Il2CppStructArray<Color>.op_Implicit(colorArray));
        texture2D.Apply();
        HostCommandPatch.ForceColorUI._texCache[col] = texture2D;
      }
      return texture2D;
    }
  }
}
