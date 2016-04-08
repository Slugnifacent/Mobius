using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mobius
{
    abstract public class Object
    {
        public Vector2 position;
        protected Rectangle boundingBox;
        protected Texture2D texture;
        protected float elapsedTime;
        public bool stupidFlag;
        public Rectangle BOUNDINGBOX {
            get { return boundingBox;}
        }

        virtual public void updateBoundingBox() {
            boundingBox.X = (int)position.X;
            boundingBox.Y = (int)position.Y;
        }

        abstract public void collisionResolution(Object item);

        virtual public void update(GameTime time) {}

        virtual public void Draw(SpriteBatch batch) {
            batch.Draw(texture, boundingBox, Color.White);
        }


        // Generic Screen Side Testing
        public bool rightSideScreen(Camera c)
        {
            return position.X > (int)c.topRight().X;
        }

        public bool leftSideScreen(Camera c)
        {
            return leftEnd() < (int)c.topLeft().X;
        }

        public bool onScreen(Camera c)
        {
            if (leftSideScreen(c) || rightSideScreen(c)) return false;
            return true;
        }

        public bool leftSideScreen(Camera c, float rightSide)
        {
            return (rightSide < (int)c.topLeft().X);
        }


        public bool rightSideScreen(Camera c, float leftSide)
        {
            return (leftSide > (int)c.topRight().X);
        }



        public float leftEnd()
        {
            return position.X + texture.Width;
        }
    }
}
