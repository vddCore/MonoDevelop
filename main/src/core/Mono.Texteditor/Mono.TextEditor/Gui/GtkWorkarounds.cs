//
// GtkWorkarounds.cs
//
// Authors: Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (C) 2011 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Gtk;

namespace Mono.TextEditor
{
	public static class GtkWorkarounds
	{
		const string USER32DLL = "User32.dll";
		
		static System.Reflection.MethodInfo glibObjectGetProp, glibObjectSetProp;
		
		public static int GtkMinorVersion = 12, GtkMicroVersion = 0;
		
		static GtkWorkarounds ()
		{
			var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
			glibObjectSetProp = typeof (GLib.Object).GetMethod ("SetProperty", flags);
			glibObjectGetProp = typeof (GLib.Object).GetMethod ("GetProperty", flags);
			
			foreach (int i in new [] { 24, 22, 20, 18, 16, 14 }) {
				if (Gtk.Global.CheckVersion (2, (uint)i, 0) == null) {
					GtkMinorVersion = i;
					break;
				}
			}
			
			for (int i = 1; i < 20; i++) {
				if (Gtk.Global.CheckVersion (2, (uint)GtkMinorVersion, (uint)i) == null) {
					GtkMicroVersion = i;
				} else {
					break;
				}
			}
			
			keymap.KeysChanged += delegate {
				mappedKeys.Clear ();
			};
		}
		
		// Note: we can't reuse RectangleF because the layout is different...
		[StructLayout (LayoutKind.Sequential)]
		struct Rect {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public int X { get { return Left; } }
			public int Y { get { return Top; } }
			public int Width { get { return Right - Left; } }
			public int Height { get { return Bottom - Top; } }
		}

		const int MonitorInfoFlagsPrimary = 0x01;

		[StructLayout (LayoutKind.Sequential)]
		unsafe struct MonitorInfo {
			public int Size;
			public Rect Frame;         // Monitor
			public Rect VisibleFrame;  // Work
			public int Flags;
			public fixed byte Device[32];
		}

		[UnmanagedFunctionPointer (CallingConvention.Winapi)]
		delegate int EnumMonitorsCallback (IntPtr hmonitor, IntPtr hdc, IntPtr prect, IntPtr user_data);

		[DllImport (USER32DLL)]
		extern static int EnumDisplayMonitors (IntPtr hdc, IntPtr clip, EnumMonitorsCallback callback, IntPtr user_data);

		[DllImport (USER32DLL)]
		extern static int GetMonitorInfoA (IntPtr hmonitor, ref MonitorInfo info);

		static Gdk.Rectangle WindowsGetUsableMonitorGeometry (Gdk.Screen screen, int monitor_id)
		{
			Gdk.Rectangle geometry = screen.GetMonitorGeometry (monitor_id);
			List<MonitorInfo> screens = new List<MonitorInfo> ();

			EnumDisplayMonitors (IntPtr.Zero, IntPtr.Zero, delegate (IntPtr hmonitor, IntPtr hdc, IntPtr prect, IntPtr user_data) {
				var info = new MonitorInfo ();

				unsafe {
					info.Size = sizeof (MonitorInfo);
				}

				GetMonitorInfoA (hmonitor, ref info);

				// In order to keep the order the same as Gtk, we need to put the primary monitor at the beginning.
				if ((info.Flags & MonitorInfoFlagsPrimary) != 0)
					screens.Insert (0, info);
				else
					screens.Add (info);

				return 1;
			}, IntPtr.Zero);

			MonitorInfo monitor = screens[monitor_id];
			Rect visible = monitor.VisibleFrame;
			Rect frame = monitor.Frame;

			// Rebase the VisibleFrame off of Gtk's idea of this monitor's geometry (since they use different coordinate systems)
			int x = geometry.X + (visible.Left - frame.Left);
			int width = visible.Width;

			int y = geometry.Y + (visible.Top - frame.Top);
			int height = visible.Height;

			return new Gdk.Rectangle (x, y, width, height);
		}
		
		public static Gdk.Rectangle GetUsableMonitorGeometry (this Gdk.Screen screen, int monitor)
		{
			if (Platform.IsWindows)
				return WindowsGetUsableMonitorGeometry (screen, monitor);

			return screen.GetMonitorGeometry (monitor);
		}
		
		public static int RunDialogWithNotification (Gtk.Dialog dialog)
		{
			return dialog.Run ();
		}
		
		public static void PresentWindowWithNotification (this Gtk.Window window)
		{
			window.Present ();
		}
		
		public static GLib.Value GetProperty (this GLib.Object obj, string name)
		{
			return (GLib.Value) glibObjectGetProp.Invoke (obj, new object[] { name });
		}
		
