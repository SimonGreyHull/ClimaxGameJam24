using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace QRLibrary
{
	internal class SoundManager
	{
		private Dictionary<string, SoundEffect> mSoundEffects = null;

		public SoundManager()
		{
			mSoundEffects = new Dictionary<string, SoundEffect>();
		}

		public void Add(string pName)
		{
			SoundEffect soundEffect = QuantumRush.Instance().Content.Load<SoundEffect>(pName);
			mSoundEffects.Add(pName, soundEffect);
		}

		public SoundEffectInstance GetSoundEffectInstance(string pName)
		{
			return mSoundEffects[pName].CreateInstance();
		}
		public SoundEffectInstance GetLoopableSoundEffectInstance(string pName)
		{
			SoundEffectInstance instance = GetSoundEffectInstance(pName);
			instance.IsLooped = true;
			return instance;
		}
	}
}
