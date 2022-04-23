//
// Authors:
//    Ben Motmans  <ben.motmans@gmail.com>
//
// Copyright (c) 2007 Ben Motmans
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

using Gtk;
using System;
using System.Data;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Database.Designer;
using MonoDevelop.Database.Components;

{
	public class SqliteGuiProvider : IGuiProvider
	{
//		public bool ShowSelectDatabaseDialog (bool create, out string database)
//		{
//			FileChooserDialog dlg = null;
//			if (create) {
//				dlg = new FileChooserDialog (
//					AddinCatalog.GetString ("Save Database"), null, FileChooserAction.Save,
//					"gtk-cancel", ResponseType.Cancel,
//					"gtk-save", ResponseType.Accept
//				);
//			} else {
//				dlg = new FileChooserDialog (
//					AddinCatalog.GetString ("Open Database"), null, FileChooserAction.Open,
//					"gtk-cancel", ResponseType.Cancel,
//					"gtk-open", ResponseType.Accept
//				);
//			}
//			dlg.SelectMultiple = false;
//			dlg.LocalOnly = true;
//			dlg.Modal = true;
//			
//			FileFilter filter = new FileFilter ();
//			filter.AddMimeType ("application/x-sqlite2");
//			filter.AddMimeType ("application/x-sqlite3");
//			filter.AddPattern ("*.db");
//			filter.Name = AddinCatalog.GetString ("SQLite databases");
//			FileFilter filterAll = new FileFilter ();
//			filterAll.AddPattern ("*");
//			filterAll.Name = AddinCatalog.GetString ("All files");
//			dlg.AddFilter (filter);
//			dlg.AddFilter (filterAll);
//
//			if (dlg.Run () == (int)ResponseType.Accept) {
//				database = dlg.Filename;
//				dlg.Destroy ();
//				return true;
//			} else {
//				dlg.Destroy ();
//				database = null;
//				return false;
//			}
//		}
		
		public bool ShowCreateDatabaseDialog (IDbFactory factory)
		{
			SqliteCreateDatabaseDialog dialog = new SqliteCreateDatabaseDialog (factory);
			int resp;
			do {
				resp = dialog.Run ();
			} while (resp != (int)ResponseType.Cancel && 
				    	     resp != (int)ResponseType.Ok && 
				    		resp != (int)ResponseType.DeleteEvent);
			dialog.Destroy ();
			if (resp == (int)ResponseType.Ok)
				return true;
			else
				return false;

		}
		
		public bool ShowAddConnectionDialog (IDbFactory factory)
		{
			return RunDialog (new SqliteDatabaseConnectionSettingsDialog (factory));
		}
		
		public bool ShowEditConnectionDialog (IDbFactory factory, 
		                                      DatabaseConnectionSettings settings, 
		                                      out DatabaseConnectionSettings newSettings)
		{
			
			SqliteDatabaseConnectionSettingsDialog dlg = new  SqliteDatabaseConnectionSettingsDialog(factory, settings);
			bool result = RunDialog (dlg);
			if (result)
				newSettings = dlg.ConnectionSettings;
			else
				newSettings = null;			
			return result;
		}

		public bool ShowTableEditorDialog (IEditSchemaProvider schemaProvider, TableSchema table, bool create)
		{
			TableEditorSettings settings = new TableEditorSettings ();
			// SQLite doesn't support "NO ACTION" on "Foreign Key"
			settings.ConstraintSettings.ForeignKeySettings.SupportsNoAction = false;
			TableEditorDialog dlg = new TableEditorDialog (schemaProvider, create, settings);
			dlg.Initialize (table);

			return RunDialog (dlg);
		}

		public bool ShowViewEditorDialog (IEditSchemaProvider schemaProvider, ViewSchema view, bool create)
		{
			ViewEditorSettings settings = new ViewEditorSettings ();
			ViewEditorDialog dlg = new ViewEditorDialog (schemaProvider, create, settings);
			dlg.Initialize (view);

			return RunDialog (dlg);
		}

		public bool ShowProcedureEditorDialog (IEditSchemaProvider schemaProvider, ProcedureSchema procedure, bool create)
		{
			ProcedureEditorSettings settings = new ProcedureEditorSettings ();
			ProcedureEditorDialog dlg = new ProcedureEditorDialog (schemaProvider, create, settings);
			dlg.Initialize (procedure);

			return RunDialog (dlg);
		}

		public bool ShowUserEditorDialog (IEditSchemaProvider schemaProvider, UserSchema user, bool create)
		{
			return false; //TODO: implement ShowUserEditorDialog
		}

		private bool RunDialog (Dialog dlg)
		{
			bool result = false;
			// If the Preview Dialog is canceled, don't execute and don't close the Editor Dialog.
			try {
				int resp;
					do {
						resp = dlg.Run ();
				         } while (resp != (int)ResponseType.Cancel && resp != (int)ResponseType.Ok && 
				         resp != (int)ResponseType.DeleteEvent);
					
				if (resp == (int)ResponseType.Ok)
					result = true;
				else
					result = false;
			} finally {
				dlg.Destroy ();
			}
			return result;
		}
	}
}