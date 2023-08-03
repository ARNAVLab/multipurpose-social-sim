using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class MapCreator : MonoBehaviour
{
    [Header("GRID SETTINGS")]
    [SerializeField] private Grid _worldGrid;
    [SerializeField] private int _defaultGridWidth;
    [SerializeField] private int _defaultGridHeight;
    [SerializeField] private SpriteRenderer _gridSprite;
    [SerializeField] private SpriteRenderer _backgroundSprite;
    [SerializeField] private TMP_InputField _gridWidthField;
    [SerializeField] private TMP_InputField _gridHeightField;

    [Header("AGENT PRESETS")]
    [SerializeField] private GameObject _agentIconPrefab;
    [SerializeField] private Transform _agentIconViewport;
    [SerializeField] private TMP_InputField _agentNameField;
    [SerializeField] private Transform _motivationListViewport;
    [SerializeField] private GameObject _motivationFieldPrefab;

    [Header("DISTRIBUTIONS")]
    [SerializeField] private GameObject _distIconPrefab;
    [SerializeField] private Transform _distIconViewport;
    [SerializeField] private TMP_InputField _distNameField;
    [SerializeField] private Transform _densityListViewport;
    [SerializeField] private GameObject _densityPrefab;
    [SerializeField] private ColorPicker _colorPicker;

    [Header("GENERATION")]
    [SerializeField] private GameObject _generateInfoPrefab;

    private int _gridWidth;
    private int _gridHeight;
    private AgentPresetButton _selectedAgentIcon;
    private List<AgentPresetButton> _agentIcons = new List<AgentPresetButton>();
    private Dictionary<string,MotivationField> _motivationFields = new Dictionary<string,MotivationField>();
    private DistributionButton _selectedDistIcon;
    private List<DistributionButton> _distIcons = new List<DistributionButton>();
    private Dictionary<AgentPresetButton,AgentDensityField> _densityFields = new Dictionary<AgentPresetButton,AgentDensityField>();

    private void Awake()
    {
        _gridWidth = _defaultGridWidth;
        _gridHeight = _defaultGridHeight;
        _gridWidthField.text = _gridWidth.ToString();
        _gridHeightField.text = _gridHeight.ToString();
        ResetGrid();
        CreateDist();
        CreateAgent();
    }

    public void CreateAgent()
    {
        GameObject agentIcon = Instantiate(_agentIconPrefab, _agentIconViewport);
        agentIcon.transform.SetSiblingIndex(_agentIconViewport.childCount - 2);
        AgentPresetButton iconScript = agentIcon.GetComponent<AgentPresetButton>();
        _agentIcons.Add(iconScript);
        iconScript.Preset = new AgentPreset();
        iconScript.Name = "New Agent";
        iconScript.Preset.Motivations = new Dictionary<string, float>();
        iconScript.Preset.Motivations.Add("accomplishment", 0);
        iconScript.Preset.Motivations.Add("social", 0);
        iconScript.Preset.Motivations.Add("physical", 0);
        iconScript.Preset.Motivations.Add("emotional", 0);
        iconScript.Preset.Motivations.Add("financial", 0);
        SelectAgent(iconScript);
    }

    public void DeleteAgent()
    {
        if (_agentIcons.Count <= 1)
            return;
        RemoveAgentFromDist();
        foreach (DistributionButton dist in _distIcons)
        {
            if (dist.Distribution.AgentWeights.ContainsKey(_selectedAgentIcon))
            {
                dist.Distribution.AgentWeights.Remove(_selectedAgentIcon);
            }
        }
        _agentIcons.Remove(_selectedAgentIcon);
        Destroy(_selectedAgentIcon.gameObject);
        SelectAgent(_agentIcons[0]);
    }

    public void SelectAgent(AgentPresetButton button)
    {
        _agentNameField.text = button.Name;

        foreach (KeyValuePair<string,MotivationField> motivationField in _motivationFields)
        {
            Destroy(motivationField.Value.gameObject);
        }
        _motivationFields.Clear();
        _selectedAgentIcon = button;
        AgentPreset preset = _selectedAgentIcon.Preset;

        foreach (KeyValuePair<string,float> motivation in preset.Motivations)
        {
            MotivationField motivationField = Instantiate(_motivationFieldPrefab, _motivationListViewport).GetComponent<MotivationField>();
            motivationField.Initialize(motivation.Key, motivation.Value);
            _motivationFields.Add(motivation.Key, motivationField);
        }
        _agentNameField.Select();
    }

    public void RenameAgent(string name)
    {
        _selectedAgentIcon.Name = name;
        foreach (KeyValuePair<AgentPresetButton, AgentDensityField> pair in _densityFields)
        {
            if (pair.Key == _selectedAgentIcon)
            {
                pair.Value.Name = name;
            }
        }
    }

    public void SetMotivationValue(string name, float value)
    {
        _selectedAgentIcon.Preset.Motivations[name] = value;
    }

    public void AddAgentToDist()
    {
        if (_selectedDistIcon.Distribution.AgentWeights.ContainsKey(_selectedAgentIcon))
            return;

        _selectedDistIcon.Distribution.AgentWeights.Add(_selectedAgentIcon, 1);
        AgentDensityField densityField = Instantiate(_densityPrefab, _densityListViewport).GetComponent<AgentDensityField>();
        densityField.Initialize(_selectedAgentIcon.Name, 1, _selectedDistIcon.GetConsequent(), _selectedAgentIcon);
        float consequent = _selectedDistIcon.GetConsequent();
        foreach (KeyValuePair<AgentPresetButton,AgentDensityField> pair in _densityFields)
        {
            pair.Value.SetRatioConsequent(consequent);
        }
        _densityFields.Add(_selectedAgentIcon, densityField);
    }

    public void RemoveAgentFromDist()
    {
        if (!_selectedDistIcon.Distribution.AgentWeights.ContainsKey(_selectedAgentIcon))
            return;

        AgentDensityField densityField = _densityFields[_selectedAgentIcon];
        _densityFields.Remove(_selectedAgentIcon);
        _selectedDistIcon.Distribution.AgentWeights.Remove(_selectedAgentIcon);
        Destroy(densityField.gameObject);

        float consequent = _selectedDistIcon.GetConsequent();
        foreach (KeyValuePair<AgentPresetButton,AgentDensityField> pair in _densityFields)
        {
            pair.Value.SetRatioConsequent(consequent);
        }
    }

    public void CreateDist()
    {
        GameObject distIcon = Instantiate(_distIconPrefab, _distIconViewport);
        distIcon.transform.SetSiblingIndex(_distIconViewport.childCount - 2);
        DistributionButton iconScript = distIcon.GetComponent<DistributionButton>();
        _distIcons.Add(iconScript);
        iconScript.Distribution = new AgentDistribution();
        iconScript.Distribution.AgentWeights = new Dictionary<AgentPresetButton, float>();
        iconScript.Name = "New Dist";
        iconScript.PaintColor = new Color32(0, 255, 255, 255);
        SelectDist(iconScript);
    }

    public void DeleteDist()
    {
        if (_distIcons.Count <= 1)
            return;
        _distIcons.Remove(_selectedDistIcon);
        Destroy(_selectedDistIcon.gameObject);
        SelectDist(_distIcons[0]);
    }

    public void SelectDist(DistributionButton button)
    {
        _distNameField.text = button.Name;

        foreach (KeyValuePair<AgentPresetButton,AgentDensityField> densityField in _densityFields)
        {
            Destroy(densityField.Value.gameObject);
        }
        _densityFields.Clear();
        _selectedDistIcon = button;

        foreach (KeyValuePair<AgentPresetButton,float> pair in button.Distribution.AgentWeights)
        {
            AgentDensityField densityField = Instantiate(_densityPrefab, _densityListViewport).GetComponent<AgentDensityField>();
            densityField.Initialize(pair.Key.Name, pair.Value, _selectedDistIcon.GetConsequent(), pair.Key);
            _densityFields.Add(pair.Key, densityField);
        }
        _colorPicker.SetColor(_selectedDistIcon.PaintColor);
        _distNameField.Select();
    }

    public void RenameDist(string name)
    {
        _selectedDistIcon.Name = name;
    }

    public void SetDensityValue(AgentPresetButton presetButton, float value)
    {
        _selectedDistIcon.Distribution.AgentWeights[presetButton] = value;
        float consequent = _selectedDistIcon.GetConsequent();
        foreach (KeyValuePair<AgentPresetButton,AgentDensityField> densityField in _densityFields)
        {
            densityField.Value.SetRatioConsequent(consequent);
        }
    }

    public void SetDistColor(Color32 color)
    {
        _selectedDistIcon.PaintColor = color;
    }

    public void SetGridWidth(string input)
    {
        int width;
        if (!int.TryParse(input, out width) || width < 1 || width > 100)
        {
            _gridWidthField.text = _gridWidth.ToString();
            return;
        }
        _gridWidth = width;
        ResetGrid();
    }

    public void SetGridHeight(string input)
    {
        int height;
        if (!int.TryParse(input, out height) || height < 1 || height > 100)
        {
            _gridHeightField.text = _gridHeight.ToString();
            return;
        }
        _gridHeight = height;
        ResetGrid();
    }

    private void ResetGrid()
    {
        _worldGrid.SetGridSize(_gridWidth, _gridHeight);
        // _gridSprite.size = new Vector2(_gridWidth + 0.125f, _gridHeight + 0.125f);
        // _backgroundSprite.transform.localScale = new Vector3(_gridSprite.size.x, _gridSprite.size.y, _backgroundSprite.transform.localScale.z);
        // _backgroundSprite.transform.localPosition = new Vector3(-0.5f * _gridSprite.size.x, -0.5f * _gridSprite.size.y, _backgroundSprite.transform.localPosition.z);
    }

    public void PaintTile(GridTile tile)
    {
        tile.SetDist(_selectedDistIcon);
    }

    public void EraseTile(GridTile tile)
    {
        tile.SetDist(null);
    }

    public void MoveBackground(Vector2 translation)
    {
        _backgroundSprite.transform.position += (Vector3)translation;
    }

    public void ResizeBackground(float resizeValue)
    {
        _backgroundSprite.transform.localScale += _backgroundSprite.transform.localScale * resizeValue;
    }

    public void LoadImage()
    {
        // StartCoroutine(GetImage(_imagePathField.text));
        StartCoroutine(GetImage(GUIUtility.systemCopyBuffer));
    }

    private IEnumerator GetImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if ((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError))
        {
            
        }
        else
        {
            Texture2D backgroundTexture = DownloadHandlerTexture.GetContent(request) as Texture2D;
            _backgroundSprite.sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), new Vector2());
        }
    }

    public void FinishGenerate()
    {
        GenerateInfo info = Instantiate(_generateInfoPrefab).GetComponent<GenerateInfo>();
    }
}
