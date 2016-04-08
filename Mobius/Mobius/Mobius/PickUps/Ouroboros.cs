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

namespace Mobius {
    public class Ouroboros : Object {

        int frames = 8;
        double timer = 0f;
        double interval = 200f;
        int currentFrame = 0;
        int spriteWidth = 32;
        int spriteHeight = 32;
        private Vector2 origin;
        Rectangle sourceRect;

        public Ouroboros(Vector2 pos) {
            position = pos;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            texture = Game1.content.Load<Texture2D>(@"Pickups\spriteSheet-ouroboros");
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
        }

        public void updateBoundingBox(Vector2 position) {
            boundingBox.X = (int)position.X - 31;
            boundingBox.Y = (int)position.Y - 31;
        }

        public override void update(GameTime gameTime) {
            //updateBoundingBox(this.position);
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
        }

        public override void Draw(SpriteBatch spriteBatch) {
             spriteBatch.Draw(texture, new Vector2(position.X, position.Y), sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0);
        }

        public override void collisionResolution(Object item) {
            if (item.GetType() == typeof(Character)) {
                position = new Vector2(-1000, 0);
            }
        }
    }
}
