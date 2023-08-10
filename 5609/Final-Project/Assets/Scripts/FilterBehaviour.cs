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


public class FilterBehaviour : MonoBehaviour
{
    //[SerializeField, Tooltip("Data container for all the Mt. St. Helens data")]
    //public MtStHelensData data;

    [SerializeField, Tooltip("Data container for all the Mt. St. Helens data")]
    public LinkedViewsSolution impressions;
    
    // data impression groups
    private DataImpressionGroup valley_group;
    private DataImpressionGroup head_group;

    // HEADCURVE ////////////////////
    // All data impressions
    private SimpleSurfaceDataImpression tree_di_h;
    private SimpleSurfaceDataImpression ground_di_h;
    private SimpleLineDataImpression wind_di_h;
    private SimpleVolumeDataImpression smoke_di_h;

    // All key data as float[]
    private float[] tree_var_h;
    private float[] ground_var_h;
    private float[] wind_var_h;
    private float[] smoke_var_h;

    // all data scalars
    public ScalarDataVariable tree_scalar_h;
    public ScalarDataVariable ground_scalar_h;
    public ScalarDataVariable wind_scalar_h;
    public ScalarDataVariable smoke_scalar_h;

    // VALLEY ////////////////////
    // All data impressions
    private SimpleSurfaceDataImpression tree_di_v;
    private SimpleSurfaceDataImpression ground_di_v;
    private SimpleLineDataImpression wind_di_v;
    private SimpleVolumeDataImpression smoke_di_v;

    // All key data as float[]
    private float[] tree_var_v;
    private float[] ground_var_v;
    private float[] wind_var_v;
    private float[] smoke_var_v;

    // all data scalars
    public ScalarDataVariable tree_scalar_v;
    public ScalarDataVariable ground_scalar_v;
    public ScalarDataVariable wind_scalar_v;
    public ScalarDataVariable smoke_scalar_v;

    /////////////////////////////////////////
    // min and max sliders
    public Slider[] tree_sliders;
    public Slider[] ground_sliders;
    public Slider[] wind_sliders;
    public Slider[] smoke_sliders;

    // track changes in sliders
    private float[] prev_tree_sliders = new float[2];
    private float[] prev_ground_sliders = new float[2];
    private float[] prev_wind_slidres = new float[2];
    private float[] prev_smoke_sliders = new float[2];

    // sliders text
    public Text[] tree_slid_txt;
    public Text[] ground_slid_txt;
    public Text[] wind_slid_txt;
    public Text[] smoke_slid_txt;

    // initialization
    bool tree_initialized = false;
    bool wind_initialized = false;
    bool ground_initialized = false;
    bool smoke_initialized = false;
    //void Start()
    // Start is called before the first frame update
    


