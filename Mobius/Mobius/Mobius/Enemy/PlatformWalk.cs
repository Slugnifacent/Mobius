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
    class Patrol : Enemy
    {
        private bool goLeft;
        private float patrolTime;
        private float elapsedTime;

        public Patrol(Vector2 startPos)
        {
            position = startPos;
            atk = ;
            hp = ;
            def = ;
            fireRate = ;
            texture = ;
            orientation = new Vector2(-1.0f, 0.0f);
            goLeft = true;
        }

        public override void Update(GameTime gameTime)
        {
            float timeInSec = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeInSec - lastShot >= fireRate)
            {
                Fire();
                lastShot = timeInSec;
            }
            orientation = playerPosition - position;
            orientation.Normalize();
            position += orientation * speed;
        }
    }
}
