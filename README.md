# OrderMQ

(Simple) Distributed application with RabbitMQ and PostgreSQL. There are three parts
to this application: a publisher (exposed as an API) and two consumers (console
applications). The publisher receives orders from the API client, persists them to
PostgreSQL (as JSONB), and publishes them onto the exchange (as serialized JSON). The consumer
listens for incoming messages from the queue via the routing key and logs the
delivery to the console.

Technology
----------
* Go
* PostgreSQL (with Marten)
* RabbitMQ

Endpoints
---------

| Method     | URI                                  | Action                                      |
|------------|--------------------------------------|---------------------------------------------|
| `POST`     | `/api/delivery`                      | `Register a delivery`                             |


Sample Usage
---------------

`http post http://localhost:5000/api/delivery name=user address="123 main st"
city="new york" email="user@example.com"`
```
{
    "id": "5ac3e5a39039e1051da55d1b", 
    "product": "ipad"
}
```
logged to console from the publisher:  
`2018/04/03 16:35:47 Sent order 5ac3e5a39039e1051da55d1b to queue: order_queue`

logged to console from the consumer:  
`2018/04/03 16:35:47 Received a message: {"id":"5ac3e5a39039e1051da55d1b","product":"ipad"}`

Run
---

First create the database, then `cd` into `./DeliveryMQ.API`, open `Startup.cs`
and point database URI to your server (must be PostgreSQL), then `cd` into
`./RabbitMQ` and open `RabbitMQClient` and point the AMQP URI to your RabbitMQ
client. Next, go to `./DeliveryMQ.Notification.Service` and
`./DeliveryMQ.Registration.Service` and open the `RabbitMQClient.cs`  files and
poin the AMQP URI's to your RabbitMQ client.

After that has been taken care of,
```
dotnet restore (API)
dotnet ef database update
dotnet run
dotnet restore (Both services)
dotnet run
Go to http://localhost:5000 and visit the above endpoint.
```

TODO
---
Dockerfile
