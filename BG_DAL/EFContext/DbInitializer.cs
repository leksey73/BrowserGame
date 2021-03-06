
using BG_DAL.Entityes;
using System.Linq;
using System.Threading.Tasks;

namespace BG_DAL.EFContext
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Personages.Any())
            {
                return;   // DB has been seeded
            }

            var usersLogin = new PersonageData[]
            {
            new PersonageData{Name="Alexander",History="Живет футболом",Possession="Нападающий",Category="Лига Европы",Capital="700000",Id=1},
            new PersonageData{Name="Roman",History="Любит Деньги",Possession="Защитник",Category="Лига Европы",Capital="500000",Id=2},
            };
            foreach (PersonageData s in usersLogin)
            {
                context.Personages.Add(s);
            }
            context.SaveChanges();

        }
    }
}