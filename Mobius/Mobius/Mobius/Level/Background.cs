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
    public class Picture
    {
        public SoundEffect sound;
        SoundEffectInstance instance;
        public Texture2D picture;
        public Texture2D picture2;
        public Vector2 position;
        public bool multiTexture;
        public bool lastTexture;
        public bool club;
        public bool street;
        

        public int leftEnd()
        {
            return (int)(position.X);
        }
        public int rightEnd()
        {
            return (int)(position.X + picture.Width);
        }
        public string name;
        public void createInstance() {
            instance = sound.CreateInstance();
            STOP();
            instance.Volume = .4f;
        }
        public void PLAY() {
            if (instance.State != SoundState.Playing) {
                if (club)
                {
                    instance.Volume = 1f;
                }
                if (street) instance.Volume = .2f;
                instance.Play();
            }
        }
        public void STOP()
        {
                instance.Stop();
        }
    }
    public class Background : Object
    {
        int interval = 2;
        Effect stoplightEffect;
        float stoplightTimer = 0;
        float stoplightFlash;
        List<Picture> backgrounds;
        RenderTarget2D renderTarget2D;
        Random random = new Random();

        Texture2D rainBumpMap;
        Texture2D Sky1;
        Texture2D Sky2;
        Texture2D Sky3;
        Texture2D Sky4;
        Texture2D Sky5;
        float rainDistance = 0;
        Vector3[] raveColors = new Vector3[12] { 
            new Vector3(1f,     0f,     0f),
            new Vector3(1f,     0.5f,   0f),
            new Vector3(1f,     1f,     0f),
            new Vector3(0.5f,   1f,     0f),
            new Vector3(0f,     1f,     0f),
            new Vector3(0f,     1f,     0.5f),
            new Vector3(0f,     1f,     1f),
            new Vector3(0f,     0.5f,   1f),
            new Vector3(0f,     0f,     1f),
            new Vector3(0.5f,   0f,     1f),
            new Vector3(1f,     0f,     1f),
            new Vector3(1f,     0f,     0.5f),};

        Vector4[] ravePositions = new Vector4[2] {
            new Vector4(0f,     0f,     0f,     0f),
            new Vector4(0f,     0f,     10f,     0f),};

        public Background(GraphicsDeviceManager graphics)
        {
            backgrounds = new List<Picture>();
            for (int index = 0; index < 5; index++) {
                backgrounds.Add(new Picture());
            }

            backgrounds[0].picture = Game1.content.Load<Texture2D>(@"Level\Youth\YouthScape");
            backgrounds[1].multiTexture = true;
            backgrounds[1].club = true;
            backgrounds[1].picture = Game1.content.Load<Texture2D>(@"Level\YoungAdult\YoungAdultScape(Background)");
            backgrounds[1].picture2 = Game1.content.Load<Texture2D>(@"Level\YoungAdult\YoungAdultScape(Foreground)");
            backgrounds[2].street = true;
            backgrounds[2].picture = Game1.content.Load<Texture2D>(@"Level\Adult\AdultScape");
            backgrounds[3].picture = Game1.content.Load<Texture2D>(@"Level\Elder\ElderScape");
            backgrounds[4].multiTexture = true;
            backgrounds[4].lastTexture = true;
            backgrounds[4].picture = Game1.content.Load<Texture2D>(@"Level\Rebirth\Rebirth(Background)");
            backgrounds[4].picture2 = Game1.content.Load<Texture2D>(@"Level\Rebirth\Rebirth(Foreground)");

            backgrounds[0].position =  new Vector2(0,0);
            backgrounds[1].position =  new Vector2(backgrounds[0].rightEnd() ,0);
            backgrounds[2].position =  new Vector2(backgrounds[1].rightEnd(), 0);
            backgrounds[3].position =  new Vector2(backgrounds[2].rightEnd(), 0);
            backgrounds[4].position =  new Vector2(backgrounds[3].rightEnd(), 0);

            backgrounds[0].name = "Youth";
            backgrounds[1].name = "YoungAdult";
            backgrounds[2].name = "Adult";
            backgrounds[3].name = "Elder";
            backgrounds[4].name = "Rebirth";

            stoplightEffect = Game1.content.Load<Effect>("Shaders/stoplight");

            rainBumpMap = Game1.content.Load<Texture2D>("Shaders/RainBumpMap");
            Sky1 = Game1.content.Load<Texture2D>("Sky2/Sky1");
            Sky2 = Game1.content.Load<Texture2D>("Sky2/Sky2");
            Sky3 = Game1.content.Load<Texture2D>("Sky2/Sky3");
            Sky4 = Game1.content.Load<Texture2D>("Sky2/Sky4");
            Sky5 = Game1.content.Load<Texture2D>("Sky2/Sky5");

            renderTarget2D = new RenderTarget2D(
                graphics.GraphicsDevice,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            backgrounds[0].sound = Game1.content.Load<SoundEffect>(@"Sounds\playground");
            backgrounds[1].sound = Game1.content.Load<SoundEffect>(@"Sounds\clubScene");
            backgrounds[2].sound = Game1.content.Load<SoundEffect>(@"Sounds\cityScape");
            backgrounds[3].sound = Game1.content.Load<SoundEffect>(@"Sounds\countrySide");
            backgrounds[4].sound = Game1.content.Load<SoundEffect>(@"Sounds\ekgMonitor");

            backgrounds[0].createInstance();
            backgrounds[1].createInstance();
            backgrounds[2].createInstance();
            backgrounds[3].createInstance();
            backgrounds[4].createInstance();
        }

        public void draw(SpriteBatch batch, Camera camera, GraphicsDeviceManager graphics, Effect shaderEffect)
        {
            for (int index = 0; index < backgrounds.Count(); index++)
            {
                if (backgrounds[index].name != "Rebirth")
                {
                    if (backgrounds[index].name == "Youth")
                    {
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["Sunset"];
                        shaderEffect.Parameters["sunPosition"].SetValue(new Vector2(0.4f, 0.5f));
                        batch.Draw(Sky1, new Rectangle((int)backgrounds[index].position.X, (int)backgrounds[index].position.Y - 10, 2049, 800), Color.CornflowerBlue);
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];
                    }
                    if (backgrounds[index].name == "YoungAdult")
                    {
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];
                        batch.Draw(Sky2, new Rectangle((int)backgrounds[index].position.X, (int)backgrounds[index].position.Y - 10, 2050, 800), Color.CornflowerBlue);
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["Rave"];
                        shaderEffect.Parameters["index2"].SetValue(0);
                        shaderEffect.Parameters["ravePositions"].SetValue(ravePositions);
                        shaderEffect.Parameters["raveColors"].SetValue(raveColors);
                    }
                    if (backgrounds[index].name == "Adult")
                    {
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];
                        batch.Draw(Sky3, new Rectangle((int)backgrounds[index].position.X, (int)backgrounds[index].position.Y - 10, 2049, 800), Color.CornflowerBlue);
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.Parameters["timer"].SetValue(stoplightTimer);
                        shaderEffect.Parameters["flash"].SetValue(stoplightFlash);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["Stoplight"];
                        shaderEffect.Parameters["rainDistance"].SetValue(rainDistance);
                    }
                    if (backgrounds[index].name == "Elder")
                    {
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];
                        batch.Draw(Sky4, new Rectangle((int)backgrounds[index].position.X, (int)backgrounds[index].position.Y - 10, 2049, 800), Color.CornflowerBlue);
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        graphics.GraphicsDevice.Textures[1] = rainBumpMap;
                        shaderEffect.Parameters["rainDistance"].SetValue(rainDistance);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["Rain"];
                    }
                    if (backgrounds[index].name == "Rebirth")
                    {
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];
                        batch.Draw(Sky5, new Rectangle((int)backgrounds[index].position.X, (int)backgrounds[index].position.Y - 10, 2049, 800), Color.CornflowerBlue);
                    }

                    batch.Draw(backgrounds[index].picture, backgrounds[index].position, Color.White);
                    if (leftSideScreen(Game1.camera, backgrounds[index].rightEnd()) || rightSideScreen(Game1.camera, backgrounds[index].leftEnd()))
                    {
                        backgrounds[index].STOP();
                    }
                    else
                    {
                        backgrounds[index].PLAY();
                    }

                    if (backgrounds[index].multiTexture)
                    {
                        if (backgrounds[index].lastTexture)
                        {
                            if (Game1.player.position.X >= backgrounds[index].rightEnd() - 340)
                            {
                                Game1.levelCompleted = true;
                                Game1.upDiff++;
                            }
                            else
                            {
                                Game1.levelCompleted = false;
                                //Game1.upDiff = 0;
                            }
                        }


                        if (backgrounds[index].lastTexture)
                        {
                            if (Game1.player.position.X >= backgrounds[index].rightEnd() - 340)
                            {

                                Game1.levelCompleted = true;
                                Game1.upDiff++;
                            }
                        }

                        if (backgrounds[index].name != "YoungAdult")
                        {
                            batch.End();
                            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                            shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];
                        }
                        
                        batch.Draw(backgrounds[index].picture2, backgrounds[index].position, Color.White);

                    
                    }
                }
            }

            for (int index = 0; index < backgrounds.Count(); index++) {
                if (backgrounds[index].name == "Rebirth") {
                    if (backgrounds[index].name == "Rebirth") {
                        batch.End();
                        batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
                        shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];
                        batch.Draw(Sky5, new Rectangle((int)backgrounds[index].position.X, (int)backgrounds[index].position.Y - 10, 2049, 800), Color.CornflowerBlue);
                    }

                    batch.Draw(backgrounds[index].picture, backgrounds[index].position, Color.White);

                    if (leftSideScreen(Game1.camera, backgrounds[index].rightEnd()) || rightSideScreen(Game1.camera, backgrounds[index].leftEnd())) {
                        backgrounds[index].STOP();
                    }
                    else {
                        backgrounds[index].PLAY();
                    }

                    if (backgrounds[index].multiTexture) {
                        if (backgrounds[index].lastTexture) {
                            if (Game1.player.position.X >= backgrounds[index].rightEnd() - 340) {
                                Game1.levelCompleted = true;
                                Game1.upDiff++;
                            }
                            else {
                                Game1.levelCompleted = false;
                                //Game1.upDiff = 0;
                            }
                            Game1.player.Draw(batch);
                        }
                    }
                    batch.Draw(backgrounds[index].picture2, backgrounds[index].position, null, Color.White,0, Vector2.Zero,1,SpriteEffects.None,0);
                }
            }
        }
        
        public void update()
        {
            stoplightTimer++;
            stoplightFlash++;
            if (stoplightTimer > 900)
            {
                stoplightTimer = 0;
                stoplightFlash = 0;
            }

            rainDistance += 0.02f;
            if (rainDistance > 1) rainDistance = 0;

            for (int i = 0; i < ravePositions.Length; i++)
            {
                if (ravePositions[i].Z < 0)
                {
                    ravePositions[i] = new Vector4((float)random.Next(3, 7) / 10, (float)random.Next(5, 9) / 10, 30f, random.Next(0, 11));
                }
                ravePositions[i].Z -= 1f;
            }

            if (leftSideScreen(Game1.camera, backgrounds[0].rightEnd()))
            {
                Picture temp = backgrounds[0];
                backgrounds.RemoveAt(0);
                backgrounds.Add(temp);
                backgrounds[backgrounds.Count() - 1].position.X = backgrounds[backgrounds.Count() - 2].rightEnd();
                switch (backgrounds[0].name) { 
                    case "Youth":
                        Game1.player.age = 0;
                        break;
                    case "YoungAdult":
                        Game1.player.age = 1;
                        break;
                    case "Adult":
                        Game1.player.age = 2;
                        break;
                    case "Elder":
                        Game1.player.age = 3;
                        break;
                    case "Rebirth":
                        Game1.player.age = 3;
                        break;
                }
                
                if (temp.lastTexture)
                {
                    Game1.upDiff = 0;
                }
            }
            for (int index = 0; index < backgrounds.Count(); index++)
            {
                backgrounds[index].position.X += (int)(Game1.camera.scrollValue * .65f);
            }
        }
        public override void collisionResolution(Object item)
        {
            throw new NotImplementedException();
        }
        public void moveBackground(Vector2 Movement)
        {
            foreach (Picture background in backgrounds)
            {
                background.position += Movement;
            }
        }
        public void resetMusic() {
            foreach (Picture picture in backgrounds) {
                picture.STOP();
            }
        }
    }

}


