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
        //private float elapsedTime;

        int frames = 8;
        double timer = 0f;
        double interval = 100f;
        int currentFrame = 0;
        int spriteWidth = 64;
        int spriteHeight = 64;
        private Vector2 origin;
        Rectangle sourceRect;
        int hurtTimer;
        float size;

        public Patrol(Vector2 startPos, Character p)
        {
            damage = Game1.content.Load<SoundEffect>(@"Sounds/enemyDamage");
            position = startPos;
            /*atk = ;
            hp = ;
            def = ;
            fireRate = ;
            texture = ;*/
            orientation = new Vector2(-1.0f, 0.0f);
            goLeft = true;
            elapsedTime = 0.0f;
            texture = Game1.content.Load<Texture2D>(@"Enemy\spriteSheet-enemyPatrol");
            boundingBox = new Rectangle((int)position.X, (int)position.Y, Enemy.size, Enemy.size);
            bulletList = new List<Bullet>();
            speed = 200.0f * Game1.difficulty;
            patrolTime = 2.0f;
            player = p;
            fireRate = 1.0f / Game1.difficulty;
            hp = 10 * Game1.difficulty;
            atk = 5 * Game1.difficulty;
            gravityFactor = 1.0f;
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
            fireCooldown += timeInSec;
            elapsedTime += timeInSec;
            if (fireCooldown >= fireRate)
            {
                Fire();
                fireCooldown = 0.0f;
            }
            if (elapsedTime >= patrolTime)
            {
                orientation.X *= -1.0f;
                elapsedTime = 0.0f;
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
            position += orientation * speed * timeInSec;
            updateBoundingBox();
            gravityFactor = 1.0f;
            //if (timeInSec - elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds)
            //{
                //orientation = playerPosition - position;
                //orientation.Normalize();
                //position += orientation * speed;
           // }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (orientation.X < 0)
                spriteBatch.Draw(texture, new Vector2(position.X + 32, position.Y + 32), sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0);
            else if (orientation.X > 0)
                spriteBatch.Draw(texture, new Vector2(position.X + 32, position.Y + 32), sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.FlipHorizontally, 0);  
            //spriteBatch.Draw(texture, new Vector2(position.X + 32, position.Y + 32), sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0);
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
                        gravityFactor = 0.0f;
                    }
                }
            }
        }
        
    }
}
