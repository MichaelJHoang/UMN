using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using IVLab.ABREngine;

using IVLab.Utilities;

public class MtStHelensData : IVLab.Plotting.TabularDataContainer
{
    public bool Loaded { get; private set; } = false;

    // Constants for data files
    private const string Dataset_h = "LANL/FireSim/mountain_headCurve40.60000/KeyData";
    private const string groundPath_h = Dataset_h + "/Ground";
    private const string smokePath_h = Dataset_h + "/Smoke";
    private const string treesPath_h = Dataset_h + "/Trees";
    private const string windPath_h = Dataset_h + "/Wind";

    // head curve fire
    public RawDataset ds_wind_h;
    public RawDataset ds_smoke_h;
    public RawDataset ds_ground_h;
    public RawDataset ds_trees_h;

    public KeyData kd_wind_h;
    public KeyData kd_smoke_h;
    public KeyData kd_ground_h;
    public KeyData kd_trees_h;

    // Constants for data files
    private const string Dataset_v = "LANL/FireSim/valley_losAlamos.60000/KeyData";
    private const string groundPath_v = Dataset_v + "/Ground";
    private const string smokePath_v = Dataset_v + "/Smoke";
    private const string treesPath_v = Dataset_v + "/Trees";
    private const string windPath_v = Dataset_v + "/Wind";

    // valley fire
    public RawDataset ds_wind_v;
    public RawDataset ds_smoke_v;
    public RawDataset ds_ground_v;
    public RawDataset ds_trees_v;

    public KeyData kd_wind_v;
    public KeyData kd_smoke_v;
    public KeyData kd_ground_v;
    public KeyData kd_trees_v;


    // Bounding box of the above lists
    [HideInInspector]
    public Bounds wind_bounds;

    void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        // Load in the data
        LoadData();

        initialized = true;
    }

    // Load Data: Same as LoadPointData from previous A5
    private void LoadData()
    {
        /// LOAD DATA heacurve
        ds_wind_h = ABREngine.Instance.Data.LoadRawDataset<MediaDataLoader>(windPath_h);
        kd_wind_h = ABREngine.Instance.Data.ImportRawDataset(windPath_h, ds_wind_h);

        ds_smoke_h = ABREngine.Instance.Data.LoadRawDataset<MediaDataLoader>(smokePath_h);
        kd_smoke_h = ABREngine.Instance.Data.ImportRawDataset(smokePath_h, ds_smoke_h);
        
        ds_trees_h = ABREngine.Instance.Data.LoadRawDataset<MediaDataLoader>(treesPath_h);
        kd_trees_h = ABREngine.Instance.Data.ImportRawDataset(treesPath_h, ds_trees_h);

        ds_ground_h = ABREngine.Instance.Data.LoadRawDataset<MediaDataLoader>(groundPath_h);
        kd_ground_h = ABREngine.Instance.Data.ImportRawDataset(groundPath_h, ds_ground_h);


        /// LOAD DATA valley
        ds_wind_v = ABREngine.Instance.Data.LoadRawDataset<MediaDataLoader>(windPath_v);
        kd_wind_v = ABREngine.Instance.Data.ImportRawDataset(windPath_v, ds_wind_v);

        ds_smoke_v = ABREngine.Instance.Data.LoadRawDataset<MediaDataLoader>(smokePath_v);
        kd_smoke_v = ABREngine.Instance.Data.ImportRawDataset(smokePath_v, ds_smoke_v);
        
        ds_trees_v = ABREngine.Instance.Data.LoadRawDataset<MediaDataLoader>(treesPath_v);
        kd_trees_v = ABREngine.Instance.Data.ImportRawDataset(treesPath_v, ds_trees_v);

        ds_ground_v = ABREngine.Instance.Data.LoadRawDataset<MediaDataLoader>(groundPath_v);
        kd_ground_v = ABREngine.Instance.Data.ImportRawDataset(groundPath_v, ds_ground_v);

        Loaded = true;
    }
}