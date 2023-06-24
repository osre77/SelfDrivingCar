using SelfDrivingCar.Core.Colliders;
using SelfDrivingCar.Core.Controller;
using SelfDrivingCar.Core.Rendering;
using SelfDrivingCar.Core.Sensors;
using System.Numerics;

namespace SelfDrivingCar.Core;

public class Entity
{
    public EntityGraph? Graph { get; set; }

    public Vector2 Position { get; set; }

    public float Angle { get; set; }

    public IList<BaseRenderer> Renderers { get; } = new List<BaseRenderer>();

    public IList<BaseController> Controllers { get; } = new List<BaseController>();

    public IList<BaseSensor> Sensors { get; } = new List<BaseSensor>();

    public IList<BaseCollider> Colliders { get; } = new List<BaseCollider>();

    public Entity()
    { }

    public Entity(Vector2 position, float angle)
    {
        Position = position;
        Angle = angle;
    }

    public void Render(RenderContext context, int layer, Vector4 viewport, float zoomFactor)
    {
        foreach (var renderer in Renderers.Where(r => r.Layer == layer))
        {
            renderer.Render(context, viewport, zoomFactor);
        }
    }

    public Entity WithController(BaseController controller)
    {
        controller.Entity = this;
        Controllers.Add(controller);
        return this;
    }

    public T? GetController<T>() where T : BaseController
    {
        return Controllers.OfType<T>().FirstOrDefault();
    }

    public IEnumerable<T> GetControllers<T>() where T : BaseController
    {
        return Controllers.OfType<T>();
    }

    public Entity WithRenderer(BaseRenderer renderer)
    {
        renderer.Entity = this;
        Renderers.Add(renderer);
        return this;
    }

    public T? GetRenderer<T>() where T : BaseRenderer
    {
        return Renderers.OfType<T>().FirstOrDefault();
    }

    public IEnumerable<T> GetRenderers<T>() where T : BaseRenderer
    {
        return Renderers.OfType<T>();
    }

    public Entity WithSensor(BaseSensor sensor)
    {
        sensor.Entity = this;
        Sensors.Add(sensor);
        return this;
    }

    public T? GetSensor<T>() where T : BaseSensor
    {
        return Sensors.OfType<T>().FirstOrDefault();
    }

    public IEnumerable<T> GetSensors<T>() where T : BaseSensor
    {
        return Sensors.OfType<T>();
    }

    public Entity WithCollider(BaseCollider collider)
    {
        collider.Entity = this;
        Colliders.Add(collider);
        return this;
    }

    public T? GetCollider<T>() where T : BaseCollider
    {
        return Colliders.OfType<T>().FirstOrDefault();
    }

    public IEnumerable<T> GetColliders<T>() where T : BaseCollider
    {
        return Colliders.OfType<T>();
    }

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
