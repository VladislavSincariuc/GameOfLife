using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameOfLife
{
    public class Input : GameComponent
    {
        public Input(Game game) : base(game)
        {

        }

        KeyboardState _oldState;
        public override void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();

            SpaceTrigger = false;
            ResetTrigger = false;

            if (currentState.IsKeyDown(Keys.Space) && _oldState.IsKeyDown(Keys.Space))
                SpaceTrigger = true;
            if (currentState.IsKeyDown(Keys.R) && _oldState.IsKeyDown(Keys.R))
                ResetTrigger = true;


            _oldState = currentState;

        }

        public bool SpaceTrigger { get; private set; }
        public bool ResetTrigger { get; private set; }

    }
}
