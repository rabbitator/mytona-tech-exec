using System.Collections.Generic;

public static class EventBus
{
    public const int PLAYER_DEATH = 0;
    public const int PLAYER_WON = 1;
    public const int MOB_KILLED = 2;

    public delegate void MessageHandlerDelegate();

    private static readonly Dictionary<int, MessageHandlerDelegate> MessageHandlers;

    static EventBus()
    {
        MessageHandlers = new Dictionary<int, MessageHandlerDelegate>();
    }

    public static void Clear()
    {
        MessageHandlers.Clear();
    }

    public static void Pub(int index)
    {
        if (MessageHandlers.TryGetValue(index, out var handler))
        {
            handler?.Invoke();
        }
    }

    public static void Sub(MessageHandlerDelegate @delegate, int index)
    {
        if (@delegate != null)
        {
            if (!MessageHandlers.ContainsKey(index))
            {
                MessageHandlers.Add(index, null);
            }

            MessageHandlers[index] += @delegate;
        }
    }

    public static void Unsub(MessageHandlerDelegate @delegate, int index)
    {
        if (@delegate == null) return;
        if (!MessageHandlers.ContainsKey(index)) return;

        MessageHandlers[index] -= @delegate;

        if (MessageHandlers[index] != null) return;

        MessageHandlers.Remove(index);
    }
}

public static class EventBus<T> where T : Message
{
    public delegate void MessageHandlerDelegate(T message);

    private static MessageHandlerDelegate Handlers = null;
    private static List<MessageHandlerDelegate> MessageHandlers;

    static EventBus()
    {
        MessageHandlers = new List<MessageHandlerDelegate>();
    }

    public static void Clear()
    {
        MessageHandlers.Clear();
    }

    public static void Pub(T message)
    {
        Handlers?.Invoke(message);
    }

    public static void Sub(MessageHandlerDelegate @delegate)
    {
        if (@delegate != null)
        {
            Handlers += @delegate;
        }
    }

    public static void Unsub(MessageHandlerDelegate @delegate)
    {
        if (@delegate != null)
        {
            Handlers -= @delegate;
        }
    }
}