#region

using System;
using System.Collections;
using System.Diagnostics;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

namespace Settings {

	#endregion

	public class FPSDisplayPlusPlus : MonoBehaviour
	{
		[SerializeField] private float rectWidth;
		[SerializeField] private float rectHeight;
		[SerializeField] private Rect startRect;
		[SerializeField] private float fps;
		[SerializeField] private int maxFPS;
		[SerializeField] private int minFPS = 9999;
		[SerializeField] private bool updateColor = true;
		[SerializeField] private bool allowDrag = true;
		[SerializeField] private float frequency = 0.5F;
		[SerializeField] private bool moreOptionInUI = true;
		//[SerializeField] private bool moreOptionInEditor = true;
		[SerializeField] private string memsize;
		[SerializeField] private string totalGc; // GCMemSize
		[SerializeField] private string managedMemory; // GCReservedMemSize
		[SerializeField] private string sysmemsize;
		[SerializeField] private string cpuusage;
		[SerializeField] private string gfxUsedMemory;
		[SerializeField] private string gfxReservedMemory;
		[SerializeField] private string audioUsedMemory;
		[SerializeField] private string audioReservedMemory;
		[SerializeField] private string videoUsedMemory;
		[SerializeField] private string videoReservedMemory;
		[SerializeField] private string profilerUsedMemory;
		[SerializeField] private string profilerReservedMemory;

		private float _accum;
		private Color _color = Color.white;
		private int _frames;
		public GUIStyle _style;
		private ProfilerRecorder _gcReservedMemory;
		private ProfilerRecorder _gcUsedMemory;  
		private ProfilerRecorder _systemUsedMemory;
		private ProfilerRecorder _totalUsedMemory;
		private ProfilerRecorder _gfxUsedMemory;
		private ProfilerRecorder _gfxReservedMemory;
		private ProfilerRecorder _audioUsedMemory;
		private ProfilerRecorder _audioReservedMemory;
		private ProfilerRecorder _videoUsedMemory;
		private ProfilerRecorder _videoReservedMemory;
		private ProfilerRecorder _profilerUsedMemory;
		private ProfilerRecorder _profilerReservedMemory;

		private ProfilerRecorder _totalReservedMemoryRecorder;
		private void Start() {
			startRect =  new(10, 10, rectWidth, rectHeight);
			
			StartCoroutine(FPS());
			InvokeRepeating(nameof(GetMinMax), 1f, 1f);
			if (moreOptionInUI)
			{
				startRect = new(10, 10, rectWidth, rectHeight);
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
			_gcUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory");
			_gcReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
			_gfxUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Used Memory");
			_gfxReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Reserved Memory");
			_audioUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Used Memory");
			_audioReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Reserved Memory");
			_videoUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Video Used Memory");
			_videoReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Video Reserved Memory");
			_profilerUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Profiler Used Memory");
			_profilerReservedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Profiler Reserved Memory");
			_systemUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");

#if UNITY_EDITOR
			// if (moreOptionInEditor)
			// { // only on development builds or editor
			// 	_texturcount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Count");
			// 	_texturememory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Memory");
			// 	_meshMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Count");
			// 	_meshMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Memory");
			// 	_materialcount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Material Count");
			// 	_materialmemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Material Memory");
			// 	_animationClipCount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "AnimationClip Count");
			// 	_animationClipMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "AnimationClip Memory");
			// 	_assetcount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Asset Count");
			// 	_gOinscene = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GameObjects in Scenes");
			// 	_totalGOinscene = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Objects in Scenes");
			// 	_totalUnityOjectcount = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Unity Object Count");
			// 	_gcalloframecnt = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocation In Frame Count");
			// 	_gcallofrrame = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
			// }
#endif
		}
		private void OnDisable()
		{
			_totalReservedMemoryRecorder.Dispose();
			_totalUsedMemory.Dispose();
			_gcUsedMemory.Dispose();
			_gcReservedMemory.Dispose();
			_systemUsedMemory.Dispose();
			_gfxUsedMemory.Dispose();
			_gfxReservedMemory.Dispose();
			_audioUsedMemory.Dispose();
			_audioReservedMemory.Dispose();
			_videoUsedMemory.Dispose();
			_videoReservedMemory.Dispose();
			_profilerUsedMemory.Dispose();
			_profilerReservedMemory.Dispose();

#if UNITY_EDITOR
		
			_meshMemoryRecorder.Dispose();
			_meshMemory.Dispose();
			_texturcount.Dispose();
			_texturememory.Dispose();
			_materialcount.Dispose();
			_materialmemory.Dispose();
			_animationClipCount.Dispose();
			_animationClipMemory.Dispose();
			_assetcount.Dispose();
			_gOinscene.Dispose();
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
				totalGc = FormatSize(_gcUsedMemory.LastValue);
				managedMemory = FormatSize(_gcReservedMemory.LastValue);
				sysmemsize = FormatSize(_systemUsedMemory.LastValue);

				gfxUsedMemory = FormatSize(_gfxUsedMemory.LastValue);
				gfxReservedMemory = FormatSize(_gfxReservedMemory.LastValue);
				audioUsedMemory = FormatSize(_audioUsedMemory.LastValue);
				audioReservedMemory = FormatSize(_audioReservedMemory.LastValue);
				videoUsedMemory = FormatSize(_videoUsedMemory.LastValue);
				videoReservedMemory = FormatSize(_videoReservedMemory.LastValue);
				profilerUsedMemory = FormatSize(_profilerUsedMemory.LastValue);
				profilerReservedMemory = FormatSize(_profilerReservedMemory.LastValue);

#if UNITY_EDITOR
				// if (moreOptionInEditor)
				// {
				// 	texturCount = _texturcount.LastValue.ToString();
				// 	textureMemory = FormatSize(_texturememory.LastValue);
				// 	materialCount = _materialcount.LastValue.ToString();
				// 	materialMemory = FormatSize(_materialmemory.LastValue);
				// 	assetCount = _assetcount.LastValue.ToString();
				// 	gameObjectInScene = _gOinscene.LastValue.ToString();
				// 	totalGameObjectInScene = _totalGOinscene.LastValue.ToString();
				// 	totalUnityOjectCount = _totalUnityOjectcount.LastValue.ToString();
				// 	gcAllocFramecount = _gcalloframecnt.LastValue.ToString();
				// 	gcAllocInFrame = FormatSize(_gcallofrrame.LastValue);
				//
				//
				// 	animationClipCount = _animationClipCount.LastValue.ToString();
				// 	animationClipMemory = FormatSize(_animationClipMemory.LastValue);
				// 	meshMemoryRecorder = _meshMemoryRecorder.LastValue.ToString();
				// 	meshMemory = FormatSize(_meshMemory.LastValue);
				// }
#endif


				yield return new WaitForSeconds(frequency);
			}
		}

