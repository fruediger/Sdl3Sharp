namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the order of color components in an array, from lowest address byte to highest address byte
/// </summary>
public enum ArrayOrder
{
    /// <summary>No specific order</summary>
    None,

    /// <summary>Order: R(ed component), G(reen component), B(lue component)</summary>
    Rgb,

    /// <summary>Order: R(ed component), G(reen component), B(lue component), A(lpha component)</summary>
    Rgba,

    /// <summary>Order: A(lpha component), R(ed component), G(reen component), B(lue component)</summary>
    Argb,

    /// <summary>Order: B(lue component), G(reen component), R(ed component)</summary>
    Bgr,

    /// <summary>Order: B(lue component), G(reen component), R(ed component), A(lpha component)</summary>
    Bgra,

    /// <summary>Order: A(lpha component), B(lue component), G(reen component), R(ed component)</summary>
    Abgr
}
