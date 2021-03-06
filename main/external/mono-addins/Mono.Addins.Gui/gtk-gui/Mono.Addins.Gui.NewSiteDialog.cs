
// This file has been generated by the GUI designer. Do not modify.
namespace Mono.Addins.Gui
{
	internal partial class NewSiteDialog
	{
		private global::Gtk.VBox vbox89;
		private global::Gtk.Label label121;
		private global::Gtk.RadioButton btnOnlineRep;
		private global::Gtk.HBox hbox68;
		private global::Gtk.Label label122;
		private global::Gtk.Label label119;
		private global::Gtk.Entry urlText;
		private global::Gtk.RadioButton btnLocalRep;
		private global::Gtk.HBox hbox69;
		private global::Gtk.Label label123;
		private global::Gtk.Label label120;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Entry pathEntry;
		private global::Gtk.Button buttonBrowse;
		private global::Gtk.Button cancelbutton1;
		private global::Gtk.Button btnOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Mono.Addins.Gui.NewSiteDialog
			this.Name = "Mono.Addins.Gui.NewSiteDialog";
			this.Title = global::Mono.Unix.Catalog.GetString ("Add New Repository");
			this.TypeHint = ((global::Gdk.WindowTypeHint)(1));
			this.BorderWidth = ((uint)(6));
			this.DefaultWidth = 550;
			// Internal child Mono.Addins.Gui.NewSiteDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog-vbox11";
			w1.Spacing = 6;
			w1.BorderWidth = ((uint)(2));
			// Container child dialog-vbox11.Gtk.Box+BoxChild
			this.vbox89 = new global::Gtk.VBox ();
			this.vbox89.Name = "vbox89";
			this.vbox89.Spacing = 6;
			this.vbox89.BorderWidth = ((uint)(6));
			// Container child vbox89.Gtk.Box+BoxChild
			this.label121 = new global::Gtk.Label ();
			this.label121.Name = "label121";
			this.label121.Xalign = 0F;
			this.label121.LabelProp = global::Mono.Unix.Catalog.GetString ("Select the location of the repository you want to register:");
			this.vbox89.Add (this.label121);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox89 [this.label121]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox89.Gtk.Box+BoxChild
			this.btnOnlineRep = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("Register an on-line repository"));
			this.btnOnlineRep.CanFocus = true;
			this.btnOnlineRep.Name = "btnOnlineRep";
			this.btnOnlineRep.Active = true;
			this.btnOnlineRep.DrawIndicator = true;
			this.btnOnlineRep.UseUnderline = true;
			this.btnOnlineRep.Group = new global::GLib.SList (global::System.IntPtr.Zero);
			this.vbox89.Add (this.btnOnlineRep);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox89 [this.btnOnlineRep]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox89.Gtk.Box+BoxChild
			this.hbox68 = new global::Gtk.HBox ();
			this.hbox68.Name = "hbox68";
			this.hbox68.Spacing = 6;
			// Container child hbox68.Gtk.Box+BoxChild
			this.label122 = new global::Gtk.Label ();
			this.label122.WidthRequest = 32;
			this.label122.Name = "label122";
			this.hbox68.Add (this.label122);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox68 [this.label122]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox68.Gtk.Box+BoxChild
			this.label119 = new global::Gtk.Label ();
			this.label119.Name = "label119";
			this.label119.LabelProp = global::Mono.Unix.Catalog.GetString ("Url:");
			this.hbox68.Add (this.label119);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox68 [this.label119]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox68.Gtk.Box+BoxChild
			this.urlText = new global::Gtk.Entry ();
			this.urlText.CanFocus = true;
			this.urlText.Name = "urlText";
			this.urlText.IsEditable = true;
			this.urlText.InvisibleChar = '???';
			this.hbox68.Add (this.urlText);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox68 [this.urlText]));
			w6.Position = 2;
			this.vbox89.Add (this.hbox68);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox89 [this.hbox68]));
			w7.Position = 2;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox89.Gtk.Box+BoxChild
			this.btnLocalRep = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("Register a local repository"));
			this.btnLocalRep.CanFocus = true;
			this.btnLocalRep.Name = "btnLocalRep";
			this.btnLocalRep.DrawIndicator = true;
			this.btnLocalRep.UseUnderline = true;
			this.btnLocalRep.Group = this.btnOnlineRep.Group;
			this.vbox89.Add (this.btnLocalRep);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox89 [this.btnLocalRep]));
			w8.Position = 3;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbox89.Gtk.Box+BoxChild
			this.hbox69 = new global::Gtk.HBox ();
			this.hbox69.Name = "hbox69";
			this.hbox69.Spacing = 6;
			// Container child hbox69.Gtk.Box+BoxChild
			this.label123 = new global::Gtk.Label ();
			this.label123.WidthRequest = 32;
			this.label123.Name = "label123";
			this.hbox69.Add (this.label123);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox69 [this.label123]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child hbox69.Gtk.Box+BoxChild
			this.label120 = new global::Gtk.Label ();
			this.label120.Name = "label120";
			this.label120.LabelProp = global::Mono.Unix.Catalog.GetString ("Path:");
			this.hbox69.Add (this.label120);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox69 [this.label120]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			// Container child hbox69.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.pathEntry = new global::Gtk.Entry ();
			this.pathEntry.CanFocus = true;
			this.pathEntry.Name = "pathEntry";
			this.pathEntry.IsEditable = true;
			this.pathEntry.InvisibleChar = '???';
			this.hbox1.Add (this.pathEntry);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.pathEntry]));
			w11.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonBrowse = new global::Gtk.Button ();
			this.buttonBrowse.CanFocus = true;
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.UseUnderline = true;
			this.buttonBrowse.Label = global::Mono.Unix.Catalog.GetString ("Browse...");
			this.hbox1.Add (this.buttonBrowse);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonBrowse]));
			w12.Position = 1;
			w12.Expand = false;
			w12.Fill = false;
			this.hbox69.Add (this.hbox1);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox69 [this.hbox1]));
			w13.Position = 2;
			this.vbox89.Add (this.hbox69);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox89 [this.hbox69]));
			w14.Position = 4;
			w14.Expand = false;
			w14.Fill = false;
			w1.Add (this.vbox89);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox89]));
			w15.Position = 0;
			// Internal child Mono.Addins.Gui.NewSiteDialog.ActionArea
			global::Gtk.HButtonBox w16 = this.ActionArea;
			w16.Name = "dialog-action_area11";
			w16.Spacing = 10;
			w16.BorderWidth = ((uint)(5));
			w16.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog-action_area11.Gtk.ButtonBox+ButtonBoxChild
			this.cancelbutton1 = new global::Gtk.Button ();
			this.cancelbutton1.CanDefault = true;
			this.cancelbutton1.CanFocus = true;
			this.cancelbutton1.Name = "cancelbutton1";
			this.cancelbutton1.UseStock = true;
			this.cancelbutton1.UseUnderline = true;
			this.cancelbutton1.Label = "gtk-cancel";
			this.AddActionWidget (this.cancelbutton1, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w17 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w16 [this.cancelbutton1]));
			w17.Expand = false;
			w17.Fill = false;
			// Container child dialog-action_area11.Gtk.ButtonBox+ButtonBoxChild
			this.btnOk = new global::Gtk.Button ();
			this.btnOk.CanDefault = true;
			this.btnOk.CanFocus = true;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseStock = true;
			this.btnOk.UseUnderline = true;
			this.btnOk.Label = "gtk-ok";
			this.AddActionWidget (this.btnOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w18 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w16 [this.btnOk]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultHeight = 249;
			this.Hide ();
			this.btnOnlineRep.Clicked += new global::System.EventHandler (this.OnOptionClicked);
			this.urlText.Changed += new global::System.EventHandler (this.OnUrlTextChanged);
			this.btnLocalRep.Clicked += new global::System.EventHandler (this.OnOptionClicked);
			this.pathEntry.Changed += new global::System.EventHandler (this.OnPathEntryChanged);
			this.buttonBrowse.Clicked += new global::System.EventHandler (this.OnButtonBrowseClicked);
		}
	}
}
