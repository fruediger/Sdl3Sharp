using System;

namespace Sdl3Sharp;

partial struct LogCategory
{
	/// <summary>
	/// The predefined log categories
	/// </summary>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogCategory">SDL_LogCategory</seealso>
	internal enum Kind
	{
		/// <summary>SDL_LOG_CATEGORY_APPLICATION</summary>
		Application,

		/// <summary>SDL_LOG_CATEGORY_ERROR</summary>
		Error,

		/// <summary>SDL_LOG_CATEGORY_ASSERT</summary>
		Assert,

		/// <summary>SDL_LOG_CATEGORY_SYSTEM</summary>
		System,

		/// <summary>SDL_LOG_CATEGORY_AUDIO</summary>
		Audio,

		/// <summary>SDL_LOG_CATEGORY_VIDEO</summary>
		Video,

		/// <summary>SDL_LOG_CATEGORY_RENDER</summary>
		Render,

		/// <summary>SDL_LOG_CATEGORY_INPUT</summary>
		Input,

		/// <summary>SDL_LOG_CATEGORY_TEST</summary>
		Test,

		/// <summary>SDL_LOG_CATEGORY_GPU</summary>
		Gpu,

		/// <summary>SDL_LOG_CATEGORY_RESERVED2</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved2,

		/// <summary>SDL_LOG_CATEGORY_RESERVED3</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved3,

		/// <summary>SDL_LOG_CATEGORY_RESERVED4</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved4,

		/// <summary>SDL_LOG_CATEGORY_RESERVED5</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved5,

		/// <summary>SDL_LOG_CATEGORY_RESERVED6</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved6,

		/// <summary>SDL_LOG_CATEGORY_RESERVED7</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved7,

		/// <summary>SDL_LOG_CATEGORY_RESERVED8</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved8,

		/// <summary>SDL_LOG_CATEGORY_RESERVED9</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved9,

		/// <summary>SDL_LOG_CATEGORY_RESERVED10</summary>
		/// <remarks>Reserved for future SDL library use. Do not use.</remarks>
		[Obsolete("Reserved for future SDL library use")]
		Reserved10,

		/// <summary>SDL_LOG_CATEGORY_CUSTOM</summary>
		/// <remarks>
		/// Beyond this point is reserved for application use, e.g.
		/// <code>
		/// enum {
		///     MYAPP_CATEGORY_AWESOME1 = SDL_LOG_CATEGORY_CUSTOM,
		///     MYAPP_CATEGORY_AWESOME2,
		///     MYAPP_CATEGORY_AWESOME3,
		///     ...
		/// };
		/// </code>
		/// </remarks>
		Custom
	}
}
