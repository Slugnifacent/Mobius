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
    class SingleShot : BulletPattern
    {
        public override void Fire(Vector2 position, Vector2 direction, bulletType type, float dmg)
        {
            if (direction.X > 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    Bullet temp = Game1.retrieveBullet();
                    temp.initialize(direction, new Vector2(position.X + 96, position.Y + 64), bulletType.Friendly, dmg);
                    Game1.addBullet(temp);
                }
            }
            else if (direction.X < 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    Bullet temp = Game1.retrieveBullet();
                    temp.initialize(direction, new Vector2(position.X, position.Y + 64), bulletType.Friendly, dmg);
                    Game1.addBullet(temp);
                }
            }
        }
    }
}
