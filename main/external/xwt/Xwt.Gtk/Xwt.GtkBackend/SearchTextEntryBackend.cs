//
// SearchTextEntryBackend.cs
//
// Author:
//       Lluis Sanchez Gual <lluis@xamarin.com>
//       Aaron Bockover <abockover@novell.com>
//       Gabriel Burt <gburt@novell.com>
//       Vsevolod Kukol <sevo@sevo.org>
//
// Copyright (c) 2014 Xamarin, Inc (http://www.xamarin.com)
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
using System;
using Xwt.Backends;
using Xwt.CairoBackend;

namespace Xwt.GtkBackend
{
	public class SearchTextEntryBackend: TextEntryBackend, ISearchTextEntryBackend
	{
		SearchEntry searchEntry;

		protected override Gtk.Entry TextEntry {
			get {
				return searchEntry.Entry;
			}
		}

		public override void Initialize ()
		{
			searchEntry = new SearchEntry ();
			searchEntry.ForceFilterButtonVisible = true;
			searchEntry.RoundedShape = true;
			searchEntry.HasFrame = true;
			((WidgetBackend)this).Widget = searchEntry;
			searchEntry.Show ();
		}

		public override void SetFocus ()
		{
			base.SetFocus ();
			TextEntry.GrabFocus ();
		}
	}

	class SearchEntry : Gtk.EventBox
	{
		Gtk.Alignment alignment;
		Gtk.Alignment entryAlignment;
		private Gtk.HBox box;
		private Gtk.Entry entry;
		private HoverImageButton filter_button;
		private HoverImageButton clear_button;

		static Xwt.Drawing.Image searchImage;
		static Xwt.Drawing.Image clearImage;

		private Gtk.Menu menu;
		private int active_filter_id = -1;

		private uint changed_timeout_id = 0;

		private string empty_message;
		private bool ready = false;

		private event EventHandler filter_changed;
		private event EventHandler entry_changed;
		EventHandler activated_event;
		bool roundedShape;
		bool hasFrame = true;
		bool customRoundedShapeDrawing = false;

		public event EventHandler Changed {
			add { entry_changed += value; }
			remove { entry_changed -= value; }
		}

		public event EventHandler Activated {
			add { activated_event += value; }
			remove { activated_event -= value; }
		}

		public event EventHandler FilterChanged {
			add { filter_changed += value; }
			remove { filter_changed -= value; }
		}

		bool forceFilterButtonVisible = true;
		public bool ForceFilterButtonVisible {
			get {
				return forceFilterButtonVisible;
			}
			set {
				forceFilterButtonVisible = value;
				ShowHideButtons ();
			}
		}

		public Gtk.Menu Menu {
			get { return menu; }
			set { menu = value; menu.Deactivated += OnMenuDeactivated; }
		}

		public Gtk.Entry Entry {
			get { return this.entry; }
		}

		public bool HasFrame {
			get { return hasFrame; }
			set { hasFrame = value; QueueDraw (); }
		}

		public bool RoundedShape {
			get { return roundedShape; }
			set {
				roundedShape = value;
				if (value)
					entry.Name = "search-entry";
				else
					entry.Name = "";
				ShowHideButtons ();
				QueueDraw ();
			}
		}

		static SearchEntry ()
		{
			clearImage = Xwt.Drawing.Image.FromResource ("searchbox-clear-light-16.png");
			searchImage = Xwt.Drawing.Image.FromResource ("searchbox-search-light-16.png");
		}

		public SearchEntry ()
		{
			AppPaintable = true;

			BuildWidget ();
			BuildMenu ();

			NoShowAll = true;
		}

		public Xwt.Drawing.Image FilterButtonPixbuf {
			get {
				return filter_button.Pixbuf;
			}
			set {
				filter_button.Pixbuf = value;
			}
		}

