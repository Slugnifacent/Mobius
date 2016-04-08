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
        int frames = 6;
        double timer = 0f;
        double interval = 100f;
        int currentFrame = 0;
        int spriteWidth = 64;
        int spriteHeight = 64;
        private Vector2 origin;
        Rectangle sourceRect;
        private Vector2 startPosition;
        private float range;
        int hurtTimer;
        float size;

        public Pattern(Vector2 startPos, Character p)
        {
            damage = Game1.content.Load<SoundEffect>(@"Sounds/enemyDamage");
            position = startPos;
            startPosition = startPos;
           /* atk = ;
            hp = ;
            def = ;
            fireRate = ;
            texture = ;
            * */
            speed = 100.0f * Game1.difficulty;
            orientation = new Vector2(0.0f, 1.0f);
            texture = Game1.content.Load<Texture2D>(@"Enemy\spriteSheet-enemyPattern");
            boundingBox = new Rectangle((int)position.X, (int)position.Y, Enemy.size, Enemy.size);
            bulletList = new List<Bullet>();
            player = p;
            fireCooldown = 0.0f;
            fireRate = 3.0f / Game1.difficulty;
            atk = 5 * Game1.difficulty;
            hp = 10 * Game1.difficulty;
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
            if (fireCooldown >= fireRate)
            {
                Fire();
                fireCooldown = timeInSec;
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

            position += orientation * speed * timeInSec;
            if (position.Y >= startPosition.Y || position.Y <= 150)
                orientation *= -1.0f;
            updateBoundingBox();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, new Vector2(position.X + 32, position.Y + 32), sourceRect, Color.White, 0.0f, origin, size, SpriteEffects.None, 0);
        }

        public override void collisionResolution(Object item)
        {
            if (item.GetType() == typeof(Bullet))
            {
                Bullet temp = (Bullet)item;
                if (temp.bulletType == bulletType.Friendly)
                {
                    size = .5f;
                    hp -= temp.dmg;
                    damage.Play();
                    temp.Dead();
                }
            }
        }
    }
}