		public static void SetProperty (this GLib.Object obj, string name, GLib.Value value)
		{
			glibObjectSetProp.Invoke (obj, new object[] { name, value });
		}
		
		public static bool TriggersContextMenu (this Gdk.EventButton evt)
		{
			return evt.Type == Gdk.EventType.ButtonPress && IsContextMenuButton (evt);
		}
		
		public static bool IsContextMenuButton (this Gdk.EventButton evt)
		{
			if (evt.Button == 3 &&
					(evt.State & (Gdk.ModifierType.Button1Mask | Gdk.ModifierType.Button2Mask)) == 0)
				return true;
			
			return false;
		}

		public static Gdk.ModifierType GetCurrentKeyModifiers ()
		{
			Gdk.ModifierType mtype;
			Gtk.Global.GetCurrentEventState (out mtype);
			return mtype;
		}
		
		public static void GetPageScrollPixelDeltas (this Gdk.EventScroll evt, double pageSizeX, double pageSizeY,
			out double deltaX, out double deltaY)
		{
			if (!GetEventScrollDeltas (evt, out deltaX, out deltaY)) {
				var direction = evt.Direction;
				deltaX = deltaY = 0;
				if (pageSizeY != 0 && (direction == Gdk.ScrollDirection.Down || direction == Gdk.ScrollDirection.Up)) {
					deltaY = System.Math.Pow (pageSizeY, 2.0 / 3.0);
					deltaX = 0.0;
					if (direction == Gdk.ScrollDirection.Up)
						deltaY = -deltaY;
				} else if (pageSizeX != 0) {
					deltaX = System.Math.Pow (pageSizeX, 2.0 / 3.0);
					deltaY = 0.0;
					if (direction == Gdk.ScrollDirection.Left)
						deltaX = -deltaX;
				}
			}
		}
		
		public static void AddValueClamped (this Gtk.Adjustment adj, double value)
		{
			adj.Value = System.Math.Max (adj.Lower, System.Math.Min (adj.Value + value, adj.Upper - adj.PageSize));
		}
		
		[DllImport (PangoUtil.LIBGTK, CallingConvention = CallingConvention.Cdecl)]
		extern static bool gdk_event_get_scroll_deltas (IntPtr eventScroll, out double deltaX, out double deltaY);
		static bool scrollDeltasNotSupported;
		
		public static bool GetEventScrollDeltas (Gdk.EventScroll evt, out double deltaX, out double deltaY)
		{
			if (!scrollDeltasNotSupported) {
				try {
					return gdk_event_get_scroll_deltas (evt.Handle, out deltaX, out deltaY);
				} catch (EntryPointNotFoundException) {
					scrollDeltasNotSupported = true;
				}
			}
			deltaX = deltaY = 0;
			return false;
		}

		static void ShowContextMenuInternal (Gtk.Menu menu, Gtk.Widget parent, int ix, int iy, Gdk.Rectangle caret, Gdk.Window window, uint time, uint button)
		{
			Gtk.MenuPositionFunc posFunc = null;

			if (parent != null) {
				menu.AttachToWidget (parent, null);
				menu.Hidden += (sender, e) => {
					if (menu.AttachWidget != null)
						menu.Detach ();
				};

				posFunc = delegate (Gtk.Menu m, out int x, out int y, out bool pushIn) {
					x = ix;
					y = iy;

					Gtk.Requisition request = m.SizeRequest ();
					var screen = parent.Screen;
					Gdk.Rectangle geometry = GetUsableMonitorGeometry (screen, screen.GetMonitorAtPoint (x, y));

					//whether to push or flip menus that would extend offscreen
					//FIXME: this is the correct behaviour for mac, check other platforms
					bool flip_left = true;
					bool flip_up   = false;

					if (x + request.Width > geometry.X + geometry.Width) {
						if (flip_left)
							x -= request.Width;
						else
							x = geometry.X + geometry.Width - request.Width;

						if (x < geometry.Left)
							x = geometry.Left;
					}

					if (y + request.Height > geometry.Y + geometry.Height) {
						if (flip_up)
							y -= request.Height;
						else
							y = geometry.Y + geometry.Height - request.Height;

						if (y < geometry.Top)
							y = geometry.Top;
					}

					pushIn = false;
				};
			}

			menu.Popup (null, null, posFunc, button, time);
		}

