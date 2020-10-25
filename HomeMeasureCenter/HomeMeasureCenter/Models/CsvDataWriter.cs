using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HomeMeasureCenter.Models
{
    public class CsvDataWriter
    {
        private const string APPLICATION_NAME = "HomeMeasureCenter";
        private static StorageFolder appFolder = null;

        public async void AddMeasurement(DateTime a_instant, double a_temperature, double a_humidity)
        {
            StorageFolder folder = await GetAppFolder();
            string fileName = a_instant.ToString("yyyyMMdd") + "_" + APPLICATION_NAME + ".csv";
            IStorageItem storageItem = await folder.TryGetItemAsync(fileName);
            StorageFile file;
            List<string> rows = new List<string>();
            if (storageItem == null)
            {
                file = await folder.CreateFileAsync(fileName);
                rows.Add("HEURE" + ";" + "TEMPERATURE (°C)" + ";" + "HUMIDITE RELATIVE (%HR)");
            }
            else
            {
                file = (StorageFile)storageItem;
            }
            rows.Add(a_instant.ToString("HH:mm:ss" + ";" + a_temperature.ToString("f1") + ";" + a_humidity.ToString("f1")));
            await FileIO.AppendLinesAsync(file, rows);
        }

        private async static Task<StorageFolder> GetAppFolder()
        {
            if(appFolder == null)
            {
                appFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(APPLICATION_NAME, CreationCollisionOption.OpenIfExists);
            }
            return appFolder;
        }

    }
}
