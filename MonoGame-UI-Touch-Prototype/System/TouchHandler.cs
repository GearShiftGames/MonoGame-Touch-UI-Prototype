// MonoGame UI Touch Prototype -- GearShiftGames
// Written by D. Sinclair, 2015
// ================
// TouchHandler.cs
// Classes: TouchHandler, TouchZone
// Contains strutcures for dealing with touch input

using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonoGame_UI_Touch_Prototype {
	class TouchHandler {
	// Gets the inputs from the touch screen and makes them accessible
	// ================

	// Member methods
		// Constructors
		public TouchHandler() {
			m_touches = new List<TouchLocation>();
			Update();
		}

		// Public methods
		public void Update() {
			// Updates the touch handler with all the touch points
			m_touchCollection = TouchPanel.GetState();

			m_touches.Clear();

			foreach(TouchLocation tl in m_touchCollection) {
				m_touches.Add(tl);
			}
		}

		public List<TouchLocation> GetTouches() {
			// Returns a list of all screen touches
			return m_touches;
		}

		public TouchLocation GetTouch(int index) {
			// NOTE: returns the first touch if out of range, will fail if there are no touches
			// Inadvisable to use until rewritten to include appropriate input validation
			if(index >= m_touches.Count) {
				index = 0;
			}

			return m_touches[index];
		}

		// Protected methods

		// Private methods

		// Getters

		// Setters

	// Member variables
		// Public variables

		// Protected variables

		// Private variables
		private TouchCollection m_touchCollection;
		private List<TouchLocation> m_touches;
	}

	class TouchZone {
	// Sets a rectangular zone which can detect whether a position is within or outwith it
	// ================

	// Member methods
		// Constructors
		public TouchZone(Vector2 min, Vector2 max) {
			SetMin(min);
			SetMax(max);
		}

		public TouchZone(TouchZone zone) {
			SetMin(zone.GetMin());
			SetMax(zone.GetMax());
		}

		// Public methods
		public bool IsInsideZone(Vector2 position) {
			// Checks whether the position is within the touch zone
			if(position.X < m_min.X)
				return false;
			if(position.X > m_max.X)
				return false;
			if(position.Y < m_min.Y)
				return false;
			if(position.Y > m_max.Y)
				return false;

			return true;
		}

		// Protected methods

		// Private methods

		// Getters
		public Vector2 GetMin() {
			// Returns the min position of the touch zone
			return m_min;
		}

		public Vector2 GetMax() {
			// Returns the max position of the touch zone
			return m_max;
		}

		// Setters
		public void SetMin(Vector2 min) {
			// Sets the min position of the touch zone
			m_min = min;
		}

		public void SetMax(Vector2 max) {
			// Sets the max position of the touch zone
			m_max = max;
		}

	// Member variables
		// Public variables

		// Protected variables

		// Private variables
		private Vector2 m_min;
		private Vector2 m_max;
	}
}
