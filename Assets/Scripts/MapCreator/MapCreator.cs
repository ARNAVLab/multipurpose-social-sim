using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

/// <summary>
/// MapCreator class responsible for managing functionality of 
/// map creator tool.
/// </summary>
public class MapCreator : MonoBehaviour
{
    [Header("GRID SETTINGS")]
    [SerializeField] [Tooltip("Reference to grid containing distribution info")] 
    private Grid _worldGrid;
    [SerializeField] [Tooltip("Starting width of grid")] 
    private int _defaultGridWidth;
    [SerializeField] [Tooltip("Starting height of grid")] 
    private int _defaultGridHeight;
    [SerializeField] [Tooltip("Reference to background sprite renderer")] 
    private SpriteRenderer _backgroundSprite;
    [SerializeField] [Tooltip("Text field for setting grid width")] 
    private TMP_InputField _gridWidthField;
    [SerializeField] [Tooltip("Text fieeld for settging grid height")] 
    private TMP_InputField _gridHeightField;

    [Header("AGENT PRESETS")]
    [SerializeField] [Tooltip("Prefab for agent icon to use in list of agent presets")] 
    private GameObject _agentIconPrefab;
    [SerializeField] [Tooltip("Reference to transform containing list of agent presets")] 
    private Transform _agentIconViewport;
    [SerializeField] [Tooltip("Text field for setting agent preset name")] 
    private TMP_InputField _agentNameField;
    [SerializeField] [Tooltip("Reference to transform containing list of the agent preset's motivation values")] 
    private Transform _motivationListViewport;
    [SerializeField] [Tooltip("Prefab for motivation values")] 
    private GameObject _motivationFieldPrefab;

    [Header("DISTRIBUTIONS")]
    [SerializeField] [Tooltip("Prefab for distribution icon to use in distribution list")] 
    private GameObject _distIconPrefab;
    [SerializeField] [Tooltip("Reference to transform containing list of distributions")] 
    private Transform _distIconViewport;
    [SerializeField] [Tooltip("Text field for setting distribution name")] 
    private TMP_InputField _distNameField;
    [SerializeField] [Tooltip("Transform containing list of agent densities in distribution")] 
    private Transform _densityListViewport;
    [SerializeField] [Tooltip("Prefab that displays density of an agent preset within a distrubution")] 
    private GameObject _densityPrefab;
    [SerializeField] [Tooltip("Reference to color picker for setting distrubution color")] 
    private ColorPicker _colorPicker;

    /// <summary>
    /// Current width of grid
    /// </summary>
    private int _gridWidth;
    /// <summary>
    /// Current height of grid
    /// </summary>
    private int _gridHeight;
    /// <summary>
    /// Currently selected agent preset icon
    /// </summary>
    private AgentPresetButton _selectedAgentIcon;
    /// <summary>
    /// List of all agent preset icons
    /// </summary>
    private List<AgentPresetButton> _agentIcons = new List<AgentPresetButton>();
    /// <summary>
    /// All motivation fields indexed by name
    /// </summary>
    private Dictionary<string,MotivationField> _motivationFields = new Dictionary<string,MotivationField>();
    /// <summary>
    /// Currently selected agent distribution icon
    /// </summary>
    private DistributionButton _selectedDistIcon;
    /// <summary>
    /// All distribution icons
    /// </summary>
    private List<DistributionButton> _distIcons = new List<DistributionButton>();
    /// <summary>
    /// All density fields indexed by the corresponding agent presets's button
    /// </summary>
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

    /// <summary>
    /// Creates an agent preset with default values
    /// </summary>
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

    /// <summary>
    /// Deletes the currently selected agent preset
    /// </summary>
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

    /// <summary>
    /// Select an agent preset and displays its info
    /// </summary>
    /// <param name="button">Agent preset button to select</param>
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

    /// <summary>
    /// Renames the currently selected agent preset
    /// </summary>
    /// <param name="name">New name of agent preset</param>
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

    /// <summary>
    /// Sets a motivation value for the currently selected agent preset
    /// </summary>
    /// <param name="name">Name of motivation</param>
    /// <param name="value">New value of motivation</param>
    public void SetMotivationValue(string name, float value)
    {
        _selectedAgentIcon.Preset.Motivations[name] = value;
    }

    /// <summary>
    /// Adds the currently selected agent to the selected distribution
    /// </summary>
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

