using System;

namespace Styx.Logic.Profiles
{
	public class ProfileNotFoundException : Exception
	{
		public ProfileNotFoundException(string profilePath)
			: base("Profile not found: " + profilePath)
		{
			ProfilePath = profilePath;
		}

		public string ProfilePath { get; }
	}
}