    // Update is called once per frame
    void Update()
    {
        if ( head_group == null || valley_group == null){
            head_group = impressions.head_group;
            valley_group = impressions.valley_group;
        }
        if (tree_di_h == null || tree_di_v == null){
            tree_di_h = impressions.di_trees_h;
            tree_di_v = impressions.di_trees_v;
        }
        Debug.Log("Here 111");
        if (tree_di_h?.keyData == null || tree_di_v?.keyData == null)
        {
            return;
        }
        if(!tree_initialized){
            // headcurve
            tree_scalar_h = tree_di_h.keyData.GetScalarVariable("theta");
            tree_var_h = tree_scalar_h.GetArray(tree_di_h.keyData);
            tree_di_h.RenderHints.PerIndexVisibility = new System.Collections.BitArray(tree_var_h.Length, true);

            // valley
            tree_scalar_v = tree_di_v.keyData.GetScalarVariable("theta");
            tree_var_v = tree_scalar_v.GetArray(tree_di_v.keyData);
            tree_di_v.RenderHints.PerIndexVisibility = new System.Collections.BitArray(tree_var_v.Length, true);

            //float min = tree_scalar_h.Range.min;
            //float max = tree_scalar_h.Range.max;

            float min;
            float max; 
            if (tree_scalar_h.Range.min <= tree_scalar_v.Range.min){
                min = tree_scalar_h.Range.min;
            } else {
                min = tree_scalar_v.Range.min;
            }
            if (tree_scalar_h.Range.max >= tree_scalar_v.Range.max){
                max = tree_scalar_h.Range.max;
            } else {
                max = tree_scalar_v.Range.max;
            }

            tree_sliders[0].minValue = min;
            tree_sliders[0].maxValue = max;
            tree_sliders[1].minValue = min;
            tree_sliders[1].maxValue = max;
            tree_sliders[0].value = min;
            tree_sliders[1].value = max;
            prev_tree_sliders[0] = min;
            prev_tree_sliders[1] = max;
            setText("tree");
            tree_initialized = true;
        }
        
        if (ground_di_h == null || ground_di_v == null)
        {
            ground_di_h = impressions.di_ground_h;
            ground_di_v = impressions.di_ground_v;
        }
        if (ground_di_h?.keyData == null || ground_di_v?.keyData == null)
        {
            return;
        }
        if (!ground_initialized){
            // headcurve
            ground_scalar_h = ground_di_h.keyData.GetScalarVariable("O2");
            ground_var_h = ground_scalar_h.GetArray(ground_di_h.keyData);
            ground_di_h.RenderHints.PerIndexVisibility = new System.Collections.BitArray(ground_var_h.Length, true);

            // valley
            ground_scalar_v = ground_di_v.keyData.GetScalarVariable("O2");
            ground_var_v = ground_scalar_v.GetArray(ground_di_v.keyData);
            ground_di_v.RenderHints.PerIndexVisibility = new System.Collections.BitArray(ground_var_v.Length, true);

            //float min = ground_scalar_h.Range.min;
            //float max = ground_scalar_h.Range.max;
            float min;
            float max;
            if ( ground_scalar_h.Range.min <= ground_scalar_v.Range.min){
                min = ground_scalar_h.Range.min;
            } else {
                min = ground_scalar_v.Range.min;
            }
            if (ground_scalar_h.Range.max >= ground_scalar_v.Range.max){
                max = ground_scalar_h.Range.max;
            } else {
                max = ground_scalar_v.Range.max;
            }

            ground_sliders[0].minValue = min;
            ground_sliders[0].maxValue = max;
            ground_sliders[1].minValue = min;
            ground_sliders[1].maxValue = max;
            ground_sliders[0].value = min;
            ground_sliders[1].value = max;

            prev_ground_sliders[0] = min;
            prev_ground_sliders[1] = max;
            setText("ground");
            ground_initialized = true;
        }

        if (wind_di_h == null || wind_di_v == null)
        {    
            wind_di_h = impressions.di_wind_h;
            wind_di_v = impressions.di_wind_v;
        }   
        if (wind_di_h?.keyData == null || wind_di_v?.keyData == null)
        {
            return;
        }
        if (!wind_initialized){
            // headcurve
            wind_scalar_h = wind_di_h.keyData.GetScalarVariables()[0];
            wind_var_h = wind_scalar_h.GetArray(wind_di_h.keyData);
            wind_di_h.RenderHints.PerIndexVisibility = new System.Collections.BitArray(wind_var_h.Length, true);

            // valley
            wind_scalar_v = wind_di_v.keyData.GetScalarVariables()[0];
            wind_var_v = wind_scalar_v.GetArray(wind_di_h.keyData);
            wind_di_v.RenderHints.PerIndexVisibility = new System.Collections.BitArray(wind_var_v.Length, true);

            //float min = wind_scalar_h.Range.min;
            //float max = wind_scalar_h.Range.max;

            float min;
            float max;
            if ( wind_scalar_h.Range.min <= wind_scalar_v.Range.min){
                min = wind_scalar_h.Range.min;
            }else {
                min = wind_scalar_v.Range.min;
            }
            if (wind_scalar_h.Range.max >= wind_scalar_v.Range.max){
                max = wind_scalar_h.Range.max;
            } else {
                max = wind_scalar_v.Range.max;

            }
            wind_sliders[0].minValue = min;
            wind_sliders[0].maxValue = max;
            wind_sliders[1].minValue = min;
            wind_sliders[1].maxValue = max;
            wind_sliders[0].value = min;
            wind_sliders[1].value = max;
            prev_wind_slidres[0] = min;
            prev_wind_slidres[1] = max;
            setText("wind");
            wind_initialized = true;
        }
        if (smoke_di_h == null || smoke_di_v == null) {
            smoke_di_h = impressions.di_smoke_h;
            smoke_di_v = impressions.di_smoke_v;
        }
        if (smoke_di_h?.keyData == null || smoke_di_v?.keyData == null)
        {
            return;
        }
        if (!smoke_initialized){
            // headcurve
            smoke_scalar_h = smoke_di_h.keyData.GetScalarVariable("theta");
            smoke_var_h = smoke_scalar_h.GetArray(smoke_di_h.keyData);
            smoke_di_h.RenderHints.PerIndexVisibility = new System.Collections.BitArray(smoke_var_h.Length, true);

            // valley
            smoke_scalar_v = smoke_di_v.keyData.GetScalarVariable("theta");
            smoke_var_v = smoke_scalar_v.GetArray(smoke_di_v.keyData);
            smoke_di_v.RenderHints.PerIndexVisibility = new System.Collections.BitArray(smoke_var_v.Length, true);

            //float min = smoke_scalar_h.Range.min;
            //float max = smoke_scalar_h.Range.max;

            float min;
            float max;
            if ( smoke_scalar_h.Range.min <= smoke_scalar_v.Range.min){
                min = smoke_scalar_h.Range.min;
            } else {
                min = smoke_scalar_v.Range.min;
            }
            if (smoke_scalar_h.Range.max >= smoke_scalar_v.Range.max){
                max = smoke_scalar_h.Range.max;
            } else {
                max = smoke_scalar_h.Range.max;
            }
            smoke_sliders[0].minValue = min;
            smoke_sliders[0].maxValue = max;
            smoke_sliders[1].minValue = min;
            smoke_sliders[1].maxValue = max;
            smoke_sliders[0].value = min;
            smoke_sliders[1].value = max;
            prev_smoke_sliders[0] = min;
            prev_smoke_sliders[1] = max;
            setText("smoke");
            smoke_initialized = true;
        }
        ///NEW!!
        bool needToRenderTree = false;
        
        float maxVal = tree_sliders[1].value;
        float minVal = tree_sliders[0].value;
        if (maxVal != prev_tree_sliders[1] || minVal != prev_tree_sliders[0]) {
            prev_tree_sliders[0] = minVal;
            prev_tree_sliders[1] = maxVal;
            setText("tree");
            Debug.Log("tree Chnage");
            for (int i=0; i < tree_var_h.Length; i++)
            {
                bool oldVisibility = tree_di_h.RenderHints.PerIndexVisibility[i];
                bool newVisibility = tree_var_h[i] < maxVal && tree_var_h[i] > minVal;
                tree_di_h.RenderHints.PerIndexVisibility[i] = newVisibility;
                if (newVisibility != oldVisibility){
                    needToRenderTree = true;
                    Debug.Log("Should Render");
                }
            }
            for (int i=0; i < tree_var_v.Length; i++)
            {
                bool oldVisibility = tree_di_v.RenderHints.PerIndexVisibility[i];
                bool newVisibility = tree_var_v[i] < maxVal && tree_var_v[i] > minVal;
                tree_di_v.RenderHints.PerIndexVisibility[i] = newVisibility;
                if (newVisibility != oldVisibility){
                    needToRenderTree = true;
                    Debug.Log("Should Render");
                }
            }
        }

        bool needToRenderWind = false;
        maxVal = wind_sliders[1].value;
        minVal = wind_sliders[0].value;
        if (maxVal != prev_wind_slidres[1] || minVal != prev_wind_slidres[0]){
            prev_wind_slidres[0] = minVal;
            prev_wind_slidres[1] = maxVal;
            setText("wind");
            Debug.Log("inside wind change");
            for (int i=0; i < wind_var_h.Length; i++)
            {
                bool oldVisibility = wind_di_h.RenderHints.PerIndexVisibility[i];
                bool newVisibility = wind_var_h[i] < maxVal && wind_var_h[i] > minVal;
                wind_di_h.RenderHints.PerIndexVisibility[i] = newVisibility;
                if (newVisibility != oldVisibility)
                    needToRenderWind = true;
            }
            for (int i=0; i < wind_var_v.Length; i++)
            {
                bool oldVisibility = wind_di_v.RenderHints.PerIndexVisibility[i];
                bool newVisibility = wind_var_v[i] < maxVal && wind_var_v[i] > minVal;
                wind_di_v.RenderHints.PerIndexVisibility[i] = newVisibility;
                if (newVisibility != oldVisibility)
                    needToRenderWind = true;
            }
        }

        bool needToRenderGround = false;
        maxVal = ground_sliders[1].value;
        minVal = ground_sliders[0].value;
        if (maxVal != prev_ground_sliders[1] || minVal != prev_ground_sliders[0]){
            prev_ground_sliders[0] = minVal;
            prev_ground_sliders[1] = maxVal;
            setText("ground");
            Debug.Log("Inside ground change");
            for (int i=0; i < ground_var_h.Length; i++)
            {
                bool oldVisibility = ground_di_h.RenderHints.PerIndexVisibility[i];
                bool newVisibility = ground_var_h[i] < maxVal && ground_var_h[i] > minVal;
                ground_di_h.RenderHints.PerIndexVisibility[i] = newVisibility;
                if (newVisibility != oldVisibility)
                    needToRenderGround = true;
            }
            for (int i=0; i < ground_var_v.Length; i++)
            {
                bool oldVisibility = ground_di_v.RenderHints.PerIndexVisibility[i];
                bool newVisibility = ground_var_v[i] < maxVal && ground_var_v[i] > minVal;
                ground_di_v.RenderHints.PerIndexVisibility[i] = newVisibility;
                if (newVisibility != oldVisibility)
                    needToRenderGround = true;
            }
        }
        
        bool needToRenderSmoke = false;
        maxVal = smoke_sliders[1].value;
        minVal = smoke_sliders[0].value;
        if (maxVal != prev_smoke_sliders[1] || minVal != prev_smoke_sliders[0]){
            prev_smoke_sliders[0] = minVal;
            prev_smoke_sliders[1] = maxVal;
            setText("smoke");
            Debug.Log("Inside smoke change!");
            for (int i=0; i < smoke_var_h.Length; i++)
            {
                bool oldVisibility = smoke_di_h.RenderHints.PerIndexVisibility[i];
                bool newVisibility = smoke_var_h[i] < maxVal && smoke_var_h[i] > minVal;
                smoke_di_h.RenderHints.PerIndexVisibility[i] = newVisibility;
                if (newVisibility != oldVisibility)
                    needToRenderSmoke = true;
            }
            for (int i=0; i < smoke_var_v.Length; i++)
            {
                bool oldVisibility = smoke_di_v.RenderHints.PerIndexVisibility[i];
                bool newVisibility = smoke_var_v[i] < maxVal && smoke_var_v[i] > minVal;
                smoke_di_v.RenderHints.PerIndexVisibility[i] = newVisibility;
                if (newVisibility != oldVisibility)
                    needToRenderSmoke = true;
            }
        }

        if (needToRenderTree || needToRenderGround || needToRenderSmoke || needToRenderWind)
        {
            if (needToRenderWind)
                wind_di_h.RenderHints.StyleChanged = true;
                wind_di_v.RenderHints.StyleChanged = true;
            if (needToRenderGround)
                ground_di_h.RenderHints.StyleChanged = true;
                ground_di_v.RenderHints.StyleChanged = true;
            if (needToRenderTree)
                tree_di_h.RenderHints.StyleChanged = true;
                tree_di_v.RenderHints.StyleChanged = true;
            if (needToRenderSmoke)
                smoke_di_h.RenderHints.StyleChanged = true;
                smoke_di_v.RenderHints.StyleChanged = true;
            Debug.Log("Rendering in Update");
            ABREngine.Instance.Render();
        }      
    }

