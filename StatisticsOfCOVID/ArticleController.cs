using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.Configuration;

namespace StatisticsOfCOVID
{
    /// <summary>
    /// Контроллер взаимодействия модели с консолью.
    /// </summary>
    public class ArticleController
    {
        /// <summary>
        /// метод загружает данные по указанному адрессу и выводит статистику в консоль.
        /// </summary>
        public void SearchArticle()
        {
            try
            {
                string URLString = ConfigurationManager.AppSettings.Get("URLString");

                //парсим по ссылке статьи
                var result = XElement.Load(URLString)
                            .Descendants("item")
                            .Select(
                                el => new Article
                                {
                                    Title = el.Element("title").Value,
                                    Link = el.Element("link").Value,
                                    Guid = el.Element("guid").Value,
                                    Description = el.Element("description").Value,
                                    PubDate = el.Element("pubDate").Value
                                });

                //считаем количесво записей
                float articleCountAll = result.Count();
                Console.WriteLine("Всего статей: " + articleCountAll);

                dbSaveDateCOVID(result.ToList());

                //проверяем их на наличие ключевого слова
                List<string> keyTexts = new List<string>();
                keyTexts.Add("вирус");
                keyTexts.Add("короновирус");
                keyTexts.Add("COVID");

                List<Article> articlesCOVID = getListCOVID(keyTexts, result.ToList());
                result = null;

                float articleCountCOVID = articlesCOVID.Count();
                Console.WriteLine("Cтатей про короновирус: " + articleCountCOVID);

                Console.WriteLine("Отношение: " + articleCountCOVID / articleCountAll * 100 + "%");

                /*foreach (var r in articlesCOVID)
                {
                    Console.WriteLine("Title: {0}", r.title);
                }*/
                SaveDateCOVIDjson((int)articleCountCOVID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        } 

        /// <summary>
        /// Метод из всех статей, отбирает необходимые по ключевым словам.
        /// </summary>
        /// <param name="keyTexts">Список ключевых слов.</param>
        /// <param name="articles">Все статьи.</param>
        /// <returns>Возвращает список статей отобраных по ключевым словам.</returns>
        private List<Article> getListCOVID(List<string> keyTexts, List<Article> articles)
        {
            List<Article> listCOVIDs = new List<Article>();

            //пробегаемся по всем заголовкам статей.
            foreach (var r in articles)
            {
                //пробегаемся по всем ключевым словам.
                foreach (var kt in keyTexts)
                {
                    //сравниваем.
                    if (r.Title.Contains(kt))
                    {
                        listCOVIDs.Add(r);
                        break;
                    }
                }
            }
            return listCOVIDs;
        }

        /// <summary>
        /// Выводит кол-во статей о короновирусе.
        /// </summary>
        /// <param name="articleCountCOVID"></param>
        private void SaveDateCOVIDjson(int articleCountCOVID)
        {
            DateTime curDate = DateTime.Now;

            var myObj = new
            {
                date = curDate.ToString("yyyy-MM-dd HH:mm:ss"),
                result = articleCountCOVID
            };

            if (!File.Exists("InfoCOVID.json"))
                File.Create("InfoCOVID.json").Close();

            using (StreamWriter fs = new StreamWriter("InfoCOVID.json", true))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(fs, myObj);
                fs.Write("\n");
            }
        }

        /// <summary>
        /// Метод сохраняет статьи в бд
        /// </summary>
        /// <param name="articles">Список загруженых статей</param>
        private void dbSaveDateCOVID(List<Article> articles)
        {
            DateTime curDate = DateTime.Now;
            string databaseName = ConfigurationManager.AppSettings.Get("PathDB");
            string tableName = "COVID_" + curDate.ToString("yyyyMMdd_HHmmss");

            try
            {
                //создаем подключение к бд.
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName)))
                {
                    connection.Open();

                    //создаем новую таблицу в бд.
                    using (SQLiteCommand command = new SQLiteCommand(String.Format(SqlRequests.CREATE_NEW_TABLE, tableName), connection))
                        command.ExecuteNonQuery();

                    //Добавляем статьи в таблицу.
                    for (int i = 0; i < articles.Count; i++)
                    {
                        using (SQLiteCommand command =
                            new SQLiteCommand(
                                String.Format(
                                    SqlRequests.ADD_NEW_ARTICLE, tableName,
                                    articles[i].Title, articles[i].Link, articles[i].Guid,
                                    articles[i].Description, articles[i].PubDate), connection
                                             )
                               ) command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("ошибка в dbSaveDateCOVID " + ex);
            }
                
        }
    }
}
