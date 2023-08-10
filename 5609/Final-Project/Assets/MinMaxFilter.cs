using UnityEngine;
using IVLab.ABREngine;

public class MinMaxFilter : MonoBehaviour
{
    [SerializeField]
    public float minValue;

    [SerializeField]
    public float maxValue;

    public ScalarDataVariable voxelVar;

    private SimpleVolumeDataImpression di;

    private bool varsInitialized = false;

    void Update()
    {
        // Fetch a reference to the DI from the engine (assume the first volume is the one we want)
        if (di == null)
        {
            var impressions = ABREngine.Instance.GetDataImpressions<SimpleVolumeDataImpression>();
            if (impressions.Count > 0)
                di = impressions[0];
        }

        // Can't continue without key data (or the data impression, for that matter)
        if (di?.keyData == null)
        {
            return;
        }

        // Assume that the first var is the one we want, because that's how Volume data are set up
        voxelVar = di.keyData.GetScalarVariables()[0];
        float[] voxels = voxelVar.GetArray(di.keyData);

        // Set min/max to filter if they aren't already
        if (!varsInitialized)
        {
            minValue = voxelVar.Range.min;
            maxValue = voxelVar.Range.max;
            varsInitialized = true;
        }

        // Set up PerIndexVisibility if it isn't already
        if (!di.RenderHints.HasPerIndexVisibility())
        {
            di.RenderHints.PerIndexVisibility = new System.Collections.BitArray(voxels.Length, true);
        }

        // Loop through each voxel and set per-index visibility based on value
        bool needToRender = false;
        for (int voxel = 0; voxel < voxels.Length; voxel++)
        {
            bool oldVisibility = di.RenderHints.PerIndexVisibility[voxel];
            bool newVisibility = voxels[voxel] < maxValue && voxels[voxel] > minValue;
            di.RenderHints.PerIndexVisibility[voxel] = newVisibility;
            if (newVisibility != oldVisibility)
                needToRender = true;
        }

        if (needToRender)
        {
            di.RenderHints.StyleChanged = true;
            ABREngine.Instance.Render();
        }
    }
}