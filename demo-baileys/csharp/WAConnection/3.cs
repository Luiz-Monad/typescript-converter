/// This file was generated by C# converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

using System.Linq;
using TypeScript.CSharp;
using Utils = Bailey;
using Base = Bailey.WAConnection;

namespace Bailey
{
    public class WAConnection : Base
    {
        /// <summary>
        /// Connect to WhatsApp Web
        /// </summary>
        async public override void connect()
        {
            if (this.state != "close")
            {
                throw new BaileysError("cannot connect when state=" + this.state, new Dictionary<string, int>() { { "status", 409 } });
            }

            var options = this.connectOptions;
            var newConnection = !this.authInfo;
            this.state = "connecting";
            this.emit("connecting");
            var tries = 0;
            var lastConnect = this.lastDisconnectTime;
            WAOpenResult result;
            while (this.state == "connecting")
            {
                tries += 1;
                try
                {
                    var diff = lastConnect ? new Date().getTime() - lastConnect.getTime() : Infinity;
                    result = await this.connectInternal(options, diff > this.connectOptions.connectCooldownMs ? 0 : this.connectOptions.connectCooldownMs);
                    this.phoneConnected = true;
                    this.state = "open";
                }
                catch (Exception error)
                {
                    lastConnect = new Date();
                    var loggedOut = error is BaileysError && UNAUTHORIZED_CODES.includes((error as BaileysError).status);
                    var willReconnect = !loggedOut && (tries < options.maxRetries) && (this.state == "connecting");
                    var reason = loggedOut ? DisconnectReason.invalidSession : error.message;
                    this.logger.warn(new Dictionary<string, dynamic>() { { "error", error } }, $"connect attempt {tries} failed: {error}{willReconnect ? ", retrying..." : ""}");
                    if ((this.state as string) != "close" && !willReconnect)
                    {
                        this.closeInternal(reason);
                    }

                    if (!willReconnect)
                        throw error;
                }
            }

            if (newConnection)
                result.newConnection = newConnection;
            this.emit("open", result);
            this.logger.info("opened connection to WhatsApp Web");
            this.conn.on("close", () => this.unexpectedDisconnect(DisconnectReason.close));
            return result;
        }

        /// <summary>
        /// Meat of the connect logic
        /// </summary>
        protected async void connectInternal(WAConnectOptions options, double delayMs = 0)
        {
            List<Action<Error>> rejections = new List<Action<Error>>();
            var rejectAll = (e) => rejections.forEach((r) => r(e));
            var rejectAllOnWSClose = ({ reason }) => rejectAll(new Error(reason));
            var connect = () => (new Promise((resolve, reject) =>
            {
                rejections.push(reject);
                var shouldUseReconnect = (this.lastDisconnectReason == DisconnectReason.close || this.lastDisconnectReason == DisconnectReason.lost) && !this.connectOptions.alwaysUseTakeover;
                var reconnectID = shouldUseReconnect && this.user.jid.replace("@s.whatsapp.net", "@c.us");
                this.conn = new WS(WS_URL, null, (origin: DEFAULT_ORIGIN, timeout: this.connectOptions.maxIdleTimeMs, agent: options.agent, headers: (Accept_Encoding: "gzip, deflate, br", Accept_Language: "en-US,en;q=0.9", Cache_Control: "no-cache", Host: "web.whatsapp.com", Pragma: "no-cache", Sec_WebSocket_Extensions: "permessage-deflate; client_max_window_bits")));
                this.conn.on("message", (data) => this.onMessageRecieved(data as dynamic));
                this.conn.once("open", () =>
                {
                    this.startKeepAliveRequest();
                    this.logger.info($"connected to WhatsApp Web server, authenticating via {reconnectID ? "reconnect" : "takeover"}");
                    try
                    {
                        this.connectionDebounceTimeout.setInterval(this.connectOptions.maxIdleTimeMs);
                        var authResult = await this.authenticate(reconnectID);
                        this.conn.removeAllListeners("error").removeAllListeners("close");
                        this.connectionDebounceTimeout.start();
                        resolve(authResult as WAOpenResult);
                    }
                    catch (Exception error)
                    {
                        reject(error);
                    }
                });
                this.conn.on("error", rejectAll);
                this.conn.on("close", () => rejectAll(new Error(DisconnectReason.close)));
            }) as Promise<WAOpenResult>);
            this.on("ws-close", rejectAllOnWSClose);
            try
            {
                if (delayMs)
                {
                    var {delay, cancel} = Utils.delayCancellable(delayMs);
                    rejections.push(cancel);
                    await delay;
                }

                var result = await connect();
                return result;
            }
            catch (Exception error)
            {
                if (this.conn)
                {
                    this.endConnection(error.message);
                }

                throw error;
            }
            finally
            {
                this.off("ws-close", rejectAllOnWSClose);
            }
        }

