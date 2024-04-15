using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Data;
public static class FakeDataFactory
{
    public static IEnumerable<Role> Roles
        => new List<Role>()
        {
                new()
                {
                    Id = 1,
                    Name = "Psychologist"
                },
                new()
                {
                    Id = 2,
                    Name = "Сurator"
                },
                new()
                {
                    Id = 3,
                    Name = "Admin"
                }
        };

    public static IEnumerable<Approach> Approaches
        => new List<Approach>()
        {
                new()
                {
                    Id = 1,
                    Name = "Psychoanalysis"
                },
                new()
                {
                    Id = 2,
                    Name = "hyponosis"
                }
        };

    public static IEnumerable<Status> Statuses
        => new List<Status>()
        {
                new()
                {
                    Id = 1,
                    Name = "New"
                },
                new()
                {
                    Id = 2,
                    Name = "InProcessing"
                },
                new()
                {
                    Id = 3,
                    Name = "Completed"
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
                UserId = 1
            }
    };

    public static IEnumerable<Questionnaire> Questionnaires
        => new List<Questionnaire>()
        {
            new Questionnaire()
            {
                Id = 1,
                Name = "Questionnaire 1",
                Pronouns = "He/Him",
                Age = 25,
                TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
                ContactId = 1,
                NeuroDifferences = "No",
                MentalSpecifics = "No",
                PsyWishes = "No",
                PsyRequest = "I want my therapist to be a woman",
                TherapyExperience = "7 month",
                IsForPay = false,
                RegistrationDate = DateTime.UtcNow
            },
            new Questionnaire()
            {
                Id = 2,
                Name = "Questionnaire 2",
                Pronouns = "She/Her",
                Age = 30,
                TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
                ContactId = 2,
                NeuroDifferences = "No",
                MentalSpecifics = "No",
                PsyWishes = "No",
                PsyRequest = "No",
                TherapyExperience = "One year",
                IsForPay = true,
                RegistrationDate = DateTime.UtcNow.AddDays(-1)
            },
            new Questionnaire()
            {
                Id = 3,
                Name = "Questionnaire 3",
                Pronouns = "They/Them",
                Age = 35,
                TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
                ContactId = 3,
                NeuroDifferences = "No",
                MentalSpecifics = "No",
                PsyWishes = "No",
                PsyRequest = "No",
                TherapyExperience = "No",
                IsForPay = false,
                RegistrationDate = DateTime.UtcNow.AddDays(-10)
            }
        };
}
