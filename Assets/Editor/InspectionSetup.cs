using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public static class InspectionSetup
{
    // Keep the controller and the preview environment under one root object.
    [MenuItem("Tools/Inspection/Configure current scene")]
    public static void ConfigureCurrentScene()
    {
        InspectionController controller = Object.FindFirstObjectByType<InspectionController>();
        if (controller == null)
            throw new System.InvalidOperationException("The scene needs an InspectionController.");

        SerializedObject settings = new SerializedObject(controller);
        if (settings.FindProperty("inspectionCamera").objectReferenceValue != null)
        {
            ConsolidateInspectionRoot(controller, (Camera)settings.FindProperty("inspectionCamera").objectReferenceValue);
            Debug.Log("Inspection is already configured under InspectionSystem.", controller);
            return;
        }

        int layer = EnsureInspectionLayer();
        int mask = 1 << layer;
        Camera playerCamera = (Camera)settings.FindProperty("playerCamera").objectReferenceValue;
        GameObject panel = (GameObject)settings.FindProperty("inspectionPanel").objectReferenceValue;
        Transform pivot = (Transform)settings.FindProperty("modelPivot").objectReferenceValue;

        GameObject stage = controller.gameObject;
        Undo.RecordObject(stage, "Configure inspection system");
        Undo.RecordObject(stage.transform, "Position inspection system");
        stage.name = "InspectionSystem";
        stage.transform.position = new Vector3(0f, -1000f, 0f);
        stage.layer = layer;

        GameObject cameraObject = new GameObject("InspectionCamera", typeof(Camera), typeof(HDAdditionalCameraData));
        cameraObject.transform.SetParent(stage.transform, false);
        cameraObject.layer = layer;
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.enabled = false;
        camera.cullingMask = mask;
        camera.fieldOfView = 60f;
        camera.nearClipPlane = 0.01f;
        camera.farClipPlane = 10f;
        camera.useOcclusionCulling = false;
        HDAdditionalCameraData cameraData = cameraObject.GetComponent<HDAdditionalCameraData>();
        cameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;
        cameraData.backgroundColorHDR = new Color(0.025f, 0.025f, 0.03f, 1f);
        cameraData.volumeLayerMask = mask;
        cameraData.antialiasing = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;

        Undo.RecordObject(pivot, "Move inspection pivot");
        Undo.SetTransformParent(pivot, stage.transform, "Move inspection pivot");
        pivot.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0.7f), Quaternion.identity);
        pivot.localScale = Vector3.one;
        pivot.gameObject.layer = layer;

        // Punctual lights have a short range, so they cannot light the playable scene.
        CreateLight(stage.transform, "Inspection Key", new Vector3(-0.8f, 0.8f, -0.4f), 12000f);
        CreateLight(stage.transform, "Inspection Fill", new Vector3(0.8f, -0.2f, 0f), 6000f);
        Volume volume = stage.GetComponent<Volume>();
        if (volume == null)
            volume = Undo.AddComponent<Volume>(stage);
        Undo.RecordObject(volume, "Configure inspection volume");
        volume.isGlobal = true;
        volume.sharedProfile = GetVolumeProfile();

        Undo.RecordObject(playerCamera, "Exclude inspection layer");
        playerCamera.cullingMask &= ~mask;
        PrefabUtility.RecordPrefabInstancePropertyModifications(playerCamera);

        Canvas canvas = panel.GetComponentInParent<Canvas>();
        Undo.RecordObject(canvas, "Overlay inspection panel");
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = null;
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        Undo.RecordObject(panelRect, "Stretch inspection panel");
        Stretch(panelRect);

        Transform background = panel.transform.Find("Background") ?? panel.transform.Find("BackGround");
        if (background != null)
        {
            Undo.RecordObject(background.GetComponent<RectTransform>(), "Stretch background");
            Stretch(background.GetComponent<RectTransform>());
            Image backgroundImage = background.GetComponent<Image>();
            Undo.RecordObject(backgroundImage, "Dark inspection background");
            backgroundImage.color = new Color(0.025f, 0.025f, 0.03f, 1f);
        }

        GameObject imageObject = new GameObject("InspectionPreview", typeof(RectTransform), typeof(RawImage), typeof(AspectRatioFitter));
        Undo.RegisterCreatedObjectUndo(imageObject, "Create inspection preview");
        imageObject.transform.SetParent(panel.transform, false);
        imageObject.layer = 5;
        RawImage image = imageObject.GetComponent<RawImage>();
        image.raycastTarget = false;
        AspectRatioFitter fitter = imageObject.GetComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        fitter.aspectRatio = 1f;

        settings.FindProperty("inspectionCamera").objectReferenceValue = camera;
        settings.FindProperty("inspectionImage").objectReferenceValue = image;
        settings.ApplyModifiedProperties();
        Undo.RecordObject(panel, "Hide inspection panel");
        panel.SetActive(false);
        EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);
        Debug.Log("Inspection configured: preview camera, RawImage, lights and HDRP volume under InspectionSystem.", controller);
    }

    private static void ConsolidateInspectionRoot(InspectionController controller, Camera camera)
    {
        GameObject system = controller.gameObject;
        Transform stage = camera.transform.parent;
        if (stage == system.transform)
        {
            Undo.RecordObject(system, "Name inspection system");
            system.name = "InspectionSystem";
            return;
        }

        Volume stageVolume = stage != null ? stage.GetComponent<Volume>() : null;
        if (stageVolume == null || stage.name != "InspectionStage")
            throw new System.InvalidOperationException("The preview camera must belong to InspectionStage or InspectionSystem.");

        Undo.RecordObject(system, "Merge inspection system");
        Undo.RecordObject(system.transform, "Position inspection system");
        system.name = "InspectionSystem";
        system.layer = stage.gameObject.layer;
        system.transform.SetPositionAndRotation(stage.position, stage.rotation);
        system.transform.localScale = stage.localScale;

        Volume systemVolume = system.GetComponent<Volume>();
        if (systemVolume == null)
            systemVolume = Undo.AddComponent<Volume>(system);
        Undo.RecordObject(systemVolume, "Merge inspection volume");
        systemVolume.enabled = stageVolume.enabled;
        systemVolume.isGlobal = stageVolume.isGlobal;
        systemVolume.priority = stageVolume.priority;
        systemVolume.blendDistance = stageVolume.blendDistance;
        systemVolume.weight = stageVolume.weight;
        systemVolume.sharedProfile = stageVolume.sharedProfile;

        while (stage.childCount > 0)
            Undo.SetTransformParent(stage.GetChild(0), system.transform, "Move inspection child");
        Undo.DestroyObjectImmediate(stage.gameObject);
        EditorSceneManager.MarkSceneDirty(system.scene);
    }

    public static void ConfigureProjectScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/OutdoorsScene.unity");
        ConfigureCurrentScene();
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
    }

    private static int EnsureInspectionLayer()
    {
        int layer = LayerMask.NameToLayer("Inspection");
        if (layer >= 0)
            return layer;

        SerializedObject tags = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tags.FindProperty("layers");
        for (int i = 8; i < 32; i++)
        {
            if (!string.IsNullOrEmpty(layers.GetArrayElementAtIndex(i).stringValue))
                continue;
            layers.GetArrayElementAtIndex(i).stringValue = "Inspection";
            tags.ApplyModifiedProperties();
            return i;
        }
        throw new System.InvalidOperationException("No free layer is available for Inspection.");
    }

    private static VolumeProfile GetVolumeProfile()
    {
        const string path = "Assets/Settings/InspectionVolume.asset";
        VolumeProfile profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
        if (profile != null)
            return profile;

        profile = ScriptableObject.CreateInstance<VolumeProfile>();
        AssetDatabase.CreateAsset(profile, path);
        Exposure exposure = profile.Add<Exposure>();
        exposure.mode.Override(ExposureMode.Fixed);
        exposure.fixedExposure.Override(12f);
        Fog fog = profile.Add<Fog>();
        fog.enabled.Override(false);
        foreach (VolumeComponent component in profile.components)
            AssetDatabase.AddObjectToAsset(component, profile);
        EditorUtility.SetDirty(profile);
        return profile;
    }

    private static void CreateLight(Transform parent, string name, Vector3 position, float intensity)
    {
        GameObject lightObject = new GameObject(name, typeof(Light), typeof(HDAdditionalLightData));
        lightObject.transform.SetParent(parent, false);
        lightObject.transform.localPosition = position;
        Light light = lightObject.GetComponent<Light>();
        light.type = LightType.Point;
        light.lightUnit = LightUnit.Candela;
        light.intensity = intensity;
        light.range = 5f;
        light.shadows = LightShadows.None;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
