#nullable disable
using System;
using System.Globalization;

namespace Styx.WoWInternals
{
	public static class WoWChat
	{
		static WoWChat()
		{
			Lua.Events.AttachEvent("CHAT_MSG_ADDON", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_AFK", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_BATTLEGROUND", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_BATTLEGROUND_LEADER", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_BG_SYSTEM_ALLIANCE", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_BG_SYSTEM_HORDE", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_BG_SYSTEM_NEUTRAL", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_CHANNEL", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_DND", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_EMOTE", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_GUILD", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_MONSTER_EMOTE", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_MONSTER_PARTY", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_MONSTER_SAY", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_MONSTER_WHISPER", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_MONSTER_YELL", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_OFFICER", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_PARTY", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_PARTY_LEADER", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_RAID", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_RAID_BOSS_EMOTE", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_RAID_BOSS_WHISPER", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_RAID_LEADER", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_RAID_WARNING", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_SAY", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_SYSTEM", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_TEXT_EMOTE", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_WHISPER", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_WHISPER_INFORM", OnLuaChatEvent);
			Lua.Events.AttachEvent("CHAT_MSG_YELL", OnLuaChatEvent);
		}

		public static event ChatMessageHandlerEx<ChatAddonEventArgs> Addon;
		public static event ChatMessageHandlerEx<ChatAuthoredEventArgs> Afk;
		public static event ChatMessageHandlerEx<ChatAuthoredEventArgs> Dnd;
		public static event ChatMessageHandlerEx<ChatAuthoredEventArgs> TextEmote;
		public static event ChatMessageHandlerEx<ChatAuthoredEventArgs> Emote;
		public static event ChatMessageHandlerEx<ChatWhisperEventArgs> Whisper;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> WhisperTo;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> Battleground;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> Yell;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> BattlegroundLeader;
		public static event ChatMessageHandlerEx<ChatSimpleMessageEventArgs> AllianceBattleground;
		public static event ChatMessageHandlerEx<ChatSimpleMessageEventArgs> HordeBattleground;
		public static event ChatMessageHandlerEx<ChatSimpleMessageEventArgs> NeutralBattleground;
		public static event ChatMessageHandlerEx<ChatSimpleMessageEventArgs> System;
		public static event ChatMessageHandlerEx<ChatChannelSpecificEventArgs> Channel;
		public static event ChatMessageHandlerEx<ChatGuildEventArgs> Guild;
		public static event ChatMessageHandlerEx<ChatMonsterEventArgs> MonsterEmote;
		public static event ChatMessageHandlerEx<ChatMonsterSayEventArgs> MonsterSay;
		public static event ChatMessageHandlerEx<ChatMonsterEventArgs> MonsterYell;
		public static event ChatMessageHandlerEx<ChatMonsterEventArgs> MonsterWhisper;
		public static event ChatMessageHandlerEx<ChatMonsterEventArgs> MonsterParty;
		public static event ChatMessageHandlerEx<ChatMonsterEventArgs> RaidBossEmote;
		public static event ChatMessageHandlerEx<ChatMonsterEventArgs> RaidBossWhisper;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> Officer;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> Party;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> PartyLeader;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> Raid;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> RaidLeader;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> RaidWarning;
		public static event ChatMessageHandlerEx<ChatLanguageSpecificEventArgs> Say;