		private void BuildWidget ()
		{
			alignment = new Gtk.Alignment (0.5f, 0.5f, 1f, 0f);
			alignment.SetPadding (1, 1, 3, 3);
			VisibleWindow = false;

			box = new Gtk.HBox ();
			entry = new FramelessEntry (this);
			filter_button = new HoverImageButton (searchImage);
			clear_button = new HoverImageButton (clearImage);

			entryAlignment = new Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			alignment.SetPadding (0, 0, 3, 3);
			entryAlignment.Add (entry);
			box.PackStart (filter_button, false, false, 0);
			box.PackStart (entryAlignment, true, true, 0);
			box.PackStart (clear_button, false, false, 0);
			alignment.Add (box);
			Add (alignment);
			alignment.ShowAll ();

			entry.StyleSet += OnInnerEntryStyleSet;
			entry.StateChanged += OnInnerEntryStateChanged;
			entry.FocusInEvent += OnInnerEntryFocusEvent;
			entry.FocusOutEvent += OnInnerEntryFocusEvent;
			entry.Changed += OnInnerEntryChanged;
			entry.Activated += delegate {
				NotifyActivated ();
			};

			filter_button.CanFocus = false;
			clear_button.CanFocus = false;

			filter_button.ButtonReleaseEvent += OnButtonReleaseEvent;
			clear_button.ButtonReleaseEvent += OnButtonReleaseEvent;
			clear_button.Clicked += OnClearButtonClicked;

			ShowHideButtons ();
		}

		protected override void OnSizeRequested (ref Gtk.Requisition requisition)
		{
			if (HeightRequest != -1 && box.HeightRequest != HeightRequest)
				box.HeightRequest = HeightRequest;
			if (box.HeightRequest != -1 && HeightRequest == -1)
				box.HeightRequest = -1;
			base.OnSizeRequested (ref requisition);
		}

		Gtk.EventBox statusLabelEventBox;
		public Gtk.EventBox AddLabelWidget (Gtk.Label label)
		{
			box.Remove (clear_button);
			statusLabelEventBox = new Gtk.EventBox ();
			statusLabelEventBox.Child = label;
			box.PackStart (statusLabelEventBox, false, false, 0);
			box.PackStart (clear_button, false, false, 0);
			UpdateStyle ();
			box.ShowAll ();
			return statusLabelEventBox;
		}

		void NotifyActivated ()
		{
			if (activated_event != null)
				activated_event (this, EventArgs.Empty);
		}

		private void BuildMenu ()
		{
			menu = new Gtk.Menu ();
			menu.Deactivated += OnMenuDeactivated;
		}

		public void PopupFilterMenu ()
		{
			ShowMenu (0);
		}

		void ShowMenu (uint time)
		{
			OnRequestMenu (EventArgs.Empty);
			if (menu.Children.Length > 0) {
				menu.Popup (null, null, OnPositionMenu, 0, time);
				menu.ShowAll ();
			}
		}

		private void ShowHideButtons ()
		{
			clear_button.Visible = entry.Text.Length > 0;
			entryAlignment.RightPadding = (uint) (!clear_button.Visible && roundedShape ? 6 : 0);

			filter_button.Visible = ForceFilterButtonVisible || (menu != null && menu.Children.Length > 0);
			entryAlignment.LeftPadding = (uint) (!filter_button.Visible && roundedShape ? 6 : 0);
		}

		private void OnPositionMenu (Gtk.Menu menu, out int x, out int y, out bool push_in)
		{
			int origin_x, origin_y, tmp;

			filter_button.GdkWindow.GetOrigin (out origin_x, out tmp);
			GdkWindow.GetOrigin (out tmp, out origin_y);

			x = origin_x + filter_button.Allocation.X;
			y = origin_y + Allocation.Y + SizeRequest ().Height;
			push_in = true;
		}

		private void OnMenuDeactivated (object o, EventArgs args)
		{
			filter_button.QueueDraw ();
		}

		private bool toggling = false;
		public bool IsCheckMenu { get; set; }
		private void OnMenuItemToggled (object o, EventArgs args)
		{
			if (IsCheckMenu || toggling || !(o is FilterMenuItem)) {
				return;
			}

			toggling = true;
			FilterMenuItem item = (FilterMenuItem)o;

			foreach (Gtk.MenuItem child_item in menu) {
				if (!(child_item is FilterMenuItem)) {
					continue;
				}

				FilterMenuItem filter_child = (FilterMenuItem)child_item;
				if (filter_child != item) {
					filter_child.Active = false;
				}
			}

			item.Active = true;
			ActiveFilterID = item.ID;
			toggling = false;
		}

