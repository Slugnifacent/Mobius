
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

    enum AnimationState {RUNNING = 0, JUMPING = 1,SHOOTING = 2}
    public class Character : Object
    {
        #region Fields
        #region Stats
        private float MAX_HEALTH;
        private float health;        
        private float Power;
        private float ShootSpeed;
        #endregion
      
        private float x_velocity, y_velocity;
        
        private Vector2 origin;
        private KeyboardState keyState;
        private KeyboardState prevState;
        private GamePadState contState;
        private GamePadState prevContState;
        
        private Vector2 direction;
        private bool colliding;
        private bool running;
        private bool canJump;

        private bool isShooting;
        private bool isJumping;
        private bool isFalling;
        public bool ouroFound;

        private bool isAgainstLeft;

        private float touchCoolDown;
        private float fireCoolDown;

        BulletPattern pattern;
        public int age;
        bool canFire;
        int frames;
        float timer = 0f;
        float shootTimer = 0f;
        float interval = 100f;
        int currentFrame = 1;
        int spriteWidth = 96;
        int spriteHeight = 128;
        Rectangle sourceRect;

        List<List<Texture2D>> animationTextures;

        SoundEffect jump;
        SoundEffect shoot;
        SoundEffect landing;
        SoundEffect damage;

        private bool isPlaying;
        float size;

        int hurtTimer;
        int animation;
        #endregion

        public Character() {
            size = 1;
           
            ResetPlayer();
            frames = 8;
            #region Animation
            animationTextures = new List<List<Texture2D>>();

            for(int index = 0; index < 4; index++)
                animationTextures.Add(new List<Texture2D>());

            animationTextures[0].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-run"));
            animationTextures[0].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-jump"));
            animationTextures[0].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-runAttack"));

            animationTextures[1].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-runYA"));
            animationTextures[1].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-jumpYA"));
            animationTextures[1].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-runAttackYA"));

            animationTextures[2].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-runA"));
            animationTextures[2].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-jumpA"));
            animationTextures[2].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-runAttackA"));

            animationTextures[3].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-runE"));
            animationTextures[3].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-jumpE"));
            animationTextures[3].Add(Game1.content.Load<Texture2D>(@"Player/spriteSheet-runAttackE"));
            

            
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);

            jump = Game1.content.Load<SoundEffect>(@"Sounds/jump");
            landing = Game1.content.Load<SoundEffect>(@"Sounds/landing");
            shoot = Game1.content.Load<SoundEffect>(@"Sounds/shoot");
            damage = Game1.content.Load<SoundEffect>(@"Sounds/playerDamage");
            MediaPlayer.IsRepeating = true;
            isPlaying = false;

            #endregion
        }

        #region Jump
        /// <summary>
        /// Chris Peterson - 1/27/12
        /// 
        /// Player Jumping Logic
        /// </summary>
        private void playerJump() {            
            if (y_velocity > 6)
            {
                return;
            }
            else {
                y_velocity-=35;
            }            
        }
        #endregion

        #region Attack
        /// <summary>
        /// Chris Peterson - 1/27/12
        /// </summary>
        private void Shoot() {
            /*if (direction.X > 0)
            {
                Bullet temp = Game1.retrieveBullet();
                temp.initialize(direction, new Vector2(position.X + 96, position.Y + 64), bulletType.Friendly);
                Game1.addBullet(temp);
            }
            else if (direction.X < 0)
            {
                Bullet temp = Game1.retrieveBullet();
                temp.initialize(direction, new Vector2(position.X, position.Y + 64), bulletType.Friendly);
                Game1.addBullet(temp);
            }*/
            pattern.Fire(new Vector2(position.X,position.Y + 5), direction, bulletType.Friendly,Power);
        }

        #endregion

        public Vector2 getPosition()
        {
            return position;
        }

        public float GetHealth()
        {
            return health;
        }

        public bool CheckDead()
        {
            if (health <= 0)
                return true;
            return false;
        }

        public void Suicide()
        {
            health = 0;
        }

        public void ResetPlayer()
        {
            position = new Vector2(100, 100);
            x_velocity = y_velocity = 0;
            direction = new Vector2(1, 0);

            #region Stats
            MAX_HEALTH = 100;
            health = MAX_HEALTH;
            Power = 5;
            ShootSpeed = .33f;
            #endregion

            boundingBox = new Rectangle((int)position.X, (int)position.Y, 96, 128);
            //texture = Game1.content.Load<Texture2D>(@"player_down_fast_1");

            isJumping = false;
            running = false;
            canJump = false;

            canFire = true;
            fireCoolDown = 0;
            elapsedTime = 0;

            isAgainstLeft = false;
            touchCoolDown = 2.0f;
            pattern = new SingleShot();
            ouroFound = false;
        }

        private void Controller() {
            prevContState = contState;
            contState = GamePad.GetState(PlayerIndex.One);
            running = false;

            if (contState.ThumbSticks.Left.X > 0.1)
            {   //Right                    
                direction.X = 1;
                if (x_velocity < 6)
                {
                    if (canJump) running = true;
                    x_velocity++;
                }
            }
            else if (contState.ThumbSticks.Left.X < -0.1)
            { //Left
                direction.X = -1;
                if (x_velocity > -4)
                {
                    if (canJump) running = true;
                    x_velocity--;
                }
            }            

            if (!running)
            {
                if (direction.X == 1)
                {
                    if (x_velocity <= 0)
                        x_velocity = 0;
                    else
                        x_velocity -= (float).1;
                }
                else if (direction.X == -1)
                {
                    if (x_velocity >= 0)
                        x_velocity = 0;
                    else
                        x_velocity += (float).1;
                }
            }

            #region Shoot
            if (contState.IsButtonDown(Buttons.X) && prevContState.IsButtonUp(Buttons.X))
            {
                if (fireCoolDown >= ShootSpeed)
                {
                    isShooting = true;
                    shoot.Play();
                    Shoot();
                    fireCoolDown = 0;
                }
            }
            #endregion

            #region Jump
            if (contState.IsButtonDown(Buttons.A) && prevContState.IsButtonUp(Buttons.A)) {
                if (canJump)
                {
                    isJumping = true;
                    canJump = false;
                    playerJump();
                }
            }
            #endregion
        }

        private void KeyboardInput() {
            prevState = keyState;
            keyState = Keyboard.GetState();
            running = false;

            #region Movement
            if (keyState.IsKeyDown(Keys.Right))
            {
                if (canJump) running = true;
                direction.X = 1;
                if (x_velocity < 6)
                    x_velocity++;
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                if (canJump) running = true;
                direction.X = -1;
                if (x_velocity > -4)
                    x_velocity--;
            }
            else
            {                
                if (!running)
                {
                    if (direction.X == 1)
                    {
                        if (x_velocity <= 0)
                            x_velocity = 0;
                        else
                            x_velocity -= (float).1;
                    }
                    else if (direction.X == -1)
                    {
                        if (x_velocity >= 0)
                            x_velocity = 0;
                        else
                            x_velocity += (float).1;
                    }
                }
            }
            #endregion

            #region Jumping
            if (keyState.IsKeyDown(Keys.Space) && !isJumping)
            {
                frames = 7;
                animation = 1;
                if (canJump)
                {
                    isJumping = true;
                    canJump = false;
                    playerJump();
                    
                }
            }

            if (isJumping)
            {
                if (y_velocity >= 0)
                {                   
                    isJumping = false;
                }
                else if (!colliding)
                {
                    y_velocity += (float).8f;
                    isFalling = true;
                }
            }
            #endregion

            #region Shooting

            if (keyState.IsKeyDown(Keys.A) && prevState.IsKeyUp(Keys.A))
            {
                if (elapsedTime >= ShootSpeed)
                {
                    isShooting = true;
                    animation = (int)AnimationState.SHOOTING;
                    shoot.Play();                    
                    Shoot();
                    elapsedTime = 0;
                }
            }

            #endregion

            #region HELP
            if (keyState.IsKeyDown(Keys.H) && prevState.IsKeyUp(Keys.H))
            {
                print_Stats();
            }
            #endregion
        }


        public void Update(GameTime time)
        {
            if (hurtTimer > 150) {
                size = 1;
                hurtTimer = 0;
            }
            hurtTimer += (int)time.ElapsedGameTime.Milliseconds;
            float timeInSec = (float)time.ElapsedGameTime.TotalSeconds;
           
            elapsedTime += timeInSec;
            fireCoolDown += timeInSec;
            Controller();
            KeyboardInput();                       

            #region Movement
            if (!isJumping && running) {
                animation = (int)AnimationState.RUNNING;
                frames = 8;
            }
            else if (isJumping)
            {
                animation = (int)AnimationState.JUMPING;
                frames = 7;
            }            

            if (x_velocity != 0) running = true;
            else running = false;

            position.X += x_velocity;

            y_velocity += (float).8f;
            if(y_velocity > 5) y_velocity = 5;
            position.Y += y_velocity + 4;

            if (position.X <= Game1.camera.topLeft().X)
            {
                if (isAgainstLeft) health = 0;
                else
                {
                    position.X = Game1.camera.topLeft().X;
                    direction.X = 1;
                    x_velocity += .5f;
                }
            }



            isAgainstLeft = false;
            if (boundingBox.Right > Game1.camera.topRight().X)
                position.X = Game1.camera.topRight().X - boundingBox.Width;
            updateBoundingBox();
            #endregion


            shootTimer += (float)time.ElapsedGameTime.TotalMilliseconds;
            if (running || isJumping)
            {
                timer += (float)time.ElapsedGameTime.TotalMilliseconds;
                if (timer >= interval)
                {
                    if (canJump) currentFrame++;
                    else
                    {
                        if (isShooting)
                        {
                            animation = (int)AnimationState.SHOOTING;// = textureShoot;
                            currentFrame = 1;
                        }
                        else
                        {
                            animation = 1;
                            currentFrame = 3;
                        }
                    }
                    timer = 0f;
                }
                if (currentFrame >= frames)
                {
                    currentFrame = 0;
                }
            }
            else
            {                
                currentFrame = 0;
                timer = 0f;                
            }


            if (shootTimer > 2000)
            {
                isShooting = false;
                shootTimer = 0;
                
            }


            if (x_velocity == 0)
            {
                if (canJump)
                {
                    animation = (isShooting) ? (int)AnimationState.SHOOTING : (int)AnimationState.RUNNING;
                    currentFrame = 3;
                }
                else
                {
                    if (isShooting)
                    {
                        animation = (int)AnimationState.SHOOTING;
                        currentFrame = 1;
                    }
                    else
                    {
                        animation = (int)AnimationState.JUMPING;
                        currentFrame = 3;
                    }
                }

            }
            else {
                if (isShooting) animation = (int)AnimationState.SHOOTING;
            }


            sourceRect.X = currentFrame * spriteWidth;
            origin.X = sourceRect.Width / 2;
            origin.Y = sourceRect.Height / 2 - 3;
            canJump = false;
        }

        public override void Draw(SpriteBatch batch)
        {
            if(direction.X < 0) batch.Draw(animationTextures[age][animation], new Vector2(position.X + 48, position.Y + 64), sourceRect, Color.White, 0.0f, origin, size, SpriteEffects.FlipHorizontally, 0);
            else                batch.Draw(animationTextures[age][animation], new Vector2(position.X + 48, position.Y + 64), sourceRect, Color.White, 0.0f, origin, size, SpriteEffects.None, 0);            
        }

        #region Increase Stats
        private void increaseHealth() {
            MAX_HEALTH += 50;
            health = MAX_HEALTH;
        }

        private void increaseSpeed() {
            ShootSpeed /= 2.0f;
        }

        private void increaseDamage() {
            Power += 5;
        }
        #endregion

        #region HELP
        private void print_Stats() {
            Console.WriteLine("Player Stats:");
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Power: " + Power);
            Console.WriteLine("Bullet Speed: " + ShootSpeed);
            Console.WriteLine("X: " + position.X + ", Y: " + position.Y);
            Console.Write("Direction: ");
            if (direction.X < 0) Console.Write("Left\n");
            else if(direction.X > 0) Console.Write("Right\n");

        }
        #endregion

        public override void collisionResolution(Object item)
        {

            if (item.GetType() == typeof(Bullet))
            {
                Bullet temp = (Bullet)item;
                if (temp.bulletType == bulletType.Enemy)
                {
                    health -= temp.dmg;
                    damage.Play();
                    temp.Dead();
                    size = .8f;
                }
                if (temp.bulletType == bulletType.Enemy)
                    temp.Dead();
               
            }

            if (item.GetType() == typeof(HealthPickUp)) 
                increaseHealth();
            

            if (item.GetType() == typeof(SpeedPickUp)) 
                increaseSpeed();
            

            if (item.GetType() == typeof(DamagePickUp)) 
                increaseDamage();
            

            if (item.GetType() == typeof(Ouroboros)) 
                 ouroFound = true;
            

            if (item.GetType() == typeof(Stationary) || item.GetType() == typeof(Jumper) || item.GetType() == typeof(Pattern) || item.GetType() == typeof(Patrol))
            {
                if (elapsedTime >= touchCoolDown)
                {
                    size = .8f;
                    health -= 10;
                    elapsedTime = 0;
                    damage.Play();
                }
            }

            if (item.GetType() == typeof(Platform))
            {
                Platform temp = (Platform)item;
                if (temp.PLATFORMTYPE == platformType.Floor)
                {
                    position.Y = item.position.Y - boundingBox.Height;
                    if (isJumping) landing.Play();
                    canJump = true;
                    isFalling = false;
                    y_velocity = 0f;
                }

                if (temp.PLATFORMTYPE == platformType.Block || temp.PLATFORMTYPE == platformType.Platform){
                    if (boundingBox.Right > item.BOUNDINGBOX.Left && boundingBox.Right < item.BOUNDINGBOX.Left + 10.0f)
                    {
                        position.X = item.position.X - boundingBox.Width;
                        isAgainstLeft = true;

                    }else if (boundingBox.Left < item.BOUNDINGBOX.Right && boundingBox.Left > item.BOUNDINGBOX.Right - 10.0f)
                    {
                        position.X = item.BOUNDINGBOX.Right;
                    }
                    
                    else if (boundingBox.Bottom > item.BOUNDINGBOX.Top && boundingBox.Bottom < item.BOUNDINGBOX.Top + 10.0f)
                    {
                        position.Y = item.BOUNDINGBOX.Top - boundingBox.Height;
                        if (isJumping) landing.Play();
                        canJump = true;
                        isFalling = false;
                        y_velocity = 0f;
                    } 
                }
            }
        }


    }
}
