using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{

    //This is Extension Method class and thats why its 'static' class. It can also be called as Utility/helper class.
    public static class StockMappers
    {

        // These methods are Extension methods.
        /*
        
            Extension methods allow you to add new methods to existing types without modifying 
            the original class, inheriting from it, or using inheritance. 
            They are especially useful when you want to extend classes from third-party libraries
            or built-in .NET types (like string, int, etc.).

            Because of 'this' as 1st argument its called extension method and what ever the 2nd argument is,
            this method becomes extension method of that Argument (Class)

            Example:
            In this method, we are pass Stock(Class) model, as 2nd Argument, so this method becomes Extension Method
            of Stock Model. 

            In Controller, we can access ToStockDto using Stock(class) instance. Like. Stocks sc; sc.stockModel();
        */ 
        public static StockDto ToStockDto(this Stock stockModel)
        {

            // converting Stock Model -> StockDTO
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Industry = stockModel.Industry,
                LastDiv = stockModel.LastDiv,
                MarketCap = stockModel.MarketCap,
                Purchase = stockModel.Purchase
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            // Converting StockDTO to Stock Model
            return new Stock
            {
                Symbol = stockDto.Symbol,   
                CompanyName = stockDto.CompanyName,
                Industry = stockDto.Industry,
                LastDiv = stockDto.LastDiv,
                MarketCap = stockDto.MarketCap,
                Purchase = stockDto.Purchase
            };
        }

        public static Stock ToStockFromUpdateDto(this UpdateStockRequestDto stockDto)
        {
            return new Stock
            { 
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Industry = stockDto.Industry,
                LastDiv = stockDto.LastDiv,
                MarketCap = stockDto.MarketCap,
                Purchase = stockDto.Purchase
            };

        }
    }
}
