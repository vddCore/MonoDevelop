//
// Authors:
//   Christian Hergert	<chris@mosaix.net>
//   Daniel Morgan <danielmorgan@verizon.net>
//   Ben Motmans  <ben.motmans@gmail.com>
//
// Copyright (C) 2005 Mosaix Communications, Inc.
// Copyright (C) 2005 Daniel Morgan
// Copyright (c) 2007-2008 Ben Motmans
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
using System.Data;
using System.Data.OracleClient;
using System.Collections.Generic;
namespace MonoDevelop.Database.Sql.Oracle
{
	public class OracleConnectionProvider : AbstractConnectionProvider
	{
		public override IPooledDbConnection CreateConnection (IConnectionPool pool, DatabaseConnectionSettings settings, out string error)
		{
			string connStr = null;
			try {	
				if (settings.UseConnectionString) {
					connStr = settings.ConnectionString;
				} else {
					//Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;Integrated Security=no; 
					connStr = String.Format ("Data Source={0};User Id={1};Password={2};Integrated Security=no;",
						settings.Database, settings.Username, settings.Password);
				}
				connStr = SetConnectionStringParameter (connStr, String.Empty, "Pooling", "false");
				OracleConnection connection = new OracleConnection (connStr);
				connection.Open ();
				
				error = null;
				return new OraclePooledDbConnection (pool, connection);
			} catch (Exception e) {
				error = e.Message;
				return null;
			}
		}
	}
}
