namespace SelfDrivingCar.Core;

/// <summary>
/// Interface for all <see cref="Entity"/> components.
/// </summary>
[PublicAPI]
public interface IComponent
{
    /// <summary>
    /// Gets or sets the owning entity of the component.
    /// </summary>
    Entity? Entity { get; internal set; }
}
