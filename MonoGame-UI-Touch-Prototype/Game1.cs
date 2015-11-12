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

		Texture2D carTexture;
		Sprite car;
		float speed = 5;
		bool brake = false;

        Texture2D circleTexture;
        Sprite circle;
        Vector2 circleStart;
		

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

			carTexture = Content.Load<Texture2D>("car");
			car = new Sprite(carTexture, new Vector2(200, 200), 0);

            circleTexture = Content.Load<Texture2D>("circle");
            circleStart = new Vector2(GraphicsDevice.DisplayMode.Width/2 - circleTexture.Width/2, GraphicsDevice.DisplayMode.Height/2 - circleTexture.Height/2);
            circle = new Sprite(circleTexture, circleStart, 0);
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

			touchC = TouchPanel.GetState();

            bool set = false;
			bool brake = false;

			// Get touch
			foreach (TouchLocation tl in touchC) {
				if (set && !brake) {		// touch 2
					brake = true;
				}
				if (!set) {					// touch 1
                    circle.SetPosition(new Vector2(tl.Position.X - circleTexture.Width / 2, tl.Position.Y - circleTexture.Height / 2));
                    set = true;
                }
			}


			// If no active touch
            if (!set) {
                circle.SetPosition(circleStart);
            }

			// Set rotation according to how far the touch is from the centre, if car is moving
			if (speed > 0.5) {
				float turn = (circleStart.X - circle.GetPosition().X) / 200 * -1;

				car.SetRotation(car.GetRotationDegrees() + turn);
			}

			// Check if car is braking
			if (brake) {
				if (speed > 0) {
					speed -= 0.1f;
				} else if (speed < 0) {
					speed = 0;
				}
			} else {
				if (speed < 5) {
					speed += 0.1f;
				}else if(speed > 5){
					speed = 5;
				}
			}

			// Move car forward by the speed
            Vector2 direction = new Vector2((float)Math.Cos(car.GetRotationRadians()), (float)Math.Sin(car.GetRotationRadians()));

            direction.Normalize();

            car.SetPosition(car.GetPosition() + direction * speed);


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
            spriteBatch.Draw(circle.GetTexture(), circle.GetPosition());
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
