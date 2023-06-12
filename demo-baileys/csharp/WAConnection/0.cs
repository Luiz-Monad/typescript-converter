/// This file was generated by C# converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

using System.Linq;
using TypeScript.CSharp;
using Utils = Bailey;

namespace Bailey
{
    public class WAConnection : EventEmitter
    {
        public WAConnection()
        {
            this.version = new Array<double>{2, 2142, 12};
            this.browserDescription = Utils.Browsers.baileys("Chrome");
            this.pendingRequestTimeoutMs = null;
            this.state = "close";
            this.connectOptions = new WAConnectOptions()
            {{"maxIdleTimeMs", 60000}, {"maxRetries", 10}, {"connectCooldownMs", 4000}, {"phoneResponseTime", 15000}, {"maxQueryResponseTime", 10000}, {"alwaysUseTakeover", true}, {"queryChatsTillReceived", true}, {"logQR", true}};
            this.autoReconnect = ReconnectMode.onConnectionLost;
            this.phoneConnected = false;
            this.chatOrderingKey = Utils.waChatKey(false);
            this.logger = logger.child(new Hashtable<String, String>()
            {{"class", "Baileys"}});
            this.shouldLogMessages = false;
            this.messageLog = new Array<(String tag, String json, bool fromMe, Array<dynamic> binaryTags)>();
            this.maxCachedMessages = 50;
            this.chats = new KeyedDB(Utils.waChatKey(false), (value) => value.jid);
            this.contacts = new Hashtable<String, WAContact>();
            this.blocklist = new Array<String>();
        }

        /// <summary>
        /// The version of WhatsApp Web we're telling the servers we are
        /// </summary>
        public Array<double> version
        {
            get;
            set;
        }

        /// <summary>
        /// The Browser we're telling the WhatsApp Web servers we are
        /// </summary>
        public Array<String> browserDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Metadata like WhatsApp id, name set on WhatsApp etc.
        /// </summary>
        public WAUser user
        {
            get;
            set;
        }

        /// <summary>
        /// Should requests be queued when the connection breaks in between; if 0, then an error will be thrown
        /// </summary>
        public double pendingRequestTimeoutMs
        {
            get;
            set;
        }

        /// <summary>
        /// The connection state
        /// </summary>
        public WAConnectionState state
        {
            get;
            set;
        }

        public WAConnectOptions connectOptions
        {
            get;
            set;
        }

        /// <summary>
        /// When to auto-reconnect
        /// </summary>
        public ReconnectMode autoReconnect
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the phone is connected
        /// </summary>
        public bool phoneConnected
        {
            get;
            set;
        }

        /// <summary>
        /// key to use to order chats
        /// </summary>
        public dynamic chatOrderingKey
        {
            get;
            set;
        }

        public dynamic logger
        {
            get;
            set;
        }

        /// <summary>
        /// log messages
        /// </summary>
        public bool shouldLogMessages
        {
            get;
            set;
        }

        public Array<(String tag, String json, bool fromMe, Array<dynamic> binaryTags)> messageLog
        {
            get;
            set;
        }

        public int maxCachedMessages
        {
            get;
            set;
        }

        public Date lastChatsReceived
        {
            get;
            set;
        }

        public KeyedDB chats
        {
            get;
            set;
        }

        public Hashtable<String, WAContact> contacts
        {
            get;
            set;
        }

        public Array<String> blocklist
        {
            get;
            set;
        }

