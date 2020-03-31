using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsOfCOVID
{
    /// <summary>
    /// Класс сожержит модель статьи
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Заголовок статьи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ссылки на статью.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Guid.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Орписание статьи.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата публикации.
        /// </summary>
        public string PubDate { get; set; }
    }
}
