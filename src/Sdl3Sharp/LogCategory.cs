using System;
using System.ComponentModel;

namespace Sdl3Sharp;

/// <summary>
/// Represents a log category
/// </summary>
public enum LogCategory
{
	/// <summary>The log category <em>Application</em></summary>
	Application,

	/// <summary>The log category <em>Error</em></summary>
	Error,

	/// <summary>The log category <em>Assert</em></summary>
	Assert,

	/// <summary>The log category <em>System</em></summary>
	System,

	/// <summary>The log category <em>Audio</em></summary>
	Audio,

	/// <summary>The log category <em>Video</em></summary>
	Video,

	/// <summary>The log category <em>Render</em></summary>
	Render,

	/// <summary>The log category <em>Input</em></summary>
	Input,

	/// <summary>The log category <em>Test</em></summary>
	Test,

	/// <summary>The log category <em>Gpu</em></summary>
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
	/// <remarks>Do not use directly. Use <see cref="LogCategoryExtensions.Custom(int)"/> instead.</remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Do not use. Use the LogCategoryExtensions.Custom(int) extension method instead.")]
	Custom
}
