using Castle.Core.Logging;
using LoginAndRegister.Controllers;
using LoginAndRegister.Dto;
using LoginAndRegister.Modals;
using LoginAndRegister.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTests.cs
{
    [TestFixture]
    public class UsersControllerNUnitTests
    {
        LoginRequestDto loginRequestDto;    
        LoginResponseDto loginResponseDto;
        LocalUser user;
        RegistrationRequestDto registrationRequestDto;  
        [SetUp]
        public void Setup()
        {
            loginRequestDto = new LoginRequestDto()
            {
                UserName = "kamil.monu@gmail.com",
                Password = "Kapil161"
            };
            user = new LocalUser()
            {
                Id = 1,
                UserName="kamil.monu@gmail.com",
                Name = "Kapil dadheech",
                Password = "Kapil161@",
                Role = "Admin"
            };
            loginResponseDto = new LoginResponseDto()
            {
                user = user,
                Token = "uhdciuwe12345ndjksnwdfjkcks4455"
            };
            registrationRequestDto = new RegistrationRequestDto()
            {
                UserName = "kamil.monu@gmail.com",
                Name = "Kapil dadheech",
                Password = "Kapil161@",
                Role = "Admin"
            };
        }
        [Test]
        public async Task InputLoginCredentials_OutputJWTToken()
        {
            var logMock = new Mock<ILogger<UsersController>>();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(u => u.Login(loginRequestDto)).ReturnsAsync(loginResponseDto);
            UsersController obj = new UsersController(userRepositoryMock.Object, logMock.Object);
            var response = await obj.Login(loginRequestDto);
            OkObjectResult result = response as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }
        [Test]
        public async Task Register_InputRegistrationRequestDto_OutputUser()
        {
            var logMock = new Mock<ILogger<UsersController>>();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(u => u.IsUniqueUser(It.IsAny<string>())).Returns(true);
            userRepositoryMock.Setup(u => u.Register(registrationRequestDto)).ReturnsAsync(user);
            UsersController obj = new UsersController(userRepositoryMock.Object, logMock.Object);
            var response = await obj.Register(registrationRequestDto);
            OkObjectResult result = response as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
