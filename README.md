# ShopExample
This repository shows simple microservices architecture

## Start app
Для разворачивания серверной части используется `docker compose`. Для старта необходимо перейти в терминале в папку src и выполнить команду `docker-compose up -d`

Для разворачивания клиентской части необходим перейти в терминале в папку `frontend` и выполнить команду `npm start`

Страницы приложения: https://github.com/smirnoff410/ShopExample/wiki/Pages

## Architecture
<img width="770" src="https://user-images.githubusercontent.com/25778862/192607915-6772bbb1-4d8f-4fa3-9465-759801dcb8c6.png">

## Event bus
RabbitMQ является одним из популярных брокеров сообщений. Поддерживает несоклько протокол обмена. Для простоты в рамках системы будем использовать обычную работу с очередями RabbitMQ

<img src="https://www.rabbitmq.com/img/tutorials/python-two.png">

У нас имеется отправитель и могут быть несколько получателей. Отправитель кладет свое сообщение в очередь RabbitMQ, а получатели в свою очередь прослушивают изменения в очереди и когда получают очередное сообщение, начинают свою обработку.

#### Подключение к брокеру

Для подключения к брокеру сообщений необходимо сформировать строку подключения, которая содержит в себе: 
- Username(имя пользователя, который подключается к брокеру);
- Password(пароль пользователя, который подключается к брокеру);
- Hostname(путь, по которому доступен брокер);
- Vistualhost(хост, в котором мы будем работать внутри брокера);
- Port(порт, по которому доступен броке).

```
var factory = new ConnectionFactory()
{
    UserName = "your_rabbit_username",
    Password = "your_rabbit_password",
    VirtualHost = "your_rabbit_virtual_host",
    HostName = "your_rabbit_host_name",
    Port = "your_rabbit_port"
};
```
И для подключения необходим следующий код
```
var connection = factory.CreateConnection();
var channel = connection.CreateModel();
```

#### Отправитель
На стороне отправителя мы будем создавать очередь и отправлять туда сообщения, а получатели будут присоединяться к очереди и забирать оттуда сообщения.

Создаем очередь отправителем
```
channel.QueueDeclare(queue: "your_queue_name", durable: false, exclusive: false, autoDelete: false, arguments: null);
```
Основным параметром тут является queue, которое присваивает название очереди.

Отпавляем сообщение в очередь
```
channel.BasicPublish(exchange: "", routingKey: "your_queue_name", basicProperties: null, body: body);
```
Указываем очередь, в которую посылаем сообщение(routingKey) и сообщение в виде массива байтов(body).

#### Получатель
На стороне получаетля необходимо так же подключиться к брокеру сообщений и создать подключение, а также указать прослушивание очереди.
```
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
};
channel.BasicConsume(queue: "your_queue_name", autoAck: true, consumer: consumer);
```

## Docker

Для удобного разворачивания обернем наши сервисы в docker контейнеры в связке с docker-compose
#### Структура докер файла для приложения .NET
- Указать образ используемого .NET sdk
- Копирование файлов проектов(.csproj) и решения (.sln) в папку контейнера
- Обновить зависомсти NuGet с помощью команды restore
- Копирование всех необходимых файлов приложения в папку контейнера
- Компилирование приложения с помощью команды publish
- Запуск файла dll приложения

Для сервиса User данного приложения был сформирован следующий docker файл
```
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY "ShopExample.sln" "ShopExample.sln"
COPY "Common/Common.csproj" "Common/Common.csproj"
COPY "Services/Catalog/Catalog.csproj" "Services/Catalog/Catalog.csproj"
COPY "Services/Basket/Basket.csproj" "Services/Basket/Basket.csproj"
COPY "Services/User/User.csproj" "Services/User/User.csproj"

RUN dotnet restore ShopExample.sln

COPY "./Common" "./Common"
COPY "./Services/Catalog" "./Services/Catalog"
COPY "./Services/Basket" "./Services/Basket"
COPY "./Services/User" "./Services/User"

RUN dotnet publish --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "User.dll"]
```

#### docker-compose
Для удобного запуска сразу нескольких docker контейнеров воспользуемся утилитой docker-compose. Она использует язык разметки yaml.
Для сервиса нам необходимо указать `hostname`, `container_name`, `dockerfile`, `ports`
```
user:
    hostname: shop_example_user
    container_name: shop_example_user
    build:
        context: .
        dockerfile: UserDockerfile
    ports:
        - "5003:80"
```

Для запуска контейнеров необходимо открыть консоль в папке с файлом docker-compose.yml и выполнить следующую команду:
`docker-compose up -d`
После этого сервис должен быть доступен по пути `http://localhost:5003`
