using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace HRVEstimation
{
    public partial class App : Application
    {
        private static SQLiteHelper db;
        private static string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataStorage.db3");

        public static SQLiteHelper myDatabase {
            get {
                if (db==null)
                {
                    db = new SQLiteHelper(databasePath);
                }
                return db;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            //if (File.Exists(databasePath))
            //{
            //    File.Delete(databasePath);
            //}
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
