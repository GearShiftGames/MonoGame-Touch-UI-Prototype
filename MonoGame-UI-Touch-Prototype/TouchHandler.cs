using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonoGame_UI_Touch_Prototype {
	class TouchHandler {
	// Member methods
		public TouchHandler() {
			m_touches = new List<TouchLocation>();
			Update();
		}

		public void Update() {
			m_touchCollection = TouchPanel.GetState();

			m_touches.Clear();

			foreach (TouchLocation tl in m_touchCollection) {
				m_touches.Add(tl);
			}
		}

		public List<TouchLocation> GetTouches() {
			return m_touches;
		}

		public TouchLocation GetTouch(int index) {
			if (index >= m_touches.Count) {
				index = 0;
			}

			return m_touches[index];
		}



	// Member variables
		private TouchCollection m_touchCollection;
		private List<TouchLocation> m_touches;
	}
}
