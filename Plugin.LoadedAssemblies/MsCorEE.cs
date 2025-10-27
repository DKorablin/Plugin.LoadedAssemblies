using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Plugin.LoadedAssemblies
{
	internal static class MsCorEE
	{
#if NET35
		public static IEnumerable<_AppDomain> EnumAppDomains()
		{
			// Obtain ICLRMetaHost interface
			Int32 hr = CLRCreateInstance(ref CLSID_CLRMetaHost, ref IID_CLRMetaHost, out Object objHost);
			if(hr < 0)
				throw new COMException("Cannot create meta host", hr);

			// Obtain ICLRRuntimeInfo interface
			Version vers = Environment.Version;
			String versString = String.Format("v{0}.{1}.{2}", vers.Major, vers.Minor, vers.Build);

			ICLRMetaHost host = (ICLRMetaHost)objHost;
			Object objRuntime = host.GetRuntime(versString, ref IID_CLRRuntimeInfo);
			ICLRRuntimeInfo runtime = (ICLRRuntimeInfo)objRuntime;
			runtime.IsStarted(out Boolean started, out _);
			if(!started) throw new COMException("CLR not started??");

			// Obtain legacy ICorRuntimeHost interface and iterate appdomains
			ICorRuntimeHost v2Host = (ICorRuntimeHost)runtime.GetInterface(ref CLSID_CorRuntimeHost, ref IID_CorRuntimeHost);
			v2Host.EnumDomains(out IntPtr hDomainEnum);
			for(; ; )
			{
				v2Host.NextDomain(hDomainEnum, out _AppDomain domain);
				if(domain == null) break;
				yield return domain;
			}
			v2Host.CloseEnum(hDomainEnum);
		}

		private static Guid CLSID_CLRMetaHost = new Guid(0x9280188d, 0xe8e, 0x4867, 0xb3, 0xc, 0x7f, 0xa8, 0x38, 0x84, 0xe8, 0xde);
		private static Guid IID_CLRMetaHost = new Guid(0xD332DB9E, 0xB9B3, 0x4125, 0x82, 0x07, 0xA1, 0x48, 0x84, 0xF5, 0x32, 0x16);
		private static Guid IID_CLRRuntimeInfo = new Guid(0xBD39D1D2, 0xBA2F, 0x486a, 0x89, 0xB0, 0xB4, 0xB0, 0xCB, 0x46, 0x68, 0x91);
		private static Guid CLSID_CorRuntimeHost = new Guid(0xcb2f6723, 0xab3a, 0x11d2, 0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e);
		private static Guid IID_CorRuntimeHost = new Guid(0xcb2f6722, 0xab3a, 0x11d2, 0x9c, 0x40, 0x00, 0xc0, 0x4f, 0xa3, 0x0a, 0x3e);

		[DllImport("mscoree.dll")]
		private static extern Int32 CLRCreateInstance(ref Guid clsid, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out Object ptr);

		[ComImport]
		[Guid("D332DB9E-B9B3-4125-8207-A14884F53216")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface ICLRMetaHost
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			Object GetRuntime(String version, ref Guid iid);
			// Rest omitted
		}

		[ComImport]
		[Guid("BD39D1D2-BA2F-486a-89B0-B4B0CB466891")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface ICLRRuntimeInfo
		{
			void GetVersionString(Char[] buffer, Int32 bufferLength);
			void GetRuntimeDirectory(Char[] buffer, Int32 bufferLength);
			Boolean IsLoaded(IntPtr hProcess);
			void LoadErrorString(UInt32 id, Char[] buffer, Int32 bufferLength, Int32 lcid);
			void LoadLibrary(String path, out IntPtr hMdodule);
			void GetProcAddress(String name, out IntPtr addr);
			[return: MarshalAs(UnmanagedType.Interface)]
			Object GetInterface(ref Guid clsid, ref Guid iid);
			Boolean IsLoadable();
			void SetDefaultStartupFlags(UInt32 flags, String configFile);
			void GetDefaultStartupFlags(out UInt32 flags, Char[] configFile, Int32 configFileLength);
			void BindAsLegacyV2Runtime();
			void IsStarted(out Boolean started, out UInt32 flags);
		}

		[ComImport]
		[Guid("CB2F6722-AB3A-11d2-9C40-00C04FA30A3E")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface ICorRuntimeHost
		{
			void CreateLogicalThreadState();
			void DeleteLogicalThreadState();
			void SwitchinLogicalThreadState(IntPtr cookie);
			void SwitchoutLogicalThreadState(out IntPtr cookie);
			void LocksHeldByLogicalThread(out Int32 count);
			void MapFile(IntPtr hFile, out IntPtr address);
			void GetConfiguration(out IntPtr config);
			void Start();
			void Stop();
			void CreateDomain(String name, Object identity, out _AppDomain domain);
			void GetDefaultDomain(out _AppDomain domain);
			void EnumDomains(out IntPtr hEnum);
			void NextDomain(IntPtr hEnum, out _AppDomain domain);
			void CloseEnum(IntPtr hEnum);
			// rest omitted
		}
#endif
	}
}