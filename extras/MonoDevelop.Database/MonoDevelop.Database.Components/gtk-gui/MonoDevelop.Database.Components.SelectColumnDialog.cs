
// This file has been generated by the GUI designer. Do not modify.
namespace MonoDevelop.Database.Components
{
	public partial class SelectColumnDialog
	{
		private global::Gtk.HBox hboxContent;

		private global::Gtk.VButtonBox vbuttonbox1;

		private global::Gtk.Button buttonSelectAll;

		private global::Gtk.Button buttonDeselectAll;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget MonoDevelop.Database.Components.SelectColumnDialog
			this.Name = "MonoDevelop.Database.Components.SelectColumnDialog";
			this.Title = global::MonoDevelop.Database.AddinCatalog.GetString ("Select Column");
			this.TypeHint = ((global::Gdk.WindowTypeHint)(1));
			this.WindowPosition = ((global::Gtk.WindowPosition)(1));
			this.DestroyWithParent = true;
			this.SkipTaskbarHint = true;
			// Internal child MonoDevelop.Database.Components.SelectColumnDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "Dialog_Vbox";
			w1.BorderWidth = ((uint)(2));
			// Container child Dialog_Vbox.Gtk.Box+BoxChild
			this.hboxContent = new global::Gtk.HBox ();
			this.hboxContent.Name = "hboxContent";
			this.hboxContent.Spacing = 6;
			// Container child hboxContent.Gtk.Box+BoxChild
			this.vbuttonbox1 = new global::Gtk.VButtonBox ();
			this.vbuttonbox1.Name = "vbuttonbox1";
			this.vbuttonbox1.Spacing = 6;
			this.vbuttonbox1.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(3));
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonSelectAll = new global::Gtk.Button ();
			this.buttonSelectAll.CanFocus = true;
			this.buttonSelectAll.Name = "buttonSelectAll";
			this.buttonSelectAll.UseUnderline = true;
			this.buttonSelectAll.Label = global::MonoDevelop.Database.AddinCatalog.GetString ("Select All");
			this.vbuttonbox1.Add (this.buttonSelectAll);
			global::Gtk.ButtonBox.ButtonBoxChild w2 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.buttonSelectAll]));
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonDeselectAll = new global::Gtk.Button ();
			this.buttonDeselectAll.CanFocus = true;
			this.buttonDeselectAll.Name = "buttonDeselectAll";
			this.buttonDeselectAll.UseUnderline = true;
			this.buttonDeselectAll.Label = global::MonoDevelop.Database.AddinCatalog.GetString ("Deselect All");
			this.vbuttonbox1.Add (this.buttonDeselectAll);
			global::Gtk.ButtonBox.ButtonBoxChild w3 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.buttonDeselectAll]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			this.hboxContent.Add (this.vbuttonbox1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hboxContent[this.vbuttonbox1]));
			w4.PackType = ((global::Gtk.PackType)(1));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			w1.Add (this.hboxContent);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(w1[this.hboxContent]));
			w5.Position = 0;
			// Internal child MonoDevelop.Database.Components.SelectColumnDialog.ActionArea
			global::Gtk.HButtonBox w6 = this.ActionArea;
			w6.Name = "GtkDialog_ActionArea";
			w6.Spacing = 6;
			w6.BorderWidth = ((uint)(5));
			w6.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child GtkDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w7 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w6[this.buttonCancel]));
			w7.Expand = false;
			w7.Fill = false;
			// Container child GtkDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.Sensitive = false;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget (this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w6[this.buttonOk]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.Show ();
			this.buttonSelectAll.Clicked += new global::System.EventHandler (this.SelectAllClicked);
			this.buttonDeselectAll.Clicked += new global::System.EventHandler (this.DeselectAllClicked);
		}
	}
}