        private void onMessageRecieved(OrType<string, Buffer> message)
        {
            if (message[0] == "!")
            {
                var timestamp = message.slice(1, message.length).toString("utf-8");
                this.lastSeen = new Date(parseInt(timestamp));
                this.emit("received-pong");
            }
            else
            {
                string messageTag;
                dynamic json;
                try
                {
                    var dec = Utils.decryptWA(message, this.authInfo.macKey, this.authInfo.encKey, new Decoder());
                    messageTag = dec[0];
                    json = dec[1];
                }
                catch (Exception error)
                {
                    this.logger.error(new Dictionary<string, dynamic>() { { "error", error } }, $"encountered error in decrypting message, closing: {error}");
                    this.unexpectedDisconnect(DisconnectReason.badSession);
                }

                if (this.shouldLogMessages)
                    this.messageLog.push((tag: messageTag, json: JSON.stringify(json), fromMe: false));
                if (!json)
                    return;
                if (this.logger.level == "trace")
                {
                    this.logger.trace(messageTag + "," + JSON.stringify(json));
                }

                var anyTriggered = false;
                anyTriggered = this.emit($"{DEF_TAG_PREFIX}{messageTag}", json);
                var l0 = json[0] || "";
                var l1 = TypeOf(json[1]) != "object" || json[1] == null ? new Dictionary<string, dynamic>() : json[1];
                var l2 = ((json[2] || new List<dynamic>())[0] || new List<dynamic>())[0] || "";
                Object.keys(l1).forEach((key) =>
                {
                    anyTriggered = this.emit($"{DEF_CALLBACK_PREFIX}{l0},{key}:{l1[key]},{l2}", json) || anyTriggered;
                    anyTriggered = this.emit($"{DEF_CALLBACK_PREFIX}{l0},{key}:{l1[key]}", json) || anyTriggered;
                    anyTriggered = this.emit($"{DEF_CALLBACK_PREFIX}{l0},{key}", json) || anyTriggered;
                });
                anyTriggered = this.emit($"{DEF_CALLBACK_PREFIX}{l0},,{l2}", json) || anyTriggered;
                anyTriggered = this.emit($"{DEF_CALLBACK_PREFIX}{l0}", json) || anyTriggered;
                if (anyTriggered)
                    return;
                if (this.logger.level == "debug")
                {
                    this.logger.debug(new Dictionary<string, bool>() { { "unhandled", true } }, messageTag + "," + JSON.stringify(json));
                }
            }
        }

        /// <summary>
        /// Send a keep alive request every X seconds, server updates & responds with last seen
        /// </summary>
        private void startKeepAliveRequest()
        {
            this.keepAliveReq && clearInterval(this.keepAliveReq);
            this.keepAliveReq = setInterval(() =>
            {
                if (!this.lastSeen)
                    this.lastSeen = new Date();
                var diff = new Date().getTime() - this.lastSeen.getTime();
                if (diff > KEEP_ALIVE_INTERVAL_MS + 5000)
                    this.unexpectedDisconnect(DisconnectReason.lost);
                else if (this.conn)
                    this.send("?,,");
            }, KEEP_ALIVE_INTERVAL_MS);
        }
    }
}