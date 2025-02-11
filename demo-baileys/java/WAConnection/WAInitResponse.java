/// This file was generated by Java converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

package demo;

public interface WABusinessProfile {
    
    String getDescription();
    
    void setDescription(String value);
    
    String getEmail();
    
    void setEmail(String value);
    
    WABusinessHours getBusiness_hours();
    
    void setBusiness_hours(WABusinessHours value);
    
    ArrayList<String> getWebsite();
    
    void setWebsite(ArrayList<String> value);
    
    ArrayList<WABusinessCategories> getCategories();
    
    void setCategories(ArrayList<WABusinessCategories> value);
    
    String getWid();
    
    void setWid(String value);
}
public interface WALocationMessage {
    
    double getDegreesLatitude();
    
    void setDegreesLatitude(double value);
    
    double getDegreesLongitude();
    
    void setDegreesLongitude(double value);
    
    String getAddress();
    
    void setAddress(String value);
}
public class BaileysError extends Error {
    private double status;
    
    public double getStatus() {
        return status;
    }
    
    public void setStatus(double value) {
        this.status = value;
    }
    private AnyXXXXXX context;
    
    public AnyXXXXXX getContext() {
        return context;
    }
    
    public void setContext(AnyXXXXXX value) {
        this.context = value;
    }
    
    public /*missing*/ BaileysError(String message, AnyXXXXXX context) {
        this(message, context, null);
    }
    
    public /*missing*/ BaileysError(String message, AnyXXXXXX context, String stack) {
        super(message);
        this.setName("BaileysError");
        this.setStatus(context.getStatus());
        this.setContext(context);
        if (stack) {
            this.setStack(stack);
        }
    }
}
public interface WAQuery {
    
    /*missing*/ getJson();
    
    void setJson(/*missing*/ value);
    
    WATag getBinaryTags();
    
    void setBinaryTags(WATag value);
    
    double getTimeoutMs();
    
    void setTimeoutMs(double value);
    
    String getTag();
    
    void setTag(String value);
    
    boolean getExpect200();
    
    void setExpect200(boolean value);
    
    boolean getWaitForOpen();
    
    void setWaitForOpen(boolean value);
    
    boolean getLongTag();
    
    void setLongTag(boolean value);
    
    boolean getRequiresPhoneConnection();
    
    void setRequiresPhoneConnection(boolean value);
    
    boolean getStartDebouncedTimeout();
    
    void setStartDebouncedTimeout(boolean value);
    
    double getMaxRetries();
    
    void setMaxRetries(double value);
}
public enum ReconnectMode {
    /** does not reconnect */
    off /* = new off(0) */ /*enum*/ (0),
    /** reconnects only when the connection is 'lost' or 'close' */
    onConnectionLost /* = new onConnectionLost(1) */ /*enum*/ (1),
    /** reconnects on all disconnects, including take overs */
    onAllErrors /* = new onAllErrors(2) */ /*enum*/ (2);
    private final int value;
    
