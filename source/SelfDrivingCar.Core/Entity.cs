using SelfDrivingCar.Core.Colliders;
using SelfDrivingCar.Core.Controller;
using SelfDrivingCar.Core.Parameters;
using SelfDrivingCar.Core.Rendering;
using SelfDrivingCar.Core.Sensors;

namespace SelfDrivingCar.Core;

/// <summary>
/// An entity of an entity graph.
/// </summary>
/// <remarks>
/// A entity has no behaviour. Its only properties are the <see cref="Position"/> and <see cref="Angle"/>.
/// Entities are defined by its components. Add additional components with the fluent API methods <c>With...</c>.
/// </remarks>
[PublicAPI]
public class Entity
{
    /// <summary>
    /// Gets or sets the graph this entity belongs to.
    /// </summary>
    public EntityGraph? Graph { get; set; }

    /// <summary>
    /// Gets or sets the position of the entity.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the angle of the entity in rad.
    /// </summary>
    public float Angle { get; set; }

    /// <summary>
    /// Gets the parameter set components of the entity.
    /// </summary>
    public IList<IParameterSet> Parameters { get; } = new List<IParameterSet>();

    /// <summary>
    /// Gets the renderer components of the entity.
    /// </summary>
    public IList<IRenderer> Renderers { get; } = new List<IRenderer>();

    /// <summary>
    /// Gets the controller components of the entity.
    /// </summary>
    public IList<IController> Controllers { get; } = new List<IController>();

    /// <summary>
    /// Gets the sensor components of the entity.
    /// </summary>
    public IList<ISensor> Sensors { get; } = new List<ISensor>();

    /// <summary>
    /// Gets the collider components of the entity.
    /// </summary>
    public IList<BaseCollider> Colliders { get; } = new List<BaseCollider>();

    /// <summary>
    /// Creates anew entity.
    /// </summary>
    public Entity()
    { }

    /// <summary>
    /// Creates anew entity with a position and angle.
    /// </summary>
    /// <param name="position">Position of the entity.</param>
    /// <param name="angle">Angle of the entity in rad.</param>
    public Entity(Vector2 position, float angle)
    {
        Position = position;
        Angle = angle;
    }


    /// <summary>
    /// Renders the entity.
    /// </summary>
    /// <param name="context">The context to render to.</param>
    /// <param name="layer">The layer number to render. Only the matching renderer components will be rendered.</param>
    /// <param name="viewport">The current visible viewport.</param>
    /// <param name="zoomFactor">The current zoom factor.</param>
    /// <remarks>
    /// The coordinate system has X from left to right and Y from bottom to top.
    /// The <paramref name="viewport"/> can be used to cull the rendering.
    /// The <paramref name="zoomFactor"/> can be used to draw elements independent of the current zoom factor.
    /// e.g. to draw a line at 3 pixels width independent of the current zoom factor set the stroke thickness to <code>3f / zoomFactor</code>.
    /// </remarks>
    public void Render(RenderContext context, int layer, Vector4 viewport, float zoomFactor)
    {
        foreach (var renderer in Renderers.Where(r => r.Layer == layer))
        {
            renderer.Render(context, viewport, zoomFactor);
        }
    }

    /// <summary>
    /// Adds a parameter set to the entity.
    /// </summary>
    /// <param name="parameterSet">The parameter set to add.</param>
    /// <returns>Returns the entity.</returns>
    public Entity WithParameterSet(IParameterSet parameterSet)
    {
        parameterSet.Entity = this;
        Parameters.Add(parameterSet);
        return this;
    }

