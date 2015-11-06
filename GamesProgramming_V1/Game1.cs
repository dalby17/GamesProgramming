using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace GamesProgramming_V1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture, player, platform;
        Vector2 texturePos, velocity;
        Vector2 texturePos1;
        Vector2 texturePos2;

        const float gravity = 100f;
        float moveSpeed = 500f;
        float moveSpeedback = -500f;

        //collision detection variables
        Point playerFrameSize = new Point(40, 41);
        Point platformFrameSize = new Point(300, 5);
        int playerCollisionRectOffset = 0;
        int platformCollisionRectOffset = 0;

        protected bool Collide()
        {
            Rectangle playerRect = new Rectangle(
                (int) texturePos.X + playerCollisionRectOffset,
                (int) texturePos.Y + playerCollisionRectOffset,
                playerFrameSize.X - (playerCollisionRectOffset *2),
                playerFrameSize.Y - (playerCollisionRectOffset *2));
            Rectangle platformRect = new Rectangle(
                (int) texturePos2.X + platformCollisionRectOffset,
                (int) texturePos2.Y + platformCollisionRectOffset,
                platformFrameSize.X - (platformCollisionRectOffset *2),
                platformFrameSize.Y - (platformCollisionRectOffset *2));

            return playerRect.Intersects(platformRect);
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";

            texturePos1 = new Vector2(0, 0);
            texturePos2 = new Vector2(0, 300);

            this.Activated += (sender, args) => { this.Window.Title = "Game Title"; };
            this.Deactivated += (sender, args) => { this.Window.Title = "Game Title (Paused)"; };
        }

        protected override void Initialize()
        {
            texturePos = velocity = new Vector2(0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = this.Content.Load<Texture2D>("Background");
            player = this.Content.Load<Texture2D>("Bike1");
            platform = this.Content.Load<Texture2D>("Platform");
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {

            if (IsActive)
            {
                KeyboardState state = Keyboard.GetState();
                //Exit Game through Esc key
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                //Tells what keys are being pressed
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (var key in state.GetPressedKeys())
                    sb.Append("Keys: ").Append(key).Append(" Pressed");

                if(sb.Length > 0)
                System.Diagnostics.Debug.WriteLine(sb.ToString());
                else
                System.Diagnostics.Debug.WriteLine("No Keys Pressed");

                //Movement of player character
                if (state.IsKeyDown(Keys.Up))
                    velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else if (state.IsKeyDown(Keys.Down))
                    velocity.X = moveSpeedback * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.X = 0;
                    velocity.Y = 1f;
                    texturePos += velocity;

                //collision detection reaction
                if (Collide())
                {
                   velocity.Y = 0f;
                }


                //collision with window size/cant go off screen
                if (texturePos.X < 0)
                {
                    texturePos.X = 0;
                }
                if (texturePos.Y < 0)
                {
                    texturePos.Y = 0;
                }

                if (texturePos.X > Window.ClientBounds.Width - playerFrameSize.X)
                {
                    texturePos.X = Window.ClientBounds.Width - playerFrameSize.X;
                }
                
                if (texturePos.Y > Window.ClientBounds.Height - playerFrameSize.Y)
                {
                    Exit();
                }


                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, texturePos1);
            spriteBatch.Draw(player, texturePos);
            spriteBatch.Draw(platform, texturePos2);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