        /// <summary>
        /// Data structure of tokens & IDs used to establish one's identiy to WhatsApp Web
        /// </summary>
        protected AuthenticationCredentials authInfo;
        /// <summary>
        /// Curve keys to initially authenticate
        /// </summary>
        protected (Uint8Array @private, Uint8Array @public)curveKeys;
        /// <summary>
        /// The websocket connection
        /// </summary>
        protected WS conn;
        protected int msgCount = 0;
        protected NodeJS.Timeout keepAliveReq;
        protected Encoder encoder = new Encoder();
        protected Decoder decoder = new Decoder();
        protected dynamic phoneCheckInterval;
        protected int phoneCheckListeners = 0;
        protected Date referenceDate = new Date();
        protected Date lastSeen = null;
        protected NodeJS.Timeout initTimeout;
        protected Date lastDisconnectTime = null;
        protected DisconnectReason lastDisconnectReason;
        protected MediaConnInfo mediaConn;
        protected dynamic connectionDebounceTimeout = Utils.debouncedTimeout(1000, () => this.state == "connecting" && this.endConnection(DisconnectReason.timedOut));
        protected dynamic messagesDebounceTimeout = Utils.debouncedTimeout(2000);
        protected dynamic chatsDebounceTimeout = Utils.debouncedTimeout(10000);
        /// <summary>
        /// Connect to WhatsAppWeb
        /// </summary>
        /// <param name = "options">
        /// the connect options
        /// </param>
        async public virtual void connect()
        {
            return null;
        }

        async public void unexpectedDisconnect(DisconnectReason error)
        {
            if (this.state == "open")
            {
                var willReconnect = (this.autoReconnect == ReconnectMode.onAllErrors || (this.autoReconnect == ReconnectMode.onConnectionLost && error != DisconnectReason.replaced)) && error != DisconnectReason.invalidSession;
                this.closeInternal(error, willReconnect);
                willReconnect && (this.connect().@catch((err) =>
                {
                }

                ));
            }
            else
            {
                this.endConnection(error);
            }
        }

        /// <summary>
        /// base 64 encode the authentication credentials and return them
        /// these can then be used to login again by passing the object to the connect () function.connect () in WhatsAppWeb.Session
        /// </summary>
        public void base64EncodedAuthInfo()
        {
            return new Dictionary<string, dynamic> ()
            {{"clientID", this.authInfo.clientID}, {"serverToken", this.authInfo.serverToken}, {"clientToken", this.authInfo.clientToken}, {"encKey", this.authInfo.encKey.toString("base64")}, {"macKey", this.authInfo.macKey.toString("base64")}};
        }

        /// <summary>
        /// Can you login to WA without scanning the QR
        /// </summary>
        public void canLogin()
        {
            return !!this.authInfo.encKey && !!this.authInfo.macKey;
        }

        /// <summary>
        /// Clear authentication info so a new connection can be created
        /// </summary>
        public void clearAuthInfo()
        {
            this.authInfo = null;
            return this;
        }

        /// <summary>
        /// Load in the authentication credentials
        /// </summary>
        /// <param name = "authInfo">
        /// the authentication credentials or file path to auth credentials
        /// </param>
        public void loadAuthInfo(dynamic authInfo)
        {
            if (!authInfo)
                throw new Error("given authInfo is null");
            if (TypeOf(authInfo) == "string")
            {
                this.logger.info($"loading authentication credentials from {authInfo}");
                var file = fs.readFileSync(authInfo, new Hashtable<String, String>()
                {{"encoding", "utf-8"}});
                authInfo = JSON.parse(file) as AnyAuthenticationCredentials;
            }

            if (AAA___ 'clientID' in  authInfo  ___AAA )
            {
                this.authInfo = new AuthenticationCredentials()
                {{"clientID", authInfo.clientID}, {"serverToken", authInfo.serverToken}, {"clientToken", authInfo.clientToken}, {"encKey", Buffer.isBuffer(authInfo.encKey) ? authInfo.encKey : Buffer.from(authInfo.encKey, "base64")}, {"macKey", Buffer.isBuffer(authInfo.macKey) ? authInfo.macKey : Buffer.from(authInfo.macKey, "base64")}};
            }
            else
            {
                (String encKey, String macKey)secretBundle = TypeOf(authInfo.WASecretBundle) == "string" ? JSON.parse(authInfo.WASecretBundle) : authInfo.WASecretBundle;
                this.authInfo = new AuthenticationCredentials()
                {{"clientID", authInfo.WABrowserId.replace(new RegExp("\\\"", "g"), "")}, {"serverToken", authInfo.WAToken2.replace(new RegExp("\\\"", "g"), "")}, {"clientToken", authInfo.WAToken1.replace(new RegExp("\\\"", "g"), "")}, {"encKey", Buffer.from(secretBundle.encKey, "base64")}, {"macKey", Buffer.from(secretBundle.macKey, "base64")}};
            }

            return this;
        }

