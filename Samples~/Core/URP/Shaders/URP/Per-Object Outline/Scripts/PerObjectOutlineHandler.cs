/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250409

using Devloader.Extensions;

using UnityEngine;

namespace Devloader.Shaders
{
    public class PerObjectOutlineHandler : MonoBehaviour
    {
        [Header("Outline shader settings")]
        [SerializeField] Shader outlineShader;
        [SerializeField] Color outlineColor = Color.black;
        [SerializeField] float outlineFactor = .1f;

        [Header("Outline mesh settings")]
        [SerializeField] bool refreshMesh = true;
        [SerializeField] bool useFixedUpdate;

        // To bake mesh
        Renderer originalRenderer;

        // To display outline
        MeshFilter outlineFilter;
        MeshRenderer outlineRenderer;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!originalRenderer && !TryGetComponent(out originalRenderer))
                Debug.LogError("A renderer is required");

            if (!outlineShader)
                outlineShader = Shader.Find("Shader Graphs/Single Object Outline");

            if (enabled && originalRenderer)
            {
                ValidateOutlineComponents();

                UpdateOutline();

                if (refreshMesh)
                    UpdateMesh();
            }
        }
#endif

        private void Awake()
        {
            if (!originalRenderer && !TryGetComponent(out originalRenderer))
            {
                Debug.LogError("A renderer is required");
                enabled = false;
            }

            if (!outlineShader)
                outlineShader = Shader.Find("Shader Graphs/Single Object Outline");
        }

        private void OnEnable()
        {
            if (originalRenderer)
            {
                ValidateOutlineComponents();

                UpdateMesh();
                UpdateOutline();
            }
        }

        private void FixedUpdate()
        {
            if (!useFixedUpdate)
                return;

            if (refreshMesh)
                UpdateMesh();
        }

        private void LateUpdate()
        {
            if (useFixedUpdate)
                return;

            if (refreshMesh)
                UpdateMesh();
        }

        void UpdateMesh()
        {
            if (originalRenderer && outlineFilter && originalRenderer.GetType() == typeof(SkinnedMeshRenderer))
            {
                Mesh mesh = new Mesh();

#if UNITY_EDITOR
                (originalRenderer as SkinnedMeshRenderer).BakeMesh(mesh, true);
#else
                (originalRenderer as SkinnedMeshRenderer).BakeMesh(mesh, true);
#endif

                outlineFilter.sharedMesh = mesh;
            }
        }

        public void UpdateOutline()
        {
#if UNITY_EDITOR
            if(outlineRenderer.sharedMaterial)
            {
                outlineRenderer.sharedMaterial.color = outlineColor;
                outlineRenderer.sharedMaterial.SetFloat("_Factor", outlineFactor);
            }
            else
            {
                outlineRenderer.sharedMaterial = new Material(outlineShader);

                outlineRenderer.sharedMaterial.color = outlineColor;
                outlineRenderer.sharedMaterial.SetFloat("_Factor", outlineFactor);
            }
#else
            if(outlineRenderer.material)
            {
                outlineRenderer.material.color = outlineColor;
                outlineRenderer.material.SetFloat("_Factor", outlineFactor);
            }
            else
            {
                outlineRenderer.material = new Material(outlineShader);

                outlineRenderer.material.color = outlineColor;
                outlineRenderer.material.SetFloat("_Factor", outlineFactor);
            }
#endif
        }

        void ValidateOutlineComponents()
        {
            if (!outlineRenderer)
            {
                if (outlineFilter)
                    outlineRenderer = outlineFilter.ValidateComponent<MeshRenderer>();
                else
                {
                    if (!this.TryFindComponent(out outlineRenderer))
                        outlineRenderer = ComponentExtension.InstantiateObject<MeshRenderer>(transform);

                    outlineFilter = outlineRenderer.ValidateComponent<MeshFilter>();
                }
            }
            else if (!outlineFilter)
                outlineFilter = outlineRenderer.ValidateComponent<MeshFilter>();
        }

        public Color Color
        {
            get => outlineColor;
            set
            {
                outlineColor = value;

#if UNITY_EDITOR
                if (outlineRenderer.sharedMaterial)
                    outlineRenderer.sharedMaterial.color = outlineColor;
#else
                if(outlineRenderer.material)
                    outlineRenderer.material.color = outlineColor;
#endif
            }
        }

        public float Factor
        {
            get => outlineFactor;
            set
            {
                outlineFactor = value;

#if UNITY_EDITOR
                if(outlineRenderer.sharedMaterial)
                    outlineRenderer.sharedMaterial.SetFloat("_Factor", outlineFactor);
#else
                if(outlineRenderer.material)
                    outlineRenderer.material.SetFloat("_Factor", outlineFactor);
#endif
            }
        }
    }
}