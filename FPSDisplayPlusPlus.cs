#region

using System;
using System.Collections;
using System.Diagnostics;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

#endregion

public class FPSDisplayPlusPlus : MonoBehaviour
{
    [SerializeField] private Rect startRect = new Rect(10, 10, 145, 50);
    [SerializeField] private float fps;
    [SerializeField] private int maxFPS;
    [SerializeField] private int minFPS = 9999;
    [SerializeField] private bool updateColor = true;
    [SerializeField] private bool allowDrag = true;
    [SerializeField] private float frequency = 0.5F;
    [SerializeField] private bool moreOptionInUI = true;
    [SerializeField] private bool moreOptionInEditor = true;
    [SerializeField] private string memsize;
    [SerializeField] private string TotalGc; // GCMemSize
    [SerializeField] private string ManagedMemory; // GCReservedMemSize
    [SerializeField] private string Sysmemsize;
    [SerializeField] private string cpuusage;
	[SerializeField] private string Gfx_Used_Memory;
    [SerializeField] private string Gfx_Reserved_Memory;
    [SerializeField] private string Audio_Used_Memory;
    [SerializeField] private string Audio_Reserved_Memory;
    [SerializeField] private string Video_Used_Memory;
    [SerializeField] private string Video_Reserved_Memory;
    [SerializeField] private string Profiler_Used_Memory;
    [SerializeField] private string Profiler_Reserved_Memory;

    private float _accum;
    private Color _color = Color.white;
    private int _frames;
	private GUIStyle _style;
	private ProfilerRecorder _GCReservedMemory;
    private ProfilerRecorder _GCUsedMemory;  
    private ProfilerRecorder _SystemUsedMemory;
    private ProfilerRecorder _totalUsedMemory;
	private ProfilerRecorder _GfxUsedMemory;
	private ProfilerRecorder _GfxReservedMemory;
	private ProfilerRecorder _AudioUsedMemory;
	private ProfilerRecorder _AudioReservedMemory;
	private ProfilerRecorder _VideoUsedMemory;
	private ProfilerRecorder _VideoReservedMemory;
	private ProfilerRecorder _ProfilerUsedMemory;
	private ProfilerRecorder _ProfilerReservedMemory;

	private ProfilerRecorder _totalReservedMemoryRecorder;
	private void Start()
    {
        StartCoroutine(FPS());
        InvokeRepeating(nameof(GetMinMax), 1f, 1f);
        if (moreOptionInUI)
        {
            startRect = new(10, 10, 145, 130);
            StartCoroutine(Stats());
        }
    }

    private void Update()
    {
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;
    }

    private void OnEnable()
	{ //  that the metric is available on Release builds
		_totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
		_totalUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        _GCUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory");
        _GCReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
		_GfxUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Used Memory");
		_GfxReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Reserved Memory");
		_AudioUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Used Memory");
		_AudioReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Reserved Memory");
		_VideoUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Video Used Memory");
		_VideoReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Video Reserved Memory");
		_ProfilerUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Profiler Used Memory");
		_ProfilerReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Profiler Reserved Memory");
        _SystemUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");

#if UNITY_EDITOR
        if (moreOptionInEditor)
		{ // only on development builds or editor
			_texturcount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Count");
			_texturememory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Memory");
			_meshMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Count");
			_MeshMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Memory");
			_materialcount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Material Count");
            _materialmemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Material Memory");
			_AnimationClipCount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "AnimationClip Count");
			_AnimationClipMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "AnimationClip Memory");
            _assetcount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Asset Count");
            _GOinscene = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GameObjects in Scenes");
            _totalGOinscene = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Objects in Scenes");
            _totalUnityOjectcount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Unity Object Count");
            _gcalloframecnt = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocation In Frame Count");
            _gcallofrrame = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
        }
