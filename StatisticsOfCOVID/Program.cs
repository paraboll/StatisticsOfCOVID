using System;
using System.Threading;

namespace StatisticsOfCOVID
{
    class Program
    {
        static void Main(string[] args)
        {
            const int HOUR = 3600000;
            //Таймер, вызывает выполнение задания раз в час.
            Timer t = new Timer(TimerCallback, null, 0, HOUR);
            Console.ReadKey();
        }

        private static void TimerCallback(Object o)
        {
            ArticleController articleController = new ArticleController();
            articleController.SearchArticle();
        }
    }
}