    void setText(string a){
        Slider[] sliders;
        Text[] slider_txt;
        if (a == "tree"){
            sliders = tree_sliders;
            slider_txt = tree_slid_txt;
        }else if (a == "ground"){
            sliders = ground_sliders;
            slider_txt = ground_slid_txt;
        }else if (a == "wind"){
            sliders = wind_sliders;
            slider_txt = wind_slid_txt;
        }else if (a == "smoke"){
            sliders = smoke_sliders;
            slider_txt = smoke_slid_txt;
        }else{
            Debug.Log("UNABLE TO SET TXT");
            return;
        }
        string txt0 = string.Format("{0:N0}", sliders[0].normalizedValue * 100) + "%";
        string txt1 = string.Format("{0:N0}", sliders[1].normalizedValue * 100) + "%";
        slider_txt[0].text = txt0;
        slider_txt[1].text = txt1;
    }
    
    ////// TOGGLE FUNCTIONS ////////
    public void smokeToggleValueChanged(bool newVal){
        smoke_di_h.RenderHints.Visible = newVal;
        smoke_di_v.RenderHints.Visible = newVal;
        smoke_sliders[0].gameObject.SetActive(newVal);
        smoke_sliders[1].gameObject.SetActive(newVal);
        ABREngine.Instance.Render();
    }

