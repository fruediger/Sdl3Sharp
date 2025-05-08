using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

[DebuggerTypeProxy(typeof(DebuggerTypeProxy))]
partial class Properties
{
	[DebuggerDisplay("???", Name = $"{{{nameof(mName)},nq}}", Type = $"{{{nameof(mType)},nq}}")]
	private class DebuggerPropertyProxy
	{	
		[DebuggerDisplay($"{{{nameof(mValue)}}}", Name = $"{{{nameof(mName)},nq}}", Type = $"{{{nameof(mType)},nq}}")]
		private sealed class WithValue<T>(T? value, string name, PropertyType type) : DebuggerPropertyProxy(name, type)
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly T? mValue = value;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string mName;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly PropertyType mType;

		private DebuggerPropertyProxy(string name, PropertyType type) { mName = name; mType = type; }

		public static DebuggerPropertyProxy Create(string name, PropertyType type) => new(name, type);

		public static DebuggerPropertyProxy Create<T>(T? value, string name, PropertyType type) => new WithValue<T>(value, name, type);
	}

	private sealed class DebuggerTypeProxy(Properties properties)
	{
		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static void EnumeratePropertiesCallback(void* userdata, uint props, byte* name)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: List<DebuggerPropertyProxy> list })
			{
				var nameUtf16 = Utf8StringMarshaller.ConvertToManaged(name);

				if (!string.IsNullOrWhiteSpace(nameUtf16))
				{
					list.Add(SDL_GetPropertyType(props, name) switch
					{
						PropertyType.Pointer => DebuggerPropertyProxy.Create(unchecked((IntPtr)SDL_GetPointerProperty(props, name, default)), nameUtf16, PropertyType.Pointer),
						PropertyType.String => DebuggerPropertyProxy.Create(Utf8StringMarshaller.ConvertToManaged(SDL_GetStringProperty(props, name, default)), nameUtf16, PropertyType.String),
						PropertyType.Number => DebuggerPropertyProxy.Create(SDL_GetNumberProperty(props, name, default), nameUtf16, PropertyType.Number),
						PropertyType.Float => DebuggerPropertyProxy.Create(SDL_GetFloatProperty(props, name, default), nameUtf16, PropertyType.Float),
						PropertyType.Boolean => DebuggerPropertyProxy.Create((bool)SDL_GetBooleanProperty(props, name, default), nameUtf16, PropertyType.Boolean),
						var type => DebuggerPropertyProxy.Create(nameUtf16, type),
					});
				}
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public DebuggerPropertyProxy[] Properties
		{
			get
			{
				unsafe
				{
					var list = new List<DebuggerPropertyProxy>();
					var gcHandle = GCHandle.Alloc(list, GCHandleType.Normal);

					try
					{
						SDL_EnumerateProperties(properties.Id, &EnumeratePropertiesCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));
					}
					finally
					{
						gcHandle.Free();
					}

					return [..list];
				}
			}
		}
	}
}
