﻿using SelfDrivingCar.Core.Sensors;
using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.Extensions;

public static class EntityExtensions
{
    public static Entity WithCarSensorPattern1(this Entity entity, out int sensorCount)
    {
        sensorCount = 5;
        return entity
            .WithSensor(new DistanceSensor(new Vector2(0f, 2.0f), CarMath.DegToRad(0f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(0.9f, 2.0f), CarMath.DegToRad(22.5f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(-0.9f, 2.0f), CarMath.DegToRad(-22.5f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(0.9f, 1.7f), CarMath.DegToRad(45f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(-0.9f, 1.7f), CarMath.DegToRad(-45f), 8f));
    }
    public static Entity WithCarSensorPattern2(this Entity entity, out int sensorCount)
    {
        sensorCount = 9;
        return entity
            .WithSensor(new DistanceSensor(new Vector2(0f, 2.0f), CarMath.DegToRad(0f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(0.9f, 2.0f), CarMath.DegToRad(20f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(-0.9f, 2.0f), CarMath.DegToRad(-20f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(0.9f, 1.7f), CarMath.DegToRad(45f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(-0.9f, 1.7f), CarMath.DegToRad(-45f), 8f))
            .WithSensor(new DistanceSensor(new Vector2(0.9f, 1.5f), CarMath.DegToRad(90f), 3f))
            .WithSensor(new DistanceSensor(new Vector2(-0.9f, 1.5f), CarMath.DegToRad(-90f), 3f))
            .WithSensor(new DistanceSensor(new Vector2(0.9f, -1.5f), CarMath.DegToRad(90f), 3f))
            .WithSensor(new DistanceSensor(new Vector2(-0.9f, -1.5f), CarMath.DegToRad(-90f), 3f));
    }
}
