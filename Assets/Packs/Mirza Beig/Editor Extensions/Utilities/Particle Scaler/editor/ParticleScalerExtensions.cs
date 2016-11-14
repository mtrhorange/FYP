
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace EditorExtensions
    {

        namespace Utilities
        {

            // =================================	
            // Classes.
            // =================================

            public static class ParticleScalerExtensions
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                // =================================	
                // Functions.
                // =================================

                // Scales against self.

                public static void scale(this ParticleSystem particleSystem, float scale, bool scaleTransformLocalPosition)
                {
                    // Get as serialized object. Allows undo/redo.

                    SerializedObject particleSystemObject = new SerializedObject(particleSystem);

                    // Scale emitter.

                    particleSystemObject.FindProperty("InitialModule.startSize.scalar").floatValue *= scale;
                    particleSystemObject.FindProperty("InitialModule.startSpeed.scalar").floatValue *= scale;

                    particleSystemObject.FindProperty("InitialModule.gravityModifier").floatValue *= scale;

                    particleSystemObject.FindProperty("ShapeModule.radius").floatValue *= scale;

                    particleSystemObject.FindProperty("ShapeModule.angle").floatValue *= scale;
                    particleSystemObject.FindProperty("ShapeModule.length").floatValue *= scale;

                    particleSystemObject.FindProperty("ShapeModule.boxX").floatValue *= scale;
                    particleSystemObject.FindProperty("ShapeModule.boxY").floatValue *= scale;
                    particleSystemObject.FindProperty("ShapeModule.boxZ").floatValue *= scale;

                    particleSystemObject.FindProperty("VelocityModule.x.scalar").floatValue *= scale;
                    particleSystemObject.FindProperty("VelocityModule.y.scalar").floatValue *= scale;
                    particleSystemObject.FindProperty("VelocityModule.z.scalar").floatValue *= scale;

                    particleSystemObject.FindProperty("ForceModule.x.scalar").floatValue *= scale;
                    particleSystemObject.FindProperty("ForceModule.y.scalar").floatValue *= scale;
                    particleSystemObject.FindProperty("ForceModule.z.scalar").floatValue *= scale;

                    particleSystemObject.FindProperty("ClampVelocityModule.x.scalar").floatValue *= scale;
                    particleSystemObject.FindProperty("ClampVelocityModule.y.scalar").floatValue *= scale;
                    particleSystemObject.FindProperty("ClampVelocityModule.z.scalar").floatValue *= scale;

                    particleSystemObject.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= scale;
                    particleSystemObject.FindProperty("ClampVelocityModule.dampen").floatValue *= scale;

                    particleSystemObject.ApplyModifiedProperties();

                    if (scaleTransformLocalPosition)
                    {
                        SerializedObject transformObject = new SerializedObject(particleSystem.transform);

                        transformObject.FindProperty("m_LocalPosition").vector3Value *= scale;

                        transformObject.ApplyModifiedProperties();
                    }

                    // Scale active particles.

                    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
                    particleSystem.GetParticles(particles);

                    for (int i = 0; i < particles.Length; i++)
                    {
                        particles[i].startSize *= scale;
                        particles[i].velocity *= scale;
                        particles[i].position *= scale;
                    }

                    particleSystem.SetParticles(particles, particles.Length);
                }

                // Scales against reference values.

                public static void scale(

                    this ParticleSystem particleSystem,
                    ParticleScalerEditorWindow.ParticleScaleValues particleScaleValues, float scale, bool scaleTransformLocalPosition)
                {
                    SerializedObject particleSystemObject = new SerializedObject(particleSystem);

                    particleSystemObject.FindProperty("InitialModule.startSize.scalar").floatValue = particleScaleValues.initialModule_startSize_scalar * scale;
                    particleSystemObject.FindProperty("InitialModule.startSpeed.scalar").floatValue = particleScaleValues.initialModule_startSpeed_scalar * scale;

                    particleSystemObject.FindProperty("InitialModule.gravityModifier").floatValue = particleScaleValues.initialModule_gravityModifier * scale;

                    particleSystemObject.FindProperty("ShapeModule.radius").floatValue = particleScaleValues.shapeModule_radius * scale;

                    particleSystemObject.FindProperty("ShapeModule.angle").floatValue = particleScaleValues.shapeModule_angle * scale;
                    particleSystemObject.FindProperty("ShapeModule.length").floatValue = particleScaleValues.shapeModule_length * scale;

                    particleSystemObject.FindProperty("ShapeModule.boxX").floatValue = particleScaleValues.shapeModule_boxX * scale;
                    particleSystemObject.FindProperty("ShapeModule.boxY").floatValue = particleScaleValues.shapeModule_boxY * scale;
                    particleSystemObject.FindProperty("ShapeModule.boxZ").floatValue = particleScaleValues.shapeModule_boxZ * scale;

                    particleSystemObject.FindProperty("VelocityModule.x.scalar").floatValue = particleScaleValues.velocityModule_x_scalar * scale;
                    particleSystemObject.FindProperty("VelocityModule.y.scalar").floatValue = particleScaleValues.velocityModule_y_scalar * scale;
                    particleSystemObject.FindProperty("VelocityModule.z.scalar").floatValue = particleScaleValues.velocityModule_z_scalar * scale;

                    particleSystemObject.FindProperty("ForceModule.x.scalar").floatValue = particleScaleValues.forceModule_x_scalar * scale;
                    particleSystemObject.FindProperty("ForceModule.y.scalar").floatValue = particleScaleValues.forceModule_y_scalar * scale;
                    particleSystemObject.FindProperty("ForceModule.z.scalar").floatValue = particleScaleValues.forceModule_z_scalar * scale;

                    particleSystemObject.FindProperty("ClampVelocityModule.x.scalar").floatValue = particleScaleValues.clampVelocityModule_x_scalar * scale;
                    particleSystemObject.FindProperty("ClampVelocityModule.y.scalar").floatValue = particleScaleValues.clampVelocityModule_y_scalar * scale;
                    particleSystemObject.FindProperty("ClampVelocityModule.z.scalar").floatValue = particleScaleValues.clampVelocityModule_z_scalar * scale;

                    particleSystemObject.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue = particleScaleValues.clampVelocityModule_magnitude_scalar * scale;
                    particleSystemObject.FindProperty("ClampVelocityModule.dampen").floatValue = particleScaleValues.clampVelocityModule_magnitude_dampen * scale;

                    particleSystemObject.ApplyModifiedProperties();

                    if (scaleTransformLocalPosition)
                    {
                        SerializedObject transformObject = new SerializedObject(particleSystem.transform);

                        transformObject.FindProperty("m_LocalPosition").vector3Value = particleScaleValues.m_localPosition * scale;
                        transformObject.ApplyModifiedProperties();
                    }
                }

                // Load scale values into self.

                public static void loadScale(

                    this ParticleSystem particleSystem,
                    ParticleScalerEditorWindow.ParticleScaleValues particleScaleValues, bool scaleTransformLocalPosition)
                {
                    SerializedObject particleSystemObject = new SerializedObject(particleSystem);

                    particleSystemObject.FindProperty("InitialModule.startSize.scalar").floatValue = particleScaleValues.initialModule_startSize_scalar;
                    particleSystemObject.FindProperty("InitialModule.startSpeed.scalar").floatValue = particleScaleValues.initialModule_startSpeed_scalar;

                    particleSystemObject.FindProperty("InitialModule.gravityModifier").floatValue = particleScaleValues.initialModule_gravityModifier;

                    particleSystemObject.FindProperty("ShapeModule.radius").floatValue = particleScaleValues.shapeModule_radius;

                    particleSystemObject.FindProperty("ShapeModule.angle").floatValue = particleScaleValues.shapeModule_angle;
                    particleSystemObject.FindProperty("ShapeModule.length").floatValue = particleScaleValues.shapeModule_length;

                    particleSystemObject.FindProperty("ShapeModule.boxX").floatValue = particleScaleValues.shapeModule_boxX;
                    particleSystemObject.FindProperty("ShapeModule.boxY").floatValue = particleScaleValues.shapeModule_boxY;
                    particleSystemObject.FindProperty("ShapeModule.boxZ").floatValue = particleScaleValues.shapeModule_boxZ;

                    particleSystemObject.FindProperty("VelocityModule.x.scalar").floatValue = particleScaleValues.velocityModule_x_scalar;
                    particleSystemObject.FindProperty("VelocityModule.y.scalar").floatValue = particleScaleValues.velocityModule_y_scalar;
                    particleSystemObject.FindProperty("VelocityModule.z.scalar").floatValue = particleScaleValues.velocityModule_z_scalar;

                    particleSystemObject.FindProperty("ForceModule.x.scalar").floatValue = particleScaleValues.forceModule_x_scalar;
                    particleSystemObject.FindProperty("ForceModule.y.scalar").floatValue = particleScaleValues.forceModule_y_scalar;
                    particleSystemObject.FindProperty("ForceModule.z.scalar").floatValue = particleScaleValues.forceModule_z_scalar;

                    particleSystemObject.FindProperty("ClampVelocityModule.x.scalar").floatValue = particleScaleValues.clampVelocityModule_x_scalar;
                    particleSystemObject.FindProperty("ClampVelocityModule.y.scalar").floatValue = particleScaleValues.clampVelocityModule_y_scalar;
                    particleSystemObject.FindProperty("ClampVelocityModule.z.scalar").floatValue = particleScaleValues.clampVelocityModule_z_scalar;

                    particleSystemObject.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue = particleScaleValues.clampVelocityModule_magnitude_scalar;
                    particleSystemObject.FindProperty("ClampVelocityModule.dampen").floatValue = particleScaleValues.clampVelocityModule_magnitude_dampen;

                    particleSystemObject.ApplyModifiedProperties();

                    if (scaleTransformLocalPosition)
                    {
                        SerializedObject transformObject = new SerializedObject(particleSystem.transform);

                        transformObject.FindProperty("m_LocalPosition").vector3Value = particleScaleValues.m_localPosition;
                        transformObject.ApplyModifiedProperties();
                    }
                }

                // =================================	
                // End functions.
                // =================================

            }

            // =================================	
            // End namespace.
            // =================================

        }

    }

}

// =================================	
// --END-- //
// =================================
