using Microsoft.AspNetCore.SignalR;

namespace TuberTreats.Models.ModelsDTO;

public class TuberOrderDTO {
    public int Id {get; set;}

    public DateTime OrderPlacedOnDate {get; set;}

    public int CustomerId {get; set;}

    public int TuberDriverId {get; set;}

    public DateTime DeliveredOnDate {get; set;}

    public List<ToppingDTO> Topping { get; set; }

    public List<TuberDriverDTO> TuberDriver {get; set;}

    public List<CustomerDTO> Customer {get; set;}

}