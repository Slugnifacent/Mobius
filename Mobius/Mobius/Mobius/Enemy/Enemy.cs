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
    public abstract class Enemy
    {
        public float hp;
        public float speed;
        public float atk;
        public float def;
        public Vector2 position;
        public Vector2 orientation;
        public Texture2D texture;
        public Rectangle boundingBox;
        public float lastShot;
        public float fireRate;
        public Vector2 aim;

        public void updateBoundingBox(Vector2 position)
        {
            boundingBox.X = (int)position.X - 15;
            boundingBox.Y = (int)position.Y - 15;
        }

        public Vector2 getCurrPos()
        {
            return position;
        }

        public Rectangle getBox()
        {
            return boundingBox;
        }

        virtual public void Fire()
        {
            bulletList.Add(new Bullet(aim, position));
        }

        virtual public void CollisionResolve()
        {
        }

        abstract public void Update(GameTime gameTime);

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }
    }
}