		private string FormatSize(double value, int decimalPlaces = 1)
		{
			string[] sizeSuffixes = { "b", "KB", "MB", "GB" };
			if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
			if (value < 0) { return "-" + FormatSize(-value, decimalPlaces); }
			if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }
			var mag = (int)Math.Log(value, 1024);
			var adjustedSize = (decimal)value / (1L << mag * 10);
			if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
			{
				mag += 1;
				adjustedSize /= 1024;
			}
			return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, sizeSuffixes[mag]);
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
			                                                             $"GC Used {totalGc}\n" +
			                                                             $"GC Res {managedMemory}\n" +
			                                                             $"Sys Mem {sysmemsize}\n" +
			                                                             "OS : "+Environment.OSVersion+"\n"
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
		private ProfilerRecorder _gOinscene;
		private ProfilerRecorder _totalGOinscene;
		private ProfilerRecorder _totalUnityOjectcount;
		private ProfilerRecorder _gcalloframecnt;
		private ProfilerRecorder _gcallofrrame;

		private ProfilerRecorder _meshMemoryRecorder;
		private ProfilerRecorder _animationClipCount;
		private ProfilerRecorder _animationClipMemory;
		private ProfilerRecorder _meshMemory;

		[SerializeField] private string texturCount;
		[SerializeField] private string textureMemory;
		[SerializeField] private string materialCount;
		[SerializeField] private string materialMemory;
		[SerializeField] private string assetCount;
		[SerializeField] private string gameObjectInScene;
		[SerializeField] private string totalGameObjectInScene;
		[SerializeField] private string totalUnityOjectCount;
		[SerializeField] private string gcAllocFramecount;
		[SerializeField] private string gcAllocInFrame;

		[SerializeField] private string meshMemoryRecorder;
		[SerializeField] private string animationClipCount;
		[SerializeField] private string animationClipMemory;
		[SerializeField] private string meshMemory;
#endif
	}

	internal class UsageRamProc
	{
		private static DateTime _lastTime;
		private static TimeSpan _lastTotalProcessorTime;
		private static DateTime _curTime;
		private static TimeSpan _curTotalProcessorTime;
		private readonly Process _pp = Process.GetCurrentProcess();

		public string GetCpuPercentage()
		{
			if (_lastTime == null || _lastTime == new DateTime())
			{
				_lastTime = DateTime.Now;
				_lastTotalProcessorTime = _pp.TotalProcessorTime;
				return "";
			}
			_curTime = DateTime.Now;
			_curTotalProcessorTime = _pp.TotalProcessorTime;

			var cpuUsage = (_curTotalProcessorTime.TotalMilliseconds - _lastTotalProcessorTime.TotalMilliseconds) / _curTime.Subtract(_lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);

			_lastTime = _curTime;
			_lastTotalProcessorTime = _curTotalProcessorTime;
			return $"{cpuUsage * 100:0.0}";
		}
	}
}