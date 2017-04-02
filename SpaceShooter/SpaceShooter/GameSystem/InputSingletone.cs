using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter.GameSystem
{
    public class InputSingletone
    {
        private static InputSingletone _instance;
        public static InputSingletone Instance => _instance ?? (_instance = new InputSingletone());

        public InputSingletone()
        {
            
        }

        public bool IsKeyPress(Keys key)
        {
            return KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key);
        }

        public MouseState MouseState;
        public MouseState LastMouseState;

        public KeyboardState KeyboardState;
        public KeyboardState LastKeyboardState;

        public GamePadState GamePadState;
        public GamePadState LastGamePadState;

        public void Update(GameTime gameTime)
        {
            LastMouseState = MouseState;
            MouseState = Mouse.GetState();

            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            LastGamePadState = GamePadState;
            GamePadState = GamePad.GetState(0);
        }
    }
}
