/* MtStHelensVis.cs
 * CSCI 5609
 *
 * Copyright (c) 2022 University of Minnesota
 * Authors: Bridger Herman <herma582@umn.edu>
 *
 */

using UnityEngine;
using IVLab.ABREngine;
using IVLab.Utilities;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using IVLab.Plotting;
using UnityEngine.UI;

public class LinkedViewsSolution : MonoBehaviour
{
    [SerializeField, Tooltip("Data container for all the Mt. St. Helens data")]
    public MtStHelensData data;

    public DataImpressionGroup valley_group;
    public DataImpressionGroup head_group;

    public SimpleLineDataImpression di_wind_v = null;
    public SimpleSurfaceDataImpression di_ground_v = null;
    public SimpleSurfaceDataImpression di_trees_v = null;
    public SimpleVolumeDataImpression di_smoke_v = null;

    public SimpleLineDataImpression di_wind_h = null;
    public SimpleSurfaceDataImpression di_ground_h = null;
    public SimpleSurfaceDataImpression di_trees_h = null;
    public SimpleVolumeDataImpression di_smoke_h = null;

    // Constants for data files
    //private const string Dataset = "LANL/FireSim/valley_losAlamos.73000/KeyData";
    private const string Dataset = "LANL/FireSim/mountain_headCurve40.60000/KeyData";
    private const string groundPath = Dataset + "/Ground";
    private const string smokePath = Dataset + "/Smoke";
    private const string treesPath = Dataset + "/Trees";
    private const string windPath = Dataset + "/Wind";

    private LineTextureGradient lineTextureGrad;

    //[SerializeField, Tooltip("UUID of the ABR colormap to apply to the 'after' surface")]
    //private string surfaceColormap;

    //[SerializeField, Tooltip("UUID of the ABR colormap to apply to the 2D points on the scatter plot")]
    //private string plot2DColormap;

    // Keep track of whether we need to render ABREngine or not.
    // You shouldn't need to mess with this.
    private bool needToRender = false;

    // Start is run when you press 'Play' in Unity - this is similar to
    // 'setup()' in Processing.
    void Start()
    {
        // NEW: All data loading is handled by `MtStHelensData` script now!
        makeLineTexture();
        // Create the data impressions in ABR
        CreateDataImpressions_h();
        CreateDataImpressions_v();

        // Render for the first time
        ABREngine.Instance.Render();
        // Load the colormap for 2D plots so it's easily accessible
    }