		private void OnInnerEntryChanged (object o, EventArgs args)
		{
			ShowHideButtons ();

			if (changed_timeout_id > 0) {
				GLib.Source.Remove (changed_timeout_id);
			}

			if (Ready)
				changed_timeout_id = GLib.Timeout.Add (25, OnChangedTimeout);
		}

		private bool OnChangedTimeout ()
		{
			OnChanged ();
			return false;
		}

		private void UpdateStyle ()
		{
			Gdk.Color color = entry.Style.Base (entry.State);
			filter_button.ModifyBg (entry.State, color);
			clear_button.ModifyBg (entry.State, color);
			if (statusLabelEventBox != null)
				statusLabelEventBox.ModifyBg (entry.State, color);

			box.BorderWidth = 0;
			var h = entry.SizeRequest ().Height + entry.Style.Ythickness * 2;
			var req = entry.SizeRequest ().Height;
			req = Math.Max (req, filter_button.SizeRequest ().Height);
			req = Math.Max (req, clear_button.SizeRequest ().Height);
			var diff = h - req;
			if (diff > 1)
				box.BorderWidth = (uint)(diff / 2);
		}

		private void OnInnerEntryStyleSet (object o, Gtk.StyleSetArgs args)
		{
			UpdateStyle ();
		}

		private void OnInnerEntryStateChanged (object o, EventArgs args)
		{
			UpdateStyle ();
		}

		private void OnInnerEntryFocusEvent (object o, EventArgs args)
		{
			QueueDraw ();
		}

		private void OnButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{
			if (args.Event.Button != 1) {
				return;
			}

			entry.HasFocus = true;

			if (o == filter_button) {
				ShowMenu (args.Event.Time);
			}
		}

		protected virtual void OnRequestMenu (EventArgs e)
		{
			EventHandler handler = this.RequestMenu;
			if (handler != null)
				handler (this, e);
		}

		public event EventHandler RequestMenu;

		public void GrabFocusEntry ()
		{
			this.entry.GrabFocus ();
		}
		private void OnClearButtonClicked (object o, EventArgs args)
		{
			active_filter_id = 0;
			entry.Text = String.Empty;
			NotifyActivated ();
		}

		protected override void OnDestroyed ()
		{
			if (menu != null) {
				menu.Destroy ();
				menu = null;
			}
			base.OnDestroyed ();
		}

		protected override bool OnKeyPressEvent (Gdk.EventKey evnt)
		{
			if (evnt.Key == Gdk.Key.Escape) {
				active_filter_id = 0;
				if (!String.IsNullOrEmpty (entry.Text)) {
					entry.Text = String.Empty;
					NotifyActivated ();
					return true;
				}
			}
			return base.OnKeyPressEvent (evnt);
		}

		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			var alloc = new Gdk.Rectangle (alignment.Allocation.X, box.Allocation.Y, alignment.Allocation.Width, box.Allocation.Height);

			if (hasFrame && (!roundedShape || (roundedShape && !customRoundedShapeDrawing))) {
				Gtk.Style.PaintShadow (entry.Style, GdkWindow, Gtk.StateType.Normal, Gtk.ShadowType.In,
					evnt.Area, entry, "entry", alloc.X, alloc.Y, alloc.Width, alloc.Height);
				/*				using (var ctx = Gdk.CairoHelper.Create (GdkWindow)) {
					ctx.LineWidth = 1;
					ctx.Rectangle (alloc.X + 0.5, alloc.Y + 0.5, alloc.Width - 1, alloc.Height - 1);
					ctx.Color = new Cairo.Color (1,0,0);
					ctx.Stroke ();
				}*/
			}
			else if (!roundedShape) {
				using (var ctx = Gdk.CairoHelper.Create (GdkWindow)) {
					ctx.RoundedRectangle (alloc.X + 0.5, alloc.Y + 0.5, alloc.Width - 1, alloc.Height - 1, 4);
					ctx.SetSourceColor (entry.Style.Base (Gtk.StateType.Normal).ToCairoColor ());
					ctx.Fill ();
				}
			}
			else {
				using (var ctx = Gdk.CairoHelper.Create (GdkWindow)) {
					RoundBorder (ctx, alloc.X + 0.5, alloc.Y + 0.5, alloc.Width - 1, alloc.Height - 1);
					ctx.SetSourceColor (entry.Style.Base (Gtk.StateType.Normal).ToCairoColor ());
					ctx.Fill ();
				}
			}

