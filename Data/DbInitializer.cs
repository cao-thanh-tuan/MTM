using MTM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTM.Data
{
    public class DbInitializer
    {
        public static void Initialize(MTMContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.Add(
                    new User { 
                        Username = "admin", 
                        Password = "AQAAAAEAACcQAAAAEKc4o3h+VVDeoSd4HaCZrklOQRjwNHSuSK79Vbn27nTTIAKC2UAdmRA5J74SHibrVg==" 
                    });
                context.SaveChanges();
            }

            if (!context.Disciples.Any())
            {
                var deciples = new Disciple[]
                {
                new Disciple{IdentitcationNumber="023789312",FirstName="Tèo",MiddleName="Văn",LastName="Nguyễn",
                    Address="1A Thống Nhất, Gò Vấp",Gender=Gender.MALE,DateOfBirth=DateTime.Parse("1988-01-01"),InitiateDate=DateTime.Parse("2008-10-05")},
                new Disciple{IdentitcationNumber="023869614",FirstName="Anh",MiddleName="Thị",LastName="Trần",
                    Address="66/5 Nguyễn Văn Lượng, Go Vap",Gender=Gender.FEMALE,DateOfBirth=DateTime.Parse("1983-04-10"),InitiateDate=DateTime.Parse("2011-11-12")},
                new Disciple{IdentitcationNumber="363827944",FirstName="Hạnh",MiddleName="Hồng",LastName="Truong",
                    Address="66/5 Nguyễn Văn Lượnng, Go Vap",Gender=Gender.UNKNOWN,DateOfBirth=DateTime.Parse("1985-06-09"),InitiateDate=DateTime.Parse("2009-08-04")}
                };
                foreach (Disciple d in deciples)
                {
                    context.Disciples.Add(d);
                }
                context.SaveChanges();

                foreach (Disciple d in deciples)
                {
                    var registers = new Registration[]
                    {
                    new Registration{Disciple=d,FromTime=DateTime.Parse("2020-04-01 15:30:00"),ToTime=DateTime.Parse("2020-04-01 19:30:00")},
                    new Registration{Disciple=d,FromTime=DateTime.Now,ToTime=DateTime.Now.AddHours(4)},
                    };

                    foreach (Registration m in registers)
                    {
                        context.Registrations.Add(m);
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
