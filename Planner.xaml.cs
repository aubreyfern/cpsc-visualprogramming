using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//Planner page
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace aFernando_FinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Planner : Page
    {
		ObservableCollection<Plan> Plans = new ObservableCollection<Plan>();
		ObservableCollection<Event> Events = new ObservableCollection<Event>();

        public Planner()
        {
            this.InitializeComponent();
			
		}

		//upcoming events
		public class Event
		{
			public string eventTitle;
			public string eventDescription;
			public string eventType;
			public string date;
			public string time;

			public string EventSource { get { return eventTitle + "-" + date; } }
		}

		//todo list
		public class Plan
		{
			public string todo;
			public string Source { get { return todo; } }

		}

		//check is radio buttons are selected
		private void Option1RadioButton_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void Option2RadioButton_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void Option3RadioButton_Checked(object sender, RoutedEventArgs e)
		{

		}

		//empty
		public void AssignColor()
		{

			foreach (Event e in Events)
			{
				if (e.eventType == "School")
				{
					//var item = upcomingEvents.Items[0] as ListBoxItem;

					//upcomingEvents.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 0));



				}

			}
		}


		//get which radio button was selected
		public string GetRadioButtonData()
		{
			if (Option1RadioButton.IsChecked==true)
				return Option1RadioButton.Content.ToString();
			else if (Option2RadioButton.IsChecked == true)
				return Option2RadioButton.Content.ToString();
			else if (Option3RadioButton.IsChecked == true)
				return Option3RadioButton.Content.ToString();
			else {
				return "";
			}
		}

		//writing event to file
		private async void WriteFileEvent()
		{
			Windows.Storage.StorageFolder storageFolder =
			Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile EventStore =
				await storageFolder.CreateFileAsync("EventStore.txt",
					Windows.Storage.CreationCollisionOption.OpenIfExists);

			//convert to date time format
			string date = DatePicker.Date.ToString();
			string dateFinal = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
			string time = TimePicker.Time.ToString();
			string timeFinal = Convert.ToDateTime(time).ToString("hh:mm");


			await Windows.Storage.FileIO.AppendTextAsync(EventStore,
				EventTitle.Text + "|" + EventDescription.Text + "|" + GetRadioButtonData() +"|"+ dateFinal+"|"+timeFinal+"|"+"\n");
		}

		//read event file for re-entery
		public async void ReadEventAsync()
		{
			Windows.Storage.StorageFolder storageFolder =
						Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile EventStore =
				await storageFolder.GetFileAsync("EventStore.txt");

			
			foreach (string pls in await Windows.Storage.FileIO.ReadLinesAsync(EventStore))
			{
				string[] parts = pls.Split('|');
				Event et = new Event();
				et.eventTitle = parts[0];
				et.eventDescription = parts[1];
				et.eventType = parts[3];
				et.date = parts[4];
				et.time = parts[5];
				Events.Add(et);
			}
		}

		//writing todo to file
			private async void WriteFile()
		{
			Windows.Storage.StorageFolder storageFolder =
			Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile PlannerStore =
				await storageFolder.CreateFileAsync("PlannerStore.txt",
					Windows.Storage.CreationCollisionOption.OpenIfExists);


			await Windows.Storage.FileIO.AppendTextAsync(PlannerStore,
				addToDoBox.Text+"\n");
		}


		//reading todo list file
		public async void ReadTodoAsync()
		{
			Windows.Storage.StorageFolder storageFolder =
						Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile PlannerStore =
				await storageFolder.GetFileAsync("PlannerStore.txt");


			foreach (string pls in await Windows.Storage.FileIO.ReadLinesAsync(PlannerStore))
			{
				Plan p = new Plan();
				p.todo = pls;
				Plans.Add(p);

			}


		}

		//empty
		private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
		{

		}

		//navigate to hub
		private void HamburgerButtonClick(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Hub));

		}

		//navigate to journal
		private void JournalButtonClicK(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Journal));
		}

		//navigate to planner
		private void Planner_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Planner));
		}


		//saving a to do in list and file
		private void todoAdd_ClickAsync(object sender, RoutedEventArgs e)
		{
			Plans.Add(new Plan()
			{
				todo = addToDoBox.Text
			});

			WriteFile();
		}

		//read in files upon re-entry

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			ReadTodoAsync();
			ReadEventAsync();
		}
		private void todo_enterKey(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
			{
				todoAdd_ClickAsync(sender, e);
			}
		}

		//save event
		private void SaveEvent_Click(object sender, RoutedEventArgs e)
		{
			Event et = new Event();
			et.eventTitle = EventTitle.Text;
			et.eventDescription = EventDescription.Text;
			et.eventType = GetRadioButtonData();
			string date= DatePicker.Date.ToString();
			et.date = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
			string time = TimePicker.Time.ToString();
			et.time = Convert.ToDateTime(time).ToString("hh:mm");
			
			Events.Add(et);

			WriteFileEvent();
			//AssignColor();
		}

		
		//empty
		private void Upcoming_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			
		}

		private void Calendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
		{

			//string date = args.AddedDates.First().ToString();
			//return date;
		}

		private void CalendarView_DayItemSelected(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
		{
			//foreach (Event e in Events)
			//{
				
			//}

		}
	}
}