			PropagateExpose (Child, evnt);

			if (hasFrame && roundedShape && customRoundedShapeDrawing) {
				using (var ctx = Gdk.CairoHelper.Create (GdkWindow)) {
					RoundBorder (ctx, alloc.X + 0.5, alloc.Y + 0.5, alloc.Width - 1, alloc.Height - 1);
					ctx.SetSourceColor (Xwt.Drawing.Color.FromName ("#8c8c8c").ToCairoColor ());
					ctx.LineWidth = 1;
					ctx.Stroke ();
				}
			}
			return true;
		}

		static void RoundBorder (Cairo.Context ctx, double x, double y, double w, double h)
		{
			double r = h / 2;
			ctx.Arc (x + r, y + r, r, Math.PI / 2, Math.PI + Math.PI / 2);
			ctx.LineTo (x + w - r, y);

			ctx.Arc (x + w - r, y + r, r, Math.PI + Math.PI / 2, Math.PI + Math.PI + Math.PI / 2);

			ctx.LineTo (x + r, y + h);

			ctx.ClosePath ();
		}

		protected override void OnShown ()
		{
			base.OnShown ();
			ShowHideButtons ();
		}

		protected virtual void OnChanged ()
		{
			if (!Ready) {
				return;
			}

			EventHandler handler = entry_changed;
			if (handler != null) {
				handler (this, EventArgs.Empty);
			}
		}

		protected virtual void OnFilterChanged ()
		{
			EventHandler handler = filter_changed;
			if (handler != null) {
				handler (this, EventArgs.Empty);
			}

			if (IsQueryAvailable) {
				OnInnerEntryChanged (this, EventArgs.Empty);
			}
		}

		public Gtk.CheckMenuItem AddFilterOption (int id, string label)
		{
			if (id < 0) {
				throw new ArgumentException ("id", "must be >= 0");
			}

			FilterMenuItem item = new FilterMenuItem (id, label);

			item.Toggled += OnMenuItemToggled;
			menu.Append (item);

			if (ActiveFilterID < 0) {
				item.Toggle ();
			}

			filter_button.Visible = true;
			return item;
		}

		public Gtk.MenuItem AddMenuItem (string label)
		{
			var item = new Gtk.MenuItem (label);
			menu.Append (item);
			return item;
		}

		public void AddFilterSeparator ()
		{
			menu.Append (new Gtk.SeparatorMenuItem ());
		}

		public void RemoveFilterOption (int id)
		{
			FilterMenuItem item = FindFilterMenuItem (id);
			if (item != null) {
				menu.Remove (item);
			}
		}

		public void ActivateFilter (int id)
		{
			FilterMenuItem item = FindFilterMenuItem (id);
			if (item != null) {
				item.Toggle ();
			}
		}

		private FilterMenuItem FindFilterMenuItem (int id)
		{
			foreach (Gtk.MenuItem item in menu) {
				if (item is FilterMenuItem && ((FilterMenuItem)item).ID == id) {
					return (FilterMenuItem)item;
				}
			}

			return null;
		}

		public string GetLabelForFilterID (int id)
		{
			FilterMenuItem item = FindFilterMenuItem (id);
			if (item == null) {
				return null;
			}

			return item.Label;
		}

		public void CancelSearch ()
		{
			entry.Text = String.Empty;
			ActivateFilter (0);
		}

		public int ActiveFilterID {
			get { return active_filter_id; }
			set {
				if (value == active_filter_id) {
					return;
				}

				active_filter_id = value;
				OnFilterChanged ();
			}
		}

		public string EmptyMessage {
			get { return entry.Sensitive ? empty_message : String.Empty; }
			set {
				empty_message = value;
				entry.QueueDraw ();
			}
		}

