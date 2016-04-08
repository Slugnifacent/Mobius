using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Mobius
{
    class Pattern : Enemy
    {
        private bool goUp;

        public Pattern(Vector2 startPos)
        {
            position = startPos;
            atk = ;
            hp = ;
            def = ;
            fireRate = ;
            texture = ;
            orientation = new Vector2(0.0f, 1.0f);
        }

        public override void Update(GameTime gameTime)
        {
            float timeInSec = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeInSec - lastShot >= fireRate)
            {
                Fire(new Vector2();
                lastShot = timeInSec;
            }
            position += orientation * speed;
            if (position.Y > 5.0f || position.Y < -5.0f)
                orientation *= -1.0f;
        }
    }
}