    // Update is run each frame -- this is similar to Processing's 'draw()' function
    void Update()
    {
        if (needToRender)
        {
            needToRender = false;
            ABREngine.Instance.Render();
        }
    }

    
    void CreateDataImpressions_v()
    {
        // Quaternion group_quat = new Quaternion(0, 45, 0, 0);
        // Guid temp_guid = new Guid("valley_group");
        // Bounds b = ABRConfig.Info.defaultBounds;
        valley_group = ABREngine.Instance.CreateDataImpressionGroup("ValleyGroup", new Vector3(1.2f, 0f, 0f));

        //List<string> groundTag = new List<string>();
        //groundTag.Add("Ground_Impression");
        // Create a data impression and register
        di_ground_v = new SimpleSurfaceDataImpression();
        di_ground_v.keyData = data.kd_ground_v;
        di_ground_v.colormap = ABREngine.Instance.VisAssets.GetVisAsset<ColormapVisAsset>(new Guid("08aa81ea-8d06-11ec-808f-0242ac110002"));
        di_ground_v.colorVariable = data.kd_ground_v.GetScalarVariable("O2");

        // Range for O2 in valley
        di_ground_v.colorVariable.Range.min = 0f;
        di_ground_v.colorVariable.Range.max = 0.2f;
        ABREngine.Instance.RegisterDataImpression(di_ground_v, valley_group);
    
        // VALLEY SMOKE DATA
        di_smoke_v = new SimpleVolumeDataImpression();
        di_smoke_v.keyData = data.kd_smoke_v;
        di_smoke_v.colormap = ABREngine.Instance.VisAssets.GetVisAsset<ColormapVisAsset>(new Guid("4f3e7738-bdb2-11ec-a8f1-0242ac110002"));
        
        // Add 'theta' variable
        di_smoke_v.colorVariable = data.kd_smoke_v.GetScalarVariable("theta");
        //di_smoke_v.RenderHints.PerIndexVisibility = new System.Collections.BitArray(data.kd_smoke.GetScalarVariable("theta").Length, true);

        // Range for theta in valley
        di_smoke_v.colorVariable.Range.min = 300.1f;
        di_smoke_v.colorVariable.Range.max = 302.0f;

        // Opacity map values for valley
        float[] points = new float[] { 0.0f, 0.15f, 0.91f, 1.0f };
        string[] values = new string[] { "0%", "100%", "100%", "0%" };
        PrimitiveGradient pg = new PrimitiveGradient(System.Guid.NewGuid(), points, values);
        di_smoke_v.opacitymap = pg;

        // Register
        ABREngine.Instance.RegisterDataImpression(di_smoke_v, valley_group);
  
        /////////////////////////////////////////////
        // TREE DATA ///////////////////////////////

        //SimpleVolumeDataImpression di_trees_v = new SimpleVolumeDataImpression();
        di_trees_v = new SimpleSurfaceDataImpression();
        di_trees_v.keyData = data.kd_trees_v;
        // Add a colormap
        di_trees_v.colormap = ABREngine.Instance.VisAssets.GetVisAsset<ColormapVisAsset>(new Guid("27f74670-b946-11ec-9ebf-0242ac110002"));
        // Add 'theta' variable
        di_trees_v.colorVariable = data.kd_trees_v.GetScalarVariable("theta");

        // Range for theta in valley, not sure that this is correct
        di_trees_v.colorVariable.Range.min = 300.1f;
        di_trees_v.colorVariable.Range.max = 302.0f;

        // Register
        ABREngine.Instance.RegisterDataImpression(di_trees_v, valley_group); 

         /////////////////////////////////////////////
        // WIND DATA ///////////////////////////////
        di_wind_v = new SimpleLineDataImpression();
        di_wind_v.keyData = data.kd_wind_v;
        // theta color variable
        di_wind_v.colormap = ABREngine.Instance.VisAssets.GetVisAsset<ColormapVisAsset>(new Guid("1ceefd72-8c50-11ec-bc9f-0242ac110002"));
        ScalarDataVariable rotation_var_scalar = data.kd_wind_v.GetScalarVariable("Rotation");
        di_wind_v.colorVariable = rotation_var_scalar;
        di_wind_v.colorVariable.Range.min = rotation_var_scalar.Range.min;
        di_wind_v.colorVariable.Range.max = rotation_var_scalar.Range.max;
        // add velocity variable
        di_wind_v.lineTexture = lineTextureGrad;
        ScalarDataVariable velocityMag_var_scalar = data.kd_wind_v.GetScalarVariable("Velocity_mag");
        di_wind_v.lineTextureVariable = velocityMag_var_scalar;
        di_wind_v.lineTextureVariable.Range.min = velocityMag_var_scalar.Range.min;
        di_wind_v.lineTextureVariable.Range.max = velocityMag_var_scalar.Range.max;

        // Register
        ABREngine.Instance.RegisterDataImpression(di_wind_v, valley_group);   
    
    }

