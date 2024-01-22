using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models.Chat;
using OutsourcePlatformApp.Models.Notifications;

namespace OutsourcePlatformApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Executor> Executors { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactLink> ContactLinks { get; set; }
        public DbSet<ActionNotification> Notifications { get; set; }
        public DbSet<OrderVacancy> OrderVacancies { get; set; }
        public DbSet<OrderRole> OrderRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ActivationToken> ActivationTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userRoles = new List<UserRole>
            {
                new UserRole { Id = 1, Name = "executor_role" },
                new UserRole { Id = 2, Name = "customer_role" },
                new UserRole { Id = 3, Name = "admin_role" }
            };
            modelBuilder.Entity<UserRole>().HasData(userRoles);
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Разработка сайтов" },
                new Category { Id = 2, Name = "Дизайн и арт" },
                new Category { Id = 3, Name = "Программирование" },
                new Category { Id = 4, Name = "Аудио/Видео" },
                new Category { Id = 5, Name = "Разработка игр" },
                new Category { Id = 6, Name = "Реклама и маркетинг" },
                new Category { Id = 7, Name = "Аутсорсинг и консалтинг" },
                new Category { Id = 8, Name = "Анимация и флеш" },
                new Category { Id = 9, Name = "3D графика" },
                new Category { Id = 10, Name = "Фотография" },
                new Category { Id = 11, Name = "Инжиниринг" },
                new Category { Id = 12, Name = "Оптимизация " },
                new Category { Id = 13, Name = "Мобильные приложения" },
                new Category { Id = 14, Name = "Сети и инфосистемы" }
            };
            modelBuilder.Entity<Category>().HasData(categories);
            var skillList = new[]
            {
                "machine learning", "opencv", "dlib", "tensorflow", "keras", "php", "mysql", "kotlin", "ios", "android",
                "deep learning", "статистика", "математическое моделирование", "тестирование ПО", "big data",
                "flash-анимация", "react native", "тестирование", "sql", "postgresql", "интерфейсы", "tilda",
                "анализ данных", "backend", "аналитика данных", "мобильная разработка", "java", "swift", "xamarin",
                "3d графика", "unity", "sketch", "радиоэлектроника", "иностранные языки", "c++", "ar", "vr",
                "экономика", "html", "css", "scss", "базы данных", "git", "python", "javascript", "figma", "frontend",
                "c#", "ux-аналитика", "linux", "project management", "коммуникабельность", "ведение документации",
                "составление отчёта", "сбор информации", "понимание композиции",
                "умение читать техническую документацию и научные статьи", "умение отвечать за качество результата",
                "работа в команде", "наблюдение за работой группы", "уточнение требований", "распределение ролей",
                "постановка и контроль выполнения задач по графику", "организация процесса разработки",
                "знание английского языка (разговорный уровень)",
                "навыки управления проектами: постановка промежуточных задач", "общение с заказчиком",
                "выявление проблем и способы их решения", "составление документации",
                "отслеживание выполнения задач во времени", "корректировка проекта",
                "умение анализировать и структурировать данные навыки коммуникации",
                "с навыками программирования и организационными способностями",
                "умение организовать процесс командной разработки", "навык обобщения и обсуждения результатов",
                "навыки коммуникации", "организация", "умение грамотно доносить свои мысли до команды",
                "решение конфликтных ситуаций", "поддержание коммуникаций в команде"
            };

            var skills = new Skill[skillList.Length];
            for (var i = 0; i < skills.Length; i++) skills[i] = new Skill { Id = i + 1, Name = skillList[i] };
            modelBuilder.Entity<Skill>().HasData(skills);

            modelBuilder.Entity<User>().HasMany(u => u.Reviews).WithOne(r => r.User);


            var orderRoles = new List<OrderRole>
            {
                new OrderRole { Id = 1, Name = "Backend разработчик" },
                new OrderRole { Id = 2, Name = "Frontend разработчик" },
                new OrderRole { Id = 3, Name = "Java Backend разработчик" },
                new OrderRole { Id = 4, Name = "C# Backend разработчик" },
                new OrderRole { Id = 5, Name = "Аналитик" },
                new OrderRole { Id = 6, Name = "UI/UX дизайнер" },
            };

            modelBuilder.Entity<OrderRole>().HasData(orderRoles);


            //modelBuilder.Entity<User>().HasData(GetCustomers());
        }
    }
}