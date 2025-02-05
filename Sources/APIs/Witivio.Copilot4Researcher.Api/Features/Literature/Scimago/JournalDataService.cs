namespace Witivio.Copilot4Researcher.Features.Literature.Scimago
{
    using System.Collections.Generic;
    using System.Formats.Asn1;
    using System.Globalization;
    using System.IO;
    using CsvHelper;
    using CsvHelper.Configuration;

    public interface IJournalDataService
    {
        Task<JournalData> SearchByTitleAsync(string title);
    }

    public class JournalDataService : IJournalDataService
    {
        private List<JournalData> _data;

        public JournalDataService()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "features/literature/scimago/data");
            _data = LoadCsvDataAsync(path).GetAwaiter().GetResult();
        }

        private async Task<List<JournalData>> LoadCsvDataAsync(string folderPath)
        {

            var csvFiles = Directory.GetFiles(folderPath, "*.csv");

            var allData = new List<JournalData>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null,
                BadDataFound = null,
                MissingFieldFound = null,
                PrepareHeaderForMatch = args => args.Header.Trim().Replace(" ", "").Replace(".", ""),
                IgnoreBlankLines = true,
                TrimOptions = TrimOptions.Trim
            };

            // Create a list of tasks for loading each file concurrently
            var loadTasks = csvFiles.Select(async filePath =>
           {
               var records = new List<JournalData>();

               using var reader = new StreamReader(filePath);
               using var csv = new CsvReader(reader, config);

               // Register any custom mappings if needed
               csv.Context.RegisterClassMap<JournalDataMap>();

               await foreach (var record in csv.GetRecordsAsync<JournalData>())
               {
                   records.Add(record);
               }

               return records;
           });
            // Wait for all tasks to complete and aggregate results
            var results = await Task.WhenAll(loadTasks);

            // Flatten the array of lists into a single list
            foreach (var records in results)
            {
                allData.AddRange(records);
            }

            return allData;
        }


        public async Task<JournalData> SearchByTitleAsync(string title)
        {


            title = SanitizeTitle(title);
            // Simulate async processing (like a database call) for demonstration
            return await Task.FromResult(_data.FirstOrDefault(j => SanitizeTitle(j.Title).Equals(title, StringComparison.OrdinalIgnoreCase)));
        }

        private string SanitizeTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return title;

            // Replace special characters with spaces
            title = title.Replace(":", " ")
                       .Replace("-", " ")
                       .Replace("_", " ");

            // Replace multiple spaces with a single space
            while (title.Contains("  "))
            {
                title = title.Replace("  ", " ");
            }

            return title.Trim();
        }
    }

}
