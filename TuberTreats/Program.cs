
using TuberTreats.Models;
using TuberTreats.Models.ModelsDTO;

List<TuberDriver> tuberDriver = new List<TuberDriver> {

    new TuberDriver {
        Id = 1,
        Name = "Jerry"
    },

    new TuberDriver {
        Id = 2,
        Name = "Jeff"
    },

    new TuberDriver {
        Id = 3,
        Name = "Jim"
    }
};

List<Customer> customer = new List<Customer> {

    new Customer {
        Id = 1,
        Name = "Larry",
        Address = "123 Fake St"

    },

     new Customer {
        Id = 2,
        Name = "Lamar",
        Address = "123 Real St"

    },

     new Customer {
        Id = 3,
        Name = "Logan",
        Address = "123 Customer St"

    },

     new Customer {
        Id = 4,
        Name = "Luke",
        Address = "123 Fake St"

    },

     new Customer {
        Id = 5,
        Name = "Louis",
        Address = "123 Fake St"

    }
};

List<Topping> topping = new List<Topping> {
    new Topping {
        Id = 1,
        Name = "Pineapple"
    },

    new Topping {
        Id = 2,
        Name ="Oreos"
    },

    new Topping {
        Id = 3,
        Name = "Sprinkles"
    },

    new Topping {
        Id = 4,
        Name = "Cool Whip"

    },

    new Topping {
        Id = 5,
        Name = "Cranberry Sauce"
    }
    
};

List<TuberTopping> tuberTopping = new List<TuberTopping> {

    new TuberTopping{
        Id = 1,
        TuberOrderId = 1,
        ToppingId = 1
    },

     new TuberTopping{
        Id = 2,
        TuberOrderId = 1,
        ToppingId = 2
    },

     new TuberTopping{
        Id = 3,
        TuberOrderId = 2,
        ToppingId = 2
    },

     new TuberTopping{
        Id = 4,
        TuberOrderId = 3,
        ToppingId = 3
    },

      new TuberTopping{
        Id = 5,
        TuberOrderId = 4,
        ToppingId = 3
    },

      new TuberTopping{
        Id = 6,
        TuberOrderId = 5,
        ToppingId = 3
    }


};

