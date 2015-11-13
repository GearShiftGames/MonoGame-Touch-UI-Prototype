// MonoGame UI Touch Prototype
// Written by D. Sinclair, 2015
// ==============================
// Game1.cs

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

		TouchHandler touchHandler;

		Texture2D carTexture;
		Sprite car;
		const float MAX_SPEED = 10;
		float speed = 15;

        Texture2D circleTexture;
        Sprite circle;
        Vector2 circleStart;

		const float STEERING_RANGE = 200;
		TouchZone controlZone;

		bool fullscreen = true;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
			// Init window (size, fullscreen)
			graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			graphics.IsFullScreen = fullscreen;
			graphics.ApplyChanges();

			touchHandler = new TouchHandler();

			// Init control zone
			controlZone = new TouchZone(new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2),
										new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height));

			base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			// Init car sprite
			carTexture = Content.Load<Texture2D>("car");
			car = new Sprite(carTexture, new Vector2(200, 200), 0);

			// Init circle sprite
            circleTexture = Content.Load<Texture2D>("circle");
            circleStart = new Vector2(GraphicsDevice.DisplayMode.Width * 0.8f - circleTexture.Width/2, GraphicsDevice.DisplayMode.Height * 0.8f - circleTexture.Height/2);
            circle = new Sprite(circleTexture, circleStart, 0);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

			if(Keyboard.GetState().IsKeyDown(Keys.F11)) {
				graphics.IsFullScreen = !fullscreen;
				graphics.ApplyChanges();
				fullscreen = !fullscreen;
			}

			// Update touch handler
			touchHandler.Update();

            bool set = false;
			bool brake = false;
			float distance = 0;

			// Get touch
			foreach (TouchLocation tl in touchHandler.GetTouches()) {
				// Check touch is within control zone
				if (controlZone.isInsideZone(tl.Position)) {
					// If there is a second touch, brake car
					if (set && !brake) {
						brake = true;
					}

					// If there is a touch
					if(!set) {
						// Find steering distance
						distance = circleStart.X - tl.Position.X;

						// Clamp steering
						if(distance > STEERING_RANGE) {
							distance = STEERING_RANGE;
						} else if(distance < -STEERING_RANGE) {
							distance = -STEERING_RANGE;
						}

						// Set circle position
						Vector2 pos = new Vector2(circleStart.X - distance - circle.GetTexture().Width/2, circleStart.Y);

						circle.SetPosition(pos);
						set = true;

					}
				}
			}


			// If no active touch
            if (!set) {
                circle.SetPosition(circleStart);
            }

			// Set rotation according to how far the touch is from the centre, if car is moving
			if (speed > 1) {
				float turn = distance / 50 * -1;

				if (brake) {
					turn *= 0.1f * speed;
				}

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
				if (speed < MAX_SPEED) {
					speed += 0.1f;
				}else if(speed > MAX_SPEED){
					speed = MAX_SPEED;
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
			spriteBatch.Draw(circle.GetTexture(), new Vector2(circleStart.X - 200, circle.GetPosition().Y));		// placeholders to bug-fix
			spriteBatch.Draw(circle.GetTexture(), new Vector2(circleStart.X + 200, circle.GetPosition().Y));		// placeholders to bug-fix
			//spriteBatch.DrawString
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
