﻿using BG_DAL.Entityes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BG_DAL.Services
{
   public interface IPersonageDataServices
    { /// <summary>
      /// Список всех персонажей
      /// </summary>
      /// <returns></returns>
        IEnumerable<PersonageData> GetAllPersonages();

        /// <summary>
        /// Информация о персонаже по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PersonageData> GetPersonageByIdAsync(int? id);

        /// <summary>
        /// Добавление нового/изменение персонажа и сохранение
        /// </summary>
        /// <param name="personage"></param>
        void SavePersonage(PersonageData personage);

        /// <summary>
        /// Удаление Персонажа
        /// </summary>
        /// <param name="personage"></param>
        Task DeletePersonageAsync(int id);

    }
}
