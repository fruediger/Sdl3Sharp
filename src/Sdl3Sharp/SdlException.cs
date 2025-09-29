using System;

namespace Sdl3Sharp;

/// <summary>
/// An exception that is thrown when there was a failure coming from or regarding a SDL function
/// </summary>
/// <remarks>
/// When a <see cref="SdlException"/> is thrown (and caught), you might want to check <see cref="Error.TryGet(out string?)"/> for more information
/// </remarks>
/// <inheritdoc/>
public sealed class SdlException(string? message = default) : Exception(message);