		public static void ShowContextMenu (Gtk.Menu menu, Gtk.Widget parent, Gdk.EventButton evt, Gdk.Rectangle caret)
		{
			int x, y;
			uint time, button;

			var window = evt != null ? evt.Window : parent.GdkWindow;

			if (window == null)
				return;

			window.GetOrigin (out x, out y);

			if (evt == null) {
				evt = Global.CurrentEvent as Gdk.EventButton;
			}

			if (evt != null) {
				button = evt.Button;
				time = evt.Time;
				x += (int)evt.X;
				y += (int)evt.Y;
			} else {
				button = 3;
				time = 0;
				x += caret.X;
				y += caret.Y;
			}

			ShowContextMenuInternal (menu, parent, x, y, caret, window, time, button);
		}

		public static void ShowContextMenu (Gtk.Menu menu, Gtk.Widget parent, int ix, int iy, Gdk.Rectangle caret)
		{
			int x, y;
			var window = parent.GdkWindow;
			var alloc = parent.Allocation;

			window.GetOrigin (out x, out y);
			x += ix;
			y += iy;

			if (caret.X >= alloc.X && caret.Y >= alloc.Y) {
				x += caret.X;
				y += caret.Y;
			} else {
				x += alloc.X;
				y += alloc.Y;
			}

			ShowContextMenuInternal (menu, parent, x, y, caret, window, Gtk.Global.CurrentEventTime, 0);
		}
		
		public static void ShowContextMenu (Gtk.Menu menu, Gtk.Widget parent, Gdk.EventButton evt)
		{
			ShowContextMenu (menu, parent, evt, Gdk.Rectangle.Zero);
		}

		public static void ShowContextMenu (Gtk.Menu menu, Gtk.Widget parent, int x, int y)
		{
			ShowContextMenu (menu, parent, x, y, Gdk.Rectangle.Zero);
		}
		
		public static void ShowContextMenu (Gtk.Menu menu, Gtk.Widget parent, Gdk.Rectangle caret)
		{
			ShowContextMenu (menu, parent, null, caret);
		}
		
		struct MappedKeys
		{
			public Gdk.Key Key;
			public Gdk.ModifierType State;
			public KeyboardShortcut[] Shortcuts;
		}
		
		//introduced in GTK 2.20
		[DllImport (PangoUtil.LIBGDK, CallingConvention = CallingConvention.Cdecl)]
		extern static bool gdk_keymap_add_virtual_modifiers (IntPtr keymap, ref Gdk.ModifierType state);
		
		static Gdk.Keymap keymap = Gdk.Keymap.Default;
		static Dictionary<ulong,MappedKeys> mappedKeys = new Dictionary<ulong,MappedKeys> ();
		
		/// <summary>Map raw GTK key input to work around platform bugs and decompose accelerator keys</summary>
		/// <param name='evt'>The raw key event</param>
		/// <param name='key'>The composed key</param>
		/// <param name='state'>The composed modifiers</param>
		/// <param name='shortcuts'>All the key/modifier decompositions that can be used as accelerators</param>
		public static void MapKeys (Gdk.EventKey evt, out Gdk.Key key, out Gdk.ModifierType state,
		                            out KeyboardShortcut[] shortcuts)
		{
			//this uniquely identifies the raw key
			ulong id;
			unchecked {
				id = (((ulong)(uint)evt.State) | (((ulong)evt.HardwareKeycode) << 32) | (((ulong)evt.Group) << 48));
			}

			bool remapKey = Platform.IsWindows && evt.HardwareKeycode == 0;
			MappedKeys mapped;
			if (remapKey || !mappedKeys.TryGetValue (id, out mapped))
				mappedKeys[id] = mapped = MapKeys (evt);
			
			shortcuts = mapped.Shortcuts;
			state = mapped.State;
			key = mapped.Key;

			if (remapKey) {
				key = (Gdk.Key)evt.KeyValue;
			}
		}
		
