using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System;

namespace MonoGame_UI_Touch_Prototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		TouchCollection touchC;

		Texture2D texture;
		Sprite car;
		float speed = 5;
		

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

			graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			graphics.IsFullScreen = true;
			graphics.ApplyChanges();

			base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			texture = Content.Load<Texture2D>("car");
			car = new Sprite(texture, new Vector2(200, 200), 0);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

			//car.SetRotation(car.GetRotationDegrees() + 0.05f);
			touchC = TouchPanel.GetState();
			bool left = false;
			bool right = false;

			foreach (TouchLocation tl in touchC) {
				if (tl.Position.X < GraphicsDevice.DisplayMode.Width / 2) {
					left = true;
				}
				if (tl.Position.X > GraphicsDevice.DisplayMode.Width / 2) {
					right = true;
				}
			}

			float turn = 0;

			if (left) {
				turn -= 1;
			}
			if (right) {
				turn += 1;
			}

			car.SetRotation(car.GetRotationDegrees() + turn);

			// move forward

            Vector2 direction = new Vector2((float)Math.Cos(car.GetRotationRadians()), (float)Math.Sin(car.GetRotationRadians()));

            direction.Normalize();

            car.SetPosition(car.GetPosition() + direction * 2);


			// TODO: Add your update logic here

			base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
			spriteBatch.Begin();
			spriteBatch.Draw(car.GetTexture(), car.GetPosition(), rotation: car.GetRotationRadians(), origin: new Vector2(car.GetTexture().Width / 2, car.GetTexture().Height / 2));
			spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
