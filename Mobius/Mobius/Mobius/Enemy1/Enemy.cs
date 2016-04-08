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
    public abstract class Enemy : Object
    {
        public float hp;
        public float speed;
        public float atk;
        public float def;
        public Vector2 orientation;
        public float fireCooldown;
        public float fireRate;
        public Vector2 aim;
        public List<Bullet> bulletList;
        protected Character player;
        protected float gravityFactor;
        protected static int size = 64;
        protected SoundEffect damage;


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
           // bulletList.Add(new Bullet(aim, position));
            aim = player.getPosition() - position;
            aim.Normalize();
            Bullet temp = Game1.retrieveBullet();
            temp.initialize(aim, position, bulletType.Enemy, atk);
            Game1.addBullet(temp);
        }

        public override void collisionResolution(Object item)
        {
            if (item.GetType() == typeof(Bullet))
            {
                Bullet temp = (Bullet)item;
                if (temp.bulletType == bulletType.Friendly)
                {
                    hp -= temp.dmg;
                    temp.Dead();
                }
            }

            if (item.GetType() == typeof(Platform))
            {
                Platform temp = (Platform)item;
                if (temp.PLATFORMTYPE == platformType.Floor)
                {
                    position.Y = item.position.Y - boundingBox.Height;
                }


                if (temp.PLATFORMTYPE == platformType.Block)
                {
                    if (boundingBox.Right > item.BOUNDINGBOX.Left && boundingBox.Right < item.BOUNDINGBOX.Left + 10.0f)
                    {
                        position.X = item.position.X - boundingBox.Width;

                    }
                    else if (boundingBox.Left < item.BOUNDINGBOX.Right && boundingBox.Left > item.BOUNDINGBOX.Right - 10.0f)
                    {
                        position.X = item.BOUNDINGBOX.Right;
                    }

                    else if (boundingBox.Bottom > item.BOUNDINGBOX.Top && boundingBox.Bottom < item.BOUNDINGBOX.Top + 10.0f)
                    {
                        position.Y = item.BOUNDINGBOX.Top - boundingBox.Height;

                    }
                }


                if (temp.PLATFORMTYPE == platformType.Platform)
                {
                    if (boundingBox.Right > item.BOUNDINGBOX.Left && boundingBox.Right < item.BOUNDINGBOX.Left + 10.0f)
                    {
                        position.X = item.position.X - boundingBox.Width;

                    }
                    else if (boundingBox.Left < item.BOUNDINGBOX.Right && boundingBox.Left > item.BOUNDINGBOX.Right - 10.0f)
                    {
                        position.X = item.BOUNDINGBOX.Right;
                    }

                    else if (boundingBox.Bottom > item.BOUNDINGBOX.Top && boundingBox.Bottom < item.BOUNDINGBOX.Top + 10.0f)
                    {
                        position.Y = item.BOUNDINGBOX.Top - boundingBox.Height;

                    }
                }
            }
        }

        public bool checkDeath()
        {
            return hp <= 0;
        }

        abstract public void Update(GameTime gameTime);

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, boundingBox, Color.White);
        }
    }
}
