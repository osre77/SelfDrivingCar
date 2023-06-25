namespace SelfDrivingCar.Core.Rendering;

/// <summary>
/// Interface for <see cref="Entity"/> render components.
/// </summary>
/// <remarks>
/// A renderer is meant to render one single aspect of an <see cref="Entity"/>.
/// This could be the entity itself or a debug representation of the entity.
/// </remarks>
[PublicAPI]
public interface IRenderer : IComponent
{
    /// <summary>
    /// Gets the layer this render is rendering on.
    /// </summary>
    int Layer { get; }

    /// <summary>
    /// Renders the aspect of the entity.
    /// </summary>
    /// <param name="context">The context to render to.</param>
    /// <param name="viewport">The current visible viewport.</param>
    /// <param name="zoomFactor">The current zoom factor.</param>
    /// <remarks>
    /// The coordinate system has X from left to right and Y from bottom to top.
    /// The <paramref name="viewport"/> can be used to cull the rendering.
    /// The <paramref name="zoomFactor"/> can be used to draw elements independent of the current zoom factor.
    /// e.g. to draw a line at 3 pixels width independent of the current zoom factor set the stroke thickness to <code>3f / zoomFactor</code>.
    /// </remarks>
    void Render(RenderContext context, Vector4 viewport, float zoomFactor);
}
