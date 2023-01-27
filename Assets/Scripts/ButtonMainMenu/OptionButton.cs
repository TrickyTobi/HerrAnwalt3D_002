using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionButton : MonoBehaviour
{
    Material _mat;
    [SerializeField] Color _highlightColor;
    [SerializeField] Color _selectedColor;
    [SerializeField] OptionController _optionController;
    Color _normalColor;

    private void Start()
    {
        _mat = GetComponent<MeshRenderer>().material;
        _normalColor = _mat.color;
    }

    private void OnMouseEnter()
    {
        _mat.color = _highlightColor;
    }

    private void OnMouseExit()
    {
        _mat.color = _normalColor;
    }

    private void OnMouseDown()
    {
        _mat.color = _selectedColor;
        _optionController.gameObject.SetActive(true);
    }

}