    /// <summary>
    /// Gets a single parameter set.
    /// </summary>
    /// <typeparam name="T">Type or interface of the parameter set.</typeparam>
    /// <returns>Returns the parameter set. If no matching parameter set is found <c>null</c> is returned.
    /// If multiple matching parameter sets exist the 1st one is returned.</returns>
    public T? GetParameterSet<T>()
    {
        return Parameters.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets an enumeration of parameter sets.
    /// </summary>
    /// <typeparam name="T">Type or interface of the parameter set.</typeparam>
    /// <returns>Returns an enumeration of all matching parameter sets.</returns>
    public IEnumerable<T> GetParameterSets<T>()
    {
        return Parameters.OfType<T>();
    }

    /// <summary>
    /// Adds a controller to the entity.
    /// </summary>
    /// <param name="controller">The controller to add.</param>
    /// <returns>Returns the entity.</returns>
    public Entity WithController(IController controller)
    {
        controller.Entity = this;
        Controllers.Add(controller);
        return this;
    }

    /// <summary>
    /// Gets a single controller.
    /// </summary>
    /// <typeparam name="T">Type or interface of the controller.</typeparam>
    /// <returns>Returns the controller. If no matching controller is found <c>null</c> is returned.
    /// If multiple matching controllers exist the 1st one is returned.</returns>
    public T? GetController<T>()
    {
        return Controllers.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets an enumeration of controllers.
    /// </summary>
    /// <typeparam name="T">Type or interface of the controller.</typeparam>
    /// <returns>Returns an enumeration of all matching controllers.</returns>
    public IEnumerable<T> GetControllers<T>()
    {
        return Controllers.OfType<T>();
    }

    /// <summary>
    /// Adds a renderer to the entity.
    /// </summary>
    /// <param name="renderer">The renderer to add.</param>
    /// <returns>Returns the entity.</returns>
    public Entity WithRenderer(IRenderer renderer)
    {
        renderer.Entity = this;
        Renderers.Add(renderer);
        return this;
    }

    /// <summary>
    /// Gets a single renderer.
    /// </summary>
    /// <typeparam name="T">Type or interface of the renderer.</typeparam>
    /// <returns>Returns the renderer. If no matching renderer is found <c>null</c> is returned.
    /// If multiple matching renderers exist the 1st one is returned.</returns>
    public T? GetRenderer<T>()
    {
        return Renderers.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets an enumeration of renderers.
    /// </summary>
    /// <typeparam name="T">Type or interface of the renderer.</typeparam>
    /// <returns>Returns an enumeration of all matching renderers.</returns>
    public IEnumerable<T> GetRenderers<T>()
    {
        return Renderers.OfType<T>();
    }

    /// <summary>
    /// Adds a sensor to the entity.
    /// </summary>
    /// <param name="sensor">The sensor to add.</param>
    /// <returns>Returns the entity.</returns>
    public Entity WithSensor(ISensor sensor)
    {
        sensor.Entity = this;
        Sensors.Add(sensor);
        return this;
    }

    /// <summary>
    /// Gets a single sensor.
    /// </summary>
    /// <typeparam name="T">Type or interface of the sensor.</typeparam>
    /// <returns>Returns the sensor. If no matching sensor is found <c>null</c> is returned.
    /// If multiple matching sensors exist the 1st one is returned.</returns>
    public T? GetSensor<T>()
    {
        return Sensors.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets an enumeration of sensors.
    /// </summary>
    /// <typeparam name="T">Type or interface of the sensors.</typeparam>
    /// <returns>Returns an enumeration of all matching sensors.</returns>
    public IEnumerable<T> GetSensors<T>()
    {
        return Sensors.OfType<T>();
    }

    /// <summary>
    /// Adds a collider to the entity.
    /// </summary>
    /// <param name="collider">The collider to add.</param>
    /// <returns>Returns the entity.</returns>
    public Entity WithCollider(BaseCollider collider)
    {
        collider.Entity = this;
        Colliders.Add(collider);
        return this;
    }

    /// <summary>
    /// Gets a single collider.
    /// </summary>
    /// <typeparam name="T">Type or interface of the collider.</typeparam>
    /// <returns>Returns the collider. If no matching collider is found <c>null</c> is returned.
    /// If multiple matching colliders exist the 1st one is returned.</returns>
    public T? GetCollider<T>()
    {
        return Colliders.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets an enumeration of colliders.
    /// </summary>
    /// <typeparam name="T">Type or interface of the collider.</typeparam>
    /// <returns>Returns an enumeration of all matching colliders.</returns>
    public IEnumerable<T> GetColliders<T>()
    {
        return Colliders.OfType<T>();
    }

    /// <summary>
    /// Simulate the entity for 1 frame.
    /// </summary>
    /// <param name="simulationTime">Current time of the simulation in seconds.</param>
    /// <param name="timeDelta">Time delta between this and the last frame in seconds.</param>
    public void Simulate(double simulationTime, double timeDelta)
    {
        foreach (var sensor in Sensors)
        {
            sensor.Simulate(simulationTime, timeDelta);
        }

        foreach (var controller in Controllers)
        {
            controller.Simulate(simulationTime, timeDelta);
        }
    }
}
