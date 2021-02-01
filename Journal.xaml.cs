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
using aFernando_FinalProject;
using CrossPieCharts;

using System.IO;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace aFernando_FinalProject
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Journal : Page
	{
		ObservableCollection<Entry> Entries = new ObservableCollection<Entry>();
		public Journal()
		{
			
			this.InitializeComponent();
			
			///ClearJournalFile();

		}


		public class Entry
		{
			public string title;
			public string body;
			public string date;
			public string access;
			public string password;
			public string Source { get { return title; } }
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			ReadFileAsync();
			
		}
		public async void ReadFileAsync()
		{
			Windows.Storage.StorageFolder storageFolder =
					Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile JournalStore =
				await storageFolder.GetFileAsync("JournalStore.txt");


			//string s = await Windows.Storage.FileIO.ReadLinesAsync(JournalStore);
			//	ReadTextAsync(JournalStore);
			//if (s.Contains('_'))

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
		private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
		{

		}

		private void HamburgerButtonClick(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Hub));

		}

		private void JournalButtonClicK(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Journal));
		}
		private void Planner_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Planner));
		}

		

		

		public string GetToggleDate()
		{
			if (Pass.IsOn == true)
			{
				return "Private";
			}
			else if (Pass.IsOn == false)
			{
				return "Public";
			}

			else
			{
				return "";
			}
		}
		public async System.Threading.Tasks.Task WriteFileAsync()
		{
			// Create sample file; replace if exists.
			Windows.Storage.StorageFolder storageFolder =
				Windows.Storage.ApplicationData.Current.LocalFolder;

			

			Windows.Storage.StorageFile JournalEntry =
				await storageFolder.CreateFileAsync("JournalEntry.txt",
					Windows.Storage.CreationCollisionOption.OpenIfExists);
		
		}
		public DateTime dt { get; set; }
		
		private  void saveButton_ClickAsync(object sender, RoutedEventArgs e)
		{


			Entries.Add(new Entry()
			{
				title = TitleTextBox.Text,
				body = bodyBox.Text,
				date = datePicker.DateFormat,
				access = GetToggleDate(),
				password = Password.Text,
				

			});

			WriteFile();

		
		}

		private async void ReadFile(object sender, TextChangedEventArgs e)
		{
			Windows.Storage.StorageFolder storageFolder =
				Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile JournalStore =
				await storageFolder.GetFileAsync("JournalStore.txt");

			
		}


		private async void ClearJournalFile()
		{
			Windows.Storage.StorageFolder storageFolder =
				Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile JournalStore =
				await storageFolder.CreateFileAsync("JournalStore.txt",
					Windows.Storage.CreationCollisionOption.ReplaceExisting);
					
		}


		private async void WriteFile()
		{
			Windows.Storage.StorageFolder storageFolder =
			Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile JournalStore =
				await storageFolder.CreateFileAsync("JournalStore.txt",
					Windows.Storage.CreationCollisionOption.OpenIfExists);


			await Windows.Storage.FileIO.AppendTextAsync(JournalStore,
				TitleTextBox.Text + "|" + bodyBox.Text + "|" + datePicker.Date + "|"+ GetToggleDate() +"|"+ Password.Text+"\n");

			
		}

		
		
		private void journalListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			TitleTextBox.Text = "";
			bodyBox.Text = "";

			if (journalListBox.SelectedIndex > -1)
			{
				//get password
				string passwordUse = PasswordBox1.Text;
				var selected = journalListBox.Items[journalListBox.SelectedIndex] as Entry;
				if (selected.access.Equals("Private"))
				{
					//try 
					try
					{
						//is password equals entry in box
						if (selected.password.Equals(passwordUse))
						{
							bodyBox.Text = selected.body.ToString();
							TitleTextBox.Text = selected.title;
							datePicker.PlaceholderText = selected.date;
							
							ErrorLabel.Text = "";

						}
						else
						{
							ErrorLabel.Text = "Enter Password!";
						}
					}
					//catch if pass is null
					catch (System.NullReferenceException fail)
					{
						ErrorLabel.Text = "Enter Passowrd!";
					}
				}
				//if public no requistes
				else if (selected.access.Equals("Public"))
				{
					bodyBox.Text = selected.body.ToString();
					TitleTextBox.Text = selected.title;
					datePicker.PlaceholderText = selected.date;
					
				}


				}
			}


		private void Pass_Click(object sender, TappedRoutedEventArgs e)
		{
			journalListBox.SelectedIndex = -1;
		}

		private void PassButton_Click(object sender, RoutedEventArgs e)
		{
			//PasswordBox1.Text = passwordUse;
		}
	}
}
