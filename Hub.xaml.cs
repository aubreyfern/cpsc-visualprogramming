using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

//Hub is the dashboard of the app
namespace aFernando_FinalProject
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Hub : Page
	{
		//bindinglists
		ObservableCollection<Entry> Entries = new ObservableCollection<Entry>();
		ObservableCollection<Plan> Plans = new ObservableCollection<Plan>();
		ObservableCollection<Event> Events = new ObservableCollection<Event>();

		public class PieChart
		{

		}
		public Hub()
		{
			this.InitializeComponent();
			//clearing items upon new initliazation
			journalListBox.Items.Clear();
			resultListBox.Items.Clear();
		}

		//todolist 
		public class Plan
		{
			public string todo;
			public string Source { get { return todo; } }

		}

		//upcoming events
		public class Event
		{
			public string eventTitle;
			public string eventDescription;
			public string eventType;
			public string date;
			public string time;

			public string Source { get { return eventTitle + "-" + date; } }
		}

		//journal Entries
		public class Entry
		{
			public string title;
			public string body;
			public string date;
			public string access;
			public string password;
			public string Source { get { return title; } }
			public string sourceDate { get { return date; } }
		}

		
		
		public async void WriteFileAsync()
		{
			Windows.Storage.StorageFolder storageFolder =
			Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile JournalStore =
				await storageFolder.CreateFileAsync("JournalStore.txt",
					Windows.Storage.CreationCollisionOption.ReplaceExisting);

		}

		//reading name file to dashboard
		public async void ReadNameFileAsync()
		{
			Windows.Storage.StorageFolder storageFolder =
					Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile Name =
				await storageFolder.GetFileAsync("helloname.txt");

			string name2 = await Windows.Storage.FileIO.ReadTextAsync(Name);
			helloLabel.Text = "Hello " + name2 + ",";

		}


		//reading events file
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

		//reading journal file
		public async void ReadFileAsync()
		{
		
			
				Windows.Storage.StorageFolder storageFolder =
						Windows.Storage.ApplicationData.Current.LocalFolder;
				Windows.Storage.StorageFile JournalStore =
					await storageFolder.GetFileAsync("JournalStore.txt");

			
				foreach (string pls in await Windows.Storage.FileIO.ReadLinesAsync(JournalStore))
				{
					string[] parts2 = pls.Split('|');


					Entry e = new Entry();
					e.title = parts2[0];
					e.body = parts2[1];

					e.date = parts2[2];
					e.access = parts2[3];
					e.password = parts2[4];

					Entries.Add(e);

				}

		}

		
		//readfiles on navigated
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			string myName = e.Parameter as String;
			
			
			ReadFileAsync();
			ReadNameFileAsync();
			ReadEventAsync();
			ReadTodoAsync();
		}

		private void NavigationViewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//empty
		}

		private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
		{

		}

		//navigation to hub
		private void HamburgerButtonClick(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Hub));
		}

		//navigation to journal 
		private void JournalButtonClicK(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Journal));
		}
		
		//empty
		private void Journal_SelectedChanged(object sender, SelectionChangedEventArgs e)
		{
		}


		//navigate to planner
		private void Planner_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Planner));
		}

		//single selection mode
		public SelectionMode SelectionMode { get; set; }

		//get journal entry info upon selection
		private void journalEntries_SelectedIndex(object sender, SelectionChangedEventArgs e)
		{
			if (journalListBox.SelectedIndex > -1)
			{

				var selected = journalListBox.Items[journalListBox.SelectedIndex] as Entry;
				string passwordUse = PasswordBox1.Text;

				if (selected.access.Equals("Private"))
				{
					try
					{
						if (selected.password.Equals(passwordUse))
						{
							viewTextBox.Text = selected.body.ToString();

							ErrorLabel.Text = "";

						}
						else
						{
							ErrorLabel.Text = "Enter Password!";
						}
					}
					catch (System.NullReferenceException fail)
					{
						ErrorLabel.Text = "Enter Passowrd!";
					}
				}

				else if (selected.access.Equals("Public"))
				{
					viewTextBox.Text = selected.body.ToString();


				}


			
			}

		}

		//searching for something
		private void Search_Click(object sender, RoutedEventArgs e)
		{
			resultListBox.Items.Clear();
			//search through journal
			foreach (Entry entrySearch in Entries)
			{
				if (searchBar.Text.Equals(entrySearch.title))
				{
					resultListBox.Items.Add(entrySearch);
				}
				else if (entrySearch.body.Contains(searchBar.Text))
				{
					resultListBox.Items.Add(entrySearch);
				}

				else if (entrySearch.date.Contains(searchBar.Text))
				{
					resultListBox.Items.Add(entrySearch);
				}
				else if (entrySearch.access.Contains(searchBar.Text))
				{
					resultListBox.Items.Add(entrySearch);

				}
			}

			//search through events
			foreach (Event eventSearch in Events)
			{
				if (eventSearch.eventTitle.Contains(searchBar.Text))
				{
					resultListBox.Items.Add(eventSearch);
				}

				else if (eventSearch.eventDescription.Contains(searchBar.Text))
				{
					resultListBox.Items.Add(eventSearch);
				}

				else if (eventSearch.eventType.Contains(searchBar.Text))
				{
					resultListBox.Items.Add(eventSearch);
				}

				else if (eventSearch.date.Contains(searchBar.Text))
				{
					resultListBox.Items.Add(eventSearch);
				}
			
			}

			


		}

		//get data from search results
		private void resultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			if (resultListBox.SelectedIndex > -1)
			{
				Type type = resultListBox.SelectedItem.GetType();
				if (type.Equals(typeof(Entry)))
				{
					var selected = resultListBox.Items[resultListBox.SelectedIndex] as Entry;
					viewTextBox.Text = selected.body.ToString();

				}
				else if (type.Equals(typeof(Event)))
				{
					var selected2 = resultListBox.Items[resultListBox.SelectedIndex] as Event;
					viewTextBox.Text = selected2.eventDescription.ToString();

				}
			}

		}
		private void Pass_Click(object sender, TappedRoutedEventArgs e)
		{
			//PasswordBox1.Text = "";
		}

		private void PassButton_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void SearchEnter_ClickKey(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
			{
				Search_Click(sender, e);
			}
		}
		
	}
}