        /// <summary>
        /// Wait for a message with a certain tag to be received
        /// </summary>
        /// <param name = "tag">
        /// the message tag to await
        /// </param>
        /// <param name = "json">
        /// query that was sent
        /// </param>
        /// <param name = "timeoutMs">
        /// timeout after which the promise will reject
        /// </param>
        async public void waitForMessage(String tag, bool requiresPhoneConnection, double timeoutMs = 0)
        {
            AAA___ (json) => void ___AAA onRecv;
            AAA___ (err) => void ___AAA onErr;
            AAA___ () => void ___AAA cancelPhoneChecker;
            if (requiresPhoneConnection)
            {
                this.startPhoneCheckInterval();
                cancelPhoneChecker = this.exitQueryIfResponseNotExpected(tag, (err) => onErr(err));
            }

            try
            {
                var result = await Utils.promiseTimeout(timeoutMs, (resolve, reject) =>
                {
                    onRecv = resolve;
                    onErr = ({ reason, status }) => reject(new BaileysError(reason, new
                    {
                    status = status
                    }

                    ));
                    this.on($"TAG:{tag}", onRecv);
                    this.on("ws-close", onErr);
                }

                );
                return result as dynamic;
            }
            finally
            {
                requiresPhoneConnection && this.clearPhoneCheckInterval();
                this.off($"TAG:{tag}", onRecv);
                this.off($"ws-close", onErr);
                cancelPhoneChecker && cancelPhoneChecker();
            }
        }

        /// <summary>
        /// Generic function for action, set queries
        /// </summary>
        async public void setQuery(Array<WANode> nodes, WATag binaryTags = new WATag{WAMetric.group, WAFlag.ignore}, String tag = null)
        {
            var json = new Array<String>{"action", (epoch: this.msgCount.toString(), type: "set"), nodes};
            var result = await this.query(new WAQuery()
            {{"json", json}, {"binaryTags", binaryTags}, {"tag", tag}, {"expect200", true}, {"requiresPhoneConnection", true}}) as Promise<Hashtable<String, double>>;
            return result;
        }

        /// <summary>
        /// Query something from the WhatsApp servers
        /// </summary>
        /// <param name = "json">
        /// the query itself
        /// </param>
        /// <param name = "binaryTags">
        /// the tags to attach if the query is supposed to be sent encoded in binary
        /// </param>
        /// <param name = "timeoutMs">
        /// timeout after which the query will be failed (set to null to disable a timeout)
        /// </param>
        /// <param name = "tag">
        /// the tag to attach to the message
        /// </param>
        async public Promise<dynamic> query(WAQuery q)
        {
            var {json, binaryTags, tag, timeoutMs, expect200, waitForOpen, longTag, requiresPhoneConnection, startDebouncedTimeout, maxRetries} = q;
            requiresPhoneConnection = requiresPhoneConnection != false;
            waitForOpen = waitForOpen != false;
            var triesLeft = maxRetries || 2;
            tag = tag || this.generateMessageTag(longTag);
            while (triesLeft >= 0)
            {
                if (waitForOpen)
                    await this.waitForConnection();
                var promise = this.waitForMessage(tag, requiresPhoneConnection, timeoutMs);
                if (this.logger.level == "trace")
                {
                    this.logger.trace(new Hashtable<String, bool>()
                    {{"fromMe", true}}, $"{tag},{JSON.stringify(json)}");
                }

                if (binaryTags)
                    tag = await this.sendBinary(json as WANode, binaryTags, tag);
                else
                    tag = await this.sendJSON(json, tag);
                try
                {
                    var response = await promise;
                    if (expect200 && response.status && Math.floor(+response.status / 100) != 2)
                    {
                        var message = STATUS_CODES[response.status] || "unknown";
                        throw new BaileysError($"Unexpected status in '{json[0] || "query"}': {STATUS_CODES[response.status]}({response.status})", new
                        {
                        query = json, message = message, status = response.status
                        }

                        );
                    }

                    if (startDebouncedTimeout)
                    {
                        this.connectionDebounceTimeout.start();
                    }

                    return response;
                }
                catch (Exception error)
                {
                    if (triesLeft == 0)
                    {
                        throw error;
                    }

                    if (error.status == 599)
                    {
                        this.unexpectedDisconnect(DisconnectReason.badSession);
                    }
                    else if ((error.message == "close" || error.message == "lost") && waitForOpen && this.state != "close" && (this.pendingRequestTimeoutMs == null || this.pendingRequestTimeoutMs > 0))
                    {
                    }
                    else
                        throw error;
                    triesLeft -= 1;
                    this.logger.debug($"query failed due to {error}, retrying...");
                }
            }
        }

