/// This file was generated by C# converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

using System.Linq;
using TypeScript.CSharp;
using Base = Bailey.WAConnection;

namespace Bailey
{
    public class WAConnection : Base
    {
        public WAConnection()
        {
            this.fetchGroupMetadataFromWA = (jid) =>
            {
                var metadata = await this.query(new WAQuery() { { "json", new List<string> { "query", "GroupMetadata", jid } }, { "expect200", true } });
                metadata.participants = metadata.participants.map((p) => ((new Dictionary<string, object>()).Spread(this.contactAddOrGet(p.id), p)));
                return metadata as WAGroupMetadata;
            };
            this.groupMetadataMinimal = (jid) =>
            {
                var query = new List<string>
                {
                    "query",
                    (type: "group", jid: jid, epoch: this.msgCount.toString()),
                    null
                };
                var response = await this.query(new WAQuery() { { "json", query }, { "binaryTags", new List<WAMetric> { WAMetric.group, WAFlag.ignore } }, { "expect200", true } });
                var json = response[2][0];
                var creatorDesc = json[1];
                var participants = json[2] ? json[2].filter((item) => item[0] == "participant") : new List<dynamic>();
                var description = json[2] ? json[2].find((item) => item[0] == "description") : null;
                return (id: jid, owner: creatorDesc.creator, creator: creatorDesc.creator, creation: parseInt(creatorDesc.create), subject: null, desc: description && description[2].toString("utf-8"), participants: participants.map((item) => ((new Dictionary<string, bool>() { { "isAdmin", item[1].type == "admin" } }).Spread(this.contactAddOrGet(item[1].jid))))) as WAGroupMetadata;
            };
            this.groupCreate = (title, participants) =>
            {
                var response = await this.groupQuery("create", null, title, participants) as WAGroupCreateResponse;
                var gid = response.gid;
                WAGroupMetadata metadata;
                try
                {
                    metadata = await this.groupMetadata(gid);
                }
                catch (Exception error)
                {
                    this.logger.warn($"error in group creation: {error}, switching gid & checking");
                    var comps = gid.replace("@g.us", "").split("-");
                    response.gid = $"{comps[0]}-{+comps[1] + 1}@g.us";
                    metadata = await this.groupMetadata(gid);
                    this.logger.warn($"group ID switched from {gid} to {response.gid}");
                }

                await this.chatAdd(response.gid, title, new Dictionary<string, dynamic>() { { "metadata", metadata } });
                return response;
            };
            this.groupLeave = (jid) =>
            {
                var response = await this.groupQuery("leave", jid);
                var chat = this.chats.get(jid);
                if (chat)
                    chat.read_only = "true";
                return response;
            };
            this.groupUpdateSubject = (jid, title) =>
            {
                var chat = this.chats.get(jid);
                if (chat.name == title)
                    throw new BaileysError("redundant change", new Dictionary<string, int>() { { "status", 400 } });
                var response = await this.groupQuery("subject", jid, title);
                if (chat)
                    chat.name = title;
                return response;
            };
            this.groupUpdateDescription = (jid, description) =>
            {
                var metadata = await this.groupMetadata(jid);
                WANode node = new WANode
                {
                    "description",
                    (id: generateMessageID(), prev: metadata.descId),
                    Buffer.from(description, "utf-8")
                };
                var response = await this.groupQuery("description", jid, null, null, new List<WANode> { node });
                return response;
            };
            this.groupAdd = (jid, participants) => this.groupQuery("add", jid, null, participants) as Promise<WAGroupModification>;
            this.groupRemove = (jid, participants) => this.groupQuery("remove", jid, null, participants) as Promise<WAGroupModification>;
            this.groupMakeAdmin = (jid, participants) => this.groupQuery("promote", jid, null, participants) as Promise<WAGroupModification>;
            this.groupDemoteAdmin = (jid, participants) => this.groupQuery("demote", jid, null, participants) as Promise<WAGroupModification>;
            this.groupSettingChange = (jid, setting, onlyAdmins) =>
            {
                WANode node = new WANode
                {
                    setting,
                    new Dictionary<string, string>()
                    {
                        {
                            "value",
                            onlyAdmins ? "true" : "false"
                        }
                    },
                    null
                };
                return this.groupQuery("prop", jid, null, null, new List<WANode> { node }) as Promise<Dictionary<string, double>>;
            };
        }

