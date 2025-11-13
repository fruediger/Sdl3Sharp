namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the order of color components in a packed pixel format, from highest bit to lowest bit
/// </summary>
public enum PackedOrder
{
    /// <summary>No specific order</summary>
    None,

    /// <summary>Order: X (component, if any), R(ed component), G(reen component), B(lue component)</summary>
    Xrgb,

    /// <summary>Order: R(ed component), G(reen component), B(lue component), X (component, if any)</summary>
    Rgbx,

    /// <summary>Order: A(lpha component), R(ed component), G(reen component), B(lue component)</summary>
    Argb,

    /// <summary>Order: R(ed component), G(reen component), B(lue component), A(lpha component)</summary>
    Rgba,

    /// <summary>Order: X (component, if any), B(lue component), G(reen component), R(ed component)</summary>
    Xbgr,

    /// <summary>Order: B(lue component), G(reen component), R(ed component), X (component, if any)</summary>
    Bgrx,

    /// <summary>Order: A(lpha component), B(lue component), G(reen component), R(ed component)</summary>
    Abgr,

    /// <summary>Order: B(lue component), G(reen component), R(ed component), A(lpha component)</summary>
    Bgra
}
