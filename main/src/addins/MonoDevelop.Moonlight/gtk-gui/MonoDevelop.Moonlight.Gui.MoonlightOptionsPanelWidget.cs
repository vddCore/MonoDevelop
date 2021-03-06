
// This file has been generated by the GUI designer. Do not modify.
namespace MonoDevelop.Moonlight.Gui
{
	public partial class MoonlightOptionsPanelWidget
	{
		private global::Gtk.VBox vbox1;
		private global::Gtk.VBox applicationOptionsBox;
		private global::Gtk.Label label1;
		private global::Gtk.Alignment alignment1;
		private global::Gtk.VBox vbox2;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Label label2;
		private global::Gtk.Entry xapFilenameEntry;
		private global::Gtk.Alignment alignment2;
		private global::Gtk.VBox vbox3;
		private global::Gtk.CheckButton generateManifestCheck;
		private global::Gtk.Alignment alignment4;
		private global::Gtk.Table manifestTable;
		private global::Gtk.ComboBoxEntry entryPointCombo;
		private global::Gtk.Label label6;
		private global::Gtk.Label label7;
		private global::Gtk.Entry manifestTemplateFilenameEntry;
		private global::Gtk.Label label3;
		private global::Gtk.Alignment alignment5;
		private global::Gtk.VBox vbox4;
		private global::Gtk.CheckButton generateTestPageCheck;
		private global::Gtk.Alignment alignment3;
		private global::Gtk.HBox testPageBox;
		private global::Gtk.Label label5;
		private global::Gtk.Entry testPageFilenameEntry;
		private global::Gtk.Label label4;
		private global::Gtk.Alignment xamlAlignment;
		private global::Gtk.VBox vbox6;
		private global::Gtk.CheckButton validateXamlCheck;
		private global::Gtk.CheckButton throwXamlErrorsCheck;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget MonoDevelop.Moonlight.Gui.MoonlightOptionsPanelWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "MonoDevelop.Moonlight.Gui.MoonlightOptionsPanelWidget";
			// Container child MonoDevelop.Moonlight.Gui.MoonlightOptionsPanelWidget.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 12;
			// Container child vbox1.Gtk.Box+BoxChild
			this.applicationOptionsBox = new global::Gtk.VBox ();
			this.applicationOptionsBox.Name = "applicationOptionsBox";
			this.applicationOptionsBox.Spacing = 12;
			// Container child applicationOptionsBox.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 0F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>XAP Generation</b>");
			this.label1.UseMarkup = true;
			this.applicationOptionsBox.Add (this.label1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.applicationOptionsBox [this.label1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child applicationOptionsBox.Gtk.Box+BoxChild
			this.alignment1 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment1.Name = "alignment1";
			this.alignment1.LeftPadding = ((uint)(16));
			// Container child alignment1.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("XAP _filename:");
			this.label2.UseUnderline = true;
			this.hbox1.Add (this.label2);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label2]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.xapFilenameEntry = new global::Gtk.Entry ();
			this.xapFilenameEntry.CanFocus = true;
			this.xapFilenameEntry.Name = "xapFilenameEntry";
			this.xapFilenameEntry.IsEditable = true;
			this.xapFilenameEntry.InvisibleChar = '???';
			this.hbox1.Add (this.xapFilenameEntry);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.xapFilenameEntry]));
			w3.Position = 1;
			this.vbox2.Add (this.hbox1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox1]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.alignment2 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment2.Name = "alignment2";
			// Container child alignment2.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.generateManifestCheck = new global::Gtk.CheckButton ();
			this.generateManifestCheck.CanFocus = true;
			this.generateManifestCheck.Name = "generateManifestCheck";
			this.generateManifestCheck.Label = global::Mono.Unix.Catalog.GetString ("_Generate manifest");
			this.generateManifestCheck.DrawIndicator = true;
			this.generateManifestCheck.UseUnderline = true;
			this.vbox3.Add (this.generateManifestCheck);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.generateManifestCheck]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.alignment4 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment4.Name = "alignment4";
			this.alignment4.LeftPadding = ((uint)(36));
			// Container child alignment4.Gtk.Container+ContainerChild
			this.manifestTable = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
			this.manifestTable.Name = "manifestTable";
			this.manifestTable.RowSpacing = ((uint)(6));
			this.manifestTable.ColumnSpacing = ((uint)(6));
			// Container child manifestTable.Gtk.Table+TableChild
			this.entryPointCombo = global::Gtk.ComboBoxEntry.NewText ();
			this.entryPointCombo.Name = "entryPointCombo";
			this.manifestTable.Add (this.entryPointCombo);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.manifestTable [this.entryPointCombo]));
			w6.TopAttach = ((uint)(1));
			w6.BottomAttach = ((uint)(2));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child manifestTable.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 0F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Entry _point:");
			this.label6.UseUnderline = true;
			this.manifestTable.Add (this.label6);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.manifestTable [this.label6]));
			w7.TopAttach = ((uint)(1));
			w7.BottomAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child manifestTable.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("Te_mplate file:");
			this.label7.UseUnderline = true;
			this.manifestTable.Add (this.label7);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.manifestTable [this.label7]));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child manifestTable.Gtk.Table+TableChild
			this.manifestTemplateFilenameEntry = new global::Gtk.Entry ();
			this.manifestTemplateFilenameEntry.CanFocus = true;
			this.manifestTemplateFilenameEntry.Name = "manifestTemplateFilenameEntry";
			this.manifestTemplateFilenameEntry.IsEditable = true;
			this.manifestTemplateFilenameEntry.InvisibleChar = '???';
			this.manifestTable.Add (this.manifestTemplateFilenameEntry);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.manifestTable [this.manifestTemplateFilenameEntry]));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			this.alignment4.Add (this.manifestTable);
			this.vbox3.Add (this.alignment4);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.alignment4]));
			w11.Position = 1;
			this.alignment2.Add (this.vbox3);
			this.vbox2.Add (this.alignment2);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.alignment2]));
			w13.Position = 1;
			this.alignment1.Add (this.vbox2);
			this.applicationOptionsBox.Add (this.alignment1);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.applicationOptionsBox [this.alignment1]));
			w15.Position = 1;
			// Container child applicationOptionsBox.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 0F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Test Page</b>");
			this.label3.UseMarkup = true;
			this.applicationOptionsBox.Add (this.label3);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.applicationOptionsBox [this.label3]));
			w16.Position = 2;
			w16.Expand = false;
			w16.Fill = false;
			// Container child applicationOptionsBox.Gtk.Box+BoxChild
			this.alignment5 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment5.Name = "alignment5";
			this.alignment5.LeftPadding = ((uint)(16));
			// Container child alignment5.Gtk.Container+ContainerChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.generateTestPageCheck = new global::Gtk.CheckButton ();
			this.generateTestPageCheck.CanFocus = true;
			this.generateTestPageCheck.Name = "generateTestPageCheck";
			this.generateTestPageCheck.Label = global::Mono.Unix.Catalog.GetString ("Generate _test page");
			this.generateTestPageCheck.DrawIndicator = true;
			this.generateTestPageCheck.UseUnderline = true;
			this.vbox4.Add (this.generateTestPageCheck);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.generateTestPageCheck]));
			w17.Position = 0;
			w17.Expand = false;
			w17.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.alignment3 = new global::Gtk.Alignment (0F, 0.5F, 1F, 1F);
			this.alignment3.Name = "alignment3";
			this.alignment3.LeftPadding = ((uint)(36));
			// Container child alignment3.Gtk.Container+ContainerChild
			this.testPageBox = new global::Gtk.HBox ();
			this.testPageBox.Name = "testPageBox";
			this.testPageBox.Spacing = 6;
			// Container child testPageBox.Gtk.Box+BoxChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Test _page filename:");
			this.label5.UseUnderline = true;
			this.testPageBox.Add (this.label5);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.testPageBox [this.label5]));
			w18.Position = 0;
			w18.Expand = false;
			w18.Fill = false;
			// Container child testPageBox.Gtk.Box+BoxChild
			this.testPageFilenameEntry = new global::Gtk.Entry ();
			this.testPageFilenameEntry.CanFocus = true;
			this.testPageFilenameEntry.Name = "testPageFilenameEntry";
			this.testPageFilenameEntry.IsEditable = true;
			this.testPageFilenameEntry.InvisibleChar = '???';
			this.testPageBox.Add (this.testPageFilenameEntry);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.testPageBox [this.testPageFilenameEntry]));
			w19.Position = 1;
			this.alignment3.Add (this.testPageBox);
			this.vbox4.Add (this.alignment3);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.alignment3]));
			w21.Position = 1;
			w21.Expand = false;
			w21.Fill = false;
			this.alignment5.Add (this.vbox4);
			this.applicationOptionsBox.Add (this.alignment5);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.applicationOptionsBox [this.alignment5]));
			w23.Position = 3;
			this.vbox1.Add (this.applicationOptionsBox);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.applicationOptionsBox]));
			w24.Position = 0;
			// Container child vbox1.Gtk.Box+BoxChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 0F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>XAML Validation</b>");
			this.label4.UseMarkup = true;
			this.vbox1.Add (this.label4);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label4]));
			w25.Position = 1;
			w25.Expand = false;
			w25.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.xamlAlignment = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.xamlAlignment.Name = "xamlAlignment";
			this.xamlAlignment.LeftPadding = ((uint)(16));
			// Container child xamlAlignment.Gtk.Container+ContainerChild
			this.vbox6 = new global::Gtk.VBox ();
			this.vbox6.Name = "vbox6";
			this.vbox6.Spacing = 6;
			// Container child vbox6.Gtk.Box+BoxChild
			this.validateXamlCheck = new global::Gtk.CheckButton ();
			this.validateXamlCheck.CanFocus = true;
			this.validateXamlCheck.Name = "validateXamlCheck";
			this.validateXamlCheck.Label = global::Mono.Unix.Catalog.GetString ("_Validate XAML");
			this.validateXamlCheck.DrawIndicator = true;
			this.validateXamlCheck.UseUnderline = true;
			this.vbox6.Add (this.validateXamlCheck);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.validateXamlCheck]));
			w26.Position = 0;
			w26.Expand = false;
			w26.Fill = false;
			// Container child vbox6.Gtk.Box+BoxChild
			this.throwXamlErrorsCheck = new global::Gtk.CheckButton ();
			this.throwXamlErrorsCheck.CanFocus = true;
			this.throwXamlErrorsCheck.Name = "throwXamlErrorsCheck";
			this.throwXamlErrorsCheck.Label = global::Mono.Unix.Catalog.GetString ("Throw _errors in XAML validation");
			this.throwXamlErrorsCheck.DrawIndicator = true;
			this.throwXamlErrorsCheck.UseUnderline = true;
			this.vbox6.Add (this.throwXamlErrorsCheck);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.throwXamlErrorsCheck]));
			w27.Position = 1;
			w27.Expand = false;
			w27.Fill = false;
			this.xamlAlignment.Add (this.vbox6);
			this.vbox1.Add (this.xamlAlignment);
			global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.xamlAlignment]));
			w29.Position = 2;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.label2.MnemonicWidget = this.xapFilenameEntry;
			this.label5.MnemonicWidget = this.testPageFilenameEntry;
			this.Hide ();
		}
	}
}
