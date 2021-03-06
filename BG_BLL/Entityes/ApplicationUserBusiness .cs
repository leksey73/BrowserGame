using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BG_DAL.Entityes
{
    /// <summary>
    /// Класс пользователь
    /// </summary>
    public class ApplicationUserBusiness : ApplicationUserData
    {
        /// <summary>
        /// Имя Пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Год рождения
        /// </summary>
        public int Year { get; set; }
    }
}