		public static void SendChatMessage(string content, ChatType chatType, string channel)
		{
			string destinationType;
			switch (chatType)
			{
				case ChatType.Say:
					destinationType = "SAY";
					break;
				case ChatType.Party:
					destinationType = "PARTY";
					break;
				case ChatType.Raid:
					destinationType = "RAID";
					break;
				case ChatType.Guild:
					destinationType = "GUILD";
					break;
				case ChatType.Officer:
					destinationType = "OFFICER";
					break;
				case ChatType.Yell:
					destinationType = "YELL";
					break;
				case ChatType.WhisperTo:
					destinationType = "WHISPER";
					break;
				case ChatType.Emote:
					destinationType = "EMOTE";
					break;
				case ChatType.Channel:
					destinationType = "CHANNEL";
					break;
				case ChatType.RaidWarning:
					destinationType = "RAID_WARNING";
					break;
				case ChatType.Battleground:
					destinationType = "BATTLEGROUND";
					break;
				default:
					return;
			}

			string escapedContent = Lua.Escape(content ?? string.Empty);
			string escapedChannel = Lua.Escape(channel ?? string.Empty);
			if (chatType == ChatType.Channel)
			{
				Lua.DoString("SendChatMessage('{0}', '{1}', GetDefaultLanguage('player'), GetChannelName('{2}'))", escapedContent, destinationType, escapedChannel);
				return;
			}

			if (chatType == ChatType.WhisperTo)
			{
				Lua.DoString("SendChatMessage('{0}', '{1}', GetDefaultLanguage('player'), '{2}')", escapedContent, destinationType, escapedChannel);
				return;
			}

			Lua.DoString("SendChatMessage('{0}', '{1}', GetDefaultLanguage('player'))", escapedContent, destinationType);
		}

		internal static void Update()
		{
			// HB-style chat dispatch comes from Lua.Events via Lua.ProcessEvents().
			// Keep this pulse hook as a no-op so existing pulse code does not need to change.
		}

		private static void OnLuaChatEvent(object sender, LuaEventArgs e)
		{
			switch (e.EventName)
			{
				case "CHAT_MSG_ADDON":
					Addon?.Invoke(new ChatAddonEventArgs(e));
					break;
				case "CHAT_MSG_AFK":
					Afk?.Invoke(new ChatAuthoredEventArgs(e));
					break;
				case "CHAT_MSG_BATTLEGROUND":
					Battleground?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_BATTLEGROUND_LEADER":
					BattlegroundLeader?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_BG_SYSTEM_ALLIANCE":
					AllianceBattleground?.Invoke(new ChatSimpleMessageEventArgs(e));
					break;
				case "CHAT_MSG_BG_SYSTEM_HORDE":
					HordeBattleground?.Invoke(new ChatSimpleMessageEventArgs(e));
					break;
				case "CHAT_MSG_BG_SYSTEM_NEUTRAL":
					NeutralBattleground?.Invoke(new ChatSimpleMessageEventArgs(e));
					break;
				case "CHAT_MSG_CHANNEL":
					Channel?.Invoke(new ChatChannelSpecificEventArgs(e));
					break;
				case "CHAT_MSG_DND":
					Dnd?.Invoke(new ChatAuthoredEventArgs(e));
					break;
				case "CHAT_MSG_EMOTE":
					Emote?.Invoke(new ChatAuthoredEventArgs(e));
					break;
				case "CHAT_MSG_GUILD":
					Guild?.Invoke(new ChatGuildEventArgs(e));
					break;
				case "CHAT_MSG_MONSTER_EMOTE":
					MonsterEmote?.Invoke(new ChatMonsterEventArgs(e));
					break;
				case "CHAT_MSG_MONSTER_PARTY":
					MonsterParty?.Invoke(new ChatMonsterEventArgs(e));
					break;
				case "CHAT_MSG_MONSTER_SAY":
					MonsterSay?.Invoke(new ChatMonsterSayEventArgs(e));
					break;
				case "CHAT_MSG_MONSTER_WHISPER":
					MonsterWhisper?.Invoke(new ChatMonsterEventArgs(e));
					break;
				case "CHAT_MSG_MONSTER_YELL":
					MonsterYell?.Invoke(new ChatMonsterEventArgs(e));
					break;
				case "CHAT_MSG_OFFICER":
					Officer?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_PARTY":
					Party?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_PARTY_LEADER":
					PartyLeader?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_RAID":
					Raid?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_RAID_BOSS_EMOTE":
					RaidBossEmote?.Invoke(new ChatMonsterEventArgs(e));
					break;
				case "CHAT_MSG_RAID_BOSS_WHISPER":
					RaidBossWhisper?.Invoke(new ChatMonsterEventArgs(e));
					break;
				case "CHAT_MSG_RAID_LEADER":
					RaidLeader?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_RAID_WARNING":
					RaidWarning?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_SAY":
					Say?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_SYSTEM":
					System?.Invoke(new ChatSimpleMessageEventArgs(e));
					break;
				case "CHAT_MSG_TEXT_EMOTE":
					TextEmote?.Invoke(new ChatAuthoredEventArgs(e));
					break;
				case "CHAT_MSG_WHISPER":
					Whisper?.Invoke(new ChatWhisperEventArgs(e));
					break;
				case "CHAT_MSG_WHISPER_INFORM":
					WhisperTo?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
				case "CHAT_MSG_YELL":
					Yell?.Invoke(new ChatLanguageSpecificEventArgs(e));
					break;
			}
		}

