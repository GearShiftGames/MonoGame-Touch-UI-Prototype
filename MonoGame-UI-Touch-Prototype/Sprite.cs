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
			return texture;
		}

		public Vector2 GetPosition() {
			return position;
		}

		public float GetRotationDegrees(){
			return rotation;
		}

		public float GetRotationRadians() {
			float rad = rotation * (3.1415f / 180);

			return rad;
		}

		// Setters
		public void SetTexture(Texture2D texture) {
			this.texture = texture;
		}

		public void SetPosition(Vector2 position) {
			this.position = position;
		}

		public void SetRotation(float rotation) {
			this.rotation = rotation;
		}

	// Member variables
		private Texture2D texture;
		private Vector2 position;
		private float rotation;

	}
}
