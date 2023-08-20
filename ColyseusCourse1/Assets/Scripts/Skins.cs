using UnityEngine;

public class Skins : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    public int length { get { return _materials.Length; } }

    public Material GetMaterial(int index)
    {
        if (index >= length - 1 || index < 0) return _materials[0]; // Если индекс за пределами, то выводим первый материал

        return _materials[index];
    }
}