#endif
    }
    private void OnDisable()
    {
		_totalReservedMemoryRecorder.Dispose();
		_totalUsedMemory.Dispose();
        _GCUsedMemory.Dispose();
        _GCReservedMemory.Dispose();
        _SystemUsedMemory.Dispose();
        _GfxUsedMemory.Dispose();
		_GfxReservedMemory.Dispose();
		_AudioUsedMemory.Dispose();
		_AudioReservedMemory.Dispose();
		_VideoUsedMemory.Dispose();
		_VideoReservedMemory.Dispose();
		_ProfilerUsedMemory.Dispose();
		_ProfilerReservedMemory.Dispose();

#if UNITY_EDITOR
		
		_meshMemoryRecorder.Dispose();
        _MeshMemory.Dispose();
		_texturcount.Dispose();
        _texturememory.Dispose();
        _materialcount.Dispose();
        _materialmemory.Dispose();
		_AnimationClipCount.Dispose();
        _AnimationClipMemory.Dispose();
		_assetcount.Dispose();
        _GOinscene.Dispose();
        _totalGOinscene.Dispose();
        _totalUnityOjectcount.Dispose();
        _gcalloframecnt.Dispose();
        _gcallofrrame.Dispose();
#endif
    }

    private void OnGUI()
    {
        if (_style == null)
        {
            _style = new(GUI.skin.label);
            _style.normal.textColor = Color.white;
            _style.alignment = TextAnchor.MiddleCenter;
        }
        GUI.color = updateColor ? _color : Color.white;
        if (!moreOptionInUI) startRect = ClampToScreen(GUI.Window(0, startRect, FPSWindow, ""));
        else startRect = ClampToScreen(GUI.Window(0, startRect, FPSWindow, ""));
        }

#if UNITY_EDITOR
    [MenuItem("GameObject/Utils/FPSDisplayPlusPlus", false, 10)]
    private static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        var go = new GameObject("FPSDisplayPlusPlus");
        go.AddComponent<FPSDisplayPlusPlus>();
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
#endif

    private void GetMinMax()
    {
        if (maxFPS < fps)
            maxFPS = (int)Mathf.Max(fps);
        if (minFPS > fps)
            minFPS = (int)Mathf.Min(fps);
    }

    private IEnumerator FPS()
    {
        while (true)
        {
            fps = _accum / _frames;
            _color = fps >= 30 ? new(0, 255, 0) : fps > 10 ? new(255, 0, 0) : new Color(255, 165, 0);
            _accum = 0;
            _frames = 0;
            yield return new WaitForSeconds(frequency);
        }
    }

    private IEnumerator Stats()
    {
        while (true)
        {
            var usage = new UsageRamProc();
            cpuusage = usage.GetCpuPercentage();
            memsize = FormatSize(_totalUsedMemory.LastValue);
			TotalGc = FormatSize(_GCUsedMemory.LastValue);
			ManagedMemory = FormatSize(_GCReservedMemory.LastValue);
            Sysmemsize = FormatSize(_SystemUsedMemory.LastValue);

			Gfx_Used_Memory = FormatSize(_GfxUsedMemory.LastValue);
			Gfx_Reserved_Memory = FormatSize(_GfxReservedMemory.LastValue);
			Audio_Used_Memory = FormatSize(_AudioUsedMemory.LastValue);
			Audio_Reserved_Memory = FormatSize(_AudioReservedMemory.LastValue);
			Video_Used_Memory = FormatSize(_VideoUsedMemory.LastValue);
			Video_Reserved_Memory = FormatSize(_VideoReservedMemory.LastValue);
			Profiler_Used_Memory = FormatSize(_ProfilerUsedMemory.LastValue);
			Profiler_Reserved_Memory = FormatSize(_ProfilerReservedMemory.LastValue);

#if UNITY_EDITOR
			if (moreOptionInEditor)
            {
                textur_count = _texturcount.LastValue.ToString();
                texture_memory = FormatSize(_texturememory.LastValue);
                material_count = _materialcount.LastValue.ToString();
                material_memory = FormatSize(_materialmemory.LastValue);
                asset_count = _assetcount.LastValue.ToString();
                GameObject_In_Scene = _GOinscene.LastValue.ToString();
                total_GameObject_in_scene = _totalGOinscene.LastValue.ToString();
                total_Unity_Oject_count = _totalUnityOjectcount.LastValue.ToString();
                gc_alloc_framecount = _gcalloframecnt.LastValue.ToString();
                gc_alloc_in_frame = FormatSize(_gcallofrrame.LastValue);

				
				Animation_Clip_Count = _AnimationClipCount.LastValue.ToString();
                Animation_Clip_Memory = FormatSize(_AnimationClipMemory.LastValue);
                mesh_Memory_Recorder = _meshMemoryRecorder.LastValue.ToString();
				Mesh_Memory = FormatSize(_MeshMemory.LastValue);
			}
#endif


            yield return new WaitForSeconds(frequency);
        }
    }

    private string FormatSize(double value, int decimalPlaces = 1)
    {
        string[] SizeSuffixes = { "b", "KB", "MB", "GB" };
        if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
        if (value < 0) { return "-" + FormatSize(-value, decimalPlaces); }
        if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }
        int mag = (int)Math.Log(value, 1024);
        decimal adjustedSize = (decimal)value / (1L << mag * 10);
        if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
        {
            mag += 1;
            adjustedSize /= 1024;
        }
        return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
    }

    private void FPSWindow(int windowID)
    {
        if (allowDrag) GUI.DragWindow(new(0, 0, Screen.width, Screen.height));
        
        if (!moreOptionInUI) GUI.Label(new(0, 0, startRect.width, startRect.height), $"FPS Display\n{Mathf.Round(fps)} FPS\n[{minFPS}Min {maxFPS}Max]", _style);
        else GUI.Label(new(0, 0, startRect.width, startRect.height), $"" +
            $"FPS Display\n{Mathf.Round(fps)} " +
            $"FPS\n[{minFPS}Min {maxFPS}Max]\n" +
            $"Cpu {cpuusage}% \n" +
            $"Mem {memsize}\n" +
            $"GC Used {TotalGc}\n" +
            $"GC Res {ManagedMemory}\n" +
            $"Sys Mem {Sysmemsize}"
            , _style);
    }

    private Rect ClampToScreen(Rect r) {
        r.x = Mathf.Clamp(r.x, 0, Screen.width - r.width);
        r.y = Mathf.Clamp(r.y, 0, Screen.height - r.height);
        return r;
    }

