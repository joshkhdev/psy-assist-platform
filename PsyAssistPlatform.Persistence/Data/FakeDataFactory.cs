using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Data;

public static class FakeDataFactory
{
    public static IEnumerable<Role> Roles => new List<Role>()
    {
        new()
        {
            Name = "Admin"
        },
        new()
        {
            Name = "Curator"
        },
        new()
        {
            Name = "Psychologist"
        }
    };

    public static IEnumerable<Approach> Approaches => new List<Approach>()
    {
        new()
        {
            Name = "Когнитивно-поведенческая терапия (КПТ)"
        },
        new()
        {
            Name = "Системная семейная терапия"
        },
        new()
        {
            Name = "Терапия принятия и ответственности (ACT)"
        },
        new()
        {
            Name = "Диалектическая поведенческая терапия (DBT)"
        },
        new()
        {
            Name = "Терапия сфокусированная на сострадании (CFT)"
        },
        new()
        {
            Name = "Ориентированная на решение психотерапия (ОРКТ)"
        },
        new()
        {
            Name = "Интегративная психотерапия"
        },
        new()
        {
            Name = "Экзистенциально-гуманистическая психотерапия"
        },
        new()
        {
            Name = "Mindfulness терапия"
        },
        new()
        {
            Name = "Сказкотерапия"
        },
        new()
        {
            Name = "Арт-терапия"
        },
        new()
        {
            Name = "Проективные методы"
        },        
    };

    public static IEnumerable<Status> Statuses => new List<Status>()
    {
        new()
        {
            Name = "New"
        },
        new()
        {
            Name = "InProcessing"
        },
        new()
        {
            Name = "Completed"
        }
    };

    public static IEnumerable<User> Users => new List<User>()
    {
        new()
        {
            Name = "Василиса Петрова",
            Email = "vasilisa@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 2
        },
        new()
        {
            Name = "Вера Соловьева",
            Email = "vera@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 3
        },
        new()
        {
            Name = "Яна Серебрякова",
            Email = "yana@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 3
        },
        new()
        {
            Name = "Иван Орлов",
            Email = "ivan@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 3
        },
        new()
        {
            Name = "Арина Белова",
            Email = "arina@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 3
        },
        new()
        {
            Name = "Дарья Миронова",
            Email = "daria@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 3
        },
        new()
        {
            Name = "Роберт Сахаров",
            Email = "robert@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 3
        },
        new()
        {
            Name = "Жанна Ефимова",
            Email = "janna@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 3
        },
        new()
        {
            Name = "Яков Иванов",
            Email = "jakob@goooooooogle.org",
            IsBlocked = false,
            Password = "qwerty",
            RoleId = 1
        },
    };

    public static IEnumerable<PsychologistProfile> PsychologistProfiles => new List<PsychologistProfile>()
    {
        new()
        {
            Name = "Василиса",
            Description = "TODO",
            IncludingQueries = "СДВГ и аутизм/РАС (прокрастинация, мотивация, предотвращение перегрузок, отстаивание личных границ и т.д.);РПП, БР, ПРЛ;тревога, раздражительность;чувство виды и стыда;панические атаки, страхи;апатия, выгорание, потеря интереса к жизни;самоценность и др",
            ExcludingQueries = "критические состояния, психозы;ПТСР;зависимости",
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 1,
            IsActive = true,
        },
        new()
        {
            Name = "Вера",
            Description = "TODO",
            IncludingQueries = "тревожные и депрессивные состояния;потеря жизненных смыслов;вопросы самопринятия,проблемы с самооценкой;отношения с другими и собой;вопросы идентичности;эмиграция (поддержка в адаптации) и др",
            ExcludingQueries = "ПРЛ;РПП;зависимости в качестве основного запроса",
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 2,
            IsActive = true,
        },
        new()
        {
            Name = "Яна",
            Description = "TODO",
            IncludingQueries = "РАС, СДВГ;депрессия;тревога;аутистическое выгорание;синдром самозванца;вопросы беременности и материнства;поддержка родителей с РАС и СДВГ",
            ExcludingQueries = "ПТСР;РПП;зависимости;острое горе;насилие",
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 3,
            IsActive = true,
        },
        new()
        {
            Name = "Иван",
            Description = "TODO",
            IncludingQueries = "тревоги, страхи;выгорание, подавленность, апатия;чувство бессмысленности, потерянности в жизни;трудности в адаптации;трудности в выстраивании отношений с другими;трудности с пониманием своих желаний и др",
            ExcludingQueries = "люди младше 18 лет;психиатрические диагнозы в качестве основного запроса",
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 4,
            IsActive = true,
        },
        new()
        {
            Name = "Арина",
            Description = "TODO",
            IncludingQueries = "проблемы с самооценкой;трудности в сфере эмоций;конфликты с близкими людьми;личностные/возрастные кризисы;страхи перемен;тревожные/депрессивные состояния и др",
            ExcludingQueries = "люди младше 18 лет;диагнозы, требующие стационарного лечения и постоянного наблюдения врача",
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 5,
            IsActive = true,
        },
        new()
        {
            Name = "Дарья",
            Description = "TODO",
            IncludingQueries = "сложности с принятием себя, самокритика, перфекционизм;прокрастинация, трудности с планированием;тревожность;недовольство своим телом;апатия, подавленность, выгорание;сложности в отношениях с окружающими и др",
            ExcludingQueries = "зависимости;острое горе;ПТСР как основной запрос;выраженная депрессия;суицидальный риск",
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 6,
            IsActive = true,
        },
        new()
        {
            Name = "Роберт",
            Description = "TODO",
            IncludingQueries = "жизненные кризисы, включая проживание утраты, ПТСР и кПТСР;проблемы идентичности, диссоциации, маскинга;зависимости (РПП, алкогольная зависимость, селфхарм);самореализация, профориентация и лайф-коучинг и др",
            ExcludingQueries = "люди в остром состоянии, которые нуждаются в параллельном лечении у психиатра и при этом его не проходят",
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 7,
            IsActive = true,
        },
        new()
        {
            Name = "Жанна",
            Description = "TODO",
            IncludingQueries = "TODO",
            ExcludingQueries = "TODO",
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 8,
            IsActive = false,
        },
    };

    public static IEnumerable<Contact> Contacts => new List<Contact>()
    {
        new()
        {
            Telegram = "@johndoe_fake",
            Email = "john.doe@fakemail.com",
            Phone = "+79991234567"
        },
        new()
        {
            Telegram = "@janedoe_fake",
            Email = "jane.doe@fakemail.com",
            Phone = "+79991234568"
        },
        new()
        {
            Telegram = "@unknowndoe_fake",
            Email = "unknown.doe@fakemail.com",
            Phone = "+79991234569"
        }
    };

    public static IEnumerable<Questionnaire> Questionnaires => new List<Questionnaire>()
    {
        new()
        {
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