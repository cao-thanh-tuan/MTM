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
                context.Users.Add(
                    new User
                    {
                        Username = "admin2",
                        Password = "AQAAAAEAACcQAAAAEKc4o3h+VVDeoSd4HaCZrklOQRjwNHSuSK79Vbn27nTTIAKC2UAdmRA5J74SHibrVg=="
                    });
                context.SaveChanges();
            }

            if (!context.Classes.Any()) {
                context.Classes.Add(new Class{Name="Lớp 1", City="TP HCM"});
                context.Classes.Add(new Class { Name = "Lớp 2" });
                context.Classes.Add(new Class { Name = "Lớp 3", City = "Hà Nội" });
                context.SaveChanges();

                var classes = context.Classes.ToArray();
                var deciples = new Disciple[]
                {
                    new Disciple{Phone="0981234567",FirstName="Tèo",MiddleName="Văn",LastName="Nguyễn",Class = classes[0],
                        Address="1A Thống Nhất, Gò Vấp",Gender=Gender.MALE,DateOfBirth=DateTime.Parse("1981-01-01"),InitiateDate=DateTime.Parse("2001-01-01")},
                    new Disciple{Phone="0901234567",FirstName="Anh",MiddleName="Thị",LastName="Trần",Class = classes[0],
                        Address="66/5 Nguyễn Văn Lượng, Gò Vấp",Gender=Gender.FEMALE,DateOfBirth=DateTime.Parse("1983-04-10"),InitiateDate=DateTime.Parse("2011-11-12")},
                    new Disciple{Phone="0911234567",FirstName="Hạnh",MiddleName="Hồng",LastName="Truong",Class = classes[1],
                        Address="61/10 Nguyễn Văn Trỗi, Quận 5",Gender=Gender.UNKNOWN,DateOfBirth=DateTime.Parse("1985-06-09"),InitiateDate=DateTime.Parse("2009-08-04")},
                    new Disciple{Phone="0921234567",FirstName="Bình",MiddleName="Gia",LastName="Trần",Class = classes[1],
                        Address="15 Hồng Hà, Quận Tân Bình",Gender=Gender.MALE,DateOfBirth=DateTime.Parse("1986-03-10"),InitiateDate=DateTime.Parse("2010-10-10")},
                    new Disciple{Phone="0931234567",FirstName="Hân",MiddleName="Gia",LastName="Truong",Class = classes[2],
                        Address="25 Nguyễn Huệ, Quận 1",Gender=Gender.FEMALE,DateOfBirth=DateTime.Parse("1981-10-11"),InitiateDate=DateTime.Parse("2011-10-11")},
                    new Disciple{Phone="0941234567",FirstName="Sơn",MiddleName="Thái",LastName="Lâm",Class = classes[2],
                        Address="75 Nguyễn Văn Trỗi, Quận 3",Gender=Gender.MALE,DateOfBirth=DateTime.Parse("1978-02-10"),InitiateDate=DateTime.Parse("2012-05-13")}
                };
                foreach (Disciple d in deciples)
                {
                    context.Disciples.Add(d);
                }
                context.SaveChanges();

                var lastWeek = DateTime.Now.AddDays(-7);
                var thisWeek = DateTime.Now;
                var nextWeek = DateTime.Now.AddDays(7);
                foreach (Disciple d in deciples)
                {
                    var registers = new Registration[]
                    {
                        new Registration{Disciple=d,
                            FromTime=new DateTime(lastWeek.Year, lastWeek.Month, lastWeek.Day, 10, 30, 00),
                            ToTime=new DateTime(lastWeek.Year, lastWeek.Month, lastWeek.Day, 14, 00, 00)},
                        new Registration{Disciple=d,
                            FromTime=new DateTime(thisWeek.Year, thisWeek.Month, thisWeek.Day, 15, 00, 00),
                            ToTime=new DateTime(thisWeek.Year, thisWeek.Month, thisWeek.Day, 18, 00, 00)},
                        new Registration{Disciple=d,
                            FromTime=new DateTime(nextWeek.Year, nextWeek.Month, nextWeek.Day, 18, 00, 00),
                            ToTime=new DateTime(nextWeek.Year, nextWeek.Month, nextWeek.Day, 22, 00, 00)},
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