#if UNITY_EDITOR
    private ProfilerRecorder _texturcount;
    private ProfilerRecorder _texturememory;
    private ProfilerRecorder _materialcount;
    private ProfilerRecorder _materialmemory;
    private ProfilerRecorder _assetcount;
    private ProfilerRecorder _GOinscene;
    private ProfilerRecorder _totalGOinscene;
    private ProfilerRecorder _totalUnityOjectcount;
    private ProfilerRecorder _gcalloframecnt;
    private ProfilerRecorder _gcallofrrame;

	private ProfilerRecorder _meshMemoryRecorder;
    private ProfilerRecorder _AnimationClipCount;
    private ProfilerRecorder _AnimationClipMemory;
    private ProfilerRecorder _MeshMemory;

	[SerializeField] private string textur_count;
    [SerializeField] private string texture_memory;
    [SerializeField] private string material_count;
    [SerializeField] private string material_memory;
    [SerializeField] private string asset_count;
    [SerializeField] private string GameObject_In_Scene;
    [SerializeField] private string total_GameObject_in_scene;
    [SerializeField] private string total_Unity_Oject_count;
    [SerializeField] private string gc_alloc_framecount;
    [SerializeField] private string gc_alloc_in_frame;

	[SerializeField] private string mesh_Memory_Recorder;
	[SerializeField] private string Animation_Clip_Count;
	[SerializeField] private string Animation_Clip_Memory;
	[SerializeField] private string Mesh_Memory;
#endif
}

internal class UsageRamProc
{
    private static DateTime lastTime;
    private static TimeSpan lastTotalProcessorTime;
    private static DateTime curTime;
    private static TimeSpan curTotalProcessorTime;
    private readonly Process pp = Process.GetCurrentProcess();

    public string GetCpuPercentage()
    {
        if (lastTime == null || lastTime == new DateTime())
        {
            lastTime = DateTime.Now;
            lastTotalProcessorTime = pp.TotalProcessorTime;
            return "";
        }
        curTime = DateTime.Now;
        curTotalProcessorTime = pp.TotalProcessorTime;

        double CPUUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) / curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);

        lastTime = curTime;
        lastTotalProcessorTime = curTotalProcessorTime;
        return $"{CPUUsage * 100:0.0}";
    }
}
