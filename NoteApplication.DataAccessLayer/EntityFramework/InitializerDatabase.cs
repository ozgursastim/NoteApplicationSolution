using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteApplication.Entities;
using System.Data.Entity;

namespace NoteApplication.DataAccessLayer.EntityFramework
{
    public class InitializerDatabase : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // Adding admin user
            NoteUser admin = new NoteUser()
            {
                Name = "Admin",
                Surname = "User",
                Email = "admin.user@email.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "admin.user",
                Password = "123456",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now.AddHours(1),
                ModifiedUsername = "admin.user"
            };

            // Adding standart user
            NoteUser testuser = new NoteUser()
            {
                Name = "Test",
                Surname = "User",
                Email = "test.user@email.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "test.user",
                Password = "1234567",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now.AddHours(1),
                ModifiedUsername = "admin.user"
            };

            context.NoteUsers.Add(admin);
            context.NoteUsers.Add(testuser);

            //Adding fake user
            for (int i = 0; i < 8; i++)
            {
                NoteUser faketestuser = new NoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "123",
                    CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };
                context.NoteUsers.Add(faketestuser);
            }

            context.SaveChanges();

            List<NoteUser> noteUsers = context.NoteUsers.ToList();

            // Adding fake categories
            for (int i = 0; i < 10; i++)
            {
                Category category = new Category()
                {
                    Title = FakeData.PlaceData.GetCity(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now.AddHours(1),
                    ModifiedUsername = "admin.user",
                };

                context.Categories.Add(category);

                // Adding fake notes
                for (int j = 0; j < FakeData.NumberData.GetNumber(5, 9); j++)
                {
                    NoteUser note_owner = noteUsers[FakeData.NumberData.GetNumber(0, noteUsers.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(10,50),
                        Owner = note_owner,
                        CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = note_owner.Username
                    };
                    category.Notes.Add(note);

                    // Adding fake comments
                    for (int k = 0; k < FakeData.NumberData.GetNumber(3, 5); k++)
                    {
                        NoteUser comment_owner = noteUsers[FakeData.NumberData.GetNumber(0, noteUsers.Count - 1)];

                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username
                        };

                        note.Comments.Add(comment);
                    }

                    // Adding fake likes
                    for (int m = 0; m < FakeData.NumberData.GetNumber(1, 9); m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = noteUsers[m]
                        };
                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
