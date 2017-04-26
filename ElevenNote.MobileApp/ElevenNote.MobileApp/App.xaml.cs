using ElevenNote.MobileApp.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ElevenNote.MobileApp
{
	public partial class App : Application
	{
        internal static readonly NoteService NoteService = new NoteService();
		public App ()
		{
			InitializeComponent();
            //MainPage is apart of Applicaiton, where App is being inherited from. So it has accessed from a property off of Applicaiton.
            //this is a very important property in the application. 
            //The keyword this is implied. 
            //this.MainPage = new ElevenNote.MobileApp.MainPage();
            //                                 Navagation Page is looking for an instance of a created view. 
            this.MainPage = new NavigationPage(new LoginPage());
		}

        //Xamarin extracts this methods becasue essentially all Mobile applications handle these functionalities.
		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
