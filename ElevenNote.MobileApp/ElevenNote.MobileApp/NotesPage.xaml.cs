using ElevenNote.MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ElevenNote.MobileApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotesPage : ContentPage
	{
        private List<NotesListItemViewModel> Notes { get; set; }
		public NotesPage ()
		{
            //reads in the Xaml and then allows us to use it's objects, so this needs to be the first thing
			InitializeComponent ();
            SetupUi();
		}


        private async Task PopluateNoteList()
        {
            //this is being connected from our Services model!
            await App.NoteService.GetAll().ContinueWith(task =>
            {
                var notes = task.Result;

                Notes = notes
                    .OrderByDescending(note => note.IsStarred)
                    .ThenByDescending(note => note.CreatedUtc)
                    .Select(s => new NotesListItemViewModel
                    {
                        NoteId = s.NoteId,
                        Title = s.Title,
                        StarImage = s.IsStarred ? "starred.png" : "notstarred.png",
                    }).ToList();

                lvwNotes.ItemsSource = Notes;

                //clears out the selected item property after a user drags to refresh the page.
                lvwNotes.SelectedItem = null;

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        //Job is to create the UI for pulling to referesh
        private void SetupUi()
        {

            lvwNotes.IsPullToRefreshEnabled = true;
            //refeshing is an EventHandler
            lvwNotes.Refreshing += async (o, args) =>
            {
                //this is the task that is being called immediately, instead of using Task, we can call a method. 
                await PopluateNoteList();
                lvwNotes.IsRefreshing = false;
                lblNoNotes.IsVisible = !Notes.Any();
            };
        }

        //there are a lot of events that you can override on a page, rotation orientation, etc. 
        //when the view appears we want it to automatically refresh itself
        //Notice all the async calls in this form
        //this is us binding to an event
        protected override async void OnAppearing()
        {
            await PopluateNoteList();
        }
    }
}
