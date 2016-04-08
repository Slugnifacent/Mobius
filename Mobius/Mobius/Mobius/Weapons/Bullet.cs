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
    public enum bulletType {Friendly, Enemy , Dead}

    public class Bullet : Object {
        public float speed;
        public Vector2 direction;
        bulletType type;
        public float dmg;
        int currentFrame;
        Rectangle textureRectangle;
        int animationTimer;

        public Bullet()
        {
            initialize(new Vector2(0, 0), new Vector2(0, 0), bulletType.Enemy, 5);
        }

        public Bullet(Vector2 dir, Vector2 pos, bulletType Type)
        {
            this.direction = dir;
            this.position = pos;
            this.speed = 10;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(16), (int)(16));
            texture = Game1.content.Load<Texture2D>(@"Bullet");
            type = Type;
        }

        public void initialize(Vector2 dir, Vector2 pos, bulletType Type, float dmg)
        {
            textureRectangle = new Rectangle(0,0,65,32);
            this.direction = dir;
            this.position = pos;
            this.speed = 10;
            this.dmg = dmg;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(16), (int)(16));
            texture = Game1.content.Load<Texture2D>(@"Bullet");
            type = Type;
            currentFrame = 0;
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
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer >= 50){
                animationTimer = 0;
                currentFrame = (currentFrame + 1 < 3) ? currentFrame + 1 : 0; 
            }
            
            
            textureRectangle.X = currentFrame * 64;
        }

        public bulletType bulletType{
            get { return type; }
        }

        public void draw(SpriteBatch spriteBatch) {
            if (direction.X > 0) spriteBatch.Draw(texture, position, textureRectangle, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            else spriteBatch.Draw(texture, position,textureRectangle, Color.White,0,new Vector2(0,0),1,SpriteEffects.FlipHorizontally,0);
        }
        public override void collisionResolution(Object item)
        {

        }

        public void Dead()
        {
            type = bulletType.Dead;
        }

    }
}
