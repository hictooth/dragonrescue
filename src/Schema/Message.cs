using System;
namespace dragonrescue.Schema;

public class Message {
    public int? MessageID;

    public string Creator;

    public MessageLevel MessageLevel;

    public MessageType MessageType;

    public string Content;

    public int? ReplyToMessageID;

    public DateTime CreateTime;

    public DateTime? UpdateDate;

    public int ConversationID;

    public string DisplayAttribute;

    public bool isPrivate;
}
