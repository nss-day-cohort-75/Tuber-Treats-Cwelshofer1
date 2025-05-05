using Microsoft.Net.Http.Headers;

namespace TuberTreats.Models;

public class  TuberDriver {
    public int Id {get; set;}

    public string Name {get; set;}

    public List<TuberOrder> TuberDeliveries {get; set;}
}