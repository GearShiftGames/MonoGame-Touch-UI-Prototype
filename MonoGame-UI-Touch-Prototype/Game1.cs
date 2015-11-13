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

		List<Player> car;
		//Sprite car;
		//const float MAX_SPEED = 10;
		//const float STEERING_RANGE = 200;
		//float speed = 15;

        Texture2D circleTexture;
        //Sprite circle;
        //Vector2 circleStart;

		//TouchZone controlZone;

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

			car = new List<Player>();
			// Init control zone
			//controlZone = new TouchZone(new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2),
			//							new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height));

			

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
			circleTexture = Content.Load<Texture2D>("circle");
			//car = new Sprite(carTexture, new Vector2(200, 200), 0);
			car.Add(new Player(carTexture,
							   new Vector2(200, 200),
							   45,
							   new TouchZone(new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2),
											 new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height)),
							   0,
							   circleTexture,
							   new Vector2(GraphicsDevice.DisplayMode.Width * 0.8f, GraphicsDevice.DisplayMode.Height * 0.8f)));

			// Init circle sprite
            
            //circleStart = new Vector2(GraphicsDevice.DisplayMode.Width * 0.8f, GraphicsDevice.DisplayMode.Height * 0.8f);
			//circleStart = new Vector2(1200, 800);
			//circle = new Sprite(circleTexture, circleStart, 0);

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

			// Get touch
			foreach (TouchLocation tl in touchHandler.GetTouches()) {
				GetCarInput(0, tl);
			}

			car[0].Update();

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
			foreach(Player pl in car) {
				spriteBatch.Draw(pl.m_car.GetTexture(), pl.m_car.GetPosition(), rotation: pl.m_car.GetRotationRadians(), origin: new Vector2(pl.m_car.GetTexture().Width / 2, pl.m_car.GetTexture().Height / 2));
				spriteBatch.Draw(pl.m_circle.GetTexture(), pl.m_circle.GetPosition());
			}
			//spriteBatch.Draw(car[0].m_car.GetTexture(), car[0].m_car.GetPosition(), rotation: car[0].m_car.GetRotationRadians(), origin: new Vector2(car[0].m_car.GetTexture().Width / 2, car[0].m_car.GetTexture().Height / 2));
			//spriteBatch.Draw(circle.GetTexture(), circle.GetPosition());
			//spriteBatch.Draw(circle.GetTexture(), new Vector2(circleStart.X - 200, circle.GetPosition().Y));		// placeholders to bug-fix
			//spriteBatch.Draw(circle.GetTexture(), new Vector2(circleStart.X + 200, circle.GetPosition().Y));		// placeholders to bug-fix
            spriteBatch.End();

            base.Draw(gameTime);
        }

		private void GetCarInput(int index, TouchLocation tl) {
			// Check touch is within control zone
			if(car[index].m_controlZone.isInsideZone(tl.Position)) {
				// If there is a second touch, brake car
				if(car[index].m_set && !car[0].GetBraking()) {
					car[0].SetBraking(true);
				}

				// If there is a touch
				if(!car[index].m_set) {
					// Find steering distance
					car[index].m_steeringValue = car[index].m_circleStart.X - tl.Position.X + car[index].m_circle.GetTexture().Width / 2;

					// Clamp steering
					if(car[index].m_steeringValue > car[index].STEERING_RANGE) {
						car[index].m_steeringValue = car[index].STEERING_RANGE;
					} else if(car[index].m_steeringValue < -car[index].STEERING_RANGE) {
						car[index].m_steeringValue = -car[index].STEERING_RANGE;
					}

					// Set circle position
					Vector2 pos = new Vector2(car[index].m_circleStart.X - car[0].m_steeringValue, car[index].m_circleStart.Y);

					car[index].m_circle.SetPosition(pos);
					car[index].m_set = true;

				}
			}
		}
    }
}
