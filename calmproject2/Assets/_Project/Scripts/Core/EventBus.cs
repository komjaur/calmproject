using System;
using System.Collections.Generic;

namespace SurvivalChaos
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> events = new Dictionary<Type, Delegate>();

        public static void Subscribe<T>(Action<T> callback)
        {
            if (events.TryGetValue(typeof(T), out var del))
                events[typeof(T)] = Delegate.Combine(del, callback);
            else
                events[typeof(T)] = callback;
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            if (events.TryGetValue(typeof(T), out var del))
            {
                var current = Delegate.Remove(del, callback);
                if (current == null) events.Remove(typeof(T));
                else events[typeof(T)] = current;
            }
        }

        public static void Raise<T>(T evt)
        {
            if (events.TryGetValue(typeof(T), out var del))
                (del as Action<T>)?.Invoke(evt);
        }
    }
}
