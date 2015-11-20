// MonoGame UI Touch Prototype -- GearShiftGames
// Written by D. Sinclair, 2015
// ================
// Game1.cs
// Classes: Game1
// Application class for the running of the game, handles the game loop

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System;

namespace MonoGame_UI_Touch_Prototype {
    public class Game1 : Game {
	// Application class for the game, handles the game loop and all game features
	// ================

	// Member methods
		// Constructors
		public Game1() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		// Public methods

		// Protected methods
		protected override void Initialize() {
			// Any game initialisation in performed here
			// Init window (size, fullscreen)
			graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			graphics.IsFullScreen = fullscreen;
			graphics.ApplyChanges();

			touchHandler = new TouchHandler();

			car = new List<Player>();

			base.Initialize();	// enumerates through components and initialises them too
		}

		protected override void LoadContent() {
			// Loads all the game content assets
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Init car sprite
			carTexture = Content.Load<Texture2D>("car");
			circleTexture = Content.Load<Texture2D>("circle");

			// Bottom right
			car.Add(new Player(carTexture,
							   new Vector2(1800, 800),
							   225,
							   new TouchZone(new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2),
											 new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height)),
							   0,
							   circleTexture,
							   new Vector2(GraphicsDevice.DisplayMode.Width * 0.8f, GraphicsDevice.DisplayMode.Height * 0.8f)));

			// Bottom left
			car.Add(new Player(carTexture,
							   new Vector2(200, 800),
							   315,
							   new TouchZone(new Vector2(0, GraphicsDevice.DisplayMode.Height / 2),
											 new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height)),
							   0,
							   circleTexture,
							   new Vector2(GraphicsDevice.DisplayMode.Width * 0.2f, GraphicsDevice.DisplayMode.Height * 0.8f)));

			// Top right
			car.Add(new Player(carTexture,
							   new Vector2(1800, 200),
							   135,
							   new TouchZone(new Vector2(GraphicsDevice.DisplayMode.Width / 2, 0),
											 new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height / 2)),
							   0,
							   circleTexture,
							   new Vector2(GraphicsDevice.DisplayMode.Width * 0.8f, GraphicsDevice.DisplayMode.Height * 0.15f)));

			// Top left
			car.Add(new Player(carTexture,
							   new Vector2(200, 200),
							   45,
							   new TouchZone(new Vector2(0, 0),
											 new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2)),
							   0,
							   circleTexture,
							   new Vector2(GraphicsDevice.DisplayMode.Width * 0.2f, GraphicsDevice.DisplayMode.Height * 0.15f)));
		}

		protected override void UnloadContent() {
			// Unloads any game content assets
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update(GameTime gameTime) {
			// Updates all game objects
			if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if(Keyboard.GetState().IsKeyDown(Keys.F11)) {
				graphics.IsFullScreen = !fullscreen;
				graphics.ApplyChanges();
				fullscreen = !fullscreen;
			}

			// Update touch handler
			touchHandler.Update();

			// Get touch
			foreach(TouchLocation tl in touchHandler.GetTouches()) {
				GetCarInput(0, tl);
				GetCarInput(1, tl);
				GetCarInput(2, tl);
				GetCarInput(3, tl);
			}

			// Update objects
			car[0].Update();
			car[1].Update();
			car[2].Update();
			car[3].Update();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			// Renders all sprites in the game to the screen

			// Clears the screen
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Render sprites
			spriteBatch.Begin();
			foreach(Player pl in car) {
				spriteBatch.Draw(pl.m_car.GetTexture(), pl.m_car.GetPosition(), rotation: pl.m_car.GetRotationRadians(), origin: new Vector2(pl.m_car.GetTexture().Width / 2, pl.m_car.GetTexture().Height / 2));
				spriteBatch.Draw(pl.m_circle.GetTexture(), pl.m_circle.GetPosition());
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}

		// Private methods
		private void GetCarInput(int index, TouchLocation tl) {
			// Gets the inputs for each player
			// Check touch is within control zone
			if(car[index].m_controlZone.IsInsideZone(tl.Position)) {
				// If there is a second touch, brake car
				if(car[index].m_set && !car[index].GetBraking()) {
					car[index].SetBraking(true);
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
					Vector2 pos = new Vector2(car[index].m_circleStart.X - car[index].m_steeringValue, car[index].m_circleStart.Y);

					car[index].m_circle.SetPosition(pos);
					car[index].m_set = true;
				}
			}
		}

		// Getters

		// Setters

	// Member variables
		// Public variables

		// Protected variables

		// Private variables
		private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
		private TouchHandler touchHandler;
		private Texture2D carTexture;
		private List<Player> car;
		private Texture2D circleTexture;
		private bool fullscreen = true;
    }
}
