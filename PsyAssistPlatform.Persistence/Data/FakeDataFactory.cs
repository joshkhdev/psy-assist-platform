using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Data;

public static class FakeDataFactory
{
    public static IEnumerable<Role> Roles => new List<Role>()
    {
        new()
        {
            Id = 1,
            Name = "Admin"
        },
        new()
        {
            Id = 2,
            Name = "Curator"
        },
        new()
        {
            Id = 3,
            Name = "Psychologist"
        }
    };

    public static IEnumerable<Approach> Approaches => new List<Approach>()
    {
        new()
        {
            Id = 1,
            Name = "Psychoanalysis"
        },
        new()
        {
            Id = 2,
            Name = "Hypnosis"
        }
    };

    public static IEnumerable<Status> Statuses => new List<Status>()
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

    public static IEnumerable<User> Users => new List<User>()
    {
        new()
        {
            Id = 1,
            Name = "Ivan Ivanov",
            Email = "ivanov@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 3
        },
        new()
        {
            Id = 2,
            Name = "Anna Petrova",
            Email = "petrova@goooooooogle.org",
            IsBlocked = false,
            Password = "ytrewq",
            RoleId = 1
        }
    };

    public static IEnumerable<PsychologistProfile> Psychologists => new List<PsychologistProfile>()
    {
        new()
        {
            Id = 1,
            Description = "No Description",
            IsActive = true,
            Name = "Ivan Ivanov",
            IncludingQueries = "List 1",
            ExcludingQueries = "List 2",
            TimeZone =  TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 1
        }
    };

    public static IEnumerable<Contact> Contacts => new List<Contact>()
    {
        new()
        {
            Id = 1,
            Telegram = "@johndoe_fake",
            Email = "john.doe@fakemail.com",
            Phone = "+79991234567"
        },
        new()
        {
            Id = 2,
            Telegram = "@janedoe_fake",
            Email = "jane.doe@fakemail.com",
            Phone = "+79991234568"
        },
        new()
        {
            Id = 3,
            Telegram = "@unknowndoe_fake",
            Email = "unknown.doe@fakemail.com",
            Phone = "+79991234569"
        }
    };

    public static IEnumerable<Questionnaire> Questionnaires => new List<Questionnaire>()
    {
        new()
        {
            Id = 1,
            Name = "John Doe",
            Pronouns = "He/Him",
            Age = 25,
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            ContactId = 1,
            NeuroDifferences = "No",
            MentalSpecifics = "No",
            PsyWishes = "No",
            PsyQuery = "I want my therapist to be a woman",
            TherapyExperience = "7 months",
            IsForPay = false,
            RegistrationDate = DateTime.UtcNow
        },
        new()
        {
            Id = 2,
            Name = "Jane Doe",
            Pronouns = "She/Her",
            Age = 30,
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            ContactId = 2,
            NeuroDifferences = "No",
            MentalSpecifics = "No",
            PsyWishes = "No",
            PsyQuery = "No",
            TherapyExperience = "One year",
            IsForPay = true,
            RegistrationDate = DateTime.UtcNow.AddDays(-1)
        },
        new()
        {
            Id = 3,
            Name = "Unknown Doe",
            Pronouns = "They/Them",
            Age = 35,
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            ContactId = 3,
            NeuroDifferences = "No",
            MentalSpecifics = "No",
            PsyWishes = "No",
            PsyQuery = "No",
            TherapyExperience = "No",
            IsForPay = false,
            RegistrationDate = DateTime.UtcNow.AddDays(-10)
        }
    };
}
