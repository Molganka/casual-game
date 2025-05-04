using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public int totalXPixels = 1024;  // Ширина текстуры
    public int totalYPixels = 1024;  // Высота текстуры
    private Color[] colorMap;  // Массив цветов для текстуры
    private Texture2D generatedTexture;  // Текстура, которую будем обновлять
    private Renderer objectRenderer;

    void Start()
    {
        // Создаем массив цветов и саму текстуру
        colorMap = new Color[totalXPixels * totalYPixels];
        generatedTexture = new Texture2D(totalXPixels, totalYPixels, TextureFormat.RGBA32, false);

        // Получаем компонент Renderer объекта
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.mainTexture = generatedTexture;  // Привязываем текстуру к материалу объекта
    }

    void Update()
    {
        // Получаем позицию объекта (X, Z)
        Vector3 objectPosition = transform.position;

        // Преобразуем мировую позицию объекта в координаты текстуры
        float normalizedX = Mathf.InverseLerp(-10f, 10f, objectPosition.x);  // Нормализуем координаты X
        float normalizedY = Mathf.InverseLerp(-10f, 10f, objectPosition.z);  // Нормализуем координаты Y

        // Переводим нормализованные значения в пиксели текстуры
        int xPixel = Mathf.FloorToInt(normalizedX * totalXPixels);
        int yPixel = Mathf.FloorToInt(normalizedY * totalYPixels);

        // Ограничиваем координаты, чтобы они не выходили за пределы текстуры
        xPixel = Mathf.Clamp(xPixel, 0, totalXPixels - 1);
        yPixel = Mathf.Clamp(yPixel, 0, totalYPixels - 1);

        // Задаем цвет пикселя (например, красный)
        colorMap[yPixel * totalXPixels + xPixel] = Color.red;

        // Обновляем текстуру, применяя изменения
        generatedTexture.SetPixels(colorMap);
        generatedTexture.Apply();
    }
}