		private static int ToInt32(object value)
		{
			if (value == null)
				return 0;

			return Convert.ToInt32(value, CultureInfo.InvariantCulture);
		}

		public delegate void ChatMessageHandlerEx<T>(T e) where T : ChatMessageBaseEventArgs;

		public class ChatMessageBaseEventArgs : LuaEventArgs
		{
			internal ChatMessageBaseEventArgs(LuaEventArgs from)
				: base(from.EventName, from.FireTimeStamp, from.Args)
			{
			}
		}

		public class ChatAddonEventArgs : ChatMessageBaseEventArgs
		{
			internal ChatAddonEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string Prefix => Args[0]?.ToString() ?? string.Empty;
			public string Message => Args[1]?.ToString() ?? string.Empty;
			public string Type => Args[2]?.ToString() ?? string.Empty;
			public string Sender => Args[3]?.ToString() ?? string.Empty;
		}

		public class ChatSimpleMessageEventArgs : ChatMessageBaseEventArgs
		{
			internal ChatSimpleMessageEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string Message => Args[0]?.ToString() ?? string.Empty;
		}

		public class ChatAuthoredEventArgs : ChatSimpleMessageEventArgs
		{
			internal ChatAuthoredEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string Author => Args[1]?.ToString() ?? string.Empty;
		}

		public class ChatLanguageSpecificEventArgs : ChatAuthoredEventArgs
		{
			internal ChatLanguageSpecificEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string Language => Args[2]?.ToString() ?? string.Empty;
		}

		public class ChatChannelSpecificEventArgs : ChatLanguageSpecificEventArgs
		{
			internal ChatChannelSpecificEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string ChannelNameWithNumber => Args[3]?.ToString() ?? string.Empty;
			public string Target => Args[4]?.ToString() ?? string.Empty;
			public string ChatFlags => Args[5]?.ToString() ?? string.Empty;
			public int ZoneFlags => ToInt32(Args[6]);
			public int ChannelNumber => ToInt32(Args[7]);
			public string ChannelName => Args[8]?.ToString() ?? string.Empty;
			public int LineId => ToInt32(Args[9]);
		}

		public class ChatGuildEventArgs : ChatMessageBaseEventArgs
		{
			internal ChatGuildEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string Message => Args[0]?.ToString() ?? string.Empty;
			public string Author => Args[1]?.ToString() ?? string.Empty;
			public string Language => Args[2]?.ToString() ?? string.Empty;
		}

		public class ChatMonsterEventArgs : ChatMessageBaseEventArgs
		{
			internal ChatMonsterEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string Message => Args[0]?.ToString() ?? string.Empty;
			public string MonsterName => Args[1]?.ToString() ?? string.Empty;
		}

		public class ChatMonsterSayEventArgs : ChatMonsterEventArgs
		{
			internal ChatMonsterSayEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string Language => Args[2]?.ToString() ?? string.Empty;
			public string Receiver => Args[4]?.ToString() ?? string.Empty;
		}

		public class ChatWhisperEventArgs : ChatLanguageSpecificEventArgs
		{
			internal ChatWhisperEventArgs(LuaEventArgs from)
				: base(from)
			{
			}

			public string Status => Args[5]?.ToString() ?? string.Empty;
			public int MessageId => ToInt32(Args[6]);
		}
	}
}
