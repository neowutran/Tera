using System;
using System.Collections.Generic;
using System.Reflection;
using Tera.Game.Messages;

namespace Tera.Game
{
    // Creates a ParsedMessage from a Message
    // Contains a mapping from OpCodeNames to message types and knows how to instantiate those
    // Since it works with OpCodeNames not numeric OpCodes, it needs an OpCodeNamer
    public class MessageFactory
    {
        private static readonly Dictionary<string, Type> OpcodeNameToType = new Dictionary<string, Type>
        {
            {"C_CHECK_VERSION", typeof(C_CHECK_VERSION)},
            {"S_EACH_SKILL_RESULT", typeof(EachSkillResultServerMessage)},
            {"S_SPAWN_USER", typeof(SpawnUserServerMessage)},
            {"S_SPAWN_ME", typeof(SpawnMeServerMessage)},
            {"S_SPAWN_NPC", typeof(SpawnNpcServerMessage)},
            {"S_SPAWN_PROJECTILE", typeof(SpawnProjectileServerMessage)},
            {"S_LOGIN", typeof(LoginServerMessage)},
            {"S_TARGET_INFO", typeof(STargetInfo)},
            {"S_START_USER_PROJECTILE", typeof(StartUserProjectileServerMessage)},
            {"S_CREATURE_CHANGE_HP", typeof(SCreatureChangeHp)},
            {"S_BOSS_GAGE_INFO", typeof(S_BOSS_GAGE_INFO)},
            {"S_NPC_TARGET_USER", typeof(SNpcTargetUser)},
            {"S_NPC_OCCUPIER_INFO", typeof(SNpcOccupierInfo)},
            {"S_CHAT", typeof(S_CHAT)},
            {"S_ABNORMALITY_BEGIN", typeof(SAbnormalityBegin)},
            {"S_ABNORMALITY_END", typeof(SAbnormalityEnd)},
            {"S_ABNORMALITY_REFRESH", typeof(SAbnormalityRefresh)},
            {"S_DESPAWN_NPC", typeof(SDespawnNpc)},
            {"S_PLAYER_CHANGE_MP", typeof(SPlayerChangeMp)},
            {"S_PARTY_MEMBER_ABNORMAL_ADD", typeof(SPartyMemberAbnormalAdd)},
            {"S_PARTY_MEMBER_CHANGE_MP", typeof(SPartyMemberChangeMp)},
            {"S_PARTY_MEMBER_CHANGE_HP", typeof(SPartyMemberChangeHp)},
            {"S_PARTY_MEMBER_ABNORMAL_CLEAR", typeof(SPartyMemberAbnormalClear)},
            {"S_PARTY_MEMBER_ABNORMAL_DEL", typeof(SPartyMemberAbnormalDel)},
            {"S_PARTY_MEMBER_ABNORMAL_REFRESH", typeof(SPartyMemberAbnormalRefresh)},
            {"S_DESPAWN_USER", typeof(SDespawnUser)},
            {"S_USER_STATUS", typeof(SUserStatus)},
            {"S_CREATURE_LIFE", typeof(SCreatureLife)},
            {"S_CREATURE_ROTATE", typeof(S_CREATURE_ROTATE)},
            {"S_NPC_STATUS", typeof(SNpcStatus)},
            {"S_NPC_LOCATION", typeof(SNpcLocation)},
            {"S_USER_LOCATION", typeof(S_USER_LOCATION)},
            {"C_PLAYER_LOCATION", typeof(C_PLAYER_LOCATION)},
            {"S_INSTANT_MOVE", typeof(S_INSTANT_MOVE)},
            {"S_ACTION_STAGE", typeof(S_ACTION_STAGE)},
            {"S_ACTION_END", typeof(S_ACTION_END)},
            {"S_CHANGE_DESTPOS_PROJECTILE", typeof(S_CHANGE_DESTPOS_PROJECTILE)},
            {"S_ADD_CHARM_STATUS", typeof(SAddCharmStatus)},
            {"S_ENABLE_CHARM_STATUS", typeof(SEnableCharmStatus)},
            {"S_REMOVE_CHARM_STATUS", typeof(SRemoveCharmStatus)},
            {"S_RESET_CHARM_STATUS", typeof(SResetCharmStatus)},
            {"S_PARTY_MEMBER_CHARM_ADD", typeof(SPartyMemberCharmAdd)},
            {"S_PARTY_MEMBER_CHARM_DEL", typeof(SPartyMemberCharmDel)},
            {"S_PARTY_MEMBER_CHARM_ENABLE", typeof(SPartyMemberCharmEnable)},
            {"S_PARTY_MEMBER_CHARM_RESET", typeof(SPartyMemberCharmReset)},
            {"S_PARTY_MEMBER_STAT_UPDATE", typeof(S_PARTY_MEMBER_STAT_UPDATE)},
            {"S_PLAYER_STAT_UPDATE", typeof(S_PLAYER_STAT_UPDATE)},
            {"S_PARTY_MEMBER_LIST", typeof(S_PARTY_MEMBER_LIST)},
            {"S_LEAVE_PARTY_MEMBER", typeof(S_LEAVE_PARTY_MEMBER)},
            {"S_BAN_PARTY_MEMBER", typeof(S_BAN_PARTY_MEMBER)},
            {"S_LEAVE_PARTY", typeof(S_LEAVE_PARTY)},
            {"S_BAN_PARTY", typeof(S_BAN_PARTY)},
            {"S_GET_USER_LIST", typeof(S_GET_USER_LIST)},
            {"S_GET_USER_GUILD_LOGO", typeof(S_GET_USER_GUILD_LOGO)},
            {"S_WHISPER", typeof(S_WHISPER)}
        };

        private readonly OpCodeNamer _opCodeNamer;
        public string Version;

        public MessageFactory(OpCodeNamer opCodeNamer, string version)
        {
            _opCodeNamer = opCodeNamer;
            Version = version;
            foreach (var name in OpcodeNameToType.Keys)
            {
                opCodeNamer.GetCode(name);
            }
        }

        public MessageFactory()
        {
            _opCodeNamer = new OpCodeNamer(new Dictionary<ushort,string>{{19900 , "C_CHECK_VERSION" }} );
            Version = "Unknown";
        }

        private ParsedMessage Instantiate(string opCodeName, TeraMessageReader reader)
        {
            Type type;
            if (!OpcodeNameToType.TryGetValue(opCodeName, out type))
                type = typeof(UnknownMessage);

            var constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,
                CallingConventions.Any, new[] {typeof(TeraMessageReader)}, null);
            if (constructor == null)
                throw new Exception("Constructor not found");
            return (ParsedMessage) constructor.Invoke(new object[] {reader});
        }

        public ParsedMessage Create(Message message)
        {
            var reader = new TeraMessageReader(message, _opCodeNamer, Version);
            var opCodeName = _opCodeNamer.GetName(message.OpCode);
            return Instantiate(opCodeName, reader);
        }
    }
}