        protected void exitQueryIfResponseNotExpected(String tag, AAA___ ({ reason, status }) => void ___AAA cancel)
        {
            NodeJS.Timeout timeout;
            var listener = ({ connected }) =>
            {
                if (connected)
                {
                    timeout = setTimeout(() =>
                    {
                        this.logger.info(new Hashtable<String, dynamic>()
                        {{"tag", tag}}, $"cancelling wait for message as a response is no longer expected from the phone");
                        cancel((reason: "Not expecting a response", status: 422));
                    }

                    , this.connectOptions.maxQueryResponseTime);
                    this.off("connection-phone-change", listener);
                }
            }

            ;
            this.on("connection-phone-change", listener);
            return () =>
            {
                this.off("connection-phone-change", listener);
                timeout && clearTimeout(timeout);
            }

            ;
        }

        /// <summary>
        /// interval is started when a query takes too long to respond
        /// </summary>
        protected void startPhoneCheckInterval()
        {
            this.phoneCheckListeners += 1;
            if (!this.phoneCheckInterval)
            {
                this.phoneCheckInterval = setInterval(() =>
                {
                    if (!this.conn)
                        return;
                    this.logger.info("checking phone connection...");
                    this.sendAdminTest();
                    if (this.phoneConnected != false)
                    {
                        this.phoneConnected = false;
                        this.emit("connection-phone-change", new Hashtable<String, bool>()
                        {{"connected", false}});
                    }
                }

                , this.connectOptions.phoneResponseTime);
            }
        }

        protected void clearPhoneCheckInterval()
        {
            this.phoneCheckListeners -= 1;
            if (this.phoneCheckListeners <= 0)
            {
                this.phoneCheckInterval && clearInterval(this.phoneCheckInterval);
                this.phoneCheckInterval = undefined;
                this.phoneCheckListeners = 0;
            }
        }

        /// <summary>
        /// checks for phone connection
        /// </summary>
        protected async void sendAdminTest()
        {
            return this.sendJSON(new dynamic{"admin", "test"});
        }

        /// <summary>
        /// Send a binary encoded message
        /// </summary>
        /// <param name = "json">
        /// the message to encode & send
        /// </param>
        /// <param name = "tags">
        /// the binary tags to tell WhatsApp what the message is all about
        /// </param>
        /// <param name = "tag">
        /// the tag to attach to the message
        /// </param>
        /// <returns>
        /// the message tag
        /// </returns>
        protected async void sendBinary(WANode json, WATag tags, String tag = null, bool longTag = false)
        {
            var binary = this.encoder.write(json);
            var buff = Utils.aesEncrypt(binary, this.authInfo.encKey);
            var sign = Utils.hmacSign(buff, this.authInfo.macKey);
            tag = tag || this.generateMessageTag(longTag);
            if (this.shouldLogMessages)
                this.messageLog.push((tag: tag, json: JSON.stringify(json), fromMe: true, binaryTags: tags));
            buff = Buffer.concat(new Array<dynamic>{Buffer.from(tag + ","), Buffer.from(tags), sign, buff});
            await this.send(buff);
            return tag;
        }

        /// <summary>
        /// Send a plain JSON message to the WhatsApp servers
        /// </summary>
        /// <param name = "json">
        /// the message to send
        /// </param>
        /// <param name = "tag">
        /// the tag to attach to the message
        /// </param>
        /// <returns>
        /// the message tag
        /// </returns>
        protected async void sendJSON(dynamic json, String tag = null, bool longTag = false)
        {
            tag = tag || this.generateMessageTag(longTag);
            if (this.shouldLogMessages)
                this.messageLog.push((tag: tag, json: JSON.stringify(json), fromMe: true));
            await this.send($"{tag},{JSON.stringify(json)}");
            return tag;
        }