		public string Query {
			get { return entry.Text.Trim (); }
			set { entry.Text = value.Trim (); }
		}

		public bool IsQueryAvailable {
			get { return Query != null && Query != String.Empty; }
		}

		public bool Ready {
			get { return ready; }
			set { ready = value; }
		}

		public new bool HasFocus {
			get { return entry.HasFocus; }
			set { entry.HasFocus = true; }
		}


		public Gtk.Entry InnerEntry {
			get { return entry; }
		}

		protected override void OnStateChanged (Gtk.StateType previous_state)
		{
			base.OnStateChanged (previous_state);

			entry.Sensitive = State != Gtk.StateType.Insensitive;
			filter_button.Sensitive = State != Gtk.StateType.Insensitive;
			clear_button.Sensitive = State != Gtk.StateType.Insensitive;
		}

		private class FilterMenuItem : Gtk.CheckMenuItem
		{
			private int id;
			private string label;

			public FilterMenuItem (int id, string label) : base(label)
			{
				this.id = id;
				this.label = label;
				DrawAsRadio = true;
			}

			public int ID {
				get { return id; }
			}

			public string Label {
				get { return label; }
			}
			/*
            // FIXME: Remove when restored to CheckMenuItem
			private bool active;
			public bool Active {
				get { return active; }
				set { active = value; }
			}*/

			public new event EventHandler Toggled;
			protected override void OnActivated ()
			{
				base.OnActivated ();
				if (Toggled != null) {
					Toggled (this, EventArgs.Empty);
				}
			}

		}

		private class FramelessEntry : Gtk.Entry
		{
			private SearchEntry parent;
			private Pango.Layout layout;
			private Gdk.GC text_gc;

			public FramelessEntry (SearchEntry parent) : base()
			{
				this.parent = parent;
				HasFrame = false;

				parent.StyleSet += OnParentStyleSet;
				WidthChars = 1;
			}

			private void OnParentStyleSet (object o, EventArgs args)
			{
				RefreshGC ();
				QueueDraw ();
			}

			private void RefreshGC ()
			{
				text_gc = null;
			}

			protected override void OnDestroyed ()
			{
				parent.StyleSet -= OnParentStyleSet;
				base.OnDestroyed ();
			}

			public static Gdk.Color ColorBlend (Gdk.Color a, Gdk.Color b)
			{
				// at some point, might be nice to allow any blend?
				double blend = 0.5;

				if (blend < 0.0 || blend > 1.0) {
					throw new ApplicationException ("blend < 0.0 || blend > 1.0");
				}

				double blendRatio = 1.0 - blend;

				int aR = a.Red >> 8;
				int aG = a.Green >> 8;
				int aB = a.Blue >> 8;

				int bR = b.Red >> 8;
				int bG = b.Green >> 8;
				int bB = b.Blue >> 8;

				double mR = aR + bR;
				double mG = aG + bG;
				double mB = aB + bB;

				double blR = mR * blendRatio;
				double blG = mG * blendRatio;
				double blB = mB * blendRatio;

				Gdk.Color color = new Gdk.Color ((byte)blR, (byte)blG, (byte)blB);
				Gdk.Colormap.System.AllocColor (ref color, true, true);
				return color;
			}