List<TuberOrder> tuberOrder = new List<TuberOrder> {

    new TuberOrder {
        Id = 1,
        OrderPlacedOnDate = new DateTime (2025, 4, 4),
        CustomerId = 1,
        TuberDriverId = 1,
        DeliveredOnDate = new DateTime (2025, 4, 4)
    },

    new TuberOrder {
        Id = 2,
        OrderPlacedOnDate = new DateTime (2025, 3, 4),
        CustomerId = 2,
        TuberDriverId = 2,
        DeliveredOnDate = new DateTime (2025, 3, 4)
    },

    new TuberOrder {
        Id = 3,
        OrderPlacedOnDate = new DateTime (2025, 2, 4),
        CustomerId = 3,
        TuberDriverId = 3,
        DeliveredOnDate = new DateTime (2025, 2, 4)    
        },

    new TuberOrder {
        Id = 4,
        OrderPlacedOnDate = new DateTime (2025, 2, 4),
        CustomerId = 3,
        TuberDriverId = 3,
        DeliveredOnDate = new DateTime (2025, 2, 4)    
        },
    
    new TuberOrder {
        Id = 5,
        OrderPlacedOnDate = new DateTime (2025, 2, 4),
        CustomerId = 5,
        TuberDriverId = 3,
        DeliveredOnDate = new DateTime (2025, 2, 4)    
        },

      new TuberOrder {
        Id = 6,
        OrderPlacedOnDate = new DateTime (2025, 2, 4),
        CustomerId = 6,
        TuberDriverId = 3,
        DeliveredOnDate = new DateTime (2025, 2, 4)    
        } 
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = 
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();




app.MapGet("/tuberorders", () => 
{
    
return tuberOrder.Select(o => new TuberOrderDTO {
    Id = o.Id,
    OrderPlacedOnDate = o.OrderPlacedOnDate,
    CustomerId = o.CustomerId,
    TuberDriverId = o.TuberDriverId,
    DeliveredOnDate = o.DeliveredOnDate,
    Topping = tuberTopping
        .Where(tt => tt.TuberOrderId == o.Id)
        .Select(tt => topping.FirstOrDefault(t => t.Id == tt.ToppingId))
        .Select(t => new ToppingDTO
        {
            Id = t.Id,
            Name = t.Name
        })
        .ToList()
});
});

app.MapGet("/tuberorders/{id}", (int id) => 
{
    TuberOrder tuberorders = tuberOrder.FirstOrDefault(tube => tube.Id == id);
    Topping toppings = topping.FirstOrDefault(t => t.Id == tuberorders.Id);
    TuberDriver tuberDrivers = tuberDriver.FirstOrDefault(td => td.Id == tuberorders.TuberDriverId);
    Customer  customers = customer.FirstOrDefault(c => c.Id == tuberorders.CustomerId);

   

    return new TuberOrderDTO 
    {
        Id = tuberorders.Id,
        OrderPlacedOnDate = tuberorders.OrderPlacedOnDate,
        CustomerId = tuberorders.CustomerId,
        TuberDriverId = tuberorders.TuberDriverId,
        DeliveredOnDate = tuberorders.DeliveredOnDate,
        Topping = tuberTopping
        .Where(tt => tt.TuberOrderId == id)
        .Select(tt => topping.FirstOrDefault(t => t.Id == tt.ToppingId))
        .Select(t => new ToppingDTO
        {
            Id = t.Id,
            Name = t.Name
        })
        .ToList(),

        TuberDriver = tuberDrivers == null ? null : new List<TuberDriverDTO> 
            {

        new TuberDriverDTO{  
        Id = tuberDrivers.Id,
        Name = tuberDrivers.Name,
        TuberDeliveries = tuberOrder.Where(to => to.TuberDriverId == tuberDrivers.Id)
        .Select(to => new TuberOrderDTO 
            {
                Id = to.Id,
                OrderPlacedOnDate = to.OrderPlacedOnDate,
                CustomerId = to.CustomerId,
                TuberDriverId = to.TuberDriverId,
                DeliveredOnDate = to.DeliveredOnDate,
                Topping = null,
                TuberDriver = null,
                Customer = null

            }).ToList()
        }},
        Customer = customers == null ? null : new List<CustomerDTO> 
        {

        new CustomerDTO {
            Id = customers.Id,
            Name = customers.Name,
            Address = customers.Address,
            TuberOrders = tuberOrder.Where(tos => tos.CustomerId == customers.Id)
            .Select(tos => new TuberOrderDTO
            {
                Id = tos.Id,
                OrderPlacedOnDate = tos.OrderPlacedOnDate,
                CustomerId = tos.CustomerId,
                TuberDriverId = tos.TuberDriverId,
                DeliveredOnDate = tos.DeliveredOnDate,
                Topping = null,
                TuberDriver = null,
                Customer = null,
            }).ToList()
        }}
        };
    });

app.MapPost("/tuberorders" , (TuberOrder tuberOrders) => {

tuberOrders.Id = tuberOrder.Max(to => to.Id) + 1;
tuberOrder.Add(tuberOrders);

tuberOrders.OrderPlacedOnDate = DateTime.Now; 

return Results.Created($"/tuberorders/{tuberOrders.Id}", new TuberOrderDTO {

    Id = tuberOrders.Id,
    OrderPlacedOnDate = tuberOrders.OrderPlacedOnDate,
    CustomerId = tuberOrders.CustomerId,
    TuberDriverId = tuberOrders.TuberDriverId,
    DeliveredOnDate = tuberOrders.DeliveredOnDate,
    Topping = tuberTopping
        .Where(tt => tt.TuberOrderId == tuberOrders.Id)
        .Select(tt => topping.FirstOrDefault(t => t.Id == tt.ToppingId))
        .Select(t => new ToppingDTO
        {
            Id = t.Id,
            Name = t.Name
        })
        .ToList()
});

});

app.MapPut("/tuberorders/{id}", (int id, TuberOrder tuberOrders) =>
{
    TuberOrder updatedorder = tuberOrder.FirstOrDefault(to => to.Id == id);

      if (updatedorder == null)
    {
        return Results.NotFound();
    }
    
    updatedorder.TuberDriverId = tuberOrders.TuberDriverId;

    return Results.NoContent();
});

app.MapPost("/tuberorders/{tuberId}/complete", (int tuberId) =>
{
TuberOrder completedOrder = tuberOrder.FirstOrDefault(to => to.Id == tuberId);
completedOrder.DeliveredOnDate = DateTime.Now;

});

app.MapGet("/toppings", () => 
{
    
return topping.Select(t => new ToppingDTO {
    Id = t.Id,
    Name = t.Name
});
});


app.MapGet("/toppings/{id}", (int id) => 
{
    
    Topping toppings = topping.FirstOrDefault(t => t.Id == id);
   
    return new ToppingDTO 
    {
        Id = toppings.Id,
        Name = toppings.Name
    };
    });


app.MapGet("/tuberToppings", () => 
{
    
return tuberTopping.Select(tt => new TuberToppingDTO {
    Id = tt.Id,
    TuberOrderId = tt.TuberOrderId,
    ToppingId = tt.ToppingId
});
});

app.MapPut("/tubertoppings/{id}", (int id, TuberTopping tuberToppings) =>
{
    TuberTopping updatedTuberTopping = tuberTopping.FirstOrDefault(tt => tt.Id == id);

      if (updatedTuberTopping == null)
    {
        return Results.NotFound();
    }
    
    updatedTuberTopping.ToppingId = tuberToppings.ToppingId;

    return Results.NoContent();
});

app.MapPost("/tuberToppings/{id}", (int id, TuberTopping tuberToppings ) => {

tuberToppings.Id = tuberTopping.Max(t => t.Id) + 1;
tuberTopping.Add(tuberToppings);


return Results.Created($"/tuberToppings/{tuberToppings.Id}", new TuberToppingDTO {

    Id = tuberToppings.Id,
    TuberOrderId = tuberToppings.TuberOrderId,
    ToppingId = tuberToppings.ToppingId
    
});

});

app.MapDelete("/tuberToppings/{id}", (int id ) =>
{
TuberTopping TuberToppingToRemove = tuberTopping.FirstOrDefault(t => t.Id == id);

tuberTopping.Remove(TuberToppingToRemove);
return Results.NoContent();
});

app.MapGet("/customers", () => 
{
    
return customer.Select(c => new CustomerDTO {
    Id = c.Id,
    Name = c.Name,
    Address = c.Address
});
});

app.MapGet("/customers/{id}", (int id) => 
{
Customer  customers = customer.FirstOrDefault(c => c.Id == id);
    
return new CustomerDTO {
    Id = customers.Id,
    Name = customers.Name,
    Address = customers.Address,
    TuberOrders = tuberOrder.Where(to => to.CustomerId == customers.Id)
    .Select(to => new TuberOrderDTO 
   {
        Id = to.Id,
        OrderPlacedOnDate = to.OrderPlacedOnDate,
        CustomerId = to.CustomerId,
        TuberDriverId = to.TuberDriverId,
        DeliveredOnDate = to.DeliveredOnDate,
        Topping = null,
        TuberDriver = null,
        Customer = null,
            }).ToList()
};
});

app.MapPost("/customers" , (Customer customers) => {

customers.Id = customer.Max(c => c.Id) + 1;
customer.Add(customers);


return Results.Created($"/customers/{customers.Id}", new CustomerDTO {

    Id = customers.Id,
    Name = customers.Name,
    Address = customers.Address,
    TuberOrders = tuberOrder.Where(to => to.CustomerId == customers.Id)
    .Select(to => new TuberOrderDTO 
   {
        Id = to.Id,
        OrderPlacedOnDate = to.OrderPlacedOnDate,
        CustomerId = to.CustomerId,
        TuberDriverId = to.TuberDriverId,
        DeliveredOnDate = to.DeliveredOnDate,
        Topping = null,
        TuberDriver = null,
        Customer = null,
            }).ToList()

});
});

app.MapDelete("/customers/{id}", (int id ) =>
{
Customer CustomerToRemove = customer.FirstOrDefault(c => c.Id == id);

customer.Remove(CustomerToRemove);
return Results.NoContent();
});

app.MapGet("/tuberDrivers", () => 
{
    
return tuberDriver.Select(td => new TuberDriverDTO {
    Id = td.Id,
    Name = td.Name,
    
});
});

app.MapGet("/tuberDrivers/{id}", (int id) => 
{
TuberDriver tuberDrivers = tuberDriver.FirstOrDefault(td => td.Id == id);
    
return new TuberDriverDTO {
    Id = tuberDrivers.Id,
    Name = tuberDrivers.Name,
    TuberDeliveries = tuberOrder.Where(to => to.TuberDriverId == tuberDrivers.Id)
        .Select(to => new TuberOrderDTO 
            {
                Id = to.Id,
                OrderPlacedOnDate = to.OrderPlacedOnDate,
                CustomerId = to.CustomerId,
                TuberDriverId = to.TuberDriverId,
                DeliveredOnDate = to.DeliveredOnDate,
                Topping = null,
                TuberDriver = null,
                Customer = null

            }).ToList()

};
});

app.Run();
//don't touch or move this!
public partial class Program { }