        /// <summary>
        /// Generic function for group queries
        /// </summary>
        async public void groupQuery(string type, string jid = null, string subject = null, List<string> participants = null, List<WANode> additionalNodes = null)
        {
            var tag = this.generateMessageTag();
            WANode json = new WANode
            {
                "group",
                (author: this.user.jid, id: tag, type: type, jid: jid, subject: subject),
                participants ? participants.map((jid) => new List<string> { "participant", new Dictionary<string, dynamic>() { { "jid", jid } }, null }) : additionalNodes
            };
            var result = await this.setQuery(new List<WANode> { json }, new WATag { WAMetric.group, 136 }, tag);
            return result;
        }

        /// <summary>
        /// Get the metadata of the group
        /// Baileys automatically caches & maintains this state
        /// </summary>
        async public void groupMetadata(string jid)
        {
            var chat = this.chats.get(jid);
            var metadata = chat.metadata;
            if (!metadata)
            {
                if (chat.read_only)
                {
                    metadata = await this.groupMetadataMinimal(jid);
                }
                else
                {
                    metadata = await this.fetchGroupMetadataFromWA(jid);
                }

                if (chat)
                    chat.metadata = metadata;
            }

            return metadata;
        }

        /// <summary>
        /// Get the metadata of the group from WA
        /// </summary>
        public dynamic fetchGroupMetadataFromWA { get; set; }
        /// <summary>
        /// Get the metadata (works after you've left the group also)
        /// </summary>
        public dynamic groupMetadataMinimal { get; set; }
        /// <summary>
        /// Create a group
        /// </summary>
        /// <param name = "title">
        /// like, the title of the group
        /// </param>
        /// <param name = "participants">
        /// people to include in the group
        /// </param>
        public dynamic groupCreate { get; set; }
        /// <summary>
        /// Leave a group
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        public dynamic groupLeave { get; set; }
        /// <summary>
        /// Update the subject of the group
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        /// <param name = "title">
        /// the new title of the group
        /// </param>
        public dynamic groupUpdateSubject { get; set; }
        /// <summary>
        /// Update the group description
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        /// <param name = "title">
        /// the new title of the group
        /// </param>
        public dynamic groupUpdateDescription { get; set; }
        /// <summary>
        /// Add somebody to the group
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        /// <param name = "participants">
        /// the people to add
        /// </param>
        public dynamic groupAdd { get; set; }
        /// <summary>
        /// Remove somebody from the group
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        /// <param name = "participants">
        /// the people to remove
        /// </param>
        public dynamic groupRemove { get; set; }
        /// <summary>
        /// Make someone admin on the group
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        /// <param name = "participants">
        /// the people to make admin
        /// </param>
        public dynamic groupMakeAdmin { get; set; }
        /// <summary>
        /// Make demote an admin on the group
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        /// <param name = "participants">
        /// the people to make admin
        /// </param>
        public dynamic groupDemoteAdmin { get; set; }
        /// <summary>
        /// Make demote an admin on the group
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        /// <param name = "participants">
        /// the people to make admin
        /// </param>
        public dynamic groupSettingChange { get; set; }

        /// <summary>
        /// Get the invite link of the given group
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        /// <returns>
        /// invite code
        /// </returns>
        async public void groupInviteCode(string jid)
        {
            var json = new List<string>
            {
                "query",
                "inviteCode",
                jid
            };
            var response = await this.query(new WAQuery() { { "json", json }, { "expect200", true }, { "requiresPhoneConnection", false } });
            return response.code as string;
        }

        /// <summary>
        /// Join group via invite code
        /// </summary>
        /// <param name = "code">
        /// the invite code
        /// </param>
        /// <returns>
        /// Object containing gid
        /// </returns>
        async public void acceptInvite(string code)
        {
            var json = new List<string>
            {
                "action",
                "invite",
                code
            };
            var response = await this.query(new WAQuery() { { "json", json }, { "expect200", true } });
            return response;
        }

        /// <summary>
        /// Revokes the current invite link for a group chat
        /// </summary>
        /// <param name = "jid">
        /// the ID of the group
        /// </param>
        async public void revokeInvite(string jid)
        {
            var json = new List<string>
            {
                "action",
                "inviteReset",
                jid
            };
            var response = await this.query(new WAQuery() { { "json", json }, { "expect200", true } });
            return response;
        }
    }
}