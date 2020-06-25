using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reminder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string ContentSeperator = "@@";
        private readonly string Db = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\db.txt";
        private readonly string Path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\study.txt";
        private readonly string NewLine = @"\r\n";
        private readonly List<string> itemsToRemove = new List<string>();
        private readonly List<string> itemsToUpdate = new List<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();

            if (!File.Exists(Path))
                File.Create(Path).Close();

            if (!File.Exists(Db))
                File.Create(Db).Close();
        }

        // Main Logic
        private async void Timer_Tick(object sender, EventArgs e)
        {
            var allItems = await SyncDb();

            var processedItems = await ProcessItems(allItems);

            var itemsToShow = processedItems.ToList();

            await CleanUp();

            itemsToShow.ForEach(a => MessageBox.Show(a));
        }

        private async Task<IList<string>> ProcessItems(ICollection<string> items)
        {
            var itemsToShow = new List<string>();
            foreach (var content in items)
            {
                if (content.StartsWith(NewLine))
                    continue;

                bool show = await ShowContent(content);
                if (show)
                    itemsToShow.Add(content);
            }

            return itemsToShow;

            async Task<bool> ShowContent(string strContent)
            {
                var items = await GetDbItems();

                foreach (var item in items)
                {
                    var (viewsLeft, lastViewed, content) = item.DeconstructDbContent();

                    if (strContent.Equals(content))
                        return CheckTiming(viewsLeft, lastViewed, content);
                }

                return false;
            }

            bool CheckTiming(int viewsLeft, DateTime lastViewed, string content)
            {
                var anHourAgo = DateTime.Now.AddHours(-1);
                var past20Mins = DateTime.Now.AddMinutes(-20);

                // Show the content if the time has come
                if ((lastViewed < anHourAgo && viewsLeft == 1) ||
                    (lastViewed < past20Mins && viewsLeft == 2))
                {
                    string newItem = content.ConstructNewContent(viewsLeft - 1);
                    itemsToUpdate.Add(newItem);
                    return true;
                }
                else
                    return false;
            }
        }

        private async Task<IList<string>> SyncDb()
        {
            var allItems = await GetFileItems();
            if (allItems.Any())
            {
                string allDbContent = GetDbContent();
                var dbItems = GetPlainDbItems(allDbContent);

                var insertList = allItems.Except(dbItems);

                WriteToDb(allDbContent, insertList);
            }
            else
            {
                // Wipe out db
                using var dbFile = new StreamWriter(Db);
            }
            return allItems;
        }

        private string GetDbContent()
        {
            var db = new StreamReader(Db);
            string allDbContent = db.ReadToEnd();
            db.Close();
            return allDbContent;
        }

        private static IList<string> GetPlainDbItems(string allDbContent)
            => allDbContent.SplitContents()
                           .Select(a => a.DeconstructDbContent().Content)
                           .ToList();

        private void WriteToDb(string allDbContent, IEnumerable<string> insertList)
        {
            var dbFile = new StreamWriter(Db);
            string newItems = GetNewItems(insertList);
            allDbContent += newItems;
            dbFile.Write(allDbContent);
            dbFile.Close();
        }

        private static string GetNewItems(IEnumerable<string> insertList)
            => string.Join(string.Empty, insertList.Select(a => a.ConstructNewContent(2)).ToList());


        private async Task<IList<string>> GetFileItems()
        {
            var file = new StreamReader(Path);
            string all = await file.ReadToEndAsync();
            file.Close();
            List<string> allItems = all.SplitContents();
            return allItems;
        }

        private async Task CleanUp()
        {
            await CleanUpDb();

            foreach (var content in itemsToUpdate)
                await UpdateOnDb(content);

            await CleanUpFile();

            itemsToRemove.Clear();
            itemsToUpdate.Clear();
        }

        private async Task UpdateOnDb(string contentToUpdate)
        {
            var items = await GetDbItems();
            UpdateContentOnDb(contentToUpdate, items);
        }

        private async Task<IList<string>> GetDbItems()
        {
            var fileToRead = new StreamReader(Db);

            string all = await fileToRead.ReadToEndAsync();
            List<string> items = all.SplitContents();
            fileToRead.Close();
            return items;
        }

        private void UpdateContentOnDb(string contentToUpdate, ICollection<string> items)
        {
            var (_, _, newContentPlain) = contentToUpdate.DeconstructDbContent();
            var file = new StreamWriter(Db);
            foreach (var content in items)
            {
                var (_, _, oldContentPlain) = content.DeconstructDbContent();

                if (oldContentPlain == newContentPlain)
                    file.Write(contentToUpdate);
                else
                    file.Write(ContentSeperator + content);
            }

            file.Close();
        }

        private async Task CleanUpFile()
        {
            var plainItems = new List<string>();
            itemsToRemove.ForEach(a => plainItems.Add(a.DeconstructDbContent().Content));

            var items = await GetFileItems();

            using var file = new StreamWriter(Path);
            foreach (var content in items)
                if (!plainItems.Contains(content))
                    await file.WriteAsync(ContentSeperator + content);
        }

        public async Task CleanUpDb()
        {
            var items = await GetDbItems();

            foreach (var item in items)
                if (item.DeconstructDbContent().RemainingViewCount == 0)
                    itemsToRemove.Add(item);

            await WriteItemsToDb(items);
        }

        private async Task WriteItemsToDb(IList<string> items)
        {
            var file = new StreamWriter(Db);
            foreach (var content in items)
                if (!itemsToRemove.Contains(content))
                    await file.WriteAsync(ContentSeperator + content);
            file.Close();
        }
    }
}
