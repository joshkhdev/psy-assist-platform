using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence
{
    public static class FakeDataFactory
    {
        public static IEnumerable<Role> Roles
            => new List<Role>()
            {
                new()
                {
                    Id = 1,
                    Name = "Psychologist"
                }
            };

        public static IEnumerable<User> Users
            => new List<User>()
            {
                new()
                {
                    Id = 1,
                    Name = "Ivan Ivanov",
                    Email = "ivanov@goooooooogle.org",
                    IsBlocked = false,
                    Password = "qwerty",
                    Role = Roles.First(),
                    RoleId = 1
                }
            };

        public static IEnumerable<Psychologist> Psychologists
            => new List<Psychologist>()
        {
            new()
            {
                Id = 1,
                Description = "No Description",
                IsActive = true,
                Name = "Ivan Ivanov",
                RequestsInclude = "List 1",
                RequestsExclude = "List 2",
                TimeZone =  TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
                User = Users.First(),
                UserId = 1
            }
        };
    }
}
