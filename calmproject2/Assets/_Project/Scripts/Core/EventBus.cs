using System;
using System.Collections.Generic;

namespace SurvivalChaos
{
    /// <summary>
    /// Lightweight publish/subscribe system for decoupled game events.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new();

        /// <summary>
        /// Registers a listener for events of type <typeparamref name="T"/>.
        /// </summary>
        public static void Subscribe<T>(Action<T> callback)
        {
            if (callback == null) return;

            if (_events.TryGetValue(typeof(T), out var existing))
                _events[typeof(T)] = Delegate.Combine(existing, callback);
            else
                _events[typeof(T)] = callback;
        }

        /// <summary>
        /// Removes a listener for events of type <typeparamref name="T"/>.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> callback)
        {
            if (callback == null) return;

            if (_events.TryGetValue(typeof(T), out var existing))
            {
                var remaining = Delegate.Remove(existing, callback);
                if (remaining == null)
                    _events.Remove(typeof(T));
                else
                    _events[typeof(T)] = remaining;
            }
        }

        /// <summary>
        /// Immediately dispatches an event instance to all listeners of its type.
        /// </summary>
        public static void Raise<T>(T evt)
        {
            if (_events.TryGetValue(typeof(T), out var del))
                (del as Action<T>)?.Invoke(evt);
        }

        /// <summary>
        /// Clears every subscription—useful for unit tests or scene resets.
        /// </summary>
        public static void Clear() => _events.Clear();
    }
}
