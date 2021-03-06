
// This file has been generated by the GUI designer. Do not modify.
namespace Mono.Addins.Gui
{
	internal partial class InstallDialog
	{
		private global::Gtk.VBox vbox3;
		private global::Gtk.ScrolledWindow scrolledwindow1;
		private global::Gtk.VBox vbox4;
		private global::Gtk.Label labelInfo;
		private global::Gtk.HSeparator insSeparator;
		private global::Gtk.VBox boxProgress;
		private global::Gtk.Label globalProgressLabel;
		private global::Gtk.ProgressBar mainProgressBar;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Mono.Addins.Gui.InstallDialog
			this.Name = "Mono.Addins.Gui.InstallDialog";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child Mono.Addins.Gui.InstallDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 9;
			this.vbox3.BorderWidth = ((uint)(9));
			// Container child vbox3.Gtk.Box+BoxChild
			this.scrolledwindow1 = new global::Gtk.ScrolledWindow ();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.VscrollbarPolicy = ((global::Gtk.PolicyType)(2));
			this.scrolledwindow1.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
			// Container child scrolledwindow1.Gtk.Container+ContainerChild
			global::Gtk.Viewport w2 = new global::Gtk.Viewport ();
			w2.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.labelInfo = new global::Gtk.Label ();
			this.labelInfo.WidthRequest = 400;
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Xalign = 0F;
			this.labelInfo.Yalign = 0F;
			this.labelInfo.LabelProp = global::Mono.Unix.Catalog.GetString ("label3");
			this.labelInfo.Wrap = true;
			this.vbox4.Add (this.labelInfo);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.labelInfo]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			w2.Add (this.vbox4);
			this.scrolledwindow1.Add (w2);
			this.vbox3.Add (this.scrolledwindow1);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.scrolledwindow1]));
			w6.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.insSeparator = new global::Gtk.HSeparator ();
			this.insSeparator.Name = "insSeparator";
			this.vbox3.Add (this.insSeparator);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.insSeparator]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.boxProgress = new global::Gtk.VBox ();
			this.boxProgress.Name = "boxProgress";
			this.boxProgress.Spacing = 6;
			// Container child boxProgress.Gtk.Box+BoxChild
			this.globalProgressLabel = new global::Gtk.Label ();
			this.globalProgressLabel.Name = "globalProgressLabel";
			this.globalProgressLabel.Xalign = 0F;
			this.globalProgressLabel.Ellipsize = ((global::Pango.EllipsizeMode)(3));
			this.boxProgress.Add (this.globalProgressLabel);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.boxProgress [this.globalProgressLabel]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child boxProgress.Gtk.Box+BoxChild
			this.mainProgressBar = new global::Gtk.ProgressBar ();
			this.mainProgressBar.Name = "mainProgressBar";
			this.boxProgress.Add (this.mainProgressBar);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.boxProgress [this.mainProgressBar]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			this.vbox3.Add (this.boxProgress);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.boxProgress]));
			w10.Position = 2;
			w10.Expand = false;
			w10.Fill = false;
			w1.Add (this.vbox3);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox3]));
			w11.Position = 0;
			// Internal child Mono.Addins.Gui.InstallDialog.ActionArea
			global::Gtk.HButtonBox w12 = this.ActionArea;
			w12.Name = "dialog1_ActionArea";
			w12.Spacing = 10;
			w12.BorderWidth = ((uint)(5));
			w12.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			w12.Add (this.buttonCancel);
			global::Gtk.ButtonBox.ButtonBoxChild w13 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w12 [this.buttonCancel]));
			w13.Expand = false;
			w13.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = global::Mono.Unix.Catalog.GetString ("Install");
			w12.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w14 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w12 [this.buttonOk]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 494;
			this.DefaultHeight = 239;
			this.insSeparator.Hide ();
			this.Hide ();
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
