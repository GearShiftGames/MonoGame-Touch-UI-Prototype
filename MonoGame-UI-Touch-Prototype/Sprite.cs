// MonoGame UI Touch Prototype
// Written by D. Sinclair, 2015
// ==============================
// Sprite.cs

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_UI_Touch_Prototype {
	class Sprite {
	// Member methods
		public Sprite(Texture2D texture, Vector2 position, float rotation) {
			SetTexture(texture);
			SetPosition(position);
			SetRotation(rotation);
		}

		// Getters
		public Texture2D GetTexture() {
			// Returns the texture of the sprite
			return m_texture;
		}

		public Vector2 GetPosition() {
			// Returns the position of the sprite
			return m_position;
		}

		public float GetRotationDegrees(){
			// Returns the rotation of the sprite, in degrees
			return m_rotation;
		}

		public float GetRotationRadians() {
			// Returns the rotation of the sprite, in radians
			float rad = m_rotation * (3.1415f / 180);

			return rad;
		}

		// Setters
		public void SetTexture(Texture2D texture) {
			// Sets the texture of the sprite
			m_texture = texture;
		}

		public void SetPosition(Vector2 position) {
			// Sets the position of the sprite
			m_position = position;
		}

		public void SetRotation(float rotation) {
			// Sets the rotation of the sprite, in degrees
			m_rotation = rotation;
		}

	// Member variables
		private Texture2D m_texture;
		private Vector2 m_position;
		private float m_rotation;		// Stored in degrees

	}
}
