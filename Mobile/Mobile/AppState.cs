﻿using DataManager;
using DataManager.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mobile
{
    public class AppState
    {
        private LocalDBManager<ReviewDatabaseContext> dbManager;

        public LocalDBManager<ReviewDatabaseContext> DBManager
        {
            get
            {
                if (dbManager == null)
                {
                    dbManager = new LocalDBManager<ReviewDatabaseContext>();
                }
                return dbManager;
            }
        }

        //private FileContentManager dbManager;

        //public FileContentManager DBManager
        //{
        //    get
        //    {
        //        if (dbManager == null)
        //        {
        //            dbManager = new FileContentManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.json"));
        //        }
        //        return dbManager;
        //    }
        //}

        //private FakeContentManager dbManager;

        //public FakeContentManager DBManager
        //{
        //    get
        //    {
        //        if (dbManager == null)
        //        {
        //            dbManager = new FakeContentManager();
        //        }
        //        return dbManager;
        //    }
        //}
    }
}