		static MappedKeys MapKeys (Gdk.EventKey evt)
		{
			MappedKeys mapped;
			Gdk.ModifierType modifier = evt.State;
			byte grp = evt.Group;
			
			if (GtkMinorVersion >= 20) {
				gdk_keymap_add_virtual_modifiers (keymap.Handle, ref modifier);
			}
			
			//full key mapping
			uint keyval;
			int effectiveGroup, level;
			Gdk.ModifierType consumedModifiers;
			TranslateKeyboardState (evt, modifier, grp, out keyval, out effectiveGroup,
				out level, out consumedModifiers);
			mapped.Key = (Gdk.Key)keyval;
            mapped.State = evt.State & ~consumedModifiers;

            //decompose the key into accel combinations
            var accelList = new List<KeyboardShortcut> ();
			
			const Gdk.ModifierType accelMods = Gdk.ModifierType.ShiftMask | Gdk.ModifierType.Mod1Mask
				| Gdk.ModifierType.ControlMask | Gdk.ModifierType.SuperMask |Gdk.ModifierType.MetaMask;
			
			//all accels ignore the lock key
			modifier &= ~Gdk.ModifierType.LockMask;
			
			//fully decomposed
			TranslateKeyboardState (evt, Gdk.ModifierType.None, 0,
				out keyval, out effectiveGroup, out level, out consumedModifiers);
			accelList.Add (new KeyboardShortcut ((Gdk.Key)keyval, modifier & accelMods));
			
			//with shift composed
			if ((modifier & Gdk.ModifierType.ShiftMask) != 0) {
				keymap.TranslateKeyboardState (evt.HardwareKeycode, Gdk.ModifierType.ShiftMask, 0,
					out keyval, out effectiveGroup, out level, out consumedModifiers);

				if (Platform.IsWindows && evt.HardwareKeycode == 0) {
					keyval = (ushort)evt.KeyValue;
				}
				
				// Prevent consumption of non-Shift modifiers (that we didn't even provide!)
				consumedModifiers &= Gdk.ModifierType.ShiftMask;
				
				var m = (modifier & ~consumedModifiers) & accelMods;
				AddIfNotDuplicate (accelList, new KeyboardShortcut ((Gdk.Key)keyval, m));
			}
			
			//with group 1 composed
			if (grp == 1) {
				TranslateKeyboardState (evt, modifier & ~Gdk.ModifierType.ShiftMask, 1,
					out keyval, out effectiveGroup, out level, out consumedModifiers);
				
				// Prevent consumption of Shift modifier (that we didn't even provide!)
				consumedModifiers &= ~Gdk.ModifierType.ShiftMask;
				
				var m = (modifier & ~consumedModifiers) & accelMods;
				AddIfNotDuplicate (accelList, new KeyboardShortcut ((Gdk.Key)keyval, m));
			}
			
			//with group 1 and shift composed
			if (grp == 1 && (modifier & Gdk.ModifierType.ShiftMask) != 0) {
				TranslateKeyboardState (evt, modifier, 1,
					out keyval, out effectiveGroup, out level, out consumedModifiers);
				var m = (modifier & ~consumedModifiers) & accelMods;
				AddIfNotDuplicate (accelList, new KeyboardShortcut ((Gdk.Key)keyval, m));
			}
			
			//and also allow the fully mapped key as an accel
			AddIfNotDuplicate (accelList, new KeyboardShortcut (mapped.Key, mapped.State & accelMods));
			
			mapped.Shortcuts = accelList.ToArray ();
			return mapped;
		}
		
		// Workaround for bug "Bug 688247 - Ctrl+Alt key not work on windows7 with bootcamp on a Mac Book Pro"
		// Ctrl+Alt should behave like right alt key - unfortunately TranslateKeyboardState doesn't handle it. 
		static void TranslateKeyboardState (Gdk.EventKey evt, Gdk.ModifierType state, int group, out uint keyval,
			out int effective_group, out int level, out Gdk.ModifierType consumed_modifiers)
		{
			uint hardware_keycode = evt.HardwareKeycode;

			if (Platform.IsWindows) {
				const Gdk.ModifierType ctrlAlt = Gdk.ModifierType.ControlMask | Gdk.ModifierType.Mod1Mask;
				if ((state & ctrlAlt) == ctrlAlt) {
					state = (state & ~ctrlAlt) | Gdk.ModifierType.Mod2Mask;
					group = 1;
				}
				// Case: Caps lock on + shift + key 
				// See: Bug 8069 - [UI Refresh] If caps lock is on, holding the shift key prevents typed characters from appearing
				if (state.HasFlag (Gdk.ModifierType.ShiftMask)) {
					state &= ~Gdk.ModifierType.ShiftMask;
				}
			}
			
			keymap.TranslateKeyboardState (hardware_keycode, state, group, out keyval, out effective_group,
				out level, out consumed_modifiers);

			if (Platform.IsWindows && hardware_keycode == 0) {
				keyval = evt.KeyValue;
			}
		}
		
		static void AddIfNotDuplicate<T> (List<T> list, T item) where T : IEquatable<T>
		{
			for (int i = 0; i < list.Count; i++) {
				if (list[i].Equals (item))
					return;
			}
			list.Add (item);
		}

