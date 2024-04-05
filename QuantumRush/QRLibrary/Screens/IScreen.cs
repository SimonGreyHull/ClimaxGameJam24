using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Screens
{
	internal interface IScreen
	{
		void Draw(float pSeconds);
		void Update(float pSeconds);
	}
}
