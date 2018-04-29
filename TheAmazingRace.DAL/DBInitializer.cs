using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class DBInitializer : CreateDatabaseIfNotExists<TheAmazingRaceDbContext>
    {
        // Uncomment this to use local images version
        //List<string> picts = new List<string> {
        //    "/Content/img/avatar.png",
        //    "/Content/img/avatar0.png",
        //    "/Content/img/avatar04.png",
        //    "/Content/img/avatar2.png",
        //    "/Content/img/avatar3.png"
        //};

        // Comment this if the cloud hosted one already unavailable
        List<string> picts = new List<string> {
            "https://amazingrace.blob.core.windows.net/userprofiles/avatar.png",
            "https://amazingrace.blob.core.windows.net/userprofiles/avatar0.png",
            "https://amazingrace.blob.core.windows.net/userprofiles/avatar04.png",
            "https://amazingrace.blob.core.windows.net/userprofiles/avatar2.png",
            "https://amazingrace.blob.core.windows.net/userprofiles/avatar3.png"
        };

        Random rand = new Random(DateTime.Now.ToString().GetHashCode());

        protected override void Seed(TheAmazingRaceDbContext context)
        {
            var roleManager = new RoleManager<Role>(new RoleStore<Role>(context));
            var userManager = new UserManager<User>(new UserStore<User>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User
            if (!roleManager.RoleExists("Administrator"))
            {
                var roles = new List<Role>
                 {
                    new Role{ Name = "Administrator" },
                    new Role{ Name = "Staff" },
                    new Role{ Name = "Participant" }
                 };

                roles.ForEach(r => roleManager.Create(r));
            }

            ////Here we create a Admin super user who will maintain the website
            var admin = new User
            {
                UserName = "admin@u.nus.edu",
                Email = "admin@u.nus.edu",
                FirstName = "Super",
                LastName = "Admin",
                DOB = DateTime.Parse("1985-04-20 00:00:00"),
                Gender = "Male",
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            };

            var result = userManager.Create(admin, "12345678");
            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, "Administrator");
            }

            var participants = CreateParticipant(admin.Id, context);
            var staffs = CreateStaff(admin.Id, context);

            var raceEventId = CreateRace(admin.Id);

            CreatePitStop(admin.Id, raceEventId);

            CreateTeams(admin.Id, raceEventId, participants);
            AddStaffToRace(raceEventId, staffs);

            //base.Seed(context);
        }

        public List<User> CreateParticipant(string createdById, TheAmazingRaceDbContext context)
        {
            var userManager = new UserManager<User>(new UserStore<User>(context));

            List<User> participants = new List<User>();
            participants.Add(new User
            {
                UserName = "aidan@u.nus.edu",
                Email = "aidan@u.nus.edu",
                FirstName = "Aidan",
                LastName = "Magnolia",
                DOB = DateTime.Parse("1986-05-21 00:00:00"),
                Gender = "Male",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            participants.Add(new User
            {
                UserName = "suzan@u.nus.edu",
                Email = "suzan@u.nus.edu",
                FirstName = "Suzan",
                LastName = "Clint",
                DOB = DateTime.Parse("1985-03-12 00:00:00"),
                Gender = "Female",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            participants.Add(new User
            {
                UserName = "abram@u.nus.edu",
                Email = "abram@u.nus.edu",
                FirstName = "Abram",
                LastName = "Kaila",
                DOB = DateTime.Parse("1987-04-28 00:00:00"),
                Gender = "Female",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            participants.Add(new User
            {
                UserName = "hardy@u.nus.edu",
                Email = "hardy@u.nus.edu",
                FirstName = "Hardy",
                LastName = "Connell",
                DOB = DateTime.Parse("1988-10-20 00:00:00"),
                Gender = "Male",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            participants.Add(new User
            {
                UserName = "sukie@u.nus.edu",
                Email = "sukie@u.nus.edu",
                FirstName = "Sukie",
                LastName = "Candace",
                DOB = DateTime.Parse("1988-06-03 00:00:00"),
                Gender = "Female",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            participants.Add(new User
            {
                UserName = "silvia@u.nus.edu",
                Email = "silvia@u.nus.edu",
                FirstName = "Silvia",
                LastName = "Linzi",
                DOB = DateTime.Parse("1989-04-23 00:00:00"),
                Gender = "Female",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            participants.Add(new User
            {
                UserName = "adair@u.nus.edu",
                Email = "adair@u.nus.edu",
                FirstName = "Adair",
                LastName = "Winfred",
                DOB = DateTime.Parse("1990-07-12 00:00:00"),
                Gender = "Female",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            foreach (var p in participants)
            {
                var resultParticipant = userManager.Create(p, "12345678");
                if (resultParticipant.Succeeded)
                {
                    userManager.AddToRole(p.Id, "Participant");
                }
            }

            return participants;
        }

        public List<User> CreateStaff(string createdById, TheAmazingRaceDbContext context)
        {
            var userManager = new UserManager<User>(new UserStore<User>(context));

            List<User> staffs = new List<User>();
            staffs.Add(new User
            {
                UserName = "leo@u.nus.edu",
                Email = "leo@u.nus.edu",
                FirstName = "Leo",
                LastName = "Raeburn",
                DOB = DateTime.Parse("1985-08-17 00:00:00"),
                Gender = "Male",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            staffs.Add(new User
            {
                UserName = "tessa@u.nus.edu",
                Email = "tessa@u.nus.edu",
                FirstName = "Tessa",
                LastName = "Maureen",
                DOB = DateTime.Parse("1991-09-29 00:00:00"),
                Gender = "Female",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            staffs.Add(new User
            {
                UserName = "bambi@u.nus.edu",
                Email = "bambi@u.nus.edu",
                FirstName = "Bambi",
                LastName = "Jillie",
                DOB = DateTime.Parse("1985-11-01 00:00:00"),
                Gender = "Male",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            staffs.Add(new User
            {
                UserName = "austyn@u.nus.edu",
                Email = "austyn@u.nus.edu",
                FirstName = "Austyn",
                LastName = "Isaiah",
                DOB = DateTime.Parse("1984-12-25 00:00:00"),
                Gender = "Male",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            staffs.Add(new User
            {
                UserName = "charlie@u.nus.edu",
                Email = "charlie@u.nus.edu",
                FirstName = "Charlie",
                LastName = "Makenna",
                DOB = DateTime.Parse("1985-02-15 00:00:00"),
                Gender = "Male",
                CreatedById = createdById,
                CreatedOn = DateTime.Now,
                PhotoUrl = picts[rand.Next(0, picts.Count)]
            });

            foreach(var s in staffs)
            {
                var resultStaff = userManager.Create(s, "12345678");
                if (resultStaff.Succeeded)
                {
                    userManager.AddToRole(s.Id, "Staff");
                }
            }

            return staffs;
        }

        public int CreateRace(string createdById)
        {
            RaceEventRepo raceEventRepo = new RaceEventRepo();

            var race = raceEventRepo.Create();
            race.EventName = "NUS ISS Amazing Race";
            race.EventDate = DateTime.Parse("2018-04-30 10:00:00");
            race.Address = "25 Heng Mui Keng Terrace";
            race.PostalCode = "119615";
            race.StartLatitude = 1.29214851229718;
            race.StartLongitude = 103.776550885502;
            race.CreatedById = createdById;
            race.CreatedOn = DateTime.Now;

            raceEventRepo.Add(race);
            raceEventRepo.SaveChanges();

            return race.Id;
        }

        public void CreatePitStop(string createdById, int raceEventId)
        {
            PitStopRepo pitStopRepo = new PitStopRepo();
            RaceEventPitStopRepo raceEventPitStopRepo = new RaceEventPitStopRepo();

            var pitstop1 = pitStopRepo.Create();
            pitstop1.PitStopName = "NUS - Central Library";
            pitstop1.ChallengeDescription = "Giants, Wizards, Elves";
            pitstop1.Address = "12 Kent Ridge Crescent";
            pitstop1.PostalCode = "119275";
            pitstop1.Latitude = 1.29672886913492;
            pitstop1.Longitude = 103.773151754394;
            pitstop1.CreatedById = createdById;
            pitstop1.CreatedOn = DateTime.Now;
            pitStopRepo.Add(pitstop1);

            var pitstop2 = pitStopRepo.Create();
            pitstop2.PitStopName = "Clementi Mall";
            pitstop2.ChallengeDescription = "Laughing Game";
            pitstop2.Address = "3155 Commonwealth Avenue West";
            pitstop2.PostalCode = "129588";
            pitstop2.Latitude = 1.31489619639367;
            pitstop2.Longitude = 103.764423054189;
            pitstop2.CreatedById = createdById;
            pitstop2.CreatedOn = DateTime.Now;
            pitStopRepo.Add(pitstop2);

            var pitstop3 = pitStopRepo.Create();
            pitstop3.PitStopName = "The Star Vista";
            pitstop3.ChallengeDescription = "Sing Song Ping Pong";
            pitstop3.Address = "1 Vista Exchange Green";
            pitstop3.PostalCode = "138617";
            pitstop3.Latitude = 1.30687193008981;
            pitstop3.Longitude = 103.788474398043;
            pitstop3.CreatedById = createdById;
            pitstop3.CreatedOn = DateTime.Now;
            pitStopRepo.Add(pitstop3);

            var pitstop4 = pitStopRepo.Create();
            pitstop4.PitStopName = "IKEA Alexandra";
            pitstop4.ChallengeDescription = "Great Wind Blows";
            pitstop4.Address = "317 Alexandra Road";
            pitstop4.PostalCode = "159965";
            pitstop4.Latitude = 1.28794376443444;
            pitstop4.Longitude = 103.806003412297;
            pitstop4.CreatedById = createdById;
            pitstop4.CreatedOn = DateTime.Now;
            pitStopRepo.Add(pitstop4);

            var pitstop5 = pitStopRepo.Create();
            pitstop5.PitStopName = "Pasir Panjang";
            pitstop5.ChallengeDescription = "Partners in Pen";
            pitstop5.Address = "10 PASIR PANJANG ROAD";
            pitstop5.PostalCode = "117438";
            pitstop5.Latitude = 1.27482749212244;
            pitstop5.Longitude = 103.79906482343;
            pitstop5.CreatedById = createdById;
            pitstop5.CreatedOn = DateTime.Now;
            pitStopRepo.Add(pitstop5);

            pitStopRepo.SaveChanges();

            raceEventPitStopRepo.Add(new RaceEventPitStop{
                RaceEventId = raceEventId,
                PitStopId = pitstop1.Id,
                Order = 1
            });

            raceEventPitStopRepo.Add(new RaceEventPitStop
            {
                RaceEventId = raceEventId,
                PitStopId = pitstop2.Id,
                Order = 2
            });

            raceEventPitStopRepo.Add(new RaceEventPitStop
            {
                RaceEventId = raceEventId,
                PitStopId = pitstop3.Id,
                Order = 3
            });

            raceEventPitStopRepo.Add(new RaceEventPitStop
            {
                RaceEventId = raceEventId,
                PitStopId = pitstop4.Id,
                Order = 4
            });

            raceEventPitStopRepo.Add(new RaceEventPitStop
            {
                RaceEventId = raceEventId,
                PitStopId = pitstop5.Id,
                Order = 5
            });

            raceEventPitStopRepo.SaveChanges();
        }

        public void CreateTeams(string createdById, int raceEventId, List<User> participants)
        {
            TeamRepo teamRepo = new TeamRepo();

            var participantId = 0;

            var team1 = teamRepo.Create();
            team1.Name = "Avengers";
            team1.RaceEventId = raceEventId;
            team1.CreatedById = createdById;
            team1.CreatedOn = DateTime.Now;
            team1.Description = "We can do this all day";
            teamRepo.Add(team1);

            var team2 = teamRepo.Create();
            team2.Name = "Wakanda";
            team2.RaceEventId = raceEventId;
            team2.CreatedById = createdById;
            team2.CreatedOn = DateTime.Now;
            team2.Description = "Wakanda Forever!";
            teamRepo.Add(team2);

            var team3 = teamRepo.Create();
            team3.Name = "Guardians";
            team3.RaceEventId = raceEventId;
            team3.CreatedById = createdById;
            team3.CreatedOn = DateTime.Now;
            team3.Description = "We are Groot!";
            teamRepo.Add(team3);

            teamRepo.SaveChanges();

            participants[participantId++].TeamId = team1.Id;
            participants[participantId++].TeamId = team1.Id;
            participants[participantId++].TeamId = team2.Id;
            participants[participantId++].TeamId = team2.Id;
            participants[participantId++].TeamId = team3.Id;
            participants[participantId++].TeamId = team3.Id;
        }

        public void AddStaffToRace(int raceEventId, List<User> staffs)
        {
            RaceEventUserRepo raceEventUserRepo = new RaceEventUserRepo();
            var staffId = 0;

            raceEventUserRepo.Add(new RaceEventUser
            {
                RaceEventId = raceEventId,
                UserId = staffs[staffId++].Id,
                CurrentLat = 1.30642273791529,
                CurrentLong = 103.771795452114
            });

            raceEventUserRepo.Add(new RaceEventUser
            {
                RaceEventId = raceEventId,
                UserId = staffs[staffId++].Id,
                CurrentLat = 1.31140529320966,
                CurrentLong = 103.778637841909
            });

            raceEventUserRepo.SaveChanges();
        }
    }
}
