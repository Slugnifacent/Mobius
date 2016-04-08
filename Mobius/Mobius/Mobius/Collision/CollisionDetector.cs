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
    public class CollisionDetector
    {
        public CollisionDetector()
        {
            // TODO: Construct any child components here
        }

        public void detect(List<Object> item, List<Object> etem)
        {
            foreach (Object thing in item)
            {
                detect(thing, etem);
            }
        }

        public void detect(Object item, List<Object> etem)
        {
            foreach (Object thing in etem)
            {
                detect(item, thing);
            }
        }

        public void detect(Object item, Object etem) {
            if (item.BOUNDINGBOX.Intersects(etem.BOUNDINGBOX)) {
                item.collisionResolution(etem);
                etem.collisionResolution(item);
                item.updateBoundingBox();
                etem.updateBoundingBox();
            }
        }

    }
}