    private /*missing*/ ReconnectMode(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
/** Types of Disconnect Reasons */
public enum DisconnectReason {
    /** The connection was closed intentionally */
    intentional /* = new intentional("intentional") */ /*enum*/ ("intentional"),
    /** The connection was terminated either by the client or server */
    close /* = new close("close") */ /*enum*/ ("close"),
    /** The connection was lost, called when the server stops responding to requests */
    lost /* = new lost("lost") */ /*enum*/ ("lost"),
    /** When WA Web is opened elsewhere & this session is disconnected */
    replaced /* = new replaced("replaced") */ /*enum*/ ("replaced"),
    /** The credentials for the session have been invalidated, i.e. logged out either from the phone or WA Web */
    invalidSession /* = new invalidSession("invalid_session") */ /*enum*/ ("invalid_session"),
    /** Received a 500 result in a query -- something has gone very wrong */
    badSession /* = new badSession("bad_session") */ /*enum*/ ("bad_session"),
    /** No idea, can be a sign of log out too */
    unknown /* = new unknown("unknown") */ /*enum*/ ("unknown"),
    /** Well, the connection timed out */
    timedOut /* = new timedOut("timed out") */ /*enum*/ ("timed out");
    private final int value;
    
    private /*missing*/ DisconnectReason(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
public interface MediaConnInfo {
    
    String getAuth();
    
    void setAuth(String value);
    
    double getTtl();
    
    void setTtl(double value);
    
    ArrayList<Unit<String>> getHosts();
    
    void setHosts(ArrayList<Unit<String>> value);
    
    Date getFetchDate();
    
    void setFetchDate(Date value);
}
public interface AuthenticationCredentials {
    
    String getClientID();
    
    void setClientID(String value);
    
    String getServerToken();
    
    void setServerToken(String value);
    
    String getClientToken();
    
    void setClientToken(String value);
    
    Buffer getEncKey();
    
    void setEncKey(Buffer value);
    
    Buffer getMacKey();
    
    void setMacKey(Buffer value);
}
public interface AuthenticationCredentialsBase64 {
    
    String getClientID();
    
    void setClientID(String value);
    
    String getServerToken();
    
    void setServerToken(String value);
    
    String getClientToken();
    
    void setClientToken(String value);
    
    String getEncKey();
    
    void setEncKey(String value);
    
    String getMacKey();
    
    void setMacKey(String value);
}
public interface AuthenticationCredentialsBrowser {
    
    String getWABrowserId();
    
    void setWABrowserId(String value);
    
    /*missing*/ getWASecretBundle();
    
    void setWASecretBundle(/*missing*/ value);
    
    String getWAToken1();
    
    void setWAToken1(String value);
    
    String getWAToken2();
    
    void setWAToken2(String value);
}
public interface WAGroupCreateResponse {
    
    double getStatus();
    
    void setStatus(double value);
    
    String getGid();
    
    void setGid(String value);
    
    /*missing*/ getParticipants();
    
    void setParticipants(/*missing*/ value);
}
public interface WAGroupMetadata {
    
    String getId();
    
    void setId(String value);
    
    String getOwner();
    
    void setOwner(String value);
    
    String getSubject();
    
    void setSubject(String value);
    
    double getCreation();
    
    void setCreation(double value);
    
    String getDesc();
    
    void setDesc(String value);
    
    String getDescOwner();
    
    void setDescOwner(String value);
    
    String getDescId();
    
    void setDescId(String value);
    
    /** is set when the group only allows admins to change group settings */
    /*missing*/ getRestrict();
    
    /** is set when the group only allows admins to change group settings */
    void setRestrict(/*missing*/ value);
    
    /** is set when the group only allows admins to write messages */
    /*missing*/ getAnnounce();
    
    /** is set when the group only allows admins to write messages */
    void setAnnounce(/*missing*/ value);
    
    ArrayList<WAGroupParticipant> getParticipants();
    
    void setParticipants(ArrayList<WAGroupParticipant> value);
}
public interface WAGroupModification {
    
    double getStatus();
    
    void setStatus(double value);
    
    HashMap<String, AnyXXXXXX> getParticipants();
    
    void setParticipants(HashMap<String, AnyXXXXXX> value);
}
public interface WAPresenceData {
    
    Presence getLastKnownPresence();
    
    void setLastKnownPresence(Presence value);
    
    double getLastSeen();
    
    void setLastSeen(double value);
    
    String getName();
    
    void setName(String value);
}
public interface WAContact {
    
    String getVerify();
    
    void setVerify(String value);
    
    /** name of the contact, the contact has set on their own on WA */
    String getNotify();
    
    /** name of the contact, the contact has set on their own on WA */
    void setNotify(String value);
    
    String getJid();
    
    void setJid(String value);
    
    /** I have no idea */
    String getVname();
    
    /** I have no idea */
    void setVname(String value);
    
    /** name of the contact, you have saved on your WA */
    String getName();
    
    /** name of the contact, you have saved on your WA */
    void setName(String value);
    
    String getIndex();
    
    void setIndex(String value);
    
    /** short name for the contact */
    String getShort();
    
    /** short name for the contact */
    void setShort(String value);
    
    String getImgUrl();
    
    void setImgUrl(String value);
}
public interface WAUser extends WAContact {
    
    AnyXXXXXX getPhone();
    
    void setPhone(AnyXXXXXX value);
}
public interface WAChat {
    
    String getJid();
    
    void setJid(String value);
    
    double getT();
    
    void setT(double value);
    
    /** number of unread messages, is < 0 if the chat is manually marked unread */
    double getCount();
    
    /** number of unread messages, is < 0 if the chat is manually marked unread */
    void setCount(double value);
    
    /*missing*/ getArchive();
    
    void setArchive(/*missing*/ value);
    
    /*missing*/ getClear();
    
    void setClear(/*missing*/ value);
    
    /*missing*/ getRead_only();
    
    void setRead_only(/*missing*/ value);
    
    String getMute();
    
    void setMute(String value);
    
    String getPin();
    
    void setPin(String value);
    
    /*missing*/ getSpam();
    
    void setSpam(/*missing*/ value);
    
    String getModify_tag();
    
    void setModify_tag(String value);
    
    String getName();
    
    void setName(String value);
    
    /** when ephemeral messages were toggled on */
    String getEph_setting_ts();
    
    /** when ephemeral messages were toggled on */
    void setEph_setting_ts(String value);
    
    /** how long each message lasts for */
    String getEphemeral();
    
    /** how long each message lasts for */
    void setEphemeral(String value);
    
    KeyedDB<WAMessage, String> getMessages();
    
    void setMessages(KeyedDB<WAMessage, String> value);
    
    String getImgUrl();
    
    void setImgUrl(String value);
    
    HashMap<String, WAPresenceData> getPresences();
    
    void setPresences(HashMap<String, WAPresenceData> value);
    
    WAGroupMetadata getMetadata();
    
    void setMetadata(WAGroupMetadata value);
}
public enum WAMetric {
    debugLog /* = new debugLog(1) */ /*enum*/ (1),
    queryResume /* = new queryResume(2) */ /*enum*/ (2),
    liveLocation /* = new liveLocation(3) */ /*enum*/ (3),
    queryMedia /* = new queryMedia(4) */ /*enum*/ (4),
    queryChat /* = new queryChat(5) */ /*enum*/ (5),
    queryContact /* = new queryContact(6) */ /*enum*/ (6),
    queryMessages /* = new queryMessages(7) */ /*enum*/ (7),
    presence /* = new presence(8) */ /*enum*/ (8),
    presenceSubscribe /* = new presenceSubscribe(9) */ /*enum*/ (9),
    group /* = new group(10) */ /*enum*/ (10),
    read /* = new read(11) */ /*enum*/ (11),
    chat /* = new chat(12) */ /*enum*/ (12),
    received /* = new received(13) */ /*enum*/ (13),
    picture /* = new picture(14) */ /*enum*/ (14),
    status /* = new status(15) */ /*enum*/ (15),
    message /* = new message(16) */ /*enum*/ (16),
    queryActions /* = new queryActions(17) */ /*enum*/ (17),
    block /* = new block(18) */ /*enum*/ (18),
    queryGroup /* = new queryGroup(19) */ /*enum*/ (19),
    queryPreview /* = new queryPreview(20) */ /*enum*/ (20),
    queryEmoji /* = new queryEmoji(21) */ /*enum*/ (21),
    queryRead /* = new queryRead(22) */ /*enum*/ (22),
    queryVCard /* = new queryVCard(29) */ /*enum*/ (29),
    queryStatus /* = new queryStatus(30) */ /*enum*/ (30),
    queryStatusUpdate /* = new queryStatusUpdate(31) */ /*enum*/ (31),
    queryLiveLocation /* = new queryLiveLocation(33) */ /*enum*/ (33),
    queryLabel /* = new queryLabel(36) */ /*enum*/ (36),
    queryQuickReply /* = new queryQuickReply(39) */ /*enum*/ (39);
    private final int value;
    
    private /*missing*/ WAMetric(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
public enum WAFlag {
    available /* = new available(160) */ /*enum*/ (160),
    other /* = new other(136) */ /*enum*/ (136),
    ignore /* = new ignore(1 << 7) */ /*enum*/ (1 << 7),
    acknowledge /* = new acknowledge(1 << 6) */ /*enum*/ (1 << 6),
    unavailable /* = new unavailable(1 << 4) */ /*enum*/ (1 << 4),
    expires /* = new expires(1 << 3) */ /*enum*/ (1 << 3),
    composing /* = new composing(1 << 2) */ /*enum*/ (1 << 2),
    recording /* = new recording(1 << 2) */ /*enum*/ (1 << 2),
    paused /* = new paused(1 << 2) */ /*enum*/ (1 << 2);
    private final int value;
    
    private /*missing*/ WAFlag(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
/** set of statuses visible to other people; see updatePresence() in WhatsAppWeb.Send */
public enum Presence {
    unavailable /* = new unavailable("unavailable") */ /*enum*/ ("unavailable"),
    available /* = new available("available") */ /*enum*/ ("available"),
    composing /* = new composing("composing") */ /*enum*/ ("composing"),
    recording /* = new recording("recording") */ /*enum*/ ("recording"),
    paused /* = new paused("paused") */ /*enum*/ ("paused");
    private final int value;
    
    private /*missing*/ Presence(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
/** Set of message types that are supported by the library */
public enum MessageType {
    text /* = new text("conversation") */ /*enum*/ ("conversation"),
    extendedText /* = new extendedText("extendedTextMessage") */ /*enum*/ ("extendedTextMessage"),
    contact /* = new contact("contactMessage") */ /*enum*/ ("contactMessage"),
    contactsArray /* = new contactsArray("contactsArrayMessage") */ /*enum*/ ("contactsArrayMessage"),
    groupInviteMessage /* = new groupInviteMessage("groupInviteMessage") */ /*enum*/ ("groupInviteMessage"),
    listMessage /* = new listMessage("listMessage") */ /*enum*/ ("listMessage"),
    buttonsMessage /* = new buttonsMessage("buttonsMessage") */ /*enum*/ ("buttonsMessage"),
    location /* = new location("locationMessage") */ /*enum*/ ("locationMessage"),
    liveLocation /* = new liveLocation("liveLocationMessage") */ /*enum*/ ("liveLocationMessage"),
    image /* = new image("imageMessage") */ /*enum*/ ("imageMessage"),
    video /* = new video("videoMessage") */ /*enum*/ ("videoMessage"),
    sticker /* = new sticker("stickerMessage") */ /*enum*/ ("stickerMessage"),
    document /* = new document("documentMessage") */ /*enum*/ ("documentMessage"),
    audio /* = new audio("audioMessage") */ /*enum*/ ("audioMessage"),
    product /* = new product("productMessage") */ /*enum*/ ("productMessage");
    private final int value;
    
    private /*missing*/ MessageType(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
public enum ChatModification {
    archive /* = new archive("archive") */ /*enum*/ ("archive"),
    unarchive /* = new unarchive("unarchive") */ /*enum*/ ("unarchive"),
    pin /* = new pin("pin") */ /*enum*/ ("pin"),
    unpin /* = new unpin("unpin") */ /*enum*/ ("unpin"),
    mute /* = new mute("mute") */ /*enum*/ ("mute"),
    unmute /* = new unmute("unmute") */ /*enum*/ ("unmute"),
    delete /* = new delete("delete") */ /*enum*/ ("delete"),
    clear /* = new clear("clear") */ /*enum*/ ("clear");
    private final int value;
    
    private /*missing*/ ChatModification(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
public enum Mimetype {
    jpeg /* = new jpeg("image/jpeg") */ /*enum*/ ("image/jpeg"),
    png /* = new png("image/png") */ /*enum*/ ("image/png"),
    mp4 /* = new mp4("video/mp4") */ /*enum*/ ("video/mp4"),
    gif /* = new gif("video/gif") */ /*enum*/ ("video/gif"),
    pdf /* = new pdf("application/pdf") */ /*enum*/ ("application/pdf"),
    ogg /* = new ogg("audio/ogg; codecs=opus") */ /*enum*/ ("audio/ogg; codecs=opus"),
    mp4Audio /* = new mp4Audio("audio/mp4") */ /*enum*/ ("audio/mp4"),
    /** for stickers */
    webp /* = new webp("image/webp") */ /*enum*/ ("image/webp");
    private final int value;
    
    private /*missing*/ Mimetype(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
public interface MessageOptions {
    
    /** the message you want to quote */
    WAMessage getQuoted();
    
    /** the message you want to quote */
    void setQuoted(WAMessage value);
    
    /** some random context info (can show a forwarded message with this too) */
    WAContextInfo getContextInfo();
    
    /** some random context info (can show a forwarded message with this too) */
    void setContextInfo(WAContextInfo value);
    
    /** optional, if you want to manually set the timestamp of the message */
    Date getTimestamp();
    
    /** optional, if you want to manually set the timestamp of the message */
    void setTimestamp(Date value);
    
    /** (for media messages) the caption to send with the media (cannot be sent with stickers though) */
    String getCaption();
    
    /** (for media messages) the caption to send with the media (cannot be sent with stickers though) */
    void setCaption(String value);
    
    /**
     * For location & media messages -- has to be a base 64 encoded JPEG if you want to send a custom thumb,
     * or set to null if you don't want to send a thumbnail.
     * Do not enter this field if you want to automatically generate a thumb
     * */
    String getThumbnail();
    
    /**
     * For location & media messages -- has to be a base 64 encoded JPEG if you want to send a custom thumb,
     * or set to null if you don't want to send a thumbnail.
     * Do not enter this field if you want to automatically generate a thumb
     * */
    void setThumbnail(String value);
    
    /** (for media messages) specify the type of media (optional for all media types except documents) */
    /*missing*/ getMimetype();
    
    /** (for media messages) specify the type of media (optional for all media types except documents) */
    void setMimetype(/*missing*/ value);
    
    /** (for media messages) file name for the media */
    String getFilename();
    
    /** (for media messages) file name for the media */
    void setFilename(String value);
    
    /** For audio messages, if set to true, will send as a `voice note` */
    boolean getPtt();
    
    /** For audio messages, if set to true, will send as a `voice note` */
    void setPtt(boolean value);
    
    /** For image or video messages, if set to true, will send as a `viewOnceMessage` */
    boolean getViewOnce();
    
    /** For image or video messages, if set to true, will send as a `viewOnceMessage` */
    void setViewOnce(boolean value);
    
    /** Optional agent for media uploads */
    Agent getUploadAgent();
    
    /** Optional agent for media uploads */
    void setUploadAgent(Agent value);
    
    /** If set to true (default), automatically detects if you're sending a link & attaches the preview*/
    boolean getDetectLinks();
    
    /** If set to true (default), automatically detects if you're sending a link & attaches the preview*/
    void setDetectLinks(boolean value);
    
    /** Optionally specify the duration of the media (audio/video) in seconds */
    double getDuration();
    
    /** Optionally specify the duration of the media (audio/video) in seconds */
    void setDuration(double value);
    
    /** Fetches new media options for every media file */
    boolean getForceNewMediaOptions();
    
    /** Fetches new media options for every media file */
    void setForceNewMediaOptions(boolean value);
    
    /** Wait for the message to be sent to the server (default true) */
    boolean getWaitForAck();
    
    /** Wait for the message to be sent to the server (default true) */
    void setWaitForAck(boolean value);
    
    /**
     * By default 'chat' -- which follows the setting of the chat */
    /*missing*/ getSendEphemeral();
    
    /**
     * By default 'chat' -- which follows the setting of the chat */
    void setSendEphemeral(/*missing*/ value);
    
    /** Force message id */
    String getMessageId();
    
    /** Force message id */
    void setMessageId(String value);
    
    /** For sticker messages, if set to true, will considered as animated sticker  */
    boolean getIsAnimated();
    
    /** For sticker messages, if set to true, will considered as animated sticker  */
    void setIsAnimated(boolean value);
}
public interface WABroadcastListInfo {
    
    double getStatus();
    
    void setStatus(double value);
    
    String getName();
    
    void setName(String value);
    
    ArrayList<Unit<String>> getRecipients();
    
    void setRecipients(ArrayList<Unit<String>> value);
}
public interface WAUrlInfo {
    
    String getCanonical-url();
    
    void setCanonical-url(String value);
    
    String getMatched-text();
    
    void setMatched-text(String value);
    
    String getTitle();
    
    void setTitle(String value);
    
    String getDescription();
    
    void setDescription(String value);
    
    Buffer getJpegThumbnail();
    
    void setJpegThumbnail(Buffer value);
}
public interface WAProfilePictureChange {
    
    double getStatus();
    
    void setStatus(double value);
    
    String getTag();
    
    void setTag(String value);
    
    String getEurl();
    
    void setEurl(String value);
}
public interface MessageInfo {
    
    ArrayList<Pair<String, String>> getReads();
    
    void setReads(ArrayList<Pair<String, String>> value);
    
    ArrayList<Pair<String, String>> getDeliveries();
    
    void setDeliveries(ArrayList<Pair<String, String>> value);
}
public interface WAMessageStatusUpdate {
    
    String getFrom();
    
    void setFrom(String value);
    
    String getTo();
    
    void setTo(String value);
    
    /** Which participant caused the update (only for groups) */
    String getParticipant();
    
    /** Which participant caused the update (only for groups) */
    void setParticipant(String value);
    
    Date getTimestamp();
    
    void setTimestamp(Date value);
    
    /** Message IDs read/delivered */
    ArrayList<String> getIds();
    
    /** Message IDs read/delivered */
    void setIds(ArrayList<String> value);
    
    /** Status of the Message IDs */
    WA_MESSAGE_STATUS_TYPE getType();
    
    /** Status of the Message IDs */
    void setType(WA_MESSAGE_STATUS_TYPE value);
}
public interface WAOpenResult {
    
    /** Was this connection opened via a QR scan */
    /*missing*/ getNewConnection();
    
    /** Was this connection opened via a QR scan */
    void setNewConnection(/*missing*/ value);
    
    WAUser getUser();
    
    void setUser(WAUser value);
    
    /*missing*/ getIsNewUser();
    
    void setIsNewUser(/*missing*/ value);
    
    AuthenticationCredentials getAuth();
    
    void setAuth(AuthenticationCredentials value);
}
public enum GroupSettingChange {
    messageSend /* = new messageSend("announcement") */ /*enum*/ ("announcement"),
    settingsChange /* = new settingsChange("locked") */ /*enum*/ ("locked");
    private final int value;
    
    private /*missing*/ GroupSettingChange(int value) {
        this.value = value;
    }
    
    public final int value() {
        return this.value;
    }
}
public interface PresenceUpdate {
    
    String getId();
    
    void setId(String value);
    
    String getParticipant();
    
    void setParticipant(String value);
    
    String getT();
    
    void setT(String value);
    
    Presence getType();
    
    void setType(Presence value);
    
    boolean getDeny();
    
    void setDeny(boolean value);
}
public interface BlocklistUpdate {
    
    ArrayList<String> getAdded();
    
    void setAdded(ArrayList<String> value);
    
    ArrayList<String> getRemoved();
    
    void setRemoved(ArrayList<String> value);
}