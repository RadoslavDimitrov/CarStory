using CarStory.Data;
using CarStory.Models.DTO.Car;

namespace CarStory.Services.Search
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext data;

        public SearchService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public SearchCarDTO SearchCar(string vinNumber)
        {
            var car = this.data.Cars.Where(c => c.VinNumber == vinNumber).FirstOrDefault();

            if(car == null) 
            {
                return null;
            }

            var carDto = new SearchCarDTO
            {
                Id = car.Id,
                VinNumber = vinNumber,
                Make = car.Make,
                Model = car.Model,
            };

            return carDto;
        }
    }
}
