using eBroker.Data.Entities;
using eBroker.Data.Interfaces;
using eBroker.Data.Repositories;
using eBroker.Services.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace eBroker.Services.Tests
{
    public class TraderServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<Trader>> _mockTraderRepository;
        private readonly Mock<IRepository<Equity>> _mockEquityRepository;
        private readonly Mock<IRepository<OwnedEquity>> _mockOwnedEquityRepository;
        public TraderServiceTest()
        {
            Mock<IRepository<Trader>> mockTraderRepo = new Mock<IRepository<Trader>>();

            mockTraderRepo.Setup(r => r.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Trader
                {
                    Id = 1,
                    Name = "ABC Test",
                    FundValue = 1000
                }));

            _mockTraderRepository = mockTraderRepo;

            Mock<IRepository<Equity>> mockEquityRepo = new Mock<IRepository<Equity>>();

            mockEquityRepo.Setup(r => r.GetAsync(It.IsAny<int>()))
               .Returns(Task.FromResult(new Equity { Id = 1, Name = "Axis Midcap", NAV = 76.68f, }));

            _mockEquityRepository = mockEquityRepo;

            Mock<IRepository<OwnedEquity>> mockOwnedEquityRepo = new Mock<IRepository<OwnedEquity>>();

            List<OwnedEquity> ownedEquities = new List<OwnedEquity>()
            {
                new OwnedEquity { EquityId = 1, TraderId = 1, Units = 10, Value = 766.88m }
            };

            IQueryable<OwnedEquity> queryableOwnedEquities = ownedEquities.AsQueryable();

            mockOwnedEquityRepo.Setup(r => r.Find(It.IsAny<Expression<Func<OwnedEquity, bool>>>()))
             .Returns(queryableOwnedEquities);


            _mockOwnedEquityRepository = mockOwnedEquityRepo;
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            //_mockUnitOfWork.Setup(u => u.BeginTransaction())
            //    .Returns();

           
        }

        [Fact]
        public async void ShouldFundAdded_True()
        {

            _mockTraderRepository.Setup(r => r.UpdateAsync(It.IsAny<Trader>()))
                .Returns(Task.FromResult(new Trader
                {
                    Id = 1,
                    Name = "ABC Test",
                    FundValue = 2000
                }));

            var traderService = new TraderService(_mockUnitOfWork.Object, _mockTraderRepository.Object, _mockEquityRepository.Object, _mockOwnedEquityRepository.Object);
            
            var result = await traderService.AddFundAsync(1000);

            Assert.True(result);
        }

        [Fact]
        public async void ShouldBuyEquity_True()
        {
            var traderService = new TraderService(_mockUnitOfWork.Object, _mockTraderRepository.Object, _mockEquityRepository.Object, _mockOwnedEquityRepository.Object);

            var date = DateTime.ParseExact("30/12/2021 10:00 AM", "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            var result = await traderService.BuyEquityAsync(new EquityModel
            {
                EquityId = 1,
                FundValue = 1000,
                PurchaseDateTime = date
            });

            Assert.False(result);
        }

        [Fact]
        public async void ShouldBuyEquity_False()
        {
            var traderService = new TraderService(_mockUnitOfWork.Object, _mockTraderRepository.Object, _mockEquityRepository.Object, _mockOwnedEquityRepository.Object);

            var date = DateTime.ParseExact("30/12/2021 10:00 AM", "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            
            var result = await traderService.BuyEquityAsync(new EquityModel
            {
                EquityId = 1,
                FundValue = 500,
                PurchaseDateTime = date
            }) ;

            Assert.False(result);
        }

    }
}