    /// <summary>
    /// Removes the currently selected agent from the selected distribution
    /// </summary>
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

    /// <summary>
    /// Creates an empty distribution
    /// </summary>
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

    /// <summary>
    /// Deletes the currently selected distribution
    /// </summary>
    public void DeleteDist()
    {
        if (_distIcons.Count <= 1)
            return;
        _distIcons.Remove(_selectedDistIcon);
        Destroy(_selectedDistIcon.gameObject);
        SelectDist(_distIcons[0]);
    }

    /// <summary>
    /// Selects a distribution and displays its info
    /// </summary>
    /// <param name="button">Distribution button to select</param>
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

    /// <summary>
    /// Renames the currently selected distribution
    /// </summary>
    /// <param name="name">New name of distribution</param>
    public void RenameDist(string name)
    {
        _selectedDistIcon.Name = name;
    }

    /// <summary>
    /// Sets the density value of an agent in the currently selected distribution
    /// </summary>
    /// <param name="presetButton">Corresponding agent preset button</param>
    /// <param name="value">New density value</param>
    public void SetDensityValue(AgentPresetButton presetButton, float value)
    {
        _selectedDistIcon.Distribution.AgentWeights[presetButton] = value;
        float consequent = _selectedDistIcon.GetConsequent();
        foreach (KeyValuePair<AgentPresetButton,AgentDensityField> densityField in _densityFields)
        {
            densityField.Value.SetRatioConsequent(consequent);
        }
    }

    /// <summary>
    /// Sets the color of the selected distribution
    /// </summary>
    /// <param name="color">New color of distribution</param>
    public void SetDistColor(Color32 color)
    {
        _selectedDistIcon.PaintColor = color;
    }

    /// <summary>
    /// Sets the width of the grid
    /// </summary>
    /// <param name="input">New width of grid as string</param>
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

    /// <summary>
    /// Sets the height of the grid
    /// </summary>
    /// <param name="input">New height of grid as string</param>
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

    /// <summary>
    /// Resets the grid width and height
    /// </summary>
    private void ResetGrid()
    {
        _worldGrid.SetGridSize(_gridWidth, _gridHeight);
        // _gridSprite.size = new Vector2(_gridWidth + 0.125f, _gridHeight + 0.125f);
        // _backgroundSprite.transform.localScale = new Vector3(_gridSprite.size.x, _gridSprite.size.y, _backgroundSprite.transform.localScale.z);
        // _backgroundSprite.transform.localPosition = new Vector3(-0.5f * _gridSprite.size.x, -0.5f * _gridSprite.size.y, _backgroundSprite.transform.localPosition.z);
    }

    /// <summary>
    /// Paints a tile with the currently selected distribution
    /// </summary>
    /// <param name="tile">Tile to paint</param>
    public void PaintTile(GridTile tile)
    {
        tile.SetDist(_selectedDistIcon);
    }

    /// <summary>
    /// Erases any distribution from a tile
    /// </summary>
    /// <param name="tile">Tile to erase from</param>
    public void EraseTile(GridTile tile)
    {
        tile.SetDist(null);
    }

    /// <summary>
    /// Moves the background sprite using translation vector
    /// </summary>
    /// <param name="translation">Vector to move background</param>
    public void MoveBackground(Vector2 translation)
    {
        _backgroundSprite.transform.position += (Vector3)translation;
    }

    /// <summary>
    /// Resizes the background by the given scale
    /// </summary>
    /// <param name="resizeValue">Value to resize by</param>
    public void ResizeBackground(float resizeValue)
    {
        _backgroundSprite.transform.localScale += _backgroundSprite.transform.localScale * resizeValue;
    }

    /// <summary>
    /// Loads an image from URL in the clipboard
    /// </summary>
    public void LoadImage()
    {
        // StartCoroutine(GetImage(_imagePathField.text));
        StartCoroutine(GetImage(GUIUtility.systemCopyBuffer));
    }

    /// <summary>
    /// Sets the background to image at the URL
    /// </summary>
    /// <param name="url">URL of image</param>
    /// <returns></returns>
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

    /// <summary>
    /// (NOT IMPLEMENTED) Finishes and generates the JSON file with distribution info from grid
    /// </summary>
    public void FinishGenerate()
    {
        // Generate a JSON file from the grid information
    }
}
