﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2015-09-02
// 

using cloudscribe.DbHelpers.MySql;
using Microsoft.Framework.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MySql
{

    internal class DBUserLogins
    {
        internal DBUserLogins(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;
        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;


        public async Task<bool> Create(
            int siteId,
            string loginProvider, 
            string providerKey,
            string providerDisplayName,
            string userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserLogins (");
            sqlCommand.Append("LoginProvider ,");
            sqlCommand.Append("ProviderKey, ");
            sqlCommand.Append("ProviderDisplayName, ");
            sqlCommand.Append("UserId ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append("?LoginProvider, ");
            sqlCommand.Append("?ProviderKey, ");
            sqlCommand.Append("?ProviderDisplayName, ");
            sqlCommand.Append("?UserId ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?LoginProvider", MySqlDbType.VarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new MySqlParameter("?ProviderKey", MySqlDbType.VarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[2].Value = userId;

            arParams[3] = new MySqlParameter("?SiteId", MySqlDbType.Int32);
            arParams[3].Value = siteId;

            arParams[4] = new MySqlParameter("?ProviderDisplayName", MySqlDbType.VarChar, 100);
            arParams[4].Value = providerDisplayName;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }



        public async Task<bool> Delete(
            string loginProvider,
            string providerKey,
            string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LoginProvider = ?LoginProvider AND ");
            sqlCommand.Append("ProviderKey = ?ProviderKey AND ");
            sqlCommand.Append("UserId = ?UserId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?LoginProvider", MySqlDbType.VarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new MySqlParameter("?ProviderKey", MySqlDbType.VarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[2].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<bool> DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("UserId = ?UserId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        public async Task<bool> DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("SiteId  = ?SiteId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteId", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }

        public async Task<DbDataReader> Find(
            int siteId,
            string loginProvider, 
            string providerKey)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId = ?SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("LoginProvider = ?LoginProvider ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ProviderKey = ?ProviderKey ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?LoginProvider", MySqlDbType.VarChar, 128);
            arParams[0].Value = loginProvider;

            arParams[1] = new MySqlParameter("?ProviderKey", MySqlDbType.VarChar, 128);
            arParams[1].Value = providerKey;

            arParams[2] = new MySqlParameter("?SiteId", MySqlDbType.Int32);
            arParams[2].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<DbDataReader> GetByUser(
            int siteId,
            string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteId = ?SiteId ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("UserId = ?UserId  ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserId", MySqlDbType.VarChar, 128);
            arParams[0].Value = userId;

            arParams[1] = new MySqlParameter("?SiteId", MySqlDbType.Int32);
            arParams[1].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        
    }
}