    void CreateDataImpressions_h()
    {
        head_group = ABREngine.Instance.CreateDataImpressionGroup("HeadGroup", new Vector3(-1.2f, 0f, 0f));
        
        //List<string> groundTag = new List<string>();
        //groundTag.Add("Ground_Impression");
        // Create a data impression and register
        di_ground_h = new SimpleSurfaceDataImpression();
        di_ground_h.keyData = data.kd_ground_h;
        di_ground_h.colormap = ABREngine.Instance.VisAssets.GetVisAsset<ColormapVisAsset>(new Guid("08aa81ea-8d06-11ec-808f-0242ac110002"));
        di_ground_h.colorVariable = data.kd_ground_h.GetScalarVariable("O2");

        // Range for O2 in valley
        di_ground_h.colorVariable.Range.min = 0f;
        di_ground_h.colorVariable.Range.max = 0.2f;
        ABREngine.Instance.RegisterDataImpression(di_ground_h, head_group);
    
        // VALLEY SMOKE DATA
        di_smoke_h = new SimpleVolumeDataImpression();
        di_smoke_h.keyData = data.kd_smoke_h;
        di_smoke_h.colormap = ABREngine.Instance.VisAssets.GetVisAsset<ColormapVisAsset>(new Guid("4f3e7738-bdb2-11ec-a8f1-0242ac110002"));
        
        // Add 'theta' variable
        di_smoke_h.colorVariable = data.kd_smoke_h.GetScalarVariable("theta");
        //di_smoke_h.RenderHints.PerIndexVisibility = new System.Collections.BitArray(data.kd_smoke.GetScalarVariable("theta").Length, true);

        // Range for theta in valley
        di_smoke_h.colorVariable.Range.min = 300.1f;
        di_smoke_h.colorVariable.Range.max = 302.0f;

        // Opacity map values for valley
        float[] points = new float[] { 0.0f, 0.15f, 0.91f, 1.0f };
        string[] values = new string[] { "0%", "100%", "100%", "0%" };
        PrimitiveGradient pg = new PrimitiveGradient(System.Guid.NewGuid(), points, values);
        di_smoke_h.opacitymap = pg;

        // Register
        ABREngine.Instance.RegisterDataImpression(di_smoke_h, head_group);
  
        /////////////////////////////////////////////
        // TREE DATA ///////////////////////////////

        //SimpleVolumeDataImpression di_trees_h = new SimpleVolumeDataImpression();
        di_trees_h = new SimpleSurfaceDataImpression();
        di_trees_h.keyData = data.kd_trees_h;
        // Add a colormap
        di_trees_h.colormap = ABREngine.Instance.VisAssets.GetVisAsset<ColormapVisAsset>(new Guid("27f74670-b946-11ec-9ebf-0242ac110002"));
        // Add 'theta' variable
        di_trees_h.colorVariable = data.kd_trees_h.GetScalarVariable("theta");

        // Range for theta in valley, not sure that this is correct
        di_trees_h.colorVariable.Range.min = 300.1f;
        di_trees_h.colorVariable.Range.max = 302.0f;

        // Register
        ABREngine.Instance.RegisterDataImpression(di_trees_h, head_group); 

         /////////////////////////////////////////////
        // WIND DATA ///////////////////////////////
        di_wind_h = new SimpleLineDataImpression();
        di_wind_h.keyData = data.kd_wind_h;
        di_wind_h.colormap = ABREngine.Instance.VisAssets.GetVisAsset<ColormapVisAsset>(new Guid("1ceefd72-8c50-11ec-bc9f-0242ac110002"));
        // // Add rotation variable
        ScalarDataVariable rotation_var_scalar = data.kd_wind_h.GetScalarVariable("Rotation");
        di_wind_h.colorVariable = rotation_var_scalar;
        di_wind_h.colorVariable.Range.min = rotation_var_scalar.Range.min;
        di_wind_h.colorVariable.Range.max = rotation_var_scalar.Range.max;
        // add velocity variable
        di_wind_h.lineTexture = lineTextureGrad;
        ScalarDataVariable velocityMag_var_scalar = data.kd_wind_h.GetScalarVariable("Velocity_mag");
        di_wind_h.lineTextureVariable = velocityMag_var_scalar;
        di_wind_h.lineTextureVariable.Range.min = velocityMag_var_scalar.Range.min;
        di_wind_h.lineTextureVariable.Range.max = velocityMag_var_scalar.Range.max;

        // Register
        ABREngine.Instance.RegisterDataImpression(di_wind_h, head_group);   
    
    }

    void makeLineTexture() {
        // LineTextureGradient g = new LineTextureGradient();

        // var line1 = ABREngine.Instance.GetVisAsset<LineTextureVisAsset>(...);
        // var line2 = ABREngine.Instance.GetVisAsset<LineTextureVisAsset>(...);

        // var lines = new List<LineTextureVisAsset>() { line1, line2 };
        // var splitPoints = new List<float> { 0.5f }; // split between line1 and line2 halfway down the data range

        // g.Initialize(Guid.NewGuid(), lines, splitPoints);

        lineTextureGrad = new LineTextureGradient();

        var line1 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("a979731c-86a6-11ec-9620-0242ac110002"));
        var line2 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("b7b8917e-86a6-11ec-b725-0242ac110002"));
        var line3 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("dcfd094c-86a6-11ec-b4a4-0242ac110002"));
        var line4 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("e7805bda-86a6-11ec-a260-0242ac110002"));
        var line5 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("f7e912dc-86a6-11ec-b1e9-0242ac110002"));
        var line6 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("06490698-86a7-11ec-8f86-0242ac110002"));      
        var line7 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("18931ed8-86a7-11ec-b592-0242ac110002"));
        var line8 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("2ed832f0-86a7-11ec-a3ae-0242ac110002"));
        var line9 = ABREngine.Instance.VisAssets.GetVisAsset<LineTextureVisAsset>(new Guid("44bbcb4a-86a7-11ec-998a-0242ac110002"));

        var lines = new List<LineTextureVisAsset>() { line1, line2, line3, line4, line5, line6, line7, line8, line9 };

        //var splitPoints = new List<float> { -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f }; // split between line1 and line2 halfway down the data range
        var splitPoints = new List<float> { .11f, .22f, .33f, .44f, .55f, .66f, .77f, .88f };
        //var splitPoints = new List<float> { .5f };

        lineTextureGrad.Initialize(Guid.NewGuid(), lines, splitPoints);
        
    }

}
