using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace aFernando_FinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

		string name;
        public MainPage()
        {
            this.InitializeComponent();
			//clearing files at the beginning of entry of app
			ClearJournalFile();
			ClearNameFile();
			ClearPlannerFile();
			ClearEventFile();
        }

		//getting name
		private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			name = nameTextBox.Text;
			WriteNameFileAsync();


		}


		//enter names leads to navigation to hub
		private void enterNameButton_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Hub),name);
			
		}

		//saving name to file
		public async void WriteNameFileAsync()
		{

			Windows.Storage.StorageFolder storageFolder =
			Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile Name =
				await storageFolder.CreateFileAsync("helloname.txt",
					Windows.Storage.CreationCollisionOption.ReplaceExisting);

			await Windows.Storage.FileIO.WriteTextAsync(Name, name);
		}

		//cearing journal,planner,events files
		private async void ClearJournalFile()
		{
			Windows.Storage.StorageFolder storageFolder =
				Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile JournalStore =
				await storageFolder.CreateFileAsync("JournalStore.txt",
					Windows.Storage.CreationCollisionOption.ReplaceExisting);

		}

		private async void ClearPlannerFile()
		{
			Windows.Storage.StorageFolder storageFolder =
				Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile PlannerStore =
				await storageFolder.CreateFileAsync("PlannerStore.txt",
					Windows.Storage.CreationCollisionOption.ReplaceExisting);

		}

		private async void ClearEventFile()
		{
			Windows.Storage.StorageFolder storageFolder =
				Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile EventStore =
				await storageFolder.CreateFileAsync("EventStore.txt",
					Windows.Storage.CreationCollisionOption.ReplaceExisting);

		}

		private async void ClearNameFile()
		{
			Windows.Storage.StorageFolder storageFolder =
				Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile Name =
				await storageFolder.CreateFileAsync("helloname.txt",
					Windows.Storage.CreationCollisionOption.ReplaceExisting);

		}

		private void enterNamec(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key ==Windows.System.VirtualKey.Enter)
			{
				enterNameButton_Click(sender, e);
			}

		}
	}
}
