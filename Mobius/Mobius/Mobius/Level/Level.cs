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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
    public enum levelComponent { Block, Floor, Platform, Enemies };
    public class Level {
        List<Object> blocks;
        List<Object> platforms;
        List<Object> floors;
        List<Object> enemies;
        public List<Object> collectibles;
        public List<Object> ouros;
        public Camera camera;
        public bool ouroHasAppeared;
        public bool itemHasAppeared;

        int blockCount = 10;
        int platformCount = 20;
        int floorCount = 80;
        int healthCount = 2;
        int bottomBlock = 128;
        Random random;
        public Background background;



        public Level(Camera c, GraphicsDeviceManager graphics) {
            random = new Random();
            blocks = new List<Object>();
            platforms = new List<Object>();
            floors = new List<Object>();
            collectibles = new List<Object>();
            ouros = new List<Object>();
            // addBlocks(blockCount);
            addFloors(floorCount);
            enemies = new List<Object>();
            //addHealthPickUp(2);
            // addPlatforms(platformCount);
            camera = c;
            background = new Background(graphics);
        }

        public void ResetLevel() {
            blocks.Clear();
            platforms.Clear();
            floors.Clear();
            collectibles.Clear();
            addFloors(floorCount);
            enemies.Clear();
            background.resetMusic();
        }

        void addFloors(int amount) {
            /*
            for (int index = 0; index < amount; index++ )
            {
                floors.Add(new Platform(platformType.Floor, new Vector2(index * 128, 736 - bottomBlock )));
            }
            */
            floors.Add(new Platform(platformType.Floor, new Vector2(0, 768)));
            floors.Add(new Platform(platformType.Floor, new Vector2(1024, 768)));
            floors.Add(new Platform(platformType.Floor, new Vector2(2 * 1024, 768)));
        }

        void addBlocks(int amount) {
            int randomPosition = 0;
            for (int index = 0; index < amount; index++) {
                randomPosition += random.Next(2, 20);
                blocks.Add(new Platform(platformType.Block, new Vector2(randomPosition * 128, 640 - bottomBlock)));
            }
        }

        void addPlatforms(int amount) {
            int randomPosition = 0;
            for (int index = 0; index < amount; index++) {
                randomPosition += random.Next(2, 20);
                platforms.Add(new Platform(platformType.Platform, new Vector2(randomPosition * 128, 544 - bottomBlock)));
            }
        }

        public void Update(GameTime time) {
            if (floors[0].leftSideScreen(camera)) {
                // Regenerate Level
                floors.RemoveAt(0);
                generateNextSegment(time);
            }

            foreach (Ouroboros o in ouros)
                o.update(time);

            for (int index = 0; index < blocks.Count(); index++) {
                if (blocks[0].leftSideScreen(camera)) {
                    blocks.RemoveAt(0);
                    index--;
                }
                else break;

            }

            for (int index = 0; index < platforms.Count(); index++) {
                if (platforms[0].leftSideScreen(camera)) {
                    platforms.RemoveAt(0);
                    index--;
                }
                else break;
            }

            for (int index = 0; index < collectibles.Count(); index++) {
                if (collectibles[index].leftSideScreen(camera) || collectibles[index].stupidFlag) {
                    collectibles.RemoveAt(index);
                    index--;
                }
                //else break;
            }
            if (collectibles.Count < 3 && collectibles.Count > 0) {
                foreach (Object collect in collectibles) {
                    collect.stupidFlag = true;
                    collect.position = new Vector2(-1000, 0);
                }
            }

            foreach (Object collect in collectibles) {
                collect.update(time);
            }
            foreach (Object ouro in ouros) {
                ouro.update(time);
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
            //    foreach (Platform item in blocks)
            //    {
            //        item.position.X -= 15;
            //    }
            //    foreach (Platform item in platforms)
            //    {
            //        item.position.X -= 15;
            //    }
            //    foreach (Platform item in floors)
            //    {
            //        item.position.X -= 15;
            //    }
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.Right))
            //{
            //    foreach (Platform item in blocks)
            //    {
            //        item.position.X += 15;
            //    }
            //    foreach (Platform item in platforms)
            //    {
            //        item.position.X += 15;
            //    }
            //    foreach (Platform item in floors)
            //    {
            //        item.position.X += 15;
            //    }
            //}
            background.update();

        }

        void addHealthPickUp(int amount) {
            //int randomPosition = 0;
            //for (int index = 0; index < amount; index++) {
            //    randomPosition += random.Next(2, 20);
            //    collectibles.Add(new HealthPickUp(new Vector2(randomPosition * 128, 544 - bottomBlock)));
            //}
        }

        private void generateNextSegment(GameTime time) {
            int segment = 128;
            int count = 0;

            floors.Add(new Platform(platformType.Floor, new Vector2(floors[1].leftEnd(), floors[1].position.Y)));

            while (count < 8) {
                if ((!itemHasAppeared || !ouroHasAppeared) && time.ElapsedGameTime.TotalSeconds > (140 * Game1.difficulty)) {
                    if (!itemHasAppeared) {
                        float position = floors[2].position.X + count * segment + 512;
                        if (position <= floors[2].leftEnd()) {
                            generatePickups(count, segment, 0);
                            count += 6;
                        }
                    }
                    if (!ouroHasAppeared) {
                        float posit = floors[2].position.X + count * segment + 512;
                        if (posit <= floors[2].leftEnd()) {
                            generatePickups(count, segment, 1);
                            count += 3;
                        }
                    }
                }
                else {
                    switch (random.Next(0, 5)) {
                        case 0:// Space
                            if (random.Next(0, 2) == 0) generateEnemy(count, segment);
                            count++;
                            break;

                        case 1:// Block
                            float pos = floors[2].position.X + count * segment + 128;
                            if (pos <= floors[2].leftEnd()) {
                                if (random.Next(0, 3) == 0) generateEnemy(count, segment);
                                blocks.Add(new Platform(platformType.Block, new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 128)));
                                count++;
                            }
                            break;
                        case 2:// Platform
                            float posi = floors[2].position.X + count * segment + 512;
                            switch (random.Next(0, 2)) {
                                case 0:
                                    if (posi <= floors[2].leftEnd()) {
                                        if (random.Next(0, 2) == 0) generateEnemy(count, segment);
                                        platforms.Add(new Platform(platformType.Platform, new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 196)));
                                    }
                                    count += 4;
                                    break;
                                case 1:
                                    if (posi <= floors[2].leftEnd()) {
                                        if (random.Next(0, 2) == 0) generateEnemy(count, segment);
                                        platforms.Add(new Platform(platformType.Platform, new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 196)));
                                    }
                                    count += 4;
                                    if (posi <= floors[2].leftEnd()) {
                                        if (random.Next(0, 2) == 0) generateEnemy(count, segment);
                                        platforms.Add(new Platform(platformType.Platform, new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 392)));
                                    }
                                    count += 4;
                                    break;
                            }
                            break;
                        case 3:// Items
                            if (!itemHasAppeared) {
                                float position = floors[2].position.X + count * segment + 512;
                                if (position <= floors[2].leftEnd()) {
                                    generatePickups(count, segment, 0);
                                    count += 4;
                                }
                            }
                            break;
                        case 4:// Ouroboros
                            if (!ouroHasAppeared) {
                                float posit = floors[2].position.X + count * segment + 512;
                                if (posit <= floors[2].leftEnd()) {
                                    generatePickups(count, segment, 1);
                                    count += 3;
                                }
                            }
                            break;
                    }
                }
                count++;
            }

        }

        private void generateEnemy(int count, int segment) {
            switch (random.Next(0, 4)) {
                case 0:// Jump
                    Jumper jump = new Jumper(new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 128), Game1.player);
                    jump.position.Y -= jump.BOUNDINGBOX.Height + 5;
                    jump.updateBoundingBox();
                    enemies.Add(jump);
                    break;

                case 1:// Stationary
                    Stationary stationary = new Stationary(new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 420), Game1.player);
                    stationary.position.Y -= stationary.BOUNDINGBOX.Height + 5;
                    stationary.updateBoundingBox();
                    enemies.Add(stationary);
                    break;

                case 2:// Pattern
                    Pattern pattern = new Pattern(new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 128), Game1.player);
                    pattern.position.Y -= pattern.BOUNDINGBOX.Height + 5;
                    pattern.updateBoundingBox();
                    enemies.Add(pattern);
                    break;

                case 3:// Patrol
                    Patrol patrol = new Patrol(new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 128), Game1.player);
                    patrol.position.Y -= patrol.BOUNDINGBOX.Height + 5;
                    patrol.updateBoundingBox();
                    enemies.Add(patrol);
                    break;
            }
        }

        private void generatePickups(int count, int segment, int type) {
            float position;
            switch (type) {
                case 0://Health/Speed/Damage at Once
                    position = floors[2].position.X + count * segment + 512;
                    if (position <= floors[2].leftEnd()) {
                        collectibles.Add(new HealthPickUp(new Vector2((floors[2].position.X + count * segment) + 260, floors[2].position.Y - 32)));
                        platforms.Add(new Platform(platformType.Platform, new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 196)));
                    }
                    if (position <= floors[2].leftEnd()) {
                        collectibles.Add(new SpeedPickUp(new Vector2((floors[2].position.X + count * segment) + 260, floors[2].position.Y - 196 - 32)));
                        platforms.Add(new Platform(platformType.Platform, new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 392)));
                    }
                    if (position <= floors[2].leftEnd()) {
                        collectibles.Add(new DamagePickUp(new Vector2((floors[2].position.X + count * segment) + 260, floors[2].position.Y - 392 - 32)));
                    }
                    itemHasAppeared = true;
                    break;

                case 1://Ouroboros
                    position = floors[2].position.X + count * segment + 512;
                    if (position <= floors[2].leftEnd()) {
                        if (random.Next(0, 2) == 0) generateEnemy(count, segment);
                        platforms.Add(new Platform(platformType.Platform, new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 196)));
                    }
                    count += 4;
                    if (position <= floors[2].leftEnd()) {
                        if (random.Next(0, 2) == 0) generateEnemy(count, segment);
                        platforms.Add(new Platform(platformType.Platform, new Vector2(floors[2].position.X + count * segment, floors[2].position.Y - 392)));
                    }
                    if (position <= floors[2].leftEnd()) {
                        //ouros.Add(new Ouroboros(new Vector2(floors[2].position.X + count * segment + 480, floors[2].position.Y - 392 - 32)));                        
                        ouros.Add(new Ouroboros(new Vector2((floors[2].position.X + count * segment) + 480, floors[2].position.Y - 392 - 32)));                        
                    }
                    ouroHasAppeared = true;
                    break;
            }
        }

        public void Draw(SpriteBatch batch, GraphicsDeviceManager graphics, Effect shaderEffect) {

            background.draw(batch, camera, graphics, shaderEffect);

            batch.End();
            shaderEffect.CurrentTechnique = shaderEffect.Techniques["Ribbon"];
            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
            
            
            foreach (Platform item in blocks) {
                item.Draw(batch);
            }

            foreach (Platform item in platforms) {
                item.Draw(batch);
            }

            foreach (Platform item in floors) {
                item.Draw(batch);
            }

            batch.End();
            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
            shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];

            foreach (Enemy item in enemies) {
                item.Draw(batch);
            }

            batch.End();
            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
            shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];

            foreach (Object collect in collectibles) {
                collect.Draw(batch);
            }
            foreach (Object daOuro in ouros) {
                daOuro.Draw(batch);
            }

        }

        public void moveLevel(Vector2 Movement) {
            background.moveBackground(Movement);
            foreach (Platform item in blocks) {
                item.position += Movement;
                item.updateBoundingBox();
            }
            foreach (Platform item in platforms) {
                item.position += Movement;
                item.updateBoundingBox();
            }
            foreach (Platform item in floors) {
                item.position += Movement;
                item.updateBoundingBox();
            }
            foreach (Enemy item in enemies) {
                item.position += Movement;
                item.updateBoundingBox();
            }
            foreach (Object collect in collectibles) {
                collect.position += Movement;
                collect.updateBoundingBox();
            }
            foreach (Ouroboros o in ouros)
            {
                o.position += Movement;
                o.updateBoundingBox();
            }
        }

        public List<Object> LEVELCOMPONENT(levelComponent TYPE) {
            switch (TYPE) {
                case levelComponent.Block:
                    return blocks;
                case levelComponent.Floor:
                    return floors;
                case levelComponent.Platform:
                    return platforms;
                case levelComponent.Enemies:
                    return enemies;
                default:
                    return null;
            }
        }
    }
}
