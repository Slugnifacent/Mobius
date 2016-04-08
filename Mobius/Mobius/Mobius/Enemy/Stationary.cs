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
    class Stationary : Enemy
    {
        public Stationary(Vector2 startPos, float difficulty)
        {
            position = startPos;
            atk = ;
            hp = ;
            def = ;
            fireRate = ;
            texture = ;
            aim = new Vector2(-1.0f, 0.0f);
        }

        public override void Fire()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            float timeInSec = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeInSec - lastShot >= fireRate)
            {
                Fire();
                lastShot = timeInSec;
            }
        }
    }
}