		[System.Runtime.InteropServices.DllImport (PangoUtil.LIBGDK, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr gdk_win32_drawable_get_handle (IntPtr drawable);
		
		enum DwmWindowAttribute
		{
			NcRenderingEnabled = 1,
			NcRenderingPolicy,
			TransitionsForceDisabled,
			AllowNcPaint,
			CaptionButtonBounds,
			NonClientRtlLayout,
			ForceIconicRepresentation,
			Flip3DPolicy,
			ExtendedFrameBounds,
			HasIconicBitmap,
			DisallowPeek,
			ExcludedFromPeek,
			Last,
		}
		
		struct Win32Rect
		{
			public int Left, Top, Right, Bottom;
			
			public Win32Rect (int left, int top, int right, int bottom)
			{
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}
		}
		
		[DllImport ("dwmapi.dll")]
		static extern int DwmGetWindowAttribute (IntPtr hwnd, DwmWindowAttribute attribute, out Win32Rect value, int valueSize);
		
		[DllImport ("dwmapi.dll")]
		static extern int DwmIsCompositionEnabled (out bool enabled);
		
		[DllImport (USER32DLL)]
		static extern bool GetWindowRect (IntPtr hwnd, out Win32Rect rect);
		
		public static void SetImCursorLocation (Gtk.IMContext ctx, Gdk.Window clientWindow, Gdk.Rectangle cursor)
		{
			// work around GTK+ Bug 663096 - Windows IME position is wrong when Aero glass is enabled
			// https://bugzilla.gnome.org/show_bug.cgi?id=663096
			if (Platform.IsWindows && System.Environment.OSVersion.Version.Major >= 6) {
				bool enabled;
				if (DwmIsCompositionEnabled (out enabled) == 0 && enabled) {
					var hwnd = gdk_win32_drawable_get_handle (clientWindow.Toplevel.Handle);
					Win32Rect rect;
					// this module gets the WINVER=6 version of GetWindowRect, which returns the correct value
					if (GetWindowRect (hwnd, out rect)) {
						int x, y;
						clientWindow.Toplevel.GetPosition (out x, out y);
						cursor.X = cursor.X - x + rect.Left;
						cursor.Y = cursor.Y - y + rect.Top - cursor.Height;
					}
				}
			}
			ctx.CursorLocation = cursor;
		}
		
		/// <summary>X coordinate of the pixels inside the right edge of the rectangle</summary>
		/// <remarks>Workaround for inconsistency of Right property between GTK# versions</remarks>
		public static int RightInside (this Gdk.Rectangle rect)
		{
			return rect.X + rect.Width - 1;
		}
		
		/// <summary>Y coordinate of the pixels inside the bottom edge of the rectangle</summary>
		/// <remarks>Workaround for inconsistency of Bottom property between GTK# versions#</remarks>
		public static int BottomInside (this Gdk.Rectangle rect)
		{
			return rect.Y + rect.Height - 1;
		}

		[DllImport (PangoUtil.LIBGTKGLUE, CallingConvention = CallingConvention.Cdecl)]
		static extern void gtksharp_container_leak_fixed_marker ();

		static HashSet<Type> fixedContainerTypes;
		static Dictionary<IntPtr,ForallDelegate> forallCallbacks;
		static bool containerLeakFixed;
		
		// Works around BXC #3801 - Managed Container subclasses are incorrectly resurrected, then leak.
		// It does this by registering an alternative callback for gtksharp_container_override_forall, which
		// ignores callbacks if the wrapper no longer exists. This means that the objects no longer enter a
		// finalized->release->dispose->re-wrap resurrection cycle.
		// We use a dynamic method to access internal/private GTK# API in a performant way without having to track
		// per-instance delegates.
		public static void FixContainerLeak (Gtk.Container c)
		{
			if (containerLeakFixed) {
				return;
			}

			FixContainerLeak (c.GetType ());
		}

		static void FixContainerLeak (Type t)
		{
			if (containerLeakFixed) {
				return;
			}

			if (fixedContainerTypes == null) {
				try {
					gtksharp_container_leak_fixed_marker ();
					containerLeakFixed = true;
					return;
				} catch (EntryPointNotFoundException) {
				}
				fixedContainerTypes = new HashSet<Type>();
				forallCallbacks = new Dictionary<IntPtr, ForallDelegate> ();
			}

			if (!fixedContainerTypes.Add (t)) {
				return;
			}

			//need to fix the callback for the type and all the managed supertypes
			var lookupGType = typeof (GLib.Object).GetMethod ("LookupGType", BindingFlags.Static | BindingFlags.NonPublic);
			do {
				var gt = (GLib.GType) lookupGType.Invoke (null, new[] { t });
				var cb = CreateForallCallback (gt.Val);
				forallCallbacks[gt.Val] = cb;
				gtksharp_container_override_forall (gt.Val, cb);
				t = t.BaseType;
			} while (fixedContainerTypes.Add (t) && t.Assembly != typeof (Gtk.Container).Assembly);
		}

		static ForallDelegate CreateForallCallback (IntPtr gtype)
		{
			var dm = new DynamicMethod (
				"ContainerForallCallback",
				typeof(void),
				new Type[] { typeof(IntPtr), typeof(bool), typeof(IntPtr), typeof(IntPtr) },
				typeof(GtkWorkarounds).Module,
				true);
			
			var invokerType = typeof(Gtk.Container.CallbackInvoker);
			
			//this was based on compiling a similar method and disassembling it
			ILGenerator il = dm.GetILGenerator ();
			var IL_002b = il.DefineLabel ();
			var IL_003f = il.DefineLabel ();
			var IL_0060 = il.DefineLabel ();
			var label_return = il.DefineLabel ();

			var loc_container = il.DeclareLocal (typeof(Gtk.Container));
			var loc_obj = il.DeclareLocal (typeof(object));
			var loc_invoker = il.DeclareLocal (invokerType);
			var loc_ex = il.DeclareLocal (typeof(Exception));

			//check that the type is an exact match
			// prevent stack overflow, because the callback on a more derived type will handle everything
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Call, typeof(GLib.ObjectManager).GetMethod ("gtksharp_get_type_id", BindingFlags.Static | BindingFlags.NonPublic));

			il.Emit (OpCodes.Ldc_I8, gtype.ToInt64 ());
			il.Emit (OpCodes.Newobj, typeof (IntPtr).GetConstructor (new Type[] { typeof (Int64) }));
			il.Emit (OpCodes.Call, typeof (IntPtr).GetMethod ("op_Equality", BindingFlags.Static | BindingFlags.Public));
			il.Emit (OpCodes.Brfalse, label_return);

			il.BeginExceptionBlock ();
			il.Emit (OpCodes.Ldnull);
			il.Emit (OpCodes.Stloc, loc_container);
			il.Emit (OpCodes.Ldsfld, typeof (GLib.Object).GetField ("Objects", BindingFlags.Static | BindingFlags.NonPublic));
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Box, typeof (IntPtr));
			il.Emit (OpCodes.Callvirt, typeof (System.Collections.Hashtable).GetProperty ("Item").GetGetMethod ());
			il.Emit (OpCodes.Stloc, loc_obj);
			il.Emit (OpCodes.Ldloc, loc_obj);
			il.Emit (OpCodes.Brfalse, IL_002b);

			var tref = typeof (GLib.Object).Assembly.GetType ("GLib.ToggleRef");
			il.Emit (OpCodes.Ldloc, loc_obj);
			il.Emit (OpCodes.Castclass, tref);
			il.Emit (OpCodes.Callvirt, tref.GetProperty ("Target").GetGetMethod ());
			il.Emit (OpCodes.Isinst, typeof (Gtk.Container));
			il.Emit (OpCodes.Stloc, loc_container);
			
			il.MarkLabel (IL_002b);
			il.Emit (OpCodes.Ldloc, loc_container);
			il.Emit (OpCodes.Brtrue, IL_003f);
			
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldarg_1);
			il.Emit (OpCodes.Ldarg_2);
			il.Emit (OpCodes.Ldarg_3);
			il.Emit (OpCodes.Call, typeof (Gtk.Container).GetMethod ("gtksharp_container_base_forall", BindingFlags.Static | BindingFlags.NonPublic));
			il.Emit (OpCodes.Br, IL_0060);
			
