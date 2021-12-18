using eBroker.Data.Entities;
using eBroker.Data.Interfaces;
using eBroker.Data.Repositories;
using eBroker.Services.Interfaces;
using eBroker.Services.Models;

namespace eBroker.Services
{
    public class TraderService : ITraderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Trader> _traderRepository;
        private readonly IRepository<Equity> _equityRepository;
        private readonly IRepository<OwnedEquity> _ownedEquityRepository;

        public TraderService(IUnitOfWork unitOfWork,
            IRepository<Trader> traderRepository,
            IRepository<Equity> equityRepository,
            IRepository<OwnedEquity> ownedEquityRepository
            )
        {
            _unitOfWork = unitOfWork;
            _traderRepository = traderRepository;
            _equityRepository = equityRepository;
            _ownedEquityRepository = ownedEquityRepository;
        }
        public async Task<bool> AddFundAsync(decimal fundValue)
        {
            if (fundValue > 100000)
            {
                decimal charges = fundValue * Constants.BROKERAGE_FEE;
                fundValue = fundValue - charges;
            }

            try
            {
                var trader = await _traderRepository.GetAsync(Constants.TRADER_ID);

                trader.FundValue = fundValue;

                await _traderRepository.UpdateAsync(trader);

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> BuyEquityAsync(EquityModel equityModel)
        {
            // validate time and week of the day
            if (!(Utility.IsValidDuration(equityModel.PurchaseDateTime)
                && Utility.IsValidDayOfWeek(equityModel.PurchaseDateTime)))
            {
                return false;
            }


            var trader = await _traderRepository.GetAsync(Constants.TRADER_ID);

            // validate fund value
            if (trader.FundValue < equityModel.FundValue)
                return false;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var equity = await _equityRepository.GetAsync(equityModel.EquityId);
                    if (equity == null)
                        throw new Exception($"{nameof(equity)} not found");


                    var units = (int)(equityModel.FundValue / (decimal)equity.NAV);

                    var ownedEquity = _ownedEquityRepository.Find(e => e.TraderId == Constants.TRADER_ID &&
                                                e.EquityId == equity.Id).FirstOrDefault();

                    if (ownedEquity != null)
                    {
                        ownedEquity.Units += units;
                        ownedEquity.Value += equityModel.FundValue;

                        await _ownedEquityRepository.UpdateAsync(ownedEquity);
                    }
                    else
                    {
                        await _ownedEquityRepository.AddAsync(new OwnedEquity
                        {
                            TraderId = Constants.TRADER_ID,
                            EquityId = equity.Id,
                            Value = equityModel.FundValue,
                            Units = units
                        });
                    }

                    trader.FundValue = trader.FundValue - equityModel.FundValue;

                    await _traderRepository.UpdateAsync(trader);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> SellEquityAsync(EquityModel equityModel)
        {
            if (!(Utility.IsValidDuration(equityModel.PurchaseDateTime)
                && Utility.IsValidDayOfWeek(equityModel.PurchaseDateTime)))
            {
                return false;
            }

            var trader = await _traderRepository.GetAsync(Constants.TRADER_ID);

            var equity = await _equityRepository.GetAsync(equityModel.EquityId);
            if (equity == null)
                throw new Exception($"{nameof(equity)} not found");

            var ownedEquity = _ownedEquityRepository.Find(e => e.TraderId == Constants.TRADER_ID &&
                                        e.EquityId == equityModel.EquityId).FirstOrDefault();

            if (ownedEquity == null)
                throw new Exception($"don't own equity with Equity Id: {equityModel.EquityId} ");


            var charges = equityModel.FundValue * Constants.BROKERAGE_FEE;
            charges = charges < 20 ? 20 : charges; // min charges 20

            var withdraw = equityModel.FundValue + charges;

            if (ownedEquity.Value < withdraw)
            {
                throw new Exception($"In sufficient balance can withdraw only {(ownedEquity.Value - charges)} ");
            }

            try
            {
                var units = (int)(equityModel.FundValue / (decimal)equity.NAV);

                ownedEquity.Value = ownedEquity.Value - withdraw;
                ownedEquity.Units = ownedEquity.Units - units;

                await _ownedEquityRepository.UpdateAsync(ownedEquity);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