        /// <summary>
        /// Send some message to the WhatsApp servers
        /// </summary>
        protected async void send(dynamic m)
        {
            this.conn.send(m);
        }

        protected async void waitForConnection()
        {
            if (this.state == "open")
                return;
            AAA___ () => void ___AAA onOpen;
            AAA___ ({ reason }) => void ___AAA onClose;
            if (this.pendingRequestTimeoutMs != null && this.pendingRequestTimeoutMs <= 0)
            {
                throw new BaileysError(DisconnectReason.close, new
                {
                status = 428
                }

                );
            }

            await (Utils.promiseTimeout(this.pendingRequestTimeoutMs, (resolve, reject) =>
            {
                onClose = ({ reason }) =>
                {
                    if (reason == DisconnectReason.invalidSession || reason == DisconnectReason.intentional)
                    {
                        reject(new Error(reason));
                    }
                }

                ;
                onOpen = resolve;
                this.on("close", onClose);
                this.on("open", onOpen);
            }

            ).finally(() =>
            {
                this.off("open", onOpen);
                this.off("close", onClose);
            }

            ));
        }

        /// <summary>
        /// Disconnect from the phone. Your auth credentials become invalid after sending a disconnect request.close() if you just want to close the connection
        /// </summary>
        async public void logout()
        {
            this.authInfo = null;
            if (this.state == "open")
            {
                await new Promise((resolve) => this.conn.send("goodbye,[\"admin\",\"Conn\",\"disconnect\"]", null, resolve));
            }

            this.user = undefined;
            this.chats.clear();
            this.contacts = new Hashtable<String, WAContact>();
            this.close();
        }

        /// <summary>
        /// Close the connection to WhatsApp Web
        /// </summary>
        public void close()
        {
            this.closeInternal(DisconnectReason.intentional);
        }

        protected void closeInternal(DisconnectReason reason = null, bool isReconnecting = false)
        {
            this.logger.info($"closed connection, reason {reason}{isReconnecting ? ", reconnecting in a few seconds..." : ""}");
            this.state = "close";
            this.phoneConnected = false;
            this.lastDisconnectReason = reason;
            this.lastDisconnectTime = new Date();
            this.endConnection(reason);
            this.emit("close", (reason: reason, isReconnecting: isReconnecting));
        }

        protected void endConnection(DisconnectReason reason)
        {
            this.conn.removeAllListeners("close");
            this.conn.removeAllListeners("error");
            this.conn.removeAllListeners("open");
            this.conn.removeAllListeners("message");
            this.initTimeout && clearTimeout(this.initTimeout);
            this.connectionDebounceTimeout.cancel();
            this.messagesDebounceTimeout.cancel();
            this.chatsDebounceTimeout.cancel();
            this.keepAliveReq && clearInterval(this.keepAliveReq);
            this.phoneCheckListeners = 0;
            this.clearPhoneCheckInterval();
            this.emit("ws-close", new Hashtable<String, dynamic>()
            {{"reason", reason}});
            try
            {
                this.conn.close();
            }
            catch
            {
            }

            this.conn = undefined;
            this.lastSeen = undefined;
            this.msgCount = 0;
        }

        /// <summary>
        /// Does a fetch request with the configuration of the connection
        /// </summary>
        protected dynamic fetchRequest = (endpoint, method = "GET", body = null, agent = null, headers = null, followRedirect = true) => (got(endpoint, (method: method, body: body, followRedirect: followRedirect, headers: new
        {
        Origin = DEFAULT_ORIGIN
        }

        , agent: new
        {
        https = agent || this.connectOptions.fetchAgent
        }

        )));
        public void generateMessageTag(bool longTag = false)
        {
            var seconds = Utils.unixTimestampSeconds(this.referenceDate);
            var tag = $"{longTag ? seconds : (seconds % 1000)}.--{this.msgCount}";
            this.msgCount += 1;
            return tag;
        }
    }
}