    public void windToggleValueChanged(bool newVal){
        wind_di_h.RenderHints.Visible = newVal;
        wind_di_v.RenderHints.Visible = newVal;
        wind_sliders[0].gameObject.SetActive(newVal);
        wind_sliders[1].gameObject.SetActive(newVal);
        ABREngine.Instance.Render();
    }

    public void groundToggleValueChanged(bool newVal){
        ground_di_h.RenderHints.Visible = newVal;
        ground_di_v.RenderHints.Visible = newVal;
        ground_sliders[0].gameObject.SetActive(newVal);
        ground_sliders[1].gameObject.SetActive(newVal);
        ABREngine.Instance.Render();
    }

    public void treeToggleValueChanged(bool newVal){
        tree_di_h.RenderHints.Visible = newVal;
        tree_di_v.RenderHints.Visible = newVal;
        // enable or disable cooresponding sliders
        tree_sliders[0].gameObject.SetActive(newVal);
        tree_sliders[1].gameObject.SetActive(newVal);
        ABREngine.Instance.Render();
    }

    public void headCurveFireToggled(bool newVal){
        tree_di_h.RenderHints.Visible = newVal;
        ground_di_h.RenderHints.Visible = newVal;
        smoke_di_h.RenderHints.Visible = newVal;
        wind_di_h.RenderHints.Visible = newVal;
        Debug.Log("head curve fire toggled");
        ABREngine.Instance.Render();

    }

