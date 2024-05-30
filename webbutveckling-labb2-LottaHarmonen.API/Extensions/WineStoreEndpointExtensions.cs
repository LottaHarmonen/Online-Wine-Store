using webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Repositories;
using webbutveckling_labb2_LottaHarmonen.Shared.DTOs;

namespace webbutveckling_labb2_LottaHarmonen.API.Extensions;

public static class WineStoreEndpointExtensions
{
    public static IEndpointRouteBuilder MapWineStoreEndpoints(this IEndpointRouteBuilder app)
    {


        //"/wines"	GET	NONE	Wine[]	200, 400
        app.MapGet("/wines", async (WineRepository wineRepo, WineTypeRepository wineTypeRepo) =>
        {
            var wines = await wineRepo.GetAllWines();
            var WineDTos = new List<WineDTO>();


            foreach (var wine in wines)
            {
                
                var WineDTO = new WineDTO()
                {
                    ProductId = wine.ProductId,
                    Name = wine.Name,
                    Description = wine.Description,
                    Status = wine.Status,
                    Price = wine.Price,
                    TypeId = wine.TypeId
                };

                WineDTos.Add(WineDTO);
            }

            return Results.Ok(WineDTos);
        });

        //"/wines/{name}"	GET	string name	Wine	200, 404
        app.MapGet("/wines/Name/{Name}", async (WineRepository wineRepo, string name) =>
        {
            var wine = await wineRepo.GetWineByName(name);
            if (wine is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No wine exists with the given name: {name}");
            }

            return Results.Ok(wine);

        });

        //"/wines/{id}"   GET int id	Wine	200, 404
        app.MapGet("/wines/{id:int}", async (WineRepository wineRepo, int id) =>
        {
            var wine = await wineRepo.GetWineById(id);
            if (wine is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No wine exists with the given id: {id}");
            }

            return Results.Ok(wine);

        });

        //"/wines/{wineType}"	GET	WineType wineType	Wine[]	200, 404
        app.MapGet("/wines/Type/{Type}", async (WineRepository wineRepo, string type) =>
        {
            var wine = await wineRepo.GetWineByType(type);
            if (wine is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No wines exist with the given type: {type}");
            }

            return Results.Ok(wine);
        });

        //"/wines/Order/{OrderNumber}"	GET	Int orderNumber	WineDTO[]	200, 404
        app.MapGet("/wines/Order/{orderNumber}", async (WineRepository wineRepo, WineTypeRepository typeRepo, int orderNumber) =>
        {

            var wines = await wineRepo.GetWinesByOrderNumber(orderNumber);

            if (wines is null)
            {
                //404 NOT FOUND!
                return Results.NotFound($"No wines found for order number: {orderNumber}");
            }

            if (wines.Any())
            {
                var winesToDisplay = new List<WineByOrderDTO>();

                foreach (var wine in wines)
                {
                    var newWineToDisplay = new WineByOrderDTO();

                    newWineToDisplay.Name = wine.Name;
                    newWineToDisplay.Price = wine.Price;

                    var typeName = await typeRepo.GetWineTypeById(wine.TypeId);

                    newWineToDisplay.Type = typeName.Name;

                    winesToDisplay.Add(newWineToDisplay);
                }
                return Results.Ok(winesToDisplay);
            }

            //404 NOT FOUND!
            return Results.NotFound($"No wines found for order number: {orderNumber}");
        });

        //"/wines"    POST Wine wine	NONE	200, 400
        app.MapPost("/wines", async (WineRepository repo, WineTypeRepository typeRepo, WineDTO newWineDTO) =>
        {
            var wine = await repo.GetWineByName(newWineDTO.Name);
            if (wine is not null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"A wine exists with the given name already: {wine.Name}");
            }


            //check if the type is available !
            var type = await typeRepo.GetWineTypeById(newWineDTO.TypeId);
            if (type is null)
            {
                //404 NOT FOUND!
                return Results.NotFound($"No type with this name exists: {newWineDTO.Name}");
            }

            var newWine = new Wine()
            {
                Name = newWineDTO.Name,
                Description = newWineDTO.Description,
                Status = newWineDTO.Status,
                Price = newWineDTO.Price,
                TypeId = type.Id
            };

            await repo.AddNewWine(newWine);
            return Results.Ok();
        });

