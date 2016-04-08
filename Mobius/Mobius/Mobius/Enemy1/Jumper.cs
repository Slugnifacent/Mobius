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
    class Jumper : Enemy
    {
        private float jumpTimer;
        private bool canJump;

        int frames = 9;
        double timer = 0f;
        double interval = 200f;
        int currentFrame = 0;
        int spriteWidth = 64;
        int spriteHeight = 64;
        private Vector2 origin;
        Rectangle sourceRect;
        int hurtTimer;
        float size;

        public Jumper(Vector2 startPos, Character p)
        {
            damage = Game1.content.Load<SoundEffect>(@"Sounds/enemyDamage");  
            position = startPos;
            hp = 10 * Game1.difficulty;
          /*  atk = ;
            hp = ;
            def = ;
            fireRate = ;
            texture = ;*/
            //aim = new Vector2(-1.0f, 0.0f);
            jumpTimer = 1.0f / Game1.difficulty;
            texture = Game1.content.Load<Texture2D>(@"Enemy\spriteSheet-enemyJumper");
            boundingBox = new Rectangle((int)position.X, (int)position.Y, Enemy.size, Enemy.size);
            elapsedTime = 0.0f;
            gravityFactor = 1.0f;
            orientation = Vector2.Zero;
            bulletList = new List<Bullet>();
            player = p;
            speed = 100 * Game1.difficulty;
            canJump = false;
            atk = 5 * Game1.difficulty;
            //fireCooldown = 0.0f;
            //fireRate = 2.0f;
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
        }

        public override void Update(GameTime gameTime)
        {
            if (hurtTimer > 150)
            {
                size = 1;
                hurtTimer = 0;
            }
            hurtTimer += (int)gameTime.ElapsedGameTime.Milliseconds;
            float timeInSec = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += timeInSec;
            /*fireCooldown += timeInSec;
            if (fireCooldown >= fireRate)
            {
                Fire();
                fireCooldown = 0.0f;
            }*/

            if (elapsedTime >= jumpTimer)
            {
                if (canJump)
                {
                    Jump();
                    elapsedTime = 0.0f;
                    canJump = false;
                    gravityFactor = 1.0f;
                }
            }

            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer >= interval) {
                currentFrame++;
                timer = 0f;
            }
            if (currentFrame >= frames) {
                currentFrame = 0;
            }

            sourceRect.X = currentFrame * spriteWidth;
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
            orientation += Game1.gravity * gravityFactor * timeInSec;
            /*if (position.Y >= 600.0f && orientation.Y > 0.0f)
                orientation.Y = 0.0f;*/
            position += (orientation * speed * timeInSec);
            updateBoundingBox();
            /*foreach (Bullet b in bulletList)
                b.update(gameTime);*/
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, new Vector2(position.X + 32, position.Y + 32), sourceRect, Color.White, 0.0f, origin, size, SpriteEffects.None, 0);
        }

        private void Jump()
        {
            orientation += new Vector2(0.0f, -2.0f);
        }
        
        public override void collisionResolution(Object item)
        {
            if (item.GetType() == typeof(Bullet))
            {
                Bullet temp = (Bullet)item;
                if (temp.bulletType == bulletType.Friendly)
                {
                    hp -= temp.dmg;
                    size = .5f;
                    damage.Play();
                    temp.Dead();
                }
            }

            if (item.GetType() == typeof(Platform))
            {
                Platform temp = (Platform)item;
                if (temp.PLATFORMTYPE == platformType.Floor)
                {
                    position.Y = item.position.Y - boundingBox.Height;
                    canJump = true;
                    gravityFactor = 0.0f;
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
                        canJump = true;
                        gravityFactor = 0.0f;
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
                        canJump = true;
                        gravityFactor = 0.0f;
                    }
                }
            }
        }
    }
}
