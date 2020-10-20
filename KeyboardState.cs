using OpenTK;
using OpenTK.Input;
using System.Collections.Generic;


namespace CG_Projekt
{
    class KeyboardState
    {
        private HashSet<Key> _pressedKeys = new HashSet<Key>();

        public KeyboardState(INativeWindow window)
        {
            window.KeyDown += (sender, e) => _pressedKeys.Add(e.Key);
            window.KeyUp += (sender, e) => _pressedKeys.Remove(e.Key);
        }

        public bool IsKeyPressed(Key key)
        {
            return _pressedKeys.Contains(key);
        }
    }
}