			il.MarkLabel (IL_003f);
			il.Emit (OpCodes.Ldloca_S, 2);
			il.Emit (OpCodes.Ldarg_2);
			il.Emit (OpCodes.Ldarg_3);
			il.Emit (OpCodes.Call, invokerType.GetConstructor (
				BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof (IntPtr), typeof (IntPtr) }, null));
			il.Emit (OpCodes.Ldloc, loc_container);
			il.Emit (OpCodes.Ldarg_1);
			il.Emit (OpCodes.Ldloc, loc_invoker);
			il.Emit (OpCodes.Box, invokerType);
			il.Emit (OpCodes.Ldftn, invokerType.GetMethod ("Invoke"));
			il.Emit (OpCodes.Newobj, typeof (Gtk.Callback).GetConstructor (
				BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof (object), typeof (IntPtr) }, null));
			var forallMeth = typeof (Gtk.Container).GetMethod ("ForAll",
				BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof (bool), typeof (Gtk.Callback) }, null);
			il.Emit (OpCodes.Callvirt, forallMeth);
			
			il.MarkLabel (IL_0060);
			
			il.BeginCatchBlock (typeof (Exception));
			il.Emit (OpCodes.Stloc, loc_ex);
			il.Emit (OpCodes.Ldloc, loc_ex);
			il.Emit (OpCodes.Ldc_I4_0);
			il.Emit (OpCodes.Call, typeof (GLib.ExceptionManager).GetMethod ("RaiseUnhandledException"));
			il.Emit (OpCodes.Leave, label_return);
			il.EndExceptionBlock ();
			
			il.MarkLabel (label_return);
			il.Emit (OpCodes.Ret);
			
			return (ForallDelegate) dm.CreateDelegate (typeof (ForallDelegate));
		}
		
		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate void ForallDelegate (IntPtr container, bool include_internals, IntPtr cb, IntPtr data);
		
		[DllImport(PangoUtil.LIBGTKGLUE, CallingConvention = CallingConvention.Cdecl)]
		static extern void gtksharp_container_override_forall (IntPtr gtype, ForallDelegate cb);

		public static string MarkupLinks (string text)
		{
			if (GtkMinorVersion < 18)
				return text;
			return HighlightUrlSemanticRule.UrlRegex.Replace (text, MatchToUrl);
		}

		static string MatchToUrl (System.Text.RegularExpressions.Match m)
		{
			var s = m.ToString ();
			return String.Format ("<a href='{0}'>{1}</a>", s, s.Replace ("_", "__"));
		}

		public static void SetLinkHandler (this Gtk.Label label, Action<string> urlHandler)
		{
			if (GtkMinorVersion >= 18)
				new UrlHandlerClosure (urlHandler).ConnectTo (label);
		}

		//create closure manually so we can apply ConnectBefore
		class UrlHandlerClosure
		{
			Action<string> urlHandler;

			public UrlHandlerClosure (Action<string> urlHandler)
			{
				this.urlHandler = urlHandler;
			}

			[GLib.ConnectBefore]
			void HandleLink (object sender, ActivateLinkEventArgs args)
			{
				urlHandler (args.Url);
				args.RetVal = true;
			}

			public void ConnectTo (Gtk.Label label)
			{
				var signal = GLib.Signal.Lookup (label, "activate-link", typeof(ActivateLinkEventArgs));
				signal.AddDelegate (new EventHandler<ActivateLinkEventArgs> (HandleLink));
			}

			class ActivateLinkEventArgs : GLib.SignalArgs
			{
				public string Url { get { return (string)base.Args [0]; } }
			}
		}

		[DllImport (PangoUtil.LIBGTK, CallingConvention = CallingConvention.Cdecl)]
		static extern bool gtk_tree_view_get_tooltip_context (IntPtr raw, ref int x, ref int y, bool keyboard_tip, out IntPtr model, out IntPtr path, IntPtr iter);

		//the GTK# version of this has 'out' instead of 'ref', preventing passing the x,y values in
		public static bool GetTooltipContext (this TreeView tree, ref int x, ref int y, bool keyboardTip,
			 out TreeModel model, out TreePath path, out Gtk.TreeIter iter)
		{
			IntPtr intPtr = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(TreeIter)));
			IntPtr handle;
			IntPtr intPtr2;
			bool result = gtk_tree_view_get_tooltip_context (tree.Handle, ref x, ref y, keyboardTip, out handle, out intPtr2, intPtr);
			model = TreeModelAdapter.GetObject (handle, false);
			path = intPtr2 == IntPtr.Zero ? null : ((TreePath)GLib.Opaque.GetOpaque (intPtr2, typeof(TreePath), false));
			iter = TreeIter.New (intPtr);
			Marshal.FreeHGlobal (intPtr);
			return result;
		}
		
		static bool supportsHiResIcons = true;

		[DllImport (PangoUtil.LIBGTK, CallingConvention = CallingConvention.Cdecl)]
		static extern void gtk_icon_source_set_scale (IntPtr source, double scale);
		
		[DllImport (PangoUtil.LIBGTK, CallingConvention = CallingConvention.Cdecl)]
		static extern void gtk_icon_source_set_scale_wildcarded (IntPtr source, bool setting);
		
		[DllImport (PangoUtil.LIBGTK, CallingConvention = CallingConvention.Cdecl)]
		static extern double gtk_widget_get_scale_factor (IntPtr widget);
		
		[DllImport (PangoUtil.LIBGDK, CallingConvention = CallingConvention.Cdecl)]
		static extern double gdk_screen_get_monitor_scale_factor (IntPtr widget, int monitor);

		[DllImport (PangoUtil.LIBGOBJECT, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr g_object_get_data (IntPtr source, string name);
		
		[DllImport (PangoUtil.LIBGTK, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr gtk_icon_set_render_icon_scaled (IntPtr handle, IntPtr style, int direction, int state, int size, IntPtr widget, IntPtr intPtr, ref double scale);

		public static bool SetSourceScale (Gtk.IconSource source, double scale)
		{
			if (!supportsHiResIcons)
				return false;

			try {
				gtk_icon_source_set_scale (source.Handle, scale);
				return true;
			} catch (DllNotFoundException) {
			} catch (EntryPointNotFoundException) {
			}
			supportsHiResIcons = false;
			return false;
		}
		
		public static bool SetSourceScaleWildcarded (Gtk.IconSource source, bool setting)
		{
			if (!supportsHiResIcons)
				return false;

			try {
				gtk_icon_source_set_scale_wildcarded (source.Handle, setting);
				return true;
			} catch (DllNotFoundException) {
			} catch (EntryPointNotFoundException) {
			}
			supportsHiResIcons = false;
			return false;
		}

		public static Gdk.Pixbuf Get2xVariant (Gdk.Pixbuf px)
		{
			if (!supportsHiResIcons)
				return null;

			try {
				IntPtr res = g_object_get_data (px.Handle, "gdk-pixbuf-2x-variant");
				if (res != IntPtr.Zero && res != px.Handle)
					return (Gdk.Pixbuf) GLib.Object.GetObject (res);
				else
					return null;
			} catch (DllNotFoundException) {
			} catch (EntryPointNotFoundException) {
			}
			supportsHiResIcons = false;
			return null;
		}

		public static void Set2xVariant (Gdk.Pixbuf px, Gdk.Pixbuf variant2x)
		{
		}
		
		public static double GetScaleFactor (Gtk.Widget w)
		{
			if (!supportsHiResIcons)
				return 1;

			try {
				return gtk_widget_get_scale_factor (w.Handle);
			} catch (DllNotFoundException) {
			} catch (EntryPointNotFoundException) {
			}
			supportsHiResIcons = false;
			return 1;
		}
		
		public static double GetScaleFactor (this Gdk.Screen screen, int monitor)
		{
			if (!supportsHiResIcons)
				return 1;

			try {
				return gdk_screen_get_monitor_scale_factor (screen.Handle, monitor);
			} catch (DllNotFoundException) {
			} catch (EntryPointNotFoundException) {
			}
			supportsHiResIcons = false;
			return 1;
		}

		public static double GetScaleFactor ()
		{
			return GetScaleFactor (Gdk.Screen.Default, 0);
		}

		public static double GetPixelScale ()
		{
			if (Platform.IsWindows)
				return GetScaleFactor ();
			else
				return 1d;
		}

		public static int ConvertToPixelScale (int size)
		{
			double scale = GetPixelScale ();

			return (int)(size * scale);
		}
		
		public static Gdk.Pixbuf RenderIcon (this Gtk.IconSet iconset, Gtk.Style style, Gtk.TextDirection direction, Gtk.StateType state, Gtk.IconSize size, Gtk.Widget widget, string detail, double scale)
		{
			if (scale == 1d)
				return iconset.RenderIcon (style, direction, state, size, widget, detail);

			if (!supportsHiResIcons)
				return null;

			try {
				IntPtr intPtr = GLib.Marshaller.StringToPtrGStrdup (detail);
				IntPtr o = gtk_icon_set_render_icon_scaled (iconset.Handle, (style != null) ? style.Handle : IntPtr.Zero, (int)direction, (int)state, (int)size, (widget != null) ? widget.Handle : IntPtr.Zero, intPtr, ref scale);
				Gdk.Pixbuf result = (Gdk.Pixbuf) GLib.Object.GetObject (o);
				GLib.Marshaller.Free (intPtr);
				return result;
			} catch (DllNotFoundException) {
			} catch (EntryPointNotFoundException) {
			}
			supportsHiResIcons = false;
			return null;
		}
	}
	
	public struct KeyboardShortcut : IEquatable<KeyboardShortcut>
	{
		public static readonly KeyboardShortcut Empty = new KeyboardShortcut ((Gdk.Key) 0, (Gdk.ModifierType) 0);
		
		Gdk.ModifierType modifier;
		Gdk.Key key;
		
		public KeyboardShortcut (Gdk.Key key, Gdk.ModifierType modifier)
		{
			this.modifier = modifier;
			this.key = key;
		}
		
		public Gdk.Key Key {
			get { return key; }
		}
		
		public Gdk.ModifierType Modifier {
			get { return modifier; }
		}
		
		public bool IsEmpty {
			get { return Key == (Gdk.Key) 0; }
		}
		
		public override bool Equals (object obj)
		{
			return obj is KeyboardShortcut && this.Equals ((KeyboardShortcut) obj);
		}
		
		public override int GetHashCode ()
		{
			//FIXME: we're only using a few bits of mod and mostly the lower bits of key - distribute it better
			return (int) Key ^ (int) Modifier;
		}
		
		public bool Equals (KeyboardShortcut other)
		{
			return other.Key == Key && other.Modifier == Modifier;
		}
	}
}
