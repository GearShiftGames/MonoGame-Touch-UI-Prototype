// MonoGame UI Touch Prototype
// Written by D. Sinclair, 2015
// ==============================
// Player.cs

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_UI_Touch_Prototype {
	class Player {
	// Member methods
		public Player(Texture2D texture, Vector2 position, float rotation, TouchZone zone, float speed, Texture2D circleTexture, Vector2 circleStart) {
			m_car = new Sprite(texture, position, rotation);
			m_controlZone = new TouchZone(zone);
			m_speed = speed;

			m_circleStart = circleStart;
			m_circle = new Sprite(circleTexture, circleStart, 0);
			
		}

		// Getters
		public void Update() {
			// If no active touch
			if(!m_set) {
				m_circle.SetPosition(m_circleStart);
			}

			// Set rotation according to the steering value (touch distance)
			if(m_speed > 1) {
				float turn = m_steeringValue / 50 * -1;

				if(m_braking) {
					turn *= 0.1f * m_speed;
				}

				m_car.SetRotation(m_car.GetRotationDegrees() + turn);
			}

			// Check if car is braking
			if(m_braking) {
				// Decelerate
				if(m_speed > 0) {
					m_speed -= 0.1f;
				} else if(m_speed < 0) {
					m_speed = 0;
				}
			} else {
				// Accelerate
				if(m_speed < MAX_SPEED) {
					m_speed += 0.1f;
				} else if(m_speed > MAX_SPEED) {
					m_speed = MAX_SPEED;
				}
			}

			// Move car forward
			Vector2 direction = new Vector2((float)Math.Cos(m_car.GetRotationRadians()), (float)Math.Sin(m_car.GetRotationRadians()));

			direction.Normalize();

			m_car.SetPosition(m_car.GetPosition() + direction * m_speed);

			m_braking = false;
			m_set = false;
			m_steeringValue = 0;
		}

		public float GetSpeed() {
			return m_speed;
		}

		public bool GetBraking() {
			return m_braking;
		}

		// Setters
		public void SetSpeed(float speed) {
			m_speed = speed;
		}

		public void SetBraking(bool braking) {
			m_braking = braking;
		}

	// Member variables
		public Sprite m_car;
		public TouchZone m_controlZone;

		private float m_speed;
		private bool m_braking;
		public float m_steeringValue;
		public bool m_set;
		private const float MAX_SPEED = 10;
		public float STEERING_RANGE = 200;

		public Sprite m_circle;
		public Vector2 m_circleStart;
	}
}