        //"/wines/{id}/{status}"	PUT	Boolean status	NONE	200, 404
        app.MapPut("/wines/{id:int}/{status}", async (WineRepository repo, int id, bool status) =>
        {
            var wine = await repo.GetWineById(id);
            if (wine is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No wine exists with the given id: {id}");
            }

            await repo.UpdateWineStatus(id, status);
            return Results.Ok();
        });

        //"/wines/{id}"	PUT	int productId, Wine wine	NONE	200, 404
        app.MapPut("/wines/{id:int}", async (WineRepository repo, WineTypeRepository typeRepo, int id, WineDTO product) =>
        {
            var productToUpdate = await repo.GetWineById(id);
            if (productToUpdate is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No wine exists with the given id: {id}");
            }
            //check if the type is available !
            //var type = await typeRepo.GetWineTypeById(product.TypeId);
            //if (type is null)
            //{
            //    //404 NOT FOUND!
            //    return Results.NotFound($"No type with this Id exists: {product.TypeId}");
            //}

            var Wine = new Wine()
            {
                ProductId = product.ProductId,
                Description = product.Description,
                Status = product.Status,
                Name = product.Name,
                Price = product.Price,
                TypeId = product.TypeId

            };

            await repo.UpdateWine(id, Wine);
            return Results.Ok($"You have updated the product");
        });

        //"/wines{id}"	DELETE	int id	NONE	200, 404
        app.MapDelete("/wines/{id:int}", async (WineRepository repo, int id) =>
        {
            var wineById = await repo.GetWineById(id);
            if (wineById is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No wine exists with the given id: {id}");
            }

            await repo.DeleteWine(id);
            return Results.Ok();
        });




        //---------------------------CUSTOMER ENDPOINTS-----------------------------------------------------------------------






        //"/customer" GET NONE	Customer[] 200
        app.MapGet("/customers", async (CustomerRepository repo) =>
        {
            return await repo.GetAllCustomers();
        });

        //"/customer/{email}" GET NONE	Customer	200, 404
        app.MapGet("/customers/email/{email}", async (CustomerRepository customerRepo, string email) =>
        {
            var customer = await customerRepo.GetCustomerByEmail(email);
            if (customer is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No customer exists with the given email: {email}");
            }

            return Results.Ok(customer);

        });

        //"/customer"	POST	Customer	NONE	200, 400
        app.MapPost("/customers", async (CustomerRepository customerRepo, CustomerDTO newCustomer) =>
        {
            var customer = await customerRepo.GetCustomerByEmail(newCustomer.Email);

            if (customer is not null)
            {
                //400 BAD REQUEST!
                return Results.BadRequest($"Customer already exists with the given email: {newCustomer.Email}");
            }

            var newcustomer = new Customer()
            {
                Address = newCustomer.Address,
                Email = newCustomer.Email,
                FirstName = newCustomer.FirstName,
                PhoneNumber = newCustomer.PhoneNumber,
                Surname = newCustomer.Surname,
                Password = newCustomer.Password,
                IsAdmin = newCustomer.IsAdmin,
                Orders = new List<Order>()
            };

            await customerRepo.AddNewCustomer(newcustomer);
            return Results.Ok();

        });

        //"/customer/{id}	PUT	int customerId, Customer customer	NONE	200, 404
        app.MapPut("/customers/{id:int}", async (CustomerRepository repo, int id, CustomerDTO customerDto) =>
        {
            var customerToUpdate = await repo.GetCustomerById(id);

            if (customerToUpdate is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No customer found with the given Id");
            }

            var customer = new Customer()
            {
                FirstName = customerDto.FirstName,
                Surname = customerDto.Surname,
                Password = customerDto.Password,
                IsAdmin = customerDto.IsAdmin,
                Email = customerDto.Email,
                Address = customerDto.Address,
                PhoneNumber = customerDto.PhoneNumber,
                Orders = customerToUpdate.Orders
            };


            await repo.UpdateCustomer(id, customer);

            return Results.Ok($"You have updated customer information successfully");
        });

        //"/customer{id}"	DELETE	int id	NONE	200, 404
        app.MapDelete("/customers/{id:int}", async (CustomerRepository repo, int id) =>
        {
            var customer = await repo.GetCustomerById(id);
            if (customer is null)
            {
                return Results.NotFound($"No customer found with the ID: {id}");
            }

            await repo.DeleteCustomer(customer.Id);

            return Results.Ok();

        });