    public void valleyFireToggled(bool newVal){
        tree_di_v.RenderHints.Visible = newVal;
        ground_di_v.RenderHints.Visible = newVal;
        smoke_di_v.RenderHints.Visible = newVal;
        wind_di_v.RenderHints.Visible = newVal;
        Debug.Log("valley fire toggled.");
        ABREngine.Instance.Render();

    }

    //------- TREE FUNCTIONS ---------- //
    //---------------------------------//
    // public void treeToggleValueChanged(bool newVal){
    //     tree_di_h.RenderHints.Visible = newVal;
    //     // enable or disable cooresponding sliders
    //     tree_sliders[0].gameObject.SetActive(newVal);
    //     tree_sliders[1].gameObject.SetActive(newVal);
    //     ABREngine.Instance.Render();
    // }

    // void renderTrees(){
    //     bool needToRender = false;
    //     Debug.Log(tree_sliders[0].value);
    //     Debug.Log(tree_sliders[1].value);
    //     float maxVal = tree_sliders[1].value;
    //     float minVal = tree_sliders[0].value;

    //     for (int i=0; i < tree_var_h.Length; i++)
    //     {
    //         bool oldVisibility = tree_di_h.RenderHints.PerIndexVisibility[i];
    //         bool newVisibility = tree_var_h[i] < maxVal && tree_var_h[i] > minVal;
    //         tree_di_h.RenderHints.PerIndexVisibility[i] = newVisibility;
    //         if (newVisibility != oldVisibility){
    //             needToRender = true;
    //             Debug.Log("Should Render");
    //         }
    //     }
    //     if (needToRender)
    //     {
    //         Debug.Log("RENDERING");
    //         tree_di_h.RenderHints.StyleChanged = true;
    //         ABREngine.Instance.Render();
    //     }
    // }

    

    // public void treeMinChanged(float value){
    //     Debug.Log("Tree Min changed");
    //     renderTrees();
    // }

    // public void treeMaxChanged(float value){
    //     //tree_minMax[1] = value;
    //     Debug.Log("Tree Max changed");

    //     renderTrees();
    // }

    // //------- WIND FUNCTIONS ---------- //
    // //---------------------------------//
    // public void windToggleValueChanged(bool newVal){
    //     wind_di_h.RenderHints.Visible = newVal;
    //     wind_sliders[0].gameObject.SetActive(newVal);
    //     wind_sliders[1].gameObject.SetActive(newVal);
    //     ABREngine.Instance.Render();
    // }