			protected override bool OnExposeEvent (Gdk.EventExpose evnt)
			{
				// The Entry's GdkWindow is the top level window onto which
				// the frame is drawn; the actual text entry is drawn into a
				// separate window, so we can ensure that for themes that don't
				// respect HasFrame, we never ever allow the base frame drawing
				// to happen
				if (evnt.Window == GdkWindow) {
					return true;
				}

				bool ret = base.OnExposeEvent (evnt);

				if (text_gc == null) {
					text_gc = new Gdk.GC (evnt.Window);
					text_gc.Copy (Style.TextGC (Gtk.StateType.Normal));
					Gdk.Color color_a = parent.Style.Base (Gtk.StateType.Normal);
					Gdk.Color color_b = parent.Style.Text (Gtk.StateType.Normal);
					text_gc.RgbFgColor = ColorBlend (color_a, color_b);
				}

				if (Text.Length > 0 || HasFocus || parent.EmptyMessage == null) {
					return ret;
				}

				if (layout == null) {
					layout = new Pango.Layout (PangoContext);
					layout.FontDescription = PangoContext.FontDescription.Copy ();
				}

				int width, height;
				layout.SetMarkup (parent.EmptyMessage);
				layout.GetPixelSize (out width, out height);
				evnt.Window.DrawLayout (text_gc, 2, (SizeRequest ().Height - height) / 2, layout);

				return ret;
			}
		}
	}

	class HoverImageButton : Gtk.EventBox
	{
		Xwt.Drawing.Image normal_pixbuf;
		Xwt.Drawing.Image active_pixbuf;
		ImageView image;
		bool is_hovering;
		bool is_pressed;

		bool draw_focus = true;
		EventHandler clicked;

		public event EventHandler Clicked {
			add { clicked += value; }
			remove { clicked -= value; }
		}

		public HoverImageButton()
		{
			Gtk.Alignment al = new Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			al.Show ();
			CanFocus = true;
			VisibleWindow = false;
			image = new ImageView();
			image.Show();
			al.Add ((Gtk.Widget)Toolkit.CurrentEngine.GetNativeWidget (image));
			Add(al);
		}

		public HoverImageButton(Xwt.Drawing.Image img) : this()
		{
			normal_pixbuf = img;
			active_pixbuf = img;
			UpdateImage ();
		}

		public new void Activate()
		{
			EventHandler handler = clicked;
			if(handler != null) {
				handler(this, EventArgs.Empty);
			}
		}

		protected override bool OnEnterNotifyEvent(Gdk.EventCrossing evnt)
		{
			//			image.Cursor = Xwt.CursorType.Hand;
			is_hovering = true;
			UpdateImage();
			return base.OnEnterNotifyEvent(evnt);
		}

		protected override bool OnLeaveNotifyEvent(Gdk.EventCrossing evnt)
		{
			//image.Cursor = Xwt.CursorType.Arrow;
			is_hovering = false;
			UpdateImage();
			return base.OnLeaveNotifyEvent(evnt);
		}

		protected override bool OnFocusInEvent(Gdk.EventFocus evnt)
		{
			bool ret = base.OnFocusInEvent(evnt);
			UpdateImage();
			return ret;
		}

		protected override bool OnFocusOutEvent(Gdk.EventFocus evnt)
		{
			bool ret = base.OnFocusOutEvent(evnt);
			UpdateImage();
			return ret;
		}

		protected override bool OnButtonPressEvent(Gdk.EventButton evnt)
		{
			if(evnt.Button != 1) {
				return base.OnButtonPressEvent(evnt);
			}

			HasFocus = true;
			is_pressed = true;
			QueueDraw();

			return base.OnButtonPressEvent(evnt);
		}

		protected override bool OnButtonReleaseEvent(Gdk.EventButton evnt)
		{
			if(evnt.Button != 1) {
				return base.OnButtonReleaseEvent(evnt);
			}

			is_pressed = false;
			QueueDraw();
			Activate();

			return base.OnButtonReleaseEvent(evnt);
		}

		protected override bool OnExposeEvent(Gdk.EventExpose evnt)
		{
			base.OnExposeEvent(evnt);

			if(HasFocus && draw_focus) {
				Gtk.Style.PaintFocus(Style, GdkWindow, Gtk.StateType.Normal, evnt.Area, this, "button",
					0, 0, Allocation.Width, Allocation.Height);
			}

			return true;
		}

		private void UpdateImage()
		{
			image.Image = is_hovering || is_pressed || HasFocus
				? active_pixbuf : normal_pixbuf;
		}

		public Xwt.Drawing.Image Pixbuf {
			get { return this.normal_pixbuf; }
			set {
				this.normal_pixbuf = value;
				active_pixbuf = normal_pixbuf;
				UpdateImage();
			}
		}

		public ImageView Image {
			get { return image; }
		}

		public bool DrawFocus {
			get { return draw_focus; }
			set {
				draw_focus = value;
				QueueDraw();
			}
		}
	}
}