        //---------------------------------------ORDER ENDPOINTS-----------------------------------------------------------





        //"/orders/{customerId}"  GET string type	wine[] 200, 404
        app.MapGet("/orders/{customerId}", async (OrderRepository repo, CustomerRepository customerRepo, WineRepository wineRepo, int customerId) =>
        {
            var customer = await customerRepo.GetCustomerById(customerId);
            if (customer is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No customer exists with the given id: {customerId}");
            }

            var ordersByCustomer = await repo.GetAllOrdersByCustomerId(customerId);

            var ordersByCustomerDto = new List<OrdersByCustomerDTO>();

            foreach (var order in ordersByCustomer)
            {
                var wines =  await wineRepo.GetWinesByOrderNumber(order.Id);

                var newOrderDto = new OrdersByCustomerDTO();
                newOrderDto.Id = order.Id;
                newOrderDto.OrderDate = order.OrderDate;
                newOrderDto.AmountOfItems = wines.Count();

                ordersByCustomerDto.Add(newOrderDto);
            }

            return Results.Ok(ordersByCustomerDto);

        });



        //"/orders"   POST Order	NONE	200, 400
        app.MapPost("/orders", async (OrderRepository orderRepo, CustomerRepository customerRepo, WineRepository wineRepo, NewOrderDTO newOrderDTO) =>
        {
            //check if the customerId exists 
            var customer = await customerRepo.GetCustomerById(newOrderDTO.CustomerId);
            if (customer is null)
            {
                //404 NOT FOUND
                return Results.NotFound($"No customer exists with the given id: {newOrderDTO.CustomerId}");
            }

            var listOfProducts = new List<Wine>();

            foreach (var id in newOrderDTO.ProductId)
            {
                var wine = await wineRepo.GetWineById(id);
                listOfProducts.Add(wine);
            }

            var newOrder = new Order()
            {
                CustomerId = newOrderDTO.CustomerId,
                OrderDate = newOrderDTO.OrderDate,
                Wines = listOfProducts
            };

            await orderRepo.AddNewOrder(newOrder);
            return Results.Ok();

        });





        //--------------------------------------------WINETYPE ENDPOINTS--------------------------------------------





        //"/wineType" GET NONE	WineType[] 200
        app.MapGet("/wineTypes", async (WineTypeRepository wineTypeRepo) =>
        {
            var types = await wineTypeRepo.GetAllWineTypes();
            return Results.Ok(types);

        });

        //"/wineType/{name}"   GET string name WineType    200, 404
        app.MapGet("/wineTypes/{name}", async (WineTypeRepository wineTypeRepo, string name) =>
        {
            var type = await wineTypeRepo.GetWineTypeByName(name);
            if (type is null)
            {
                //404 NOT FOUND
                return Results.NotFound($"Type does not exist: {name}");
            }
            return Results.Ok(type);

        });

        //"/wineType/{id}"   GET Int id WineType    200, 404
        app.MapGet("/wineTypes/{id:int}", async (WineTypeRepository wineTypeRepo, int id) =>
        {
            var type = await wineTypeRepo.GetWineTypeById(id);
            if (type is null)
            {
                //404 NOT FOUND
                return Results.NotFound($"Type with the given id does not exist: {id}");
            }
            return Results.Ok(type);

        });

        //"/wineType" POST WineType type	NONE	200, 404
        app.MapPost("/wineTypes", async (WineTypeRepository repo, WineType newWineType) =>
        {
            var type = await repo.GetWineTypeByName(newWineType.Name);
            if (type is not null)
            {
                //400 BAD REQUEST
                return Results.BadRequest($"Type with the given name does already exist: {newWineType.Name}");
            }

            await repo.AddNewWineType(newWineType);
            return Results.Ok();
        });

        //"/wineType/{id}"	PUT	String name	NONE	200, 400
        app.MapPut("/wineTypes/{id:int}", async (WineTypeRepository repo, int id, WineType wineTypeToUpdate) =>
        {

            var wineType = await repo.GetWineTypeById(id);
            if (wineType is null)
            {
                //404 NOT FOUND!!
                return Results.NotFound($"No type exists with the given id: {id}");
            }

            await repo.UpdateWineType(id, wineTypeToUpdate);
            return Results.Ok();
        });

        return app;
    }









}