    // void renderWind(){
    //     bool needToRender = false;
    //     float maxVal = wind_sliders[1].value;
    //     float minVal = wind_sliders[0].value;
    //     for (int i=0; i < wind_var_h.Length; i++)
    //     {
    //         bool oldVisibility = wind_di_h.RenderHints.PerIndexVisibility[i];
    //         bool newVisibility = wind_var_h[i] < maxVal && wind_var_h[i] > minVal;
    //         wind_di_h.RenderHints.PerIndexVisibility[i] = newVisibility;
    //         if (newVisibility != oldVisibility)
    //             needToRender = true;
    //     }
    //     // if (needToRenderTree || needToRenderGround || needToRenderSmoke || needToRenderWind)
    //     // {
    //     //     if (needToRenderWind)
    //     //         wind_di_h.RenderHints.StyleChanged = true;
    //     //     if (needToRenderGround)
    //     //         ground_di_h.RenderHints.StyleChanged = true;
    //     //     if (needToRenderTree)
    //     //         tree_di_h.RenderHints.StyleChanged = true;
    //     //     if (needToRenderSmoke)
    //     //         smoke_di_h.RenderHints.StyleChanged = true;
            
    //     //     ABREngine.Instance.Render();
    //     //}
    // }

    // public void windMinChanged(float value){
    //     //wind_minMax[0] = value;
    //     renderWind();
    // }

    // public void windMaxChanged(float value){
    //     //wind_minMax[1] = value;
    //     renderWind();
    // }

    // //------- GROUND FUNCTIONS ---------- //
    // //---------------------------------//
    // public void groundToggleValueChanged(bool newVal){
    //     ground_di_h.RenderHints.Visible = newVal;
    //     ground_sliders[0].gameObject.SetActive(newVal);
    //     ground_sliders[1].gameObject.SetActive(newVal);
    //     ABREngine.Instance.Render();
    // }

    // void renderGround(){
    //     bool needToRender = false;
    //     float maxVal = ground_sliders[1].value;
    //     float minVal = ground_sliders[0].value;
    //     for (int i=0; i < ground_var_h.Length; i++)
    //     {
    //         Debug.Log(i);
    //         bool oldVisibility = ground_di_h.RenderHints.PerIndexVisibility[i];
    //         bool newVisibility = ground_var_h[i] < maxVal && ground_var_h[i] > minVal;
    //         ground_di_h.RenderHints.PerIndexVisibility[i] = newVisibility;
    //         if (newVisibility != oldVisibility)
    //             needToRender = true;
    //     }
    //     if (needToRender)
    //     {
    //         ground_di_h.RenderHints.StyleChanged = true;
    //         ABREngine.Instance.Render();
    //     }
    // }

    // public void groundMinChanged(float value){
    //     //ground_minMax[0] = value;
    //     renderGround();
    // }

    // public void groundMaxChanged(float value){
    //     //ground_minMax[1] = value;
    //     renderGround();
    // }

    // //------- SMOKE FUNCTIONS ---------- //
    // //---------------------------------//
    // public void smokeToggleValueChanged(bool newVal){
    //     smoke_di_h.RenderHints.Visible = newVal;
    //     smoke_sliders[0].gameObject.SetActive(newVal);
    //     smoke_sliders[1].gameObject.SetActive(newVal);
    //     ABREngine.Instance.Render();
    // }

    // void renderSmoke(){
    //     bool needToRender = false;
    //     Debug.Log("in smoke render 111");
    //     float maxVal = smoke_sliders[1].value;
    //     float minVal = smoke_sliders[0].value;
    //     for (int i=0; i < smoke_var_h.Length; i++)
    //     {
    //         Debug.Log("in smoke render 222");
    //         Debug.Log(i);
    //         bool oldVisibility = smoke_di_h.RenderHints.PerIndexVisibility[i];
    //         bool newVisibility = smoke_var_h[i] < maxVal && smoke_var_h[i] > minVal;
    //         smoke_di_h.RenderHints.PerIndexVisibility[i] = newVisibility;
    //         if (newVisibility != oldVisibility)
    //             needToRender = true;
    //     }
    //     if (needToRender)
    //     {
    //         Debug.Log("in smoke render 3333");
    //         smoke_di_h.RenderHints.StyleChanged = true;
    //         ABREngine.Instance.Render();
    //     }

    // }

    // public void smokeMinChanged(float value){
    //     //smoke_minMax[0] = value;
    //     Debug.Log("Smoke Min Changed");

    //     renderSmoke();
    // }

    // public void smokeMaxChanged(float value){
    //     //smoke_minMax[1] = value;
    //     Debug.Log("Smoke Max Changed");
    //     Debug.Log(value);
    //     renderSmoke();
    // }

}
