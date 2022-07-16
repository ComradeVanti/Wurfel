using System;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public static class UnityEventExt
    {

        public static void ListenOnce<T>(this UnityEvent<T> unityEvent, Action<T> listener)
        {
            void Wrapper(T value)
            {
                unityEvent.RemoveListener(Wrapper);
                listener(value);
            }

            unityEvent.AddListener(Wrapper);
        }

        public static void ListenOnce(this UnityEvent unityEvent, Action listener)
        {
            void Wrapper()
            {
                unityEvent.RemoveListener(Wrapper);
                listener();
            }

            unityEvent.AddListener(Wrapper);
        }

    }

}