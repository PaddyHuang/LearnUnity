using UnityEngine;

public class HighLighter : MonoBehaviour
{
    private MeshRenderer m_MeshRenderer;

    private Material[] coverMaterialGroup; //多一个覆盖材质
    private Material[] origGroup; //还原原来的材质

    private bool isHighlighting; //是否在高亮状态下

    void Start()
    {
        m_MeshRenderer = transform.GetComponent<MeshRenderer>();

        string shaderNamePath = "Legacy Shaders/Transparent/Diffuse";
        Shader find = Shader.Find(shaderNamePath);
        Material coverMaterial = new Material(find);

        Color flickerColor = new Color(24 / 255f, 152 / 255f, 214 / 255f, 128 / 255f);

        coverMaterial.color = flickerColor;
        int count = m_MeshRenderer.materials.Length + 1;
        origGroup = m_MeshRenderer.materials;

        coverMaterialGroup = new Material[count];
        for (int i = 0; i < origGroup.Length; i++)
            coverMaterialGroup[i] = origGroup[i];

        coverMaterialGroup[coverMaterialGroup.Length - 1] = coverMaterial;
    }

    /// <summary> 
    /// 开启高亮方法 </summary>
    public void SwitchHighLighting(Transform tr)
    {
        if (ReferenceEquals(m_MeshRenderer, null))
            return;
        if (transform == tr)
        {
            m_MeshRenderer.materials = coverMaterialGroup;
            isHighlighting = true;
        }
        else if (isHighlighting)
        {
            m_MeshRenderer.materials = origGroup;
            isHighlighting = false;
        }
    }
}