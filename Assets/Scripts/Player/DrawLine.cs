using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public int totalXPixels = 1024;  // ������ ��������
    public int totalYPixels = 1024;  // ������ ��������
    private Color[] colorMap;  // ������ ������ ��� ��������
    private Texture2D generatedTexture;  // ��������, ������� ����� ���������
    private Renderer objectRenderer;

    void Start()
    {
        // ������� ������ ������ � ���� ��������
        colorMap = new Color[totalXPixels * totalYPixels];
        generatedTexture = new Texture2D(totalXPixels, totalYPixels, TextureFormat.RGBA32, false);

        // �������� ��������� Renderer �������
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.mainTexture = generatedTexture;  // ����������� �������� � ��������� �������
    }

    void Update()
    {
        // �������� ������� ������� (X, Z)
        Vector3 objectPosition = transform.position;

        // ����������� ������� ������� ������� � ���������� ��������
        float normalizedX = Mathf.InverseLerp(-10f, 10f, objectPosition.x);  // ����������� ���������� X
        float normalizedY = Mathf.InverseLerp(-10f, 10f, objectPosition.z);  // ����������� ���������� Y

        // ��������� ��������������� �������� � ������� ��������
        int xPixel = Mathf.FloorToInt(normalizedX * totalXPixels);
        int yPixel = Mathf.FloorToInt(normalizedY * totalYPixels);

        // ������������ ����������, ����� ��� �� �������� �� ������� ��������
        xPixel = Mathf.Clamp(xPixel, 0, totalXPixels - 1);
        yPixel = Mathf.Clamp(yPixel, 0, totalYPixels - 1);

        // ������ ���� ������� (��������, �������)
        colorMap[yPixel * totalXPixels + xPixel] = Color.red;

        // ��������� ��������, �������� ���������
        generatedTexture.SetPixels(colorMap);
        generatedTexture.Apply();
    }
}
