using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Mobius {
    public class Bullet {
        public Vector2 position;
        public Texture2D texture;
        protected Rectangle boundingBox;
        public float speed;
        public Vector2 direction;

        public Bullet(Vector2 dir, Vector2 pos) {
            this.direction = dir;
            this.position = pos;
            this.speed = 10;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(16), (int)(16));
            texture = Game1.content.Load<Texture2D>(@"player_down_fast_1");
        }

        public void move() {
            position.X += (direction.X * speed);
            position.Y += (direction.Y * speed);
        }

        public void updateBoundingBox(Vector2 position) {
            boundingBox.X = (int)position.X - 15;
            boundingBox.Y = (int)position.Y - 15;
        }

        public void update(GameTime gameTime) {
            move();
            updateBoundingBox(this.position);
        }

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, boundingBox, Color.White);
        }
    }
}
