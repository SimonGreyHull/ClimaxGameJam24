using System.Collections.Generic;
using System.Linq;

namespace QRLibrary.Screens
{
	internal class ScreenManager
	{
		private List<IScreen> _scenes;

		public ScreenManager()
		{
			_scenes = new List<IScreen>();
		}

		public void Push(IScreen p_Scene)
		{
			_scenes.Add(p_Scene);
		}

		public void Pop()
		{
			if (_scenes.Count > 0)
			{
				_scenes.RemoveAt(_scenes.Count - 1);
			}
		}

		public IScreen Top
		{
			get
			{
				if (_scenes.Count > 0)
				{
					return _scenes.Last();
				}
				return null;
			}
		}
		public IScreen Previous
		{
			get
			{
				if (_scenes.Count > 1)
				{
					return _scenes[_scenes.Count - 2];
				}
				return null;
			}
		}

		public void Update(float pSeconds)
		{
			if (_scenes.Count > 0)
			{
				Top.Update(pSeconds);
			}
		}

		public void Draw(float pSeconds)
		{
			if (_scenes.Count > 0)
			{
				Top.Draw(pSeconds);
			}
		}
	}
}
