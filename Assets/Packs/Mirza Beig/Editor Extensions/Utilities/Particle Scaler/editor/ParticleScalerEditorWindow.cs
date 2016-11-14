
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Reflection;

using System.Collections;
using System.Collections.Generic;

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

            //[CustomEditor(typeof(ParticlePrefab))]
            //[CustomEditor(typeof(Transform))]
            //[CustomEditor(typeof(ParticleSystem))]
            //[CanEditMultipleObjects]

            public class ParticleScalerEditorWindow : EditorWindow
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // All values in the editor affect scale.

                public class ParticleScaleValues
                {
                    // ...

                    public ParticleScaleValues(ParticleSystem particleSystem)
                    {
                        save(particleSystem);
                    }

                    // Save to internal values from ParticleSystem component.

                    public void save(ParticleSystem particleSystem)
                    {
                        SerializedObject particleSystemObject = new SerializedObject(particleSystem);

                        initialModule_startSize_scalar = particleSystemObject.FindProperty("InitialModule.startSize.scalar").floatValue;
                        initialModule_startSpeed_scalar = particleSystemObject.FindProperty("InitialModule.startSpeed.scalar").floatValue;

                        initialModule_gravityModifier = particleSystemObject.FindProperty("InitialModule.gravityModifier").floatValue;

                        shapeModule_radius = particleSystemObject.FindProperty("ShapeModule.radius").floatValue;

                        shapeModule_angle = particleSystemObject.FindProperty("ShapeModule.angle").floatValue;
                        shapeModule_length = particleSystemObject.FindProperty("ShapeModule.length").floatValue;

                        shapeModule_boxX = particleSystemObject.FindProperty("ShapeModule.boxX").floatValue;
                        shapeModule_boxY = particleSystemObject.FindProperty("ShapeModule.boxY").floatValue;
                        shapeModule_boxZ = particleSystemObject.FindProperty("ShapeModule.boxZ").floatValue;

                        velocityModule_x_scalar = particleSystemObject.FindProperty("VelocityModule.x.scalar").floatValue;
                        velocityModule_y_scalar = particleSystemObject.FindProperty("VelocityModule.y.scalar").floatValue;
                        velocityModule_z_scalar = particleSystemObject.FindProperty("VelocityModule.z.scalar").floatValue;

                        forceModule_x_scalar = particleSystemObject.FindProperty("ForceModule.x.scalar").floatValue;
                        forceModule_y_scalar = particleSystemObject.FindProperty("ForceModule.y.scalar").floatValue;
                        forceModule_z_scalar = particleSystemObject.FindProperty("ForceModule.z.scalar").floatValue;

                        clampVelocityModule_x_scalar = particleSystemObject.FindProperty("ClampVelocityModule.x.scalar").floatValue;
                        clampVelocityModule_y_scalar = particleSystemObject.FindProperty("ClampVelocityModule.y.scalar").floatValue;
                        clampVelocityModule_z_scalar = particleSystemObject.FindProperty("ClampVelocityModule.z.scalar").floatValue;

                        clampVelocityModule_magnitude_scalar = particleSystemObject.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue;
                        clampVelocityModule_magnitude_dampen = particleSystemObject.FindProperty("ClampVelocityModule.dampen").floatValue;

                        SerializedObject transformObject = new SerializedObject(particleSystem.transform);

                        m_localPosition = transformObject.FindProperty("m_LocalPosition").vector3Value;
                        transformObject.ApplyModifiedProperties();
                    }

                    // ...

                    public float initialModule_startSize_scalar;
                    public float initialModule_startSpeed_scalar;

                    public float initialModule_gravityModifier;

                    public float shapeModule_radius;

                    public float shapeModule_angle;
                    public float shapeModule_length;

                    public float shapeModule_boxX;
                    public float shapeModule_boxY;
                    public float shapeModule_boxZ;

                    public float velocityModule_x_scalar;
                    public float velocityModule_y_scalar;
                    public float velocityModule_z_scalar;

                    public float forceModule_x_scalar;
                    public float forceModule_y_scalar;
                    public float forceModule_z_scalar;

                    public float clampVelocityModule_x_scalar;
                    public float clampVelocityModule_y_scalar;
                    public float clampVelocityModule_z_scalar;

                    public float clampVelocityModule_magnitude_scalar;
                    public float clampVelocityModule_magnitude_dampen;

                    public Vector3 m_localPosition;
                }

                // =================================	
                // Variables.
                // =================================

                // Scale to apply.

                float scale = 1.0f;

                // Also scale transform local position?

                bool scaleTransformLocalPosition = true;

                // Scale automatically when moving the slider?

                bool realtimeScaling = false;

                // Button sizes.

                float stateButtonHeight = 25.0f;
                float scalePresetButtonHeight = 25.0f;
                float applyCustomScaleButtonHeight = 35.0f;

                float debugButtonHeight = 25.0f;
                float moduleScalePresetButtonHeight = 25.0f;

                // Playback.

                ParticlePlayback particlePlayback = new ParticlePlayback();

                // Selected objects in editor and all the particle systems components.

                List<GameObject> selectedGameObjectsWithParticleSystems = new List<GameObject>();

                // Particle systems and their scale values.

                // I also keep last frame's particle systems because I update
                // the list of particle systems on update. So clearing particles
                // inside the systems may not do anything as the particles are
                // updated and the list set to a length of zero before OnSelectionChange.

                List<ParticleSystem> particleSystems = new List<ParticleSystem>();
                List<ParticleSystem> particleSystemsFromLastFrame = new List<ParticleSystem>();

                List<ParticleScaleValues> particleSystemScaleValues = new List<ParticleScaleValues>();

                // For labeling and tooltips.

                GUIContent guiContentLabel;

                // =================================	
                // Functions.
                // =================================

                // ...

                [MenuItem("Window/Mirza Beig/Particle Scaler")]
                static void showEditor()
                {
                    ParticleScalerEditorWindow window =
                        EditorWindow.GetWindow<ParticleScalerEditorWindow>(false, "Mirza Beig - Particle Scaler");

                    // Static init.

                    // ...

                    // Invoke non-static init.

                    window.initialize();

                    // Do a first check.

                    window.OnSelectionChange();
                }

                // Initialize.

                void initialize()
                {

                }

                // ...

                void OnSelectionChange()
                {
                    // Clear if set to clear on selection change.

                    if (particlePlayback.clearParticlesOnSelectionChange)
                    {
                        ParticleEditorUtility.clearParticles(particleSystems);
                        ParticleEditorUtility.clearParticles(particleSystemsFromLastFrame);

                        particlePlayback.repaintEditorCameraWindows();
                    }

                    // Pause all selected particles.

                    else if (!Application.isPlaying)
                    {
                        particlePlayback.pause(particleSystems);
                    }

                    // Clear for updated selection.

                    particleSystemScaleValues.Clear();
                    realtimeScaling = false;

                    // (Re-)verify current list of particles.

                    ParticleEditorUtility.getSelectedParticleSystems(ref particleSystems, ref selectedGameObjectsWithParticleSystems);

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystemScaleValues.Add(new ParticleScaleValues(particleSystems[i]));
                    }

                    // Nothing selected. Undo any changes.

                    if (particleSystems.Count == 0 && realtimeScaling)
                    {
                        loadFromParticleScaleValues();
                    }
                }

                // ...

                void applyScaleToAll()
                {
                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystems[i].scale(scale, scaleTransformLocalPosition);
                        particlePlayback.loopback(particleSystems[i]);
                    }
                }
                void applyScaleToAll(float scale)
                {
                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystems[i].scale(scale, scaleTransformLocalPosition);
                        particlePlayback.loopback(particleSystems[i]);
                    }
                }

                // ...

                public void saveParticleScaleValues()
                {
                    scale = 1.0f;

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystemScaleValues[i] = new ParticleScaleValues(particleSystems[i]);
                    }
                }

                // ...

                public void loadFromParticleScaleValues()
                {
                    scale = 1.0f;

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystems[i].loadScale(particleSystemScaleValues[i], scaleTransformLocalPosition);
                        particleSystems[i].setPlaybackPosition(particlePlayback.particlePlaybackPosition);
                    }
                }

                // ...

                void scaleColour(float scale)
                {
                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        //particleSystems[i].startColor *= scale;

                        SerializedObject particleSystemObject = new SerializedObject(particleSystems[i]);

                        SerializedProperty psys_prop_min = particleSystemObject.FindProperty("InitialModule.startColor.minColor");
                        SerializedProperty psys_prop_max = particleSystemObject.FindProperty("InitialModule.startColor.maxColor");

                        Color psys_min = psys_prop_min.colorValue;
                        Color psys_max = psys_prop_max.colorValue;

                        psys_min *= scale;
                        psys_max *= scale;

                        psys_prop_min.colorValue = psys_min;
                        psys_prop_max.colorValue = psys_max;

                        particleSystemObject.ApplyModifiedProperties();
                    }
                }

                // ...

                void scaleLifetime(float scale)
                {
                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        SerializedObject particleSystemObject = new SerializedObject(particleSystems[i]);

                        // Base.

                        SerializedProperty psys_prop_base_duration = particleSystemObject.FindProperty("lengthInSec");
                        SerializedProperty psys_prop_base_startDelay = particleSystemObject.FindProperty("startDelay");

                        psys_prop_base_duration.floatValue *= scale;
                        psys_prop_base_startDelay.floatValue *= scale;

                        // Initial.

                        SerializedProperty psys_prop_initial_startLifetime_scalar = particleSystemObject.FindProperty("InitialModule.startLifetime.scalar");
                        psys_prop_initial_startLifetime_scalar.floatValue *= scale;

                        // Emission.

                        SerializedProperty psys_prop_emission_rate_scalar = particleSystemObject.FindProperty("EmissionModule.rate.scalar");
                        SerializedProperty psys_prop_emission_burstCount = particleSystemObject.FindProperty("EmissionModule.m_BurstCount");

                        psys_prop_emission_rate_scalar.floatValue /= scale; // Opposite to scale.

                        for (int j = 0; j < psys_prop_emission_burstCount.intValue; j++)
                        {
                            SerializedProperty psys_prop_emission_cntNum = particleSystemObject.FindProperty("EmissionModule.cnt" + j);
                            SerializedProperty psys_prop_emission_timeNum = particleSystemObject.FindProperty("EmissionModule.time" + j);

                            psys_prop_emission_cntNum.intValue = Mathf.CeilToInt(psys_prop_emission_cntNum.intValue * scale);
                            psys_prop_emission_timeNum.floatValue *= scale;
                        }

                        // Apply.

                        particleSystemObject.ApplyModifiedProperties();
                    }
                }

                // ...

                void scalePlaybackSpeed(float scale)
                {
                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystems[i].playbackSpeed *= scale;
                    }
                }

                // ...

                void OnGUI()
                {
                    // Get windows.

                    particlePlayback.updateEditorCameraWindowReferences();

                    // Looks nicer.

                    EditorGUILayout.Separator();

                    // Debug settings.

                    EditorGUILayout.LabelField("- Debug Settings:", EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    {
                        // Print all properties of the ParticleSystem component (names).

                        guiContentLabel = new GUIContent("Print ParticleSystem Properties",
                        "Print all properties of the ParticleSystem component.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(debugButtonHeight)))
                        {
                            //PropertyInfo[] properties = typeof(ParticleSystem).GetProperties();

                            //for (int i = 0; i < properties.Length; i++)
                            //{
                            //    Debug.Log(properties[i].Name);
                            //}

                            if (particleSystems.Count != 0)
                            {
                                SerializedObject particleSystemObject = new SerializedObject(particleSystems[0]);
                                SerializedProperty particleSystemProperty = particleSystemObject.GetIterator();

                                particleSystemProperty = particleSystemObject.FindProperty("EmissionModule");

                                //Debug.Log(particleSystemProperty.name);
                                //particleSystemProperty.Next(true);

                                do
                                {
                                    Debug.Log(particleSystemProperty.name);
                                }
                                while (particleSystemProperty.Next(true));
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    // Scale settings.

                    EditorGUILayout.LabelField("- Scale Settings:", EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    EditorGUI.BeginChangeCheck();
                    {
                        guiContentLabel = new GUIContent("Scale", "ParticleSystem component scale factor.");
                        scale = EditorGUILayout.Slider(guiContentLabel, scale, 0.5f, 2.0f);
                    }
                    if (EditorGUI.EndChangeCheck() && realtimeScaling)
                    {
                        for (int i = 0; i < particleSystems.Count; i++)
                        {
                            // Scale particles. Note that particles already on-screen like this
                            // will not scale. So force a restart with current values after setting scale.

                            particleSystems[i].scale(particleSystemScaleValues[i], scale, scaleTransformLocalPosition);
                            particleSystems[i].setPlaybackPosition(particlePlayback.particlePlaybackPosition);
                        }
                    }

                    EditorGUILayout.Separator();

                    // Extension options.

                    guiContentLabel = new GUIContent("Scale Transform Position", "Scale local position in Transform component.");
                    scaleTransformLocalPosition = EditorGUILayout.Toggle(guiContentLabel, scaleTransformLocalPosition);

                    EditorGUILayout.Separator();

                    GUI.enabled = particleSystems.Count != 0;

                    // Save scale state of selected.

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (realtimeScaling)
                        {
                            guiContentLabel = new GUIContent("Real-Time Scaling [CURRENTLY: ON]", "Automatically scales selection with the slider.");
                        }
                        else
                        {
                            guiContentLabel = new GUIContent("Real-Time Scaling [CURRENTLY: OFF]", "Scaling requires one of the apply buttons to be clicked.");
                        }

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(stateButtonHeight)))
                        {
                            realtimeScaling = !realtimeScaling;

                            if (realtimeScaling)
                            {
                                saveParticleScaleValues();
                            }
                            else
                            {
                                loadFromParticleScaleValues();
                            }
                        }

                        //guiContentLabel = new GUIContent("Restore",
                        //    "Restore scale before modifications to the slider.");

                        //if (GUILayout.Button(guiContentLabel, GUILayout.Height(stateButtonHeight)))
                        //{
                        //    loadFromParticleScaleValues();
                        //}
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    // Buttons for quick-apply scale factor presets.

                    EditorGUILayout.BeginHorizontal();
                    {
                        guiContentLabel = new GUIContent("Scale x0.5",
                            "Apply a preset scale factor of x0.5.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(scalePresetButtonHeight)))
                        {
                            applyScaleToAll(0.5f);

                            if (realtimeScaling)
                            {
                                saveParticleScaleValues();
                            }
                        }

                        guiContentLabel = new GUIContent("Scale x0.75",
                            "Apply a preset scale factor of x0.75.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(scalePresetButtonHeight)))
                        {
                            applyScaleToAll(0.75f);

                            if (realtimeScaling)
                            {
                                saveParticleScaleValues();
                            }
                        }

                        guiContentLabel = new GUIContent("Scale x1.5",
                            "Apply a preset scale factor of x1.5.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(scalePresetButtonHeight)))
                        {
                            applyScaleToAll(1.5f);

                            if (realtimeScaling)
                            {
                                saveParticleScaleValues();
                            }
                        }

                        guiContentLabel = new GUIContent("Scale x2.0",
                            "Apply a preset scale factor of x2.0.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(scalePresetButtonHeight)))
                        {
                            applyScaleToAll(2.0f);

                            if (realtimeScaling)
                            {
                                saveParticleScaleValues();
                            }
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                    //EditorGUILayout.Separator();

                    // Button to apply.

                    guiContentLabel = new GUIContent("Apply Scale",
                        "Apply custom scaling factor to all ParticleSystem components in select GameObjects.");

                    if (GUILayout.Button(guiContentLabel, GUILayout.Height(applyCustomScaleButtonHeight)))
                    {
                        if (!realtimeScaling)
                        {
                            applyScaleToAll();
                        }
                        else
                        {
                            saveParticleScaleValues();
                        }
                    }

                    EditorGUILayout.Separator();

                    // Module settings.

                    EditorGUILayout.LabelField("- Module Settings:", EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    {
                        // Colour.

                        guiContentLabel = new GUIContent("Colour x0.5",
                        "Apply custom colour scaling factor to all ParticleSystem components in select GameObjects.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(moduleScalePresetButtonHeight)))
                        {
                            scaleColour(0.5f);
                        }

                        guiContentLabel = new GUIContent("Colour x2.0",
                            "Apply custom colour scaling factor to all ParticleSystem components in select GameObjects.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(moduleScalePresetButtonHeight)))
                        {
                            scaleColour(2.0f);
                        }

                        // Lifetime.

                        guiContentLabel = new GUIContent("Lifetime x0.5",
                        "Apply custom lifetime scaling factor to all ParticleSystem components in select GameObjects.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(moduleScalePresetButtonHeight)))
                        {
                            scaleLifetime(0.5f);
                        }

                        guiContentLabel = new GUIContent("Lifetime x2.0",
                            "Apply custom lifetime scaling factor to all ParticleSystem components in select GameObjects.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(moduleScalePresetButtonHeight)))
                        {
                            scaleLifetime(2.0f);
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        // Playback speed.

                        guiContentLabel = new GUIContent("Playback Speed x0.5",
                        "Apply custom playback speed scaling factor to all ParticleSystem components in select GameObjects.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(moduleScalePresetButtonHeight)))
                        {
                            scalePlaybackSpeed(0.5f);
                        }

                        guiContentLabel = new GUIContent("Playback Speed x2.0",
                            "Apply custom playback speed scaling factor to all ParticleSystem components in select GameObjects.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(moduleScalePresetButtonHeight)))
                        {
                            scalePlaybackSpeed(2.0f);
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    GUI.enabled = true;

                    EditorGUILayout.Separator();

                    // Playback settings.

                    particlePlayback.GUIPlaybackSettings(particleSystems);

                    EditorGUILayout.Separator();

                    // Selected objects.

                    particlePlayback.GUIParticleSelection(selectedGameObjectsWithParticleSystems);
                }

                // ...

                void OnInspectorUpdate()
                {
                    Repaint();
                }

                // ...

                void Update()
                {
                    // (Re-)verify current list of particles.

                    particleSystemsFromLastFrame =
                        new List<ParticleSystem>(particleSystems);

                    ParticleEditorUtility.getSelectedParticleSystems(
                        ref particleSystems, ref selectedGameObjectsWithParticleSystems);

                    // Nothing selected. Undo any changes.

                    if (particleSystems.Count == 0 && realtimeScaling)
                    {
                        loadFromParticleScaleValues();
                    }

                    particlePlayback.update(particleSystems);
                }

                // ...

                void OnFocus()
                {
                    // (Re-)verify current list of particles.

                    ParticleEditorUtility.getSelectedParticleSystems(
                        ref particleSystems, ref selectedGameObjectsWithParticleSystems);

                    // Nothing selected. Undo any changes.

                    if (particleSystems.Count == 0 && realtimeScaling)
                    {
                        loadFromParticleScaleValues();
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
