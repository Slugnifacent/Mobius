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


namespace Mobius
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
    public enum platformType { Floor, Block, Platform };


    public class Platform : Object
    {
        platformType type;
        /// <summary>
        /// Constructor for the Platform Class
        /// </summary>
        /// /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public  Platform(platformType Type,Vector2 Position)
        {
            position = Position;
            switch (Type) { 
                case platformType.Block:
                    texture = Game1.content.Load<Texture2D>(@"Level\Block");
                    break;
                case platformType.Floor:
                    texture = Game1.content.Load<Texture2D>(@"Level\Floor");
                    break;
                case platformType.Platform:
                    texture = Game1.content.Load<Texture2D>(@"Level\Platform");
                    break;
                default:
                    System.Console.WriteLine("Platform texture Not loaded Properly");
                    break;
            }
            type = Type;
            boundingBox = new Rectangle((int)position.X,(int)position.Y,texture.Width,texture.Height);
            updateBoundingBox();
        }


        public void Draw(SpriteBatch batch) {
            batch.Draw(texture, position, null, Color.White);
        }
        public override void collisionResolution(Object item)
        {
           
        }

        public platformType PLATFORMTYPE {
            get
            {
                return type;            
            }
        }
    }
}
